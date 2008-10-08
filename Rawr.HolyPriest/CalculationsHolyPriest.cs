using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.HolyPriest
{
	[Rawr.Calculations.RawrModelInfo("HolyPriest", "Spell_Holy_Renew", Character.CharacterClass.Priest)]
	public class CalculationsHolyPriest : CalculationsBase 
    {
        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Priest; } }

        private string _currentChartName = null;
        
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                switch (_currentChartName)
                {
                    case "Spell HpS":
                        _subPointNameColors.Add("HpS", System.Drawing.Color.Red);
                        break;
                    case "Spell HpM":
                        _subPointNameColors.Add("HpM", System.Drawing.Color.Red);
                        break;
                    default:
                        _subPointNameColors.Add("HPS-Burst", System.Drawing.Color.Red);
                        _subPointNameColors.Add("HPS-Sustained", System.Drawing.Color.Blue);
                        _subPointNameColors.Add("Survivability", System.Drawing.Color.Green);
                        break;
                }
               
                return _subPointNameColors;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Health",
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
                    "Basic Stats:Spell Power",
					"Basic Stats:Mp5",
					"Basic Stats:Regen InFSR",
					"Basic Stats:Regen OutFSR",
                    "Basic Stats:Spell Crit",
					"Basic Stats:Healing Crit",
					"Basic Stats:Spell Haste",
                    "Basic Stats:Global Cooldown",
                    "Spells:Greater Heal",
                    "Spells:Flash Heal",
				    "Spells:Binding Heal",
                    "Spells:Renew",
                    "Spells:Prayer of Mending",
                    "Spells:Power Word Shield",
                    "Spells:PoH",
				    "Spells:Holy Nova",
                    "Spells:Lightwell",
				    "Spells:CoH",
                    "Spells:Penance",
                    "Spells:Gift of the Naaru"
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelHolyPriest();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { "Spell HpS", "Spell HpM", "Spell AoE HpS", "Spell AoE HpM", "Relative Stat Values" };
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHolyPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHolyPriest(); }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]{
                        Item.ItemType.None,
                        Item.ItemType.Cloth,
                        Item.ItemType.Dagger,
                        Item.ItemType.Wand,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        private float SimulateSimple(Spell heal)
        {
            return 0;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            Stats statsRace = GetRaceStats(character);
            CharacterCalculationsHolyPriest calculatedStats = new CharacterCalculationsHolyPriest();
            CalculationOptionsPriest calculationOptions = character.CalculationOptions as CalculationOptionsPriest;
            if (calculationOptions == null)
                return null;

            calculatedStats.Race = character.Race;
            calculatedStats.BasicStats = stats;
            calculatedStats.Character = character;

            // Trinkets that give average stat increases go here:

            calculatedStats.BasicStats.Stamina = (float)Math.Floor(calculatedStats.BasicStats.Stamina * (1 + character.PriestTalents.Enlightenment * 0.01f));
            calculatedStats.BasicStats.Health = (float)Math.Floor(calculatedStats.BasicStats.Stamina * 10f + statsRace.Health);
            calculatedStats.BasicStats.Spirit = (float)Math.Floor(calculatedStats.BasicStats.Spirit * (1 + character.PriestTalents.SpiritOfRedemption * 0.05f) * (1 + character.PriestTalents.Enlightenment * 0.01f));
            calculatedStats.BasicStats.Intellect = (float)Math.Floor(calculatedStats.BasicStats.Intellect * (1 + character.PriestTalents.MentalStrength * 0.03f));
            calculatedStats.BasicStats.Mana = (float)Math.Floor((calculatedStats.BasicStats.Intellect - 20f) * 15f + 20f + statsRace.Mana);
            calculatedStats.BasicStats.SpellCrit = (calculatedStats.BasicStats.Intellect / 80f) +
                (calculatedStats.BasicStats.CritRating / 22.08f) + 1.24f;
            calculatedStats.BasicStats.SpellHaste += character.PriestTalents.Enlightenment * 1f;
            calculatedStats.BasicStats.SpellPower += calculatedStats.BasicStats.Spirit * character.PriestTalents.SpiritualGuidance * 0.05f;
          
            calculatedStats.BasicStats.SpellCombatManaRegeneration += character.PriestTalents.Meditation * 0.1f;

            calculatedStats.SpiritRegen = (float)Math.Floor(5 * (0.001f + 0.0093271 * calculatedStats.BasicStats.Spirit * Math.Sqrt(calculatedStats.BasicStats.Intellect)));

            calculatedStats.RegenInFSR = calculatedStats.SpiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration;
            calculatedStats.RegenOutFSR = calculatedStats.SpiritRegen;

            Stats simstats = new Stats() + calculatedStats.BasicStats;

            if (simstats.SpiritFor20SecOnUse2Min > 0)
            {   // Trinkets with Use: Increases Spirit with. (Like Earring of Soulful Meditation)
                simstats.Spirit += simstats.SpiritFor20SecOnUse2Min * 20f / 120f * (1 + character.PriestTalents.SpiritOfRedemption * 0.05f) * (1 + character.PriestTalents.Enlightenment * 0.01f);
            }

            if (simstats.BangleProc > 0)
            {   // Bangle of Endless Blessings. Use: 130 spirit over 20 seconds. 120 sec cd. Also a 15% mana reg proc for 15s on 60s cd.
                simstats.Spirit += 130f * 20f / 120f * (1 + character.PriestTalents.SpiritOfRedemption * 0.05f) * (1 + character.PriestTalents.Enlightenment * 0.01f); ;
                simstats.SpellCombatManaRegeneration += 0.15f * 15f / 60f;
            }
            // We put in our averaged values (use/procs) here.
            float simregen = (float)Math.Floor(0.001f + 0.0093271 * simstats.Spirit * Math.Sqrt(simstats.Intellect));
            
            // MP5 from Replenishment
            simstats.Mp5 += calculatedStats.BasicStats.Mana * 0.0025f * calculationOptions.Replenishment / 100f * 5;

            // MP5 from Shadowfiend (15 second duration, 1.5 attack speed = 10 attacks @ 4% regen = total 40%)
            // Cooldown is 5 minutes - talents in shadow.
            simstats.Mp5 += (simstats.Mana * 0.4f * calculationOptions.Shadowfiend / 100f)
                / ((5f - character.PriestTalents.ImprovedFade * 1f) * 60f) * 5f;

            // Mana as MP5 based on fight length.
            float periodicRegenInFSR = (simregen * simstats.SpellCombatManaRegeneration
                + simstats.Mp5 / 5); // MP1

            float periodicRegenOutFSR = (simregen
                + simstats.Mp5 / 5); // MP1



            int Rotation = calculationOptions.Rotation;
            if (Rotation == 0) // OOOH MAGIC ROTATION!!!
            {
                if (character.PriestTalents.Penance > 0)
                    Rotation = 6; // Disc-MT
                else
                    Rotation = 4; // Holy-MT
                Rotation = 1; // Not yet implemented any of those :P
            }

            // Insightful Earthstorm Diamond.
            float metaSpellCostReduction = simstats.ManaRestorePerCast_5_15 * 0.05f;
            float mana = simstats.Mana; // Add on mana potion here.
            float fightingtime = calculationOptions.FightLength * 60f;
            float healedamount, fightlength, timeslice, lastcast, hccounter;
            float hcchance = (character.PriestTalents.HolyConcentration * 0.1f + character.PriestTalents.ImprovedHolyConcentration * .05f)
                * (simstats.SpellCrit + character.PriestTalents.HolySpecialization * 1f) / 100f;
            float ihchaste = character.PriestTalents.ImprovedHolyConcentration * 10f;
            int ihchasted, manause;


            switch (Rotation)
            {
                case 1: // Greater Heal Spam
                    //int x = SimulateSimple(GreaterHeal);
                    Spell gh = GreaterHeal.GetAllCommonRanks(simstats, character)[0];
                    simstats.SpellHaste += ihchaste;
                    Spell gh_hc = GreaterHeal.GetAllCommonRanks(simstats, character)[0];
                    calculatedStats.HPSBurstPoints = gh.HpS * (1f - hcchance) + gh_hc.HpS * hcchance;
                    healedamount = fightlength = timeslice = hccounter = 0;
                    ihchasted = manause = 0;
                    lastcast = 5f;
                    while (true) // Keep on fighting to the end!
                    {
                        timeslice = 0.1f; // Default, 0.1 second passes.
                        if (mana > gh.ManaCost)
                        { // Cast Greater Heal
                            if (ihchasted > 0)
                            {
                                ihchasted--;
                                timeslice = gh_hc.CastTime;
                                manause = gh_hc.ManaCost;
                                healedamount += gh_hc.HpS * timeslice;
                            }
                            else
                            {
                                timeslice = gh.CastTime;
                                manause = gh_hc.ManaCost;
                                healedamount += gh.HpS * timeslice;
                            }
                            if (hccounter < 1f)
                            {
                                mana -= manause;
                                mana += manause * calculationOptions.Serendipity / 100f * character.PriestTalents.Serendipity * 0.25f / 3f;
                                lastcast = 0;
                            }
                            else
                            {
                                hccounter -= 1f;
                                if (ihchaste > 0)
                                    ihchasted = 2;
                            }
                            mana += metaSpellCostReduction;
                            hccounter += hcchance;
                            if (fightlength >= fightingtime)
                                break;
                        }
                        else
                            lastcast += timeslice;
                        if (lastcast > 5f)
                        {
                            float timeFSR = lastcast - timeslice;
                            if (timeFSR < 5f)
                            { // Just entered FSR, partial time spent OFSR
                                mana += periodicRegenInFSR * (5f - timeFSR)
                                    + periodicRegenOutFSR * (lastcast - 5f);                                
                            }
                            else // Full time FSR regen
                                mana += periodicRegenOutFSR * timeslice;
                        }
                        else
                            mana += periodicRegenInFSR * timeslice;
                        fightlength += timeslice;
                    }
                    calculatedStats.HPSSustainPoints = healedamount / fightlength;
                    break;
                case 2: // Flash Heal Spam
                    Spell fh = FlashHeal.GetAllCommonRanks(simstats, character)[0];
                    calculatedStats.HPSBurstPoints = fh.HpS * ihchaste;
                    healedamount = fightlength = timeslice = hccounter = 0;
                    lastcast = 5f;
                    while (true) // Keep on fighting to the end!
                    {
                        timeslice = 0.1f; // Default, 0.1 second passes.
                        if (mana > fh.ManaCost)
                        { // Cast Greater Heal
                            timeslice = fh.CastTime;
                            healedamount += fh.HpS * timeslice;
                            if (hccounter < 1f)
                            {
                                mana -= fh.ManaCost;
                                mana += fh.ManaCost * calculationOptions.Serendipity / 100f * character.PriestTalents.Serendipity * 0.25f / 3f;
                            }
                            else
                                hccounter -= 1f;
                            mana += metaSpellCostReduction;
                            hccounter += hcchance;
                            lastcast = 0;
                            if (fightlength >= fightingtime)
                                break;
                        }
                        else
                            lastcast += timeslice;
                        if (lastcast > 5f)
                        {
                            float timeFSR = lastcast - timeslice;
                            if (timeFSR < 5f)
                            { // Just entered FSR, partial time spent OFSR
                                mana += periodicRegenInFSR * (5f - timeFSR)
                                    + periodicRegenOutFSR * (lastcast - 5f);
                            }
                            else // Full time FSR regen
                                mana += periodicRegenOutFSR * timeslice;
                        }
                        else
                            mana += periodicRegenInFSR * timeslice;
                        fightlength += timeslice;
                    }
                    calculatedStats.HPSSustainPoints = healedamount / fightlength;
                    break;
                case 3: // Circle of Healing Spam
                    Spell coh = CircleOfHealing.GetAllCommonRanks(simstats, character, 5)[0];
                    calculatedStats.HPSBurstPoints = coh.HpS;
                    healedamount = fightlength = timeslice = 0;
                    lastcast = 5f;
                    while (true) // Keep on fighting to the end!
                    {
                        timeslice = 0.1f; // Default, 0.1 second passes.
                        if (mana > coh.ManaCost)
                        { // Cast Greater Heal
                            timeslice = coh.CastTime;
                            healedamount += coh.HpS * timeslice;
                            mana -= coh.ManaCost;
                            mana += metaSpellCostReduction;
                            lastcast = coh.GlobalCooldown;
                            if (fightlength >= fightingtime)
                                break;
                        }
                        else
                            lastcast += timeslice;
                        if (lastcast > 5f)
                        {
                            float timeFSR = lastcast - timeslice;
                            if (timeFSR < 5f)
                            { // Just entered FSR, partial time spent OFSR
                                mana += periodicRegenInFSR * (5f - timeFSR)
                                    + periodicRegenOutFSR * (lastcast - 5f);
                            }
                            else // Full time FSR regen
                                mana += periodicRegenOutFSR * timeslice;
                        }
                        else
                            mana += periodicRegenInFSR * timeslice;
                        fightlength += timeslice;
                    }
                    calculatedStats.HPSSustainPoints = healedamount / fightlength;
                    break; 
                default:          
                    calculatedStats.HPSBurstPoints = calculatedStats.BasicStats.SpellPower * 1.88f
                        + (calculatedStats.BasicStats.HealingDoneFor15SecOnUse2Min * 15f / 120f)
                        + (calculatedStats.BasicStats.HealingDoneFor15SecOnUse90Sec * 15f / 90f)
                        + (calculatedStats.BasicStats.HealingDoneFor20SecOnUse2Min * 20f / 120f)
                        + (calculatedStats.BasicStats.SpiritFor20SecOnUse2Min * character.PriestTalents.SpiritualGuidance * 0.05f * 20f / 120f);

                    /*calculatedStats.RegenPoints = (calculatedStats.RegenInFSR * calculationOptions.TimeInFSR * 0.01f +
                       calculatedStats.RegenOutFSR * (100 - calculationOptions.TimeInFSR) * 0.01f)
                        + calculatedStats.BasicStats.MementoProc * 3f * 5f / (45f + 9.5f * 2f)
                        + calculatedStats.BasicStats.ManaregenFor8SecOnUse5Min * 5f * (8f * (1 - calculatedStats.BasicStats.HasteRating / 15.7f / 100f)) / (60f * 5f)
                        + (calculatedStats.BasicStats.BonusManaPotion * 2400f * 5f / 120f)
                        + procSpiritRegen + procSpiritRegen2
                        + (calculatedStats.BasicStats.Mp5OnCastFor20SecOnUse2Min > 0 ? 588f * 5f / 120f : 0)
                        + (calculatedStats.BasicStats.ManaregenOver20SecOnUse3Min * 5f / 180f)
                        + (calculatedStats.BasicStats.ManaregenOver20SecOnUse5Min * 5f / 300f)
                        + (calculatedStats.BasicStats.ManacostReduceWithin15OnHealingCast / (2.0f * 50f)) * 5f
                        + (calculatedStats.BasicStats.FullManaRegenFor15SecOnSpellcast > 0?(((calculatedStats.RegenOutFSR - calculatedStats.RegenInFSR) / 5f) * 15f / 125f) * 5f: 0)
                        + (calculatedStats.BasicStats.BangleProc > 0 ? (((calculatedStats.RegenOutFSR - calculatedStats.RegenInFSR) / 5f) * 0.25f * 15f / 125f) * 5f : 0);
                    */
                    calculatedStats.HPSSustainPoints = 0;
                    break;
            }

            // If opponent has 25% crit, each 39.42308044 resilience gives -1% damage from dots and -1% chance to be crit. Also reduces crits by 2%.
            // This effectively means you gain 12.5% extra health from removing 12.5% dot and 12.5% crits at resilience cap (492.5 (39.42308044*12.5))
            // In addition, the remaining 12.5% crits are reduced by 25% (12.5%*200%damage*75% = 18.75%)
            // At resilience cap I'd say that your hp's are scaled by 1.125*1.1875 = ~30%. Probably wrong but good enough.
            calculatedStats.SurvivabilityPoints = calculatedStats.BasicStats.Health * calculationOptions.Survivability / 100f * (1 + 0.3f * calculatedStats.BasicStats.Resilience / 492.7885f);

            calculatedStats.OverallPoints = calculatedStats.HPSBurstPoints + calculatedStats.HPSSustainPoints + calculatedStats.SurvivabilityPoints;

            return calculatedStats;
        }

        public Stats GetRaceStats(Character character)
        {
            switch (character.Race)
            {
                case Character.CharacterRace.NightElf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 57f,
                        Intellect = 145f,
                        Spirit = 151f
                    };
                case Character.CharacterRace.Dwarf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 61f,
                        Intellect = 144f,
                        Spirit = 150f
                    };
                case Character.CharacterRace.Draenei:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 57f,
                        Intellect = 146f,
                        Spirit = 153f
                    };
                case Character.CharacterRace.Human:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 58f,
                        Intellect = 145f,
                        Spirit = 152f,
                        BonusSpiritMultiplier = 0.1f
                    };
                case Character.CharacterRace.BloodElf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 56f,
                        Intellect = 149f,
                        Spirit = 150f
                    };
                case Character.CharacterRace.Troll:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 59f,
                        Intellect = 141f,
                        Spirit = 152f
                    };
                case Character.CharacterRace.Undead:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 59f,
                        Intellect = 143f,
                        Spirit = 156f,
                    };
            }
            return new Stats();
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
			statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit));
            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f);
            statsTotal.Health = statsTotal.Health + (statsTotal.Stamina * 10f);

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            ComparisonCalculationBase comparison;
            CharacterCalculationsHolyPriest p;
            List<Spell>[] spellList;

            switch (chartName)
            {
                case "Spell AoE HpS":
                    _currentChartName = "Spell HpS";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 5),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 5),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)
                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if(spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpS;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();
                case "Spell AoE HpM":
                    _currentChartName = "Spell HpM";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 5),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 5),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)
                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if (spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpM;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();
                case "Spell HpS":
                    _currentChartName = "Spell HpS";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 1),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 1),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)

                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if (spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpS;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();
                case "Spell HpM":
                    _currentChartName = "Spell HpM";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 1),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 1),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)
                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if (spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpM;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();

                case "Relative Stat Values":
                    CharacterCalculationsHolyPriest calcsBase = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsIntellect = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSpirit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsMP5 = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Mp5 = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSpellPower = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsHaste = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsCrit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 50 } }) as CharacterCalculationsHolyPriest;

                    return new ComparisonCalculationBase[] {
                        new ComparisonCalculationHolyPriest() { Name = "1 Intellect",
                            OverallPoints = (calcsIntellect.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsIntellect.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsIntellect.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsIntellect.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Spirit",
                            OverallPoints = (calcsSpirit.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSpirit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSpirit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSpirit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 MP5",
                            OverallPoints = (calcsMP5.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsMP5.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsMP5.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsMP5.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Spell Power",
                            OverallPoints = (calcsSpellPower.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSpellPower.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSpellPower.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSpellPower.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Haste",
                            OverallPoints = (calcsHaste.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsHaste.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsHaste.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsHaste.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Crit",
                            OverallPoints = (calcsCrit.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsCrit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsCrit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsCrit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        }};
                default:
                    _currentChartName = null;
                    return new ComparisonCalculationBase[0];
            }
        }

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
                MementoProc = stats.MementoProc,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                BonusPoHManaCostReductionMultiplier = stats.BonusPoHManaCostReductionMultiplier,
                BonusGHHealingMultiplier = stats.BonusGHHealingMultiplier,
                ManaregenFor8SecOnUse5Min = stats.ManaregenFor8SecOnUse5Min,
                HealingDoneFor15SecOnUse2Min = stats.HealingDoneFor15SecOnUse2Min,
                HealingDoneFor20SecOnUse2Min = stats.HealingDoneFor20SecOnUse2Min,
                HealingDoneFor15SecOnUse90Sec = stats.HealingDoneFor15SecOnUse90Sec,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
                SpellHasteFor20SecOnUse2Min = stats.SpellHasteFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaregenOver20SecOnUse3Min = stats.ManaregenOver20SecOnUse3Min,
                ManaregenOver20SecOnUse5Min = stats.ManaregenOver20SecOnUse5Min,
                ManacostReduceWithin15OnHealingCast = stats.ManacostReduceWithin15OnHealingCast,
                FullManaRegenFor15SecOnSpellcast = stats.FullManaRegenFor15SecOnSpellcast,
                BangleProc = stats.BangleProc
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellPower + stats.CritRating
                + stats.HasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion + stats.MementoProc + stats.ManaregenFor8SecOnUse5Min
                + stats.BonusPoHManaCostReductionMultiplier + stats.SpellCombatManaRegeneration
                + stats.HealingDoneFor15SecOnUse2Min + stats.HealingDoneFor20SecOnUse2Min
                + stats.HealingDoneFor15SecOnUse90Sec + stats.SpiritFor20SecOnUse2Min
                + stats.SpellHasteFor20SecOnUse2Min + stats.Mp5OnCastFor20SecOnUse2Min
                + stats.ManaregenOver20SecOnUse3Min + stats.ManaregenOver20SecOnUse5Min
                + stats.ManacostReduceWithin15OnHealingCast + stats.FullManaRegenFor15SecOnSpellcast
                + stats.BangleProc) > 0;
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsPriest;
            return calcOpts;
        }
    }
}
