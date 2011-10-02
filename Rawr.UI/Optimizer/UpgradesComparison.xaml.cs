using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Rawr.Optimizer;
#if !SILVERLIGHT
using Microsoft.Win32;
#endif

namespace Rawr.UI
{
    public partial class UpgradesComparison : ChildWindow
    {
        public Dictionary<string, ComparisonCalculationUpgrades[]> itemCalculations;
        private string[] customSubpoints;

        private CharacterSlot slot;
        public CharacterSlot Slot
        {
            get { return slot; }
            set
            {
                slot = value;
                UpdateGraph();
            }
        }

#if !SILVERLIGHT
        // We don't need this dialog to be modal in WPF
        public new void Show()
        {
            ((Window)this).Show();
        }
#endif

        public UpgradesComparison(TextReader reader)
        {
            InitializeComponent();
            MainPage.Instance.UpgradeListOpen = true;
            Graph.Mode = ComparisonGraph.DisplayMode.Overall;
            Graph.Character = MainPage.Instance.Character;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SerializationData));
                SerializationData data = (SerializationData)serializer.Deserialize(reader);

                SetCustomSubpoints(data.CustomSubpoints);
                itemCalculations = new Dictionary<string, ComparisonCalculationUpgrades[]>();
                for (int i = 0; i < data.Keys.Count; i++)
                {
                    itemCalculations[data.Keys[i]] = data.ItemCalculations[i];
                }

                SlotCombo.SelectedIndex = 0;
            }
            catch (Exception /*e*/)
            {
                Close();
                MessageBox.Show("The chosen file is not a Rawr Upgrade List.", "Unable to Load Upgrade List", MessageBoxButton.OK);
            }
            finally { reader.Dispose(); }
        }

        public UpgradesComparison(Dictionary<CharacterSlot, List<ComparisonCalculationUpgrades>> upgrades, string[] customSubpoints)
        {
            InitializeComponent();
            MainPage.Instance.UpgradeListOpen = true;
            Graph.Mode = ComparisonGraph.DisplayMode.Overall;
            Graph.Character = MainPage.Instance.Character;

            SetCustomSubpoints(customSubpoints);

            itemCalculations = new Dictionary<string, ComparisonCalculationUpgrades[]>();
            List<ComparisonCalculationUpgrades> all = new List<ComparisonCalculationUpgrades>();
            foreach (KeyValuePair<CharacterSlot, List<ComparisonCalculationUpgrades>> kvp in upgrades) {
                all.AddRange(kvp.Value);
                itemCalculations["Gear." + kvp.Key.ToString()] = kvp.Value.ToArray();
            }
            if (all.Count == 0) {
                all.Add(new ComparisonCalculationUpgrades() {
                    Name = "The List/Evaluation returned no Upgrades *",
                    Description = "Evaluate Upgrade (Item): The item itself could not be placed into an item set with your available gear."
                                + "\r\nEvaluate Upgrade (Slot): No items in this slot could be placed into an item set with your available gear."
                                + "\r\nBuild Upgrade List: No items, enchants, tinkerings, reforgings or enchants could be found as part of a set with your current gear to improve your score."
                                + "\r\n\r\nIn any of these cases, you could try a higher optimizer thoroughness setting or different filters to see if you can generate an upgrade path."
                                + " Otherwise, it is reasonably safe to say that the item or group of items is/are not an upgrade(s).",
                });
            }
            itemCalculations["Gear.All"] = all.ToArray();

            SlotCombo.SelectedIndex = 0;
        }

        private void SetCustomSubpoints(string[] subpoints)
        {
            if (subpoints == null) customSubpoints = new string[0];
            else customSubpoints = subpoints;

            List<string> sortOptionsList = new List<string>();
            sortOptionsList.Add("Alphabetical");
            sortOptionsList.Add("Overall");
            foreach (string s in customSubpoints) sortOptionsList.Add(s);
            SortCombo.ItemsSource = sortOptionsList;
            SortCombo.SelectedIndex = 1;
        }

        public void UpdateGraph()
        {
            if (Slot == CharacterSlot.AutoSelect) Graph.DisplayCalcs(itemCalculations["Gear.All"]);
            else Graph.DisplayCalcs(itemCalculations["Gear." + Slot.ToString()]);
        }

        private void SlotChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SlotCombo.SelectedIndex == 0) Slot = CharacterSlot.AutoSelect;
            else Slot = (CharacterSlot)SlotCombo.SelectedIndex;
        }

        private void SortChanged(object sender, SelectionChangedEventArgs e)
        {
            Graph.Sort = (ComparisonSort)(SortCombo.SelectedIndex - 2);
        }

        public class SerializationData
        {
            public string[] CustomSubpoints;
            public List<string> Keys;
            public List<ComparisonCalculationUpgrades[]> ItemCalculations;
        }

        private void SaveButton_Clicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Rawr Upgrade List Files|*.xml";
            sfd.DefaultExt = "*.xml";
            if (sfd.ShowDialog().GetValueOrDefault())
            {
                SerializationData data = new SerializationData();
                data.CustomSubpoints = customSubpoints;
                data.Keys = new List<string>(itemCalculations.Keys);
                data.ItemCalculations = new List<ComparisonCalculationUpgrades[]>(itemCalculations.Values);

                XmlSerializer serializer = new XmlSerializer(typeof(SerializationData));
                using (Stream fileStream = sfd.OpenFile()) serializer.Serialize(fileStream, data);
            }
        }

        private void ExpandButton_Clicked(object sender, RoutedEventArgs e)
        {
            Width = 750;
            BT_Expand.Visibility = Visibility.Collapsed;
            BT_Contract.Visibility = Visibility.Visible;
        }

        private void ContractButton_Clicked(object sender, RoutedEventArgs e)
        {
            Width = 500;
            BT_Expand.Visibility = Visibility.Visible;
            BT_Contract.Visibility = Visibility.Collapsed;
        }

        #region Export Options
        private void CopyCSVDataToClipboard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GetChartDataCSV());
        }

        private void ExportToCSV(object sender, RoutedEventArgs e)
        {
            if (!App.Current.IsRunningOutOfBrowser) {
                MessageBox.Show("This function can only be run when Rawr is installed offline due to a Silverlight Permissions issue."
                              + "\n\nYou can install Rawr offline using the button in the Upper Right-Hand corner of the program",
                                "Cannot Perform Action", MessageBoxButton.OK);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".csv";
            dialog.Filter = "Comma Separated Values | *.csv";
            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                try
                {
                    using (StreamWriter writer = File.CreateText(dialog.SafeFileName)) // no path data and no way to get it? wtf?
                    {
                        writer.Write(GetChartDataCSV());
                        writer.Flush();
                        writer.Close();
                        writer.Dispose();
                    }
                }
                catch (Exception ex) {
                    Base.ErrorBox eb = new Base.ErrorBox("Error Saving CSV File", ex.Message);
                    eb.Show();
                }
            }
        }

        private string GetChartDataCSV()
        {
            StringBuilder sb = new StringBuilder("\"Name\",\"Equipped\",\"Slot\",\"Gem1\",\"Gem2\",\"Gem3\",\"Enchant\",\"Source\",\"ItemId\",\"GemmedId\",\"Overall\"");
            foreach (string subPointName in Calculations.SubPointNameColors.Keys)
            {
                sb.AppendFormat(",\"{0}\"", subPointName);
            }
            sb.AppendLine();
            List<ComparisonCalculationUpgrades> calcsToExport = new List<ComparisonCalculationUpgrades>();
            foreach (string key in itemCalculations.Keys)
            {
                calcsToExport.AddRange(itemCalculations[key].ToList());
            }
            if (calcsToExport == null || calcsToExport.Count <= 0) { return "The chart selected is either not Exportable or is of an Empty List."; }
            foreach (ComparisonCalculationUpgrades comparisonCalculation in calcsToExport)
            {
                ItemInstance itemInstance = comparisonCalculation.ItemInstance;
                Item item = comparisonCalculation.Item;
                if (itemInstance != null)
                {
                    sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\"",
                        itemInstance.Item.Name.Replace(',', ';'),
                        comparisonCalculation.Equipped,
                        itemInstance.Slot,
                        itemInstance.Gem1 != null ? itemInstance.Gem1.Name : null,
                        itemInstance.Gem2 != null ? itemInstance.Gem2.Name : null,
                        itemInstance.Gem3 != null ? itemInstance.Gem3.Name : null,
                        itemInstance.Enchant.Name,
                        itemInstance.Item.LocationInfo[0].Description.Split(',')[0]
                            + (itemInstance.Item.LocationInfo.Count > 1 /*&& itemInstance.Item.LocationInfo[1] != null*/ ? "|" + itemInstance.Item.LocationInfo[1].Description.Split(',')[0] : "")
                            + (itemInstance.Item.LocationInfo.Count > 2 /*&& itemInstance.Item.LocationInfo[2] != null*/ ? "|" + itemInstance.Item.LocationInfo[2].Description.Split(',')[0] : ""),
                        itemInstance.Id,
                        itemInstance.GemmedId,
                        comparisonCalculation.OverallPoints);
                    if (comparisonCalculation.SubPoints != null)
                    {
                        foreach (float subPoint in comparisonCalculation.SubPoints)
                        {
                            sb.AppendFormat(",\"{0}\"", subPoint);
                        }
                    }
                    sb.AppendLine();
                }
                else if (item != null)
                {
                    sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\"",
                        item.Name.Replace(',', ';'),
                        comparisonCalculation.Equipped,
                        item.Slot,
                        null,
                        null,
                        null,
                        null,
                        item.LocationInfo[0].Description.Split(',')[0]
                            + (item.LocationInfo.Count > 1 /*&& item.LocationInfo[1] != null*/ ? "|" + item.LocationInfo[1].Description.Split(',')[0] : "")
                            + (item.LocationInfo.Count > 2 /*&& item.LocationInfo[2] != null*/ ? "|" + item.LocationInfo[2].Description.Split(',')[0] : ""),
                        item.Id,
                        null,
                        comparisonCalculation.OverallPoints);
                    foreach (float subPoint in comparisonCalculation.SubPoints)
                        sb.AppendFormat(",\"{0}\"", subPoint);
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\"",
                        comparisonCalculation.Name.Replace(',', ';'),
                        comparisonCalculation.Equipped,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        comparisonCalculation.OverallPoints);
                    if (comparisonCalculation.SubPoints != null)
                    {
                        foreach (float subPoint in comparisonCalculation.SubPoints)
                        {
                            sb.AppendFormat(",\"{0}\"", subPoint);
                        }
                    }
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private void ExportToRawrAddon(object sender, RoutedEventArgs e)
        {
            RawrAddonSaveDialog rawrAddonSave = new RawrAddonSaveDialog(MainPage.Instance.Character, itemCalculations["Gear.All"], customSubpoints);
            rawrAddonSave.Show();
        }
        #endregion
    }
}
