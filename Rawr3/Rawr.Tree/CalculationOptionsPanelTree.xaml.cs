using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;


namespace Rawr.Tree
{
    public partial class CalculationOptionsPanelTree : UserControl, ICalculationOptionsPanel 
    {
        public CalculationOptionsPanelTree()
        {
            InitializeComponent();
        }


        public UserControl PanelControl { get { return this; } }

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
//                character = value;
//                LoadCalculationOptions();

                if (character != null && character.CalculationOptions != null && character.CalculationOptions is CalculationOptionsTree)
                    ((CalculationOptionsTree)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(calcOpts_PropertyChanged);

                character = value;
                if (character.CalculationOptions == null)
                    character.CalculationOptions = new CalculationOptionsTree();

                CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;
                DataContext = calcOpts;
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(calcOpts_PropertyChanged);
            }
        }

//        private bool _loadingCalculationOptions;
/*        public void LoadCalculationOptions()
        {
//            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsTree();

//            _loadingCalculationOptions = false;
        }
        */

        private void calcOpts_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }
    }
}
