using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Rawr.Warlock
{
    public partial class TextInputDialog : ChildWindow
    {
        public TextInputDialog(string title, string message, string initialValue, IEnumerable<string> forbiddenValues)
        {
            InitializeComponent();

            this.inputBox.TextChanged += new TextChangedEventHandler(inputBox_TextChanged);
            this.Closing += new EventHandler<CancelEventArgs>(this_FormClosing);

            this.Title = title;
            this.MessageLabel.Text = m_initialMessage = message;
            this.inputBox.Text = initialValue;
            m_forbiddenValues = forbiddenValues;

            inputBox_TextChanged(this, null);
        }
        public string Result { get { return this.inputBox.Text; } set { this.inputBox.Text = value; } }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void inputBox_TextChanged(object sender, TextChangedEventArgs e) {
            this.okButton.IsEnabled = (!String.IsNullOrEmpty(this.inputBox.Text));
        }
        private void this_FormClosing(object sender, CancelEventArgs e) {
            if (m_forbiddenValues.Contains(this.inputBox.Text)) {
                e.Cancel = true;
                this.MessageLabel.Text = "That name is already in use. " + m_initialMessage;
            }
        }

        private string m_initialMessage;
        private IEnumerable<string> m_forbiddenValues;
    }
}

