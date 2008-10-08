using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{

       /*things to do: 
        
         
        * Find better default image for sockets
        * Add scroll bar when >4 gemmings are used
        * When the OK button is pressed, all items in ItemCache.RelevantItems (I think that's the right list)
        * should be filtered through, and checked for the gemmings listed here. If the gemming is missing, add it
        * If the Delete non-listed gemmings box is checked, remove any instances of items that have
        * gemmings that differ from those listed here. 

       */
        


    public partial class FormMassGemReplacement : Form, IFormItemSelectionProvider
    {
        
        private FormItemSelection _formItemSelection;
        
        //in future developments, the two lists could be merged. I just
        //wanted to keep the machinery for the class (listofgemmings)
        //apart from the interface (groupboxes) as I worked on them
        //iteratively.

        private static List<Gemming> listOfGemmings = new List<Gemming>();
        private List<GroupBox> groupboxesForGemmings = new List<GroupBox>(); 
        

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

            //this is used when reloading the form
            
            foreach (Gemming gem in listOfGemmings)
            {
                groupboxesForGemmings.Add(createGroupBox());
            }

            //makes everything pretty....

            if (listOfGemmings.Count == 0)
                this.buttonAddGemming_Click(this, null);
            
            cycleGroups();
            adjustform();
        }

        
        private Button removeButton(GroupBox parentBox)
        {
            //creates a button in the groupboxes that is used to 
            //remove the associated gemming

            Button newButton = new Button();
            newButton.Height = 19;
            newButton.Width = 57;
            newButton.Text = "Remove";
            newButton.Tag = (int)parentBox.Tag;
            newButton.BackColor = Color.FromKnownColor(KnownColor.LightGray);
            newButton.Click += new System.EventHandler(this.buttonRemoveGemming_Click);
            newButton.Location = new Point(8 + (63 * parentBox.Controls.Count) +8, 34);

            return newButton; 
        }

        private Rawr.ItemButton gemButton (GroupBox parentBox, KnownColor colorReq){

            //creates an ItemButton for use in groupbox gemmings

            ItemButton newButton = new ItemButton();

            newButton.Height = 57;
            newButton.Width = 57;
            newButton.Tag = (int)parentBox.Tag; 
            newButton.BackColor = Color.FromKnownColor(colorReq);
            newButton.Leave += new System.EventHandler(this.buttonGem_Click);
            newButton.Location = new Point(8+ (63*parentBox.Controls.Count), 17);
            
            if (colorReq.Equals((KnownColor) KnownColor.Gray))
            {
                newButton.CharacterSlot = Character.CharacterSlot.Metas;
                newButton.Text = "Meta";

            }
            
            else
            {
                newButton.CharacterSlot = Character.CharacterSlot.Gems;
                newButton.Text = colorReq.ToString();
            }
            
            return newButton;

    }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            //code here is going to sift through items in Relevantitems
            //and delete/regem/create new items as needed

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
            foreach (GroupBox box in groupboxesForGemmings)
            {
                updateButtons(box);
            }
        }

        
        private void updateButtons(GroupBox incoming)
        {
            //sifts through the itemButtons in a particular groupbox and
            //updates the images in them to reflect the selectedItem 
            //in their associated gemming
        
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
                                iButton.SelectedItem = listOfGemmings[incomingTag].gemRed;
                                break;
                            case (KnownColor.Blue):
                                iButton.SelectedItem = listOfGemmings[incomingTag].gemBlue;
                                break;
                            case (KnownColor.Yellow):
                                iButton.SelectedItem = listOfGemmings[incomingTag].gemYellow;
                                break;
                            case (KnownColor.Gray):
                                iButton.SelectedItem = listOfGemmings[incomingTag].gemMeta;
                                break;
                            default:
                                throw new IndexOutOfRangeException("rawr.itembutton.backcolor is used for parsing buttons. make sure they're set correctly");
                                break;

                        }
                       if (iButton.SelectedItem == null)
                       {
                           if (iButton.BackColor.ToKnownColor().Equals(KnownColor.Gray))
                               iButton.Text = "Meta";
                           else
                               iButton.Text = iButton.BackColor.ToKnownColor().ToString();
                       }                      
                       

                    }
                }
            }
        }
        
        private void CancelButton_Click(object sender, EventArgs e)
        {
                    
        }
        
        //adds another slot for gemming

        private GroupBox createGroupBox()
        {
            GroupBox newGroupBox = new GroupBox();

            newGroupBox.Parent = this;
            newGroupBox.Location = new Point(8, (84 * groupboxesForGemmings.Count) + 43);
            newGroupBox.Width = 337;
            newGroupBox.Height = 80;
            newGroupBox.Tag = (int)(groupboxesForGemmings.Count);
            newGroupBox.Text = "Gemming #" + ((int)newGroupBox.Tag + 1).ToString();            
            newGroupBox.Visible = true;

            newGroupBox.Controls.Add(gemButton(newGroupBox, (KnownColor.Red)));
            newGroupBox.Controls.Add(gemButton(newGroupBox, (KnownColor.Yellow)));
            newGroupBox.Controls.Add(gemButton(newGroupBox, (KnownColor.Blue)));
            newGroupBox.Controls.Add(gemButton(newGroupBox, (KnownColor.Gray)));

            newGroupBox.Controls.Add(removeButton(newGroupBox));

            return newGroupBox;

        }

        private void buttonAddGemming_Click(object sender, EventArgs e)
        {
            //adds a slot for a new gemming, as well as the groupbox interface

            if (listOfGemmings.Count < 8)
            {
                
                groupboxesForGemmings.Add(createGroupBox());
                listOfGemmings.Add(new Gemming());
                
                adjustform();
            }

            else
                MessageBox.Show("You may have a maximum of 8 gemmings", "8 Gemmings Allowed");
        }

        
        private void adjustform()
        {
            //shifts form height

            if (listOfGemmings.Count == 0)
                this.Height = 170;
            else
                this.Height = (170 + (86 * (listOfGemmings.Count - 1)));
        }

        private void buttonRemoveGemming_Click(object sender, EventArgs e)
        {
            //removes the gemming associated with the groupbox the button
            //that was pressed, as well as the last groupbox in groupboxesForGemmings
            //gems are cycled down to fill the appropriate slot.

            //(Is there a linkedlist way of achieving this?)

            //I'd like to note I hate list -> array -> list indexing...

            int value = int.Parse(((Control)sender).Tag.ToString());
            int shifts = listOfGemmings.Count - (value + 1);

            if (shifts != 0)

                for (int i = 0; i < shifts; i++)
                {

                    listOfGemmings[value + (i)].copyGemming(listOfGemmings[value + i + 1]);

                }

            if (listOfGemmings.Count >= 1)
            {
                listOfGemmings.RemoveAt(listOfGemmings.Count - 1);
                groupboxesForGemmings[groupboxesForGemmings.Count - 1].Dispose();
                groupboxesForGemmings.RemoveAt(groupboxesForGemmings.Count - 1);
        }
           
            cycleGroups();
            adjustform();

        }

        
        private void buttonGem_Click(object sender, EventArgs e)
        {
            
            //this needs to be renamed, it's tied to the Leave event, as most of the
            //mouse events were already tied into Rawr.Itembox and I wanted to keep
            //this form reasonably isolated. 

            //It finds the button that sent the event, tracks it back to a 
            //groupbox via a tag, and then parses the button by it's BackColor

            ItemButton senderButton = (Rawr.ItemButton)sender;
            int intRowNumber = (int)senderButton.Tag;
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