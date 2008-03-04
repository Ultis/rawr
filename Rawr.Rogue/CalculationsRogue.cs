using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Rogue
{
	[System.ComponentModel.DisplayName("Rogue|INV_ThrowingKnife_04")]
	class CalculationsRogue : CalculationsBase
	{
		private CalculationOptionsPanelBase _calculationOptionsPanel = null;
		public override CalculationOptionsPanelBase CalculationOptionsPanel
		{
			get
			{
				if ( _calculationOptionsPanel == null )
				{
					_calculationOptionsPanel = new CalculationOptionsPanelRogue();
				}
				return _calculationOptionsPanel;
			}
		}

		private List<Item.ItemType> _relevantItemTypes = null;
		public override List<Item.ItemType> RelevantItemTypes
		{
			get
			{
				if ( _relevantItemTypes == null )
				{
					_relevantItemTypes = new List<Item.ItemType>( new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.Leather,

						Item.ItemType.Bow,
						Item.ItemType.Crossbow,
						Item.ItemType.Gun,
						Item.ItemType.Thrown,

						Item.ItemType.Dagger,
						Item.ItemType.FistWeapon,
						Item.ItemType.OneHandSword,
						Item.ItemType.OneHandMace,
					} );
				}
				return _relevantItemTypes;
			}
		}

		private string[] _characterDisplayCalculationLabels = null;
		public override string[] CharacterDisplayCalculationLabels
		{
			get
			{
				if ( _characterDisplayCalculationLabels == null )
					_characterDisplayCalculationLabels = new string[] {
						"Basic Stats:Health",
						"Basic Stats:Agility",
						"Basic Stats:Strength",
						"Basic Stats:Attack Power",
						"Basic Stats:Hit Rating",
						"Basic Stats:Crit Rating",
						"Basic Stats:Expertise Rating",
						"Basic Stats:Haste Rating",
						"Basic Stats:Armor Penetration",

						"Survivability:Dodge Rating",
						"Survivability:Parry Rating",
					};
				return _characterDisplayCalculationLabels;
			}
		}

		private string[] _customChartNames = null;
		public override string[] CustomChartNames
		{
			get
			{
				if ( _customChartNames == null )
					_customChartNames = new string[] {
					};
				return _customChartNames;
			}
		}

		private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
		public override Dictionary<string, System.Drawing.Color> SubPointNameColors
		{
			get
			{
				if ( _subPointNameColors == null )
				{
					_subPointNameColors = new Dictionary<string, System.Drawing.Color>();
					_subPointNameColors.Add( "DPS", System.Drawing.Color.FromArgb( 160, 0, 224 ) );
				}
				return _subPointNameColors;
			}
		}

		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationRogue(); }
		public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsRogue(); }

		public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
		{
			Stats stats = GetCharacterStats( character, additionalItem );

			CharacterCalculationsRogue calculatedStats = new CharacterCalculationsRogue();
			calculatedStats.BasicStats = stats;

			return ( calculatedStats );
		}
	
		public override Stats GetCharacterStats(Character character, Item additionalItem)
		{
			Stats statsRace = GetRaceStats(character.Race);
			Stats statsBaseGear = GetItemStats( character, additionalItem );
			Stats statsEnchants = GetEnchantsStats( character );
			Stats statsBuffs = GetBuffsStats( character.ActiveBuffs );

			Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

			float agiBase = ( float )Math.Floor( statsRace.Agility * ( 1 + statsRace.BonusAgilityMultiplier ) );
			float agiBonus = ( float )Math.Floor( statsGearEnchantsBuffs.Agility * ( 1 + statsRace.BonusAgilityMultiplier ) );
			float strBase = ( float )Math.Floor( statsRace.Strength * ( 1 + statsRace.BonusStrengthMultiplier ) );
			float strBonus = ( float )Math.Floor( statsGearEnchantsBuffs.Strength * ( 1 + statsRace.BonusStrengthMultiplier ) );
			float staBase = ( float )Math.Floor( statsRace.Stamina * ( 1 + statsRace.BonusStaminaMultiplier ) );
			float staBonus = ( float )Math.Floor( statsGearEnchantsBuffs.Stamina * ( 1 + statsRace.BonusStaminaMultiplier ) );

			Stats statsTotal = new Stats();
			statsTotal.BonusAttackPowerMultiplier = ( ( 1 + statsRace.BonusAttackPowerMultiplier ) * ( 1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier ) ) - 1;
			statsTotal.BonusAgilityMultiplier = ( ( 1 + statsRace.BonusAgilityMultiplier ) * ( 1 + statsGearEnchantsBuffs.BonusAgilityMultiplier ) ) - 1;
			statsTotal.BonusStrengthMultiplier = ( ( 1 + statsRace.BonusStrengthMultiplier ) * ( 1 + statsGearEnchantsBuffs.BonusStrengthMultiplier ) ) - 1;
			statsTotal.BonusStaminaMultiplier = ( ( 1 + statsRace.BonusStaminaMultiplier ) * ( 1 + statsGearEnchantsBuffs.BonusStaminaMultiplier ) ) - 1;

			
			statsTotal.Agility = ( agiBase + ( float )Math.Floor( ( agiBase * statsBuffs.BonusAgilityMultiplier ) + agiBonus * ( 1 + statsBuffs.BonusAgilityMultiplier ) ) );
			statsTotal.Agility *= ( 1 + ( 0.01f * int.Parse( character.CalculationOptions["Vitality"] ) ) );
			
			statsTotal.Strength = ( strBase + ( float )Math.Floor( ( strBase * statsBuffs.BonusStrengthMultiplier ) + strBonus * ( 1 + statsBuffs.BonusStrengthMultiplier ) ) );
			
			statsTotal.Stamina = ( staBase + ( float )Math.Round( ( staBase * statsBuffs.BonusStaminaMultiplier ) + staBonus * ( 1 + statsBuffs.BonusStaminaMultiplier ) ) );
			statsTotal.Stamina *= ( 1 + ( 0.02f * int.Parse( character.CalculationOptions["Vitality"] ) ) );

			statsTotal.Health = ( float )Math.Round( ( ( statsRace.Health + statsGearEnchantsBuffs.Health + ( statsTotal.Stamina * 10f ) ) ) );

			statsTotal.AttackPower = ( float )Math.Floor( ( statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + statsTotal.Agility + statsTotal.Strength ) * ( 1f + statsTotal.BonusAttackPowerMultiplier ) );
			
			
			statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
			statsTotal.CritRating += ( ( statsTotal.Agility / 40f ) * 22.08f );
			statsTotal.CritRating += ( 22.08f * int.Parse( character.CalculationOptions["Malice"] ) );

			statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
			statsTotal.HitRating += ( 15.76f * int.Parse( character.CalculationOptions["Precision"] ) );

			statsTotal.ExpertiseRating = statsRace.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
			statsTotal.ExpertiseRating += ( 5f * int.Parse( character.CalculationOptions["WeaponExpertise"] ) );

			statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;

			statsTotal.DodgeRating = statsRace.DodgeRating + statsGearEnchantsBuffs.DodgeRating;
			statsTotal.DodgeRating = ( ( statsTotal.Agility / 20f ) * 18.92f );
			statsTotal.DodgeRating += ( 18.92f * int.Parse( character.CalculationOptions["LightningReflexes"] ) );

			statsTotal.ParryRating = statsRace.ParryRating + statsGearEnchantsBuffs.ParryRating;
			statsTotal.ParryRating += ( 23.65f * int.Parse( character.CalculationOptions["Deflection"] ) );

			statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;

			return ( statsTotal );
		}


		private static float[,] BaseRogueRaceStats = new float[,] 
		{
							//	Agility,	Strength,	Stamina
			/*Night Elf*/	{	163f,		92f,		88f,	},		
			/*Tauren*/		{	0f,			0f,			0f,		},		
			/*Human*/		{	158f,		95f,		89f,	},	
			/*Orc*/			{	155f,		98f,		91f,	},		
			/*Gnome*/		{	161f,		90f,		88f,	},		
			/*Troll*/		{	160f,		96f,		90f,	},	
			/*Dwarf*/		{	154f,		97f,		92f,	},
			/*Undead*/		{	156f,		94f,		90f,	},	
			/*Draenei*/		{	0f,			0f,			0f,		},
			/*BloodElf*/	{	160f,		92f,		87f,	},
		};

		private Stats GetRaceStats( Character.CharacterRace race )
		{
			if ( race == Character.CharacterRace.Tauren || race == Character.CharacterRace.Draenei )
				return new Stats();

			Stats statsRace = new Stats()
			{
				Health = 3524f,
				Agility = ( float )BaseRogueRaceStats[( int )race, 0],
				Strength = ( float )BaseRogueRaceStats[( int )race, 1],
				Stamina = ( float )BaseRogueRaceStats[( int )race, 2],

				AttackPower = 120f,

				DodgeRating = (float)(-0.59 * 18.92f),
			};

			if ( race == Character.CharacterRace.NightElf )
				statsRace.DodgeRating += 18.92f;

			return statsRace;
		}

		public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
		{
			return new ComparisonCalculationBase[0];
		}

		public override Stats GetRelevantStats(Stats stats)
		{
			return new Stats()
			{
				Agility = stats.Agility,
				Strength = stats.Strength,

				AttackPower = stats.AttackPower,

				HitRating = stats.HitRating,
				CritRating = stats.CritRating,

				ArmorPenetration = stats.ArmorPenetration,
			};
		}

		public override bool HasRelevantStats(Stats stats)
		{
			return true;
		}
	}

	public class CharacterCalculationsRogue : CharacterCalculationsBase
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

		private Stats _basicStats;
		public Stats BasicStats
		{
			get { return _basicStats; }
			set { _basicStats = value; }
		}

		private int _targetLevel;
		public int TargetLevel
		{
			get { return _targetLevel; }
			set { _targetLevel = value; }
		}

		private float _avoidedAttacks;
		public float AvoidedAttacks
		{
			get { return _avoidedAttacks; }
			set { _avoidedAttacks = value; }
		}

		private float _dodgedAttacks;
		public float DodgedAttacks
		{
			get { return _dodgedAttacks; }
			set { _dodgedAttacks = value; }
		}

		private float _missedAttacks;
		public float MissedAttacks
		{
			get { return _missedAttacks; }
			set { _missedAttacks = value; }
		}

		private float _whiteCrit;
		public float WhiteCrit
		{
			get { return _whiteCrit; }
			set { _whiteCrit = value; }
		}

		private float _yellowCrit;
		public float YellowCrit
		{
			get { return _yellowCrit; }
			set { _yellowCrit = value; }
		}

		private float _attackSpeed;
		public float AttackSpeed
		{
			get { return _attackSpeed; }
			set { _attackSpeed = value; }
		}

		private float _armorMitigation;
		public float ArmorMitigation
		{
			get { return _armorMitigation; }
			set { _armorMitigation = value; }
		}

		private float _cycleTime;
		public float CycleTime
		{
			get { return _cycleTime; }
			set { _cycleTime = value; }
		}

		private float _meleeDamage;
		public float MeleeDamage
		{
			get { return _meleeDamage; }
			set { _meleeDamage = value; }
		}

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();
			dictValues.Add( "Health", BasicStats.Health.ToString() );

			dictValues.Add( "Agility", BasicStats.Agility.ToString() );
			dictValues.Add( "Strength", BasicStats.Strength.ToString() );

			dictValues.Add( "Attack Power", BasicStats.AttackPower.ToString() );

			dictValues.Add( "Hit Rating", BasicStats.HitRating.ToString() );
			dictValues.Add( "Crit Rating", BasicStats.CritRating.ToString() );
			dictValues.Add( "Expertise Rating", BasicStats.ExpertiseRating.ToString() );
			dictValues.Add( "Haste Rating", BasicStats.HasteRating.ToString() );
			dictValues.Add( "Armor Penetration", BasicStats.ArmorPenetration.ToString() );

			dictValues.Add( "Dodge Rating", BasicStats.DodgeRating.ToString() );
			dictValues.Add( "Parry Rating", BasicStats.ParryRating.ToString() );

			return dictValues;
		}
    }

	public class ComparisonCalculationRogue : ComparisonCalculationBase
	{
		private string _name = string.Empty;
		public override string Name
		{
			get { return _name; }
			set { _name = value; }
		}

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

		private Item _item = null;
		public override Item Item
		{
			get { return _item; }
			set { _item = value; }
		}

		private bool _equipped = false;
		public override bool Equipped
		{
			get { return _equipped; }
			set { _equipped = value; }
		}

		public override string ToString()
		{
			return string.Format("{0}: ({1}O {2}DPS)", Name, Math.Round(OverallPoints), Math.Round(DPSPoints));
		}
	}
}
