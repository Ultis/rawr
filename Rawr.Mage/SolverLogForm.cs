using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Rawr.Mage
{
    public partial class SolverLogForm : Form
    {
        private static SolverLogForm instance;
        public static SolverLogForm Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SolverLogForm();
                }
                return instance;
            }
        }

        private class TextBoxWriter : TextWriter
        {
            public TextBox TextBox { get; set; }
            private StringBuilder sb = new StringBuilder();

            public override Encoding Encoding
            {
                get { return Encoding.Default; }
            }

            public void UpdateTextBox()
            {
                if (TextBox.Visible)
                {
                    TextBox.Invoke((MethodInvoker)delegate                    
                    {
                        TextBox.Text = sb.ToString();
                        TextBox.Select(TextBox.Text.Length, 0);
                        TextBox.ScrollToCaret();
                    });
                }
            }

            public override void Write(char value)
            {
                sb.Append(value);
                UpdateTextBox();
            }

            public override void WriteLine(string value)
            {
                sb.AppendLine(value);
                UpdateTextBox();
            }

            public override void Write(string value)
            {
                sb.Append(value);
                UpdateTextBox();
            }
        }

        private TextBoxWriter writer;

        public SolverLogForm()
        {
            InitializeComponent();

            Trace.Listeners.Add(new TextWriterTraceListener(writer = new TextBoxWriter() { TextBox = textBoxLog }));
        }

        private Solver advancedSolver;
        private object solverLock = new object();

        public bool IsSolverEnabled(Solver solver)
        {
            lock (solverLock)
            {
                return solver == advancedSolver;
            }
        }

        public void EnableSolver(Solver solver)
        {
            lock (solverLock)
            {
                advancedSolver = solver;
            }
            UpdateCancelButton();
        }

        public void DisableSolver(Solver solver)
        {
            lock (solverLock)
            {
                if (advancedSolver == solver)
                {
                    advancedSolver = null;
                }
            }
            UpdateCancelButton();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            lock (solverLock)
            {
                if (advancedSolver != null)
                {
                    advancedSolver.CancelAsync();
                }
            }
        }

        private void SolverLogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void UpdateCancelButton()
        {
            if (Visible)
            {
                Invoke((MethodInvoker)delegate
                {
                    lock (solverLock)
                    {
                        buttonCancel.Enabled = (advancedSolver != null);
                    }
                });
            }
        }

        private void SolverLogForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                writer.UpdateTextBox();
                UpdateCancelButton();
            }
        }
    }
}
