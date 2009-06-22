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

namespace Rawr.Silverlight
{
    public partial class CustomItemInstance : ChildWindow
    {

        private Character character;
        public Character Character
        {
            get { return character; }
            set { character = value; }
        }

        private ItemInstance customInstance;
        public ItemInstance CustomInstance
        {
            get { return customInstance; }
            set { customInstance = value; }
        }

        public CustomItemInstance(Character character, ItemInstance baseInstance)
        {
            InitializeComponent();
            Character = character;
            CustomInstance = baseInstance.Clone();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

