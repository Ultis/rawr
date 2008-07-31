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
		private static Dictionary<string, string> _modelIcons = null;
		public static Dictionary<string, string> ModelIcons
		{
			get
			{
				if (_modelIcons == null)
				{
					Models.ToString();
				}
				return _modelIcons;
			}
		}

		private static SortedList<string, Type> _models = null;
		public static SortedList<string, Type> Models
		{
			get
			{
				if (_models == null)
				{
					_models = new SortedList<string, Type>();
					_modelIcons = new Dictionary<string, string>();

                    string dir = AppDomain.CurrentDomain.BaseDirectory;
                    // when running in service the dlls are in relative search path
                    if (AppDomain.CurrentDomain.RelativeSearchPath != null) dir = AppDomain.CurrentDomain.RelativeSearchPath;
					DirectoryInfo info = new DirectoryInfo(dir);
					foreach (FileInfo file in info.GetFiles("*.dll"))
					{
						try
						{
							Assembly assembly = Assembly.LoadFrom(file.FullName);

							foreach (Type type in assembly.GetTypes())
							{
								if (type.IsSubclassOf(typeof(CalculationsBase)))
								{
									DisplayNameAttribute[] displayNameAttributes = type.GetCustomAttributes(typeof(DisplayNameAttribute), false) as DisplayNameAttribute[];
									string[] displayName = type.Name.Split('|');
									if (displayNameAttributes.Length > 0)
										displayName = displayNameAttributes[0].DisplayName.Split('|');
									_models[displayName[0]] = type;
									_modelIcons[displayName[0]] = displayName[1];
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

        public static event EventHandler ModelChanging;
        protected static void OnModelChanging()
        {
            if (ModelChanging != null)
                ModelChanging(null, EventArgs.Empty);
        }

        private static Dictionary<Type, CalculationsBase> ModelCache = new Dictionary<Type, CalculationsBase>();

        public static CalculationsBase GetModel(Type type)
        {
            CalculationsBase model;
            if (!ModelCache.TryGetValue(type, out model))
            {
                model = (CalculationsBase)Activator.CreateInstance(type);
                ModelCache[type] = model;
            }
            return model;
        }
        public static CalculationsBase GetModel(string model) { return GetModel(Models[model]); }

		public static void LoadModel(Type type) { LoadModel(GetModel(type)); }
		public static void LoadModel(CalculationsBase model)
		{
            OnModelChanging();
            Instance = model;
			OnModelChanged();
		}

		private static CalculationsBase _instance;
		public static CalculationsBase Instance
		{
			get { return _instance; }
			private set { _instance = value; }
		}

		//public static Character CachedCharacter
		//{
		//    get { return Instance.CachedCharacter; } 
		//}
        public static bool CanUseAmmo
        {
            get { return Instance.CanUseAmmo; }
        }
		public static string[] CharacterDisplayCalculationLabels
		{
			get { return Instance.CharacterDisplayCalculationLabels; }
		}
		public static string[] OptimizableCalculationLabels 
		{
			get { return Instance.OptimizableCalculationLabels; } 
		}
		public static string[] CustomChartNames
		{
			get { return Instance.CustomChartNames; }
		}
		public static Dictionary<string, System.Drawing.Color> SubPointNameColors
		{
			get { return Instance.SubPointNameColors; }
		}
		public static CalculationOptionsPanelBase CalculationOptionsPanel
		{
			get { return Instance.CalculationOptionsPanel; }
		}
		public static Character.CharacterClass TargetClass
		{
			get { return Instance.TargetClass; }
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
		public static ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
		{
			return Instance.GetCustomChartData(character, chartName);
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
		public static ICalculationOptionBase DeserializeDataObject(string xml)
		{
			if (Instance != null)
				return Instance.DeserializeDataObject(xml);
			return null;
		}
      
		public static string GetCharacterStatsString(Character character)
		{
			return Instance.GetCharacterStatsString(character);
		}

        public static string ValidModel(string model)
        {
            if (Models.Keys.Contains(model))
            {
                return model;
            }
            if(Models.Keys.Count > 0)
            {
                return Models.Keys[0];
            }
            return null;
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
		/// The names of all custom charts provided by the model, if any.
		/// </summary>
		public abstract string[] CustomChartNames { get; }

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
		/// Character class that this model is for.
		/// </summary>
		public abstract Character.CharacterClass TargetClass { get; }
		
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

		/// <summary>
		/// An array of strings which define what calculations (in addition to the subpoint ratings)
		/// will be available to the optimizer
		/// </summary>
		public virtual string[] OptimizableCalculationLabels { get { return new string[0]; } }

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
		/// Gets data to fill a custom chart, based on the chart name, as defined in CustomChartNames.
		/// </summary>
		/// <param name="character">The character to build the chart for.</param>
		/// <param name="chartName">The name of the custom chart to get data for.</param>
		/// <returns>The data for the custom chart.</returns>
		public abstract ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName);
		
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
		
		/// <summary>
		/// Deserializes the model's CalculationOptions data object from xml
		/// </summary>
		/// <param name="xml">The serialized xml representing the model's CalculationOptions data object.</param>
		/// <returns>The model's CalculationOptions data object.</returns>
		public abstract ICalculationOptionBase DeserializeDataObject(string xml);

		public virtual void ClearCache()
		{
			_cachedCharacterStatsWithSlotEmpty = null;
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
			ClearCache();
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
				enchantCalc.Item = new Item(enchant.Name, Item.ItemQuality.Temp, Item.ItemType.None, 
					-1 * (enchant.Id + (10000 * (int)enchant.Slot)), null, Item.ItemSlot.None, null, false, enchant.Stats, new Sockets(), 0, 0, 0, 0, 0,
					Item.ItemDamageType.Physical, 0, null);
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

        public static void RemoveConflictingBuffs(List<Buff> activeBuffs, Buff buff)
        {
            //Buff b = Buff.GetBuffByName(buff);

			if (buff != null)
			{
				for (int i = 0; i < activeBuffs.Count; i++)
				{
					if (activeBuffs[i] != buff)
					{
						//Buff b2 = Buff.GetBuffByName(activeBuffs[i]);
                        Buff b2 = activeBuffs[i];
                        foreach (string buffName in b2.ConflictingBuffs)
						{
							if (Array.IndexOf<string>(buff.ConflictingBuffs, buffName) >= 0)
							{
								activeBuffs.RemoveAt(i);
								i--;
								break;
							}
						}
					}
				}
			}
        }

		public virtual List<ComparisonCalculationBase> GetBuffCalculations(Character character, CharacterCalculationsBase currentCalcs, Buff.BuffType buffType, bool activeOnly)
		{
			ClearCache();
			List<ComparisonCalculationBase> buffCalcs = new List<ComparisonCalculationBase>();
			CharacterCalculationsBase calcsEquipped = null;
			CharacterCalculationsBase calcsUnequipped = null;
            Character charAutoActivated = character.Clone();
            foreach (Buff autoBuff in currentCalcs.AutoActivatedBuffs)
            {
                if (!charAutoActivated.ActiveBuffs.Contains(autoBuff))
                {
                    charAutoActivated.ActiveBuffs.Add(autoBuff);
                    RemoveConflictingBuffs(charAutoActivated.ActiveBuffs, autoBuff);
                }
            }
            charAutoActivated.DisableBuffAutoActivation = true;
            foreach (Buff buff in Buff.GetBuffsByType(buffType))
			{
                if (!activeOnly || charAutoActivated.ActiveBuffs.Contains(buff))
				{
                    Character charUnequipped = charAutoActivated.Clone();
                    Character charEquipped = charAutoActivated.Clone();
                    charUnequipped.DisableBuffAutoActivation = true;
                    charEquipped.DisableBuffAutoActivation = true;
					if (charUnequipped.ActiveBuffs.Contains(buff))
						charUnequipped.ActiveBuffs.Remove(buff);
					if (string.IsNullOrEmpty(buff.RequiredBuff))
					{
						//if (charUnequipped.ActiveBuffs.Contains("Improved " + buff.Name))
						//	charUnequipped.ActiveBuffs.Remove("Improved " + buff.Name);
                        charUnequipped.ActiveBuffs.RemoveAll(x => x.Name == "Improved " + buff.Name);
					}
					else
						//if (charUnequipped.ActiveBuffs.Contains(buff.RequiredBuff))
						//	charUnequipped.ActiveBuffs.Remove(buff.RequiredBuff);
                        charUnequipped.ActiveBuffs.RemoveAll(x => x.Name == buff.RequiredBuff);

					if (!charEquipped.ActiveBuffs.Contains(buff))
						charEquipped.ActiveBuffs.Add(buff);
                    if (string.IsNullOrEmpty(buff.RequiredBuff))
                    {
                        //if (charEquipped.ActiveBuffs.Contains("Improved " + buff.Name))
                        //	charEquipped.ActiveBuffs.Remove("Improved " + buff.Name);
                        charEquipped.ActiveBuffs.RemoveAll(x => x.Name == "Improved " + buff.Name);
                    }
                    else
                    {
                        //if (!charEquipped.ActiveBuffs.Contains(buff.RequiredBuff))
                        //	charEquipped.ActiveBuffs.Add(buff.RequiredBuff);
                        Buff requiredBuff = Buff.GetBuffByName(buff.RequiredBuff);
                        if (!charEquipped.ActiveBuffs.Contains(requiredBuff))
                        	charEquipped.ActiveBuffs.Add(requiredBuff);
                    }

                    RemoveConflictingBuffs(charEquipped.ActiveBuffs, buff);
                    RemoveConflictingBuffs(charUnequipped.ActiveBuffs, buff);

					calcsUnequipped = GetCharacterCalculations(charUnequipped);
					calcsEquipped = GetCharacterCalculations(charEquipped);

					ComparisonCalculationBase buffCalc = CreateNewComparisonCalculation();
					buffCalc.Name = buff.Name;
                    buffCalc.Item = new Item() { Name = buff.Name, Stats = buff.Stats, Quality = Item.ItemQuality.Temp };
                    buffCalc.Equipped = charAutoActivated.ActiveBuffs.Contains(buff);
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

        public unsafe virtual void AccumulateItemStats(Stats stats, Character character, Item additionalItem)
		{
			List<Item> items = new List<Item>(new Item[] {character.Back, character.Chest, character.Feet, character.Finger1,
				character.Finger2, character.Hands, character.Head, character.Legs, character.Neck,
				character.Shirt, character.Shoulders, character.Tabard, character.Trinket1, character.Trinket2,
				character.Waist, character.MainHand, character.Ranged, character.Projectile, 
				character.ProjectileBag, character.Wrist});
			if (additionalItem != null)
				items.Add(additionalItem);
			if (character.MainHand == null || character.MainHand.Slot != Item.ItemSlot.TwoHand)
				items.Add(character.OffHand);

            fixed (float* pRawAdditiveData = stats._rawAdditiveData, pRawMultiplicativeData = stats._rawMultiplicativeData, pRawNoStackData = stats._rawNoStackData)
            {
                stats.BeginUnsafe(pRawAdditiveData, pRawMultiplicativeData, pRawNoStackData);
                foreach (Item item in items)
                    if (item != null)
                        stats.AccumulateUnsafe(item.GetTotalStats(character));
                stats.EndUnsafe();
            }
		}

        public virtual Stats GetItemStats(Character character, Item additionalItem)
        {
            Stats stats = new Stats();
            AccumulateItemStats(stats, character, additionalItem);
            return stats;
        }

        public unsafe virtual void AccumulateEnchantsStats(Stats stats, Character character)
        {
            fixed (float* pRawAdditiveData = stats._rawAdditiveData, pRawMultiplicativeData = stats._rawMultiplicativeData, pRawNoStackData = stats._rawNoStackData)
            {
                stats.BeginUnsafe(pRawAdditiveData, pRawMultiplicativeData, pRawNoStackData);
                stats.AccumulateUnsafe(character.BackEnchant.Stats);
                stats.AccumulateUnsafe(character.ChestEnchant.Stats);
                stats.AccumulateUnsafe(character.FeetEnchant.Stats);
                stats.AccumulateUnsafe(character.Finger1Enchant.Stats);
                stats.AccumulateUnsafe(character.Finger2Enchant.Stats);
                stats.AccumulateUnsafe(character.HandsEnchant.Stats);
                stats.AccumulateUnsafe(character.HeadEnchant.Stats);
                stats.AccumulateUnsafe(character.LegsEnchant.Stats);
                stats.AccumulateUnsafe(character.ShouldersEnchant.Stats);
                if (character.MainHand != null &&
                    (character.MainHandEnchant.Slot == Item.ItemSlot.OneHand ||
                    (character.MainHandEnchant.Slot == Item.ItemSlot.TwoHand &&
                    character.MainHand.Slot == Item.ItemSlot.TwoHand)))
                {
                    stats.AccumulateUnsafe(character.MainHandEnchant.Stats);
                }
                if (character.OffHand != null && (character.MainHand == null || character.MainHand.Slot != Item.ItemSlot.TwoHand) &&
                    (
                        (
                            character.OffHandEnchant.Slot == Item.ItemSlot.OneHand &&
                            (character.OffHand.Slot == Item.ItemSlot.OneHand ||
                            character.OffHand.Slot == Item.ItemSlot.OffHand) &&
                            character.OffHand.Type != Item.ItemType.None &&
                            character.OffHand.Type != Item.ItemType.Shield
                        )
                        ||
                        (
                            character.OffHandEnchant.Slot == Item.ItemSlot.OffHand &&
                            character.OffHand.Slot == Item.ItemSlot.OffHand &&
                            character.OffHand.Type == Item.ItemType.Shield
                        )
                    )
                   )
                {
                    stats.AccumulateUnsafe(character.OffHandEnchant.Stats);
                }
                stats.AccumulateUnsafe(character.RangedEnchant.Stats);
                stats.AccumulateUnsafe(character.WristEnchant.Stats);
                stats.EndUnsafe();
            }
        }

		public virtual Stats GetEnchantsStats(Character character)
		{
            Stats stats = new Stats();
            AccumulateEnchantsStats(stats, character);
            return stats;
        }

        public virtual void AccumulateBuffsStats(Stats stats, IEnumerable<string> buffs)
        {
            foreach (string buffName in buffs)
                if (!string.IsNullOrEmpty(buffName))
                {
                    Buff buff = Buff.GetBuffByName(buffName);
                    if (buff != null)
                    {
                        stats.Accumulate(buff.Stats);
                    }
                }
        }

        public unsafe virtual void AccumulateBuffsStats(Stats stats, IEnumerable<Buff> buffs)
        {
            fixed (float* pRawAdditiveData = stats._rawAdditiveData, pRawMultiplicativeData = stats._rawMultiplicativeData, pRawNoStackData = stats._rawNoStackData)
            {
                stats.BeginUnsafe(pRawAdditiveData, pRawMultiplicativeData, pRawNoStackData);
                foreach (Buff buff in buffs)
                    if (buff != null)
                    {
                        stats.AccumulateUnsafe(buff.Stats);
                    }
                stats.EndUnsafe();
            }
        }

		public virtual Stats GetBuffsStats(IEnumerable<string> buffs)
		{
            Stats stats = new Stats();
            AccumulateBuffsStats(stats, buffs);
            return stats;
		}

        public virtual Stats GetBuffsStats(IEnumerable<Buff> buffs)
        {
            Stats stats = new Stats();
            AccumulateBuffsStats(stats, buffs);
            return stats;
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
			try
			{
				return (string.IsNullOrEmpty(item.RequiredClasses) || item.RequiredClasses.Contains(TargetClass.ToString())) &&
					(RelevantItemTypes.Contains(item.Type)) && HasRelevantStats(item.Stats);
			}
			catch (Exception )
			{
				return false;
			}
		}

        public virtual bool CanUseAmmo
        {
            get { return false; }
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
		/// Should sum up to OverallPoints. You could have this field build/parse an array of floats based
		/// on floats stored in other fields, or you could have this get/set a private float[], and
		/// have the fields of your individual Sub points refer to specific indexes of this field.
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

        /// <summary>
        /// List of buffs that were automatically activated by the model during GetCharacterCalculations().
        /// </summary>
        private List<Buff> _autoActivatedBuffs = new List<Buff>();
        public List<Buff> AutoActivatedBuffs
        {
            get
            {
                return _autoActivatedBuffs;
            }
        }

		public virtual float GetOptimizableCalculationValue(string calculation) { return 0f; }
	}

	/// <summary>
	/// Base ComparisonCalculation class, which will hold the final result data of rating calculations
	/// of an individual Item/Enchant/Buff/etc. Most likely, no additional fields will be needed,
	/// other than fields for each individual Sub rating.
	/// </summary>
	public abstract class ComparisonCalculationBase
	{
		/// <summary>
		/// The Name of the object being rated. This will show up as the label on the left, in the chart.
		/// </summary>
		public abstract string Name { get; set; }

		/// <summary>
		/// The Overall rating points for the object being rated.
		/// </summary>
		public abstract float OverallPoints { get; set; }

		/// <summary>
		/// The Sub rating points for the object being rated, in ther order defined in SubPointNameColors.
		/// May refer to a private float[], or build/parse a float[] and get/set the individual Sub point
		/// fields.
		/// </summary>
		public abstract float[] SubPoints { get; set; }

		/// <summary>
		/// The Item, or other object, being rated. This property is used to build the tooltip for this
		/// object in the chart. If this is null, no tooltip will be displayed. If the object is not an
		/// Item, a new blank item may be created for this field, containing just a Name and Stats.
		/// </summary>
		public abstract Item Item { get; set; }

		/// <summary>
		/// Whether the object beign rated is currently equipped by the character. This controls whether
		/// the item's label is highlighted in light blue on the charts.
		/// </summary>
		public abstract bool Equipped { get; set; }

        /// <summary>
        /// Complete gear set that includes item in Item based on which the OverallPoints and SubPoints
        /// are based. Used by optimizer upgrade calculations.
        /// </summary>
        public Character Character { get; set; }

        /// <summary>
        /// Enchant associated with the Item. Used by optimizer upgrade calculations.
        /// </summary>
        public Enchant Enchant { get; set; }
    }

	/// <summary>
	/// Base CalculationOptionsPanel class which should be inherited by a custom user control for the model.
	/// The instance of the custom class returned by CalculationOptionsPanel will be placed in the Options 
	/// tab on the main form when the model is active. Should contain controls to edit the CalculationOptions
	/// on the character.
	/// </summary>
	public class CalculationOptionsPanelBase : System.Windows.Forms.UserControl
	{
		private Character _character = null;
		/// <summary>
		/// The current character. Will be set whenever the model loads or a character is loaded.
		/// 
		/// IMPORTANT: Call Character.OnItemsChanged() after changing the value of any CalculationOptions,
		/// other than in LoadCalculationOptions().
		/// </summary>
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

		/// <summary>
		/// Sets default values for each CalculationOption used by the model, if they don't already exist, 
		/// and then populates the controls with the current values in Character.CalculationOptions. You
		/// don't need to call Character.OnItemsChanged() at the end of this method, only after changing the
		/// value of any CalculationOptions from other methods (such as value changing event handlers on
		/// the controls).
		/// </summary>
		protected virtual void LoadCalculationOptions() { }
	}

}
//takemyhandigiveittoyounowyouownmealliamyousaidyouwouldneverleavemeibelieveyouibelieve
