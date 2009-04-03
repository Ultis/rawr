using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Rawr
{
	public partial class FormStart : Form
	{
		private int _currentTab = 0;
		private FormMain _formMain;
		private WebRequestWrapper _webRequestWrapper = new WebRequestWrapper();
		public FormStart(FormMain formMain)
		{
			InitializeComponent();
			labelVersionHistory.Text = @"v2.2.0b6
 - PLEASE NOTE: This is a beta of Rawr 2.2. It has not received the same level of testing that we normally put into releases, but we're releasing it in its current form, due to the large number of changes. If you do run into bugs, please post them on our Issue Tracker. Please use the current release version, Rawr 2.1.9, if you encounter showstopping bugs in Rawr 2.2.0b6. Thanks!
 - Start Page! There's now a start page when you launch Rawr, which helps you get started. We're still filling in some of the content on this page, but we're looking for lots of feedback on how you like it, and what else you might want on it.
 - Chart Selection Interface! There is now a more streamlined interface for choosing charts!
 - Chart Exports! There's now an Exports menu above the charts, which allow you to export the data in the current chart to either Clipboard, CSV, or Image.
 - Multilingual Support! In the options, you'll find a locale setting. After setting that, items loaded from wowhead will load their foreign names.
 - Food/Flask/Elixirs Optimization! There are now options to optimize food/flask/elixirs in the Optimizer dialog.
 - On the Relative Stat Values chart, there are 3 additional exports: Copy a Pawn string to Clipboard, View weighted upgrades on Wowhead, and View weighted upgrades on Lootrank.
 - You can now load an individual item from PTR Wowhead by pasting in the PTR Wowhead link to it on the Add Item dialog.
 - There's now a setting in the Options dialog to add the source class/spec to buffs on the Buffs tab.
 - Rawr.Tree: More 3.1 changes. More optimizer additional requirements.
 - Rawr.Cat: More 3.1 changes.
 - Rawr.Bear: More 3.1 changes.
 - Rawr.Mage: Added Mana Source/Usage charts. More 3.1 changes. Improved advanced calculations. Incanter's Absorbtion is now (simply) modeled.
 - Rawr.Moonkin: More 3.1 changes.
 - Rawr.RestoSham: Couple bugfixes.
 - Rawr.Retribution: Added Glyph chart. More 3.1 changes.
 - Rawr.Healadin: Added Glyph chart. More 3.1 changes.
 - Rawr.EnhSham: Coupled bugfixes. More 3.1 changes.
 - Rawr.ProtWarr: More 3.1 changes.
 - Rawr.Warlock: More 3.1 changes. Improvements to talent/spell support.";
			labelVersionHistory.Height = 540;

			this.DoubleBuffered = true;
			_formMain = formMain;

			string version = "v" + System.Reflection.Assembly.GetCallingAssembly().GetName().Version.ToString();
			while (version.EndsWith(".0")) version = version.Substring(0, version.Length - 2);
			labelVersion.Text = version;
			checkBoxShowAtLaunch.Checked = Properties.Recent.Default.ShowStartPage;

			int recentCount = _formMain.ConfigRecentCharacters.Length;
			labelRecentCharacters.Visible = recentCount > 0;
			linkLabelRecentCharacter1.Visible = recentCount > 0;
			if (recentCount > 0)
				linkLabelRecentCharacter1.Text = 
					System.IO.Path.GetFileNameWithoutExtension(_formMain.ConfigRecentCharacters[0]);
			linkLabelRecentCharacter2.Visible = recentCount > 1;
			if (recentCount > 1)
				linkLabelRecentCharacter2.Text =
					System.IO.Path.GetFileNameWithoutExtension(_formMain.ConfigRecentCharacters[1]);
			linkLabelRecentCharacter3.Visible = recentCount > 2;
			if (recentCount > 2)
				linkLabelRecentCharacter3.Text =
					System.IO.Path.GetFileNameWithoutExtension(_formMain.ConfigRecentCharacters[2]);
			linkLabelRecentCharacter4.Visible = recentCount > 3;
			if (recentCount > 3)
				linkLabelRecentCharacter4.Text =
					System.IO.Path.GetFileNameWithoutExtension(_formMain.ConfigRecentCharacters[3]);
			linkLabelRecentCharacter5.Visible = recentCount > 4;
			if (recentCount > 4)
				linkLabelRecentCharacter5.Text =
					System.IO.Path.GetFileNameWithoutExtension(_formMain.ConfigRecentCharacters[4]);

			ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadDidYouKnows));
			ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadKnownIssues));
		}

		private void DownloadDidYouKnows(object ignored)
		{
			string dyk = _webRequestWrapper.GetRandomDidYouKnow();
			if (this.Visible)
				this.Invoke(new WaitCallback(DisplayDidYouKnow), dyk);
		}

		private void DisplayDidYouKnow(object dyk)
		{
			labelDidYouKnow.Text = dyk.ToString();
		}

		private void DownloadKnownIssues(object ignored)
		{
			string ki = _webRequestWrapper.GetKnownIssues();
			if (this.Visible)
				this.Invoke(new WaitCallback(DisplayKnownIssues), ki);
		}

		private void DisplayKnownIssues(object ki)
		{
			labelKnownIssues.Text = ki.ToString();
		}

		private void panelDidYouKnowTab_Click(object sender, EventArgs e)
		{
			if (_currentTab != 0)
			{
				_currentTab = 0;
				panelTabBackground.BackgroundImage = Properties.Resources.RawrStart_1;
				panelDidYouKnow.Visible = true;
				panelDidYouKnow.Cursor = Cursors.Default;
				panelVersionHistory.Visible = panelKnownIssues.Visible = false;
				panelVersionHistory.Cursor = panelKnownIssues.Cursor = Cursors.Hand;
			}
		}

		private void panelVersionHistoryTab_Click(object sender, EventArgs e)
		{
			if (_currentTab != 1)
			{
				_currentTab = 1;
				panelTabBackground.BackgroundImage = Properties.Resources.RawrStart_2;
				panelVersionHistory.Visible = true;
				panelVersionHistory.Cursor = Cursors.Default;
				panelDidYouKnow.Visible = panelKnownIssues.Visible = false;
				panelDidYouKnow.Cursor = panelKnownIssues.Cursor = Cursors.Hand;
			}
		}

		private void panelKnownIssuesTab_Click(object sender, EventArgs e)
		{
			if (_currentTab != 2)
			{
				_currentTab = 2;
				panelTabBackground.BackgroundImage = Properties.Resources.RawrStart_3;
				panelKnownIssues.Visible = true;
				panelKnownIssues.Cursor = Cursors.Default;
				panelDidYouKnow.Visible = panelVersionHistory.Visible = false;
				panelDidYouKnow.Cursor = panelVersionHistory.Cursor = Cursors.Hand;
			}
		}

		private void linkLabelCreateNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (_formMain.NewCharacter()) this.Close();
		}

		private void linkLabelLoadArmory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (_formMain.LoadCharacterFromArmory()) this.Close();
		}

		private void linkLabelOpenSaved_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (_formMain.OpenCharacter()) this.Close();
		}

		private void linkLabelRecentCharacter1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_formMain.LoadSavedCharacter(_formMain.ConfigRecentCharacters[0]);
			this.Close();
		}

		private void linkLabelRecentCharacter2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_formMain.LoadSavedCharacter(_formMain.ConfigRecentCharacters[1]);
			this.Close();
		}

		private void linkLabelRecentCharacter3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_formMain.LoadSavedCharacter(_formMain.ConfigRecentCharacters[2]);
			this.Close();
		}

		private void linkLabelRecentCharacter4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_formMain.LoadSavedCharacter(_formMain.ConfigRecentCharacters[3]);
			this.Close();
		}

		private void linkLabelRecentCharacter5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_formMain.LoadSavedCharacter(_formMain.ConfigRecentCharacters[4]);
			this.Close();
		}

		private void panelTourOfRawr_Click(object sender, EventArgs e) //Tour of Rawr
		{ linkLabelHelp3_LinkClicked(null, null); }

		private void linkLabelHelp1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //Rawr Website
		{ Help.ShowHelp(null, "http://rawr.codeplex.com/"); }

		private void linkLabelHelp2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //Donate
		{ Help.ShowHelp(null, "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=2451163"); }

		private void linkLabelHelp3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //Tour of Rawr
		{ Help.ShowHelp(null, "http://www.youtube.com/watch?v=OjRM5SUoOoQ"); }

		private void linkLabelHelp4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //Gemmings
		{ Help.ShowHelp(null, "http://www.codeplex.com/Rawr/Wiki/View.aspx?title=Gemmings"); }

		private void linkLabelHelp5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //Gear Optimization
		{ Help.ShowHelp(null, "http://www.codeplex.com/Rawr/Wiki/View.aspx?title=GearOptimization"); }

		private void linkLabelHelp6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //Batch Tools
		{ Help.ShowHelp(null, "http://www.codeplex.com/Rawr/Wiki/View.aspx?title=BatchTools"); }

		private void linkLabelHelp7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //Item Filtering
		{ Help.ShowHelp(null, "http://www.codeplex.com/Rawr/Wiki/View.aspx?title=ItemFiltering"); }

		private void checkBoxShowAtLaunch_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Recent.Default.ShowStartPage = checkBoxShowAtLaunch.Checked;
		}
	}
}
