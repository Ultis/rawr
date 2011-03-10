using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.UI
{
    public partial class CalculationDisplay : UserControl
    {

        public CalculationDisplay()
        {
            InitializeComponent();
            BuildControls();
#if SILVERLIGHT
            TheScrollViewer.SetIsMouseWheelScrollingEnabled(true);
#endif
            Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
        }

        void Calculations_ModelChanged(object sender, EventArgs e)
        {
            BuildControls();
        }

        Dictionary<string, AccordionItem> AccordionItems = new Dictionary<string, AccordionItem>();
        Dictionary<string, TextBlock> LabelLabels = new Dictionary<string, TextBlock>();
        Dictionary<string, TextBlock> ValueLabels = new Dictionary<string, TextBlock>();
        private void BuildControls()
        {
            CalculationsAccordion.Items.Clear();
            AccordionItems.Clear();
            LabelLabels.Clear();
            ValueLabels.Clear();

            string[] displayLabelConfigurationStrings = null;
            if (Calculations.Instance != null)
            {
                displayLabelConfigurationStrings = Calculations.CharacterDisplayCalculationLabels;
            }
            else
            {
                displayLabelConfigurationStrings = new string[0];
            }
            foreach (string displayLabelConfigurationString in displayLabelConfigurationStrings)
            {
                #region Determine the Group that the value belongs to. If an Accordion isn't created for this group, make a new one.
                string[] displayLabelConfigurationSplit = displayLabelConfigurationString.Split(':');
                string group = displayLabelConfigurationSplit[0];
                if (!AccordionItems.ContainsKey(group))
                {
                    AccordionItem accordionItem = new AccordionItem();
                    accordionItem.Header = group;
                    accordionItem.Tag = group;
                    accordionItem.IsSelected = true;
                    StackPanel accordionSp = new StackPanel();
                    accordionSp.Margin = new Thickness(4);
                    accordionItem.Content = accordionSp;

                    AccordionItems[group] = accordionItem;
                    CalculationsAccordion.Items.Add(accordionItem);
                }
                #endregion

                string name = displayLabelConfigurationSplit[1];
                string[] nameSplit = name.Split('*');
                name = nameSplit[0];

                #region Create a Horizontal StackPanel to function as the Row
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                #endregion

                #region Create the left side label which describes what the value is. A tooltip explains in further detail if set
                TextBlock labelLabel = new TextBlock();
                labelLabel.MouseEnter += new MouseEventHandler(labelLabel_MouseEnter);
                labelLabel.MouseLeave += new MouseEventHandler(labelLabel_MouseLeave);
                labelLabel.Width = 160;
                labelLabel.Text = name + (nameSplit.Length > 1 ? ": *" : ":");
                sp.Children.Add(labelLabel);
                LabelLabels[name] = labelLabel;
                TooltipDictionary.Add(labelLabel, new string[] { name, nameSplit.Length > 1 ? nameSplit[1] : "" });
                //if (nameSplit.Length > 1) { ToolTipService.SetToolTip(labelLabel, nameSplit[1]); } // turning this off in favor of new static tooltip
                #endregion

                #region Create the right side label states the value. A tooltip explains in further detail with more value related info if set
                TextBlock labelValue = new TextBlock();
                ValueLabels[name] = labelValue;
                sp.Children.Add(labelValue);
                #endregion

                ((StackPanel)AccordionItems[group].Content).Children.Add(sp);
            }
        }

        #region Tooltip Functions/Variables
        private static Dictionary<UIElement, string[]> TooltipDictionary = new Dictionary<UIElement, string[]>();
        public static StatTooltip StatTooltip = null;
        void labelLabel_MouseEnter(object sender, MouseEventArgs e)
        {
            string[] tmp;
            if (TooltipDictionary.TryGetValue(sender as UIElement, out tmp)) {
                StatTooltip = new StatTooltip() { Header = tmp[0], Message = tmp[1] };
                StatTooltip.Show(sender as UIElement, 162, 0);
            }
        }
        void labelLabel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (StatTooltip != null) { StatTooltip.Hide(); }
        }
        #endregion

        public void SetCalculations(Dictionary<string, string> displayCalculationValues)
        {
            foreach (KeyValuePair<string, string> kvp in displayCalculationValues)
            {
                string[] valueSplit = kvp.Value.Split('*');
                string value = valueSplit[0];
                string valueTTHeader = valueSplit.Length > 2 ? valueSplit[1] : "";
                string valueTT = (valueSplit.Length > 2 ? valueSplit[2] : (valueSplit.Length > 1 ? valueSplit[1] : ""));
                TextBlock label;
                //
                if (ValueLabels.TryGetValue(kvp.Key, out label))
                {
                    TooltipDictionary[label] = new string[] { valueTTHeader, valueTT };
                    label.MouseEnter -= new MouseEventHandler(labelLabel_MouseEnter);
                    label.MouseLeave -= new MouseEventHandler(labelLabel_MouseLeave);
                    label.MouseEnter += new MouseEventHandler(labelLabel_MouseEnter);
                    label.MouseLeave += new MouseEventHandler(labelLabel_MouseLeave);
                    if (valueSplit.Length > 1) {
                        label.Text = value + " *";
                        //ToolTipService.SetToolTip(label, valueSplit[1]);
                    } else {
                        label.Text = value;
                        //ToolTipService.SetToolTip(label, null);
                    }
                }
            }
        }

    }
}