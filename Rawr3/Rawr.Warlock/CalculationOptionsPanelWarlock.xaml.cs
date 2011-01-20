using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Rawr.Warlock
{
    public partial class CalculationOptionsPanelWarlock : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelWarlock()
        {
            InitializeComponent();
            DataContext = this;
        }
        public UserControl PanelControl { get { return this; } }      
        public Character Character
        {
            get { return m_character; }
            set
            {
                // Kill any old event connections
                if (m_character != null && m_character.CalculationOptions != null
                    && m_character.CalculationOptions is CalculationOptionsWarlock)
                    ((CalculationOptionsWarlock)m_character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsPanelWarlock_PropertyChanged);
                // Apply the new character
                m_character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = m_calcOpts;
                // Add new event connections
                m_calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelWarlock_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsPanelWarlock_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }
        public void LoadCalculationOptions()
        {
            m_loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsWarlock();
            m_calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
            if (m_calcOpts.Rotations.Count == 0)
            {
                CalculationOptionsWarlock.AddDefaultRotations(m_calcOpts.Rotations);
            }
            RefreshRotationPanel();
            m_loadingCalculationOptions = false;
        }

        private void CalculationOptionsPanelWarlock_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (m_loadingCalculationOptions) { return; }
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }        
        private void RefreshRotationPanel() {
            m_loadingCalculationOptions = true;

            Rotation rotation = m_calcOpts.GetActiveRotation();

            rotationCombo.Items.Clear();
            foreach (Rotation r in m_calcOpts.Rotations) {
                rotationCombo.Items.Add(r.Name);
            }
            rotationCombo.SelectedItem = rotation.Name;

            rotationMenu.Items.Clear();
            foreach (string spell in Spell.ALL_SPELLS) {
                if (!GetActivePriorities().Contains(spell)
                    && !fillerCombo.Items.Contains(spell)
                    && !executeCombo.Items.Contains(spell)
                    && spell != "Shadow Bolt (Instant)") {

                    rotationMenu.Items.Add(spell);
                }
            }

            rotationList.Items.Clear();
            foreach (string spell in GetActivePriorities()) {
                rotationList.Items.Add(spell);
            }

            fillerCombo.SelectedItem = rotation.Filler;
            if (rotation.Execute == null) {
                executeCombo.SelectedItem = "None";
            } else {
                executeCombo.SelectedItem = rotation.Execute;
            }

            RefreshRotationButtons();

            m_loadingCalculationOptions = false;
        }
        private void RefreshRotationButtons() {
            int itemCount = rotationList.Items.Count;
            int curIndex = rotationList.SelectedIndex;

            rotationAddButton.IsEnabled = rotationMenu.SelectedIndex >= 0;
            rotationUpButton.IsEnabled = curIndex > 0;
            rotationDownButton.IsEnabled = curIndex >= 0 && curIndex < itemCount - 1;
            rotationClearButton.IsEnabled = itemCount > 0;

            rotationErrorLabel.Text = m_calcOpts.GetActiveRotation().GetError();
        }
        private void RotationSwap(int swapWith) {
            int oldIndex = rotationList.SelectedIndex;
            int newIndex = oldIndex + swapWith;
            Utilities.SwapElements(GetActivePriorities(), oldIndex, newIndex);
            RefreshRotationPanel();
            rotationList.SelectedIndex = newIndex;
        }
        private List<string> GetActivePriorities() {
            return m_calcOpts.GetActiveRotation().SpellPriority;
        }       
        private void rotationAddButton_Click(object sender, RoutedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            m_loadingCalculationOptions = true;
            GetActivePriorities().Add((string) rotationMenu.SelectedItem);
            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
            m_loadingCalculationOptions = false;
        }
        private void rotationRemoveButton_Click(object sender, RoutedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            int index = rotationList.SelectedIndex;
            GetActivePriorities().RemoveAt(index);
            RefreshRotationPanel();
            int itemCount = rotationList.Items.Count;
            if (itemCount > 0) {
                rotationList.SelectedIndex = Math.Min(index, itemCount - 1);
            }
            Character.OnCalculationsInvalidated();
        }
        private void rotationUpButton_Click(object sender, RoutedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            RotationSwap(-1);
            Character.OnCalculationsInvalidated();
        }
        private void rotationDownButton_Click(object sender, RoutedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            RotationSwap(1);
            Character.OnCalculationsInvalidated();
        }
        private void rotationClearButton_Click(object sender, RoutedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            GetActivePriorities().Clear();
            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
        }
        private void rotationMenu_SelectedIndexChanged(object sender, SelectionChangedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            RefreshRotationButtons();
        }
        private void rotationList_SelectedIndexChanged(object sender, SelectionChangedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            RefreshRotationButtons();
        }
        private void fillerCombo_SelectedIndexChanged(object sender, SelectionChangedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            m_calcOpts.GetActiveRotation().Filler
                = (string) fillerCombo.SelectedItem;
            Character.OnCalculationsInvalidated();
        }
        private void rotationCombo_SelectedIndexChanged(object sender, SelectionChangedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }
            m_calcOpts.ActiveRotationIndex = rotationCombo.SelectedIndex;
            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
        }
        private void newRotationButton_Click(object sender, RoutedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            TextInputDialog d = new TextInputDialog("New Rotation", "Choose a name:", "", 
                m_calcOpts.Rotations.Select(x => x.Name));
            d.Closed += new EventHandler((a, b) => {
                if (d.DialogResult.GetValueOrDefault()) {
                    m_calcOpts.Rotations.Add(new Rotation(d.Result, "Shadow Bolt", null));
                    m_calcOpts.ActiveRotationIndex = m_calcOpts.Rotations.Count - 1;
                    RefreshRotationPanel();
                    Character.OnCalculationsInvalidated();
                }
            });
            d.Show();
        }
        private void rotationRenameButton_Click(object sender, RoutedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            Rotation rotation = m_calcOpts.GetActiveRotation();
            TextInputDialog d = new TextInputDialog("Rename Rotation", "Rename to:", rotation.Name, 
                m_calcOpts.Rotations.Select(x => x.Name).Where(x => x != rotation.Name));
            d.Closed += new EventHandler((a, b) => {
                if (d.DialogResult.GetValueOrDefault()) {
                    rotation.Name = d.Result;
                    RefreshRotationPanel();
                    Character.OnCalculationsInvalidated();
                }
            });
            d.Show();
        }
        private void deleteRotationButton_Click(object sender, RoutedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            m_calcOpts.RemoveActiveRotation();
            if (m_calcOpts.Rotations.Count == 0) {
                m_calcOpts.Rotations.Add(new Rotation("New Rotation", "Shadow Bolt", null));
                m_calcOpts.ActiveRotationIndex = 0;
            }

            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
        }
        private void executeCombo_SelectedIndexChanged(object sender, SelectionChangedEventArgs e) {
            if (m_loadingCalculationOptions) { return; }

            if ((string)executeCombo.SelectedItem == "None") {
                m_calcOpts.GetActiveRotation().Execute = null;
            } else {
                m_calcOpts.GetActiveRotation().Execute = (string)executeCombo.SelectedItem;
            }
            Character.OnCalculationsInvalidated();
        }        
        private void TimerButton_Click(object sender, RoutedEventArgs e) {
            /*System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
            CalculationsWarlock calculations = (CalculationsWarlock)Calculations.Instance;
            Character character = Character;
            clock.Start();
            for (int i = 0; i < 4000; i++) {
                calculations.GetCharacterCalculations(character);
            }
            clock.Stop();
            MessageBox.Show(clock.Elapsed.TotalSeconds + " seconds.");*/
        }

        private bool m_loadingCalculationOptions;
        private CalculationOptionsWarlock m_calcOpts = null;
        private Character m_character;

        protected Stats[] BuildStatsList()
        {
            List<Stats> statsList = new List<Stats>();
            if (CK_Stats_0.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Intellect = 1f }); }
            if (CK_Stats_1.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Spirit = 1f }); }
            if (CK_Stats_2.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { SpellPower = 2f }); }
            if (CK_Stats_3.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { CritRating = 1f }); }
            if (CK_Stats_4.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { HitRating = 1f }); }
            if (CK_Stats_5.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { SpellPenetration = 1f }); }
            if (CK_Stats_6.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { HasteRating = 1f }); }
            if (CK_Stats_7.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { MasteryRating = 1f }); }
            return statsList.ToArray();
        }
        private void CB_CalculationToGraph_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_calcOpts != null) // in WPF can be called while loading xaml
            {
                m_calcOpts.CalculationToGraph = (string)CB_CalculationToGraph.SelectedItem;
            }
        }

        protected void BT_StatsGraph_Click(object sender, RoutedEventArgs e)
        {
            Stats[] statsList = BuildStatsList();
            StatGraphWindow gw = new StatGraphWindow();
            string explanatoryText = "This graph shows how adding or subtracting\nmultiples of a stat affects your dps.\n\nAt the Zero position is your current dps.\n" +
                         "To the right of the zero vertical is adding stats.\nTo the left of the zero vertical is subtracting stats.\n" +
                         "The vertical axis shows the amount of dps added or lost";
            gw.GetGraph.SetupStatsGraph(Character, statsList, m_calcOpts.StatsIncrement, explanatoryText, m_calcOpts.CalculationToGraph);
            gw.Show();
        }
    }
}
