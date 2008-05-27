using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Rawr.Mage
{
	[System.ComponentModel.DisplayName("Mage|Spell_Holy_MagicalSentry")]
    class CalculationsMage : CalculationsBase
    {
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Dps", System.Drawing.Color.FromArgb(0, 128, 255));
                    _subPointNameColors.Add("Survivability", System.Drawing.Color.FromArgb(64, 128, 32));
                }
                return _subPointNameColors;
            }
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMage));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsMage calcOpts = serializer.Deserialize(reader) as CalculationOptionsMage;
            return calcOpts;
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Armor",
					"Basic Stats:Health",
					"Basic Stats:Mana",
                    "Spell Stats:Spell Crit Rate",
                    "Spell Stats:Spell Hit Rate",
                    "Spell Stats:Spell Penetration",
                    "Spell Stats:Casting Speed",
                    "Spell Stats:Arcane Damage",
                    "Spell Stats:Fire Damage",
                    "Spell Stats:Frost Damage",
                    "Solution:Total Damage",
                    "Solution:Dps",
                    "Solution:Tps*Threat per second",
                    "Solution:Spell Cycles",
                    "Solution:Sequence*Cycle sequence reconstruction based on optimum cycles",
                    "Spell Info:Wand",
                    "Spell Info:Arcane Missiles",
                    //"Spell Info:AM?*Arcane Missiles with Netherwind proc",
                    "Spell Info:Arcane Blast*Spammed",
                    "Spell Info:Arcane Blast(0)*Non-debuffed",
                    //"Spell Info:Arcane Barrage*Requires talent points",
                    "Spell Info:Scorch",
                    "Spell Info:Fire Blast",
                    "Spell Info:Pyroblast*Requires talent points",
                    "Spell Info:Fireball",
                    "Spell Info:FireballScorch*Must enable Maintain Scorch and have points in Improved Scorch talent to enable",
                    "Spell Info:FireballFireBlast",
                    "Spell Info:Frostbolt",
                    "Spell Info:ABAMP*Pause so that AB debuff runs out mid-cast",
                    "Spell Info:ABAM*Spam with no pause",
                    "Spell Info:ABAMCC*AM when AM procs clearcast, ramp up AB before returning to AB-AM",
                    "Spell Info:ABAM3CC*AM when AM procs clearcast, ramp up with AB-AM",
                    "Spell Info:AB3AMSc*Prefer pause over longer filler",
                    "Spell Info:ABAM3Sc*Prefer pause over longer filler",
                    "Spell Info:ABAM3Sc2*Fill until debuff almost out",
                    "Spell Info:ABAM3FrB*Prefer pause over longer filler",
                    "Spell Info:ABAM3FrB2*Fill until debuff almost out",
                    "Spell Info:ABAM3ScCC*AM when AM procs clearcast",
                    "Spell Info:ABAM3Sc2CC*AM when AM procs clearcast",
                    "Spell Info:ABAM3FrBCC*AM when AM procs clearcast",
                    //"Spell Info:ABAM3FrBCCFail*AM when AM procs clearcast",
                    "Spell Info:ABAM3FrBScCC*AM when AM procs clearcast",
                    "Spell Info:ABFrB*Prefer pause over longer filler",
                    "Spell Info:ABFrB3FrB*Prefer pause over longer filler",
                    "Spell Info:ABFrB3FrBSc*Fill until debuff almost out, Scorch used at specific haste levels where adding another Frostbolt results in drop of AB debuff and alternative results in relatively large pause",
                    //"Spell Info:AB3Sc*Fill until debuff almost out",
                    "Spell Info:ABFB3FBSc*Typically FB-FB-Sc filler",
                    "Spell Info:Arcane Explosion",
                    "Spell Info:Blizzard",
                    "Spell Info:Cone of Cold",
                    "Spell Info:Flamestrike*Spammed",
                    "Spell Info:Blast Wave*Requires talent points",
                    "Spell Info:Dragon's Breath*Requires talent points",
                    "Regeneration:MP5",
                    "Regeneration:Mana Regen",
                    "Regeneration:Health Regen",
                    "Survivability:Arcane Resist",
                    "Survivability:Fire Resist",
                    "Survivability:Nature Resist",
                    "Survivability:Frost Resist",
                    "Survivability:Shadow Resist",
                    "Survivability:Physical Mitigation",
                    "Survivability:Resilience",
                    "Survivability:Defense",
                    "Survivability:Crit Reduction",
                    "Survivability:Dodge",
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { "Talents (per talent point)", "Talent Specs", "Item Budget" };
                return _customChartNames;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelMage();
                }
                return _calculationOptionsPanel;
            }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.Cloth,
						Item.ItemType.Dagger,
						Item.ItemType.OneHandSword,
						Item.ItemType.Staff,
						Item.ItemType.Wand,
					});
                }
                return _relevantItemTypes;
            }
        }

        public override string GetCharacterStatsString(Character character)
        {
			StringBuilder stats = new StringBuilder();
			stats.AppendFormat("Character:\t\t{0}@{1}-{2}\r\nRace:\t\t{3}",
				character.Name, character.Region, character.Realm, character.Race);

            CalculationOptionsMage CalculationOptions = (CalculationOptionsMage)character.CalculationOptions;
            CharacterCalculationsMage calculations;
            if (CalculationOptions.SMPDisplay)
            {
                bool savedIncrementalOptimizations = CalculationOptions.IncrementalOptimizations;
                CalculationOptions.IncrementalOptimizations = false;
                calculations = (CharacterCalculationsMage)GetCharacterCalculations(character, null, CalculationOptions, CalculationOptions.IncrementalSetArmor, false, true);
                CalculationOptions.IncrementalOptimizations = savedIncrementalOptimizations;
            }
            else
            {
                calculations = (CharacterCalculationsMage)GetCharacterCalculations(character);
            }

            Dictionary<string, string> dict = calculations.GetCharacterDisplayCalculationValuesInternal();
			foreach (KeyValuePair<string, string> kvp in dict)
			{
                if (kvp.Key != "Sequence" && kvp.Key != "Spell Cycles")
                {
                    string[] value = kvp.Value.Split('*');
                    if (value.Length == 2)
                    {
                        stats.AppendFormat("\r\n{0}: {1}\r\n{2}\r\n", kvp.Key, value[0], value[1]);
                    }
                    else
                    {
                        stats.AppendFormat("\r\n{0}: {1}", kvp.Key, value[0]);
                    }
                }
			}

            // spell cycles
            stats.AppendFormat("\r\n\r\nSpell Cycles:\r\n\r\n");
            if (calculations.MageArmor != null) stats.AppendLine(calculations.MageArmor);
            Dictionary<string, double> combinedSolution = new Dictionary<string, double>();
            Dictionary<string, int> combinedSolutionData = new Dictionary<string, int>();
            for (int i = 0; i < calculations.SolutionStats.Length; i++)
            {
                if (calculations.Solution[i] > 0.01)
                {
                    switch (i)
                    {
                        case 0:
                            stats.AppendLine(String.Format("{0}: {1:F} sec", "Idle Regen", calculations.Solution[0]));
                            break;
                        case 2:
                            stats.AppendLine(String.Format("{0}: {1:F}x", "Evocation", calculations.Solution[i] / calculations.EvocationDuration));
                            break;
                        case 3:
                            stats.AppendLine(String.Format("{0}: {1:F}x", "Mana Potion", calculations.Solution[i] / calculations.ManaPotionTime));
                            break;
                        case 4:
                            stats.AppendLine(String.Format("{0}: {1:F}x", "Mana Gem", calculations.Solution[i] / calculations.ManaPotionTime));
                            break;
                        case 5:
                            stats.AppendLine(String.Format("{0}: {1:F}x", "Drums of Battle", calculations.Solution[i] / calculations.GlobalCooldown));
                            break;
                        default:
                            double value;
                            Spell s = calculations.SolutionSpells[i];
                            string label = ((calculations.SolutionStats[i].BuffLabel.Length > 0) ? (calculations.SolutionStats[i].BuffLabel + "+") : "") + s.Name;
                            combinedSolution.TryGetValue(label, out value);
                            combinedSolution[label] = value + calculations.Solution[i];
                            combinedSolutionData[label] = i;
                            break;
                    }
                }
            }
            foreach (KeyValuePair<string, double> kvp in combinedSolution)
            {
                Spell s = calculations.SolutionSpells[combinedSolutionData[kvp.Key]];
                if (s != null)
                {
                    stats.AppendLine(String.Format("{0}: {1:F} sec ({2:F} dps, {3:F} mps, {4:F} tps) {5}", kvp.Key, kvp.Value, s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond, s.ThreatPerSecond, s.Sequence));
                }
                else
                {
                    stats.AppendLine(String.Format("{0}: {1:F} sec", kvp.Key, kvp.Value));
                }
            }
            if (calculations.WaterElemental) stats.AppendLine(String.Format("Water Elemental: {0:F}x", calculations.WaterElementalDuration / 45f));

            // sequence
            string sequence = dict["Sequence"];
            if (sequence != "*Disabled" && sequence != "*Unavailable")
            {
                string[] value = sequence.Split('*');
                stats.AppendFormat("\r\n\r\nSequence:\r\n\r\n");
                stats.Append(value[1]);
            }

			return stats.ToString();
        }

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Mage; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationMage(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsMage(); }

        private bool IsItemActivatable(Item item)
        {
            if (item == null) return false;
            return (item.Stats.SpellDamageFor15SecOnUse2Min + item.Stats.SpellDamageFor20SecOnUse2Min + item.Stats.SpellHasteFor20SecOnUse2Min + item.Stats.Mp5OnCastFor20SecOnUse2Min + item.Stats.SpellDamageFor15SecOnManaGem + item.Stats.SpellDamageFor15SecOnUse90Sec + item.Stats.SpellHasteFor20SecOnUse5Min > 0);
        }

        private class CompactLP : IComparable<CompactLP>
        {
            private int lpRows, cRows;
            private int lpCols, cCols;
            public LP lp;
            bool[] rowEnabled, colEnabled;
            double[] rowScale, colScale;
            int[] CRow, CCol;
            double[] compactSolution = null;
            bool needsDual;
            //public string Log = string.Empty;
            //public int[] disabledHex;

            public void ForceRecalculation()
            {
                compactSolution = null;
            }

            public CompactLP Clone()
            {
                //if (compactSolution != null && !allowReuse) throw new InvalidOperationException();
                CompactLP clone = (CompactLP)this.MemberwiseClone();
                clone.compactSolution = null;
                //clone.lp = (double[,])clone.lp.Clone();
                clone.lp = lp.Clone();
                //if (disabledHex != null) clone.disabledHex = (int[])disabledHex.Clone();
                return clone;
            }

            public CompactLP(int rows, int cols)
            {
                lpRows = rows;
                lpCols = cols;

                rowEnabled = new bool[rows];
                colEnabled = new bool[cols];

                for (int i = 0; i < rows; i++)
                {
                    rowEnabled[i] = true;
                }
                for (int j = 0; j < cols; j++)
                {
                    colEnabled[j] = true;
                }

                CRow = new int[lpRows + 1];
                CCol = new int[lpCols + 1];
            }

            public void Compact()
            {
                cRows = 0;
                cCols = 0;

                for (int i = 0; i < lpRows; i++)
                {
                    if (rowEnabled[i])
                    {
                        CRow[i] = cRows;
                        cRows++;
                    }
                    else
                    {
                        CRow[i] = -1;
                    }
                }
                for (int j = 0; j < lpCols; j++)
                {
                    if (colEnabled[j])
                    {
                        CCol[j] = cCols;
                        cCols++;
                    }
                    else
                    {
                        CCol[j] = -1;
                    }
                }
                CRow[lpRows] = cRows;
                CCol[lpCols] = cCols;

                rowScale = new double[cRows + 1];
                colScale = new double[cCols + 1];
                for (int i = 0; i <= cRows; i++)
                {
                    rowScale[i] = 1.0;
                }
                for (int j = 0; j <= cCols; j++)
                {
                    colScale[j] = 1.0;
                }

                lp = new LP(cRows, cCols);
            }

            public void SetRowScale(int row, double value)
            {
                row = CRow[row];
                if (row == -1) return;
                rowScale[row] = value;
            }

            public void SetColumnScale(int col, double value)
            {
                col = CCol[col];
                if (col == -1) return;
                colScale[col] = value;
            }

            public void DisableRow(int row)
            {
                rowEnabled[row] = false;
            }

            public void DisableColumn(int col)
            {
                colEnabled[col] = false;
            }

            public bool IsRowEnabled(int row)
            {
                return rowEnabled[row];
            }

            public bool IsColumnEnabled(int col)
            {
                return colEnabled[col];
            }

            public double this[int row, int col]
            {
                get
                {
                    row = CRow[row];
                    col = CCol[col];
                    if (row == -1 || col == -1) return 0;
                    return lp[row, col] / rowScale[row] / colScale[col];
                }
                set
                {
                    row = CRow[row];
                    col = CCol[col];
                    if (row == -1 || col == -1) return;
                    lp[row, col] = value * rowScale[row] * colScale[col];
                    compactSolution = null;
                }
            }

            public void EraseColumn(int col)
            {
                col = CCol[col];
                if (col == -1) return;
                /*for (int row = 0; row <= cRows; row++)
                {
                    lp[row, col] = 0;
                }*/
                lp.DisableColumn(col);
                compactSolution = null;
                needsDual = true;
            }

            bool constraintAdded;

            public void AddConstraint()
            {
                compactSolution = null;
                needsDual = true;
                if (constraintAdded) return;
                lp.AddConstraint();
                constraintAdded = true;
            }

            public void UpdateConstraintElement(int col, double value)
            {
                col = CCol[col];
                if (col == -1) return;
                lp.SetConstraintElement(col, lp.GetConstraintElement(col) + value * colScale[col]);
            }

            public void UpdateConstraintRHS(double value)
            {
                lp.SetConstraintRHS(lp.GetConstraintRHS() + value);
            }

            private void SolveInternal()
            {
                if (compactSolution == null)
                {
                    lp.EndConstruction();
                    if (needsDual)
                    {
                        //System.Diagnostics.Debug.WriteLine("Solving H=" + HeroismHash.ToString("X") + ", AP=" + APHash.ToString("X") + ", IV=" + IVHash.ToString("X"));
                        compactSolution = lp.SolveDual();
                    }
                    else
                    {
                        compactSolution = lp.SolvePrimal();
                    }
                }
            }

            public double[] Solve()
            {
                SolveInternal();
                double[] expanded = new double[lpCols + 1];

                for (int j = 0; j <= lpCols; j++)
                {
                    if (CCol[j] >= 0) expanded[j] = compactSolution[CCol[j]] * colScale[CCol[j]];
                }
                expanded[lpCols] /= rowScale[cRows];
                return expanded;
            }

            public void SolvePrimalDual()
            {
                lp.EndConstruction();
                lp.SolvePrimal();
                compactSolution = lp.SolveDual();
            }

            public double Value
            {
                get
                {
                    SolveInternal();
                    return compactSolution[compactSolution.Length - 1] / rowScale[cRows];
                }
            }

            int IComparable<CompactLP>.CompareTo(CompactLP other)
            {
                return this.Value.CompareTo(other.Value);
            }
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            return GetCharacterCalculations(character, additionalItem, false);
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool computeIncrementalSet)
        {
            CharacterCalculationsBase ret;
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            bool savedIncrementalOptimizations = calculationOptions.IncrementalOptimizations;
            if (computeIncrementalSet) calculationOptions.IncrementalOptimizations = false;
            if (calculationOptions.IncrementalOptimizations && !character.DisableBuffAutoActivation)
            {
                ret = GetCharacterCalculations(character, additionalItem, calculationOptions, calculationOptions.IncrementalSetArmor, computeIncrementalSet);
            }
            else if (calculationOptions.AutomaticArmor && !character.DisableBuffAutoActivation)
            {
                CharacterCalculationsBase mage = GetCharacterCalculations(character, additionalItem, calculationOptions, "Mage Armor", computeIncrementalSet);
                CharacterCalculationsBase molten = GetCharacterCalculations(character, additionalItem, calculationOptions, "Molten Armor", computeIncrementalSet);
                CharacterCalculationsBase calc = (mage.OverallPoints > molten.OverallPoints) ? mage : molten;
                calculationOptions.IncrementalSetArmor = ((CharacterCalculationsMage)calc).MageArmor;
                if (computeIncrementalSet) StoreIncrementalSet(character, (CharacterCalculationsMage)calc);
                ret = calc;
            }
            else
            {
                CharacterCalculationsBase calc = GetCharacterCalculations(character, additionalItem, calculationOptions, null, computeIncrementalSet);
                if (!character.DisableBuffAutoActivation) calculationOptions.IncrementalSetArmor = ((CharacterCalculationsMage)calc).MageArmor;
                if (computeIncrementalSet) StoreIncrementalSet(character, (CharacterCalculationsMage)calc);
                ret = calc;
            }
            calculationOptions.IncrementalOptimizations = savedIncrementalOptimizations;
            return ret;
        }

        private void StoreIncrementalSet(Character character, CharacterCalculationsMage calculations)
        {
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            List<int> cooldownList = new List<int>();
            List<SpellId> spellList = new List<SpellId>();
            List<int> segmentList = new List<int>();
            for (int i = 0; i < calculations.SolutionStats.Length; i++)
            {
                if (calculations.Solution[i] > 0 && calculations.IncrementalSetSpell[i] != SpellId.None)
                {
                    cooldownList.Add(calculations.IncrementalSetCooldown[i]);
                    spellList.Add(calculations.IncrementalSetSpell[i]);
                    if (calculations.IncrementalSetSegment != null) segmentList.Add(calculations.IncrementalSetSegment[i]);
                }
            }
            calculationOptions.IncrementalSetCooldowns = cooldownList.ToArray();
            calculationOptions.IncrementalSetSpells = spellList.ToArray();
            calculationOptions.IncrementalSetSegments = segmentList.ToArray();
            calculationOptions.IncrementalSetArmor = calculations.MageArmor;

            List<int> filteredCooldowns = ListUtils.RemoveDuplicates(cooldownList);
            filteredCooldowns.Sort();
            calculationOptions.IncrementalSetSortedCooldowns = filteredCooldowns.ToArray();
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, CalculationOptionsMage calculationOptions, string armor, bool computeIncrementalSet)
        {
            if (calculationOptions.SMP && !calculationOptions.SMPDisplay)
            {
                return GetCharacterCalculations(character, additionalItem, calculationOptions, armor, computeIncrementalSet, true);
            }
            else
            {
                return GetCharacterCalculations(character, additionalItem, calculationOptions, armor, computeIncrementalSet, false);
            }
        }

        // promoted variables from GetCharacterCalculations to make it easier to refactor the code, not thread-safe
        private const double segmentDuration = 30;
        private int segments;
        private List<CharacterCalculationsMage> statsList;
        private List<SpellId> spellList;
        private const int colOffset = 7;
        private CompactLP lp;
        private Heap<CompactLP> heap;
        private double[] solution;
        private CharacterCalculationsMage calculatedStats;

        private double MaximizeColdsnapDuration(double fightDuration, double coldsnapCooldown, double effectDuration, double effectCooldown, out int coldsnapCount)
        {
            int bestColdsnap = 0;
            double bestEffect = 0.0;
            List<int> coldsnap = new List<int>();
            List<double> startTime = new List<double>();
            List<double> coldsnapTime = new List<double>();
            int index = 0;
            coldsnap.Add(2);
            startTime.Add(0.0);
            coldsnapTime.Add(0.0);
            do
            {
                if (index > 0 && startTime[index - 1] + effectDuration >= fightDuration)
                {
                    double effect = (index - 1) * effectDuration + Math.Max(fightDuration - startTime[index - 1], 0.0);
                    if (effect > bestEffect)
                    {
                        bestEffect = effect;
                        bestColdsnap = 0;
                        for (int i = 0; i < index; i++)
                        {
                            if (startTime[i] < fightDuration - 20.0) bestColdsnap += coldsnap[i]; // if it is a coldsnap for a very short elemental, don't count it for IV
                        }
                    }
                    index--;
                }
                coldsnap[index]--;
                if (coldsnap[index] < 0)
                {
                    index--;
                }
                else
                {
                    double time = 0.0;
                    if (index > 0)
                    {
                        time = startTime[index - 1] + effectDuration;
                        int lastColdsnap = -1;
                        for (int j = 0; j < index; j++)
                        {
                            if (coldsnap[j] == 1) lastColdsnap = j;
                        }
                        if (coldsnap[index] == 1)
                        {
                            // use coldsnap
                            double normalTime = Math.Max(time, startTime[index - 1] + effectCooldown);
                            double coldsnapReady = 0.0;
                            if (lastColdsnap >= 0) coldsnapReady = coldsnapTime[lastColdsnap] + coldsnapCooldown;
                            if (coldsnapReady >= normalTime)
                            {
                                // coldsnap won't be ready until effect will be back anyway, so we don't actually need it
                                coldsnap[index] = 0;
                                time = normalTime;
                            }
                            else
                            {
                                // go now or when coldsnap is ready
                                time = Math.Max(coldsnapReady, time);
                                coldsnapTime[index] = Math.Max(coldsnapReady, startTime[index - 1]);
                            }
                        }
                        else
                        {
                            // we are not allowed to use coldsnap even if we could
                            // make sure to adjust by coldsnap constraints
                            time = Math.Max(time, startTime[index - 1] + effectCooldown);
                        }
                    }
                    else
                    {
                        coldsnap[index] = 0;
                    }
                    startTime[index] = time;
                    index++;
                    if (index >= coldsnap.Count)
                    {
                        coldsnap.Add(0);
                        coldsnapTime.Add(0.0);
                        startTime.Add(0.0);
                    }
                    coldsnap[index] = 2;
                }
            } while (index >= 0);
            coldsnapCount = bestColdsnap;
            return bestEffect;
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, CalculationOptionsMage calculationOptions, string armor, bool computeIncrementalSet, bool useSMP)
        {
            List<string> autoActivatedBuffs = new List<string>();
            Stats rawStats = GetRawStats(character, additionalItem, calculationOptions, autoActivatedBuffs, armor);
            Stats characterStats = GetCharacterStats(character, additionalItem, rawStats, calculationOptions);

            bool savedSmartOptimization = calculationOptions.SmartOptimization;
            bool savedABCycles = calculationOptions.ABCycles;
            bool savedDestructionPotion = calculationOptions.DestructionPotion;
            bool savedFlameCap = calculationOptions.FlameCap;

            if (useSMP) calculationOptions.SmartOptimization = true;
            segments = useSMP ? (int)Math.Ceiling(calculationOptions.FightDuration / segmentDuration) : 1;

            bool heroismAvailable = calculationOptions.HeroismAvailable;
            bool apAvailable = calculationOptions.ArcanePower == 1;
            bool ivAvailable = calculationOptions.IcyVeins == 1;
            bool combustionAvailable = calculationOptions.Combustion == 1;
            bool mfAvailable = calculationOptions.MoltenFury > 0;
            bool trinket1Available = IsItemActivatable(character.Trinket1);
            bool trinket2Available = IsItemActivatable(character.Trinket2);
            bool coldsnap = calculationOptions.ColdSnap == 1;
            float coldsnapCooldown = 8 * 60 * (1 - 0.1f * calculationOptions.IceFloes);
            float combustionCount = 0;

            double trinket1cooldown = 0, trinket1duration = 0, trinket2cooldown = 0, trinket2duration = 0, t1length = 0, t2length = 0;
            bool t1ismg = false, t2ismg = false;

            if (calculationOptions.SmartOptimization)
            {
                if (calculationOptions.SpellPower == 0)
                {
                    calculationOptions.ABCycles = false;
                }
                else
                {
                    calculationOptions.DestructionPotion = false;
                    calculationOptions.FlameCap = false;
                }
            }

            #region Setup Trinkets
            if (trinket1Available)
            {
                Stats s = character.Trinket1.Stats;
                if (s.SpellDamageFor20SecOnUse2Min + s.SpellHasteFor20SecOnUse2Min + s.Mp5OnCastFor20SecOnUse2Min > 0)
                {
                    trinket1duration = 20;
                    trinket1cooldown = 120;
                }
                if (s.SpellDamageFor15SecOnManaGem > 0)
                {
                    trinket1duration = 15;
                    trinket1cooldown = 120;
                    t1ismg = true;
                }
                if (s.SpellDamageFor15SecOnUse90Sec > 0)
                {
                    trinket1duration = 15;
                    trinket1cooldown = 90;
                }
                if (s.SpellHasteFor20SecOnUse5Min > 0)
                {
                    trinket1duration = 20;
                    trinket1cooldown = 300;
                }
                if (s.SpellDamageFor15SecOnUse2Min > 0)
                {
                    trinket1duration = 15;
                    trinket1cooldown = 120;
                }
                t1length = (1 + (int)((calculatedStats.FightDuration - trinket1duration) / trinket1cooldown)) * trinket1duration;
            }
            if (trinket2Available)
            {
                Stats s = character.Trinket2.Stats;
                if (s.SpellDamageFor20SecOnUse2Min + s.SpellHasteFor20SecOnUse2Min + s.Mp5OnCastFor20SecOnUse2Min > 0)
                {
                    trinket2duration = 20;
                    trinket2cooldown = 120;
                }
                if (s.SpellDamageFor15SecOnManaGem > 0)
                {
                    trinket2duration = 15;
                    trinket2cooldown = 120;
                    t2ismg = true;
                }
                if (s.SpellDamageFor15SecOnUse90Sec > 0)
                {
                    trinket2duration = 15;
                    trinket2cooldown = 90;
                }
                if (s.SpellHasteFor20SecOnUse5Min > 0)
                {
                    trinket2duration = 20;
                    trinket2cooldown = 300;
                }
                if (s.SpellDamageFor15SecOnUse2Min > 0)
                {
                    trinket2duration = 15;
                    trinket2cooldown = 120;
                }
                t2length = (1 + (int)((calculatedStats.FightDuration - trinket2duration) / trinket2cooldown)) * trinket2duration;
            }
            #endregion

            if (armor == null)
            {
                if (character.ActiveBuffs.Contains("Mage Armor")) armor = "Mage Armor";
                if (character.ActiveBuffs.Contains("Molten Armor")) armor = "Molten Armor";
            }

            #region Load Stats
            // temporary buffs: Arcane Power, Icy Veins, Molten Fury, Combustion?, Trinket1, Trinket2, Heroism, Destro Pot, Flame Cap, Drums?
            // compute stats for temporary bonuses, each gives a list of spells used for final LP, solutions of LP stored in calculatedStats
            statsList = new List<CharacterCalculationsMage>();

            calculatedStats = null;

            int incrementalSetIndex = 0;
            int incrementalSortedIndex = 0;
            if (calculationOptions.IncrementalOptimizations && calculationOptions.IncrementalSetSortedCooldowns.Length == 0) goto EscapeCooldownLoop;
            for (int mf = 0; mf < 2; mf++)
            for (int heroism = 0; heroism < 2; heroism++)
            for (int ap = 0; ap < 2; ap++)
            for (int iv = 0; iv < 2; iv++)
            for (int combustion = 0; combustion < 2; combustion++)
            for (int drums = 0; drums < 2; drums++)
            for (int flameCap = 0; flameCap < 2; flameCap++)
            for (int destructionPotion = 0; destructionPotion < 2; destructionPotion++)
            {
                if (!calculationOptions.IncrementalOptimizations || (calculationOptions.IncrementalSetSortedCooldowns[incrementalSortedIndex] == incrementalSetIndex))
                {
                    for (int trinket1 = 0; trinket1 < 2; trinket1++)
                        for (int trinket2 = 0; trinket2 < 2; trinket2++)
                            if ((mfAvailable || mf == 1) && (heroismAvailable || heroism == 1) && (apAvailable || ap == 1) && (ivAvailable || iv == 1) && (calculationOptions.DestructionPotion || destructionPotion == 1) && (calculationOptions.FlameCap || flameCap == 1) && (trinket1Available || trinket1 == 1) && (trinket2Available || trinket2 == 1) && (combustion == 1 || calculationOptions.Combustion == 1) && (drums == 1 || calculationOptions.DrumsOfBattle))
                            {
                                if (!(trinket1 == 0 && trinket2 == 0) || (character.Trinket1.Stats.SpellDamageFor15SecOnManaGem > 0 || character.Trinket2.Stats.SpellDamageFor15SecOnManaGem > 0)) // only leave through trinkets that can stack
                                {
                                    if (!((trinket1 == 0 && t1ismg && flameCap == 0) || (trinket2 == 0 && t2ismg && flameCap == 0))) // do not allow SCB together with flame cap
                                    {
                                        if (!((calculationOptions.HeroismControl == 1 && heroism == 0 && mf == 0) || (calculationOptions.HeroismControl == 2 && heroism == 0 && (mf == 0 || ap == 0 || iv == 0 || destructionPotion == 0 || flameCap == 0 || trinket1 == 0 || trinket2 == 0 || combustion == 0 || drums == 0))))
                                        {
                                            statsList.Add(GetTemporaryCharacterCalculations(characterStats, calculationOptions, armor, character, additionalItem, ap == 0, mf == 0, iv == 0, heroism == 0, destructionPotion == 0, flameCap == 0, trinket1 == 0, trinket2 == 0, combustion == 0, drums == 0, incrementalSetIndex));
                                            if (ap != 0 && mf != 0 && iv != 0 && heroism != 0 && destructionPotion != 0 && flameCap != 0 && trinket1 != 0 && trinket2 != 0 && combustion != 0 && drums != 0)
                                            {
                                                calculatedStats = statsList[statsList.Count - 1];
                                            }
                                        }
                                    }
                                }
                            }
                    if (calculationOptions.IncrementalOptimizations)
                    {
                        incrementalSortedIndex++;
                        if (incrementalSortedIndex >= calculationOptions.IncrementalSetSortedCooldowns.Length) goto EscapeCooldownLoop;
                    }
                }
                incrementalSetIndex++;
            }
            EscapeCooldownLoop:
            if (calculatedStats == null) calculatedStats = GetTemporaryCharacterCalculations(characterStats, calculationOptions, armor, character, additionalItem, false, false, false, false, false, false, false, false, false, false, incrementalSetIndex - 1);
            #endregion

            calculatedStats.AutoActivatedBuffs.AddRange(autoActivatedBuffs);
            calculatedStats.MageArmor = armor;

            #region Load Spells
            spellList = new List<SpellId>();

            if (calculationOptions.SmartOptimization)
            {
                if (calculationOptions.EmpoweredFireball > 0)
                {
                    spellList.Add(calculationOptions.MaintainScorch ? SpellId.FireballScorch : SpellId.Fireball);
                }
                else if (calculationOptions.EmpoweredFrostbolt > 0)
                {
                    spellList.Add(SpellId.Frostbolt);
                }
                else if (calculationOptions.SpellPower > 0)
                {
                    spellList.Add(SpellId.ArcaneBlast33);
                    if (calculationOptions.ImprovedFrostbolt > 0) spellList.Add(SpellId.Frostbolt);
                    if (calculationOptions.ImprovedFireball > 0) spellList.Add(calculationOptions.MaintainScorch ? SpellId.FireballScorch : SpellId.Fireball);
                    if (calculationOptions.ImprovedArcaneMissiles + calculationOptions.EmpoweredArcaneMissiles > 0) spellList.Add(SpellId.ArcaneMissiles);
                }
                else
                {
                    spellList.Add(SpellId.ArcaneMissiles);
                    spellList.Add(SpellId.Scorch);
                    spellList.Add(calculationOptions.MaintainScorch ? SpellId.FireballScorch : SpellId.Fireball);
                    spellList.Add(SpellId.Frostbolt);
                    spellList.Add(SpellId.ArcaneBlast33);
                }
            }
            else
            {
                spellList.Add(SpellId.ArcaneMissiles);
                spellList.Add(SpellId.Scorch);
                spellList.Add(calculationOptions.MaintainScorch ? SpellId.FireballScorch : SpellId.Fireball);
                spellList.Add(SpellId.FireballFireBlast);
                spellList.Add(SpellId.Frostbolt);
                spellList.Add(SpellId.ArcaneBlast33);
            }
            if (calculationOptions.ABCycles)
            {
                if (calculationOptions.EmpoweredArcaneMissiles > 0)
                {
                    spellList.Add(SpellId.ABAMP);
                    spellList.Add(SpellId.ABAM);
                    spellList.Add(SpellId.AB3AMSc);
                    spellList.Add(SpellId.ABAM3Sc);
                    spellList.Add(SpellId.ABAM3Sc2);
                    spellList.Add(SpellId.ABAM3FrB);
                    spellList.Add(SpellId.ABAM3FrB2);
                    spellList.Add(SpellId.ABAM3ScCCAM);
                    spellList.Add(SpellId.ABAM3Sc2CCAM);
                    spellList.Add(SpellId.ABAM3FrBCCAM);
                    spellList.Add(SpellId.ABAM3FrBScCCAM);
                    spellList.Add(SpellId.ABAMCCAM);
                    spellList.Add(SpellId.ABAM3CCAM);
                }
                if (calculationOptions.ImprovedFrostbolt > 0)
                {
                    spellList.Add(SpellId.ABFrB3FrB);
                    spellList.Add(SpellId.ABFrB3FrBSc);
                }
                if (calculationOptions.ImprovedFireball > 0)
                {
                    spellList.Add(SpellId.ABFB3FBSc);
                    //spellList.Add(SpellId.AB3Sc);
                }
            }
            if (calculationOptions.AoeDuration > 0)
            {
                spellList.Add(SpellId.ArcaneExplosion);
                spellList.Add(SpellId.FlamestrikeSpammed);
                spellList.Add(SpellId.FlamestrikeSingle);
                spellList.Add(SpellId.Blizzard);
                spellList.Add(SpellId.ConeOfCold);
                if (calculationOptions.BlastWave == 1) spellList.Add(SpellId.BlastWave);
                if (calculationOptions.DragonsBreath == 1) spellList.Add(SpellId.DragonsBreath);
            }
            #endregion

            int rowOffset = 42;
            int lpRows = rowOffset + (useSMP ? 11 * segments : 0); // packing constraints for each of 10 cooldowns + timing for each segment
            int lpCols = colOffset - 1 + spellList.Count * statsList.Count * segments;
            lp = new CompactLP(lpRows, lpCols);
            double[] tps = new double[lpCols];
            calculatedStats.SolutionStats = new CharacterCalculationsMage[lpCols];
            calculatedStats.SolutionSpells = new Spell[lpCols];
            if (useSMP) calculatedStats.SolutionSegments = new int[lpCols];
            //calculatedStats.SolutionLabel = new string[lpCols];

            int[] incrementalSetCooldown = null;
            SpellId[] incrementalSetSpell = null;
            int[] incrementalSetSegment = null;
            if (computeIncrementalSet)
            {
                incrementalSetCooldown = new int[lpCols];
                incrementalSetSpell = new SpellId[lpCols];
                if (useSMP) incrementalSetSegment = new int[lpCols];
            }

            calculatedStats.Trinket1Duration = trinket1duration;
            calculatedStats.Trinket1Cooldown = trinket1cooldown;
            calculatedStats.Trinket2Duration = trinket2duration;
            calculatedStats.Trinket2Cooldown = trinket2cooldown;

            combustionCount = combustionAvailable ? (1 + (int)((calculatedStats.FightDuration - 15f) / 195f)) : 0;

            #region Water Elemental
            int coldsnapCount = coldsnap ? (1 + (int)((calculatedStats.FightDuration - 45f) / coldsnapCooldown)) : 0;

            // water elemental
            if (calculationOptions.SummonWaterElemental == 1)
            {
                int targetLevel = calculationOptions.TargetLevel;
                calculatedStats.WaterElemental = true;
                // 45 sec, 3 min cooldown + cold snap
                // 2.5 sec Waterbolt, affected by heroism, totems, 0.4x frost damage from character
                // TODO consider adding water elemental as part of optimization for stacking with cooldowns
                // TODO add GCD for summoning and mana cost
                float spellHit = 0;
                if (character.ActiveBuffs.Contains("Totem of Wrath")) spellHit += 0.03f;
                if (character.ActiveBuffs.Contains("Inspiring Presence")) spellHit += 0.01f;
                float hitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + spellHit);
                float spellCrit = 0.05f;
                if (character.ActiveBuffs.Contains("Winter's Chill") || calculationOptions.WintersChill == 1) spellHit += 0.1f;
                float multiplier = hitRate;
                if (character.ActiveBuffs.Contains("Curse of the Elements")) multiplier *= 1.1f;
                if (character.ActiveBuffs.Contains("Improved Curse of the Elements")) multiplier *= 1.13f / 1.1f;
                if (character.ActiveBuffs.Contains("Misery")) multiplier *= 1.05f;
                float realResistance = calculationOptions.FrostResist;
                float partialResistFactor = (realResistance == 1) ? 0 : (1 - realResistance - ((targetLevel > 70) ? ((targetLevel - 70) * 0.02f) : 0f));
                multiplier *= partialResistFactor;
                calculatedStats.WaterElementalDps = (521.5f + (0.4f * calculatedStats.FrostDamage + (character.ActiveBuffs.Contains("Wrath of Air") ? 101 : 0)) * 2.5f / 3.5f) * multiplier * (1 + 0.5f * spellCrit) / 2.5f;
                calculatedStats.WaterElementalDuration = (float)(1 + (int)((calculatedStats.FightDuration - 45f) / 180f)) * 45;
                if (coldsnap) calculatedStats.WaterElementalDuration = (float)MaximizeColdsnapDuration(calculationOptions.FightDuration, coldsnapCooldown, 45.0, 180.0, out coldsnapCount);
                /*calculatedStats.WaterElementalDuration = (float)(1 + coldsnapCount + (int)((calculatedStats.FightDuration - coldsnapCount * coldsnapDelay - 45f) / 180f)) * 45;
                float nextElementalEnd = (float)((calculatedStats.WaterElementalDuration / 45f - coldsnapCount) * 180f + coldsnapCount * coldsnapDelay + 45f);
                if (nextElementalEnd - 45.0f < calculationOptions.FightDuration) calculatedStats.WaterElementalDuration += calculationOptions.FightDuration - nextElementalEnd + 45.0f;
                calculatedStats.WaterElementalDuration = Math.Min(calculatedStats.WaterElementalDuration, calculationOptions.FightDuration);*/
                if (heroismAvailable)
                {
                    float heroTime = Math.Min(40.0f, calculatedStats.WaterElementalDuration);
                    calculatedStats.WaterElementalDamage = calculatedStats.WaterElementalDps * ((calculatedStats.WaterElementalDuration - heroTime) + heroTime * 1.3f);
                }
                else
                    calculatedStats.WaterElementalDamage = calculatedStats.WaterElementalDuration * calculatedStats.WaterElementalDps;
            }
            #endregion

            // fill model [mana regen, time limit, evocation limit, mana pot limit, heroism cooldown, ap cooldown, ap+heroism cooldown, iv cooldown, mf cooldown, mf+dp cooldown, mf+iv cooldown, dp+heroism cooldown, dp+iv cooldown, flame cap cooldown, molten+flame, dp+flame, trinket1, trinket2, trinket1+mf, trinket2+mf, trinket1+heroism, trinket2+heroism, mana gem > scb, dps time, aoe duration, flamestrike, cone of cold, blast wave, dragon's breath, combustion, combustion+mf, heroism+iv, drums, drums+mf, drums+heroism, drums+iv, drums+ap, threat, pot+gem, drumsmax]
            double aplength = (1 + (int)((calculatedStats.FightDuration - 30f) / 180f)) * 15;
            double ivlength = 0.0;
            if (calculationOptions.SummonWaterElemental == 0 && coldsnap)
            {
                ivlength = Math.Floor(MaximizeColdsnapDuration(calculationOptions.FightDuration, coldsnapCooldown, 20.0, 180.0, out coldsnapCount));
            }
            else if (calculationOptions.SummonWaterElemental == 1 && coldsnap)
            {
                double wecount = (calculatedStats.WaterElementalDuration / 45.0);
                if (wecount >= Math.Floor(wecount) + 20.0 / 45.0)
                    ivlength = Math.Ceiling(wecount) * 20.0;
                else
                    ivlength = Math.Floor(wecount) * 20.0;
            }
            else
            {
                ivlength = (1 + (int)((calculatedStats.FightDuration - 20f) / 180f)) * 20;
            }
            double mflength = calculationOptions.MoltenFuryPercentage * calculatedStats.FightDuration;
            double dpivstackArea = calculatedStats.FightDuration;
            //if (mfAvailable && heroismAvailable) dpivstackArea -= 120; // only applies if heroism and iv cannot stack
            double dpivlength = 15 * (int)(dpivstackArea / 360f);
            if (dpivstackArea % 360f < 195)
            {
                dpivlength += 15;
            }
            else
            {
                dpivlength += 30;
            }
            double dpflamelength = 15 * (int)(calculatedStats.FightDuration / 360f);
            if (calculatedStats.FightDuration % 360f < 195)
            {
                dpflamelength += 15;
            }
            else
            {
                dpflamelength += 30;
            }
            double drumsivlength = 20 * (int)(calculatedStats.FightDuration / 360f);
            if (calculatedStats.FightDuration % 360f < 195)
            {
                drumsivlength += 20;
            }
            else
            {
                drumsivlength += 40;
            }
            double drumsaplength = 15 * (int)(calculatedStats.FightDuration / 360f);
            if (calculatedStats.FightDuration % 360f < 195)
            {
                drumsaplength += 15;
            }
            else
            {
                drumsaplength += 30;
            }

            #region Disable Constraints/Variables
            // disable unused constraints and variables
            incrementalSortedIndex = 0;
            int lastCooldownSet = -1;
            int lastCooldownSetSortedIndex = 0;
            if (character.Ranged == null || character.Ranged.Type != Item.ItemType.Wand) lp.DisableColumn(1);
            for (int seg = 0; seg < segments; seg++)
            {
                if (calculationOptions.IncrementalOptimizations)
                {
                    if (useSMP)
                    {
                        while (incrementalSortedIndex < calculationOptions.IncrementalSetCooldowns.Length && seg > calculationOptions.IncrementalSetSegments[incrementalSortedIndex])
                        {
                            incrementalSortedIndex++;
                        }
                    }
                }
                lastCooldownSet = -1;
                for (int buffset = 0; buffset < statsList.Count; buffset++)
                {
                    if (calculationOptions.IncrementalOptimizations)
                    {
                        if (statsList[buffset].IncrementalSetIndex != lastCooldownSet)
                        {
                            lastCooldownSet = statsList[buffset].IncrementalSetIndex;
                            while (incrementalSortedIndex < calculationOptions.IncrementalSetCooldowns.Length && (!useSMP || seg == calculationOptions.IncrementalSetSegments[incrementalSortedIndex]) && lastCooldownSet > calculationOptions.IncrementalSetCooldowns[incrementalSortedIndex])
                            {
                                incrementalSortedIndex++;
                            }
                            lastCooldownSetSortedIndex = incrementalSortedIndex;
                        }
                    }
                    for (int spell = 0; spell < spellList.Count; spell++)
                    {
                        incrementalSortedIndex = lastCooldownSetSortedIndex; // rewind sorted index
                        bool viable = true;
                        if (calculationOptions.IncrementalOptimizations)
                        {
                            viable = false;
                            while (incrementalSortedIndex < calculationOptions.IncrementalSetCooldowns.Length && (!useSMP || seg == calculationOptions.IncrementalSetSegments[incrementalSortedIndex]) && statsList[buffset].IncrementalSetIndex == calculationOptions.IncrementalSetCooldowns[incrementalSortedIndex])
                            {
                                if (spellList[spell] == calculationOptions.IncrementalSetSpells[incrementalSortedIndex])
                                {
                                    viable = true;
                                    break;
                                }
                                incrementalSortedIndex++;
                            }
                        }
                        if (useSMP && statsList[buffset].MoltenFury && (seg + 1) * segmentDuration <= calculationOptions.FightDuration - mflength) viable = false;
                        if (statsList[buffset] == calculatedStats && seg < segments - 1) viable = false;
                        if (viable)
                        {
                            Spell s = statsList[buffset].GetSpell(spellList[spell]);
                            bool spellRelevant = true;
                            if (!s.AffectedByFlameCap && statsList[buffset].FlameCap) spellRelevant = false;
                            if (s.ABCycle && !calculationOptions.ABCycles) spellRelevant = false;
                            if (calculationOptions.SmartOptimization)
                            {
                                if (calculationOptions.EmpoweredFireball > 0)
                                {
                                    if (!s.AreaEffect && !(s is Fireball || s is FireballScorch)) spellRelevant = false;
                                }
                            }
                            if (!spellRelevant)
                            {
                                int index = (seg * statsList.Count + buffset) * spellList.Count + spell + colOffset - 1;
                                lp.DisableColumn(index);
                            }
                        }
                        else
                        {
                            int index = (seg * statsList.Count + buffset) * spellList.Count + spell + colOffset - 1;
                            lp.DisableColumn(index);
                        }
                    }
                }
            }
            if (!heroismAvailable) lp.DisableRow(5);
            if (!apAvailable) lp.DisableRow(6);
            if (!heroismAvailable || !apAvailable) lp.DisableRow(7);
            if (!ivAvailable) lp.DisableRow(8);
            if (!mfAvailable) lp.DisableRow(9);
            if (!mfAvailable || !calculationOptions.DestructionPotion) lp.DisableRow(10);
            if (!mfAvailable || !ivAvailable) lp.DisableRow(11);
            if (!heroismAvailable || !calculationOptions.DestructionPotion) lp.DisableRow(12);
            if (!ivAvailable || !calculationOptions.DestructionPotion) lp.DisableRow(13);
            if (!mfAvailable || !calculationOptions.FlameCap) lp.DisableRow(15);
            if (!calculationOptions.FlameCap || !calculationOptions.DestructionPotion) lp.DisableRow(16);
            if (!trinket1Available) lp.DisableRow(17);
            if (!trinket2Available) lp.DisableRow(18);
            if (!(mfAvailable && trinket1Available)) lp.DisableRow(19);
            if (!(mfAvailable && trinket2Available)) lp.DisableRow(20);
            if (!(heroismAvailable && trinket1Available)) lp.DisableRow(21);
            if (!(heroismAvailable && trinket2Available)) lp.DisableRow(22);
            if (calculationOptions.AoeDuration > 0)
            {
                if (calculationOptions.BlastWave == 0) lp.DisableRow(28);
                if (calculationOptions.DragonsBreath == 0) lp.DisableRow(29);
            }
            else
            {
                lp.DisableRow(25);
                lp.DisableRow(26);
                lp.DisableRow(27);
                lp.DisableRow(28);
                lp.DisableRow(29);
            }
            if (!combustionAvailable) lp.DisableRow(30);
            if (!(combustionAvailable && mfAvailable)) lp.DisableRow(31);
            if (!(combustionAvailable && heroismAvailable)) lp.DisableRow(32);
            if (!(ivAvailable && heroismAvailable)) lp.DisableRow(33);
            if (!calculationOptions.DrumsOfBattle) lp.DisableRow(34);
            if (!(calculationOptions.DrumsOfBattle && mfAvailable)) lp.DisableRow(35);
            if (!(calculationOptions.DrumsOfBattle && heroismAvailable)) lp.DisableRow(36);
            if (!(calculationOptions.DrumsOfBattle && ivAvailable)) lp.DisableRow(37);
            if (!(calculationOptions.DrumsOfBattle && apAvailable)) lp.DisableRow(38);
            if (calculationOptions.TpsLimit >= 5000 || calculationOptions.TpsLimit == 0.0) lp.DisableRow(39);
            if (!calculationOptions.DrumsOfBattle) lp.DisableRow(41);
            if (useSMP)
            {
                // mf, heroism, ap, iv, combustion, drums, flamecap, destruction, t1, t2
                // mf
                for (int seg = 0; seg < segments; seg++)
                {
                    if (mfAvailable || (seg + 1) * segmentDuration <= calculationOptions.FightDuration - mflength) lp.DisableRow(rowOffset + 0 * segments + seg);
                }
                // heroism
                for (int seg = 0; seg < segments; seg++)
                {
                    lp.DisableRow(rowOffset + 1 * segments + seg); // disable all, might change this if we decide to model multiple heroisms
                }
                // ap
                bool allCovered = !apAvailable;
                for (int seg = 0; seg < segments; seg++)
                {
                    if (allCovered) lp.DisableRow(rowOffset + 2 * segments + seg);
                    double cool = 180;
                    if (seg * segmentDuration + cool >= calculationOptions.FightDuration) allCovered = true;
                }
                // iv
                allCovered = !ivAvailable;
                for (int seg = 0; seg < segments; seg++)
                {
                    if (allCovered) lp.DisableRow(rowOffset + 3 * segments + seg);
                    double cool = 180 + (coldsnap ? 20 : 0);
                    if (seg * segmentDuration + cool >= calculationOptions.FightDuration) allCovered = true;
                }
                // combustion
                allCovered = !combustionAvailable;
                for (int seg = 0; seg < segments; seg++)
                {
                    if (allCovered) lp.DisableRow(rowOffset + 4 * segments + seg);
                    double cool = 180 + 15;
                    if (seg * segmentDuration + cool >= calculationOptions.FightDuration) allCovered = true;
                }
                // drums
                allCovered = !calculationOptions.DrumsOfBattle;
                for (int seg = 0; seg < segments; seg++)
                {
                    if (allCovered) lp.DisableRow(rowOffset + 5 * segments + seg);
                    double cool = 120;
                    if (seg * segmentDuration + cool >= calculationOptions.FightDuration) allCovered = true;
                }
                // flamecap
                allCovered = !calculationOptions.FlameCap;
                for (int seg = 0; seg < segments; seg++)
                {
                    if (allCovered) lp.DisableRow(rowOffset + 6 * segments + seg);
                    double cool = 180;
                    if (seg * segmentDuration + cool >= calculationOptions.FightDuration) allCovered = true;
                }
                // destruction
                allCovered = !calculationOptions.DestructionPotion;
                for (int seg = 0; seg < segments; seg++)
                {
                    if (allCovered) lp.DisableRow(rowOffset + 7 * segments + seg);
                    double cool = 120;
                    if (seg * segmentDuration + cool >= calculationOptions.FightDuration) allCovered = true;
                }
                // t1
                allCovered = !trinket1Available;
                for (int seg = 0; seg < segments; seg++)
                {
                    if (allCovered) lp.DisableRow(rowOffset + 8 * segments + seg);
                    double cool = trinket1cooldown;
                    if (seg * segmentDuration + cool >= calculationOptions.FightDuration) allCovered = true;
                }
                // t2
                allCovered = !trinket2Available;
                for (int seg = 0; seg < segments; seg++)
                {
                    if (allCovered) lp.DisableRow(rowOffset + 9 * segments + seg);
                    double cool = trinket2cooldown;
                    if (seg * segmentDuration + cool >= calculationOptions.FightDuration) allCovered = true;
                }
            }
            #endregion

            lp.Compact();

            #region Set LP Scaling
            lp.SetRowScale(0, 0.1);
            lp.SetRowScale(3, 40.0 / calculatedStats.ManaPotionTime);
            lp.SetRowScale(4, 40.0 / calculatedStats.ManaPotionTime);
            lp.SetRowScale(14, 40.0 / calculatedStats.ManaPotionTime);
            lp.SetRowScale(30, 10.0);
            lp.SetRowScale(31, 10.0);
            lp.SetRowScale(32, 10.0);
            lp.SetRowScale(34, 30.0);
            lp.SetRowScale(39, 0.05);
            lp.SetRowScale(41, 30.0);
            lp.SetRowScale(lpRows, 0.05);
            lp.SetColumnScale(3, calculatedStats.ManaPotionTime);
            lp.SetColumnScale(4, calculatedStats.ManaPotionTime);
            #endregion

            float threatFactor = (1 + characterStats.ThreatIncreaseMultiplier) * (1 - characterStats.ThreatReductionMultiplier);

            #region Formulate LP
            // idle regen
            //calculatedStats.SolutionLabel[0] = "Idle Regen";
            lp[0, 0] = -(calculatedStats.ManaRegen * (1 - calculationOptions.Fragmentation) + calculatedStats.ManaRegen5SR * calculationOptions.Fragmentation);
            lp[1, 0] = 1;
            lp[24, 0] = -1;
            lp[lpRows, 0] = 0;
            // wand
            //calculatedStats.SolutionLabel[1] = "Wand";
            if (character.Ranged != null && character.Ranged.Type == Item.ItemType.Wand)
            {
                Spell wand = new Wand(character, calculatedStats, (MagicSchool)character.Ranged.DamageType, character.Ranged.MinDamage, character.Ranged.MaxDamage, character.Ranged.Speed);
                calculatedStats.SetSpell(SpellId.Wand, wand);
                lp[0, 1] = wand.CostPerSecond - wand.ManaRegenPerSecond;
                lp[1, 1] = 1;
                lp[39, 1] = tps[1] = wand.ThreatPerSecond;
                lp[lpRows, 1] = wand.DamagePerSecond;
            }
            // evocation
            double evocationDuration = (8f + characterStats.EvocationExtension) / calculatedStats.CastingSpeed;
            calculatedStats.EvocationDuration = evocationDuration;
            //calculatedStats.SolutionLabel[2] = "Evocation";
            float evocationMana = characterStats.Mana;
            calculatedStats.EvocationRegen = calculatedStats.ManaRegen5SR + 0.15f * evocationMana / 2f * calculatedStats.CastingSpeed;
            if (calculationOptions.EvocationWeapon + calculationOptions.EvocationSpirit > 0)
            {
                Stats evocationRawStats = rawStats.Clone();
                if (character.MainHand != null)
                {
                    evocationRawStats.Intellect -= character.MainHand.GetTotalStats().Intellect;
                    evocationRawStats.Spirit -= character.MainHand.GetTotalStats().Spirit;
                }
                if (character.OffHand != null)
                {
                    evocationRawStats.Intellect -= character.OffHand.GetTotalStats().Intellect;
                    evocationRawStats.Spirit -= character.OffHand.GetTotalStats().Spirit;
                }
                if (character.Ranged != null)
                {
                    evocationRawStats.Intellect -= character.Ranged.GetTotalStats().Intellect;
                    evocationRawStats.Spirit -= character.Ranged.GetTotalStats().Spirit;
                }
                if (character.MainHandEnchant != null)
                {
                    evocationRawStats.Intellect -= character.MainHandEnchant.Stats.Intellect;
                    evocationRawStats.Spirit -= character.MainHandEnchant.Stats.Spirit;
                }
                evocationRawStats.Intellect += calculationOptions.EvocationWeapon;
                evocationRawStats.Spirit += calculationOptions.EvocationSpirit;
                Stats evocationStats = GetCharacterStats(character, additionalItem, evocationRawStats, calculationOptions);
                float evocationRegen = ((0.001f + evocationStats.Spirit * 0.009327f * (float)Math.Sqrt(evocationStats.Intellect)) * evocationStats.SpellCombatManaRegeneration + evocationStats.Mp5 / 5f + calculatedStats.SpiritRegen * (5 - characterStats.SpellCombatManaRegeneration) * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration) + 0.15f * evocationStats.Mana / 2f * calculatedStats.CastingSpeed;
                if (evocationRegen > calculatedStats.EvocationRegen)
                {
                    evocationMana = evocationStats.Mana;
                    calculatedStats.EvocationRegen = evocationRegen;
                }
            }
            lp[0, 2] = -calculatedStats.EvocationRegen;
            lp[1, 2] = 1;
            lp[2, 2] = 1;
            lp[39, 2] = tps[2] = 0.15f * evocationMana / 2f * calculatedStats.CastingSpeed * 0.5f * threatFactor; // should split among all targets if more than one, assume one only
            lp[lpRows, 2] = 0;
            // mana pot
            //calculatedStats.SolutionLabel[3] = "Mana Potion";
            calculatedStats.MaxManaPotion = 1 + (int)((calculatedStats.FightDuration - 30f) / 120f);
            lp[0, 3] = -calculatedStats.ManaRegen5SR - (1 + characterStats.BonusManaPotion) * 2400f / calculatedStats.ManaPotionTime;
            lp[1, 3] = 1;
            lp[3, 3] = 1;
            lp[39, 3] = tps[3] = (1 + characterStats.BonusManaPotion) * 2400f / calculatedStats.ManaPotionTime * 0.5f * threatFactor;
            lp[40, 3] = 40 / calculatedStats.ManaPotionTime;
            lp[lpRows, 3] = 0;
            // mana gem
            //calculatedStats.SolutionLabel[4] = "Mana Gem";
            calculatedStats.MaxManaGem = Math.Min(5, 1 + (int)((calculatedStats.FightDuration - 30f) / 120f));
            double manaGemRegenRate = (1 + characterStats.BonusManaGem) * (-Math.Min(3, 1 + (int)((calculatedStats.FightDuration - 30f) / 120f)) * 2400f - ((calculatedStats.FightDuration >= 390) ? 1100f : 0f) - ((calculatedStats.FightDuration >= 510) ? 850 : 0)) / (calculatedStats.MaxManaGem * calculatedStats.ManaPotionTime);
            lp[0, 4] = -calculatedStats.ManaRegen5SR + manaGemRegenRate;
            lp[1, 4] = 1;
            lp[4, 4] = 1;
            lp[14, 4] = 1;
            lp[23, 4] = - 1 / calculatedStats.ManaPotionTime;
            lp[39, 4] = tps[4] = manaGemRegenRate * 0.5f * threatFactor;
            lp[40, 4] = 40 / calculatedStats.ManaPotionTime;
            lp[lpRows, 4] = 0;
            // drums
            //calculatedStats.SolutionLabel[5] = "Drums of Battle";
            lp[0, 5] = -calculatedStats.ManaRegen5SR;
            lp[1, 5] = 1;
            lp[34, 5] = - 1 / calculatedStats.GlobalCooldown;
            lp[41, 5] = 1 / calculatedStats.GlobalCooldown;
            lp[lpRows, 5] = 0;
            // spells
            for (int seg = 0; seg < segments; seg++)
            {
                for (int buffset = 0; buffset < statsList.Count; buffset++)
                {
                    for (int spell = 0; spell < spellList.Count; spell++)
                    {
                        int index = (seg * statsList.Count + buffset) * spellList.Count + spell + colOffset - 1;
                        if (lp.IsColumnEnabled(index))
                        {
                            Spell s = statsList[buffset].GetSpell(spellList[spell]);
                            if ((s.AffectedByFlameCap || !statsList[buffset].FlameCap) && (!s.ABCycle || calculationOptions.ABCycles))
                            {
                                calculatedStats.SolutionStats[index] = statsList[buffset];
                                calculatedStats.SolutionSpells[index] = s;
                                if (useSMP) calculatedStats.SolutionSegments[index] = seg;
                                //calculatedStats.SolutionLabel[index] = ((statsList[buffset].BuffLabel.Length > 0) ? (statsList[buffset].BuffLabel + "+") : "") + s.Name;
                                if (computeIncrementalSet)
                                {
                                    incrementalSetCooldown[index] = statsList[buffset].IncrementalSetIndex;
                                    incrementalSetSpell[index] = spellList[spell];
                                    if (useSMP) incrementalSetSegment[index] = seg;
                                }
                                lp[0, index] = s.CostPerSecond - s.ManaRegenPerSecond;
                                lp[1, index] = 1;
                                if (statsList[buffset].DestructionPotion) lp[3, index] = calculatedStats.ManaPotionTime / 15f;
                                lp[5, index] = (statsList[buffset].Heroism ? 1 : 0);
                                lp[6, index] = (statsList[buffset].ArcanePower ? 1 : 0);
                                lp[7, index] = ((statsList[buffset].Heroism && statsList[buffset].ArcanePower) ? 1 : 0);
                                lp[8, index] = (statsList[buffset].IcyVeins ? 1 : 0);
                                lp[9, index] = (statsList[buffset].MoltenFury ? 1 : 0);
                                lp[10, index] = ((statsList[buffset].MoltenFury && statsList[buffset].DestructionPotion) ? 1 : 0);
                                lp[11, index] = ((statsList[buffset].MoltenFury && statsList[buffset].IcyVeins) ? 1 : 0);
                                lp[12, index] = ((statsList[buffset].DestructionPotion && statsList[buffset].Heroism) ? 1 : 0);
                                lp[13, index] = ((statsList[buffset].DestructionPotion && statsList[buffset].IcyVeins) ? 1 : 0);
                                lp[14, index] = (statsList[buffset].FlameCap ? (calculatedStats.ManaPotionTime / 40f) : 0); ;
                                lp[15, index] = ((statsList[buffset].MoltenFury && statsList[buffset].FlameCap) ? 1 : 0); ;
                                lp[16, index] = ((statsList[buffset].DestructionPotion && statsList[buffset].FlameCap) ? 1 : 0);
                                lp[17, index] = (statsList[buffset].Trinket1 ? 1 : 0);
                                lp[18, index] = (statsList[buffset].Trinket2 ? 1 : 0);
                                lp[19, index] = ((statsList[buffset].MoltenFury && statsList[buffset].Trinket1) ? 1 : 0);
                                lp[20, index] = ((statsList[buffset].MoltenFury && statsList[buffset].Trinket2) ? 1 : 0);
                                lp[21, index] = ((statsList[buffset].Heroism && statsList[buffset].Trinket1) ? 1 : 0);
                                lp[22, index] = ((statsList[buffset].Heroism && statsList[buffset].Trinket2) ? 1 : 0);
                                lp[23, index] = ((statsList[buffset].Trinket1 && t1ismg) ? 1 / trinket1duration : 0) + ((statsList[buffset].Trinket2 && t2ismg) ? 1 / trinket2duration : 0);
                                //aoe duration, flamestrike, cone of cold, blast wave, dragon's breath
                                lp[25, index] = (s.AreaEffect ? 1 : 0);
                                if (s.AreaEffect)
                                {
                                    Flamestrike fs = s as Flamestrike;
                                    if (fs != null)
                                    {
                                        if (!fs.SpammedDot) lp[26, index] = fs.DotDuration / fs.CastTime;
                                    }
                                    else
                                    {
                                        lp[26, index] = -1;
                                    }
                                    ConeOfCold coc = s as ConeOfCold;
                                    if (coc != null)
                                    {
                                        lp[27, index] = (coc.Cooldown / coc.CastTime - 1);
                                    }
                                    else
                                    {
                                        lp[27, index] = -1;
                                    }
                                    BlastWave bw = s as BlastWave;
                                    if (bw != null)
                                    {
                                        lp[28, index] = (bw.Cooldown / bw.CastTime - 1);
                                    }
                                    else
                                    {
                                        lp[28, index] = -1;
                                    }
                                    DragonsBreath db = s as DragonsBreath;
                                    if (db != null)
                                    {
                                        lp[29, index] = (db.Cooldown / db.CastTime - 1);
                                    }
                                    else
                                    {
                                        lp[29, index] = -1;
                                    }
                                }
                                lp[30, index] = (statsList[buffset].Combustion) ? (1 / (statsList[buffset].CombustionDuration * s.CastTime / s.CastProcs)) : 0;
                                lp[31, index] = (statsList[buffset].Combustion && statsList[buffset].MoltenFury) ? (1 / (statsList[buffset].CombustionDuration * s.CastTime / s.CastProcs)) : 0;
                                lp[32, index] = (statsList[buffset].Combustion && statsList[buffset].Heroism) ? (1 / (statsList[buffset].CombustionDuration * s.CastTime / s.CastProcs)) : 0;
                                lp[33, index] = (statsList[buffset].IcyVeins && statsList[buffset].Heroism) ? 1 : 0;
                                //drums, drums+mf, drums+heroism, drums+iv, drums+ap
                                lp[34, index] = (statsList[buffset].DrumsOfBattle) ? 1 / (30 - calculatedStats.GlobalCooldown) : 0;
                                lp[35, index] = (statsList[buffset].DrumsOfBattle && statsList[buffset].MoltenFury) ? 1 : 0;
                                lp[36, index] = (statsList[buffset].DrumsOfBattle && statsList[buffset].Heroism) ? 1 : 0;
                                lp[37, index] = (statsList[buffset].DrumsOfBattle && statsList[buffset].IcyVeins) ? 1 : 0;
                                lp[38, index] = (statsList[buffset].DrumsOfBattle && statsList[buffset].ArcanePower) ? 1 : 0;
                                lp[39, index] = tps[index] = s.ThreatPerSecond;
                                //lp[40, index] = (statsList[buffset].FlameCap ? 1 : 0) + (statsList[buffset].DestructionPotion ? 40.0 / 15.0 : 0);
                                lp[lpRows, index] = s.DamagePerSecond;
                                if (useSMP)
                                {
                                    // mf, heroism, ap, iv, combustion, drums, flamecap, destro, t1, t2
                                    if (statsList[buffset].MoltenFury)
                                    {
                                        lp[rowOffset + 0 * segments + seg, index] = 1;
                                    }
                                    //lp[rowOffset + 1 * segments + seg, index] = 1;
                                    if (statsList[buffset].ArcanePower)
                                    {
                                        for (int ss = 0; ss < segments; ss++)
                                        {
                                            double cool = 180;
                                            int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                            if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                            if (seg >= ss && seg <= maxs) lp[rowOffset + 2 * segments + ss, index] = 1;
                                        }
                                    }
                                    if (statsList[buffset].IcyVeins)
                                    {
                                        for (int ss = 0; ss < segments; ss++)
                                        {
                                            double cool = 180 + (coldsnap ? 20 : 0);
                                            int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                            if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                            if (seg >= ss && seg <= maxs) lp[rowOffset + 3 * segments + ss, index] = 1;
                                        }
                                    }
                                    if (statsList[buffset].Combustion)
                                    {
                                        for (int ss = 0; ss < segments; ss++)
                                        {
                                            double cool = 180 + 15;
                                            int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                            if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                            if (seg >= ss && seg <= maxs) lp[rowOffset + 4 * segments + ss, index] = 1 / (statsList[buffset].CombustionDuration * s.CastTime / s.CastProcs);
                                        }
                                    }
                                    if (statsList[buffset].DrumsOfBattle)
                                    {
                                        for (int ss = 0; ss < segments; ss++)
                                        {
                                            double cool = 120;
                                            int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                            if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                            if (seg >= ss && seg <= maxs) lp[rowOffset + 5 * segments + ss, index] = 1;
                                        }
                                    }
                                    if (statsList[buffset].FlameCap)
                                    {
                                        for (int ss = 0; ss < segments; ss++)
                                        {
                                            double cool = 180;
                                            int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                            if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                            if (seg >= ss && seg <= maxs) lp[rowOffset + 6 * segments + ss, index] = 1;
                                        }
                                    }
                                    if (statsList[buffset].DestructionPotion)
                                    {
                                        for (int ss = 0; ss < segments; ss++)
                                        {
                                            double cool = 120;
                                            int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                            if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                            if (seg >= ss && seg <= maxs) lp[rowOffset + 7 * segments + ss, index] = 1;
                                        }
                                    }
                                    if (statsList[buffset].Trinket1)
                                    {
                                        for (int ss = 0; ss < segments; ss++)
                                        {
                                            double cool = trinket1cooldown;
                                            int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                            if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                            if (seg >= ss && seg <= maxs) lp[rowOffset + 8 * segments + ss, index] = 1;
                                        }
                                    }
                                    if (statsList[buffset].Trinket2)
                                    {
                                        for (int ss = 0; ss < segments; ss++)
                                        {
                                            double cool = trinket2cooldown;
                                            int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                            if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                            if (seg >= ss && seg <= maxs) lp[rowOffset + 9 * segments + ss, index] = 1;
                                        }
                                    }
                                    if (statsList[buffset] != calculatedStats) lp[rowOffset + 10 * segments + seg, index] = 1;
                                }
                            }
                        }
                    }
                }
            }
            // mana burn estimate
            float manaBurn = 80;
            if (calculationOptions.AoeDuration > 0)
            {
                Spell s = calculatedStats.GetSpell(SpellId.ArcaneExplosion);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (calculationOptions.EmpoweredFireball > 0)
            {
                Spell s = calculatedStats.GetSpell(SpellId.Fireball);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (calculationOptions.EmpoweredFrostbolt > 0)
            {
                Spell s = calculatedStats.GetSpell(SpellId.Frostbolt);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (calculationOptions.SpellPower > 0)
            {
                Spell s = calculatedStats.GetSpell(SpellId.ArcaneBlast33);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            if (ivAvailable)
            {
                manaBurn *= 1.1f;
            }
            if (apAvailable)
            {
                manaBurn *= 1.1f;
            }

            if (calculatedStats.FightDuration - 7800 / manaBurn < 0) // fix for maximum pot+gem constraint
            {
                manaBurn = 7800 / calculatedStats.FightDuration;
            }

            lp[0, lpCols] = characterStats.Mana;
            lp[1, lpCols] = calculatedStats.FightDuration;
            lp[2, lpCols] = evocationDuration * Math.Max(1, (1 + Math.Floor((calculatedStats.FightDuration - 200f) / 480f)));
            lp[3, lpCols] = calculatedStats.MaxManaPotion * calculatedStats.ManaPotionTime;
            lp[4, lpCols] = calculatedStats.MaxManaGem * calculatedStats.ManaPotionTime;
            if (heroismAvailable) lp[5, lpCols] = 40;
            if (apAvailable) lp[6, lpCols] = aplength;
            if (heroismAvailable && apAvailable) lp[7, lpCols] = 15;
            if (ivAvailable) lp[8, lpCols] = ivlength;
            if (mfAvailable) lp[9, lpCols] = mflength;
            if (mfAvailable) lp[10, lpCols] = 15;
            if (mfAvailable && ivAvailable) lp[11, lpCols] = coldsnap ? 40 : 20;
            if (heroismAvailable) lp[12, lpCols] = 15;
            if (ivAvailable) lp[13, lpCols] = dpivlength;
            if (calculationOptions.FlameCap && !(!calculationOptions.SmartOptimization && calculationOptions.SpellPower > 0))
            {
                lp[14, lpCols] = ((int)(calculatedStats.FightDuration / 180f + 2f / 3f)) * calculatedStats.ManaPotionTime * 3f / 2f;
            }
            else
            {
                lp[14, lpCols] = calculatedStats.MaxManaGem * calculatedStats.ManaPotionTime;
            }
            if (mfAvailable) lp[15, lpCols] = 60;
            lp[16, lpCols] = dpflamelength;
            if (trinket1Available) lp[17, lpCols] = t1length;
            if (trinket2Available) lp[18, lpCols] = t2length;
            if (mfAvailable && trinket1Available) lp[19, lpCols] = trinket1duration;
            if (mfAvailable && trinket2Available) lp[20, lpCols] = trinket2duration;
            if (heroismAvailable && trinket1Available) lp[21, lpCols] = trinket1duration;
            if (heroismAvailable && trinket2Available) lp[22, lpCols] = trinket2duration;
            lp[24, lpCols] = - (1 - calculationOptions.DpsTime) * calculationOptions.FightDuration;
            lp[25, lpCols] = calculationOptions.AoeDuration * calculationOptions.FightDuration;
            lp[30, lpCols] = combustionCount;
            lp[31, lpCols] = 1;
            lp[32, lpCols] = 1;
            lp[33, lpCols] = coldsnap ? 40 : 20;
            lp[35, lpCols] = 30 - calculatedStats.GlobalCooldown;
            lp[36, lpCols] = 30 - calculatedStats.GlobalCooldown;
            lp[37, lpCols] = drumsivlength;
            lp[38, lpCols] = drumsaplength;
            lp[39, lpCols] = calculationOptions.TpsLimit * calculationOptions.FightDuration;
            int manaConsum = ((int)((calculatedStats.FightDuration - 7800 / manaBurn) / 60f + 2));
            if ((t1ismg || t2ismg) && manaConsum < calculatedStats.MaxManaGem) manaConsum = calculatedStats.MaxManaGem;
            lp[40, lpCols] = manaConsum * 40;
            lp[41, lpCols] = (1 + (int)((calculatedStats.FightDuration - 30) / 120));

            if (useSMP)
            {
                // mf
                if (mfAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        if (calculationOptions.FightDuration - mflength < seg * segmentDuration) lp[rowOffset + 0 * segments + seg, lpCols] = segmentDuration;
                        else lp[rowOffset + 0 * segments + seg, lpCols] = Math.Max(0, segmentDuration - (calculationOptions.FightDuration - mflength - seg * segmentDuration));
                    }
                }
                // heroism, nothing needed for now
                // ap
                for (int seg = 0; seg < segments; seg++)
                {
                    lp[rowOffset + 2 * segments + seg, lpCols] = 15;
                }
                // iv
                for (int seg = 0; seg < segments; seg++)
                {
                    lp[rowOffset + 3 * segments + seg, lpCols] = 20 + (coldsnap ? 20 : 0);
                }
                // combustion
                for (int seg = 0; seg < segments; seg++)
                {
                    lp[rowOffset + 4 * segments + seg, lpCols] = 1;
                }
                // drums
                for (int seg = 0; seg < segments; seg++)
                {
                    lp[rowOffset + 5 * segments + seg, lpCols] = 30 - calculatedStats.GlobalCooldown;
                }
                // flamecap
                for (int seg = 0; seg < segments; seg++)
                {
                    lp[rowOffset + 6 * segments + seg, lpCols] = 60;
                }
                // destruction
                for (int seg = 0; seg < segments; seg++)
                {
                    lp[rowOffset + 7 * segments + seg, lpCols] = 15;
                }
                // t1
                for (int seg = 0; seg < segments; seg++)
                {
                    lp[rowOffset + 8 * segments + seg, lpCols] = trinket1duration;
                }
                // t2
                for (int seg = 0; seg < segments; seg++)
                {
                    lp[rowOffset + 9 * segments + seg, lpCols] = trinket2duration;
                }
                // timing
                for (int seg = 0; seg < segments; seg++)
                {
                    lp[rowOffset + 10 * segments + seg, lpCols] = segmentDuration;
                }
            }
            #endregion

            #region SMP Branch & Bound
            int maxHeap = Properties.Settings.Default.MaxHeapLimit;
            if (useSMP)
            {
                lp.SolvePrimalDual(); // solve primal and recalculate to get a stable starting point
                heap = new Heap<CompactLP>(HeapType.MaximumHeap);
                heap.Push(lp);

                double max = lp.Value;

                bool valid = true;
                do
                {
                    if (heap.Head.Value > max + 0.001) // lowered instability threshold, in case it is still an issue just recompute the solution which "should" give a stable result hopefully
                    {
                        // recovery measures first
                        double current = heap.Head.Value;
                        lp = heap.Pop();
                        lp.ForceRecalculation();
                        // some testing indicates that the recalculated solution gives the correct result, so the previous solution is most likely to be the problematic one, since we just discarded it not a big deal
                        //if (lp.Value <= max + 1.0)
                        //{
                            // give more fudge room in case the previous max was the one that was unstable
                            max = lp.Value;
                            heap.Push(lp);
                            continue;
                        //}
                        //System.Windows.Forms.MessageBox.Show("Instability detected, aborting SMP algorithm (max = " + max + ", value = " + lp.Value + ")");
                        // find something reasonably stable
                        //while (heap.Count > 0 && (lp = heap.Pop()).Value > max + 0.000001) { }
                        //break;
                    }
                    lp = heap.Pop();
                    max = lp.Value;
                    // this is the best non-evaluated option (highest partially-constrained LP, the optimum has to be lower)
                    // if this one is valid than all others are sub-optimal
                    // validate all segments for each cooldown
                    solution = lp.Solve();
                    /*System.Diagnostics.Trace.WriteLine("Solution basis (value = " + lp.Value + "):");
                    for (int index = 0; index < lpCols; index++)
                    {
                        if (solution[index] > 0.000001) System.Diagnostics.Trace.WriteLine(index);
                    }*/
                    if (heap.Count > maxHeap)
                    {
                        System.Windows.Forms.MessageBox.Show("SMP algorithm exceeded maximum allowed computation limit. Displaying the last working solution.");
                        break;
                    }
                    valid = true;
                    // make sure all cooldowns are tightly packed and not fragmented
                    // mf is trivially satisfied
                    // heroism
                    if (heroismAvailable)
                    {
                        valid = ValidateCooldown(Cooldown.Heroism, 40, -1);
                    }
                    // ap
                    if (valid && apAvailable)
                    {
                        valid = ValidateCooldown(Cooldown.ArcanePower, 15, 180);
                    }
                    // iv
                    if (valid && ivAvailable)
                    {
                        valid = ValidateCooldown(Cooldown.IcyVeins, 20 + (coldsnap ? 20 : 0), 180 + (coldsnap ? 20 : 0));
                    }
                    // combustion
                    if (valid && combustionAvailable)
                    {
                        valid = ValidateCooldown(Cooldown.Combustion, 15, 180 + 15); // the durations are only used to compute segment distances, for 30 sec segments this should work pretty well
                    }
                    // drums
                    if (valid && calculationOptions.DrumsOfBattle)
                    {
                        valid = ValidateCooldown(Cooldown.DrumsOfBattle, 30, 120);
                    }
                    // flamecap
                    if (valid && calculationOptions.FlameCap)
                    {
                        valid = ValidateCooldown(Cooldown.FlameCap, 60, 180);
                    }
                    // destruction
                    if (valid && calculationOptions.DestructionPotion)
                    {
                        valid = ValidateCooldown(Cooldown.DestructionPotion, 15, 120);
                    }
                    // t1
                    if (valid && trinket1Available)
                    {
                        valid = ValidateCooldown(Cooldown.Trinket1, trinket1duration, trinket1cooldown);
                    }
                    // t2
                    if (valid && trinket2Available)
                    {
                        valid = ValidateCooldown(Cooldown.Trinket2, trinket2duration, trinket2cooldown);
                    }
                    /*if (valid && t1ismg && calculationOptions.FlameCap)
                    {
                        valid = ValidateSCB(Cooldown.Trinket1);
                    }
                    if (valid && t2ismg && calculationOptions.FlameCap)
                    {
                        valid = ValidateSCB(Cooldown.Trinket2);
                    }*/
                    // eliminate packing cycles
                    // example:
                    // H+IV:10
                    // IV+Icon:10
                    // H+Icon:10
                    if (valid)
                    {
                        for (int seg = 0; seg < segments - 1; seg++)
                        {
                            // collect all cooldowns on the boundary seg...(seg+1)
                            // assume one instance of cooldown max (coldsnap theoretically doesn't satisfy this, but I think it should still work)
                            // calculate hex values for boolean arithmetic
                            // verify if there are cycles

                            // ###   ###
                            // ######
                            //    ######

                            // ##
                            //  ##
                            //   ##
                            //    ##
                            //     ##
                            // #    #

                            // cycle = no element can be placed at the start, all have two tails that intersect to 0
                            // inside the boundary we can have more than one cycle and several nice packings
                            // find elements that can be placed at the start, those are the ones with nice packing
                            // for each one you find remove the corresponding group
                            // if we remove everything then there are no cycles

                            List<int> hexList = new List<int>();
                            for (int index = seg * statsList.Count * spellList.Count + colOffset - 1; index < (seg + 2) * statsList.Count * spellList.Count + colOffset - 1; index++)
                            {
                                CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                if (stats != null && solution[index] > 0)
                                {
                                    int hex = stats.GetHex();
                                    if (hex != 0 && !hexList.Contains(hex)) hexList.Add(hex);
                                }
                            }

                            // placed  ## ### ## #
                            //         .. ...  
                            //          .   . .. .
                            // active   #   #    #
                            //
                            // future   #   ##      <= ok
                            //          #   #  #    <= not ok

                            // take newHex = (future - future & active)
                            // if newHex & placed > 0 then we have cycle

                            bool ok = true;
                            int placed = 0;
                            while (ok && hexList.Count > 0)
                            {
                                ok = false;
                                // check if any can be at the start
                                for (int i = 0; i < hexList.Count; i++)
                                {
                                    int tail = hexList[i];
                                    for (int j = 0; j < hexList.Count; j++)
                                    {
                                        int intersect = hexList[i] & hexList[j];
                                        if (i != j && intersect > 0)
                                        {
                                            tail &= hexList[j];
                                            if (tail == 0) break;
                                        }
                                        int newHex = hexList[j] - intersect;
                                        if ((newHex & placed) > 0)
                                        {
                                            tail = 0;
                                            break;
                                        }
                                    }
                                    if (tail != 0)
                                    {
                                        // i is good
                                        ok = true;
                                        placed |= hexList[i];
                                        hexList.RemoveAt(i);
                                        break;
                                    }
                                }
                            }
                            if (hexList.Count > 0)
                            {
                                // we have a cycle
                                // to break the cycle we have to remove one of the elements
                                // if all are present then obviously we have a cycle, so the true solution must have one of them missing
                                for (int i = 0; i < hexList.Count; i++)
                                {
                                    CompactLP hexRemovedLP = lp.Clone();
                                    //hexRemovedLP.Log += "Breaking cycle at boundary " + seg + ", removing " + hexList[i] + "\r\n";
                                    for (int index = seg * statsList.Count * spellList.Count + colOffset - 1; index < (seg + 2) * statsList.Count * spellList.Count + colOffset - 1; index++)
                                    {
                                        CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                        if (stats != null && stats.GetHex() == hexList[i]) hexRemovedLP.EraseColumn(index);
                                    }
                                    heap.Push(hexRemovedLP);
                                }
                                valid = false;
                                break;
                            }
                        }
                    }
                } while (heap.Count > 0 && !valid);
                //System.Diagnostics.Trace.WriteLine("Heap at solution " + heap.Count);
            }
            #endregion

            calculatedStats.Solution = lp.Solve();
            if (computeIncrementalSet)
            {
                calculatedStats.IncrementalSetCooldown = incrementalSetCooldown;
                calculatedStats.IncrementalSetSpell = incrementalSetSpell;
                if (useSMP) calculatedStats.IncrementalSetSegment = incrementalSetSegment;
            }

            calculatedStats.SubPoints[0] = ((float)calculatedStats.Solution[lpCols] + calculatedStats.WaterElementalDamage) / calculationOptions.FightDuration;
            calculatedStats.SubPoints[1] = calculatedStats.BasicStats.Health * calculationOptions.SurvivabilityRating;
            calculatedStats.OverallPoints = calculatedStats.SubPoints[0] + calculatedStats.SubPoints[1];
            float threat = 0;
            for (int i = 0; i < lpCols; i++)
            {
                threat += (float)(tps[i] * calculatedStats.Solution[i]);
            }
            calculatedStats.Tps = threat / calculationOptions.FightDuration;

            calculationOptions.SmartOptimization = savedSmartOptimization;
            calculationOptions.ABCycles = savedABCycles;
            calculationOptions.DestructionPotion = savedDestructionPotion;
            calculationOptions.FlameCap = savedFlameCap;

            return calculatedStats;
        }

        private const int CooldownMax = 10;

        private bool ValidateCooldown(Cooldown cooldown, double effectDuration, double cooldownDuration)
        {
            //const double eps = 0.000001;
            double[] segCount = new double[segments];
            for (int outseg = 0; outseg < segments; outseg++)
            {
                double s = 0.0;
                for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                {
                    CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                    if (stats != null && stats.GetCooldown(cooldown))
                    {
                        s += solution[index];
                    }
                }
                segCount[outseg] = s;
            }
            int mindist = (int)Math.Ceiling(effectDuration / segmentDuration);
            int mindist2 = (int)Math.Floor(effectDuration / segmentDuration);
            int maxdist = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor((cooldownDuration - effectDuration) / segmentDuration));
            int maxdist2 = (cooldownDuration < 0) ? 3 * segments : ((int)Math.Floor(cooldownDuration / segmentDuration));

            bool valid = true;

            for (int seg = 0; seg < segments; seg++)
            {
                double inseg = segCount[seg];
                if (inseg > 0)
                {
                    // verify that outside duration segments are 0
                    for (int outseg = 0; outseg < segments; outseg++)
                    {
                        if (Math.Abs(outseg - seg) > mindist && Math.Abs(outseg - seg) < maxdist)
                        {
                            if (segCount[outseg] > 0)
                            {
                                valid = false;
                                break;
                            }
                        }
                    }
                    if (!valid)
                    {
                        //if (lp.disabledHex == null) lp.disabledHex = new int[CooldownMax];
                        // branch on whether cooldown is used in this segment
                        CompactLP cooldownUsed = lp.Clone();
                        // cooldown not used
                        //lp.IVHash += 1 << seg;
                        //lp.Log += "Disable " + cooldown.ToString() + " at " + seg + "\r\n";
                        for (int index = seg * statsList.Count * spellList.Count + colOffset - 1; index < (seg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                        {
                            CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                            if (stats != null && stats.GetCooldown(cooldown)) lp.EraseColumn(index);
                        }
                        heap.Push(lp);
                        // cooldown used
                        //cooldownUsed.Log += "Use " + cooldown.ToString() + " at " + seg + ", disable around\r\n";
                        for (int outseg = 0; outseg < segments; outseg++)
                        {
                            if (Math.Abs(outseg - seg) > mindist && Math.Abs(outseg - seg) < maxdist)
                            {
                                //cooldownUsed.IVHash += 1 << outseg;
                                for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                                {
                                    CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                    if (stats != null && stats.GetCooldown(cooldown)) cooldownUsed.EraseColumn(index);
                                }
                            }
                        }
                        heap.Push(cooldownUsed);
                        return false;
                    }
                }
            }

            /*double t1 = 0.0;
            double t2 = 0.0;
            double bestCoverage = 0.0;

            if (cooldownDuration < 0) cooldownDuration = 3 * segments * segmentDuration;

            for (int seg = 0; seg < segments; seg++)
            {
                double inseg = segCount[seg];
                if (inseg > 0 && (seg == 0 || segCount[seg - 1] == 0.0))
                {
                    double t = seg;
                    if (seg < segments - 1 && segCount[seg + 1] > 0.0) t = seg + 1 - inseg / segmentDuration;
                    double max = t + cooldownDuration / segmentDuration;
                    // verify that outside duration segments are 0
                    for (int outseg = seg + 1; outseg < segments; outseg++)
                    {
                        if (segCount[outseg] > 0)
                        {
                            double tt = outseg + 1 - segCount[outseg] / segmentDuration;
                            if ((outseg >= t + effectDuration / segmentDuration + eps) && (tt < max - eps))                            
                            {
                                valid = false;
                                // make sure that we pairwise invalidate
                                // (outseg >= tin + effectDuration / segmentDuration && outseg < (int)(tin + cooldownDuration / segmentDuration)
                                // outseg + 1 <= tin + cooldownDuration / segmentDuration
                                // outseg - effectDuration / segmentDuration >= tin >= outseg + 1 - cooldownDuration / segmentDuration
                                // cooldownDuration >= effectDuration + 2 * segmentDuration !!! if this isn't true then we have problems, means segmentDuration has to be small enough, 30 sec = good
                                // (seg >= tout + (effectDuration - cooldownDuration) / segmentDuration && seg < (int)tout)
                                // seg + 1 <= tout <= seg + (cooldownDuration - effectDuration) / segmentDuration
                                double tin = t;
                                double tout = tt;
                                if (tin < outseg + 1 - cooldownDuration / segmentDuration) tin = outseg + 1 - cooldownDuration / segmentDuration;
                                if (tout > seg + (cooldownDuration - effectDuration) / segmentDuration) tout = seg + (cooldownDuration - effectDuration) / segmentDuration;
                                double c1 = 0.0;
                                double c2 = 0.0;
                                for (int s = 0; s < segments; s++)
                                {
                                    if ((s >= tin + (effectDuration - cooldownDuration) / segmentDuration - eps && s + 1 < tin - eps) || (s >= tin + effectDuration / segmentDuration + eps && s + 1 < tin + cooldownDuration / segmentDuration + eps))
                                    {
                                        c1 += segCount[s];
                                    }
                                    if ((s >= tout + (effectDuration - cooldownDuration) / segmentDuration - eps && s + 1 < tout - eps) || (s >= tout + effectDuration / segmentDuration + eps && s + 1 < tout + cooldownDuration / segmentDuration + eps))
                                    {
                                        c2 += segCount[s];
                                    }
                                }
                                double coverage = Math.Min(c1, c2);
                                if (coverage < eps)
                                {
                                    coverage = 0.0; // troubles
                                }
                                if (coverage > bestCoverage)
                                {
                                    t1 = tin;
                                    t2 = tout;
                                    bestCoverage = coverage;
                                }
                            }
                        }
                    }
                }
            }
            if (!valid)
            {
                //if (lp.disabledHex == null) lp.disabledHex = new int[CooldownMax];
                // branch on whether t1 or t2 is active, they can't be both
                CompactLP t1active = lp.Clone();
                // cooldown used
                //t1active.Log += "Use " + cooldown.ToString() + " at " + t1 + ", disable around\r\n";
                for (int outseg = 0; outseg < segments; outseg++)
                {
                    if ((outseg >= t1 + (effectDuration - cooldownDuration) / segmentDuration - eps && outseg + 1 < t1 - eps) || (outseg >= t1 + effectDuration / segmentDuration + eps && outseg + 1 < t1 + cooldownDuration / segmentDuration + eps))
                    {
                        for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                        {
                            CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                            if (stats != null && stats.GetCooldown(cooldown)) t1active.EraseColumn(index);
                        }
                    }
                }
                heap.Push(t1active);
                // cooldown not used
                //lp.Log += "Use " + cooldown.ToString() + " at " + t2 + ", disable around\r\n";
                for (int outseg = 0; outseg < segments; outseg++)
                {
                    if ((outseg >= t2 + (effectDuration - cooldownDuration) / segmentDuration - eps && outseg + 1 < t2 - eps) || (outseg >= t2 + effectDuration / segmentDuration + eps && outseg + 1 < t2 + cooldownDuration / segmentDuration + eps))
                    {
                        for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                        {
                            CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                            if (stats != null && stats.GetCooldown(cooldown)) lp.EraseColumn(index);
                        }
                    }
                }
                heap.Push(lp);

                return false;
            }*/
            for (int seg = 0; seg < segments; seg++)
            {
                double inseg = segCount[seg];
                double leftseg = 0.0;
                double rightseg = 0.0;
                for (int outseg = 0; outseg < segments; outseg++)
                {
                    if (Math.Abs(outseg - seg) <= mindist && outseg != seg)
                    {
                        if (outseg < seg)
                        {
                            leftseg += segCount[outseg];
                        }
                        else if (outseg > seg)
                        {
                            rightseg += segCount[outseg];
                        }
                    }
                }
                if (valid && inseg < segmentDuration - 0.000001 && leftseg > 0 && rightseg > 0 /*&& cooldown != Cooldown.IcyVeins*/) // coldsnapped icy veins doesn't have to be contiguous, but getting better results assuming it is
                {
                    // fragmentation
                    // either left must be disabled, right disabled, or seg to max
                    CompactLP leftDisabled = lp.Clone();
                    //leftDisabled.Log += "Disable " + cooldown.ToString() + " left of " + seg + "\r\n";
                    for (int outseg = 0; outseg < segments; outseg++)
                    {
                        if ((outseg < seg || Math.Abs(outseg - seg) > mindist) && Math.Abs(outseg - seg) < maxdist)
                        {
                            for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                            {
                                CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                if (stats != null && stats.GetCooldown(cooldown)) leftDisabled.EraseColumn(index);
                            }
                        }
                    }
                    heap.Push(leftDisabled);
                    CompactLP rightDisabled = lp.Clone();
                    //rightDisabled.Log += "Disable " + cooldown.ToString() + " right of " + seg + "\r\n";
                    for (int outseg = 0; outseg < segments; outseg++)
                    {
                        if ((outseg > seg || Math.Abs(outseg - seg) > mindist) && Math.Abs(outseg - seg) < maxdist)
                        {
                            for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                            {
                                CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                if (stats != null && stats.GetCooldown(cooldown)) rightDisabled.EraseColumn(index);
                            }
                        }
                    }
                    heap.Push(rightDisabled);
                    //lp.Log += "Force " + cooldown.ToString() + " to max at " + seg + "\r\n";
                    lp.AddConstraint();
                    for (int index = seg * statsList.Count * spellList.Count + colOffset - 1; index < (seg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                    {
                        CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                        if (stats != null)
                        {
                            if (stats.GetCooldown(cooldown)) lp.UpdateConstraintElement(index, -1);
                            // for some strange reason trying to help doesn't really help
                            // so don't try to help
                            //else lp.EraseColumn(index); // to make it easier on the solver also let it know that anything that doesn't have this cooldown can't be in solution
                        }
                    }
                    lp.UpdateConstraintRHS(-segmentDuration);
                    heap.Push(lp);
                    return false;
                }
            }
            return valid;
        }

        private bool ValidateSCB(Cooldown trinket)
        {
            double[] trinketCount = new double[segments];
            double[] flamecapCount = new double[segments];
            for (int outseg = 0; outseg < segments; outseg++)
            {
                double s = 0.0;
                for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                {
                    CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                    if (stats != null && stats.GetCooldown(trinket))
                    {
                        s += solution[index];
                    }
                }
                trinketCount[outseg] = s;
            }
            for (int outseg = 0; outseg < segments; outseg++)
            {
                double s = 0.0;
                for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                {
                    CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                    if (stats != null && stats.FlameCap)
                    {
                        s += solution[index];
                    }
                }
                flamecapCount[outseg] = s;
            }
            int rightdist = ((int)Math.Floor((120.0 - 15.0) / segmentDuration));
            int leftdist = ((int)Math.Floor((180.0 - 60.0) / segmentDuration));

            bool valid = true;
            int flamecapSeg = 0;
            int trinketSeg = 0;
            int minDist = int.MaxValue;

            for (int seg = 0; seg < segments; seg++) // trinket
            {
                double inseg = trinketCount[seg];
                if (inseg > 0)
                {
                    for (int outseg = 0; outseg < segments; outseg++) // flamecap
                    {
                        if ((outseg > seg - leftdist) || (outseg < seg + rightdist))
                        {
                            if (flamecapCount[outseg] > 0)
                            {
                                valid = false;
                                if (Math.Abs(seg - outseg) < minDist)
                                {
                                    trinketSeg = seg;
                                    flamecapSeg = outseg;
                                    minDist = Math.Abs(seg - outseg);
                                }
                            }
                        }
                    }
                }
            }
            if (!valid)
            {
                // branch on whether trinket is used or flame cap is used
                CompactLP trinketUsed = lp.Clone();
                // flame cap used
                //lp.Log += "Disable " + trinket.ToString() + " close to " + flamecapSeg + "\r\n";
                for (int inseg = 0; inseg < segments; inseg++)
                {
                    if ((inseg > flamecapSeg - rightdist) || (inseg < flamecapSeg + leftdist))
                    {
                        for (int index = inseg * statsList.Count * spellList.Count + colOffset - 1; index < (inseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                        {
                            CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                            if (stats != null && stats.GetCooldown(trinket)) lp.EraseColumn(index);
                        }
                    }
                }
                heap.Push(lp);
                // trinket used
                //trinketUsed.Log += "Disable Flame Cap close to " + trinketSeg + "\r\n";
                for (int outseg = 0; outseg < segments; outseg++)
                {
                    if ((outseg > trinketSeg - leftdist) || (outseg < trinketSeg + rightdist))
                    {
                        for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                        {
                            CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                            if (stats != null && stats.FlameCap) trinketUsed.EraseColumn(index);
                        }
                    }
                }
                heap.Push(trinketUsed);
                return false;
            }
            return valid;
        }

        private float Combustion(float critRate)
        {
            float c0 = 1, c1 = 0, c2 = 0, c3 = 0;
            float duration = 0;

            for (int cast = 1; cast <= 13; cast++)
            {
                c3 = critRate * c2;
                c2 = c2 * (1 - critRate) + c1 * critRate;
                c1 = c1 * (1 - critRate) + c0 * critRate;
                c0 = c0 * (1 - critRate);
                critRate = Math.Min(critRate + 0.1f, 1f);
                duration += c3 * cast;
            }
            return duration;
        }

        public CharacterCalculationsMage GetTemporaryCharacterCalculations(Stats characterStats, CalculationOptionsMage calculationOptions, string armor, Character character, Item additionalItem, bool arcanePower, bool moltenFury, bool icyVeins, bool heroism, bool destructionPotion, bool flameCap, bool trinket1, bool trinket2, bool combustion, bool drums, int incrementalSetIndex)
        {
            CharacterCalculationsMage calculatedStats = new CharacterCalculationsMage();
            //Stats stats = characterStats.Clone();
            calculatedStats.IncrementalSetIndex = incrementalSetIndex;
            calculatedStats.BasicStats = characterStats;
            calculatedStats.Character = character;
            calculatedStats.CalculationOptions = calculationOptions;

            float levelScalingFactor = (1 - (70 - 60) / 82f * 3);

            calculatedStats.SpellDamageRating = characterStats.SpellDamageRating;
            calculatedStats.SpellHasteRating = characterStats.SpellHasteRating;

            if (destructionPotion) calculatedStats.SpellDamageRating += 120;

            if (trinket1)
            {
                Stats t = character.Trinket1.Stats;
                calculatedStats.SpellDamageRating += t.SpellDamageFor20SecOnUse2Min + t.SpellDamageFor15SecOnManaGem + t.SpellDamageFor15SecOnUse90Sec + t.SpellDamageFor15SecOnUse2Min;
                calculatedStats.SpellHasteRating += t.SpellHasteFor20SecOnUse2Min + t.SpellHasteFor20SecOnUse5Min;
                calculatedStats.Mp5OnCastFor20Sec = t.Mp5OnCastFor20SecOnUse2Min;
                calculatedStats.Trinket1Name = character.Trinket1.Name;
            }
            if (trinket2)
            {
                Stats t = character.Trinket2.Stats;
                calculatedStats.SpellDamageRating += t.SpellDamageFor20SecOnUse2Min + t.SpellDamageFor15SecOnManaGem + t.SpellDamageFor15SecOnUse90Sec + t.SpellDamageFor15SecOnUse2Min;
                calculatedStats.SpellHasteRating += t.SpellHasteFor20SecOnUse2Min + t.SpellHasteFor20SecOnUse5Min;
                calculatedStats.Mp5OnCastFor20Sec = t.Mp5OnCastFor20SecOnUse2Min;
                calculatedStats.Trinket2Name = character.Trinket2.Name;
            }
            if (drums)
            {
                calculatedStats.SpellHasteRating += 80;
            }

            calculatedStats.CastingSpeed = 1 + calculatedStats.SpellHasteRating / 995f * levelScalingFactor;
            calculatedStats.ArcaneDamage = characterStats.SpellArcaneDamageRating + calculatedStats.SpellDamageRating;
            calculatedStats.FireDamage = characterStats.SpellFireDamageRating + calculatedStats.SpellDamageRating + (flameCap ? 80.0f : 0.0f);
            calculatedStats.FrostDamage = characterStats.SpellFrostDamageRating + calculatedStats.SpellDamageRating;
            calculatedStats.NatureDamage = /* characterStats.SpellNatureDamageRating + */ calculatedStats.SpellDamageRating;
            calculatedStats.ShadowDamage = characterStats.SpellShadowDamageRating + calculatedStats.SpellDamageRating;

            calculatedStats.SpellCrit = 0.01f * (characterStats.Intellect * 0.0125f + 0.9075f) + 0.01f * calculationOptions.ArcaneInstability + 0.01f * calculationOptions.ArcanePotency + characterStats.SpellCritRating / 1400f * levelScalingFactor + characterStats.MageSpellCrit;
            if (destructionPotion) calculatedStats.SpellCrit += 0.02f;
            calculatedStats.SpellHit = characterStats.SpellHitRating * levelScalingFactor / 800f;

            int targetLevel = calculationOptions.TargetLevel;
            calculatedStats.ArcaneHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit + (calculationOptions.WotLK ? 0.01f : 0.02f) * calculationOptions.ArcaneFocus);
            calculatedStats.FireHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit + 0.01f * calculationOptions.ElementalPrecision);
            calculatedStats.FrostHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit + 0.01f * calculationOptions.ElementalPrecision);
            calculatedStats.NatureHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit);
            calculatedStats.ShadowHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit);

            calculatedStats.SpiritRegen = 0.001f + characterStats.Spirit * 0.009327f * (float)Math.Sqrt(characterStats.Intellect);
            calculatedStats.ManaRegen = calculatedStats.SpiritRegen + characterStats.Mp5 / 5f + calculatedStats.SpiritRegen * 4 * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration;
            calculatedStats.ManaRegen5SR = calculatedStats.SpiritRegen * characterStats.SpellCombatManaRegeneration + characterStats.Mp5 / 5f + calculatedStats.SpiritRegen * (5 - characterStats.SpellCombatManaRegeneration) * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration;
            calculatedStats.ManaRegenDrinking = calculatedStats.ManaRegen + 240f;
            calculatedStats.HealthRegen = 0.0312f * characterStats.Spirit + characterStats.Hp5 / 5f;
            calculatedStats.HealthRegenCombat = characterStats.Hp5 / 5f;
            calculatedStats.HealthRegenEating = calculatedStats.ManaRegen + 250f;
            calculatedStats.MeleeMitigation = (1 - 1 / (1 + 0.1f * characterStats.Armor / (8.5f * (70 + 4.5f * (70 - 59)) + 40)));
            calculatedStats.Defense = 350 + characterStats.DefenseRating / 2.37f;
            int molten = (armor == "Molten Armor") ? 1 : 0;
            calculatedStats.PhysicalCritReduction = (0.04f * (calculatedStats.Defense - 5 * 70) / 100 + characterStats.Resilience / 2500f * levelScalingFactor + molten * 0.05f);
            calculatedStats.SpellCritReduction = (characterStats.Resilience / 2500f * levelScalingFactor + molten * 0.05f);
            calculatedStats.CritDamageReduction = (characterStats.Resilience / 2500f * 2f * levelScalingFactor);
            calculatedStats.Dodge = ((0.0443f * characterStats.Agility + 3.28f + 0.04f * (calculatedStats.Defense - 5 * 70)) / 100f + characterStats.DodgeRating / 1200 * levelScalingFactor);

            // spell calculations

            calculatedStats.ArcanePower = arcanePower;
            calculatedStats.MoltenFury = moltenFury;
            calculatedStats.IcyVeins = icyVeins;
            calculatedStats.Heroism = heroism;
            calculatedStats.DestructionPotion = destructionPotion;
            calculatedStats.FlameCap = flameCap;
            calculatedStats.Trinket1 = trinket1;
            calculatedStats.Trinket2 = trinket2;
            calculatedStats.Combustion = combustion;
            calculatedStats.DrumsOfBattle = drums;

            if (icyVeins)
            {
                calculatedStats.CastingSpeed *= 1.2f;
            }
            if (heroism)
            {
                calculatedStats.CastingSpeed *= 1.3f;
            }

            calculatedStats.Latency = calculationOptions.Latency;
            calculatedStats.FightDuration = calculationOptions.FightDuration;
            calculatedStats.ClearcastingChance = 0.02f * calculationOptions.ArcaneConcentration;

            calculatedStats.GlobalCooldownLimit = 1f;
            calculatedStats.GlobalCooldown = Math.Max(calculatedStats.GlobalCooldownLimit, 1.5f / calculatedStats.CastingSpeed);

            calculatedStats.ArcaneSpellModifier = (1 + 0.01f * calculationOptions.ArcaneInstability) * (1 + 0.01f * calculationOptions.PlayingWithFire) * (1 + characterStats.BonusSpellPowerMultiplier);
            if (arcanePower)
            {
                calculatedStats.ArcaneSpellModifier *= 1.3f;
            }
            if (moltenFury)
            {
                calculatedStats.ArcaneSpellModifier *= (1 + 0.1f * calculationOptions.MoltenFury);
            }
            calculatedStats.FireSpellModifier = calculatedStats.ArcaneSpellModifier * (1 + 0.02f * calculationOptions.FirePower);
            calculatedStats.FrostSpellModifier = calculatedStats.ArcaneSpellModifier * (1 + 0.02f * calculationOptions.PiercingIce) * (1 + 0.01f * calculationOptions.ArcticWinds);
            calculatedStats.NatureSpellModifier = calculatedStats.ArcaneSpellModifier;
            calculatedStats.ShadowSpellModifier = calculatedStats.ArcaneSpellModifier;
            calculatedStats.ArcaneSpellModifier *= (1 + characterStats.BonusArcaneSpellPowerMultiplier);
            calculatedStats.FireSpellModifier *= (1 + characterStats.BonusFireSpellPowerMultiplier);
            calculatedStats.FrostSpellModifier *= (1 + characterStats.BonusFrostSpellPowerMultiplier);
            calculatedStats.NatureSpellModifier *= (1 + characterStats.BonusNatureSpellPowerMultiplier);
            calculatedStats.ShadowSpellModifier *= (1 + characterStats.BonusShadowSpellPowerMultiplier);

            calculatedStats.ResilienceCritDamageReduction = 1;
            calculatedStats.ResilienceCritRateReduction = 0;

            calculatedStats.ArcaneCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * calculationOptions.SpellPower)) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.FireCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * calculationOptions.SpellPower)) * (1 + 0.08f * calculationOptions.Ignite) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.FrostCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.2f * calculationOptions.IceShards + 0.25f * calculationOptions.SpellPower)) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.NatureCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * calculationOptions.SpellPower)) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.ShadowCritBonus = (1 + (1.5f * (1 + characterStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * calculationOptions.SpellPower)) * calculatedStats.ResilienceCritDamageReduction;

            calculatedStats.ArcaneCritRate = calculatedStats.SpellCrit;
            calculatedStats.FireCritRate = calculatedStats.SpellCrit + 0.02f * calculationOptions.CriticalMass + 0.01f * calculationOptions.Pyromaniac;
            if (combustion)
            {
                calculatedStats.CombustionDuration = Combustion(calculatedStats.FireCritRate);
                calculatedStats.FireCritRate = 3 / calculatedStats.CombustionDuration;
            }
            calculatedStats.FrostCritRate = calculatedStats.SpellCrit + characterStats.SpellFrostCritRating / 22.08f / 100f;
            calculatedStats.NatureCritRate = calculatedStats.SpellCrit;
            calculatedStats.ShadowCritRate = calculatedStats.SpellCrit;

            float threatFactor = (1 + characterStats.ThreatIncreaseMultiplier) * (1 - characterStats.ThreatReductionMultiplier);

            calculatedStats.ArcaneThreatMultiplier = threatFactor * (1 - calculationOptions.ArcaneSubtlety * 0.2f);
            calculatedStats.FireThreatMultiplier = threatFactor * (1 - calculationOptions.BurningSoul * 0.05f);
            calculatedStats.FrostThreatMultiplier = threatFactor * (1 - ((calculationOptions.FrostChanneling > 0) ? (0.01f + 0.03f * calculationOptions.FrostChanneling) : 0f));
            calculatedStats.NatureThreatMultiplier = threatFactor;
            calculatedStats.ShadowThreatMultiplier = threatFactor;

            return calculatedStats;
        }

        private Stats GetRawStats(Character character, Item additionalItem, CalculationOptionsMage calculationOptions, List<string> autoActivatedBuffs, string armor)
        {
            Stats stats = new Stats();
            AccumulateItemStats(stats, character, additionalItem);
            AccumulateEnchantsStats(stats, character);
            List<string> activeBuffs = new List<string>();
            activeBuffs.AddRange(character.ActiveBuffs);

            if (!character.DisableBuffAutoActivation)
            {
                if (calculationOptions.MaintainScorch)
                {
                    if (calculationOptions.ImprovedScorch > 0)
                    {
                        if (!character.ActiveBuffs.Contains("Improved Scorch"))
                        {
                            activeBuffs.Add("Improved Scorch");
                            autoActivatedBuffs.Add("Improved Scorch");
                        }
                    }
                }
                if (calculationOptions.WintersChill > 0)
                {
                    if (!character.ActiveBuffs.Contains("Winter's Chill"))
                    {
                        activeBuffs.Add("Winter's Chill");
                        autoActivatedBuffs.Add("Winter's Chill");
                    }
                }
                if (armor != null)
                {
                    if (!character.ActiveBuffs.Contains(armor))
                    {
                        activeBuffs.Add(armor);
                        autoActivatedBuffs.Add(armor);
                        RemoveConflictingBuffs(activeBuffs, armor);
                    }
                }
            }

            AccumulateBuffsStats(stats, activeBuffs);

            return stats;
        }

        // required by base class, but never used
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            return GetCharacterStats(character, additionalItem, GetRawStats(character, additionalItem, calculationOptions, new List<string>(), null), calculationOptions);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats rawStats, CalculationOptionsMage calculationOptions)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.BloodElf:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 49f,
                        Intellect = 149f,
                        Spirit = 144,
                        BonusIntellectMultiplier = 0.03f * calculationOptions.ArcaneMind,
                    };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 50f,
                        Intellect = 152f,
                        Spirit = 147,
                        BonusIntellectMultiplier = 0.03f * calculationOptions.ArcaneMind,
                    };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 50f,
                        Intellect = 154f,
                        Spirit = 145,
                        ArcaneResistance = 10,
                        BonusIntellectMultiplier = 1.05f * (1 + 0.03f * calculationOptions.ArcaneMind) - 1
                    };
                    break;
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 51f,
                        Intellect = 151f,
                        Spirit = 145,
                        BonusIntellectMultiplier = 0.03f * calculationOptions.ArcaneMind,
                        BonusSpiritMultiplier = 0.1f
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 52f,
                        Intellect = 147f,
                        Spirit = 146,
                        BonusIntellectMultiplier = 0.03f * calculationOptions.ArcaneMind,
                    };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats()
                    {
                        Health = 3213f,
                        Mana = 1961f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 52f,
                        Intellect = 149f,
                        Spirit = 150,
                        BonusIntellectMultiplier = 0.03f * calculationOptions.ArcaneMind,
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            Stats statsGearEnchantsBuffs = rawStats;

            if (calculationOptions.WotLK && calculationOptions.StudentOfTheMind > 0)
            {
                statsGearEnchantsBuffs.BonusSpiritMultiplier = (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier) * (1.01f + 0.03f * calculationOptions.StudentOfTheMind) - 1;
            }
            Stats statsTotal = statsGearEnchantsBuffs + statsRace;
            statsTotal.Strength = (float)Math.Floor((Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier)) + statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor((Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier)) + statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier)) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Intellect = (float)Math.Floor((Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier)) + statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier)) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));
            statsTotal.Stamina = (float)Math.Floor((Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier)) + statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Spirit = (float)Math.Floor((Math.Floor(statsRace.Spirit * (1 + statsRace.BonusSpiritMultiplier)) + statsGearEnchantsBuffs.Spirit * (1 + statsRace.BonusSpiritMultiplier)) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier));
            
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusSpiritMultiplier = ((1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier)) - 1;

            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect + statsGearEnchantsBuffs.Mana);
            statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2f + statsTotal.Intellect * calculationOptions.ArcaneFortitude);

            float magicAbsorption = (calculationOptions.WotLK ? 0.2f * 70.0f : 2.0f) * calculationOptions.MagicAbsorption;
            int frostWarding = calculationOptions.FrostWarding;
            statsTotal.AllResist += magicAbsorption;

            /*if (calculationOptions.MageArmor == "Ice")
            {
                statsTotal.Armor += (float)Math.Floor(645 * (1 + 0.15f * frostWarding));
                statsTotal.FrostResistance += (float)Math.Floor(18 * (1 + 0.15f * frostWarding));
            }*/

            statsTotal.SpellCombatManaRegeneration += 0.1f * calculationOptions.ArcaneMeditation;

            statsTotal.SpellPenetration += 5 * calculationOptions.ArcaneSubtlety;

            statsTotal.Mp5 += calculationOptions.ShadowPriest;

            statsTotal.SpellDamageFromIntellectPercentage += 0.05f * calculationOptions.MindMastery;

            statsTotal.AllResist += statsTotal.MageAllResist;

            if (calculationOptions.WotLK && calculationOptions.PotentSpirit > 0) statsTotal.SpellCritRating += (calculationOptions.PotentSpirit == 2 ? 0.15f : 0.07f) * statsTotal.Spirit;

            statsTotal.SpellDamageRating += statsTotal.SpellDamageFromIntellectPercentage * statsTotal.Intellect;
            statsTotal.SpellDamageRating += statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit;

            return statsTotal;
        }

        private static string[] TalentList = { "ArcaneSubtlety", "ArcaneFocus", "ImprovedArcaneMissiles", "WandSpecialization", "MagicAbsorption", "ArcaneConcentration", "MagicAttunement", "ArcaneImpact", "ArcaneFortitude", "ImprovedManaShield", "ImprovedCounterspell", "ArcaneMeditation", "ImprovedBlink", "PresenceOfMind", "ArcaneMind", "PrismaticCloak", "ArcaneInstability", "ArcanePotency", "EmpoweredArcaneMissiles", "ArcanePower", "SpellPower", "MindMastery", "Slow", "ImprovedFireball", "Impact", "Ignite", "FlameThrowing", "ImprovedFireBlast", "Incinerate", "ImprovedFlamestrike", "Pyroblast", "BurningSoul", "ImprovedScorch", "ImprovedFireWard", "MasterOfElements", "PlayingWithFire", "CriticalMass", "BlastWave", "BlazingSpeed", "FirePower", "Pyromaniac", "Combustion", "MoltenFury", "EmpoweredFireball", "DragonsBreath", "FrostWarding", "ImprovedFrostbolt", "ElementalPrecision", "IceShards", "Frostbite", "ImprovedFrostNova", "Permafrost", "PiercingIce", "IcyVeins", "ImprovedBlizzard", "ArcticReach", "FrostChanneling", "Shatter", "FrozenCore", "ColdSnap", "ImprovedConeOfCold", "IceFloes", "WintersChill", "IceBarrier", "ArcticWinds", "EmpoweredFrostbolt", "SummonWaterElemental", "PotentSpirit", "StudentOfTheMind" };
        private static string[] TalentListFriendly = { "Arcane Subtlety", "Arcane Focus", "Improved Arcane Missiles", "Wand Specialization", "Magic Absorption", "Arcane Concentration", "Magic Attunement", "Arcane Impact", "Arcane Fortitude", "Improved Mana Shield", "Improved Counterspell", "Arcane Meditation", "Improved Blink", "Presence of Mind", "Arcane Mind", "Prismatic Cloak", "Arcane Instability", "Arcane Potency", "Empowered Arcane Missiles", "Arcane Power", "Spell Power", "Mind Mastery", "Slow", "Improved Fireball", "Impact", "Ignite", "Flame Throwing", "Improved Fire Blast", "Incinerate", "Improved Flamestrike", "Pyroblast", "Burning Soul", "Improved Scorch", "Improved Fire Ward", "Master of Elements", "Playing with Fire", "Critical Mass", "Blast Wave", "Blazing Speed", "Fire Power", "Pyromaniac", "Combustion", "Molten Fury", "Empowered Fireball", "Dragon's Breath", "Frost Warding", "Improved Frostbolt", "Elemental Precision", "Ice Shards", "Frostbite", "Improved Frost Nova", "Permafrost", "Piercing Ice", "Icy Veins", "Improved Blizzard", "Arctic Reach", "Frost Channeling", "Shatter", "Frozen Core", "Cold Snap", "Improved Cone of Cold", "Ice Floes", "Winter's Chill", "Ice Barrier", "Arctic Winds", "Empowered Frostbolt", "Summon Water Elemental", "Potent Spirit", "Student of the Mind" };
        private static int[] MaxTalentPoints = { 2, 5, 5, 2, 5, 5, 2, 3, 1, 2, 2, 3, 2, 1, 5, 2, 3, 3, 3, 1, 2, 5, 1, 5, 5, 5, 2, 3, 2, 3, 1, 2, 3, 2, 3, 3, 3, 1, 2, 5, 3, 1, 2, 5, 1, 2, 5, 3, 5, 3, 2, 3, 3, 1, 3, 2, 3, 5, 3, 1, 3, 2, 5, 1, 5, 5, 1, 2, 3 };

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsMage baseCalc, currentCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Talents (per talent point)":
                    CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
                    currentCalc = GetCharacterCalculations(character) as CharacterCalculationsMage;
                    bool savedIncrementalOptimizations = calculationOptions.IncrementalOptimizations;
                    bool savedSmartOptimizations = calculationOptions.SmartOptimization;

                    calculationOptions.IncrementalOptimizations = false;
                    calculationOptions.SmartOptimization = true;

                    for (int index = 0; index < TalentList.Length; index++ )
                    {
                        string talent = TalentList[index];
                        int maxPoints = MaxTalentPoints[index];
                        int currentPoints = calculationOptions.GetTalentByName(talent);

                        if (currentPoints > 0)
                        {
                            calculationOptions.SetTalentByName(talent, 0);
                            calc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = string.Format("{0} ({1})", TalentListFriendly[index], currentPoints);
                            comparison.Equipped = true;
                            comparison.OverallPoints = (currentCalc.OverallPoints - calc.OverallPoints) / (float)currentPoints;
                            subPoints = new float[calc.SubPoints.Length];
                            for (int i = 0; i < calc.SubPoints.Length; i++)
                            {
                                subPoints[i] = (currentCalc.SubPoints[i] - calc.SubPoints[i]) / (float)currentPoints;
                            }
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }

                        if (currentPoints < MaxTalentPoints[index])
                        {
                            calculationOptions.SetTalentByName(talent, MaxTalentPoints[index]);
                            calc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = string.Format("{0} ({1})", TalentListFriendly[index], MaxTalentPoints[index]);
                            comparison.Equipped = false;
                            comparison.OverallPoints = (calc.OverallPoints - currentCalc.OverallPoints) / (float)(MaxTalentPoints[index] - currentPoints);
                            subPoints = new float[calc.SubPoints.Length];
                            for (int i = 0; i < calc.SubPoints.Length; i++)
                            {
                                subPoints[i] = (calc.SubPoints[i] - currentCalc.SubPoints[i]) / (float)(MaxTalentPoints[index] - currentPoints);
                            }
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }

                        calculationOptions.SetTalentByName(talent, currentPoints);
                    }

                    calculationOptions.IncrementalOptimizations = savedIncrementalOptimizations;
                    calculationOptions.SmartOptimization = savedSmartOptimizations;

                    return comparisonList.ToArray();
                case "Talent Specs":
                    currentCalc = GetCharacterCalculations(character) as CharacterCalculationsMage;
                    comparison = CreateNewComparisonCalculation();
                    comparison.Name = "Current";
                    comparison.Equipped = true;
                    comparison.OverallPoints = currentCalc.OverallPoints;
                    subPoints = new float[currentCalc.SubPoints.Length];
                    for (int i = 0; i < currentCalc.SubPoints.Length; i++)
                    {
                        subPoints[i] = currentCalc.SubPoints[i];
                    }
                    comparison.SubPoints = subPoints;

                    comparisonList.Add(comparison);

                    string[] talentSpecList = new string[] { "Fire (2/48/11)", "Fire (10/48/3)", "Fire/Cold Snap (0/40/21)", "Frost (10/0/51)", "Arcane (48/0/13)", "Arcane (43/0/18)", "Arcane/Fire (40/18/3)", "Arcane/Fire (40/10/11)", "Arcane/Frost (40/0/21)" };
                    Character charClone = character.Clone();
                    calculationOptions = charClone.CalculationOptions as CalculationOptionsMage;
                    calculationOptions = calculationOptions.Clone();
                    charClone.CalculationOptions = calculationOptions;
                    calculationOptions.IncrementalOptimizations = false;
                    calculationOptions.SmartOptimization = true;

                    for (int index = 0; index < talentSpecList.Length; index++)
                    {
                        LoadTalentSpec(charClone, talentSpecList[index]);

                        calc = GetCharacterCalculations(charClone) as CharacterCalculationsMage;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = talentSpecList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = calc.OverallPoints;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            subPoints[i] = calc.SubPoints[i];
                        }
                        comparison.SubPoints = subPoints;

                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Item Budget":
                    Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { SpellDamageRating = 11.7f } },
                        new Item() { Stats = new Stats() { Mp5 = 4 } },
                        new Item() { Stats = new Stats() { SpellCritRating = 10 } },
                        new Item() { Stats = new Stats() { SpellHasteRating = 10 } },
                        new Item() { Stats = new Stats() { SpellHitRating = 10 } },
                    };
                    string[] statList = new string[] {
                        "11.7 Spell Damage",
                        "4 Mana per 5 sec",
                        "10 Spell Crit Rating",
                        "10 Spell Haste Rating",
                        "10 Spell Hit Rating",
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                    for (int index = 0; index < statList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsMage;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = statList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                        }
                        comparison.SubPoints = subPoints;

                        comparisonList.Add(comparison);
                    }

                    //Intellect
                    CharacterCalculationsMage calcAtAdd;
                    Stats statsAtAdd = baseCalc.BasicStats;
                    float baseInt = baseCalc.BasicStats.Intellect;
                    float intToAdd = 0f;
                    while (baseInt == statsAtAdd.Intellect && intToAdd < 2)
                    {
                        intToAdd += 0.01f;
                        statsAtAdd = GetCharacterStats(character, new Item() { Stats = new Stats() { Intellect = intToAdd } });
                    }
                    calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = intToAdd } }) as CharacterCalculationsMage;

                    Stats statsAtSubtract = baseCalc.BasicStats;
                    float intToSubtract = 0f;
                    while (baseInt == statsAtSubtract.Intellect && intToSubtract > -2)
                    {
                        intToSubtract -= 0.01f;
                        statsAtSubtract = GetCharacterStats(character, new Item() { Stats = new Stats() { Intellect = intToSubtract } });
                    }
                    intToSubtract += 0.01f;

                    comparison = CreateNewComparisonCalculation();
                    comparison.Name = "10 Intellect";
                    comparison.Equipped = false;
                    comparison.OverallPoints = 10 * (calcAtAdd.OverallPoints - baseCalc.OverallPoints) / (intToAdd - intToSubtract);
                    subPoints = new float[baseCalc.SubPoints.Length];
                    for (int i = 0; i < baseCalc.SubPoints.Length; i++)
                    {
                        subPoints[i] = 10 * (calcAtAdd.SubPoints[i] - baseCalc.SubPoints[i]) / (intToAdd - intToSubtract);
                    }
                    comparison.SubPoints = subPoints;

                    comparisonList.Add(comparison);

                    //Spirit
                    statsAtAdd = baseCalc.BasicStats;
                    float baseSpi = baseCalc.BasicStats.Spirit;
                    float spiToAdd = 0f;
                    while (baseSpi == statsAtAdd.Spirit && spiToAdd < 2)
                    {
                        spiToAdd += 0.01f;
                        statsAtAdd = GetCharacterStats(character, new Item() { Stats = new Stats() { Spirit = spiToAdd } });
                    }
                    calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = spiToAdd } }) as CharacterCalculationsMage;

                    statsAtSubtract = baseCalc.BasicStats;
                    float spiToSubtract = 0f;
                    while (baseSpi == statsAtSubtract.Spirit && spiToSubtract > -2)
                    {
                        spiToSubtract -= 0.01f;
                        statsAtSubtract = GetCharacterStats(character, new Item() { Stats = new Stats() { Spirit = spiToSubtract } });
                    }
                    spiToSubtract += 0.01f;

                    comparison = CreateNewComparisonCalculation();
                    comparison.Name = "10 Spirit";
                    comparison.Equipped = false;
                    comparison.OverallPoints = 10 * (calcAtAdd.OverallPoints - baseCalc.OverallPoints) / (spiToAdd - spiToSubtract);
                    subPoints = new float[baseCalc.SubPoints.Length];
                    for (int i = 0; i < baseCalc.SubPoints.Length; i++)
                    {
                        subPoints[i] = 10 * (calcAtAdd.SubPoints[i] - baseCalc.SubPoints[i]) / (spiToAdd - spiToSubtract);
                    }
                    comparison.SubPoints = subPoints;

                    comparisonList.Add(comparison);

                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
					"Health",
                    "Nature Resistance",
                    "Fire Resistance",
                    "Frost Resistance",
                    "Shadow Resistance",
                    "Arcane Resistance",
					};
                return _optimizableCalculationLabels;
            }
        }

        public static void LoadTalentSpec(Character character, string talentSpec)
        {
            string talentCode = null;
            switch (talentSpec)
            {
                case "Fire (2/48/11)":
                    talentCode = "2000000000000000000000050520201230333115312510532000010000000000000";
                    break;
                case "Fire (10/48/3)":
                    talentCode = "2300050000000000000000050520211230333105312510030000000000000000000";
                    break;
                case "Fire/Cold Snap (0/40/21)":
                    talentCode = "0000000000000000000000050510200230233005112500535000310030010000000";
                    break;
                case "Frost (10/0/51)":
                    talentCode = "2300050000000000000000000000000000000000000000535020312235010251551";
                    break;
                case "Arcane (48/0/13)":
                    talentCode = "2550050300230150333125000000000000000000000000534000010000000000000";
                    break;
                case "Arcane (43/0/18)":
                    talentCode = "2250050300030150333125000000000000000000000000515000310030000000000";
                    break;
                case "Arcane/Fire (40/18/3)":
                    talentCode = "2500050300230150330125050520001230000000000000030000000000000000000";
                    break;
                case "Arcane/Fire (40/10/11)":
                    talentCode = "2500050300230150330125050500000000000000000000532000010000000000000";
                    break;
                case "Arcane/Frost (40/0/21)":
                    talentCode = "2500052300030150330125000000000000000000000000535000310030010000000";
                    break;
            }

            LoadTalentCode(character, talentCode);
        }

        public static void LoadTalentCode(Character character, string talentCode)
        {
            if (talentCode == null || talentCode.Length < 66) return;
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;

            calculationOptions.ArcaneSubtlety = int.Parse(talentCode.Substring(0, 1));
            calculationOptions.ArcaneFocus = int.Parse(talentCode.Substring(1, 1));
            calculationOptions.ImprovedArcaneMissiles = int.Parse(talentCode.Substring(2, 1));
            calculationOptions.WandSpecialization = int.Parse(talentCode.Substring(3, 1));
            calculationOptions.MagicAbsorption = int.Parse(talentCode.Substring(4, 1));
            calculationOptions.ArcaneConcentration = int.Parse(talentCode.Substring(5, 1));
            calculationOptions.MagicAttunement = int.Parse(talentCode.Substring(6, 1));
            calculationOptions.ArcaneImpact = int.Parse(talentCode.Substring(7, 1));
            calculationOptions.ArcaneFortitude = int.Parse(talentCode.Substring(8, 1));
            calculationOptions.ImprovedManaShield = int.Parse(talentCode.Substring(9, 1));
            calculationOptions.ImprovedCounterspell = int.Parse(talentCode.Substring(10, 1));
            calculationOptions.ArcaneMeditation = int.Parse(talentCode.Substring(11, 1));
            calculationOptions.ImprovedBlink = int.Parse(talentCode.Substring(12, 1));
            calculationOptions.PresenceOfMind = int.Parse(talentCode.Substring(13, 1));
            calculationOptions.ArcaneMind = int.Parse(talentCode.Substring(14, 1));
            calculationOptions.PrismaticCloak = int.Parse(talentCode.Substring(15, 1));
            calculationOptions.ArcaneInstability = int.Parse(talentCode.Substring(16, 1));
            calculationOptions.ArcanePotency = int.Parse(talentCode.Substring(17, 1));
            calculationOptions.EmpoweredArcaneMissiles = int.Parse(talentCode.Substring(18, 1));
            calculationOptions.ArcanePower = int.Parse(talentCode.Substring(19, 1));
            calculationOptions.SpellPower = int.Parse(talentCode.Substring(20, 1));
            calculationOptions.MindMastery = int.Parse(talentCode.Substring(21, 1));
            calculationOptions.Slow = int.Parse(talentCode.Substring(22, 1));
            calculationOptions.ImprovedFireball = int.Parse(talentCode.Substring(23, 1));
            calculationOptions.Impact = int.Parse(talentCode.Substring(24, 1));
            calculationOptions.Ignite = int.Parse(talentCode.Substring(25, 1));
            calculationOptions.FlameThrowing = int.Parse(talentCode.Substring(26, 1));
            calculationOptions.ImprovedFireBlast = int.Parse(talentCode.Substring(27, 1));
            calculationOptions.Incinerate = int.Parse(talentCode.Substring(28, 1));
            calculationOptions.ImprovedFlamestrike = int.Parse(talentCode.Substring(29, 1));
            calculationOptions.Pyroblast = int.Parse(talentCode.Substring(30, 1));
            calculationOptions.BurningSoul = int.Parse(talentCode.Substring(31, 1));
            calculationOptions.ImprovedScorch = int.Parse(talentCode.Substring(32, 1));
            calculationOptions.ImprovedFireWard = int.Parse(talentCode.Substring(33, 1));
            calculationOptions.MasterOfElements = int.Parse(talentCode.Substring(34, 1));
            calculationOptions.PlayingWithFire = int.Parse(talentCode.Substring(35, 1));
            calculationOptions.CriticalMass = int.Parse(talentCode.Substring(36, 1));
            calculationOptions.BlastWave = int.Parse(talentCode.Substring(37, 1));
            calculationOptions.BlazingSpeed = int.Parse(talentCode.Substring(38, 1));
            calculationOptions.FirePower = int.Parse(talentCode.Substring(39, 1));
            calculationOptions.Pyromaniac = int.Parse(talentCode.Substring(40, 1));
            calculationOptions.Combustion = int.Parse(talentCode.Substring(41, 1));
            calculationOptions.MoltenFury = int.Parse(talentCode.Substring(42, 1));
            calculationOptions.EmpoweredFireball = int.Parse(talentCode.Substring(43, 1));
            calculationOptions.DragonsBreath = int.Parse(talentCode.Substring(44, 1));
            calculationOptions.FrostWarding = int.Parse(talentCode.Substring(45, 1));
            calculationOptions.ImprovedFrostbolt = int.Parse(talentCode.Substring(46, 1));
            calculationOptions.ElementalPrecision = int.Parse(talentCode.Substring(47, 1));
            calculationOptions.IceShards = int.Parse(talentCode.Substring(48, 1));
            calculationOptions.Frostbite = int.Parse(talentCode.Substring(49, 1));
            calculationOptions.ImprovedFrostNova = int.Parse(talentCode.Substring(50, 1));
            calculationOptions.Permafrost = int.Parse(talentCode.Substring(51, 1));
            calculationOptions.PiercingIce = int.Parse(talentCode.Substring(52, 1));
            calculationOptions.IcyVeins = int.Parse(talentCode.Substring(53, 1));
            calculationOptions.ImprovedBlizzard = int.Parse(talentCode.Substring(54, 1));
            calculationOptions.ArcticReach = int.Parse(talentCode.Substring(55, 1));
            calculationOptions.FrostChanneling = int.Parse(talentCode.Substring(56, 1));
            calculationOptions.Shatter = int.Parse(talentCode.Substring(57, 1));
            calculationOptions.FrozenCore = int.Parse(talentCode.Substring(58, 1));
            calculationOptions.ColdSnap = int.Parse(talentCode.Substring(59, 1));
            calculationOptions.ImprovedConeOfCold = int.Parse(talentCode.Substring(60, 1));
            calculationOptions.IceFloes = int.Parse(talentCode.Substring(61, 1));
            calculationOptions.WintersChill = int.Parse(talentCode.Substring(62, 1));
            calculationOptions.IceBarrier = int.Parse(talentCode.Substring(63, 1));
            calculationOptions.ArcticWinds = int.Parse(talentCode.Substring(64, 1));
            calculationOptions.EmpoweredFrostbolt = int.Parse(talentCode.Substring(65, 1));
            calculationOptions.SummonWaterElemental = int.Parse(talentCode.Substring(66, 1));
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
				AllResist = stats.AllResist,
				MageAllResist = stats.MageAllResist,
				ArcaneResistance = stats.ArcaneResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                NatureResistance = stats.NatureResistance,
                ShadowResistance = stats.ShadowResistance,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                DefenseRating = stats.DefenseRating,
                DodgeRating = stats.DodgeRating,
                Health = stats.Health,
                Mp5 = stats.Mp5,
                Resilience = stats.Resilience,
                SpellCritRating = stats.SpellCritRating,
                SpellDamageRating = stats.SpellDamageRating,
                SpellFireDamageRating = stats.SpellFireDamageRating,
                SpellHasteRating = stats.SpellHasteRating,
                SpellHitRating = stats.SpellHitRating,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                Mana = stats.Mana,
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellPenetration = stats.SpellPenetration,
                SpellFrostDamageRating = stats.SpellFrostDamageRating,
                Armor = stats.Armor,
                Hp5 = stats.Hp5,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                BonusArcaneSpellPowerMultiplier = stats.BonusArcaneSpellPowerMultiplier,
                BonusFireSpellPowerMultiplier = stats.BonusFireSpellPowerMultiplier,
                BonusFrostSpellPowerMultiplier = stats.BonusFrostSpellPowerMultiplier,
                SpellFrostCritRating = stats.SpellFrostCritRating,
                ArcaneBlastBonus = stats.ArcaneBlastBonus,
                SpellDamageFor6SecOnCrit = stats.SpellDamageFor6SecOnCrit,
                EvocationExtension = stats.EvocationExtension,
                BonusMageNukeMultiplier = stats.BonusMageNukeMultiplier,
                LightningCapacitorProc = stats.LightningCapacitorProc,
                SpellDamageFor20SecOnUse2Min = stats.SpellDamageFor20SecOnUse2Min,
                SpellHasteFor20SecOnUse2Min = stats.SpellHasteFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaRestorePerCast = stats.ManaRestorePerCast,
                ManaRestorePerHit = stats.ManaRestorePerHit,
                BonusManaGem = stats.BonusManaGem,
                SpellDamageFor15SecOnManaGem = stats.SpellDamageFor15SecOnManaGem,
                SpellDamageFor10SecOnHit_10_45 = stats.SpellDamageFor10SecOnHit_10_45,
                SpellDamageFromIntellectPercentage = stats.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                SpellDamageFor10SecOnResist = stats.SpellDamageFor10SecOnResist,
                SpellDamageFor15SecOnCrit_20_45 = stats.SpellDamageFor15SecOnCrit_20_45,
                SpellDamageFor15SecOnUse90Sec = stats.SpellDamageFor15SecOnUse90Sec,
                SpellHasteFor5SecOnCrit_50 = stats.SpellHasteFor5SecOnCrit_50,
                SpellHasteFor6SecOnCast_15_45 = stats.SpellHasteFor6SecOnCast_15_45,
                SpellDamageFor10SecOnHit_5 = stats.SpellDamageFor10SecOnHit_5,
                SpellHasteFor6SecOnHit_10_45 = stats.SpellHasteFor6SecOnHit_10_45,
                SpellDamageFor10SecOnCrit_20_45 = stats.SpellDamageFor10SecOnCrit_20_45,
                BonusManaPotion = stats.BonusManaPotion,
                MageSpellCrit = stats.MageSpellCrit,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                SpellHasteFor20SecOnUse5Min = stats.SpellHasteFor20SecOnUse5Min,
                AldorRegaliaInterruptProtection = stats.AldorRegaliaInterruptProtection,
                SpellDamageFor15SecOnUse2Min = stats.SpellDamageFor15SecOnUse2Min,
                ShatteredSunAcumenProc = stats.ShatteredSunAcumenProc
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            float mageStats = stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellCritRating + stats.SpellDamageRating + stats.SpellFireDamageRating + stats.SpellHasteRating + stats.SpellHitRating + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusSpiritMultiplier + stats.SpellFrostDamageRating + stats.SpellArcaneDamageRating + stats.SpellPenetration + stats.Mana + stats.SpellCombatManaRegeneration + stats.BonusArcaneSpellPowerMultiplier + stats.BonusFireSpellPowerMultiplier + stats.BonusFrostSpellPowerMultiplier + stats.SpellFrostCritRating + stats.ArcaneBlastBonus + stats.SpellDamageFor6SecOnCrit + stats.EvocationExtension + stats.BonusMageNukeMultiplier + stats.LightningCapacitorProc + stats.SpellDamageFor20SecOnUse2Min + stats.SpellHasteFor20SecOnUse2Min + stats.Mp5OnCastFor20SecOnUse2Min + stats.ManaRestorePerHit + stats.ManaRestorePerCast + stats.SpellDamageFor15SecOnManaGem + stats.BonusManaGem + stats.SpellDamageFor10SecOnHit_10_45 + stats.SpellDamageFromIntellectPercentage + stats.SpellDamageFromSpiritPercentage + stats.SpellDamageFor10SecOnResist + stats.SpellDamageFor15SecOnCrit_20_45 + stats.SpellDamageFor15SecOnUse90Sec + stats.SpellHasteFor5SecOnCrit_50 + stats.SpellHasteFor6SecOnCast_15_45 + stats.SpellDamageFor10SecOnHit_5 + stats.SpellHasteFor6SecOnHit_10_45 + stats.SpellDamageFor10SecOnCrit_20_45 + stats.BonusManaPotion + stats.MageSpellCrit + stats.ThreatReductionMultiplier + stats.AllResist + stats.MageAllResist + stats.ArcaneResistance + stats.FireResistance + stats.FrostResistance + stats.NatureResistance + stats.ShadowResistance + stats.SpellHasteFor20SecOnUse5Min + stats.AldorRegaliaInterruptProtection + stats.SpellDamageFor15SecOnUse2Min + stats.ShatteredSunAcumenProc;
            float ignoreStats = stats.Agility + stats.Strength + stats.AttackPower + stats.Healing + stats.DefenseRating + stats.Defense + stats.Dodge + stats.Parry + stats.DodgeRating + stats.ParryRating + stats.Hit + stats.HitRating + stats.ExpertiseRating + stats.Expertise + stats.Block + stats.BlockRating + stats.BlockValue + stats.SpellShadowDamageRating + stats.SpellNatureDamageRating;
            return (mageStats > 0 || ((stats.Health + stats.Stamina) > 0 && ignoreStats == 0.0f));
        }
    }
}
