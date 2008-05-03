using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Rawr.Mage
{
	[System.ComponentModel.DisplayName("Mage|Spell_Holy_MagicalSentry")]
    class CalculationsMage : CalculationsBase
    {
        public static void LoadRelevantItems()
        {
            int[] idlist = { 30735, 30725, 28515, 29240, 29255, 24267, 24263, 29241, 28799, 24257, 29242, 29258, 28517, 28670, 30050, 30531, 28797, 28804, 29076, 29986, 30064, 30206, 30020, 21870, 21869, 28404, 28405, 28402, 28409, 28411, 28410, 24256, 25854, 25855, 25857, 25856, 25858, 30680, 30205, 29080, 30668, 28507, 28477, 30079, 30673, 30763, 30764, 30761, 30762, 30532, 30207, 30734, 29078, 30675, 28654, 24260, 30210, 30024, 28726, 28980, 28981, 28982, 29001, 29002, 29003, 29918, 29122, 28565, 29079, 30684, 24258, 30056, 28602, 30196, 28766, 28585, 29257, 28379, 28378, 28570, 28653, 29369, 28780, 21863, 21864, 21865, 21846, 21847, 21848, 24266, 24262, 28594, 28744, 24255, 30067, 29077, 28586, 31340, 29177, 29349, 28762, 30061, 29172, 29922, 31319, 29997, 28516, 29383, 28530, 31338, 30059, 31321, 29381, 29352, 28830, 28789, 30049, 30619, 29270, 30099, 28649, 28346, 29387, 29119, 29370, 28734, 28781, 29273, 31339, 29368, 32649, 29123, 28730, 28745, 28528, 29386, 29272, 28245, 28244, 30008, 30022, 28727, 31746, 29379, 29367, 30109, 30052, 30738, 28753, 30083, 30667, 29384, 28757, 30666, 28579, 28674, 29269, 30629, 29126, 30720, 30028, 30626, 30834, 28675, 28510, 30620, 29271, 28603, 29347, 30007, 28785, 29181, 31326, 30627, 31113, 29277, 29278, 29281, 29285, 29286, 29282, 29287, 29279, 29283, 28509, 29359, 24557, 28633, 31334, 28658, 29355, 29988, 30021, 28783, 29350, 28673, 28320, 29982, 32089, 29972, 32239, 32343, 30894, 30872, 28792, 32587, 32586, 32270, 32247, 30913, 32256, 31056, 31055, 31058, 31059, 31057, 28793, 30037, 32655, 30038, 32541, 30107, 32757, 30884, 29992, 30015, 32374, 29303, 30870, 29987, 32349, 30888, 32338, 30916, 32327, 32047, 32048, 32049, 32050, 32051, 32962, 32055, 33056, 32799, 32811, 32787, 33067, 33065, 32807, 32820, 32795, 32525, 32483, 33054, 32331, 34073, 32488, 33494, 34049, 33192, 33304, 34162, 33357, 33285, 33453, 33497, 33489, 33584, 33588, 33591, 33293, 33500, 33586, 33766, 33291, 33829, 33466, 33317, 34066, 33757, 33758, 33759, 33760, 33761, 33764, 33853, 33920, 33921, 34577, 34579, 34540, 34182, 35326, 34557, 34574, 34447, 35321, 34810, 34607, 34808, 34406, 34405, 34386, 34399, 34610, 34678, 34393, 34366, 34364, 34470, 35320, 35319, 34347, 34837, 35700, 35703, 34889, 34919, 34937, 34938, 34918, 34917, 34936, 35290, 33140, 32204, 32225, 33133, 32207, 32215, 32210, 32202, 32218, 32224, 32196, 32201, 32203, 32221, 24047, 34220, 24065, 25890, 35503, 35318, 24050, 24056, 31861, 25901, 24037, 25893, 24059, 35315, 24066, 35316, 24030, 24035, 24039, 31867 };
            foreach (int id in idlist)
            {
                Item.LoadFromId(id, true, "Batch Load", true);
            }
        }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Dps", System.Drawing.Color.FromArgb(0, 128, 255));
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
                    "Spell Info:Arcane Blast*Spammed",
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

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Mage; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationMage(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsMage(); }

        private bool IsItemActivatable(Item item)
        {
            if (item == null) return false;
            return (item.Stats.SpellDamageFor20SecOnUse2Min + item.Stats.SpellHasteFor20SecOnUse2Min + item.Stats.Mp5OnCastFor20SecOnUse2Min + item.Stats.SpellDamageFor15SecOnManaGem + item.Stats.SpellDamageFor15SecOnUse90Sec + item.Stats.SpellHasteFor20SecOnUse5Min > 0);
        }

        private class CompactLP : IComparable<CompactLP>
        {
            private int lpRows, cRows;
            private int lpCols, cCols;
            public LP lp;
            bool[] rowEnabled, colEnabled;
            int[] CRow, CCol;
            double[] compactSolution = null;
            bool allowReuse;
            bool needsDual;

            public int HeroismHash;
            public int APHash;
            public int IVHash;

            public CompactLP Clone()
            {
                if (compactSolution != null && !allowReuse) throw new InvalidOperationException();
                CompactLP clone = (CompactLP)this.MemberwiseClone();
                clone.compactSolution = null;
                //clone.lp = (double[,])clone.lp.Clone();
                clone.lp = lp.Clone();
                return clone;
            }

            public CompactLP(int rows, int cols, bool allowReuse)
            {
                this.allowReuse = allowReuse;
                lpRows = rows;
                lpCols = cols;

                rowEnabled = new bool[rows];
                colEnabled = new bool[cols];

                for (int i = 0; i < rows; i++) rowEnabled[i] = true;
                for (int j = 0; j < cols; j++) colEnabled[j] = true;

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

                lp = new LP(cRows, cCols);
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
                    if (CRow[row] == -1 || CCol[col] == -1) return 0;
                    return lp[CRow[row], CCol[col]];
                }
                set
                {
                    if (CRow[row] == -1 || CCol[col] == -1) return;
                    lp[CRow[row], CCol[col]] = value;
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
                    if (CCol[j] >= 0) expanded[j] = compactSolution[CCol[j]];
                }
                return expanded;
            }

            public double Value
            {
                get
                {
                    SolveInternal();
                    return compactSolution[compactSolution.Length - 1];
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
            CompiledCalculationOptions calculationOptions = new CompiledCalculationOptions(character);
            if (computeIncrementalSet) calculationOptions.IncrementalOptimizations = false;
            if (calculationOptions.IncrementalOptimizations && !calculationOptions.DisableBuffAutoActivation)
            {
                return GetCharacterCalculations(character, additionalItem, calculationOptions, calculationOptions.IncrementalSetArmor, computeIncrementalSet);
            }
            else if (calculationOptions.AutomaticArmor && !calculationOptions.DisableBuffAutoActivation)
            {
                CharacterCalculationsBase mage = GetCharacterCalculations(character, additionalItem, calculationOptions, "Mage Armor", computeIncrementalSet);
                CharacterCalculationsBase molten = GetCharacterCalculations(character, additionalItem, calculationOptions, "Molten Armor", computeIncrementalSet);
                CharacterCalculationsBase calc = (mage.OverallPoints > molten.OverallPoints) ? mage : molten;
                if (computeIncrementalSet) StoreIncrementalSet(character, (CharacterCalculationsMage)calc);
                return calc;
            }
            else
            {
                CharacterCalculationsBase calc = GetCharacterCalculations(character, additionalItem, calculationOptions, null, computeIncrementalSet);
                if (computeIncrementalSet) StoreIncrementalSet(character, (CharacterCalculationsMage)calc);
                return calc;
            }
        }

        private void StoreIncrementalSet(Character character, CharacterCalculationsMage calculations)
        {
            List<string> cooldownList = new List<string>();
            List<string> spellList = new List<string>();
            List<string> segmentList = new List<string>();
            for (int i = 0; i < calculations.SolutionLabel.Length; i++)
            {
                if (calculations.Solution[i] > 0 && calculations.IncrementalSetSpell[i] != SpellId.None)
                {
                    cooldownList.Add(calculations.IncrementalSetCooldown[i].ToString(CultureInfo.InvariantCulture));
                    spellList.Add(((int)calculations.IncrementalSetSpell[i]).ToString(CultureInfo.InvariantCulture));
                    if (calculations.IncrementalSetSegment != null) segmentList.Add(calculations.IncrementalSetSegment[i].ToString(CultureInfo.InvariantCulture));
                }
            }
            character.CalculationOptions["IncrementalSetCooldowns"] = string.Join(":", cooldownList.ToArray());
            character.CalculationOptions["IncrementalSetSpells"] = string.Join(":", spellList.ToArray());
            character.CalculationOptions["IncrementalSetSegments"] = string.Join(":", segmentList.ToArray());
            character.CalculationOptions["IncrementalSetArmor"] = calculations.MageArmor;
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, CompiledCalculationOptions calculationOptions, string armor, bool computeIncrementalSet)
        {
            if (calculationOptions.SMP && !calculationOptions.SMPDisplay)
            {
                return GetCharacterCalculations_SMP(character, additionalItem, calculationOptions, armor, computeIncrementalSet);
            }
            else
            {
                return GetCharacterCalculations_Basic(character, additionalItem, calculationOptions, armor, computeIncrementalSet);
            }
        }

        public CharacterCalculationsBase GetCharacterCalculations_Basic(Character character, Item additionalItem, CompiledCalculationOptions calculationOptions, string armor, bool computeIncrementalSet)
        {
            List<string> autoActivatedBuffs = new List<string>();
            Stats rawStats = GetRawStats(character, additionalItem, calculationOptions, autoActivatedBuffs, armor);
            Stats characterStats = GetCharacterStats(character, additionalItem, rawStats, calculationOptions);

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

            if (armor == null)
            {
                if (character.ActiveBuffs.Contains("Mage Armor")) armor = "Mage Armor";
                if (character.ActiveBuffs.Contains("Molten Armor")) armor = "Molten Armor";
            }

            // temporary buffs: Arcane Power, Icy Veins, Molten Fury, Combustion?, Trinket1, Trinket2, Heroism, Destro Pot, Flame Cap, Drums?
            // compute stats for temporary bonuses, each gives a list of spells used for final LP, solutions of LP stored in calculatedStats
            List<CharacterCalculationsMage> statsList = new List<CharacterCalculationsMage>();

            CharacterCalculationsMage calculatedStats = null;

            int incrementalSetIndex = 0;
            for (int mf = 0; mf < 2; mf++)
                for (int heroism = 0; heroism < 2; heroism++)
                    for (int ap = 0; ap < 2; ap++)
                        for (int iv = 0; iv < 2; iv++)
                            for (int combustion = 0; combustion < 2; combustion++)
                                for (int drums = 0; drums < 2; drums++)
                                    for (int flameCap = 0; flameCap < 2; flameCap++)
                                        for (int destructionPotion = 0; destructionPotion < 2; destructionPotion++)
                                        {
                                            if (!calculationOptions.IncrementalOptimizations || Array.IndexOf<int>(calculationOptions.IncrementalSetCooldowns, incrementalSetIndex) >= 0)
                                            {
                                                for (int trinket1 = 0; trinket1 < 2; trinket1++)
                                                    for (int trinket2 = 0; trinket2 < 2; trinket2++)
                                                        if ((mfAvailable || mf == 1) && (heroismAvailable || heroism == 1) && (apAvailable || ap == 1) && (ivAvailable || iv == 1) && (calculationOptions.DestructionPotion || destructionPotion == 1) && (calculationOptions.FlameCap || flameCap == 1) && (trinket1Available || trinket1 == 1) && (trinket2Available || trinket2 == 1) && (combustion == 1 || calculationOptions.Combustion == 1) && (drums == 1 || calculationOptions.DrumsOfBattle))
                                                        {
                                                            if (!(trinket1 == 0 && trinket2 == 0) || (character.Trinket1.Stats.SpellDamageFor15SecOnManaGem > 0 || character.Trinket2.Stats.SpellDamageFor15SecOnManaGem > 0)) // only leave through trinkets that can stack
                                                            {
                                                                statsList.Add(GetTemporaryCharacterCalculations(characterStats, calculationOptions, armor, character, additionalItem, ap == 0, mf == 0, iv == 0, heroism == 0, destructionPotion == 0, flameCap == 0, trinket1 == 0, trinket2 == 0, combustion == 0, drums == 0, incrementalSetIndex));
                                                                if (ap != 0 && mf != 0 && iv != 0 && heroism != 0 && destructionPotion != 0 && flameCap != 0 && trinket1 != 0 && trinket2 != 0 && combustion != 0 && drums != 0)
                                                                {
                                                                    calculatedStats = statsList[statsList.Count - 1];
                                                                }
                                                            }
                                                        }
                                            }
                                            incrementalSetIndex++;
                                        }
            if (calculatedStats == null) calculatedStats = GetTemporaryCharacterCalculations(characterStats, calculationOptions, armor, character, additionalItem, false, false, false, false, false, false, false, false, false, false, incrementalSetIndex - 1);

            calculatedStats.AutoActivatedBuffs.AddRange(autoActivatedBuffs);
            calculatedStats.MageArmor = armor;

            List<SpellId> spellList = new List<SpellId>();

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
                    if (calculationOptions.ImprovedArcaneMissiles > 0) spellList.Add(SpellId.ArcaneMissiles);
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
                spellList.Add(SpellId.ABAMP);
                spellList.Add(SpellId.ABAM);
                spellList.Add(SpellId.AB3AMSc);
                spellList.Add(SpellId.ABAM3Sc);
                spellList.Add(SpellId.ABAM3Sc2);
                spellList.Add(SpellId.ABAM3FrB);
                spellList.Add(SpellId.ABAM3FrB2);
                spellList.Add(SpellId.ABFrB3FrB);
                spellList.Add(SpellId.ABFrB3FrBSc);
                spellList.Add(SpellId.ABFB3FBSc);
                //spellList.Add(SpellId.AB3Sc);
                spellList.Add(SpellId.ABAM3ScCCAM);
                spellList.Add(SpellId.ABAM3Sc2CCAM);
                spellList.Add(SpellId.ABAM3FrBCCAM);
                //spellList.Add("ABAM3FrBCCAMFail");
                spellList.Add(SpellId.ABAM3FrBScCCAM);
                spellList.Add(SpellId.ABAMCCAM);
                spellList.Add(SpellId.ABAM3CCAM);
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

            int lpRows = 42;
            int colOffset = 7;
            int lpCols = colOffset - 1 + spellList.Count * statsList.Count;
            CompactLP lp = new CompactLP(lpRows, lpCols, false);
            double[] tps = new double[lpCols];
            calculatedStats.SolutionStats = new CharacterCalculationsMage[lpCols];
            calculatedStats.SolutionSpells = new Spell[lpCols];
            calculatedStats.SolutionLabel = new string[lpCols];

            int[] incrementalSetCooldown = null;
            SpellId[] incrementalSetSpell = null;
            if (computeIncrementalSet)
            {
                incrementalSetCooldown = new int[lpCols];
                incrementalSetSpell = new SpellId[lpCols];
            }

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
                t1length = (1 + (int)((calculatedStats.FightDuration - trinket1duration) / trinket1cooldown)) * trinket1duration;
                calculatedStats.Trinket1Name = character.Trinket1.Name;
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
                t2length = (1 + (int)((calculatedStats.FightDuration - trinket2duration) / trinket2cooldown)) * trinket2duration;
                calculatedStats.Trinket2Name = character.Trinket2.Name;
            }

            calculatedStats.Trinket1Duration = trinket1duration;
            calculatedStats.Trinket1Cooldown = trinket1cooldown;
            calculatedStats.Trinket2Duration = trinket2duration;
            calculatedStats.Trinket2Cooldown = trinket2cooldown;

            combustionCount = combustionAvailable ? (1 + (int)((calculatedStats.FightDuration - 15f) / 195f)) : 0;

            int coldsnapCount = coldsnap ? (1 + (int)((calculatedStats.FightDuration - 45f) / coldsnapCooldown)) : 0;
            double coldsnapDelay = 0;
            if (ivAvailable) coldsnapDelay = 20;

            // water elemental
            if (calculationOptions.SummonWaterElemental == 1)
            {
                coldsnapDelay = 45;
                int targetLevel = calculationOptions.TargetLevel;
                calculatedStats.WaterElemental = true;
                // 45 sec, 3 min cooldown + cold snap
                // 2.5 sec Waterbolt, affected by heroism, totems, 0.4x frost damage from character
                // TODO consider adding water elemental as part of optimization for stacking with cooldowns
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
                calculatedStats.WaterElementalDuration = (float)(1 + coldsnapCount + (int)((calculatedStats.FightDuration - coldsnapCount * coldsnapDelay - 45f) / 180f)) * 45;
                if (heroismAvailable)
                    calculatedStats.WaterElementalDamage = calculatedStats.WaterElementalDps * ((calculatedStats.WaterElementalDuration - 40) + 40 * 1.3f);
                else
                    calculatedStats.WaterElementalDamage = calculatedStats.WaterElementalDuration * calculatedStats.WaterElementalDps;
            }

            // fill model [mana regen, time limit, evocation limit, mana pot limit, heroism cooldown, ap cooldown, ap+heroism cooldown, iv cooldown, mf cooldown, mf+dp cooldown, mf+iv cooldown, dp+heroism cooldown, dp+iv cooldown, flame cap cooldown, molten+flame, dp+flame, trinket1, trinket2, trinket1+mf, trinket2+mf, trinket1+heroism, trinket2+heroism, mana gem > scb, dps time, aoe duration, flamestrike, cone of cold, blast wave, dragon's breath, combustion, combustion+mf, heroism+iv, drums, drums+mf, drums+heroism, drums+iv, drums+ap, threat, pot+gem, drumsmax]
            double aplength = (1 + (int)((calculatedStats.FightDuration - 30f) / 180f)) * 15;
            double ivlength = (1 + coldsnapCount + (int)((calculatedStats.FightDuration - coldsnapCount * coldsnapDelay - 30f) / 180f)) * 20;
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

            // disable unused constraints and variables
            if (character.Ranged == null || character.Ranged.Type != Item.ItemType.Wand) lp.DisableColumn(1);
            for (int buffset = 0; buffset < statsList.Count; buffset++)
            {
                for (int spell = 0; spell < spellList.Count; spell++)
                {
                    bool viable = true;
                    if (calculationOptions.IncrementalOptimizations)
                    {
                        viable = false;
                        for (int i = 0; i < calculationOptions.IncrementalSetCooldowns.Length; i++)
                        {
                            if (statsList[buffset].IncrementalSetIndex == calculationOptions.IncrementalSetCooldowns[i] && spellList[spell] == calculationOptions.IncrementalSetSpells[i])
                            {
                                viable = true;
                                break;
                            }
                        }
                    }
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
                            int index = buffset * spellList.Count + spell + colOffset - 1;
                            lp.DisableColumn(index);
                        }
                    }
                    else
                    {
                        int index = buffset * spellList.Count + spell + colOffset - 1;
                        lp.DisableColumn(index);
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
            if (!calculationOptions.DrumsOfBattle) lp.DisableRow(41);

            lp.Compact();

            float threatFactor = (1 + characterStats.ThreatIncreaseMultiplier) * (1 - characterStats.ThreatReductionMultiplier);

            // idle regen
            calculatedStats.SolutionLabel[0] = "Idle Regen";
            lp[0, 0] = -(calculatedStats.ManaRegen * (1 - calculationOptions.Fragmentation) + calculatedStats.ManaRegen5SR * calculationOptions.Fragmentation);
            lp[1, 0] = 1;
            lp[24, 0] = -1;
            lp[lpRows, 0] = 0;
            // wand
            calculatedStats.SolutionLabel[1] = "Wand";
            if (character.Ranged != null && character.Ranged.Type == Item.ItemType.Wand)
            {
                Spell wand = new Wand(character, calculatedStats, (MagicSchool)character.Ranged.DamageType, character.Ranged.MinDamage, character.Ranged.MaxDamage, character.Ranged.Speed);
                calculatedStats.SetSpell(SpellId.Wand, wand);
                lp[0, 1] = wand.CostPerSecond - wand.ManaRegenPerSecond;
                lp[1, 1] = 1;
                lp[39, 1] = wand.ThreatPerSecond;
                lp[lpRows, 1] = wand.DamagePerSecond;
            }
            // evocation
            double evocationDuration = (8f + characterStats.EvocationExtension) / calculatedStats.CastingSpeed;
            calculatedStats.EvocationDuration = evocationDuration;
            calculatedStats.SolutionLabel[2] = "Evocation";
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
            lp[39, 2] = 0.15f * evocationMana / 2f * calculatedStats.CastingSpeed * 0.5f * threatFactor; // should split among all targets if more than one, assume one only
            lp[lpRows, 2] = 0;
            // mana pot
            calculatedStats.SolutionLabel[3] = "Mana Potion";
            calculatedStats.MaxManaPotion = 1 + (int)((calculatedStats.FightDuration - 30f) / 120f);
            lp[0, 3] = -calculatedStats.ManaRegen5SR - (1 + characterStats.BonusManaPotion) * 2400f / calculatedStats.ManaPotionTime;
            lp[1, 3] = 1;
            lp[3, 3] = 1;
            lp[39, 3] = (1 + characterStats.BonusManaPotion) * 2400f / calculatedStats.ManaPotionTime * 0.5f * threatFactor;
            lp[40, 3] = 40 / calculatedStats.ManaPotionTime;
            lp[lpRows, 3] = 0;
            // mana gem
            calculatedStats.SolutionLabel[4] = "Mana Gem";
            calculatedStats.MaxManaGem = Math.Min(5, 1 + (int)((calculatedStats.FightDuration - 30f) / 120f));
            double manaGemRegenRate = (1 + characterStats.BonusManaGem) * (-Math.Min(3, 1 + (int)((calculatedStats.FightDuration - 30f) / 120f)) * 2400f - ((calculatedStats.FightDuration >= 390) ? 1100f : 0f) - ((calculatedStats.FightDuration >= 510) ? 850 : 0)) / (calculatedStats.MaxManaGem * calculatedStats.ManaPotionTime);
            lp[0, 4] = -calculatedStats.ManaRegen5SR + manaGemRegenRate;
            lp[1, 4] = 1;
            lp[4, 4] = 1;
            lp[14, 4] = 1;
            lp[23, 4] = -1 / calculatedStats.ManaPotionTime;
            lp[39, 4] = manaGemRegenRate * 0.5f * threatFactor;
            lp[40, 4] = 40 / calculatedStats.ManaPotionTime;
            lp[lpRows, 4] = 0;
            // drums
            calculatedStats.SolutionLabel[5] = "Drums of Battle";
            lp[0, 5] = -calculatedStats.ManaRegen5SR;
            lp[1, 5] = 1;
            lp[34, 5] = -1 / calculatedStats.GlobalCooldown;
            lp[41, 5] = 1 / calculatedStats.GlobalCooldown;
            lp[lpRows, 5] = 0;
            // spells
            for (int buffset = 0; buffset < statsList.Count; buffset++)
            {
                for (int spell = 0; spell < spellList.Count; spell++)
                {
                    int index = buffset * spellList.Count + spell + colOffset - 1;
                    if (lp.IsColumnEnabled(index))
                    {
                        Spell s = statsList[buffset].GetSpell(spellList[spell]);
                        if ((s.AffectedByFlameCap || !statsList[buffset].FlameCap) && (!s.ABCycle || calculationOptions.ABCycles))
                        {
                            calculatedStats.SolutionStats[index] = statsList[buffset];
                            calculatedStats.SolutionSpells[index] = s;
                            calculatedStats.SolutionLabel[index] = ((statsList[buffset].BuffLabel.Length > 0) ? (statsList[buffset].BuffLabel + "+") : "") + s.Name;
                            if (computeIncrementalSet)
                            {
                                incrementalSetCooldown[index] = statsList[buffset].IncrementalSetIndex;
                                incrementalSetSpell[index] = spellList[spell];
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
                            lp[39, index] = s.ThreatPerSecond;
                            //lp[40, index] = (statsList[buffset].FlameCap ? 1 : 0) + (statsList[buffset].DestructionPotion ? 40.0 / 15.0 : 0);
                            lp[lpRows, index] = s.DamagePerSecond;
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
            lp[24, lpCols] = -(1 - calculationOptions.DpsTime) * calculationOptions.FightDuration;
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
            lp[40, lpCols] = ((int)((calculatedStats.FightDuration - 7800 / manaBurn) / 60f + 2)) * 40;
            lp[41, lpCols] = (1 + (int)((calculatedStats.FightDuration - 30) / 120));

            for (int col = 0; col < lpCols; col++) tps[col] = lp[39, col];

            calculatedStats.Solution = lp.Solve();

            if (computeIncrementalSet)
            {
                calculatedStats.IncrementalSetCooldown = incrementalSetCooldown;
                calculatedStats.IncrementalSetSpell = incrementalSetSpell;
            }

            calculatedStats.SubPoints[0] = ((float)calculatedStats.Solution[lpCols] + calculatedStats.WaterElementalDamage) / calculationOptions.FightDuration;
            calculatedStats.OverallPoints = calculatedStats.SubPoints[0];
            float threat = 0;
            for (int i = 0; i < lpCols; i++)
            {
                threat += (float)(tps[i] * calculatedStats.Solution[i]);
            }
            calculatedStats.Tps = threat / calculationOptions.FightDuration;

            return calculatedStats;
        }

        public CharacterCalculationsBase GetCharacterCalculations_SMP(Character character, Item additionalItem, CompiledCalculationOptions calculationOptions, string armor, bool computeIncrementalSet)
        {
            List<string> autoActivatedBuffs = new List<string>();
            Stats rawStats = GetRawStats(character, additionalItem, calculationOptions, autoActivatedBuffs, armor);
            Stats characterStats = GetCharacterStats(character, additionalItem, rawStats, calculationOptions);

            calculationOptions.SmartOptimization = true;
            double segmentDuration = 30;
            int segments = (int)Math.Ceiling(calculationOptions.FightDuration / segmentDuration);

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

            if (armor == null)
            {
                if (character.ActiveBuffs.Contains("Mage Armor")) armor = "Mage Armor";
                if (character.ActiveBuffs.Contains("Molten Armor")) armor = "Molten Armor";
            }

            // temporary buffs: Arcane Power, Icy Veins, Molten Fury, Combustion?, Trinket1, Trinket2, Heroism, Destro Pot, Flame Cap, Drums?
            // compute stats for temporary bonuses, each gives a list of spells used for final LP, solutions of LP stored in calculatedStats
            List<CharacterCalculationsMage> statsList = new List<CharacterCalculationsMage>();

            CharacterCalculationsMage calculatedStats = null;

            int incrementalSetIndex = 0;
            for (int mf = 0; mf < 2; mf++)
            for (int heroism = 0; heroism < 2; heroism++)
            for (int ap = 0; ap < 2; ap++)
            for (int iv = 0; iv < 2; iv++)
            for (int combustion = 0; combustion < 2; combustion++)
            for (int drums = 0; drums < 2; drums++)
            for (int flameCap = 0; flameCap < 2; flameCap++)
            for (int destructionPotion = 0; destructionPotion < 2; destructionPotion++)
            {
                if (!calculationOptions.IncrementalOptimizations || Array.IndexOf<int>(calculationOptions.IncrementalSetCooldowns, incrementalSetIndex) >= 0)
                {
                    for (int trinket1 = 0; trinket1 < 2; trinket1++)
                        for (int trinket2 = 0; trinket2 < 2; trinket2++)
                            if ((mfAvailable || mf == 1) && (heroismAvailable || heroism == 1) && (apAvailable || ap == 1) && (ivAvailable || iv == 1) && (calculationOptions.DestructionPotion || destructionPotion == 1) && (calculationOptions.FlameCap || flameCap == 1) && (trinket1Available || trinket1 == 1) && (trinket2Available || trinket2 == 1) && (combustion == 1 || calculationOptions.Combustion == 1) && (drums == 1 || calculationOptions.DrumsOfBattle))
                            {
                                if (!(trinket1 == 0 && trinket2 == 0) || (character.Trinket1.Stats.SpellDamageFor15SecOnManaGem > 0 || character.Trinket2.Stats.SpellDamageFor15SecOnManaGem > 0)) // only leave through trinkets that can stack
                                {
                                    statsList.Add(GetTemporaryCharacterCalculations(characterStats, calculationOptions, armor, character, additionalItem, ap == 0, mf == 0, iv == 0, heroism == 0, destructionPotion == 0, flameCap == 0, trinket1 == 0, trinket2 == 0, combustion == 0, drums == 0, incrementalSetIndex));
                                    if (ap != 0 && mf != 0 && iv != 0 && heroism != 0 && destructionPotion != 0 && flameCap != 0 && trinket1 != 0 && trinket2 != 0 && combustion != 0 && drums != 0)
                                    {
                                        calculatedStats = statsList[statsList.Count - 1];
                                    }
                                }
                            }
                }
                incrementalSetIndex++;
            }
            if (calculatedStats == null) calculatedStats = GetTemporaryCharacterCalculations(characterStats, calculationOptions, armor, character, additionalItem, false, false, false, false, false, false, false, false, false, false, incrementalSetIndex - 1);

            calculatedStats.AutoActivatedBuffs.AddRange(autoActivatedBuffs);
            calculatedStats.MageArmor = armor;

            List<SpellId> spellList = new List<SpellId>();

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

            int rowOffset = 42;
            int lpRows = rowOffset + 11 * segments; // packing constraints for each of 10 cooldowns + timing for each segment
            int colOffset = 7;
            int lpCols = colOffset - 1 + spellList.Count * statsList.Count * segments;
            CompactLP lp = new CompactLP(lpRows, lpCols, true);
            double[] tps = new double[lpCols];
            calculatedStats.SolutionStats = new CharacterCalculationsMage[lpCols];
            calculatedStats.SolutionSpells = new Spell[lpCols];
            calculatedStats.SolutionSegments = new int[lpCols];
            calculatedStats.SolutionLabel = new string[lpCols];

            int[] incrementalSetCooldown = null;
            SpellId[] incrementalSetSpell = null;
            int[] incrementalSetSegment = null;
            if (computeIncrementalSet)
            {
                incrementalSetCooldown = new int[lpCols];
                incrementalSetSpell = new SpellId[lpCols];
                incrementalSetSegment = new int[lpCols];
            }

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
                t1length = (1 + (int)((calculatedStats.FightDuration - trinket1duration) / trinket1cooldown)) * trinket1duration;
                calculatedStats.Trinket1Name = character.Trinket1.Name;
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
                t2length = (1 + (int)((calculatedStats.FightDuration - trinket2duration) / trinket2cooldown)) * trinket2duration;
                calculatedStats.Trinket2Name = character.Trinket2.Name;
            }

            calculatedStats.Trinket1Duration = trinket1duration;
            calculatedStats.Trinket1Cooldown = trinket1cooldown;
            calculatedStats.Trinket2Duration = trinket2duration;
            calculatedStats.Trinket2Cooldown = trinket2cooldown;

            combustionCount = combustionAvailable ? (1 + (int)((calculatedStats.FightDuration - 15f) / 195f)) : 0;

            int coldsnapCount = coldsnap ? (1 + (int)((calculatedStats.FightDuration - 45f) / coldsnapCooldown)) : 0;
            double coldsnapDelay = 0;
            if (ivAvailable) coldsnapDelay = 20;

            // water elemental
            if (calculationOptions.SummonWaterElemental == 1)
            {
                coldsnapDelay = 45;
                int targetLevel = calculationOptions.TargetLevel;
                calculatedStats.WaterElemental = true;
                // 45 sec, 3 min cooldown + cold snap
                // 2.5 sec Waterbolt, affected by heroism, totems, 0.4x frost damage from character
                // TODO consider adding water elemental as part of optimization for stacking with cooldowns
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
                calculatedStats.WaterElementalDuration = (float)(1 + coldsnapCount + (int)((calculatedStats.FightDuration - coldsnapCount * coldsnapDelay - 45f) / 180f)) * 45;
                if (heroismAvailable)
                    calculatedStats.WaterElementalDamage = calculatedStats.WaterElementalDps * ((calculatedStats.WaterElementalDuration - 40) + 40 * 1.3f);
                else
                    calculatedStats.WaterElementalDamage = calculatedStats.WaterElementalDuration * calculatedStats.WaterElementalDps;
            }

            // fill model [mana regen, time limit, evocation limit, mana pot limit, heroism cooldown, ap cooldown, ap+heroism cooldown, iv cooldown, mf cooldown, mf+dp cooldown, mf+iv cooldown, dp+heroism cooldown, dp+iv cooldown, flame cap cooldown, molten+flame, dp+flame, trinket1, trinket2, trinket1+mf, trinket2+mf, trinket1+heroism, trinket2+heroism, mana gem > scb, dps time, aoe duration, flamestrike, cone of cold, blast wave, dragon's breath, combustion, combustion+mf, heroism+iv, drums, drums+mf, drums+heroism, drums+iv, drums+ap, threat, pot+gem, drumsmax]
            double aplength = (1 + (int)((calculatedStats.FightDuration - 30f) / 180f)) * 15;
            double ivlength = (1 + coldsnapCount + (int)((calculatedStats.FightDuration - coldsnapCount * coldsnapDelay - 30f) / 180f)) * 20;
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

            // disable unused constraints and variables
            if (character.Ranged == null || character.Ranged.Type != Item.ItemType.Wand) lp.DisableColumn(1);
            for (int seg = 0; seg < segments; seg++)
            {
                for (int buffset = 0; buffset < statsList.Count; buffset++)
                {
                    for (int spell = 0; spell < spellList.Count; spell++)
                    {
                        bool viable = true;
                        if (calculationOptions.IncrementalOptimizations)
                        {
                            viable = false;
                            for (int i = 0; i < calculationOptions.IncrementalSetCooldowns.Length; i++)
                            {
                                if (seg == calculationOptions.IncrementalSetSegments[i] && statsList[buffset].IncrementalSetIndex == calculationOptions.IncrementalSetCooldowns[i] && spellList[spell] == calculationOptions.IncrementalSetSpells[i])
                                {
                                    viable = true;
                                    break;
                                }
                            }
                        }
                        if (statsList[buffset].MoltenFury && (seg + 1) * segmentDuration <= calculationOptions.FightDuration - mflength) viable = false;
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
            if (!calculationOptions.DrumsOfBattle) lp.DisableRow(41);
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

            lp.Compact();

            float threatFactor = (1 + characterStats.ThreatIncreaseMultiplier) * (1 - characterStats.ThreatReductionMultiplier);

            // idle regen
            calculatedStats.SolutionLabel[0] = "Idle Regen";
            lp[0, 0] = -(calculatedStats.ManaRegen * (1 - calculationOptions.Fragmentation) + calculatedStats.ManaRegen5SR * calculationOptions.Fragmentation);
            lp[1, 0] = 1;
            lp[24, 0] = -1;
            lp[lpRows, 0] = 0;
            // wand
            calculatedStats.SolutionLabel[1] = "Wand";
            if (character.Ranged != null && character.Ranged.Type == Item.ItemType.Wand)
            {
                Spell wand = new Wand(character, calculatedStats, (MagicSchool)character.Ranged.DamageType, character.Ranged.MinDamage, character.Ranged.MaxDamage, character.Ranged.Speed);
                calculatedStats.SetSpell(SpellId.Wand, wand);
                lp[0, 1] = wand.CostPerSecond - wand.ManaRegenPerSecond;
                lp[1, 1] = 1;
                lp[39, 1] = wand.ThreatPerSecond;
                lp[lpRows, 1] = wand.DamagePerSecond;
            }
            // evocation
            double evocationDuration = (8f + characterStats.EvocationExtension) / calculatedStats.CastingSpeed;
            calculatedStats.EvocationDuration = evocationDuration;
            calculatedStats.SolutionLabel[2] = "Evocation";
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
            lp[39, 2] = 0.15f * evocationMana / 2f * calculatedStats.CastingSpeed * 0.5f * threatFactor; // should split among all targets if more than one, assume one only
            lp[lpRows, 2] = 0;
            // mana pot
            calculatedStats.SolutionLabel[3] = "Mana Potion";
            calculatedStats.MaxManaPotion = 1 + (int)((calculatedStats.FightDuration - 30f) / 120f);
            lp[0, 3] = -calculatedStats.ManaRegen5SR - (1 + characterStats.BonusManaPotion) * 2400f / calculatedStats.ManaPotionTime;
            lp[1, 3] = 1;
            lp[3, 3] = 1;
            lp[39, 3] = (1 + characterStats.BonusManaPotion) * 2400f / calculatedStats.ManaPotionTime * 0.5f * threatFactor;
            lp[40, 3] = 40 / calculatedStats.ManaPotionTime;
            lp[lpRows, 3] = 0;
            // mana gem
            calculatedStats.SolutionLabel[4] = "Mana Gem";
            calculatedStats.MaxManaGem = Math.Min(5, 1 + (int)((calculatedStats.FightDuration - 30f) / 120f));
            double manaGemRegenRate = (1 + characterStats.BonusManaGem) * (-Math.Min(3, 1 + (int)((calculatedStats.FightDuration - 30f) / 120f)) * 2400f - ((calculatedStats.FightDuration >= 390) ? 1100f : 0f) - ((calculatedStats.FightDuration >= 510) ? 850 : 0)) / (calculatedStats.MaxManaGem * calculatedStats.ManaPotionTime);
            lp[0, 4] = -calculatedStats.ManaRegen5SR + manaGemRegenRate;
            lp[1, 4] = 1;
            lp[4, 4] = 1;
            lp[14, 4] = 1;
            lp[23, 4] = - 1 / calculatedStats.ManaPotionTime;
            lp[39, 4] = manaGemRegenRate * 0.5f * threatFactor;
            lp[40, 4] = 40 / calculatedStats.ManaPotionTime;
            lp[lpRows, 4] = 0;
            // drums
            calculatedStats.SolutionLabel[5] = "Drums of Battle";
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
                                calculatedStats.SolutionSegments[index] = seg;
                                calculatedStats.SolutionLabel[index] = ((statsList[buffset].BuffLabel.Length > 0) ? (statsList[buffset].BuffLabel + "+") : "") + s.Name;
                                if (computeIncrementalSet)
                                {
                                    incrementalSetCooldown[index] = statsList[buffset].IncrementalSetIndex;
                                    incrementalSetSpell[index] = spellList[spell];
                                    incrementalSetSegment[index] = seg;
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
                                lp[39, index] = s.ThreatPerSecond;
                                //lp[40, index] = (statsList[buffset].FlameCap ? 1 : 0) + (statsList[buffset].DestructionPotion ? 40.0 / 15.0 : 0);
                                lp[lpRows, index] = s.DamagePerSecond;
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
                                        int maxs = (int)Math.Ceiling(ss + cool / segmentDuration);
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                        if (seg >= ss && seg <= maxs) lp[rowOffset + 2 * segments + ss, index] = 1;
                                    }
                                }
                                if (statsList[buffset].IcyVeins)
                                {
                                    for (int ss = 0; ss < segments; ss++)
                                    {
                                        double cool = 180 + (coldsnap ? 20 : 0);
                                        int maxs = (int)Math.Ceiling(ss + cool / segmentDuration);
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                        if (seg >= ss && seg <= maxs) lp[rowOffset + 3 * segments + ss, index] = 1;
                                    }
                                }
                                if (statsList[buffset].Combustion)
                                {
                                    for (int ss = 0; ss < segments; ss++)
                                    {
                                        double cool = 180 + 15;
                                        int maxs = (int)Math.Ceiling(ss + cool / segmentDuration);
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                        if (seg >= ss && seg <= maxs) lp[rowOffset + 4 * segments + ss, index] = 1 / (statsList[buffset].CombustionDuration * s.CastTime / s.CastProcs);
                                    }
                                }
                                if (statsList[buffset].DrumsOfBattle)
                                {
                                    for (int ss = 0; ss < segments; ss++)
                                    {
                                        double cool = 120;
                                        int maxs = (int)Math.Ceiling(ss + cool / segmentDuration);
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                        if (seg >= ss && seg <= maxs) lp[rowOffset + 5 * segments + ss, index] = 1;
                                    }
                                }
                                if (statsList[buffset].FlameCap)
                                {
                                    for (int ss = 0; ss < segments; ss++)
                                    {
                                        double cool = 180;
                                        int maxs = (int)Math.Ceiling(ss + cool / segmentDuration);
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                        if (seg >= ss && seg <= maxs) lp[rowOffset + 6 * segments + ss, index] = 1;
                                    }
                                }
                                if (statsList[buffset].DestructionPotion)
                                {
                                    for (int ss = 0; ss < segments; ss++)
                                    {
                                        double cool = 120;
                                        int maxs = (int)Math.Ceiling(ss + cool / segmentDuration);
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                        if (seg >= ss && seg <= maxs) lp[rowOffset + 7 * segments + ss, index] = 1;
                                    }
                                }
                                if (statsList[buffset].Trinket1)
                                {
                                    for (int ss = 0; ss < segments; ss++)
                                    {
                                        double cool = trinket1cooldown;
                                        int maxs = (int)Math.Ceiling(ss + cool / segmentDuration);
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                        if (seg >= ss && seg <= maxs) lp[rowOffset + 8 * segments + ss, index] = 1;
                                    }
                                }
                                if (statsList[buffset].Trinket2)
                                {
                                    for (int ss = 0; ss < segments; ss++)
                                    {
                                        double cool = trinket2cooldown;
                                        int maxs = (int)Math.Ceiling(ss + cool / segmentDuration);
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
            lp[40, lpCols] = ((int)((calculatedStats.FightDuration - 7800 / manaBurn) / 60f + 2)) * 40;
            lp[41, lpCols] = (1 + (int)((calculatedStats.FightDuration - 30) / 120));

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

            for (int col = 0; col < lpCols; col++) tps[col] = lp[39, col];

            Heap<CompactLP> heap = new Heap<CompactLP>(HeapType.MaximumHeap);
            heap.Push(lp);

            double max = lp.Value;

            bool valid = true;
            do
            {
                if (heap.Head.Value > max + 0.000001)
                {
                    System.Windows.Forms.MessageBox.Show("Instability detected, aborting SMP algorithm");
                    // find something reasonably stable
                    while (heap.Count > 0 && (lp = heap.Pop()).Value > max + 0.000001) { }
                    break;
                }
                lp = heap.Pop();
                //max = lp.Value; instability fix?
                // this is the best non-evaluated option (highest partially-constrained LP, the optimum has to be lower)
                // if this one is valid than all others are sub-optimal
                // validate all segments for each cooldown
                double[] solution = lp.Solve();
                valid = true;
                for (int seg = 0; seg < segments; seg++)
                {
                    // mf is trivially satisfied
                    // heroism
                    double inseg = 0;
                    for (int index = seg * statsList.Count * spellList.Count + colOffset - 1; index < (seg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                    {
                        CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                        if (stats != null && stats.Heroism) inseg += solution[index];
                    }
                    if (inseg > 0)
                    {
                        double duration = 40;
                        int mindist = (int)Math.Ceiling(duration / segmentDuration);
                        // verify that outside duration segments are 0
                        valid = true;
                        for (int outseg = 0; outseg < segments; outseg++)
                        {
                            if (Math.Abs(outseg - seg) > mindist)
                            {
                                for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                                {
                                    CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                    if (stats != null && stats.Heroism && solution[index] > 0)
                                    {
                                        valid = false;
                                        goto breakHeroism;
                                    }
                                }
                            }
                        }
                    breakHeroism:
                        if (!valid)
                        {
                            // branch on whether cooldown is used in this segment
                            CompactLP cooldownUsed = lp.Clone();
                            // cooldown not used
                            lp.HeroismHash += 1 << seg;
                            for (int index = seg * statsList.Count * spellList.Count + colOffset - 1; index < (seg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                            {
                                CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                if (stats != null && stats.Heroism) lp.EraseColumn(index);
                            }
                            heap.Push(lp);
                            // cooldown used
                            for (int outseg = 0; outseg < segments; outseg++)
                            {
                                if (Math.Abs(outseg - seg) > mindist)
                                {
                                    cooldownUsed.HeroismHash += 1 << outseg;
                                    for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                                    {
                                        CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                        if (stats != null && stats.Heroism) cooldownUsed.EraseColumn(index);
                                    }
                                }
                            }
                            heap.Push(cooldownUsed);
                            break;
                        }
                    }
                    // ap
                    inseg = 0;
                    for (int index = seg * statsList.Count * spellList.Count + colOffset - 1; index < (seg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                    {
                        CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                        if (stats != null && stats.ArcanePower) inseg += solution[index];
                    }
                    if (inseg > 0)
                    {
                        double duration = 15;
                        double cool = 180;
                        int mindist = (int)Math.Ceiling(duration / segmentDuration);
                        int maxdist = (int)Math.Floor((cool - duration) / segmentDuration);
                        // verify that outside duration segments are 0
                        valid = true;
                        for (int outseg = 0; outseg < segments; outseg++)
                        {
                            if (Math.Abs(outseg - seg) > mindist && Math.Abs(outseg - seg) < maxdist)
                            {
                                for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                                {
                                    CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                    if (stats != null && stats.ArcanePower && solution[index] > 0)
                                    {
                                        valid = false;
                                        goto breakAP;
                                    }
                                }
                            }
                        }
                    breakAP:
                        if (!valid)
                        {
                            // branch on whether cooldown is used in this segment
                            CompactLP cooldownUsed = lp.Clone();
                            // cooldown not used
                            lp.APHash += 1 << seg;
                            for (int index = seg * statsList.Count * spellList.Count + colOffset - 1; index < (seg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                            {
                                CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                if (stats != null && stats.ArcanePower) lp.EraseColumn(index);
                            }
                            heap.Push(lp);
                            // cooldown used
                            for (int outseg = 0; outseg < segments; outseg++)
                            {
                                if (Math.Abs(outseg - seg) > mindist && Math.Abs(outseg - seg) < maxdist)
                                {
                                    cooldownUsed.APHash += 1 << outseg;
                                    for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                                    {
                                        CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                        if (stats != null && stats.ArcanePower) cooldownUsed.EraseColumn(index);
                                    }
                                }
                            }
                            heap.Push(cooldownUsed);
                            break;
                        }
                    }
                    // iv
                    inseg = 0;
                    for (int index = seg * statsList.Count * spellList.Count + colOffset - 1; index < (seg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                    {
                        CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                        if (stats != null && stats.IcyVeins) inseg += solution[index];
                    }
                    if (inseg > 0)
                    {
                        double duration = 20 + (coldsnap ? 20 : 0);
                        double cool = 180 + (coldsnap ? 20 : 0);
                        int mindist = (int)Math.Ceiling(duration / segmentDuration);
                        int maxdist = (int)Math.Floor((cool - duration) / segmentDuration);
                        // verify that outside duration segments are 0
                        valid = true;
                        for (int outseg = 0; outseg < segments; outseg++)
                        {
                            if (Math.Abs(outseg - seg) > mindist && Math.Abs(outseg - seg) < maxdist)
                            {
                                for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                                {
                                    CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                    if (stats != null && stats.IcyVeins && solution[index] > 0)
                                    {
                                        valid = false;
                                        goto breakIV;
                                    }
                                }
                            }
                        }
                    breakIV:
                        if (!valid)
                        {
                            // branch on whether cooldown is used in this segment
                            CompactLP cooldownUsed = lp.Clone();
                            // cooldown not used
                            lp.IVHash += 1 << seg;
                            for (int index = seg * statsList.Count * spellList.Count + colOffset - 1; index < (seg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                            {
                                CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                if (stats != null && stats.IcyVeins) lp.EraseColumn(index);
                            }
                            heap.Push(lp);
                            // cooldown used
                            for (int outseg = 0; outseg < segments; outseg++)
                            {
                                if (Math.Abs(outseg - seg) > mindist && Math.Abs(outseg - seg) < maxdist)
                                {
                                    cooldownUsed.IVHash += 1 << outseg;
                                    for (int index = outseg * statsList.Count * spellList.Count + colOffset - 1; index < (outseg + 1) * statsList.Count * spellList.Count + colOffset - 1; index++)
                                    {
                                        CharacterCalculationsMage stats = calculatedStats.SolutionStats[index];
                                        if (stats != null && stats.IcyVeins) cooldownUsed.EraseColumn(index);
                                    }
                                }
                            }
                            heap.Push(cooldownUsed);
                            break;
                        }
                    }
                }
            } while (heap.Count > 0 && !valid);

            calculatedStats.Solution = lp.Solve();
            if (computeIncrementalSet)
            {
                calculatedStats.IncrementalSetCooldown = incrementalSetCooldown;
                calculatedStats.IncrementalSetSpell = incrementalSetSpell;
                calculatedStats.IncrementalSetSegment = incrementalSetSegment;
            }

            calculatedStats.SubPoints[0] = ((float)calculatedStats.Solution[lpCols] + calculatedStats.WaterElementalDamage) / calculationOptions.FightDuration;
            calculatedStats.OverallPoints = calculatedStats.SubPoints[0];
            float threat = 0;
            for (int i = 0; i < lpCols; i++)
            {
                threat += (float)(tps[i] * calculatedStats.Solution[i]);
            }
            calculatedStats.Tps = threat / calculationOptions.FightDuration;

            return calculatedStats;
        }

        public double[] LPSolve(double[,] data, int rows, int cols)
        {
            double[,] a = data;
            int[] XN;
            int[] XB;
            
            bool feasible;
            int i, j, r, c, t;
            double v, bestv;

            bestv = 0;
            c = 0;
            r = 0;
            
            XN = new int[cols];
            XB = new int[rows];
            
            for (i = 0; i < rows; i++)
                XB[i] = cols + i;
            for (j = 0; j < cols; j++)
                XN[j] = j;

            int round = 0;

            do
            {
                feasible = true;
                // check feasibility
                for (i = 0; i < rows; i++)
                {
                    if (a[i, cols] < 0)
                    {
                        feasible = false;
                        bestv = 0;
                        for (j = 0; j < cols; j++)
                        {
                            if (a[i, j] < bestv)
                            {
                                bestv = a[i, j];
                                c = j;
                            }
                        }
                        break;
                    }
                }
                if (feasible)
                {
                    // standard problem
                    bestv = 0;
                    for (j = 0; j < cols; j++)
                    {
                        if (a[rows, j] > bestv)
                        {
                            bestv = a[rows, j];
                            c = j;
                        }
                    }
                }
                if (bestv == 0) break;
                bestv = -1;
                for (i = 0; i < rows; i++)
                {
                    if (a[i, c] > 0)
                    {
                        v = a[i, cols] / a[i, c];
                        if (bestv == -1 || v < bestv)
                        {
                            bestv = v;
                            r = i;
                        }
                    }
                }
                if (bestv == -1) break;
                v = a[r, c];
                a[r, c] = 1;
                for (j = 0; j <= cols; j++)
                {
                    a[r, j] = a[r, j] / v;
                }
                for (i = 0; i <= rows; i++)
                {
                    if (i != r)
                    {
                        v = a[i, c];
                        a[i, c] = 0;
                        for (j = 0; j <= cols; j++)
                        {
                            a[i, j] = a[i, j] - a[r, j] * v;
                            if (a[i, j] < 0.00000000001 && a[i, j] > -0.00000000001) a[i, j] = 0; // compensate for floating point errors
                        }
                    }
                }
                t = XN[c];
                XN[c] = XB[r];
                XB[r] = t;
                round++;
            } while (round < 5000); // fail safe for infinite loops caused by floating point instability
            
            double[] ret = new double[cols + 1];
            for (i = 0; i < rows; i++)
            {
                if (XB[i] < cols) ret[XB[i]] = a[i, cols];
            }
            ret[cols] = -a[rows, cols];

            return ret;
        }

        static unsafe double[] LPSolveUnsafe(double[,] data, int rows, int cols)
        {
            double[] ret = new double[cols + 1];
            int[] xn = new int[cols + 1];
            int[] xb = new int[rows + 1];
            if (cols > 30000) return ret; // prevent unstable solutions

            double* ai, aij, arows;

            fixed (double* a = data)
            {
                fixed (int* XN = xn, XB = xb)
                {
                    arows = a + rows * (cols + 1);

                    bool feasible;
                    int i, j, r, c, t;
                    double v, bestv;

                    bestv = 0;
                    c = 0;
                    r = 0;

                    for (i = 0; i < rows; i++)
                        XB[i] = cols + i;
                    for (j = 0; j < cols; j++)
                        XN[j] = j;

                    int round = 0;

                    do
                    {
                        feasible = true;
                        // check feasibility
                        for (i = 0, ai = a; i < rows; i++, ai += (cols + 1))
                        {
                            if (ai[cols] < 0)
                            {
                                feasible = false;
                                bestv = 0;
                                for (j = 0, aij = ai; j < cols; j++, aij++)
                                {
                                    if (*aij < bestv)
                                    {
                                        bestv = *aij;
                                        c = j;
                                    }
                                }
                                break;
                            }
                        }
                        if (feasible)
                        {
                            // standard problem
                            bestv = 0;
                            for (j = 0, aij = arows; j < cols; j++, aij++)
                            {
                                if (*aij > bestv)
                                {
                                    bestv = *aij;
                                    c = j;
                                }
                            }
                        }
                        if (bestv == 0) break;
                        bestv = -1;
                        for (i = 0, ai = a; i < rows; i++, ai += (cols + 1))
                        {
                            if (ai[c] > 0)
                            {
                                v = ai[cols] / ai[c];
                                if (bestv == -1 || v < bestv)
                                {
                                    if (ai[c] > 0.0000000001)
                                    {
                                        bestv = v;
                                        r = i;
                                    }
                                    else
                                    {
                                        ai[c] = 0;
                                    }
                                }
                            }
                        }
                        if (bestv == -1) break;
                        aij = a + r * (cols + 1) + c;
                        v = *aij;
                        *aij = 1;
                        ai = a + r * (cols + 1);
                        for (j = 0, aij = ai; j <= cols; j++, aij++)
                        {
                            *aij /= v;
                        }
                        for (i = 0, ai = a; i <= rows; i++, ai += (cols + 1))
                        {
                            if (i != r)
                            {
                                v = ai[c];
                                ai[c] = 0;
                                for (j = 0, aij = ai; j <= cols; j++, aij++)
                                {
                                    *aij -= a[r * (cols + 1) + j] * v;
                                    if (*aij < 0.00000000001 && *aij > -0.00000000001) *aij = 0; // compensate for floating point errors
                                }
                            }
                        }
                        //System.Diagnostics.Debug.WriteLine(round + ": " + XN[c] + " <=> " + XB[r]);
                        t = XN[c];
                        XN[c] = XB[r];
                        XB[r] = t;
                        round++;
                        if (round == 5000) round++;
                    } while (round < 5000); // fail safe for infinite loops caused by floating point instability

                    for (i = 0; i < rows; i++)
                    {
                        if (XB[i] < cols) ret[XB[i]] = a[i * (cols + 1) + cols];
                    }
                    ret[cols] = -a[rows * (cols + 1) + cols];
                }
            }
            return ret;
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

        public CharacterCalculationsMage GetTemporaryCharacterCalculations(Stats characterStats, CompiledCalculationOptions calculationOptions, string armor, Character character, Item additionalItem, bool arcanePower, bool moltenFury, bool icyVeins, bool heroism, bool destructionPotion, bool flameCap, bool trinket1, bool trinket2, bool combustion, bool drums, int incrementalSetIndex)
        {
            CharacterCalculationsMage calculatedStats = new CharacterCalculationsMage();
            Stats stats = characterStats.Clone();
            calculatedStats.IncrementalSetIndex = incrementalSetIndex;
            calculatedStats.BasicStats = stats;
            calculatedStats.Character = character;
            calculatedStats.CalculationOptions = calculationOptions;

            float levelScalingFactor = (1 - (70 - 60) / 82f * 3);

            stats.SpellDamageRating += stats.SpellDamageFromIntellectPercentage * stats.Intellect;
            stats.SpellDamageRating += stats.SpellDamageFromSpiritPercentage * stats.Spirit;

            if (destructionPotion) stats.SpellDamageRating += 120;
            if (flameCap) stats.SpellFireDamageRating += 80;

            if (trinket1)
            {
                Stats t = character.Trinket1.Stats;
                stats.SpellDamageRating += t.SpellDamageFor20SecOnUse2Min + t.SpellDamageFor15SecOnManaGem + t.SpellDamageFor15SecOnUse90Sec;
                stats.SpellHasteRating += t.SpellHasteFor20SecOnUse2Min + t.SpellHasteFor20SecOnUse5Min;
                calculatedStats.Mp5OnCastFor20Sec = t.Mp5OnCastFor20SecOnUse2Min;
            }
            if (trinket2)
            {
                Stats t = character.Trinket2.Stats;
                stats.SpellDamageRating += t.SpellDamageFor20SecOnUse2Min + t.SpellDamageFor15SecOnManaGem + t.SpellDamageFor15SecOnUse90Sec;
                stats.SpellHasteRating += t.SpellHasteFor20SecOnUse2Min + t.SpellHasteFor20SecOnUse5Min;
                calculatedStats.Mp5OnCastFor20Sec = t.Mp5OnCastFor20SecOnUse2Min;
            }
            if (drums)
            {
                stats.SpellHasteRating += 80;
            }

            calculatedStats.CastingSpeed = 1 + stats.SpellHasteRating / 995f * levelScalingFactor;
            calculatedStats.ArcaneDamage = stats.SpellArcaneDamageRating + stats.SpellDamageRating;
            calculatedStats.FireDamage = stats.SpellFireDamageRating + stats.SpellDamageRating;
            calculatedStats.FrostDamage = stats.SpellFrostDamageRating + stats.SpellDamageRating;
            calculatedStats.NatureDamage = /* stats.SpellNatureDamageRating + */ stats.SpellDamageRating;
            calculatedStats.ShadowDamage = stats.SpellShadowDamageRating + stats.SpellDamageRating;

            calculatedStats.SpellCrit = 0.01f * (stats.Intellect * 0.0125f + 0.9075f) + 0.01f * calculationOptions.ArcaneInstability + 0.01f * calculationOptions.ArcanePotency + stats.SpellCritRating / 1400f * levelScalingFactor + stats.MageSpellCrit;
            if (destructionPotion) calculatedStats.SpellCrit += 0.02f;
            calculatedStats.SpellHit = stats.SpellHitRating * levelScalingFactor / 800f;

            int targetLevel = calculationOptions.TargetLevel;
            calculatedStats.ArcaneHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit + 0.02f * calculationOptions.ArcaneFocus);
            calculatedStats.FireHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit + 0.01f * calculationOptions.ElementalPrecision);
            calculatedStats.FrostHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit + 0.01f * calculationOptions.ElementalPrecision);
            calculatedStats.NatureHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit);
            calculatedStats.ShadowHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit);

            calculatedStats.SpiritRegen = 0.001f + stats.Spirit * 0.009327f * (float)Math.Sqrt(stats.Intellect);
            calculatedStats.ManaRegen = calculatedStats.SpiritRegen + stats.Mp5 / 5f + calculatedStats.SpiritRegen * 4 * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * stats.Mana / calculationOptions.FightDuration;
            calculatedStats.ManaRegen5SR = calculatedStats.SpiritRegen * stats.SpellCombatManaRegeneration + stats.Mp5 / 5f + calculatedStats.SpiritRegen * (5 - stats.SpellCombatManaRegeneration) * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * stats.Mana / calculationOptions.FightDuration;
            calculatedStats.ManaRegenDrinking = calculatedStats.ManaRegen + 240f;
            calculatedStats.HealthRegen = 0.0312f * stats.Spirit + stats.Hp5 / 5f;
            calculatedStats.HealthRegenCombat = stats.Hp5 / 5f;
            calculatedStats.HealthRegenEating = calculatedStats.ManaRegen + 250f;
            calculatedStats.MeleeMitigation = (1 - 1 / (1 + 0.1f * stats.Armor / (8.5f * (70 + 4.5f * (70 - 59)) + 40)));
            calculatedStats.Defense = 350 + stats.DefenseRating / 2.37f;
            int molten = (armor == "Molten Armor") ? 1 : 0;
            calculatedStats.PhysicalCritReduction = (0.04f * (calculatedStats.Defense - 5 * 70) / 100 + stats.Resilience / 2500f * levelScalingFactor + molten * 0.05f);
            calculatedStats.SpellCritReduction = (stats.Resilience / 2500f * levelScalingFactor + molten * 0.05f);
            calculatedStats.CritDamageReduction = (stats.Resilience / 2500f * 2f * levelScalingFactor);
            calculatedStats.Dodge = ((0.0443f * stats.Agility + 3.28f + 0.04f * (calculatedStats.Defense - 5 * 70)) / 100f + stats.DodgeRating / 1200 * levelScalingFactor);

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

            List<String> buffList = new List<string> ();
            if (moltenFury) buffList.Add("Molten Fury");
            if (heroism) buffList.Add("Heroism");
            if (icyVeins) buffList.Add("Icy Veins");
            if (arcanePower) buffList.Add("Arcane Power");
            if (combustion) buffList.Add("Combustion");
            if (drums) buffList.Add("Drums of Battle");
            if (flameCap) buffList.Add("Flame Cap");
            if (trinket1) buffList.Add(character.Trinket1.Name);
            if (trinket2) buffList.Add(character.Trinket2.Name);
            if (destructionPotion) buffList.Add("Destruction Potion");

            calculatedStats.BuffLabel = string.Join("+", buffList.ToArray());

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

            calculatedStats.ArcaneSpellModifier = (1 + 0.01f * calculationOptions.ArcaneInstability) * (1 + 0.01f * calculationOptions.PlayingWithFire) * (1 + stats.BonusSpellPowerMultiplier);
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
            calculatedStats.ArcaneSpellModifier *= (1 + stats.BonusArcaneSpellPowerMultiplier);
            calculatedStats.FireSpellModifier *= (1 + stats.BonusFireSpellPowerMultiplier);
            calculatedStats.FrostSpellModifier *= (1 + stats.BonusFrostSpellPowerMultiplier);
            calculatedStats.NatureSpellModifier *= (1 + stats.BonusNatureSpellPowerMultiplier);
            calculatedStats.ShadowSpellModifier *= (1 + stats.BonusShadowSpellPowerMultiplier);

            calculatedStats.ResilienceCritDamageReduction = 1;
            calculatedStats.ResilienceCritRateReduction = 0;

            calculatedStats.ArcaneCritBonus = (1 + (1.5f * (1 + stats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * calculationOptions.SpellPower)) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.FireCritBonus = (1 + (1.5f * (1 + stats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * calculationOptions.SpellPower)) * (1 + 0.08f * calculationOptions.Ignite) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.FrostCritBonus = (1 + (1.5f * (1 + stats.BonusSpellCritMultiplier) - 1) * (1 + 0.2f * calculationOptions.IceShards + 0.25f * calculationOptions.SpellPower)) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.NatureCritBonus = (1 + (1.5f * (1 + stats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * calculationOptions.SpellPower)) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.ShadowCritBonus = (1 + (1.5f * (1 + stats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * calculationOptions.SpellPower)) * calculatedStats.ResilienceCritDamageReduction;

            calculatedStats.ArcaneCritRate = calculatedStats.SpellCrit;
            calculatedStats.FireCritRate = calculatedStats.SpellCrit + 0.02f * calculationOptions.CriticalMass + 0.01f * calculationOptions.Pyromaniac;
            if (combustion)
            {
                calculatedStats.CombustionDuration = Combustion(calculatedStats.FireCritRate);
                calculatedStats.FireCritRate = 3 / calculatedStats.CombustionDuration;
            }
            calculatedStats.FrostCritRate = calculatedStats.SpellCrit + stats.SpellFrostCritRating / 22.08f / 100f;
            calculatedStats.NatureCritRate = calculatedStats.SpellCrit;
            calculatedStats.ShadowCritRate = calculatedStats.SpellCrit;

            float threatFactor = (1 + stats.ThreatIncreaseMultiplier) * (1 - stats.ThreatReductionMultiplier);

            calculatedStats.ArcaneThreatMultiplier = threatFactor * (1 - calculationOptions.ArcaneSubtlety * 0.2f);
            calculatedStats.FireThreatMultiplier = threatFactor * (1 - calculationOptions.BurningSoul * 0.05f);
            calculatedStats.FrostThreatMultiplier = threatFactor * (1 - ((calculationOptions.FrostChanneling > 0) ? (0.01f + 0.03f * calculationOptions.FrostChanneling) : 0f));
            calculatedStats.NatureThreatMultiplier = threatFactor;
            calculatedStats.ShadowThreatMultiplier = threatFactor;

            return calculatedStats;
        }

        private Stats GetRawStats(Character character, Item additionalItem, CompiledCalculationOptions calculationOptions, List<string> autoActivatedBuffs, string armor)
        {
            Stats stats = new Stats();
            AccumulateItemStats(stats, character, additionalItem);
            AccumulateEnchantsStats(stats, character);
            List<string> activeBuffs = new List<string>();
            activeBuffs.AddRange(character.ActiveBuffs);

            if (!calculationOptions.DisableBuffAutoActivation)
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
            CompiledCalculationOptions calculationOptions = new CompiledCalculationOptions(character);
            return GetCharacterStats(character, additionalItem, GetRawStats(character, additionalItem, calculationOptions, new List<string>(), null), calculationOptions);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats rawStats, CompiledCalculationOptions calculationOptions)
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

            int magicAbsorption = 2 * calculationOptions.MagicAbsorption;
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

            return statsTotal;
        }

        private static string[] ArmorList = new string[] { "Mage", "Molten", "Ice" };
        private static string[] TalentList = { "ArcaneSubtlety", "ArcaneFocus", "ImprovedArcaneMissiles", "WandSpecialization", "MagicAbsorption", "ArcaneConcentration", "MagicAttunement", "ArcaneImpact", "ArcaneFortitude", "ImprovedManaShield", "ImprovedCounterspell", "ArcaneMeditation", "ImprovedBlink", "PresenceOfMind", "ArcaneMind", "PrismaticCloak", "ArcaneInstability", "ArcanePotency", "EmpoweredArcaneMissiles", "ArcanePower", "SpellPower", "MindMastery", "Slow", "ImprovedFireball", "Impact", "Ignite", "FlameThrowing", "ImprovedFireBlast", "Incinerate", "ImprovedFlamestrike", "Pyroblast", "BurningSoul", "ImprovedScorch", "ImprovedFireWard", "MasterOfElements", "PlayingWithFire", "CriticalMass", "BlastWave", "BlazingSpeed", "FirePower", "Pyromaniac", "Combustion", "MoltenFury", "EmpoweredFireball", "DragonsBreath", "FrostWarding", "ImprovedFrostbolt", "ElementalPrecision", "IceShards", "Frostbite", "ImprovedFrostNova", "Permafrost", "PiercingIce", "IcyVeins", "ImprovedBlizzard", "ArcticReach", "FrostChanneling", "Shatter", "FrozenCore", "ColdSnap", "ImprovedConeOfCold", "IceFloes", "WintersChill", "IceBarrier", "ArcticWinds", "EmpoweredFrostbolt", "SummonWaterElemental" };
        private static string[] TalentListFriendly = { "Arcane Subtlety", "Arcane Focus", "Improved Arcane Missiles", "Wand Specialization", "Magic Absorption", "Arcane Concentration", "Magic Attunement", "Arcane Impact", "Arcane Fortitude", "Improved Mana Shield", "Improved Counterspell", "Arcane Meditation", "Improved Blink", "Presence of Mind", "Arcane Mind", "Prismatic Cloak", "Arcane Instability", "Arcane Potency", "Empowered Arcane Missiles", "Arcane Power", "Spell Power", "Mind Mastery", "Slow", "Improved Fireball", "Impact", "Ignite", "Flame Throwing", "Improved Fire Blast", "Incinerate", "Improved Flamestrike", "Pyroblast", "Burning Soul", "Improved Scorch", "Improved Fire Ward", "Master of Elements", "Playing with Fire", "Critical Mass", "Blast Wave", "Blazing Speed", "Fire Power", "Pyromaniac", "Combustion", "Molten Fury", "Empowered Fireball", "Dragon's Breath", "Frost Warding", "Improved Frostbolt", "Elemental Precision", "Ice Shards", "Frostbite", "Improved Frost Nova", "Permafrost", "Piercing Ice", "Icy Veins", "Improved Blizzard", "Arctic Reach", "Frost Channeling", "Shatter", "Frozen Core", "Cold Snap", "Improved Cone of Cold", "Ice Floes", "Winter's Chill", "Ice Barrier", "Arctic Winds", "Empowered Frostbolt", "Summon Water Elemental" };
        private static int[] MaxTalentPoints = { 2, 5, 5, 2, 5, 5, 2, 3, 1, 2, 2, 3, 2, 1, 5, 2, 3, 3, 3, 1, 2, 5, 1, 5, 5, 5, 2, 3, 2, 3, 1, 2, 3, 2, 3, 3, 3, 1, 2, 5, 3, 1, 2, 5, 1, 2, 5, 3, 5, 3, 2, 3, 3, 1, 3, 2, 3, 5, 3, 1, 3, 2, 5, 1, 5, 5, 1 };

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsMage baseCalc, currentCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Talents (per talent point)":
                    currentCalc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                    for (int index = 0; index < TalentList.Length; index++ )
                    {
                        string talent = TalentList[index];
                        int maxPoints = MaxTalentPoints[index];
                        int currentPoints = int.Parse(character.CalculationOptions[talent], CultureInfo.InvariantCulture);

                        if (currentPoints > 0)
                        {
                            character.CalculationOptions[talent] = "0";
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
                            character.CalculationOptions[talent] = MaxTalentPoints[index].ToString();
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

                        character.CalculationOptions[talent] = currentPoints.ToString();
                    }

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
                    charClone.CalculationOptions["IncrementalOptimizations"] = "0";
                    charClone.CalculationOptions["SmartOptimization"] = "1";

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

            character.CalculationOptions["ArcaneSubtlety"] = talentCode.Substring(0, 1);
            character.CalculationOptions["ArcaneFocus"] = talentCode.Substring(1, 1);
            character.CalculationOptions["ImprovedArcaneMissiles"] = talentCode.Substring(2, 1);
            character.CalculationOptions["WandSpecialization"] = talentCode.Substring(3, 1);
            character.CalculationOptions["MagicAbsorption"] = talentCode.Substring(4, 1);
            character.CalculationOptions["ArcaneConcentration"] = talentCode.Substring(5, 1);
            character.CalculationOptions["MagicAttunement"] = talentCode.Substring(6, 1);
            character.CalculationOptions["ArcaneImpact"] = talentCode.Substring(7, 1);
            character.CalculationOptions["ArcaneFortitude"] = talentCode.Substring(8, 1);
            character.CalculationOptions["ImprovedManaShield"] = talentCode.Substring(9, 1);
            character.CalculationOptions["ImprovedCounterspell"] = talentCode.Substring(10, 1);
            character.CalculationOptions["ArcaneMeditation"] = talentCode.Substring(11, 1);
            character.CalculationOptions["ImprovedBlink"] = talentCode.Substring(12, 1);
            character.CalculationOptions["PresenceOfMind"] = talentCode.Substring(13, 1);
            character.CalculationOptions["ArcaneMind"] = talentCode.Substring(14, 1);
            character.CalculationOptions["PrismaticCloak"] = talentCode.Substring(15, 1);
            character.CalculationOptions["ArcaneInstability"] = talentCode.Substring(16, 1);
            character.CalculationOptions["ArcanePotency"] = talentCode.Substring(17, 1);
            character.CalculationOptions["EmpoweredArcaneMissiles"] = talentCode.Substring(18, 1);
            character.CalculationOptions["ArcanePower"] = talentCode.Substring(19, 1);
            character.CalculationOptions["SpellPower"] = talentCode.Substring(20, 1);
            character.CalculationOptions["MindMastery"] = talentCode.Substring(21, 1);
            character.CalculationOptions["Slow"] = talentCode.Substring(22, 1);
            character.CalculationOptions["ImprovedFireball"] = talentCode.Substring(23, 1);
            character.CalculationOptions["Impact"] = talentCode.Substring(24, 1);
            character.CalculationOptions["Ignite"] = talentCode.Substring(25, 1);
            character.CalculationOptions["FlameThrowing"] = talentCode.Substring(26, 1);
            character.CalculationOptions["ImprovedFireBlast"] = talentCode.Substring(27, 1);
            character.CalculationOptions["Incinerate"] = talentCode.Substring(28, 1);
            character.CalculationOptions["ImprovedFlamestrike"] = talentCode.Substring(29, 1);
            character.CalculationOptions["Pyroblast"] = talentCode.Substring(30, 1);
            character.CalculationOptions["BurningSoul"] = talentCode.Substring(31, 1);
            character.CalculationOptions["ImprovedScorch"] = talentCode.Substring(32, 1);
            character.CalculationOptions["ImprovedFireWard"] = talentCode.Substring(33, 1);
            character.CalculationOptions["MasterOfElements"] = talentCode.Substring(34, 1);
            character.CalculationOptions["PlayingWithFire"] = talentCode.Substring(35, 1);
            character.CalculationOptions["CriticalMass"] = talentCode.Substring(36, 1);
            character.CalculationOptions["BlastWave"] = talentCode.Substring(37, 1);
            character.CalculationOptions["BlazingSpeed"] = talentCode.Substring(38, 1);
            character.CalculationOptions["FirePower"] = talentCode.Substring(39, 1);
            character.CalculationOptions["Pyromaniac"] = talentCode.Substring(40, 1);
            character.CalculationOptions["Combustion"] = talentCode.Substring(41, 1);
            character.CalculationOptions["MoltenFury"] = talentCode.Substring(42, 1);
            character.CalculationOptions["EmpoweredFireball"] = talentCode.Substring(43, 1);
            character.CalculationOptions["DragonsBreath"] = talentCode.Substring(44, 1);
            character.CalculationOptions["FrostWarding"] = talentCode.Substring(45, 1);
            character.CalculationOptions["ImprovedFrostbolt"] = talentCode.Substring(46, 1);
            character.CalculationOptions["ElementalPrecision"] = talentCode.Substring(47, 1);
            character.CalculationOptions["IceShards"] = talentCode.Substring(48, 1);
            character.CalculationOptions["Frostbite"] = talentCode.Substring(49, 1);
            character.CalculationOptions["ImprovedFrostNova"] = talentCode.Substring(50, 1);
            character.CalculationOptions["Permafrost"] = talentCode.Substring(51, 1);
            character.CalculationOptions["PiercingIce"] = talentCode.Substring(52, 1);
            character.CalculationOptions["IcyVeins"] = talentCode.Substring(53, 1);
            character.CalculationOptions["ImprovedBlizzard"] = talentCode.Substring(54, 1);
            character.CalculationOptions["ArcticReach"] = talentCode.Substring(55, 1);
            character.CalculationOptions["FrostChanneling"] = talentCode.Substring(56, 1);
            character.CalculationOptions["Shatter"] = talentCode.Substring(57, 1);
            character.CalculationOptions["FrozenCore"] = talentCode.Substring(58, 1);
            character.CalculationOptions["ColdSnap"] = talentCode.Substring(59, 1);
            character.CalculationOptions["ImprovedConeOfCold"] = talentCode.Substring(60, 1);
            character.CalculationOptions["IceFloes"] = talentCode.Substring(61, 1);
            character.CalculationOptions["WintersChill"] = talentCode.Substring(62, 1);
            character.CalculationOptions["IceBarrier"] = talentCode.Substring(63, 1);
            character.CalculationOptions["ArcticWinds"] = talentCode.Substring(64, 1);
            character.CalculationOptions["EmpoweredFrostbolt"] = talentCode.Substring(65, 1);
            character.CalculationOptions["SummonWaterElemental"] = talentCode.Substring(66, 1);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                AllResist = stats.AllResist,
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
                AldorRegaliaInterruptProtection = stats.AldorRegaliaInterruptProtection
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            float mageStats = stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellCritRating + stats.SpellDamageRating + stats.SpellFireDamageRating + stats.SpellHasteRating + stats.SpellHitRating + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusSpiritMultiplier + stats.SpellFrostDamageRating + stats.SpellArcaneDamageRating + stats.SpellPenetration + stats.Mana + stats.SpellCombatManaRegeneration + stats.BonusArcaneSpellPowerMultiplier + stats.BonusFireSpellPowerMultiplier + stats.BonusFrostSpellPowerMultiplier + stats.SpellFrostCritRating + stats.ArcaneBlastBonus + stats.SpellDamageFor6SecOnCrit + stats.EvocationExtension + stats.BonusMageNukeMultiplier + stats.LightningCapacitorProc + stats.SpellDamageFor20SecOnUse2Min + stats.SpellHasteFor20SecOnUse2Min + stats.Mp5OnCastFor20SecOnUse2Min + stats.ManaRestorePerHit + stats.ManaRestorePerCast + stats.SpellDamageFor15SecOnManaGem + stats.BonusManaGem + stats.SpellDamageFor10SecOnHit_10_45 + stats.SpellDamageFromIntellectPercentage + stats.SpellDamageFromSpiritPercentage + stats.SpellDamageFor10SecOnResist + stats.SpellDamageFor15SecOnCrit_20_45 + stats.SpellDamageFor15SecOnUse90Sec + stats.SpellHasteFor5SecOnCrit_50 + stats.SpellHasteFor6SecOnCast_15_45 + stats.SpellDamageFor10SecOnHit_5 + stats.SpellHasteFor6SecOnHit_10_45 + stats.SpellDamageFor10SecOnCrit_20_45 + stats.BonusManaPotion + stats.MageSpellCrit + stats.ThreatReductionMultiplier + stats.AllResist + stats.ArcaneResistance + stats.FireResistance + stats.FrostResistance + stats.NatureResistance + stats.ShadowResistance + stats.SpellHasteFor20SecOnUse5Min + stats.AldorRegaliaInterruptProtection;
            float ignoreStats = stats.Agility + stats.Strength + stats.AttackPower + stats.Healing + stats.DefenseRating + stats.Defense + stats.Dodge + stats.Parry + stats.DodgeRating + stats.ParryRating + stats.Hit + stats.HitRating + stats.ExpertiseRating + stats.Expertise + stats.Block + stats.BlockRating + stats.BlockValue + stats.SpellShadowDamageRating + stats.SpellNatureDamageRating;
            return (mageStats > 0 || ((stats.Health + stats.Stamina) > 0 && ignoreStats == 0.0f));
        }
    }
}
