using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.UI
{
    public partial class BossPicker : UserControl
    {
        public BossPicker()
        {
            InitializeComponent();
        }
        private Character character;
        public Character Character
        {
            get { return character; }
            set {
                if (character != null) character.ClassChanged -= new EventHandler(character_ClassChanged);
                character = value;
                if (character != null)
                {
                    character.ClassChanged += new EventHandler(character_ClassChanged);
                    character_ClassChanged(this, EventArgs.Empty);
                }
            }
        }
        private void character_ClassChanged(object sender, EventArgs e)
        {
        }
    }
}
