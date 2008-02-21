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

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            // base stats
            CharacterCalculationsMage calculatedStats = GetTemporaryCharacterCalculations(character, additionalItem, false, false, false, false);
            // temporary buffs: Arcane Power, Icy Veins, Molten Fury, Combustion?, Trinket1, Trinket2, Heroism
            // compute stats for temporary bonuses, each gives a list of spells used for final LP, solutions of LP stored in calculatedStats

            List<string> spellList = new List<string>() { "Arcane Missiles", "Fireball", "Frostbolt", "Arcane Blast (spam)" };

            // for now just a simple model with mana and time
            int lpRows = 5;
            int colOffset = 6;
            int lpCols = colOffset - 1 + spellList.Count;
            double[,] lp = new double[lpRows + 2, lpCols + 2];

            // fill model [mana regen, time limit, evocation limit, mana pot limit]

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
            lp[lpRows + 1, 5] = 0;
            // spells
            for (int spell = 0; spell < spellList.Count; spell++)
            {
                Spell s = calculatedStats.GetSpell(spellList[spell]);
                calculatedStats.SolutionLabel.Add(s.Name);
                lp[1, spell + colOffset] = s.CostPerSecond - s.ManaRegenPerSecond;
                lp[2, spell + colOffset] = 1;
                lp[lpRows + 1, spell + colOffset] = s.DamagePerSecond;
            }
            lp[1, lpCols + 1] = calculatedStats.BasicStats.Mana;
            lp[2, lpCols + 1] = calculatedStats.FightDuration;
            lp[3, lpCols + 1] = evocationDuration * Math.Max(1, (1 + Math.Floor((calculatedStats.FightDuration - 200f) / 480f)));
            lp[4, lpCols + 1] = calculatedStats.MaxManaPotion * calculatedStats.ManaPotionTime;
            lp[5, lpCols + 1] = calculatedStats.MaxManaGem * calculatedStats.ManaPotionTime;

            calculatedStats.Solution = LPSolve(lp, lpRows, lpCols);

            calculatedStats.SubPoints[0] = (float)calculatedStats.Solution[lpCols + 1];
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
            } while (true);
            
            double[] ret = new double[cols + 2];
            for (i = 1; i <= rows; i++)
            {
                if (XB[i] <= cols) ret[XB[i]] = a[i, cols + 1];
            }
            ret[cols + 1] = -a[rows + 1, cols + 1];

            return ret;
        }

        public CharacterCalculationsMage GetTemporaryCharacterCalculations(Character character, Item additionalItem, bool arcanePower, bool moltenFury, bool icyVeins, bool heroism)
        {
            CharacterCalculationsMage calculatedStats = new CharacterCalculationsMage();
            Stats stats = GetCharacterStats(character, additionalItem);
            calculatedStats.BasicStats = stats;
            calculatedStats.Character = character;

            float levelScalingFactor = (1 - (70 - 60) / 82f * 3);
            int molten = 0;
            if (character.CalculationOptions["MageArmor"].Equals("Molten")) molten = 1;

            float mindMasteryDamage = (0.05f * int.Parse(character.CalculationOptions["MindMastery"]) * stats.Intellect);
            float improvedSpiritDamage = 0;
            if (character.ActiveBuffs.Contains("Improved Divine Spirit")) improvedSpiritDamage = 0.1f * stats.Spirit;

            calculatedStats.CastingSpeed = 1 + stats.SpellHasteRating / 995f * levelScalingFactor;
            calculatedStats.ArcaneDamage = stats.SpellArcaneDamageRating + stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;
            calculatedStats.FireDamage = stats.SpellFireDamageRating + stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;
            calculatedStats.FrostDamage = stats.SpellFrostDamageRating + stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;
            calculatedStats.NatureDamage = /* stats.SpellNatureDamageRating + */ stats.SpellDamageRating + mindMasteryDamage + improvedSpiritDamage;

            calculatedStats.SpellCrit = 0.01f * (stats.Intellect * 0.0125f + 0.9075f) + 0.01f * int.Parse(character.CalculationOptions["ArcaneInstability"]) + 0.01f * int.Parse(character.CalculationOptions["ArcanePotency"]) + stats.SpellCritRating / 1400f * levelScalingFactor + 0.03f * molten;
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
                LightningCapacitorProc = stats.LightningCapacitorProc
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return stats.ToString().Equals("") || (stats.AllResist + stats.ArcaneResistance + stats.FireResistance + stats.FrostResistance + stats.NatureResistance + stats.ShadowResistance + stats.Stamina + stats.Intellect + stats.Spirit + stats.Health + stats.Mp5 + stats.Resilience + stats.SpellCritRating + stats.SpellDamageRating + stats.SpellFireDamageRating + stats.SpellHasteRating + stats.SpellHitRating + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusStaminaMultiplier + stats.BonusSpiritMultiplier + stats.SpellFrostDamageRating + stats.SpellArcaneDamageRating + stats.SpellPenetration + stats.Mana + stats.SpellCombatManaRegeneration + stats.BonusArcaneSpellPowerMultiplier + stats.BonusFireSpellPowerMultiplier + stats.BonusFrostSpellPowerMultiplier + stats.SpellFrostCritRating + stats.ArcaneBlastBonus + stats.SpellDamageOnCritProc + stats.EvocationExtension + stats.BonusMageNukeMultiplier + stats.LightningCapacitorProc) > 0;
        }
    }
}
