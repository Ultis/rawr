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
    public partial class CustomItemInstance : ChildWindow
    {

        private Character character;
        public Character Character
        {
            get { return character; }
            set { character = value; }
        }

        private ItemInstance customInstance;
        public ItemInstance CustomInstance
        {
            get { return customInstance; }
            set {
                string pt = "\r\n- start";
                try {
                    customInstance = value;
                    pt += "\r\n- if";
                    if (customInstance != null)
                    {
                        pt += "\r\n- customInstance != null";
                        itemButtonWithEnchant.ComparisonItemList.SelectedItem = value.Item;
                    } else {
                        pt += "\r\n- customInstance == null";
                        itemButtonWithEnchant.ComparisonItemList.SelectedItem = null;
                    }
                    pt += "\r\n- UpdateSockets()";
                    UpdateSockets();
                    pt += "\r\n- end";
                } catch (Exception ex) {
                    new Base.ErrorBox()
                    {
                        Title = "Error Trying to set Custom Item Instance",
                        Function = "CustomInstance()",
                        Info = "Point:" + pt,
                        TheException = ex,
                    }.Show();
                } finally {
                }
            }
        }

        public CustomItemInstance(Character character, ItemInstance baseInstance)
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif

            Character = character;
            CustomInstance = baseInstance.Clone();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) { this.DialogResult = true; }
        private void CancelButton_Click(object sender, RoutedEventArgs e) { this.DialogResult = false; }

        // ================================================================================================
        private ItemListControl _formItemSelection;
        public ItemListControl FormItemSelection
        {
            get
            {
                if (_formItemSelection == null)
                    _formItemSelection = new ItemListControl();
                return _formItemSelection;
            }
        }

        public CharacterSlot CharacterSlot
        {
            get { return itemButtonWithEnchant.Slot; }
            set { itemButtonWithEnchant.Slot = value; }
        }

        private void itemButtonWithEnchant_Leave(object sender, EventArgs e)
        {
            UpdateSockets();
        }

        private void UpdateSockets()
        {
            if (CustomInstance != null && CustomInstance.Item != null)
            {
                Item item = CustomInstance.Item;
                bool blacksmithingSocket = true;
                Character character = FormItemSelection.Character;
                if ((itemButtonWithEnchant.Slot == CharacterSlot.Waist && character.WaistBlacksmithingSocketEnabled)
                    || (itemButtonWithEnchant.Slot == CharacterSlot.Waist && character.WaistBlacksmithingSocketEnabled)
                    || (itemButtonWithEnchant.Slot == CharacterSlot.Waist && character.WaistBlacksmithingSocketEnabled))
                {
                    blacksmithingSocket = true;
                }
                ItemSlot slot = item.SocketColor1;
                if (slot == ItemSlot.None && blacksmithingSocket)
                {
                    slot = ItemSlot.Prismatic;
                    blacksmithingSocket = false;
                }
            }
            /*itemButtonWithEnchant.ComparisonItemList.IsPopulated = false;
            itemButtonWithEnchant.ComparisonEnchantList.IsPopulated = false;*/
        }

        private void SetFormItemSelection(ItemInstance item)
        {
            if (MainPage.Instance.Character != null)
            {
                Character emptyGemCharacter = MainPage.Instance.Character.Clone();
                emptyGemCharacter[CharacterSlot] = item;
                _formItemSelection.Character = emptyGemCharacter;
                //_formItemSelection.Character.CurrentCalculations = Calculations.GetCharacterCalculations(emptyGemCharacter);
            }
        }    
    }
}

