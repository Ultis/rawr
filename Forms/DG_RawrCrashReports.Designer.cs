namespace Rawr
{
    partial class DG_RawrCrashReports
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
            this.TLP_MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.TB_StackTrace = new System.Windows.Forms.TextBox();
            this.LB_StackTrace = new System.Windows.Forms.Label();
            this.LB_ErrorMessage = new System.Windows.Forms.Label();
            this.TB_ErrorMessage = new System.Windows.Forms.TextBox();
            this.BT_OK = new System.Windows.Forms.Button();
            this.BT_CopyToClip = new System.Windows.Forms.Button();
            this.LB_SugFix = new System.Windows.Forms.Label();
            this.LB_Trouble = new System.Windows.Forms.Label();
            this.TB_SugFix = new System.Windows.Forms.TextBox();
            this.LB_Instr = new System.Windows.Forms.Label();
            this.TB_Instr = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.TLP_MainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // TLP_MainLayout
            // 
            this.TLP_MainLayout.ColumnCount = 4;
            this.TLP_MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TLP_MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TLP_MainLayout.Controls.Add(this.TB_StackTrace, 1, 1);
            this.TLP_MainLayout.Controls.Add(this.LB_StackTrace, 1, 0);
            this.TLP_MainLayout.Controls.Add(this.LB_ErrorMessage, 0, 0);
            this.TLP_MainLayout.Controls.Add(this.TB_ErrorMessage, 0, 1);
            this.TLP_MainLayout.Controls.Add(this.BT_OK, 3, 8);
            this.TLP_MainLayout.Controls.Add(this.BT_CopyToClip, 2, 8);
            this.TLP_MainLayout.Controls.Add(this.LB_SugFix, 0, 2);
            this.TLP_MainLayout.Controls.Add(this.LB_Trouble, 0, 4);
            this.TLP_MainLayout.Controls.Add(this.TB_SugFix, 0, 3);
            this.TLP_MainLayout.Controls.Add(this.LB_Instr, 0, 6);
            this.TLP_MainLayout.Controls.Add(this.TB_Instr, 0, 7);
            this.TLP_MainLayout.Controls.Add(this.richTextBox1, 0, 5);
            this.TLP_MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP_MainLayout.Location = new System.Drawing.Point(0, 0);
            this.TLP_MainLayout.Name = "TLP_MainLayout";
            this.TLP_MainLayout.RowCount = 9;
            this.TLP_MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLP_MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.TLP_MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29F));
            this.TLP_MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TLP_MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.TLP_MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP_MainLayout.Size = new System.Drawing.Size(652, 455);
            this.TLP_MainLayout.TabIndex = 0;
            // 
            // TB_StackTrace
            // 
            this.TLP_MainLayout.SetColumnSpan(this.TB_StackTrace, 3);
            this.TB_StackTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TB_StackTrace.Location = new System.Drawing.Point(235, 23);
            this.TB_StackTrace.MaxLength = 65536;
            this.TB_StackTrace.Multiline = true;
            this.TB_StackTrace.Name = "TB_StackTrace";
            this.TB_StackTrace.ReadOnly = true;
            this.TLP_MainLayout.SetRowSpan(this.TB_StackTrace, 7);
            this.TB_StackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_StackTrace.Size = new System.Drawing.Size(414, 399);
            this.TB_StackTrace.TabIndex = 0;
            this.TB_StackTrace.Text = "No Stack Trace";
            // 
            // LB_StackTrace
            // 
            this.LB_StackTrace.AutoSize = true;
            this.TLP_MainLayout.SetColumnSpan(this.LB_StackTrace, 3);
            this.LB_StackTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_StackTrace.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_StackTrace.Location = new System.Drawing.Point(235, 3);
            this.LB_StackTrace.Margin = new System.Windows.Forms.Padding(3);
            this.LB_StackTrace.Name = "LB_StackTrace";
            this.LB_StackTrace.Size = new System.Drawing.Size(414, 14);
            this.LB_StackTrace.TabIndex = 1;
            this.LB_StackTrace.Text = "Stack Trace:";
            // 
            // LB_ErrorMessage
            // 
            this.LB_ErrorMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_ErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_ErrorMessage.Location = new System.Drawing.Point(3, 3);
            this.LB_ErrorMessage.Margin = new System.Windows.Forms.Padding(3);
            this.LB_ErrorMessage.Name = "LB_ErrorMessage";
            this.LB_ErrorMessage.Size = new System.Drawing.Size(226, 14);
            this.LB_ErrorMessage.TabIndex = 2;
            this.LB_ErrorMessage.Text = "Error Message";
            // 
            // TB_ErrorMessage
            // 
            this.TB_ErrorMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TB_ErrorMessage.Location = new System.Drawing.Point(3, 23);
            this.TB_ErrorMessage.Multiline = true;
            this.TB_ErrorMessage.Name = "TB_ErrorMessage";
            this.TB_ErrorMessage.ReadOnly = true;
            this.TB_ErrorMessage.Size = new System.Drawing.Size(226, 115);
            this.TB_ErrorMessage.TabIndex = 3;
            this.TB_ErrorMessage.Text = "No Error Message";
            // 
            // BT_OK
            // 
            this.BT_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BT_OK.Location = new System.Drawing.Point(573, 428);
            this.BT_OK.Name = "BT_OK";
            this.BT_OK.Size = new System.Drawing.Size(75, 23);
            this.BT_OK.TabIndex = 0;
            this.BT_OK.Text = "&OK";
            this.BT_OK.UseVisualStyleBackColor = true;
            // 
            // BT_CopyToClip
            // 
            this.BT_CopyToClip.AutoSize = true;
            this.BT_CopyToClip.Location = new System.Drawing.Point(467, 428);
            this.BT_CopyToClip.Name = "BT_CopyToClip";
            this.BT_CopyToClip.Size = new System.Drawing.Size(100, 23);
            this.BT_CopyToClip.TabIndex = 4;
            this.BT_CopyToClip.Text = "&Copy to Clipboard";
            this.BT_CopyToClip.UseVisualStyleBackColor = true;
            this.BT_CopyToClip.Click += new System.EventHandler(this.BT_CopyToClip_Click);
            // 
            // LB_SugFix
            // 
            this.LB_SugFix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_SugFix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_SugFix.Location = new System.Drawing.Point(3, 144);
            this.LB_SugFix.Margin = new System.Windows.Forms.Padding(3);
            this.LB_SugFix.Name = "LB_SugFix";
            this.LB_SugFix.Size = new System.Drawing.Size(226, 14);
            this.LB_SugFix.TabIndex = 5;
            this.LB_SugFix.Text = "Suggested Fix";
            // 
            // LB_Trouble
            // 
            this.LB_Trouble.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_Trouble.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_Trouble.Location = new System.Drawing.Point(3, 264);
            this.LB_Trouble.Margin = new System.Windows.Forms.Padding(3);
            this.LB_Trouble.Name = "LB_Trouble";
            this.LB_Trouble.Size = new System.Drawing.Size(226, 14);
            this.LB_Trouble.TabIndex = 6;
            this.LB_Trouble.Text = "Troubleshooting";
            // 
            // TB_SugFix
            // 
            this.TB_SugFix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TB_SugFix.Location = new System.Drawing.Point(3, 164);
            this.TB_SugFix.Multiline = true;
            this.TB_SugFix.Name = "TB_SugFix";
            this.TB_SugFix.ReadOnly = true;
            this.TB_SugFix.Size = new System.Drawing.Size(226, 94);
            this.TB_SugFix.TabIndex = 7;
            this.TB_SugFix.Text = "No Suggestions";
            // 
            // LB_Instr
            // 
            this.LB_Instr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_Instr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_Instr.Location = new System.Drawing.Point(3, 353);
            this.LB_Instr.Margin = new System.Windows.Forms.Padding(3);
            this.LB_Instr.Name = "LB_Instr";
            this.LB_Instr.Size = new System.Drawing.Size(226, 14);
            this.LB_Instr.TabIndex = 9;
            this.LB_Instr.Text = "Instructions";
            // 
            // TB_Instr
            // 
            this.TB_Instr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TB_Instr.Location = new System.Drawing.Point(3, 373);
            this.TB_Instr.Multiline = true;
            this.TB_Instr.Name = "TB_Instr";
            this.TB_Instr.ReadOnly = true;
            this.TB_Instr.Size = new System.Drawing.Size(226, 49);
            this.TB_Instr.TabIndex = 10;
            this.TB_Instr.Text = "If you still have this problem after performing the suggested fix, please copy an" +
                "d paste this into an e-mail to WarcraftRawr@gmail.com. Thanks!";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 284);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(226, 63);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "You can also check the Troubleshooting Guide for more steps on getting Rawr to ru" +
                "n smoothly. http://rawr.codeplex.com/wikipage?title=Troubleshooting";
            // 
            // DG_RawrCrashReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 455);
            this.Controls.Add(this.TLP_MainLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DG_RawrCrashReports";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Rawr Crash Reports";
            this.TLP_MainLayout.ResumeLayout(false);
            this.TLP_MainLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TLP_MainLayout;
        private System.Windows.Forms.TextBox TB_StackTrace;
        private System.Windows.Forms.Label LB_StackTrace;
        private System.Windows.Forms.Label LB_ErrorMessage;
        private System.Windows.Forms.TextBox TB_ErrorMessage;
        private System.Windows.Forms.Button BT_OK;
        private System.Windows.Forms.Button BT_CopyToClip;
        private System.Windows.Forms.Label LB_SugFix;
        private System.Windows.Forms.Label LB_Trouble;
        private System.Windows.Forms.TextBox TB_SugFix;
        private System.Windows.Forms.Label LB_Instr;
        private System.Windows.Forms.TextBox TB_Instr;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}