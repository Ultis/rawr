using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Mage
{
    class CompiledCalculationOptions
    {
        public int TargetLevel { get; set; }
        public int AoeTargetLevel { get; set; }
        public float Latency { get; set; }
        public string MageArmor { get; set; }
        public int AoeTargets { get; set; }
        public float ArcaneResist { get; set; }
        public float FireResist { get; set; }
        public float FrostResist { get; set; }
        public float NatureResist { get; set; }
        public float FightDuration { get; set; }
        public float ShadowPriest { get; set; }
        public bool HeroismAvailable { get; set; }
        public float MoltenFuryPercentage { get; set; }

        public int Pyromaniac { get; set; }
        public int ElementalPrecision { get; set; }
        public int FrostChanneling { get; set; }
        public int MasterOfElements { get; set; }
        public int ArcaneConcentration { get; set; }
        public int MindMastery { get; set; }
        public int ArcaneInstability { get; set; }
        public int ArcanePotency { get; set; }
        public int ArcaneFocus { get; set; }
        public int PlayingWithFire { get; set; }
        public int MoltenFury { get; set; }
        public int FirePower { get; set; }
        public int PiercingIce { get; set; }
        public int SpellPower { get; set; }
        public int Ignite { get; set; }
        public int IceShards { get; set; }
        public int CriticalMass { get; set; }
        public int Combustion { get; set; }
        public int ImprovedFrostbolt { get; set; }
        public int EmpoweredFrostbolt { get; set; }
        public int ImprovedFireball { get; set; }
        public int EmpoweredFireball { get; set; }
        public int ArcaneImpact { get; set; }
        public int EmpoweredArcaneMissiles { get; set; }

        public CompiledCalculationOptions(Character character)
        {
            TargetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
            AoeTargetLevel = int.Parse(character.CalculationOptions["AoeTargetLevel"]);
            Latency = float.Parse(character.CalculationOptions["Latency"]);
            MageArmor = character.CalculationOptions["MageArmor"];
            AoeTargets = int.Parse(character.CalculationOptions["AoeTargets"]);
            ArcaneResist = float.Parse(character.CalculationOptions["ArcaneResist"]);
            FireResist = float.Parse(character.CalculationOptions["FireResist"]);
            FrostResist = float.Parse(character.CalculationOptions["FrostResist"]);
            NatureResist = float.Parse(character.CalculationOptions["NatureResist"]);
            FightDuration = float.Parse(character.CalculationOptions["FightDuration"]);
            ShadowPriest = float.Parse(character.CalculationOptions["ShadowPriest"]);
            HeroismAvailable = character.CalculationOptions["HeroismAvailable"] == "1";
            MoltenFuryPercentage = float.Parse(character.CalculationOptions["MoltenFuryPercentage"]);

            Pyromaniac = int.Parse(character.CalculationOptions["Pyromaniac"]);
            ElementalPrecision = int.Parse(character.CalculationOptions["ElementalPrecision"]);
            FrostChanneling = int.Parse(character.CalculationOptions["FrostChanneling"]);
            MasterOfElements = int.Parse(character.CalculationOptions["MasterOfElements"]);
            ArcaneConcentration = int.Parse(character.CalculationOptions["ArcaneConcentration"]);
            MindMastery = int.Parse(character.CalculationOptions["MindMastery"]);
            ArcaneInstability = int.Parse(character.CalculationOptions["ArcaneInstability"]);
            ArcanePotency = int.Parse(character.CalculationOptions["ArcanePotency"]);
            ArcaneFocus = int.Parse(character.CalculationOptions["ArcaneFocus"]);
            PlayingWithFire = int.Parse(character.CalculationOptions["PlayingWithFire"]);
            MoltenFury = int.Parse(character.CalculationOptions["MoltenFury"]);
            FirePower = int.Parse(character.CalculationOptions["FirePower"]);
            PiercingIce = int.Parse(character.CalculationOptions["PiercingIce"]);
            SpellPower = int.Parse(character.CalculationOptions["SpellPower"]);
            Ignite = int.Parse(character.CalculationOptions["Ignite"]);
            IceShards = int.Parse(character.CalculationOptions["IceShards"]);
            CriticalMass = int.Parse(character.CalculationOptions["CriticalMass"]);
            Combustion = int.Parse(character.CalculationOptions["Combustion"]);
            ImprovedFrostbolt = int.Parse(character.CalculationOptions["ImprovedFrostbolt"]);
            EmpoweredFrostbolt = int.Parse(character.CalculationOptions["EmpoweredFrostbolt"]);
            ImprovedFireball = int.Parse(character.CalculationOptions["ImprovedFireball"]);
            EmpoweredFireball = int.Parse(character.CalculationOptions["EmpoweredFireball"]);
            ArcaneImpact = int.Parse(character.CalculationOptions["ArcaneImpact"]);
            EmpoweredArcaneMissiles = int.Parse(character.CalculationOptions["EmpoweredArcaneMissiles"]);
        }
    }

    class CharacterCalculationsMage : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        private Stats _basicStats;
        public Stats BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
        }

        public CompiledCalculationOptions CalculationOptions { get; set; }

        public Character Character { get; set; }

        public float SpellCrit { get; set; }
        public float SpellHit { get; set; }
        public float CastingSpeed { get; set; }
        public float GlobalCooldown { get; set; }
        public float ArcaneDamage { get; set; }
        public float FireDamage { get; set; }
        public float FrostDamage { get; set; }
        public float NatureDamage { get; set; }
        public float SpiritRegen { get; set; }
        public float ManaRegen { get; set; }
        public float ManaRegen5SR { get; set; }
        public float ManaRegenDrinking { get; set; }
        public float HealthRegen { get; set; }
        public float HealthRegenCombat { get; set; }
        public float HealthRegenEating { get; set; }
        public float MeleeMitigation { get; set; }
        public float Defense { get; set; }
        public float PhysicalCritReduction { get; set; }
        public float SpellCritReduction { get; set; }
        public float CritDamageReduction { get; set; }
        public float Dodge { get; set; }
        public float ArcaneSpellModifier { get; set; }
        public float FireSpellModifier { get; set; }
        public float FrostSpellModifier { get; set; }
        public float NatureSpellModifier { get; set; }
        public float ArcaneCritBonus { get; set; }
        public float NatureCritBonus { get; set; }
        public float FireCritBonus { get; set; }
        public float FrostCritBonus { get; set; }
        public float ArcaneCritRate { get; set; }
        public float NatureCritRate { get; set; }
        public float FireCritRate { get; set; }
        public float FrostCritRate { get; set; }
        public float ArcaneHitRate { get; set; }
        public float FireHitRate { get; set; }
        public float FrostHitRate { get; set; }
        public float NatureHitRate { get; set; }
        public float ResilienceCritDamageReduction { get; set; }
        public float ResilienceCritRateReduction { get; set; }
        public float Latency { get; set; }
        public float FightDuration { get; set; }
        public float ClearcastingChance { get; set; }

        public bool ArcanePower { get; set; }
        public bool IcyVeins { get; set; }
        public bool MoltenFury { get; set; }
        public bool Heroism { get; set; }
        public bool DestructionPotion { get; set; }
        public bool FlameCap { get; set; }
        public bool Trinket1 { get; set; }
        public bool Trinket2 { get; set; }
        public bool WaterElemental { get; set; }
        public float Mp5OnCastFor20Sec { get; set; }

        public float WaterElementalDps { get; set; }
        public float WaterElementalDuration { get; set; }
        public float WaterElementalDamage { get; set; }

        public string BuffLabel { get; set; }

        private Dictionary<string, Spell> Spells = new Dictionary<string, Spell> ();
        public double EvocationDuration;
        public double ManaPotionTime = 0.1f;
        public int MaxManaPotion;
        public int MaxManaGem;
        public List<string> SolutionLabel = new List<string>();
        public double[] Solution;

        public Spell GetSpell(string spellName)
        {
            if (Spells.ContainsKey(spellName)) return Spells[spellName];
            Spell s = null;

            switch (spellName)
            {
                case "Lightning Bolt":
                    s = new LightningBolt(Character, this);
                    break;
                case "Arcane Missiles":
                    s = new ArcaneMissiles(Character, this);
                    break;
                case "Frostbolt":
                    s = new Frostbolt(Character, this);
                    break;
                case "Fireball":
                    s = new Fireball(Character, this);
                    break;
                case "Arcane Blast (spam)":
                    s = new ArcaneBlast(Character, this, 3, 3);
                    break;
            }
            if (s != null) Spells[spellName] = s;

            return s;
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", BasicStats.Spirit.ToString());
            dictValues.Add("Armor", BasicStats.Armor.ToString());
            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Spell Crit Rate", String.Format("{0:F}%", 100 * SpellCrit));
            dictValues.Add("Spell Hit Rate", String.Format("{0:F}%", 100 * SpellHit));
            dictValues.Add("Spell Penetration", BasicStats.SpellPenetration.ToString());
            dictValues.Add("Casting Speed", CastingSpeed.ToString());
            dictValues.Add("Arcane Damage", ArcaneDamage.ToString());
            dictValues.Add("Fire Damage", FireDamage.ToString());
            dictValues.Add("Frost Damage", FrostDamage.ToString());
            dictValues.Add("MP5", BasicStats.Mp5.ToString());
            dictValues.Add("Mana Regen", Math.Floor(ManaRegen * 5).ToString() + String.Format("*Mana Regen in 5SR: {0}\nMana Regen Drinking: {1}", Math.Floor(ManaRegen5SR * 5), Math.Floor(ManaRegenDrinking * 5)));
            dictValues.Add("Health Regen", Math.Floor(HealthRegenCombat * 5).ToString() + String.Format("*Health Regen Eating: {0}", Math.Floor(HealthRegenEating * 5)));
            dictValues.Add("Arcane Resist", (BasicStats.AllResist + BasicStats.ArcaneResistance).ToString());
            dictValues.Add("Fire Resist", (BasicStats.AllResist + BasicStats.FireResistance).ToString());
            dictValues.Add("Nature Resist", (BasicStats.AllResist + BasicStats.NatureResistance).ToString());
            dictValues.Add("Frost Resist", (BasicStats.AllResist + BasicStats.FrostResistance).ToString());
            dictValues.Add("Shadow Resist", (BasicStats.AllResist + BasicStats.ShadowResistance).ToString());
            dictValues.Add("Physical Mitigation", String.Format("{0:F}%", 100 * MeleeMitigation));
            dictValues.Add("Resilience", BasicStats.Resilience.ToString());
            dictValues.Add("Defense", Defense.ToString());
            dictValues.Add("Crit Reduction", String.Format("{0:F}%*Spell Crit Reduction: {0:F}%\nPhysical Crit Reduction: {1:F}%\nCrit Damage Reduction: {2:F}%", SpellCritReduction * 100, PhysicalCritReduction * 100, CritDamageReduction * 100));
            dictValues.Add("Dodge", String.Format("{0:F}%", 100 * Dodge));
            Spell s = GetSpell("Arcane Missiles");
            dictValues.Add("Arcane Missiles", String.Format("{0:F} Dps*{1:F} Mps", s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond));
            s = GetSpell("Arcane Blast (spam)");
            dictValues.Add("Arcane Blast", String.Format("{0:F} Dps*{1:F} Mps", s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond));
            s = GetSpell("Fireball");
            dictValues.Add("Fireball", String.Format("{0:F} Dps*{1:F} Mps", s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond));
            s = GetSpell("Frostbolt");
            dictValues.Add("Frostbolt", String.Format("{0:F} Dps*{1:F} Mps", s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond));
            dictValues.Add("Total Damage", String.Format("{0:F}", OverallPoints));
            dictValues.Add("Dps", String.Format("{0:F}", OverallPoints / FightDuration));
            StringBuilder sb = new StringBuilder("*");
            for (int i = 0; i < SolutionLabel.Count; i++)
            {
                if (Solution[i] > 0.01)
                {
                    switch (i)
                    {
                        case 2:
                            sb.AppendLine(String.Format("{0}: {1:F}x", SolutionLabel[i], Solution[i] / EvocationDuration));
                            break;
                        case 3:
                        case 4:
                            sb.AppendLine(String.Format("{0}: {1:F}x", SolutionLabel[i], Solution[i] / ManaPotionTime));
                            break;
                        default:
                            sb.AppendLine(String.Format("{0}: {1:F} sec", SolutionLabel[i], Solution[i]));
                            break;
                    }
                }
            }
            if (WaterElemental) sb.AppendLine(String.Format("Water Elemental: {0:F}x", WaterElementalDuration / 45f));
            dictValues.Add("Spell Cycles", sb.ToString());
            return dictValues;
        }
    }
}
