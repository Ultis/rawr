namespace Rawr.UserControls.Options
{
	partial class GeneralSettings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralSettings));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.checkBoxUseMultithreading = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbGerman = new System.Windows.Forms.RadioButton();
            this.rbRussian = new System.Windows.Forms.RadioButton();
            this.rbSpanish = new System.Windows.Forms.RadioButton();
            this.rbFrench = new System.Windows.Forms.RadioButton();
            this.rbEnglish = new System.Windows.Forms.RadioButton();
            this.chbBuffSource = new System.Windows.Forms.CheckBox();
            this.chbGemNames = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // checkBoxUseMultithreading
            // 
            resources.ApplyResources(this.checkBoxUseMultithreading, "checkBoxUseMultithreading");
            this.checkBoxUseMultithreading.Name = "checkBoxUseMultithreading";
            this.checkBoxUseMultithreading.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // rbGerman
            // 
            resources.ApplyResources(this.rbGerman, "rbGerman");
            this.rbGerman.Name = "rbGerman";
            this.rbGerman.UseVisualStyleBackColor = true;
            this.rbGerman.CheckedChanged += new System.EventHandler(this.rbGerman_CheckedChanged);
            // 
            // rbRussian
            // 
            resources.ApplyResources(this.rbRussian, "rbRussian");
            this.rbRussian.Name = "rbRussian";
            this.rbRussian.UseVisualStyleBackColor = true;
            this.rbRussian.CheckedChanged += new System.EventHandler(this.rbRussian_CheckedChanged);
            // 
            // rbSpanish
            // 
            resources.ApplyResources(this.rbSpanish, "rbSpanish");
            this.rbSpanish.Name = "rbSpanish";
            this.rbSpanish.UseVisualStyleBackColor = true;
            this.rbSpanish.CheckedChanged += new System.EventHandler(this.rbSpanish_CheckedChanged);
            // 
            // rbFrench
            // 
            resources.ApplyResources(this.rbFrench, "rbFrench");
            this.rbFrench.Name = "rbFrench";
            this.rbFrench.UseVisualStyleBackColor = true;
            this.rbFrench.CheckedChanged += new System.EventHandler(this.rbFrench_CheckedChanged);
            // 
            // rbEnglish
            // 
            resources.ApplyResources(this.rbEnglish, "rbEnglish");
            this.rbEnglish.Checked = true;
            this.rbEnglish.Name = "rbEnglish";
            this.rbEnglish.TabStop = true;
            this.rbEnglish.UseVisualStyleBackColor = true;
            this.rbEnglish.CheckedChanged += new System.EventHandler(this.rbEnglish_CheckedChanged);
            // 
            // chbBuffSource
            // 
            resources.ApplyResources(this.chbBuffSource, "chbBuffSource");
            this.chbBuffSource.Name = "chbBuffSource";
            this.chbBuffSource.UseVisualStyleBackColor = true;
            // 
            // chbGemNames
            // 
            resources.ApplyResources(this.chbGemNames, "chbGemNames");
            this.chbGemNames.Name = "chbGemNames";
            this.chbGemNames.UseVisualStyleBackColor = true;
            // 
            // GeneralSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chbGemNames);
            this.Controls.Add(this.chbBuffSource);
            this.Controls.Add(this.rbEnglish);
            this.Controls.Add(this.rbFrench);
            this.Controls.Add(this.rbSpanish);
            this.Controls.Add(this.rbRussian);
            this.Controls.Add(this.rbGerman);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxUseMultithreading);
            this.Name = "GeneralSettings";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.CheckBox checkBoxUseMultithreading;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbEnglish;
        private System.Windows.Forms.RadioButton rbFrench;
        private System.Windows.Forms.RadioButton rbSpanish;
        private System.Windows.Forms.RadioButton rbRussian;
        private System.Windows.Forms.RadioButton rbGerman;
        private System.Windows.Forms.CheckBox chbBuffSource;
        private System.Windows.Forms.CheckBox chbGemNames;


	}
}
