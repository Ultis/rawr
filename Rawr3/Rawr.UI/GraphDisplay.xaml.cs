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
using System.Diagnostics;
using System.Text;
using System.IO;
#if !SILVERLIGHT
using Microsoft.Win32;
#endif

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
                    _character.ClassChanged -= new EventHandler(character_ModelChanged);
                }
                _character = value;
                _character.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                _character.ClassChanged += new EventHandler(character_ModelChanged);
                ComparisonGraph.Character = _character;
                //CustomCombo.ItemsSource = new List<string>(Calculations.CustomChartNames);
                //CustomCombo.SelectedIndex = Calculations.CustomChartNames.Length > 0 ? 0 : -1;
                SetCustomSubpoints(Character.CurrentCalculations.SubPointNameColors.Keys.ToArray());
                UpdateBoxes();
                UpdateGraph();
            }
        }
        //#endregion

        public void character_CalculationsInvalidated(object sender, EventArgs e)
        {
            UpdateBoxes();
            UpdateGraph();
        }
        public void character_ModelChanged(object sender, EventArgs e)
        {
            SetCustomSubpoints(Character.CurrentCalculations.SubPointNameColors.Keys.ToArray());
            UpdateBoxes();
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

            #region Filter Side-bar
#if !DEBUG // Remove when the Filters on the Side work
            // This keeps the right edge from looking indented for no reason
            FilterAccordion.Margin = new Thickness(0);        
#endif

#if SILVERLIGHT
            SV_Bind.SetIsMouseWheelScrollingEnabled(true);
            SV_Prof.SetIsMouseWheelScrollingEnabled(true);
            SV_iLevel.SetIsMouseWheelScrollingEnabled(true);
            SV_Type.SetIsMouseWheelScrollingEnabled(true);
            SV_Gems.SetIsMouseWheelScrollingEnabled(true);
#endif

            #region From Refine Types of Items Listed
            checkBoxes = new List<CheckBox>();

            checkBoxes.Add(CheckBoxPlate);
            checkBoxes.Add(CheckBoxMail);
            checkBoxes.Add(CheckBoxLeather);
            checkBoxes.Add(CheckBoxCloth);

            checkBoxes.Add(CheckBoxDagger);
            checkBoxes.Add(CheckBoxFistWeapon);
            checkBoxes.Add(CheckBoxOneHandedAxe);
            checkBoxes.Add(CheckBoxOneHandedMace);
            checkBoxes.Add(CheckBoxOneHandedSword);

            checkBoxes.Add(CheckBoxStaff);
            checkBoxes.Add(CheckBoxPolearm);
            checkBoxes.Add(CheckBoxTwoHandedAxe);
            checkBoxes.Add(CheckBoxTwoHandedMace);
            checkBoxes.Add(CheckBoxTwoHandedSword);

            checkBoxes.Add(CheckBoxBow);
            checkBoxes.Add(CheckBoxCrossBow);
            checkBoxes.Add(CheckBoxGun);
            checkBoxes.Add(CheckBoxThrown);
            checkBoxes.Add(CheckBoxRelic);
            checkBoxes.Add(CheckBoxWand);

            checkBoxes.Add(CheckBoxShield);
            checkBoxes.Add(CheckBoxMisc);

            UpdateBoxes();
            #endregion

            #region From Show by iLevel
            RB_iLvl_Checks.SetBinding(RadioButton.IsCheckedProperty, new System.Windows.Data.Binding("iLvl_UseChecks") { Mode = BindingMode.TwoWay });
            if (!RB_iLvl_Checks.IsChecked.GetValueOrDefault(false)) { RB_iLvl_Sliders.IsChecked = true; }
            CK_iLvl_0.SetBinding(CheckBox.IsCheckedProperty, new System.Windows.Data.Binding("ilvlF_0") { Mode = BindingMode.TwoWay });
            CK_iLvl_1.SetBinding(CheckBox.IsCheckedProperty, new System.Windows.Data.Binding("ilvlF_1") { Mode = BindingMode.TwoWay });
            CK_iLvl_2.SetBinding(CheckBox.IsCheckedProperty, new System.Windows.Data.Binding("ilvlF_2") { Mode = BindingMode.TwoWay });
            CK_iLvl_3.SetBinding(CheckBox.IsCheckedProperty, new System.Windows.Data.Binding("ilvlF_3") { Mode = BindingMode.TwoWay });
            CK_iLvl_4.SetBinding(CheckBox.IsCheckedProperty, new System.Windows.Data.Binding("ilvlF_4") { Mode = BindingMode.TwoWay });
            CK_iLvl_5.SetBinding(CheckBox.IsCheckedProperty, new System.Windows.Data.Binding("ilvlF_5") { Mode = BindingMode.TwoWay });
            CK_iLvl_6.SetBinding(CheckBox.IsCheckedProperty, new System.Windows.Data.Binding("ilvlF_6") { Mode = BindingMode.TwoWay });
            CK_iLvl_7.SetBinding(CheckBox.IsCheckedProperty, new System.Windows.Data.Binding("ilvlF_7") { Mode = BindingMode.TwoWay });
            RS_iLvl.SetBinding(RangeSlider.LowerValueProperty, new System.Windows.Data.Binding("ilvlF_SLMin") { Mode = BindingMode.TwoWay });
            RS_iLvl.SetBinding(RangeSlider.UpperValueProperty, new System.Windows.Data.Binding("ilvlF_SLMax") { Mode = BindingMode.TwoWay });
            #endregion

            FilterTree.ItemsSource = ItemFilter.FilterList.FilterList;
            // Do an initial hide
            ButtonExpand_Click(null, null);
            FilterPopup.Visibility = Visibility.Collapsed; // The actual button will be removed later, just hiding it for now
            AI_Source.IsSelected = true;
            AI_Type.IsSelected = AI_Bind.IsSelected = AI_Prof.IsSelected = AI_iLevel.IsSelected = AI_Gems.IsSelected = false;
            FilterAccordion.SelectionMode = AccordionSelectionMode.One;
            #endregion

            _loading = false;
            UpdateGraph();
        }

        private string ConvertBuffSelector(string buffGroup) {
            switch (buffGroup) {
                case "Raid Buffs":        {
                    return 
                        // Stats
                          "Agility and Strength|Intellect|Spirit|Stamina|Stat Add|Stat Multiplier"
                        + "|Health|Attack Power|Attack Power (%)|Ranged Attack Power|Spell Power|Haste (%)|Spell Haste|Physical Haste"
                        // Offensive
                        + "|Damage (%)|Bleed Damage"
                        + "|Physical Critical Strike Chance|Spell Critical Strike Chance|Focus Magic, Spell Critical Strike Chance"
                        // Defensive
                        + "|Armor|Damage Reduction (Major %)|Damage Reduction (Minor %)|Resistance"
                        // Healing
                        + "|Healing Received (%)"
                        // Sustaining
                        + "|Replenishment|Mana Regeneration|Mana Restore|Burst Mana Regeneration"
                        // Other
                        + "|Racial Buffs|Class Buffs"
                        + "|Temp Power Boost";
                }
                case "Raid Debuffs": {
                    return
                        // Ailment
                           "Boss Attack Speed"
                        // Stat Reduction
                        + "|Melee Hit Chance Reduction|Spell Hit Taken|Critical Strike Chance Taken|Spell Critical Strike Taken"
                        + "|Armor (Major)|Armor (Minor)"
                        // Vulvernability
                        + "|Physical Vulnerability|Spell Sensitivity"
                        + "|Disease Damage Taken|Spell Damage Taken";
                }
                case "Current": { return "Current"; }
                default: { return buffGroup; }
            }
        }

        public void UpdateGraph()
        {
            if (_loading || Calculations.Instance == null || Character == null) return;
            string[] parts = CurrentGraph.Split('|');
            switch (parts[0])
            {
                case "Gear":                UpdateGraphGear(parts[1]); break;
                case "Enchants":            UpdateGraphEnchants(parts[1]); break;
                case "Tinkerings":          UpdateGraphTinkerings(parts[1]); break;
                case "Gems":                UpdateGraphGems(parts[1]); break;
                case "Buffs":               UpdateGraphBuffs(parts[1]); break;
                case "Races":               UpdateGraphRaces(parts[1]); break;
                case "Talents and Glyphs":  UpdateGraphTalents(parts[1]); break;
                case "Equipped":            UpdateGraphEquipped(parts[1]); break;
                case "Item Sets":           UpdateGraphItemSets(parts[1]); break;
                case "Available":           UpdateGraphAvailable(parts[1]); break;
                case "Direct Upgrades":     UpdateGraphDirectUpgrades(parts[1]); break;
                case "Stat Values":         UpdateGraphStatValues(parts[1]); break;
                default: UpdateGraphModelSpecific(parts[1]); break;
            }
        }

        private void SetGraphControl(Control graphControl)
        {
            if (GraphScroll.Content != graphControl)
            {
                GraphScroll.Content = graphControl;
            }
        }

        #region Variables
        private int _calculationCount = 0;
        private ComparisonCalculationBase[] _itemCalculations = null;
        private ComparisonCalculationBase[] _itemSetCalculations = null;
        private ComparisonCalculationBase[] _enchantCalculations = null;
        private ComparisonCalculationBase[] _buffCalculations = null;
        private ComparisonCalculationBase[] _raceCalculations = null;
        private ComparisonCalculationBase[] _talentCalculations = null;
        private ComparisonCalculationBase[] _glyphCalculations = null;
        private ComparisonCalculationBase[] _rsvCalculations = null;
        private ComparisonCalculationBase[] _mdlCalculations = null;
        private AutoResetEvent _autoResetEvent = null;
        private CharacterSlot _characterSlot = CharacterSlot.AutoSelect;
        #region Race Stuff
        public static readonly CharacterRace[] CharacterRaces = {
            CharacterRace.None,
            CharacterRace.Human,
            CharacterRace.Orc,
            CharacterRace.Dwarf,
            CharacterRace.NightElf,
            CharacterRace.Undead,
            CharacterRace.Tauren,
            CharacterRace.Gnome,
            CharacterRace.Troll,
            CharacterRace.BloodElf,
            CharacterRace.Draenei,
            CharacterRace.Worgen,
            CharacterRace.Goblin,
        };
        public static Dictionary<CharacterRace, string[]> CharacterRaceBonuses = new Dictionary<CharacterRace,string[]>()
        {
            {CharacterRace.None, new string[] { "No Racial Bonuses" }},
            {CharacterRace.Human, new string[] {
                    "Diplomacy: Reputation gains increased by 10%.",
                    "Every Man for Himself: Removes all movement impairing effects and all effects which cause loss of control of your character. This effect shares a cooldown with other similar effects.",
                    "Mace Specialization: Expertise with Maces and Two-Handed Maces increased by 3.",
                    "Sword Specialization: Expertise with Swords and Two-Handed Swords increased by 3.",
                    "The Human Spirit: Spirit increased by 3%.",
                }
            },
            {CharacterRace.Orc, new string[] {
                    "Axe Specialization: Expertise with Fist Weapons, Axes and Two-Handed Axes increased by 3.",
                    "Blood Fury: Increases melee attack power by 1169 and your spell damage by 584. Lasts 15 sec.",
                    "Command: Damage dealt by Death Knight, Hunter and Warlock pets increased by 5%.",
                    "Hardiness: Duration of Stun effects reduced by an additional 15%.",
                }
            },
            {CharacterRace.Dwarf, new string[] {
                    "Explorer: You find additional fragments when looting archaeological finds and you can survey faster than normal archaeologists.",
                    "Frost Resistance: Increases your resistance to harmful Frost effects by x.",
                    "Gun Specialization: Your chance to critically hit with Guns is increased by 1%.",
                    "Mace Specialization: Expertise with Maces and Two-Handed Maces increased by 3.",
                    "Stoneform: Removes all poison, disease and bleed effects and increases your armor by 10% for 8 sec.",
                }
            },
            {CharacterRace.NightElf, new string[] {
                    "Nature Resistance: Increases your resistance to harmful Nature effects by x.",
                    "Quickness: Reduces the chance that melee and ranged attackers will hit you by 2%.",
                    "Shadowmeld: Activate to slip into the shadows, reducing the chance for enemies to detect your presence. Lasts until cancelled or upon moving. Any threat is restored versus enemies still in combat upon cancellation of this effect.",
                    "Wisp Spirit: Transform into a wisp upon death, increasing speed by 75%.",
                }
            },
            {CharacterRace.Undead, new string[] {
                    "Cannibalize: When activated, regenerates 7% of total health and mana every 2 sec for 10 sec. Only works on Humanoig or Undead corpses within 5 yards. Any movement, action, or damage taken while Cannibalizing will cancel the effect.",
                    "Shadow Resistance: Increases your resistance to harmful Shadow effects by 1.",
                    "Underwater Breathing: Underwater breath lasts 233% longer than normal.",
                    "Will of the Forsaken: Removes any Charm, Fear and Sleep effect. This effect shares a 30 sec cooldown with other similar effects. (2 min cd)",
                }
            },
            {CharacterRace.Tauren, new string[] {
                    "Cultivation: Herbalism skill increased by 15 and you gather herbs faster than normal herbalists.",
                    "Endurance: Base Health increased by 5%.",
                    "Nature Resistance: Increases your resistance to harmful Nature effects by 1.",
                    "War Stomp: Stuns up to 5 enemies within 8 yards for 2 sec. (0.5 sec cast, 2 min cd)",
                }
            },
            {CharacterRace.Gnome, new string[] {
                    "Arcane Resistance: Increases your resistance to harmful Arcane effects by x.",
                    "Engineering Specialization: Engineering skill increased by 15.",
                    "Escape Artist: Escape the effects of any immobilzation or movement speed reduction effect.",
                    "Expansive Mind: Mana pool increased by 5%.",
                    "Shortblade Specialization: Expertise with Daggers and One-Handed Swords increased by 3.",
                }
            },
            {CharacterRace.Troll, new string[] {
                    "Beast Slaying: Damage dealt versus Beasts increased by 5%.",
                    "Berserking: Increases your attack and casting speed by 20% for 10 sec.",
                    "Bow Specialization: Your chance to critically hit with Bows is increased by 1%.",
                    "Da Voodoo Shuffle: Reduces the duration of all movement impairing effects by 15%. Trolls be flippin' out mon!",
                    "Regeneration: Health regeneration rate increased by 10%. 10% of total Health regeneration may continue during combat.",
                    "Throwing Specialization: Your chance to critically hit with Throwing Weapons is increased by 1%.",
                }
            },
            {CharacterRace.BloodElf, new string[] {
                    "Arcane Affinity: Enchanting skill increased by 10.",
                    "Arcane Resistance: Increases your resistance to harmful Arcane effects by x.",
                    "Silence all enemies within 8 yards for 2 sec and restores (6% of your mana|15 Runic Power|15 Focus|15 Energy|15 Rage). Non-player victim spellcasting is also interrupted for 3 sec.",
                }
            },
            {CharacterRace.Draenei, new string[] {
                    "Gemcutting: Jewelcrafting skill increased by 10",
                    "Gift of the Naaru: Heals the target for x over 15 sec. The amount healed is increased by your attack power.",
                    "Heroic Presence: Increases your chance to hit with all spells and attacks by 1%.",
                    "Shadow Resistance: Increases your resistance to harmful Shadow effects by x.",
                }
            },
            {CharacterRace.Worgen, new string[] {
                    "Aberration: Increases your resistance to harmful Nature and Shadow effects by x.",
                    "Darkflight: Activates your true form, increasing movement speed by 40% for 10 sec.",
                    "Enable Worgen Altered Form: Enables Worgens to switch between human and Worgen forms.",
                    "Flayer: Skinning skill increased by 15 and allows you to skin faster.",
                    "Running Wild: Drop to all fours to run as fast as a wild animal.",
                    "Transform: Worgen: Tranform into Worgen Form.",
                    "Two Forms: Turn into your currently inactive form.",
                    "Visiousness: Increases critical strike chance by 1%.",
                }
            },
            {CharacterRace.Goblin, new string[] {
                    "Best Deals Anywhere: Always receive the best possible gold discount, regardless of faction.",
                    "Better Living Through Chemistry: Alchemy skill increased by 15.",
                    "Pack Hobgoblin: Calls in your friend, Gobber, allowing you bank access for 1 min.",
                    "Rocket Barrage: Launches your belt rockets at an enemy, dealing (1+0.25*AP.429*$SPFI+Level*2+$INT*0.50193) fire damage.",
                    "Rocket Jump: Activates your rocket belt to jump forward.",
                    "Time is Money: Cash in on a 1% increase to attack and casting speed.",
                }
            },
        };
        #endregion
        #endregion

        #region Update Graph
        private void UpdateGraphGear(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            _characterSlot = (CharacterSlot)Enum.Parse(typeof(CharacterSlot), subgraph.Replace(" ", ""), true);
            ComparisonGraph.Slot = _characterSlot;
            bool seenEquippedItem = (Character[_characterSlot] == null);

            Calculations.ClearCache();
            List<ItemInstance> relevantItemInstances = Character.GetRelevantItemInstances(_characterSlot);

            _itemCalculations = new ComparisonCalculationBase[relevantItemInstances.Count];
            _calculationCount = 0;
            _autoResetEvent = new AutoResetEvent(false);

#if DEBUG
            System.Diagnostics.Debug.WriteLine("Starting Comparison Calculations");
            DateTime start = DateTime.Now;
#endif
            if (relevantItemInstances.Count > 0)
            {
                foreach (ItemInstance item in relevantItemInstances)
                {
                    if (!seenEquippedItem && Character[_characterSlot].Equals(item)) seenEquippedItem = true;
                    ThreadPool.QueueUserWorkItem(GetItemInstanceCalculations, item);
                }
                _autoResetEvent.WaitOne();
            }
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Finished Comparison Calculations: Total " + DateTime.Now.Subtract(start).TotalMilliseconds.ToString() + "ms");
#endif

            List<ComparisonCalculationBase> listItemCalculations = new List<ComparisonCalculationBase>(_itemCalculations);
            if (!seenEquippedItem) listItemCalculations.Add(Calculations.GetItemCalculations(Character[_characterSlot], Character, _characterSlot));
            _itemCalculations = FilterTopXGemmings(listItemCalculations);


            CGL_Legend.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            ComparisonGraph.DisplayCalcs(_itemCalculations);
        }

        #region Race Icons
        Dictionary<CharacterRace, string> raceIcons = new Dictionary<CharacterRace, string>() {
            { CharacterRace.Human, "race_human_male" },
            { CharacterRace.Orc, "race_orc_male" },
            { CharacterRace.Dwarf, "race_dwarf_male" },
            { CharacterRace.NightElf, "race_nightelf_male" },
            { CharacterRace.Undead, "race_scourge_male" },
            { CharacterRace.Tauren, "race_tauren_male" },
            { CharacterRace.Gnome, "race_gnome_male" },
            { CharacterRace.Troll, "race_troll_male" },
            { CharacterRace.Goblin, "race_goblin_male" },
            { CharacterRace.BloodElf, "race_bloodelf_male" },
            { CharacterRace.Draenei, "race_draenei_male" },
            { CharacterRace.Worgen, "race_worgen_male" },
        };
        #endregion
        private void UpdateGraphRaces(string subgraph)
        {
            List<ComparisonCalculationBase> raceCalculations = new List<ComparisonCalculationBase>();
            Character newChar = Character.Clone();
            CharacterRace origRace = (CharacterRace)((int)(Character.Race));
            //CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(Character, null, false, true, false);
            CharacterCalculationsBase newCalc;
            CharacterCalculationsBase noneCalc;
            ComparisonCalculationBase compare;

            {
                newChar.Race = CharacterRace.None;
                noneCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                compare = Calculations.GetCharacterComparisonCalculations(noneCalc, noneCalc, CharacterRace.None.ToString(), false, false);
                foreach (string s in CharacterRaceBonuses[CharacterRace.None])
                {
                    compare.Description += s + "\n";
                }
                compare.Description.TrimEnd('\n');
                compare.Item = null;
                raceCalculations.Add(compare);
            }

            foreach (CharacterRace r in CharacterRaces)
            {
                bool isAllowed = false;
                foreach (string cl in MainPage.GetClassAllowableRaces[Character.CurrentModel])
                {
                    if (cl.Replace(" ", "") == r.ToString()/* || r.ToString() == "None"*/)
                    {
                        isAllowed = true; break;
                    } // otherwise skip it as it's not a valid Race/Class combo
                }
                if (!isAllowed) { continue; }
                newChar.Race = r;
                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                compare = Calculations.GetCharacterComparisonCalculations(noneCalc, newCalc, r.ToString(), Character.Race == r, false);
                foreach (string s in CharacterRaceBonuses[r])
                {
                    compare.Description += s + "\n";
                }
                compare.Description.TrimEnd('\n');
                compare.ImageSource = raceIcons[r];
                compare.Item = null;
                raceCalculations.Add(compare);
            }

            Character.Race = origRace;
            CGL_Legend.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            ComparisonGraph.DisplayCalcs(_raceCalculations = raceCalculations.ToArray());
        }

        private void GetItemInstanceCalculations(object item)
        {
            ComparisonCalculationBase result = Calculations.GetItemCalculations((ItemInstance)item, Character, _characterSlot);
            _itemCalculations[Interlocked.Increment(ref _calculationCount) - 1] = result;
            if (_calculationCount == _itemCalculations.Length) _autoResetEvent.Set();
        }
        
        private void UpdateGraphEnchants(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            ItemSlot slot = (ItemSlot)Enum.Parse(typeof(ItemSlot), subgraph.Replace(" 1", "").Replace(" 2", "").Replace(" ", ""), true);
            CGL_Legend.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            ComparisonGraph.DisplayCalcs(_enchantCalculations = Calculations.GetEnchantCalculations(slot, Character, Calculations.GetCharacterCalculations(Character), false).ToArray());
        }

        private void UpdateGraphTinkerings(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            ItemSlot slot = (ItemSlot)Enum.Parse(typeof(ItemSlot), subgraph.Replace(" 1", "").Replace(" 2", "").Replace(" ", ""), true);
            CGL_Legend.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            ComparisonGraph.DisplayCalcs(_enchantCalculations = Calculations.GetTinkeringCalculations(slot, Character, Calculations.GetCharacterCalculations(Character), false).ToArray());
        }

        private void UpdateGraphGems(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            CharacterSlot cslot = CharacterSlot.Gems;
            ItemSlot islot = ItemSlot.None;
            switch (subgraph)
            {
                case "Red":     islot = ItemSlot.Red; break;
                case "Yellow":  islot = ItemSlot.Yellow; break;
                case "Blue":    islot = ItemSlot.Blue; break;
                case "Meta":    cslot = CharacterSlot.Metas; break;
                case "Cogwheel": cslot = CharacterSlot.Cogwheels; break;
                case "Hydraulic": cslot = CharacterSlot.Hydraulics; break;
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

            CGL_Legend.LegendItems = Calculations.SubPointNameColors;
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

        private void UpdateGraphBuffs(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            CGL_Legend.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            ComparisonGraph.DisplayCalcs(_buffCalculations = Calculations.GetBuffCalculations(Character, 
                Calculations.GetCharacterCalculations(Character), ConvertBuffSelector(subgraph)).ToArray());
        }

        #region Class to Glyph Icon
        public static Dictionary<CharacterClass, string[]> classToGlyph = new Dictionary<CharacterClass, string[]>() {
            {CharacterClass.DeathKnight, new string[] { "inv_glyph_minordeathknight", "inv_glyph_majordeathknight", "inv_glyph_primedeathknight" } },
            {CharacterClass.Druid,       new string[] { "inv_glyph_minordruid", "inv_glyph_majordruid", "inv_glyph_primedruid" } },
            {CharacterClass.Hunter,      new string[] { "inv_glyph_minorhunter", "inv_glyph_majorhunter", "inv_glyph_primehunter" } },
            {CharacterClass.Mage,        new string[] { "inv_glyph_minormage", "inv_glyph_majormage", "inv_glyph_primemage" } },
            {CharacterClass.Paladin,     new string[] { "inv_glyph_minorpaladin", "inv_glyph_majorpaladin", "inv_glyph_primepaladin" } },
            {CharacterClass.Priest,      new string[] { "inv_glyph_minorpriest", "inv_glyph_majorpriest", "inv_glyph_primepriest" } },
            {CharacterClass.Rogue,       new string[] { "inv_glyph_minorrogue", "inv_glyph_majorrogue", "inv_glyph_primerogue" } },
            {CharacterClass.Shaman,      new string[] { "inv_glyph_minorshaman", "inv_glyph_majorshaman", "inv_glyph_primeshaman" } },
            {CharacterClass.Warlock,     new string[] { "inv_glyph_minorwarlock", "inv_glyph_majorwarlock", "inv_glyph_primewarlock" } },
            {CharacterClass.Warrior,     new string[] { "inv_glyph_minorwarrior", "inv_glyph_majorwarrior", "inv_glyph_primewarrior" } },
        };
        #endregion
        private void UpdateGraphTalents(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            if        (subgraph == "Individual Talents") {
                #region
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
                            compare = Calculations.GetCharacterComparisonCalculations(newCalc, currentCalc, talentData.Name, talentData.MaxPoints == orig, orig != 0 && orig != talentData.MaxPoints);
                        }
                        else
                        {
                            newChar.CurrentTalents.Data[talentData.Index]++;
                            newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                            compare = Calculations.GetCharacterComparisonCalculations(currentCalc, newCalc, talentData.Name, talentData.MaxPoints == orig, orig != 0 && orig != talentData.MaxPoints);
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
                        compare.ImageSource = talentData.Icon;
                        talentCalculations.Add(compare);
                        newChar.CurrentTalents.Data[talentData.Index] = orig;
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(_talentCalculations = talentCalculations.ToArray());
                #endregion
            } else if (subgraph == "Individual Talents (Full)") {
                #region
                List<ComparisonCalculationBase> talentCalculations = new List<ComparisonCalculationBase>();
                Character newCharEmpty = Character.Clone();
                Character newCharFull = Character.Clone();
                CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(Character, null, false, true, false); ;
                CharacterCalculationsBase newCalcEmpty, newCalcFull;
                ComparisonCalculationBase compare;

                foreach (PropertyInfo pi in Character.CurrentTalents.GetType().GetProperties())
                {
                    TalentDataAttribute[] talentDatas = pi.GetCustomAttributes(typeof(TalentDataAttribute), true) as TalentDataAttribute[];
                    int orig;
                    if (talentDatas.Length > 0) {
                        TalentDataAttribute talentData = talentDatas[0];
                        orig = Character.CurrentTalents.Data[talentData.Index];
                        //
                        newCharEmpty.CurrentTalents.Data[talentData.Index] = 0;
                        newCalcEmpty = Calculations.GetCharacterCalculations(newCharEmpty, null, false, true, false);
                        //
                        newCharFull.CurrentTalents.Data[talentData.Index] = talentData.MaxPoints - 1;
                        newCalcFull = Calculations.GetCharacterCalculations(newCharFull, null, false, true, false);
                        //
                        compare = Calculations.GetCharacterComparisonCalculations(newCalcEmpty, newCalcFull, talentData.Name, talentData.MaxPoints == orig, orig != 0 && orig != talentData.MaxPoints);
                        //
                        string text = string.Format("Current Rank {0}/{1}\r\n\r\n", orig, talentData.MaxPoints);
                        text += talentData.Description[talentData.MaxPoints - 1];
                        compare.Description = text;
                        compare.Item = null;
                        compare.ImageSource = talentData.Icon;
                        talentCalculations.Add(compare);
                        newCharEmpty.CurrentTalents.Data[talentData.Index] = orig;
                        newCharFull.CurrentTalents.Data[talentData.Index] = orig;
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(_talentCalculations = talentCalculations.ToArray());
                #endregion
            } else if (subgraph == "Talent Specs") {
                #region
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
                    compare = Calculations.GetCharacterComparisonCalculations(baseCalc, newCalc, sts.Name, same, false);
                    compare.Item = null;
                    compare.Name = sts.ToString();
                    compare.Description = sts.Spec;
                    talentCalculations.Add(compare);
                    found = found || same;
                }
                if (!found)
                {
                    newCalc = Calculations.GetCharacterCalculations(Character, null, false, true, true);
                    compare = Calculations.GetCharacterComparisonCalculations(baseCalc, newCalc, "Custom", true, false);
                    talentCalculations.Add(compare);
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(_talentCalculations = talentCalculations.ToArray());
                #endregion
            } else if (subgraph == "Glyphs : All") {
                #region
                List<ComparisonCalculationBase> glyphCalculations = new List<ComparisonCalculationBase>();
                Character newChar = Character.Clone();
                CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(Character, null, false, true, false);
                CharacterCalculationsBase newCalc;
                ComparisonCalculationBase compare;
                List<string> relevant = Calculations.GetModel(Character.CurrentModel).GetRelevantGlyphs();

                foreach (PropertyInfo pi in Character.CurrentTalents.GetType().GetProperties()) {
                    GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                    bool orig;
                    if (glyphDatas.Length > 0) {
                        GlyphDataAttribute glyphData = glyphDatas[0];
                        if (relevant == null || relevant.Contains(glyphData.Name)) {
                            orig = Character.CurrentTalents.GlyphData[glyphData.Index];
                            if (orig) {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = false;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(newCalc, currentCalc, glyphData.Name, orig, false);
                            } else {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = true;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(currentCalc, newCalc, glyphData.Name, orig, false);
                            }
                            // JOTHAY: WTB Tooltips that show info on these charts
                            compare.Description = glyphData.Description;
                            compare.Item = null;
                            compare.ImageSource = classToGlyph[Character.Class][(int)glyphData.Type];
                            glyphCalculations.Add(compare);
                            newChar.CurrentTalents.GlyphData[glyphData.Index] = orig;
                        }
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(_glyphCalculations = glyphCalculations.ToArray());
                #endregion
            } else if (subgraph == "Glyphs : Prime") {
                #region
                List<ComparisonCalculationBase> glyphCalculations = new List<ComparisonCalculationBase>();
                Character newChar = Character.Clone();
                CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(Character, null, false, true, false);
                CharacterCalculationsBase newCalc;
                ComparisonCalculationBase compare;
                List<string> relevant = Calculations.GetModel(Character.CurrentModel).GetRelevantGlyphs();

                foreach (PropertyInfo pi in Character.CurrentTalents.GetType().GetProperties()) {
                    GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                    bool orig;
                    if (glyphDatas.Length > 0) {
                        GlyphDataAttribute glyphData = glyphDatas[0];
                        if (relevant == null || (relevant.Contains(glyphData.Name) && glyphData.Type == GlyphType.Prime)) {
                            orig = Character.CurrentTalents.GlyphData[glyphData.Index];
                            if (orig) {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = false;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(newCalc, currentCalc, glyphData.Name, orig, false);
                            } else {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = true;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(currentCalc, newCalc, glyphData.Name, orig, false);
                            }
                            // JOTHAY: WTB Tooltips that show info on these charts
                            compare.Description = glyphData.Description;
                            compare.Item = null;
                            compare.ImageSource = classToGlyph[Character.Class][(int)glyphData.Type];
                            glyphCalculations.Add(compare);
                            newChar.CurrentTalents.GlyphData[glyphData.Index] = orig;
                        }
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(_glyphCalculations = glyphCalculations.ToArray());
                #endregion
            } else if (subgraph == "Glyphs : Major") {
                #region
                List<ComparisonCalculationBase> glyphCalculations = new List<ComparisonCalculationBase>();
                Character newChar = Character.Clone();
                CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(Character, null, false, true, false);
                CharacterCalculationsBase newCalc;
                ComparisonCalculationBase compare;
                List<string> relevant = Calculations.GetModel(Character.CurrentModel).GetRelevantGlyphs();

                foreach (PropertyInfo pi in Character.CurrentTalents.GetType().GetProperties()) {
                    GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                    bool orig;
                    if (glyphDatas.Length > 0) {
                        GlyphDataAttribute glyphData = glyphDatas[0];
                        if (relevant == null || (relevant.Contains(glyphData.Name) && glyphData.Type == GlyphType.Major)) {
                            orig = Character.CurrentTalents.GlyphData[glyphData.Index];
                            if (orig) {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = false;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(newCalc, currentCalc, glyphData.Name, orig, false);
                            } else {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = true;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(currentCalc, newCalc, glyphData.Name, orig, false);
                            }
                            // JOTHAY: WTB Tooltips that show info on these charts
                            compare.Description = glyphData.Description;
                            compare.Item = null;
                            compare.ImageSource = classToGlyph[Character.Class][(int)glyphData.Type];
                            glyphCalculations.Add(compare);
                            newChar.CurrentTalents.GlyphData[glyphData.Index] = orig;
                        }
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(_glyphCalculations = glyphCalculations.ToArray());
                #endregion
            } else if (subgraph == "Glyphs : Minor") {
                #region
                List<ComparisonCalculationBase> glyphCalculations = new List<ComparisonCalculationBase>();
                Character newChar = Character.Clone();
                CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(Character, null, false, true, false);
                CharacterCalculationsBase newCalc;
                ComparisonCalculationBase compare;
                List<string> relevant = Calculations.GetModel(Character.CurrentModel).GetRelevantGlyphs();

                foreach (PropertyInfo pi in Character.CurrentTalents.GetType().GetProperties()) {
                    GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                    bool orig;
                    if (glyphDatas.Length > 0) {
                        GlyphDataAttribute glyphData = glyphDatas[0];
                        if (relevant == null || (relevant.Contains(glyphData.Name) && glyphData.Type == GlyphType.Minor)) {
                            orig = Character.CurrentTalents.GlyphData[glyphData.Index];
                            if (orig) {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = false;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(newCalc, currentCalc, glyphData.Name, orig, false);
                            } else {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = true;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(currentCalc, newCalc, glyphData.Name, orig, false);
                            }
                            // JOTHAY: WTB Tooltips that show info on these charts
                            compare.Description = glyphData.Description;
                            compare.Item = null;
                            compare.ImageSource = classToGlyph[Character.Class][(int)glyphData.Type];
                            glyphCalculations.Add(compare);
                            newChar.CurrentTalents.GlyphData[glyphData.Index] = orig;
                        }
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(_glyphCalculations = glyphCalculations.ToArray());
                #endregion
            }
        }

        private void UpdateGraphEquipped(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            CGL_Legend.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();

            if (subgraph == "Gear" || subgraph == "All")
            {
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
            }
            if (subgraph == "Enchants" || subgraph == "All")
            {
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
            }
            if (subgraph == "Tinkerings" || subgraph == "All")
            {
                ItemSlot[] slots = new ItemSlot[]
                {
                     ItemSlot.Back, ItemSlot.Hands, ItemSlot.Waist,
                };
                foreach (ItemSlot slot in slots)
                    foreach (ComparisonCalculationBase calc in Calculations.GetTinkeringCalculations(slot, Character, Calculations.GetCharacterCalculations(Character), true))
                        itemCalculations.Add(calc);
            }
            if (subgraph == "Buffs" || subgraph == "All")
            {
                itemCalculations.AddRange(Calculations.GetBuffCalculations(Character, Calculations.GetCharacterCalculations(Character), ConvertBuffSelector("Current")));
                ComparisonGraph.DisplayCalcs(itemCalculations.ToArray());
            }
            // Now Push the results to the screen
            ComparisonGraph.DisplayCalcs(_itemCalculations = itemCalculations.ToArray());
        }

        private void UpdateGraphAvailable(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            if (subgraph == "Gear")
            {
                foreach (string availableItem in Character.AvailableItems)
                {
                    availableItem.ToString();
                }

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

        private void UpdateGraphItemSets(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            CGL_Legend.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            List<ComparisonCalculationBase> setCalculations = new List<ComparisonCalculationBase>();

            ItemSet newItemSetNaked = new ItemSet() { Name = "Naked", };
            foreach (CharacterSlot cs in Character.EquippableCharacterSlots)
            {
                newItemSetNaked.Add(null);
            }
            if (!Character.ItemSetListContainsItemSet(newItemSetNaked))
            {
                setCalculations.Add(Calculations.GetItemSetCalculations(newItemSetNaked, Character));
            }

            ItemSet newItemSetCurrent = new ItemSet() { Name = "Current", };
            foreach (CharacterSlot cs in Character.EquippableCharacterSlots)
            {
                newItemSetCurrent.Add(Character[cs]);
            }
            if (!Character.ItemSetListContainsItemSet(newItemSetCurrent)) {
                setCalculations.Add(Calculations.GetItemSetCalculations(newItemSetCurrent, Character));
            }

            foreach (ItemSet IS in Character.GetItemSetList())
            {
                if (IS == null) { continue; }
                setCalculations.Add(Calculations.GetItemSetCalculations(IS, Character));
            }
            // Now Push the results to the screen
            ComparisonGraph.DisplayCalcs(_itemSetCalculations = setCalculations.ToArray());
        }

        private void UpdateGraphDirectUpgrades(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            if (subgraph == "Gear")
            {
                UpdateGraphDirectUpgradesGear(false);
            }
            else if (subgraph == "Gear / Cost")
            {
                UpdateGraphDirectUpgradesGear(true);
            }
            else if (subgraph == "Enchants")
            {
                ComparisonCalculationBase calc = Calculations.CreateNewComparisonCalculation();
                calc.Name = "Chart Not Yet Implemented";
                ComparisonGraph.DisplayCalcs(_itemCalculations = new ComparisonCalculationBase[] { calc });
            }
        }

        private void UpdateGraphDirectUpgradesGear(bool divideByCost)
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

                                    if (!divideByCost || item.Item.Cost > 0.0f)
                                    {
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
                                            if (divideByCost)
                                            {
                                                itemCalc.OverallPoints = difference / item.Item.Cost;
                                            }
                                            else
                                            {
                                                itemCalc.OverallPoints = difference;
                                            }
                                            itemCalculations.Add(itemCalc);
                                        }
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
            ComparisonGraph.DisplayCalcs(_itemCalculations = FilterTopXGemmings(itemCalculations));
        }

        /// <summary>
        /// This function is a copy and is used to populate the Rawr Addon Export
        /// <para>Do not tie the above two functions to this as it has a couple differences</para>
        /// </summary>
        /// <param name="divideByCost"></param>
        /// <returns></returns>
        public ComparisonCalculationBase[] GetDirectUpgradesGearCalcs(bool divideByCost)
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

                if (Finger2 < Finger1) { slotMap[ItemSlot.Finger] = CharacterSlot.Finger2; }
                if (Trinket2 < Trinket1) { slotMap[ItemSlot.Trinket] = CharacterSlot.Trinket2; }

                float MainHand = (Character[CharacterSlot.MainHand] == null ? 0 : Calculations.GetItemCalculations(
                    Character[CharacterSlot.MainHand], Character, CharacterSlot.MainHand).OverallPoints);
                float OffHand = (Character[CharacterSlot.OffHand] == null ? 0 : Calculations.GetItemCalculations(
                    Character[CharacterSlot.OffHand], Character, CharacterSlot.OffHand).OverallPoints);

                if (MainHand > OffHand) { slotMap[ItemSlot.OneHand] = CharacterSlot.OffHand; }

                foreach (KeyValuePair<ItemSlot, CharacterSlot> kvp in Item.DefaultSlotMap)
                {
                    try
                    {
                        ItemSlot iSlot = kvp.Key;
                        CharacterSlot slot;

                        if (slotMap.ContainsKey(iSlot)) {
                            slot = slotMap[iSlot];
                        } else {
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
                                        switch (slot) {
                                            case CharacterSlot.Finger1 : { otherSlot = CharacterSlot.Finger2 ; break; }
                                            case CharacterSlot.Finger2 : { otherSlot = CharacterSlot.Finger1 ; break; }
                                            case CharacterSlot.Trinket1: { otherSlot = CharacterSlot.Trinket2; break; }
                                            case CharacterSlot.Trinket2: { otherSlot = CharacterSlot.Trinket1; break; }
                                            case CharacterSlot.MainHand: { otherSlot = CharacterSlot.OffHand ; break; }
                                            case CharacterSlot.OffHand : { otherSlot = CharacterSlot.MainHand; break; }
                                        }
                                        if (otherSlot != CharacterSlot.None && Character[otherSlot] != null && Character[otherSlot].Id == item.Id)
                                        {
                                            continue;
                                        }
                                    }

                                    if (!divideByCost || item.Item.Cost > 0.0f)
                                    {
                                        ComparisonCalculationBase itemCalc = Calculations.GetItemCalculations(item, Character, slot);
                                        // Makes it more visually apparent that you are wearing an item that could be adjusted
                                        itemCalc.PartEquipped = Character[slot].Item.Id == item.Id;
                                        float difference = itemCalc.OverallPoints - slotCalc.OverallPoints;
                                        if (difference > 0) {
                                            //itemCalc.SubPoints = new float[itemCalc.SubPoints.Length];
                                            for (int x=0; x < itemCalc.SubPoints.Length; x++)
                                            {
                                                itemCalc.SubPoints[x] = itemCalc.SubPoints[x] - slotCalc.SubPoints[x];
                                            }
                                            //if (divideByCost) {
                                                //itemCalc.OverallPoints = difference / item.Item.Cost;
                                            //} else {
                                                itemCalc.OverallPoints = difference;
                                            //}
                                            itemCalculations.Add(itemCalc);
                                        }
                                    }

                                    items[item.GemmedId] = item;
                                }
                            }
                        }
                    } catch (Exception) { }
                }
            }

            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Overall;
            return FilterTopXGemmings(itemCalculations, 1);
        }

        private void UpdateGraphStatValues(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            if (subgraph == "Relative Stat Values")
            {
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(_rsvCalculations = CalculationsBase.GetRelativeStatValues(Character));
            }
        }

        private void UpdateGraphModelSpecific(string subgraph)
        {
            Control control = Calculations.GetCustomChartControl(subgraph);
            if (control != null)
            {
                SetGraphControl(control);
                Calculations.UpdateCustomChartData(Character, subgraph, control);
            }
            else
            {
                SetGraphControl(ComparisonGraph);
                var calcs = Calculations.GetCustomChartData(Character, subgraph); // should be called before requesting SubPointNameColors because some models swap colors depending on subgraph, ideally an api would be changed that allows query of subpoints based on chart name
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(_mdlCalculations = calcs);
            }
        }
        #endregion

        // takes a list of items and returns a sorted filtered array of the top X gemmings
        private ComparisonCalculationBase[] FilterTopXGemmings(List<ComparisonCalculationBase> listItemCalculations, int countOverride=0)
        {
            List<ComparisonCalculationBase> filteredItemCalculations = new List<ComparisonCalculationBase>();
            if (countOverride < 00) countOverride = 00; // Prevent an invalid number
            if (countOverride > 10) countOverride = 10; // Hey, lets not go totally crazy here
            int maxGemmings = (countOverride > 0 ? countOverride : Properties.GeneralSettings.Default.CountGemmingsShown);

            listItemCalculations.Sort(ComparisonGraph.CompareItemCalculations);
            
            Dictionary<int, int> countItem = new Dictionary<int, int>();
            foreach (ComparisonCalculationBase itemCalculation in listItemCalculations)
            {
                int itemId = 0;
                if (itemCalculation.ItemInstance != null)
                {
                    itemId = itemCalculation.ItemInstance.Id;
                }
                else if (itemCalculation.Item != null)
                {
                    itemId = itemCalculation.Item.Id;
                }
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

        private IEnumerable<string> customSubpoints;
        private void SetCustomSubpoints(IEnumerable<string> subpoints)
        {
            if (subpoints == null) customSubpoints = new string[0];
            else customSubpoints = subpoints;

            if (customSubpoints.Count<string>() == 3) {
                // Add combination sorts, like Mit+Surv(noThreat) or Mit+Threat(noSurv)
                List<string> newCustom = customSubpoints.ToList();
                newCustom.Add(newCustom[0] + "+" + newCustom[1]);
                newCustom.Add(newCustom[0] + "+" + newCustom[2]);
                newCustom.Add(newCustom[1] + "+" + newCustom[2]);
                customSubpoints = newCustom;
            } else if (customSubpoints.Count<string>() == 4) {
                // Add combination sorts, like Mit+Surv(noThreat) or Mit+Threat(noSurv)
                List<string> newCustom = customSubpoints.ToList();
                newCustom.Add(newCustom[0] + "+" + newCustom[1]);
                newCustom.Add(newCustom[0] + "+" + newCustom[2]);
                newCustom.Add(newCustom[0] + "+" + newCustom[3]);
                newCustom.Add(newCustom[1] + "+" + newCustom[2]);
                newCustom.Add(newCustom[1] + "+" + newCustom[3]);
                newCustom.Add(newCustom[2] + "+" + newCustom[3]);
                customSubpoints = newCustom;
            } else if (customSubpoints.Count<string>() == 5) {
                // Add combination sorts, like Mit+Surv(noThreat) or Mit+Threat(noSurv)
                List<string> newCustom = customSubpoints.ToList();
                newCustom.Add(newCustom[0] + "+" + newCustom[1]);
                newCustom.Add(newCustom[0] + "+" + newCustom[2]);
                newCustom.Add(newCustom[0] + "+" + newCustom[3]);
                newCustom.Add(newCustom[0] + "+" + newCustom[4]);
                newCustom.Add(newCustom[1] + "+" + newCustom[2]);
                newCustom.Add(newCustom[1] + "+" + newCustom[3]);
                newCustom.Add(newCustom[1] + "+" + newCustom[4]);
                newCustom.Add(newCustom[2] + "+" + newCustom[3]);
                newCustom.Add(newCustom[2] + "+" + newCustom[4]);
                newCustom.Add(newCustom[3] + "+" + newCustom[4]);
                customSubpoints = newCustom;
            }

            List<string> sortOptionsList = new List<string>();
            sortOptionsList.Add("Alphabetical");
            sortOptionsList.Add("Overall");
            foreach (string s in customSubpoints) sortOptionsList.Add(s);
            if (SortCombo.Items.Count == 2) { SortCombo.Items.Clear(); } // Then it's a default list and needs to be removed
            else { SortCombo.SelectedItem = "Overall"; } // There's a valid list in place and we need to enforce Overall as selected instead of a subpoint
            SortCombo.ItemsSource = sortOptionsList;
            SortCombo.SelectedItem = "Overall";
        }

        private void SortChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortCombo != null && SortCombo.SelectedIndex != -1)
            {
                if (ComparisonGraph != null)
                {
                    ComparisonGraph.Sort = (ComparisonSort)(SortCombo.SelectedIndex - 2);
                }
            }
        }

        private void TB_LiveFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // search next selected item
                int sel = ComparisonGraph.FindItem(TB_LiveFilter.Text, 0/*ComparisonGraph.SelectedItemIndex + 1*/);

                // change
                ComparisonGraph.SelectedItemIndex = sel;

                // if N/A - ding
                TB_LiveFilter.Background = new SolidColorBrush(((sel < 0) ? Color.FromArgb(255, 255, 218, 248) : Colors.White));

                // Weird Focus issue
                //toolStripItemComparison.Focus();
                //txtFilterBox.Focus();

                // we handled it fine
                e.Handled = true;
            }
            else
            {
                if (TB_LiveFilter.Background != new SolidColorBrush(Colors.White))
                    TB_LiveFilter.Background = new SolidColorBrush(Colors.White);
                // Weird Focus issue
                //TB_LiveFilter.Focus();
            }
        }

        #region Filter Side-bar
        // From GridSplitter
        DateTime lastClicked;
        GridLength cachedColumnWidth;
        void GridSplitter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            // The Poor Man's DoubleClick:
            if (DateTime.Now.Subtract(lastClicked).TotalMilliseconds < 500) {
                // What column is it in?
                int colIndex = (int)FilterSplitter.GetValue(Grid.ColumnProperty);
                // And what grid are we talking about?
                Grid grid = FilterSplitter.Parent as Grid;
                Debug.Assert(grid.ColumnDefinitions.Count > 0);
                // Cool...now look at the column definition:
                GridLength gridLength = grid.ColumnDefinitions[colIndex].Width;
                // In order to see if we've already slammed the splitter up against the side:
                GridLength splitterWidth = new GridLength(10/*splitter.Width*/);
                if (gridLength.Equals(splitterWidth)) {
                    // Already collapsed..restore:
                    grid.ColumnDefinitions[colIndex].Width = cachedColumnWidth;
                } else {
                    // Collapse the column:
                    cachedColumnWidth = gridLength;
                    grid.ColumnDefinitions[colIndex].Width = splitterWidth;
                }
            }
            lastClicked = DateTime.Now;
        }
        private void ButtonExpand_Click(object sender, RoutedEventArgs e)
        {
            // What column is it in?
            int colIndex = (int)FilterSplitter.GetValue(Grid.ColumnProperty);
            // And what grid are we talking about?
            Grid grid = FilterSplitter.Parent as Grid;
            Debug.Assert(grid.ColumnDefinitions.Count > 0);
            // Cool...now look at the column definition:
            GridLength gridLength = grid.ColumnDefinitions[colIndex].Width;
            // In order to see if we've already slammed the splitter up against the side:
            GridLength splitterWidth = new GridLength(10/*splitter.Width*/);
            if (gridLength.Equals(splitterWidth)) {
                // Already collapsed..restore:
                grid.ColumnDefinitions[colIndex].Width = cachedColumnWidth;
            } else {
                // Collapse the column:
                cachedColumnWidth = gridLength;
                grid.ColumnDefinitions[colIndex].Width = splitterWidth;
            }
        }

        // From Items By Source
        private void ShowItemFilters(object sender, RoutedEventArgs args)
        {
            new EditItemFilter().Show();
        }
        private void BT_SourceFilters_Reset_Click(object sender, RoutedEventArgs e) { SourceFilters_Helper(true, true); }
        private void BT_SourceFilters_CheckAll_Click(object sender, RoutedEventArgs e) { SourceFilters_Helper(true); }
        private void BT_SourceFilters_UnCheckAll_Click(object sender, RoutedEventArgs e) { SourceFilters_Helper(false); }
        private void SourceFilters_Helper(bool Checked, bool Reset=false) {
            if (Reset) { Checked = true; }
            Character.IsLoading = true;
#if SILVERLIGHT
            List<object> sourceFiltersItems = FilterTree.Items.ToList();
#else
            List<object> sourceFiltersItems = new List<object>(FilterTree.Items.Cast<object>());
#endif
            object last = sourceFiltersItems.Last<object>();
            sourceFiltersItems.RemoveAt(sourceFiltersItems.Count-1);
            ItemFilterOther TheOtherOne = last as ItemFilterOther;
            foreach(ItemFilterRegex c in sourceFiltersItems) {
                if (c.Enabled != Checked) {
                    c.Enabled = Checked;
                }
                if (Reset && c.Name.Contains("Disable")) { c.Enabled = false; }
            }
            TheOtherOne.Enabled = Checked;
            // I think this will cut back substantially on the time to process Reset and other commands
            Character.IsLoading = false;
            Character.OnCalculationsInvalidated();
        }

        // From Items By iLevel
        private void BT_SourceFiltersILvl_Reset_Click(object sender, RoutedEventArgs e)
        {
            Character.IsLoading = true;
            RB_iLvl_Checks.IsChecked = true;
            CK_iLvl_0.IsChecked = true;
            CK_iLvl_1.IsChecked = true;
            CK_iLvl_2.IsChecked = true;
            CK_iLvl_3.IsChecked = true;
            CK_iLvl_4.IsChecked = true;
            CK_iLvl_5.IsChecked = true;
            CK_iLvl_6.IsChecked = true;
            CK_iLvl_7.IsChecked = true;
            RS_iLvl.LowerValue = 278;
            RS_iLvl.UpperValue = 377;
            Character.IsLoading = false;
            ItemCache.Instance.OnItemsChanged();
        }
        private void BT_SourceFiltersILvl_UnCheckAll_Click(object sender, RoutedEventArgs e)
        {
            Character.IsLoading = true;
            RB_iLvl_Checks.IsChecked = true;
            CK_iLvl_0.IsChecked = false;
            CK_iLvl_1.IsChecked = false;
            CK_iLvl_2.IsChecked = false;
            CK_iLvl_3.IsChecked = false;
            CK_iLvl_4.IsChecked = false;
            CK_iLvl_5.IsChecked = false;
            CK_iLvl_6.IsChecked = false;
            CK_iLvl_7.IsChecked = false;
            Character.IsLoading = false;
            ItemCache.Instance.OnItemsChanged();
        }

        // From Refine Types of Items Listed
        private List<ItemType> modelRelevant;
        private List<ItemType> userRelevant;
        private List<CheckBox> checkBoxes;
        public void UpdateBoxes()
        {
            if (Calculations.Instance == null || Calculations.Instance.RelevantItemTypes == null) return;
            modelRelevant = Calculations.Instance != null && Calculations.Instance.RelevantItemTypes != null ? Calculations.Instance.RelevantItemTypes : new List<ItemType>();
            userRelevant = Calculations.Instance != null ? ItemFilter.GetRelevantItemTypesList(Calculations.Instance) : new List<ItemType>();
            if ((modelRelevant.Contains(ItemType.Libram) || modelRelevant.Contains(ItemType.Idol)
                || modelRelevant.Contains(ItemType.Totem) || modelRelevant.Contains(ItemType.Sigil))
                && !modelRelevant.Contains(ItemType.Relic))
            {
                modelRelevant.Add(ItemType.Relic);
            }
            if ((userRelevant.Contains(ItemType.Libram) || userRelevant.Contains(ItemType.Idol)
                || userRelevant.Contains(ItemType.Totem) || userRelevant.Contains(ItemType.Sigil))
                && !userRelevant.Contains(ItemType.Relic))
            {
                userRelevant.Add(ItemType.Relic);
            }
            foreach (CheckBox box in checkBoxes)
            {
                if (box == CheckBoxRelic) {
                    box.IsEnabled = modelRelevant.Contains(ItemType.Libram) || modelRelevant.Contains(ItemType.Idol)
                        || modelRelevant.Contains(ItemType.Totem) || modelRelevant.Contains(ItemType.Sigil)
                        || modelRelevant.Contains(ItemType.Relic);
                    box.IsChecked = userRelevant.Contains(ItemType.Libram) || userRelevant.Contains(ItemType.Idol)
                        || userRelevant.Contains(ItemType.Totem) || userRelevant.Contains(ItemType.Sigil)
                        || userRelevant.Contains(ItemType.Relic);
                } else {
                    box.IsEnabled = modelRelevant.Contains((ItemType)Enum.Parse(typeof(ItemType), (string)box.Tag, true));
                    box.IsChecked = userRelevant.Contains((ItemType)Enum.Parse(typeof(ItemType), (string)box.Tag, true));
                }
            }
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e) {
            if (_loading) return;
            userRelevant.Clear();
            modelRelevant = Calculations.Instance.RelevantItemTypes;
            foreach (CheckBox box in checkBoxes) {
                if (box.IsChecked.GetValueOrDefault(false) && box.IsEnabled) {
                    if (box == CheckBoxRelic) {
                        if (modelRelevant.Contains(ItemType.Libram) || modelRelevant.Contains(ItemType.Relic)) { userRelevant.Add(ItemType.Libram); userRelevant.Add(ItemType.Relic); }
                        if (modelRelevant.Contains(ItemType.Totem) || modelRelevant.Contains(ItemType.Relic)) { userRelevant.Add(ItemType.Totem); userRelevant.Add(ItemType.Relic); }
                        if (modelRelevant.Contains(ItemType.Idol) || modelRelevant.Contains(ItemType.Relic)) { userRelevant.Add(ItemType.Idol); userRelevant.Add(ItemType.Relic); }
                        if (modelRelevant.Contains(ItemType.Sigil) || modelRelevant.Contains(ItemType.Relic)) { userRelevant.Add(ItemType.Sigil); userRelevant.Add(ItemType.Relic); }
                        if (modelRelevant.Contains(ItemType.Relic) || modelRelevant.Contains(ItemType.Relic)) { userRelevant.Add(ItemType.Relic); }
                    } else {
                        userRelevant.Add((ItemType)Enum.Parse(typeof(ItemType), (string)box.Tag, true));
                    }
                }
            }
            ItemCache.OnItemsChanged();
            //this.DialogResult = true;
        }

        // From Professions
        public void UpdateProfFilters() {

        }
        private void BT_ApplyProfFilter_Click(object sender, RoutedEventArgs e) {

        }
        #endregion

        #region Export Options
        private void CopyCSVDataToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GetChartDataCSV());
        }

        private void ExportToPawn(object sender, RoutedEventArgs e)
        {
            StringBuilder pawn = new System.Text.StringBuilder();
            pawn.Append("( Pawn: v1: \"Rawr\": "); // adds pawn header
            pawn.Append(getPawnWeightFilter(Character));
            switch (Character.Class) {
                case CharacterClass.DeathKnight:
                case CharacterClass.Druid:
                case CharacterClass.Paladin:
                case CharacterClass.Rogue:
                case CharacterClass.Shaman:
                case CharacterClass.Warrior:
                    { pawn.Append(" MeleeDps=1, "); break; }
                default: break;
            }
            pawn.AppendLine(" )"); // adds pawn footer
            try { Clipboard.SetText(pawn.ToString()); } catch { }
        }
        private static string getPawnWeightFilter(Character character)
        {
            StringBuilder wtf = new StringBuilder();
            ComparisonCalculationBase[] statValues = CalculationsBase.GetRelativeStatValues(character);
            foreach (ComparisonCalculationBase ccb in statValues)
            {
                string stat = getPawnStatID(ccb.Name);
                if (!stat.Equals(string.Empty))
                    wtf.Append(stat + "=" + ccb.OverallPoints.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) + ", ");
            }
            if (wtf.Length == 0)
                return string.Empty;
            else
                return wtf.ToString().Substring(0, wtf.Length - 2); // remove trailing comma
        }
        private static string getPawnStatID(string Name)
        {
            switch (Name)
            {
                case " Strength": return "Strength";
                case " Agility": return "Agility";
                case " Stamina": return "Stamina";
                case " Intellect": return "Intellect";
                case " Spirit": return "Spirit";

                case " Health": return "Health";
                case " Mana": return "Mana";
                case " Health per 5 sec": return "Hp5";
                case " Mana per 5 sec": return "Mp5";

                case " Armor": return "Armor";
                case " Defense Rating": return "DefenseRating";
                case " Block Value": return "BlockValue";
                case " Block Rating": return "BlockRating";
                case " Dodge Rating": return "DodgeRating";
                case " Parry Rating": return "ParryRating";
                case " Bonus Armor": return string.Empty;
                case " Resilience": return "ResilienceRating";

                case " Attack Power": return "Ap";
                case " Spell Power": return "SpellPower";
                case " Expertise Rating": return "ExpertiseRating";
                case " Hit Rating": return "HitRating";
                case " Crit Rating": return "CritRating";
                case " Haste Rating": return "HasteRating";
                case " Melee Crit": return string.Empty;

                case " Feral Attack Power": return string.Empty;
                case " Spell Crit Rating": return string.Empty;
                case " Spell Arcane Damage": return "ArcaneSpellDamage";
                case " Spell Fire Damage": return "FireSpellDamage";
                case " Spell Nature Damage": return "NatureSpellDamage";
                case " Spell Shadow Damage": return "ShadowSpellDamage";
                case " Armor Penetration Rating": return "ArmorPenetration";
            }
            return string.Empty;
        }

        private void ExportToImage(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".png";
            dialog.Filter = "PNG|*.png|GIF|*.gif|JPG|*.jpg|BMP|*.bmp";
            if (dialog.ShowDialog().GetValueOrDefault(false) /*== DialogResult.OK*/)
            {
                try
                {
                    // JOTHAY NOTE TODO: I couldn't find the supported matching class for ImageFormat
                    /*ImageFormat imgFormat = ImageFormat.Bmp;
                    if (dialog.SafeFileName.EndsWith(".png")) imgFormat = ImageFormat.Png;
                    else if (dialog.SafeFileName.EndsWith(".gif")) imgFormat = ImageFormat.Gif;
                    else if (dialog.SafeFileName.EndsWith(".jpg") || dialog.SafeFileName.EndsWith(".jpeg")) imgFormat = ImageFormat.Jpeg;
                    ComparisonGraph.PrerenderedGraph.Save(dialog.SafeFileName, imgFormat);*/
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void ExportToCSV(object sender, RoutedEventArgs e)
        {
            if (!App.Current.IsRunningOutOfBrowser) {
                MessageBox.Show("This function can only be run when Rawr is installed offline due to a Silverlight Permissions issue."
                              + "\n\nYou can install Rawr offline using the button in the Upper Right-Hand corner of the program",
                                "Cannot Perform Action", MessageBoxButton.OK);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".csv";
            dialog.Filter = "Comma Separated Values | *.csv";
            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                try
                {
                    using (StreamWriter writer = File.CreateText(dialog.SafeFileName)) // no path data and no way to get it? wtf?
                    {
                        writer.Write(GetChartDataCSV());
                        writer.Flush();
                        writer.Close();
                        writer.Dispose();
                    }
                }
                catch (Exception ex) {
                    Base.ErrorBox eb = new Base.ErrorBox("Error Saving CSV File", ex.Message);
                    eb.Show();
                }
            }
        }

        private string GetChartDataCSV()
        {
            StringBuilder sb = new StringBuilder("\"Name\",\"Equipped\",\"Slot\",\"Gem1\",\"Gem2\",\"Gem3\",\"Enchant\",\"Source\",\"ItemId\",\"GemmedId\",\"Overall\"");
            foreach (string subPointName in Calculations.SubPointNameColors.Keys)
            {
                sb.AppendFormat(",\"{0}\"", subPointName);
            }
            sb.AppendLine();
            ComparisonCalculationBase[] calcsToExport = _itemCalculations;
            string[] parts = CurrentGraph.Split('|');
            switch (parts[0])
            {
                case "Gear":                calcsToExport = _itemCalculations; break;
                case "Enchants":            calcsToExport = _enchantCalculations; break;
                case "Tinkerings":          calcsToExport = _enchantCalculations; break;
                case "Gems":                calcsToExport = _itemCalculations; break;
                case "Buffs":               calcsToExport = _buffCalculations; break;
                case "Races":               calcsToExport = _raceCalculations; break;
                case "Talents and Glyphs":  calcsToExport = (parts[1].Contains("Talent")) ? _talentCalculations : _glyphCalculations; break;
                case "Equipped":            calcsToExport = _itemCalculations; break;
                case "Item Sets":           calcsToExport = _itemSetCalculations; break;
                case "Available":           calcsToExport = _itemCalculations; break;
                case "Direct Upgrades":     calcsToExport = _itemCalculations; break;
                case "Stat Values":         calcsToExport = _rsvCalculations; break;
                default:                    calcsToExport = _mdlCalculations; break; // Model Specific calcs
            }
            if (calcsToExport == null || calcsToExport.Length <= 0) { return "The chart selected is either not Exportable or is of an Empty List."; }
            foreach (ComparisonCalculationBase comparisonCalculation in calcsToExport)
            {
                ItemInstance itemInstance = comparisonCalculation.ItemInstance;
                Item item = comparisonCalculation.Item;
                if (itemInstance != null)
                {
                    sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\"",
                        itemInstance.Item.Name.Replace(',', ';'),
                        comparisonCalculation.Equipped,
                        itemInstance.Slot,
                        itemInstance.Gem1 != null ? itemInstance.Gem1.Name : null,
                        itemInstance.Gem2 != null ? itemInstance.Gem2.Name : null,
                        itemInstance.Gem3 != null ? itemInstance.Gem3.Name : null,
                        itemInstance.Enchant.Name,
                        itemInstance.Item.LocationInfo[0].Description.Split(',')[0]
                            + (itemInstance.Item.LocationInfo.Count > 1 && itemInstance.Item.LocationInfo[1] != null ? "|" + itemInstance.Item.LocationInfo[1].Description.Split(',')[0] : "")
                            + (itemInstance.Item.LocationInfo.Count > 2 && itemInstance.Item.LocationInfo[2] != null ? "|" + itemInstance.Item.LocationInfo[2].Description.Split(',')[0] : ""),
                        itemInstance.Id,
                        itemInstance.GemmedId,
                        comparisonCalculation.OverallPoints);
                    foreach (float subPoint in comparisonCalculation.SubPoints)
                        sb.AppendFormat(",\"{0}\"", subPoint);
                    sb.AppendLine();
                }
                else if (item != null)
                {
                    sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\"",
                        item.Name.Replace(',', ';'),
                        comparisonCalculation.Equipped,
                        item.Slot,
                        null,
                        null,
                        null,
                        null,
                        item.LocationInfo[0].Description.Split(',')[0]
                            + (item.LocationInfo.Count > 1 && item.LocationInfo[1] != null ? "|" + item.LocationInfo[1].Description.Split(',')[0] : "")
                            + (item.LocationInfo.Count > 2 && item.LocationInfo[2] != null ? "|" + item.LocationInfo[2].Description.Split(',')[0] : ""),
                        item.Id,
                        null,
                        comparisonCalculation.OverallPoints);
                    foreach (float subPoint in comparisonCalculation.SubPoints)
                        sb.AppendFormat(",\"{0}\"", subPoint);
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\"",
                        comparisonCalculation.Name.Replace(',', ';'),
                        comparisonCalculation.Equipped,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        comparisonCalculation.OverallPoints);
                    foreach (float subPoint in comparisonCalculation.SubPoints)
                        sb.AppendFormat(",\"{0}\"", subPoint);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
        #endregion
    }
}
