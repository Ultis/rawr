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

namespace Rawr.Enhance
{
    public partial class CalculationOptionsPanelEnhance : ICalculationOptionsPanel
    {
        public CalculationOptionsPanelEnhance()
        {
            InitializeComponent();
            btnExport.Click += new RoutedEventHandler(btnExport_Click);
            SetEnhSimTextBoxText();
        }

        #region ICalculationOptionsPanel Members
        public UserControl PanelControl { get { return this; } }

        private CalculationOptionsEnhance calcOpts;
        private BossOptions bossOpts;

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsEnhance)
                    ((CalculationOptionsEnhance)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsPanelEnhance_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelEnhance_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsPanelEnhance_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsEnhance();
            calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
            bossOpts = Character.BossOptions;
            // Model Specific Code
            //
            _loadingCalculationOptions = false;
        }

        private void CalculationOptionsPanelEnhance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_loadingCalculationOptions) { return; }
            // This would handle any special changes, especially combobox assignments, but not when the pane is trying to load
            /*if (e.PropertyName == "SomeProperty")
            {
                // Do some code
            }*/
            if (e.PropertyName.Contains("SG_")
                || e.PropertyName == "StatsList"
                || e.PropertyName == "StatsIncrement"
                || e.PropertyName == "CalculationToGraph")
            {
                // These fields shouldn't be forcing new calcs on each click
                return;
            }
            //
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }
        #endregion

        private Stats[] BuildStatsList()
        {
            List<Stats> statsList = new List<Stats>();
            if (CK_Stats_0.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Strength = 1f }); }
            if (CK_Stats_1.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Agility = 1f }); }
            if (CK_Stats_2.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { AttackPower = 2f }); }
            if (CK_Stats_3.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { CritRating = 1f }); }
            if (CK_Stats_4.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { HitRating = 1f }); }
            if (CK_Stats_5.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { ExpertiseRating = 1f }); }
            if (CK_Stats_6.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { HasteRating = 1f }); }
            if (CK_Stats_7.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { MasteryRating = 1f }); }
            if (CK_Stats_8.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Intellect = 1f }); }
            return statsList.ToArray();
        }

        private void BT_StatsGraph_Click(object sender, RoutedEventArgs e)
        {
            Stats[] statsList = BuildStatsList();
            StatGraphWindow gw = new StatGraphWindow();
            string explanatoryText = "This graph shows how adding or subtracting\nmultiples of a stat affects your dps.\n\nAt the Zero position is your current dps.\n" +
                         "To the right of the zero vertical is adding stats.\nTo the left of the zero vertical is subtracting stats.\n" +
                         "The vertical axis shows the amount of dps added or lost";
            gw.GetGraph.SetupStatsGraph(Character, statsList, calcOpts.StatsIncrement, explanatoryText, calcOpts.CalculationToGraph);
            gw.Show();
        }

        private void btnExport_Click(Object sender,RoutedEventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                Enhance.EnhSim simExport = new Enhance.EnhSim(Character, calcOpts, bossOpts);
                simExport.copyToClipboard();
            }
        }

        private void SetEnhSimTextBoxText()
        {
            EnhSim_Instructions.Text = @"The EnhSim export option exists for users that wish to have very detailed analysis of their stats. For most users the standard model should be quite sufficient.

If you wish to use the EnhSim Simulator you will need to get the latest version from http://enhsim.codeplex.com

Once you have installed the simulator the easiest way to run it is to run EnhSimGUI and use the Clipboard copy functions.

Press the button above to copy your current Rawr.Enhance data to the clipboard then in EnhSimGUI click on the 'Import from Clipboard' button to replace the values in the EnhSimGUI with your Rawr values. Now all you need to do is click Simulate to get your results.

Refer to the EnhSim website for more detailed instructions on how to use the sim and its various options.";
        }
    }
}
