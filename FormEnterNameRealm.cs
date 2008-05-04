using System.Windows.Forms;
using System;

namespace Rawr
{
    public partial class FormEnterNameRealm : Form
    {
        public FormEnterNameRealm()
        {
            InitializeComponent();
			comboBoxRegion.SelectedIndex = 0;
        }

        public string CharacterName
        {
            get { return textBoxName.Text; }
        }

        public string Realm
        {
            get { return textBoxRealm.Text; }
        }

        public Character.CharacterRegion ArmoryRegion
        {
            get { return (Character.CharacterRegion)Enum.Parse(typeof(Character.CharacterRegion), comboBoxRegion.Text); }
        }

        private void FormEnterNameRealm_Activated(object sender, System.EventArgs e)
        {
            this.textBoxName.Focus();
        }
    }
}