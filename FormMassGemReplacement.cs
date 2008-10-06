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

            if (listOfGemmings.Count == 0)
                listOfGemmings.Add(temp);
          
            cycleGroups();
            adjustform();
        }









        /*as of right now, the form is stubbed out, and the command to use it is 
        //commented out in the formmain.cs. 

        things to do: 
        
         * Shift from hard-coded to dynamic adding of gemmings (use of groupboxes)
         * Add list of groupboxes, so they can be better managed (See: cycleGroups())
         * Find better default image for sockets
         * Add scroll bar when >4 gemmings are used
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
        //this is going to be used with a list of groupboxes that generate the
        //buttons used in the interface.
        private void cycleGroups()
        {

            updateButtons(groupBox1);
            updateButtons(groupBox2);
            updateButtons(groupBox3);
            updateButtons(groupBox4);

           
        }

        //sifts through the itemButtons in a particular groupbox and
        //updates the images in them to reflect their current
        //selectedItem
        private void updateButtons(GroupBox incoming)
        {
            ItemButton iButton;
            int incomingTag = (int) incoming.Tag;
            if (incomingTag <= listOfGemmings.Count)     
            {
                foreach (Control tempControl in (incoming.Controls))
                {
                    if (tempControl is ItemButton)
                    {
                        iButton = (ItemButton)tempControl;

                        switch (iButton.BackColor.ToKnownColor())
                        {

                            case (KnownColor.Red):
                                iButton.SelectedItem = listOfGemmings[incomingTag - 1].gemRed;
                                break;
                            case (KnownColor.Blue):
                                iButton.SelectedItem = listOfGemmings[incomingTag - 1].gemBlue;
                                break;
                            case (KnownColor.Yellow):
                                iButton.SelectedItem = listOfGemmings[incomingTag - 1].gemYellow;
                                break;
                            case (KnownColor.Gray):
                                iButton.SelectedItem = listOfGemmings[incomingTag - 1].gemMeta;
                                break;
                            default:
                                throw new IndexOutOfRangeException("rawr.itembutton.backcolor is used for parsing buttons. make sure they're set correctly");
                                break;

                        }
                        iButton.UpdateSelectedItem();


                    }
                }
            }
        }
        
        private void CancelButton_Click(object sender, EventArgs e)
        {
                    
        }
        
        //adds another slot for gemming
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

        //shifts form height
        private void adjustform()
        {
            if (listOfGemmings.Count == 0)
                this.Height = 164 + 86;
            else
                this.Height = (164 + (86 * (listOfGemmings.Count - 1)));
        }


        //removes the gemming associated with the groupbox the button is in
        
        private void buttonRemoveGemming_Click(object sender, EventArgs e)
        {
            //damn this is ugly...
            int value = int.Parse(((Control)sender).Tag.ToString());
            int shifts = listOfGemmings.Count - value;

            if (shifts != 0)

                for (int i = 0; i < shifts; i++)
                {

                    listOfGemmings[value + (i - 1)].copyGemming(listOfGemmings[value + i]);

                }

            if (listOfGemmings.Count != 1)
                listOfGemmings.RemoveAt(listOfGemmings.Count - 1);

            if (listOfGemmings.Count == 0)
                listOfGemmings.Add(temp);

            cycleGroups();
            adjustform();

        }

        //this needs to be renamed, it's tied to the Leave event, as most of the
        //mouse events were already tied into Rawr.Itembox (can you have 2 things
        //catch an event?)

        private void buttonGem_Click(object sender, EventArgs e)
        {
            ItemButton senderButton = (Rawr.ItemButton)sender;
            int intRowNumber = ((int)senderButton.Tag) - 1;
            KnownColor currColor = senderButton.BackColor.ToKnownColor();

            switch (currColor)
            {

                case (KnownColor.Red):
                    listOfGemmings[intRowNumber].gemRed = senderButton.SelectedItem;
                    break;
                case (KnownColor.Blue):
                    listOfGemmings[intRowNumber].gemBlue = senderButton.SelectedItem;
                    break;
                case (KnownColor.Yellow):
                    listOfGemmings[intRowNumber].gemYellow = senderButton.SelectedItem;
                    break;
                case (KnownColor.Gray):
                    listOfGemmings[intRowNumber].gemMeta = senderButton.SelectedItem;
                    break;
                default:
                    throw new IndexOutOfRangeException("the name used for the gems is used in parsing their colors. Please follow the existing convention.");
                    break;
            }

        }
    }





    public class Gemming
    {
        //commented code is default gemmings, usable for testing

        private Item _gemRed;//= Rawr.Item.LoadFromId(24028, "");
        private Item _gemYellow;// = Rawr.Item.LoadFromId(24028, "");
        private Item _gemBlue;// = Rawr.Item.LoadFromId(24028, "");
        private Item _gemMeta;// = Rawr.Item.LoadFromId(24028, "");

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
            set
            {
                _gemRed = value;
            }
        }
        public Item gemYellow
        {
            get
            {
                return _gemYellow;
            }
            set
            {
                _gemYellow = value;
            }
        }
        public Item gemBlue
        {
            get
            {
                return _gemBlue;
            }
            set
            {
                _gemBlue = value;
            }
        }
        public Item gemMeta
        {
            get
            {
                return _gemMeta;
            }

            set
            {
                _gemMeta = value;
            }
        }





    }

}