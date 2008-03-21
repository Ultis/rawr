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
        public int AoeTargets { get; set; }
        public float ArcaneResist { get; set; }
        public float FireResist { get; set; }
        public float FrostResist { get; set; }
        public float NatureResist { get; set; }
        public float ShadowResist { get; set; }
        public float FightDuration { get; set; }
        public float TpsLimit { get; set; }
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
        public bool DisableBuffAutoActivation { get; set; }
        public bool AutomaticArmor { get; set; }
        public bool IncrementalOptimizations { get; set; }
        public int[] IncrementalSetCooldowns;
        public string[] IncrementalSetSpells;
        public string IncrementalSetArmor;

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
            AoeTargets = int.Parse(character.CalculationOptions["AoeTargets"], CultureInfo.InvariantCulture);
            ArcaneResist = float.Parse(character.CalculationOptions["ArcaneResist"], CultureInfo.InvariantCulture);
            FireResist = float.Parse(character.CalculationOptions["FireResist"], CultureInfo.InvariantCulture);
            FrostResist = float.Parse(character.CalculationOptions["FrostResist"], CultureInfo.InvariantCulture);
            NatureResist = float.Parse(character.CalculationOptions["NatureResist"], CultureInfo.InvariantCulture);
            ShadowResist = float.Parse(character.CalculationOptions["ShadowResist"], CultureInfo.InvariantCulture);
            FightDuration = float.Parse(character.CalculationOptions["FightDuration"], CultureInfo.InvariantCulture);
            TpsLimit = float.Parse(character.CalculationOptions["TpsLimit"], CultureInfo.InvariantCulture);
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
            DisableBuffAutoActivation = character.CalculationOptions.ContainsKey("DisableBuffAutoActivation") && character.CalculationOptions["DisableBuffAutoActivation"] == "Yes";
            AutomaticArmor = int.Parse(character.CalculationOptions["AutomaticArmor"], CultureInfo.InvariantCulture) == 1;
            IncrementalOptimizations = int.Parse(character.CalculationOptions["IncrementalOptimizations"], CultureInfo.InvariantCulture) == 1;
            if (character.CalculationOptions.ContainsKey("IncrementalSetCooldowns"))
            {
                string[] cooldowns = character.CalculationOptions["IncrementalSetCooldowns"].Split(':');
                if (character.CalculationOptions["IncrementalSetCooldowns"] == "") cooldowns = new string[] { };
                IncrementalSetCooldowns = new int[cooldowns.Length];
                for (int i = 0; i < cooldowns.Length; i++)
                {
                    IncrementalSetCooldowns[i] = int.Parse(cooldowns[i], CultureInfo.InvariantCulture);
                }
            }
            if (character.CalculationOptions.ContainsKey("IncrementalSetSpells"))
            {
                IncrementalSetSpells = character.CalculationOptions["IncrementalSetSpells"].Split(':');
            }
            if (character.CalculationOptions.ContainsKey("IncrementalSetArmor"))
            {
                IncrementalSetArmor = character.CalculationOptions["IncrementalSetArmor"];
            }

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

        public int IncrementalSetIndex { get; set; }
        public int[] IncrementalSetCooldown { get; set; }
        public string[] IncrementalSetSpell { get; set; }

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

        public float ArcaneThreatMultiplier { get; set; }
        public float FireThreatMultiplier { get; set; }
        public float FrostThreatMultiplier { get; set; }
        public float NatureThreatMultiplier { get; set; }
        public float ShadowThreatMultiplier { get; set; }

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
        public string MageArmor { get; set; }

        private Dictionary<string, Spell> Spells = new Dictionary<string, Spell> ();
        public double EvocationDuration;
        public double EvocationRegen;
        public double ManaPotionTime = 0.1f;
        public double Trinket1Duration;
        public double Trinket1Cooldown;
        public double Trinket2Duration;
        public double Trinket2Cooldown;
        public int MaxManaPotion;
        public int MaxManaGem;
        public List<string> SolutionLabel = new List<string>();
        public double[] Solution;
        public CharacterCalculationsMage[] SolutionStats;
        public Spell[] SolutionSpells;
        public float Tps;

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

        private string TimeFormat(double time)
        {
            TimeSpan span = new TimeSpan((long)(Math.Round(time, 2) / 0.0000001));
            return span.ToString();
        }

        public string ReconstructSequence()
        {
            if (FightDuration > 900) return "*Unavailable";
            int N = 0;
            List<CharacterCalculationsMage> stats = new List<CharacterCalculationsMage>();
            List<Spell> spells = new List<Spell>();
            List<double> remainingTime = new List<double>();
            for (int i = 0; i < SolutionLabel.Count; i++)
            {
                if (Solution[i] > 0.01)
                {
                    if (SolutionStats[i] != null)
                    {
                        stats.Add(SolutionStats[i]);
                        spells.Add(SolutionSpells[i]);
                        remainingTime.Add(Solution[i]);
                        N++;
                    }
                }
            }

            List<int> sequence = new List<int>();
            string bestTiming = "*Invalid";
            double bestUnderused = FightDuration;
            string[] effectList = new string[] { "Mana Potion", "Mana Gem", "Evocation", "Drums of Battle", "Flame Cap", "Destruction Potion", (Character.Trinket1 != null) ? Character.Trinket1.Name : "", (Character.Trinket2 != null) ? Character.Trinket2.Name : "", "Heroism", "Molten Fury", "Combustion", "Arcane Power", "Icy Veins"};
            float[] gemValue = new float[] { 2400f, 2400f, 2400f, 1100f, 850f };
            int E = effectList.Length;
            bool lastSequenceValid = true;
            StringBuilder timing = new StringBuilder();

            do
            {
                // move to next sequence
                if (lastSequenceValid)
                {
                    //sequence.Add(0);
                    sequence.Add(3);
                }
                else
                {
                    // pop last effect and move on
                    sequence[sequence.Count - 1]++;
                    while (sequence.Count > 0 && sequence[sequence.Count - 1] >= E)
                    {
                        sequence.RemoveAt(sequence.Count - 1);
                        if (sequence.Count > 0) sequence[sequence.Count - 1]++;
                    }
                    if (sequence.Count == 0) break;
                }
                // evaluate sequence
                lastSequenceValid = true;
                double time = 0;
                double mana = BasicStats.Mana;
                int gemCount = 0;
                double potionCooldown = 0;
                double gemCooldown = 0;
                double trinket1Cooldown = 0;
                double trinket2Cooldown = 0;
                bool heroismUsed = false;
                bool moltenFuryUsed = false;
                double evocationCooldown = 0;
                double drumsCooldown = 0;
                double apCooldown = 0;
                double ivCooldown = 0;
                double combustionCooldown = 0;
                double trinket1time = -1;
                double trinket2time = -1;
                double flameCapTime = -1;
                double drumsTime = -1;
                double destructionTime = -1;
                double combustionTime = -1;
                double moltenFuryTime = -1;
                double heroismTime = -1;
                double apTime = -1;
                double ivTime = -1;
                double[] remaining = remainingTime.ToArray();
                double[] remainingBase = new double[] { Solution[0], Solution[1], Solution[2], Solution[3], Solution[4], Solution[5] };
                timing.Length = 0;
                timing.Append("*");
                string lastSpell = null;
                double fight = FightDuration;
                bool timeLimitReached = false;
                for (int e = 0; e <= sequence.Count; e++)
                {
                    int effect = -1;
                    if (e < sequence.Count)
                    {
                        effect = sequence[e];
                    }
                    else
                    {
                        if (remainingBase[0] < 0 && -remainingBase[0] > bestUnderused) lastSequenceValid = false;
                        if (time >= fight) timeLimitReached = true;
                    }
                    bool effectApplied = false;
                    do
                    {
                        double minTimeNeeded = 0;
                        switch (effect)
                        {
                            case -1: // No effect, run remaining time
                                if (time >= fight) effectApplied = true;
                                minTimeNeeded = fight - time;
                                break;
                            case 0: // Mana Potion
                                if (remainingBase[3] <= 0)
                                {
                                    lastSequenceValid = false;
                                }
                                else if (potionCooldown > 0)
                                {
                                    minTimeNeeded = potionCooldown;
                                }
                                else if (mana <= BasicStats.Mana - (1 + BasicStats.BonusManaPotion) * 2400f)
                                {
                                    timing.AppendLine(TimeFormat(time) + ": " + effectList[effect] + " (" + Math.Round(mana).ToString() + " mana)");
                                    mana += (1 + BasicStats.BonusManaPotion) * 2400f;
                                    potionCooldown = 120;
                                    remainingBase[3] -= ManaPotionTime;
                                    fight -= ManaPotionTime;
                                    effectApplied = true;
                                    lastSpell = null;
                                }
                                else
                                {
                                    float highestMps = 0;
                                    for (int i = 0; i < N; i++)
                                    {
                                        if ((stats[i].ArcanePower != apTime < 0) && (stats[i].MoltenFury != moltenFuryTime < 0) && (stats[i].IcyVeins != ivTime < 0) && (stats[i].Heroism != heroismTime < 0) && (stats[i].DestructionPotion != destructionTime < 0) && (stats[i].FlameCap != flameCapTime < 0) && (stats[i].Trinket1 != trinket1time < 0) && (stats[i].Trinket2 != trinket2time < 0) && (stats[i].Combustion != combustionTime < 0) && (stats[i].DrumsOfBattle != drumsTime < 0))
                                        {
                                            if (remaining[i] > 0 && spells[i].CostPerSecond - spells[i].ManaRegenPerSecond > highestMps)
                                            {
                                                highestMps = spells[i].CostPerSecond - spells[i].ManaRegenPerSecond;
                                            }
                                        }
                                    }
                                    if (highestMps > 0)
                                    {
                                        minTimeNeeded = (mana - (BasicStats.Mana - (1 + BasicStats.BonusManaPotion) * 2400f)) / highestMps;
                                    }
                                    else
                                    {
                                        lastSequenceValid = false;
                                    }
                                }
                                break;
                            case 1: // Mana Gem
                                if (remainingBase[4] <= 0)
                                {
                                    lastSequenceValid = false;
                                }
                                if (gemCooldown > 0)
                                {
                                    minTimeNeeded = gemCooldown;
                                }
                                else if (mana <= BasicStats.Mana - (1 + BasicStats.BonusManaGem) * gemValue[gemCount])
                                {
                                    timing.AppendLine(TimeFormat(time) + ": " + effectList[effect] + " (" + Math.Round(mana).ToString() + " mana)");
                                    mana += (1 + BasicStats.BonusManaGem) * gemValue[gemCount];
                                    gemCooldown = 120;
                                    fight -= ManaPotionTime;
                                    remainingBase[4] -= ManaPotionTime;
                                    effectApplied = true;
                                    lastSpell = null;
                                }
                                else
                                {
                                    float highestMps = 0;
                                    for (int i = 0; i < N; i++)
                                    {
                                        if ((stats[i].ArcanePower != apTime < 0) && (stats[i].MoltenFury != moltenFuryTime < 0) && (stats[i].IcyVeins != ivTime < 0) && (stats[i].Heroism != heroismTime < 0) && (stats[i].DestructionPotion != destructionTime < 0) && (stats[i].FlameCap != flameCapTime < 0) && (stats[i].Trinket1 != trinket1time < 0) && (stats[i].Trinket2 != trinket2time < 0) && (stats[i].Combustion != combustionTime < 0) && (stats[i].DrumsOfBattle != drumsTime < 0))
                                        {
                                            if (remaining[i] > 0 && spells[i].CostPerSecond - spells[i].ManaRegenPerSecond > highestMps)
                                            {
                                                highestMps = spells[i].CostPerSecond - spells[i].ManaRegenPerSecond;
                                            }
                                        }
                                    }
                                    if (highestMps > 0)
                                    {
                                        minTimeNeeded = (mana - (BasicStats.Mana - (1 + BasicStats.BonusManaGem) * gemValue[gemCount])) / highestMps;
                                    }
                                    else
                                    {
                                        lastSequenceValid = false;
                                    }
                                }
                                break;
                            case 2: // Evocation
                                if (evocationCooldown > 0)
                                {
                                    minTimeNeeded = evocationCooldown;
                                }
                                else if (mana <= BasicStats.Mana - Math.Min(EvocationDuration, remainingBase[2]) * EvocationRegen)
                                {
                                    double move = Math.Min(EvocationDuration, remainingBase[2]);
                                    timing.AppendLine(TimeFormat(time) + ": " + effectList[effect] + " (" + Math.Round(mana).ToString() + " mana)");
                                    mana += move * EvocationRegen;
                                    evocationCooldown = 60 * 8;
                                    remainingBase[2] -= move;
                                    effectApplied = true;


                                    lastSpell = null;

                                    if (apTime >= 0 && 15 - (time - apTime) <= move) apTime = -1;
                                    if (ivTime >= 0 && 20 - (time - ivTime) <= move) ivTime = -1;
                                    if (heroismTime >= 0 && 40 - (time - heroismTime) <= move) heroismTime = -1;
                                    if (destructionTime >= 0 && 15 - (time - destructionTime) <= move) destructionTime = -1;
                                    if (flameCapTime >= 0 && 60 - (time - flameCapTime) <= move) flameCapTime = -1;
                                    if (trinket1time >= 0 && Trinket1Duration - (time - trinket1time) <= move) trinket1time = -1;
                                    if (trinket2time >= 0 && Trinket2Duration - (time - trinket2time) <= move) trinket2time = -1;
                                    if (drumsTime >= 0 && 30 - (time - drumsTime) <= move) drumsTime = -1;

                                    time += move;

                                    apCooldown -= move;
                                    ivCooldown -= move;
                                    potionCooldown -= move;
                                    gemCooldown -= move;
                                    trinket1Cooldown -= move;
                                    trinket2Cooldown -= move;
                                    combustionCooldown -= move;
                                    drumsCooldown -= move;
                                }
                                else
                                {
                                    float highestMps = 0;
                                    for (int i = 0; i < N; i++)
                                    {
                                        if ((stats[i].ArcanePower != apTime < 0) && (stats[i].MoltenFury != moltenFuryTime < 0) && (stats[i].IcyVeins != ivTime < 0) && (stats[i].Heroism != heroismTime < 0) && (stats[i].DestructionPotion != destructionTime < 0) && (stats[i].FlameCap != flameCapTime < 0) && (stats[i].Trinket1 != trinket1time < 0) && (stats[i].Trinket2 != trinket2time < 0) && (stats[i].Combustion != combustionTime < 0) && (stats[i].DrumsOfBattle != drumsTime < 0))
                                        {
                                            if (remaining[i] > 0 && spells[i].CostPerSecond - spells[i].ManaRegenPerSecond > highestMps)
                                            {
                                                highestMps = spells[i].CostPerSecond - spells[i].ManaRegenPerSecond;
                                            }
                                        }
                                    }
                                    if (highestMps > 0)
                                    {
                                        minTimeNeeded = (mana - (BasicStats.Mana - Math.Min(EvocationDuration, remainingBase[2]) * EvocationRegen)) / highestMps;
                                    }
                                    else
                                    {
                                        lastSequenceValid = false;
                                    }
                                }
                                break;
                            case 3: // Drums of Battle
                                lastSequenceValid = false;
                                break;
                            case 4: // Flame Cap
                                lastSequenceValid = false;
                                break;
                            case 5: // Destruction Potion
                                lastSequenceValid = false;
                                break;
                            case 6: // Trinket1
                                if (Trinket1Duration == 0)
                                {
                                    lastSequenceValid = false;
                                }
                                else if (trinket1Cooldown > 0)
                                {
                                    minTimeNeeded = trinket1Cooldown;
                                }
                                else
                                {
                                    trinket1Cooldown = Trinket1Cooldown;
                                    trinket1time = time;
                                    effectApplied = true;
                                    lastSpell = null;
                                    timing.AppendLine(TimeFormat(time) + ": " + effectList[effect] + " (" + Math.Round(mana).ToString() + " mana)");
                                }
                                break;
                            case 7: // Trinket2
                                if (Trinket2Duration == 0)
                                {
                                    lastSequenceValid = false;
                                }
                                if (trinket2Cooldown > 0)
                                {
                                    minTimeNeeded = trinket2Cooldown;
                                }
                                else
                                {
                                    trinket2Cooldown = Trinket2Cooldown;
                                    trinket2time = time;
                                    effectApplied = true;
                                    lastSpell = null;
                                    timing.AppendLine(TimeFormat(time) + ": " + effectList[effect] + " (" + Math.Round(mana).ToString() + " mana)");
                                }
                                break;
                            case 8: // Heroism
                                if (heroismUsed)
                                {
                                    lastSequenceValid = false;
                                }
                                else
                                {
                                    heroismUsed = true;
                                    heroismTime = time;
                                    effectApplied = true;
                                    lastSpell = null;
                                    timing.AppendLine(TimeFormat(time) + ": " + effectList[effect] + " (" + Math.Round(mana).ToString() + " mana)");
                                }
                                break;
                            case 9: // Molten Fury
                                lastSequenceValid = false;
                                break;
                            case 10: // Combustion
                                lastSequenceValid = false;
                                break;
                            case 11: // Arcane Power
                                if (CalculationOptions.ArcanePower == 0)
                                {
                                    lastSequenceValid = false;
                                }
                                else if (apCooldown > 0)
                                {
                                    minTimeNeeded = apCooldown;
                                }
                                else
                                {
                                    apCooldown = 180;
                                    apTime = time;
                                    effectApplied = true;
                                    lastSpell = null;
                                    timing.AppendLine(TimeFormat(time) + ": " + effectList[effect] + " (" + Math.Round(mana).ToString() + " mana)");
                                }
                                break;
                            case 12: // Icy Veins
                                if (CalculationOptions.IcyVeins == 0)
                                {
                                    lastSequenceValid = false;
                                }
                                if (ivCooldown > 0)
                                {
                                    minTimeNeeded = ivCooldown;
                                }
                                else
                                {
                                    ivCooldown = 180;
                                    ivTime = time;
                                    effectApplied = true;
                                    lastSpell = null;
                                    timing.AppendLine(TimeFormat(time) + ": " + effectList[effect] + " (" + Math.Round(mana).ToString() + " mana)");
                                }
                                break;
                        }
                        if (minTimeNeeded > 0)
                        {
                            // progress time
                            if (fight - time < minTimeNeeded) minTimeNeeded = fight - time;
                            float highestMps = float.NegativeInfinity;
                            float lowestMps = float.PositiveInfinity;
                            Spell spell = null;
                            CharacterCalculationsMage stat = null;
                            int selectedIndex = -1;
                            double selectedRemaining = 0;
                            for (int i = 0; i < N; i++)
                            {
                                if ((stats[i].ArcanePower != apTime < 0) && (stats[i].MoltenFury != moltenFuryTime < 0) && (stats[i].IcyVeins != ivTime < 0) && (stats[i].Heroism != heroismTime < 0) && (stats[i].DestructionPotion != destructionTime < 0) && (stats[i].FlameCap != flameCapTime < 0) && (stats[i].Trinket1 != trinket1time < 0) && (stats[i].Trinket2 != trinket2time < 0) && (stats[i].Combustion != combustionTime < 0) && (stats[i].DrumsOfBattle != drumsTime < 0))
                                {
                                    if (remaining[i] > 0 && (mana > 0 || spells[i].CostPerSecond - spells[i].ManaRegenPerSecond < 0) && spells[i].CostPerSecond - spells[i].ManaRegenPerSecond > highestMps)
                                    {
                                        highestMps = spells[i].CostPerSecond - spells[i].ManaRegenPerSecond;
                                        stat = stats[i];
                                        spell = spells[i];
                                        selectedRemaining = remaining[i];
                                        selectedIndex = i;
                                    }
                                    if (remaining[i] > 0 && spells[i].CostPerSecond - spells[i].ManaRegenPerSecond < lowestMps)
                                    {
                                        lowestMps = spells[i].CostPerSecond - spells[i].ManaRegenPerSecond;
                                    }
                                }
                            }
                            if (float.IsNegativeInfinity(highestMps))
                            {
                                // this cooldown combo is either overused or does not appear in solution
                                // try using up idle/wand time
                                if (remainingBase[0] > 0)
                                {
                                    stat = this;
                                    spell = null;
                                    selectedRemaining = remainingBase[0];
                                    highestMps = -ManaRegen;
                                }
                                else if (remainingBase[1] > 0)
                                {
                                    stat = this;
                                    spell = GetSpell("Wand");
                                    selectedRemaining = remainingBase[1];
                                    highestMps = spell.CostPerSecond - spell.ManaRegenPerSecond;
                                }
                                else
                                {
                                    stat = null;
                                    spell = null;
                                    selectedRemaining = double.PositiveInfinity;
                                    highestMps = -ManaRegen;
                                }
                            }
                            double apLeft = (apTime < 0) ? 0 : 15 - (time - apTime);
                            double ivLeft = (ivTime < 0) ? 0 : 20 - (time - ivTime);
                            double heroismLeft = (heroismTime < 0) ? 0 : 40 - (time - heroismTime);
                            double dpLeft = (destructionTime < 0) ? 0 : 15 - (time - destructionTime);
                            double fcLeft = (flameCapTime < 0) ? 0 : 60 - (time - flameCapTime);
                            double t1Left = (trinket1time < 0) ? 0 : Trinket1Duration - (time - trinket1time);
                            double t2Left = (trinket2time < 0) ? 0 : Trinket2Duration - (time - trinket2time);
                            double combustionLeft = (combustionTime < 0 || stat == null) ? 0 : stat.CombustionDuration - (time - combustionTime);
                            double drumsLeft = (drumsTime < 0) ? 0 : 30 - (time - drumsTime);

                            double shortestLeft = double.PositiveInfinity;
                            if (apLeft > 0) shortestLeft = Math.Min(shortestLeft, apLeft);
                            if (ivLeft > 0) shortestLeft = Math.Min(shortestLeft, ivLeft);
                            if (heroismLeft > 0) shortestLeft = Math.Min(shortestLeft, heroismLeft);
                            if (dpLeft > 0) shortestLeft = Math.Min(shortestLeft, dpLeft);
                            if (fcLeft > 0) shortestLeft = Math.Min(shortestLeft, fcLeft);
                            if (t1Left > 0) shortestLeft = Math.Min(shortestLeft, t1Left);
                            if (t2Left > 0) shortestLeft = Math.Min(shortestLeft, t2Left);
                            if (combustionLeft > 0) shortestLeft = Math.Min(shortestLeft, combustionLeft);
                            if (drumsLeft > 0) shortestLeft = Math.Min(shortestLeft, drumsLeft);

                            // move forward and drop cooldowns that fell off
                            double move = Math.Min(Math.Min(minTimeNeeded, selectedRemaining), shortestLeft);
                            //if (move * highestMps > mana) move = mana / highestMps;
                            if (selectedIndex >= 0)
                            {
                                remaining[selectedIndex] -= move;
                                if (spell.Name != lastSpell)
                                {
                                    lastSpell = spell.Name;
                                    timing.AppendLine(TimeFormat(time) + ": " + lastSpell + " (" + Math.Round(mana).ToString() + " mana, " + highestMps.ToString("F") + " mps)");
                                }
                            }
                            else if (remainingBase[0] > 0)
                            {
                                remainingBase[0] -= move;
                                if ("Idle Regen" != lastSpell)
                                {
                                    lastSpell = "Idle Regen";
                                    timing.AppendLine(TimeFormat(time) + ": " + lastSpell + " (" + Math.Round(mana).ToString() + " mana, " + highestMps.ToString("F") + " mps)");
                                }
                            }
                            else if (remainingBase[1] > 0)
                            {
                                remainingBase[1] -= move;
                                if ("Wand" != lastSpell)
                                {
                                    lastSpell = "Wand";
                                    timing.AppendLine(TimeFormat(time) + ": " + lastSpell + " (" + Math.Round(mana).ToString() + " mana, " + highestMps.ToString("F") + " mps)");
                                }
                            }
                            else
                            {
                                // emergency regen, only enough to be able to continue casting
                                /*if (BasicStats.Mana > 0 && mana <= 0 && !float.IsPositiveInfinity(lowestMps))
                                {
                                    move = lowestMps * move / (lowestMps + ManaRegen) + 0.001;
                                }
                                else*/ if (effect >= 0)
                                {
                                    lastSequenceValid = false;
                                }
                                remainingBase[0] -= move;
                                if ("Idle Regen" != lastSpell)
                                {
                                    lastSpell = "Idle Regen";
                                    timing.AppendLine(TimeFormat(time) + ": " + lastSpell + " (" + Math.Round(mana).ToString() + " mana, " + highestMps.ToString("F") + " mps)");
                                }
                            }
                            apCooldown -= move;
                            ivCooldown -= move;
                            potionCooldown -= move;
                            gemCooldown -= move;
                            trinket1Cooldown -= move;
                            trinket2Cooldown -= move;
                            combustionCooldown -= move;
                            drumsCooldown -= move;
                            time += move;
                            //mana -= move * highestMps;
                            //if (mana > BasicStats.Mana) mana = BasicStats.Mana;
                            if (apLeft <= move) apTime = -1;
                            if (ivLeft <= move) ivTime = -1;
                            if (heroismLeft <= move) heroismTime = -1;
                            if (dpLeft <= move) destructionTime = -1;
                            if (fcLeft <= move) flameCapTime = -1;
                            if (t1Left <= move) trinket1time = -1;
                            if (t2Left <= move) trinket2time = -1;
                            if (combustionLeft <= move) combustionTime = -1;
                            if (drumsLeft <= move) drumsTime = -1;
                        }
                    } while (!effectApplied && lastSequenceValid && time < fight);
                }
                double rem = 0;
                for (int i = 0; i <= 5; i++) rem += Math.Max(0, remainingBase[i]);
                for (int i = 0; i < N; i++) rem += Math.Max(0, remaining[i]);
                if (rem > 0)
                {
                    timing.AppendLine();
                    timing.AppendLine(string.Format("Divergence: {0:F} sec", rem));
                }
                if (rem < bestUnderused)
                {
                    bestUnderused = rem;
                    bestTiming = timing.ToString();
                }
                if (bestUnderused == 0) break;
                if (timeLimitReached) lastSequenceValid = false;
            } while (true);
            
            return bestTiming;
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
                        dictValues.Add(s.Name, String.Format("{0:F} Dps*{1:F} Mps\n{2:F} Tps\n{3:F} sec\n{4:F}x Amplify\n{5:F}% Crit Rate\n{6:F}% Hit Rate\n{7:F} Crit Multiplier\nAB Spam Tradeoff: {8:F} Dpm", s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond, s.ThreatPerSecond, bs.CastTime - Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, (AB.DamagePerSecond - s.DamagePerSecond) / (AB.CostPerSecond - AB.ManaRegenPerSecond - s.CostPerSecond + s.ManaRegenPerSecond)));
                    }
                    else
                    {
                        dictValues.Add(s.Name, String.Format("{0:F} Dps*{1:F} Mps\n{2:F} Tps\nAB Spam Tradeoff: {3:F} Dpm\nAverage Cast Time: {4:F} sec\n{5}", s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond, s.ThreatPerSecond, (AB.DamagePerSecond - s.DamagePerSecond) / (AB.CostPerSecond - AB.ManaRegenPerSecond - s.CostPerSecond + s.ManaRegenPerSecond), s.CastTime, s.Sequence));
                    }
                }
            }
            dictValues.Add("Total Damage", String.Format("{0:F}", OverallPoints * FightDuration));
            dictValues.Add("Dps", String.Format("{0:F}", OverallPoints));
            dictValues.Add("Tps", String.Format("{0:F}", Tps));
            //dictValues.Add("Sequence", ReconstructSequence());
            StringBuilder sb = new StringBuilder("*");
            if (MageArmor != null) sb.AppendLine(MageArmor);
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
