using System;
using System.Text;
using System.Windows.Forms;

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
        public Cycle DPSCycle = new Cycle();
		public PoisonBase TempMainHandEnchant = new NoPoison();
		public PoisonBase TempOffHandEnchant = new NoPoison();
    }
}