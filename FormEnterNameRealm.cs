using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class FormEnterNameRealm : Form
    {
        public FormEnterNameRealm()
        {
            InitializeComponent();
        }

        public string CharacterName { get { return textBoxName.Text; } }
        public string Realm { get { return textBoxRealm.Text; } }
		public Character.CharacterRegion ArmoryRegion { get { return radioButtonUS.Checked ? Character.CharacterRegion.US : Character.CharacterRegion.EU; } }
    }
}