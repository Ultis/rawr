namespace Rawr
{
	partial class TalentPicker
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.panelTop = new System.Windows.Forms.Panel();
            this.comboBoxTalentSpec = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageTree3 = new System.Windows.Forms.TabPage();
            this.talentTree3 = new Rawr.TalentTree();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageTree1 = new System.Windows.Forms.TabPage();
            this.talentTree1 = new Rawr.TalentTree();
            this.tabPageTree2 = new System.Windows.Forms.TabPage();
            this.talentTree2 = new Rawr.TalentTree();
            this.panelTop.SuspendLayout();
            this.tabPageTree3.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageTree1.SuspendLayout();
            this.tabPageTree2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.comboBoxTalentSpec);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panelTop.Size = new System.Drawing.Size(297, 24);
            this.panelTop.TabIndex = 1;
            // 
            // comboBoxTalentSpec
            // 
            this.comboBoxTalentSpec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTalentSpec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTalentSpec.FormattingEnabled = true;
            this.comboBoxTalentSpec.Location = new System.Drawing.Point(71, 0);
            this.comboBoxTalentSpec.Name = "comboBoxTalentSpec";
            this.comboBoxTalentSpec.Size = new System.Drawing.Size(226, 21);
            this.comboBoxTalentSpec.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Talent Spec: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageTree3
            // 
            this.tabPageTree3.AutoScroll = true;
            this.tabPageTree3.BackColor = System.Drawing.Color.Transparent;
            this.tabPageTree3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageTree3.Controls.Add(this.talentTree3);
            this.tabPageTree3.Location = new System.Drawing.Point(4, 25);
            this.tabPageTree3.Name = "tabPageTree3";
            this.tabPageTree3.Size = new System.Drawing.Size(289, 471);
            this.tabPageTree3.TabIndex = 2;
            this.tabPageTree3.Text = "Restoration";
            // 
            // talentTree3
            // 
            this.talentTree3.AutoSize = true;
            this.talentTree3.CharacterClass = Rawr.Character.CharacterClass.Paladin;
            this.talentTree3.Location = new System.Drawing.Point(0, 0);
            this.talentTree3.Name = "talentTree3";
            this.talentTree3.Size = new System.Drawing.Size(269, 717);
            this.talentTree3.TabIndex = 0;
            this.talentTree3.TreeName = "Holy";
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageTree1);
            this.tabControlMain.Controls.Add(this.tabPageTree2);
            this.tabControlMain.Controls.Add(this.tabPageTree3);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.HotTrack = true;
            this.tabControlMain.ItemSize = new System.Drawing.Size(98, 21);
            this.tabControlMain.Location = new System.Drawing.Point(0, 24);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.Padding = new System.Drawing.Point(3, 3);
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(297, 500);
            this.tabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageTree1
            // 
            this.tabPageTree1.AutoScroll = true;
            this.tabPageTree1.BackColor = System.Drawing.Color.Transparent;
            this.tabPageTree1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageTree1.Controls.Add(this.talentTree1);
            this.tabPageTree1.Location = new System.Drawing.Point(4, 25);
            this.tabPageTree1.Name = "tabPageTree1";
            this.tabPageTree1.Size = new System.Drawing.Size(289, 471);
            this.tabPageTree1.TabIndex = 0;
            this.tabPageTree1.Text = "Balance";
            // 
            // talentTree1
            // 
            this.talentTree1.AutoSize = true;
            this.talentTree1.CharacterClass = Rawr.Character.CharacterClass.Paladin;
            this.talentTree1.Location = new System.Drawing.Point(0, 0);
            this.talentTree1.Name = "talentTree1";
            this.talentTree1.Size = new System.Drawing.Size(270, 717);
            this.talentTree1.TabIndex = 0;
            this.talentTree1.TreeName = "Holy";
            // 
            // tabPageTree2
            // 
            this.tabPageTree2.AutoScroll = true;
            this.tabPageTree2.BackColor = System.Drawing.Color.Transparent;
            this.tabPageTree2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPageTree2.Controls.Add(this.talentTree2);
            this.tabPageTree2.Location = new System.Drawing.Point(4, 25);
            this.tabPageTree2.Name = "tabPageTree2";
            this.tabPageTree2.Size = new System.Drawing.Size(289, 471);
            this.tabPageTree2.TabIndex = 1;
            this.tabPageTree2.Text = "Feral Combat";
            // 
            // talentTree2
            // 
            this.talentTree2.AutoSize = true;
            this.talentTree2.CharacterClass = Rawr.Character.CharacterClass.Paladin;
            this.talentTree2.Location = new System.Drawing.Point(0, 0);
            this.talentTree2.Name = "talentTree2";
            this.talentTree2.Size = new System.Drawing.Size(269, 717);
            this.talentTree2.TabIndex = 0;
            this.talentTree2.TreeName = "Holy";
            // 
            // TalentPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.panelTop);
            this.Name = "TalentPicker";
            this.Size = new System.Drawing.Size(297, 524);
            this.panelTop.ResumeLayout(false);
            this.tabPageTree3.ResumeLayout(false);
            this.tabPageTree3.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageTree1.ResumeLayout(false);
            this.tabPageTree1.PerformLayout();
            this.tabPageTree2.ResumeLayout(false);
            this.tabPageTree2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.ComboBox comboBoxTalentSpec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPageTree3;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageTree1;
        private System.Windows.Forms.TabPage tabPageTree2;
        private TalentTree talentTree1;
        private TalentTree talentTree3;
        private TalentTree talentTree2;
	}
}
