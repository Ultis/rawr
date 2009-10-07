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
		string[] _recentCharacters;
		public FormStart(FormMain formMain)
		{
			InitializeComponent();
			labelVersionHistory.Text = @"v2.2.20.0
 - Further improvements to the default filters and item cache.
 - Updated parsing of proc effects from Onyxia items, and a few other stray items.
 - Added a contextual menu item to open an item in the Armory.
 - Improved optimizer availability for items when loading from Character Profiler.
 - Fix for changing item IDs.
 - Fix for available gems in batch optimizations. Also added a comparison to saved version feature for batch optimizations.
 - Rawr.Bear: Updated Predatory Strikes mechanics for 3.2.
 - Rawr.Cat: Updated Predatory Strikes mechanics for 3.2.
 - Rawr.DPSDK: Added armor and bonus armor as relevant stats. Reworked some damage multipliers for improved accuracy, especially for blood. Support for nested special effects.
 - Rawr.DPSWarr: Tons of improvements, including 3.2.2 changes, fixes to maintaining buffs, cooldown usage, stat display, boss handlers, cleave DPS, whirlwind DPS, special effects tweaks, rage generation, armor penetration, rend damage, sword specialization, and tweaks to the rounding of hit.
 - Rawr.Elemental: Improved support for some trinkets. Fix for haste dropping spells below 1sec. Fixed base damage values for Earth and Frost Shock, and Lightning Bolt.
 - Rawr.Enhance: Support for Boss Handler features. Support for nested special effects.
 - Rawr.Mage: Updates to 3.2.2 cycles. Improvement to Arcane Cycles feature. Support for cooldown stacking for all on use spell power and haste effects, and berserking.
 - Rawr.ProtPaladin: More 3.2.2 updates. Better support for special effects, as well as some set bonuses and librams. Improved modeling of Shield of Righteousness. Added damage reduction from Ardent Defender. Fix for slightly off base stats.
 - Rawr.ProtWarr: Fix for Devastate threat, and other minor tweaks.
 - Rawr.RestoSham: Support for more special effects.
 - Rawr.Rogue: Added poison-affecting stats as relevant. Added more set bonuses. Fix for Backstab, Mutilate, poison, and Envenom damage. Fix for several energy costs and energy-related talents. Fix for Murder, Hunger for Blood, and Mace Specialization talents. 
 - Rawr.ShadowPriest: Lots of fixes to haste/rotation interaction. Fixes for spell coefficients and modifiers. Added support for more set bonuses. 
 - Rawr.TankDK: More 3.2.2 updates. Fix for damage reduction buffs. Fix for white damage from offhands. Added option to display ratings in Burst & Reaction Time.
 - Rawr.Tree: More 3.2.2 updates. Basic Val'anyr modeling. Support for a few more set bonuses.
 - Rawr.Warlock: Updated formulae for Empowered Imp, Backlash, and Cataclysm, and physical buff support for pets.";
 			labelVersionHistory.Height = 460;

			this.DoubleBuffered = true;
			_formMain = formMain;

			string version = "v" + System.Reflection.Assembly.GetCallingAssembly().GetName().Version.ToString();
			while (version.EndsWith(".0")) version = version.Substring(0, version.Length - 2);
			labelVersion.Text = version;
			checkBoxShowAtLaunch.Checked = Properties.Recent.Default.ShowStartPage;

			_recentCharacters = _formMain.ConfigRecentCharacters.Clone() as string[];
			Array.Reverse(_recentCharacters);
			int recentCount = _recentCharacters.Length;
			labelRecentCharacters.Visible = recentCount > 0;
			linkLabelRecentCharacter1.Visible = recentCount > 0;
			if (recentCount > 0)
				linkLabelRecentCharacter1.Text =
					System.IO.Path.GetFileNameWithoutExtension(_recentCharacters[0]);
			linkLabelRecentCharacter2.Visible = recentCount > 1;
			if (recentCount > 1)
				linkLabelRecentCharacter2.Text =
					System.IO.Path.GetFileNameWithoutExtension(_recentCharacters[1]);
			linkLabelRecentCharacter3.Visible = recentCount > 2;
			if (recentCount > 2)
				linkLabelRecentCharacter3.Text =
					System.IO.Path.GetFileNameWithoutExtension(_recentCharacters[2]);
			linkLabelRecentCharacter4.Visible = recentCount > 3;
			if (recentCount > 3)
				linkLabelRecentCharacter4.Text =
					System.IO.Path.GetFileNameWithoutExtension(_recentCharacters[3]);
			linkLabelRecentCharacter5.Visible = recentCount > 4;
			if (recentCount > 4)
				linkLabelRecentCharacter5.Text =
					System.IO.Path.GetFileNameWithoutExtension(_recentCharacters[4]);

			ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadDidYouKnows));
			ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadKnownIssues));
		}

		private void DownloadDidYouKnows(object ignored)
		{
			string dyk = _webRequestWrapper.GetRandomDidYouKnow();
			try
			{
				if (this.Visible || this.Disposing || this.IsDisposed)
					this.Invoke(new WaitCallback(DisplayDidYouKnow), dyk);
			}
			catch { }
		}

		private void DisplayDidYouKnow(object dyk)
		{
			if (this.Visible || this.Disposing || this.IsDisposed)
				labelDidYouKnow.Text = dyk.ToString();
		}

		private void DownloadKnownIssues(object ignored)
		{
			string ki = _webRequestWrapper.GetKnownIssues();
			try
			{
				if (this.Visible || this.Disposing || this.IsDisposed)
					this.Invoke(new WaitCallback(DisplayKnownIssues), ki);
			}
			catch { }
		}

		private void DisplayKnownIssues(object ki)
		{
			if (this.Visible || this.Disposing || this.IsDisposed)
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
			_formMain.LoadSavedCharacter(_recentCharacters[0]);
			this.Close();
		}

		private void linkLabelRecentCharacter2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_formMain.LoadSavedCharacter(_recentCharacters[1]);
			this.Close();
		}

		private void linkLabelRecentCharacter3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_formMain.LoadSavedCharacter(_recentCharacters[2]);
			this.Close();
		}

		private void linkLabelRecentCharacter4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_formMain.LoadSavedCharacter(_recentCharacters[3]);
			this.Close();
		}

		private void linkLabelRecentCharacter5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_formMain.LoadSavedCharacter(_recentCharacters[4]);
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
