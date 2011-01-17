using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.UI
{
    public partial class EnterId : ChildWindow
    {
        public List<int> Values
        {
            get {
                String input = textItemId.Text;

                Regex wowhead = new Regex(@"(?<idlist>(?:\d{4,5}[,;]?\s?)*)");
                Match m = wowhead.Match(input);

                if (m.Success)
                {
                    string idtxt = "";
                    List<int> retVal = new List<int>() { };
                    foreach (Char c in m.Groups["idlist"].Value)
                    {
                        if (c != ',' && c != ';' && c != ' ') {
                            idtxt += c;
                        } else {
                            if (idtxt == "") { continue; }
                            retVal.Add(int.Parse(idtxt));
                            idtxt = "";
                        }
                    }

                    return retVal;
                    //return int.Parse(m.Groups[2].Value);
                }

                return new List<int>() { };
            }
        }
        public int Value
        {
            get
            {
                String input = textItemId.Text;

                Regex wowhead = new Regex(@"http://(www|cata|ptr).wowhead.com\/item=([-+]?\d+)");
                Match m = wowhead.Match(input);

                if (m.Success)
                {
                    return int.Parse(m.Groups[2].Value);
                }

                Regex thottbot = new Regex(@"http://thottbot.com/i([-+]?\d+)");
                m = thottbot.Match(input);

                if (m.Success)
                {
                    return int.Parse(m.Groups[1].Value);
                }

                Regex numeric = new Regex(@"([-+]?\d+)");
                m = numeric.Match(input);

                if (m.Success)
                {
                    return int.Parse(m.Groups[1].Value);
                }

                return 0;
            }
        }

        public bool UsePTR { get { return CK_WH.IsChecked.GetValueOrDefault(false)
            && (CK_PTR.IsChecked.GetValueOrDefault(false) || textItemId.Text.Contains("ptr") || textItemId.Text.Contains("cata")); } }

        public string ItemName
        {
            get { return textItemId.Text; }
        }

        public EnterId()
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

