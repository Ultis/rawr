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
            SetEnhSimTextBoxText();
            DataContext = this;
        }

        #region ICalculationOptionsPanel Members
        public UserControl PanelControl { get { return this; } }

        private Character character;
        public Character Character
        {
            get
            {
                return character;
            }
            set
            {
                character = value;
                LoadCalculationOptions();
            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (character != null && character.CalculationOptions != null && character.CalculationOptions is CalculationOptionsEnhance)
                ((CalculationOptionsEnhance)character.CalculationOptions).PropertyChanged -= new PropertyChangedEventHandler(calcOpts_PropertyChanged);

            if (character.CalculationOptions == null)
                character.CalculationOptions = new CalculationOptionsEnhance();

            CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            DataContext = calcOpts;
            calcOpts.PropertyChanged += new PropertyChangedEventHandler(calcOpts_PropertyChanged);

            _loadingCalculationOptions = false;
        }
        #endregion

        private void calcOpts_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
            // set all the options from calcOpts
            Character.OnCalculationsInvalidated();
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
