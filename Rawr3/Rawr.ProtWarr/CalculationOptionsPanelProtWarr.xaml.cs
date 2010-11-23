using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.ProtWarr
{
    public partial class CalculationOptionsPanelProtWarr : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelProtWarr()
        {
            InitializeComponent();
            //this.Layout.SelectAll();
        }

        #region ICalculationOptionsPanel Members
        public UserControl PanelControl { get { return this; } }

        CalculationOptionsProtWarr calcOpts = null;

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsProtWarr)
                    ((CalculationOptionsProtWarr)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsPanelProtWarr_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelProtWarr_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsPanelProtWarr_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsProtWarr();
            calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;

            // Model Specific Code
            bool showSliders = false;
            if (calcOpts.RankingMode == 1)
                showSliders = true;

            MitigationLabel.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
            MitigationValue.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
            //MitigationValue.Value = calcOpts.MitigationScale;

            ThreatLabel.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
            ThreatValue.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
            //ThreatValue.Text = calcOpts.ThreatScale.ToString();
            //

            _loadingCalculationOptions = false;
        }

        void CalculationOptionsPanelProtWarr_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_loadingCalculationOptions) { return; }

            // This would handle any special changes, especially combobox assignments, but not when the pane is trying to load
            switch (e.PropertyName)
            {
                case "RankingMode":
                    bool showSliders = false;
                    if (calcOpts.RankingMode == 1)
                        showSliders = true;

                    MitigationLabel.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
                    MitigationValue.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
                    ThreatLabel.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
                    ThreatValue.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
                    break;
            }
            //

            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }
        #endregion
    }

    public class RankingModeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int rankingMode = (int)value;
            switch (rankingMode)
            {
                case 3: return "Burst Time";
                case 4: return "Damage Output";
                default: return "Default";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string rankingMode = (string)value;
            switch (rankingMode)
            {
                case "Burst Time": return 3;
                case "Damage Output": return 4;
                default: return 1;
            }
        }

        #endregion
    }

    public class ThreatScaleConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)((float)value / 8.0d);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (float)((double)value * 8.0d);
        }

        #endregion
    }

    public class MitigationScaleConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)((float)value / 0.125f);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (float)((double)value * 0.125d);
        }

        #endregion
    }
}
