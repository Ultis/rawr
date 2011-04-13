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

namespace Rawr.UI
{
    public partial class RandomSuffixEditor : ChildWindow
    {
        private Item item;
        private List<SuffixAvailability> data;

        public class SuffixAvailability
        {
            public int Id { get; set; }
            public string Suffix { get; set; }
            public Stats Stats { get; set; }
            public bool Available { get; set; }
        }

        public RandomSuffixEditor(Item item)
        {
            this.item = item;

            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif

            data = new List<SuffixAvailability>();
            foreach (var s in RandomSuffix.GetAllSuffixes())
            {
                SuffixAvailability sa;
                data.Add(sa = new SuffixAvailability()
                {
                    Id = s,
                    Suffix = RandomSuffix.GetSuffix(s),
                    Available = item.AllowedRandomSuffixes != null && item.AllowedRandomSuffixes.Contains(s)
                });
                RandomSuffix.AccumulateStats(sa.Stats = new Stats(), item, s);
            }

            SuffixGrid.ItemsSource = data;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // save new suffixes
            if (item.AllowedRandomSuffixes == null)
            {
                item.AllowedRandomSuffixes = new List<int>();
            }
            item.AllowedRandomSuffixes.Clear();
            foreach (var s in data)
            {
                if (s.Available)
                {
                    item.AllowedRandomSuffixes.Add(s.Id);
                }
            }

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

