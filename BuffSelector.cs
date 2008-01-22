using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class BuffSelector : UserControl
	{
		//phobos and deimos in the air
		public BuffSelector()
		{
			InitializeComponent();
			this.Controls.Clear();
			BuildControls();
		}

		//i want to be free... from desolation and despair
		Dictionary<Buff.BuffCategory, GroupBox> GroupBoxes = new Dictionary<Buff.BuffCategory, GroupBox>();
		Dictionary<Buff, CheckBox> CheckBoxes = new Dictionary<Buff, CheckBox>();
		private void BuildControls()
		{
			this.SuspendLayout();
			foreach (Buff.BuffCategory category in Enum.GetValues(typeof(Buff.BuffCategory)))
			{
				GroupBox groupBox = new GroupBox();
				groupBox.Text = Buff.GetBuffCategoryFriendlyName(category);
				groupBox.Tag = category;
				groupBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
				GroupBoxes.Add(category, groupBox);
				this.Controls.Add(groupBox);
			}

			foreach (Buff buff in Buff.GetAllRelevantBuffs())
			{
				CheckBox checkBox = new CheckBox();
				checkBox.Tag = buff;
				if (string.IsNullOrEmpty(buff.RequiredBuff)) checkBox.Text = buff.Name;
				checkBox.AutoSize = true;
				checkBox.CheckedChanged += new EventHandler(checkBoxBuff_CheckedChanged);
				GroupBoxes[buff.Category].Controls.Add(checkBox);
				CheckBoxes.Add(buff, checkBox);
			}

			int groupY = 3;
			foreach (GroupBox groupBox in GroupBoxes.Values)
			{
				int checkY = 19;
				foreach (CheckBox checkBox in groupBox.Controls)
				{
					Buff buff = checkBox.Tag as Buff;
					if (string.IsNullOrEmpty(buff.RequiredBuff))
					{
						checkBox.Location = new Point(6, checkY);
						checkY += 23;
					}
				}
				groupBox.Bounds = new Rectangle(3, groupY, 208, checkY);
				groupY += checkY + 6;
				bool hasImprovedBuffs = false;
				foreach (CheckBox checkBox in groupBox.Controls)
				{
					Buff buff = checkBox.Tag as Buff;
					if (!string.IsNullOrEmpty(buff.RequiredBuff))
					{
						hasImprovedBuffs = true;
						foreach (CheckBox requiredCheckBox in groupBox.Controls)
							if (requiredCheckBox.Text == buff.RequiredBuff)
							{
								checkBox.Location = new Point(154, requiredCheckBox.Top + 1);
								break;
							}
					}
				}

				if (hasImprovedBuffs)
				{
					Label labelImproved = new Label();
					labelImproved.Text = "Improved";
					labelImproved.Location = new Point(136, 6);
					groupBox.Controls.Add(labelImproved);
				}
			}
			this.ResumeLayout();
		}

		//when will this loneliness be over
		private bool _loadingBuffsFromCharacter = false;
		private Character _character = null;
		public Character Character
		{
			get { return _character; }
			set
			{
				if (_character != null)
				{
					_character.ItemsChanged -= new EventHandler(Character_ItemsChanged);
				}
				_character = value;
				
				if (Character != null)
				{
					_character.ItemsChanged += new EventHandler(Character_ItemsChanged);
					LoadBuffsFromCharacter();
				}

				UpdateEnabledStates();
			}
		}

		private void LoadBuffsFromCharacter()
		{
			_loadingBuffsFromCharacter = true;
			foreach (CheckBox checkBox in CheckBoxes.Values)
				checkBox.Checked = false;
			foreach (string buffName in Character.ActiveBuffs)
			{
				Buff activeBuff = Buff.GetBuffByName(buffName);
				if (CheckBoxes.ContainsKey(activeBuff))
					CheckBoxes[activeBuff].Checked = true;
			}
			_loadingBuffsFromCharacter = false;
		}

		void Character_ItemsChanged(object sender, EventArgs e)
		{
			LoadBuffsFromCharacter();
		}


		//why can't we see
		private void checkBoxBuff_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loadingBuffsFromCharacter && Character != null)
			{
				UpdateEnabledStates();
				UpdateCharacterBuffs();
				Character.OnItemsChanged();
			}
		}

		//that when we bleed
		private void UpdateCharacterBuffs()
		{
			List<string> activeBuffs = new List<string>();
			foreach (CheckBox checkBox in CheckBoxes.Values)
				if (checkBox.Checked && checkBox.Enabled)
					activeBuffs.Add((checkBox.Tag as Buff).Name);
			Character.ActiveBuffs = activeBuffs;
		}

		//we bleed the same
		private void UpdateEnabledStates()
		{
			//If I was in World War II, they'd call me Spitfire!

			foreach (CheckBox checkBox in CheckBoxes.Values)
			{
				checkBox.Enabled = true;
				Buff buff = checkBox.Tag as Buff;
				if (!string.IsNullOrEmpty(buff.SetName))
				{
					checkBox.Enabled = false;
					continue;
				}
				if (!string.IsNullOrEmpty(buff.RequiredBuff))
				{
					checkBox.Enabled = CheckBoxes[Buff.GetBuffByName(buff.RequiredBuff)].Checked;
					if (!checkBox.Enabled) continue;
				}
				foreach (string buffName in buff.ConflictingBuffs)
				{
					Buff conflictingBuff = Buff.GetBuffByName(buffName);
					if (CheckBoxes.ContainsKey(conflictingBuff))
					{
						checkBox.Enabled = !CheckBoxes[conflictingBuff].Checked;
						if (!checkBox.Enabled) break;
					}
				}
			}

		}
	}
}
