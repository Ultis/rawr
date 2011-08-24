using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;
using Rawr.Hunter.Skills;

namespace Rawr.Hunter {
    [Rawr.Calculations.RawrModelInfo("Hunter", "Inv_Weapon_Bow_07", CharacterClass.Hunter)]
    public class CalculationsHunter : CalculationsBase
    {
        #region Variables and Properties

        #region Hunter Gemming Templates
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // == Relevant Gem IDs for Hunters ==
                #region Red
                int[] delicate = { 52082, 52212, 52212, 52258 };    // Agi
                #endregion
                #region Purple
                int[] shifting = { 52096, 52238, 52238, 52238 };    // Agi / Stam - Shifting
                int[] glinting = { 52102, 52220, 52220, 52220 };    // Agi / Hit - Glinting
                #endregion
                #region Blue
                int[] rigid = { 52089, 52235, 52235, 52264 };       // Hit - Rigid
                #endregion
                #region Green
                int[] forceful = { 52124, 52218, 52218, 52218 };    // Haste / Stam - Forceful
                int[] jagged = { 52121, 52223, 52223, 52223 };      // Crit  / Stam - Jagged
                int[] lightning = { 52125, 52225, 52225, 52225 };   // Haste / Hit - Lightning
                int[] piercing = { 52122, 52228, 52228, 52228 };    // Crit / Hit - Piercing
                int[] puissant = { 52126, 52231, 52231, 52231 };    // Mastery / Stam - Puissant
                int[] sensei = { 52128, 52237, 52237, 52237 };      // Mastery / Hit - Sensei
                #endregion
                #region Yellow
                int[] fractured = { 52094, 52219, 52219, 52269 };   // Mastery - Fractured
                int[] quick = { 52093, 52232, 52232, 52268 };       // Haste - Quick
                int[] smooth = { 52091, 52241, 52241, 52266 };      // Crit - Smooth
                #endregion
                #region Orange
                int[] adept = { 52115, 52204, 52204, 52204 };       // Agi / Mastery - Adept
                int[] deadly = { 52109, 52209, 52209, 52209 };      // Agi / Crit - Deadly
                int[] deft = { 52112, 52211, 52211, 52211 };        // Agi / Haste - Deft
                #endregion
                #region Meta
                int agile = 68778; // 54 Agi  3% Crit DMG
                int chaotic = 52291; // 54 Crit 3% Crit DMG
                #endregion
                #region Cogwheel
                int[] cog_hit = { 59493, 59493, 59493, 59493 }; fixArray(cog_hit);
                int[] cog_mst = { 59480, 59480, 59480, 59480 }; fixArray(cog_mst);
                int[] cog_crt = { 59478, 59478, 59478, 59478 }; fixArray(cog_crt);
                int[] cog_has = { 59479, 59479, 59479, 59479 }; fixArray(cog_has);
                #endregion

                List<GemmingTemplate> list = new List<GemmingTemplate>();
                for (int tier = 0; tier < 2; tier++)
                {
                    list.AddRange(new GemmingTemplate[]
                        {
                            CreateHunterGemmingTemplate(tier, delicate, delicate, delicate, delicate, agile), 
                            CreateHunterGemmingTemplate(tier, delicate, adept, glinting, delicate, agile),
                            CreateHunterGemmingTemplate(tier, delicate, deadly, shifting, delicate, agile),
                            CreateHunterGemmingTemplate(tier, delicate, deft, glinting, delicate, chaotic),


                            new GemmingTemplate() { Model = "Hunter", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_mst[0], MetaId = agile, },
                            new GemmingTemplate() { Model = "Hunter", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_crt[0], MetaId = agile, },
                            new GemmingTemplate() { Model = "Hunter", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_has[0], MetaId = agile, },

                            new GemmingTemplate() { Model = "Hunter", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_crt[0], MetaId = agile, },
                            new GemmingTemplate() { Model = "Hunter", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_has[0], MetaId = agile, },

                            new GemmingTemplate() { Model = "Hunter", Group = "Cogwheels", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_has[0], MetaId = agile, },
                        });
                }

                return list;
            }
        }

        private static void fixArray(int[] thearray)
        {
            if (thearray[0] == 0) return; // Nothing to do, they are all 0
            if (thearray[1] == 0) thearray[1] = thearray[0]; // There was a Green Rarity, but no Blue Rarity
            if (thearray[2] == 0) thearray[2] = thearray[1]; // There was a Blue Rarity (or Green Rarity as set above), but no Purple Rarity
            if (thearray[3] == 0) thearray[3] = thearray[2]; // There was a Purple Rarity (or Blue Rarity/Green Rarity as set above), but no Jewel
        }
        private const int DEFAULT_GEMMING_TIER = 1;
        private GemmingTemplate CreateHunterGemmingTemplate(int tier, int[] red, int[] yellow, int[] blue, int[] prismatic, int meta)
        {
            return new GemmingTemplate()
            {
                Model = "Hunter",
                Group = (new string[] { "Uncommon", "Rare", "Epic", "Jeweler" })[tier],
                Enabled = (tier == DEFAULT_GEMMING_TIER),
                RedId = red[tier],
                YellowId = yellow[tier],
                BlueId = blue[tier],
                PrismaticId = prismatic[tier],
                MetaId = meta
            };
        }
        #endregion

        #region Display Strings
        public override string GetCharacterStatsString(Character character)
        {
            if (character == null) { return ""; }
            StringBuilder stats = new StringBuilder();
            stats.AppendFormat("Character:\t\t{0}@{1}-{2}\r\nRace:\t\t{3}",
                character.Name, character.Region, character.Realm, character.Race);

            char[] splits = { ':', '*' };
            Dictionary<string, string> dict = GetCharacterCalculations(character, null, false, false, true).GetAsynchronousCharacterDisplayCalculationValues();
            foreach (string s in CharacterDisplayCalculationLabels)
            {
                string[] label = s.Split(splits);
                if (dict.ContainsKey(label[1]))
                {
                    stats.AppendFormat("\r\n{0}:\t\t{1}", label[1], dict[label[1]].Split('*')[0]);
                }
            }

            return stats.ToString();
        }
        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                {
                    _characterDisplayCalculationLabels = new string[] {
                        #region Basic Stats
                        "Basic Stats:Health and Stamina",
                        "Basic Stats:Focus",
                        "Basic Stats:Armor",
                        "Basic Stats:Agility",
                        "Basic Stats:Ranged Attack Power",
                        @"Basic Stats:Hit*8.00% chance to miss base for Yellow Attacks",
//Focused Aim 0 - 8%-0%=8%=263 Rating soft cap
//Focused Aim 1 - 8%-1%=7%=230 Rating soft cap
//Focused Aim 2 - 8%-2%=6%=197 Rating soft cap
//Focused Aim 3 - 8%-3%=5%=164 Rating soft cap",
                        "Basic Stats:Crit",
                        "Basic Stats:Haste",
                        "Basic Stats:Mastery",
                        #endregion

                        #region Pet Stats
                        "Pet Stats:Pet Health",
                        "Pet Stats:Pet Armor",
                        "Pet Stats:Pet Focus",
                        "Pet Stats:Pet Attack Power",
                        "Pet Stats:Pet Hit %",
                        "Pet Stats:Pet Dodge %",
                        "Pet Stats:Pet Melee Crit %",
                        "Pet Stats:Pet Specials Crit %",
                        "Pet Stats:Pet White DPS",
                        "Pet Stats:Pet Kill Command DPS",
                        "Pet Stats:Pet Specials DPS",
                        #endregion

                        #region Shot Information
                        "Shot Stats:Aimed Shot",
                        "Shot Stats:Arcane Shot",
                        "Shot Stats:Multi Shot",
                        "Shot Stats:Cobra Shot",
                        "Shot Stats:Steady Shot",
                        "Shot Stats:Kill Shot",
                        "Shot Stats:Explosive Shot",
                        "Shot Stats:Black Arrow",
                        "Shot Stats:Chimera Shot",
                        #endregion

                        #region Sting Information
                        "Sting Stats:Serpent Sting",
                        #endregion

                        #region Trap Information
                        "Trap Stats:Immolation Trap",
                        "Trap Stats:Explosive Trap",
                        "Trap Stats:Freezing Trap",
                        "Trap Stats:Frost Trap",
                        #endregion

                        #region DPS Numbers
                        "Hunter DPS:Autoshot DPS",
                        "Hunter DPS:Priority Rotation DPS",
                        "Hunter DPS:Wild Quiver DPS",
                        "Hunter DPS:Kill Shot low HP gain",
                        "Hunter DPS:Aspect Loss",
                        "Hunter DPS:Piercing Shots DPS",
                        "Hunter DPS:Special DMG Procs DPS*Like Bandit's Insignia or Hand-Mounted Pyro Rockets",
                        #endregion

                        #region Combined Information
                        "Combined DPS:Hunter DPS",
                        "Combined DPS:Pet DPS",
                        "Combined DPS:Total DPS"
                        #endregion
                    };
                }
                return _characterDisplayCalculationLabels;
            }
        }
        #endregion

        #region Optimizable Labels
        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                {
                    _optimizableCalculationLabels = new string[] {
                        "Health",
                        "Attack Power",
                        "Agility",
                        "Crit %",
                        "Haste %",
                        "% Chance to Miss (Yellow)",
                    };
                }
                return _optimizableCalculationLabels;
            }
        }
        #endregion

        #region Color Variables
        private Dictionary<string, Color> _subPointNameColors = null;
        private Dictionary<string, Color> _subPointNameColorsDPS = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColorsDPS == null)
                {
                    _subPointNameColorsDPS = new Dictionary<string, Color>();
                    _subPointNameColorsDPS.Add("Hunter DPS", Color.FromArgb(255, 255, 0, 0));
                    _subPointNameColorsDPS.Add("Pet DPS", Color.FromArgb(255, 255, 100, 0));
                    _subPointNameColorsDPS.Add("Hunter Survivability", Color.FromArgb(255, 64, 128, 32));
                    _subPointNameColorsDPS.Add("Pet Survivability", Color.FromArgb(255, 29, 131, 87));
                }
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = _subPointNameColorsDPS;
                }
                return _subPointNameColors;
            }
        }
        #endregion

        public override CharacterClass TargetClass { get { return CharacterClass.Hunter; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHunter(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHunter(); }
        public ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel { get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelHunter()); } }

        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            CalculationOptionsHunter calcOpts = null;
            StringReader sr = null;
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(CalculationOptionsHunter));
                sr = new StringReader(xml);
                calcOpts = s.Deserialize(sr) as CalculationOptionsHunter;
            }
            finally { sr.Dispose(); }

            // convert buffs here!
            calcOpts.petActiveBuffs = new List<Buff>(calcOpts._petActiveBuffsXml.ConvertAll(buff => Buff.GetBuffByName(buff)));
            calcOpts.petActiveBuffs.RemoveAll(buff => buff == null);

            return calcOpts;
        }

        #endregion

        #region Relevancy

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<ItemType>(new[] {
                        ItemType.None,
                        ItemType.Leather, ItemType.Mail,
                        ItemType.Bow, ItemType.Crossbow, ItemType.Gun,
                        ItemType.FistWeapon, ItemType.Dagger, ItemType.OneHandAxe, ItemType.OneHandSword,
                        ItemType.Polearm, ItemType.Staff,
                        ItemType.TwoHandAxe, ItemType.TwoHandSword
                    });
                }
                return _relevantItemTypes;
            }
        }

        private static List<string> _relevantGlyphs = null;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>() {
                    #region Prime
                    "Glyph of Aimed Shot", //   @"When you critically hit with Aimed Shot, you instantly gain 5 Focus.")]
                    "Glyph of Arcane Shot",  // @"Your Arcane Shot deals 12% more damage.")]
                    "Glyph of Chimera Shot", // @"Reduces the cooldown of Chimera Shot by 1 sec.")]
                    // "Glyph of Dazzled Prey", // @"Your Steady Shot generates an additional 2 Focus on targets afflicted by a daze effect.")]
                    "Glyph of Explosive Shot", //   @"Increases the critical strike chance of Explosive Shot by 6%.")]
                    "Glyph of Kill Command", // @"Reduces the Focus cost of your Kill Command by 3.")]
                    "Glyph of Kill Shot", // @"If the damage from your Kill Shot fails to kill a target at or below 20% health, your Kill Shot's cooldown is instantly reset. This effect has a 6 sec cooldown.")]
                    "Glyph of Rapid Fire", // @"Increases the haste from Rapid Fire by an additional 10%.")]
                    "Glyph of Serpent Sting", // @"Increases the periodic critical strike chance of your Serpent Sting by 6%.")]
                    "Glyph of Steady Shot", // @"Increases the damage dealt by Steady Shot by 10%.")]
                    #endregion
                    #region Major
                    "Glyph of Bestial Wrath", // @"Decreases the cooldown of Bestial Wrath by 20 sec.")]
                    // "Glyph of Concussive Shot", // @"Your Concussive Shot also limits the maximum run speed of your target.")]
                    // "Glyph of Deterrence", // @"Decreases the cooldown of Deterrence by 10 sec.")]
                    // "Glyph of Disengage", // @"Decreases the cooldown of Disengage by 5 sec.")]
                    "Glyph of Freezing Trap", // @"When your Freezing Trap breaks, the victim's movement speed is reduced by 70% for 4 sec.")]
                    "Glyph of Ice Trap", // @"Increases the radius of the effect from your Ice Trap by 2 yards.")]
                    "Glyph of Immolation Trap", // @"Decreases the duration of the effect from your Immolation Trap by 6 sec., but damage while active is increased by 100%.")]
                    "Glyph of Master's Call", // @"Increases the duration of your Master's Call by 4 sec.")]
                    // "Glyph of Mending", // @"Increases the total amount of healing done by your Mend Pet ability by 60%.")]
                    // "Glyph of Misdirection", // @"When you use Misdirection on your pet, the cooldown on your Misdirection is reset.")]
                    // "Glyph of Raptor Strike", // @"Reduces damage taken by 20% for 5 sec after using Raptor Strike.")]
                    "Glyph of Scatter Shot", // @"Increases the range of Scatter Shot by 3 yards.")]
                    "Glyph of Silencing Shot", // @"When you successfully silence an enemy's spell cast with Silencing Shot, you instantly gain 10 focus.")]
                    "Glyph of Snake Trap", // @"Snakes from your Snake Trap take 90% reduced damage from area of effect spells.")]
                    "Glyph of Trap Launcher", // @"Reduces the focus cost of Trap Launcher by 10.")]
                    "Glyph of Wyvern Sting", // @"Decreases the cooldown of your Wyvern Sting by 6 sec.")]
                    #endregion
                    #region Minor
                    "Glyph of Aspect of the Pack", // @"Increases the range of your Aspect of the Pack ability by 15 yards.")]
                    "Glyph of Feign Death", // @"Reduces the cooldown of your Feign Death spell by 5 sec.")]
                    "Glyph of Lesser Proportion", // @"Slightly reduces the size of your Pet.")]
                    "Glyph of Revive Pet", // @"Reduces the pushback suffered from damaging attacks while casting Revive Pet by 100%.")]
                    "Glyph of Scare Beast" // @"Reduces the pushback suffered from damaging attacks while casting Scare Beast by 75%.")]
                    #endregion
                };
            }
            return _relevantGlyphs;
        }

        private bool HidingBadStuff { get { return HidingBadStuff_Def || HidingBadStuff_Spl || HidingBadStuff_PvP; } }
        private static bool _HidingBadStuff_Def = false;
        internal static bool HidingBadStuff_Def { get { return _HidingBadStuff_Def; } set { _HidingBadStuff_Def = value; } }
        private static bool _HidingBadStuff_Spl = true;
        internal static bool HidingBadStuff_Spl { get { return _HidingBadStuff_Spl; } set { _HidingBadStuff_Spl = value; } }
        private static bool _HidingBadStuff_PvP = false;
        internal static bool HidingBadStuff_PvP { get { return _HidingBadStuff_PvP; } set { _HidingBadStuff_PvP = value; } }

        internal static List<Trigger> _RelevantTriggers = null;
        internal static List<Trigger> RelevantTriggers {
            get {
                return _RelevantTriggers ?? (_RelevantTriggers = new List<Trigger>() {
                    Trigger.Use,
                    //
                    Trigger.PhysicalAttack,
                    Trigger.RangedHit,
                    Trigger.PhysicalHit,
                    Trigger.RangedCrit,
                    Trigger.PhysicalCrit,
                    //
                    Trigger.DoTTick,
                    Trigger.DamageDone,
                    Trigger.DamageOrHealingDone,
                    // Trigger.DamageTaken,
                    // Pets only
                    Trigger.MeleeHit,
                    Trigger.MeleeCrit,
                    Trigger.PetClawBiteSmackCrit,
                    // Hunter Specific
                    Trigger.HunterAutoShotHit,
                    Trigger.SteadyShotHit,
                    Trigger.CobraShotHit,
                    Trigger.SerpentWyvernStingsDoDamage,
                    Trigger.EnergyOrFocusDropsBelow20PercentOfMax,
                });
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            StatsHunter relevantStats = new StatsHunter() {
                #region Basic Stats
                Stamina = stats.Stamina,
                Health = stats.Health,
                Agility = stats.Agility,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                #endregion

                #region Ratings
                AttackPower = stats.AttackPower,
                RangedAttackPower = stats.RangedAttackPower,
                PhysicalCrit = stats.PhysicalCrit,
                CritRating = stats.CritRating,
                RangedCritRating = stats.RangedCritRating,
                PhysicalHit = stats.PhysicalHit,
                HitRating = stats.HitRating,
                RangedHitRating = stats.RangedHitRating,
                PhysicalHaste = stats.PhysicalHaste,
                HasteRating = stats.HasteRating,
                RangedHasteRating = stats.RangedHasteRating,
                MasteryRating = stats.MasteryRating,

                TargetArmorReduction = stats.TargetArmorReduction,
                Miss = stats.Miss,
                ScopeDamage = stats.ScopeDamage,
                #endregion

                #region Special
                HighestStat = stats.HighestStat,
                HighestSecondaryStat = stats.HighestSecondaryStat,
                Paragon = stats.Paragon,
                MovementSpeed = stats.MovementSpeed,
                StunDurReduc = stats.StunDurReduc,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                DarkmoonCardDeathProc = stats.DarkmoonCardDeathProc,
                #endregion

                #region Survivability
                HealthRestore = stats.HealthRestore,
                HealthRestoreFromMaxHealth = stats.HealthRestoreFromMaxHealth,
                #endregion

                // Set Bonuses

                #region Multipliers
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,

                DamageTakenReductionMultiplier = stats.DamageTakenReductionMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                //BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                // BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                #endregion

                #region Damage Procs
                ShadowDamage = stats.ShadowDamage,
                ArcaneDamage = stats.ArcaneDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,
                FrostDamage = stats.FrostDamage,
                FireDamage = stats.FireDamage,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                #endregion
            };
           foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (RelevantTriggers.Contains(effect.Trigger) && (HasRelevantStats(effect.Stats) || HasSurvivabilityStats(effect.Stats)))
                {
                    relevantStats.AddSpecialEffect(effect);
                }
            }
            return relevantStats;
        }
        public override bool HasRelevantStats(Stats stats) {
            bool relevant = HasWantedStats(stats) && !HasIgnoreStats(stats);
            return relevant;
        }

        private bool HasWantedStats(Stats stats)
        {
            #region Base Stats
            if (stats.Agility != 0) { return true; }
//            if (stats.Strength != 0) { return true; }  // Hunters don't need STR.
            if (stats.AttackPower != 0) { return true; }
            if (stats.RangedAttackPower != 0) { return true; }
            #endregion
            #region Ratings
            if (stats.CritRating != 0) { return true; }
            if (stats.RangedCritRating != 0) { return true; }
            if (stats.HasteRating != 0) { return true; }
            if (stats.RangedHasteRating != 0) { return true; }
            if (stats.PhysicalHaste != 0) { return true; }
            if (stats.HitRating != 0) { return true; }
            if (stats.RangedHitRating != 0) { return true; }
            if (stats.MasteryRating != 0) { return true; }
            #endregion
            #region Bonuses
            if (stats.TargetArmorReduction != 0) { return true; }
            if (stats.PhysicalCrit != 0) { return true; }
            if (stats.RangedHaste != 0) { return true; }
            if (stats.PhysicalHit != 0) { return true; }
            if (stats.MovementSpeed != 0) { return true; }
            if (stats.StunDurReduc != 0) { return true; }
            if (stats.SnareRootDurReduc != 0) { return true; }
            if (stats.FearDurReduc != 0) { return true; }
            #endregion
            // Target Debuffs
            #region Procs
            if (stats.DarkmoonCardDeathProc != 0) { return true; }
            if (stats.HighestStat != 0) { return true; }
            if (stats.HighestSecondaryStat != 0) { return true; }
            if (stats.Paragon != 0) { return true; }
            if (stats.ManaorEquivRestore != 0) { return true; }
            #endregion
            #region Damage Procs
            if (stats.ShadowDamage != 0) { return true; }
            if (stats.ArcaneDamage != 0) { return true; }
            if (stats.HolyDamage != 0) { return true; }
            if (stats.NatureDamage != 0) { return true; }
            if (stats.FrostDamage != 0) { return true; }
            if (stats.FireDamage != 0) { return true; }
            if (stats.BonusShadowDamageMultiplier != 0) { return true; }
            if (stats.BonusArcaneDamageMultiplier != 0) { return true; }
            if (stats.BonusHolyDamageMultiplier != 0) { return true; }
            if (stats.BonusNatureDamageMultiplier != 0) { return true; }
            if (stats.BonusFrostDamageMultiplier != 0) { return true; }
            if (stats.BonusFireDamageMultiplier != 0) { return true; }
            #endregion
            #region Multipliers
            if (stats.BonusAgilityMultiplier != 0) { return true; }
            if (stats.BonusAttackPowerMultiplier != 0) { return true; }
            if (stats.BonusCritDamageMultiplier != 0) { return true; }
            if (stats.BonusIntellectMultiplier != 0) { return true; }
            if (stats.BonusDamageMultiplier != 0) { return true; }
//            if (stats.BonusSpiritMultiplier != 0) { return true; }
            if (stats.DamageTakenReductionMultiplier != 0) { return true; }
            if (stats.BaseArmorMultiplier != 0) { return true; }
            if (stats.BonusArmorMultiplier != 0) { return true; }
            if (stats.BonusBleedDamageMultiplier != 0) { return true; }
            if (stats.BonusPhysicalDamageMultiplier != 0) { return true; }
//            if (stats.BonusManaMultiplier != 0) { return true; } // No focus any more.
            #endregion
            // Set Bonuses
            #region Special
            if (stats.ScopeDamage != 0) { return true; }
            // if (stats.BonusManaPotionEffectMultiplier != 0) { return true; }
            #endregion
            // Special Effects
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger) && HasRelevantStats(effect.Stats))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasSurvivabilityStats(Stats stats)
        {
            // Health Base
            if (stats.Stamina != 0) { return true; }
            if (stats.Health != 0) { return true; }
            if (stats.BonusStaminaMultiplier != 0) { return true; }
            if (stats.BonusHealthMultiplier != 0) { return true; }
            // Health Regen
            if (stats.HealthRestore != 0) { return true; }
            if (stats.HealthRestoreFromMaxHealth != 0) { return true; }
            // Armor
            if (stats.Armor != 0) { return true; }
            if (stats.BonusArmor != 0) { return true; }
            if (stats.BaseArmorMultiplier != 0) { return true; }
            if (stats.BonusArmorMultiplier != 0) { return true; }
            // Multipliers
            if (stats.DamageTakenReductionMultiplier != 0) { return true; }
            // Target Debuffs
            if (stats.BossPhysicalDamageDealtReductionMultiplier != 0) { return true; }
            // Special Effects
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTriggers.Contains(effect.Trigger) && HasSurvivabilityStats(effect.Stats))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasIgnoreStats(Stats stats)
        {
            if (!HidingBadStuff) { return false; }

            // Remove Spellcasting Stuff
            if (HidingBadStuff_Spl)
            {
                if (stats.Mp5 != 0) { return true; }
                if (stats.SpellPower != 0) { return true; }
                if (stats.Mana != 0) { return true; }
                if (stats.ManaRestore != 0) { return true; }
                if (stats.Spirit != 0) { return true; }
                if (stats.Intellect != 0) { return true; }
                if (stats.BonusSpiritMultiplier != 0) { return true; }
                if (stats.BonusIntellectMultiplier != 0) { return true; }
                if (stats.SpellPenetration != 0) { return true; }
                if (stats.BonusManaMultiplier != 0) { return true; }
            }
            // Remove Defensive Stuff
            if (HidingBadStuff_Def)
            {
                if (stats.Dodge != 0) { return true; }
                if (stats.Parry != 0) { return true; }
                if (stats.DodgeRating != 0) { return true; }
                if (stats.ParryRating != 0) { return true; }
                if (stats.BlockRating != 0) { return true; }
                if (stats.Block != 0) { return true; }
                if (stats.SpellReflectChance != 0) { return true; }
            }
            // Remove PvP Items
            if (HidingBadStuff_PvP)
            {
                if (stats.Resilience != 0) { return true; }
            }
            // Special Effects
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                // The effect doesn't have a relevant trigger
                if (!RelevantTriggers.Contains(effect.Trigger)) { return true; }
                // The Effect has Ignore Stats
                if (HasIgnoreStats(effect.Stats)) { return true; }
            }
            return false;
        }

        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.Ranged && (item.Type != ItemType.Gun && item.Type != ItemType.Bow && item.Type != ItemType.Crossbow)) { return false; }
            else
            {
                Stats stats = item.Stats;
                bool wantedStats = HasWantedStats(stats);
                bool survstats = HasSurvivabilityStats(stats);
                bool ignoreStats = HasIgnoreStats(stats);
                return (wantedStats || survstats) && !ignoreStats && base.IsItemRelevant(item);
            }
        }

        public override bool IsEnchantRelevant(Enchant enchant, Character character)
        {
            if (enchant == null || character == null) { return false; }
            return
                IsEnchantAllowedForClass(enchant, character.Class) &&
                IsProfEnchantRelevant(enchant, character) &&
                (HasWantedStats(enchant.Stats) ||
                    (HasSurvivabilityStats(enchant.Stats) && !HasIgnoreStats(enchant.Stats)));
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            string name = buff.Name;
            // Force some buffs to active
            if (name.Contains("Flask of the Winds")
                || name.Contains("Potion of the Tol'vir")
                || name.Contains("Agility Food")
            ) {
                return true;
            }
            // Force some buffs to go away
            else if (!buff.AllowedClasses.Contains(CharacterClass.Warrior))
            { return false; }
            else if (character != null && Rawr.Properties.GeneralSettings.Default.HideProfEnchants && !character.HasProfession(buff.Professions))
            { return false; }
            //
            bool haswantedStats = HasWantedStats(buff.Stats);
            bool hassurvStats = HasSurvivabilityStats(buff.Stats);
            bool hasbadstats = HasIgnoreStats(buff.Stats);
            bool retVal = haswantedStats || (hassurvStats && !hasbadstats);
            return retVal;
        }

        private static readonly SpecialEffect _SE_2T12_cs = new SpecialEffect(Trigger.CobraShotHit, new StatsHunter() { BonusFireWeaponDamage = 0.80f, }, 0f, 0f, 0.10f);
        private static readonly SpecialEffect _SE_2T12_ss = new SpecialEffect(Trigger.SteadyShotHit, new StatsHunter() { BonusFireWeaponDamage = 0.80f, }, 0f, 0f, 0.10f);

        private static readonly SpecialEffect _SE_4T12 = new SpecialEffect(Trigger.HunterAutoShotHit, new StatsHunter() { FourPieceTier12 = 1f, }, 2f, 15, 0.1f);

        public StatsHunter GetBuffsStats(Character character, CalculationOptionsHunter calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            List<Buff> buffGroup = new List<Buff>();
            #region Maintenance Auto-Fixing
            /* Removes the Sunder Armor if you are maintaining it yourself
             * Also removes Acid Spit and Expose Armor
             * We are now calculating this internally for better accuracy and to provide value to relevant talents
             * if (calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SunderArmor_])
             * {
             *   buffGroup.Clear();
             *   buffGroup.Add(Buff.GetBuffByName("Sunder Armor"));
             *   buffGroup.Add(Buff.GetBuffByName("Acid Spit"));
             *   buffGroup.Add(Buff.GetBuffByName("Expose Armor"));
             *   MaintBuffHelper(buffGroup, character, removedBuffs);
             * }
             */
            #endregion

            #region Passive Ability Auto-Fixing
            // Removes the Trueshot Aura Buff and it's equivalents Unleashed Rage and Abomination's Might if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            if (character.HunterTalents.TrueshotAura > 0) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Trueshot Aura"));
                buffGroup.Add(Buff.GetBuffByName("Unleashed Rage"));
                buffGroup.Add(Buff.GetBuffByName("Abomination's Might"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }

            // Removes the Ferocious Inspiration Buff and it's equivalents Sanctified Retribution and Arcane Tactics if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            if (character.HunterTalents.FerociousInspiration > 0) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Ferocious Inspiration"));
                buffGroup.Add(Buff.GetBuffByName("Sanctified Retribution"));
                buffGroup.Add(Buff.GetBuffByName("Arcane Tactics"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }


            // Removes the Hunting Party Buff and it's equivalents Improved Icy Talons and Windfury Totem if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            if (character.HunterTalents.HuntingParty > 0)
            {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Hunting Party"));
                buffGroup.Add(Buff.GetBuffByName("Improved Icy Talons"));
                buffGroup.Add(Buff.GetBuffByName("Windfury Totem"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }

            // Removes the Hunter's Mark if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            buffGroup.Clear();
            buffGroup.Add(Buff.GetBuffByName("Hunter's Mark"));
            MaintBuffHelper(buffGroup, character, removedBuffs);
            #endregion

            StatsHunter statsBuffs = new StatsHunter();
            statsBuffs.Accumulate(GetBuffsStats(character.ActiveBuffs));
            AccumulateSetBonusStats(statsBuffs, character.SetBonusCount);

            #region PvP Set Bonus
            int PvPcount;
            character.SetBonusCount.TryGetValue("Gladiator's Pursuit", out PvPcount);
            if (PvPcount >= 2)
            {
                statsBuffs.Resilience += 400f;
                statsBuffs.Agility += 70f;
            }
            if (PvPcount >= 4)
            {
                statsBuffs.BonusFocusRegenMultiplier = 0.05f;
                statsBuffs.Agility += 90f;
            }
            #endregion

            #region Tier 11 Set Bonus
            int T11count;
            character.SetBonusCount.TryGetValue("Lightning-Charged Battlegear", out T11count);
            if (T11count >= 2)
            {
                statsBuffs.BonusSerpentStingCritChance = 0.05f;
            }
            if (T11count >= 4)
            {
                statsBuffs.FourPieceTier11 = 0.2f;
            }
            #endregion

            #region Tier 12 Set Bonus
            int T12count;
            character.SetBonusCount.TryGetValue("Flamewaker's Battlegear", out T12count);
            if (T12count >= 2)
            {
                statsBuffs.AddSpecialEffect(_SE_2T12_cs);
                statsBuffs.AddSpecialEffect(_SE_2T12_ss);
            }
            if (T12count >= 4)
            {
                statsBuffs.AddSpecialEffect(_SE_4T12);
            }
            #endregion

            foreach (Buff b in removedBuffs) { character.ActiveBuffsAdd(b); }
            foreach (Buff b in addedBuffs) { character.ActiveBuffs.Remove(b); }

            return statsBuffs;
        }
        private void MaintBuffHelper(List<Buff> buffGroup, Character character, List<Buff> removedBuffs)
        {
            foreach (Buff b in buffGroup)
            {
                if (character.ActiveBuffs.Remove(b)) { removedBuffs.Add(b); }
            }
        }

        public override void SetDefaults(Character character) { }
        #endregion

        #region Special Comparison Charts
        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] {
                        "Pet Talents",
                        "Spammed Shots DPS",
                        "Spammed Shots FPS",
                        "Rotation DPS",
                        "Item Budget",
                    };
                }
                return _customChartNames;
            }
        }
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            CharacterCalculationsHunter calculations = GetCharacterCalculations(character) as CharacterCalculationsHunter;

            switch (chartName)
            {
                #region Pet Talents
                case "Pet Talents":
                    _subPointNameColors = _subPointNameColorsDPS;
                    return GetPetTalentChart(character, calculations);
                #endregion
                #region Spammed Shots DPS
                case "Spammed Shots DPS":
                    _subPointNameColors = _subPointNameColorsDPS;
                    return new ComparisonCalculationBase[] {
                        /*
                        comparisonFromShotSpammedDPS(calculations.aimedShot),
                        comparisonFromShotSpammedDPS(calculations.arcaneShot),
                        comparisonFromShotSpammedDPS(calculations.multiShot),
                        comparisonFromShotSpammedDPS(calculations.serpentSting),
                        comparisonFromShotSpammedDPS(calculations.cobraShot),
                        comparisonFromShotSpammedDPS(calculations.steadyShot),
                        comparisonFromShotSpammedDPS(calculations.killShot),
                        comparisonFromShotSpammedDPS(calculations.explosiveShot),
                        comparisonFromShotSpammedDPS(calculations.blackArrow),
                        comparisonFromShotSpammedDPS(calculations.immolationTrap),
                        comparisonFromShotSpammedDPS(calculations.explosiveTrap),
                        comparisonFromShotSpammedDPS(calculations.freezingTrap),
                        comparisonFromShotSpammedDPS(calculations.frostTrap),
                        comparisonFromShotSpammedDPS(calculations.chimeraShot),
                         */
                    };
                #endregion
                #region Rotation DPS
                case "Rotation DPS":
                    _subPointNameColors = _subPointNameColorsDPS;
                    return new ComparisonCalculationBase[] {
                        /*
                        comparisonFromShotRotationDPS(calculations.aimedShot),
                        comparisonFromShotRotationDPS(calculations.arcaneShot),
                        comparisonFromShotRotationDPS(calculations.multiShot),
                        comparisonFromShotRotationDPS(calculations.serpentSting),
                        //comparisonFromShotRotationDPS(calculations.scorpidSting),
                        //comparisonFromShotRotationDPS(calculations.viperSting),
                        comparisonFromShotRotationDPS(calculations.cobraShot),
                        comparisonFromShotRotationDPS(calculations.steadyShot),
                        comparisonFromShotRotationDPS(calculations.killShot),
                        comparisonFromShotRotationDPS(calculations.explosiveShot),
                        comparisonFromShotRotationDPS(calculations.blackArrow),
                        comparisonFromShotRotationDPS(calculations.immolationTrap),
                        comparisonFromShotRotationDPS(calculations.explosiveTrap),
                        comparisonFromShotRotationDPS(calculations.freezingTrap),
                        comparisonFromShotRotationDPS(calculations.frostTrap),
                        //comparisonFromShotRotationDPS(calculations.volley),
                        comparisonFromShotRotationDPS(calculations.chimeraShot),
                        //comparisonFromDoubles("Autoshot", calculations.AutoshotDPS, 0),
                        //comparisonFromDoubles("WildQuiver", calculations.WildQuiverDPS, 0),
                        //comparisonFromDoubles("KillShotSub20", calculations.killShotSub20FinalGain, 0),
                        //comparisonFromDoubles("AspectBeastLoss", calculations.aspectBeastLostDPS, 0),
                        //comparisonFromDoubles("PetAutoAttack", 0, calculations.petWhiteDPS),
                        //comparisonFromDoubles("PetSkills", 0, calculations.petSpecialDPS),
                        //comparisonFromDoubles("KillCommand", 0, calculations.petKillCommandDPS),
                         */
                    };
                #endregion
                #region Item Budget
                case "Item Budget":
                    _subPointNameColors = _subPointNameColorsDPS;
                    return new ComparisonCalculationBase[] { 
                        comparisonFromStat(character, calculations, new Stats() { Agility = 10f }, "10 Agility"),
//                        comparisonFromStat(character, calculations, new Stats() { Mp5 = 4f }, "4 MP5"),
                        comparisonFromStat(character, calculations, new Stats() { CritRating = 10f }, "10 Crit Rating"),
                        comparisonFromStat(character, calculations, new Stats() { HitRating = 10f }, "10 Hit Rating"),
                        comparisonFromStat(character, calculations, new Stats() { AttackPower = 20f }, "20 Attack Power"),
                        comparisonFromStat(character, calculations, new Stats() { RangedAttackPower = 25f }, "25 Ranged Attack Power"),
                        comparisonFromStat(character, calculations, new Stats() { HasteRating = 10f }, "10 Haste Rating"),
                        comparisonFromStat(character, calculations, new Stats() { MasteryRating = 10f }, "10 Mastery Rating"),
                    };
                #endregion
            }

            return new ComparisonCalculationBase[0];
        }
/*        public virtual List<ComparisonCalculationBase> GetPetBuffCalculations(Character character, CalculationOptionsHunter calcOpts, CharacterCalculationsBase currentCalcs, string filter)
        {
            //ClearCache();
            List<ComparisonCalculationBase> buffCalcs = new List<ComparisonCalculationBase>();
            CharacterCalculationsBase calcsOpposite = null;
            //CharacterCalculationsBase calcsEquipped = null;
            //CharacterCalculationsBase calcsUnequipped = null;
            Character charAutoActivated = character.Clone();
            foreach (Buff autoBuff in currentCalcs.AutoActivatedBuffs)
            {
                if (!charAutoActivated.ActiveBuffs.Contains(autoBuff))
                {
                    charAutoActivated.ActiveBuffsAdd(autoBuff);
                    RemoveConflictingBuffs(charAutoActivated.ActiveBuffs, autoBuff);
                }
            }
            charAutoActivated.DisableBuffAutoActivation = true;

            string[] multiFilter = filter.Split('|');

            List<Buff> relevantBuffs = new List<Buff>();
            foreach (Buff buff in RelevantPetBuffs)
            {
                bool isinMultiFilter = false;
                if (multiFilter.Length > 0)
                {
                    foreach (string mFilter in multiFilter)
                    {
                        if (buff.Group.Equals(mFilter, StringComparison.CurrentCultureIgnoreCase))
                        {
                            isinMultiFilter = true;
                            break;
                        }
                    }
                }
                if (filter == null || filter == "All" || filter == "Current"
                    || buff.Group.Equals(filter, StringComparison.CurrentCultureIgnoreCase)
                    || isinMultiFilter)
                {
                    relevantBuffs.Add(buff);
                    relevantBuffs.AddRange(buff.Improvements);
                }
            }

            foreach (Buff buff in relevantBuffs)
            {
                if (!"Current".Equals(filter, StringComparison.CurrentCultureIgnoreCase) || (charAutoActivated.CalculationOptions as CalculationOptionsHunter).petActiveBuffs.Contains(buff))
                {
                    Character charOpposite = charAutoActivated.Clone();
                    //Character charUnequipped = charAutoActivated.Clone();
                    //Character charEquipped = charAutoActivated.Clone();
                    CalculationOptionsHunter calcOptsOpposite = charOpposite.CalculationOptions as CalculationOptionsHunter;
                    //CalculationOptionsHunter calcOptsUnequipped = charUnequipped.CalculationOptions as CalculationOptionsHunter;
                    //CalculationOptionsHunter calcOptsEquipped = charEquipped.CalculationOptions as CalculationOptionsHunter;
                    bool which = (charAutoActivated.CalculationOptions as CalculationOptionsHunter).petActiveBuffs.Contains(buff);
                    charOpposite.DisableBuffAutoActivation = true;
                    //charUnequipped.DisableBuffAutoActivation = true;
                    //charEquipped.DisableBuffAutoActivation = true;
                    if (which) { calcOptsOpposite.petActiveBuffs.Remove(buff); } else { calcOptsOpposite.petActiveBuffs.Add(buff); }
                    //if (calcOptsUnequipped.petActiveBuffs.Contains(buff)) { calcOptsUnequipped.petActiveBuffs.Remove(buff); }
                    //if (!calcOptsEquipped.petActiveBuffs.Contains(buff)) { calcOptsEquipped.petActiveBuffs.Add(buff); }

                    RemoveConflictingBuffs(calcOptsOpposite.petActiveBuffs, buff);
                    //RemoveConflictingBuffs(calcOptsEquipped.petActiveBuffs, buff);
                    //RemoveConflictingBuffs(calcOptsUnequipped.petActiveBuffs, buff);

                    calcsOpposite = GetCharacterCalculations(charOpposite, null, false, false, false);
                    //calcsUnequipped = GetCharacterCalculations(charUnequipped, null, false, false, false);
                    //calcsEquipped = GetCharacterCalculations(charEquipped, null, false, false, false);

                    ComparisonCalculationBase buffCalc = CreateNewComparisonCalculation();
                    buffCalc.Name = buff.Name;
                    buffCalc.Item = new Item() { Name = buff.Name, Stats = buff.Stats, Quality = ItemQuality.Temp };
                    buffCalc.Equipped = which;//(charAutoActivated.CalculationOptions as CalculationOptionsHunter).petActiveBuffs.Contains(buff);
                    buffCalc.OverallPoints = (which ? currentCalcs.OverallPoints - calcsOpposite.OverallPoints
                                                    : calcsOpposite.OverallPoints - currentCalcs.OverallPoints);
                    //buffCalc.OverallPoints = currentCalcs.OverallPoints - (buffCalc.Equipped ? calcsEquipped.OverallPoints : calcsUnequipped.OverallPoints);
                    float[] subPoints = new float[calcsOpposite.SubPoints.Length];
                    //float[] subPoints = new float[calcsEquipped.SubPoints.Length];
                    for (int i = 0; i < calcsOpposite.SubPoints.Length; i++) {
                        subPoints[i] = (which ? currentCalcs.SubPoints[i] - calcsOpposite.SubPoints[i]
                                              : calcsOpposite.SubPoints[i] - currentCalcs.SubPoints[i]);
                    }
                    //for (int i = 0; i < calcsEquipped.SubPoints.Length; i++) { subPoints[i] = calcsEquipped.SubPoints[i] - calcsUnequipped.SubPoints[i]; }
                    buffCalc.SubPoints = subPoints;
                    buffCalcs.Add(buffCalc);
                    // Revert, cuz it's evil
                    if (!which) { calcOptsOpposite.petActiveBuffs.Remove(buff); } else { calcOptsOpposite.petActiveBuffs.Add(buff); }
                }
            }
            return buffCalcs;
        }*/

        private ComparisonCalculationHunter[] GetPetTalentChart(Character character, CharacterCalculationsHunter calcs)
        {
            List<ComparisonCalculationHunter> talentCalculations = new List<ComparisonCalculationHunter>();
            Character baseChar = character.Clone(); CalculationOptionsHunter baseCalcOpts = baseChar.CalculationOptions as CalculationOptionsHunter;
            Character newChar = character.Clone(); CalculationOptionsHunter newCalcOpts = newChar.CalculationOptions as CalculationOptionsHunter;
            CharacterCalculationsHunter currentCalc;
            CharacterCalculationsHunter newCalc;
            ComparisonCalculationHunter compare;
            currentCalc = (CharacterCalculationsHunter)Calculations.GetCharacterCalculations(baseChar, null, false, true, false);

            foreach (PropertyInfo pi in baseCalcOpts.PetTalents.GetType().GetProperties())
            {
                PetTalentDataAttribute[] petTalentDatas = pi.GetCustomAttributes(typeof(PetTalentDataAttribute), true) as PetTalentDataAttribute[];
                int orig;
                if (petTalentDatas.Length > 0) {
                    PetTalentDataAttribute petTalentData = petTalentDatas[0];
                    orig = baseCalcOpts.PetTalents.Data[petTalentData.Index];
                    if (petTalentData.MaxPoints == (int)pi.GetValue(baseCalcOpts.PetTalents, null)) {
                        newCalcOpts.PetTalents.Data[petTalentData.Index]--;
                        newCalc = (CharacterCalculationsHunter)GetCharacterCalculations(newChar, null, false, true, false);
                        compare = (ComparisonCalculationHunter)GetCharacterComparisonCalculations(newCalc, currentCalc, petTalentData.Name, petTalentData.MaxPoints == orig, orig != 0 && orig != petTalentData.MaxPoints);
                    } else {
                        newCalcOpts.PetTalents.Data[petTalentData.Index]++;
                        newCalc = (CharacterCalculationsHunter)GetCharacterCalculations(newChar, null, false, true, false);
                        compare = (ComparisonCalculationHunter)GetCharacterComparisonCalculations(currentCalc, newCalc, petTalentData.Name, petTalentData.MaxPoints == orig, orig != 0 && orig != petTalentData.MaxPoints);
                    }
                    string text = string.Format("Current Rank {0}/{1}\r\n\r\n", orig, petTalentData.MaxPoints);
                    if (orig == 0) {
                        // We originally didn't have it, so first rank is next rank
                        text += "Next Rank:\r\n";
                        text += petTalentData.Description[0];
                    } else if (orig >= petTalentData.MaxPoints) {
                        // We originally were at max, so there isn't a next rank, just show the capped one
                        text += petTalentData.Description[petTalentData.MaxPoints - 1];
                    } else {
                        // We aren't at 0 or MaxPoints originally, so it's just a point in between
                        text += petTalentData.Description[orig - 1];
                        text += "\r\n\r\nNext Rank:\r\n";
                        text += petTalentData.Description[orig];
                    }
                    compare.Description = text;
                    compare.Item = null;
                    talentCalculations.Add(compare);
                    newCalcOpts.PetTalents.Data[petTalentData.Index] = orig;
                }
            }
            return talentCalculations.ToArray();
        }
        /*private ComparisonCalculationHunter[] GetPetTalentSpecsChart(Character character, CharacterCalculationsHunter calcs)
        {
            List<ComparisonCalculationBase> talentCalculations = new List<ComparisonCalculationBase>();
            Character newChar = character.Clone();

            /*PetTalentsBase nothing = character.CurrentTalents.Clone();
            for (int i = 0; i < nothing.Data.Length; i++) nothing.Data[i] = 0;
            for (int i = 0; i < nothing.GlyphData.Length; i++) nothing.GlyphData[i] = false;
            newChar.CurrentTalents = nothing;

            CharacterCalculationsBase baseCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, true);
            CharacterCalculationsBase newCalc;
            ComparisonCalculationBase compare;

            bool same, found = false;
            foreach (SavedTalentSpec sts in SavedTalentSpec.SpecsFor(character.Class))
            {
                same = false;
                if (sts.Equals(character.CurrentTalents)) same = true;
                newChar.CurrentTalents = sts.TalentSpec();
                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, true);
                compare = Calculations.GetCharacterComparisonCalculations(baseCalc, newCalc, sts.Name, same);
                compare.Item = null;
                compare.Name = sts.ToString();
                compare.Description = sts.Spec;
                talentCalculations.Add(compare);
                found = found || same;
            }
            if (!found)
            {
                newCalc = Calculations.GetCharacterCalculations(character, null, false, true, true);
                compare = Calculations.GetCharacterComparisonCalculations(baseCalc, newCalc, "Custom", true);
                talentCalculations.Add(compare);
            }
            //CGL_Legend.LegendItems = Calculations.SubPointNameColors;
            //ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
            //ComparisonGraph.Mode = ComparisonGraph.DisplayMode.Subpoints;
            //ComparisonGraph.DisplayCalcs(talentCalculations.ToArray());
            return talentCalculations.ToArray();

            /*List<ComparisonCalculationHunter> talentCalculations = new List<ComparisonCalculationHunter>();
            Character baseChar = character.Clone(); CalculationOptionsHunter baseCalcOpts = baseChar.CalculationOptions as CalculationOptionsHunter;
            Character newChar = character.Clone(); CalculationOptionsHunter newCalcOpts = newChar.CalculationOptions as CalculationOptionsHunter;
            CharacterCalculationsHunter currentCalc;
            CharacterCalculationsHunter newCalc;
            ComparisonCalculationHunter compare;
            currentCalc = (CharacterCalculationsHunter)Calculations.GetCharacterCalculations(baseChar, null, false, true, false);

            foreach (PropertyInfo pi in baseCalcOpts.PetTalents.GetType().GetProperties())
            {
                PetTalentDataAttribute[] petTalentDatas = pi.GetCustomAttributes(typeof(PetTalentDataAttribute), true) as PetTalentDataAttribute[];
                int orig;
                if (petTalentDatas.Length > 0) {
                    PetTalentDataAttribute petTalentData = petTalentDatas[0];
                    orig = baseCalcOpts.PetTalents.Data[petTalentData.Index];
                    if (petTalentData.MaxPoints == (int)pi.GetValue(baseCalcOpts.PetTalents, null)) {
                        newCalcOpts.PetTalents.Data[petTalentData.Index]--;
                        newCalc = (CharacterCalculationsHunter)GetCharacterCalculations(newChar, null, false, true, false);
                        compare = (ComparisonCalculationHunter)GetCharacterComparisonCalculations(newCalc, currentCalc, petTalentData.Name, petTalentData.MaxPoints == orig);
                    } else {
                        newCalcOpts.PetTalents.Data[petTalentData.Index]++;
                        newCalc = (CharacterCalculationsHunter)GetCharacterCalculations(newChar, null, false, true, false);
                        compare = (ComparisonCalculationHunter)GetCharacterComparisonCalculations(currentCalc, newCalc, petTalentData.Name, petTalentData.MaxPoints == orig);
                    }
                    string text = string.Format("Current Rank {0}/{1}\r\n\r\n", orig, petTalentData.MaxPoints);
                    if (orig == 0) {
                        // We originally didn't have it, so first rank is next rank
                        text += "Next Rank:\r\n";
                        text += petTalentData.Description[0];
                    } else if (orig >= petTalentData.MaxPoints) {
                        // We originally were at max, so there isn't a next rank, just show the capped one
                        text += petTalentData.Description[petTalentData.MaxPoints - 1];
                    } else {
                        // We aren't at 0 or MaxPoints originally, so it's just a point in between
                        text += petTalentData.Description[orig - 1];
                        text += "\r\n\r\nNext Rank:\r\n";
                        text += petTalentData.Description[orig];
                    }
                    compare.Description = text;
                    compare.Item = null;
                    talentCalculations.Add(compare);
                    newCalcOpts.PetTalents.Data[petTalentData.Index] = orig;
                }
            }
            return talentCalculations.ToArray();
        }*/
#if FALSE
        private ComparisonCalculationHunter comparisonFromShotSpammedDPS(ShotData shot)
        {
            ComparisonCalculationHunter comp =  new ComparisonCalculationHunter();

            float shotWait = shot.Duration > shot.Cd ? shot.Duration : shot.Cd;
            float dps = shotWait > 0 ? (float)(shot.Damage / shotWait) : 0;

            comp.Name = Enum.GetName(typeof(Shots), shot.Type);
            comp.HunterDPSPoints = dps;
            comp.OverallPoints = dps;
            return comp;
        }
        private ComparisonCalculationHunter comparisonFromShotSpammedMPS(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();

            float shotWait = shot.Duration > shot.Cd ? shot.Duration : shot.Cd;
            float mps = shotWait > 0 ? (float)(shot.FocusCost / shotWait) : 0;

            comp.Name = Enum.GetName(typeof(Shots), shot.Type);
            comp.SubPoints = new float[] { mps };
            comp.OverallPoints = mps;
            return comp;
        }
        private ComparisonCalculationHunter comparisonFromShotRotationDPS(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();
            comp.Name = Enum.GetName(typeof(Shots), shot.Type);
            comp.SubPoints = new float[] { (float)shot.DPS };
            comp.OverallPoints = (float)shot.DPS;
            return comp;
        }
        private ComparisonCalculationHunter comparisonFromShotRotationFocusPS(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();
            comp.Name = Enum.GetName(typeof(Shots), shot.Type);
            comp.SubPoints = new float[] { (float)shot.FocusPS };
            comp.OverallPoints = (float)shot.FocusPS;
            return comp;
        }
        private ComparisonCalculationHunter comparisonFromShotDPF(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();

            float dpm = shot.FocusCost > 0 ? (float)(shot.Damage / shot.FocusCost) : 0;

            comp.Name = Enum.GetName(typeof(Shots), shot.Type);
            comp.SubPoints = new float[] { dpm };
            comp.OverallPoints = dpm;
            return comp;
        }
#endif

        private ComparisonCalculationHunter comparisonFromStat(Character character, CharacterCalculationsHunter calcBase, Stats stats, string label)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();

            CharacterCalculationsHunter calcStat = GetCharacterCalculations(character, new Item() { Stats = stats }) as CharacterCalculationsHunter;

            comp.Name = label;
            comp.HunterDPSPoints = calcStat.HunterDpsPoints - calcBase.HunterDpsPoints;
            comp.PetDPSPoints = calcStat.PetDpsPoints - calcBase.PetDpsPoints;
            comp.OverallPoints = calcStat.OverallPoints - calcBase.OverallPoints;

            return comp;
        }
        private ComparisonCalculationHunter comparisonFromDouble(string label, float value)
        {
            return new ComparisonCalculationHunter()
            {
                Name = label,
                SubPoints = new float[] { (float)value },
                OverallPoints = (float)value,
            };
        }
        private ComparisonCalculationHunter comparisonFromDoubles(string label, float value1, float value2)
        {
            return new ComparisonCalculationHunter()
            {
                Name = label,
                SubPoints = new float[] { (float)value1, (float)value2 },
                OverallPoints = (float)(value1 + value2),
            };
        }
        #endregion

        #region CalculationsBase Overrides
        /*        private void GenPrioRotation(CharacterCalculationsHunter calculatedStats, CalculationOptionsHunter calcOpts, HunterTalents talents) {
            calculatedStats.priorityRotation = new ShotPriority(calcOpts);
            calculatedStats.priorityRotation.priorities[0] = getShotByIndex(calcOpts.PriorityIndex1, calculatedStats);
            calculatedStats.priorityRotation.priorities[1] = getShotByIndex(calcOpts.PriorityIndex2, calculatedStats);
            calculatedStats.priorityRotation.priorities[2] = getShotByIndex(calcOpts.PriorityIndex3, calculatedStats);
            calculatedStats.priorityRotation.priorities[3] = getShotByIndex(calcOpts.PriorityIndex4, calculatedStats);
            calculatedStats.priorityRotation.priorities[4] = getShotByIndex(calcOpts.PriorityIndex5, calculatedStats);
            calculatedStats.priorityRotation.priorities[5] = getShotByIndex(calcOpts.PriorityIndex6, calculatedStats);
            calculatedStats.priorityRotation.priorities[6] = getShotByIndex(calcOpts.PriorityIndex7, calculatedStats);
            calculatedStats.priorityRotation.priorities[7] = getShotByIndex(calcOpts.PriorityIndex8, calculatedStats);
            calculatedStats.priorityRotation.priorities[8] = getShotByIndex(calcOpts.PriorityIndex9, calculatedStats);
            calculatedStats.priorityRotation.priorities[9] = getShotByIndex(calcOpts.PriorityIndex10, calculatedStats);
            calculatedStats.priorityRotation.validateShots(talents);
        }
        private void GenAbilityCds(Character character, CharacterCalculationsHunter calculatedStats, CalculationOptionsHunter calcOpts, BossOptions bossOpts, HunterTalents talents)
        {
            calculatedStats.serpentSting.Cd = 1.5f;
            calculatedStats.serpentSting.Duration = talents.GlyphOfSerpentSting ? 21 : 15;

            calculatedStats.aimedShot.Cd = talents.GlyphOfAimedShot ? 8 : 10;

            calculatedStats.explosiveShot.Cd = 6;

            calculatedStats.chimeraShot.Cd = talents.GlyphOfChimeraShot ? 9 : 10;

            calculatedStats.arcaneShot.Cd = 6;

            calculatedStats.multiShot.Cd = /*talents.GlyphOfMultiShot ? 9 : 10;

            calculatedStats.blackArrow.Cd = 30 - (talents.Resourcefulness * 2);
            calculatedStats.blackArrow.Duration = 15;

            calculatedStats.killShot.Cd = talents.GlyphOfKillShot ? 9 : 15;


            calculatedStats.immolationTrap.Cd = 30 - talents.Resourcefulness * 2;
            calculatedStats.immolationTrap.Duration = talents.GlyphOfImmolationTrap ? 9 : 15;

            calculatedStats.explosiveTrap.Cd = 30 - talents.Resourcefulness * 2;
            calculatedStats.explosiveTrap.Duration = 20;

            calculatedStats.freezingTrap.Cd = 30 - talents.Resourcefulness * 2;
            calculatedStats.freezingTrap.Duration = 20;

            calculatedStats.frostTrap.Cd = 30 - talents.Resourcefulness * 2;
            calculatedStats.frostTrap.Duration = 30;

//            if (bossOpts.MultiTargs && bossOpts.MultiTargsTime > 0) {
//                // Good to go, now change the cooldown based on the multitargs uptime
//                calculatedStats.volley.Duration = 6f;
//                calculatedStats.volley.Cd = (1f / (bossOpts.MultiTargsTime / calculatedStats.volley.Duration)) * bossOpts.BerserkTimer;
//#endif
//            } else {
//                // invalidate it
//                calculatedStats.volley.Cd = -1f;
//                //calculatedStats.volley.CastTime = -1f;
//                calculatedStats.volley.Duration = -1f;
//            }

            if (calculatedStats.priorityRotation.containsShot(Shots.Readiness)) {
                calculatedStats.rapidFire.Cd = 157.5f - (30f * talents.RapidKilling);
            } else {
                calculatedStats.rapidFire.Cd = (5 - talents.RapidKilling) * 60f;
            }
            calculatedStats.rapidFire.Duration = 15;

            // We will set the correct value for this later, after we've calculated haste
            calculatedStats.steadyShot.Cd = 2;
            // TODO Zhok: Same 4 cobra?

            calculatedStats.readiness.Cd = 180;

            calculatedStats.bestialWrath.Cd = (talents.GlyphOfBestialWrath ? 100f : 120f) * (1f - talents.Longevity * 0.10f);
            calculatedStats.bestialWrath.Duration = calcOpts.PetFamily == PETFAMILY.None ? 0 : 10;

            // We can calculate the rough frequencies now
            calculatedStats.priorityRotation.initializeTimings();
            if (!calcOpts.UseRotationTest) {
                calculatedStats.priorityRotation.calculateFrequencies();
                calculatedStats.priorityRotation.calculateLALProcs(character);
                calculatedStats.priorityRotation.calculateFrequencies();
                calculatedStats.priorityRotation.calculateFrequencySums();
            }
        }
        private static void GenRotation(Character character, Stats stats, CharacterCalculationsHunter calculatedStats,
            CalculationOptionsHunter calcOpts, BossOptions bossOpts, HunterTalents talents,
            out float rangedWeaponSpeed, out float rangedWeaponDamage, out float autoShotSpeed,
            out float autoShotsPerSecond, out float specialShotsPerSecond, out float totalShotsPerSecond, out float shotsPerSecondWithoutHawk,
            out RotationTest rotationTest)
        {
            #region Ranged Weapon Stats
            rangedWeaponDamage = 0;
            rangedWeaponSpeed = 0;

            if (character.Ranged != null) {
                rangedWeaponDamage = (character.Ranged.Item.MinDamage + character.Ranged.Item.MaxDamage) / 2f;
                rangedWeaponSpeed = character.Ranged.Item.Speed;
            }
            /* Projectiles/Ammo have been removed in Cata
            if (character.Projectile != null) {
                rangedAmmoDPS = (float)(character.Projectile.Item.MaxDamage + character.Projectile.Item.MinDamage) / 2f;
            }
            #endregion
            #region Static Haste Calcs
            // default quiver speed
            calculatedStats.hasteFromBase = 0.15f;
            // haste from haste rating
            calculatedStats.hasteFromRating = StatConversion.GetHasteFromRating(stats.HasteRating, character.Class);
            // haste buffs
            calculatedStats.hasteFromRangedBuffs = stats.RangedHaste;
            // total hastes
            calculatedStats.hasteStaticTotal = stats.PhysicalHaste;
            // Needed by the rotation test
            calculatedStats.autoShotStaticSpeed = rangedWeaponSpeed / (1f + calculatedStats.hasteStaticTotal);
            #endregion
            #region Rotation Test
            // Using the rotation test will get us better frequencies
            //RotationTest
                rotationTest = new RotationTest(character, calculatedStats, calcOpts, bossOpts);

            if (calcOpts.UseRotationTest) {
                // The following properties of CalculatedStats must be ready by this call:
                //  * priorityRotation (shot order, durations, cooldowns)
                //  * quickShotsEffect
                //  * hasteStaticTotal
                //  * autoShotStaticSpeed

                rotationTest.RunTest();
            }
            #endregion
            #region Dynamic Haste Calcs
            /* http://elitistjerks.com/f74/t65904-hunter_dps_analyzer/p25/#post1887407
             * 1) Base focus regen is 4.00.
             * 2) Pathing adds an additional 1% base focus regen per point (4.12 with 3/3 and no gear).
             * 3) WF/IT/HP and ISS don't modify base regen directly.
             * 4) Each 1% gear haste adds 2% base focus regen.
             * 5) Rapid Fire adds 40% base regen (4.00->5.60).
             * 6) Hero adds 30% base regen (4.00->5.20).
             * 7) Glyph of Rapid Fire adds 10% base regen (4.00->6.00).
             * 8) Focused Fire adds 15% base regen (4.00->4.60).
             
            // Now we have the haste, we can calculate steady shot cast time,
            // then rebuild other various stats.
            // (future enhancement: we only really need to rebuild steady shot)
            calculatedStats.steadyShot.Cd = 2f / (1f + calculatedStats.hasteStaticTotal);
            if (calcOpts.UseRotationTest) {
                calculatedStats.priorityRotation.initializeTimings();
                calculatedStats.priorityRotation.recalculateRatios();
                calculatedStats.priorityRotation.calculateFrequencySums();
            } else {
                calculatedStats.priorityRotation.initializeTimings();
                calculatedStats.priorityRotation.calculateFrequencies();
                calculatedStats.priorityRotation.calculateFrequencySums();
            }
            //float
                autoShotSpeed = rangedWeaponSpeed / (1f + calculatedStats.hasteStaticTotal);
            #endregion
            #region Shots Per Second
            float baseAutoShotsPerSecond = autoShotSpeed > 0 ? 1f / autoShotSpeed : 0;
            //float
                autoShotsPerSecond = baseAutoShotsPerSecond;
            //float
                specialShotsPerSecond = calculatedStats.priorityRotation.specialShotsPerSecond;
            //float
                totalShotsPerSecond = autoShotsPerSecond + specialShotsPerSecond;

            float crittingSpecialsPerSecond = calculatedStats.priorityRotation.critSpecialShotsPerSecond;
            float crittingShotsPerSecond = autoShotsPerSecond + crittingSpecialsPerSecond;

            //float
                shotsPerSecondWithoutHawk = specialShotsPerSecond + baseAutoShotsPerSecond;

            calculatedStats.BaseAttackSpeed = (float)autoShotSpeed;
            calculatedStats.shotsPerSecondCritting = crittingShotsPerSecond;
            #endregion
        }

        public float ConstrainCrit(float lvlDifMOD, float chance) { return Math.Min(1f + lvlDifMOD, Math.Max(0f, chance)); }
*/

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsHunter calc = new CharacterCalculationsHunter();
            if (character == null) { return calc; }
            CalculationOptionsHunter calcOpts = character.CalculationOptions as CalculationOptionsHunter;
            if (calcOpts == null) { return calc; }
            //
            calc.character = character;
            calc.CalcOpts = calcOpts;
            BossOptions bossOpts = character.BossOptions;
            calc.BossOpts = bossOpts;

            HunterTalents talents = character.HunterTalents;
            Specialization hunterspec = GetSpecialization(talents);

            calc.Hunter = new HunterBase(character, this.GetCharacterStats(character, additionalItem, true), talents, hunterspec, bossOpts.Level);
            CombatFactors combatFactors = new CombatFactors(character, calc.Hunter.Stats, calcOpts, bossOpts);
//            WhiteAttacks whiteAttacks = new WhiteAttacks(character, calc.Hunter.Stats, combatFactors, calcOpts, bossOpts);

            Rotation Rot = new Rotation();
            Rot.CombatFactors = combatFactors;
            Rot.Initialize(calc); // Sets up the shots in the rotation and provides connection with the Calc object.



            if (needsDisplayCalculations)
            {
                calc.HunterUnBuffed = new HunterBase(character, GetCharacterStats(character, additionalItem, false), talents, hunterspec, bossOpts.Level);
            }

            return calc;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return this.GetCharacterStats(character, additionalItem, false);
        }

        private StatsHunter GetCharacterStats(Character character, Item additionalItem, bool bGetMaxStats)
        {
            StatsHunter statsTotal = new StatsHunter();
            try
            {
                #region NullChecks
                if (null == character)
                {
#if DEBUG
                    throw new NullReferenceException("Character is Null");
#else
                    return statsTotal;
#endif
                }
                CalculationOptionsHunter calcOpts = character.CalculationOptions as CalculationOptionsHunter;
                if (null == calcOpts) { calcOpts = new CalculationOptionsHunter(); }
                HunterTalents talents = character.HunterTalents;
                if (null == talents) { return statsTotal; }
                Specialization tree = GetSpecialization(talents);
                CharacterCalculationsHunter calculatedStats = new CharacterCalculationsHunter();
                if (null == calculatedStats)
                {
#if DEBUG
                    throw new NullReferenceException("Character Calculations is Null");
#else
                    return statsTotal;
#endif
                }
                BossOptions bossOpts = new BossOptions();
                bossOpts = character.BossOptions;
                if (null == bossOpts)
                {
#if DEBUG
                    throw new NullReferenceException("Boss Options is Null");
#else
                    return statsTotal;
#endif
                }
                #endregion

                statsTotal.Accumulate(BaseStats.GetBaseStats(character.Level, CharacterClass.Hunter, character.Race));
                statsTotal.Accumulate(GetItemStats(character, additionalItem));
                AccumulateBuffsStats(statsTotal, character.ActiveBuffs);
                // DO NOT MOVE GetRelevantStats any lower in this progression of functions.
                // It will erase stats specific to hunters.
                statsTotal = GetRelevantStats(statsTotal) as StatsHunter;
                statsTotal.Accumulate(GetTalentStats(character.HunterTalents));

                statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
                statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
                calculatedStats.critFromAgi = statsTotal.Agility;
                // Agi bonus to Survival doesn't affect crit?
                if (tree == Specialization.Survival) calculatedStats.critFromAgi /= 1.1f;
                statsTotal.AttackPower += (statsTotal.Agility * 2f); 
                statsTotal.AttackPower = statsTotal.RangedAttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
                statsTotal.Health += (float)Math.Floor((statsTotal.Stamina - 20f) * 14f + 20f);
                statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
                statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier)) + statsTotal.BonusArmor;

                statsTotal.NatureResistance += statsTotal.NatureResistanceBuff;
                statsTotal.FireResistance += statsTotal.FireResistanceBuff;
                statsTotal.FrostResistance += statsTotal.FrostResistanceBuff;
                statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff;
                statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff;

                int targetLevel = bossOpts.Level;
                float hasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Hunter);
                statsTotal.RangedHaste = (1f + hasteBonus) * (1f + statsTotal.BonusHasteMultiplier) * (1f + statsTotal.PhysicalHaste) - 1f;
                float hitBonus = StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating) + statsTotal.PhysicalHit;
                float chanceMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[targetLevel - character.Level] - hitBonus);
                float chanceAvoided = chanceMiss;

                float rawChanceCrit = StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating)
                                    + StatConversion.GetCritFromAgility(calculatedStats.critFromAgi, CharacterClass.Hunter)
                                    + statsTotal.PhysicalCrit
                                    + StatConversion.NPC_LEVEL_CRIT_MOD[targetLevel - character.Level];
                calculatedStats.critRateOverall = rawChanceCrit * (1f - chanceAvoided);
                float chanceHit = 1f - chanceAvoided;

                if (bGetMaxStats)
                {
                    #region Special Effects
                    StatsHunter statsProcs = new StatsHunter();

                    Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>();
                    Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>();

                    CalculateTriggers(character, calculatedStats, statsTotal, calcOpts, bossOpts, triggerIntervals, triggerChances);

                    /*if (calcOpts.PetFamily == PETFAMILY.Wolf
                        && calculatedStats.pet.priorityRotation.getSkillFrequency(PetAttacks.FuriousHowl) > 0)
                    {
                        statsTotal.AddSpecialEffect(FuriousHowl);
                    }
                    */
                    foreach (SpecialEffect effect in statsTotal.SpecialEffects())
                    {
                        statsProcs.Accumulate(getSpecialEffects(effect, triggerIntervals, triggerChances, character));
                    }
                    #region Handle Results of Special Effects
                    // Base Stats
                    statsProcs.Stamina = (float)Math.Floor(statsProcs.Stamina * (1f + statsTotal.BonusStaminaMultiplier) * (1f + statsProcs.BonusStaminaMultiplier));
                    statsProcs.Agility = statsProcs.Agility * (1f + statsTotal.BonusAgilityMultiplier) * (1f + statsProcs.BonusAgilityMultiplier);
                    statsProcs.Agility += statsProcs.HighestStat * (1f + statsTotal.BonusAgilityMultiplier) * (1f + statsProcs.BonusAgilityMultiplier);
                    statsProcs.Agility += statsProcs.Paragon * (1f + statsTotal.BonusAgilityMultiplier) * (1f + statsProcs.BonusAgilityMultiplier);
                    statsProcs.HighestStat = statsProcs.Paragon = 0f; // we've added them into agi so kill it
                    statsProcs.Health += (float)Math.Floor(statsProcs.Stamina * 10f);

                    float HighestSecondaryStatValue = statsProcs.HighestSecondaryStat; // how much HighestSecondaryStat to add
                    statsProcs.HighestSecondaryStat = 0f; // remove HighestSecondaryStat stat, since it's not needed
                    if (statsTotal.CritRating > statsTotal.HasteRating && statsTotal.CritRating > statsTotal.MasteryRating)
                    {
                        statsTotal.CritRating += HighestSecondaryStatValue;
                    }
                    else if (statsTotal.HasteRating > statsTotal.CritRating && statsTotal.HasteRating > statsTotal.MasteryRating)
                    {
                        statsTotal.HasteRating += HighestSecondaryStatValue;
                    }
                    else /*if (statsTotal.MasteryRating > statsTotal.CritRating && statsTotal.MasteryRating > statsTotal.HasteRating)*/
                    {
                        statsTotal.MasteryRating += HighestSecondaryStatValue;
                    }


                    // Armor
                    statsProcs.Armor = statsProcs.Armor * (1f + statsTotal.BaseArmorMultiplier + statsProcs.BaseArmorMultiplier);
                    //statsProcs.BonusArmor += statsProcs.Agility * 2f;
                    statsProcs.BonusArmor = statsProcs.BonusArmor * (1f + statsTotal.BonusArmorMultiplier + statsProcs.BonusArmorMultiplier);
                    statsProcs.Armor += statsProcs.BonusArmor;
                    statsProcs.BonusArmor = 0; //it's been added to Armor so kill it

                    // Attack Power
                    statsProcs.BonusAttackPowerMultiplier *= (1f + statsProcs.BonusRangedAttackPowerMultiplier);
                    statsProcs.BonusRangedAttackPowerMultiplier = 0; // it's been added to Attack Power so kill it
                    float totalBAPMProcs = (1f + statsTotal.BonusAttackPowerMultiplier) * (1f + statsProcs.BonusAttackPowerMultiplier) - 1f;
                    float apFromAGIProcs = (1f + totalBAPMProcs) * (statsProcs.Agility) * 2f;
                    //float apFromSTRProcs    = (1f + totalBAPMProcs) * (statsProcs.Strength);
                    float apBonusOtherProcs = (1f + totalBAPMProcs) * (statsProcs.AttackPower + statsProcs.RangedAttackPower);
                    statsProcs.AttackPower = Math.Max(0f, apFromAGIProcs + /*apFromSTRProcs +*/ apBonusOtherProcs);
                    statsProcs.RangedAttackPower = statsProcs.AttackPower;
                    statsTotal.AttackPower *= (1f + statsProcs.BonusAttackPowerMultiplier); // Make sure the originals get your AP% procs

                    // Crit
                    statsProcs.PhysicalCrit += StatConversion.GetCritFromAgility(statsProcs.Agility, character.Class);
                    statsProcs.PhysicalCrit += StatConversion.GetCritFromRating(statsProcs.CritRating + statsProcs.RangedCritRating, character.Class);

                    // Haste
                    statsProcs.PhysicalHaste = (1f + statsProcs.PhysicalHaste)
                                             * (1f + statsProcs.RangedHaste)
                                             * (1f + StatConversion.GetPhysicalHasteFromRating(statsProcs.HasteRating, character.Class))
                                             - 1f;
                    #endregion
                    // Add it back into the fold
                    statsTotal.Accumulate(statsProcs);
                    #endregion
                }
                return statsTotal;
            }
            catch (Exception ex)
            {
                new Base.ErrorBox()
                {
                    Title = "Error in getting Character Stats",
                    Function = "GetCharacterStats()",
                    TheException = ex,
                }.Show();
            }
            return new StatsHunter();
        }

        private StatsHunter GetTalentStats(HunterTalents talents)
        {
            StatsHunter s = new StatsHunter();
            #region Specializations : Abilities/Bonuses from picking a tree.
            switch ((Specialization)talents.HighestTree)
            {
                // BM
                case Specialization.BeastMastery:
                    {
                        // Initimitation 
                        // TODO: Implemented in Shots/Abilities
                        // Animal Handler
                        s.BonusAttackPowerMultiplier += 0.25f;
                        // Mastery: Master of Beasts
                        // TODO: s.BonusPetDamageMultiplier += .13 + 0.0167 * Mastery
                        break;
                    }
                // MM
                case Specialization.Marksmanship:
                    {
                        // Aimed Shot
                        // TODO: Implemented in Shots/Abilities
                        // Artisan Quiver
                        // TODO: Auto-attack damage + 15%
                        // Mastery: Wild Quiver
                        // TODO: Additional Auto-Shot 16.8% + 2.1% per Mastery
                        break;
                    }
                // SV
                case Specialization.Survival:
                    {
                        // Explosive Shot
                        // TODO: Implemented in Shots/Abilities
                        // Intothe Wilderness
                        s.BonusAgilityMultiplier += .1f;
                        // Mastery: Essence of the Viper
                        // Bonus magic damage 8% + 1% per mastery
                        break;
                    }

            }
            #endregion

            #region Beast Master
            // Talent: Ferocious Inspiration
            s.BonusDamageMultiplier += talents.FerociousInspiration * 0.03f;
            #endregion

            #region Marksman
            // Talent: Trueshot Aura (Always on and part of the paperdoll numbers)
            s.BonusAttackPowerMultiplier += talents.TrueshotAura * 0.1f;
            #endregion
            #region Survival
            s.BonusStaminaMultiplier += talents.HunterVsWild * 0.05f;
            s.BonusHasteMultiplier += talents.Pathing * 0.01f;
            s.BonusAgilityMultiplier += talents.HuntingParty * 0.02f;
            // This conflics w/ IcyTalons and other similar buffs.
            s.PhysicalHaste += talents.HuntingParty * .1f;
            #endregion
            
            return s;
        }

        /// <summary>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
        /// Local version of GetItemStats()
        /// Includes the Armor style bonus.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="additionalItem"></param>
        /// <returns></returns>
        public override Stats GetItemStats(Character character, Item additionalItem)
        {
            Stats stats = base.GetItemStats(character, additionalItem);
            // Add in armor specialty
            if (Character.ValidateArmorSpecialization(character, ItemType.Mail))
            {
                stats.BonusAgilityMultiplier += .05f;
            }
            return stats;
        }

        //private static readonly SpecialEffect FuriousHowl = new SpecialEffect(Trigger.Use, new StatsHunter() { AttackPower = 320f, PetAttackPower = 320f, }, 20f, 40f);

        private static void CalculateTriggers(Character character, CharacterCalculationsHunter calculatedStats, StatsHunter statsTotal, CalculationOptionsHunter calcOpts, BossOptions bossOpts, 
            Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances)
        {
            const float GCD = 1f;
            const float GCDpSec = 1f / GCD;
            int levelDif = bossOpts.Level - character.Level;
            float critMOD = StatConversion.NPC_LEVEL_CRIT_MOD[levelDif];
            HunterTalents talents = character.HunterTalents;
            float rangedWeaponSpeed = 2.4f; 
            float rangedWeaponDamage = 1;
            float autoShotSpeed = rangedWeaponSpeed; // Should be the same as rangedWeaponSpeed hasted.
            float autoShotsPerSecond = 1 / autoShotSpeed;
            float specialShotsPerSecond = GCDpSec; // I know it doesn't start w/ a special every GCD, but something to start with more than 0.
            float totalShotsPerSecond = GCDpSec + autoShotsPerSecond; 
            float shotsPerSecondWithoutHawk = 0;
//            RotationTest rotationTest;
            //GenRotation(character, statsTotal, calculatedStats, calcOpts, bossOpts, talents,
            //    out rangedWeaponSpeed, out rangedWeaponDamage, out autoShotSpeed,
            //    out autoShotsPerSecond, out specialShotsPerSecond, out totalShotsPerSecond, out shotsPerSecondWithoutHawk,
            //    out rotationTest);
            float ChanceToMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[levelDif] - statsTotal.PhysicalHit);
            float ChanceToSpellMiss = Math.Max(0f, StatConversion.GetSpellMiss(levelDif, false) - statsTotal.SpellHit);

            // TODO: Ensure that we don't have any div by 0 issues here.

            #region Generic
            // Use
            triggerIntervals[Trigger.Use] = 0f; // Should be Effect cooldown.
            triggerChances[Trigger.Use] = 1f;

            // Physical Hit
            triggerIntervals[Trigger.PhysicalAttack] =
            triggerIntervals[Trigger.RangedHit] =
            triggerIntervals[Trigger.PhysicalHit] = 
            triggerIntervals[Trigger.RangedCrit] =
            triggerIntervals[Trigger.PhysicalCrit] = 1f / totalShotsPerSecond; // Note with DOT crits, this may have to be adjusted.

            triggerChances[Trigger.PhysicalAttack] = 
            triggerChances[Trigger.RangedHit] =
            triggerChances[Trigger.PhysicalHit] = (1f - ChanceToMiss);
            triggerChances[Trigger.RangedCrit] =
            triggerChances[Trigger.PhysicalCrit] = Math.Min(1f + critMOD, Math.Max(0f, statsTotal.PhysicalCrit));

            // Dots & damage done.
            triggerIntervals[Trigger.DoTTick] = talents.PiercingShots > 0 ? 1f : 0f; // Also need to add other DOTs into this value. 
            triggerIntervals[Trigger.DamageDone] = Math.Max(0f, 1f / (totalShotsPerSecond + ((talents.PiercingShots > 0 ? 1f : 0f) > 0 ? 1f / (talents.PiercingShots > 0 ? 1f : 0f) : 0f)));
            triggerIntervals[Trigger.DamageOrHealingDone] = Math.Max(0f, 1f / (totalShotsPerSecond + ((talents.PiercingShots > 0 ? 1f : 0f) > 0 ? 1f / (talents.PiercingShots > 0 ? 1f : 0f) : 0f))); // Need to add Self/pet-Heals

            triggerChances[Trigger.DoTTick] = 1f; // This should be up-time on DoTs & PS. 
            triggerChances[Trigger.DamageDone] = 1f;
            triggerChances[Trigger.DamageOrHealingDone] = 1f;

            #endregion 

            #region Pets only
            //triggerIntervals[Trigger.MeleeHit]                              = 
            //triggerIntervals[Trigger.MeleeCrit]                             = Math.Max(0f, calculatedStats.PetCalc.PetCompInterval);
            //triggerIntervals[Trigger.PetClawBiteSmackCrit]                  = Math.Max(0f, calculatedStats.PetCalc.PetClawBiteSmackInterval);

            //triggerChances[Trigger.MeleeHit]                              = calculatedStats.PetCalc.WhAtkTable.AnyLand;
            //triggerChances[Trigger.MeleeCrit]                             = Math.Min(1f + critMOD, Math.Max(0f, calculatedStats.PetCalc.WhAtkTable.Crit));
            //triggerChances[Trigger.PetClawBiteSmackCrit]                  = Math.Min(1f + critMOD, Math.Max(0f, calculatedStats.PetCalc.WhAtkTable.Crit));
            #endregion

            #region Hunter Specific
            triggerIntervals[Trigger.HunterAutoShotHit] = 1f / autoShotsPerSecond;
            triggerIntervals[Trigger.SteadyShotHit] = 0; // calculatedStats.steadyShot.Cd;
            triggerIntervals[Trigger.CobraShotHit] = 0; // calculatedStats.cobraShot.Cd;
            triggerIntervals[Trigger.EnergyOrFocusDropsBelow20PercentOfMax] = 4f; // Approximating as 80% chance every 4 seconds. TODO: Put in some actual method of calculating this

            triggerChances[Trigger.HunterAutoShotHit] = (1f - ChanceToMiss);
            triggerChances[Trigger.SteadyShotHit] = (1f - ChanceToMiss);
            triggerChances[Trigger.CobraShotHit] = (1f - ChanceToMiss);
            triggerChances[Trigger.EnergyOrFocusDropsBelow20PercentOfMax] = 0.80f; // Approximating as 80% chance every 4 seconds. TODO: Put in some actual method of calculating this
            #endregion
        }

        /// <summary>
        /// With Triggers already setup, just pass into the Accumulate function and return the values
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public StatsHunter getSpecialEffects(SpecialEffect effect, Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances, Character Char)
        {
            StatsHunter statsAverage = new StatsHunter();
            triggerIntervals[Trigger.Use] = effect.Cooldown;
            if (float.IsInfinity(effect.Cooldown)) triggerIntervals[Trigger.Use] = Char.BossOptions.BerserkTimer;

            ItemInstance RangeWeap = Char.Ranged;
            float unhastedAttackSpeed = (RangeWeap != null ? RangeWeap.Speed : 2.4f);

            effect.AccumulateAverageStats(statsAverage, triggerIntervals, triggerChances, unhastedAttackSpeed, Char.BossOptions.BerserkTimer);
            return statsAverage;
        }

        #if FALSE
        private StatsHunter GetSpecialEffectsStats(Character Char, Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances, StatsHunter statsTotal, StatsHunter statsToProcess)
        {
            CalculationOptionsHunter calcOpts = Char.CalculationOptions as CalculationOptionsHunter;
            //BossOptions bossOpts = Char.BossOptions;
            ItemInstance RangeWeap = Char.MainHand;
            float speed = (RangeWeap != null ? RangeWeap.Speed : 2.4f);
            HunterTalents talents = Char.HunterTalents;
            StatsHunter statsProcs = new StatsHunter();
            float fightDuration_M = 450f;   //bossOpts.BerserkTimer;
            StatsHunter _stats, _stats2;
            try
            {
                foreach (SpecialEffect effect in (statsToProcess != null ? statsToProcess.SpecialEffects() : statsTotal.SpecialEffects()))
                {
                    float fightDuration = fightDuration_M;
                    /*float oldArp = float.Parse(effect.Stats.ArmorPenetrationRating.ToString());
                    float arpToHardCap = StatConversion.RATING_PER_ARMORPENETRATION;
                    if (effect.Stats.ArmorPenetrationRating > 0) {
                        float arpenBuffs = 0.0f;
                        float currentArp = arpenBuffs + StatConversion.GetArmorPenetrationFromRating(statsTotal.ArmorPenetrationRating
                            + (statsToProcess != null ? statsToProcess.ArmorPenetrationRating : 0f));
                        arpToHardCap *= (1f - currentArp);
                    }*/
                    if (triggerIntervals.ContainsKey(effect.Trigger) && (triggerIntervals[effect.Trigger] > 0f || effect.Trigger == Trigger.Use))
                    {
                        float weight = 1f;
                        switch (effect.Trigger)
                        {
                            case Trigger.Use:
                                _stats = new StatsHunter();
                                if (effect.Stats._rawSpecialEffectDataSize == 1 && statsToProcess == null)
                                {
                                    float uptime = effect.GetAverageUptime(0f, 1f, speed, fightDuration);
                                    _stats.AddSpecialEffect(effect.Stats._rawSpecialEffectData[0]);
                                    _stats2 = GetSpecialEffectsStats(Char, triggerIntervals, triggerChances, statsTotal, _stats);
                                    _stats = _stats2 * uptime;
                                }
                                else
                                {
                                    /*if (effect.Stats.ArmorPenetrationRating > 0 && arpToHardCap < effect.Stats.ArmorPenetrationRating) {
                                        float uptime = effect.GetAverageUptime(0f, 1f, speed, fightDuration);
                                        weight = uptime;
                                        _stats.ArmorPenetrationRating = arpToHardCap;
                                    } else {*/
                                    //    _stats = effect.GetAverageStats(0f, 1f, speed, fightDuration);
                                    _stats = effect.GetAverageStats(triggerIntervals, triggerChances, speed, fightDuration, 1f) as StatsHunter;
                                    //}
                                }
                                statsProcs.Accumulate(_stats, weight);
                                _stats = null;
                                break;
                            case Trigger.RangedHit:
                            case Trigger.PhysicalHit:
                            case Trigger.PhysicalAttack:
                                _stats = new StatsHunter();
                                weight = 1.0f;
                                {
                                    /*if (effect.Stats.ArmorPenetrationRating > 0 && arpToHardCap < effect.Stats.ArmorPenetrationRating) {
                                        float uptime = effect.GetAverageUptime(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], speed, fightDuration);
                                        weight = uptime;
                                        _stats.ArmorPenetrationRating = arpToHardCap;
                                    } else {*/
                                    _stats = effect.GetAverageStats(triggerIntervals, triggerChances, speed, fightDuration, weight) as StatsHunter;
                                    //}
                                }
                                statsProcs.Accumulate(_stats, weight);
                                _stats = null;
                                break;
                            case Trigger.MeleeHit: // Pets Only
                            case Trigger.MeleeCrit: // Pets Only
                            case Trigger.RangedCrit:
                            case Trigger.PhysicalCrit:
                            case Trigger.DoTTick:
                            case Trigger.DamageDone: // physical and dots
                            case Trigger.DamageOrHealingDone: // physical and dots
                            case Trigger.HunterAutoShotHit:
                            case Trigger.SteadyShotHit:
                            case Trigger.CobraShotHit:
                            case Trigger.PetClawBiteSmackCrit:
                                _stats = new StatsHunter();
                                weight = 1.0f;
                                /*if (effect.Stats.ArmorPenetrationRating > 0 && arpToHardCap < effect.Stats.ArmorPenetrationRating) {
                                    float uptime = effect.GetAverageUptime(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], speed, fightDuration);
                                    weight = uptime;
                                    _stats.ArmorPenetrationRating = arpToHardCap;
                                } else {*/
                                _stats = effect.GetAverageStats(triggerIntervals, triggerChances, speed, fightDuration, weight) as StatsHunter;
                                //}
                                statsProcs.Accumulate(_stats, weight);
                                break;
                            case Trigger.SerpentWyvernStingsDoDamage:
                                _stats = new StatsHunter();
                                weight = 1.0f;
                                /*if (effect.Stats.ArmorPenetrationRating > 0 && arpToHardCap < effect.Stats.ArmorPenetrationRating) {
                                    float uptime = effect.GetAverageUptime(triggerIntervals[effect.Trigger], triggerChances[effect.Trigger], speed, fightDuration);
                                    weight = uptime;
                                    _stats.ArmorPenetrationRating = arpToHardCap;
                                } else {*/
                                _stats = effect.GetAverageStats(triggerIntervals, triggerChances, speed, fightDuration, weight) as StatsHunter;
                                //}
                                statsProcs.Accumulate(_stats, weight);
                                break;
                        }
                    }
                    //effect.Stats.ArmorPenetrationRating = oldArp;
                }
            }
            catch (Exception ex)
            {
                new Base.ErrorBox()
                {
                    Title = "Error in getting Special Effect information",
                    Function = "GetSpecialEffectsStats()",
                    TheException = ex,
                }.Show();
            }
            return statsProcs;
        }
        #endif

        #endregion //overrides

        #region Private Functions
        private Specialization GetSpecialization(HunterTalents t)
        {
            Specialization curSpecialization = Specialization.BeastMastery;
            if (t.HighestTree < EnumHelper.GetCount(typeof(Specialization)))
                curSpecialization = (Specialization)t.HighestTree;
            return curSpecialization;
        }
        
        /*
        private ShotData getShotByIndex(int index, CharacterCalculationsHunter calculatedStats)
        {
            // This Index should really match the ENUM values for Shot.
            if (index ==  1) return calculatedStats.aimedShot;
            if (index ==  2) return calculatedStats.arcaneShot;
            if (index ==  3) return calculatedStats.multiShot;
            if (index ==  4) return calculatedStats.serpentSting;
            //if (index ==  5) return calculatedStats.scorpidSting;
            //if (index ==  6) return calculatedStats.viperSting;
            if (index ==  7) return calculatedStats.cobraShot;
            if (index ==  8) return calculatedStats.steadyShot;
            if (index ==  9) return calculatedStats.killShot;
            if (index == 10) return calculatedStats.explosiveShot;
            if (index == 11) return calculatedStats.blackArrow;
            if (index == 12) return calculatedStats.immolationTrap;
            if (index == 13) return calculatedStats.explosiveTrap;
            if (index == 14) return calculatedStats.freezingTrap;
            if (index == 15) return calculatedStats.frostTrap;
            //if (index == 16) return calculatedStats.volley;
            if (index == 17) return calculatedStats.chimeraShot;
            if (index == 18) return calculatedStats.rapidFire;
            if (index == 19) return calculatedStats.readiness;
            if (index == 20) return calculatedStats.bestialWrath;
            return null;
        }
         */
        public static float CalcEffectiveDamage(float damageNormal, float missChance, float critChance, float critAdjust, float damageAdjust) {
            /* OLD CODE
             * 
            float damageCrit  =  damageNormal * (1f + critAdjust);
            float damageTotal = (damageNormal * (1f - critChance)
                                 + (damageCrit * critChance));
            float damageReal  = damageTotal * damageAdjust * (1f - missChance);

            return damageReal;*/

            float dmg = damageNormal; // MhDamage;                  // Base Damage
            //dmg *= combatFactors.DamageBonus;      // Global Damage Bonuses
            //dmg *= combatFactors.DamageReduction;  // Global Damage Penalties
            dmg *= damageAdjust;

            // Work the Attack Table
            float dmgDrop = (1f
                - missChance // MHAtkTable.Miss   // no damage when being missed
                - critChance // MHAtkTable.Crit   // crits   handled below
                );

            float dmgCrit = dmg
                          * critChance // MHAtkTable.Crit
                          * (1f
                             + critAdjust//combatFactors.BonusWhiteCritDmg
                             );//Bonus Damage when critting

            dmg *= dmgDrop;

            dmg += dmgCrit;

            return dmg;
        }
/*        public float GetArmorDamageReduction(Character Char, Stats StatS, CalculationOptionsHunter CalcOpts, BossOptions BossOpts) {
            float armorReduction;
            float arpenBuffs = 0.0f;

            if (BossOpts == null) {
                armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level, (int)StatConversion.NPC_ARMOR[3], StatS.TargetArmorReduction, arpenBuffs)); // default is vs raid boss
            } else {
                armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level, BossOpts.Armor, StatS.TargetArmorReduction, arpenBuffs));
            }

            return armorReduction;
        }
*/
        #endregion
    }
}
