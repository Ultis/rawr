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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TalentPicker));
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabPageTree1 = new System.Windows.Forms.TabPage();
			this.tabPageTree2 = new System.Windows.Forms.TabPage();
			this.tabPageTree3 = new System.Windows.Forms.TabPage();
			this.panelTop = new System.Windows.Forms.Panel();
			this.comboBoxTalentSpec = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControlMain.SuspendLayout();
			this.panelTop.SuspendLayout();
			this.SuspendLayout();
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
			this.tabPageTree1.BackColor = System.Drawing.Color.Black;
			this.tabPageTree1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPageTree1.BackgroundImage")));
			this.tabPageTree1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.tabPageTree1.Location = new System.Drawing.Point(4, 25);
			this.tabPageTree1.Name = "tabPageTree1";
			this.tabPageTree1.Size = new System.Drawing.Size(289, 471);
			this.tabPageTree1.TabIndex = 0;
			this.tabPageTree1.Text = "Balance";
			// 
			// tabPageTree2
			// 
			this.tabPageTree2.AutoScroll = true;
			this.tabPageTree2.BackColor = System.Drawing.Color.Black;
			this.tabPageTree2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPageTree2.BackgroundImage")));
			this.tabPageTree2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.tabPageTree2.Location = new System.Drawing.Point(4, 25);
			this.tabPageTree2.Name = "tabPageTree2";
			this.tabPageTree2.Size = new System.Drawing.Size(289, 471);
			this.tabPageTree2.TabIndex = 1;
			this.tabPageTree2.Text = "Feral Combat";
			// 
			// tabPageTree3
			// 
			this.tabPageTree3.AutoScroll = true;
			this.tabPageTree3.BackColor = System.Drawing.Color.Black;
			this.tabPageTree3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPageTree3.BackgroundImage")));
			this.tabPageTree3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.tabPageTree3.Location = new System.Drawing.Point(4, 25);
			this.tabPageTree3.Name = "tabPageTree3";
			this.tabPageTree3.Size = new System.Drawing.Size(289, 471);
			this.tabPageTree3.TabIndex = 2;
			this.tabPageTree3.Text = "Restoration";
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
			// TalentPicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControlMain);
			this.Controls.Add(this.panelTop);
			this.Name = "TalentPicker";
			this.Size = new System.Drawing.Size(297, 524);
			this.tabControlMain.ResumeLayout(false);
			this.panelTop.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabPageTree1;
		private System.Windows.Forms.TabPage tabPageTree2;
		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.ComboBox comboBoxTalentSpec;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage tabPageTree3;
	}
}
