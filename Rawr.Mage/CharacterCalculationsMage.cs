using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

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
        public float ShadowResist { get; set; }
        public float FightDuration { get; set; }
        public float ShadowPriest { get; set; }
        public bool HeroismAvailable { get; set; }
        public bool DestructionPotion { get; set; }
        public bool FlameCap { get; set; }
        public bool ABCycles { get; set; }
        public float MoltenFuryPercentage { get; set; }
        public bool MaintainScorch { get; set; }
        public float InterruptFrequency { get; set; }
        public bool JudgementOfWisdom { get; set; }
        public float EvocationWeapon { get; set; }
        public float AoeDuration { get; set; }
        public bool SmartOptimization { get; set; }
        public float DpsTime { get; set; }
        public bool DrumsOfBattle { get; set; }
        public bool Enable2_3Mode { get; set; }

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
        public int Incinerate { get; set; }
        public int ImprovedScorch { get; set; }
        public int WintersChill { get; set; }
        public int BurningSoul { get; set; }
        public int ImprovedArcaneMissiles { get; set; }
        public int WandSpecialization { get; set; }
        public int BlastWave { get; set; }
        public int DragonsBreath { get; set; }
        public int ArcanePower { get; set; }
        public int IcyVeins { get; set; }
        public int ColdSnap { get; set; }
        public int IceFloes { get; set; }
        public int SummonWaterElemental { get; set; }
        public int ArcaneMind { get; set; }
        public int ArcaneFortitude { get; set; }
        public int MagicAbsorption { get; set; }
        public int FrostWarding { get; set; }
        public int ArcaneMeditation { get; set; }
        public int ArcaneSubtlety { get; set; }
        public int ImprovedFireBlast { get; set; }
        public int ImprovedFlamestrike { get; set; }
        public int ImprovedFrostNova { get; set; }
        public int ImprovedConeOfCold { get; set; }

        public CompiledCalculationOptions(Character character)
        {
            TargetLevel = int.Parse(character.CalculationOptions["TargetLevel"], CultureInfo.InvariantCulture);
            AoeTargetLevel = int.Parse(character.CalculationOptions["AoeTargetLevel"], CultureInfo.InvariantCulture);
            Latency = float.Parse(character.CalculationOptions["Latency"], CultureInfo.InvariantCulture);
            MageArmor = character.CalculationOptions["MageArmor"];
            AoeTargets = int.Parse(character.CalculationOptions["AoeTargets"], CultureInfo.InvariantCulture);
            ArcaneResist = float.Parse(character.CalculationOptions["ArcaneResist"], CultureInfo.InvariantCulture);
            FireResist = float.Parse(character.CalculationOptions["FireResist"], CultureInfo.InvariantCulture);
            FrostResist = float.Parse(character.CalculationOptions["FrostResist"], CultureInfo.InvariantCulture);
            NatureResist = float.Parse(character.CalculationOptions["NatureResist"], CultureInfo.InvariantCulture);
            ShadowResist = float.Parse(character.CalculationOptions["ShadowResist"], CultureInfo.InvariantCulture);
            FightDuration = float.Parse(character.CalculationOptions["FightDuration"], CultureInfo.InvariantCulture);
            ShadowPriest = float.Parse(character.CalculationOptions["ShadowPriest"], CultureInfo.InvariantCulture);
            HeroismAvailable = int.Parse(character.CalculationOptions["HeroismAvailable"], CultureInfo.InvariantCulture) == 1;
            MoltenFuryPercentage = float.Parse(character.CalculationOptions["MoltenFuryPercentage"], CultureInfo.InvariantCulture);
            DestructionPotion = int.Parse(character.CalculationOptions["DestructionPotion"], CultureInfo.InvariantCulture) == 1;
            FlameCap = int.Parse(character.CalculationOptions["FlameCap"], CultureInfo.InvariantCulture) == 1;
            ABCycles = int.Parse(character.CalculationOptions["ABCycles"], CultureInfo.InvariantCulture) == 1;
            MaintainScorch = int.Parse(character.CalculationOptions["MaintainScorch"], CultureInfo.InvariantCulture) == 1;
            InterruptFrequency = float.Parse(character.CalculationOptions["InterruptFrequency"], CultureInfo.InvariantCulture);
            JudgementOfWisdom = character.ActiveBuffs.Contains("Judgement of Wisdom");
            EvocationWeapon = float.Parse(character.CalculationOptions["EvocationWeapon"], CultureInfo.InvariantCulture);
            AoeDuration = float.Parse(character.CalculationOptions["AoeDuration"], CultureInfo.InvariantCulture);
            SmartOptimization = int.Parse(character.CalculationOptions["SmartOptimization"], CultureInfo.InvariantCulture) == 1;
            DpsTime = float.Parse(character.CalculationOptions["DpsTime"], CultureInfo.InvariantCulture);
            DrumsOfBattle = int.Parse(character.CalculationOptions["DrumsOfBattle"], CultureInfo.InvariantCulture) == 1;
            Enable2_3Mode = int.Parse(character.CalculationOptions["2_3Mode"], CultureInfo.InvariantCulture) == 1;

            Pyromaniac = int.Parse(character.CalculationOptions["Pyromaniac"], CultureInfo.InvariantCulture);
            ElementalPrecision = int.Parse(character.CalculationOptions["ElementalPrecision"], CultureInfo.InvariantCulture);
            FrostChanneling = int.Parse(character.CalculationOptions["FrostChanneling"], CultureInfo.InvariantCulture);
            MasterOfElements = int.Parse(character.CalculationOptions["MasterOfElements"], CultureInfo.InvariantCulture);
            ArcaneConcentration = int.Parse(character.CalculationOptions["ArcaneConcentration"], CultureInfo.InvariantCulture);
            MindMastery = int.Parse(character.CalculationOptions["MindMastery"], CultureInfo.InvariantCulture);
            ArcaneInstability = int.Parse(character.CalculationOptions["ArcaneInstability"], CultureInfo.InvariantCulture);
            ArcanePotency = int.Parse(character.CalculationOptions["ArcanePotency"], CultureInfo.InvariantCulture);
            ArcaneFocus = int.Parse(character.CalculationOptions["ArcaneFocus"], CultureInfo.InvariantCulture);
            PlayingWithFire = int.Parse(character.CalculationOptions["PlayingWithFire"], CultureInfo.InvariantCulture);
            MoltenFury = int.Parse(character.CalculationOptions["MoltenFury"], CultureInfo.InvariantCulture);
            FirePower = int.Parse(character.CalculationOptions["FirePower"], CultureInfo.InvariantCulture);
            PiercingIce = int.Parse(character.CalculationOptions["PiercingIce"], CultureInfo.InvariantCulture);
            SpellPower = int.Parse(character.CalculationOptions["SpellPower"], CultureInfo.InvariantCulture);
            Ignite = int.Parse(character.CalculationOptions["Ignite"], CultureInfo.InvariantCulture);
            IceShards = int.Parse(character.CalculationOptions["IceShards"], CultureInfo.InvariantCulture);
            CriticalMass = int.Parse(character.CalculationOptions["CriticalMass"], CultureInfo.InvariantCulture);
            Combustion = int.Parse(character.CalculationOptions["Combustion"], CultureInfo.InvariantCulture);
            ImprovedFrostbolt = int.Parse(character.CalculationOptions["ImprovedFrostbolt"], CultureInfo.InvariantCulture);
            EmpoweredFrostbolt = int.Parse(character.CalculationOptions["EmpoweredFrostbolt"], CultureInfo.InvariantCulture);
            ImprovedFireball = int.Parse(character.CalculationOptions["ImprovedFireball"], CultureInfo.InvariantCulture);
            EmpoweredFireball = int.Parse(character.CalculationOptions["EmpoweredFireball"], CultureInfo.InvariantCulture);
            ArcaneImpact = int.Parse(character.CalculationOptions["ArcaneImpact"], CultureInfo.InvariantCulture);
            EmpoweredArcaneMissiles = int.Parse(character.CalculationOptions["EmpoweredArcaneMissiles"], CultureInfo.InvariantCulture);
            Incinerate = int.Parse(character.CalculationOptions["Incinerate"], CultureInfo.InvariantCulture);
            ImprovedScorch = int.Parse(character.CalculationOptions["ImprovedScorch"], CultureInfo.InvariantCulture);
            WintersChill = int.Parse(character.CalculationOptions["WintersChill"], CultureInfo.InvariantCulture);
            BurningSoul = int.Parse(character.CalculationOptions["BurningSoul"], CultureInfo.InvariantCulture);
            ImprovedArcaneMissiles = int.Parse(character.CalculationOptions["ImprovedArcaneMissiles"], CultureInfo.InvariantCulture);
            WandSpecialization = int.Parse(character.CalculationOptions["WandSpecialization"], CultureInfo.InvariantCulture);
            BlastWave = int.Parse(character.CalculationOptions["BlastWave"], CultureInfo.InvariantCulture);
            DragonsBreath = int.Parse(character.CalculationOptions["DragonsBreath"], CultureInfo.InvariantCulture);
            ArcanePower = int.Parse(character.CalculationOptions["ArcanePower"], CultureInfo.InvariantCulture);
            IcyVeins = int.Parse(character.CalculationOptions["IcyVeins"], CultureInfo.InvariantCulture);
            ColdSnap = int.Parse(character.CalculationOptions["ColdSnap"], CultureInfo.InvariantCulture);
            IceFloes = int.Parse(character.CalculationOptions["IceFloes"], CultureInfo.InvariantCulture);
            SummonWaterElemental = int.Parse(character.CalculationOptions["SummonWaterElemental"], CultureInfo.InvariantCulture);
            ArcaneMind = int.Parse(character.CalculationOptions["ArcaneMind"], CultureInfo.InvariantCulture);
            ArcaneFortitude = int.Parse(character.CalculationOptions["ArcaneFortitude"], CultureInfo.InvariantCulture);
            MagicAbsorption = int.Parse(character.CalculationOptions["MagicAbsorption"], CultureInfo.InvariantCulture);
            FrostWarding = int.Parse(character.CalculationOptions["FrostWarding"], CultureInfo.InvariantCulture);
            ArcaneMeditation = int.Parse(character.CalculationOptions["ArcaneMeditation"], CultureInfo.InvariantCulture);
            ArcaneSubtlety = int.Parse(character.CalculationOptions["ArcaneSubtlety"], CultureInfo.InvariantCulture);
            ImprovedFireBlast = int.Parse(character.CalculationOptions["ImprovedFireBlast"], CultureInfo.InvariantCulture);
            ImprovedFlamestrike = int.Parse(character.CalculationOptions["ImprovedFlamestrike"], CultureInfo.InvariantCulture);
            ImprovedFrostNova = int.Parse(character.CalculationOptions["ImprovedFrostNova"], CultureInfo.InvariantCulture);
            ImprovedConeOfCold = int.Parse(character.CalculationOptions["ImprovedConeOfCold"], CultureInfo.InvariantCulture);
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
        public float GlobalCooldownLimit { get; set; }

        public float ArcaneDamage { get; set; }
        public float FireDamage { get; set; }
        public float FrostDamage { get; set; }
        public float NatureDamage { get; set; }
        public float ShadowDamage { get; set; }

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
        public float ShadowSpellModifier { get; set; }

        public float ArcaneCritBonus { get; set; }
        public float FireCritBonus { get; set; }
        public float FrostCritBonus { get; set; }
        public float NatureCritBonus { get; set; }
        public float ShadowCritBonus { get; set; }

        public float ArcaneCritRate { get; set; }
        public float FireCritRate { get; set; }
        public float FrostCritRate { get; set; }
        public float NatureCritRate { get; set; }
        public float ShadowCritRate { get; set; }

        public float ArcaneHitRate { get; set; }
        public float FireHitRate { get; set; }
        public float FrostHitRate { get; set; }
        public float NatureHitRate { get; set; }
        public float ShadowHitRate { get; set; }

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
        public bool DrumsOfBattle { get; set; }
        public bool WaterElemental { get; set; }
        public bool Combustion { get; set; }
        public float CombustionDuration { get; set; }
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

        public void SetSpell(string spellName, Spell spell)
        {
            Spells[spellName] = spell;
        }

        public Spell GetSpell(string spellName)
        {
            Spell s = null;
            if (Spells.TryGetValue(spellName, out s)) return s;

            switch (spellName)
            {
                case "Lightning Bolt":
                    s = new LightningBolt(Character, this);
                    break;
                case "Arcane Missiles":
                    s = new ArcaneMissiles(Character, this);
                    break;
                case "Arcane Missiles CC":
                    s = new ArcaneMissilesCC(Character, this);
                    break;
                case "Arcane Missiles no proc":
                    s = new ArcaneMissiles(Character, this, true, false, false);
                    break;
                case "Arcane Missiles FTF":
                    s = new ArcaneMissiles(Character, this);
                    break;
                case "Arcane Missiles FTT":
                    s = new ArcaneMissiles(Character, this);
                    break;
                case "Frostbolt":
                    s = new Frostbolt(Character, this);
                    break;
                case "Frostbolt no CC":
                    s = new Frostbolt(Character, this, false);
                    break;
                case "Fireball":
                    s = new Fireball(Character, this);
                    break;
                case "Fire Blast":
                    s = new FireBlast(Character, this);
                    break;
                case "Scorch":
                    s = new Scorch(Character, this);
                    break;
                case "Scorch no CC":
                    s = new Scorch(Character, this, false);
                    break;
                case "Arcane Blast 3,3":
                case "Arcane Blast":
                    s = new ArcaneBlast(Character, this, 3, 3);
                    break;
                case "Arcane Blast 3,3 no CC":
                    s = new ArcaneBlast(Character, this, 3, 3, false);
                    break;
                case "Arcane Blast 0,0":
                    s = new ArcaneBlast(Character, this, 0, 0);
                    break;
                case "Arcane Blast 0,0 no CC":
                    s = new ArcaneBlast(Character, this, 0, 0, false);
                    break;
                case "Arcane Blast 1,0":
                    s = new ArcaneBlast(Character, this, 1, 0);
                    break;
                case "Arcane Blast 0,1":
                    s = new ArcaneBlast(Character, this, 0, 1);
                    break;
                case "Arcane Blast 1,1":
                    s = new ArcaneBlast(Character, this, 1, 1);
                    break;
                case "Arcane Blast 1,1 no CC":
                    s = new ArcaneBlast(Character, this, 1, 1, false);
                    break;
                case "Arcane Blast 2,2":
                    s = new ArcaneBlast(Character, this, 2, 2);
                    break;
                case "Arcane Blast 2,2 no CC":
                    s = new ArcaneBlast(Character, this, 2, 2, false);
                    break;
                case "Arcane Blast 1,2":
                    s = new ArcaneBlast(Character, this, 1, 2);
                    break;
                case "Arcane Blast 2,3":
                    s = new ArcaneBlast(Character, this, 2, 3);
                    break;
                case "Arcane Blast 3,0":
                    s = new ArcaneBlast(Character, this, 3, 0);
                    break;
                case "ABAM":
                    s = new ABAM(Character, this);
                    break;
                case "ABAMP":
                    s = new ABAMP(Character, this);
                    break;
                case "AB3AMSc":
                    s = new AB3AMSc(Character, this);
                    break;
                case "ABAM3Sc":
                    s = new ABAM3Sc(Character, this);
                    break;
                case "ABAM3Sc2":
                    s = new ABAM3Sc2(Character, this);
                    break;
                case "ABAM3FrB":
                    s = new ABAM3FrB(Character, this);
                    break;
                case "ABAM3FrB2":
                    s = new ABAM3FrB2(Character, this);
                    break;
                case "AB3FrB":
                    s = new AB3FrB(Character, this);
                    break;
                case "ABFrB3FrB":
                    s = new ABFrB3FrB(Character, this);
                    break;
                case "ABFrB3FrB2":
                    s = new ABFrB3FrB2(Character, this);
                    break;
                case "ABFrB3FrBSc":
                    s = new ABFrB3FrBSc(Character, this);
                    break;
                case "ABFB3FBSc":
                    s = new ABFB3FBSc(Character, this);
                    break;
                case "AB3Sc":
                    s = new AB3Sc(Character, this);
                    break;
                case "FireballScorch":
                    s = new FireballScorch(Character, this);
                    break;
                case "FireballFireBlast":
                    s = new FireballFireBlast(Character, this);
                    break;
                case "ABAM3ScCCAM":
                    s = new ABAM3ScCCAM(Character, this);
                    break;
                case "ABAM3Sc2CCAM":
                    s = new ABAM3Sc2CCAM(Character, this);
                    break;
                case "ABAM3FrBCCAM":
                    s = new ABAM3FrBCCAM(Character, this);
                    break;
                case "ABAM3FrBScCCAM":
                    s = new ABAM3FrBScCCAM(Character, this);
                    break;
                case "ABAMCCAM":
                    s = new ABAMCCAM(Character, this);
                    break;
                case "ABAM3CCAM":
                    s = new ABAM3CCAM(Character, this);
                    break;
                case "Arcane Explosion":
                    s = new ArcaneExplosion(Character, this);
                    break;
                case "Flamestrike (spammed)":
                    s = new Flamestrike(Character, this, true);
                    break;
                case "Flamestrike (single)":
                    s = new Flamestrike(Character, this, false);
                    break;
                case "Blizzard":
                    s = new Blizzard(Character, this);
                    break;
                case "Blast Wave":
                    s = new BlastWave(Character, this);
                    break;
                case "Dragon's Breath":
                    s = new DragonsBreath(Character, this);
                    break;
                case "Cone of Cold":
                    s = new ConeOfCold(Character, this);
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
            dictValues.Add("Spell Crit Rate", String.Format("{0:F}%*{1} Spell Crit Rating", 100 * SpellCrit, BasicStats.SpellCritRating));
            dictValues.Add("Spell Hit Rate", String.Format("{0:F}%*{1} Spell Hit Rating", 100 * SpellHit, BasicStats.SpellHitRating));
            dictValues.Add("Spell Penetration", BasicStats.SpellPenetration.ToString());
            dictValues.Add("Casting Speed", String.Format("{0}*{1} Spell Haste Rating", CastingSpeed, BasicStats.SpellHasteRating));
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
            List<string> spellList = new List<string>() { "Wand", "Arcane Missiles", "Scorch", "Fireball", "Frostbolt", "Arcane Blast", "ABAMP", "ABAM", "AB3AMSc", "ABAM3Sc", "ABAM3Sc2", "ABAM3FrB", "ABAM3FrB2", "ABFrB3FrB", "ABFrB3FrBSc", "ABFB3FBSc", "FireballScorch", "FireballFireBlast", "Fire Blast", "ABAM3ScCCAM", "ABAM3Sc2CCAM", "ABAM3FrBCCAM", "ABAM3FrBScCCAM", "ABAMCCAM", "ABAM3CCAM", "Arcane Explosion", "Flamestrike (spammed)", "Blizzard", "Blast Wave", "Dragon's Breath", "Cone of Cold" };
            Spell AB = GetSpell("Arcane Blast");
            foreach (string spell in spellList)
            {
                Spell s = GetSpell(spell);
                if (s != null)
                {
                    if (s is BaseSpell)
                    {
                        BaseSpell bs = s as BaseSpell;
                        dictValues.Add(s.Name, String.Format("{0:F} Dps*{1:F} Mps\n{2:F} sec\n{3:F}x Amplify\n{4:F}% Crit Rate\n{5:F}% Hit Rate\n{6:F} Crit Multiplier\nAB Spam Tradeoff: {7:F} Dpm", s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond, bs.CastTime - Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, (AB.DamagePerSecond - s.DamagePerSecond) / (AB.CostPerSecond - AB.ManaRegenPerSecond - s.CostPerSecond + s.ManaRegenPerSecond)));
                    }
                    else
                    {
                        dictValues.Add(s.Name, String.Format("{0:F} Dps*{1:F} Mps\nAB Spam Tradeoff: {2:F} Dpm\nAverage Cast Time: {3:F} sec\n{4}", s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond, (AB.DamagePerSecond - s.DamagePerSecond) / (AB.CostPerSecond - AB.ManaRegenPerSecond - s.CostPerSecond + s.ManaRegenPerSecond), s.CastTime, s.Sequence));
                    }
                }
            }
            dictValues.Add("Total Damage", String.Format("{0:F}", OverallPoints * FightDuration));
            dictValues.Add("Dps", String.Format("{0:F}", OverallPoints));
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
                        case 5:
                            sb.AppendLine(String.Format("{0}: {1:F}x", SolutionLabel[i], Solution[i] / GlobalCooldown));
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
