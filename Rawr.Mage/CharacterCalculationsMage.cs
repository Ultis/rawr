using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
using Rawr.Mage.SequenceReconstruction;

namespace Rawr.Mage
{
    public enum VariableType
    {
        None,
        IdleRegen,
        Wand,
        Evocation,
        ManaPotion,
        ManaGem,
        DrumsOfBattle,
        Drinking,
        TimeExtension,
        AfterFightRegen,
        ManaOverflow,
        Spell,
    }

    public struct SolutionVariable
    {
        public int Segment;
        public CastingState State;
        public Spell Spell;
        public VariableType Type;

        public bool IsZeroTime
        {
            get
            {
                return Type == VariableType.ManaPotion || Type == VariableType.ManaGem || Type == VariableType.ManaOverflow;
            }
        }
    }

    public sealed class CharacterCalculationsMage : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DpsRating
        {
            get
            {
                return _subPoints[0];
            }
        }

        public float SurvivabilityRating
        {
            get
            {
                return _subPoints[1];
            }
        }

        public CalculationsMage Calculations { get; set; }
        public Stats BaseStats { get; set; }
        public CastingState BaseState { get; set; }
        public CalculationOptionsMage CalculationOptions { get; set; }

        public Character Character { get; set; }

        public bool WaterElemental { get; set; }
        public float WaterElementalDps { get; set; }
        public float WaterElementalDuration { get; set; }
        public float WaterElementalDamage { get; set; }

        public float StartingMana { get; set; }

        public string MageArmor { get; set; }

        public double EvocationDuration;
        public double EvocationRegen;
        //public double ManaPotionTime = 0.1f;
        public double Trinket1Duration;
        public double Trinket1Cooldown;
        public double Trinket2Duration;
        public double Trinket2Cooldown;
        public string Trinket1Name;
        public string Trinket2Name;
        public int MaxManaPotion;
        public int MaxManaGem;
        public int MaxEvocation;

        public double[] Solution;
        public List<SolutionVariable> SolutionVariable;
        public float Tps;

        public int ColumnIdleRegen = -1;
        public int ColumnWand = -1;
        public int ColumnEvocation = -1;
        public int ColumnManaGem = -1;
        public int ColumnManaPotion = -1;
        public int ColumnDrumsOfBattle = -1;
        public int ColumnDrinking = -1;
        public int ColumnTimeExtension = -1;
        public int ColumnAfterFightRegen = -1;
        public int ColumnManaOverflow = -1;

        public float ChanceToDie { get; set; }
        public float MeanIncomingDps { get; set; }

        public string ReconstructSequence()
        {
            CalculationOptions.SequenceReconstruction = null;
            CalculationOptions.Calculations = null;
            if (!CalculationOptions.ReconstructSequence) return "*Disabled";
            if (CalculationOptions.FightDuration > 900) return "*Unavailable";

            SequenceItem.Calculations = this;
            Sequence sequence = new Sequence();

            for (int i = 0; i < SolutionVariable.Count; i++)
            {
                if (Solution[i] > 0.01 && SolutionVariable[i].Type != VariableType.ManaOverflow)
                {
                    sequence.Add(new SequenceItem(i, Solution[i]));
                }
            }

            StringBuilder timing = new StringBuilder();
            double bestUnexplained = double.PositiveInfinity;
            string bestTiming = "*";

            // evaluate sequence
            double unexplained;
            /*unexplained = sequence.Evaluate(timing, Sequence.EvaluationMode.Unexplained);
            if (unexplained < bestUnexplained)
            {
                bestUnexplained = unexplained;
                bestTiming = timing.ToString();
            }*/

            sequence.GroupMoltenFury();
            SequenceGroup heroismGroup = sequence.GroupHeroism();
            if (CalculationOptions.HeroismControl == 3)
            {
                heroismGroup.MinTime = CalculationOptions.FightDuration - CalculationOptions.MoltenFuryPercentage * CalculationOptions.FightDuration;
            }
            sequence.GroupCombustion();
            sequence.GroupArcanePower();
            sequence.GroupDestructionPotion();
            List<SequenceGroup> list1 = sequence.GroupTrinket1();
            List<SequenceGroup> list2 = sequence.GroupTrinket2();
            List<SequenceGroup> list = null;
            if (Character.Trinket1 != null && Character.Trinket1.Stats.SpellDamageFor15SecOnManaGem > 0)
            {
                list = list1;
            }
            if (Character.Trinket2 != null && Character.Trinket2.Stats.SpellDamageFor15SecOnManaGem > 0)
            {
                list = list2;
            }
            if (list != null)
            {
                float manaBurn = 80;
                if (CalculationOptions.AoeDuration > 0)
                {
                    Spell s = BaseState.GetSpell(SpellId.ArcaneExplosion);
                    manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
                }
                else if (CalculationOptions.EmpoweredFireball > 0)
                {
                    Spell s = BaseState.GetSpell(SpellId.Fireball);
                    manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
                }
                else if (CalculationOptions.EmpoweredFrostbolt > 0)
                {
                    Spell s = BaseState.GetSpell(SpellId.Frostbolt);
                    manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
                }
                else if (CalculationOptions.SpellPower > 0)
                {
                    Spell s = BaseState.GetSpell(SpellId.ArcaneBlast33);
                    manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
                }
                if (CalculationOptions.IcyVeins > 0)
                {
                    manaBurn *= 1.1f;
                }
                if (CalculationOptions.ArcanePower > 0)
                {
                    manaBurn *= 1.1f;
                }

                double overflow = Solution[ColumnManaOverflow];
                double tmin = (2400.0 * (1 + BaseStats.BonusManaGem) - overflow) / manaBurn;

                foreach (SequenceGroup g in list)
                {
                    if (g.Segment == 0) g.MinTime = tmin;
                }
            }
            sequence.GroupIcyVeins(); // should come after trinkets because of coldsnap
            sequence.GroupDrumsOfBattle();
            sequence.GroupFlameCap();

            sequence.SortGroups();

            // mana gem/pot/evo positioning
            sequence.RepositionManaConsumption();

            sequence.RemoveIndex(ColumnTimeExtension);
            sequence.Compact(true);
            CalculationOptions.SequenceReconstruction = sequence;
            CalculationOptions.Calculations = this;

            // evaluate sequence
            unexplained = sequence.Evaluate(timing, Sequence.EvaluationMode.Unexplained);
            if (unexplained < bestUnexplained)
            {
                bestUnexplained = unexplained;
                bestTiming = timing.ToString();
            }

            return bestTiming;
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            if (CalculationOptions.DisplaySegmentCooldowns != CalculationOptions.ComparisonSegmentCooldowns || CalculationOptions.DisplayIntegralMana != CalculationOptions.ComparisonIntegralMana)
            {
                bool savedIncrementalOptimizations = CalculationOptions.IncrementalOptimizations;
                CalculationOptions.IncrementalOptimizations = false;
                CharacterCalculationsMage smp = Solver.GetCharacterCalculations(Character, null, CalculationOptions, Calculations, MageArmor, CalculationOptions.DisplaySegmentCooldowns, CalculationOptions.DisplayIntegralMana);
                Dictionary<string, string> ret = smp.GetCharacterDisplayCalculationValuesInternal();
                ret["Dps"] = String.Format("{0:F}*{1:F}% Error margin", smp.DpsRating, Math.Abs(DpsRating - smp.DpsRating) / DpsRating * 100);
                CalculationOptions.IncrementalOptimizations = savedIncrementalOptimizations;
                return ret;
            }
            else
            {
                return GetCharacterDisplayCalculationValuesInternal();
            }
        }

        internal Dictionary<string, string> GetCharacterDisplayCalculationValuesInternal()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Stamina", BaseStats.Stamina.ToString());
            dictValues.Add("Intellect", BaseStats.Intellect.ToString());
            dictValues.Add("Spirit", BaseStats.Spirit.ToString());
            dictValues.Add("Armor", BaseStats.Armor.ToString());
            dictValues.Add("Health", BaseStats.Health.ToString());
            dictValues.Add("Mana", BaseStats.Mana.ToString());
            dictValues.Add("Spell Crit Rate", String.Format("{0:F}%*{1} Spell Crit Rating", 100 * BaseState.SpellCrit, BaseStats.SpellCritRating));
            dictValues.Add("Spell Hit Rate", String.Format("{0:F}%*{1} Spell Hit Rating", 100 * BaseState.SpellHit, BaseStats.SpellHitRating));
            dictValues.Add("Spell Penetration", BaseStats.SpellPenetration.ToString());
            dictValues.Add("Casting Speed", String.Format("{0}*{1} Spell Haste Rating", BaseState.CastingSpeed, BaseState.SpellHasteRating));
            dictValues.Add("Arcane Damage", BaseState.ArcaneDamage.ToString());
            dictValues.Add("Fire Damage", BaseState.FireDamage.ToString());
            dictValues.Add("Frost Damage", BaseState.FrostDamage.ToString());
            dictValues.Add("MP5", BaseStats.Mp5.ToString());
            dictValues.Add("Mana Regen", Math.Floor(BaseState.ManaRegen * 5).ToString() + String.Format("*Mana Regen in 5SR: {0}\r\nMana Regen Drinking: {1}", Math.Floor(BaseState.ManaRegen5SR * 5), Math.Floor(BaseState.ManaRegenDrinking * 5)));
            dictValues.Add("Health Regen", Math.Floor(BaseState.HealthRegenCombat * 5).ToString() + String.Format("*Health Regen Eating: {0}", Math.Floor(BaseState.HealthRegenEating * 5)));
            dictValues.Add("Arcane Resist", (BaseStats.ArcaneResistance).ToString());
            dictValues.Add("Fire Resist", (BaseStats.FireResistance).ToString());
            dictValues.Add("Nature Resist", (BaseStats.NatureResistance).ToString());
            dictValues.Add("Frost Resist", (BaseStats.FrostResistance).ToString());
            dictValues.Add("Shadow Resist", (BaseStats.ShadowResistance).ToString());
            dictValues.Add("Physical Mitigation", String.Format("{0:F}%", 100 * BaseState.MeleeMitigation));
            dictValues.Add("Resilience", BaseStats.Resilience.ToString());
            dictValues.Add("Defense", BaseState.Defense.ToString());
            dictValues.Add("Crit Reduction", String.Format("{0:F}%*Spell Crit Reduction: {0:F}%\r\nPhysical Crit Reduction: {1:F}%\r\nCrit Damage Reduction: {2:F}%", BaseState.SpellCritReduction * 100, BaseState.PhysicalCritReduction * 100, BaseState.CritDamageReduction * 100));
            dictValues.Add("Dodge", String.Format("{0:F}%", 100 * BaseState.Dodge));
            dictValues.Add("Chance to Die", String.Format("{0:F}%", 100 * ChanceToDie));
            dictValues.Add("Mean Incoming Dps", String.Format("{0:F}", MeanIncomingDps));
            List<SpellId> spellList = new List<SpellId>() { SpellId.Wand, SpellId.ArcaneMissiles, SpellId.ABMBAM, SpellId.ArcaneBarrage, SpellId.Scorch, SpellId.Fireball, SpellId.Pyroblast, SpellId.Frostbolt, SpellId.ArcaneBlast33, SpellId.ABAMP, SpellId.ABAM, SpellId.AB3AMSc, SpellId.ABAM3Sc, SpellId.ABAM3Sc2, SpellId.ABAM3FrB, SpellId.ABAM3FrB2, SpellId.ABFrB3FrB, SpellId.ABFrB3FrBSc, SpellId.ABFB3FBSc, SpellId.FireballScorch, SpellId.FireballFireBlast, SpellId.FireBlast, SpellId.ABAM3ScCCAM, SpellId.ABAM3Sc2CCAM, SpellId.ABAM3FrBCCAM, SpellId.ABAM3FrBScCCAM, SpellId.ABAMCCAM, SpellId.ABAM3CCAM, SpellId.ArcaneExplosion, SpellId.FlamestrikeSpammed, SpellId.Blizzard, SpellId.BlastWave, SpellId.DragonsBreath, SpellId.ConeOfCold, SpellId.ABFrB, SpellId.FrostfireBolt, SpellId.ABABar };
            Spell AB = BaseState.GetSpell(SpellId.ArcaneBlast33);
            BaseSpell bs;
            foreach (SpellId spell in spellList)
            {
                Spell s = BaseState.GetSpell(spell);
                if (s != null)
                {
                    if (s is BaseSpell)
                    {
                        bs = s as BaseSpell;
                        dictValues.Add(bs.Name, String.Format("{0:F} Dps*{1:F} Mps\r\n{2:F} Tps\r\n{3:F} sec\r\n{14:F} Mana\r\n{9:F} - {10:F} Hit\r\n{11:F} - {12:F} Crit{13}\r\n{4:F}x Amplify\r\n{5:F}% Crit Rate\r\n{6:F}% Hit Rate\r\n{7:F} Crit Multiplier\r\nAB Spam Tradeoff: {8:F} Dpm", bs.DamagePerSecond, bs.CostPerSecond - bs.ManaRegenPerSecond, bs.ThreatPerSecond, bs.CastTime - BaseState.Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, (AB.DamagePerSecond - bs.DamagePerSecond) / (AB.CostPerSecond - AB.ManaRegenPerSecond - bs.CostPerSecond + bs.ManaRegenPerSecond), bs.MinHitDamage / bs.HitProcs, bs.MaxHitDamage / bs.HitProcs, bs.MinCritDamage / bs.HitProcs, bs.MaxCritDamage / bs.HitProcs, ((bs.DotDamage > 0) ? ("\n" + bs.DotDamage.ToString("F") + " Dot") : ""), bs.Cost));
                    }
                    else
                    {
                        dictValues.Add(s.Name, String.Format("{0:F} Dps*{1:F} Mps\r\n{2:F} Tps\r\nAB Spam Tradeoff: {3:F} Dpm\r\nAverage Cast Time: {4:F} sec\r\n{5}", s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond, s.ThreatPerSecond, (AB.DamagePerSecond - s.DamagePerSecond) / (AB.CostPerSecond - AB.ManaRegenPerSecond - s.CostPerSecond + s.ManaRegenPerSecond), s.CastTime, s.Sequence));
                    }
                }
            }
            bs = BaseState.GetSpell(SpellId.ArcaneBlast00) as BaseSpell;
            dictValues.Add("Arcane Blast(0)", String.Format("{0:F} Dps*{1:F} Mps\r\n{2:F} Tps\r\n{3:F} sec\r\n{14:F} Mana\r\n{9:F} - {10:F} Hit\r\n{11:F} - {12:F} Crit{13}\r\n{4:F}x Amplify\r\n{5:F}% Crit Rate\r\n{6:F}% Hit Rate\r\n{7:F} Crit Multiplier\r\nAB Spam Tradeoff: {8:F} Dpm", bs.DamagePerSecond, bs.CostPerSecond - bs.ManaRegenPerSecond, bs.ThreatPerSecond, bs.CastTime - BaseState.Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, (AB.DamagePerSecond - bs.DamagePerSecond) / (AB.CostPerSecond - AB.ManaRegenPerSecond - bs.CostPerSecond + bs.ManaRegenPerSecond), bs.MinHitDamage / bs.HitProcs, bs.MaxHitDamage / bs.HitProcs, bs.MinCritDamage / bs.HitProcs, bs.MaxCritDamage / bs.HitProcs, ((bs.DotDamage > 0) ? ("\n" + bs.DotDamage.ToString("F") + " Dot") : ""), bs.Cost));
            bs = BaseState.GetSpell(SpellId.ArcaneMissilesMB) as BaseSpell;
            dictValues.Add("MBAM", String.Format("{0:F} Dps*{1:F} Mps\r\n{2:F} Tps\r\n{3:F} sec\r\n{14:F} Mana\r\n{9:F} - {10:F} Hit\r\n{11:F} - {12:F} Crit{13}\r\n{4:F}x Amplify\r\n{5:F}% Crit Rate\r\n{6:F}% Hit Rate\r\n{7:F} Crit Multiplier\r\nAB Spam Tradeoff: {8:F} Dpm", bs.DamagePerSecond, bs.CostPerSecond - bs.ManaRegenPerSecond, bs.ThreatPerSecond, bs.CastTime - BaseState.Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, (AB.DamagePerSecond - bs.DamagePerSecond) / (AB.CostPerSecond - AB.ManaRegenPerSecond - bs.CostPerSecond + bs.ManaRegenPerSecond), bs.MinHitDamage / bs.HitProcs, bs.MaxHitDamage / bs.HitProcs, bs.MinCritDamage / bs.HitProcs, bs.MaxCritDamage / bs.HitProcs, ((bs.DotDamage > 0) ? ("\n" + bs.DotDamage.ToString("F") + " Dot") : ""), bs.Cost));
            float totalDamage = (CalculationOptions.TargetDamage > 0.0f) ? CalculationOptions.TargetDamage : DpsRating * CalculationOptions.FightDuration;
            dictValues.Add("Total Damage", String.Format("{0:F}", totalDamage));
            dictValues.Add("Score", String.Format("{0:F}", OverallPoints));
            dictValues.Add("Dps", String.Format("{0:F}", DpsRating));
            dictValues.Add("Tps", String.Format("{0:F}", Tps));
            dictValues.Add("Sequence", ReconstructSequence());
            StringBuilder sb = new StringBuilder("*");
            if (MageArmor != null) sb.AppendLine(MageArmor);
            Dictionary<string, double> combinedSolution = new Dictionary<string, double>();
            Dictionary<string, int> combinedSolutionData = new Dictionary<string, int>();
            double idleRegen = 0;
            double evocation = 0;
            double manaPotion = 0;
            double manaGem = 0;
            double drums = 0;
            bool segmentedOutput = false;
            Dictionary<string, SpellContribution> byspell = new Dictionary<string, SpellContribution>();
            for (int i = 0; i < SolutionVariable.Count; i++)
            {
                if (Solution[i] > 0.01)
                {
                    switch (SolutionVariable[i].Type)
                    {
                        case VariableType.IdleRegen:
                            idleRegen += Solution[i];
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F} sec", "Idle Regen", Solution[i], SolutionVariable[i].Segment));
                            break;
                        case VariableType.Evocation:
                            evocation += Solution[i];
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Evocation", Solution[i] / EvocationDuration, SolutionVariable[i].Segment));
                            break;
                        case VariableType.ManaPotion:
                            manaPotion += Solution[i];
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Mana Potion", Solution[i], SolutionVariable[i].Segment));
                            break;
                        case VariableType.ManaGem:
                            manaGem += Solution[i];
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Mana Gem", Solution[i], SolutionVariable[i].Segment));
                            break;
                        case VariableType.DrumsOfBattle:
                            drums += Solution[i];
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Drums of Battle", Solution[i] / BaseState.GlobalCooldown, SolutionVariable[i].Segment));
                            break;
                        case VariableType.Drinking:
                            sb.AppendLine(String.Format("{0}: {1:F} sec", "Drinking", Solution[i]));
                            break;
                        case VariableType.TimeExtension:
                            break;
                        case VariableType.AfterFightRegen:
                            sb.AppendLine(String.Format("{0}: {1:F} sec", "Drinking Regen", Solution[i]));
                            break;
                        case VariableType.Wand:
                        case VariableType.Spell:
                            double value;
                            Spell s = SolutionVariable[i].Spell;
                            s.AddSpellContribution(byspell, (float)Solution[i]);
                            string label = ((SolutionVariable[i].State.BuffLabel.Length > 0) ? (SolutionVariable[i].State.BuffLabel + "+") : "") + s.Name;
                            combinedSolution.TryGetValue(label, out value);
                            combinedSolution[label] = value + Solution[i];
                            combinedSolutionData[label] = i;
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F} sec", label, Solution[i], SolutionVariable[i].Segment.ToString()));
                            break;
                    }
                }
            }
            if (!segmentedOutput)
            {
                if (idleRegen > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F} sec", "Idle Regen", idleRegen));
                }
                if (evocation > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Evocation", evocation / EvocationDuration));
                }
                if (manaPotion > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Mana Potion", manaPotion));
                }
                if (manaGem > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Mana Gem", manaGem));
                }
                if (drums > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Drums of Battle", drums / BaseState.GlobalCooldown));
                }
                foreach (KeyValuePair<string, double> kvp in combinedSolution)
                {
                    Spell s = SolutionVariable[combinedSolutionData[kvp.Key]].Spell;
                    if (s != null)
                    {
                        sb.AppendLine(String.Format("{0}: {1:F} sec ({2:F} dps, {3:F} mps, {4:F} tps)", kvp.Key, kvp.Value, s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond, s.ThreatPerSecond));
                    }
                    else
                    {
                        sb.AppendLine(String.Format("{0}: {1:F} sec", kvp.Key, kvp.Value));
                    }
                }
            }
            if (WaterElemental) sb.AppendLine(String.Format("Water Elemental: {0:F}x", WaterElementalDuration / 45f));
            dictValues.Add("Spell Cycles", sb.ToString());
            sb = new StringBuilder("*");
            List<SpellContribution> contribList = new List<SpellContribution>(byspell.Values);
            contribList.Sort();
            foreach (SpellContribution contrib in contribList)
            {
                sb.AppendFormat("{0}: {1:F}%, {2:F} Damage, {3:F} Hits\n", contrib.Name, 100.0 * contrib.Damage / totalDamage, contrib.Damage, contrib.Hits);
            }
            dictValues.Add("By Spell", sb.ToString());
            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health":
                    return BaseStats.Health;
                case "Nature Resistance":
                    return BaseStats.NatureResistance;
                case "Fire Resistance":
                    return BaseStats.FireResistance;
                case "Frost Resistance":
                    return BaseStats.FrostResistance;
                case "Shadow Resistance":
                    return BaseStats.ShadowResistance;
                case "Arcane Resistance":
                    return BaseStats.ArcaneResistance;
                case "Chance to Live":
                    return 100 * (1 - ChanceToDie);
                case "Spell Hit Rating":
                    return BaseStats.SpellHitRating;
                case "Spell Haste Rating":
                    return BaseStats.SpellHasteRating;
                case "PVP Trinket":
                    return BaseStats.PVPTrinket;
                case "Movement Speed":
                    return BaseStats.MovementSpeed;
            }
            return 0;
        }
    }
}
