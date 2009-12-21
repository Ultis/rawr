using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Reflection;
using System.Threading;

namespace Rawr.UI
{
    public partial class GraphDisplay : UserControl
    {

        //public enum Graph : int
        //{   
        //    Gear = 0,
        //    Enchants = 1,
        //    Gems = 2,
        //    Buffs = 3,
        //    TalentSpecs = 5,
        //    Talents = 6,
        //    Glyphs = 7,
        //    RelativeStatValue = 8,
        //    Custom = 10
        //}

        //#region Getters/Setters
        //private Graph currentGraph;
        //public Graph CurrentGraph
        //{
        //    get { return currentGraph; }
        //    set
        //    {
        //        currentGraph = value;
        //        UpdateGraph();
        //    }
        //}

        //private CharacterSlot gear;
        //public CharacterSlot Gear
        //{
        //    get { return gear; }
        //    set
        //    {
        //        gear = value;
        //        ComparisonGraph.Slot = gear;
        //        UpdateGraph();
        //    }
        //}

        //private ItemSlot enchant;
        //public ItemSlot Enchant
        //{
        //    get { return enchant; }
        //    set
        //    {
        //        enchant = value;
        //        UpdateGraph();
        //    }
        //}

        //private CharacterSlot gem;
        //public CharacterSlot Gem
        //{
        //    get { return gem; }
        //    set
        //    {
        //        gem = value;
        //        UpdateGraph();
        //    }
        //}

        //private BuffGroup allBuffs;
        //public BuffGroup AllBuffs
        //{
        //    get { return allBuffs; }
        //    set
        //    {
        //        allBuffs = value;
        //        UpdateGraph();
        //    }
        //}

        //private int custom;
        //public int Custom
        //{
        //    get { return custom; }
        //    set
        //    {
        //        custom = value;
        //        UpdateGraph();
        //    }
        //}

        private string _currentGraph = "Gear|Head";
        public string CurrentGraph
        {
            get { return _currentGraph; }
            set
            {
                _currentGraph = value;
                UpdateGraph();
            }
        }

        private Character _character;
        public Character Character
        {
            get { return _character; }
            set
            {
                if (_character != null)
                {
                    _character.CalculationsInvalidated -= new EventHandler(character_CalculationsInvalidated);
                }
                _character = value;
                _character.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                ComparisonGraph.Character = _character;
				//CustomCombo.ItemsSource = new List<string>(Calculations.CustomChartNames);
				//CustomCombo.SelectedIndex = Calculations.CustomChartNames.Length > 0 ? 0 : -1;

				UpdateGraph();
            }
        }
        //#endregion

        public void character_CalculationsInvalidated(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private bool _loading;
        public GraphDisplay()
        {
            _loading = true;
            // Required to initialize variables
            InitializeComponent();
            ChartPicker1.GraphDisplay = this;
#if SILVERLIGHT
            GraphScroll.SetIsMouseWheelScrollingEnabled(true);
#endif
            //GraphCombo.SelectedIndex = (int)Graph.RelativeStatValue;
            //GearCombo.SelectedIndex = (int)CharacterSlot.Head;
            //EnchantCombo.SelectedIndex = (int)CharacterSlot.Head;
            //GemCombo.SelectedIndex = 0;
            //BuffCombo.SelectedIndex = 0;
            //CustomCombo.SelectedIndex = -1;
            _loading = false;
            UpdateGraph();
        }

        private string ConvertBuffSelector(string buffGroup) {
            switch (buffGroup) {
				case "Raid Buffs":        {
                    return "Agility and Strength|Armor|Damage Reduction (Major %)"
                         + "|Damage Reduction (Minor %)|Healing Received (%)|Attack Power"
                         + "|Attack Power (%)|Spell Power|Spell Sensitivity|Spirit"
                         + "|Damage (%)|Haste (%)|Health|Resistance"
                         + "|Physical Critical Strike Chance|Spell Critical Strike Chance"
                         + "|Focus Magic, Spell Critical Strike Chance|Spell Haste"
                         + "|Physical Haste|Stamina|Stat Add|Stat Multiplier"
                         + "|Melee Hit Chance Reduction|Racial Buffs|Armor (Major)"
                         + "|Armor (Minor)|Bleed Damage|Critical Strike Chance Taken"
                         + "|Spell Critical Strike Taken|Physical Vulnerability"
                         + "|Special Mobs|Intellect|Replenishment|Mana Regeneration"
                         + "|Ranged Attack Power|Mana Restore|Spell Damage Taken"
                         + "|Spell Hit Taken|Boss Attack Speed|Class Buffs|Disease Damage Taken"
                         + "|Burst Mana Regeneration";
                }
                default:                         { return buffGroup; }
            }
        }

        public void UpdateGraph()
        {
            if (_loading || Calculations.Instance == null || Character == null) return;
            string[] parts = CurrentGraph.Split('|');
            switch (parts[0])
            {
                case "Gear":                UpgradeGraphGear(parts[1]); break;
                case "Enchants":            UpgradeGraphEnchants(parts[1]); break;
                case "Gems":                UpgradeGraphGems(parts[1]); break;
                case "Buffs":               UpgradeGraphBuffs(parts[1]); break;
                case "Talents and Glyphs":  UpgradeGraphTalents(parts[1]); break;
                case "Equipped":            UpgradeGraphEquipped(parts[1]); break;
                case "Available":           UpgradeGraphAvailable(parts[1]); break;
                case "Direct Upgrades":     UpgradeGraphDirectUpgrades(parts[1]); break;
                case "Stat Values":         UpgradeGraphStatValues(parts[1]); break;
                default:                    UpgradeGraphModelSpecific(parts[1]); break;
            }
        }

		private int _calculationCount = 0;
		private ComparisonCalculationBase[] _itemCalculations = null;
		private AutoResetEvent _autoResetEvent = null;
		private CharacterSlot _characterSlot = CharacterSlot.AutoSelect;

        private void UpgradeGraphGear(string subgraph)
        {
			_characterSlot = (CharacterSlot)Enum.Parse(typeof(CharacterSlot), subgraph.Replace(" ", ""), true);
			bool seenEquippedItem = (Character[_characterSlot] == null);

            Calculations.ClearCache();
			List<ItemInstance> relevantItemInstances = Character.GetRelevantItemInstances(_characterSlot);
            _itemCalculations = new ComparisonCalculationBase[relevantItemInstances.Count];
			_calculationCount = 0;
			_autoResetEvent = new AutoResetEvent(false);
					
            if (relevantItemInstances.Count > 0)
            {
                foreach (ItemInstance item in relevantItemInstances)
                {
					if (!seenEquippedItem && Character[_characterSlot].Equals(item)) seenEquippedItem = true;
					ThreadPool.QueueUserWorkItem(GetItemInstanceCalculations, item);
				}
				_autoResetEvent.WaitOne();
            }

			List<ComparisonCalculationBase> listItemCalculations = new List<ComparisonCalculationBase>(_itemCalculations);
			if (!seenEquippedItem) listItemCalculations.Add(Calculations.GetItemCalculations(Character[_characterSlot], Character, _characterSlot));
			_itemCalculations = FilterTopXGemmings(listItemCalculations);

            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
			ComparisonGraph.DisplayCalcs(_itemCalculations);
        }

		private void GetItemInstanceCalculations(object item)
		{
			ComparisonCalculationBase result = Calculations.GetItemCalculations((ItemInstance)item, Character, _characterSlot);
			_itemCalculations[Interlocked.Increment(ref _calculationCount) - 1] = result;
			if (_calculationCount == _itemCalculations.Length) _autoResetEvent.Set();
		}
        

        private void UpgradeGraphEnchants(string subgraph)
        {
            ItemSlot slot = (ItemSlot)Enum.Parse(typeof(ItemSlot), subgraph.Replace(" 1","").Replace(" 2","").Replace(" ", ""), true);
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            ComparisonGraph.DisplayCalcs(Calculations.GetEnchantCalculations(slot, Character, Calculations.GetCharacterCalculations(Character), false).ToArray());
        }

        private void UpgradeGraphGems(string subgraph)
        {
            CharacterSlot cslot = CharacterSlot.Gems;
            ItemSlot islot = ItemSlot.None;
            switch (subgraph)
            {
                case "Red":     islot = ItemSlot.Red; break;
                case "Yellow":  islot = ItemSlot.Yellow; break;
                case "Blue":    islot = ItemSlot.Blue; break;
                case "Meta":    cslot = CharacterSlot.Metas; break;
            }
            Calculations.ClearCache();
            Character.ClearRelevantGems(); // we need to reset relevant items for gems to allow colour selection
			List<Item> relevantItems = Character.GetRelevantItems(cslot, islot);
			_itemCalculations = new ComparisonCalculationBase[relevantItems.Count];
			_calculationCount = 0;
			_autoResetEvent = new AutoResetEvent(false);

            if (relevantItems.Count > 0)
            {
				foreach (Item item in relevantItems)
				{
					ThreadPool.QueueUserWorkItem(GetItemCalculations, item);
				}
				_autoResetEvent.WaitOne();
            }

			_itemCalculations = FilterTopXGemmings(new List<ComparisonCalculationBase>(_itemCalculations));

			ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            ComparisonGraph.DisplayCalcs(_itemCalculations);
		}

		private void GetItemCalculations(object item)
		{
			ComparisonCalculationBase result = Calculations.GetItemCalculations((Item)item, Character, _characterSlot);
			_itemCalculations[Interlocked.Increment(ref _calculationCount) - 1] = result;
			if (_calculationCount == _itemCalculations.Length) _autoResetEvent.Set();
		}


        private void UpgradeGraphBuffs(string subgraph)
        {
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            ComparisonGraph.DisplayCalcs(Calculations.GetBuffCalculations(Character, 
                Calculations.GetCharacterCalculations(Character), ConvertBuffSelector(subgraph)).ToArray());
        }

        private void UpgradeGraphTalents(string subgraph)
        {
            if (subgraph == "Individual Talents")
            {
                List<ComparisonCalculationBase> talentCalculations = new List<ComparisonCalculationBase>();
                Character newChar = Character.Clone();
                CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(Character, null, false, true, false); ;
                CharacterCalculationsBase newCalc;
                ComparisonCalculationBase compare;

                foreach (PropertyInfo pi in Character.CurrentTalents.GetType().GetProperties())
                {
                    TalentDataAttribute[] talentDatas = pi.GetCustomAttributes(typeof(TalentDataAttribute), true) as TalentDataAttribute[];
                    int orig;
                    if (talentDatas.Length > 0)
                    {
                        TalentDataAttribute talentData = talentDatas[0];
                        orig = Character.CurrentTalents.Data[talentData.Index];
                        if (talentData.MaxPoints == (int)pi.GetValue(Character.CurrentTalents, null))
                        {
                            newChar.CurrentTalents.Data[talentData.Index]--;
                            newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                            compare = Calculations.GetCharacterComparisonCalculations(newCalc, currentCalc, talentData.Name, talentData.MaxPoints == orig);
                        }
                        else
                        {
                            newChar.CurrentTalents.Data[talentData.Index]++;
                            newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                            compare = Calculations.GetCharacterComparisonCalculations(currentCalc, newCalc, talentData.Name, talentData.MaxPoints == orig);
                        }
                        string text = string.Format("Current Rank {0}/{1}\r\n\r\n", orig, talentData.MaxPoints);
                        if (orig == 0)
                        {
                            // We originally didn't have it, so first rank is next rank
                            text += "Next Rank:\r\n";
                            text += talentData.Description[0];
                        }
                        else if (orig >= talentData.MaxPoints)
                        {
                            // We originally were at max, so there isn't a next rank, just show the capped one
                            text += talentData.Description[talentData.MaxPoints - 1];
                        }
                        else
                        {
                            // We aren't at 0 or MaxPoints originally, so it's just a point in between
                            text += talentData.Description[orig - 1];
                            text += "\r\n\r\nNext Rank:\r\n";
                            text += talentData.Description[orig];
                        }
                        compare.Description = text;
                        compare.Item = null;
                        talentCalculations.Add(compare);
                        newChar.CurrentTalents.Data[talentData.Index] = orig;
                    }
                }
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(talentCalculations.ToArray());
            }
            else if (subgraph == "Talent Specs")
            {
                List<ComparisonCalculationBase> talentCalculations = new List<ComparisonCalculationBase>();
                Character newChar = Character.Clone();

                TalentsBase nothing = Character.CurrentTalents.Clone();
                for (int i = 0; i < nothing.Data.Length; i++) nothing.Data[i] = 0;
                for (int i = 0; i < nothing.GlyphData.Length; i++) nothing.GlyphData[i] = false;
                newChar.CurrentTalents = nothing;

                CharacterCalculationsBase baseCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, true);
                CharacterCalculationsBase newCalc;
                ComparisonCalculationBase compare;

                bool same, found = false;
                foreach (SavedTalentSpec sts in SavedTalentSpec.SpecsFor(Character.Class))
                {
                    same = false;
                    if (sts.Equals(Character.CurrentTalents)) same = true;
                    newChar.CurrentTalents = sts.TalentSpec();
                    newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, true);
                    compare = Calculations.GetCharacterComparisonCalculations(baseCalc, newCalc, sts.Name, same);
                    compare.Item = null;
                    talentCalculations.Add(compare);
                    found = found || same;
                }
                if (!found)
                {
                    newCalc = Calculations.GetCharacterCalculations(Character, null, false, true, true);
                    compare = Calculations.GetCharacterComparisonCalculations(baseCalc, newCalc, "Custom", true);
                    talentCalculations.Add(compare);
                }
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(talentCalculations.ToArray());
            }
            else if (subgraph == "Glyphs")
            {
                List<ComparisonCalculationBase> glyphCalculations = new List<ComparisonCalculationBase>();
                Character newChar = Character.Clone();
                CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(Character, null, false, true, false);
                CharacterCalculationsBase newCalc;
                ComparisonCalculationBase compare;
                List<string> relevant = Calculations.GetModel(Character.CurrentModel).GetRelevantGlyphs();

                foreach (PropertyInfo pi in Character.CurrentTalents.GetType().GetProperties())
                {
                    GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                    bool orig;
                    if (glyphDatas.Length > 0)
                    {
                        GlyphDataAttribute glyphData = glyphDatas[0];
                        if (relevant == null || relevant.Contains(glyphData.Name))
                        {
                            orig = Character.CurrentTalents.GlyphData[glyphData.Index];
                            if (orig)
                            {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = false;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(newCalc, currentCalc, glyphData.Name, orig);
                            }
                            else
                            {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = true;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(currentCalc, newCalc, glyphData.Name, orig);
                            }
                            // JOTHAY: WTB Tooltips that show info on these charts
                            compare.Description = glyphData.Description;
                            compare.Item = null;
                            glyphCalculations.Add(compare);
                            newChar.CurrentTalents.GlyphData[glyphData.Index] = orig;
                        }
                    }
                }
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(glyphCalculations.ToArray());
            }
        }

        private void UpgradeGraphEquipped(string subgraph)
        {
            if (subgraph == "Gear")
            {
				List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
				CharacterSlot[] slots = new CharacterSlot[]
				{
					 CharacterSlot.Back, CharacterSlot.Chest, CharacterSlot.Feet, CharacterSlot.Finger1,
					 CharacterSlot.Finger2, CharacterSlot.Hands, CharacterSlot.Head, CharacterSlot.Legs,
					 CharacterSlot.MainHand, CharacterSlot.Neck, CharacterSlot.OffHand, CharacterSlot.Projectile,
					 CharacterSlot.ProjectileBag, CharacterSlot.Ranged, CharacterSlot.Shoulders,
					 CharacterSlot.Trinket1, CharacterSlot.Trinket2, CharacterSlot.Waist, CharacterSlot.Wrist
				};
				foreach (CharacterSlot slot in slots)
				{
					ItemInstance item = Character[slot];
					if (item != null)
					{
						itemCalculations.Add(Calculations.GetItemCalculations(item, Character, slot));
					}
				}

				ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
				ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
				ComparisonGraph.DisplayCalcs(itemCalculations.ToArray());
            }
            else if (subgraph == "Enchants")
            {
				List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
				ItemSlot[] slots = new ItemSlot[]
				{
					 ItemSlot.Back, ItemSlot.Chest, ItemSlot.Feet, ItemSlot.Finger,
					 ItemSlot.Hands, ItemSlot.Head, ItemSlot.Legs,
					 ItemSlot.MainHand, ItemSlot.OffHand, ItemSlot.Ranged, ItemSlot.Shoulders,
					 ItemSlot.Waist, ItemSlot.Wrist
				};
				
				foreach (ItemSlot slot in slots)
					foreach (ComparisonCalculationBase calc in Calculations.GetEnchantCalculations(slot, Character, Calculations.GetCharacterCalculations(Character), true))
						itemCalculations.Add(calc);
				
				ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
				ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
				ComparisonGraph.DisplayCalcs(itemCalculations.ToArray());
			}
            else if (subgraph == "Buffs")
            {
                UpgradeGraphBuffs("Current");
            }
        }

        private void UpgradeGraphAvailable(string subgraph)
        {
            if (subgraph == "Gear")
            {
                ComparisonCalculationBase calc = Calculations.CreateNewComparisonCalculation();
                calc.Name = "Chart Not Yet Implemented";
                ComparisonGraph.DisplayCalcs(new ComparisonCalculationBase[] { calc });
            }
            else if (subgraph == "Enchants")
            {
                ComparisonCalculationBase calc = Calculations.CreateNewComparisonCalculation();
                calc.Name = "Chart Not Yet Implemented";
                ComparisonGraph.DisplayCalcs(new ComparisonCalculationBase[] { calc });
            }
        }

        private void UpgradeGraphDirectUpgrades(string subgraph)
        {
            if (subgraph == "Gear")
            {
                List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
                Dictionary<ItemSlot, CharacterSlot> slotMap = new Dictionary<ItemSlot, CharacterSlot>();
                if (Character != null)
                {
                    Dictionary<string, ItemInstance> items = new Dictionary<string, ItemInstance>();

                    float Finger1 = (Character[CharacterSlot.Finger1] == null ? 0 : Calculations.GetItemCalculations(
                        Character[CharacterSlot.Finger1], Character, CharacterSlot.Finger1).OverallPoints);
                    float Finger2 = (Character[CharacterSlot.Finger2] == null ? 0 : Calculations.GetItemCalculations(
                        Character[CharacterSlot.Finger2], Character, CharacterSlot.Finger2).OverallPoints);

                    float Trinket1 = (Character[CharacterSlot.Trinket1] == null ? 0 : Calculations.GetItemCalculations(
                        Character[CharacterSlot.Trinket1], Character, CharacterSlot.Trinket1).OverallPoints);
                    float Trinket2 = (Character[CharacterSlot.Trinket2] == null ? 0 : Calculations.GetItemCalculations(
                        Character[CharacterSlot.Trinket2], Character, CharacterSlot.Trinket2).OverallPoints);

                    if (Finger2 < Finger1)
                    {
                        slotMap[ItemSlot.Finger] = CharacterSlot.Finger2;
                    }

                    if (Trinket2 < Trinket1)
                    {
                        slotMap[ItemSlot.Trinket] = CharacterSlot.Trinket2;
                    }

                    float MainHand = (Character[CharacterSlot.MainHand] == null ? 0 : Calculations.GetItemCalculations(
                        Character[CharacterSlot.MainHand], Character, CharacterSlot.MainHand).OverallPoints);
                    float OffHand = (Character[CharacterSlot.OffHand] == null ? 0 : Calculations.GetItemCalculations(
                        Character[CharacterSlot.OffHand], Character, CharacterSlot.OffHand).OverallPoints);

                    if (MainHand > OffHand)
                    {
                        slotMap[ItemSlot.OneHand] = CharacterSlot.OffHand;

                    }

                    foreach (KeyValuePair<ItemSlot, CharacterSlot> kvp in Item.DefaultSlotMap)
                    {
                        try
                        {
                            ItemSlot iSlot = kvp.Key;
                            CharacterSlot slot;

                            if (slotMap.ContainsKey(iSlot))
                            {
                                slot = slotMap[iSlot];
                            }
                            else
                            {
                                slot = kvp.Value;
                            }
                            if (slot != CharacterSlot.None)
                            {
                                ComparisonCalculationBase slotCalc;
                                ItemInstance currentItem = Character[slot];
                                if (currentItem == null)
                                    slotCalc = Calculations.CreateNewComparisonCalculation();
                                else
                                    slotCalc = Calculations.GetItemCalculations(currentItem, Character, slot);

                                foreach (ItemInstance item in Character.GetRelevantItemInstances(slot))
                                {
                                    if (!items.ContainsKey(item.GemmedId) && (currentItem == null || currentItem.GemmedId != item.GemmedId))
                                    {
                                        if (currentItem != null && currentItem.Item.Unique)
                                        {
                                            CharacterSlot otherSlot = CharacterSlot.None;
                                            switch (slot)
                                            {
                                                case CharacterSlot.Finger1:
                                                    otherSlot = CharacterSlot.Finger2;
                                                    break;
                                                case CharacterSlot.Finger2:
                                                    otherSlot = CharacterSlot.Finger1;
                                                    break;
                                                case CharacterSlot.Trinket1:
                                                    otherSlot = CharacterSlot.Trinket2;
                                                    break;
                                                case CharacterSlot.Trinket2:
                                                    otherSlot = CharacterSlot.Trinket1;
                                                    break;
                                                case CharacterSlot.MainHand:
                                                    otherSlot = CharacterSlot.OffHand;
                                                    break;
                                                case CharacterSlot.OffHand:
                                                    otherSlot = CharacterSlot.MainHand;
                                                    break;
                                            }
                                            if (otherSlot != CharacterSlot.None && Character[otherSlot] != null && Character[otherSlot].Id == item.Id)
                                            {
                                                continue;
                                            }

                                        }

                                        ComparisonCalculationBase itemCalc = Calculations.GetItemCalculations(item, Character, slot);
                                        //bool include = false;
                                        //for (int i = 0; i < itemCalc.SubPoints.Length; i++)
                                        //{
                                        //    itemCalc.SubPoints[i] -= slotCalc.SubPoints[i];
                                        //    include |= itemCalc.SubPoints[i] > 0;
                                        //}
                                        //itemCalc.OverallPoints -= slotCalc.OverallPoints;
                                        //if ( itemCalc.OverallPoints > 0)
                                        //{
                                        //    itemCalculations.Add(itemCalc);
                                        //}

                                        float difference = itemCalc.OverallPoints - slotCalc.OverallPoints;
                                        if (difference > 0)
                                        {
                                            itemCalc.SubPoints = new float[itemCalc.SubPoints.Length];
                                            itemCalc.OverallPoints = difference;
                                            itemCalculations.Add(itemCalc);
                                        }

                                        items[item.GemmedId] = item;
                                    }
                                }
                            }

                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                //ComparisonGraph.RoundValues = true;
                //ComparisonGraph.CustomRendered = false;
                //ComparisonGraph.DisplayMode = ComparisonGraph.GraphDisplayMode.Overall;
                //ComparisonGraph.ItemCalculations = FilterTopXGemmings(itemCalculations);
                //ComparisonGraph.EquipSlot = CharacterSlot.AutoSelect;
                //ComparisonGraph.SlotMap = slotMap;
                //_characterSlot = CharacterSlot.None;

                //Dictionary<string, Color> overall = new Dictionary<string,Color>();
                //overall.Add("Overall", Colors.Purple);
                //ComparisonGraph.LegendItems = overall;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Overall;
                ComparisonGraph.DisplayCalcs(FilterTopXGemmings(itemCalculations));
            }
            else if (subgraph == "Enchants")
            {
                ComparisonCalculationBase calc = Calculations.CreateNewComparisonCalculation();
                calc.Name = "Chart Not Yet Implemented";
                ComparisonGraph.DisplayCalcs(new ComparisonCalculationBase[] { calc });
            }
        }

        private void UpgradeGraphStatValues(string subgraph)
        {
            if (subgraph == "Relative Stat Values")
            {
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(CalculationsBase.GetRelativeStatValues(Character));
            }
        }

        private void UpgradeGraphModelSpecific(string subgraph)
        {
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            ComparisonGraph.DisplayCalcs(Calculations.GetCustomChartData(Character, subgraph));
        }

        // takes a list of items and returns a sorted filtered array of the top X gemmings
        private ComparisonCalculationBase[] FilterTopXGemmings(List<ComparisonCalculationBase> listItemCalculations)
        {
            List<ComparisonCalculationBase> filteredItemCalculations = new List<ComparisonCalculationBase>();
            int maxGemmings = Properties.GeneralSettings.Default.CountGemmingsShown;

            IOrderedEnumerable<ComparisonCalculationBase> orderedCalcs;
            if (ComparisonGraph.Sort == ComparisonSort.Alphabetical)
                orderedCalcs = listItemCalculations.OrderBy(calc => calc == null ? "" : calc.Name);
            else if (ComparisonGraph.Sort == ComparisonSort.Overall)
                orderedCalcs = listItemCalculations.OrderByDescending(calc => calc == null ? 0 : calc.OverallPoints);
            else
                orderedCalcs = listItemCalculations.OrderByDescending(calc => calc == null ? 0 : calc.SubPoints[(int)ComparisonGraph.Sort]);

            Dictionary<int, int> countItem = new Dictionary<int, int>();
            foreach (ComparisonCalculationBase itemCalculation in listItemCalculations)
            {
                int itemId = itemCalculation.ItemInstance.Id;
                if (!countItem.ContainsKey(itemId)) countItem.Add(itemId, 0);
                if (countItem[itemId]++ < maxGemmings ||
                    itemCalculation.Equipped || itemCalculation.ItemInstance.ForceDisplay)
                {
                    //Debug.Print("Itemid: " + itemId + " Instances of that item : " + countItem[itemId]);
                    filteredItemCalculations.Add(itemCalculation);
                }
            }
            return filteredItemCalculations.ToArray();
        }

        private void SetCustomSubpoints(IEnumerable<string> subpoints)
        {

        }

        private void SortChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortCombo != null)
                ComparisonGraph.Sort = (ComparisonSort)(SortCombo.SelectedIndex - 2);
        }
    }
}