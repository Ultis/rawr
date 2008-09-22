using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Rawr
{
	public partial class TalentPicker : UserControl
	{
		public TalentPicker()
		{
			InitializeComponent();
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}

		private Character _character = null;
		public Character Character
		{
			get { return _character; }
			set
			{
				if (_character != null)
				{
					_character.ClassChanged -= new EventHandler(_character_ClassChanged);
				}
				_character = value;
				if (_character != null)
				{
					_character.ClassChanged += new EventHandler(_character_ClassChanged);
					Talents = _character.CurrentTalents;
				}
			}
		}

		private TalentsBase _talents = null;
		public TalentsBase Talents
		{
			get { return _talents; }
			set
			{
				_talents = value;
				UpdateTrees();
			}
		}

		void _character_ClassChanged(object sender, EventArgs e)
		{
			Talents = _character.CurrentTalents;
		}

		private List<string> _treeNames = new List<string>();
		private void UpdateTrees()
		{
			ClearTalentPickerItemsAndPrerequisiteArrows();
			if (Talents != null)
			{
				_treeNames = new List<string>((string[])Talents.GetType().GetField("TreeNames").GetValue(Talents));
				tabPageTree1.BackgroundImage = ItemIcons.GetTalentTreeBackground(_character.Class, _treeNames[0]);
				tabPageTree2.BackgroundImage = ItemIcons.GetTalentTreeBackground(_character.Class, _treeNames[1]);
				tabPageTree3.BackgroundImage = ItemIcons.GetTalentTreeBackground(_character.Class, _treeNames[2]);

				//_startIndexes = new int[] { 999, 999, 999 };
				foreach (PropertyInfo pi in Talents.GetType().GetProperties())
				{
					TalentDataAttribute[] talentDatas = pi.GetCustomAttributes(typeof(TalentDataAttribute), true) as TalentDataAttribute[];
					if (talentDatas.Length > 0)
					{
						TalentDataAttribute talentData = talentDatas[0];
						//_startIndexes[talentData.Tree] = Math.Min(_startIndexes[talentData.Tree], talentData.Index);
						TalentPickerItem item = new TalentPickerItem(_character.Class, talentData.Name, _treeNames[talentData.Tree], talentData.Description,
							false, talentData.Index, talentData.Prerequisite, talentData.Row, talentData.Column, (int)pi.GetValue(Talents, null), talentData.MaxPoints);
						item.Location = new Point(-45 + (talentData.Column * 63), -57 + (talentData.Row * 65));
						switch (talentData.Tree)
						{
							case 0: tabPageTree1.Controls.Add(item); break;
							case 1: tabPageTree2.Controls.Add(item); break;
							case 2: tabPageTree3.Controls.Add(item); break;
						}
						_talentPickerItems.Add(talentData.Index, item);
						item.CurrentRankChanged += new EventHandler(item_CurrentRankChanged);
					}
				}
				foreach (TalentPickerItem item in _talentPickerItems.Values)
				{
					if (item.Prerequisite != -1)
					{
						TalentPickerArrow arrow = new TalentPickerArrow(item, _talentPickerItems[item.Prerequisite]);
						_prerequisiteArrows.Add(arrow);
						item.Parent.Controls.Add(arrow);
					}
				}
				foreach (TalentPickerArrow arrow in _prerequisiteArrows) arrow.SendToBack();
				UpdateAvailability();
			}
		}

		private void UpdateAvailability()
		{
			foreach (TalentPickerItem item in _talentPickerItems.Values)
			{
				bool meetsPrerequisites = true;
				if (item.Prerequisite != -1)
				{
					TalentPickerItem prerequisiteItem = _talentPickerItems[item.Prerequisite];
					meetsPrerequisites = prerequisiteItem.CurrentRank == prerequisiteItem.MaxRank;
				}
				item.Available = meetsPrerequisites && GetTalentCount(_treeNames.IndexOf(item.TalentTree) + 1, item.Row) >= (item.Row - 1) * 5;
			}
			foreach (TalentPickerArrow arrow in _prerequisiteArrows) arrow.UpdateArrow();
			tabPageTree1.Text = string.Format("{0} ({1})", _treeNames[0], GetTalentCount(1, 100));
			tabPageTree2.Text = string.Format("{0} ({1})", _treeNames[1], GetTalentCount(2, 100));
			tabPageTree3.Text = string.Format("{0} ({1})", _treeNames[2], GetTalentCount(3, 100));
		}

		private int GetTalentCount(int tree, int row)
		{
			int talentCount = 0;
			ControlCollection treeControls = tree == 1 ? tabPageTree1.Controls : (tree == 2 ? tabPageTree2.Controls : tabPageTree3.Controls);
			foreach (Control ctrl in treeControls)
			{
				TalentPickerItem item = ctrl as TalentPickerItem;
				if (item != null)
				{
					if (item.Row < row)
						talentCount += item.CurrentRank;
				}
			}
			return talentCount;
		}

		private Dictionary<int, TalentPickerItem> _talentPickerItems = new Dictionary<int, TalentPickerItem>();
		private List<TalentPickerArrow> _prerequisiteArrows = new List<TalentPickerArrow>();
		private void ClearTalentPickerItemsAndPrerequisiteArrows()
		{
			foreach (TalentPickerItem item in _talentPickerItems.Values)
			{
				item.CurrentRankChanged -= new EventHandler(item_CurrentRankChanged);
				item.Parent.Controls.Remove(item);
				item.Dispose();
			}
			_talentPickerItems.Clear();
			foreach (TalentPickerArrow arrow in _prerequisiteArrows)
			{
				arrow.Parent.Controls.Remove(arrow);
				arrow.Dispose();
			}
			_prerequisiteArrows.Clear();
		}

		void item_CurrentRankChanged(object sender, EventArgs e)
		{
			TalentPickerItem item = sender as TalentPickerItem;
			UpdateAvailability();
			bool cancel = false;
			foreach (TalentPickerItem otherItem in _talentPickerItems.Values)
				if (!otherItem.Available && otherItem.CurrentRank > 0)
				{
					cancel = true;
					break;
				}
			if (cancel)
			{
				item.CurrentRank = Talents.Data[item.Index];
			}
			else
			{
				Talents.Data[item.Index] = item.CurrentRank;
				_character.OnCalculationsInvalidated();
			}
		}
	}
}
