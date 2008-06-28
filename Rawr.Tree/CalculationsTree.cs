using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.Tree
{
    [System.ComponentModel.DisplayName("Tree|Ability_Druid_TreeofLife")]
    public class CalculationsTree : CalculationsBase
    {
        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Druid; } }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("HpS", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Mp5", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Survival", System.Drawing.Color.Green);
                    _subPointNameColors.Add("ToL", System.Drawing.Color.Yellow);
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
					"Basic Stats:Healing",
					"Basic Stats:Mp5",
					"Basic Stats:Spell Crit",
					"Basic Stats:Spell Haste",

                    "Extended Stats:Mana per Cast (5%)",
                    "Extended Stats:HPS Points",
                    "Extended Stats:Mp5 Points",
                    "Extended Stats:Survival Points",
                    "Extended Stats:ToL Points",
                    "Extended Stats:Overall Points",

                    
					"Rotation:Rotation duration",
					"Rotation:Rotation heal",
					"Rotation:Rotation cost",
					"Rotation:Rotation HPS",
					"Rotation:Rotation HPM",
					"Rotation:Max fight duration",
            	    
                    "Lifebloom:LB Tick","Lifebloom:LB Heal","Lifebloom:LB HPS","Lifebloom:LB HPM",
                    "Lifebloom Stack:LBS Tick","Lifebloom Stack:LBS HPS","Lifebloom Stack:LBS HPM",
                    "Rejuvenation:RJ Tick","Rejuvenation:RJ HPS","Rejuvenation:RJ HPM",
                    "Regrowth:RG Tick","Regrowth:RG Heal","Regrowth:RG HPS","Regrowth:RG HPM",
					"Healing Touch:HT Heal","Healing Touch:HT HPS","Healing Touch:HT HPM",
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelTree();
                }
                return _calculationOptionsPanel;
            }
        }

        public override string[] CustomChartNames
        {
            get { return new string[0]; }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationTree(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsTree(); }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    // I don't know of a fist weapon or two hand mace with healing stats, so...
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]{
                        Item.ItemType.None,
                        Item.ItemType.Cloth,
                        Item.ItemType.Leather,
                        Item.ItemType.Dagger,
                        Item.ItemType.Idol,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;
            CharacterCalculationsTree calculatedStats = new CharacterCalculationsTree();

            calculatedStats.BasicStats = GetCharacterStats(character, additionalItem);

            calculatedStats.BasicStats.SpellCrit = (float)Math.Round((calculatedStats.BasicStats.Intellect / 80) +
                (calculatedStats.BasicStats.SpellCritRating / 22.08) + 1.85 + calcOpts.NaturalPerfection, 2);

            calculatedStats.BasicStats.SpellCombatManaRegeneration += 0.1f * calcOpts.Intensity;

            calculatedStats.BasicStats.TreeOfLifeAura += (calculatedStats.BasicStats.Spirit / 4f);
            calculatedStats.BasicStats.TreeOfLifeAura *= calcOpts.TreeOfLife;

            float baseRegenConstant = 0.00932715221261f;
            float spiritRegen = 0.001f + baseRegenConstant * (float)Math.Sqrt(calculatedStats.BasicStats.Intellect) * calculatedStats.BasicStats.Spirit;

            calculatedStats.OS5SRRegen = spiritRegen + calculatedStats.BasicStats.Mp5 / 5f;
            calculatedStats.IS5SRRegen = spiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5 / 5f;

            Spell lbs = new LifebloomStack(character, calculatedStats.BasicStats, true);
            Spell lb = new Lifebloom(character, calculatedStats.BasicStats, true);
            Spell rj = new Rejuvenation(character, calculatedStats.BasicStats, true);
            Spell rg = new Regrowth(character, calculatedStats.BasicStats, true);

            Spell ht = new HealingTouch(character, calculatedStats.BasicStats);
            Spell lbsrh = new LifebloomStack(character, calculatedStats.BasicStats, false);
            Spell lbrh = new Lifebloom(character, calculatedStats.BasicStats, false);
            Spell rjrh = new Rejuvenation(character, calculatedStats.BasicStats, false);
            Spell rgrh = new Regrowth(character, calculatedStats.BasicStats, false);
            Spell nothing = new Nothing(character, calculatedStats.BasicStats);


            calculatedStats.Spells = new List<Spell>();
            calculatedStats.Spells.Add(lbs);
            calculatedStats.Spells.Add(lb);
            calculatedStats.Spells.Add(rj);
            calculatedStats.Spells.Add(rg);
            calculatedStats.Spells.Add(ht);
            calculatedStats.Spells.Add(lbrh);
            calculatedStats.Spells.Add(rjrh);
            calculatedStats.Spells.Add(rgrh);
            calculatedStats.Spells.Add(lbsrh);
            calculatedStats.Spells.Add(nothing);

            calculatedStats.FightLength = calcOpts.FightLength;

            // Calculate scores in another function later to reduce clutter
            int health = (int)calculatedStats.BasicStats.Health;
            int healthBelow = (int)(health < calcOpts.TargetHealth ? health : calcOpts.TargetHealth);
            int healthAbove = health - healthBelow;

            calculatedStats.AddMp5Points(calculatedStats.IS5SRRegen * 5f, "Regen");
            calculatedStats.AddMp5Points(calcOpts.Spriest, "Shadow Priest");
            calculatedStats.AddMp5Points((calcOpts.ManaPotAmt * (1 + calculatedStats.BasicStats.BonusManaPotion)) / (calcOpts.ManaPotDelay * 12), "Potion");

            getBestSpellRotation(calcOpts, calculatedStats);

            if (calculatedStats.BestSpellRotation != null)
            {
                calculatedStats.HpSPoints = calculatedStats.BestSpellRotation.healPerCycle / calculatedStats.BestSpellRotation.bestCycleDuration * calculatedStats.FightFraction;
                if (calcOpts.InnervateSelf)
                {
                    float manaGain = calculatedStats.OS5SRRegen * 5 * 20 - calculatedStats.IS5SRRegen * 20;
                    float manaUsed = calculatedStats.BestSpellRotation.manaPerCycle / calculatedStats.BestSpellRotation.bestCycleDuration * 20;
                    if (manaGain > calculatedStats.BasicStats.Mana + manaUsed)
                        manaGain = calculatedStats.BasicStats.Mana + manaUsed;
                    // The model does not try to calculate if you can use a different cycle during innervate in order to spam more regrowth (since your mana would fill up to 100% anyway)
                    calculatedStats.AddMp5Points(manaGain / (calcOpts.InnervateDelay * 12), "Innervate");
                }
                calculatedStats.AddMp5Points(5 * calculatedStats.BestSpellRotation.numberOfSpells * calculatedStats.BasicStats.ManaRestorePerCast_5_15 * 0.05f / calculatedStats.BestSpellRotation.currentCycleDuration, "Mana per Cast (5%)");
                calculatedStats.AddMp5Points(5 * calculatedStats.BasicStats.MementoProc * 3 / (45 + calculatedStats.BestSpellRotation.currentCycleDuration / calculatedStats.BestSpellRotation.numberOfSpells * 5), "Memento of Tyrande");
            }
            else
            {
                calculatedStats.HpSPoints = 0;
            }

            calculatedStats.SurvivalPoints = healthBelow / calcOpts.SurvScalingBelow + healthAbove / calcOpts.SurvScalingAbove;
            calculatedStats.ToLPoints = calculatedStats.BasicStats.TreeOfLifeAura;

            calculatedStats.OverallPoints = calculatedStats.HpSPoints + calculatedStats.Mp5Points + calculatedStats.SurvivalPoints + calculatedStats.ToLPoints;

            return calculatedStats;
        }

        /*
        // rotationIndex is a number 0 <= rotationIndex <= spellCycles^numCyclesPerRotation
        private SpellRotation GetRotationNumber(List<SpellRotation> spellCycles, long rotationIndex, int numCyclesPerRotation, float maxCycleDuration)
        {
            List<SpellRotation> cyclesInRotation = new List<SpellRotation>();

            for (int i = 0; i < numCyclesPerRotation; i++)
            {
                cyclesInRotation.Add(spellCycles[(int)(rotationIndex % spellCycles.Count)]);
                rotationIndex /= spellCycles.Count;
            }

            return new SpellRotation(cyclesInRotation, numCyclesPerRotation * maxCycleDuration, numCyclesPerRotation);
        }*/

        private List<List<Spell>> allCombinations(Spell[][] spellList, float maxCycleDuration)
        {
            List<List<Spell>> cur = new List<List<Spell>>();

            for (int j = 0; j < spellList[0].GetLength(0); j++)
            {
                List<Spell> l = new List<Spell>();
                l.Add(spellList[0][j]);
                cur.Add(l);
            }

            for (int i = 1; i < spellList.GetLength(0); i++)
            {
                List<List<Spell>> newList = new List<List<Spell>>();

                for (int j = 0; j < spellList[i].GetLength(0); j++)
                {
                    foreach (List<Spell> l in cur)
                    {
                       List<Spell> nl = new List<Spell>(l);
                       nl.Add(spellList[i][j]);
                       newList.Add(nl);
                    }
                }
                cur = newList;
            }

            return cur;
        }

        private float rotationMultiplier(SpellRotation rot, CalculationOptionsTree calcOpts, CharacterCalculationsTree calculatedStats)
        {
            if (rot == null)
                return 0;

            float innervateMana = 0f;
            if (calcOpts.InnervateSelf)
            {
                float manaGain = calculatedStats.OS5SRRegen * 5 * 20 - calculatedStats.IS5SRRegen * 20;
                float manaUsed = rot.manaPerCycle / rot.currentCycleDuration * 20;
                if (manaGain > calculatedStats.BasicStats.Mana + manaUsed)
                    manaGain = calculatedStats.BasicStats.Mana + manaUsed;
                innervateMana = manaGain / (calcOpts.InnervateDelay * 60) * rot.currentCycleDuration;
            }

            float manaCostPerCycle = rot.manaPerCycle -
                       (calculatedStats.Mp5Points * rot.currentCycleDuration / 5 +
                       innervateMana +
                       rot.numberOfSpells * calculatedStats.BasicStats.ManaRestorePerCast_5_15 * 0.05f +
                       rot.currentCycleDuration * calculatedStats.BasicStats.MementoProc * 3 / (45 + rot.currentCycleDuration / rot.numberOfSpells * 5));

            float HPSMultiplier = 1.0f;

            if (manaCostPerCycle > 0)
            {
                HPSMultiplier = (rot.currentCycleDuration * calculatedStats.BasicStats.Mana / manaCostPerCycle) / (calcOpts.FightLength * 60);
                if (HPSMultiplier > 1.0f)
                    HPSMultiplier = 1.0f;
            }

            return HPSMultiplier;
        }

        public static long LongPower(long a, long b)
        {
            long f = 1;
            for (int i = 1; i <= b; i++)
                f *= a;
            return f;
        }


        private List<SpellRotation> filterSpellRotations(List<SpellRotation> spellRotations, CalculationOptionsTree calcOpts, CharacterCalculationsTree calculatedStats)
        {
            spellRotations.RemoveAll(delegate(SpellRotation sr)
            {
                sr.currentCycleDuration = sr.maxCycleDuration;
                return rotationMultiplier(sr, calcOpts, calculatedStats) < 0.6f;
            });

            spellRotations.RemoveAll(delegate(SpellRotation sr2)
            {
                return spellRotations.Exists(delegate(SpellRotation sr)
                {
                    return sr2.manaPerCycle > sr.manaPerCycle && sr.healPerCycle > sr2.healPerCycle;
                });
            });

            List<SpellRotation> res = new List<SpellRotation>();

            foreach (SpellRotation cycle in spellRotations)
            {
                if (!res.Exists(delegate(SpellRotation sr)
                {
                    return cycle.manaPerCycle == sr.manaPerCycle && cycle.healPerCycle == sr.healPerCycle;
                }))
                {
                    res.Add(cycle);
                }
            }

            return res;
        }

        private void getBestSpellRotation(CalculationOptionsTree calcOpts, CharacterCalculationsTree calculatedStats)
        {
            Spell[][] spellList = new Spell[calcOpts.availableSpells.GetLength(0)][];

            for (int i = 0; i < calcOpts.availableSpells.GetLength(0); i++)
            {
                spellList[i] = new Spell[calcOpts.availableSpells[i].GetLength(0)];
                for (int j = 0; j < calcOpts.availableSpells[i].GetLength(0); j++)
                {
                    spellList[i][j] = calculatedStats.Spells.Find(delegate(Spell s) { return s.Name.Equals(calcOpts.availableSpells[i][j]); });
                }
            }

            float bestScore = 0f;
            SpellRotation bestRotation = null;

            List<SpellRotation> spellCyclesBeforeFilter = new List<SpellRotation>();

            foreach (List<Spell> spells in allCombinations(spellList, calcOpts.MaxCycleDuration))
            {
                spellCyclesBeforeFilter.Add(new SpellRotation(spells, calcOpts.MaxCycleDuration));
            }

            calculatedStats.NumCycles = spellCyclesBeforeFilter.Count;

            List<SpellRotation> spellCycles = filterSpellRotations(spellCyclesBeforeFilter, calcOpts, calculatedStats);

            calculatedStats.NumCyclesAfterFilter = spellCycles.Count;
            calculatedStats.DebugText = "";

            if (calculatedStats.NumCyclesAfterFilter == 0)
            {
                calculatedStats.BestSpellRotation = null;
                calculatedStats.FightFraction = 0f;
                calculatedStats.NumCyclesPerRotation = 0;
                calculatedStats.NumRotations = 0;
                return;
            }

            foreach (SpellRotation cycle in spellCycles)
            {
                calculatedStats.DebugText += cycle.HPM + "," + cycle.healPerCycle / cycle.tightCycleDuration + "\n";
            }

            List<SpellRotation> possibleRotations = new List<SpellRotation>();
            possibleRotations.Add(new SpellRotation(new List<Spell>(), 0f));

            calculatedStats.DebugText += "Filtering brought down the combinations to: ";
            for (int i = 0; i < calcOpts.NumCyclesPerRotation; i++)
            {
                List<SpellRotation> tmpPossibleRotations = new List<SpellRotation>();
                foreach (SpellRotation sr in possibleRotations)
                {
                    foreach (SpellRotation cycle in spellCycles)
                    {
                        List<SpellRotation> tmpList = new List<SpellRotation>();
                        tmpList.Add(sr);
                        tmpList.Add(cycle);
                        tmpPossibleRotations.Add(new SpellRotation(tmpList, calcOpts.MaxCycleDuration*(i+1), i+1));
                    }
                }

                possibleRotations = filterSpellRotations(tmpPossibleRotations, calcOpts, calculatedStats);
                calculatedStats.DebugText += possibleRotations.Count + (i == calcOpts.NumCyclesPerRotation - 1 ? "..." : "=");
            }
            
            calculatedStats.DebugText += possibleRotations.Count;

            /*
            long numberOfRotations = LongPower(spellCycles.Count, calcOpts.NumCyclesPerRotation);

            for (long i = 0; i < numberOfRotations; i++)*/
            foreach (SpellRotation rot in possibleRotations)
            {
                float maxLength = calcOpts.MaxCycleDuration * calcOpts.NumCyclesPerRotation;
                float bestLocalScore = 0f;
                float granularity = 0.1f;

                // SpellRotation rot = GetRotationNumber(spellCycles, i, calcOpts.NumCyclesPerRotation, calcOpts.MaxCycleDuration);

                rot.currentCycleDuration = ((float)Math.Ceiling(rot.tightCycleDuration * 10)) / 10f;

                // No use trying all combinations if we already have a better score
                if (bestLocalScore > rot.healPerCycle / rot.tightCycleDuration)
                    continue;

                float multiplier;
                do
                {
                    multiplier = rotationMultiplier(rot, calcOpts, calculatedStats);
                    float score = rot.healPerCycle / rot.currentCycleDuration * multiplier;

                    if (score > bestLocalScore)
                    {
                        bestLocalScore = score;
                        rot.bestCycleDuration = rot.currentCycleDuration;
                    }

                    rot.currentCycleDuration += granularity;
                } while (rot.currentCycleDuration < maxLength && multiplier < 1f);

                if (bestLocalScore > bestScore)
                {
                    bestRotation = rot;
                    bestScore = bestLocalScore;
                }
            }
            bestRotation.currentCycleDuration = bestRotation.bestCycleDuration;

            calculatedStats.BestSpellRotation = bestRotation;
            calculatedStats.FightFraction = rotationMultiplier(calculatedStats.BestSpellRotation, calcOpts, calculatedStats);
            calculatedStats.NumCyclesPerRotation = calcOpts.NumCyclesPerRotation;
            calculatedStats.NumRotations = LongPower(spellCycles.Count, calcOpts.NumCyclesPerRotation);
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;

            Stats statsRace = character.Race == Character.CharacterRace.NightElf ?
                new Stats()
                {
                    Health = 3434f,
                    Mana = 2470f,
                    Stamina = 82f,
                    Agility = 75f,
                    Intellect = 120f,
                    Spirit = 133f,
                    BonusSpiritMultiplier = calcOpts.LivingSpirit * 0.05f
                } :
                new Stats()
                {
                    Health = 3434f,
                    Mana = 2470f,
                    Stamina = 85f,
                    Agility = 64.5f,
                    Intellect = 115f,
                    Spirit = 135f,
                    BonusSpiritMultiplier = calcOpts.LivingSpirit * 0.05f,
                    BonusHealthMultiplier = 0.05f
                };

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor((statsTotal.Intellect) * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Healing = (float)Math.Round(statsTotal.Healing + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit));
            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f);
            statsTotal.Health = statsTotal.Health + (float)Math.Round(statsTotal.Stamina * 10f * (1 + statsTotal.BonusHealthMultiplier));

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return null;
        }

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
                MementoProc = stats.MementoProc,
                AverageHeal = stats.AverageHeal,
                ManaRestorePerCast_5_15 = stats.ManaRestorePerCast_5_15,
                LifebloomFinalHealBonus = stats.LifebloomFinalHealBonus,
                RegrowthExtraTicks = stats.RegrowthExtraTicks,
                BonusHealingTouchMultiplier = stats.BonusHealingTouchMultiplier,
                TreeOfLifeAura = stats.TreeOfLifeAura,
                ReduceRejuvenationCost = stats.ReduceRejuvenationCost,
                ReduceRegrowthCost = stats.ReduceRegrowthCost,
                ReduceHealingTouchCost = stats.ReduceHealingTouchCost,
                RejuvenationHealBonus = stats.RejuvenationHealBonus,
                LifebloomTickHealBonus = stats.LifebloomTickHealBonus,
                HealingTouchFinalHealBonus = stats.HealingTouchFinalHealBonus,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Health + stats.Intellect + stats.Spirit + stats.Mp5 + stats.Healing + stats.SpellCritRating
                + stats.SpellHasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion + stats.MementoProc + stats.AverageHeal
                + stats.ManaRestorePerCast_5_15 + stats.LifebloomFinalHealBonus + stats.RegrowthExtraTicks
                + stats.BonusHealingTouchMultiplier + stats.TreeOfLifeAura
                + stats.ReduceRejuvenationCost + stats.ReduceRegrowthCost + stats.ReduceHealingTouchCost
                + stats.RejuvenationHealBonus + stats.LifebloomTickHealBonus + stats.HealingTouchFinalHealBonus
                ) > 0;
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTree));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsTree calcOpts = serializer.Deserialize(reader) as CalculationOptionsTree;
            return calcOpts;
        }
    }
}
