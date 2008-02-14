using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace Rawr
{
	public class Calculations
	{
		private static SortedList<string, Type> _models = null;
		public static SortedList<string, Type> Models
		{
			get
			{
				if (_models == null)
				{
					_models = new SortedList<string, Type>();

					DirectoryInfo info = new DirectoryInfo(".");
					foreach (FileInfo file in info.GetFiles("*.dll"))
					{
						try
						{
							Assembly assembly = Assembly.LoadFile(file.FullName);

							foreach (Type type in assembly.GetTypes())
							{
								if (type.IsSubclassOf(typeof(CalculationsBase)))
								{
									DisplayNameAttribute[] displayNameAttributes = type.GetCustomAttributes(typeof(DisplayNameAttribute), false) as DisplayNameAttribute[];
									string displayName = type.Name;
									if (displayNameAttributes.Length > 0)
										displayName = displayNameAttributes[0].DisplayName;
									_models[displayName] = type;
								}
							}
						}
						catch (Exception e)
						{
							System.Diagnostics.Debug.Write(e.Message);
						}
					}

					if (_models.Count == 0)
						throw new TypeLoadException("Unable to find any model plug in dlls.  Please check that the files exist and are in the correct location");
				}
				return _models;
			}
		}

		public static event EventHandler ModelChanged;
		protected static void OnModelChanged()
		{
			if (ModelChanged != null)
				ModelChanged(null, EventArgs.Empty);
		}

		public static void LoadModel(string displayName) { LoadModel(Models[displayName]); }
		public static void LoadModel(Type type) { LoadModel((CalculationsBase)Activator.CreateInstance(type)); }
		public static void LoadModel(CalculationsBase model)
		{
            Instance = model;
			OnModelChanged();
		}

		private static CalculationsBase _instance;
		public static CalculationsBase Instance
		{
			get { return _instance; }
			private set { _instance = value; }
		}

		public static Character CachedCharacter
		{
			get { return Instance.CachedCharacter; } 
		}
		public static string[] CharacterDisplayCalculationLabels
		{
			get { return Instance.CharacterDisplayCalculationLabels; }
		}
		public static Dictionary<string, System.Drawing.Color> SubPointNameColors
		{
			get { return Instance.SubPointNameColors; }
		}
		public static CalculationOptionsPanelBase CalculationOptionsPanel
		{
			get { return Instance.CalculationOptionsPanel; }
		}

		public static ComparisonCalculationBase CreateNewComparisonCalculation()
		{
			return Instance.CreateNewComparisonCalculation();
		}
		public static CharacterCalculationsBase CreateNewCharacterCalculations()
		{
			return Instance.CreateNewCharacterCalculations();
		}
		public static void ClearCache()
		{
			Instance.ClearCache();
		}
		public static ComparisonCalculationBase GetItemCalculations(Item item, Character character, Character.CharacterSlot slot)
		{
			return Instance.GetItemCalculations(item, character, slot);
		}
		public static List<ComparisonCalculationBase> GetEnchantCalculations(Item.ItemSlot slot, Character character, CharacterCalculationsBase currentCalcs)
		{
			return Instance.GetEnchantCalculations(slot, character, currentCalcs);
		}
		public static List<ComparisonCalculationBase> GetBuffCalculations(Character character, CharacterCalculationsBase currentCalcs, Buff.BuffType buffType, bool activeOnly)
		{
			return Instance.GetBuffCalculations(character, currentCalcs, buffType, activeOnly);
		}
		public static CharacterCalculationsBase GetCharacterCalculations(Character character)
		{
			return Instance.GetCharacterCalculations(character);
		}
		public static Stats GetCharacterStats(Character character)
		{
			return Instance.GetCharacterStats(character);
		}
		public static Stats GetCharacterStats(Character character, Item additionalItem)
		{
			return Instance.GetCharacterStats(character, additionalItem);
		}
		public static Stats GetEnchantsStats(Character character)
		{
			return Instance.GetEnchantsStats(character);
		}
		public static Stats GetBuffsStats(List<string> buffs)
		{
			return Instance.GetBuffsStats(buffs);
		}
		public static ComparisonCalculationBase[] GetCombatTable(CharacterCalculationsBase currentCalculations)
		{
			return Instance.GetCombatTable(currentCalculations);
		}
		public static Stats GetRelevantStats(Stats stats)
		{
			return Instance.GetRelevantStats(stats);
		}
		public static bool HasRelevantStats(Stats stats)
		{
			if (Instance != null)
				return Instance.HasRelevantStats(stats);
			return false;
		}
		public static bool IsItemRelevant(Item item)
		{
			if (Instance != null)
				return Instance.IsItemRelevant(item);
			return false;
		}
		public static string GetCharacterStatsString(Character character)
		{
			return Instance.GetCharacterStatsString(character);
		}
	}

	/// <summary>
	/// CalculationsBase is the base class which each model's main Calculations class will inherit from.
	/// </summary>
	public abstract class CalculationsBase
	{
		protected CharacterCalculationsBase _cachedCharacterStatsWithSlotEmpty = null;
		protected Character.CharacterSlot _cachedSlot = Character.CharacterSlot.Shirt;
		protected Character _cachedCharacter = null;
		public virtual Character CachedCharacter { get { return _cachedCharacter; } }
		
		/// <summary>
		/// Dictionary<string, Color> that includes the names of each rating which your model will use,
		/// and a color for each. These colors will be used in the charts.
		/// 
		/// EXAMPLE: 
		/// subPointNameColors = new Dictionary<string, System.Drawing.Color>();
		/// subPointNameColors.Add("Mitigation", System.Drawing.Colors.Red);
		/// subPointNameColors.Add("Survival", System.Drawing.Colors.Blue);
		/// </summary>
		public abstract Dictionary<string, System.Drawing.Color> SubPointNameColors { get; }
		
		/// <summary>
		/// An array of strings which will be used to build the calculation display.
		/// Each string must be in the format of "Heading:Label". Heading will be used as the
		/// text of the group box containing all labels that have the same Heading.
		/// Label will be the label of that calculation, and may be appended with '*' followed by
		/// a description of that calculation which will be displayed in a tooltip for that label.
		/// Label (without the tooltip string) must be unique.
		/// 
		/// EXAMPLE:
		/// characterDisplayCalculationLabels = new string[]
		/// {
		///		"Basic Stats:Health",
		///		"Basic Stats:Armor",
		///		"Advanced Stats:Dodge",
		///		"Advanced Stats:Miss*Chance to be missed"
		/// };
		/// </summary>
		public abstract string[] CharacterDisplayCalculationLabels { get; }

		/// <summary>
		/// A custom panel inheriting from CalculationOptionsPanelBase which contains controls for
		/// setting CalculationOptions for the model. CalculationOptions are stored in the Character,
		/// and can be used by multiple models. See comments on CalculationOptionsPanelBase for more details.
		/// </summary>
		public abstract CalculationOptionsPanelBase CalculationOptionsPanel { get; }

		/// <summary>
		/// List<Item.ItemType> containing all of the ItemTypes relevant to this model. Typically this
		/// means all types of armor/weapons that the intended class is able to use, but may also
		/// be trimmed down further if some aren't typically used. Item.ItemType.None should almost
		/// always be included, because that type includes items with no proficiancy requirement, such
		/// as rings, necklaces, cloaks, held in off hand items, etc.
		/// 
		/// EXAMPLE:
		/// relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
		/// {
		///     Item.ItemType.None,
		///     Item.ItemType.Leather,
		///     Item.ItemType.Idol,
		///     Item.ItemType.Staff,
		///     Item.ItemType.TwoHandMace
		/// });
		/// </summary>
		public abstract List<Item.ItemType> RelevantItemTypes { get; }
		
		
		/// <summary>
		/// Method to get a new instance of the model's custom ComparisonCalculation class.
		/// </summary>
		/// <returns>A new instance of the model's custom ComparisonCalculation class, 
		/// which inherits from ComparisonCalculationBase</returns>
		public abstract ComparisonCalculationBase CreateNewComparisonCalculation();

		/// <summary>
		/// Method to get a new instance of the model's custom CharacterCalculations class.
		/// </summary>
		/// <returns>A new instance of the model's custom CharacterCalculations class, 
		/// which inherits from CharacterCalculationsBase</returns>
		public abstract CharacterCalculationsBase CreateNewCharacterCalculations();


		public virtual CharacterCalculationsBase GetCharacterCalculations(Character character) { return GetCharacterCalculations(character, null); }
		/// <summary>
		/// GetCharacterCalculations is the primary method of each model, where a majority of the calculations
		/// and formulae will be used. GetCharacterCalculations should call GetCharacterStats(), and based on
		/// those total stats for the character, and any calculationoptions on the character, perform all the 
		/// calculations required to come up with the final calculations defined in 
		/// CharacterDisplayCalculationLabels, including an Overall rating, and all Sub ratings defined in 
		/// SubPointNameColors.
		/// </summary>
		/// <param name="character">The character to perform calculations for.</param>
		/// <param name="additionalItem">An additional item to treat the character as wearing.
		/// This is used for gems, which don't have a slot on the character to fit in, so are just
		/// added onto the character, in order to get gem calculations.</param>
		/// <returns>A custom CharacterCalculations object which inherits from CharacterCalculationsBase,
		/// containing all of the final calculations defined in CharacterDisplayCalculationLabels. See
		/// CharacterCalculationsBase comments for more details.</returns>
		public abstract CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem);
		
		public virtual Stats GetCharacterStats(Character character) { return GetCharacterStats(character, null); }
		/// <summary>
		/// GetCharacterStats is the 2nd-most calculation intensive method in a model. Here the model will
		/// combine all of the information about the character, including race, gear, enchants, buffs,
		/// calculationoptions, etc., to form a single combined Stats object. Three of the methods below
		/// can be called from this method to help total up stats: GetItemStats(character, additionalItem),
		/// GetEnchantsStats(character), and GetBuffsStats(character.ActiveBuffs).
		/// </summary>
		/// <param name="character">The character whose stats should be totaled.</param>
		/// <param name="additionalItem">An additional item to treat the character as wearing.
		/// This is used for gems, which don't have a slot on the character to fit in, so are just
		/// added onto the character, in order to get gem calculations.</param>
		/// <returns>A Stats object containing the final totaled values of all character stats.</returns>
		public abstract Stats GetCharacterStats(Character character, Item additionalItem);
		
		/// <summary>
		/// Depreciated. Functions currently, and is required, but feel free to just return an empty array.
		/// I'll be revamping this into a system for models to define custom charts.
		/// </summary>
		/// <param name="currentCalculations"></param>
		/// <returns></returns>
		public abstract ComparisonCalculationBase[] GetCombatTable(CharacterCalculationsBase currentCalculations);
		
		/// <summary>
		/// Filters a Stats object to just the stats relevant to the model.
		/// </summary>
		/// <param name="stats">A complete Stats object containing all stats.</param>
		/// <returns>A filtered Stats object containing only the stats relevant to the model.</returns>
		public abstract Stats GetRelevantStats(Stats stats);

		/// <summary>
		/// Tests whether there are positive relevant stats in the Stats object.
		/// </summary>
		/// <param name="stats">The complete Stats object containing all stats.</param>
		/// <returns>True if any of the positive stats in the Stats are relevant.</returns>
		public abstract bool HasRelevantStats(Stats stats);
		
		public virtual void ClearCache()
		{
			_cachedCharacter = null;
			_cachedSlot = Character.CharacterSlot.Shirt;
		}

		public virtual ComparisonCalculationBase GetItemCalculations(Item item, Character character, Character.CharacterSlot slot)
		{
			bool useCache = character == _cachedCharacter && slot == _cachedSlot;
			Character characterWithSlotEmpty = null;

			if (!useCache)
				characterWithSlotEmpty = character.Clone();
			Character characterWithNewItem = character.Clone();

			if (slot != Character.CharacterSlot.Metas && slot != Character.CharacterSlot.Gems)
			{
				if (!useCache) characterWithSlotEmpty[slot] = null;
				characterWithNewItem[slot] = item;
			}


			CharacterCalculationsBase characterStatsWithSlotEmpty;
			if (useCache)
				characterStatsWithSlotEmpty = _cachedCharacterStatsWithSlotEmpty;
			else
			{
				characterStatsWithSlotEmpty = GetCharacterCalculations(characterWithSlotEmpty);
				_cachedCharacter = character;
				_cachedSlot = slot;
				_cachedCharacterStatsWithSlotEmpty = characterStatsWithSlotEmpty;
			}


			Item additionalItem = null;
			if (item.FitsInSlot(Character.CharacterSlot.Gems) || item.FitsInSlot(Character.CharacterSlot.Metas)) additionalItem = item;
			CharacterCalculationsBase characterStatsWithNewItem = GetCharacterCalculations(characterWithNewItem, additionalItem);

			ComparisonCalculationBase itemCalc = CreateNewComparisonCalculation();
			itemCalc.Item = item;
			itemCalc.Name = item.Name;
			itemCalc.Equipped = character[slot] == item;
			itemCalc.OverallPoints = characterStatsWithNewItem.OverallPoints - characterStatsWithSlotEmpty.OverallPoints;
			float[] subPoints = new float[characterStatsWithNewItem.SubPoints.Length];
			for (int i = 0; i < characterStatsWithNewItem.SubPoints.Length; i++)
			{
				subPoints[i] = characterStatsWithNewItem.SubPoints[i] - characterStatsWithSlotEmpty.SubPoints[i];
			}
			itemCalc.SubPoints = subPoints;

			characterStatsWithNewItem.ToString();

			return itemCalc;
		}

		public virtual List<ComparisonCalculationBase> GetEnchantCalculations(Item.ItemSlot slot, Character character, CharacterCalculationsBase currentCalcs)
		{
			List<ComparisonCalculationBase> enchantCalcs = new List<ComparisonCalculationBase>();
			CharacterCalculationsBase calcsEquipped = null;
			CharacterCalculationsBase calcsUnequipped = null;
			foreach (Enchant enchant in Enchant.FindEnchants(slot))
			{
				//if (enchantCalcs.ContainsKey(enchant.Id)) continue;

				bool isEquipped = character.GetEnchantBySlot(enchant.Slot) == enchant;
				if (isEquipped)
				{
					calcsEquipped = currentCalcs;
					Character charUnequipped = character.Clone();
					charUnequipped.SetEnchantBySlot(enchant.Slot, null);
					calcsUnequipped = GetCharacterCalculations(charUnequipped);
				}
				else
				{
					Character charUnequipped = character.Clone();
					Character charEquipped = character.Clone();
					charUnequipped.SetEnchantBySlot(enchant.Slot, null);
					charEquipped.SetEnchantBySlot(enchant.Slot, enchant);
					calcsUnequipped = GetCharacterCalculations(charUnequipped);
					calcsEquipped = GetCharacterCalculations(charEquipped);
				}
				ComparisonCalculationBase enchantCalc = CreateNewComparisonCalculation();
				enchantCalc.Name = enchant.Name;
				enchantCalc.Item = new Item();
				enchantCalc.Item.Name = enchant.Name;
				enchantCalc.Item.Stats = enchant.Stats;
				enchantCalc.Equipped = isEquipped;
				enchantCalc.OverallPoints = calcsEquipped.OverallPoints - calcsUnequipped.OverallPoints;
				float[] subPoints = new float[calcsEquipped.SubPoints.Length];
				for (int i = 0; i < calcsEquipped.SubPoints.Length; i++)
				{
					subPoints[i] = calcsEquipped.SubPoints[i] - calcsUnequipped.SubPoints[i];
				}
				enchantCalc.SubPoints = subPoints;
				enchantCalcs.Add(enchantCalc);
			}
			return enchantCalcs;
		}

		public virtual List<ComparisonCalculationBase> GetBuffCalculations(Character character, CharacterCalculationsBase currentCalcs, Buff.BuffType buffType, bool activeOnly)
		{
			List<ComparisonCalculationBase> buffCalcs = new List<ComparisonCalculationBase>();
			CharacterCalculationsBase calcsEquipped = null;
			CharacterCalculationsBase calcsUnequipped = null;
			foreach (Buff buff in Buff.GetBuffsByType(buffType))
			{
				if (!activeOnly || character.ActiveBuffs.Contains(buff.Name))
				{
					Character charUnequipped = character.Clone();
					Character charEquipped = character.Clone();
					if (charUnequipped.ActiveBuffs.Contains(buff.Name))
						charUnequipped.ActiveBuffs.Remove(buff.Name);
					if (string.IsNullOrEmpty(buff.RequiredBuff))
					{
						if (charUnequipped.ActiveBuffs.Contains("Improved " + buff.Name))
							charUnequipped.ActiveBuffs.Remove("Improved " + buff.Name);
					}
					else
						if (charUnequipped.ActiveBuffs.Contains(buff.RequiredBuff))
							charUnequipped.ActiveBuffs.Remove(buff.RequiredBuff);

					if (!charEquipped.ActiveBuffs.Contains(buff.Name))
						charEquipped.ActiveBuffs.Add(buff.Name);
					if (string.IsNullOrEmpty(buff.RequiredBuff))
					{
						if (charEquipped.ActiveBuffs.Contains("Improved " + buff.Name))
							charEquipped.ActiveBuffs.Remove("Improved " + buff.Name);
					}
					else
						if (!charEquipped.ActiveBuffs.Contains(buff.RequiredBuff))
							charEquipped.ActiveBuffs.Add(buff.RequiredBuff);

					foreach (string conflictingBuff in buff.ConflictingBuffs)
					{
						if (charEquipped.ActiveBuffs.Contains(conflictingBuff))
							charEquipped.ActiveBuffs.Remove(conflictingBuff);
						if (charUnequipped.ActiveBuffs.Contains(conflictingBuff))
							charUnequipped.ActiveBuffs.Remove(conflictingBuff);
					}

					calcsUnequipped = GetCharacterCalculations(charUnequipped);
					calcsEquipped = GetCharacterCalculations(charEquipped);

					ComparisonCalculationBase buffCalc = CreateNewComparisonCalculation();
					buffCalc.Name = buff.Name;
					buffCalc.Item = new Item() { Name = buff.Name, Stats = buff.Stats };
					buffCalc.Equipped = character.ActiveBuffs.Contains(buff.Name);
					buffCalc.OverallPoints = calcsEquipped.OverallPoints - calcsUnequipped.OverallPoints;
					float[] subPoints = new float[calcsEquipped.SubPoints.Length];
					for (int i = 0; i < calcsEquipped.SubPoints.Length; i++)
					{
						subPoints[i] = calcsEquipped.SubPoints[i] - calcsUnequipped.SubPoints[i];
					}
					buffCalc.SubPoints = subPoints;
					buffCalcs.Add(buffCalc);
				}
			}
			return buffCalcs;
		}

		public virtual Stats GetItemStats(Character character, Item additionalItem)
		{
			List<Item> items = new List<Item>(new Item[] {character.Back, character.Chest, character.Feet, character.Finger1,
				character.Finger2, character.Hands, character.Head, character.Legs, character.Neck,
				character.Shirt, character.Shoulders, character.Tabard, character.Trinket1, character.Trinket2,
				character.Waist, character.MainHand, character.OffHand, character.Ranged, character.Projectile, 
				character.ProjectileBag, character.Wrist});
			if (additionalItem != null)
				items.Add(additionalItem);

			Stats statsItems = new Stats();
			foreach (Item item in items)
				if (item != null)
					statsItems += item.GetTotalStats(character);

			return statsItems;
		}

		public virtual Stats GetEnchantsStats(Character character)
		{
			return character.BackEnchant.Stats + character.ChestEnchant.Stats + character.FeetEnchant.Stats + 
				character.Finger1Enchant.Stats + character.Finger2Enchant.Stats + character.HandsEnchant.Stats +
				character.HeadEnchant.Stats + character.LegsEnchant.Stats + character.ShouldersEnchant.Stats + 
				(character.OffHand == null ? character.MainHandEnchant.Stats : character.MainHandEnchant.Stats + 
				character.OffHandEnchant.Stats) + character.RangedEnchant.Stats + character.WristEnchant.Stats;
		}

		public virtual Stats GetBuffsStats(IEnumerable<string> buffs)
		{
			Stats statsBuffs = new Stats();
			foreach (string buffName in buffs)
				statsBuffs += Buff.GetBuffByName(buffName).Stats;

			return statsBuffs;
		}

		public virtual string GetCharacterStatsString(Character character)
		{
			StringBuilder stats = new StringBuilder();
			stats.AppendFormat("Character:\t\t{0}@{1}-{2}\r\nRace:\t\t{3}",
				character.Name, character.Region, character.Realm, character.Race);

			foreach (KeyValuePair<string, string> kvp in GetCharacterCalculations(character).GetCharacterDisplayCalculationValues())
			{
				stats.AppendFormat("\r\n{0}:\t\t{1}", kvp.Key, kvp.Value.Split('*')[0]);
			}

			return stats.ToString();
		}

		public virtual bool IsItemRelevant(Item item)
		{
			return (item.Type == null || RelevantItemTypes.Contains(item.Type)) &&
				HasRelevantStats(item.Stats);
		}
	}

	/// <summary>
	/// Base CharacterCalculations class, which will hold the final result data of the calculations from
	/// GetCharacterCalculations(). Your class should inherit from this, and include fields to hold the
	/// needed data. May also include additional data needed to properly format the display calculations,
	/// such as values of some of the CalculationOptions.
	/// </summary>
	public abstract class CharacterCalculationsBase
	{
		/// <summary>
		/// The Overall rating points for the whole character.
		/// </summary>
		public abstract float OverallPoints { get; set; }
		
		/// <summary>
		/// The Sub rating points for the whole character, in the order defined in SubPointNameColors.
		/// Should sum up to OverallPoints.
		/// </summary>
		public abstract float[] SubPoints { get; set; }

		/// <summary>
		/// Builds a dictionary containing the values to display for each of the calculations defined in 
		/// CharacterDisplayCalculationLabels. The key should be the Label of each display calculation, 
		/// and the value should be the value to display, optionally appended with '*' followed by any 
		/// string you'd like displayed as a tooltip on the value.
		/// </summary>
		/// <returns>A Dictionary<string, string> containing the values to display for each of the 
		/// calculations defined in CharacterDisplayCalculationLabels.</returns>
		public abstract Dictionary<string, string> GetCharacterDisplayCalculationValues();
	}

	/// <summary>
	/// TODO
	/// </summary>
	public abstract class ComparisonCalculationBase
	{
		public abstract string Name { get; set; }
		public abstract float OverallPoints { get; set; }
		public abstract float[] SubPoints { get; set; }
		public abstract Item Item { get; set; }
		public abstract bool Equipped { get; set; }
	}

	/// <summary>
	/// TODO
	/// </summary>
	public class CalculationOptionsPanelBase : System.Windows.Forms.UserControl
	{

        public virtual System.Drawing.Icon Icon
        {
            get;
            set;
        }

		private Character _character = null;
		public Character Character
		{
			get
			{
				return _character;
			}
			set
			{
				_character = value;
				LoadCalculationOptions();
			}
		}

		protected virtual void LoadCalculationOptions() { }
	}

}
//takemyhandigiveittoyounowyouownmealliamyousaidyouwouldneverleavemeibelieveyouibelieve