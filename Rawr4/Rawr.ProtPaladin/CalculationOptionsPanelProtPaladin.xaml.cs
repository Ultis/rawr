using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;

namespace Rawr.ProtPaladin
{
    public partial class CalculationOptionsPanelProtPaladin : UserControl, ICalculationOptionsPanel
    {
        #region Initialization

        private static string[] TargetTypes = new string[]
        {
            "Unspecified",
            "Humanoid",
            "Undead",
            "Demon",
            "Elemental",
            "Giant",
            "Mechanical",
            "Beast",
            "Dragonkin"
        };

        private static string[] MagicDamageTypes = new string[]
        {
            "None",
            "Physical",
            "Holy",
            "Fire",
            "Nature",
            "Frost",
            "Frostfire",
            "Shadow",
            "Arcane",
            "Spellfire",
            "Naturefire"
        };

        private static string[] TrinketOnUseHandling = new string[]
        {
            "Ignore",
            "Averaged Uptime",
            "Active"
        };

        private static string[] SealChoices = new string[]
        {
            "Seal of Truth",
            "Seal of Righteousness"
        };

        private static string[] MainAttackChoices = new string[]
        {
            "Crusader Strike",
            "Hammer of the Righteous"
        };

        private static string[] PriorityChoices = new string[]
        {
            "AS > HW",
            "AS > Con > HW",
            "AS > Con > HoW > HW",
            "AS > HoW > HW",
            "AS > HoW > Con > HW",
            "HW > AS",
            "HW > Con > AS",
            "HW > Con > HoW > AS",
            "HW > HoW > Con > AS",
            "HW > HoW > AS",
            "Con > AS > HW",
            "Con > AS > HoW > HW",
            "Con > HW > AS",
            "Con > HW > HoW > AS",
            "Con > HoW > AS > HW",
            "Con > HoW > HW > AS",
            "HoW > AS > HW",
            "HoW > AS > Con > HW",
            "HoW > HW > AS",
            "HoW > HW > Con > AS",
            "HoW > Con > AS > HW",
            "HoW > Con > HW > AS"
        };

        public CalculationOptionsPanelProtPaladin()
        {
            InitializeComponent();

            // Setup TargetType combo box
            cboTargetType.ItemsSource = TargetTypes;

            // Setup MagicDamageType combo box
            cboMagicDamageType.ItemsSource = MagicDamageTypes;

            // Setup TrinketOnUseHandling combo box
            cboTrinketOnUseHandling.ItemsSource = TrinketOnUseHandling;

            // Setup SealChoice combo box
            cboSealChoice.ItemsSource = SealChoices;

            // Setup MainAttackChoice combo box
            cboMainAttackChoice.ItemsSource = MainAttackChoices;

            // Setup PriorityChoice combo box
            cboPriorityChoice.ItemsSource = PriorityChoices;

            // PTR Mode - Allow PTR mode or not by leaving one of these lines commented out
            borPTR.Visibility = Visibility.Collapsed; // PTR mode not available
            //borPTR.Visibility = Visibility.Visible;   // PTR mode available
        }

        #endregion

        #region ICalculationOptionsPanel
        public UserControl PanelControl { get { return this; } }

        private CalculationOptionsProtPaladin calcOpts;
        
        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsProtPaladin)
                    ((CalculationOptionsProtPaladin)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsPanelProtPaladin_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelProtPaladin_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsPanelProtPaladin_PropertyChanged(null, new PropertyChangedEventArgs(""));

                if (tbBossAttackSpeed != null)
                    tbBossAttackSpeed.Text = string.Format("{0:N2} seconds", calcOpts.BossAttackSpeed);

                if (tbBossAttackSpeedMagic != null)
                    tbBossAttackSpeedMagic.Text = string.Format("{0:N2} seconds", calcOpts.BossAttackSpeedMagic);

                if (tbThreatScale != null)
                    tbThreatScale.Text = (calcOpts.ThreatScale / 10f).ToString("N2");

                if (tbMitigationScale != null)
                    tbMitigationScale.Text = (calcOpts.MitigationScale / 0.125f).ToString("N2");
            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsProtPaladin();
            calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
            // Model Specific Code
            //
            _loadingCalculationOptions = false;
        }

        void CalculationOptionsPanelProtPaladin_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        #region Events

        private void btnResetBossAttackValue_Click(object sender, RoutedEventArgs e)
        {
            calcOpts.BossAttackValue = 80000;
        }

        private void sliBossAttackSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbBossAttackSpeed != null)
                tbBossAttackSpeed.Text = string.Format("{0:N2} seconds", e.NewValue);
        }

        private void btnResetBossAttackSpeed_Click(object sender, RoutedEventArgs e)
        {
            calcOpts.BossAttackSpeed = 2.0f;
        }

        private void btnResetBossAttackValueMagic_Click(object sender, RoutedEventArgs e)
        {
            calcOpts.BossAttackValueMagic = 20000;
        }

        private void silBossAttackSpeedMagic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbBossAttackSpeedMagic != null)
                tbBossAttackSpeedMagic.Text = string.Format("{0:N2} seconds", e.NewValue);
        }

        private void btnResetBossAttackSpeedMagic_Click(object sender, RoutedEventArgs e)
        {
            calcOpts.BossAttackSpeedMagic = 1.0f;
        }

        private void cboRankingMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = cboRankingMode.Items.IndexOf(e.AddedItems[0]);

            // Only enable threat scale for RankingModes other than 2
            if (btnResetThreatScale != null && silThreatScale != null)
                btnResetThreatScale.IsEnabled = silThreatScale.IsEnabled = (selectedIndex != 2);

            // Only enable mitigation scale for RankingModes 0
            if (btnResetMitigationScale != null && silMitigationScale != null)
                btnResetMitigationScale.IsEnabled = silMitigationScale.IsEnabled = (selectedIndex == 0);

            // Set the default ThreatScale
            if (selectedIndex == 2)
                calcOpts.ThreatScale = 0f;
            else
                calcOpts.ThreatScale = 10f;
        }

        private void silThreatScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbThreatScale != null)
                tbThreatScale.Text = (e.NewValue / 10f).ToString("N2");
        }

        private void btnResetThreatScale_Click(object sender, RoutedEventArgs e)
        {
            calcOpts.ThreatScale = 10f;
        }

        private void silMitigationScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (tbMitigationScale != null)
                tbMitigationScale.Text = (e.NewValue / 0.125f).ToString("N2");
        }

        private void btnResetMitigationScale_Click(object sender, RoutedEventArgs e) {
            calcOpts.MitigationScale = 0.125f;
        }

        #endregion
    }
}
