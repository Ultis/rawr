using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class FormMassGemReplacement : Form, IFormItemSelectionProvider
    {
        //used to enable selection of relevant gems
        private FormItemSelection _formItemSelection;

        public FormItemSelection FormItemSelection
        {
            get
            {
                if (_formItemSelection == null || _formItemSelection.IsDisposed)
                {
                    _formItemSelection = new FormItemSelection();
                    _formItemSelection.Character = FormMain.Instance.FormItemSelection.Character;
                    _formItemSelection.Items = ItemCache.RelevantItems;
                }
                return _formItemSelection;
            }
        }

        public FormMassGemReplacement()
        {
            InitializeComponent();
        }

        /*as of right now, the form is stubbed out, and the command to use it is 
        //commented out in the formmain.cs. 

        things to do: 
         * 
         * Enable buttons (ok, cancel)
         * Enable Add another gemming button to add another line to the list of gemmings 
         * With each additional set of gemmings, the window should expand to show them (up to 4)
         * Remove button should remove the line of gemmings it's associate with
         * The gemmings should be stored, and should be able to be added or removed dynamically
         * After 4 sets are shown, enable the scrollbar and let the user scroll up/down through them
         * Make the window less ugly (also improve usability)
         * When the OK button is pressed, all items in ItemCache.RelevantItems (I think that's the right list)
         * should be filtered through, and checked for the gemmings listed here. If the gemming is missing, add it
         * If the Delete non-listed gemmings box is checked, remove any instances of items that have
         * gemmings that differ from those listed here. 

        */

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
    }
}
