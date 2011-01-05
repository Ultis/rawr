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
                WriteLine(4, "version = \"56979\","); // need some global way of getting Rawr version
                WriteLine(4, "realm = \"" + character.Realm + "\",");
                WriteLine(4, "name = \"" + character.Name + "\",");
                WriteSubPointTypes(4);
                WriteCharacter("character", character); // replace with ItemSet itemSet = character.GetItemSetByName("Current");
                WriteLoadedSet(character);
                WriteDirectUpgrades();
            WriteLine(0, "}");
            return output.ToString();
        }

        private void WriteCharacter(string block, Character character)
        {
            WriteLine(4, block + " = {");
            WriteLine(8, "items = {");
            WriteItem(12, 1, character, CharacterSlot.Head);
            WriteItem(12, 2, character, CharacterSlot.Neck);
            WriteItem(12, 3, character, CharacterSlot.Shoulders);
            WriteItem(12, 4, character, CharacterSlot.Shirt);
            WriteItem(12, 5, character, CharacterSlot.Chest);
            WriteItem(12, 6, character, CharacterSlot.Waist);
            WriteItem(12, 7, character, CharacterSlot.Legs);
            WriteItem(12, 8, character, CharacterSlot.Feet);
            WriteItem(12, 9, character, CharacterSlot.Wrist);
            WriteItem(12, 10, character, CharacterSlot.Hands);
            WriteItem(12, 11, character, CharacterSlot.Finger1);
            WriteItem(12, 12, character, CharacterSlot.Finger2);
            WriteItem(12, 13, character, CharacterSlot.Trinket1);
            WriteItem(12, 14, character, CharacterSlot.Trinket2);
            WriteItem(12, 15, character, CharacterSlot.Back);
            WriteItem(12, 16, character, CharacterSlot.MainHand);
            WriteItem(12, 17, character, CharacterSlot.OffHand);
            WriteItem(12, 18, character, CharacterSlot.Ranged);
            WriteItem(12, 19, character, CharacterSlot.Tabard);
            WriteLine(8, "},");
            WriteLine(4, "},");
        }

        private void WriteLoadedSet(Character character)
        {
            // ItemSet itemSet = character.GetItemSetByName("Last Loaded Set");
        }

        private void WriteDirectUpgrades()
        {
            if (DUCalcs == null)
            {
                WriteLine(4, "upgrades = {}, ");
                return;
            }
            WriteLine(4, "upgrades = {");

            WriteLine(4, "}, ");
        }
  
        private void WriteLine(int indent, string text)
        {
            output.Append(' ', indent);
            output.AppendLine(text);
        }

        private void WriteItem(int indent, int slotId, Character character, CharacterSlot slot)
        {
            ItemInstance item = character[slot];
            if (item == null)
            {
                WriteLine(indent, "{ slot = " + slotId + ", item = \"" + nullItem + "\" },");
                return;
            }
            WriteLine(indent, "{ slot = " + slotId + ", item = \"" + item.ToItemString() + "\", ");
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

