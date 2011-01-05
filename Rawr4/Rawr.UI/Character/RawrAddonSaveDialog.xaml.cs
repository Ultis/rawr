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
                WriteLine(4, "version = \"56879\","); // need some global way of getting Rawr version
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
            WriteItem(12, 1, itemSet[CharacterSlot.Head], character, CharacterSlot.Head);
            WriteItem(12, 2, itemSet[CharacterSlot.Neck], character, CharacterSlot.Neck);
            WriteItem(12, 3, itemSet[CharacterSlot.Shoulders], character, CharacterSlot.Shoulders);
            WriteItem(12, 4, itemSet[CharacterSlot.Shirt], character, CharacterSlot.Shirt);
            WriteItem(12, 5, itemSet[CharacterSlot.Chest], character, CharacterSlot.Chest);
            WriteItem(12, 6, itemSet[CharacterSlot.Waist], character, CharacterSlot.Waist);
            WriteItem(12, 7, itemSet[CharacterSlot.Legs], character, CharacterSlot.Legs);
            WriteItem(12, 8, itemSet[CharacterSlot.Feet], character, CharacterSlot.Feet);
            WriteItem(12, 9, itemSet[CharacterSlot.Wrist], character, CharacterSlot.Wrist);
            WriteItem(12, 10, itemSet[CharacterSlot.Hands], character, CharacterSlot.Hands);
            WriteItem(12, 11, itemSet[CharacterSlot.Finger1], character, CharacterSlot.Finger1);
            WriteItem(12, 12, itemSet[CharacterSlot.Finger2], character, CharacterSlot.Finger2);
            WriteItem(12, 13, itemSet[CharacterSlot.Trinket1], character, CharacterSlot.Trinket1);
            WriteItem(12, 14, itemSet[CharacterSlot.Trinket2], character, CharacterSlot.Trinket2);
            WriteItem(12, 15, itemSet[CharacterSlot.Back], character, CharacterSlot.Back);
            WriteItem(12, 16, itemSet[CharacterSlot.MainHand], character, CharacterSlot.MainHand);
            WriteItem(12, 17, itemSet[CharacterSlot.OffHand], character, CharacterSlot.OffHand);
            WriteItem(12, 18, itemSet[CharacterSlot.Ranged], character, CharacterSlot.Ranged);
            WriteItem(12, 19, itemSet[CharacterSlot.Tabard], character, CharacterSlot.Tabard);
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
            }
            WriteLine(4, "}, ");
        }
  
        private void WriteLine(int indent, string text)
        {
            output.Append(' ', indent);
            output.AppendLine(text);
        }

        private void WriteItem(int indent, int slotId, ItemInstance item, Character character, CharacterSlot slot)
        {
            if (item == null)
            {
                WriteLine(indent, "{ slot = " + slotId + ", item = \"" + nullItem + "\" },");
                return;
            }
            WriteLine(indent, "{ slot = " + slotId + ", item = \"" + item.ToItemString() + "\", display = " + item.DisplaySlot + ", ");
            ComparisonCalculationBase itemCalcs = Calculations.GetItemCalculations(item, character, slot);
            WriteLine(indent + 4, "overall = " + itemCalcs.OverallPoints + ", ");
            WriteLine(indent + 4, "subpoint = { ");
            for(int i = 0; i < itemCalcs.SubPoints.Count(); i++)
            {
                WriteLine(indent + 8, itemCalcs.SubPoints[i] + ", ");
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

