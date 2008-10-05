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
        private static List<Gemming> listOfGemmings = new List<Gemming>();
        private static List<Gemming> heldoverGemmings;
        
        Gemming temp = new Gemming();

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
            listOfGemmings.Add(temp);
        
        }

       
        


            

        

        /*as of right now, the form is stubbed out, and the command to use it is 
        //commented out in the formmain.cs. 

        things to do: 
        
         * enable  the storage of gems in the gemmings
         * 
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

        private void OKButton_Click(object sender, EventArgs e)
        {
            {
               /* foreach (Item item in ItemCache.AllItems)
                {
                    if (item.Sockets.Color1 != Item.ItemSlot.None && (!form.FillEmptySockets || item.Gem1Id == 0))
                    {
                        switch (item.Sockets.Color1)
                        {
                            case Item.ItemSlot.Red:
                                item.Gem1 = form.GemRed;
                                break;

                            case Item.ItemSlot.Blue:
                                item.Gem1 = form.GemBlue;
                                break;

                            case Item.ItemSlot.Yellow:
                                item.Gem1 = form.GemYellow;
                                break;

                            case Item.ItemSlot.Meta:
                                item.Gem1 = form.GemMeta;
                                break;
                        }
                    }
                    if (item.Sockets.Color2 != Item.ItemSlot.None && (!form.FillEmptySockets || item.Gem2Id == 0))
                    {
                        switch (item.Sockets.Color2)
                        {
                            case Item.ItemSlot.Red:
                                item.Gem2 = form.GemRed;
                                break;

                            case Item.ItemSlot.Blue:
                                item.Gem2 = form.GemBlue;
                                break;

                            case Item.ItemSlot.Yellow:
                                item.Gem2 = form.GemYellow;
                                break;

                            case Item.ItemSlot.Meta:
                                item.Gem2 = form.GemMeta;
                                break;
                        }
                    }
                    if (item.Sockets.Color3 != Item.ItemSlot.None && (!form.FillEmptySockets || item.Gem3Id == 0))
                    {
                        switch (item.Sockets.Color3)
                        {
                            case Item.ItemSlot.Red:
                                item.Gem3 = form.GemRed;
                                break;

                            case Item.ItemSlot.Blue:
                                item.Gem3 = form.GemBlue;
                                break;

                            case Item.ItemSlot.Yellow:
                                item.Gem3 = form.GemYellow;
                                break;

                            case Item.ItemSlot.Meta:
                                item.Gem3 = form.GemMeta;
                                break;
                        }
                 }   
                }*/
            }
            //form.Dispose();


        }

        private void CancelButton_Click(object sender, EventArgs e)
        {


        }

      

        private void buttonAddGemming_Click(object sender, EventArgs e)
        {
            if (listOfGemmings.Count < 4)
            {
                
                listOfGemmings.Add(new Gemming());
                adjustform();
             }

            else
                MessageBox.Show("You may have a maximum of 4 gemmings", "4 Gemmings Allowed");
        }

        private void adjustform()
        {
            this.Height = (164 + (86 * (listOfGemmings.Count - 1)));
        }

		private void buttonRemoveGemming_Click(object sender, EventArgs e)
		{
			//damn this is ugly...
			int value = int.Parse(((Control)sender).Tag.ToString());
			int shifts = listOfGemmings.Count - value;
			
			if (shifts != 0)
			
                for(int i = 0;i<shifts;i++){
				
                    listOfGemmings[value+(i-1)].copyGemming(listOfGemmings[value+i]);
			
                }
			
			if (listOfGemmings.Count != 1)
                listOfGemmings.RemoveAt(listOfGemmings.Count - 1);

			adjustform();
            
        }


    }

   public class Gemming
    {

        private Item _gemRed = Rawr.Item.LoadFromId(24028,"");
        private Item _gemYellow = Rawr.Item.LoadFromId(24028,"");
        private Item _gemBlue = Rawr.Item.LoadFromId(24028, "");
        private Item _gemMeta = Rawr.Item.LoadFromId(24028,"");

        public Gemming()
        {

        }

        public Gemming(Item Red, Item Yellow, Item Blue, Item Meta)
        {
            _gemRed = Red;
            _gemYellow = Yellow;
            _gemBlue = Blue;
            _gemMeta = Meta;
        }

     
        public void copyGemming(Gemming sourceGemming)
        {
            this._gemRed = sourceGemming.gemRed;
            this._gemYellow = sourceGemming.gemYellow;
            this._gemBlue = sourceGemming.gemBlue;
            this._gemMeta = sourceGemming.gemMeta;
        }

        public Item gemRed
        {
            get
            {
                return _gemRed;
            }

        }
        public Item gemYellow
        {
            get
            {
                return _gemYellow;
            }
        }
        public Item gemBlue
        {
            get
            {
                return _gemBlue;
            }
        }
        public Item gemMeta
        {
            get
            {
                return _gemMeta;
            }
        }

        

        

    }
}
