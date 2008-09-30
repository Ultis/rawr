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
                    talentTree1.CharacterClass = value.Class;
                    talentTree2.CharacterClass = value.Class;
                    talentTree3.CharacterClass = value.Class;
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
			ClearTalentPickerItems();
			if (Talents != null)
			{
                _treeNames = new List<string>((string[])Talents.GetType().GetField("TreeNames").GetValue(Talents));
                talentTree1.CharacterClass = _character.Class;
                talentTree1.TreeName = _treeNames[0];
                talentTree2.CharacterClass = _character.Class;
                talentTree2.TreeName = _treeNames[1];
                talentTree3.CharacterClass = _character.Class;
                talentTree3.TreeName = _treeNames[2];

                TalentTree currentTree;              
				foreach (PropertyInfo pi in Talents.GetType().GetProperties())
				{
					TalentDataAttribute[] talentDatas = pi.GetCustomAttributes(typeof(TalentDataAttribute), true) as TalentDataAttribute[];
					if (talentDatas.Length > 0)
                    {
                        TalentDataAttribute talentData = talentDatas[0];
                        switch (talentData.Tree)
                        {
                            case 0: currentTree = talentTree1; break;
                            case 1: currentTree = talentTree2; break;
                            default: currentTree = talentTree3; break;
                        }
                        TalentItem item = new TalentItem(currentTree, talentData.Name, talentData.Row - 1, talentData.Column - 1, talentData.Index, talentData.Description,
                            (int)pi.GetValue(Talents, null), talentData.MaxPoints, talentData.Prerequisite >= 0 ? _talentPickerItems[talentData.Prerequisite] : null);
                        _talentPickerItems[talentData.Index] = item;
                        currentTree.AddTalent(item);
						//TalentPickerItem item = new TalentPickerItem(_character.Class, talentData.Name, _treeNames[talentData.Tree], talentData.Description,
						//	false, talentData.Index, talentData.Prerequisite, talentData.Row, talentData.Column, (int)pi.GetValue(Talents, null), talentData.MaxPoints);
						//item.Location = new Point(-45 + (talentData.Column * 63), -57 + (talentData.Row * 65));
						item.CurrentRankChanged += new EventHandler(item_CurrentRankChanged);
					}
				}
                talentTree1.Redraw();
                talentTree2.Redraw();
                talentTree3.Redraw();
                item_CurrentRankChanged(null, null);
			}
		}

        private TalentItem[] _talentPickerItems = new TalentItem[100];
        private void ClearTalentPickerItems()
        {
            foreach (TalentItem item in _talentPickerItems)
            {
                if (item != null)
                {
                    item.CurrentRankChanged -= new EventHandler(item_CurrentRankChanged);
                }
            }
            talentTree1.Reset();
            talentTree2.Reset();
            talentTree3.Reset();
        }

        void item_CurrentRankChanged(object sender, EventArgs e)
        {
            TalentItem item = sender as TalentItem;
            tabPageTree1.Text = string.Format("{0} ({1})", _treeNames[0], talentTree1.TotalPoints());
            tabPageTree2.Text = string.Format("{0} ({1})", _treeNames[1], talentTree2.TotalPoints());
            tabPageTree3.Text = string.Format("{0} ({1})", _treeNames[2], talentTree3.TotalPoints());
            if (item != null)
            {
                Talents.Data[item.Index] = item.CurrentRank;
                _character.OnCalculationsInvalidated();
            }
        }
	}
}
