using System;
using System.Collections.Generic;

namespace Rawr.RestoSham
{
    [Rawr.Calculations.RawrModelInfo("RestoSham", "Spell_Nature_Magicimmunity", Character.CharacterClass.Shaman)]
    class CalculationsRestoSham : CalculationsBase
    {
        //
        // Colors of the ratings we track:
        //
        private Dictionary<string, System.Drawing.Color> _subpointColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subpointColors == null)
                {
                    _subpointColors = new Dictionary<string, System.Drawing.Color>();
                    _subpointColors.Add("Healing", System.Drawing.Color.Green);
                    _subpointColors.Add("CH Target 2", System.Drawing.Color.Blue);
                    _subpointColors.Add("CH Target 3", System.Drawing.Color.Red);
                    _subpointColors.Add("Healing Way", System.Drawing.Color.Magenta);
                }
                return _subpointColors;
            }
        }


        //
        // Character calulcations display labels:
        //
        private string[] _characterDisplayCalcLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalcLabels == null)
                {
                    _characterDisplayCalcLabels = new string[] {
                          "Basic Stats:Health",
                          "Basic Stats:Mana",
                          "Basic Stats:Stamina",
                          "Basic Stats:Intellect",
                          "Basic Stats:Spirit",
                          "Basic Stats:Spell Power",
                          "Basic Stats:MP5 (in FSR)*Mana regeneration while casting (inside the 5-second rule)",
                          "Basic Stats:MP5 (outside FSR)*Mana regeneration while not casting (outside the 5-second rule)",
                          "Basic Stats:Heal Spell Crit",
                          "Basic Stats:Spell Haste",
                          "Fight Details:Fight HPS" ,
                          "Fight Details:ES + LHW HPS",
                          "Fight Details:ES + LHW OOM*Seconds into the fight you are expected to go Out of Mana",
                          "Fight Details:ES + HW HPS",
                          "Fight Details:ES + HW OOM*Seconds into the fight you are expected to go Out of Mana",
                          "Fight Details:ES + CH HPS",
                          "Fight Details:ES + CH OOM*Seconds into the fight you are expected to go Out of Mana",
                          "Fight Details:ES + RT + CHx2 HPS",
                          "Fight Details:ES + RT + CHx2 OOM*Seconds into the fight you are expected to go Out of Mana" };
                }
                return _characterDisplayCalcLabels;
            }
        }


        //
        // Custom chart names:
        //
        public override string[] CustomChartNames
        {
            get
            {
                return new string[]{"Healing Spell Ranks",
                                      "Stat Relative Weights"};
            }
        }


        //
        // Calculations options panel:
        //
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                    _calculationOptionsPanel = new CalculationOptionsPanelRestoSham();
                return _calculationOptionsPanel;
            }
        }


        //
        // Item types we're interested in:
        //
        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[] {
                         Item.ItemType.None,
                         Item.ItemType.Cloth,
                         Item.ItemType.Leather,
                         Item.ItemType.Mail,
                         Item.ItemType.Totem,
                         Item.ItemType.OneHandMace,
                         Item.ItemType.OneHandAxe,
                         Item.ItemType.Shield,
                         Item.ItemType.Staff,
                         Item.ItemType.Dagger });
                }
                return _relevantItemTypes;
            }
        }


        //
        // This model is for shammies!
        //
        public override Character.CharacterClass TargetClass
        {
            get { return Character.CharacterClass.Shaman; }
        }


        //
        // Get instances of our calculation classes:
        //
        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationRestoSham();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsRestoSham();
        }


        //
        // Do the actual calculations:
        //
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            return GetCharacterCalculations(character, additionalItem, null);
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem,
                                                                  Stats statModifier)
        {
            Stats stats = GetCharacterStats(character, additionalItem, statModifier);
            CharacterCalculationsRestoSham calcStats = new CharacterCalculationsRestoSham();
            calcStats.BasicStats = stats;

            calcStats.Mp5OutsideFSR = 5f * (.001f + (float)Math.Sqrt((double)stats.Intellect) * stats.Spirit * .009327f) + stats.Mp5;
            calcStats.SpellCrit = .022f + character.StatConversion.GetSpellCritFromIntellect(stats.Intellect) / 100f
                + character.StatConversion.GetSpellCritFromRating(stats.CritRating) / 100f + stats.SpellCrit;

            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;

            // Total Mana Pool for the fight:

            float onUse = 0.0f;
            if (options.ManaPotAmount > 0)
                onUse += (options.ManaPotAmount * (1 + stats.BonusManaPotion)) / (options.FightLength * 60 / 5);
            if (options.ManaTideEveryCD)
                onUse += (((float)Math.Truncate(options.FightLength / 5.025f) + 1) *
                    (stats.Mana * (.24f + ((options.ManaTidePlus ? .04f : 0))))) * character.ShamanTalents.ManaTideTotem;

            float mp5 = (stats.Mp5 * (1f - (options.OutsideFSRPct / 100f)));
            mp5 += (calcStats.Mp5OutsideFSR * (options.OutsideFSRPct / 100f));
            mp5 += (float)Math.Round((stats.Intellect * ((character.ShamanTalents.UnrelentingStorm / 3) * .1f)), 0);
            calcStats.TotalManaPool = stats.Mana + onUse + (mp5 * (60f / 5f) * options.FightLength) +
                ((stats.ManaRestoreFromMaxManaPerSecond * stats.Mana) * ((options.FightLength * 60f)) * .85f);
            if (character.ActiveBuffsContains("Earthliving Weapon"))
                stats.SpellPower += (character.ShamanTalents.ElementalWeapons * .01f * 150f);


            // This is my new calcs, still testing:
            float CurrentMana = calcStats.TotalManaPool;
            if (options.ESInterval < 32)
                options.ESInterval = 32;
            float Awaken = (.1f * character.ShamanTalents.AncestralAwakening);
            float TankCH = (options.TankHeal ? 1 : 1.75f);
            float Orb = 400 * (1 + (character.ShamanTalents.ImprovedShields * .05f));
            float Orbs = 3 + (options.WaterShield2 ? 1 : 0);
            float Redux = (1f - ((character.ShamanTalents.TidalFocus) * .01f));
            float Time = (options.FightLength * 60f);
            float Critical = 1f + ((calcStats.SpellCrit + stats.BonusCritHealMultiplier) / 2f) + (.01f * (character.ShamanTalents.TidalMastery + character.ShamanTalents.ThunderingStrikes + (character.ShamanTalents.BlessingOfTheEternals * 2)));
            float Purify = (1f + ((character.ShamanTalents.Purification) * .02f));
            float Healing = 1.88f * stats.SpellPower;
            float ESC = ((((Time / options.ESInterval) * (((2022f + (Healing * 3f)) * (1f + (.05f * (character.ShamanTalents.ImprovedShields + character.ShamanTalents.ImprovedEarthShield)))) / 6f * (6f + character.ShamanTalents.ImprovedEarthShield))) / Time) * Purify);
            float ESCMPS = (((Time / options.ESInterval) * (660f * Redux)));
            float Hasted = 1 - (stats.HasteRating / 3279);
            float LHWC = 1.5f + (((1.5f * Hasted) / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield * .01f));
            float HWC = (3f - (.1f * character.ShamanTalents.ImprovedHealingWave)) + (((1.5f * Hasted) / 3 * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3));
            float CHC = 2.5f;
            float RTC = 1.5f + (((1.5f * Hasted) / Orbs * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3));
            float LHWM = (550 * Redux) - ((Orb * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield * .01f));
            float HWM = (1099 * Redux) - ((Orb * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3));
            float CHM = (835 * Redux);
            float RTM = (792 * Redux) - ((Orb * stats.SpellCrit) * (character.ShamanTalents.ImprovedWaterShield / 3));
            float EFL = Time - (1.5f * (Time / options.ESInterval));
            float LHWHeal = (((1720f + (Healing * (LHWC / 3.5f))) * Purify) * (Critical + Awaken)) * ((options.LHWPlus ? (options.TankHeal ? 1.2f : 1) : 1));
            float HWHeal = ((3250f + (Healing * (HWC / 3.5f))) * Purify) * (Critical + Awaken);
            float CHHeal = (((1130f + (Healing * (CHC / 3.5f))) * (1f + (((character.ShamanTalents.ImprovedChainHeal / 2f)) * .02f)) * Purify) * Critical) * TankCH;
            float CHRTHeal = (((1130f + (Healing * (CHC / 3.5f))) * (1f + (((character.ShamanTalents.ImprovedChainHeal / 2f)) * .02f)) * Purify) * 1.2f * Critical) * TankCH;
            float RTHeal = ((1670f + (Healing * .5f)) * Purify) * (Critical + Awaken);
            float RTHot = ((1670f + (Healing * .5f)) * Purify) / 15f;
            float ESLHWMPSMT = (ESCMPS + ((EFL / (LHWC * Hasted)) * LHWM)) / Time;
            float ESHWMPSMT = (ESCMPS + ((EFL / ((HWC) * Hasted)) * HWM)) / Time;
            float ESCHMPSMT = (ESCMPS + ((EFL / (CHC * Hasted)) * CHM)) / Time;
            float RTMPSMT = (ESCMPS + ((EFL / (RTC * Hasted)) * RTM)) / Time; ;

            // HPS Calcs
            calcStats.ESLHWHPSMT = (ESC + ((LHWHeal * (EFL / (LHWC * Hasted))) / EFL)) * (Math.Min(((CurrentMana / ESLHWMPSMT) / Time), 1));
            calcStats.ESHWHPSMT = (ESC + ((HWHeal * (EFL / ((HWC) * Hasted))) / EFL)) * (Math.Min(((CurrentMana / ESHWMPSMT) / Time), 1));
            calcStats.ESCHHPSMT = (ESC + ((CHHeal * (EFL / (CHC * Hasted))) / EFL)) * (Math.Min(((CurrentMana / ESCHMPSMT) / Time), 1));
            calcStats.ESRTCHCHHPSMT = ((ESC + ((RTHeal * (EFL / (LHWC * Hasted))) / EFL)) * (Math.Min(((CurrentMana / RTMPSMT) / Time), 1)) / 3) + (calcStats.ESCHHPSMT / 3 * 2);

            // MPS Calcs
            calcStats.ESLHWMPSMT = CurrentMana / ESLHWMPSMT;
            calcStats.ESHWMPSMT = CurrentMana / ESHWMPSMT;
            calcStats.ESCHMPSMT = CurrentMana / ESCHMPSMT;
            calcStats.ESRTCHCHMPSMT = CurrentMana / ((RTMPSMT + ESCHMPSMT + ESCHMPSMT) / 3);


            // Calculate Best HPS
            if (calcStats.ESCHHPSMT > calcStats.ESHWHPSMT)
                if (calcStats.ESCHHPSMT > calcStats.ESLHWHPSMT)
                    if (calcStats.ESCHHPSMT > calcStats.ESRTCHCHHPSMT)
                        calcStats.FightHPS = calcStats.ESCHHPSMT;
            if (calcStats.ESHWHPSMT > calcStats.ESCHHPSMT)
                if (calcStats.ESHWHPSMT > calcStats.ESLHWHPSMT)
                    if (calcStats.ESHWHPSMT > calcStats.ESRTCHCHHPSMT)
                        calcStats.FightHPS = calcStats.ESHWHPSMT;
            if (calcStats.ESLHWHPSMT > calcStats.ESCHHPSMT)
                if (calcStats.ESLHWHPSMT > calcStats.ESHWHPSMT)
                    if (calcStats.ESLHWHPSMT > calcStats.ESRTCHCHHPSMT)
                        calcStats.FightHPS = calcStats.ESLHWHPSMT;
            if (calcStats.ESRTCHCHHPSMT > calcStats.ESHWHPSMT)
                if (calcStats.ESRTCHCHHPSMT > calcStats.ESLHWHPSMT)
                    if (calcStats.ESRTCHCHHPSMT > calcStats.ESCHHPSMT)
                        calcStats.FightHPS = calcStats.ESRTCHCHHPSMT;

            calcStats.TotalHealed = calcStats.FightHPS * (options.FightLength * 60f);
            calcStats.OverallPoints = calcStats.TotalHealed / 10f;
            calcStats.SubPoints[0] = calcStats.TotalHealed / 10f;

            return calcStats;
        }


        //
        // Create the statistics for a given character:
        //
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, null);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats statModifier)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 135, Intellect = 141, Spirit = 145 };
                    break;

                case Character.CharacterRace.Tauren:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 138, Intellect = 135, Spirit = 145 };
                    statsRace.BonusHealthMultiplier = 0.05f;
                    break;

                case Character.CharacterRace.Orc:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 138, Intellect = 137, Spirit = 146 };
                    break;

                case Character.CharacterRace.Troll:
                    statsRace = new Stats() { Health = 6485, Mana = 4396, Stamina = 137, Intellect = 136, Spirit = 144 };
                    break;

                default:
                    statsRace = new Stats();
                    break;
            }

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;
            if (statModifier != null)
                statsTotal += statModifier;

            statsTotal.Stamina = (float)Math.Round((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Round((statsTotal.Intellect)) * (1 + (statsTotal.BonusIntellectMultiplier + (.02f * character.ShamanTalents.AncestralKnowledge)));
            statsTotal.Spirit = (float)Math.Round((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower) + (float)Math.Round((statsTotal.Intellect * .05f * character.ShamanTalents.NaturesBlessing), 0);
            statsTotal.Mana = statsTotal.Mana + 20 + ((statsTotal.Intellect - 20) * 15);
            statsTotal.Health = (statsTotal.Health + 20 + ((statsTotal.Stamina - 20) * 10f)) * (1 + statsTotal.BonusHealthMultiplier);


            // Fight options:

            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;
            float OrbRegen = (options.WaterShield3 ? 130 : 100);
            statsTotal.Mp5 += (options.WaterShield ? OrbRegen : 0);

            return statsTotal;
        }


        //
        // Class used by stat relative weights custom chart.
        //
        class StatRelativeWeight
        {
            public StatRelativeWeight(string name, Stats stat)
            {
                this.Stat = stat;
                this.Name = name;
            }

            public Stats Stat;
            public string Name;
            public float PctChange;
        }


        //
        // Data for custom charts:
        //
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            CharacterCalculationsRestoSham calc = GetCharacterCalculations(character) as CharacterCalculationsRestoSham;
            if (calc == null)
                calc = new CharacterCalculationsRestoSham();

            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;
            if (options == null)
                options = new CalculationOptionsRestoSham();

            List<ComparisonCalculationBase> list = new List<ComparisonCalculationBase>();
            switch (chartName)
            {
                case "Stat Relative Weights":
                    StatRelativeWeight[] stats = new StatRelativeWeight[] {
                      new StatRelativeWeight("Int", new Stats() { Intellect = 1f }),
                      new StatRelativeWeight("Spirit", new Stats() { Spirit = 1f }),
                      new StatRelativeWeight("+Heal", new Stats() { SpellPower = 1f}),
                      new StatRelativeWeight("Mp5", new Stats() { Mp5 = 1f }),
                      new StatRelativeWeight("Spell Crit", new Stats() { CritRating = 1f })};

                    // Get the percentage total healing is changed by a change in a single stat:

                    float healPct = 0f;
                    foreach (StatRelativeWeight weight in stats)
                    {
                        CharacterCalculationsRestoSham statCalc = (CharacterCalculationsRestoSham)GetCharacterCalculations(character, null, weight.Stat);
                        weight.PctChange = (statCalc.TotalHealed - calc.TotalHealed) / calc.TotalHealed;
                        if (weight.Name == "+Heal")
                            healPct = weight.PctChange;
                    }

                    // Create the chart data points:

                    foreach (StatRelativeWeight weight in stats)
                    {
                        ComparisonCalculationRestoSham comp = new ComparisonCalculationRestoSham(weight.Name);
                        comp.OverallPoints = weight.PctChange / healPct;
                        comp.SubPoints[0] = comp.OverallPoints;
                        list.Add(comp);
                    }

                    break;


            }

            ComparisonCalculationBase[] retVal = new ComparisonCalculationBase[list.Count];
            if (list.Count > 0)
                list.CopyTo(retVal);
            return retVal;
        }


        //
        // Get stats which are relevant to resto shammies:
        //
        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                BonusManaPotion = stats.BonusManaPotion,
                ManaSpringMp5Increase = stats.ManaSpringMp5Increase,
                ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier
            };
        }


        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellPower + stats.CritRating +
                    stats.HasteRating + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier + stats.BonusCritHealMultiplier +
                    stats.BonusManaPotion + stats.ManaSpringMp5Increase + stats.ManaRestoreOnCast_5_15 + stats.ManaRestoreFromMaxManaPerSecond) > 0;
        }


        //
        // Retrieve our options from XML:
        //
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                            new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRestoSham));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsRestoSham calcOpts = serializer.Deserialize(reader) as CalculationOptionsRestoSham;
            return calcOpts;
        }
    }
}
