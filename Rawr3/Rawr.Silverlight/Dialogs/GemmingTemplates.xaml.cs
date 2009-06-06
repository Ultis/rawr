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
using System.Windows.Data;

namespace Rawr.Silverlight
{
    public partial class GemmingTemplates : ChildWindow
    {
        public GemmingTemplates()
        {
            InitializeComponent();

            TemplateGrid.ItemsSource = GemmingTemplate.CurrentTemplates;
            TemplateGrid.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
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

