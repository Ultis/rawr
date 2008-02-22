using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Mage
{
    [System.ComponentModel.DisplayName("Mage")]
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
                    _subPointNameColors.Add("Damage", System.Drawing.Color.FromArgb(160, 0, 224));
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
                    "Regeneration:MP5",
                    "Regeneration:Mana Regen",
                    "Regeneration:Health Regen",
                    "Spell Info:Arcane Missiles",
                    "Spell Info:Arcane Blast*spammed",
                    "Spell Info:Fireball",
                    "Spell Info:Frostbolt",
                    "Solution:Total Damage",
                    "Solution:Dps",
                    "Solution:Spell Cycles",
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
                    _customChartNames = new string[] {};
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

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationMage(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsMage(); }

        private void CombineTemporaryBuffs(List<CharacterCalculationsMage> statsList, Stats characterStats, Character character, Item additionalItem, bool arcanePower, bool moltenFury, bool icyVeins, bool heroism)
        {
            bool trinket1Available = IsItemActivatable(character.Trinket1);
            bool trinket2Available = IsItemActivatable(character.Trinket2);

            for (int flameCap = 0; flameCap < 2; flameCap++)
            {
                for (int trinket1 = 0; trinket1 < 2; trinket1++)
                {
                    for (int trinket2 = 0; trinket2 < 2; trinket2++)
                    {
                        for (int destructionPotion = 0; destructionPotion < 2; destructionPotion++)
                        {
                            if ((trinket1Available || trinket1 == 1) && (trinket2Available || trinket2 == 1))
                            {
                                if (!(trinket1 == 0 && trinket2 == 0)) // only leave through trinkets that can stack
                                {
                                    statsList.Add(GetTemporaryCharacterCalculations(characterStats, character, additionalItem, arcanePower, moltenFury, icyVeins, heroism, destructionPotion == 0, flameCap == 0, trinket1 == 0, trinket2 == 0));
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool IsItemActivatable(Item item)
        {
            if (item == null) return false;
            return (item.Stats.SpellDamageFor20SecOnUse2Min + item.Stats.SpellHasteFor20SecOnUse2Min + item.Stats.Mp5OnCastFor20SecOnUse2Min > 0);
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            bool heroismAvailable = int.Parse(character.CalculationOptions["HeroismAvailable"]) == 1;
            bool apAvailable = int.Parse(character.CalculationOptions["ArcanePower"]) == 1;
            bool ivAvailable = int.Parse(character.CalculationOptions["IcyVeins"]) == 1;
            bool mfAvailable = int.Parse(character.CalculationOptions["MoltenFury"]) > 0;
            bool trinket1Available = IsItemActivatable(character.Trinket1);
            bool trinket2Available = IsItemActivatable(character.Trinket2);

            double trinket1cooldown = 0, trinket1duration = 0, trinket2cooldown = 0, trinket2duration = 0, t1length = 0, t2length = 0;

            Stats characterStats = GetCharacterStats(character, additionalItem);

            // temporary buffs: Arcane Power, Icy Veins, Molten Fury, Combustion?, Trinket1, Trinket2, Heroism, Destro Pot, Flame Cap, Drums?
            // compute stats for temporary bonuses, each gives a list of spells used for final LP, solutions of LP stored in calculatedStats
            List<CharacterCalculationsMage> statsList = new List<CharacterCalculationsMage>();
            if (mfAvailable && heroismAvailable) CombineTemporaryBuffs(statsList, characterStats, character, additionalItem, false, true, false, true);
            if (mfAvailable && ivAvailable) CombineTemporaryBuffs(statsList, characterStats, character, additionalItem, false, true, true, false);
            if (mfAvailable) CombineTemporaryBuffs(statsList, characterStats, character, additionalItem, false, true, false, false);
            if (heroismAvailable && apAvailable) CombineTemporaryBuffs(statsList, characterStats, character, additionalItem, true, false, false, true);
            if (heroismAvailable) CombineTemporaryBuffs(statsList, characterStats, character, additionalItem, false, false, false, true);
            if (ivAvailable && apAvailable) CombineTemporaryBuffs(statsList, characterStats, character, additionalItem, true, false, true, false);
            if (apAvailable) CombineTemporaryBuffs(statsList, characterStats, character, additionalItem, true, false, false, false);
            if (ivAvailable) CombineTemporaryBuffs(statsList, characterStats, character, additionalItem, false, false, true, false);
            CombineTemporaryBuffs(statsList, characterStats, character, additionalItem, false, false, false, false);

            CharacterCalculationsMage calculatedStats = statsList[statsList.Count - 1];

            List<string> spellList = new List<string>() { "Arcane Missiles", "Fireball", "Frostbolt", "Arcane Blast (spam)" };

            int lpRows = 23;
            int colOffset = 6;
            int lpCols = colOffset - 1 + spellList.Count * statsList.Count;
            double[,] lp = new double[lpRows + 2, lpCols + 2];

            if (trinket1Available)
            {
                Stats s = character.Trinket1.Stats;
                if (s.SpellDamageFor20SecOnUse2Min + s.SpellHasteFor20SecOnUse2Min + s.Mp5OnCastFor20SecOnUse2Min > 0)
                {
                    trinket1duration = 20;
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
                t2length = (1 + (int)((calculatedStats.FightDuration - trinket2duration) / trinket2cooldown)) * trinket2duration;
            }

            // fill model [mana regen, time limit, evocation limit, mana pot limit, heroism cooldown, ap cooldown, ap+heroism cooldown, iv cooldown, mf cooldown, mf+dp cooldown, mf+iv cooldown, dp+heroism cooldown, dp+iv cooldown, flame cap cooldown, molten+flame, dp+flame, trinket1, trinket2, trinket1+mf, trinket2+mf, trinket1+heroism, trinket2+heroism]
            double aplength = (1 + (int)((calculatedStats.FightDuration - 30f) / 180f)) * 15;
            double ivlength = (1 + (int)((calculatedStats.FightDuration - 30f) / 180f)) * 20;
            double mflength = float.Parse(character.CalculationOptions["MoltenFuryPercentage"]) * calculatedStats.FightDuration;
            double dpivstackArea = calculatedStats.FightDuration;
            if (mfAvailable && heroismAvailable) dpivstackArea -= 120;
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

            // idle regen
            calculatedStats.SolutionLabel.Add("Idle Regen");
            lp[1, 1] = -calculatedStats.ManaRegen;
            lp[2, 1] = 1;
            lp[lpRows + 1, 1] = 0;
            // wand
            calculatedStats.SolutionLabel.Add("Wand");
            lp[1, 2] = -calculatedStats.ManaRegen; // TODO add JoW
            lp[2, 2] = 1;
            lp[lpRows + 1, 2] = 0; // TODO add wand dps
            // evocation
            double evocationDuration = (8f + calculatedStats.BasicStats.EvocationExtension) / calculatedStats.CastingSpeed;
            calculatedStats.EvocationDuration = evocationDuration;
            calculatedStats.SolutionLabel.Add("Evocation");
            lp[1, 3] = -calculatedStats.ManaRegen5SR - 0.15f * calculatedStats.BasicStats.Mana / 2f; // TODO add evocation weapons
            lp[2, 3] = 1;
            lp[3, 3] = 1;
            lp[lpRows + 1, 3] = 0;
            // mana pot
            calculatedStats.SolutionLabel.Add("Mana Potion");
            calculatedStats.MaxManaPotion = 1 + (int)((calculatedStats.FightDuration - 30f) / 120f);
            lp[1, 4] = -calculatedStats.ManaRegen5SR - 2400f / calculatedStats.ManaPotionTime;
            lp[2, 4] = 1;
            lp[4, 4] = 1;
            lp[lpRows + 1, 4] = 0;
            // mana gem
            calculatedStats.SolutionLabel.Add("Mana Gem");
            calculatedStats.MaxManaGem = Math.Min(5, 1 + (int)((calculatedStats.FightDuration - 30f) / 120f));
            lp[1, 5] = -calculatedStats.ManaRegen5SR + (-Math.Min(3, 1 + (int)((calculatedStats.FightDuration - 30f) / 120f)) * 2400f - ((calculatedStats.FightDuration >= 390) ? 1100f : 0f) - ((calculatedStats.FightDuration >= 510) ? 850 : 0)) / (calculatedStats.MaxManaGem * calculatedStats.ManaPotionTime);
            lp[2, 5] = 1;
            lp[5, 5] = 1;
            lp[15, 5] = 1;
            lp[lpRows + 1, 5] = 0;
            // spells
            for (int buffset = 0; buffset < statsList.Count; buffset++)
            {
                for (int spell = 0; spell < spellList.Count; spell++)
                {
                    Spell s = statsList[buffset].GetSpell(spellList[spell]);
                    calculatedStats.SolutionLabel.Add(((statsList[buffset].BuffLabel.Length > 0) ? (statsList[buffset].BuffLabel + "+") : "") + s.Name);
                    int index = buffset * spellList.Count + spell + colOffset;
                    lp[1, index] = s.CostPerSecond - s.ManaRegenPerSecond;
                    lp[2, index] = 1;
                    if (statsList[buffset].DestructionPotion) lp[4, index] = calculatedStats.ManaPotionTime / 15f;
                    lp[6, index] = (statsList[buffset].Heroism ? 1 : 0);
                    lp[7, index] = (statsList[buffset].ArcanePower ? 1 : 0);
                    lp[8, index] = ((statsList[buffset].Heroism && statsList[buffset].ArcanePower) ? 1 : 0);
                    lp[9, index] = (statsList[buffset].IcyVeins ? 1 : 0);
                    lp[10, index] = (statsList[buffset].MoltenFury ? 1 : 0);
                    lp[11, index] = ((statsList[buffset].MoltenFury && statsList[buffset].DestructionPotion) ? 1 : 0);
                    lp[12, index] = ((statsList[buffset].MoltenFury && statsList[buffset].IcyVeins) ? 1 : 0);
                    lp[13, index] = ((statsList[buffset].DestructionPotion && statsList[buffset].Heroism) ? 1 : 0);
                    lp[14, index] = ((statsList[buffset].DestructionPotion && statsList[buffset].IcyVeins) ? 1 : 0);
                    lp[15, index] = (statsList[buffset].FlameCap ? (calculatedStats.ManaPotionTime / 40f) : 0); ;
                    lp[16, index] = ((statsList[buffset].MoltenFury && statsList[buffset].FlameCap) ? 1 : 0); ;
                    lp[17, index] = ((statsList[buffset].DestructionPotion && statsList[buffset].FlameCap) ? 1 : 0);
                    lp[18, index] = (statsList[buffset].Trinket1 ? 1 : 0);
                    lp[19, index] = (statsList[buffset].Trinket2 ? 1 : 0);
                    lp[20, index] = ((statsList[buffset].MoltenFury && statsList[buffset].Trinket1) ? 1 : 0);
                    lp[21, index] = ((statsList[buffset].MoltenFury && statsList[buffset].Trinket2) ? 1 : 0);
                    lp[22, index] = ((statsList[buffset].Heroism && statsList[buffset].Trinket1) ? 1 : 0);
                    lp[23, index] = ((statsList[buffset].Heroism && statsList[buffset].Trinket2) ? 1 : 0);
                    lp[lpRows + 1, index] = s.DamagePerSecond;
                }
            }
            lp[1, lpCols + 1] = calculatedStats.BasicStats.Mana;
            lp[2, lpCols + 1] = calculatedStats.FightDuration;
            lp[3, lpCols + 1] = evocationDuration * Math.Max(1, (1 + Math.Floor((calculatedStats.FightDuration - 200f) / 480f)));
            lp[4, lpCols + 1] = calculatedStats.MaxManaPotion * calculatedStats.ManaPotionTime;
            lp[5, lpCols + 1] = calculatedStats.MaxManaGem * calculatedStats.ManaPotionTime;
            if (heroismAvailable) lp[6, lpCols + 1] = 40;
            if (apAvailable) lp[7, lpCols + 1] = aplength;
            if (heroismAvailable && apAvailable) lp[8, lpCols + 1] = 15;
            if (ivAvailable) lp[9, lpCols + 1] = ivlength;
            if (mfAvailable) lp[10, lpCols + 1] = mflength;
            if (mfAvailable) lp[11, lpCols + 1] = 15;
            if (mfAvailable && ivAvailable) lp[12, lpCols + 1] = 20;
            if (heroismAvailable) lp[13, lpCols + 1] = 15;
            if (ivAvailable) lp[14, lpCols + 1] = dpivlength;
            lp[15, lpCols + 1] = ((int)(calculatedStats.FightDuration / 120f + 1)) * calculatedStats.ManaPotionTime;
            if (mfAvailable) lp[16, lpCols + 1] = 60;
            lp[17, lpCols + 1] = dpflamelength;
            if (trinket1Available) lp[18, lpCols + 1] = t1length;
            if (trinket2Available) lp[19, lpCols + 1] = t2length;
            if (mfAvailable && trinket1Available) lp[20, lpCols + 1] = trinket1duration;
            if (mfAvailable && trinket2Available) lp[21, lpCols + 1] = trinket2duration;
            if (heroismAvailable && trinket1Available) lp[22, lpCols + 1] = trinket1duration;
            if (heroismAvailable && trinket2Available) lp[23, lpCols + 1] = trinket2duration;

            calculatedStats.Solution = LPSolve(lp, lpRows, lpCols);

            // water elemental
            if (int.Parse(character.CalculationOptions["SummonWaterElemental"]) == 1)
            {
                int targetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
                calculatedStats.WaterElemental = true;
                // 45 sec, 3 min cooldown + cold snap
                // 2.5 sec Waterbolt, affected by heroism, totems, 0.4x frost damage from character
                // TODO consider adding water elemental as part of optimization for stacking with cooldowns
                // TODO cold snap
                float spellHit = 0;
                if (character.ActiveBuffs.Contains("Totem of Wrath")) spellHit += 0.03f;
                if (character.ActiveBuffs.Contains("Heroic Presence")) spellHit += 0.01f;
                float hitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + spellHit);
                float spellCrit = 0.05f;
                if (character.ActiveBuffs.Contains("Winter's Chill")) spellHit += 0.1f;
                float multiplier = hitRate;
                if (character.ActiveBuffs.Contains("Curse of the Elements")) multiplier *= 1.1f;
                if (character.ActiveBuffs.Contains("Curse of the Elements (Malediction)")) multiplier *= 1.13f;
                if (character.ActiveBuffs.Contains("Misery")) multiplier *= 1.05f;
                float realResistance = float.Parse(character.CalculationOptions["FrostResist"]);
                float partialResistFactor = (realResistance == 1) ? 0 : (1 - realResistance - ((targetLevel > 70) ? ((targetLevel - 70) * 0.02f) : 0f));
                multiplier *= partialResistFactor;
                calculatedStats.WaterElementalDps = (521.5f + (0.4f * calculatedStats.FrostDamage + (character.ActiveBuffs.Contains("Wrath of Air") ? 101 : 0)) * 2f / 3f) * multiplier * (1 + 0.5f * spellCrit) / 2.5f;
                calculatedStats.WaterElementalDuration = (1 + (int)((calculatedStats.FightDuration - 45f) / 180f)) * 45;
                if (heroismAvailable)
                    calculatedStats.WaterElementalDamage = calculatedStats.WaterElementalDps * ((calculatedStats.WaterElementalDuration - 40) + 40 * 1.3f);
                else
                    calculatedStats.WaterElementalDamage = calculatedStats.WaterElementalDuration * calculatedStats.WaterElementalDps;
            }

            calculatedStats.SubPoints[0] = (float)calculatedStats.Solution[lpCols + 1] + calculatedStats.WaterElementalDamage;
            calculatedStats.OverallPoints = calculatedStats.SubPoints[0];

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
            
            XN = new int[cols + 1];
            XB = new int[rows + 1];
            
            for (i = 1; i <= rows; i++)
                XB[i] = cols + i;
            for (j = 1; j <= cols; j++)
                XN[j] = j;

            int round = 0;

            do
            {
                feasible = true;
                // check feasibility
                for (i = 1; i <= rows; i++)
                {
                    if (a[i, cols + 1] < 0)
                    {
                        feasible = false;
                        bestv = 0;
                        for (j = 1; j <= cols; j++)
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
                    for (j = 1; j <= cols; j++)
                    {
                        if (a[rows + 1, j] > bestv)
                        {
                            bestv = a[rows + 1, j];
                            c = j;
                        }
                    }
                }
                if (bestv == 0) break;
                bestv = -1;
                for (i = 1; i <= rows; i++)
                {
                    if (a[i, c] > 0)
                    {
                        v = a[i, cols + 1] / a[i, c];
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
                for (j = 1; j <= cols + 1; j++)
                {
                    a[r, j] = a[r, j] / v;
                }
                for (i = 1; i <= rows + 1; i++)
                {
                    if (i != r)
                    {
                        v = a[i, c];
                        a[i, c] = 0;
                        for (j = 1; j <= cols + 1; j++)
                        {
                            a[i, j] = a[i, j] - a[r, j] * v;
                            if (Math.Abs(a[i, j]) < 0.00000000000001)
                            {
                                a[i, j] = 0; // compensate for floating point errors
                            }
                        }
                    }
                }
                t = XN[c];
                XN[c] = XB[r];
                XB[r] = t;
                round++;
            } while (round < 5000); // fail safe for infinite loops caused by floating point instability
            
            double[] ret = new double[cols + 2];
            for (i = 1; i <= rows; i++)
            {
                if (XB[i] <= cols) ret[XB[i]] = a[i, cols + 1];
            }
            ret[cols + 1] = -a[rows + 1, cols + 1];

            return ret;
        }

        public CharacterCalculationsMage GetTemporaryCharacterCalculations(Stats characterStats, Character character, Item additionalItem, bool arcanePower, bool moltenFury, bool icyVeins, bool heroism, bool destructionPotion, bool flameCap, bool trinket1, bool trinket2)
        {
            CharacterCalculationsMage calculatedStats = new CharacterCalculationsMage();
            Stats stats = characterStats.Clone();
            calculatedStats.BasicStats = stats;
            calculatedStats.Character = character;

            float levelScalingFactor = (1 - (70 - 60) / 82f * 3);
            int molten = 0;
            if (character.CalculationOptions["MageArmor"].Equals("Molten")) molten = 1;

            float mindMasteryDamage = (0.05f * int.Parse(character.CalculationOptions["MindMastery"]) * stats.Intellect);
            float improvedSpiritDamage = 0;
            if (character.ActiveBuffs.Contains("Improved Divine Spirit")) improvedSpiritDamage = 0.1f * stats.Spirit;
            if (destructionPotion) stats.SpellDamageRating += 120;
            if (flameCap) stats.SpellFireDamageRating += 80;

            if (trinket1)
            {
                Stats t = character.Trinket1.Stats;
                stats.SpellDamageRating += t.SpellDamageFor20SecOnUse2Min;
                stats.SpellHasteRating += t.SpellHasteFor20SecOnUse2Min;
                calculatedStats.Mp5OnCastFor20Sec = t.Mp5OnCastFor20SecOnUse2Min;
            }
            if (trinket2)
            {
                Stats t = character.Trinket2.Stats;
                stats.SpellDamageRating += t.SpellDamageFor20SecOnUse2Min;
                stats.SpellHasteRating += t.SpellHasteFor20SecOnUse2Min;
                calculatedStats.Mp5OnCastFor20Sec = t.Mp5OnCastFor20SecOnUse2Min;
            }

            calculatedStats.CastingSpeed = 1 + stats.SpellHasteRating / 995f * levelScalingFactor;
            calculatedStats.ArcaneDamage = stats.SpellArcaneDamageRating + stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;
            calculatedStats.FireDamage = stats.SpellFireDamageRating + stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;
            calculatedStats.FrostDamage = stats.SpellFrostDamageRating + stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;
            calculatedStats.NatureDamage = stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;

            calculatedStats.SpellCrit = 0.01f * (stats.Intellect * 0.0125f + 0.9075f) + 0.01f * int.Parse(character.CalculationOptions["ArcaneInstability"]) + 0.01f * int.Parse(character.CalculationOptions["ArcanePotency"]) + stats.SpellCritRating / 1400f * levelScalingFactor + 0.03f * molten;
            if (destructionPotion) calculatedStats.SpellCrit += 0.02f;
            calculatedStats.SpellHit = stats.SpellHitRating * levelScalingFactor / 800f;

            int targetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
            calculatedStats.ArcaneHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit + 0.02f * int.Parse(character.CalculationOptions["ArcaneFocus"]));
            calculatedStats.FireHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit + 0.01f * int.Parse(character.CalculationOptions["ElementalPrecision"]));
            calculatedStats.FrostHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit + 0.01f * int.Parse(character.CalculationOptions["ElementalPrecision"]));
            calculatedStats.NatureHitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + calculatedStats.SpellHit);

            calculatedStats.SpiritRegen = 0.001f + stats.Spirit * 0.009327f * (float)Math.Sqrt(stats.Intellect);
            calculatedStats.ManaRegen = calculatedStats.SpiritRegen + stats.Mp5 / 5f;
            calculatedStats.ManaRegen5SR = calculatedStats.SpiritRegen * stats.SpellCombatManaRegeneration + stats.Mp5 / 5f;
            calculatedStats.ManaRegenDrinking = calculatedStats.ManaRegen + 240f;
            calculatedStats.HealthRegen = 0.0312f * stats.Spirit + stats.Hp5 / 5f;
            calculatedStats.HealthRegenCombat = stats.Hp5 / 5f;
            calculatedStats.HealthRegenEating = calculatedStats.ManaRegen + 250f;
            calculatedStats.MeleeMitigation = (1 - 1 / (1 + 0.1f * stats.Armor / (8.5f * (70 + 4.5f * (70 - 59)) + 40)));
            calculatedStats.Defense = 350 + stats.DefenseRating / 2.37f;
            calculatedStats.PhysicalCritReduction = (0.04f * (calculatedStats.Defense - 5 * 70) / 100 + stats.Resilience / 2500f * levelScalingFactor + 0.05f * molten);
            calculatedStats.SpellCritReduction = (stats.Resilience / 2500f * levelScalingFactor + 0.05f * molten);
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

            List<String> buffList = new List<string> ();
            if (moltenFury) buffList.Add("Molten Fury");
            if (heroism) buffList.Add("Heroism");
            if (icyVeins) buffList.Add("Icy Veins");
            if (arcanePower) buffList.Add("Arcane Power");
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

            calculatedStats.Latency = float.Parse(character.CalculationOptions["Latency"]);
            calculatedStats.FightDuration = float.Parse(character.CalculationOptions["FightDuration"]);
            calculatedStats.ClearcastingChance = 0.02f * float.Parse(character.CalculationOptions["ArcaneConcentration"]);

            calculatedStats.GlobalCooldown = Math.Max(1f, 1.5f / calculatedStats.CastingSpeed);

            calculatedStats.ArcaneSpellModifier = (1 + 0.01f * int.Parse(character.CalculationOptions["ArcaneInstability"])) * (1 + 0.01f * int.Parse(character.CalculationOptions["PlayingWithFire"])) * (1 + stats.BonusSpellPowerMultiplier);
            if (arcanePower)
            {
                calculatedStats.ArcaneSpellModifier *= 1.3f;
            }
            if (moltenFury)
            {
                calculatedStats.ArcaneSpellModifier *= (1 + 0.1f * int.Parse(character.CalculationOptions["MoltenFury"]));
            }
            calculatedStats.FireSpellModifier = calculatedStats.ArcaneSpellModifier * (1 + 0.02f * int.Parse(character.CalculationOptions["FirePower"]));
            calculatedStats.FrostSpellModifier = calculatedStats.ArcaneSpellModifier * (1 + 0.02f * int.Parse(character.CalculationOptions["PiercingIce"]));
            calculatedStats.NatureSpellModifier = calculatedStats.ArcaneSpellModifier;
            calculatedStats.ArcaneSpellModifier *= (1 + stats.BonusArcaneSpellPowerMultiplier);
            calculatedStats.FireSpellModifier *= (1 + stats.BonusFireSpellPowerMultiplier);
            calculatedStats.FrostSpellModifier *= (1 + stats.BonusFrostSpellPowerMultiplier);

            calculatedStats.ResilienceCritDamageReduction = 1;
            calculatedStats.ResilienceCritRateReduction = 0;

            calculatedStats.ArcaneCritBonus = (1 + (1.5f * (1 + stats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * int.Parse(character.CalculationOptions["SpellPower"]))) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.FireCritBonus = (1 + (1.5f * (1 + stats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * int.Parse(character.CalculationOptions["SpellPower"]))) * (1 + 0.08f * int.Parse(character.CalculationOptions["Ignite"])) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.FrostCritBonus = (1 + (1.5f * (1 + stats.BonusSpellCritMultiplier) - 1) * (1 + 0.2f * int.Parse(character.CalculationOptions["IceShards"]) + 0.25f * int.Parse(character.CalculationOptions["SpellPower"]))) * calculatedStats.ResilienceCritDamageReduction;
            calculatedStats.NatureCritBonus = (1 + (1.5f * (1 + stats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * int.Parse(character.CalculationOptions["SpellPower"]))) * calculatedStats.ResilienceCritDamageReduction;

            calculatedStats.ArcaneCritRate = calculatedStats.SpellCrit;
            calculatedStats.FireCritRate = calculatedStats.SpellCrit + 0.02f * int.Parse(character.CalculationOptions["CriticalMass"]) + 0.01f * int.Parse(character.CalculationOptions["Pyromaniac"]);
            if (int.Parse(character.CalculationOptions["Combustion"]) == 1) calculatedStats.FireCritRate += (float)(-0.04f * Math.Pow(calculatedStats.FireCritRate, 3) + 0.09f * Math.Pow(calculatedStats.FireCritRate, 2) - 0.08f * calculatedStats.FireCritRate + 0.03f);
            calculatedStats.FrostCritRate = calculatedStats.SpellCrit + stats.SpellFrostCritRating / 22.08f / 100f;
            calculatedStats.NatureCritRate = calculatedStats.SpellCrit;

            return calculatedStats;
        }


        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            // TODO: add racial base stats for other races
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
                        BonusIntellectMultiplier = 0.03f * int.Parse(character.CalculationOptions["ArcaneMind"]),
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
                        BonusIntellectMultiplier = 0.03f * int.Parse(character.CalculationOptions["ArcaneMind"]),
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
                        BonusIntellectMultiplier = 1.05f * (1 + 0.03f * int.Parse(character.CalculationOptions["ArcaneMind"])) - 1
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
                        BonusIntellectMultiplier = 0.03f * int.Parse(character.CalculationOptions["ArcaneMind"]),
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
                        BonusIntellectMultiplier = 0.03f * int.Parse(character.CalculationOptions["ArcaneMind"]),
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
                        BonusIntellectMultiplier = 0.03f * int.Parse(character.CalculationOptions["ArcaneMind"]),
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;

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
            statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2f + statsTotal.Intellect * int.Parse(character.CalculationOptions["ArcaneFortitude"]));

            int magicAbsorption = 2 * int.Parse(character.CalculationOptions["MagicAbsorption"]);
            int frostWarding = int.Parse(character.CalculationOptions["FrostWarding"]);
            statsTotal.AllResist += magicAbsorption;

            if (character.CalculationOptions["MageArmor"].Equals("Mage"))
            {
                statsTotal.SpellCombatManaRegeneration += 0.3f;
                statsTotal.AllResist += 18;
            }
            if (character.CalculationOptions["MageArmor"].Equals("Ice"))
            {
                statsTotal.Armor += (float)Math.Floor(645 * (1 + 0.15f * frostWarding));
                statsTotal.FrostResistance += (float)Math.Floor(18 * (1 + 0.15f * frostWarding));
            }

            statsTotal.SpellCombatManaRegeneration += 0.1f * int.Parse(character.CalculationOptions["ArcaneMeditation"]);

            statsTotal.SpellPenetration += 5 * int.Parse(character.CalculationOptions["ArcaneSubtlety"]);

            statsTotal.Mp5 += float.Parse(character.CalculationOptions["ShadowPriest"]);

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            throw new NotImplementedException();
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
                SpellDamageOnCritProc = stats.SpellDamageOnCritProc,
                EvocationExtension = stats.EvocationExtension,
                BonusMageNukeMultiplier = stats.BonusMageNukeMultiplier,
                LightningCapacitorProc = stats.LightningCapacitorProc,
                SpellDamageFor20SecOnUse2Min = stats.SpellDamageFor20SecOnUse2Min,
                SpellHasteFor20SecOnUse2Min = stats.SpellHasteFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return stats.ToString().Equals("") || (stats.AllResist + stats.ArcaneResistance + stats.FireResistance + stats.FrostResistance + stats.NatureResistance + stats.ShadowResistance + stats.Stamina + stats.Intellect + stats.Spirit + stats.Health + stats.Mp5 + stats.Resilience + stats.SpellCritRating + stats.SpellDamageRating + stats.SpellFireDamageRating + stats.SpellHasteRating + stats.SpellHitRating + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusStaminaMultiplier + stats.BonusSpiritMultiplier + stats.SpellFrostDamageRating + stats.SpellArcaneDamageRating + stats.SpellPenetration + stats.Mana + stats.SpellCombatManaRegeneration + stats.BonusArcaneSpellPowerMultiplier + stats.BonusFireSpellPowerMultiplier + stats.BonusFrostSpellPowerMultiplier + stats.SpellFrostCritRating + stats.ArcaneBlastBonus + stats.SpellDamageOnCritProc + stats.EvocationExtension + stats.BonusMageNukeMultiplier + stats.LightningCapacitorProc + stats.SpellDamageFor20SecOnUse2Min + stats.SpellHasteFor20SecOnUse2Min + stats.Mp5OnCastFor20SecOnUse2Min) > 0;
        }
    }
}
