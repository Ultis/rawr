using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Forms.Utilities
{
    public static class FormHelper
    {
        public static Form GetMainForm()
        {
            Form ret = null;
            for (int i = 0; i < Application.OpenForms.Count; i++)
            {
                if (Application.OpenForms[i] is FormMain)
                {
                    ret = Application.OpenForms[i];
                    break;
                }
            }
            return ret;
        }
    }
}
