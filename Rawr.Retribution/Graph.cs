using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Retribution
{
    public partial class Graph : Form
    {
        private Bitmap bitGraph;
        public Graph(Bitmap bitmap)
        {
            this.bitGraph = bitmap;
            InitializeComponent();
        }

       
        private void Graph_Load(object sender, EventArgs e)
        {
            pictureBoxGraph.Image = bitGraph;
            pictureBoxGraph.SizeMode = PictureBoxSizeMode.Zoom;
        }

    }
}
