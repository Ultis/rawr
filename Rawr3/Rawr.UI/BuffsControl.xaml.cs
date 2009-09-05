using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Rawr.UI
{
	public partial class BuffsControl : UserControl
	{

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                character = value;
                _loadingBuffsFromCharacter = true;
                BuildControls();
                LoadBuffsFromCharacter();
                UpdateEnabledStates();
                _loadingBuffsFromCharacter = false;
            }
        }

        private bool _loadingBuffsFromCharacter;
        private void LoadBuffsFromCharacter()
        {
            if (CheckBoxes == null || Character == null || Character.ActiveBuffs == null) return;
            foreach (KeyValuePair<Buff, CheckBox> kvp in CheckBoxes)
            {
                kvp.Value.IsChecked = false;                
            }
            List<CheckBox> toCheck = new List<CheckBox>();
            foreach (Buff b in Character.ActiveBuffs)
            {
                if (CheckBoxes.ContainsKey(b))
                {
                    toCheck.Add(CheckBoxes[b]);
                }
            }
            foreach (CheckBox cb in toCheck) cb.IsChecked = true;
        }

        private void UpdateCharacterBuffs()
        {
            List<Buff> activeBuffs = new List<Buff>();
            foreach (CheckBox checkBox in CheckBoxes.Values)
                if (checkBox.IsChecked.GetValueOrDefault(false) && checkBox.IsEnabled)
                    activeBuffs.Add((checkBox.Tag as Buff));
            Character.ActiveBuffs = activeBuffs;
        }

        public void UpdateEnabledStates()
        {
            List<string> currentConflictNames = new List<string>();
            foreach (CheckBox checkBox in CheckBoxes.Values)
            {
                if (checkBox.IsChecked.GetValueOrDefault(false))
                {
                    Buff buff = checkBox.Tag as Buff;
                    foreach (string conflictName in buff.ConflictingBuffs)
                        if (!string.IsNullOrEmpty(conflictName) && !currentConflictNames.Contains(conflictName))
                            currentConflictNames.Add(conflictName);
                }
            }

            foreach (CheckBox checkBox in CheckBoxes.Values)
            {
                checkBox.IsEnabled = true;
                Buff buff = checkBox.Tag as Buff;
                if (!string.IsNullOrEmpty(buff.SetName))
                {
                    checkBox.IsEnabled = false;
                    continue;
                }
                if (string.IsNullOrEmpty(buff.Group))
                {
                    if (FindParent.ContainsKey(buff) && !CheckBoxes[FindParent[buff]].IsChecked.GetValueOrDefault(false))
                    {
                        checkBox.IsEnabled = false;
                        continue;
                    }
                }
                if (!checkBox.IsChecked.GetValueOrDefault(false))
                {
                    foreach (string buffName in buff.ConflictingBuffs)
                    {
                        if (currentConflictNames.Contains(buffName))
                        {
                            checkBox.IsEnabled = false;
                            break;
                        }
                    }
                }
            }
        }

        private Dictionary<Buff, CheckBox> CheckBoxes;
        private Dictionary<Buff, Buff> FindParent;
        private void BuildControls()
        {
            if (CheckBoxes == null) CheckBoxes = new Dictionary<Buff, CheckBox>();
            else CheckBoxes.Clear();

            if (FindParent == null) FindParent = new Dictionary<Buff, Buff>();
            else FindParent.Clear();

            BuffStack.Children.Clear();
            Dictionary<string, GroupBox> buffGroups = new Dictionary<string, GroupBox>();
            if (Buff.RelevantBuffs != null)
            {
                foreach (Buff b in Buff.RelevantBuffs)
                {
                    GroupBox gb;
                    if (buffGroups.ContainsKey(b.Group)) gb = buffGroups[b.Group];
                    else
                    {
                        gb = new GroupBox();
                        gb.Header = b.Group;
                        StackPanel sp = new StackPanel();
                        gb.Content = sp;
                        BuffStack.Children.Add(gb);
                        buffGroups[b.Group] = gb;
                    }
                    CheckBox buffCb = new CheckBox();
                    buffCb.Content = b.Name;
                    ToolTipService.SetToolTip(buffCb, Calculations.GetRelevantStats(b.Stats).ToString());
                    buffCb.Checked += new RoutedEventHandler(buffCb_CheckedChange);
                    buffCb.Unchecked += new RoutedEventHandler(buffCb_CheckedChange);
                    buffCb.Tag = b;
                    ((StackPanel)gb.Content).Children.Add(buffCb);
                    CheckBoxes[b] = buffCb;

                    foreach (Buff i in b.Improvements)
                    {
                        buffCb = new CheckBox();
                        buffCb.Content = i.Name;
                        ToolTipService.SetToolTip(buffCb, Calculations.GetRelevantStats(i.Stats).ToString());
                        buffCb.Margin = new Thickness(16, 0, 0, 0);
                        buffCb.Checked += new RoutedEventHandler(buffCb_CheckedChange);
                        buffCb.Unchecked += new RoutedEventHandler(buffCb_CheckedChange);
                        buffCb.Tag = i;
                        CheckBoxes[i] = buffCb;
                        FindParent[i] = b;
                        ((StackPanel)gb.Content).Children.Add(buffCb);
                    }
                }
            }
        }

        void buffCb_CheckedChange(object sender, RoutedEventArgs e)
        {
            if (!_loadingBuffsFromCharacter && Character != null)
            {
                CheckBox cb = (CheckBox)sender;
                Buff b = (Buff)cb.Tag;
                UpdateEnabledStates();
                UpdateCharacterBuffs();
                Character.OnCalculationsInvalidated();
            }
        }

		public BuffsControl()
		{
			InitializeComponent();
		}
	}
}