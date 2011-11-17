using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Rawr.Base;

namespace Rawr.ProtWarr
{
    public struct Player
    {
        public Character Character;
        public Base.StatsWarrior Stats;
        public WarriorTalents Talents;
        public CalculationOptionsProtWarr CalcOpts;
        public BossOptions BossOpts;
        public DefendModel DefendModel;
        public AttackModel AttackModel;

        public Player(Character character)
        {
            this.Character = character;
            this.Stats = new Base.StatsWarrior();
            this.Talents = this.Character.WarriorTalents;
            this.CalcOpts = this.Character.CalculationOptions as CalculationOptionsProtWarr;
            this.BossOpts = this.Character.BossOptions;
            this.DefendModel = null;
            this.AttackModel = null;
        }
    }

    [Rawr.Calculations.RawrModelInfo("ProtWarr", "Ability_Warrior_DefensiveStance", CharacterClass.Warrior, CharacterRole.Tank)]
    public class CalculationsProtWarr : CalculationsBase
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Relevant Gem IDs for ProtWarr
                // Red
                int[] bold      = { 52081, 52206, 52255 };  // Strength
                int[] flashing  = { 52083, 52216, 52259 };  // Parry
                int[] precise   = { 52085, 52230, 52260 };  // Expertise

                // Blue
                int[] solid     = { 52086, 52242, 52261 };  // Stamina

                // Yellow
                int[] subtle    = { 52090, 52247, 52265 };  // Dodge
                int[] fractured = { 52094, 52219, 52269 };  // Mastery

                // Orange
                int[] resolute  = { 52107, 52249, 52249 };  // Expertise + Dodge
                int[] skillful  = { 52114, 52240, 52240 };  // Strength + Mastery
                int[] fine      = { 52116, 52215, 52215 };  // Parry + Mastery
                int[] keen      = { 52118, 52224, 52224 };  // Expertise + Mastery
                
                // Purple
                int[] sovereign = { 52095, 52243, 52243 };  // Strength + Stamina
                int[] defender  = { 52097, 52210, 52210 };  // Parry + Stamina
                int[] guardian  = { 52099, 52221, 52221 };  // Expertise + Stamina

                // Green
                int[] regal     = { 52119, 52233, 52233 };  // Dodge + Stamina
                int[] puissant  = { 52126, 52231, 52231 };  // Mastery + Stamina

                // Meta
                int[] austere   = { 52294 };                // Stamina + 2% Armor

                string[] groupNames = new string[] { "Uncommon", "Rare", "Jewelcrafter" };
                int[,][] gemmingTemplates = new int[,][]
                {
                    //Red       Yellow      Blue        Prismatic   Meta
                    { flashing, fractured,  fractured,  fractured,  austere },  // Max Avoidance

                    { fine,     fine,       puissant,   fractured,  austere },  // Avoidance Heavy (Mastery)
                    { flashing, fine,       defender,   flashing,   austere },  // Avoidance Heavy (Parry)
                    { flashing, subtle,     regal,      fractured,  austere },  // Avoidance Heavy (Dodge)

                    { defender, puissant,   solid,      solid,      austere },  // Balanced Avoidance (Mastery)
                    { defender, regal,      solid,      solid,      austere },  // Balanced Avoidance (Dodge)

                    { solid,    solid,      solid,      solid,      austere },  // Max Health
                };

                // Generate List of Gemming Templates
                // groupNames[] index starts at 1 to ignore Uncommon gems currently
                List<GemmingTemplate> gemmingTemplate = new List<GemmingTemplate>();
                for (int j = 1; j <= groupNames.GetUpperBound(0); j++)
                {
                    for (int i = 0; i <= gemmingTemplates.GetUpperBound(0); i++)
                    {
                        gemmingTemplate.Add(new GemmingTemplate()
                        {
                            Model = "ProtWarr",
                            Group = groupNames[j],
                            RedId = gemmingTemplates[i, 0][j],
                            YellowId = gemmingTemplates[i, 1][j],
                            BlueId = gemmingTemplates[i, 2][j],
                            PrismaticId = gemmingTemplates[i, 3][j],
                            MetaId = gemmingTemplates[i, 4][0],
                            Enabled = j == 1 // Rare gems default
                        });
                    }
                }
                return gemmingTemplate;
            }
        }

        #region Variables and Properties

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelProtWarr();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
                    "Base Stats:Health",
                    "Base Stats:Strength",
                    "Base Stats:Agility",
                    "Base Stats:Stamina",

                    "Defensive Stats:Armor",
                    "Defensive Stats:Dodge",
                    "Defensive Stats:Parry",
                    "Defensive Stats:Block",
                    "Defensive Stats:Critical Block",
                    "Defensive Stats:Miss",
                    "Defensive Stats:Chance to be Crit",
                    "Defensive Stats:Guaranteed Reduction",
                    "Defensive Stats:Avoidance",
                    "Defensive Stats:Total Mitigation",
                    "Defensive Stats:Attacker Speed",
                    "Defensive Stats:Damage Taken",

                    "Offensive Stats:Weapon Speed",
                    "Offensive Stats:Attack Power",
                    "Offensive Stats:Hit",
                    "Offensive Stats:Haste",
                    "Offensive Stats:Armor Penetration",
                    "Offensive Stats:Crit",
                    "Offensive Stats:Expertise",
                    // Never really used in current WoW itemization, just taking up space
                    //"Offensive Stats:Weapon Damage",
                    "Offensive Stats:Missed Attacks",
                    "Offensive Stats:Total Damage/sec",
                    "Offensive Stats:Total Threat/sec",
                    "Offensive Stats:Rotation",

                    "Resistances:Nature Resist",
                    "Resistances:Fire Resist",
                    "Resistances:Frost Resist",
                    "Resistances:Shadow Resist",
                    "Resistances:Arcane Resist",
                    "Complex Stats:Ranking Mode*The currently selected ranking mode. Ranking modes can be changed in the Options tab.",
                    @"Complex Stats:Overall Points*Overall Points are a sum of Mitigation and Survival Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and 
Survival Points individually may be important.",
                    @"Complex Stats:Mitigation Points*Mitigation Points represent the amount of damage you mitigate, 
on average, through armor mitigation and avoidance. It is directly 
relational to your Damage Taken. Ideally, you want to maximize 
Mitigation Points, while maintaining 'enough' Survival Points 
(see Survival Points). If you find yourself dying due to healers 
running OOM, or being too busy healing you and letting other 
raid members die, then focus on Mitigation Points.",
                    @"Complex Stats:Survival Points*Survival Points represents the total raw physical damage 
(pre-avoidance/block) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers.
If you find that you are being killed by burst damage,
focus on Survival Points.",
                    @"Complex Stats:Threat Points*Threat Points represents the average between unlimited
threat and limited threat scaled by the threat scale.",
                    "Complex Stats:Nature Survival",
                    "Complex Stats:Fire Survival",
                    "Complex Stats:Frost Survival",
                    "Complex Stats:Shadow Survival",
                    "Complex Stats:Arcane Survival",
                    };
                return _characterDisplayCalculationLabels;
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
                    "% Total Mitigation",
                    "% Guaranteed Reduction",
                    "% Chance to be Crit",
                    "% Avoidance",
                    "% Avoidance+Block",
                    "Block Value",
                    "Threat/sec",
                    "% Chance to be Avoided", 
                    "% Chance to be Dodged",
                    "% Chance to be Parried",
                    "% Chance to Miss",
                    "Burst Time", 
                    "Nature Survival",
                    "Fire Survival",
                    "Frost Survival",
                    "Shadow Survival",
                    "Arcane Survival",
                    };
                return _optimizableCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] {
                    "Ability Damage",
                    "Ability Threat",
                    //"Rotation Damage",
                    //"Rotation Threat",
                    "Combat Table",
                    "Item Budget",
                    };
                return _customChartNames;
            }
        }

        private Dictionary<string, System.Windows.Media.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Windows.Media.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("Survival", System.Windows.Media.Colors.Blue);
                    _subPointNameColors.Add("Mitigation", System.Windows.Media.Colors.Red);
                    _subPointNameColors.Add("Threat", System.Windows.Media.Colors.Green);
                }
                return _subPointNameColors;
            }
        }

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd("Battle Shout");
            character.ActiveBuffsAdd("Devotion Aura");
            character.ActiveBuffsAdd("Ancestral Healing");
            character.ActiveBuffsAdd("Trueshot Aura");
            character.ActiveBuffsAdd("Ferocious Inspiration");
            character.ActiveBuffsAdd("Power Word: Fortitude");
            character.ActiveBuffsAdd("Rampage");
            character.ActiveBuffsAdd("Windfury Totem");
            character.ActiveBuffsAdd("Blessing of Kings");
            character.ActiveBuffsAdd("Sunder Armor");
            character.ActiveBuffsAdd("Demoralizing Shout");
            character.ActiveBuffsAdd("Mangle");
            character.ActiveBuffsAdd("Thunder Clap");
            character.ActiveBuffsAdd("Flask of Steelskin");
            if (character.PrimaryProfession == Profession.Alchemy || character.SecondaryProfession == Profession.Alchemy)
                character.ActiveBuffsAdd("Flask of Steelskin (Mixology)");
            character.ActiveBuffsAdd("Mastery Food");

            #region Boss Options
            // Never in back of the Boss
            character.BossOptions.InBack = false;

            int avg = character.AvgWornItemLevel;
            int[] points = new int[] { 350, 358, 365 };
            #region Need a Boss Attack
            character.BossOptions.DamagingTargs = true;
            if (character.BossOptions.DefaultMeleeAttack == null) {
                character.BossOptions.Attacks.Add(BossHandler.ADefaultMeleeAttack);
            }
            if        (avg <= points[0]) {
                character.BossOptions.Health = 20000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_10];
            } else if (avg <= points[1]) {
                character.BossOptions.Health = 35000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_25];
            } else if (avg <= points[2]) {
                character.BossOptions.Health = 50000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_10H];
            } else if (avg >  points[2]) {
                character.BossOptions.Health = 65000000;
                character.BossOptions.DefaultMeleeAttack.DamagePerHit = BossHandler.StandardMeleePerHit[(int)BossHandler.TierLevels.T11_25H];
            }
            #endregion
            #endregion
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Devastate");
                _relevantGlyphs.Add("Glyph of Revenge");
                _relevantGlyphs.Add("Glyph of Shield Slam");

                _relevantGlyphs.Add("Glyph of Cleaving");
                _relevantGlyphs.Add("Glyph of Heroic Throw");
                _relevantGlyphs.Add("Glyph of Intervene");
                _relevantGlyphs.Add("Glyph of Long Charge");
                _relevantGlyphs.Add("Glyph of Piercing Howl");
                _relevantGlyphs.Add("Glyph of Rapid Charge");
                _relevantGlyphs.Add("Glyph of Resonating Power");
                _relevantGlyphs.Add("Glyph of Shield Wall");
                _relevantGlyphs.Add("Glyph of Shockwave");
                _relevantGlyphs.Add("Glyph of Spell Reflection");
                _relevantGlyphs.Add("Glyph of Sunder Armor");
                _relevantGlyphs.Add("Glyph of Thunder Clap");
                _relevantGlyphs.Add("Glyph of Victory Rush");

                _relevantGlyphs.Add("Glyph of Battle");
                _relevantGlyphs.Add("Glyph of Berserker Rage");
                _relevantGlyphs.Add("Glyph of Bloody Healing");
                _relevantGlyphs.Add("Glyph of Command");
                _relevantGlyphs.Add("Glyph of Demoralizing Shout");
                _relevantGlyphs.Add("Glyph of Enduring Victory");
                _relevantGlyphs.Add("Glyph of Intimidating Shout");              
            }
            return _relevantGlyphs;
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationProtWarr(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsProtWarr(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsProtWarr));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsProtWarr calcOpts = serializer.Deserialize(reader) as CalculationOptionsProtWarr;
            return calcOpts;
        }
        #endregion

        #region Talent SpecialEffects
        Type StatsWarriorType = typeof(StatsWarrior);
        // We need these to be static so they aren't re-created

        private static SpecialEffect[] _SE_HoldTheLine = {
            null,
            new SpecialEffect(Trigger.DamageParried, new StatsWarrior() { CriticalBlock = 0.1f, PhysicalCrit = 0.1f }, 5, 0, 1.0f),
            new SpecialEffect(Trigger.DamageParried, new StatsWarrior() { CriticalBlock = 0.1f, PhysicalCrit = 0.1f }, 10, 0, 1.0f),
        };

        private static SpecialEffect[] _SE_BastionOfDefense = {
            null,
            new SpecialEffect(Trigger.DamageAvoided, new StatsWarrior() { BonusPhysicalDamageMultiplier = 0.05f }, 12, 0, 0.1f),
            new SpecialEffect(Trigger.DamageAvoided, new StatsWarrior() { BonusPhysicalDamageMultiplier = 0.1f }, 12, 0, 0.2f),
        };

        private static SpecialEffect[] _SE_4T12 = {
            new SpecialEffect(Trigger.ShieldBlock, new Stats() { Parry = 0.06f, }, 10f, 60f - (0 * 0.10f)),
            new SpecialEffect(Trigger.ShieldBlock, new Stats() { Parry = 0.06f, }, 10f, 60f - (1 * 0.10f)),
            new SpecialEffect(Trigger.ShieldBlock, new Stats() { Parry = 0.06f, }, 10f, 60f - (2 * 0.10f)),
            new SpecialEffect(Trigger.ShieldBlock, new Stats() { Parry = 0.06f, }, 10f, 60f - (3 * 0.10f)),
        };
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            #region Setup uniform variables from all models
            CharacterCalculationsProtWarr calculatedStats = new CharacterCalculationsProtWarr();
            if (character == null) { return calculatedStats; }
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            if (calcOpts == null) { return calculatedStats; }
            BossOptions bossOpts = character.BossOptions;
            // Make sure there is at least one attack in the list.
            // If there's not, add a Default Melee Attack for processing
            if (bossOpts.Attacks.Count < 1) {
                character.IsLoading = true;
                bossOpts.DamagingTargs = true;
                bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                character.IsLoading = false;
            }
            // Make sure there is a default melee attack
            // If the above processed, there will be one so this won't have to process
            // If the above didn't process and there isn't one, add one now
            if (bossOpts.DefaultMeleeAttack == null) {
                character.IsLoading = true;
                bossOpts.DamagingTargs = true;
                bossOpts.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                character.IsLoading = false;
            }
            // Since the above forced there to be an attack it's safe to do this without a null check
            StatsWarrior stats = GetCharacterStats(character, additionalItem) as StatsWarrior;
            calculatedStats.BasicStats = stats;
            calculatedStats.TargetLevel = bossOpts.Level;
            //calculatedStats.CharacterLevel = character.Level;
            #endregion

            int levelDifference = (bossOpts.Level - character.Level);

            Player player = new Player(character);

            AccumulateCharacterStats(player, additionalItem);

            player.DefendModel = new DefendModel(player);
            player.AttackModel = new AttackModel(player, AttackModelMode.Optimal);

            if (needsDisplayCalculations)
            {
                calculatedStats.CritReduction = Lookup.AvoidanceChance(player, HitResult.Crit);
                calculatedStats.ArmorReduction = Lookup.ArmorReduction(player);

                calculatedStats.BaseAttackerSpeed = player.BossOpts.DefaultMeleeAttack.AttackSpeed;
                calculatedStats.AttackerSpeed = Lookup.TargetWeaponSpeed(player);
                calculatedStats.DamageTakenPerHit = player.DefendModel.DamagePerHit;
                calculatedStats.DamageTakenPerBlock = player.DefendModel.DamagePerBlock;
                calculatedStats.DamageTakenPerCritBlock = player.DefendModel.DamagePerCritBlock;
                calculatedStats.DamageTakenPerCrit = player.DefendModel.DamagePerCrit;
                calculatedStats.DamageTaken = player.DefendModel.DamagePerSecond;

                calculatedStats.ArcaneReduction = (1.0f - Lookup.MagicReduction(player, DamageType.Arcane));
                calculatedStats.FireReduction = (1.0f - Lookup.MagicReduction(player, DamageType.Fire));
                calculatedStats.FrostReduction = (1.0f - Lookup.MagicReduction(player, DamageType.Frost));
                calculatedStats.NatureReduction = (1.0f - Lookup.MagicReduction(player, DamageType.Nature));
                calculatedStats.ShadowReduction = (1.0f - Lookup.MagicReduction(player, DamageType.Shadow));

                calculatedStats.Crit = player.AttackModel.Abilities[Ability.None].AttackTable.Critical;
                calculatedStats.Expertise = Lookup.BonusExpertisePercentage(player);
                calculatedStats.Haste = Lookup.BonusHastePercentage(player);
                calculatedStats.WeaponSpeed = Lookup.WeaponSpeed(player);
            }

            calculatedStats.TargetLevel = player.BossOpts.Level;
            calculatedStats.Abilities = player.AttackModel.Abilities;
            calculatedStats.BasicStats = player.Stats;

            calculatedStats.Miss = player.DefendModel.DefendTable.Miss;
            calculatedStats.Dodge = player.DefendModel.DefendTable.Dodge;
            calculatedStats.Parry = player.DefendModel.DefendTable.Parry;
            calculatedStats.Block = player.DefendModel.DefendTable.AnyBlock;
            calculatedStats.BaseBlock = player.DefendModel.DefendTable.BaseBlock + player.DefendModel.DefendTable.BaseCriticalBlock;
            calculatedStats.BuffedBlock = player.DefendModel.DefendTable.BuffedBlock + player.DefendModel.DefendTable.BuffedCriticalBlock;
            calculatedStats.CriticalBlock = player.DefendModel.DefendTable.CriticalBlock;
            calculatedStats.AnyMiss = player.DefendModel.DefendTable.AnyMiss;
            calculatedStats.AnyAvoid = player.DefendModel.DefendTable.AnyAvoid;
            calculatedStats.BaseAnyAvoid = player.DefendModel.DefendTable.BaseAnyAvoid;
            calculatedStats.BuffedAnyAvoid = player.DefendModel.DefendTable.BuffedAnyAvoid;
            calculatedStats.CritVulnerability = player.DefendModel.DefendTable.Critical;
            calculatedStats.GuaranteedReduction = player.DefendModel.GuaranteedReduction;
            calculatedStats.TotalMitigation = player.DefendModel.Mitigation;

            calculatedStats.ArcaneSurvivalPoints = player.Stats.Health / Lookup.MagicReduction(player, DamageType.Arcane);
            calculatedStats.FireSurvivalPoints = player.Stats.Health / Lookup.MagicReduction(player, DamageType.Fire);
            calculatedStats.FrostSurvivalPoints = player.Stats.Health / Lookup.MagicReduction(player, DamageType.Frost);
            calculatedStats.NatureSurvivalPoints = player.Stats.Health / Lookup.MagicReduction(player, DamageType.Nature);
            calculatedStats.ShadowSurvivalPoints = player.Stats.Health / Lookup.MagicReduction(player, DamageType.Shadow);

            calculatedStats.Hit = Lookup.BonusHitPercentage(player);
            calculatedStats.AvoidedAttacks = player.AttackModel.Abilities[Ability.None].AttackTable.AnyMiss;
            calculatedStats.DodgedAttacks = player.AttackModel.Abilities[Ability.None].AttackTable.Dodge;
            calculatedStats.ParriedAttacks = player.AttackModel.Abilities[Ability.None].AttackTable.Parry;
            calculatedStats.MissedAttacks = player.AttackModel.Abilities[Ability.None].AttackTable.Miss;

            calculatedStats.HeroicStrikeFrequency = player.CalcOpts.HeroicStrikeFrequency;
            calculatedStats.ThreatPerSecond = player.AttackModel.ThreatPerSecond;
            calculatedStats.ThreatModelName = player.AttackModel.ShortName;
            calculatedStats.ThreatModel = player.AttackModel.Name + "\n" + player.AttackModel.Description;
            calculatedStats.TotalDamagePerSecond = player.AttackModel.DamagePerSecond;

            calculatedStats.BurstTime = player.DefendModel.BurstTime;
            calculatedStats.RankingMode = player.CalcOpts.RankingMode;
            switch (player.CalcOpts.RankingMode)
            {
                case 2:
                    // Burst Time Mode
                    calculatedStats.SurvivalPoints      = player.DefendModel.BurstTime * 100.0f;
                    calculatedStats.MitigationPoints    = 0.0f;
                    calculatedStats.ThreatPoints        = 0.0f;
                    break;
                case 3:
                    // CT Coverage mode
                    calculatedStats.SurvivalPoints      = 0.0f;
                    calculatedStats.MitigationPoints    = (player.DefendModel.DefendTable.Miss + player.DefendModel.DefendTable.Parry + player.DefendModel.DefendTable.Dodge + player.DefendModel.DefendTable.Block + player.DefendModel.DefendTable.CriticalBlock)*10000f;
                    calculatedStats.ThreatPoints        = 0.0f;
                    break;
                default:
                    // Mitigation Scale Mode
                    double survivalCap = player.CalcOpts.HitsToSurvive * player.BossOpts.DefaultMeleeAttack.DamagePerHit / 1000.0d;
                    double survivalRaw = player.DefendModel.EffectiveHealth / 1000.0d;

                    if (survivalRaw <= survivalCap)
                    {
                        calculatedStats.SurvivalPoints = player.DefendModel.EffectiveHealth;
                    }
                    else
                    {
                        double topLeft          = Math.Pow(((survivalRaw - survivalCap) / survivalCap) + StatConversion.SurvivalScalerBase, 1.0d / 4.0d);
                        double fraction         = (topLeft - StatConversion.SurvivalScalerTopRight) / 2.0d;
                        double cappedSurvival   = survivalCap * fraction + survivalCap;
                        calculatedStats.SurvivalPoints = (float)cappedSurvival * 1000.0f;
                    }

                    calculatedStats.MitigationPoints    = StatConversion.MitigationScaler / (1.0f - player.DefendModel.Mitigation);
                    calculatedStats.ThreatPoints        = player.AttackModel.ThreatPerSecond * player.CalcOpts.ThreatScale;
                    break;
            }

            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            if (character.CalculationOptions == null)
                character.CalculationOptions = new CalculationOptionsProtWarr();

            // Make sure there is at least one attack in the list.
            // If there's not, add a Default Melee Attack for processing
            if (character.BossOptions.Attacks.Count < 1)
            {
                character.IsLoading = true;
                character.BossOptions.DamagingTargs = true;
                character.BossOptions.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                character.IsLoading = false;
            }
            // Make sure there is a default melee attack
            // If the above processed, there will be one so this won't have to process
            // If the above didn't process and there isn't one, add one now
            if (character.BossOptions.DefaultMeleeAttack == null)
            {
                character.IsLoading = true;
                character.BossOptions.DamagingTargs = true;
                character.BossOptions.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                character.IsLoading = false;
            }

            Player player = new Player(character);
            AccumulateCharacterStats(player, additionalItem);

            return player.Stats;
        }

        private void AccumulateCharacterStats(Player player, Item additionalItem)
        {
            float baseStrength, baseBonusMasteryBlockPercentage, baseParryRatingFromStrength;

            // Items and Buffs
            StatsWarrior statsItemsBuffs = new StatsWarrior();
            AccumulateItemStats(statsItemsBuffs, player.Character, additionalItem);
            statsItemsBuffs.Accumulate(GetBuffsStats(player)); // AccumulateBuffsStats(statsItemsBuffs, player.Character.ActiveBuffs);
            player.Stats.Accumulate(statsItemsBuffs);

            // Race Stats
            StatsWarrior statsRace = new StatsWarrior();
            statsRace.Accumulate(BaseStats.GetBaseStats(player.Character.Level, CharacterClass.Warrior, player.Character.Race));
            statsRace.Expertise += BaseStats.GetRacialExpertise(player.Character, ItemSlot.MainHand);
            player.Stats.Accumulate(statsRace);

            // Talents
            StatsWarrior statsTalents = new StatsWarrior() {
                Block = 0.15f, // Sentinel
                BonusStaminaMultiplier = (1.0f + 0.15f) * (1.0f + (Character.ValidateArmorSpecialization(player.Character, ItemType.Plate) ? 0.05f : 0.0f)) - 1.0f, // Sentinel & Plate Specialization
                BaseArmorMultiplier = player.Talents.Toughness * 0.03f,
            };
            if (player.Talents.HoldTheLine > 0)
                statsTalents.AddSpecialEffect(_SE_HoldTheLine[player.Talents.HoldTheLine]);
            if (player.Talents.BastionOfDefense > 0)
                statsTalents.AddSpecialEffect(_SE_BastionOfDefense[player.Talents.BastionOfDefense]);
            player.Stats.Accumulate(statsTalents);

            // Base Stats
            player.Stats.BaseAgility = statsRace.Agility;
            player.Stats.Stamina = (float)Math.Floor((statsRace.Stamina + statsTalents.Stamina) * (1.0f + player.Stats.BonusStaminaMultiplier));
            player.Stats.Stamina += (float)Math.Floor(statsItemsBuffs.Stamina * (1.0f + player.Stats.BonusStaminaMultiplier));
            player.Stats.Strength = baseStrength = (float)Math.Floor((statsRace.Strength + statsTalents.Strength) * (1.0f + player.Stats.BonusStrengthMultiplier));
            player.Stats.Strength += (float)Math.Floor(statsItemsBuffs.Strength * (1.0f + player.Stats.BonusStrengthMultiplier));
            player.Stats.Agility = (float)Math.Floor((statsRace.Agility + statsTalents.Agility) * (1.0f + player.Stats.BonusAgilityMultiplier));
            player.Stats.Agility += (float)Math.Floor(statsItemsBuffs.Agility * (1.0f + player.Stats.BonusAgilityMultiplier));

            // First-Pass Avoidance for Accurate Special Effect Trigger Chances
            baseBonusMasteryBlockPercentage = Lookup.BonusMasteryBlockPercentage(player);
            baseParryRatingFromStrength = (player.Stats.Strength - baseStrength) * 0.27f; // Parry Rating conversion ignores base Strength
            player.Stats.Block += baseBonusMasteryBlockPercentage;
            player.Stats.ParryRating += baseParryRatingFromStrength; 

            // Calculate Procs and Special Effects
            player.Stats.Accumulate(GetSpecialEffectStats(player));

            // Highest Stat Effects
            if (player.Stats.Strength > player.Stats.Agility)
                player.Stats.Strength += (float)Math.Floor((player.Stats.HighestStat + player.Stats.Paragon) * (1.0f + player.Stats.BonusStrengthMultiplier));
            else
                player.Stats.Agility += (float)Math.Floor((player.Stats.HighestStat + player.Stats.Paragon) * (1.0f + player.Stats.BonusAgilityMultiplier));
            if (player.Stats.HighestSecondaryStat > 0)
            {
                float paragonValue = player.Stats.HighestSecondaryStat; // how much paragon to add
                player.Stats.HighestSecondaryStat = 0f; // remove Paragon stat, since it's not needed
                if (player.Stats.CritRating > player.Stats.HasteRating && player.Stats.CritRating > player.Stats.MasteryRating)
                    player.Stats.CritRating += paragonValue;
                else if (player.Stats.HasteRating > player.Stats.CritRating && player.Stats.HasteRating > player.Stats.MasteryRating)
                    player.Stats.HasteRating += paragonValue;
                else 
                        player.Stats.MasteryRating += paragonValue;
            }
            // Defensive Stats, Add Difference of First-Pass and Second-Pass Block/Parry
            player.Stats.Armor = (float)Math.Ceiling(player.Stats.Armor * (1.0f + player.Stats.BaseArmorMultiplier));
            player.Stats.Armor += (float)Math.Ceiling(player.Stats.BonusArmor * (1.0f + player.Stats.BonusArmorMultiplier));
            player.Stats.Block += (Lookup.BonusMasteryBlockPercentage(player) - baseBonusMasteryBlockPercentage);
            player.Stats.ParryRating += (((player.Stats.Strength - baseStrength) * 0.27f) - baseParryRatingFromStrength); // Parry Rating conversion ignores base Strength

            player.Stats.NatureResistance += player.Stats.NatureResistanceBuff;
            player.Stats.FireResistance += player.Stats.FireResistanceBuff;
            player.Stats.FrostResistance += player.Stats.FrostResistanceBuff;
            player.Stats.ShadowResistance += player.Stats.ShadowResistanceBuff;
            player.Stats.ArcaneResistance += player.Stats.ArcaneResistanceBuff;

            // Final Derived Stats
            player.Stats.Health += StatConversion.GetHealthFromStamina(player.Stats.Stamina, CharacterClass.Warrior) + player.Stats.BattlemasterHealthProc;
            player.Stats.Health = (float)Math.Floor(player.Stats.Health * (1.0f + player.Stats.BonusHealthMultiplier));
            player.Stats.AttackPower += player.Stats.Strength * 2.0f + (player.Stats.Health * 0.1f * player.CalcOpts.AverageVengeance);
            player.Stats.AttackPower = (float)Math.Floor(player.Stats.AttackPower * (1.0f + player.Stats.BonusAttackPowerMultiplier));
        }

        public StatsWarrior GetBuffsStats(Player player)
        {
            Base.StatsWarrior statsBuffs = new Base.StatsWarrior();
            statsBuffs.Accumulate(GetBuffsStats(player.Character.ActiveBuffs, player.Character.SetBonusCount));

            if (player.Character.ActiveBuffs.Find<Buff>(x => x.SpellId == 22738) != null)
            {
                statsBuffs.BonusWarrior_PvP_4P_InterceptCDReduc = 5f;
            }
            if (player.Character.ActiveBuffs.Find<Buff>(x => x.SpellId == 70843) != null)
            {
                statsBuffs.BonusShieldSlamDamageMultiplier = 0.20f;
                statsBuffs.BonusShockwaveDamageMultiplier = 0.20f;
            }
            if (player.Character.ActiveBuffs.Find<Buff>(x => x.SpellId == 70844) != null)
            {
                //Your Battle Shout and Commanding Shout abilities now cause you to absorb damage equal to 20% of your maximum health. Lasts 10 sec.
                //statsBuffs.BonusWarrior_PvP_4P_InterceptCDReduc = 5f;
            }
            if (player.Character.ActiveBuffs.Find<Buff>(x => x.SpellId == 90296) != null)
            {
                statsBuffs.BonusShieldSlamDamageMultiplier = 0.05f;
            }
            if (player.Character.ActiveBuffs.Find<Buff>(x => x.SpellId == 90296) != null)
            {
                statsBuffs.BonusShieldWallDurMultiplier = 0.50f;
            }

            int T12count;
            player.Character.SetBonusCount.TryGetValue("Molten Giant Battleplate", out T12count);
            if (T12count >= 2)
            {
                statsBuffs.BonusShieldSlamDamageMultiplier = 0.20f;
            }
            if (T12count >= 4)
            {
                statsBuffs.AddSpecialEffect(_SE_4T12[player.Talents.ShieldMastery]);
            }

            return statsBuffs;
        }

        private StatsWarrior GetSpecialEffectStats(Player player)
        {
            StatsWarrior statsSpecialEffects = new StatsWarrior();

            Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>();
            Dictionary<Trigger, float> triggerChances   = new Dictionary<Trigger, float>();

            player.DefendModel = new DefendModel(player);
            player.AttackModel = new AttackModel(player, AttackModelMode.Optimal);

            float effectiveMasteryRating        = Lookup.MaxEffectiveMasteryRating(player);
            float effectiveBuffedMasteryRating  = effectiveMasteryRating * (1.0f - 10.0f / player.CalcOpts.ShieldBlockInterval) +
                                                    Lookup.MaxEffectiveBuffedMasteryRating(player) * (10.0f / player.CalcOpts.ShieldBlockInterval);
            
            triggerIntervals[Trigger.Use]                   = 0.0f;
            triggerIntervals[Trigger.MeleeAttack]           = player.AttackModel.WeaponAttacksPerSecond;
            triggerIntervals[Trigger.MeleeHit]              = triggerIntervals[Trigger.MeleeAttack];
            triggerIntervals[Trigger.MeleeCrit]             = triggerIntervals[Trigger.MeleeAttack];
            triggerIntervals[Trigger.PhysicalHit]           = triggerIntervals[Trigger.MeleeAttack];
            triggerIntervals[Trigger.PhysicalAttack]        = triggerIntervals[Trigger.MeleeAttack];
            triggerIntervals[Trigger.PhysicalCrit]          = triggerIntervals[Trigger.MeleeAttack];
            triggerIntervals[Trigger.ExecuteHit]            = triggerIntervals[Trigger.MeleeAttack];
            triggerIntervals[Trigger.DoTTick]               = (player.Talents.DeepWounds > 0) ? 2.0f : 0.0f;
            triggerIntervals[Trigger.DamageDone]            = triggerIntervals[Trigger.MeleeAttack] + triggerIntervals[Trigger.DoTTick];
            triggerIntervals[Trigger.DamageOrHealingDone]   = triggerIntervals[Trigger.DamageDone];
            triggerIntervals[Trigger.DamageTaken]           = 1.0f / player.DefendModel.AttackerSwingsPerSecond;
            triggerIntervals[Trigger.DamageAvoided]         = triggerIntervals[Trigger.DamageTaken];
            triggerIntervals[Trigger.DamageParried]         = triggerIntervals[Trigger.DamageTaken];
            triggerIntervals[Trigger.DamageTakenPutsMeBelow35PercHealth] = triggerIntervals[Trigger.DamageTaken];
            triggerIntervals[Trigger.ShieldBlock]           = 60f - (player.Talents.ShieldMastery * 10f);

            triggerChances[Trigger.Use]                     = 1.0f;
            triggerChances[Trigger.MeleeAttack]             = 1.0f;
            triggerChances[Trigger.MeleeHit]                = player.AttackModel.HitsPerSecond / player.AttackModel.WeaponAttacksPerSecond;
            triggerChances[Trigger.MeleeCrit]               = player.AttackModel.CritsPerSecond / player.AttackModel.WeaponAttacksPerSecond;
            triggerChances[Trigger.PhysicalAttack]          = 1.0f;
            triggerChances[Trigger.PhysicalHit]             = triggerChances[Trigger.MeleeHit];
            triggerChances[Trigger.PhysicalCrit]            = triggerChances[Trigger.MeleeCrit];
            triggerChances[Trigger.ExecuteHit]              = triggerChances[Trigger.MeleeHit];
            triggerChances[Trigger.DoTTick]                 = (player.Talents.DeepWounds > 0) ? 1.0f : 0.0f;
            triggerChances[Trigger.DamageDone]              = (player.AttackModel.HitsPerSecond + ((player.Talents.DeepWounds > 0) ? 2.0f : 0.0f))
                                                                / (player.AttackModel.WeaponAttacksPerSecond + ((player.Talents.DeepWounds > 0) ? 1.0f : 0.0f));
            triggerChances[Trigger.DamageOrHealingDone]     = triggerChances[Trigger.DamageDone];
            triggerChances[Trigger.DamageTaken]             = player.DefendModel.AttackerHitsPerSecond / player.DefendModel.AttackerSwingsPerSecond;
            triggerChances[Trigger.DamageAvoided]           = player.DefendModel.DefendTable.AnyAvoid;
            triggerChances[Trigger.DamageParried]           = player.DefendModel.DefendTable.Parry;
            triggerChances[Trigger.DamageTakenPutsMeBelow35PercHealth] = triggerChances[Trigger.DamageTaken] * 0.35f;
            triggerChances[Trigger.ShieldBlock]             = 1.0f;

            foreach (SpecialEffect effect in player.Stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger))
                {
                    // Effective Mastery Capping on Large Proc Effects
                    if ((effect.Trigger == Trigger.Use && effect.Stats.MasteryRating > effectiveMasteryRating) || effect.Stats.MasteryRating > effectiveBuffedMasteryRating)
                    {
                        Stats cappedStats = new Stats();
                        cappedStats.Accumulate(effect.Stats);
                        // Assume Use Effects Bypass Shield Block Collision
                        if (effect.Trigger == Trigger.Use)
                            cappedStats.MasteryRating = effectiveMasteryRating;
                        else
                            cappedStats.MasteryRating = effectiveBuffedMasteryRating;
                        // calculate average up-time of this trinket
                        float averageUpTime = 0.0f;
                        if (effect.Trigger == Trigger.ExecuteHit)
                            averageUpTime = effect.GetAverageFactor(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], player.AttackModel.WeaponSpeed, player.BossOpts.BerserkTimer * (float)player.BossOpts.Under20Perc);
                        else
                            averageUpTime = effect.GetAverageFactor(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], player.AttackModel.WeaponSpeed, player.BossOpts.BerserkTimer);
                        // accumulate the capped stats from the trinket into our final stats
                        statsSpecialEffects.Accumulate(cappedStats, averageUpTime);
                    }
                    else
                    {
                        if (effect.Trigger == Trigger.ExecuteHit)
                            effect.AccumulateAverageStats(statsSpecialEffects, triggerIntervals, triggerChances, player.AttackModel.WeaponSpeed, player.BossOpts.BerserkTimer * (float)player.BossOpts.Under20Perc);
                        else
                            effect.AccumulateAverageStats(statsSpecialEffects, triggerIntervals, triggerChances, player.AttackModel.WeaponSpeed, player.BossOpts.BerserkTimer);
                    }
                }
            }

            // Base Stats
            statsSpecialEffects.Stamina = (float)Math.Floor(statsSpecialEffects.Stamina * (1.0f + player.Stats.BonusStaminaMultiplier));
            statsSpecialEffects.Strength = (float)Math.Floor(statsSpecialEffects.Strength * (1.0f + player.Stats.BonusStrengthMultiplier));
            statsSpecialEffects.Agility = (float)Math.Floor(statsSpecialEffects.Agility * (1.0f + player.Stats.BonusAgilityMultiplier));

            return statsSpecialEffects;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            CharacterCalculationsProtWarr calculations = GetCharacterCalculations(character) as CharacterCalculationsProtWarr;

            switch (chartName)
            {
                /*
                #region Rotation Damage and Threat
                case "Rotation Damage":
                case "Rotation Threat":
                    {
                        CalculationOptionsProtWarr options = character.CalculationOptions as CalculationOptionsProtWarr;
                        string[] rotations = Enum.GetNames(typeof(AttackModelMode));
                        // Optimal rotation will need to be ignored as it is outside the scope of this
                        ComparisonCalculationBase[] comparisons = new ComparisonCalculationBase[rotations.Length - 1];

                        int j = 0;
                        for (int i = 0; i < rotations.Length; i++)
                        {
                            if (rotations[i] != "Optimal")
                            {
                                AttackModel am = new AttackModel(character, calculations.BasicStats, options, (AttackModelMode)Enum.Parse(typeof(AttackModelMode), rotations[i]));
                                ComparisonCalculationProtWarr comparison = new ComparisonCalculationProtWarr();
                                comparison.Name = am.Name;

                                if (chartName == "Rotation Damage")
                                    comparison.ThreatPoints = am.DamagePerSecond;
                                else
                                    comparison.ThreatPoints = am.ThreatPerSecond;

                                comparisons[j] = comparison;
                                j++;
                            }
                        }
                        return comparisons;
                    }
                #endregion
                */
                #region Ability Damage and Threat
                case "Ability Damage":
                case "Ability Threat":
                    {
                        // Vigilance will need to be ignores as it is outside the scope of this
                        ComparisonCalculationBase[] comparisons = new ComparisonCalculationBase[calculations.Abilities.Count];
                        // Index Deep Wounds for including in everything that can crit
                        AbilityModel DeepWounds = calculations.Abilities[Ability.DeepWounds];

                        int j = 0;
                        foreach (AbilityModel ability in calculations.Abilities)
                        {
                            ComparisonCalculationProtWarr comparison = new ComparisonCalculationProtWarr();
                            comparison.Name = ability.Name;

                            // Deep Wounds gets colored red to make the integrated parts easier to understand
                            if (ability.Ability == Ability.DeepWounds)
                            {
                                if (chartName == "Ability Damage")
                                    comparison.MitigationPoints = ability.Damage;
                                else
                                    comparison.MitigationPoints = ability.Threat;
                            }
                            else
                            {
                                if (chartName == "Ability Damage")
                                {
                                    comparison.ThreatPoints = ability.Damage;
                                    comparison.MitigationPoints = ability.CritPercentage * DeepWounds.Damage;
                                }
                                else
                                {
                                    comparison.ThreatPoints = ability.Threat;
                                    comparison.MitigationPoints = ability.CritPercentage * DeepWounds.Threat;
                                }
                            }

                            comparison.OverallPoints = comparison.SurvivalPoints + comparison.ThreatPoints + comparison.MitigationPoints;
                            comparisons[j] = comparison;
                            j++;
                        }
                        return comparisons;
                    }
                #endregion
                #region Combat Table
                case "Combat Table":
                    {
                        ComparisonCalculationProtWarr calcMiss = new ComparisonCalculationProtWarr();
                        ComparisonCalculationProtWarr calcDodge = new ComparisonCalculationProtWarr();
                        ComparisonCalculationProtWarr calcParry = new ComparisonCalculationProtWarr();
                        ComparisonCalculationProtWarr calcBlock = new ComparisonCalculationProtWarr();
                        ComparisonCalculationProtWarr calcCritBlock = new ComparisonCalculationProtWarr();
                        ComparisonCalculationProtWarr calcCrit = new ComparisonCalculationProtWarr();
                        ComparisonCalculationProtWarr calcCrush = new ComparisonCalculationProtWarr();
                        ComparisonCalculationProtWarr calcHit = new ComparisonCalculationProtWarr();
                        if (calculations != null)
                        {
                            calcMiss.Name = "Miss";
                            calcDodge.Name = "Dodge";
                            calcParry.Name = "Parry";
                            calcBlock.Name = "Block";
                            calcCritBlock.Name = "Critical Block";
                            calcCrit.Name = "Crit";
                            calcCrush.Name = "Crush";
                            calcHit.Name = "Hit";

                            calcMiss.OverallPoints = calcMiss.MitigationPoints = calculations.Miss * 100.0f;
                            calcDodge.OverallPoints = calcDodge.MitigationPoints = calculations.Dodge * 100.0f;
                            calcParry.OverallPoints = calcParry.MitigationPoints = calculations.Parry * 100.0f;
                            calcCritBlock.OverallPoints = calcCritBlock.MitigationPoints = calculations.CriticalBlock * 100.0f;
                            calcBlock.OverallPoints = calcBlock.MitigationPoints = (calculations.Block - calculations.CriticalBlock) * 100.0f;
                            calcCrit.OverallPoints = calcCrit.SurvivalPoints = calculations.CritVulnerability * 100.0f;
                            calcHit.OverallPoints = calcHit.SurvivalPoints = (1.0f - calculations.AnyAvoid - calculations.CritVulnerability) * 100.0f;
                        }
                        return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcParry, calcBlock, calcCritBlock, calcCrit, calcCrush, calcHit };
                    }
                #endregion
                #region Item Budget
                case "Item Budget":
                    CharacterCalculationsProtWarr calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcStrengthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = 100.0f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcAgilityValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = 100.0f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcStaminaValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 150.0f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcHealthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Health = 1500.0f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcArmorValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BonusArmor = 400.0f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcDodgeValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DodgeRating = 100.0f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcParryValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ParryRating = 100.0f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcMasteryValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { MasteryRating = 100.0f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 100.0f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcExpertiseValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 100.0f } }) as CharacterCalculationsProtWarr;
                    CharacterCalculationsProtWarr calcHitValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 100.0f } }) as CharacterCalculationsProtWarr;

                    return new ComparisonCalculationBase[] { 
                        new ComparisonCalculationProtWarr() { Name = "100 Strength",
                            OverallPoints = (calcStrengthValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcStrengthValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcStrengthValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcStrengthValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "100 Agility",
                            OverallPoints = (calcAgilityValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcAgilityValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcAgilityValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcAgilityValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "150 Stamina",
                            OverallPoints = (calcStaminaValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcStaminaValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcStaminaValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcStaminaValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "1500 Health",
                            OverallPoints = (calcHealthValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcHealthValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHealthValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHealthValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "400 Armor",
                            OverallPoints = (calcArmorValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcArmorValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcArmorValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcArmorValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "100 Dodge Rating",
                            OverallPoints = (calcDodgeValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcDodgeValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcDodgeValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "100 Parry Rating",
                            OverallPoints = (calcParryValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcParryValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcParryValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcParryValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "100 Mastery Rating",
                            OverallPoints = (calcMasteryValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcMasteryValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcMasteryValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcMasteryValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "100 Haste Rating",
                            OverallPoints = (calcHasteValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcHasteValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHasteValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHasteValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "100 Expertise Rating",
                            OverallPoints = (calcExpertiseValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcExpertiseValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcExpertiseValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcExpertiseValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtWarr() { Name = "100 Hit Rating",
                            OverallPoints = (calcHitValue.OverallPoints - calcBaseValue.OverallPoints), 
                            MitigationPoints = (calcHitValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHitValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHitValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                    };
                #endregion
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            // Filters out Non-Shield Offhand Enchants and Ranged Enchants
            if ((slot == ItemSlot.OffHand && enchant.Slot != ItemSlot.OffHand) || slot == ItemSlot.Ranged) return false;
            // Filters out Death Knight and Two-Hander Enchants
            if (enchant.Name.StartsWith("Rune of the") || enchant.Slot == ItemSlot.TwoHand) return false;
            
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            // Only Displays Shields in Off-Hand Slot as Devastate is Shield-Only Post-3.2
            if (slot == CharacterSlot.OffHand && item.Type != ItemType.Shield)
                return false;

            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        public override bool IsItemRelevant(Item item)
        {
            // Fishing Poles, they love to muck up the list!
            if (item.Id == 45991 || item.Id == 25978)
                return false;

            return base.IsItemRelevant(item);
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            bool NotClassSetBonus = 
                ((buff.Group == "Set Bonuses") && !(
                    buff.Name.StartsWith("Dreadnaught") || 
                    buff.Name.StartsWith("Siegebreaker") || 
                    buff.Name.StartsWith("Wrynn") ||
                    buff.Name.StartsWith("Ymirjar") ||
                    buff.Name.Contains("Gladiator") ||
                    buff.Name.Contains("Earthen Battleplate") ||
                    buff.Name.Contains("Molten Giant Battleplate"))
                );
            if (buff.SpellId == 70843) { return true; } // T10 2P
            if (buff.SpellId == 70844) { return true; } // T10 4P
            if (buff.SpellId == 90296) { return true; } // T11 2P
            if (buff.SpellId == 90297) { return true; } // T11 4P
            if (buff.SpellId == 99239) { return true; } // T12 2P
            if (buff.SpellId == 99242) { return true; } // T12 4P

            return base.IsBuffRelevant(buff, character) && !NotClassSetBonus;
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats relevantStats = new Stats()
            {
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                Dodge = stats.Dodge,
                DodgeRating = stats.DodgeRating,
                ParryRating = stats.ParryRating,
                BlockRating = stats.BlockRating,
                MasteryRating = stats.MasteryRating,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                Health = stats.Health,
                BattlemasterHealthProc = stats.BattlemasterHealthProc,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                DamageTakenReductionMultiplier = stats.DamageTakenReductionMultiplier,
                BossPhysicalDamageDealtReductionMultiplier = stats.BossPhysicalDamageDealtReductionMultiplier,
                PhysicalDamageTakenReductionMultiplier = stats.PhysicalDamageTakenReductionMultiplier,
                Miss = stats.Miss,
                ArcaneResistance = stats.ArcaneResistance,
                NatureResistance = stats.NatureResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                ShadowResistance = stats.ShadowResistance,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,

                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                CritRating = stats.CritRating,
                PhysicalCrit = stats.PhysicalCrit,
                HitRating = stats.HitRating,
                PhysicalHit = stats.PhysicalHit,
                HasteRating = stats.HasteRating,
                PhysicalHaste = stats.PhysicalHaste,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                TargetArmorReduction = stats.TargetArmorReduction,
                WeaponDamage = stats.WeaponDamage,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,
                BonusBlockValueMultiplier = stats.BonusBlockValueMultiplier,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                BossAttackSpeedReductionMultiplier = stats.BossAttackSpeedReductionMultiplier,

                HighestStat = stats.HighestStat,
                HighestSecondaryStat = stats.HighestSecondaryStat,
                Paragon = stats.Paragon,
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger) && HasRelevantStats(effect.Stats))
                {
                    relevantStats.AddSpecialEffect(effect);
                }
            }

            return relevantStats;
        }

        private List<Trigger> _relevantTriggers = null;
        public List<Trigger> RelevantTriggers {
            get {
                if (_relevantTriggers == null) {
                    _relevantTriggers = new List<Trigger>(){
                        Trigger.Use,
                        Trigger.MeleeCrit,
                        Trigger.MeleeHit,
                        Trigger.PhysicalCrit,
                        Trigger.PhysicalHit,
                        Trigger.DoTTick,
                        Trigger.DamageDone,
                        Trigger.DamageOrHealingDone,
                        Trigger.DamageParried,
                        Trigger.DamageAvoided,
                        Trigger.DamageTaken,
                        Trigger.DamageTakenPutsMeBelow35PercHealth,
                        Trigger.ShieldBlock,
                    };
                }
                return _relevantTriggers;
            }
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool superRelevant  = IsSuperRelevant(stats);   // Stats that will automatically mark the item as relevant
            bool relevantEnchant= IsRelevantEnchant(stats); // Stats that appear on the Peerless Stats enchant
            bool relevant       = false;                    // Stats which are potentially relevant
            bool notRelevant    = false;                    // Stats which mark the item as irrelevant if not super-relevant

            if (!superRelevant && !relevantEnchant)
            {
                relevant    = IsRelevant(stats);
                notRelevant = IsNotRelevant(stats);
                
                foreach (SpecialEffect effect in stats.SpecialEffects())
                {
                    if (RelevantTriggers.Contains(effect.Trigger))
                    {
                        superRelevant |= IsSuperRelevant(effect.Stats);
                        if (superRelevant)
                            break;

                        relevant    |= IsRelevant(effect.Stats);
                        notRelevant &= IsNotRelevant(effect.Stats);
                    }
                }
            }

            return (superRelevant || relevantEnchant || (relevant && !notRelevant));
        }

        private bool IsSuperRelevant(Stats stats)
        {
            return
                (
                    stats.BonusArmor + stats.BonusArmorMultiplier +
                    stats.BonusStaminaMultiplier + stats.Dodge + stats.DodgeRating + stats.ParryRating + stats.BlockRating + stats.MasteryRating + stats.BonusHealthMultiplier +
                    stats.DamageTakenReductionMultiplier + stats.PhysicalDamageTakenReductionMultiplier + stats.BossPhysicalDamageDealtReductionMultiplier + stats.Miss +
                    stats.ArcaneResistance + stats.NatureResistance + stats.FireResistance +
                    stats.FrostResistance + stats.ShadowResistance + stats.ArcaneResistanceBuff +
                    stats.NatureResistanceBuff + stats.FireResistanceBuff + stats.FrostResistanceBuff + stats.ShadowResistanceBuff +
                    stats.ThreatIncreaseMultiplier + stats.BonusBlockValueMultiplier
                ) != 0;
        }

        private bool IsRelevant(Stats stats)
        {
            return
                (stats.Agility + stats.Armor +
                    stats.BonusAgilityMultiplier + stats.BonusStrengthMultiplier + stats.BonusAttackPowerMultiplier +
                    stats.Health + stats.BattlemasterHealthProc + stats.Stamina +
                    stats.Strength + stats.AttackPower + stats.CritRating + stats.HitRating + stats.HasteRating +
                    stats.PhysicalHit + stats.PhysicalHaste + stats.PhysicalCrit +
                    stats.ExpertiseRating + stats.ArmorPenetration + stats.TargetArmorReduction + stats.WeaponDamage +
                    stats.BonusCritDamageMultiplier +
                    stats.BonusDamageMultiplier + stats.BonusWhiteDamageMultiplier +
                    stats.BonusBleedDamageMultiplier + stats.BossAttackSpeedReductionMultiplier +
                    stats.HighestStat + stats.HighestSecondaryStat + stats.Paragon +
                    stats.MovementSpeed + stats.FearDurReduc + stats.StunDurReduc + stats.SnareRootDurReduc
                ) != 0;
        }

        private bool IsNotRelevant(Stats stats)
        {
            return
                (stats.Spirit + stats.Intellect + stats.SpellPower + stats.Mana + stats.Mp5 + stats.SpellPenetration +
                    stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier + stats.BonusManaMultiplier
                ) != 0;
        }

        private bool IsRelevantEnchant(Stats stats)
        {
            return ( 
                // Peerless Stats
                (stats.Agility == 20 && stats.Strength == 20 && stats.Stamina == 20 && stats.Intellect == 20 && stats.Spirit == 20) 
                );
        }

    }
}
