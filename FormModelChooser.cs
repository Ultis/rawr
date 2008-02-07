using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Rawr
{
	public partial class FormModelChooser : Form
	{
        private SortedList<String,CalculationsBase> _models = new SortedList<String,CalculationsBase>();
		public FormModelChooser()
		{
			InitializeComponent();

            _models.Clear(); 

            DirectoryInfo info = new DirectoryInfo(".");

            foreach(FileInfo file in info.GetFiles("*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(file.FullName);
                   
                    foreach (Type type in assembly.GetTypes())
                    {
                        if(type.IsSubclassOf(typeof(CalculationsBase)))
                        {
                            CalculationsBase model = (CalculationsBase) Activator.CreateInstance(type);
                            _models[model.DisplayName] = model;
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Write(e.Message);
                }
            }

            if(_models.Count == 0)
            {
                throw new TypeLoadException("Unable to find any model plug in dlls.  Please check that the files exist and are in the correct location");
            }

            listBoxModels.Items.Clear();

            foreach (string name in _models.Keys)
            {
                listBoxModels.Items.Add(name);
            }
            listBoxModels.SelectedIndex = 0;
		}

		private void listBoxModels_SelectedIndexChanged(object sender, EventArgs e)
		{
			buttonLoad.Enabled = listBoxModels.SelectedItem != null;
		}

		private void buttonLoad_Click(object sender, EventArgs e)
        {
            LoadModel();
		}

        private void listBoxModels_DoubleClick(object sender, EventArgs e)
        {
            LoadModel();
        }

        private void LoadModel()
        {
            Calculations.LoadModel(_models[listBoxModels.SelectedItem.ToString()]);
            _models.Clear();
            this.Close();
        }
	}
}
