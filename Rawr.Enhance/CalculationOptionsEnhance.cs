using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsEnhance : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsEnhance));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

        // Boss parameters
        private BossHandler Boss = new BossHandler();
        public string BossName = "Custom";
        public int TargetLevel = 83;
        public float TargetArmor = StatConversion.NPC_ARMOR[83 - 80];
        public bool InBack = true; 
        public int InBackPerc = 100;
		
        public int AverageLag = 250;
		public string MainhandImbue = "Windfury";
		public string OffhandImbue = "Flametongue";
		public float FightLength = 10.0f;
		public float TargetFireResistance = 0;
		public float TargetNatureResistance = 0;
		public bool Magma = true;
		public bool BaseStatOption = true;
        private Dictionary<EnhanceAbility, int> priorityList = new Dictionary<EnhanceAbility, int>();
        
        public void SetBoss(BossHandler boss) 
        {
            Boss = boss;
            BossName = boss.Name;
            TargetLevel = boss.Level;
            TargetArmor = (int)boss.Armor;
            FightLength = boss.BerserkTimer / 60f;
            InBack = ((InBackPerc = (int)(boss.InBackPerc_Melee * 100f)) != 0);
            TargetFireResistance = boss.Resistance(ItemDamageType.Fire);
            TargetNatureResistance = boss.Resistance(ItemDamageType.Nature);
        }

        public int GetAbilityPriority(EnhanceAbility abilityType)
        { 
            int value = -1; 
            priorityList.TryGetValue(abilityType, out value); 
            return value;
        }

        public void SetAbilityPriority(EnhanceAbility abilityType, int priority)
        {
            int value = GetAbilityPriority(abilityType);
            if (value != -1)
                priorityList.Remove(abilityType);
            if (priority > 0)
                priorityList.Add(abilityType, priority);
        }        
	}
}
