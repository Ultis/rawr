using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Rawr.CustomControls;
using Rawr.Forms.Utilities;

namespace Rawr
{
	public partial class BuffSelector : UserControl
	{
        private ExtendedToolTipCheckBox disabledToolTipControl = null;
		//phobos and deimos in the air
		public BuffSelector()
		{
			InitializeComponent();
            if (!this.DesignMode)
			{ 
				BuildControls();
				Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
                ScrollHook.hookRec(this);
			}
		}

        void BuffSelector_MouseMove(object sender, MouseEventArgs e)
        {
            Control child = (sender as Control).GetChildAtPoint(e.Location);
            if (child != null && child.Enabled == false)
            {
                if (disabledToolTipControl != child && child is ExtendedToolTipCheckBox)
                {
                    if (disabledToolTipControl != null)
                    {
                        disabledToolTipControl.ForceHideToolTip();
                    }
                    disabledToolTipControl = child as ExtendedToolTipCheckBox;
                    disabledToolTipControl.ForceShowToolTip();
                }
            }
            else if (disabledToolTipControl != null)
            {
                disabledToolTipControl.ForceHideToolTip();
                disabledToolTipControl = null;
            }
        }

		void Calculations_ModelChanged(object sender, EventArgs e)
		{
			BuildControls();
			LoadBuffsFromCharacter();
		}

		//i want to be free... from desolation and despair
		Dictionary<Buff.BuffCategory, GroupBox> GroupBoxes = new Dictionary<Buff.BuffCategory, GroupBox>();
		Dictionary<Buff, CheckBox> CheckBoxes = new Dictionary<Buff, CheckBox>();
		private void BuildControls()
		{
			this.Controls.Clear();
			this.GroupBoxes.Clear();
			this.CheckBoxes.Clear();
			this.SuspendLayout();
			foreach (Buff.BuffCategory category in Enum.GetValues(typeof(Buff.BuffCategory)))
			{
				GroupBox groupBox = new GroupBox();
				groupBox.Text = Buff.GetBuffCategoryFriendlyName(category);
				groupBox.Tag = category;
				groupBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                groupBox.MouseMove += new MouseEventHandler(BuffSelector_MouseMove);
				groupBox.Dock = DockStyle.Top;
				GroupBoxes.Add(category, groupBox);
				this.Controls.Add(groupBox);
				groupBox.BringToFront();
			}

            List<Buff> buffs = Buff.GetAllRelevantBuffs();
            List<Buff> missingBuffs = new List<Buff>();
            foreach (Buff buff in buffs)
            {
                if (!string.IsNullOrEmpty(buff.RequiredBuff))
                {
                    Buff reqBuff = Buff.GetBuffByName(buff.RequiredBuff);
                    if (!buffs.Contains(reqBuff)) missingBuffs.Add(reqBuff);
                }
            }
            buffs.AddRange(missingBuffs);

			foreach (Buff buff in buffs)
			{
                ExtendedToolTipCheckBox checkBox = new ExtendedToolTipCheckBox();
				checkBox.Tag = buff;
				if (string.IsNullOrEmpty(buff.RequiredBuff)) checkBox.Text = buff.Name;
				checkBox.AutoSize = true;
                checkBox.ToolTipText = buff.Stats.ToString();
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
				groupBox.Bounds = new Rectangle(3, groupY, this.Width, checkY);
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
								checkBox.Location = new Point(this.Width - this.Width / 4 - checkBox.Width / 2, requiredCheckBox.Top + 1);
								break;
							}
					}
				}

				if (hasImprovedBuffs)
				{
					Label labelImproved = new Label();
					labelImproved.Text = "Improved";
					labelImproved.AutoSize = true;
					labelImproved.Location = new Point(this.Width - (4 * labelImproved.Width) / 5, 6);
					groupBox.Controls.Add(labelImproved);
				}
			}
            ScrollHook.hookRec(this);
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
					_character.CalculationsInvalidated -= new EventHandler(Character_ItemsChanged);
				}
				_character = value;
				
				if (Character != null)
				{
					_character.CalculationsInvalidated += new EventHandler(Character_ItemsChanged);
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
			foreach (Buff buff in Character.ActiveBuffs)
			{
                if (buff != null)
                {
                    if (CheckBoxes.ContainsKey(buff))
                        CheckBoxes[buff].Checked = true;
                }
			}
			_loadingBuffsFromCharacter = false;
		}

		void Character_ItemsChanged(object sender, EventArgs e)
		{
			if (this.InvokeRequired)
			{
				InvokeHelper.Invoke(this, "Character_ItemsChanged", new object[] { null, null });
				return;
			}
			LoadBuffsFromCharacter();
		}


		//why can't we see
		private void checkBoxBuff_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loadingBuffsFromCharacter && Character != null)
			{
				UpdateEnabledStates();
				UpdateCharacterBuffs();
				Character.OnCalculationsInvalidated();
			}
		}

		//that when we bleed
		private void UpdateCharacterBuffs()
		{
			List<Buff> activeBuffs = new List<Buff>();
			foreach (CheckBox checkBox in CheckBoxes.Values)
				if (checkBox.Checked && checkBox.Enabled)
					activeBuffs.Add((checkBox.Tag as Buff));
			Character.ActiveBuffs = activeBuffs;
		}

		//we bleed the same
		private void UpdateEnabledStates()
		{
			//If I was in World War II, they'd call me Spitfire!
			List<string> currentConflictNames = new List<string>();
			foreach (CheckBox checkBox in CheckBoxes.Values)
			{
				if (checkBox.Checked)
				{
					Buff buff = checkBox.Tag as Buff;
					currentConflictNames.AddRange(buff.ConflictingBuffs);
				}
			}

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
				if (!checkBox.Checked)
				{
					foreach (string buffName in buff.ConflictingBuffs)
					{
						if (currentConflictNames.Contains(buffName))
						{
							checkBox.Enabled = false;
							break;
						}
					}
				}
			}

		}
	}
}
