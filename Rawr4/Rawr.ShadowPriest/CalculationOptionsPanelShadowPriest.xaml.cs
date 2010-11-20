using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Rawr.ShadowPriest.Spells;

namespace Rawr.ShadowPriest
{
    public partial class CalculationOptionsPanelShadowPriest : UserControl, ICalculationOptionsPanel
    {

        public CalculationOptionsPanelShadowPriest()
        {
            InitializeComponent();

                        // Set about text
            tbModuleNotes.Text =
                "Notes:\r\n" +
               "Shadow Priest Notes";
            /*
            SpellBox spellBox = new SpellBox();

            
            foreach (Spell s in spellBox.Spells)
            {
                lsSpellPriority.Items.Add(s.name);
            }
            */
        }
        public UserControl PanelControl { get { return this; } }

        CalculationOptionsShadowPriest calcOpts = null;

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {

                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsShadowPriest)
                    ((CalculationOptionsShadowPriest)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsShadowPriest_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsShadowPriest_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsShadowPriest_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        private bool _loadingCalculationOptions;

        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsShadowPriest();
            calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
            // Model Specific Code
            //
            _loadingCalculationOptions = false;
        }

        void CalculationOptionsShadowPriest_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_loadingCalculationOptions) { return; }
            // This would handle any special changes, especially combobox assignments, but not when the pane is trying to load
            if (e.PropertyName == "SomeProperty")
            {
                // Do some code
            }
            //
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }
        /*
        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSpells.SelectedItem == null)
                return;

            lsSpellPriority.Items.Add(cmbSpells.SelectedItem);
            cmbSpells.Items.RemoveAt(cmbSpells.SelectedIndex);
            cmbSpells.SelectedItem = "";
        }

        private void bUp_Click(object sender, RoutedEventArgs e)
        {
            if (lsSpellPriority.SelectedItem == null || lsSpellPriority.SelectedIndex == 0)
                return;

            lsSpellPriority.Items.Insert(lsSpellPriority.SelectedIndex - 1, lsSpellPriority.SelectedItem);
            lsSpellPriority.Items.RemoveAt(lsSpellPriority.SelectedIndex);
        }

        private void bDown_Click(object sender, RoutedEventArgs e)
        {
            if (lsSpellPriority.SelectedItem == null || lsSpellPriority.SelectedIndex == lsSpellPriority.Items.Count - 1)
                return;

            lsSpellPriority.Items.Insert(lsSpellPriority.SelectedIndex + 2, lsSpellPriority.SelectedItem);
            lsSpellPriority.Items.RemoveAt(lsSpellPriority.SelectedIndex);
        }

        private void bRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lsSpellPriority.SelectedItem == null)
                return;

            cmbSpells.Items.Add(lsSpellPriority.SelectedItem);
            lsSpellPriority.Items.RemoveAt(lsSpellPriority.SelectedIndex);
        }

        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            if (lsSpellPriority.Items.Count == 0)
                return;

            foreach (Object o in lsSpellPriority.Items)
                cmbSpells.Items.Add(o);

            lsSpellPriority.Items.Clear();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            character.OnCalculationsInvalidated();
        }
        */

    }
}
