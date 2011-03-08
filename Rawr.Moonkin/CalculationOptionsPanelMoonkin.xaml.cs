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

namespace Rawr.Moonkin
{
    public partial class CalculationOptionsPanelMoonkin : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelMoonkin()
        {
            InitializeComponent();
        }

        private Stats[] BuildStatsList()
        {
            return new Stats[] {
                new Stats { Intellect = 1f },
                new Stats { Spirit = 1f },
                new Stats { SpellPower = 1f },
                new Stats { CritRating = 1f },
                new Stats { HitRating = 1f },
                new Stats { HasteRating = 1f },
                new Stats { MasteryRating = 1f }
            };
        }

        protected void BT_StatsGraph_Click(object sender, RoutedEventArgs e)
        {
            Stats[] statsList = BuildStatsList();
            StatGraphWindow gw = new StatGraphWindow();
            string explanatoryText = "This graph shows how adding or subtracting\nmultiples of a stat affects your dps.\n\nAt the Zero position is your current dps.\n" +
                         "To the right of the zero vertical is adding stats.\nTo the left of the zero vertical is subtracting stats.\n" +
                         "The vertical axis shows the amount of dps added or lost";
            gw.GetGraph.SetupStatsGraph(Character, statsList, 500, explanatoryText, "Burst Damage");
            gw.Show();
        }

        #region ICalculationOptionsPanel Members
        public UserControl PanelControl { get { return this; } }

        CalculationOptionsMoonkin calcOpts = null;

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsMoonkin)
                    ((CalculationOptionsMoonkin)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsPanelMoonkin_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelMoonkin_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsPanelMoonkin_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsMoonkin();
            calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
            if (calcOpts.TargetLevel < 85) calcOpts.TargetLevel = 88;
            // Model Specific Code
            //
            _loadingCalculationOptions = false;
        }

        void CalculationOptionsPanelMoonkin_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
        #endregion
    }
}
