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
			get
			{
				return _instance;
			}
			private set
			{
				_instance = value;
			}
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

	public abstract class CalculationsBase
	{
		protected CharacterCalculationsBase _cachedCharacterStatsWithSlotEmpty = null;
		protected Character.CharacterSlot _cachedSlot = Character.CharacterSlot.Shirt;
		protected Character _cachedCharacter = null;
		public virtual Character CachedCharacter { get { return _cachedCharacter; } }
		
		public abstract Dictionary<string, System.Drawing.Color> SubPointNameColors { get; }
		public abstract string[] CharacterDisplayCalculationLabels { get; }
		public abstract CalculationOptionsPanelBase CalculationOptionsPanel { get; }
		public abstract List<Item.ItemType> RelevantItemTypes { get; }
		
		public abstract ComparisonCalculationBase CreateNewComparisonCalculation();
		public abstract CharacterCalculationsBase CreateNewCharacterCalculations();

		public abstract CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem);
		public abstract Stats GetCharacterStats(Character character, Item additionalItem);
		public abstract ComparisonCalculationBase[] GetCombatTable(CharacterCalculationsBase currentCalculations);
		public abstract Stats GetRelevantStats(Stats stats);
		public abstract bool HasRelevantStats(Stats stats);
		public virtual CharacterCalculationsBase GetCharacterCalculations(Character character) { return GetCharacterCalculations(character, null); }
		public virtual Stats GetCharacterStats(Character character) { return GetCharacterStats(character, null); }
		
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

	public abstract class CharacterCalculationsBase
	{
		public abstract float OverallPoints { get; set; }
		public abstract float[] SubPoints { get; set; }
		public abstract Dictionary<string, string> GetCharacterDisplayCalculationValues();
	}

	public abstract class ComparisonCalculationBase
	{
		public abstract string Name { get; set; }
		public abstract float OverallPoints { get; set; }
		public abstract float[] SubPoints { get; set; }
		public abstract Item Item { get; set; }
		public abstract bool Equipped { get; set; }
	}

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