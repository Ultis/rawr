using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class PaperDoll : UserControl
    {
        private Character _character = null;
        private Color _defaultColor = Color.White;
        private bool _loadingCharacter = false;

        public PaperDoll()
        {
            InitializeComponent();
            _character = new Character();
            itemButtonHead.SelectedItem = itemButtonWrist.SelectedItem =
            itemButtonBack.SelectedItem = itemButtonChest.SelectedItem = itemButtonFeet.SelectedItem =
            itemButtonFinger1.SelectedItem = itemButtonFinger2.SelectedItem = itemButtonHands.SelectedItem =
            itemButtonHead.SelectedItem = itemButtonRanged.SelectedItem = itemButtonLegs.SelectedItem =
            itemButtonShoulders.SelectedItem = itemButtonMainHand.SelectedItem = itemButtonOffHand.SelectedItem =
            itemButtonProjectile.SelectedItemInstance = itemButtonProjectileBag.SelectedItemInstance =
            itemButtonNeck.SelectedItemInstance = itemButtonShirt.SelectedItemInstance = itemButtonWaist.SelectedItemInstance =
            itemButtonTabard.SelectedItemInstance = itemButtonTrinket1.SelectedItemInstance = itemButtonTrinket2.SelectedItemInstance = new ItemInstance();
        }

        #region ItemSlots
        public ItemInstance Head
        {
            get { return Character.Head ?? new ItemInstance(); }
            set { itemButtonHead.SelectedItem = value; itemButtonHead.UpdateSelectedItem(); }
        }

        public ItemInstance Neck
        {
            get { return Character.Neck ?? new ItemInstance(); }
            set { itemButtonNeck.SelectedItemInstance = value; itemButtonNeck.UpdateSelectedItem(); }
        }

        public ItemInstance Shoulders
        {
            get { return Character.Shoulders ?? new ItemInstance(); }
            set { itemButtonShoulders.SelectedItem = value; itemButtonShoulders.UpdateSelectedItem(); }
        }

        public ItemInstance Back
        {
            get { return Character.Back ?? new ItemInstance(); }
            set { itemButtonBack.SelectedItem = value; itemButtonBack.UpdateSelectedItem(); }
        }

        public ItemInstance Chest
        {
            get { return Character.Chest ?? new ItemInstance(); }
            set { itemButtonChest.SelectedItem = value; itemButtonChest.UpdateSelectedItem(); }
        }

        public ItemInstance Shirt
        {
            get { return Character.Shirt ?? new ItemInstance(); }
            set { itemButtonShirt.SelectedItemInstance = value; itemButtonShirt.UpdateSelectedItem(); }
        }

        public ItemInstance Tabard
        {
            get { return Character.Tabard ?? new ItemInstance(); }
            set { itemButtonTabard.SelectedItemInstance = value; itemButtonTabard.UpdateSelectedItem(); }
        }

        public ItemInstance Wrist
        {
            get { return Character.Wrist ?? new ItemInstance(); }
            set { itemButtonWrist.SelectedItem = value; itemButtonWrist.UpdateSelectedItem(); }
        }

        public ItemInstance MainHand
        {
            get { return Character.MainHand ?? new ItemInstance(); }
            set { itemButtonMainHand.SelectedItem = value; itemButtonMainHand.UpdateSelectedItem(); }
        }

        public ItemInstance OffHand
        {
            get { return Character.OffHand ?? new ItemInstance(); }
            set { itemButtonOffHand.SelectedItem = value; itemButtonOffHand.UpdateSelectedItem(); }
        }

        public ItemInstance Ranged
        {
            get { return Character.Ranged ?? new ItemInstance(); }
            set { itemButtonRanged.SelectedItem = value; itemButtonRanged.UpdateSelectedItem(); }
        }

        public ItemInstance Projectile
        {
            get { return Character.Projectile ?? new ItemInstance(); }
            set { itemButtonProjectile.SelectedItemInstance = value; itemButtonProjectile.UpdateSelectedItem(); }
        }

        public ItemInstance ProjectileBag
        {
            get { return Character.ProjectileBag ?? new ItemInstance(); }
            set { itemButtonProjectileBag.SelectedItemInstance = value; itemButtonProjectileBag.UpdateSelectedItem(); }
        }

        public ItemInstance Hands
        {
            get { return Character.Hands ?? new ItemInstance(); }
            set { itemButtonHands.SelectedItem = value; itemButtonHands.UpdateSelectedItem(); }
        }

        public ItemInstance Waist
        {
            get { return Character.Waist ?? new ItemInstance(); }
            set { itemButtonWaist.SelectedItemInstance = value; itemButtonWaist.UpdateSelectedItem(); }
        }

        public ItemInstance Legs
        {
            get { return Character.Legs ?? new ItemInstance(); }
            set { itemButtonLegs.SelectedItem = value; itemButtonLegs.UpdateSelectedItem(); }
        }

        public ItemInstance Feet
        {
            get { return Character.Feet ?? new ItemInstance(); }
            set { itemButtonFeet.SelectedItem = value; itemButtonFeet.UpdateSelectedItem(); }
        }

        public ItemInstance Finger1
        {
            get { return Character.Finger1 ?? new ItemInstance(); }
            set { itemButtonFinger1.SelectedItem = value; itemButtonFinger1.UpdateSelectedItem(); }
        }

        public ItemInstance Finger2
        {
            get { return Character.Finger2 ?? new ItemInstance(); }
            set { itemButtonFinger2.SelectedItem = value; itemButtonFinger2.UpdateSelectedItem(); }
        }

        public ItemInstance Trinket1
        {
            get { return Character.Trinket1 ?? new ItemInstance(); }
            set { itemButtonTrinket1.SelectedItemInstance = value; itemButtonTrinket1.UpdateSelectedItem(); }
        }

        public ItemInstance Trinket2
        {
            get { return Character.Trinket2 ?? new ItemInstance(); }
            set { itemButtonTrinket2.SelectedItemInstance = value; itemButtonTrinket2.UpdateSelectedItem(); }
        }
        #endregion

        public Character Character
        {
            get
            {
                if (_character == null)
                {
                    Character character = new Character();
                    Character = character;
                }
                return _character;
            }
            set
            {
                if (_character != null)
                {
                    _character.CalculationsInvalidated -= new EventHandler(_character_ItemsChanged);
                }
                _character = value;
                if (_character != null)
                {
                    this.Cursor = Cursors.WaitCursor;
                    _character.IsLoading = true; // we do not need ItemsChanged event triggering until we call OnItemsChanged at the end

                    Character.CurrentModel = null;

                    Calculations.CalculationOptionsPanel.Character = _character;
                    ItemToolTip.Instance.Character =
                        ItemEnchantContextualMenu.Instance.Character = ItemContextualMenu.Instance.Character =
                        itemButtonBack.Character = itemButtonChest.Character = itemButtonFeet.Character =
                        itemButtonFinger1.Character = itemButtonFinger2.Character = itemButtonHands.Character =
                        itemButtonHead.Character = itemButtonRanged.Character = itemButtonLegs.Character =
                        itemButtonNeck.Character = itemButtonShirt.Character = itemButtonShoulders.Character =
                        itemButtonTabard.Character = itemButtonTrinket1.Character = itemButtonTrinket2.Character =
                        itemButtonWaist.Character = itemButtonMainHand.Character = itemButtonOffHand.Character =
                        itemButtonProjectile.Character = itemButtonProjectileBag.Character = itemButtonWrist.Character = _character;

                    _character.CalculationsInvalidated += new EventHandler(_character_ItemsChanged);
                    _loadingCharacter = true;

                    textBoxName.Text = Character.Name;
                    textBoxRealm.Text = Character.Realm;
                    comboBoxRegion.Text = Character.Region.ToString();
                    comboBoxRace.Text = Character.Race.ToString();
                    checkBoxEnforceGemRequirements.Checked = Character.EnforceGemRequirements;
                    {
                        comboBoxClass.Text = Character.Class.ToString();
                    }
                    _loadingCharacter = false;
                    _character.IsLoading = false;
                    _character_ItemsChanged(_character, EventArgs.Empty); // this way it won't cause extra invalidations for other listeners of the event
                    this.Cursor = Cursors.Default;
                }
            }
        }

        void _character_ItemsChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                Invoke((EventHandler)_character_ItemsChanged, sender, e);
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            if (!_loadingCharacter)
            {
                itemButtonBack.UpdateSelectedItem(); itemButtonChest.UpdateSelectedItem(); itemButtonFeet.UpdateSelectedItem();
                itemButtonFinger1.UpdateSelectedItem(); itemButtonFinger2.UpdateSelectedItem(); itemButtonHands.UpdateSelectedItem();
                itemButtonHead.UpdateSelectedItem(); itemButtonRanged.UpdateSelectedItem(); itemButtonLegs.UpdateSelectedItem();
                itemButtonNeck.UpdateSelectedItem(); itemButtonShirt.UpdateSelectedItem(); itemButtonShoulders.UpdateSelectedItem();
                itemButtonTabard.UpdateSelectedItem(); itemButtonTrinket1.UpdateSelectedItem(); itemButtonTrinket2.UpdateSelectedItem();
                itemButtonWaist.UpdateSelectedItem(); itemButtonMainHand.UpdateSelectedItem(); itemButtonOffHand.UpdateSelectedItem();
                itemButtonProjectile.UpdateSelectedItem(); itemButtonProjectileBag.UpdateSelectedItem(); itemButtonWrist.UpdateSelectedItem();
            }
            if (_character.IsMetaGemActive)
                itemButtonHead.BackColor = _defaultColor;
            else
                itemButtonHead.BackColor = Color.Red;
            this.Cursor = Cursors.Default;
        }

        public void SetResultText(String message)
        {
            labResults.Text = message;
        }
    }
}
