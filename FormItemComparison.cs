using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class FormItemComparison : Form, IFormItemSelectionProvider
    {

        private List<ComparisonSetControl> comparisonSets;
        private List<GroupBox> comparisonGroupBoxes;
        private List<Button> comparisonRemoveButtons;

        private Character _baseCharacter;
        public Character BaseCharacter
        {
            get { return _baseCharacter; }
            set
            {
                _baseCharacter = value;
                if (_baseCharacter != null)
                {
                    FormItemSelection.Character = _baseCharacter;
                    _baseCharacter.CalculationsInvalidated += new EventHandler(UpdateCalculations);

                    foreach (ComparisonSetControl csc in comparisonSets) csc.BaseCharacter = BaseCharacter;
                    comparisonGraph.Character = BaseCharacter;

                    UpdateCalculations(null, null);
                }
            }
        }

        private CharacterCalculationsBase _currentCalculations;
        public CharacterCalculationsBase CurrentCalculations
        {
            get
            {
                if (_currentCalculations == null)
                {
                    if (BaseCharacter != null)
                    {
                        _currentCalculations = Calculations.GetCharacterCalculations(BaseCharacter, null, true, true, true);
                    }
                    else return null;
                }
                return _currentCalculations;
            }
        }

        public FormItemComparison()
        {
            InitializeComponent();

            comparisonSets = new List<ComparisonSetControl>();
            comparisonGroupBoxes = new List<GroupBox>();
            comparisonRemoveButtons = new List<Button>();

            AddComparisonSet(1);            
        }

        private void AddComparisonSet(int number)
        {
            ComparisonSetControl set = new ComparisonSetControl();
            GroupBox group = new GroupBox();
            Button button = new Button();

            group.SuspendLayout();
            this.panel1.Controls.Add(group);

            group.Controls.Add(set);
            group.Controls.Add(button);
            group.Location = new System.Drawing.Point(3, 3 + 336 * (number - 1));
            group.Name = "groupBox1";
            group.Size = new System.Drawing.Size(222, 330);
            group.TabIndex = number;
            group.TabStop = false;
            group.Text = "Comparison Set #" + number;
            group.Tag = number;

            button.Location = new System.Drawing.Point(158, -1);
            button.Name = "removeButton";
            button.Size = new System.Drawing.Size(55, 20);
            button.TabIndex = 1;
            button.Text = "Remove";
            button.UseVisualStyleBackColor = true;
            button.Tag = number;
            button.Click += new EventHandler(removeButton_Click);

            set.Location = new System.Drawing.Point(6, 19);
            set.Name = "comparisonSetControl1";
            set.Size = new System.Drawing.Size(210, 305);
            set.TabIndex = 2;
            set.CalculationsInvalidated += new EventHandler(UpdateGraph);
            set.BaseCharacter = BaseCharacter;
            set.Tag = number;

            addSetButton.Top = 3 + 336 * number;

            group.ResumeLayout(false);

            comparisonSets.Add(set);
            comparisonGroupBoxes.Add(group);
            comparisonRemoveButtons.Add(button);
        }

        public void UpdateCalculations(object sender, EventArgs e)
        {
            _currentCalculations = null;
            if (BaseCharacter != null)
            {
                FormItemSelection.CurrentCalculations = CurrentCalculations;
                UpdateGraph(this, EventArgs.Empty);
            }
        }

        private FormItemSelection _formItemSelection;
        public FormItemSelection FormItemSelection
        {
            get
            {
                if (_formItemSelection == null || _formItemSelection.IsDisposed)
                    _formItemSelection = new FormItemSelection();
                return _formItemSelection;
            }
        }

        private void UpdateGraph(object sender, EventArgs e)
        {
            List<ComparisonCalculationBase> compareCalcs = new List<ComparisonCalculationBase>();

            int i = 0;
            foreach (ComparisonSetControl csc in comparisonSets)
            {
                Item item = new Item();
                item.Name = "Comparison Set #" + ++i;
                item.Quality = Item.ItemQuality.Temp;
                item.Stats = Calculations.GetItemStats(csc.CompositeCharacter, null) - Calculations.GetItemStats(BaseCharacter, null);
                
                ComparisonCalculationBase comp = Calculations.GetCharacterComparisonCalculations(CurrentCalculations, csc.CurrentCalculations, item.Name, false);
                comp.Item = item;
                compareCalcs.Add(comp);


            }

            comparisonGraph.ItemCalculations = compareCalcs.ToArray();
        }

        private void addSetButton_Click(object sender, EventArgs e)
        {
            AddComparisonSet(comparisonSets.Count + 1);
            UpdateGraph(this, EventArgs.Empty);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                List<Button> newRemoveButtons = new List<Button>();
                List<ComparisonSetControl> newSets = new List<ComparisonSetControl>();
                List<GroupBox> newGroupBoxes = new List<GroupBox>();

                int number = (int)((Button)sender).Tag;
                int i = 0;
                foreach (GroupBox group in comparisonGroupBoxes)
                {
                    if ((int)group.Tag == number)
                    {
                        foreach (Control c in group.Controls) c.Dispose();
                        group.Dispose();
                    }
                    else
                    {
                        group.Top = 3 + 336 * i++;
                        group.Text = "Comparison Set #" + i;
                        group.Tag = i;
                        foreach (Control c in group.Controls)
                        {
                            if (c.GetType() == typeof(Button)) newRemoveButtons.Add(c as Button);
                            if (c.GetType() == typeof(ComparisonSetControl)) newSets.Add(c as ComparisonSetControl);
                            c.Tag = i;
                        }
                        newGroupBoxes.Add(group);
                    }
                }
                addSetButton.Top = 3 + 336 * i;

                comparisonGroupBoxes = newGroupBoxes;
                comparisonSets = newSets;
                comparisonRemoveButtons = newRemoveButtons;

                UpdateGraph(this, EventArgs.Empty);
            }
        }

    }
}
