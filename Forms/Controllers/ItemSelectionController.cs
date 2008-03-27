using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Forms.Controllers
{
    public class ItemSelectionController
    {
        private static ItemSelectionController _Controller;
        private Character _character;
        private Item[] _items;
        private ComparisonCalculationBase[] _itemCalculations;
        private ComparisonGraph.ComparisonSort _sort;
        private Character.CharacterSlot _characterSlot;

        private ItemSelectionController()
        {
            _characterSlot = Character.CharacterSlot.None;
             _sort = ComparisonGraph.ComparisonSort.Overall;
            _itemCalculations = new ComparisonCalculationBase[0];
        }
        
        public static ItemSelectionController Instance
        {
            get 
            {
                if (_Controller == null)
                {
                    _Controller = new ItemSelectionController();
                }
                return _Controller;
            }
        }

        public ComparisonCalculationBase[] ItemCalculations
        {
            get
            {
                return _itemCalculations;
            }
        }

        private ComparisonCalculationBase[] SortList(ComparisonCalculationBase[] calculationBase)
        {
            List<ComparisonCalculationBase> calcs = new List<ComparisonCalculationBase>(calculationBase);
            calcs.Sort(new System.Comparison<ComparisonCalculationBase>(CompareItemCalculations));
            return calcs.ToArray();
        }

        public ComparisonGraph.ComparisonSort Sort
        {
            get { return _sort; }
            set
            {
                _sort = value;
                _itemCalculations = SortList(_itemCalculations);
            }
        }

        public Item[] Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public Character.CharacterSlot CharacterSlot
        {
            get { return _characterSlot; }
            set { _characterSlot = value; }
        }

        public Character Character
        {
            get { return _character; }
            set { _character = value; }
        }

        protected int CompareItemCalculations(ComparisonCalculationBase a, ComparisonCalculationBase b)
        {
            if (Sort == ComparisonGraph.ComparisonSort.Overall)
                return a.OverallPoints.CompareTo(b.OverallPoints);
            else if (Sort == ComparisonGraph.ComparisonSort.Alphabetical)
                return b.Name.CompareTo(a.Name);
            else
                return a.SubPoints[(int)Sort].CompareTo(b.SubPoints[(int)Sort]);
        }

        public void LoadGearBySlot(Character.CharacterSlot slot)
        {
            if (slot != _characterSlot)
            {
                _characterSlot = slot;
                List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
                if (_Controller.Items != null && _Controller.Character != null)
                {
                    foreach (Item item in _Controller.Items)
                    {
                        if (item.FitsInSlot(slot))
                        {
                            itemCalculations.Add(Calculations.GetItemCalculations(item, _Controller.Character, slot));
                        }
                    }
                }
                itemCalculations.Sort(new System.Comparison<ComparisonCalculationBase>(CompareItemCalculations));
                _itemCalculations = itemCalculations.ToArray();
            }
        }
    }
}
