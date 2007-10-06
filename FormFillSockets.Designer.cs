namespace Rawr
{
	partial class FormFillSockets
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.radioButtonAll = new System.Windows.Forms.RadioButton();
			this.radioButtonEmpty = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.gemButtonMeta = new Rawr.ItemButton();
			this.gemButtonYellow = new Rawr.ItemButton();
			this.gemButtonBlue = new Rawr.ItemButton();
			this.gemButtonRed = new Rawr.ItemButton();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// radioButtonAll
			// 
			this.radioButtonAll.AutoSize = true;
			this.radioButtonAll.Location = new System.Drawing.Point(12, 39);
			this.radioButtonAll.Name = "radioButtonAll";
			this.radioButtonAll.Size = new System.Drawing.Size(101, 17);
			this.radioButtonAll.TabIndex = 4;
			this.radioButtonAll.TabStop = true;
			this.radioButtonAll.Text = "Fill ALL Sockets";
			this.radioButtonAll.UseVisualStyleBackColor = true;
			// 
			// radioButtonEmpty
			// 
			this.radioButtonEmpty.AutoSize = true;
			this.radioButtonEmpty.Checked = true;
			this.radioButtonEmpty.Location = new System.Drawing.Point(119, 39);
			this.radioButtonEmpty.Name = "radioButtonEmpty";
			this.radioButtonEmpty.Size = new System.Drawing.Size(143, 17);
			this.radioButtonEmpty.TabIndex = 4;
			this.radioButtonEmpty.TabStop = true;
			this.radioButtonEmpty.Text = "Fill Only EMPTY Sockets";
			this.radioButtonEmpty.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Reds";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(85, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Blues";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(161, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Yellows";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(237, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(36, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Metas";
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(154, 156);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 7;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(235, 156);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// gemButtonMeta
			// 
			this.gemButtonMeta.Character = null;
			this.gemButtonMeta.CharacterSlot = Rawr.Character.CharacterSlot.Metas;
			//this.gemButtonMeta.Items = new Rawr.Item[0];
			this.gemButtonMeta.Location = new System.Drawing.Point(240, 80);
			this.gemButtonMeta.Name = "gemButtonMeta";
			this.gemButtonMeta.SelectedItem = null;
			this.gemButtonMeta.SelectedItemId = 0;
			this.gemButtonMeta.Size = new System.Drawing.Size(70, 70);
			this.gemButtonMeta.TabIndex = 3;
			this.gemButtonMeta.UseVisualStyleBackColor = true;
			// 
			// gemButtonYellow
			// 
			this.gemButtonYellow.Character = null;
			this.gemButtonYellow.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
			//this.gemButtonYellow.Items = new Rawr.Item[0];
			this.gemButtonYellow.Location = new System.Drawing.Point(164, 80);
			this.gemButtonYellow.Name = "gemButtonYellow";
			this.gemButtonYellow.SelectedItem = null;
			this.gemButtonYellow.SelectedItemId = 0;
			this.gemButtonYellow.Size = new System.Drawing.Size(70, 70);
			this.gemButtonYellow.TabIndex = 2;
			this.gemButtonYellow.UseVisualStyleBackColor = true;
			this.gemButtonYellow.Click += new System.EventHandler(this.gemButtonYellow_Click);
			// 
			// gemButtonBlue
			// 
			this.gemButtonBlue.Character = null;
			this.gemButtonBlue.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
			//this.gemButtonBlue.Items = new Rawr.Item[0];
			this.gemButtonBlue.Location = new System.Drawing.Point(88, 80);
			this.gemButtonBlue.Name = "gemButtonBlue";
			this.gemButtonBlue.SelectedItem = null;
			this.gemButtonBlue.SelectedItemId = 0;
			this.gemButtonBlue.Size = new System.Drawing.Size(70, 70);
			this.gemButtonBlue.TabIndex = 1;
			this.gemButtonBlue.UseVisualStyleBackColor = true;
			// 
			// gemButtonRed
			// 
			this.gemButtonRed.Character = null;
			this.gemButtonRed.CharacterSlot = Rawr.Character.CharacterSlot.Gems;
			//this.gemButtonRed.Items = new Rawr.Item[0];
			this.gemButtonRed.Location = new System.Drawing.Point(12, 80);
			this.gemButtonRed.Name = "gemButtonRed";
			this.gemButtonRed.SelectedItem = null;
			this.gemButtonRed.SelectedItemId = 0;
			this.gemButtonRed.Size = new System.Drawing.Size(70, 70);
			this.gemButtonRed.TabIndex = 0;
			this.gemButtonRed.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(298, 31);
			this.label5.TabIndex = 5;
			this.label5.Text = "This feature will fill ALL or ONLY EMPTY  sockets in ALL items in Rawr\'s database" +
				".";
			// 
			// FormFillSockets
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(322, 191);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.radioButtonEmpty);
			this.Controls.Add(this.radioButtonAll);
			this.Controls.Add(this.gemButtonMeta);
			this.Controls.Add(this.gemButtonYellow);
			this.Controls.Add(this.gemButtonBlue);
			this.Controls.Add(this.gemButtonRed);
			this.Name = "FormFillSockets";
			this.Text = "Fill Sockets...";
			this.Load += new System.EventHandler(this.FormFillSockets_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ItemButton gemButtonRed;
		private ItemButton gemButtonBlue;
		private ItemButton gemButtonYellow;
		private ItemButton gemButtonMeta;
		private System.Windows.Forms.RadioButton radioButtonAll;
		private System.Windows.Forms.RadioButton radioButtonEmpty;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label label5;
	}
}