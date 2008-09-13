using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class FormRelevantItemRefinement : Form
    {
        //_relevantItemTypes is used to carry info on which types
        // are utilized by the class period (as listed in instance.RelevantItemTypes)
        static private List<Item.ItemType> _relevantItemTypes = null;

        //_RelevantItemTypes is a static piece of data which determines which item
        // types are being selected for use (have their boxes checked) Right Now. 
        static private List<Item.ItemType> _RelevantItemTypes = null;

        //lists all the checkboxes in the form (used to track the types of items)
        private List<System.Windows.Forms.CheckBox> checkBoxes = new List<CheckBox>();

        public FormRelevantItemRefinement(Object Sender)
        {

            InitializeComponent();
            updateBoxes();

            checkBoxes.Add(this.checkBoxPlateArmor);
            checkBoxes.Add(this.checkBoxMailArmor);
            checkBoxes.Add(this.checkBoxLeatherArmor);
            checkBoxes.Add(this.checkBoxClothArmor);
            
            checkBoxes.Add(this.checkBoxDagger);
            checkBoxes.Add(this.checkBoxFistWeapon);
            checkBoxes.Add(this.checkBoxOneHandedAxe);
            checkBoxes.Add(this.checkBoxOneHandedMace);
            checkBoxes.Add(this.checkBoxOneHandedSword);

            checkBoxes.Add(this.checkBoxStaff);
            checkBoxes.Add(this.checkBoxPolearm);
            checkBoxes.Add(this.checkBoxTwoHandedAxe);
            checkBoxes.Add(this.checkBoxTwoHandedMace);
            checkBoxes.Add(this.checkBoxTwoHandedSword);

            checkBoxes.Add(this.checkBoxBow);
            checkBoxes.Add(this.checkBoxCrossbow);
            checkBoxes.Add(this.checkBoxGun);
            checkBoxes.Add(this.checkBoxThrown);
            checkBoxes.Add(this.checkBoxWand);
            checkBoxes.Add(this.checkBoxShield);
            checkBoxes.Add(this.checkBoxMisc);
        }


        public void resetLists()
        {

            _relevantItemTypes = null;

            uncheckCheckBox();
            disableCheckBox();
        }

        public void updateBoxes()
        {
            if (_relevantItemTypes == null)
            {
              
                _relevantItemTypes = Calculations.Instance.RelevantItemTypes;

                _RelevantItemTypes = Calculations.Instance.RelevantItemTypes;

                // for each type that is listed as useful for the model
                // check and enable the checkboxes listing them.
                foreach (Item.ItemType item in _RelevantItemTypes)
                {

                    foreach (CheckBox box in checkBoxes)
                    {
                        
                            if (item == (Item.ItemType)box.Tag)
                            {
                                box.Checked = true;
                                box.Enabled = true;
                            }
                        
                    }
                   /* if (item == Item.ItemType.Cloth)
                    {
                        checkBoxClothArmor.Enabled = true;
                        checkBoxClothArmor.Checked = true;
                    }
                   
                    */
                }
            }
            else
            {
                // if you're not refreshing things, check those currently
                // used in listing the relevant items. Enable those listed
                // as relevant for the model.

                foreach (CheckBox box in checkBoxes)
                {
                                        
                    foreach (Item.ItemType enableItem in _relevantItemTypes)
                    {
                        if (enableItem == (Item.ItemType)box.Tag)
                            box.Enabled = true;
                    }
                    foreach (Item.ItemType checkItem in _RelevantItemTypes)
                    {
                        if (checkItem == (Item.ItemType)box.Tag)
                            box.Checked = true;
                    }
                }
            }

        }


        //pulls the array of items in, and adds the additional
        //restrictions requested by the user
        public Item[] Refine(Item[] incomingArray)
        {

            List<Item> refinedArray = new List<Item>();

            foreach (Item item in incomingArray)
            {
                if (_RelevantItemTypes.Contains(item.Type))
                    refinedArray.Add(item);
            }
            return refinedArray.ToArray();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            _RelevantItemTypes = new List<Item.ItemType>();

            foreach (Item.ItemType item in _relevantItemTypes)
            {
                foreach (CheckBox box in checkBoxes)
                {
                    if (box.Checked == true)
                    {
                        if (item == (Item.ItemType) box.Tag)
                            _RelevantItemTypes.Add(item);
                    }
                }
              
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.uncheckCheckBox();
            //it's a wash, ignore everything and make sure nothing stays checked
        }

        private void uncheckCheckBox()
        {
            foreach (CheckBox box in checkBoxes)
            {
                box.Checked = false;
            }
        }

        private void disableCheckBox()
        {

            foreach (CheckBox box in checkBoxes)
            {
                box.Enabled = false;
            }


        }
    }
}
        
   