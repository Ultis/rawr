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
using System.Text.RegularExpressions;

namespace Rawr.Mage
{
    public partial class CooldownRestrictionsDialog : ChildWindow
    {
        public CooldownRestrictionsDialog()
        {
            InitializeComponent();
        }

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
                CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
                if (calculationOptions.CooldownRestrictions != null)
                {
                    textBoxCooldownRestrictions.Text = Regex.Replace(calculationOptions.CooldownRestrictions, "(\r|\n)+", Environment.NewLine);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            calculationOptions.CooldownRestrictions = Regex.Replace(textBoxCooldownRestrictions.Text, "(\r|\n)+", Environment.NewLine);
            Character.OnCalculationsInvalidated();
        }

#if !SILVERLIGHT
        private void ListBoxState_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string data = GetDataFromListBox(ListBoxState, e.GetPosition(ListBoxState));

            if (data != null)
            {
                DragDrop.DoDragDrop(ListBoxState, data, DragDropEffects.Copy);
            }
        }

        private static string GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return (string)((ListBoxItem)data).Content;
                }
            }

            return null;
        }
#endif
    }
}

