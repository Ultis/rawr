using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.Silverlight
{
	public partial class TalentPicker : UserControl
	{

        //private TalentsBase talents;
        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
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
            Tree1.Talents = Character.CurrentTalents;
            TreeTab1.Header = Tree1.TreeName;
            Tree2.Talents = Character.CurrentTalents;
            TreeTab2.Header = Tree2.TreeName;
            Tree3.Talents = Character.CurrentTalents;
            TreeTab3.Header = Tree3.TreeName;
            Glyph.Character = Character;
        }

		public TalentPicker()
		{
			// Required to initialize variables
            InitializeComponent();
            Tree1.Tree = 0;
            Tree2.Tree = 1;
            Tree3.Tree = 2;

            Tree1.TalentsChanged += new EventHandler(TalentsChanged);
            Tree2.TalentsChanged += new EventHandler(TalentsChanged);
            Tree3.TalentsChanged += new EventHandler(TalentsChanged);
            Glyph.TalentsChanged += new EventHandler(TalentsChanged);
		}

        public void TalentsChanged(object sender, EventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }

        //private SavedTalentSpec currentSpec;
	}
}