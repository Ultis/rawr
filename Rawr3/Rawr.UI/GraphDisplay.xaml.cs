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

            SV_Bind.SetIsMouseWheelScrollingEnabled(true);
            SV_Prof.SetIsMouseWheelScrollingEnabled(true);
            SV_iLevel.SetIsMouseWheelScrollingEnabled(true);
            SV_Type.SetIsMouseWheelScrollingEnabled(true);
            SV_Gems.SetIsMouseWheelScrollingEnabled(true);

            // From Refine Types of Items Listed
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
                case "Gems":                UpdateGraphGems(parts[1]); break;
                case "Buffs":               UpdateGraphBuffs(parts[1]); break;
                case "Races":               UpdateGraphRaces(parts[1]); break;
                case "Talents and Glyphs":  UpdateGraphTalents(parts[1]); break;
                case "Equipped":            UpdateGraphEquipped(parts[1]); break;
                case "Available":           UpdateGraphAvailable(parts[1]); break;
                case "Direct Upgrades":     UpdateGraphDirectUpgrades(parts[1]); break;
                case "Stat Values":         UpdateGraphStatValues(parts[1]); break;
                default:                    UpdateGraphModelSpecific(parts[1]); break;
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
        private AutoResetEvent _autoResetEvent = null;
        private CharacterSlot _characterSlot = CharacterSlot.AutoSelect;
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
                compare.Item = null;
                raceCalculations.Add(compare);
            }

            Character.Race = origRace;
            CGL_Legend.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            ComparisonGraph.DisplayCalcs(raceCalculations.ToArray());
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
            ComparisonGraph.DisplayCalcs(Calculations.GetEnchantCalculations(slot, Character, Calculations.GetCharacterCalculations(Character), false).ToArray());
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
            ComparisonGraph.DisplayCalcs(Calculations.GetBuffCalculations(Character, 
                Calculations.GetCharacterCalculations(Character), ConvertBuffSelector(subgraph)).ToArray());
        }

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
                        talentCalculations.Add(compare);
                        newChar.CurrentTalents.Data[talentData.Index] = orig;
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(talentCalculations.ToArray());
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
                        talentCalculations.Add(compare);
                        newCharEmpty.CurrentTalents.Data[talentData.Index] = orig;
                        newCharFull.CurrentTalents.Data[talentData.Index] = orig;
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(talentCalculations.ToArray());
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
                ComparisonGraph.DisplayCalcs(talentCalculations.ToArray());
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
                            glyphCalculations.Add(compare);
                            newChar.CurrentTalents.GlyphData[glyphData.Index] = orig;
                        }
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(glyphCalculations.ToArray());
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
                            glyphCalculations.Add(compare);
                            newChar.CurrentTalents.GlyphData[glyphData.Index] = orig;
                        }
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(glyphCalculations.ToArray());
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
                            glyphCalculations.Add(compare);
                            newChar.CurrentTalents.GlyphData[glyphData.Index] = orig;
                        }
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(glyphCalculations.ToArray());
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
                            glyphCalculations.Add(compare);
                            newChar.CurrentTalents.GlyphData[glyphData.Index] = orig;
                        }
                    }
                }
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(glyphCalculations.ToArray());
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
            if (subgraph == "Buffs" || subgraph == "All")
            {
                itemCalculations.AddRange(Calculations.GetBuffCalculations(Character, Calculations.GetCharacterCalculations(Character), ConvertBuffSelector("Current")));
                ComparisonGraph.DisplayCalcs(itemCalculations.ToArray());
            }
            // Now Push the results to the screen
            ComparisonGraph.DisplayCalcs(itemCalculations.ToArray());
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
                ComparisonGraph.DisplayCalcs(new ComparisonCalculationBase[] { calc });
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
            ComparisonGraph.DisplayCalcs(FilterTopXGemmings(itemCalculations));
        }

        private void UpdateGraphStatValues(string subgraph)
        {
            SetGraphControl(ComparisonGraph);
            if (subgraph == "Relative Stat Values")
            {
                CGL_Legend.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
                ComparisonGraph.DisplayCalcs(CalculationsBase.GetRelativeStatValues(Character));
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
                ComparisonGraph.DisplayCalcs(calcs);
            }
        }
        #endregion

        // takes a list of items and returns a sorted filtered array of the top X gemmings
        private ComparisonCalculationBase[] FilterTopXGemmings(List<ComparisonCalculationBase> listItemCalculations)
        {
            List<ComparisonCalculationBase> filteredItemCalculations = new List<ComparisonCalculationBase>();
            int maxGemmings = Properties.GeneralSettings.Default.CountGemmingsShown;

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
                ComparisonGraph.Sort = (ComparisonSort)(SortCombo.SelectedIndex - 2);
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
            List<object> sourceFiltersItems = FilterTree.Items.ToList();
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
            foreach (CheckBox box in checkBoxes) {
                if (box == CheckBoxRelic) {
                    box.IsEnabled = modelRelevant.Contains(ItemType.Libram) || modelRelevant.Contains(ItemType.Idol)
                       || modelRelevant.Contains(ItemType.Totem) ||modelRelevant.Contains(ItemType.Sigil);
                    box.IsChecked = userRelevant.Contains(ItemType.Libram) || userRelevant.Contains(ItemType.Idol)
                        || userRelevant.Contains(ItemType.Totem) || userRelevant.Contains(ItemType.Sigil);
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
                        if (modelRelevant.Contains(ItemType.Libram)) userRelevant.Add(ItemType.Libram);
                        if (modelRelevant.Contains(ItemType.Totem)) userRelevant.Add(ItemType.Totem);
                        if (modelRelevant.Contains(ItemType.Idol)) userRelevant.Add(ItemType.Idol);
                        if (modelRelevant.Contains(ItemType.Sigil)) userRelevant.Add(ItemType.Sigil);
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
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".csv";
            dialog.Filter = "Comma Separated Values | *.csv";
            if (dialog.ShowDialog().GetValueOrDefault(false) /*== DialogResult.OK*/)
            {
                try
                {
                    using (StreamWriter writer = File.CreateText(dialog.SafeFileName))
                    {
                        writer.Write(GetChartDataCSV());
                        writer.Flush();
                        writer.Close();
                        writer.Dispose();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private string GetChartDataCSV()
        {
            StringBuilder sb = new StringBuilder("Name,Equipped,Slot,Gem1,Gem2,Gem3,Enchant,Source,ItemId,GemmedId,Overall");
            foreach (string subPointName in Calculations.SubPointNameColors.Keys)
            {
                sb.AppendFormat(",{0}", subPointName);
            }
            sb.AppendLine();
            foreach (ComparisonCalculationBase comparisonCalculation in _itemCalculations)
            {
                ItemInstance itemInstance = comparisonCalculation.ItemInstance;
                Item item = comparisonCalculation.Item;
                if (itemInstance != null)
                {
                    sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                        itemInstance.Item.Name.Replace(',', ';'),
                        comparisonCalculation.Equipped,
                        itemInstance.Slot,
                        itemInstance.Gem1 != null ? itemInstance.Gem1.Name : null,
                        itemInstance.Gem2 != null ? itemInstance.Gem2.Name : null,
                        itemInstance.Gem3 != null ? itemInstance.Gem3.Name : null,
                        itemInstance.Enchant.Name,
                        itemInstance.Item.LocationInfo[0].Description.Split(',')[0] + (itemInstance.Item.LocationInfo[1]!=null ? "|" + itemInstance.Item.LocationInfo[1].Description.Split(',')[0] : ""),
                        itemInstance.Id,
                        itemInstance.GemmedId,
                        comparisonCalculation.OverallPoints);
                    foreach (float subPoint in comparisonCalculation.SubPoints)
                        sb.AppendFormat(",{0}", subPoint);
                    sb.AppendLine();
                }
                else if (item != null)
                {
                    sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                        item.Name.Replace(',', ';'),
                        comparisonCalculation.Equipped,
                        item.Slot,
                        null,
                        null,
                        null,
                        null,
                        item.LocationInfo[0].Description.Split(',')[0] + (item.LocationInfo[1] != null ? "|" + item.LocationInfo[1].Description.Split(',')[0] : ""),
                        item.Id,
                        null,
                        comparisonCalculation.OverallPoints);
                    foreach (float subPoint in comparisonCalculation.SubPoints)
                        sb.AppendFormat(",{0}", subPoint);
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
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
                        sb.AppendFormat(",{0}", subPoint);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
        #endregion
    }
}