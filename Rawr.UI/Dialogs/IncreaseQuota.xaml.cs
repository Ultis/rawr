using System;
using System.Windows;
using System.Windows.Controls;

namespace Rawr.UI
{
    public partial class IncreaseQuota : ChildWindow
    {
        private int size;

        public IncreaseQuota(int kilobytes)
        {
            size = kilobytes;
            InitializeComponent();

            QuotaLabel.Text = string.Format("Rawr requires at least {0} mb to run. You need to increase the storage quota for this application to run.", Math.Round(size / 1024d, 2));
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = FileUtils.EnsureQuota(size);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

