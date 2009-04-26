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
        private List<Button> comparisonButtons;
        private List<TabPage> comparisonPages;

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
            comparisonPages = new List<TabPage>();
            comparisonButtons = new List<Button>();

            AddComparisonSet(1);            
        }

        private TabPage AddComparisonSet(int number)
        {
            ComparisonSetControl set = new ComparisonSetControl();
            TabPage tabPage = new TabPage();
            Button button = new Button();

            this.tabControl.Controls.Add(tabPage);
            tabPage.Controls.Add(button);
            tabPage.Controls.Add(set);

            tabPage.Location = new System.Drawing.Point(4, 22);
            tabPage.Name = "Set" + number;
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Size = new System.Drawing.Size(234, 422);
            tabPage.TabIndex = 0;
            tabPage.Tag = number;
            tabPage.Text = "Set #" + number;
            tabPage.UseVisualStyleBackColor = true;

            button.Location = new System.Drawing.Point(132, 311);
            button.Name = "removeButton";
            button.Size = new System.Drawing.Size(75, 23);
            button.TabIndex = 1;
            button.Text = "Remove";
            button.UseVisualStyleBackColor = true;
            button.Tag = number;
            button.Click += new EventHandler(removeButton_Click);

            set.Location = new System.Drawing.Point(0, 2);
            set.Name = "comparisonSetControl1";
            set.Size = new System.Drawing.Size(210, 305);
            set.TabIndex = 2;
            set.CalculationsInvalidated += new EventHandler(UpdateGraph);
            set.BaseCharacter = BaseCharacter;
            set.Tag = number;

            comparisonSets.Add(set);
            comparisonButtons.Add(button);
            comparisonPages.Add(tabPage);

            return tabPage;
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
            tabControl.SelectedTab = AddComparisonSet(tabControl.Controls.Count + 1);
            UpdateGraph(this, EventArgs.Empty);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                List<ComparisonSetControl> newSets = new List<ComparisonSetControl>();
                List<TabPage> newPages = new List<TabPage>();
                List<Button> newButtons = new List<Button>();

                int number = (int)((Button)sender).Tag;
                int i = 1;

                tabControl.SuspendLayout();
                foreach (TabPage tabPage in tabControl.Controls)
                {
                    if ((int)tabPage.Tag == number)
                    {
                        foreach (Control c in tabPage.Controls)
                        {
                            c.Dispose();
                        }
                        tabPage.Dispose();
                        tabControl.Controls.Remove(tabPage);
                    }
                    else
                    {
                        foreach (Control c in tabPage.Controls)
                        {
                            c.Tag = number;
                            if (c.GetType() == typeof(Button)) newButtons.Add(c as Button);
                            if (c.GetType() == typeof(ComparisonSetControl)) newSets.Add(c as ComparisonSetControl);
                        }
                        tabPage.Tag = i++;
                        newPages.Add(tabPage);
                    }
                }
                tabControl.ResumeLayout(false);

                comparisonPages = newPages;
                comparisonSets = newSets;
                comparisonButtons = newButtons;

                UpdateGraph(this, EventArgs.Empty);
            }
        }

    }
}
