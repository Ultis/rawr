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
#if !SILVERLIGHT
using Microsoft.Win32;
#endif

namespace Rawr.UI
{
    public partial class CharProfLoadDialog : ChildWindow
    {
        static CharProfLoadDialog()
        {
            // Nothing to do here
        }

        public Character Character { get; private set; }

        public CharProfLoadDialog()
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
#endif

            if (Rawr.Properties.RecentSettings.Default.RecentCharProfiler == null) {
                Rawr.Properties.RecentSettings.Default.RecentCharProfiler =
                    "C:\\Program Files\\World of Warcraft\\WTF\\Account\\";
            }
            TB_FilePath.Text = Rawr.Properties.RecentSettings.Default.RecentCharProfiler;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Run Load Character from Profiler action
            try
            {
                CharacterProfilerData characterList = new CharacterProfilerData(TB_FilePath.Text);

                FormChooseCharacter form = new FormChooseCharacter(characterList);

                form.Show();
                if (form.DialogResult == true)
                {
                    //StartProcessing();
                    //BackgroundWorker bw = new BackgroundWorker();
                    //bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_LoadCharacterProfilerComplete);
                    //bw.DoWork += new DoWorkEventHandler(bw_LoadCharacterProfiler);
                    //bw.RunWorkerAsync(form.Character);
                    Base.ErrorBox eb = new Base.ErrorBox("Load Character from Character Profiler", "Not Yet Implemented");
                    eb.Show();
                }

                //form.Dispose();
            }
            catch (InvalidDataException ex)
            {
                MessageBox.Show("Unable to parse saved variable file: " + ex.Message);
                Base.ErrorBox eb = new Base.ErrorBox("Load Character from Character Profiler", "Not Yet Implemented");
                eb.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading saved variable file: " + ex.Message);
                Base.ErrorBox eb = new Base.ErrorBox("Error reading saved variable file", "Not Yet Implemented");
                eb.Show();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Character Profiler Saved Variables Files  (*.lua)|*.lua";
            ofd.Multiselect = false;
            if (ofd.ShowDialog().GetValueOrDefault(false))
            {
                //System.IO.FileStream file = ofd.File.OpenRead();
                //System.IO.Stream file = ofd.File.OpenRead();

                try
                {
#if SILVERLIGHT
                    System.IO.FileInfo file = ofd.File;
                    TB_FilePath.Text = file.FullName;
#else
                    TB_FilePath.Text = ofd.FileName;
#endif
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading saved variable file: " + ex.Message);
                }
            }
        }
    }
}

