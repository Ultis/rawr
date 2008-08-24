using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Rogue {
    public partial class CalculationOptionsPanelRogue : CalculationOptionsPanelBase {
		private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

		public CalculationOptionsPanelRogue()
		{
			InitializeComponent();

			armorBosses.Add(3800, ": Shade of Aran");
			armorBosses.Add(4700, ": Roar");
			armorBosses.Add(5500, ": Netherspite");
			armorBosses.Add(6100, ": Julianne, Curator");
			armorBosses.Add(6200, ": Karathress, Vashj, Solarian, Kael'thas, Winterchill, Anetheron, Kaz'rogal, Azgalor, Archimonde, Teron, Shahraz");
			armorBosses.Add(6700, ": Maiden, Illhoof");
			armorBosses.Add(7300, ": Strawman");
			armorBosses.Add(7500, ": Attumen");
			armorBosses.Add(7600, ": Romulo, Nightbane, Malchezaar, Doomwalker");
			armorBosses.Add(7700, ": Hydross, Lurker, Leotheras, Tidewalker, Al'ar, Naj'entus, Supremus, Akama, Gurtogg");
			armorBosses.Add(8200, ": Midnight");
			armorBosses.Add(8800, ": Void Reaver");            
		}

        private bool _loadingCalculationOptions = false;

        protected override void LoadCalculationOptions() {
            _loadingCalculationOptions = true;

            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsRogue(Character);

            CalculationOptionsRogue calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
            comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();

            _loadingCalculationOptions = false;
        }


        private void button1_Click(object sender, EventArgs e) {
            if (Character != null && Character.Talents != null && Character.Talents.Trees != null && Character.Talents.Trees.Count > 0) {
            }
            else {
                MessageBox.Show("Error: There are no talents, which is a problem you know!");
            }
        }

        private void trackBarTargetArmor_Scroll(object sender, EventArgs e) {

        }

        private void trackBarTargetArmor_ValueChanged(object sender, EventArgs e) {
            if (!_loadingCalculationOptions) {
                CalculationOptionsRogue calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                trackBarTargetArmor.Value = 100 * (trackBarTargetArmor.Value / 100);
                labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");

                calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
                calcOpts.TargetArmor = trackBarTargetArmor.Value;

                Character.OnItemsChanged();
            }
        }
    }

    [Serializable]
    public class CalculationOptionsRogue : ICalculationOptionBase {
        public string GetXml() {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(xml);
            s.Serialize(sw, this);
            return xml.ToString();
        }

        public CalculationOptionsRogue() { }
        public CalculationOptionsRogue(Character character)
            : this() {
        }

        public int TargetLevel = 73;
        public int TargetArmor = 7700;
    }
}