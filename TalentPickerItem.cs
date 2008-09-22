using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class TalentPickerItem : UserControl
	{
		private ToolTip _toolTip = new ToolTip();
		//public TalentPickerItem()
		//{
		//    InitializeComponent();
		//    this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		//}

		void TalentPickerItem_MouseEnter(object sender, EventArgs e)
		{
			string text = string.Format("{0}\r\nRank {1}/{2}\r\n\r\n", TalentName, CurrentRank, MaxRank);
			if (CurrentRank == 0) text += Description[0];
			else if (CurrentRank == MaxRank) text += Description[MaxRank - 1];
			else text += string.Format("{0}\r\n\r\nNext Rank:\r\n{1}", Description[CurrentRank - 1], Description[CurrentRank]);
			
			_toolTip.Show(text, this, 48, 24);
		}

		void TalentPickerItem_MouseLeave(object sender, EventArgs e)
		{
			_toolTip.Hide(this);
		}

		public TalentPickerItem(Character.CharacterClass charClass, string talentName, string talentTree, string[] description, bool available, int index, 
			int prerequisite, int row, int column, int currentRank, int maxRank)
		{
			InitializeComponent();
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
			_characterClass = charClass;
			_talentName = talentName;
			_talentTree = talentTree;
			_description = description;
			_available = available;
			_index = index;
			_prerequisite = prerequisite;
			_row = row;
			_column = column;
			_currentRank = currentRank;
			_maxRank = maxRank;
			UpdateTalentPickerItem();
			this.panelOverlay.MouseLeave += new EventHandler(TalentPickerItem_MouseLeave);
			this.panelOverlay.MouseEnter += new EventHandler(TalentPickerItem_MouseEnter);
		}

		private Character.CharacterClass _characterClass = Character.CharacterClass.Druid;
		public Character.CharacterClass CharacterClass
		{
			get { return _characterClass; }
			set
			{
				_characterClass = value;
				UpdateTalentPickerItem();
			}
		}

		private string _talentName = "";
		public string TalentName
		{
			get { return _talentName; }
			set
			{
				_talentName = value;
				UpdateTalentPickerItem();
			}
		}

		private string _talentTree = "";
		public string TalentTree
		{
			get { return _talentTree; }
			set
			{
				_talentTree = value;
				UpdateTalentPickerItem();
			}
		}

		private bool _available = false;
		public bool Available
		{
			get { return _available; }
			set
			{
				if (_available != value)
				{
					_available = value;
					UpdateTalentPickerItem();
				}
			}
		}

		private string[] _description = new string[] { "" };
		public string[] Description { get { return _description; } set { _description = value; } }
		private int _index = 0;
		public int Index { get { return _index; } set { _index = value; } }
		private int _prerequisite = -1;
		public int Prerequisite { get { return _prerequisite; } set { _prerequisite = value; } }
		private int _row = 0;
		public int Row { get { return _row; } set { _row = value; } }
		private int _column = 0;
		public int Column { get { return _column; } set { _column = value; } }
		public Panel OverlayPanel { get { return panelOverlay; } }

		private int _maxRank = 5;
		public int MaxRank
		{
			get { return _maxRank; }
			set
			{
				_maxRank = value;
				UpdateTalentPickerItem();
			}
		}

		private int _currentRank = 0;
		public int CurrentRank
		{
			get { return _currentRank; }
			set
			{
				if (_currentRank != value)
				{
					_currentRank = value;
					UpdateTalentPickerItem();
					OnCurrentRankChanged();
				}
			}
		}

		public event EventHandler CurrentRankChanged;
		public void OnCurrentRankChanged()
		{
			if (CurrentRankChanged != null)
				CurrentRankChanged(this, EventArgs.Empty);
		}

		private void UpdateTalentPickerItem()
		{
			labelRank.Text = string.Format("{0}/{1}", _currentRank, _maxRank);
			if (!_available)
			{
				SetOverlayImage(Properties.Resources.icon_over_grey);
				labelRank.ForeColor = Color.White;
				SetBackgroundImage(ItemIcons.GetTalentIcon(CharacterClass, TalentTree, TalentName + "-off"));
			}
			else
			{
				if (_currentRank == 0)
				{
					SetOverlayImage(Properties.Resources.icon_over_grey);
					labelRank.ForeColor = Color.Lime;
					SetBackgroundImage(ItemIcons.GetTalentIcon(CharacterClass, TalentTree, TalentName + "-off"));
				}
				else if (_currentRank == _maxRank)
				{
					SetOverlayImage(Properties.Resources.icon_over_yellow);
					labelRank.ForeColor = Color.Yellow;
					SetBackgroundImage(ItemIcons.GetTalentIcon(CharacterClass, TalentTree, TalentName));
				}
				else
				{
					SetOverlayImage(Properties.Resources.icon_over_green);
					labelRank.ForeColor = Color.Lime;
					SetBackgroundImage(ItemIcons.GetTalentIcon(CharacterClass, TalentTree, TalentName));
				}
			}
		}

		private void SetBackgroundImage(Image img) { if (BackgroundImage != img) BackgroundImage = img; }
		private void SetOverlayImage(Image img) { if (panelOverlay.BackgroundImage != img) panelOverlay.BackgroundImage = img; }

		private void panelOverlay_MouseClick(object sender, MouseEventArgs e)
		{
			if (Available)
			{
				if (e.Button == MouseButtons.Right)
				{
					if (Control.ModifierKeys == Keys.Shift)
						CurrentRank = 0;
					else
						CurrentRank = Math.Max(CurrentRank - 1, 0);
				}
				else
				{
					if (Control.ModifierKeys == Keys.Shift)
						CurrentRank = MaxRank;
					else
						CurrentRank = Math.Min(CurrentRank + 1, MaxRank);
				}
				TalentPickerItem_MouseEnter(null, null);
			}
		}
	}
}
