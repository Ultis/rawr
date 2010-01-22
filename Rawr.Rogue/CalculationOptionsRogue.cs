using System;
using System.Text;
using Rawr.Rogue.ClassAbilities;
using Rawr.Rogue.ComboPointGenerators;
using Rawr.Rogue.Poisons;
using Rawr.Rogue.SpecialAbilities;

namespace Rawr.Rogue
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsRogue : ICalculationOptionBase
    {
        public string GetXml()
        {
			var s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
			var xml = new StringBuilder();
			var sw = new System.IO.StringWriter(xml);
			s.Serialize(sw, this);
			return xml.ToString();
        }

        public int TargetLevel = 83;
		public int TargetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
        public Cycle DpsCycle = new Cycle();
		public PoisonBase TempMainHandEnchant = new NoPoison();
		public PoisonBase TempOffHandEnchant = new NoPoison();
        public ComboPointGenerator CpGenerator = new SinisterStrike();
        public Feint Feint = new Feint();
        public float TurnTheTablesUptime;
        public float Duration;

        public float ComboPointsNeededForCycle()
        {
            return DpsCycle.TotalComboPoints - (DpsCycle.Components.Count * Talents.Ruthlessness.Bonus);
        }
    }
}