namespace Rawr
{
    partial class FormItemComparison
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
            this.addSetButton = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comparisonGraph = new Rawr.ComparisonGraph();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // addSetButton
            // 
            this.addSetButton.AutoSize = true;
            this.addSetButton.Location = new System.Drawing.Point(145, 339);
            this.addSetButton.Name = "addSetButton";
            this.addSetButton.Size = new System.Drawing.Size(80, 23);
            this.addSetButton.TabIndex = 2;
            this.addSetButton.Text = "Add New Set";
            this.addSetButton.UseVisualStyleBackColor = true;
            this.addSetButton.Click += new System.EventHandler(this.addSetButton_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.LargeChange = 41;
            this.vScrollBar1.Location = new System.Drawing.Point(417, 0);
            this.vScrollBar1.Maximum = 40;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 463);
            this.vScrollBar1.SmallChange = 32;
            this.vScrollBar1.TabIndex = 4;
            this.vScrollBar1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.addSetButton);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 467);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.comparisonGraph);
            this.panel2.Controls.Add(this.vScrollBar1);
            this.panel2.Location = new System.Drawing.Point(263, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(438, 467);
            this.panel2.TabIndex = 6;
            // 
            // comparisonGraph
            // 
            this.comparisonGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comparisonGraph.BackColor = System.Drawing.Color.White;
            this.comparisonGraph.Character = null;
            this.comparisonGraph.CustomRendered = false;
            this.comparisonGraph.CustomRenderedChartName = null;
            this.comparisonGraph.DisplayMode = Rawr.ComparisonGraph.GraphDisplayMode.Subpoints;
            this.comparisonGraph.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comparisonGraph.ItemCalculations = new Rawr.ComparisonCalculationBase[0];
            this.comparisonGraph.Location = new System.Drawing.Point(0, 0);
            this.comparisonGraph.Margin = new System.Windows.Forms.Padding(4);
            this.comparisonGraph.Name = "comparisonGraph";
            this.comparisonGraph.RoundValues = true;
            this.comparisonGraph.ScrollBar = this.vScrollBar1;
            this.comparisonGraph.Size = new System.Drawing.Size(416, 462);
            this.comparisonGraph.SlotMap = null;
            this.comparisonGraph.Sort = Rawr.ComparisonGraph.ComparisonSort.Overall;
            this.comparisonGraph.TabIndex = 3;
            // 
            // FormItemComparison
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 491);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(612, 421);
            this.Name = "FormItemComparison";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Set Comparison";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addSetButton;
        private ComparisonGraph comparisonGraph;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}