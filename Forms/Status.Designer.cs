namespace Rawr.Forms
{
	partial class Status
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.Cancel = new System.Windows.Forms.Button();
			this.ShowHideDetails = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.Tasks = new System.Windows.Forms.TabPage();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.statusEventArgsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.Errors = new System.Windows.Forms.TabPage();
			this.keyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.doneDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.tabControl1.SuspendLayout();
			this.Tasks.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusEventArgsBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 41);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(308, 23);
			this.progressBar1.TabIndex = 0;
			// 
			// Cancel
			// 
			this.Cancel.Enabled = false;
			this.Cancel.Location = new System.Drawing.Point(331, 12);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(75, 23);
			this.Cancel.TabIndex = 1;
			this.Cancel.Text = "Cancell All";
			this.Cancel.UseVisualStyleBackColor = true;
			// 
			// ShowHideDetails
			// 
			this.ShowHideDetails.Location = new System.Drawing.Point(331, 41);
			this.ShowHideDetails.Name = "ShowHideDetails";
			this.ShowHideDetails.Size = new System.Drawing.Size(75, 23);
			this.ShowHideDetails.TabIndex = 2;
			this.ShowHideDetails.Text = "<< Details";
			this.ShowHideDetails.UseVisualStyleBackColor = true;
			this.ShowHideDetails.Click += new System.EventHandler(this.ShowHideDetails_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(221, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "{0} of {1} Tasks have completed successfully";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(12, 90);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(394, 10);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.Tasks);
			this.tabControl1.Controls.Add(this.Errors);
			this.tabControl1.Location = new System.Drawing.Point(16, 110);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(390, 171);
			this.tabControl1.TabIndex = 6;
			// 
			// Tasks
			// 
			this.Tasks.Controls.Add(this.dataGridView1);
			this.Tasks.Location = new System.Drawing.Point(4, 22);
			this.Tasks.Name = "Tasks";
			this.Tasks.Padding = new System.Windows.Forms.Padding(3);
			this.Tasks.Size = new System.Drawing.Size(382, 145);
			this.Tasks.TabIndex = 0;
			this.Tasks.Text = "Tasks";
			this.Tasks.UseVisualStyleBackColor = true;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.doneDataGridViewCheckBoxColumn});
			this.dataGridView1.DataSource = this.statusEventArgsBindingSource;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dataGridView1.Location = new System.Drawing.Point(3, 3);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dataGridView1.RowTemplate.ReadOnly = true;
			this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new System.Drawing.Size(376, 139);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.VirtualMode = true;
			// 
			// statusEventArgsBindingSource
			// 
			this.statusEventArgsBindingSource.DataSource = typeof(Rawr.StatusEventArgs);
			// 
			// Errors
			// 
			this.Errors.Location = new System.Drawing.Point(4, 22);
			this.Errors.Name = "Errors";
			this.Errors.Padding = new System.Windows.Forms.Padding(3);
			this.Errors.Size = new System.Drawing.Size(382, 145);
			this.Errors.TabIndex = 1;
			this.Errors.Text = "Errors";
			this.Errors.UseVisualStyleBackColor = true;
			// 
			// keyDataGridViewTextBoxColumn
			// 
			this.keyDataGridViewTextBoxColumn.DataPropertyName = "Key";
			this.keyDataGridViewTextBoxColumn.HeaderText = "Key";
			this.keyDataGridViewTextBoxColumn.Name = "keyDataGridViewTextBoxColumn";
			this.keyDataGridViewTextBoxColumn.ReadOnly = true;
			this.keyDataGridViewTextBoxColumn.Width = 142;
			// 
			// descriptionDataGridViewTextBoxColumn
			// 
			this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
			this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
			this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
			this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
			this.descriptionDataGridViewTextBoxColumn.Width = 233;
			// 
			// doneDataGridViewCheckBoxColumn
			// 
			this.doneDataGridViewCheckBoxColumn.DataPropertyName = "Done";
			this.doneDataGridViewCheckBoxColumn.HeaderText = "Done";
			this.doneDataGridViewCheckBoxColumn.Name = "doneDataGridViewCheckBoxColumn";
			this.doneDataGridViewCheckBoxColumn.ReadOnly = true;
			this.doneDataGridViewCheckBoxColumn.Visible = false;
			// 
			// Status
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(418, 293);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ShowHideDetails);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.progressBar1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "Status";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Status";
			this.tabControl1.ResumeLayout(false);
			this.Tasks.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusEventArgsBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.Button ShowHideDetails;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage Tasks;
		private System.Windows.Forms.TabPage Errors;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.BindingSource statusEventArgsBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn doneDataGridViewCheckBoxColumn;
	}
}