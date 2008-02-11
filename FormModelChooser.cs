using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Rawr
{
	public partial class FormModelChooser : Form
	{
        private SortedList<String, Type> _models = new SortedList<String, Type>();
		public SortedList<String, Type> Models { get { return _models; } }

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
							System.ComponentModel.DisplayNameAttribute[] displayNameAttributes = type.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), false) as System.ComponentModel.DisplayNameAttribute[];
							string displayName = type.Name;
							if (displayNameAttributes.Length > 0) 
								displayName = displayNameAttributes[0].DisplayName;
							_models[displayName] = type;
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

            CalculationsBase model = (CalculationsBase)Activator.CreateInstance(_models[listBoxModels.SelectedItem.ToString()]);
            Calculations.LoadModel(model);
            //_models.Clear();
            this.Close();
        }
	}
}
