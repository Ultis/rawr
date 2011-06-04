using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Tree {
    public class CalculationOptionsTree : ICalculationOptionBase, INotifyPropertyChanged {
    public string GetXml()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsTree));
        StringBuilder xml = new StringBuilder();
        StringWriter writer = new StringWriter(xml);
        serializer.Serialize(writer, this);
        return xml.ToString();
    }

    public CalculationOptionsTree()
    {
        }

    public bool Notify = true;

    private float tankRatio = 0.05f;
    public float TankRatio
    {
        get { return tankRatio; }
        set { tankRatio = value; OnPropertyChanged("TankRatio"); }
    }

    private float raidBurstRatio = 0.05f;
    public float RaidBurstRatio
    {
        get { return raidBurstRatio; }
        set { raidBurstRatio = value; OnPropertyChanged("RaidBurstRatio"); }
    }

    private float tankBurstRatio = 0.20f;
    public float TankBurstRatio
    {
        get { return tankBurstRatio; }
        set { tankBurstRatio = value; OnPropertyChanged("TankBurstRatio"); }
    }

    private bool restoration = true;
    public bool Restoration
    {
        get { return restoration; }
        set { restoration = value; OnPropertyChanged("Restoration"); }
    }

    private bool timedInnervates = true;
    public bool TimedInnervates
    {
        get { return timedInnervates; }
        set { timedInnervates = value; OnPropertyChanged("TimedInnervates"); }
    }

    private bool boostIntellectBeforeInnervate = true;
    public bool BoostIntellectBeforeInnervate
    {
        get { return boostIntellectBeforeInnervate; }
        set { boostIntellectBeforeInnervate = value; OnPropertyChanged("BoostIntellectBeforeInnervate"); }
    }

    private bool innervateOther = false;
    public bool InnervateOther
    {
        get { return innervateOther; }
        set { innervateOther = value; OnPropertyChanged("InnervateOther"); }
    }

    private int externalInnervateSize = 0;
    public int ExternalInnervateSize
    {
        get { return externalInnervateSize; }
        set { externalInnervateSize = value; OnPropertyChanged("ExternalInnervateSize"); }
    }

    private bool separateHasteEffects = false;
    public bool SeparateHasteEffects
    {
        get { return separateHasteEffects; }
        set { separateHasteEffects = value; OnPropertyChanged("SeparateHasteEffects"); }
    }

    private float procTriggerInterval = 0.3f;
    public float ProcTriggerInterval
    {
        get { return procTriggerInterval; }
        set { procTriggerInterval = value; OnPropertyChanged("ProcTriggerInterval"); }
    }

    private float procPeriodicTriggerInterval = 0.3f;
    public float ProcPeriodicTriggerInterval
    {
        get { return procPeriodicTriggerInterval; }
        set { procPeriodicTriggerInterval = value; OnPropertyChanged("ProcPeriodicTriggerInterval"); }
    }

    private int procTriggerIterations = 1;
    public int ProcTriggerIterations
    {
        get { return procTriggerIterations; }
        set { procTriggerIterations = value; OnPropertyChanged("ProcTriggerIterations"); }
    }

    private float lifebloomWastedDuration = 0.5f;
    public float LifebloomWastedDuration
    {
        get { return lifebloomWastedDuration; }
        set { lifebloomWastedDuration = value; OnPropertyChanged("LifebloomWastedDuration"); }
    }

    private float wildGrowthCastDelay = 1.0f;
    public float WildGrowthCastDelay
    {
        get { return wildGrowthCastDelay; }
        set { wildGrowthCastDelay = value; OnPropertyChanged("WildGrowthCastDelay"); }
    }

    private float swiftmendCastDelay = 1.0f;
    public float SwiftmendCastDelay
    {
        get { return swiftmendCastDelay; }
        set { swiftmendCastDelay = value; OnPropertyChanged("SwiftmendCastDelay"); }
    }
    
    private float naturesSwiftnessCastDelay = 5.0f;
    public float NaturesSwiftnessCastDelay
    {
        get { return naturesSwiftnessCastDelay; }
        set { naturesSwiftnessCastDelay = value; OnPropertyChanged("NaturesSwiftnessCastDelay"); }
    }

    private float tranquilityCastDelay = 5.0f;
    public float TranquilityCastDelay
    {
        get { return tranquilityCastDelay; }
        set { tranquilityCastDelay = value; OnPropertyChanged("TranquilityCastDelay"); }
    }

    private float glyphOfRegrowthExtraDuration = 2.0f;
    public float GlyphOfRegrowthExtraDuration
    {
        get { return glyphOfRegrowthExtraDuration; }
        set { glyphOfRegrowthExtraDuration = value; OnPropertyChanged("GlyphOfRegrowthExtraDuration"); }
    }

    private bool tankSwiftmend = true;
    public bool TankSwiftmend
    {
        get { return tankSwiftmend; }
        set { tankSwiftmend = value; OnPropertyChanged("TankSwiftmend"); }
    }

    private bool tankWildGrowth;
    public bool TankWildGrowth
    {
        get { return tankWildGrowth; }
        set { tankWildGrowth = value; OnPropertyChanged("TankWildGrowth"); }
    }

    private bool refreshLifebloomWithDirectHeals;
    public bool RefreshLifebloomWithDirectHeals
    {
        get { return refreshLifebloomWithDirectHeals; }
        set { refreshLifebloomWithDirectHeals = value; OnPropertyChanged("RefreshLifebloomWithDirectHeals"); }
    }

    private bool rejuvenationTankDuringRaid;
    public bool RejuvenationTankDuringRaid
    {
        get { return rejuvenationTankDuringRaid; }
        set { rejuvenationTankDuringRaid = value; OnPropertyChanged("RejuvenationTankDuringRaid"); }
    }

    private float efflorescenceTargets = 2.0f;
    public float EfflorescenceTargets
    {
        get { return efflorescenceTargets; }
        set { efflorescenceTargets = value; OnPropertyChanged("EfflorescenceTargets"); }
    }

    private float wildGrowthSymbiosisRate = 0.1f;
    public float WildGrowthSymbiosisRate
    {
        get { return wildGrowthSymbiosisRate; }
        set { wildGrowthSymbiosisRate = value; OnPropertyChanged("WildGrowthSymbiosisRate"); }
    }

    private float wildGrowthEffectiveHealing = 0.8f;
    public float WildGrowthEffectiveHealing
    {
        get { return wildGrowthEffectiveHealing; }
        set { wildGrowthEffectiveHealing = value; OnPropertyChanged("WildGrowthEffectiveHealing"); }
    }

    private float tranquilitySymbiosisRate = 0.1f;
    public float TranquilitySymbiosisRate
    {
        get { return tranquilitySymbiosisRate; }
        set { tranquilitySymbiosisRate = value; OnPropertyChanged("TranquilitySymbiosisRate"); }
    }

    private float raidSTSymbiosisRate = 0.4f;
    public float RaidSTSymbiosisRate
    {
        get { return raidSTSymbiosisRate; }
        set { raidSTSymbiosisRate = value; OnPropertyChanged("RaidSTSymbiosisRate"); }
    }

    private float livingSeedEffectiveHealing = 0.2f;
    public float LivingSeedEffectiveHealing
    {
        get { return livingSeedEffectiveHealing; }
        set { livingSeedEffectiveHealing = value; OnPropertyChanged("LivingSeedEffectiveHealing"); }
    }

    private float toLLifebloomEffectiveHealing = 0.7f;
    public float ToLLifebloomEffectiveHealing
    {
        get { return toLLifebloomEffectiveHealing; }
        set { toLLifebloomEffectiveHealing = value; OnPropertyChanged("ToLLifebloomEffectiveHealing"); }
    }

    private float rejuvenationEffectiveHealing = 0.7f;
    public float RejuvenationEffectiveHealing
    {
        get { return rejuvenationEffectiveHealing; }
        set { rejuvenationEffectiveHealing = value; OnPropertyChanged("RejuvenationEffectiveHealing"); }
    }

    private float healingTouchEffectiveHealing = 0.85f;
    public float HealingTouchEffectiveHealing
    {
        get { return healingTouchEffectiveHealing; }
        set { healingTouchEffectiveHealing = value; OnPropertyChanged("HealingTouchEffectiveHealing"); }
    }

    private float nourishEffectiveHealing = 1.0f;
    public float NourishEffectiveHealing
    {
        get { return nourishEffectiveHealing; }
        set { nourishEffectiveHealing = value; OnPropertyChanged("NourishEffectiveHealing"); }
    }

    private float raidUnevenlyAllocatedFillerMana = 1.0f;
    public float RaidUnevenlyAllocatedFillerMana
    {
        get { return raidUnevenlyAllocatedFillerMana; }
        set { raidUnevenlyAllocatedFillerMana = value; OnPropertyChanged("RaidUnevenlyAllocatedFillerMana"); }
    }

    private float tankUnevenlyAllocatedFillerMana = 1.0f;
    public float TankUnevenlyAllocatedFillerMana
    {
        get { return tankUnevenlyAllocatedFillerMana; }
        set { tankUnevenlyAllocatedFillerMana = value; OnPropertyChanged("TankUnevenlyAllocatedFillerMana"); }
    }

    private float tankRaidHealingWeight = 0.0f;
    public float TankRaidHealingWeight
    {
        get { return tankRaidHealingWeight; }
        set { tankRaidHealingWeight = value; OnPropertyChanged("TankRaidHealingWeight"); }
    }

    private bool harmony = false;
    public bool Harmony
    {
        get { return harmony; }
        set { harmony = value; OnPropertyChanged("Harmony"); }
    }

    private double harmonyCoefficient = 0.0125;
    public double HarmonyCoefficient
    {
        get { return harmonyCoefficient; }
        set { harmonyCoefficient = value; OnPropertyChanged("HarmonyCoefficient"); }
    }

    private bool crit100Bonus = false;
    public bool Crit100Bonus
    {
        get { return crit100Bonus; }
        set { crit100Bonus = value; OnPropertyChanged("Crit100Bonus"); }
    }

    private bool t12_2p = false;
    public bool T12_2P
    {
        get { return t12_2p; }
        set { t12_2p = value; OnPropertyChanged("T12_2P"); }
    }

    private bool t12_4p_swiftmend = false;
    public bool T12_4P_Swiftmend
    {
        get { return t12_4p_swiftmend; }
        set { t12_4p_swiftmend = value; OnPropertyChanged("T12_4P_Swiftmend"); }
    }

    private double swiftmendExtraHealEffectiveHealing = 2.0;
    public double SwiftmendExtraHealEffectiveHealing
    {
        get { return swiftmendExtraHealEffectiveHealing; }
        set { swiftmendExtraHealEffectiveHealing = value; OnPropertyChanged("SwiftmendExtraHealEffectiveHealing"); }
    }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
