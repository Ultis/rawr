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
        
     */

    

    public partial class FormMassGemReplacement : Form, IFormItemSelectionProvider
    {

        private FormItemSelection _formItemSelection;

        //in future developments, the two lists could possibly be merged. I just
        //wanted to keep the class (gemming) the machinery for the class (listofgemmings)
        //apart from the interface (groupboxes) as I worked on them
        //iteratively.
        private List<Gemming> holdoverGemmings = new List<Gemming>();
        private static List<Gemming> listOfGemmings = new List<Gemming>();
        private List<GroupBox> groupboxesForGemmings = new List<GroupBox>();
        private Character currCharacter = FormMain.Instance.Character;

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


        public FormMassGemReplacement(){
        
            InitializeComponent();

            //this is used when reloading the form

            foreach (Gemming gem in listOfGemmings){
            
                groupboxesForGemmings.Add(createGroupBox());
                holdoverGemmings.Add(gem);
            
            }

            //makes everything pretty....

            if (listOfGemmings.Count == 0)
                this.buttonAddGemming_Click(this, null);

            cycleGroups();
            adjustform();
        }


        private Button removeButton(GroupBox parentBox){
        
            //creates a button in the groupboxes that is used to 
            //remove the associated gemming

            Button newButton = new Button();
            newButton.Height = 19;
            newButton.Width = 57;
            newButton.Text = "Remove";
            newButton.Tag = (int)parentBox.Tag;
            newButton.BackColor = Color.FromKnownColor(KnownColor.LightGray);
            newButton.Click += new System.EventHandler(this.buttonRemoveGemming_Click);
            newButton.Location = new Point(8 + (63 * parentBox.Controls.Count) + 8, 34);

            return newButton;
        }

        private Rawr.ItemButton gemButton(GroupBox parentBox, KnownColor colorReq){
        

            //creates an ItemButton for use in groupbox gemmings

            ItemButton newButton = new ItemButton();

            newButton.Height = 57;
            newButton.Width = 57;
            newButton.Tag = (int)parentBox.Tag;
            newButton.BackColor = Color.FromKnownColor(colorReq);
            newButton.Leave += new System.EventHandler(this.buttonGem_Click);
            newButton.Location = new Point(8 + (63 * parentBox.Controls.Count), 17);

            if (colorReq.Equals((KnownColor)KnownColor.Gray)){
            
                newButton.CharacterSlot = Character.CharacterSlot.Metas;
                newButton.Text = "Meta";

            }

            else{
            
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
             List<Item> listGemmableItems = new List<Item>();
            
            //get rid of duplicate gemming listings
            //it's done here, instead of in-situ so
            //people can create multiple gemmings at once

            deleteDuplicateGemmings();
            
            if (checkBoxDeleteNonlistedGemmings.Checked == false) {
           
                foreach (Item item in ItemCache.RelevantItems){
                
                    //does the item have sockets
                    if ((item.Sockets.Color1 != Item.ItemSlot.None ||
                        item.Sockets.Color2 != Item.ItemSlot.None ||
                        item.Sockets.Color3 != Item.ItemSlot.None)){
                    
                        //assembles a list of only one instance of each gemmable item
                        //the first item is by definition not a copy, and needs to be gemmed
                        if (listGemmableItems.Count == 0){
                        
                            listGemmableItems.Add(item);
                            foreach (Gemming set in listOfGemmings){
                            
                                string gemmedId = (item.Id.ToString()) + (set.gemmingID(item));
                                Item thingHolder = ItemCache.FindItemById(gemmedId, true, true);
                           
                            }

                            continue;
                        }

                        //if it is a copy don't add it
                        if (item.Id == (listGemmableItems[(listGemmableItems.Count) - 1]).Id)
                               
                            continue;
                       

                            //for all subsequent socketed items, that are not copies, socket
                        //them with all apropriate (non-repeating) gemmings
                        else{
                        
                            listGemmableItems.Add(item);
                            foreach (Gemming set in listOfGemmings){
                            
                                string gemmedId = (item.Id.ToString()) + (set.gemmingID(item));
                                Item thingHolder = ItemCache.FindItemById(gemmedId, true, true);
                           
                            }
                        }
                    }
                }
            }
            else{

                foreach (Item item in ItemCache.RelevantItems){
                
                    //if the list of gemmable items has things in it, and the item just brought in has a different ID than
                    //the last item you added to the list of gemmable items.

                    //OR

                    //if the item has the same ID as the last item in RelevantItems, and it has sockets

                    if (((listGemmableItems.Count != 0) && (item.Id != (listGemmableItems[(listGemmableItems.Count) - 1]).Id)) ||
                        ((item.GemmedId == ItemCache.RelevantItems[ItemCache.RelevantItems.Length - 1].GemmedId) &&
                        (item.Sockets.Color1 != Item.ItemSlot.None ||
                         item.Sockets.Color2 != Item.ItemSlot.None ||
                         item.Sockets.Color3 != Item.ItemSlot.None))) {
                   

                        //if the last item in the list is unique, we need to process it properly
                        if (item.GemmedId == ItemCache.RelevantItems[ItemCache.RelevantItems.Length - 1].GemmedId){
                        
                            listGemmableItems.Add(item);

                            foreach (Gemming set in listOfGemmings)
                                ItemCache.AddItem(set.gemIt(item), true, false);
                        }

                        //delete all socketed items
                        foreach (Item exItem in listGemmableItems){

                            if (currCharacter.GetItemAvailability(exItem) != Character.ItemAvailability.NotAvailabe)
                            
                                continue;
                            
                            else if (currCharacter.IsEquipped(exItem))
                                continue;

                            else
                                ItemCache.DeleteItem(exItem);
                    }


                        //add items that have the gemming patterns we want
                        //using the first item we added to the list (which matches all the rest)
                        //as a template for the gemmings
                        foreach (Gemming set in listOfGemmings)
                            ItemCache.AddItem(set.gemIt(listGemmableItems[0]), true, true);

                        //if the last item added to the list wasn't the last item in relevantItems
                        if (item.GemmedId != ItemCache.RelevantItems[ItemCache.RelevantItems.Length - 1].GemmedId)

                            //reset the list for the next batch
                            listGemmableItems = new List<Item>();


                    }

                        //does the item have sockets
                     if (item.Sockets.Color1 != Item.ItemSlot.None ||
                             item.Sockets.Color2 != Item.ItemSlot.None ||
                             item.Sockets.Color3 != Item.ItemSlot.None){
                    
                        listGemmableItems.Add(item);

                    }
                }
            }
        }




        //this is going to be used with a list of groupboxes that generate the
        //buttons used in the interface.
        private void cycleGroups(){
        
            foreach (GroupBox box in groupboxesForGemmings)
                updateButtons(box);
            
        }


        private void updateButtons(GroupBox incoming)
        {
            //sifts through the itemButtons in a particular groupbox and
            //updates the images in them to reflect the selectedItem 
            //in their associated gemming

            ItemButton iButton;
            int incomingTag = (int)incoming.Tag;
            
            if (incomingTag <= listOfGemmings.Count){
            
                foreach (Control tempControl in (incoming.Controls)) {
               
                    if (tempControl is ItemButton){
                    
                        iButton = (ItemButton)tempControl;

                        switch (iButton.BackColor.ToKnownColor()){
                        

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
                        
                        if (iButton.SelectedItem == null){
                        
                            if (iButton.BackColor.ToKnownColor().Equals(KnownColor.Gray))
                                iButton.Text = "Meta";
                            else
                                iButton.Text = iButton.BackColor.ToKnownColor().ToString();
                        }
                    }
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e){
        
            listOfGemmings = new List<Gemming>();
            
            foreach (Gemming gemming in holdoverGemmings)
                listOfGemmings.Add(gemming);
            
        }

        

        private GroupBox createGroupBox(){
        
            //adds groupbox and associated buttons
            
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

        private void buttonAddGemming_Click(object sender, EventArgs e) {
       
            //adds a slot for a new gemming, as well as the groupbox interface

            if (listOfGemmings.Count < 10){
            
                groupboxesForGemmings.Add(createGroupBox());
                listOfGemmings.Add(new Gemming());
                adjustform();
           
            }

            else
                MessageBox.Show("You may have a maximum of 10 gemmings", "10 Gemmings Allowed");
        }

        private void deleteDuplicateGemmings()
        {
            List<Gemming> gemList = new List<Gemming>();
            List<int> hash = new List<int>();
            foreach (Gemming gem in listOfGemmings){
            
                if (!hash.Contains(gem.GetHashCode())){
                   
                    gemList.Add(gem);
                    hash.Add(gem.GetHashCode());
               
                }

                else{
               
                    groupboxesForGemmings[groupboxesForGemmings.Count - 1].Dispose();
                    groupboxesForGemmings.RemoveAt(groupboxesForGemmings.Count - 1);
              
                }

            }

            listOfGemmings.Clear();
            listOfGemmings = new List<Gemming>();

            foreach (Gemming popGem in gemList)
                listOfGemmings.Add(popGem);
            
            cycleGroups();
        }

        private void adjustform(){
        
            //shifts form height

            if (listOfGemmings.Count == 0)
                this.Height = 170;
            else
                this.Height = (170 + (86 * (listOfGemmings.Count - 1)));
        }

        private void buttonRemoveGemming_Click(object sender, EventArgs e){
        
            //removes the gemming associated with the groupbox the button
            //that was pressed, as well as the last groupbox in groupboxesForGemmings
            //gems are cycled down to fill the appropriate slot.

            //I'd like to note I hate list -> array -> list indexing...

            int value = int.Parse(((Control)sender).Tag.ToString());
            int shifts = listOfGemmings.Count - (value + 1);

            if (shifts != 0)

                for (int i = 0; i < shifts; i++)
                    listOfGemmings[value + (i)].copyGemming(listOfGemmings[value + i + 1]);
                            
            if (listOfGemmings.Count >= 1){
            
                listOfGemmings.RemoveAt(listOfGemmings.Count - 1);
                groupboxesForGemmings[groupboxesForGemmings.Count - 1].Dispose();
                groupboxesForGemmings.RemoveAt(groupboxesForGemmings.Count - 1);
            }

            cycleGroups();
            adjustform();

        }


        private void buttonGem_Click(object sender, EventArgs e){
        

            //this needs to be renamed, it's tied to the Leave event, as most of the
            //mouse events were already tied into Rawr.Itembox and I wanted to keep
            //this form reasonably isolated. 

            //It finds the button that sent the event, tracks it back to a 
            //groupbox via a tag, and then parses the button by it's BackColor

            ItemButton senderButton = (Rawr.ItemButton)sender;
            int intRowNumber = (int)senderButton.Tag;
            KnownColor currColor = senderButton.BackColor.ToKnownColor();

            switch (currColor){
            
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
       
        private Item _gemRed;
        private Item _gemYellow;
        private Item _gemBlue;
        private Item _gemMeta;
        

        public Gemming()
        {

        }

        private bool allNull() {
       
            if ((_gemRed == null) && (_gemYellow == null) && (_gemBlue == null) && (gemMeta == null))
                return true;
            else
                return false;
            }

        public override int GetHashCode()
        {
            int a = 0, b = 0, c = 0, d = 0;

            if (gemRed != null)
                a = gemRed.Id;
           
            if (gemYellow != null)
                b = 2 * gemYellow.Id;
           
            if (gemBlue != null)
                c = 3 * gemBlue.Id;
            
            if (gemMeta != null)
                d = 4 * gemMeta.Id;

            return (a + b + c + d);
        }

        public Gemming(Item Red, Item Yellow, Item Blue, Item Meta){
        
            _gemRed = Red;
            _gemYellow = Yellow;
            _gemBlue = Blue;
            _gemMeta = Meta;
        }

        public Item gemIt(Item itemToBeGemmed){
        
            int[] gemID = { 0, 0, 0 };

            if (itemToBeGemmed.Sockets.Color1 != Item.ItemSlot.None)
            {

                switch (itemToBeGemmed.Sockets.Color1)
                {

                    case Item.ItemSlot.Red:
                        if (gemRed == null)
                            gemID[0] = -1;
                        else
                            gemID[0] = gemRed.Id;
                        break;

                    case Item.ItemSlot.Blue:
                        if (gemBlue == null)
                            gemID[0] = -1;
                        else
                            gemID[0] = gemBlue.Id;
                        break;

                    case Item.ItemSlot.Yellow:
                        if (gemYellow == null)
                            gemID[0] = -1;
                        else
                            gemID[0] = gemYellow.Id;
                        break;

                    case Item.ItemSlot.Meta:
                        if (gemMeta == null)
                            gemID[0] = -1;
                        else
                            gemID[0] = gemMeta.Id;
                        break;
                }
            }
            else
                gemID[0] = -1;
            
            if (itemToBeGemmed.Sockets.Color2 != Item.ItemSlot.None) {
           
                switch (itemToBeGemmed.Sockets.Color2){
                
                    case Item.ItemSlot.Red:
                        if (gemRed == null)
                            gemID[1] = -1;
                        else
                            gemID[1] = gemRed.Id;
                        break;

                    case Item.ItemSlot.Blue:
                        if (gemBlue == null)
                            gemID[1] = -1;
                        else
                            gemID[1] = gemBlue.Id;
                        break;

                    case Item.ItemSlot.Yellow:
                        if (gemYellow == null)
                            gemID[1] = -1;
                        else
                            gemID[1] = gemYellow.Id;
                        break;

                    case Item.ItemSlot.Meta:
                        if (gemMeta == null)
                            gemID[1] = -1;
                        else
                            gemID[1] = gemMeta.Id;
                        break;
                }

               
            }
             else
                gemID[1] = -1;
                if (itemToBeGemmed.Sockets.Color3 != Item.ItemSlot.None){
            
                switch (itemToBeGemmed.Sockets.Color3){
                
                    case Item.ItemSlot.Red:
                        if (gemRed == null)
                            gemID[2] = -1;
                        else
                            gemID[2] = gemRed.Id;
                        break;

                    case Item.ItemSlot.Blue:
                        if (gemBlue == null)
                            gemID[2] = -1;
                        else
                            gemID[2] = gemBlue.Id;
                        break;

                    case Item.ItemSlot.Yellow:
                        if (gemYellow == null)
                            gemID[2] = -1;
                        else
                            gemID[2] = gemYellow.Id;
                        break;

                    case Item.ItemSlot.Meta:
                        if (gemMeta == null)
                            gemID[2] = -1;
                        else
                            gemID[2] = gemMeta.Id;
                        break;
                }

                
            }
else
                gemID[2]=-1;

                if ((gemID[0] == -1 && gemID[1] == -1 && gemID[2] == -1)&&(!allNull()))
                    return null;
                else
                {
                    if (gemID[0] == -1) gemID[0]= 0;
                    if (gemID[1] == -1) gemID[1] = 0;
                    if (gemID[2] == -1) gemID[2] =  0;
                }
            Item copy = new Item(itemToBeGemmed.Name, itemToBeGemmed.Quality, itemToBeGemmed.Type, itemToBeGemmed.Id, itemToBeGemmed.IconPath, itemToBeGemmed.Slot, itemToBeGemmed.SetName, 
                                 itemToBeGemmed.Unique, itemToBeGemmed.Stats.Clone(), itemToBeGemmed.Sockets.Clone(), gemID[0], gemID[1], gemID[2],
                                 itemToBeGemmed.MinDamage, itemToBeGemmed.MaxDamage, itemToBeGemmed.DamageType, itemToBeGemmed.Speed, itemToBeGemmed.RequiredClasses);
            
            return copy;
      
        }

        public void copyGemming(Gemming sourceGemming){
        
            this._gemRed = sourceGemming.gemRed;
            this._gemYellow = sourceGemming.gemYellow;
            this._gemBlue = sourceGemming.gemBlue;
            this._gemMeta = sourceGemming.gemMeta;
        }

        public string gemmingID(Item item){
        
            string gem1ID = "0";
            string gem2ID = "0";
            string gem3ID = "0";

            if (item.Sockets.Color1 != Item.ItemSlot.None){
            
                switch (item.Sockets.Color1){
                
                    case Item.ItemSlot.Red:
                        if (this.gemRed == null)
                            gem1ID = "0";
                        else
                            gem1ID = this.gemRed.Id.ToString();
                        break;

                    case Item.ItemSlot.Blue:
                        if (this.gemBlue == null)
                            gem1ID = "0";
                        else
                            gem1ID = this.gemBlue.Id.ToString();
                        break;

                    case Item.ItemSlot.Yellow:
                        if (this.gemYellow == null)
                            gem1ID = "0";
                        else
                            gem1ID = this.gemYellow.Id.ToString();
                        break;

                    case Item.ItemSlot.Meta:
                        if (this.gemMeta == null)
                            gem1ID = "0";
                        else
                            gem1ID = this.gemMeta.Id.ToString();
                        break;
                }
            }
            if (item.Sockets.Color2 != Item.ItemSlot.None){
            
                switch (item.Sockets.Color2){
                
                    case Item.ItemSlot.Red:
                        if (this.gemRed == null)
                            gem2ID = "0";
                        else
                            gem2ID = this.gemRed.Id.ToString();
                        break;

                    case Item.ItemSlot.Blue:
                        if (this.gemBlue == null)
                            gem2ID = "0";
                        else
                            gem2ID = this.gemBlue.Id.ToString();
                        break;

                    case Item.ItemSlot.Yellow:
                        if (this.gemYellow == null)
                            gem2ID = "0";
                        else
                            gem2ID = this.gemYellow.Id.ToString();
                        break;

                    case Item.ItemSlot.Meta:
                        if (this.gemMeta == null)
                            gem2ID = "0";
                        else
                            gem2ID = this.gemMeta.Id.ToString();
                        break;
                }
            }
            if (item.Sockets.Color3 != Item.ItemSlot.None){
            
                switch (item.Sockets.Color3){
                
                    case Item.ItemSlot.Red:
                        if (this.gemRed == null)
                            gem3ID = "0";
                        else
                            gem3ID = this.gemRed.Id.ToString();
                        break;

                    case Item.ItemSlot.Blue:
                        if (this.gemBlue == null)
                            gem3ID = "0";
                        else
                            gem3ID = this.gemBlue.Id.ToString();
                        break;

                    case Item.ItemSlot.Yellow:
                        if (this.gemYellow == null)
                            gem3ID = "0";
                        else
                            gem3ID = this.gemYellow.Id.ToString();
                        break;

                    case Item.ItemSlot.Meta:
                        if (this.gemMeta == null)
                            gem3ID = "0";
                        else
                            gem3ID = this.gemMeta.Id.ToString();
                        break;
                }
            }

            return "." + gem1ID + "." + gem2ID + "." + gem3ID;

        }

        public Item gemRed {
       
            get
            {
                return _gemRed;
            }
            set
            {
                _gemRed = value;
            }
        }
        
        public Item gemYellow{
        
            get
            {
                return _gemYellow;
            }
            set
            {
                _gemYellow = value;
            }
        }
        
        public Item gemBlue{
        
            get
            {
                return _gemBlue;
            }
            set
            {
                _gemBlue = value;
            }
        }
        
        public Item gemMeta{
        
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