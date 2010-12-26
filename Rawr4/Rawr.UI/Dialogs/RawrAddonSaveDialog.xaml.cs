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

        public RawrAddonSaveDialog(Character character)
        {
            InitializeComponent();
            if (character != null)
            {
                output = new StringBuilder();
                TB_XMLDump.Text = BuildExportLua(character);
                TB_XMLDump.SelectAll();
            }
        }

        private string BuildExportLua(Character character)
        {
            string nullItem = "item:0:0:0:0:0:0:0:0:0:0";
            // this routine will build a LUA representation of the character for Rawr.Addon 
            // and populate the textbox with that for cut and paste into addon
            WriteLine(0, "Rawr:LoadWebData{");
            // WriteLine(4, "version = \"" + VERSION + "\""); // need some global way of getting Rawr version
                WriteLine(4, "realm = \"" + character.Realm + "\",");
                WriteLine(4, "name = \"" + character.Name + "\",");
                WriteLine(4, "character = {");
                    WriteLine(8, "items = {");
                    WriteLine(12, "{ slot = 1, item = \"" + (character.Head != null ? character.Head.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 2, item = \"" + (character.Neck != null ? character.Neck.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 3, item = \"" + (character.Shoulders != null ? character.Shoulders.ToItemString() : nullItem) + "\" },");
                        // WriteLine(12, "{ slot = 4, item = \"" + (character.Shirt != null ? character.Shirt.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 5, item = \"" + (character.Chest != null ? character.Chest.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 6, item = \"" + (character.Waist != null ? character.Waist.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 7, item = \"" + (character.Legs != null ? character.Legs.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 8, item = \"" + (character.Feet != null ? character.Feet.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 9, item = \"" + (character.Wrist != null ? character.Wrist.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 10, item = \"" + (character.Hands != null ? character.Hands.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 11, item = \"" + (character.Finger1 != null ? character.Finger1.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 12, item = \"" + (character.Finger2 != null ? character.Finger2.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 13, item = \"" + (character.Trinket1 != null ? character.Trinket1.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 14, item = \"" + (character.Trinket2 != null ? character.Trinket2.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 15, item = \"" + (character.Back != null ? character.Back.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 16, item = \"" + (character.MainHand != null ? character.MainHand.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 17, item = \"" + (character.OffHand != null ? character.OffHand.ToItemString() : nullItem) + "\" },");
                        WriteLine(12, "{ slot = 18, item = \"" + (character.Ranged != null ? character.Ranged.ToItemString() : nullItem) + "\" },");
                        // WriteLine(12, "{ slot = 19, item = \"" + (character.Tabard != null ? character.Tabard.ToItemString() : nullItem) + "\" },");
                    WriteLine(8, "},");
                WriteLine(4, "},");
            WriteLine(0, "}");
            return output.ToString();
        }
  
        private void WriteLine(int indent, string text)
        {
            output.Append(' ', indent);
            output.AppendLine(text);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

