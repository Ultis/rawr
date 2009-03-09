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

		private static Dictionary<string, Character.CharacterClass> _modelClasses = null;
		public static Dictionary<string, Character.CharacterClass> ModelClasses
		{
			get
			{
				if (_modelClasses == null)
				{
					Models.ToString();
				}
				return _modelClasses;
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
					_modelClasses = new Dictionary<string, Character.CharacterClass>();

                    string dir = AppDomain.CurrentDomain.BaseDirectory + "Data";
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
									RawrModelInfoAttribute[] modelInfos = type.GetCustomAttributes(typeof(RawrModelInfoAttribute), false) as RawrModelInfoAttribute[];
									string[] displayName = type.Name.Split('|');
									//if (displayNameAttributes.Length > 0)
									//	displayName = displayNameAttributes[0].DisplayName.Split('|');
									_models[modelInfos[0].Name] = type;
									_modelIcons[modelInfos[0].Name] = modelInfos[0].IconPath;
									_modelClasses[modelInfos[0].Name] = modelInfos[0].TargetClass;
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

        public static CalculationsBase GetModel(string model) { return GetModel(Models[model]); }
		public static CalculationsBase GetModel(Type type)
        {
            CalculationsBase model;
            if (!ModelCache.TryGetValue(type, out model))
            {
                model = (CalculationsBase)Activator.CreateInstance(type);
                // extract name
                RawrModelInfoAttribute[] modelInfos = type.GetCustomAttributes(typeof(RawrModelInfoAttribute), false) as RawrModelInfoAttribute[];
                model.Name = modelInfos[0].Name;
                ModelCache[type] = model;
            }
            return model;
        }
        
		public static void LoadModel(Type type) { LoadModel(GetModel(type)); }
		public static void LoadModel(CalculationsBase model)
		{
			if (Instance != model)
			{
				OnModelChanging();
				Instance = model;
				OnModelChanged();
			}
		}

		[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
		public sealed class RawrModelInfoAttribute : Attribute
		{
			public RawrModelInfoAttribute(string name, string iconPath, Character.CharacterClass targetClass)
			{
				_name = name;
				_iconPath = iconPath;
				_targetClass = targetClass;
			}

			private readonly string _name;
			private readonly string _iconPath;
			private readonly Character.CharacterClass _targetClass;

			public string Name { get { return _name; } }
			public string IconPath { get { return _iconPath; } }
			public Character.CharacterClass TargetClass { get { return _targetClass; } }
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
		public static bool SupportsMultithreading
		{
			get { return Instance.SupportsMultithreading; }
		}
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
        public static string[] CustomRenderedChartNames
        {
            get { return Instance.CustomRenderedChartNames; }
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
		public static ComparisonCalculationBase GetItemCalculations(ItemInstance item, Character character, Character.CharacterSlot slot)
		{
			return Instance.GetItemCalculations(item, character, slot);
		}
        public static ComparisonCalculationBase GetItemCalculations(Item item, Character character, Character.CharacterSlot slot)
        {
            return Instance.GetItemCalculations(item, character, slot);
        }
        public static List<ComparisonCalculationBase> GetEnchantCalculations(Item.ItemSlot slot, Character character, CharacterCalculationsBase currentCalcs)
		{
			return Instance.GetEnchantCalculations(slot, character, currentCalcs);
		}
		public static List<ComparisonCalculationBase> GetBuffCalculations(Character character, CharacterCalculationsBase currentCalcs, bool activeOnly)
		{
			return Instance.GetBuffCalculations(character, currentCalcs, activeOnly);
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
		/*public static Stats GetEnchantsStats(Character character)
		{
			return Instance.GetEnchantsStats(character);
		}*/
		public static Stats GetBuffsStats(List<string> buffs)
		{
			return Instance.GetBuffsStats(buffs);
		}
		public static ComparisonCalculationBase GetCharacterComparisonCalculations(CharacterCalculationsBase baseCalculations,
			Character character, string name, bool equipped)
		{
			return Instance.GetCharacterComparisonCalculations(baseCalculations, character, name, equipped);
		}
		public static ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
		{
			return Instance.GetCustomChartData(character, chartName);
		}
        public static void RenderCustomChart(Character character, string chartName, System.Drawing.Graphics g, int width, int height)
        {
            Instance.RenderCustomChart(character, chartName, g, width, height);
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
		public static bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot)
		{
			if (Instance != null)
				return Instance.ItemFitsInSlot(item, character, slot);
			return false;
		}
		public static bool EnchantFitsInSlot(Enchant item, Character character, Item.ItemSlot slot)
		{
			if (Instance != null)
				return Instance.EnchantFitsInSlot(item, character, slot);
			return false;
		}
		public static bool IncludeOffHandInCalculations(Character character)
		{
			if (Instance != null)
				return Instance.IncludeOffHandInCalculations(character);
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
        /// <summary>
        /// Name of the model. When loaded in model cache, it's assigned the value of its corresponding key
        /// </summary>
        public string Name;

		/// <summary>
		/// If true, the ThreadPool will be used to more quickly run calculations on different items
		/// </summary>
		public virtual bool SupportsMultithreading { get { return true; } }

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
        /// The names of charts for which the model provides custom rendering.
        /// </summary>
        public virtual string[] CustomRenderedChartNames { get { return new string[] { }; } }

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
        /// Render custom chart, based on the chart name, as defined in CustomRenderedChartNames.
        /// </summary>
        /// <param name="character">The character to build the chart for.</param>
        /// <param name="chartName">The name of the custom chart to get data for.</param>
        /// <param name="g">Graphics object used to render the chart.</param>
        /// <param name="width">Width of the graph.</param>
        /// <param name="height">Height of the graph.</param>
        public virtual void RenderCustomChart(Character character, string chartName, System.Drawing.Graphics g, int width, int height) { }

        /// <summary>
        /// List of default gemming templates recommended by the model
        /// </summary>
        public abstract List<GemmingTemplate> DefaultGemmingTemplates
        {
            get;
        }

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

		public virtual ComparisonCalculationBase GetItemCalculations(ItemInstance item, Character character, Character.CharacterSlot slot)
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

			CharacterCalculationsBase characterStatsWithNewItem = GetCharacterCalculations(characterWithNewItem, null);

			ComparisonCalculationBase itemCalc = CreateNewComparisonCalculation();
			itemCalc.ItemInstance = item;
            itemCalc.Item = item.Item;
			itemCalc.Name = item.Item.Name;
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

        public virtual ComparisonCalculationBase GetItemCalculations(Item additionalItem, Character character, Character.CharacterSlot slot)
        {
            bool useCache = character == _cachedCharacter && slot == _cachedSlot;
            Character characterWithSlotEmpty = null;

            if (!useCache)
                characterWithSlotEmpty = character.Clone();
            Character characterWithNewItem = character.Clone();

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

            CharacterCalculationsBase characterStatsWithNewItem = GetCharacterCalculations(characterWithNewItem, additionalItem);

            ComparisonCalculationBase itemCalc = CreateNewComparisonCalculation();
            itemCalc.Item = additionalItem;
            itemCalc.Name = additionalItem.Name;
            itemCalc.Equipped = false;
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
			foreach (Enchant enchant in Enchant.FindEnchants(slot, character))
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
					-1 * (enchant.Id + (10000 * (int)enchant.Slot)), null, Item.ItemSlot.None, null, 
                    false, enchant.Stats, null, Item.ItemSlot.None, Item.ItemSlot.None, Item.ItemSlot.None,
                    0, 0, Item.ItemDamageType.Physical, 0, null);
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
							if (buff.ConflictingBuffs.Contains(buffName))
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

		public virtual List<ComparisonCalculationBase> GetBuffCalculations(Character character, CharacterCalculationsBase currentCalcs, bool activeOnly)
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

			List<Buff> relevantBuffs = new List<Buff>();
			foreach (Buff buff in Buff.RelevantBuffs)
			{
				relevantBuffs.Add(buff);
				relevantBuffs.AddRange(buff.Improvements);
			}

			foreach (Buff buff in relevantBuffs)
			{
                if (!activeOnly || charAutoActivated.ActiveBuffs.Contains(buff))
				{
                    Character charUnequipped = charAutoActivated.Clone();
                    Character charEquipped = charAutoActivated.Clone();
                    charUnequipped.DisableBuffAutoActivation = true;
                    charEquipped.DisableBuffAutoActivation = true;
					if (charUnequipped.ActiveBuffs.Contains(buff))
						charUnequipped.ActiveBuffs.Remove(buff);
					if (!charEquipped.ActiveBuffs.Contains(buff))
						charEquipped.ActiveBuffs.Add(buff);
					//if (string.IsNullOrEmpty(buff.RequiredBuff))
					//{
					//    charUnequipped.ActiveBuffs.RemoveAll(x => x.Name == "Improved " + buff.Name);
					//}
					//else
					//    charUnequipped.ActiveBuffs.RemoveAll(x => x.Name == buff.RequiredBuff);

					//if (!charEquipped.ActiveBuffs.Contains(buff))
					//    charEquipped.ActiveBuffs.Add(buff);
					//if (string.IsNullOrEmpty(buff.RequiredBuff))
					//{
					//    charEquipped.ActiveBuffs.RemoveAll(x => x.Name == "Improved " + buff.Name);
					//}
					//else
					//{
					//    Buff requiredBuff = Buff.GetBuffByName(buff.RequiredBuff);
					//    if (!charEquipped.ActiveBuffs.Contains(requiredBuff))
					//        charEquipped.ActiveBuffs.Add(requiredBuff);
					//}

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

		public virtual ComparisonCalculationBase GetCharacterComparisonCalculations(CharacterCalculationsBase baseCalculations, 
			Character character, string name, bool equipped)
		{
			CharacterCalculationsBase characterCalculations = GetCharacterCalculations(character);
			ComparisonCalculationBase comparisonCalculations = CreateNewComparisonCalculation();
			comparisonCalculations.Name = name;
			comparisonCalculations.Item = new Item() { Name = name };
			comparisonCalculations.Equipped = equipped;
			comparisonCalculations.OverallPoints = characterCalculations.OverallPoints - baseCalculations.OverallPoints;
			float[] subPoints = new float[characterCalculations.SubPoints.Length];
			for (int i = 0; i < characterCalculations.SubPoints.Length; i++)
			{
				subPoints[i] = characterCalculations.SubPoints[i] - baseCalculations.SubPoints[i];
			}
			comparisonCalculations.SubPoints = subPoints;
			return comparisonCalculations;
		}

        public unsafe virtual void AccumulateItemStats(Stats stats, Character character, Item additionalItem)
		{
			List<ItemInstance> items = new List<ItemInstance>(new ItemInstance[] {character.Back, character.Chest, character.Feet, character.Finger1,
				character.Finger2, character.Hands, character.Head, character.Legs, character.Neck,
				character.Shirt, character.Shoulders, character.Tabard, character.Trinket1, character.Trinket2,
				character.Waist, character.MainHand, character.Ranged, character.Projectile, 
				character.ProjectileBag, character.Wrist/*, character.ExtraHandsSocket, character.ExtraWaistSocket,
				character.ExtraWristSocket*/});
			if (IncludeOffHandInCalculations(character))
				items.Add(character.OffHand);

            fixed (float* pRawAdditiveData = stats._rawAdditiveData, pRawMultiplicativeData = stats._rawMultiplicativeData, pRawNoStackData = stats._rawNoStackData)
            {
                stats.BeginUnsafe(pRawAdditiveData, pRawMultiplicativeData, pRawNoStackData);
                foreach (ItemInstance item in items)
                    if ((object)item != null)
                        stats.AccumulateUnsafe(item.GetTotalStats(character));
                if (additionalItem != null)
                    stats.AccumulateUnsafe(additionalItem.Stats);
                stats.EndUnsafe();
            }
		}

        public virtual Stats GetItemStats(Character character, Item additionalItem)
        {
            Stats stats = new Stats();
            AccumulateItemStats(stats, character, additionalItem);
            return stats;
        }

        // this is now included in accumulating item stats
        /*public unsafe virtual void AccumulateEnchantsStats(Stats stats, Character character)
        {
            fixed (float* pRawAdditiveData = stats._rawAdditiveData, pRawMultiplicativeData = stats._rawMultiplicativeData, pRawNoStackData = stats._rawNoStackData)
            {
                stats.BeginUnsafe(pRawAdditiveData, pRawMultiplicativeData, pRawNoStackData);
                stats.AccumulateUnsafe(character.BackEnchant.Stats, true);
                stats.AccumulateUnsafe(character.ChestEnchant.Stats, true);
                stats.AccumulateUnsafe(character.FeetEnchant.Stats, true);
                stats.AccumulateUnsafe(character.Finger1Enchant.Stats, true);
                stats.AccumulateUnsafe(character.Finger2Enchant.Stats, true);
                stats.AccumulateUnsafe(character.HandsEnchant.Stats, true);
                stats.AccumulateUnsafe(character.HeadEnchant.Stats, true);
                stats.AccumulateUnsafe(character.LegsEnchant.Stats, true);
                stats.AccumulateUnsafe(character.ShouldersEnchant.Stats, true);
                if (character.MainHand != null &&
                    (character.MainHandEnchant.Slot == Item.ItemSlot.OneHand ||
                    (character.MainHandEnchant.Slot == Item.ItemSlot.TwoHand &&
                    character.MainHand.Slot == Item.ItemSlot.TwoHand)))
                {
                    stats.AccumulateUnsafe(character.MainHandEnchant.Stats, true);
                }
                if (character.OffHand != null && IncludeOffHandInCalculations(character) &&
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
							character.OffHandEnchant.Slot == Item.ItemSlot.TwoHand &&
							character.OffHand.Slot == Item.ItemSlot.TwoHand
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
                    stats.AccumulateUnsafe(character.OffHandEnchant.Stats, true);
                }
                stats.AccumulateUnsafe(character.RangedEnchant.Stats, true);
                stats.AccumulateUnsafe(character.WristEnchant.Stats, true);
                stats.EndUnsafe();
            }
        }*/

        /*
		public virtual Stats GetEnchantsStats(Character character)
		{
            Stats stats = new Stats();
            AccumulateEnchantsStats(stats, character);
            return stats;
        }*/

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
                        stats.AccumulateUnsafe(buff.Stats, true);
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
				return (string.IsNullOrEmpty(item.RequiredClasses) || item.RequiredClasses.Replace(" ","").Contains(TargetClass.ToString())) &&
					(RelevantItemTypes.Contains(item.Type)) && HasRelevantStats(item.Stats);
			}
			catch (Exception )
			{
				return false;
			}
		}

		public virtual bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot)
		{
			return item.FitsInSlot(slot);
		}

		public virtual bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot)
		{
			return enchant.FitsInSlot(slot);
		}

		public virtual bool IncludeOffHandInCalculations(Character character)
		{
			return (object)character.MainHand == null || character.MainHand.Slot != Item.ItemSlot.TwoHand;
		}

        public virtual bool CanUseAmmo
        {
            get { return false; }
        }

        /// <summary>
        /// Get the relative stat values for all relevant Stats of the given Character and it's current Model.
        /// See http://www.wowhead.com/?help=stat-weighting for more info of stat values.
        /// Note that these are volatile and should be updated whenever any aspect of the character is changed.
        /// </summary>
        /// <param name="character">The character for which to calculate the relative stat values.</param>
        /// <returns>An array of the comparitive calculations for each of the relative stat values.</returns>
        public static ComparisonCalculationBase[] GetRelativeStatValues(Character character)
        {
            List<ComparisonCalculationBase> retComparisonCalcutions = new List<ComparisonCalculationBase>();
            //
			Stats allStats = new Stats();
			for (int i = 0; i < allStats._rawAdditiveData.Length; i++) allStats._rawAdditiveData[i] = 1f;
			
            float divisor = 1.0f;
            IDictionary<PropertyInfo, float> relevantStats = Calculations.GetRelevantStats(allStats).Values(x => x > 0);
            foreach (KeyValuePair<PropertyInfo, float> pair in relevantStats)
            {
                ComparisonCalculationBase ccb = GetRelativeStatValue(character, pair.Key);
                if (ccb != null && ccb.OverallPoints > 0f) { 
                    retComparisonCalcutions.Add(ccb); 
                    if(ccb.getBaseStatOption(character) && ccb.Name.Equals(ccb.BaseStat))
                        divisor = ccb.OverallPoints;
                }
            }
            // apply Relative stats divisor to adjust scaling to 1.00 for Base Stat
            if (divisor != 1.0f && divisor != 0.0f)
                foreach (ComparisonCalculationBase ccb in retComparisonCalcutions)
                {
                    for (int i = 0; i < ccb.SubPoints.Length; i++)
                        ccb.SubPoints[i] /= divisor;
                    ccb.OverallPoints /= divisor;
                }

            // Return results
            return retComparisonCalcutions.ToArray();
        }
        /// <summary>
        /// Get the relative stat value for the given property (of a Stats object) of the given Character.
        /// See http://www.wowhead.com/?help=stat-weighting for more info of stat values.
        /// Note that these values are volatile and should be updated whenever any aspect of the 
        /// character is changed.
        /// </summary>
        /// <param name="character">The character for which to calculate the relative stat values.</param>
        /// <param name="property">The property of a Stats object for which to get the relative stat value.</param>
        /// <returns>The comparitive calculations of the relative stat value of the given Property (of a 
        /// Stats object) for the given Character.</returns>
        public static ComparisonCalculationBase GetRelativeStatValue(Character character, PropertyInfo property)
        {
            const float resolution = 0.005f; // the minimum resolution of change for the purpose of testing continuity and determining step locations
            ComparisonCalculationBase ccb = null;
            float minRange = CommonStat.GetCommonStatMinimumRange(property);
			if (minRange >= 0)
            {
                // Get change bounds
                Item item = new Item() { Stats = new Stats() };
                CharacterCalculationsBase charCalcsBase = Calculations.GetCharacterCalculations(character);
                float basePoints = charCalcsBase.OverallPoints;
                float upperChangePoint = 1.0f;
                float lowerChangePoint = 0.0f;
                if (!PropertyValueIsContinuous(character, basePoints, property, item, resolution))
                {
                    upperChangePoint = GetStatValueUpperChangePoint(character, basePoints, property, item, minRange + resolution, minRange + 10.0f, resolution);
                    lowerChangePoint = GetStatValueLowerChangePoint(character, basePoints, property, item, -minRange - 10.0f, -minRange, resolution);
                }
                float changePointDifference = upperChangePoint - lowerChangePoint;
                // Get new overall points with the [upperChangePoint] improvement
                property.SetValue(item.Stats, upperChangePoint, null);
                item.InvalidateCachedData();
				CharacterCalculationsBase charCalcsUpper = Calculations.Instance.GetCharacterCalculations(character, item);
				// Get new overall points with the [lowerChangePoint] improvement
				property.SetValue(item.Stats, lowerChangePoint, null);
				item.InvalidateCachedData();
				CharacterCalculationsBase charCalcsLower = Calculations.Instance.GetCharacterCalculations(character, item);
				// Create new CCB, populate, and return it.
                ccb = Calculations.CreateNewComparisonCalculation();
				ccb.Name = Extensions.DisplayName(property);
                //      transfer OverallPoints
                ccb.OverallPoints = (charCalcsUpper.OverallPoints - charCalcsLower.OverallPoints) / changePointDifference;
                //      transfer SubPoints
                ccb.SubPoints = new float[charCalcsUpper.SubPoints.Length];
                for (int i = 0; i < charCalcsUpper.SubPoints.Length; i++)
                {
                    ccb.SubPoints[i] = (charCalcsUpper.SubPoints[i] - charCalcsLower.SubPoints[i]) / changePointDifference;
                }
            }
            return ccb;
        }
        /// <summary>
        /// Determine whether the property in question is continuous or follows a "step" progression (at which point change points 
        /// must be determined).  The simplest way to do this is check whether adding or subtracting 0.01 from the stat produces a 
        /// different value in both the addition and subtraction case.  If it does, then the property is continuous (at least to 
        /// the resolution of the given resolution).
        /// </summary>
        /// <param name="character">The character whose property is being evaluated for continuity.</param>
        /// <param name="basePoints">The base number of points that the character has.</param>
        /// <param name="property">The property to evaluate for continuity.</param>
        /// <param name="tagItem">A "tag" item to reduce memory allocation.</param>
        /// <param name="resolution">The resolution at which continuity is being checked.</param>
        /// <returns>Whether the property was deemed continuous.</returns>
        private static bool PropertyValueIsContinuous(Character character, float basePoints, PropertyInfo property, Item tagItem, float resolution)
        {
            bool continuous;
            property.SetValue(tagItem.Stats, resolution, null);
            tagItem.InvalidateCachedData();
            continuous = basePoints != Calculations.Instance.GetCharacterCalculations(character, tagItem).OverallPoints;
            // if continuity was detected in the first alteration, then test the second direction (to guard against cases 
            // where we just happen to be on the threshold)
            if (continuous)
            {
                property.SetValue(tagItem.Stats, -1 * resolution, null);
                tagItem.InvalidateCachedData();
                // Since we've already determined that the first alteration was continuous, whether this one is 
                // determines whether both are continuous.
                continuous = basePoints != Calculations.Instance.GetCharacterCalculations(character, tagItem).OverallPoints;
            }
            return continuous;
        }
        /// <summary>
        /// Do a "binary search" to ascertain the value between the given bounds whose absolute value 
        /// is greatest but at which no change in the OverallPoints value of the given Character occurs.
        /// </summary>
        /// <param name="character">The character for which the change point is to be determined.</param>
        /// <param name="basePoints">The base OverallPoints value of the character, the value from 
        /// which we are ascertaining the change point as a consequence of changing the given 
        /// Property's value.</param>
        /// <param name="property">The property whose ability to change the given Character's 
        /// OverallPoints value we are evaluating.</param>
        /// <param name="tagItem">A tag item (used for the purposes of avoiding repeated memory 
        /// allocations) that will be used to reflect the changes in doing the OverallPoints 
        /// calcualation.</param>
        /// <param name="upperBound">The upper bound of the binary search space.  This must initially reflect a value at which OverallPoints always changes.</param>
        /// <param name="lowerBound">The lower bound of the binary search space.  This must initially reflect a value at which no change in OverallPoints occurs.</param>
        /// <returns>The value at which the OverallPoints of the given character is not changed but 
        /// at which the OverallPoints of the given Character *would* change if 
        /// Math.Sign(upperBound) * 0.01
        /// were added to it.</returns>
        private static float GetStatValueUpperChangePoint(Character character, float basePoints, PropertyInfo property, Item tagItem, float lowerBound, float upperBound, float resolution)
        {
            // Exit condition: If we've reached a change point at the smallest desired 
            // resolution, return the "no change" value.
            if ((upperBound - lowerBound) <= resolution)
            {
                return upperBound;
            }
            // Recusive condition: We still need to reduce the difference between the 
            // upper and lower bounds of the range we are searching.
            else
            {
                // Set the stat of the item to the mid point of the range to test which 
                // half contains the change point.
                float midPoint = (upperBound + lowerBound) / 2f;
                property.SetValue(tagItem.Stats, midPoint, null);
                tagItem.InvalidateCachedData();
                // If the midpoint leaves the OverallPoints unchanged, the change point 
                // is in the upper half of the given range.
                float newOverall = Calculations.Instance.GetCharacterCalculations(character, tagItem).OverallPoints;
                if (basePoints == newOverall)
                {
					return GetStatValueUpperChangePoint(character, basePoints, property, tagItem, midPoint, upperBound, resolution);
                }
                // Otherwise, the change point is in the lower half of the given range.
                else
                {
					return GetStatValueUpperChangePoint(character, basePoints, property, tagItem, lowerBound, midPoint, resolution);
                }
            }
        }

		private static float GetStatValueLowerChangePoint(Character character, float basePoints, PropertyInfo property, Item tagItem, float lowerBound, float upperBound, float resolution)
		{
			// Exit condition: If we've reached a change point at the smallest desired 
			// resolution, return the "no change" value.
			if ((upperBound - lowerBound) <= resolution)
			{
				return upperBound;
			}
			// Recusive condition: We still need to reduce the difference between the 
			// upper and lower bounds of the range we are searching.
			else
			{
				// Set the stat of the item to the mid point of the range to test which 
				// half contains the change point.
				float midPoint = (upperBound + lowerBound) / 2f;
				property.SetValue(tagItem.Stats, midPoint, null);
				tagItem.InvalidateCachedData();
				// If the midpoint leaves the OverallPoints unchanged, the change point 
				// is in the lower half of the given range.
				float newOverall = Calculations.Instance.GetCharacterCalculations(character, tagItem).OverallPoints;
				if (basePoints == newOverall)
				{
					return GetStatValueLowerChangePoint(character, basePoints, property, tagItem, lowerBound, midPoint, resolution);
				}
				// Otherwise, the change point is in the upper half of the given range.
				else
				{
					return GetStatValueLowerChangePoint(character, basePoints, property, tagItem, midPoint, upperBound, resolution);
				}
			}
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
		/// The ItemInstance being rated. This property is used to build the tooltip for this
		/// object in the chart.
		/// </summary>
		public abstract ItemInstance ItemInstance { get; set; }

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
        /// Name of the Stat to set to 1.00 for relative stats calcs
        /// </summary>
        public virtual String BaseStat { get; set; }
        
        /// <summary>
        /// User Option whether to use the Base Stat feature for relative stats calcs
        /// </summary>
        public virtual bool getBaseStatOption(Character character) { return false; }
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
