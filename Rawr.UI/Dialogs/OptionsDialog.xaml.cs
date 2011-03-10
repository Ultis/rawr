using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using Rawr.Properties;

namespace Rawr.UI
{
    public partial class OptionsDialog : ChildWindow
    {
        public OptionsDialog()
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
#endif

            CK_MultiThreading.IsChecked                     = GeneralSettings.Default.UseMultithreading;
            CK_BuffSource.IsChecked                         = GeneralSettings.Default.DisplayBuffSource;
            CK_GemNames.IsChecked                           = GeneralSettings.Default.DisplayGemNames;
            CK_ShowRelToEqu.IsChecked                       = GeneralSettings.Default.ShowRelativeToEquipped;
            //CK_DisplayExtraItemInfo.IsChecked             = GeneralSettings.Default.DisplayExtraItemInfo;
            CK_HideEnchantsBasedOnProfs.IsChecked           = GeneralSettings.Default.HideProfEnchants;
            CK_EnforceGemCheck.IsChecked                    = GeneralSettings.Default.EnforceGemRequirements;
            CK_EnforceMeta.IsChecked                        = GeneralSettings.Default.EnforceGemRequirements_Meta;
            CK_EnforceJC.IsChecked                          = GeneralSettings.Default.EnforceGemRequirements_JC;
            CK_EnforceUnique.IsChecked                      = GeneralSettings.Default.EnforceGemRequirements_Unique;
            CK_EnforceMeta.IsEnabled                        = CK_EnforceGemCheck.IsChecked == true;
            CK_EnforceJC.IsEnabled                          = CK_EnforceGemCheck.IsChecked == true;
            CK_EnforceUnique.IsEnabled                      = CK_EnforceGemCheck.IsChecked == true;
            CK_CompactMode.IsChecked                        = GeneralSettings.Default.DisplayInCompactMode;
            CK_PTRModeCheck.IsChecked                       = GeneralSettings.Default.PTRMode;
            LO_EnforceGemSubs.Visibility                    = Visibility.Collapsed; // Astry doesn't want those options visible
            WarningsCheck.IsChecked                         = OptimizerSettings.Default.WarningsEnabled;
            TemplateGemsCheck.IsChecked                     = OptimizerSettings.Default.TemplateGemsEnabled;
            OptimizationMethodCombo.SelectedIndex           = (int)OptimizerSettings.Default.OptimizationMethod;
            GreedyMethodCombo.SelectedIndex                 = (int)OptimizerSettings.Default.GreedyOptimizationMethod;
            ProcEffectModeCombo.SelectedIndex               = GeneralSettings.Default.ProcEffectMode;
            EffectCombinationsCalculationMode.SelectedIndex = GeneralSettings.Default.CombinationEffectMode;
            CB_ItemNameWidthSetting.SelectedIndex           = GeneralSettings.Default.ItemNameWidthSetting;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            GeneralSettings.Default.ShowRelativeToEquipped        = CK_ShowRelToEqu.IsChecked.GetValueOrDefault(false);
            OptimizerSettings.Default.TemplateGemsEnabled         = TemplateGemsCheck.IsChecked.GetValueOrDefault(true);
            OptimizerSettings.Default.WarningsEnabled             = WarningsCheck.IsChecked.GetValueOrDefault(true);
            GeneralSettings.Default.UseMultithreading             = CK_MultiThreading.IsChecked.GetValueOrDefault(true);
            GeneralSettings.Default.DisplayBuffSource             = CK_BuffSource.IsChecked.GetValueOrDefault(true);
            GeneralSettings.Default.DisplayGemNames               = CK_GemNames.IsChecked.GetValueOrDefault(true);
            //GeneralSettings.Default.DisplayExtraItemInfo        = CK_DisplayExtraItemInfo.IsChecked.GetValueOrDefault();
            GeneralSettings.Default.HideProfEnchants              = CK_HideEnchantsBasedOnProfs.IsChecked.GetValueOrDefault(true);
            GeneralSettings.Default.EnforceGemRequirements        = CK_EnforceGemCheck.IsChecked.GetValueOrDefault(true);
            GeneralSettings.Default.EnforceGemRequirements_Meta   = CK_EnforceMeta.IsChecked.GetValueOrDefault(true);
            GeneralSettings.Default.EnforceGemRequirements_JC     = CK_EnforceJC.IsChecked.GetValueOrDefault(true);
            GeneralSettings.Default.EnforceGemRequirements_Unique = CK_EnforceUnique.IsChecked.GetValueOrDefault(true);
            GeneralSettings.Default.DisplayInCompactMode          = CK_CompactMode.IsChecked.GetValueOrDefault(false);
            GeneralSettings.Default.ItemNameWidthSetting          = CB_ItemNameWidthSetting.SelectedIndex;
            GeneralSettings.Default.PTRMode                       = CK_PTRModeCheck.IsChecked.GetValueOrDefault(false);
            OptimizerSettings.Default.OptimizationMethod          = (OptimizationMethod)OptimizationMethodCombo.SelectedIndex;
            OptimizerSettings.Default.GreedyOptimizationMethod    = (GreedyOptimizationMethod)OptimizationMethodCombo.SelectedIndex;
            GeneralSettings.Default.ProcEffectMode                = ProcEffectModeCombo.SelectedIndex;
            GeneralSettings.Default.CombinationEffectMode         = EffectCombinationsCalculationMode.SelectedIndex;
            OnDisplayBuffChanged();
            OnHideProfessionsChanged();
            SpecialEffect.UpdateCalculationMode();
            MainPage.Instance.Character.OnCalculationsInvalidated();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) { this.DialogResult = false; }

        public static event EventHandler DisplayBuffChanged;
        protected static void OnDisplayBuffChanged() { if (DisplayBuffChanged != null) { DisplayBuffChanged(null, EventArgs.Empty); } }

        public static event EventHandler HideProfessionsChanged;
        protected static void OnHideProfessionsChanged() { if (HideProfessionsChanged != null) { HideProfessionsChanged(null, EventArgs.Empty); } }

        private void EnforceGemCheck_CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox sndr = sender as CheckBox;
            if        (sndr == CK_EnforceGemCheck) {
                // We just changed the primary Gem Enforcer
                if (CK_EnforceGemCheck.IsChecked == true) {
                    // We set it to true, so we want the subs to be true
                    CK_EnforceMeta.IsChecked = true;
                    CK_EnforceJC.IsChecked = true;
                    CK_EnforceUnique.IsChecked = true;
                    CK_EnforceMeta.IsEnabled = true;
                    CK_EnforceJC.IsEnabled = true;
                    CK_EnforceUnique.IsEnabled = true;
                } else {
                    // We set it to false, so we want the subs to be false
                    //CK_EnforceMeta.IsChecked = false;
                    //CK_EnforceJC.IsChecked = false;
                    //CK_EnforceUnique.IsChecked = false;
                    CK_EnforceMeta.IsEnabled = false;
                    CK_EnforceJC.IsEnabled = false;
                    CK_EnforceUnique.IsEnabled = false;
                }
            } else if (sndr == CK_EnforceMeta    ) {
                // do nothing
            } else if (sndr == CK_EnforceJC      ) {
                // do nothing
            } else if (sndr == CK_EnforceUnique  ) {
                // do nothing
            }
        }
    }
}

