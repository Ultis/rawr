using System;
using System.Collections.Generic;

namespace Rawr.RestoSham
  {
    [System.ComponentModel.DisplayName("RestoSham|Spell_Nature_Magicimmunity")]
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
                    _subpointColors = new Dictionary<string,System.Drawing.Color>();
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
                          "Basic Stats:Healing",
                          "Basic Stats:MP5 (in FSR)*Mana regeneration while casting (inside the 5-second rule)",
                          "Basic Stats:MP5 (outside FSR)*Mana regeneration while not casting (outside the 5-second rule)",
                          "Basic Stats:Heal Spell Crit",
                          "Basic Stats:Spell Haste",
                          "Fight Details:Total Mana Pool",
                          "Fight Details:Average Heal",
                          "Fight Details:Average Cast Time",
                          "Fight Details:Average Mana Cost",
                          "Fight Details:Total Healed",
                          "Fight Details:Fight HPS" };
                  }
                return _characterDisplayCalcLabels;
              }
          }


        //
        // Custom chart names:
        //
        public override string[] CustomChartNames
          {
            get { return new string[]{"Healing Spell Ranks",
                                      "Stat Relative Weights"}; }
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
            calcStats.SpellCrit = .022f + ((stats.Intellect / 80f) / 100) + ((stats.SpellCritRating / 22.08f) / 100) +
                                  stats.SpellCrit;
          
            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;
            
            // Total Mana Pool for the fight:
            
            float onUse = 0.0f;
            if (options.ManaPotTime > 0)
              onUse += (float)Math.Truncate(options.FightLength / options.ManaPotTime) *
                        (options.ManaPotAmount * (1 + stats.BonusManaPotion));
            if (options.ManaTideEveryCD)
              onUse += ((float)Math.Truncate(options.FightLength / 5.025f) + 1) * (stats.Mana * .24f);
            
            float mp5 = (stats.Mp5  * (1f - (options.OutsideFSRPct / 100f)));
            mp5 += (calcStats.Mp5OutsideFSR * (options.OutsideFSRPct / 100f));
            mp5 += options.SPriestMP5;
            if (character.ActiveBuffsContains("Mana Spring Totem"))
              {
                int points = GetTalentPoints("Restorative Totems", "Restoration", character.Talents);
                mp5 += 50f * (points * .05f);
                
                mp5 += stats.ManaSpringMp5Increase;
              }
            
            calcStats.TotalManaPool = stats.Mana + onUse + (mp5 * (60f / 5f) * options.FightLength);
            
            // Get a list of the heals we're casting, and assign relative weights to them:
            
            List<HealSpell> list = new List<HealSpell>();
            float totalRatio = options.HWRatio + options.LHWRatio + options.CHRatio;
            
            if (options.LHWRatio > 0)
              {
                LesserHealingWave lhw = new LesserHealingWave();
                lhw.Calcluate(stats, character);
                lhw.Weight = options.LHWWeight;
                list.Add(lhw);
              }
              
            if (options.HWRatio > 0)
              {
                if (options.HWDownrank.Ratio > 0)
                  {
                    HealingWave hw = new HealingWave(options.HWDownrank.MaxRank);
                    hw.Calcluate(stats, character);
                    hw.Weight = options.HWWeight * (options.HWDownrank.Ratio / 100f);
                    list.Add(hw);
                  }
                if (options.HWDownrank.Ratio < 100)
                  {
                    HealingWave hw = new HealingWave(options.HWDownrank.MinRank);
                    hw.Calcluate(stats, character);
                    hw.Weight = options.HWWeight * ((100 - options.HWDownrank.Ratio) / 100f);
                    list.Add(hw);
                  }
              }

            if (options.CHRatio > 0)
              {
                if (options.CHDownrank.Ratio > 0)
                  {
                    ChainHeal ch = new ChainHeal(options.CHDownrank.MaxRank);
                    ch.Calcluate(stats, character);
                    ch.Weight = options.CHWeight * (options.CHDownrank.Ratio / 100f);
                    list.Add(ch);
                  }
				if (options.CHDownrank.Ratio < 100)
                  {
                    ChainHeal ch = new ChainHeal(options.CHDownrank.MinRank);
                    ch.Calcluate(stats, character);
                    ch.Weight = options.CHWeight * ((100 - options.CHDownrank.Ratio) / 100f);
                    list.Add(ch);
                  }
              }
            
            // Now get weighted average heal, weighted average mana cost, and weighted average cast time:
            
            calcStats.AverageHeal = 0;
            calcStats.AverageManaCost = 0;
            calcStats.AverageCastTime = 0;
            foreach (HealSpell spell in list)
              {
                calcStats.AverageHeal += spell.AverageHealed * spell.Weight;
                calcStats.AverageCastTime += spell.CastTime * spell.Weight;
                calcStats.AverageManaCost += spell.ManaCost * spell.Weight;
              }
            calcStats.AverageManaCost -= stats.ManaRestorePerCast_5_15 * .02f;  // Insightful Earthstorm Diamond
            
            // Earth Shield computations:
            
            float esMana = 0f;
            float esHeal = 0f;
            float esNum = 0f;
            if (options.ESInterval > 0)
              {
                EarthShield es = new EarthShield(options.ESRank);
                es.Calcluate(stats, character);
                esNum = (float)Math.Round((options.FightLength / (options.ESInterval / 60f)) + 1, 0);
                esMana = es.ManaCost * esNum;
                esHeal = es.AverageHealed * esNum;
              }
            float numHeals = Math.Min((options.FightLength * 60) / calcStats.AverageCastTime, (calcStats.TotalManaPool - esMana) / calcStats.AverageManaCost);
            
            // Now, Shattered Sun Pendant of Restoration Aldor proc.  From what I understand, this has a
            //  4% proc rate with no cooldown. This is a rough estimation of the value of this proc, and
            //  doesn't take into account other things that might proc it (Gift of the Naaru, Earth Shield
            //  if cast on the shaman, etc):
            
            if (options.ExaltedFaction == Faction.Aldor && stats.ShatteredSunRestoProc > 0)
              {
                // Determine how many more "casts" we get from Chain Heal second / third targets
                //  and Earth Shield (assumption is Earth Shield procs it?):
                
                float ssNumHeals = numHeals;
                if (options.CHRatio > 0)
                  ssNumHeals += numHeals * options.CHWeight * (options.NumCHTargets - 1);
                ssNumHeals += esNum;
                float ssHeal = (((ssNumHeals * .04f) * 10f) / (options.FightLength * 60f)) * 220f;
                
                // Now, we have to recalculate the amount healed based on the proc's addition to healing bonus:
                
                stats.Healing += ssHeal;
                calcStats.AverageHeal = 0f;
                foreach (HealSpell spell in list)
                  {
                    spell.Calcluate(stats, character);
                    calcStats.AverageHeal += spell.AverageHealed * spell.Weight;
                  }
                stats.Healing -= ssHeal;
              }
            
            calcStats.TotalHealed = (numHeals * calcStats.AverageHeal) + esHeal;
            
            // Shattered Sun Pendant of Restoration Scryers proc. This is going to be a best case
            //  scenario (procs every cooldown, and the proc actually heals someone. Healers using
            //  a mouseover healing system (like Clique) might not benefit at all from this proc):
            
            if (options.ExaltedFaction == Faction.Scryers && stats.ShatteredSunRestoProc > 0)
              {
                float numProcs = (float)Math.Round((options.FightLength / 60f) / 45f, 0) + 1f;
                float crit = .022f + ((stats.Intellect / 80f) / 100) + ((stats.SpellCritRating / 22.08f) / 100) +
                                  stats.SpellCrit;
                float critRate = 1 + 0.5f * crit;
                float ssHealed = numProcs * 650 * critRate;
                
                calcStats.TotalHealed += ssHealed;
              }
            
            calcStats.FightHPS = calcStats.TotalHealed / (options.FightLength * 60);
            
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
            Stats  statsRace;
            switch (character.Race)
              {
                case Character.CharacterRace.Draenei:
                  statsRace = new Stats() { Health = 3159, Mana = 2958, Stamina = 113, Intellect = 109, Spirit = 122 };
                  break;
                  
                case Character.CharacterRace.Tauren:
                  statsRace = new Stats() { Health = 3159, Mana = 2958, Stamina = 116, Intellect = 103, Spirit = 122 };
                  statsRace.BonusHealthMultiplier = 0.05f;
                  break;
                  
                case Character.CharacterRace.Orc:
                  statsRace = new Stats() { Health = 3159, Mana = 2958, Stamina = 116, Intellect = 105, Spirit = 123 };
                  break;
                  
                case Character.CharacterRace.Troll:
                  statsRace = new Stats() { Health = 3159, Mana = 2958, Stamina = 115, Intellect = 104, Spirit = 121 };
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
            statsTotal.Intellect = (float)Math.Round((statsTotal.Intellect)) * (1 + statsTotal.BonusIntellectMultiplier);
            statsTotal.Spirit = (float)Math.Round((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Healing = (float)Math.Round(statsTotal.Healing);
            statsTotal.Mana = statsTotal.Mana + 20 + ((statsTotal.Intellect - 20) * 15);
            statsTotal.Health = (statsTotal.Health + 20 + ((statsTotal.Stamina - 20) * 10f)) * (1 + statsTotal.BonusHealthMultiplier);
          
            // Apply talents to stats:
            
            ApplyTalents(statsTotal, character.Talents);
            
            // Fight options:
            
            CalculationOptionsRestoSham options = character.CalculationOptions as CalculationOptionsRestoSham;
            statsTotal.Mp5 += (options.WaterShield ? 50 : 0);
          
            return statsTotal;
          }


        /// <summary>
        /// Adjust a stats object according to any applicable talents.
        /// </summary>
        private void ApplyTalents(Stats statsTotal, TalentTree talentTree)
          {
            int points;
            
            // Unrelenting Storm: Gives 2% (per talent point) of intellect as mp5:
            
            points = GetTalentPoints("Unrelenting Storm", "Elemental", talentTree);
            statsTotal.Mp5 += (float)Math.Round((statsTotal.Intellect * .02f * points), 0);
          
            // Tidal Mastery: Increases crit chance of heals by 1% per talent point:
            
            points = GetTalentPoints("Tidal Mastery", "Restoration", talentTree);
            statsTotal.SpellCrit += .01f * points;
              
            // Nature's Blessing: Adds 10% (per talent point) of intellect as bonus healing:
            
            points = GetTalentPoints("Nature's Blessing", "Restoration", talentTree);
            statsTotal.Healing += (float)Math.Round((statsTotal.Intellect * .1f * points), 0);
          
            // Ancestral Knowledge: Increases total mana by 1% per talent point.
            
            points = GetTalentPoints("Ancestral Knowledge", "Enhancement", talentTree);
            statsTotal.Mana += statsTotal.Mana * (.01f * points);
          }


        /// <summary>
        /// Search a talent tree for a specified talent and get the number of points invested in that talent.
        /// </summary>
        public static int GetTalentPoints(string szName, string szTree, TalentTree talents)
          {
            if (!talents.Trees.ContainsKey(szTree))
              return 0;
              
            foreach (TalentItem ti in talents.Trees[szTree])
              if (ti.Name.Trim().ToLower() == szName.ToLower())
                return ti.PointsInvested;
                
            return 0;
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
            
            public Stats  Stat;
            public string Name;
            public float  PctChange;
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
                      new StatRelativeWeight("+Heal", new Stats() { Healing = 1f }),
                      new StatRelativeWeight("Mp5", new Stats() { Mp5 = 1f }),
                      new StatRelativeWeight("Spell Crit", new Stats() { SpellCritRating = 1f })};
                      
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
                
                case "Healing Spell Ranks":
                  // Healing Wave ranks:
                  
                  for (int i = 1; i <= 12; i++)
                    {
                      HealingWave hw = new HealingWave(i);
                      hw.Calcluate(calc.BasicStats, character);
                      ComparisonCalculationRestoSham comp = new ComparisonCalculationRestoSham(hw.FullName);
                      comp.OverallPoints = hw.AverageHealed + hw.HealingWay;
                      comp.SubPoints[0] = hw.AverageHealed;
                      comp.SubPoints[3] = hw.HealingWay;
                      list.Add(comp);
                    }
                    
                  // Lesser Healing Wave ranks:
                  
                  for (int i = 1; i <= 7; i++)
                    {
                      LesserHealingWave lhw = new LesserHealingWave(i);
                      lhw.Calcluate(calc.BasicStats, character);
                      ComparisonCalculationRestoSham comp = new ComparisonCalculationRestoSham(lhw.FullName);
                      comp.OverallPoints = comp.SubPoints[0] = lhw.AverageHealed;
                      list.Add(comp);
                    }
                  
                  // Chain Heal ranks:
                  
                  for (int i = 1; i <= 5; i++)
                    {
                      ChainHeal ch = new ChainHeal(i);
                      ch.Calcluate(calc.BasicStats, character);
                      ComparisonCalculationRestoSham comp = new ComparisonCalculationRestoSham(ch.FullName);
                      comp.OverallPoints = ch.TotalHealed;
                      for (int j = 0; j < 3; j++)
                        comp.SubPoints[j] = ch.HealsOnTargets[j];
                      list.Add(comp);
                    }
                  
                  // The Draenei racial:
                  
                  if (character.Race == Character.CharacterRace.Draenei)
                    {
                      GiftOfTheNaaru gift = new GiftOfTheNaaru();
                      gift.Calcluate(calc.BasicStats, character);
                      ComparisonCalculationRestoSham comp = new ComparisonCalculationRestoSham(gift.FullName);
                      comp.OverallPoints = comp.SubPoints[0] = gift.AverageHealed;
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
                Healing = stats.Healing,
                SpellCritRating = stats.SpellCritRating,
                SpellHasteRating = stats.SpellHasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                BonusManaPotion = stats.BonusManaPotion,
                ManaSpringMp5Increase = stats.ManaSpringMp5Increase,
                LHWManaReduction = stats.LHWManaReduction,
                CHHealIncrease = stats.CHHealIncrease,
                CHManaReduction = stats.CHManaReduction,
                ManaRestorePerCast_5_15 = stats.ManaRestorePerCast_5_15,
                ShatteredSunRestoProc = stats.ShatteredSunRestoProc
              };
          }


        public override bool HasRelevantStats(Stats stats)
          {
            return (stats.Stamina + stats.Intellect + stats.Spirit + stats.Mp5 + stats.Healing + stats.SpellCritRating +
                    stats.SpellHasteRating + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier +
                    stats.BonusManaPotion + stats.CHManaReduction + stats.CHHealIncrease + stats.LHWManaReduction +
                    stats.ManaSpringMp5Increase + stats.ManaRestorePerCast_5_15 + stats.ShatteredSunRestoProc) > 0;
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
