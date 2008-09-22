namespace Rawr
{
	partial class TalentPickerItem
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TalentPickerItem));
			this.labelRank = new System.Windows.Forms.Label();
			this.panelOverlay = new System.Windows.Forms.Panel();
			this.panelOverlay.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelRank
			// 
			this.labelRank.BackColor = System.Drawing.Color.Transparent;
			this.labelRank.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
			this.labelRank.ForeColor = System.Drawing.Color.White;
			this.labelRank.Location = new System.Drawing.Point(29, 38);
			this.labelRank.Margin = new System.Windows.Forms.Padding(0);
			this.labelRank.Name = "labelRank";
			this.labelRank.Size = new System.Drawing.Size(28, 15);
			this.labelRank.TabIndex = 1;
			this.labelRank.Text = "0/5";
			this.labelRank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelRank.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelOverlay_MouseClick);
			// 
			// panelOverlay
			// 
			this.panelOverlay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelOverlay.BackgroundImage")));
			this.panelOverlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.panelOverlay.Controls.Add(this.labelRank);
			this.panelOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelOverlay.Location = new System.Drawing.Point(0, 0);
			this.panelOverlay.Name = "panelOverlay";
			this.panelOverlay.Size = new System.Drawing.Size(63, 65);
			this.panelOverlay.TabIndex = 0;
			this.panelOverlay.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelOverlay_MouseClick);
			// 
			// TalentPickerItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.Controls.Add(this.panelOverlay);
			this.Name = "TalentPickerItem";
			this.Size = new System.Drawing.Size(63, 65);
			this.panelOverlay.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelRank;
		private System.Windows.Forms.Panel panelOverlay;
	}
}
