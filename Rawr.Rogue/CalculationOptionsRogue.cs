using System;
using System.Text;
using System.Windows.Forms;
using Rawr.Rogue.ClassAbilities;
using Rawr.Rogue.ComboPointGenerators;
using Rawr.Rogue.Poisons;
using Rawr.Rogue.SpecialAbilities;

namespace Rawr.Rogue
{
    [Serializable]
    public class CalculationOptionsRogue : ICalculationOptionBase
    {
        public string GetXml()
        {
			try
			{
				var s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
				var xml = new StringBuilder();
				var sw = new System.IO.StringWriter(xml);
				s.Serialize(sw, this);
				return xml.ToString();
			}
			catch (Exception ex)
			{
#if DEBUG
				MessageBox.Show("DEBUG: Exception attempting to serialize CalculationOptionsRogue: " + ex.Message);
#endif
				return string.Empty;
			}
        }

        public int TargetLevel;
        public int TargetArmor;
        public Cycle DpsCycle = new Cycle();
		public PoisonBase TempMainHandEnchant = new NoPoison();
		public PoisonBase TempOffHandEnchant = new NoPoison();
        public ComboPointGenerator CpGenerator = new SinisterStrike();
        public Feint Feint = new Feint();
        public float TurnTheTablesUptime;
        public bool TargetIsValidForMurder;

        public float ComboPointsNeededForCycle()
        {
            return DpsCycle.TotalComboPoints - (DpsCycle.Components.Count * Talents.Ruthlessness.Bonus);
        }
    }
}