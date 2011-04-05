using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;

namespace Rawr.UI
{
	public partial class Rawr4RepoSaveDialog : ChildWindow
	{
		public Character Character { get; private set; }
		private Rawr4RepoService _repoService = new Rawr4RepoService();

		public void ShowReload()
		{
			this.Show();
			OKButton_Click(null, null);
			BT_OK.IsEnabled = false;
		}

		public Rawr4RepoSaveDialog()
		{
			InitializeComponent();

#if !SILVERLIGHT
			this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
			this.WindowState = System.Windows.WindowState.Normal;
			this.ResizeMode = System.Windows.ResizeMode.NoResize;
#endif

			_repoService.ProgressChanged += new EventHandler<EventArgs<string>>(_armoryService_ProgressChanged);
			_repoService.SetCharacterCompleted += new EventHandler<EventArgs<string>>(_armoryService_SetCharacterCompleted);

			bool doBadPrepop = false;
			if (Rawr.Properties.RecentSettings.Default.RecentRepoChars != null) {
				int count = Rawr.Properties.RecentSettings.Default.RecentRepoChars.Count;
				if (count > 0) {
					string[] autocomplete = new string[count];
					Rawr.Properties.RecentSettings.Default.RecentRepoChars.CopyTo(autocomplete, 0);
					NameText.IsTextCompletionEnabled = true;
					NameText.ItemsSource = autocomplete;
					NameText.Text = Rawr.Properties.RecentSettings.Default.RecentRepoChars[count - 1];
				} else {
					doBadPrepop = true;
				}
			} else {
				Rawr.Properties.RecentSettings.Default.RecentChars = new List<string>() { };
				doBadPrepop = true;
			}
			if (doBadPrepop) {
				NameText.Text = string.Format("{0} {1} {2} {3}",
					(MainPage.Instance.RegionCombo.SelectedItem as ComboBoxItem).Content as string,
					!String.IsNullOrEmpty(MainPage.Instance.RealmText.Text) ? MainPage.Instance.RealmText.Text : "[Realm]",
					!String.IsNullOrEmpty(MainPage.Instance.NameText.Text) ? MainPage.Instance.NameText.Text : "[CharacterName]",
					MainPage.Instance.Character.CurrentModel);
			}
            NameText.Text = NameText.Text.Replace(".", " ");
			BT_CancelProcessing.IsEnabled = false;
		}

		private bool OKButtonIsEnabled {
			get
			{
				bool ok = true;
				// Validate not Empty
				if (String.IsNullOrEmpty(NameText.Text)) { ok = false; }
				//
				return ok;
			}
		}

		private void NameText_TextChanged(object sender, RoutedEventArgs e)
		{
			BT_OK.IsEnabled = OKButtonIsEnabled;
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			MemoryStream stream = new MemoryStream();
			MainPage.Instance.Character.Save(stream);
			stream.Position = 0;
			StreamReader reader = new StreamReader(stream);

			_repoService.SetCharacterAsync(NameText.Text, PWText.Text, reader.ReadToEnd());

			ProgressBarStatus.IsIndeterminate = true;
			BT_OK.IsEnabled = NameText.IsEnabled = false;
			BT_CancelProcessing.IsEnabled = true;
		}
		private void CancelButton_Click(object sender, RoutedEventArgs e) {
			if (BT_CancelProcessing.IsEnabled) {
				BT_OK.IsEnabled = NameText.IsEnabled = true;
				BT_CancelProcessing.IsEnabled = false;
				ProgressBarStatus.IsIndeterminate = false;
				TextBlockStatus.Text = string.Empty;
			}
			this.DialogResult = false;
		}

		private void BT_CancelProcessing_Click(object sender, RoutedEventArgs e)
		{
			_repoService.CancelAsync();
			BT_OK.IsEnabled = NameText.IsEnabled = true;
			BT_CancelProcessing.IsEnabled = false;
			ProgressBarStatus.IsIndeterminate = false;
			TextBlockStatus.Text = string.Empty;
		}

#if !SILVERLIGHT
		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{

			System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}
#endif

		void _armoryService_ProgressChanged(object sender, EventArgs<string> e)
		{
			string[] progress = e.Value.Split('|');
			TextBlockStatus.Text = progress[0];
			if (progress.Length > 1)
			{
				ToolTipStatus.Visibility = Visibility.Visible;
				ToolTipStatus.Content = progress[1];
			}
			else
				ToolTipStatus.Visibility = Visibility.Collapsed;
		}

		void _armoryService_SetCharacterCompleted(object sender, EventArgs<string> e)
		{
			ProgressBarStatus.IsIndeterminate = true;
			ProgressBarStatus.Value = ProgressBarStatus.Maximum;
			if (e.Value.Contains("WRONG PASSWORD")) {
				MessageBox.Show("You entered an invalid password. Please try again.",
					"Error Saving File", MessageBoxButton.OK);
			}
			this.DialogResult = true;
		}

		public void Save(string identifier)
		{
			NameText.Text = identifier;
			OKButton_Click(null, null);
		}

	}
}

