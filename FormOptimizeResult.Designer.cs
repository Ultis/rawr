namespace Rawr
{
    partial class FormOptimizeResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptimizeResult));
            this.buttonKeep = new System.Windows.Forms.Button();
            this.buttonChange = new System.Windows.Forms.Button();
            this.paperDollAfter = new Rawr.PaperDoll();
            this.paperDollBefore = new Rawr.PaperDoll();
            this.SuspendLayout();
            // 
            // buttonKeep
            // 
            this.buttonKeep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonKeep.Location = new System.Drawing.Point(164, 688);
            this.buttonKeep.Name = "buttonKeep";
            this.buttonKeep.Size = new System.Drawing.Size(179, 23);
            this.buttonKeep.TabIndex = 2;
            this.buttonKeep.Text = "Keep Existing Gear Setup";
            this.buttonKeep.UseVisualStyleBackColor = true;
            this.buttonKeep.Click += new System.EventHandler(this.buttonKeep_Click);
            // 
            // buttonChange
            // 
            this.buttonChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonChange.Location = new System.Drawing.Point(682, 688);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(179, 23);
            this.buttonChange.TabIndex = 3;
            this.buttonChange.Text = "Load Optimized Gear Setup";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // paperDollAfter
            // 
            this.paperDollAfter.Cursor = System.Windows.Forms.Cursors.Default;
            this.paperDollAfter.Location = new System.Drawing.Point(519, 4);
            this.paperDollAfter.Name = "paperDollAfter";
            this.paperDollAfter.Size = new System.Drawing.Size(476, 678);
            this.paperDollAfter.TabIndex = 1;
            // 
            // paperDollBefore
            // 
            this.paperDollBefore.Cursor = System.Windows.Forms.Cursors.Default;
            this.paperDollBefore.Location = new System.Drawing.Point(3, 4);
            this.paperDollBefore.Name = "paperDollBefore";
            this.paperDollBefore.Size = new System.Drawing.Size(476, 678);
            this.paperDollBefore.TabIndex = 0;
            // 
            // FormOptimizeResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 719);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.buttonKeep);
            this.Controls.Add(this.paperDollAfter);
            this.Controls.Add(this.paperDollBefore);
            this.Name = "FormOptimizeResult";
            this.Text = "Optimizer Results";
            this.ResumeLayout(false);

        }

        #endregion

        private PaperDoll paperDollBefore;
        private PaperDoll paperDollAfter;
        private System.Windows.Forms.Button buttonKeep;
        private System.Windows.Forms.Button buttonChange;


    }
}