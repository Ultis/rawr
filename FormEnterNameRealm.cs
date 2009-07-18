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

        public CharacterRegion ArmoryRegion
        {
            get { return (CharacterRegion)Enum.Parse(typeof(CharacterRegion), comboBoxRegion.Text); }
        }

        private void FormEnterNameRealm_Activated(object sender, System.EventArgs e)
        {
            this.textBoxName.Focus();
        }
    }
}