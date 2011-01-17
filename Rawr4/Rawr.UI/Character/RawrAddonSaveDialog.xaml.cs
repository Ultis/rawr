using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Text;

namespace Rawr.UI
{
    public partial class RawrAddonSaveDialog : ChildWindow
    {
        private StringBuilder output;
        private ComparisonCalculationBase[] DUCalcs = null; // holds direct upgrade calculations
        private static string nullItem = "item:0:0:0:0:0:0:0:0:0:0";

        public RawrAddonSaveDialog(Character character, ComparisonCalculationBase[] duCalcs)
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif

            if (character != null)
            {
                output = new StringBuilder();
                DUCalcs = duCalcs;
                TB_XMLDump.Text = BuildExportLua(character);
                TB_XMLDump.SelectAll();
            }
        }
        
        private string BuildExportLua(Character character)
        {
            // this routine will build a LUA representation of the character for Rawr.Addon 
            // and populate the textbox with that for cut and paste into addon
            WriteLine(0, "Rawr:LoadWebData{");
                WriteLine(4, "version = \"57012\","); // need some global way of getting Rawr version
                WriteLine(4, "realm = \"" + character.Realm + "\",");
                WriteLine(4, "name = \"" + character.Name + "\",");
                WriteSubPointTypes(4);
                WriteItemSet("character", character.GetItemSetByName("Current"), character);
                WriteItemSet("loaded", character.GetItemSetByName("Last Loaded Set"), character);
                WriteDirectUpgrades();
            WriteLine(0, "}");
            return output.ToString();
        }

        private void WriteItemSet(string block, ItemSet itemSet, Character character)
        {
            WriteLine(4, block + " = {");
            WriteLine(8, "items = {");
            if (itemSet == null)
            {
                for (int slotId = 1; slotId <= 19; slotId++)
                {
                    WriteLine(12, "{ slot = " + slotId + ", item = \"" + nullItem + "\" },");
                }
            }
            else
            {
                WriteItem(12, itemSet[CharacterSlot.Head], character, CharacterSlot.Head);
                WriteItem(12, itemSet[CharacterSlot.Neck], character, CharacterSlot.Neck);
                WriteItem(12, itemSet[CharacterSlot.Shoulders], character, CharacterSlot.Shoulders);
                WriteItem(12, itemSet[CharacterSlot.Shirt], character, CharacterSlot.Shirt);
                WriteItem(12, itemSet[CharacterSlot.Chest], character, CharacterSlot.Chest);
                WriteItem(12, itemSet[CharacterSlot.Waist], character, CharacterSlot.Waist);
                WriteItem(12, itemSet[CharacterSlot.Legs], character, CharacterSlot.Legs);
                WriteItem(12, itemSet[CharacterSlot.Feet], character, CharacterSlot.Feet);
                WriteItem(12, itemSet[CharacterSlot.Wrist], character, CharacterSlot.Wrist);
                WriteItem(12, itemSet[CharacterSlot.Hands], character, CharacterSlot.Hands);
                WriteItem(12, itemSet[CharacterSlot.Finger1], character, CharacterSlot.Finger1);
                WriteItem(12, itemSet[CharacterSlot.Finger2], character, CharacterSlot.Finger2);
                WriteItem(12, itemSet[CharacterSlot.Trinket1], character, CharacterSlot.Trinket1);
                WriteItem(12, itemSet[CharacterSlot.Trinket2], character, CharacterSlot.Trinket2);
                WriteItem(12, itemSet[CharacterSlot.Back], character, CharacterSlot.Back);
                WriteItem(12, itemSet[CharacterSlot.MainHand], character, CharacterSlot.MainHand);
                WriteItem(12, itemSet[CharacterSlot.OffHand], character, CharacterSlot.OffHand);
                WriteItem(12, itemSet[CharacterSlot.Ranged], character, CharacterSlot.Ranged);
                WriteItem(12, itemSet[CharacterSlot.Tabard], character, CharacterSlot.Tabard);
            }
            WriteLine(8, "},");
            WriteLine(4, "},");
        }

        private void WriteDirectUpgrades()
        {
            if (DUCalcs == null)
            {
                WriteLine(4, "upgrades = {}, ");
                return;
            }
            WriteLine(4, "upgrades = {");
            foreach (ComparisonCalculationBase itemCalc in DUCalcs)
            {
                ItemInstance item = itemCalc.ItemInstance;
                if (item.SlotId != 0)
                    WriteItem(8, item, itemCalc, item.SlotId);
            }
            WriteLine(4, "}, ");
        }
  
        private void WriteLine(int indent, string text)
        {
            output.Append(' ', indent);
            output.AppendLine(text);
        }

        private void WriteItem(int indent, ItemInstance item, Character character, CharacterSlot slot)
        {
            int slotId = Item.GetSlotIdbyCharacterSlot(slot);
            if (slotId == 0 || item == null)
            {
                WriteLine(indent, "{ slot = " + slotId + ", item = \"" + nullItem + "\" },");
            }
            else
            {
                ComparisonCalculationBase itemCalc = Calculations.GetItemCalculations(item, character, slot);
                WriteItem(indent, item, itemCalc, slotId);
            }
        }

        private void WriteItem(int indent, ItemInstance item, ComparisonCalculationBase itemCalc, int slotId)
        {
            WriteLine(indent, "{ slot = " + slotId + ", item = \"" + item.ToItemString() + "\", ");
            WriteLine(indent + 4, "overall = " + itemCalc.OverallPoints + ", ");
            WriteLine(indent + 4, "subpoint = { ");
            for(int i = 0; i < itemCalc.SubPoints.Count(); i++)
            {
                WriteLine(indent + 8, itemCalc.SubPoints[i] + ", ");
            }
            WriteLine(indent + 4, "},");
            WriteLine(indent, "},");
        }

        private void WriteSubPointTypes(int indent)
        {
            int subpoints = Calculations.Instance.SubPointNameColors.Keys.Count;
            WriteLine(indent, "subpoints = {");
            WriteLine(indent + 4, "count = " + subpoints + ", ");
            WriteLine(indent + 4, "subpoint = { ");
            foreach (KeyValuePair<string, Color> kvp in Calculations.Instance.SubPointNameColors)
            {
                WriteLine(indent + 8, "\"" + kvp.Key + "\", ");
            }
            WriteLine(indent + 4, "},");
            WriteLine(indent + 4, "colour = { ");
            foreach (KeyValuePair<string, Color> kvp in Calculations.Instance.SubPointNameColors)
            {
                WriteLine(indent + 8, "\"" + kvp.Value + "\", ");
            }
            WriteLine(indent + 4, "},");
            WriteLine(indent, "},");
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

