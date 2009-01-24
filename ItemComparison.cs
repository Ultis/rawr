using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace Rawr
{
    public partial class ItemComparison : UserControl
    {
        public Item[] Items;

        public ComparisonGraph.ComparisonSort Sort
        {
            get
            {
                return comparisonGraph1.Sort;
            }
            set
            {
                comparisonGraph1.Sort = value;
            }
        }

        private Character _character;
        public Character Character
        {
            get
            {
                return _character;
            }
            set
            {
                _character = value;
                comparisonGraph1.Character = _character;
            }
        }

        public ItemComparison()
        {
            InitializeComponent();
        }

        private DynamicGemmer gemmer = new DynamicGemmer();

        public void LoadGearBySlot(Character.CharacterSlot slot)
        {
			Calculations.ClearCache();
            List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
            if (Items != null && Character != null)
            {
                itemCalculations = gemmer.LoadItemsBySlot(slot, Items, Character, false, null);
            }

            comparisonGraph1.RoundValues = true;
            comparisonGraph1.CustomRendered = false;
            comparisonGraph1.ItemCalculations = itemCalculations.ToArray();
            comparisonGraph1.EquipSlot = slot == Character.CharacterSlot.Gems || slot == Character.CharacterSlot.Metas ?
                Character.CharacterSlot.None : slot;
        }

        public void LoadEnchantsBySlot(Item.ItemSlot slot, CharacterCalculationsBase currentCalculations)
        {
            if (Items != null && Character != null)
            {
                comparisonGraph1.RoundValues = true;
                comparisonGraph1.CustomRendered = false;
                comparisonGraph1.ItemCalculations = Calculations.GetEnchantCalculations(slot, Character, currentCalculations).ToArray();
                comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
            }
        }

        public void LoadBuffs(CharacterCalculationsBase currentCalculations, bool activeOnly)
        {
            if (Items != null && Character != null)
            {
                comparisonGraph1.RoundValues = true;
                comparisonGraph1.CustomRendered = false;
                comparisonGraph1.ItemCalculations = Calculations.GetBuffCalculations(Character, currentCalculations, activeOnly).ToArray();
                comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
            }
        }

        public void LoadAvailableGear(CharacterCalculationsBase currentCalculations)
        {
            List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
            SortedList<Item.ItemSlot, Character.CharacterSlot> slotMap = new SortedList<Item.ItemSlot, Character.CharacterSlot>();
            if (Items != null && Character != null)
            {
                SortedList<string, Item> items = new SortedList<string, Item>();

                float Finger1 = (Character[Character.CharacterSlot.Finger1] == null ? 0 : Calculations.GetItemCalculations(
                    Character[Character.CharacterSlot.Finger1], Character, Character.CharacterSlot.Finger1).OverallPoints);
                float Finger2 = (Character[Character.CharacterSlot.Finger2] == null ? 0 : Calculations.GetItemCalculations(
                    Character[Character.CharacterSlot.Finger2], Character, Character.CharacterSlot.Finger2).OverallPoints);

                float Trinket1 = (Character[Character.CharacterSlot.Trinket1] == null ? 0 : Calculations.GetItemCalculations(
                    Character[Character.CharacterSlot.Trinket1], Character, Character.CharacterSlot.Trinket1).OverallPoints);
                float Trinket2 = (Character[Character.CharacterSlot.Trinket2] == null ? 0 : Calculations.GetItemCalculations(
                    Character[Character.CharacterSlot.Trinket2], Character, Character.CharacterSlot.Trinket2).OverallPoints);

                if (Finger2 < Finger1)
                {
                    slotMap[Item.ItemSlot.Finger] = Character.CharacterSlot.Finger2;
                }

                if (Trinket2 < Trinket1)
                {
                    slotMap[Item.ItemSlot.Trinket] = Character.CharacterSlot.Trinket2;
                }

                float MainHand = (Character[Character.CharacterSlot.MainHand] == null ? 0 : Calculations.GetItemCalculations(
                    Character[Character.CharacterSlot.MainHand], Character, Character.CharacterSlot.MainHand).OverallPoints);
                float OffHand = (Character[Character.CharacterSlot.OffHand] == null ? 0 : Calculations.GetItemCalculations(
                    Character[Character.CharacterSlot.OffHand], Character, Character.CharacterSlot.OffHand).OverallPoints);

                if (MainHand > OffHand)
                {
                    slotMap[Item.ItemSlot.OneHand] = Character.CharacterSlot.OffHand;

                }



                foreach (Item relevantItem in ItemCache.RelevantItems)
                {
                    try
                    {
                        Item.ItemSlot iSlot = relevantItem.Slot;
                        Character.CharacterSlot slot;

                        if (slotMap.ContainsKey(iSlot))
                        {
                            slot = slotMap[iSlot];
                        }
                        else
                        {
                            slot = Item.DefaultSlotMap[iSlot];
                        }
                        if (slot != Character.CharacterSlot.None)
                        {
                            ComparisonCalculationBase slotCalc;
							Item currentItem = Character[slot];
							if (currentItem == null)
								slotCalc = Calculations.CreateNewComparisonCalculation();
							else
								slotCalc = Calculations.GetItemCalculations(currentItem, Character, slot);

                            foreach (Item item in ItemCache.Instance.FindAllItemsById(relevantItem.Id))
                            {
								if (!items.ContainsKey(item.GemmedId) && (currentItem == null || currentItem.GemmedId != item.GemmedId))
                                {
									if (currentItem != null && currentItem.Unique)
                                    {
                                        Character.CharacterSlot otherSlot = Character.CharacterSlot.None;
                                        switch (slot)
                                        {
                                            case Character.CharacterSlot.Finger1:
                                                otherSlot = Character.CharacterSlot.Finger2;
                                                break;
                                            case Character.CharacterSlot.Finger2:
                                                otherSlot = Character.CharacterSlot.Finger1;
                                                break;
                                            case Character.CharacterSlot.Trinket1:
                                                otherSlot = Character.CharacterSlot.Trinket2;
                                                break;
                                            case Character.CharacterSlot.Trinket2:
                                                otherSlot = Character.CharacterSlot.Trinket1;
                                                break;
                                            case Character.CharacterSlot.MainHand:
                                                otherSlot = Character.CharacterSlot.OffHand;
                                                break;
                                            case Character.CharacterSlot.OffHand:
                                                otherSlot = Character.CharacterSlot.MainHand;
                                                break;
                                        }
										if (otherSlot != Character.CharacterSlot.None && Character[otherSlot] != null && Character[otherSlot].Id == item.Id)
                                        {
                                            continue;
                                        }

                                    }

                                    ComparisonCalculationBase itemCalc = Calculations.GetItemCalculations(item, Character, slot);
                                    bool include = false;
                                    for (int i = 0; i < itemCalc.SubPoints.Length; i++)
                                    {
                                        itemCalc.SubPoints[i] -= slotCalc.SubPoints[i];
                                        include |= itemCalc.SubPoints[i] > 0;
                                    }
                                    itemCalc.OverallPoints -= slotCalc.OverallPoints;
                                    if ( itemCalc.OverallPoints > 0)
                                    {
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

            comparisonGraph1.RoundValues = true;
            comparisonGraph1.CustomRendered = false;
            comparisonGraph1.DisplayMode = ComparisonGraph.GraphDisplayMode.Overall;
            comparisonGraph1.ItemCalculations = itemCalculations.ToArray();
            comparisonGraph1.EquipSlot = Character.CharacterSlot.AutoSelect;
            comparisonGraph1.SlotMap = slotMap;
        }

        public ComparisonGraph.GraphDisplayMode DisplayMode
        {
            get{
                return comparisonGraph1.DisplayMode;
            }
            set
            {
                comparisonGraph1.DisplayMode = value;
            }
        }

        public void LoadCurrentGearEnchantsBuffs(CharacterCalculationsBase currentCalculations)
        {
            List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
            if (Items != null && Character != null)
            {
                foreach (Character.CharacterSlot slot in Enum.GetValues(typeof(Character.CharacterSlot)))
                    if (Character[slot] != null)
                        itemCalculations.Add(Calculations.GetItemCalculations(Character[slot], Character, slot));

                foreach (ComparisonCalculationBase calc in Calculations.GetEnchantCalculations(Item.ItemSlot.None, Character, currentCalculations))
                    if (calc.Equipped)
                        itemCalculations.Add(calc);

                foreach (ComparisonCalculationBase calc in Calculations.GetBuffCalculations(Character, currentCalculations, true))
                    itemCalculations.Add(calc);
            }

            comparisonGraph1.RoundValues = true;
            comparisonGraph1.CustomRendered = false;
            comparisonGraph1.ItemCalculations = itemCalculations.ToArray();
            comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
        }

        public void LoadTalents()
        {
            List<ComparisonCalculationBase> talentCalculations = new List<ComparisonCalculationBase>();
            Character baseChar = Character.Clone();
            Character newChar = Character.Clone();
            CharacterCalculationsBase currentCalc;
            int orig;
            foreach (PropertyInfo pi in baseChar.CurrentTalents.GetType().GetProperties())
            {
                TalentDataAttribute[] talentDatas = pi.GetCustomAttributes(typeof(TalentDataAttribute), true) as TalentDataAttribute[];
                if (talentDatas.Length > 0)
                {
                    TalentDataAttribute talentData = talentDatas[0];
                    orig = baseChar.CurrentTalents.Data[talentData.Index];
                    if (talentData.MaxPoints == (int)pi.GetValue(baseChar.CurrentTalents, null)) baseChar.CurrentTalents.Data[talentData.Index]--;
                    else newChar.CurrentTalents.Data[talentData.Index]++;
                    currentCalc = Calculations.GetCharacterCalculations(baseChar);
                    talentCalculations.Add(Calculations.GetCharacterComparisonCalculations(currentCalc, newChar, talentData.Name, talentData.MaxPoints == orig));
                    baseChar.CurrentTalents.Data[talentData.Index] = orig;
                    newChar.CurrentTalents.Data[talentData.Index] = orig;
                }
            }

            comparisonGraph1.RoundValues = true;
            comparisonGraph1.CustomRendered = false;
            comparisonGraph1.ItemCalculations = talentCalculations.ToArray();
            comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
        }

        public void LoadTalentSpecs(TalentPicker picker)
        {
            List<ComparisonCalculationBase> talentCalculations = new List<ComparisonCalculationBase>();
            if (Items != null && Character != null)
            {
                Character baseChar = Character.Clone();
                switch (baseChar.Class)
                {
                    case Character.CharacterClass.Warrior: baseChar.WarriorTalents = new WarriorTalents(); break;
                    case Character.CharacterClass.Paladin: baseChar.PaladinTalents = new PaladinTalents(); break;
                    case Character.CharacterClass.Hunter: baseChar.HunterTalents = new HunterTalents(); break;
                    case Character.CharacterClass.Rogue: baseChar.RogueTalents = new RogueTalents(); break;
                    case Character.CharacterClass.Priest: baseChar.PriestTalents = new PriestTalents(); break;
                    case Character.CharacterClass.Shaman: baseChar.ShamanTalents = new ShamanTalents(); break;
                    case Character.CharacterClass.Mage: baseChar.MageTalents = new MageTalents(); break;
                    case Character.CharacterClass.Warlock: baseChar.WarlockTalents = new WarlockTalents(); break;
                    case Character.CharacterClass.Druid: baseChar.DruidTalents = new DruidTalents(); break;
                    case Character.CharacterClass.DeathKnight: baseChar.DeathKnightTalents = new DeathKnightTalents(); break;
                    default: baseChar.DruidTalents = new DruidTalents(); break;
                }
                CharacterCalculationsBase currentCalculations = Calculations.GetCharacterCalculations(baseChar);
                Character newChar;
                foreach (SavedTalentSpec spec in picker.SpecsFor(Character.Class))
                {
                    newChar = Character.Clone();
                    newChar.CurrentTalents = spec.TalentSpec();
                    talentCalculations.Add(Calculations.GetCharacterComparisonCalculations(currentCalculations, newChar, spec.Name, spec == picker.CurrentSpec()));
                    //talentCalculations.Add(Calculations.GetCharacterComparisonCalculations(currentCalculations, newChar, spec.Name, ));
                }
            }

            comparisonGraph1.RoundValues = true;
            comparisonGraph1.CustomRendered = false;
            comparisonGraph1.ItemCalculations = talentCalculations.ToArray();
            comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
        }

        public void LoadRelativeStatValues()
        {
            comparisonGraph1.RoundValues = true;
            comparisonGraph1.CustomRendered = false;
            comparisonGraph1.ItemCalculations = CalculationsBase.GetRelativeStatValues(Character);
            comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
        }

        public void LoadCustomChart(string chartName)
        {
            comparisonGraph1.RoundValues = true;
            comparisonGraph1.CustomRendered = false;
            comparisonGraph1.ItemCalculations = Calculations.GetCustomChartData(Character, chartName);
            comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
        }

        internal void LoadCustomRenderedChart(string chartName)
        {
            comparisonGraph1.RoundValues = true;
            comparisonGraph1.CustomRendered = true;
            comparisonGraph1.CustomRenderedChartName = chartName;
            comparisonGraph1.ItemCalculations = null;
            comparisonGraph1.EquipSlot = Character.CharacterSlot.None;
        }
    }
}
