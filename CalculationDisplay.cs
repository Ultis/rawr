using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class CalculationDisplay : UserControl
	{
		private ToolTip ToolTip = new ToolTip();
		public CalculationDisplay()
		{
			InitializeComponent();
			ToolTip.AutomaticDelay = 100;
			ToolTip.AutoPopDelay = 100000;
			
			BuildControls();
		}

		Dictionary<string, GroupBox> GroupBoxes = new Dictionary<string, GroupBox>();
		Dictionary<string, Label> LabelLabels = new Dictionary<string, Label>();
		Dictionary<string, Label> ValueLabels = new Dictionary<string, Label>();
		private void BuildControls()
		{
			this.Controls.Clear();
			string[] displayLabelConfigurationStrings = Calculations.CharacterDisplayCalculationLabels;

			this.SuspendLayout();
			foreach (string displayLabelConfigurationString in displayLabelConfigurationStrings)
			{
				string[] displayLabelConfigurationSplit = displayLabelConfigurationString.Split(':');
				string group = displayLabelConfigurationSplit[0];
				if (!GroupBoxes.ContainsKey(group))
				{
					GroupBox groupBox = new GroupBox();
					groupBox.Text = group;
					groupBox.Tag = group;
					groupBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
					GroupBoxes.Add(group, groupBox);
					this.Controls.Add(groupBox);
				}

				string name = displayLabelConfigurationSplit[1];
				string[] nameSplit = name.Split('*');
				name = nameSplit[0];
				
				Label labelLabel = new Label();
				labelLabel.Left = 4;
				labelLabel.Text = name + (nameSplit.Length > 1 ? ": *" : ":");
				labelLabel.AutoSize = true;
				GroupBoxes[group].Controls.Add(labelLabel);
				LabelLabels.Add(name, labelLabel);
				if (nameSplit.Length > 1) ToolTip.SetToolTip(labelLabel, nameSplit[1]);
				
				Label labelValue = new Label();
				labelValue.Left = 102;
				labelValue.AutoSize = true;
				GroupBoxes[group].Controls.Add(labelValue);
				ValueLabels.Add(name, labelValue);
			}

			int groupY = 3;
			foreach (GroupBox groupBox in GroupBoxes.Values)
			{
				int labelY = 19;
				foreach (Label label in groupBox.Controls)
				{
					label.Top = labelY;
					if (ValueLabels.ContainsValue(label))
					{
						labelY += 18;
					}
				}
				groupBox.Bounds = new Rectangle(6, groupY, 202, labelY);
				groupY += labelY + 6;
			}
			this.ResumeLayout();
		}

		public void SetCalculations(CharacterCalculationsBase characterCalculations)
		{
			foreach (KeyValuePair<string, string> kvp in characterCalculations.GetCharacterDisplayCalculationValues())
			{
				string[] valueSplit = kvp.Value.Split('*');
				string value = valueSplit[0];
				if (valueSplit.Length > 1)
				{
					ValueLabels[kvp.Key].Text = value + " *";
					ToolTip.SetToolTip(ValueLabels[kvp.Key], valueSplit[1]);
				}
				else
					ValueLabels[kvp.Key].Text = value;
			}
		}
	}
}
