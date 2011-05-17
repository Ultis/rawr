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
using System.Globalization;

namespace Rawr.ProtWarr
{
    public partial class CalculationOptionsPanelProtWarr : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelProtWarr()
        {
            InitializeComponent();
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

            LB_HitsToSurvive.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
            NUD_HitsToSurvive.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
            LB_ThreatScale.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
            NUD_ThreatScale.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);

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

                    LB_HitsToSurvive.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
                    NUD_HitsToSurvive.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
                    LB_ThreatScale.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
                    NUD_ThreatScale.Visibility = (showSliders ? Visibility.Visible : Visibility.Collapsed);
                    break;
            }
            this.UpdateLayout();
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }
        #endregion
        private void btnResetHitsToSurvive_Click(object sender, RoutedEventArgs e) { calcOpts.HitsToSurvive = 3.1f; }
        private void btnResetBurstScale_Click(object sender, RoutedEventArgs e) { calcOpts.BurstScale = 3.0f; }
        private void btnResetThreatScale_Click(object sender, RoutedEventArgs e) { calcOpts.ThreatScale = 5f; }
    }

    #region Converters
    public class RankingModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int rankingMode = (int)value;
            switch (rankingMode)
            {
                case 2: return "Burst Time";
                default: return "Mitigation Scale";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string rankingMode = (string)value;
            switch (rankingMode)
            {
                case "Burst Time": return 2;
                default: return 1;
            }
        }
    }
    public class ThreatValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            float threatValue = (float)value;
            if (threatValue == 1f) return "Almost None";
            if (threatValue == 5f) return "MT";
            if (threatValue == 25f) return "OT";
            if (threatValue == 50f) return "Crazy About Threat";
            else return "Custom...";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string threatValue = (string)value;
            switch (threatValue)
            {
                case "Almost None": return 1f;
                case "MT": return 5f;
                case "OT": return 25f;
                case "Crazy About Threat": return 50f;
            }
            return null;
        }
    }
    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(float)) return System.Convert.ToSingle(value) * 100.0f;
            if (targetType == typeof(double)) return System.Convert.ToDouble(value) * 100.0d;
            return System.Convert.ToDouble(value) * 100.0d;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(float)) return System.Convert.ToSingle(value) / 100f;
            if (targetType == typeof(double)) return System.Convert.ToDouble(value) / 100d;
            return System.Convert.ToDouble(value) / 100d;
        }
    }
    #endregion
}
