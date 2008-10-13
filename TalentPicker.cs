using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;

namespace Rawr
{
	public partial class TalentPicker : UserControl
	{

        private static readonly string _SavedFilePath;
        static TalentPicker()
        {
            _SavedFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Data\\Talents.xml");
        }

		public TalentPicker()
		{
            LoadTalentSpecs();
			InitializeComponent();
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}

        private List<SavedTalentSpec> _savedTalents;
        private void LoadTalentSpecs()
        {
            try
            {
                if (File.Exists(_SavedFilePath))
                {
                    using (StreamReader reader = new StreamReader(_SavedFilePath, Encoding.UTF8))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<SavedTalentSpec>));
                        _savedTalents = (List<SavedTalentSpec>)serializer.Deserialize(reader);
                        reader.Close();
                    }
                }
            }
            catch (Exception)
            {
                ;
            }
            if (_savedTalents == null)
            {
                _savedTalents = new List<SavedTalentSpec>(10);
            }
        }
        private void SaveTalentSpecs()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_SavedFilePath, false, Encoding.UTF8))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<SavedTalentSpec>));
                    serializer.Serialize(writer, _savedTalents);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                Log.Write(ex.StackTrace);
            }
        }

		private Character _character = null;
		public Character Character
		{
			get { return _character; }
			set
			{
				if (_character != null)
				{
					_character.ClassChanged -= new EventHandler(_character_ClassChanged);
				}
				_character = value;
				if (_character != null)
				{
					_character.ClassChanged += new EventHandler(_character_ClassChanged);
                    talentTree1.CharacterClass = value.Class;
                    talentTree2.CharacterClass = value.Class;
                    talentTree3.CharacterClass = value.Class;
                    Talents = _character.CurrentTalents;
                    UpdateSavedTalents();
				}
			}
		}

		private TalentsBase _talents = null;
		public TalentsBase Talents
		{
			get { return _talents; }
			set
			{
                _talents = value;
                if (_character != null) _character.CurrentTalents = value;
				UpdateTrees();
			}
		}

		void _character_ClassChanged(object sender, EventArgs e)
        {
            Talents = _character.CurrentTalents;
            UpdateSavedTalents();
		}


        public SavedTalentSpec CustomSpec { get; set; }
        public List<SavedTalentSpec> SpecsFor(Character.CharacterClass charClass)
        {
            List<SavedTalentSpec> classTalents = new List<SavedTalentSpec>();
            foreach (SavedTalentSpec spec in _savedTalents)
            {
                if (spec.Class == _character.Class)
                {
                    classTalents.Add(spec);
                }
            }
            if (((SavedTalentSpec)comboBoxTalentSpec.SelectedItem).Spec == null)
            {
                CustomSpec = new SavedTalentSpec("Custom", _talents);
                classTalents.Add(CustomSpec);
            }
            return classTalents;
        }

        public SavedTalentSpec CurrentSpec()
        {
            if (((SavedTalentSpec)comboBoxTalentSpec.SelectedItem).Spec == null) return CustomSpec;
            return (SavedTalentSpec)comboBoxTalentSpec.SelectedItem;
        }

        private bool _updateSaved = false;
        private void UpdateSavedTalents()
        {
            if (_character != null)
            {
                List<SavedTalentSpec> classTalents = new List<SavedTalentSpec>();
                SavedTalentSpec current = null;
                foreach (SavedTalentSpec spec in _savedTalents)
                {
                    if (spec.Class == _character.Class) { 
                        classTalents.Add(spec);
                        if (spec.Equals(_talents)) current = spec;
                    }
                }
                if (current == null)
                {
                    current = new SavedTalentSpec("Custom", null);
                    classTalents.Add(current);
                }
                _updateSaved = true;
                comboBoxTalentSpec.DataSource = classTalents;
                comboBoxTalentSpec.SelectedItem = current;
                _updateSaved = false;
            }
        }

		private List<string> _treeNames = new List<string>();
		private void UpdateTrees()
		{
			ClearTalentPickerItems();
			if (Talents != null)
			{
                _treeNames = new List<string>((string[])Talents.GetType().GetField("TreeNames").GetValue(Talents));
                talentTree1.CharacterClass = _character.Class;
                talentTree1.TreeName = _treeNames[0];
                talentTree2.CharacterClass = _character.Class;
                talentTree2.TreeName = _treeNames[1];
                talentTree3.CharacterClass = _character.Class;
                talentTree3.TreeName = _treeNames[2];

                TalentTree currentTree;              
				foreach (PropertyInfo pi in Talents.GetType().GetProperties())
				{
					TalentDataAttribute[] talentDatas = pi.GetCustomAttributes(typeof(TalentDataAttribute), true) as TalentDataAttribute[];
					if (talentDatas.Length > 0)
                    {
                        TalentDataAttribute talentData = talentDatas[0];
                        switch (talentData.Tree)
                        {
                            case 0: currentTree = talentTree1; break;
                            case 1: currentTree = talentTree2; break;
                            default: currentTree = talentTree3; break;
                        }
                        TalentItem item = new TalentItem(currentTree, talentData.Name, talentData.Row - 1, talentData.Column - 1, talentData.Index, LineWrapDescription(talentData.Description),
                            (int)pi.GetValue(Talents, null), talentData.MaxPoints, talentData.Prerequisite >= 0 ? _talentPickerItems[talentData.Prerequisite] : null);
                        _talentPickerItems[talentData.Index] = item;
                        currentTree.AddTalent(item);
						//TalentPickerItem item = new TalentPickerItem(_character.Class, talentData.Name, _treeNames[talentData.Tree], talentData.Description,
						//	false, talentData.Index, talentData.Prerequisite, talentData.Row, talentData.Column, (int)pi.GetValue(Talents, null), talentData.MaxPoints);
						//item.Location = new Point(-45 + (talentData.Column * 63), -57 + (talentData.Row * 65));
						item.CurrentRankChanged += new EventHandler(item_CurrentRankChanged);
					}
				}
                talentTree1.Redraw();
                talentTree2.Redraw();
                talentTree3.Redraw();
                item_CurrentRankChanged(null, null);
			}
		}

		private string[] LineWrapDescription(string[] descriptions)
		{
			List<string> wrappedDescriptions = new List<string>();
			foreach (string description in descriptions)
			{
				string[] lines = description.Replace("\t","").Split(new string[] { "\r\n" }, StringSplitOptions.None);
				List<string> wrappedLines = new List<string>();
				foreach (string line in lines)
				{
					string lineRemaining = line;
					StringBuilder sbLine = new StringBuilder();
					while (lineRemaining.Length > 70)
					{
						sbLine.Append(lineRemaining.Substring(0, lineRemaining.LastIndexOf(' ', 70)) + "\r\n");
						lineRemaining = lineRemaining.Substring(lineRemaining.LastIndexOf(' ', 70) + 1);
					}
					sbLine.Append(lineRemaining);
					wrappedLines.Add(sbLine.ToString());
				}
				wrappedDescriptions.Add(string.Join("\r\n", wrappedLines.ToArray()));
			}
			return wrappedDescriptions.ToArray();
		}

        private TalentItem[] _talentPickerItems = new TalentItem[100];
        private void ClearTalentPickerItems()
        {
            foreach (TalentItem item in _talentPickerItems)
            {
                if (item != null)
                {
                    item.CurrentRankChanged -= new EventHandler(item_CurrentRankChanged);
                }
            }
            _talentPickerItems = new TalentItem[100];
            talentTree1.Reset();
            talentTree2.Reset();
            talentTree3.Reset();
        }

        void item_CurrentRankChanged(object sender, EventArgs e)
        {
            TalentItem item = sender as TalentItem;
            tabPageTree1.Text = string.Format("{0} ({1})", _treeNames[0], talentTree1.TotalPoints());
            tabPageTree2.Text = string.Format("{0} ({1})", _treeNames[1], talentTree2.TotalPoints());
            tabPageTree3.Text = string.Format("{0} ({1})", _treeNames[2], talentTree3.TotalPoints());
            if (item != null)
            {
                Talents.Data[item.Index] = item.CurrentRank;
            }
            UpdateSavedTalents();
            _character.OnCalculationsInvalidated();
        }

        private void comboBoxTalentSpec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((SavedTalentSpec)comboBoxTalentSpec.SelectedItem).Spec == null)
            {
                talentSpecButton.Text = "Save";
            }
            else
            {
                talentSpecButton.Text = "Delete";
                if (!_updateSaved) Talents = ((SavedTalentSpec)comboBoxTalentSpec.SelectedItem).TalentSpec();
            }
        }

        private void talentSpecButton_Click(object sender, EventArgs e)
        {
            if (((SavedTalentSpec)comboBoxTalentSpec.SelectedItem).Spec == null)
            {
                List<SavedTalentSpec> classTalents = new List<SavedTalentSpec>();
                foreach (SavedTalentSpec spec in _savedTalents)
                {
                    if (spec.Class == _character.Class) classTalents.Add(spec);
                }
                FormSaveTalentSpec form = new FormSaveTalentSpec(classTalents);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    SavedTalentSpec spec = form.TalentSpec();
                    String specName = form.TalentSpecName();
                    if (spec == null)
                    {
                        spec = new SavedTalentSpec(specName, _talents);
                        _savedTalents.Add(spec);
                    }
                    else spec.Spec = _talents.ToString();
                    UpdateSavedTalents();
                    SaveTalentSpecs();
                    _character.OnCalculationsInvalidated();
                }
                form.Dispose();
            }
            else
            {
                _savedTalents.Remove((SavedTalentSpec)comboBoxTalentSpec.SelectedItem);
                UpdateSavedTalents();
                SaveTalentSpecs();
                _character.OnCalculationsInvalidated();
            }
        }

	}
}
