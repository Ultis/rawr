using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
	[Serializable]
	public class CalculationOptions : ICalculationOptionBase
	{
		private int _TargetLevel = 73;
		private int _TargetArmor = 7700;
		private bool _EnforceMetaGem = true;

		public int TargetLevel
		{
			get { return _TargetLevel; }
			set { _TargetLevel = value; }
		}

		public int TargetArmor
		{
			get { return _TargetArmor; }
			set { _TargetArmor = value; }
		}

		public bool EnforceMetaGem
		{
			get { return _EnforceMetaGem; }
			set { _EnforceMetaGem = true; }
		}

		#region ICalculationOptionBase Members

		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptions));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		#endregion
	}
}
