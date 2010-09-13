using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class DG_RawrCrashReports : Form
    {
        private string _ErrorMessage = "";
        private string _StackTrace = "";
        private string _SuggestedFix = "";

        public string ErrorMessage {
            get { return _ErrorMessage != "" ? _ErrorMessage : "No Error Message"; }
            set {
                _ErrorMessage = value;
                TB_ErrorMessage.Text = ErrorMessage;
            }
        }
        public string StackTrace {
            get { return _StackTrace != "" ? _StackTrace : "No Stack Trace"; }
            set {
                _StackTrace = value;
                TB_StackTrace.Text = StackTrace;
            }
        }
        public string SuggestedFix {
            get { return _SuggestedFix != "" ? _SuggestedFix : "No Suggested Fix"; }
            set {
                _SuggestedFix = value;
                TB_SugFix.Text = SuggestedFix;
            }
        }

        public DG_RawrCrashReports()
        {
            InitializeComponent();
        }

        private void BT_CopyToClip_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(string.Format("I have performed the Suggested Fix and continue to receive this error."+
                        "\r\n\r\n== Error Message ==\r\n{0}\r\n\r\n== StackTrace ==\r\n{1}", ErrorMessage, StackTrace));
        }
    }
}
