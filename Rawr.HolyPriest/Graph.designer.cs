namespace Rawr.HolyPriest
{
    partial class Graph
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
            this.components = new System.ComponentModel.Container();
            this.zg1 = new ZedGraph.ZedGraphControl();
            this.zg2 = new ZedGraph.ZedGraphControl();
            this.chLines = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // zg1
            // 
            this.zg1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zg1.Location = new System.Drawing.Point(12, 114);
            this.zg1.Name = "zg1";
            this.zg1.ScrollGrace = 0;
            this.zg1.ScrollMaxX = 0;
            this.zg1.ScrollMaxY = 0;
            this.zg1.ScrollMaxY2 = 0;
            this.zg1.ScrollMinX = 0;
            this.zg1.ScrollMinY = 0;
            this.zg1.ScrollMinY2 = 0;
            this.zg1.Size = new System.Drawing.Size(708, 364);
            this.zg1.TabIndex = 0;
            // 
            // zg2
            // 
            this.zg2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zg2.Location = new System.Drawing.Point(12, 484);
            this.zg2.Name = "zg2";
            this.zg2.ScrollGrace = 0;
            this.zg2.ScrollMaxX = 0;
            this.zg2.ScrollMaxY = 0;
            this.zg2.ScrollMaxY2 = 0;
            this.zg2.ScrollMinX = 0;
            this.zg2.ScrollMinY = 0;
            this.zg2.ScrollMinY2 = 0;
            this.zg2.Size = new System.Drawing.Size(707, 359);
            this.zg2.TabIndex = 1;
            // 
            // chLines
            // 
            this.chLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chLines.CheckOnClick = true;
            this.chLines.ColumnWidth = 220;
            this.chLines.FormattingEnabled = true;
            this.chLines.Location = new System.Drawing.Point(12, 12);
            this.chLines.MultiColumn = true;
            this.chLines.Name = "chLines";
            this.chLines.Size = new System.Drawing.Size(707, 94);
            this.chLines.TabIndex = 2;
            this.chLines.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chLines_ItemCheck);
            // 
            // Graph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(732, 858);
            this.Controls.Add(this.chLines);
            this.Controls.Add(this.zg2);
            this.Controls.Add(this.zg1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Graph";
            this.Text = "Graph";
            this.Load += new System.EventHandler(this.Graph_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zg1;
        private ZedGraph.ZedGraphControl zg2;
        private System.Windows.Forms.CheckedListBox chLines;

    }
}