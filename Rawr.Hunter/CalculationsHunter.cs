using System;
using System.Collections.Generic;
#if RAWR3 || SILVERLIGHT
using System.Windows.Media;
#else
using System.Drawing;
#endif
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

namespace Rawr.Hunter {
    [Rawr.Calculations.RawrModelInfo("Hunter", "Inv_Weapon_Bow_07", CharacterClass.Hunter)]
	public class CalculationsHunter : CalculationsBase {
        #region Variables and Properties

        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
				////Relevant Gem IDs for Hunters
				//Red
				int[] delicate = { 41434, 39997, 40112, 42143 };
				//Purple
				int[] shifting = { 41460, 40023, 40130 };
				//Green
                int[] vivid = { 41481, 40088, 40166 };
				//Yellow
                int[] rigid = { 41447, 40014, 40125, 42156 };
				//Orange
                int[] glinting = { 41491, 40044, 40148 };
				//Meta
				int relentless = 41398;

				return new List<GemmingTemplate>()
				{
					new GemmingTemplate() { Model = "Hunter", Group = "Uncommon",             RedId = delicate[0], YellowId = delicate[0], BlueId = delicate[0], PrismaticId = delicate[0], MetaId = relentless }, //Max Agi
					new GemmingTemplate() { Model = "Hunter", Group = "Uncommon",             RedId = delicate[0], YellowId = glinting[0], BlueId = shifting[0], PrismaticId = delicate[0], MetaId = relentless }, //Agi/Hit
					new GemmingTemplate() { Model = "Hunter", Group = "Uncommon",             RedId = glinting[0], YellowId = rigid[0]   , BlueId = vivid[0]   , PrismaticId = rigid[0]   , MetaId = relentless }, //Hit
						
					new GemmingTemplate() { Model = "Hunter", Group = "Rare",                 RedId = delicate[1], YellowId = delicate[1], BlueId = delicate[1], PrismaticId = delicate[1], MetaId = relentless }, //Max Agi
					new GemmingTemplate() { Model = "Hunter", Group = "Rare",                 RedId = delicate[1], YellowId = glinting[1], BlueId = shifting[1], PrismaticId = delicate[1], MetaId = relentless }, //Agi/Hit
					new GemmingTemplate() { Model = "Hunter", Group = "Rare",                 RedId = glinting[1], YellowId = rigid[1]   , BlueId = vivid[1]   , PrismaticId = rigid[1]   , MetaId = relentless }, //Hit
						
					new GemmingTemplate() { Model = "Hunter", Group = "Epic", Enabled = true, RedId = delicate[2], YellowId = delicate[2], BlueId = delicate[2], PrismaticId = delicate[2], MetaId = relentless }, //Max Agi
					new GemmingTemplate() { Model = "Hunter", Group = "Epic", Enabled = true, RedId = delicate[2], YellowId = glinting[2], BlueId = shifting[2], PrismaticId = delicate[2], MetaId = relentless }, //Agi/Hit
					new GemmingTemplate() { Model = "Hunter", Group = "Epic", Enabled = true, RedId = glinting[2], YellowId = rigid[2]   , BlueId = vivid[2]   , PrismaticId = rigid[2]   , MetaId = relentless }, //Hit
						
					new GemmingTemplate() { Model = "Hunter", Group = "Jeweler",              RedId = delicate[3], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[3], MetaId = relentless }, //Max Agi
					new GemmingTemplate() { Model = "Hunter", Group = "Jeweler",              RedId = delicate[2], YellowId = delicate[3], BlueId = delicate[3], PrismaticId = delicate[2], MetaId = relentless }, //Agi/Hit
					new GemmingTemplate() { Model = "Hunter", Group = "Jeweler",              RedId = rigid[3]   , YellowId = rigid[2]   , BlueId = rigid[3]   , PrismaticId = rigid[2]   , MetaId = relentless }, //Hit
				};
            }
        }

        #if RAWR3 || SILVERLIGHT
            private ICalculationOptionsPanel calculationOptionsPanel = null;
            public override ICalculationOptionsPanel CalculationOptionsPanel
        #else
            private CalculationOptionsPanelBase calculationOptionsPanel = null;
		    public override CalculationOptionsPanelBase CalculationOptionsPanel
        #endif
            {
                get {
				    return calculationOptionsPanel ?? (calculationOptionsPanel = new CalculationOptionsPanelHunter());
                }
            }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    _characterDisplayCalculationLabels = new string[] {
				        "Basic Stats:Health and Stamina",
				        "Basic Stats:Mana",
				        "Basic Stats:Armor",
				        "Basic Stats:Agility",
                        "Basic Stats:Ranged Attack Power",
				        "Basic Stats:Intellect",
				        @"Basic Stats:Hit*8.00% chance to miss base for Yellow Attacks
Focused Aim 0 - 8%-0%=8%=264 Rating soft cap
Focused Aim 1 - 8%-1%=7%=230 Rating soft cap
Focused Aim 2 - 8%-2%=6%=197 Rating soft cap
Focused Aim 3 - 8%-3%=5%=164 Rating soft cap",
				        "Basic Stats:Crit",
				        @"Basic Stats:Armor Penetration*Rating Cap- 1400",
				        "Basic Stats:Haste",

                        "Pet Stats:Pet Attack Power",
				        "Pet Stats:Pet Hit Percentage",
				        "Pet Stats:Pet Melee Crit Percentage",
				        "Pet Stats:Pet Specials Crit Percentage",
				        "Pet Stats:Pet White DPS",
				        "Pet Stats:Pet Kill Command DPS",
				        "Pet Stats:Pet Specials DPS",

				        "Shot Stats:Aimed Shot",
				        "Shot Stats:Arcane Shot",
				        "Shot Stats:Multi Shot",
				        "Shot Stats:Serpent Sting",
				        "Shot Stats:Scorpid Sting",
				        "Shot Stats:Viper Sting",
                        "Shot Stats:Silencing Shot",
                        "Shot Stats:Steady Shot",
                        "Shot Stats:Kill Shot",
				        "Shot Stats:Explosive Shot",
				        "Shot Stats:Black Arrow",
                        "Shot Stats:Immolation Trap",
                        "Shot Stats:Chimera Shot",
                        "Shot Stats:Rapid Fire",
                        "Shot Stats:Readiness",
                        "Shot Stats:Beastial Wrath",
                        "Shot Stats:Blood Fury",
                        "Shot Stats:Berserk",

                        "Mana:Mana Usage Per Second",
                        "Mana:Mana Regen Per Second",
                        "Mana:Potion Regen Per Second",
                        "Mana:Viper Regen Per Second",
                        "Mana:Normal Change",
                        "Mana:Change during Viper",
                        "Mana:Time to OOM",
                        "Mana:Time to Full",
                        "Mana:Viper Damage Penalty",
                        "Mana:Viper Uptime",

                        "Hunter DPS:Autoshot DPS",
                        "Hunter DPS:Priority Rotation DPS",
                        "Hunter DPS:Wild Quiver DPS",
                        "Hunter DPS:Proc DPS",
                        "Hunter DPS:Kill Shot low HP gain",
                        "Hunter DPS:Aspect Loss",
                        // 29-10-2009 Drizz: Adding to display the piercing shot effect in stats
                        "Hunter DPS:Piercing Shots DPS",

				        "Combined DPS:Hunter DPS",
				        "Combined DPS:Pet DPS",
				        "Combined DPS:Total DPS"
    			    };
                }
                return _characterDisplayCalculationLabels;
            }
        }

		private string[] _optimizableCalculationLabels = null;
		public override string[] OptimizableCalculationLabels {
			get {
                if (_optimizableCalculationLabels == null) {
                    _optimizableCalculationLabels = new string[] {
					    "Health",
                        "Attack Power",
                        "Agility",
                        "Mana",
					    "Crit %",
                        "Haste %",
                        "Armor Penetration %",
                        "% Chance to Miss (Yellow)",
					};
                }
				return _optimizableCalculationLabels;
			}
		}

        private Dictionary<string, Color> _subPointNameColors = null;
        private Dictionary<string, Color> _subPointNameColorsDPS = null;
        private Dictionary<string, Color> _subPointNameColorsMPS = null;
        private Dictionary<string, Color> _subPointNameColorsDPM = null;
        public override Dictionary<string, Color> SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColorsDPS = new Dictionary<string, Color>();
                    _subPointNameColorsDPS.Add("Hunter DPS", Color.FromArgb(255, 0, 128, 255));
                    _subPointNameColorsDPS.Add("Pet DPS", Color.FromArgb(255, 255, 100, 0));

                    _subPointNameColorsMPS = new Dictionary<string, Color>();
                    _subPointNameColorsMPS.Add("MPS", Color.FromArgb(255, 0, 0, 255));

                    _subPointNameColorsDPM = new Dictionary<string, Color>();
                    _subPointNameColorsDPM.Add("Damage per Mana", Color.FromArgb(255, 0, 0, 255));

                    _subPointNameColors = _subPointNameColorsDPS;
                }
                return _subPointNameColors;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHunter(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHunter(); }

		public override ICalculationOptionBase DeserializeDataObject(string xml) {
			XmlSerializer s = new XmlSerializer(typeof(CalculationOptionsHunter));
			StringReader sr = new StringReader(xml);
			CalculationOptionsHunter calcOpts = s.Deserialize(sr) as CalculationOptionsHunter;

            // convert buffs here!
            calcOpts.petActiveBuffs = new List<Buff>(calcOpts._petActiveBuffsXml.ConvertAll(buff => Buff.GetBuffByName(buff)));
            calcOpts.petActiveBuffs.RemoveAll(buff => buff == null);

			return calcOpts;
        }

        #endregion

        #region Relevancy

        public override CharacterClass TargetClass { get { return CharacterClass.Hunter; } }

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<ItemType>(new[] {
					    ItemType.None,
					    ItemType.AmmoPouch,
                        ItemType.Arrow,
                        ItemType.Bow,
                        ItemType.Bullet,
                        ItemType.Crossbow,
                        ItemType.Dagger,
					    ItemType.FistWeapon,
                        ItemType.Gun,
                        ItemType.Leather,
                        ItemType.Mail,
                        ItemType.OneHandAxe,                        
                        ItemType.OneHandSword,
                        ItemType.Polearm,
                        ItemType.Quiver,
                        ItemType.Staff,                    
                        ItemType.TwoHandAxe,
                        ItemType.TwoHandSword
				    });
                }
                return _relevantItemTypes;
            }
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot) {
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        private bool HidingBadStuff { get { return HidingBadStuff_Spl || HidingBadStuff_PvP; } }
        private static bool _HidingBadStuff_Spl = true;
        internal static bool HidingBadStuff_Spl { get { return _HidingBadStuff_Spl; } set { _HidingBadStuff_Spl = value; } }
        private static bool _HidingBadStuff_PvP = true;
        internal static bool HidingBadStuff_PvP { get { return _HidingBadStuff_PvP; } set { _HidingBadStuff_PvP = value; } }
        private static bool _HidingBadStuff_Prof = false;
        internal static bool HidingBadStuff_Prof { get { return _HidingBadStuff_Prof; } set { _HidingBadStuff_Prof = value; } }

        public override Stats GetRelevantStats(Stats stats)
        {
			Stats relevantStats = new Stats() {
                // Basic Stats
                Stamina = stats.Stamina,
                Health = stats.Health,
                Agility = stats.Agility,
                Strength = stats.Strength,
				Intellect = stats.Intellect,
				Mana = stats.Mana,
				Mp5 = stats.Mp5,
                Armor = stats.Armor,
                // Ratings
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

                ArmorPenetrationRating = stats.ArmorPenetrationRating,
				ArmorPenetration = stats.ArmorPenetration,
				Miss = stats.Miss,
				ScopeDamage = stats.ScopeDamage,

                // Special
                HighestStat = stats.HighestStat,
                Paragon = stats.Paragon,
 				BonusHunter_PvP_4pc = stats.BonusHunter_PvP_4pc,
                FireDamage = stats.FireDamage,
                ArcaneDamage = stats.ArcaneDamage,
                ShadowDamage = stats.ShadowDamage,
                ManaRestoreFromBaseManaPPM = stats.ManaRestoreFromBaseManaPPM,
                MovementSpeed = stats.MovementSpeed,
                StunDurReduc = stats.StunDurReduc,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                DarkmoonCardDeathProc = stats.DarkmoonCardDeathProc,
                ManaorEquivRestore = stats.ManaorEquivRestore,
                HealthRestore = stats.HealthRestore,

                // Multipliers
				BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
				BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
				BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
				BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
				BonusRangedAttackPowerMultiplier = stats.BonusRangedAttackPowerMultiplier,

				BonusManaPotion = stats.BonusManaPotion,
				DamageTakenMultiplier = stats.DamageTakenMultiplier,
				BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
				BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
				BonusPetDamageMultiplier = stats.BonusPetDamageMultiplier,
				BonusPetCritChance = stats.BonusPetCritChance,
				BonusHunter_T7_4P_ViperSpeed = stats.BonusHunter_T7_4P_ViperSpeed,
				BonusHunter_T8_2P_SerpDmg = stats.BonusHunter_T8_2P_SerpDmg,
				BonusHunter_T8_4P_SteadyShotAPProc = stats.BonusHunter_T8_4P_SteadyShotAPProc,
				BonusHunter_T9_2P_SerpCanCrit = stats.BonusHunter_T9_2P_SerpCanCrit,
				BonusHunter_T9_4P_SteadyShotPetAPProc = stats.BonusHunter_T9_4P_SteadyShotPetAPProc,

                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if ((effect.Trigger == Trigger.Use ||
                     effect.Trigger == Trigger.PhysicalCrit ||
                     effect.Trigger == Trigger.PhysicalHit ||
                     effect.Trigger == Trigger.DoTTick ||
                     effect.Trigger == Trigger.DamageDone ||
                     effect.Trigger == Trigger.DamageTaken)
                    && HasRelevantStats(effect.Stats))
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
            bool isRelevant = (
                // Base Stats
                stats.Agility +
                stats.Strength +
                stats.AttackPower + stats.RangedAttackPower +
                stats.Intellect +
                // Ratings
                stats.Mp5 +
                stats.ArmorPenetrationRating +
                stats.CritRating  + stats.RangedCritRating +
                stats.HasteRating + stats.RangedHasteRating +
                stats.HitRating   + stats.RangedHitRating +
                // Bonuses
                stats.ArmorPenetration +
                stats.PhysicalCrit +
                stats.RangedHaste +
                stats.PhysicalHit +
                stats.MovementSpeed + stats.StunDurReduc + stats.SnareRootDurReduc + stats.FearDurReduc +
                stats.ManaRestore + // Mana Pots
                stats.ManaRestoreFromMaxManaPerSecond + // Reblenishment Buffs
                stats.ManaRestoreFromBaseManaPPM + // Judgement of Wisdom Buff
                // Target Debuffs
                // Procs
                stats.DarkmoonCardDeathProc +
                stats.HighestStat +
                stats.Paragon +
                stats.ManaorEquivRestore +
                // Multipliers
                stats.BonusAgilityMultiplier +
                stats.BonusAttackPowerMultiplier +
                stats.BonusRangedAttackPowerMultiplier +
                stats.BonusCritMultiplier +
                stats.BonusIntellectMultiplier +
                stats.BonusPetDamageMultiplier +
                stats.BonusDamageMultiplier +
                stats.BonusSpiritMultiplier +
                stats.DamageTakenMultiplier +
                stats.BaseArmorMultiplier +
                stats.BonusArmorMultiplier +
                stats.BonusBleedDamageMultiplier +
                stats.BonusPhysicalDamageMultiplier +
                // Set Bonuses
                stats.BonusHunter_T7_4P_ViperSpeed +
                stats.BonusHunter_T8_2P_SerpDmg +
                stats.BonusHunter_T8_4P_SteadyShotAPProc +
                stats.BonusHunter_T9_2P_SerpCanCrit +
                stats.BonusHunter_T9_4P_SteadyShotPetAPProc +
                stats.BonusHunter_PvP_4pc +
                // Special
                stats.ScopeDamage +
                stats.BonusManaPotion +
                stats.BonusPetCritChance +
                // Special Damage Type Effectors
                stats.FireDamage + stats.BonusFireDamageMultiplier +
                stats.ArcaneDamage + stats.BonusArcaneDamageMultiplier +
                stats.ShadowDamage + stats.BonusShadowDamageMultiplier +
                stats.FrostDamage + stats.BonusFrostDamageMultiplier +
                stats.HolyDamage + stats.BonusHolyDamageMultiplier +
                stats.NatureDamage + stats.BonusNatureDamageMultiplier
            ) != 0;

            foreach (SpecialEffect e in stats.SpecialEffects())
            {
                if (e.Trigger == Trigger.DamageDone
                    || e.Trigger == Trigger.DoTTick
                    || e.Trigger == Trigger.PhysicalCrit
                    || e.Trigger == Trigger.PhysicalHit
                    || e.Trigger == Trigger.Use
                    || e.Trigger == Trigger.DamageTaken)
                {
                    isRelevant |= HasRelevantStats(e.Stats);
                }
            }

            return isRelevant;
        }

        private bool HasSurvivabilityStats(Stats stats)
        {
            bool retVal = false;
            if ((stats.Health
                + stats.Stamina
                + stats.BonusHealthMultiplier
                + stats.BonusStaminaMultiplier
                + stats.HealthRestore
                ) > 0)
            {
                retVal = true;
            }
            return retVal;
        }

        private bool HasIgnoreStats(Stats stats)
        {
            if (!HidingBadStuff) { return false; }
            return (
                // Remove Spellcasting only Stuff
                (HidingBadStuff_Spl ? stats.SpellPower + stats.SpellPenetration
                                    : 0f)
                // Remove PvP Items
                + (HidingBadStuff_PvP ? stats.Resilience
                                      : 0f)
                ) > 0;
        }

        private static Character cacheChar = null;

        public bool CheckHasProf(Profession p)
        {
            if (cacheChar == null) { return false; }
            if (cacheChar.PrimaryProfession == p) { return true; }
            if (cacheChar.SecondaryProfession == p) { return true; }

            return false;
        }

        public override bool IsEnchantRelevant(Enchant enchant) {
            string name = enchant.Name;
            if (name.Contains("Rune of the Fallen Crusader"))
            {
                return false; // Bad DK Enchant, Bad!
            }
            else if (HidingBadStuff_Prof)
            {
                if (!CheckHasProf(Profession.Enchanting))
                {
                    if (enchant.Slot == ItemSlot.Finger &&
                        (name.Contains("Assault") ||
                         name.Contains("Stats") ||
                         name.Contains("Striking") ||
                         name.Contains("Stamina"))
                        )
                    {
                        return false;
                    }
                }
                if (!CheckHasProf(Profession.Engineering))
                {
                    if (name.Contains("Mind Amplification Dish") ||
                        name.Contains("Flexweave Underlay") ||
                        name.Contains("Hyperspeed Accelerators") ||
                        name.Contains("Reticulated Armor Webbing") ||
                        name.Contains("Nitro Boosts"))
                    {
                        return false;
                    }
                }
                if (!CheckHasProf(Profession.Inscription))
                {
                    if (name.Contains("Master's") ||
                        name.Contains("Inscription of Triumph"))
                    {
                        return false;
                    }
                }
                if (!CheckHasProf(Profession.Leatherworking))
                {
                    if (name.Contains("Fur Lining - Attack Power") ||
                        name.Contains("Fur Lining - Stamina"))
                    {
                        return false;
                    }
                }
                if (!CheckHasProf(Profession.Tailoring))
                {
                    if (name.Contains("Swordguard Embroidery"))
                    {
                        return false;
                    }
                }
            }
            return HasWantedStats(enchant.Stats) || (HasSurvivabilityStats(enchant.Stats) && !HasIgnoreStats(enchant.Stats));
            //return base.IsEnchantRelevant(enchant);
        }

        public override bool IsBuffRelevant(Buff buff) {
            string name = buff.Name;
            // Force some buffs to active
            if (name.Contains("Potion of Wild Magic")
                || name.Contains("Insane Strength Potion")
            ) {
                return true;
            }
            // Force some buffs to go away
            else if (!buff.AllowedClasses.Contains(CharacterClass.Hunter)
                     // Gets selected due to a bug saying it increases BonusAspectOfTheViperAttackSpeed
                     || name.Contains("Concentration Aura")
                     // these four foods give stam, which is the only useful part of their buff.
                     // removed because you shouldn't use these foods - other foods are always better.
                     || name.Contains("Spirit Food")
                     || name.Contains("Strength Food")
                     || name.Contains("Expertise Food")
                     || name.Contains("Spell Power Food")
            ) {
                return false;
            }
            bool haswantedStats = HasWantedStats(buff.Stats);
            bool hassurvStats = HasSurvivabilityStats(buff.Stats);
            bool hasbadstats = HasIgnoreStats(buff.Stats);
            bool retVal = haswantedStats || (hassurvStats && !hasbadstats);
            return retVal;
            //return base.IsBuffRelevant(buff);               
        }
       
		public override bool CanUseAmmo { get { return true; } }

		public override bool IsItemRelevant(Item item) {
            if ( // Manual override for +X to all Stats gems
                   item.Name == "Nightmare Tear"
                || item.Name == "Enchanted Tear"
                || item.Name == "Enchanted Pearl"
                || item.Name == "Chromatic Sphere"
                ) {
                return true;
                //}else if (item.Type == ItemType.Polearm && 
            } else if (item.Slot == ItemSlot.Ranged && item.Type == ItemType.Idol) {
                return false;
            } else if (item.Slot == ItemSlot.Projectile ||
                (item.Slot == ItemSlot.Ranged && (item.Type == ItemType.Gun || item.Type == ItemType.Bow || item.Type == ItemType.Crossbow)))
            {
                return true;
            } else {
                Stats stats = item.Stats;
                bool wantedStats = HasWantedStats(stats);
                bool survstats = HasSurvivabilityStats(stats);
                bool ignoreStats = HasIgnoreStats(stats);
                return (wantedStats || survstats) && !ignoreStats && base.IsItemRelevant(item);
            }
		}

        private static List<Buff> _relevantPetBuffs = new List<Buff>();
        public static List<Buff> RelevantPetBuffs {
            get {
                if (_relevantPetBuffs.Count == 0) {
                    _relevantPetBuffs = Buff.AllBuffs.FindAll(buff => CalculationsHunter.IsPetBuffRelevant(buff));
                }
                return _relevantPetBuffs;
            }
        }

        private static bool IsPetBuffRelevant(Buff buff)
        {
            if (buff.Group == "Elixirs and Flasks") return false;
            if (buff.Group == "Food") return false;
            if (buff.Group == "Set Bonuses") return false;
            if (buff.Group == "Profession Buffs") return false;
            if (buff.Group == "Temporary Buffs") return false;
            if (buff.Group == "Critical Strike Chance Taken") return false; // target debuff

            // Greater Blessing of Kings
            if (buff.Stats.BonusAgilityMultiplier != 0) return true;
            if (buff.Stats.BonusStrengthMultiplier != 0) return true;
            if (buff.Stats.BonusStaminaMultiplier != 0) return true;

            //Commanding Shout & Blood Pact
            //if (buff.Stats.Health != 0) return true;

            // Greater Blessing of Might & Battle Shout
		    if (buff.Stats.AttackPower != 0) return true;

            // True Shot Aura (not you) & Abo. Might & Unl. Rage
		    if (buff.Stats.BonusAttackPowerMultiplier != 0) return true;

            // Leader of the Pack/Rampage
		    if (buff.Stats.PhysicalCrit != 0) return true;

            // Strength of Earth Totem & Horn of Winter &  Gift of the Wild & Prayer of Fortitude & Scrolls
            if (buff.Stats.Strength != 0) return true;
            if (buff.Stats.Agility != 0) return true;
            //if (buff.Stats.Stamina != 0) return true;

            // WF Totem & Imp. Icy Talons & Swift Ret & Moonkin Aura
		    if (buff.Stats.PhysicalHaste != 0) return true;

            // Ret Aura & Feroc. Insp.
		    if (buff.Stats.BonusDamageMultiplier  != 0) return true;

            // Draenei racial (not you)
		    if (buff.Stats.PhysicalHit != 0) return true;

            // Pet Food
            //if (buff.Stats.PetStamina != 0) return true;
            if (buff.Stats.PetStrength != 0) return true;

            return false;
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsHunter calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            float hasRelevantBuff;

            #region Racials to Force Enable
            // Draenei should always have this buff activated
            // NOTE: for other races we don't wanna take it off if the user has it active, so not adding code for that
            if (character.Race == CharacterRace.Draenei
                && !character.ActiveBuffs.Contains(Buff.GetBuffByName("Heroic Presence")))
            {
                character.ActiveBuffsAdd(("Heroic Presence"));
            }
            #endregion

            #region Professions to Force Enable
            // Miners should always have this buff activated
            if (CheckHasProf(Profession.Mining)
                && !character.ActiveBuffs.Contains(Buff.GetBuffByName("Toughness")))
            {
                character.ActiveBuffsAdd(("Toughness"));
            }
            // Skinners should always have this buff activated
            if (CheckHasProf(Profession.Skinning)
                && !character.ActiveBuffs.Contains(Buff.GetBuffByName("Master of Anatomy")))
            {
                character.ActiveBuffsAdd(("Master of Anatomy"));
            }
            // Engineers should always have this buff activated IF the Primary is
            if (CheckHasProf(Profession.Engineering))
            {
                List<String> list = new List<string>() {
                    "Runic Mana Injector",
                    "Runic Healing Injector",
                };
                foreach (String name in list)
                {
                    if (character.ActiveBuffs.Contains(Buff.GetBuffByName(name)) &&
                        !character.ActiveBuffs.Contains(Buff.GetBuffByName(name + " (Engineer Bonus)")))
                    {
                        character.ActiveBuffsAdd((name + " (Engineer Bonus)"));
                    }
                }
            }
            // NOTE: Might do this again for Alchemy and Mixology but
            // there could be conflicts for different specialties
            #endregion

            #region Passive Ability Auto-Fixing
            // Removes the Trueshot Aura Buff and it's equivalents Unleashed Rage and Abomination's Might if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            /*{
                hasRelevantBuff = character.WarriorTalents.Trauma;
                Buff a = Buff.GetBuffByName("Trueshot Aura");
                Buff b = Buff.GetBuffByName("Unleashed Rage");
                Buff c = Buff.GetBuffByName("Abomination's Might");
                if (hasRelevantBuff > 0)
                {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); removedBuffs.Add(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); removedBuffs.Add(b); }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); removedBuffs.Add(c); }
                }
            }*/
            #endregion

            #region Special Pot Handling
            foreach (Buff potionBuff in character.ActiveBuffs.FindAll(b => b.Name.Contains("Potion")))
            {
                if (potionBuff.Stats._rawSpecialEffectData != null
                    && potionBuff.Stats._rawSpecialEffectData[0] != null)
                {
                    Stats newStats = new Stats();
                    newStats.AddSpecialEffect(new SpecialEffect(potionBuff.Stats._rawSpecialEffectData[0].Trigger,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Stats,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Duration,
                                                                calcOpts.Duration,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Chance,
                                                                potionBuff.Stats._rawSpecialEffectData[0].MaxStack));

                    Buff newBuff = new Buff() { Stats = newStats };
                    character.ActiveBuffs.Remove(potionBuff);
                    character.ActiveBuffsAdd(newBuff);
                    removedBuffs.Add(potionBuff);
                    addedBuffs.Add(newBuff);
                }
            }
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            #endregion

            foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs) {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }
        public override void SetDefaults(Character character) {
        }

        #endregion

        #region Special Comparison Charts
        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] {
                        "Spammed Shots DPS",
                        "Spammed Shots MPS",
                        "Rotation DPS",
                        "Rotation MPS",
                        "Shot Damage per Mana",
                        "Item Budget"
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
                case "Spammed Shots DPS":

                    return new ComparisonCalculationBase[] {
                        comparisonFromShotSpammedDPS(calculations.aimedShot),
                        comparisonFromShotSpammedDPS(calculations.arcaneShot),
                        comparisonFromShotSpammedDPS(calculations.multiShot),
                        comparisonFromShotSpammedDPS(calculations.serpentSting),
                        comparisonFromShotSpammedDPS(calculations.scorpidSting),
                        comparisonFromShotSpammedDPS(calculations.viperSting),
                        comparisonFromShotSpammedDPS(calculations.silencingShot),
                        comparisonFromShotSpammedDPS(calculations.steadyShot),
                        comparisonFromShotSpammedDPS(calculations.killShot),
                        comparisonFromShotSpammedDPS(calculations.explosiveShot),
                        comparisonFromShotSpammedDPS(calculations.blackArrow),
                        comparisonFromShotSpammedDPS(calculations.immolationTrap),
                        comparisonFromShotSpammedDPS(calculations.chimeraShot),
                    };

                case "Spammed Shots MPS":

                    _subPointNameColors = _subPointNameColorsMPS;
                    return new ComparisonCalculationBase[] {
                        comparisonFromShotSpammedMPS(calculations.aimedShot),
                        comparisonFromShotSpammedMPS(calculations.arcaneShot),
                        comparisonFromShotSpammedMPS(calculations.multiShot),
                        comparisonFromShotSpammedMPS(calculations.serpentSting),
                        comparisonFromShotSpammedMPS(calculations.scorpidSting),
                        comparisonFromShotSpammedMPS(calculations.viperSting),
                        comparisonFromShotSpammedMPS(calculations.silencingShot),
                        comparisonFromShotSpammedMPS(calculations.steadyShot),
                        comparisonFromShotSpammedMPS(calculations.killShot),
                        comparisonFromShotSpammedMPS(calculations.explosiveShot),
                        comparisonFromShotSpammedMPS(calculations.blackArrow),
                        comparisonFromShotSpammedMPS(calculations.immolationTrap),
                        comparisonFromShotSpammedMPS(calculations.chimeraShot),
                        comparisonFromShotSpammedMPS(calculations.rapidFire),
                        comparisonFromShotSpammedMPS(calculations.readiness),
                        comparisonFromShotSpammedMPS(calculations.beastialWrath),
                        comparisonFromShotSpammedMPS(calculations.bloodFury),
                        comparisonFromShotSpammedMPS(calculations.berserk),
                    };

                case "Rotation DPS":

                    return new ComparisonCalculationBase[] {
                        comparisonFromShotRotationDPS(calculations.aimedShot),
                        comparisonFromShotRotationDPS(calculations.arcaneShot),
                        comparisonFromShotRotationDPS(calculations.multiShot),
                        comparisonFromShotRotationDPS(calculations.serpentSting),
                        comparisonFromShotRotationDPS(calculations.scorpidSting),
                        comparisonFromShotRotationDPS(calculations.viperSting),
                        comparisonFromShotRotationDPS(calculations.silencingShot),
                        comparisonFromShotRotationDPS(calculations.steadyShot),
                        comparisonFromShotRotationDPS(calculations.killShot),
                        comparisonFromShotRotationDPS(calculations.explosiveShot),
                        comparisonFromShotRotationDPS(calculations.blackArrow),
                        comparisonFromShotRotationDPS(calculations.immolationTrap),
                        comparisonFromShotRotationDPS(calculations.chimeraShot),
                        comparisonFromDoubles("Autoshot", calculations.AutoshotDPS, 0),
                        comparisonFromDoubles("WildQuiver", calculations.WildQuiverDPS, 0),
                        comparisonFromDoubles("OnProc", calculations.OnProcDPS, 0),
                        comparisonFromDoubles("KillShotSub20", calculations.killShotSub20FinalGain, 0),
                        comparisonFromDoubles("AspectBeastLoss", calculations.aspectBeastLostDPS, 0),
                        comparisonFromDoubles("PetAutoAttack", 0, calculations.petWhiteDPS),
                        comparisonFromDoubles("PetSkills", 0, calculations.petSpecialDPS),
                        comparisonFromDoubles("KillCommand", 0, calculations.petKillCommandDPS),
                    };

                case "Rotation MPS":

                    _subPointNameColors = _subPointNameColorsMPS;
                    return new ComparisonCalculationBase[] {
                        comparisonFromShotRotationMPS(calculations.aimedShot),
                        comparisonFromShotRotationMPS(calculations.arcaneShot),
                        comparisonFromShotRotationMPS(calculations.multiShot),
                        comparisonFromShotRotationMPS(calculations.serpentSting),
                        comparisonFromShotRotationMPS(calculations.scorpidSting),
                        comparisonFromShotRotationMPS(calculations.viperSting),
                        comparisonFromShotRotationMPS(calculations.silencingShot),
                        comparisonFromShotRotationMPS(calculations.steadyShot),
                        comparisonFromShotRotationMPS(calculations.killShot),
                        comparisonFromShotRotationMPS(calculations.explosiveShot),
                        comparisonFromShotRotationMPS(calculations.blackArrow),
                        comparisonFromShotRotationMPS(calculations.immolationTrap),
                        comparisonFromShotRotationMPS(calculations.chimeraShot),
                        comparisonFromShotRotationMPS(calculations.rapidFire),
                        comparisonFromShotRotationMPS(calculations.readiness),
                        comparisonFromShotRotationMPS(calculations.beastialWrath),
                        comparisonFromShotRotationMPS(calculations.bloodFury),
                        comparisonFromShotRotationMPS(calculations.berserk),
                        comparisonFromDouble("KillCommand", calculations.petKillCommandMPS),
                    };

                case "Shot Damage per Mana":

                    _subPointNameColors = _subPointNameColorsDPM;
                    return new ComparisonCalculationBase[] {
                        comparisonFromShotDPM(calculations.aimedShot),
                        comparisonFromShotDPM(calculations.arcaneShot),
                        comparisonFromShotDPM(calculations.multiShot),
                        comparisonFromShotDPM(calculations.serpentSting),
                        comparisonFromShotDPM(calculations.scorpidSting),
                        comparisonFromShotDPM(calculations.viperSting),
                        comparisonFromShotDPM(calculations.silencingShot),
                        comparisonFromShotDPM(calculations.steadyShot),
                        comparisonFromShotDPM(calculations.killShot),
                        comparisonFromShotDPM(calculations.explosiveShot),
                        comparisonFromShotDPM(calculations.blackArrow),
                        comparisonFromShotDPM(calculations.immolationTrap),
                        comparisonFromShotDPM(calculations.chimeraShot),
                    };

                case "Item Budget":

                    return new ComparisonCalculationBase[] { 
                        comparisonFromStat(character, calculations, new Stats() { Intellect = 10f }, "10 Intellect"),
                        comparisonFromStat(character, calculations, new Stats() { Agility = 10f }, "10 Agility"),
                        comparisonFromStat(character, calculations, new Stats() { Mp5 = 4f }, "4 MP5"),
                        comparisonFromStat(character, calculations, new Stats() { CritRating = 10f }, "10 Crit Rating"),
                        comparisonFromStat(character, calculations, new Stats() { HitRating = 10f }, "10 Hit Rating"),
                        comparisonFromStat(character, calculations, new Stats() { ArmorPenetrationRating = 10f }, "1.4 Armor Penetration Rating"),
                        comparisonFromStat(character, calculations, new Stats() { AttackPower = 20f }, "20 Attack Power"),
                        comparisonFromStat(character, calculations, new Stats() { RangedAttackPower = 25f }, "25 Ranged Attack Power"),
                        comparisonFromStat(character, calculations, new Stats() { HasteRating = 10f }, "10 Haste Rating"),
                    };
            }

            return new ComparisonCalculationBase[0];

        }
        private ComparisonCalculationHunter comparisonFromShotSpammedDPS(ShotData shot)
        {
            ComparisonCalculationHunter comp =  new ComparisonCalculationHunter();

            float shotWait = shot.duration > shot.cooldown ? shot.duration : shot.cooldown;
            float dps = shotWait > 0 ? (float)(shot.damage / shotWait) : 0;

            comp.Name = Enum.GetName(typeof(Shots), shot.type);
            comp.HunterDpsPoints = dps;
            comp.OverallPoints = dps;
            return comp;
        }

        private ComparisonCalculationHunter comparisonFromShotSpammedMPS(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();

            float shotWait = shot.duration > shot.cooldown ? shot.duration : shot.cooldown;
            float mps = shotWait > 0 ? (float)(shot.mana / shotWait) : 0;

            comp.Name = Enum.GetName(typeof(Shots), shot.type);
            comp.SubPoints = new float[] { mps };
            comp.OverallPoints = mps;
            return comp;
        }

        private ComparisonCalculationHunter comparisonFromShotRotationDPS(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();
            comp.Name = Enum.GetName(typeof(Shots), shot.type);
            comp.SubPoints = new float[] { (float)shot.dps };
            comp.OverallPoints = (float)shot.dps;
            return comp;
        }

        private ComparisonCalculationHunter comparisonFromShotRotationMPS(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();
            comp.Name = Enum.GetName(typeof(Shots), shot.type);
            comp.SubPoints = new float[] { (float)shot.mps };
            comp.OverallPoints = (float)shot.mps;
            return comp;
        }

        private ComparisonCalculationHunter comparisonFromShotDPM(ShotData shot)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();

            float dpm = shot.mana > 0 ? (float)(shot.damage / shot.mana) : 0;

            comp.Name = Enum.GetName(typeof(Shots), shot.type);
            comp.SubPoints = new float[] { dpm };
            comp.OverallPoints = dpm;
            return comp;
        }

        private ComparisonCalculationHunter comparisonFromStat(Character character, CharacterCalculationsHunter calcBase, Stats stats, string label)
        {
            ComparisonCalculationHunter comp = new ComparisonCalculationHunter();

            CharacterCalculationsHunter calcStat = GetCharacterCalculations(character, new Item() { Stats = stats }) as CharacterCalculationsHunter;

            comp.Name = label;
            comp.HunterDpsPoints = calcStat.HunterDpsPoints - calcBase.HunterDpsPoints;
            comp.PetDpsPoints = calcStat.PetDpsPoints - calcBase.PetDpsPoints;
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

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations) {
            cacheChar = character;
            CharacterCalculationsHunter calculatedStats = new CharacterCalculationsHunter();
            if (character == null) { return calculatedStats; }
            calculatedStats.character = character;
            CalculationOptionsHunter calcOpts = character.CalculationOptions as CalculationOptionsHunter;
            calculatedStats.calcOpts = calcOpts;
            Stats stats = GetCharacterStats(character, additionalItem);
            HunterTalents talents = character.HunterTalents;

            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);
            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Hunter, character.Race);

            calculatedStats.BasicStats = stats;
            calculatedStats.BaseHealth = statsRace.Health;

            calculatedStats.pet = new PetCalculations(character, calculatedStats, calcOpts,
                                        statsBuffs, GetBuffsStats(calcOpts.petActiveBuffs), statsItems);
            
            if (character.Ranged == null || (character.Ranged.Item.Type != ItemType.Bow
                                             && character.Ranged.Item.Type != ItemType.Gun
                                             && character.Ranged.Item.Type != ItemType.Crossbow))
            {
                //skip all the calculations if there is no ranged weapon
                return calculatedStats;
            }

            int   levelDifI = calcOpts.TargetLevel - character.Level;
            float levelDifF = (float)levelDifI;

            // shot basics
            #region August 2009 Priority Rotation Setup

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

            #endregion
            #region August 2009 Shot Cooldowns & Durations

            calculatedStats.serpentSting.cooldown = 1.5f;
            calculatedStats.serpentSting.duration = talents.GlyphOfSerpentSting ? 21 : 15;

            calculatedStats.aimedShot.cooldown = talents.GlyphOfAimedShot ? 8 : 10;

            calculatedStats.explosiveShot.cooldown = 6;

            calculatedStats.chimeraShot.cooldown = talents.GlyphOfChimeraShot ? 9 : 10;

            calculatedStats.arcaneShot.cooldown = 6;

            calculatedStats.multiShot.cooldown = talents.GlyphOfMultiShot ? 9 : 10;

            calculatedStats.blackArrow.cooldown = 30 - (talents.Resourcefulness * 2);
            calculatedStats.blackArrow.duration = 15;

            calculatedStats.killShot.cooldown = talents.GlyphOfKillShot ? 9 : 15;

            calculatedStats.silencingShot.cooldown = 20;

            calculatedStats.scorpidSting.cooldown = 20;
            calculatedStats.scorpidSting.duration = 15;

            calculatedStats.viperSting.cooldown = 15;
            calculatedStats.viperSting.duration = 8;

            calculatedStats.immolationTrap.cooldown = 30 - talents.Resourcefulness * 2;
            calculatedStats.immolationTrap.duration = talents.GlyphOfImmolationTrap ? 9 : 15;

            if (calculatedStats.priorityRotation.containsShot(Shots.Readiness)) {
                calculatedStats.rapidFire.cooldown = 157.5f - (30f * talents.RapidKilling);
            } else {
                calculatedStats.rapidFire.cooldown = (5 - talents.RapidKilling) * 60f;
            }
            calculatedStats.rapidFire.duration = 15;

            // We will set the correct value for this later, after we've calculated haste
            calculatedStats.steadyShot.cooldown = 2;

            calculatedStats.immolationTrap.cooldown = 30 - (talents.Resourcefulness * 2);
            calculatedStats.immolationTrap.duration = talents.GlyphOfImmolationTrap ? 9 : 15;

            calculatedStats.readiness.cooldown = 180;

            calculatedStats.beastialWrath.cooldown = (talents.GlyphOfBestialWrath ? 100f : 120f) * (1f - talents.Longevity * 0.1f);
            calculatedStats.beastialWrath.duration = calcOpts.PetFamily == PetFamily.None ? 0 : 18;

            calculatedStats.bloodFury.cooldown = 120;
            calculatedStats.bloodFury.duration = 15;

            calculatedStats.berserk.cooldown = 180;
            calculatedStats.berserk.duration = 10;

            // We can calculate the rough frequencies now
            calculatedStats.priorityRotation.initializeTimings();
            if (!calcOpts.useRotationTest) {
                calculatedStats.priorityRotation.calculateFrequencies();
                calculatedStats.priorityRotation.calculateLALProcs(character);
                calculatedStats.priorityRotation.calculateFrequencies();
                calculatedStats.priorityRotation.calculateFrequencySums();
            }

            #endregion

            // speed
            #region August 2009 Ranged Weapon Stats

            float rangedWeaponDamage = 0;
            float rangedWeaponSpeed = 0;
            float rangedAmmoDPS = 0;

            if (character.Ranged != null) {
                rangedWeaponDamage = (float)(character.Ranged.Item.MinDamage + character.Ranged.Item.MaxDamage) / 2f;
                rangedWeaponSpeed = (float)Math.Round(character.Ranged.Item.Speed * 10f) / 10f;
            }
            if (character.Projectile != null) {
                rangedAmmoDPS = (float)(character.Projectile.Item.MaxDamage + character.Projectile.Item.MinDamage) / 2f;
            }

            #endregion
            #region August 2009 Static Haste Calcs
            // default quiver speed
            calculatedStats.hasteFromBase = 0.15f;
            // haste from haste rating
            calculatedStats.hasteFromRating = StatConversion.GetHasteFromRating(stats.HasteRating, character.Class);
            // serpent swiftness
            calculatedStats.hasteFromTalentsStatic = 0.04f * talents.SerpentsSwiftness;
            // haste buffs
            calculatedStats.hasteFromRangedBuffs = calculatedStats.BasicStats.RangedHaste;
            // total hastes
            calculatedStats.hasteStaticTotal = stats.PhysicalHaste;
            // Needed by the rotation test
            calculatedStats.autoShotStaticSpeed = rangedWeaponSpeed / (1f + calculatedStats.hasteStaticTotal);
            #endregion
            #region Rotation Test
            // Quick shots effect is needed for rotation test
            calculatedStats.quickShotsEffect = 0;
            if ((calcOpts.selectedAspect == Aspect.Hawk || calcOpts.selectedAspect == Aspect.Dragonhawk)
                && talents.ImprovedAspectOfTheHawk > 0)
            {
                float quickShotsEffect = 0.03f * talents.ImprovedAspectOfTheHawk;
                if (talents.GlyphOfTheHawk) { quickShotsEffect += 0.06f; }
                calculatedStats.quickShotsEffect = quickShotsEffect;
            }

            // Using the rotation test will get us better frequencies
            RotationTest rotationTest = new RotationTest(character, calculatedStats, calcOpts);

            if (calcOpts.useRotationTest) {
                // The following properties of CalculatedStats must be ready by this call:
                //  * priorityRotation (shot order, durations, cooldowns)
                //  * quickShotsEffect
                //  * hasteStaticTotal
                //  * autoShotStaticSpeed

                rotationTest.RunTest();
            }
            #endregion
            #region August 2009 Dynamic Haste Calcs
            // Now we have the haste, we can calculate steady shot cast time,
            // then rebuild other various stats.
            // (future enhancement: we only really need to rebuild steady shot)
            calculatedStats.steadyShot.cooldown = 2f / (1f + calculatedStats.hasteStaticTotal);
            if (calcOpts.useRotationTest) {
                calculatedStats.priorityRotation.initializeTimings();
                calculatedStats.priorityRotation.recalculateRatios();
                calculatedStats.priorityRotation.calculateFrequencySums();
            } else {
                calculatedStats.priorityRotation.initializeTimings();
                calculatedStats.priorityRotation.calculateFrequencies();
                calculatedStats.priorityRotation.calculateFrequencySums();
            }
            float autoShotSpeed = rangedWeaponSpeed / (1f + calculatedStats.hasteStaticTotal);
            #endregion

            // Hits
            #region Hit vs Miss Chance
            float ChanceToMiss = (float)Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[calcOpts.TargetLevel - character.Level] - stats.PhysicalHit);
            #endregion
            #region August 2009 Quick Shots
            float QSBaseFrequencyIncrease = 0;

            if (calcOpts.selectedAspect == Aspect.Hawk || calcOpts.selectedAspect == Aspect.Dragonhawk) {
                if (talents.ImprovedAspectOfTheHawk > 0) {
                    float quickShotsProcChance = 0.1f;

                    float quickShotsSpeed = autoShotSpeed / (1 + calculatedStats.quickShotsEffect);

                    float quickShotsInInitialProc = (autoShotSpeed > 0 ? (12f - autoShotSpeed) / quickShotsSpeed + 1f : 1f) * (1f - ChanceToMiss);
                    float quickShotsInReProc = (quickShotsSpeed > 0 ? 12f / quickShotsSpeed : 1f) * (1f - ChanceToMiss);

                    float quickShotsProcInitial = 1f - (float)Math.Pow(1f - quickShotsProcChance, quickShotsInInitialProc);
                    float quickShotsProcSubsequent = 1f - (float)Math.Pow(1f - quickShotsProcChance, quickShotsInReProc);

                    float quickShotsAvgShotsBeforeInit = 0;
                    if (quickShotsProcChance > 0 && quickShotsProcInitial > 0) {
                        quickShotsAvgShotsBeforeInit = ((1f - (float)Math.Pow(0.9f, quickShotsInInitialProc + 1f)) / 0.01f - (quickShotsInInitialProc + 1f) * (float)Math.Pow(0.9f, quickShotsInInitialProc) / 0.1f) / quickShotsProcInitial * 0.1f;
                    }
                        
                    float quickShotsAvgShotsBeforeNext = 0;
                    if (quickShotsProcChance > 0 && quickShotsProcSubsequent > 0) {
                        quickShotsAvgShotsBeforeNext = ((1f - (float)Math.Pow(0.9f, quickShotsInReProc + 1f)) / 0.01f - (quickShotsInReProc + 1f) * (float)Math.Pow(0.9f, quickShotsInReProc) / 0.1f) / quickShotsProcSubsequent * 0.1f;
                    }

                    float quickShotsAverageChainQuick = quickShotsInInitialProc * (1f - quickShotsProcInitial)
                                                       + quickShotsProcInitial * (1f - quickShotsProcSubsequent)
                                                       * (quickShotsAvgShotsBeforeNext * quickShotsProcSubsequent / ((float)Math.Pow(1f - quickShotsProcSubsequent, 2))
                                                       + (quickShotsAvgShotsBeforeInit + quickShotsInReProc) / (1f - quickShotsProcSubsequent));

                    float quickShotsAverageChainSlow = quickShotsProcChance > 0 ? 1f / quickShotsProcChance : 0f;

                    float quickShotsUptime;

                    if (calcOpts.useRotationTest) {
                        quickShotsUptime = rotationTest.IAotHUptime;
                    } else {
                        quickShotsUptime = quickShotsProcChance > 0 ? quickShotsAverageChainQuick / (quickShotsAverageChainQuick + quickShotsAverageChainSlow) : 0;
                    }

                    QSBaseFrequencyIncrease = autoShotSpeed > 0 ? (1f / quickShotsSpeed - 1f / autoShotSpeed) * quickShotsUptime : 0;
                }
            }

            #endregion
            #region August 2009 Shots Per Second
            float baseAutoShotsPerSecond = autoShotSpeed > 0 ? 1f / autoShotSpeed : 0;
            float autoShotsPerSecond = baseAutoShotsPerSecond + QSBaseFrequencyIncrease;
            float specialShotsPerSecond = calculatedStats.priorityRotation.specialShotsPerSecond;
            float totalShotsPerSecond = autoShotsPerSecond + specialShotsPerSecond;

            float crittingSpecialsPerSecond = calculatedStats.priorityRotation.critSpecialShotsPerSecond;
            float crittingShotsPerSecond = autoShotsPerSecond + crittingSpecialsPerSecond;

            float shotsPerSecondWithoutHawk = specialShotsPerSecond + baseAutoShotsPerSecond;

            calculatedStats.BaseAttackSpeed = (float)autoShotSpeed;
            calculatedStats.shotsPerSecondCritting = crittingShotsPerSecond;
            #endregion

            // Crits
            #region August 2009 Crit Chance
            calculatedStats.critBase = -0.0153f;
            calculatedStats.critFromAgi = StatConversion.GetCritFromAgility(stats.Agility, character.Class);
            calculatedStats.critFromRating = StatConversion.GetCritFromRating(calculatedStats.BasicStats.CritRating, character.Class);
            calculatedStats.critFromRacial = 0;
            if (character.Ranged != null &&
                ((character.Race == CharacterRace.Dwarf && character.Ranged.Item.Type == ItemType.Gun) ||
                (character.Race == CharacterRace.Troll && character.Ranged.Item.Type == ItemType.Bow)))
            {
                calculatedStats.critFromRacial = 0.01f;
            }

            // Simple talents
            calculatedStats.critFromLethalShots = talents.LethalShots * 0.01f;
            calculatedStats.critFromKillerInstincts = talents.KillerInstinct * 0.01f;
            calculatedStats.critFromMasterMarksman = talents.MasterMarksman * 0.01f;

            // Crit Depression
            float critdepression = StatConversion.NPC_LEVEL_CRIT_MOD[levelDifI];
            calculatedStats.critFromDepression = 0f - critdepression;

            calculatedStats.critRateOverall = stats.PhysicalCrit;

            #endregion
            #region August 2009 Bonus Crit Chance
            //Improved Barrage
            float improvedBarrageCritModifier = 0.04f * talents.ImprovedBarrage;

            // Survival instincts
            float survivalInstinctsCritModifier = 0.02f * talents.SurvivalInstincts;

            // Explosive Shot Glyph
            float glyphOfExplosiveShotCritModifier = talents.GlyphOfExplosiveShot ? 0.04f : 0;

            // Sniper Training
            float sniperTrainingCritModifier = talents.SniperTraining * 0.05f;

            //Trueshot Aura Glyph
            float trueshotAuraGlyphCritModifier = 0;
            if (talents.GlyphOfTrueshotAura == true) {
                if (talents.TrueshotAura > 0) {
                    trueshotAuraGlyphCritModifier = 0.1f;
                }
            }
            #endregion
            #region August 2009 Shot Crit Chances

            // crit chance = base_crit + 5%_rift_stalker_bonus + (2% * survivial_instincts)
            // to-maybe-do: add rift stalker set bonus
            float steadyShotCritChance = stats.PhysicalCrit + survivalInstinctsCritModifier;
            calculatedStats.steadyShot.critChance = steadyShotCritChance;

            // crit = base_crit + trueshot_aura_glyph + improved_barrage
            float aimedShotCrit = stats.PhysicalCrit + trueshotAuraGlyphCritModifier + improvedBarrageCritModifier;
            calculatedStats.aimedShot.critChance = aimedShotCrit;

            // crit = base_crit + glyph_of_es + survival_instincts
            float explosiveShotCrit = stats.PhysicalCrit + glyphOfExplosiveShotCritModifier + survivalInstinctsCritModifier;
            calculatedStats.explosiveShot.critChance = explosiveShotCrit;

            // 29-10-2009 Drizz: Adding the critchance to SerpentSting
            calculatedStats.serpentSting.critChance = stats.PhysicalCrit;

            calculatedStats.chimeraShot.critChance = stats.PhysicalCrit;

            float arcaneShotCrit = stats.PhysicalCrit + survivalInstinctsCritModifier;
            calculatedStats.arcaneShot.critChance = arcaneShotCrit;

            float multiShotCrit = stats.PhysicalCrit + improvedBarrageCritModifier;
            calculatedStats.multiShot.critChance = multiShotCrit;

            float killShotCrit = stats.PhysicalCrit + sniperTrainingCritModifier;
            calculatedStats.killShot.critChance = killShotCrit;

            calculatedStats.silencingShot.critChance = stats.PhysicalCrit;

            calculatedStats.priorityRotation.calculateCrits();

            #endregion

            // pet - part 1
            #region Pet MPS/Timing Calculations
            // this first block needs to run before the mana adjustments code,
            // since kill command effects mana usage.
            float baseMana = statsRace.Mana;
            calculatedStats.baseMana = statsRace.Mana;
            calculatedStats.pet.calculateTimings();
            #endregion

            // target debuffs
            #region Target Debuffs
            float targetDebuffsAP = 0; // Buffs!E77

            // The pet debuffs deal with stacking correctly themselves
            float targetDebuffsArmor = 1f - (1f - calculatedStats.petArmorDebuffs)
                                          * (1f - statsBuffs.ArmorPenetration); // Buffs!G77

            float targetDebuffsMP5JudgmentOfWisdom = 0;
            if (statsBuffs.ManaRestoreFromBaseManaPPM > 0)
            {
                // Note: we ignore the value stored in Buff.cs and calculate it as the spreadsheet
                // does, using shots per second and a derived 50% proc chance.                
                float jowAvgShotTime = autoShotsPerSecond + specialShotsPerSecond > 0f ? 1f / (autoShotsPerSecond + specialShotsPerSecond) : 0f;
                float jowProcChance = 0.5f;
                float jowTimeToProc = jowProcChance > 0 ? 0.25f + jowAvgShotTime / jowProcChance : 0f;
                float jowManaGained = statsRace.Mana * 0.02f;
                float jowMPSGained = jowTimeToProc > 0f ? jowManaGained / jowTimeToProc : 0f;
                targetDebuffsMP5JudgmentOfWisdom = jowTimeToProc > 0f ? jowManaGained / jowTimeToProc * 5f : 0f;
            }
            float targetDebuffsMP5 = targetDebuffsMP5JudgmentOfWisdom; // Buffs!H77

            float targetDebuffsFire = statsBuffs.BonusFireDamageMultiplier; // Buffs!I77
            float targetDebuffsArcane = statsBuffs.BonusArcaneDamageMultiplier; // Buffs!J77
            float targetDebuffsNature = statsBuffs.BonusNatureDamageMultiplier; // Buffs!K77
            float targetDebuffsShadow = statsBuffs.BonusShadowDamageMultiplier;

            float targetDebuffsPetDamage = statsBuffs.BonusPhysicalDamageMultiplier;

            calculatedStats.targetDebuffsArmor = 1f - targetDebuffsArmor;
            calculatedStats.targetDebuffsNature = 1f + targetDebuffsNature;
            calculatedStats.targetDebuffsPetDamage = 1f + targetDebuffsPetDamage;

            //29-10-2009 Drizz: For PiercingShots
            float targetDebuffBleed = statsBuffs.BonusBleedDamageMultiplier;
            #endregion

            // mana consumption
            #region August 2009 Mana Adjustments

            float efficiencyManaAdjust = 1f - (talents.Efficiency * 0.03f);

            float thrillOfTheHuntManaAdjust = 1f - (calculatedStats.priorityRotation.critsCompositeSum * 0.4f * (talents.ThrillOfTheHunt / 3f));

            float masterMarksmanManaAdjust = 1f - (talents.MasterMarksman * 0.05f);

            float glyphOfArcaneShotManaAdjust = 1f;
            if (calculatedStats.priorityRotation.containsShot(Shots.SerpentSting)
                || calculatedStats.priorityRotation.containsShot(Shots.ScorpidSting))
            {
                glyphOfArcaneShotManaAdjust = talents.GlyphOfArcaneShot ? 0.8f : 1f;
            }

            float resourcefullnessManaAdjust = 1f - talents.Resourcefulness * 0.2f;

            // Improved Steady Shot

            float ISSAimedShotManaAdjust = 1f;
            float ISSArcaneShotManaAdjust = 1f;
            float ISSChimeraShotManaAdjust = 1f;

            float ISSChimeraShotDamageAdjust = 1f;
            float ISSArcaneShotDamageAdjust = 1f;
            float ISSAimedShotDamageAdjust = 1f;

            float ISSProcChance = 0.05f * talents.ImprovedSteadyShot;
            if (ISSProcChance > 0f)
            {                
                if (calcOpts.useRotationTest)
                {
                    ISSChimeraShotDamageAdjust = 1f + rotationTest.ISSChimeraUptime * 0.15f;
                    ISSArcaneShotDamageAdjust  = 1f + rotationTest.ISSArcaneUptime * 0.15f;
                    ISSAimedShotDamageAdjust   = 1f + rotationTest.ISSAimedUptime * 0.15f;

                    ISSChimeraShotManaAdjust   = 1f - rotationTest.ISSChimeraUptime * 0.2f;
                    ISSArcaneShotManaAdjust    = 1f - rotationTest.ISSArcaneUptime * 0.2f;
                    ISSAimedShotManaAdjust     = 1f - rotationTest.ISSAimedUptime * 0.2f;
                }
                else
                {
                    float ISSRealProcChance = 0f; // N120
                    if (calculatedStats.steadyShot.freq > 0f)
                    {
                        float ISSSteadyFreq = calculatedStats.steadyShot.freq;
                        float ISSOtherFreq = calculatedStats.arcaneShot.freq
                                            + calculatedStats.chimeraShot.freq
                                            + calculatedStats.aimedShot.freq;

                        ISSRealProcChance = 1f - (float)Math.Pow(1f - ISSProcChance, ISSOtherFreq / ISSSteadyFreq);
                    }
                    float ISSProcFreqChimera = ISSRealProcChance > 0f ? calculatedStats.chimeraShot.freq / ISSRealProcChance : 0f; // N121
                    float ISSProcFreqArcane = ISSRealProcChance > 0f ? calculatedStats.arcaneShot.freq / ISSRealProcChance : 0f; // N122
                    float ISSProcFreqAimed = ISSRealProcChance > 0f ? calculatedStats.aimedShot.freq / ISSRealProcChance : 0f; // N123

                    float ISSProcFreqSumInverse = (ISSProcFreqChimera > 0f ? 1f / ISSProcFreqChimera : 0f)
                                                 + (ISSProcFreqArcane > 0f ? 1f / ISSProcFreqArcane : 0f)
                                                 + (ISSProcFreqAimed  > 0f ? 1f / ISSProcFreqAimed : 0f);
                    float ISSProcFreqCombined = ISSProcFreqSumInverse > 0f ? 1f / ISSProcFreqSumInverse : 0f; // N124

                    ISSChimeraShotDamageAdjust = ISSProcFreqChimera > 0f ? 1f + ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqChimera * 0.15f : 1f;
                    ISSArcaneShotDamageAdjust  = ISSProcFreqArcane  > 0f ? 1f + ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqArcane * 0.15f : 1f;
                    ISSAimedShotDamageAdjust   = ISSProcFreqAimed   > 0f ? 1f + ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqAimed * 0.15f : 1f;

                    ISSChimeraShotManaAdjust   = ISSProcFreqChimera > 0f ? 1f - ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqChimera * 0.2f : 1f;
                    ISSArcaneShotManaAdjust    = ISSProcFreqArcane  > 0f ? 1f - ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqArcane * 0.2f : 1f;
                    ISSAimedShotManaAdjust     = ISSProcFreqAimed   > 0f ? 1f - ISSRealProcChance * ISSProcFreqCombined / ISSProcFreqAimed * 0.2f : 1f;
                }
            }

            float resourcefulnessManaAdjust = 1f - (talents.Resourcefulness * 0.2f);

            #endregion
            #region August 2009 Shot Mana Usage

            // we do this ASAP so that we can get the MPS.
            // this allows us to calculate viper/aspect bonuses & penalties

            calculatedStats.steadyShot.mana = (baseMana * 0.05f) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust * masterMarksmanManaAdjust;
            calculatedStats.serpentSting.mana = (baseMana * 0.09f) * efficiencyManaAdjust;
            calculatedStats.aimedShot.mana = (baseMana * 0.08f) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust * masterMarksmanManaAdjust * ISSAimedShotManaAdjust;
            calculatedStats.explosiveShot.mana = (baseMana * 0.07f) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;
            calculatedStats.chimeraShot.mana = (baseMana * 0.12f) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust * masterMarksmanManaAdjust * ISSChimeraShotManaAdjust;
            calculatedStats.arcaneShot.mana = (baseMana * 0.05f) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust * ISSArcaneShotManaAdjust * glyphOfArcaneShotManaAdjust;
            calculatedStats.multiShot.mana = (baseMana * 0.09f) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;
            calculatedStats.blackArrow.mana = (baseMana * 0.06f) * efficiencyManaAdjust * resourcefulnessManaAdjust;
            calculatedStats.killShot.mana = (baseMana * 0.07f) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;
            calculatedStats.silencingShot.mana = (baseMana * 0.06f) * efficiencyManaAdjust * thrillOfTheHuntManaAdjust;
            calculatedStats.scorpidSting.mana = (baseMana * 0.11f) * efficiencyManaAdjust;
            calculatedStats.viperSting.mana = (baseMana * 0.08f) * efficiencyManaAdjust;
            calculatedStats.immolationTrap.mana = (baseMana * 0.13f) * resourcefullnessManaAdjust;
            calculatedStats.rapidFire.mana = (baseMana * 0.03f);

            calculatedStats.priorityRotation.calculateRotationMPS();

            #endregion
            #region August 2009 Mana Regen

            float mp5FromBuffs = statsItems.Mp5 + statsBuffs.Mp5;
            calculatedStats.manaRegenGearBuffs = mp5FromBuffs / 5;

            //29-10-2009 Drizz: TODO Probably missing T7 Viper bonus

            // Viper Regen if viper is up 100%
            calculatedStats.manaRegenConstantViper = 0;
            if (calcOpts.selectedAspect == Aspect.Viper)
            {
                float viperGlyphAdjust = talents.GlyphOfAspectOfTheViper ? 1.1f : 1;
                float viperRegenShots = calculatedStats.BasicStats.Mana * rangedWeaponSpeed / 100 * totalShotsPerSecond * viperGlyphAdjust;
                float viperRegenPassive = calculatedStats.BasicStats.Mana * 0.04f / 3;
                calculatedStats.manaRegenConstantViper = viperRegenShots + viperRegenPassive;
            }

            // Roar of Recovery - calculated in the pet model
            // calculatedStats.manaRegenRoarOfRecovery = 0;
            
            // Rapid Recuperation
            calculatedStats.manaRegenRapidRecuperation = 0;
            if (calculatedStats.rapidFire.freq > 0)
            {
                float rapidRecuperationManaGain = 0.02f * talents.RapidRecuperation * calculatedStats.BasicStats.Mana * 5;
                calculatedStats.manaRegenRapidRecuperation = rapidRecuperationManaGain / calculatedStats.rapidFire.freq;
            }

            // Chimera shot refreshing Viper
            calculatedStats.manaRegenChimeraViperProc = 0;
            if (calculatedStats.priorityRotation.chimeraRefreshesViper)
            {
                if (calculatedStats.chimeraShot.freq > 0)
                {
                    //29-10-2009 Drizz: Comment, 3092 is fetched from the Viper Sting Table on the SpellValues sheet (v92b). The 0.6 comes from ChimeraShotEffect.
                    calculatedStats.manaRegenChimeraViperProc = 0.6f * 3092 / calculatedStats.chimeraShot.freq;
                }
            }

            // Invigoration - this is being calculated in the pet model
            //calculatedStats.manaRegenInvigoration = 0;

            // Hunting Party
            float huntingPartyProc = (float)talents.HuntingParty / 3.0f;

            float huntingPartyArcaneFreq = calculatedStats.arcaneShot.freq;
            float huntingPartyArcaneCrit = calculatedStats.arcaneShot.critChance;
            float huntingPartyArcaneUptime = huntingPartyArcaneFreq > 0 ? 1 - (float)Math.Pow(1 - huntingPartyArcaneCrit * huntingPartyProc, 15 / huntingPartyArcaneFreq) : 0;

            float huntingPartyExplosiveFreq = calculatedStats.explosiveShot.freq; // spreadsheet divides by 3, but doesn't use that value?
            float huntingPartyExplosiveCrit = calculatedStats.explosiveShot.critChance;
            float huntingPartyExplosiveUptime = huntingPartyExplosiveFreq > 0 ? 1 - (float)Math.Pow(1 - huntingPartyExplosiveCrit * huntingPartyProc, 15 / huntingPartyExplosiveFreq) : 0;

            float huntingPartySteadyFreq = calculatedStats.steadyShot.freq;
            float huntingPartySteadyCrit = calculatedStats.steadyShot.critChance;
            float huntingPartySteadyUptime = huntingPartySteadyFreq > 0 ? 1 - (float)Math.Pow(1 - huntingPartySteadyCrit * huntingPartyProc, 15 / huntingPartySteadyFreq) : 0;

            float huntingPartyCumulativeUptime = huntingPartyArcaneUptime + ((1 - huntingPartyArcaneUptime) * huntingPartyExplosiveUptime);
            float huntingPartyUptime = huntingPartyCumulativeUptime + ((1 - huntingPartyCumulativeUptime) * huntingPartySteadyUptime);

            calculatedStats.manaRegenHuntingParty = 0.002f * calculatedStats.BasicStats.Mana * huntingPartyUptime;

            // If we've got a replenishment buff up, use that instead of our own Hunting Party
            float manaRegenReplenishment = statsBuffs.ManaRestoreFromMaxManaPerSecond * calculatedStats.BasicStats.Mana;
            if (manaRegenReplenishment > 0)
            {
                calculatedStats.manaRegenHuntingParty = manaRegenReplenishment;
            }

            // Target Debuffs
            calculatedStats.manaRegenTargetDebuffs = targetDebuffsMP5 / 5;

            // Total
            calculatedStats.manaRegenTotal =
                calculatedStats.manaRegenGearBuffs +
                calculatedStats.manaRegenConstantViper +
                calculatedStats.manaRegenRoarOfRecovery +
                calculatedStats.manaRegenRapidRecuperation +
                calculatedStats.manaRegenChimeraViperProc +
                calculatedStats.manaRegenInvigoration +
                calculatedStats.manaRegenHuntingParty +
                calculatedStats.manaRegenTargetDebuffs;


            #endregion
            #region August 2009 Aspect Usage

            float manaRegenTier7ViperBonus = stats.BonusHunter_T7_4P_ViperSpeed > 0 ? 1.2f : 1f;

            float glpyhOfAspectOfTheViperBonus = talents.GlyphOfAspectOfTheViper ? 1.1f : 1f;

            // 021109 - Drizz: Comment
            // From spreadsheet
            // =FinalMana*RangedWeaponSpeed/100*ShotsPerSecondWithoutHawk*IF(T7ViperBonus,1.2,1)*...
            // ...IF(COUNTIF(GlyphsUsed,"Glyph of Aspect of the Viper"),1+...
            // ...VLOOKUP("Glyph of Aspect of the Viper",GlyphMatrix,3,FALSE),1)+FinalMana*0.04/3

            calculatedStats.manaRegenViper = calculatedStats.BasicStats.Mana * (float)Math.Round(rangedWeaponSpeed, 1) / 100f * shotsPerSecondWithoutHawk
                                        * manaRegenTier7ViperBonus * glpyhOfAspectOfTheViperBonus
                                        + calculatedStats.BasicStats.Mana * 0.04f / 3f;

            float manaFromPotion = 0;
            if (calcOpts.useManaPotion == ManaPotionType.RunicManaPotion) manaFromPotion = 4300;
            if (calcOpts.useManaPotion == ManaPotionType.SuperManaPotion) manaFromPotion = 2400;

            bool manaHasAlchemistStone = false;
            //if (IsWearingTrinket(character, 35751)) manaHasAlchemistStone = true; // Assassin's Alchemist Stone
            //if (IsWearingTrinket(character, 44324)) manaHasAlchemistStone = true; // Mighty Alchemist's Stone

            calculatedStats.manaRegenPotion = manaFromPotion / calcOpts.Duration * (manaHasAlchemistStone ? 1.4f : 1.0f);

            float beastialWrathUptime = calculatedStats.beastialWrath.freq > 0 ? calculatedStats.beastialWrath.duration / calculatedStats.beastialWrath.freq : 0;

            float beastWithinManaBenefit = beastialWrathUptime * 0.2f;

            calculatedStats.manaUsageKillCommand = calculatedStats.petKillCommandMPS * (1 - beastWithinManaBenefit);
            calculatedStats.manaUsageRotation = calculatedStats.priorityRotation.MPS;

            calculatedStats.manaUsageTotal = calculatedStats.manaUsageRotation
                                           + calculatedStats.manaUsageKillCommand;

            calculatedStats.manaChangeDuringViper = calculatedStats.manaRegenViper + calculatedStats.manaRegenPotion + calculatedStats.manaRegenTotal - calculatedStats.manaUsageTotal;
            calculatedStats.manaChangeDuringNormal = calculatedStats.manaRegenTotal + calculatedStats.manaRegenPotion - calculatedStats.manaUsageTotal;

            calculatedStats.manaTimeToFull = calculatedStats.manaChangeDuringViper > 0 ? calculatedStats.BasicStats.Mana / calculatedStats.manaChangeDuringViper : -1;
            calculatedStats.manaTimeToOOM = calculatedStats.manaChangeDuringNormal < 0 ? calculatedStats.BasicStats.Mana / (0-calculatedStats.manaChangeDuringNormal) : -1;

            float viperTimeNeededToLastFight = 0;
            if (calculatedStats.manaTimeToOOM >= 0 && calculatedStats.manaTimeToOOM < calcOpts.Duration && calculatedStats.manaRegenViper > 0)
            {
                viperTimeNeededToLastFight = (((0 - calculatedStats.manaChangeDuringNormal) * calcOpts.Duration) - calculatedStats.BasicStats.Mana) / calculatedStats.manaRegenViper;
            }

            float aspectUptimeHawk = 0;

            float aspectUptimeViper = 0;
            if (calculatedStats.manaTimeToOOM >= 0 && calcOpts.aspectUsage != AspectUsage.AlwaysOn)
            {
                if (calcOpts.aspectUsage == AspectUsage.ViperRegen)
                {
                    aspectUptimeViper = calculatedStats.manaTimeToFull / (calculatedStats.manaTimeToFull + calculatedStats.manaTimeToOOM);
                }
                else
                {
                    if (viperTimeNeededToLastFight > 0)
                    {
                        aspectUptimeViper = viperTimeNeededToLastFight / calcOpts.Duration;
                    }
                }
            }

            float aspectUptimeBeast = calcOpts.useBeastDuringBeastialWrath ? beastialWrathUptime : 0;

            switch (calcOpts.selectedAspect)
            {
                case Aspect.Viper:
                    aspectUptimeViper = calcOpts.useBeastDuringBeastialWrath ? 1 - aspectUptimeBeast : 1;
                    break;

                case Aspect.Beast:
                    aspectUptimeBeast = (calcOpts.aspectUsage == AspectUsage.AlwaysOn) ? 1 : 1 - aspectUptimeViper;
                    break;

                case Aspect.Hawk:
                case Aspect.Dragonhawk:
                    aspectUptimeHawk = 1 - aspectUptimeViper - aspectUptimeBeast;
                    break;
            }


            // we now know aspect uptimes - calculate bonuses and penalties

            float viperDamageEffect = talents.AspectMastery == 1 ? 0.4f : 0.5f;
            float viperDamagePenalty = aspectUptimeViper * viperDamageEffect;

            float beastStaticAPBonus = talents.GlyphOfTheBeast ? 0.12f : 0.1f;
            float beastAPBonus = aspectUptimeBeast * beastStaticAPBonus;

            float tier7ViperDamageAdjust = 1.0f + (character.ActiveBuffsContains("Cryptstalker Battlegear 4 Piece Bonus") ? 0.2f * aspectUptimeViper : 0);

            calculatedStats.aspectUptimeHawk = aspectUptimeHawk;
            calculatedStats.aspectUptimeBeast = aspectUptimeBeast;
            calculatedStats.aspectUptimeViper = aspectUptimeViper;
            calculatedStats.aspectViperPenalty = viperDamagePenalty;
            calculatedStats.aspectBonusAPBeast = beastAPBonus;

            #endregion

            // damage
            #region August 2009 Ranged Attack Power

            calculatedStats.apFromBase = 0.0f + character.Level * 2f;
            calculatedStats.apFromAgil = 0.0f + stats.Agility - 10f;
            calculatedStats.apFromCarefulAim = (float)Math.Floor((talents.CarefulAim / 3f) * (calculatedStats.BasicStats.Intellect));
            calculatedStats.apFromHunterVsWild = (float)Math.Floor((talents.HunterVsWild * 0.1f) * (calculatedStats.BasicStats.Stamina));
            calculatedStats.apFromGear = 0.0f + calculatedStats.BasicStats.AttackPower;

            // Darkmoon Card: Crusade
            //if (IsWearingTrinket(character, 31856)) calculatedStats.apFromGear += 120;

            /* Orc Racial is now handled in SpecialEffects, the effect is added in BaseStats
            calculatedStats.apFromBloodFury = 0;
            if (character.Race == CharacterRace.Orc && calculatedStats.bloodFury.freq > 0) {
                calculatedStats.apFromBloodFury = (4f * character.Level) + 2f;
                calculatedStats.apFromBloodFury *= CalcUptime(calculatedStats.bloodFury.duration, calculatedStats.bloodFury.freq, calcOpts);
            }*/

            // Aspect of the Hawk
            calculatedStats.apFromAspectOfTheHawk = 0;
            if (calcOpts.selectedAspect == Aspect.Hawk || calcOpts.selectedAspect == Aspect.Dragonhawk) {
                calculatedStats.apFromAspectOfTheHawk = 300f * aspectUptimeHawk;
            }

            // Aspect Mastery
            calculatedStats.apFromAspectMastery = 0;
            if (talents.AspectMastery > 0) {
                calculatedStats.apFromAspectMastery = calculatedStats.apFromAspectOfTheHawk * 0.3f * aspectUptimeHawk;
            }

            // Furious Howl was calculated earlier by the pet model
            //calculatedStats.apFromFuriousHowl = 0;

            // Expose Weakness
            float exposeWeaknessShotsPerSecond = crittingShotsPerSecond;
            float exposeWeaknessCritChance = calculatedStats.priorityRotation.critsCompositeSum;
            float exposeWeaknessAgility = stats.Agility * 0.25f;
            float exposeWeaknessPercent = (talents.ExposeWeakness / 3f);
            float exposeWeaknessUptime = 1f - (float)Math.Pow(1f - (exposeWeaknessPercent * exposeWeaknessCritChance), 7 * exposeWeaknessShotsPerSecond);

            calculatedStats.apFromExposeWeakness = exposeWeaknessUptime * exposeWeaknessAgility;

            // CallOfTheWild - this is calculated in the pet model
            //calculatedStats.apFromCallOfTheWild = 0;

            calculatedStats.apFromTrueshotAura = (0.1f * talents.TrueshotAura);
            calculatedStats.apFromBuffs = 0;
            if (talents.TrueshotAura == 0) {
                calculatedStats.apFromBuffs = stats.BonusAttackPowerMultiplier;
            }

            calculatedStats.apFromHuntersMark = 500;
            calculatedStats.apFromHuntersMark += 500f * 0.1f * talents.ImprovedHuntersMark;
            if (talents.GlyphOfHuntersMark == true)
            {
                calculatedStats.apFromHuntersMark += 0.2f * 500f;
            }


            calculatedStats.apFromDebuffs = targetDebuffsAP;

            // debuffs may contain hunters mark too - only count the debuff if it's worth more
            // than our mark is
            float apFromHuntersMarkDebuff = 0;
            if (character.ActiveBuffsContains("Hunter's Mark")) apFromHuntersMarkDebuff += 500;
            if (character.ActiveBuffsContains("Glyphed Hunter's Mark")) apFromHuntersMarkDebuff += 100;
            if (character.ActiveBuffsContains("Improved Hunter's Mark")) apFromHuntersMarkDebuff += 150;
            if (character.ActiveBuffsContains("Improved and Glyphed Hunter's Mark")) apFromHuntersMarkDebuff += 250;
            calculatedStats.apFromGear -= apFromHuntersMarkDebuff;

            if (apFromHuntersMarkDebuff > calculatedStats.apFromHuntersMark) {
                // the hunters mark buff (not ours) is better than ours.
                // add the difference as a target debuff
                calculatedStats.apFromDebuffs += apFromHuntersMarkDebuff - calculatedStats.apFromHuntersMark;
            }

            calculatedStats.apFromProc = 0;

            // Tier-8 4-set bonus
            if (character.ActiveBuffsContains("Scourgestalker Battlegear 4 Piece Bonus")) {
                calculatedStats.apFromProc += 600f * CalcTrinketUptime(15f, 45f, 0.1f, calculatedStats.steadyShot.freq > 0 ? 1f / calculatedStats.steadyShot.freq : 0);
            }

            // TODO: add multiplicitive buffs
            float apScalingFactor = 1f
                * (1f + calculatedStats.apFromCallOfTheWild)
                * (1f + calculatedStats.apFromTrueshotAura)
                * (1f + calculatedStats.apFromBuffs);

            // use for pet calculations
            calculatedStats.apSelfBuffed = 0f
                + calculatedStats.apFromBase
                + calculatedStats.apFromAgil
                + calculatedStats.apFromCarefulAim
                + calculatedStats.apFromHunterVsWild
                + calculatedStats.apFromGear // includes buffs
                + calculatedStats.apFromBloodFury
                + calculatedStats.apFromAspectOfTheHawk
                + calculatedStats.apFromAspectMastery
                + calculatedStats.apFromFuriousHowl
                + calculatedStats.apFromProc;

            // used for hunter calculations
            calculatedStats.apTotal = calculatedStats.apSelfBuffed
                                    + calculatedStats.apFromExposeWeakness
                                    + calculatedStats.apFromDebuffs
                                    + calculatedStats.apFromHuntersMark;

            // apply scaling
            calculatedStats.apTotal      *= apScalingFactor;
            calculatedStats.apSelfBuffed *= apScalingFactor;

            float RAP = calculatedStats.apTotal;

            #endregion
            #region August 2009 Armor Penetration
            float ArmorDamageReduction = GetArmorDamageReduction(calcOpts, character, stats);
            calculatedStats.damageReductionFromArmor = (1f - ArmorDamageReduction);
            #endregion
            #region August 2009 Damage Adjustments

            //Partial Resists
            float averageResist = (calcOpts.TargetLevel - 80) * 0.02f;
            float resist10 = 5 * averageResist;
            float resist20 = 2.5f * averageResist;
            float partialResistDamageAdjust = 1 - (resist10 * 0.1f + resist20 * 0.2f);

            //Beast Within
            float beastWithinDamageAdjust = 1;
            if (calculatedStats.beastialWrath.freq > 0)
            {
                beastWithinDamageAdjust = 1 + (0.1f * beastialWrathUptime);
            }            

            //Focused Fire
            float focusedFireDamageAdjust = 1 + 0.01f * talents.FocusedFire;

            //Sanc. Retribution Aura
            float sancRetributionAuraDamageAdjust = 1 + statsBuffs.BonusDamageMultiplier;

            //Black Arrow Damage Multiplier
            float blackArrowUptime = 0;
            if (calculatedStats.priorityRotation.containsShot(Shots.BlackArrow))
            {
                SpecialEffect blackarrow = new SpecialEffect(Trigger.Use,new Stats(),
                                            calculatedStats.blackArrow.duration, calculatedStats.blackArrow.freq);
                blackArrowUptime = blackarrow.GetAverageUptime(0f, 1f, calculatedStats.autoShotStaticSpeed, (float)calcOpts.Duration);
            }
            float blackArrowAuraDamageAdjust = 1f + (0.06f * blackArrowUptime);
            float blackArrowSelfDamageAdjust = 1f + (RAP / 225000f);

            //Noxious Stings
            float noxiousStingsSerpentUptime = 0;
            if (calculatedStats.serpentSting.freq > 0) noxiousStingsSerpentUptime = calculatedStats.serpentSting.duration / calculatedStats.serpentSting.freq;
            if (calculatedStats.priorityRotation.chimeraRefreshesSerpent) noxiousStingsSerpentUptime = 1;
            float noxiousStingsDamageAdjust = 1f + (0.01f * talents.NoxiousStings * noxiousStingsSerpentUptime);
            float noxiousStingsSerpentDamageAdjust = 1f + (0.01f * talents.NoxiousStings);

            //Ferocious Inspiration (calculated by pet model)
            float ferociousInspirationDamageAdjust = calculatedStats.ferociousInspirationDamageAdjust;
            float ferociousInspirationArcaneDamageAdjust = 1f + (0.03f * talents.FerociousInspiration);

            //Improved Tracking
            float improvedTrackingDamageAdjust = 1f + 0.01f * talents.ImprovedTracking;

            //Ranged Weapon Specialization
            float rangedWeaponSpecializationDamageAdjust = 1;
            if (talents.RangedWeaponSpecialization == 1) rangedWeaponSpecializationDamageAdjust = 1.01f;
            if (talents.RangedWeaponSpecialization == 2) rangedWeaponSpecializationDamageAdjust = 1.03f;
            if (talents.RangedWeaponSpecialization == 3) rangedWeaponSpecializationDamageAdjust = 1.05f;

            //Marked For Death (assume hunter's mark is on target)
            float markedForDeathDamageAdjust = 1f + 0.01f * talents.MarkedForDeath;

            //DamageTakenDebuffs
            float targetPhysicalDebuffsDamageAdjust = (1f + statsBuffs.BonusPhysicalDamageMultiplier);

            //Barrage
            float barrageDamageAdjust = 1f + 0.04f * talents.Barrage;

            //Sniper Training
            float sniperTrainingDamageAdjust = 1 + 0.02f * talents.SniperTraining;

            //Improve Stings
            float improvedStingsDamageAdjust = 1 + 0.1f * talents.ImprovedStings;

            //TrapMastery
            float trapMasteryDamageAdjust = 1 + 0.1f * talents.TrapMastery;

            // T.N.T.
            float TNTDamageAdjust = 1 + 0.02f * talents.TNT;


            // These intermediates group the two common sets of adjustments
            float talentDamageAdjust = focusedFireDamageAdjust
                                            * beastWithinDamageAdjust
                                            * sancRetributionAuraDamageAdjust
                                            * blackArrowAuraDamageAdjust
                                            * noxiousStingsDamageAdjust
                                            * ferociousInspirationDamageAdjust
                                            * improvedTrackingDamageAdjust
                                            * rangedWeaponSpecializationDamageAdjust
                                            * markedForDeathDamageAdjust;

            float talentDamageStingAdjust = focusedFireDamageAdjust
                                            * beastWithinDamageAdjust
                                            * sancRetributionAuraDamageAdjust
                                            * blackArrowAuraDamageAdjust
                                            * noxiousStingsDamageAdjust
                                            * ferociousInspirationDamageAdjust;

            #endregion
            #region August 2009 Bonus Crit Damage

            //MortalShots
            float mortalShotsCritDamage = 0.06f * talents.MortalShots;

            //CritDamageMetaGems
            float metaGemCritDamage = 1 + (statsItems.BonusCritMultiplier * 2);

            //Marked For Death
            float markedForDeathCritDamage = 0.02f * talents.MarkedForDeath;

            float baseCritDamage = (1 + mortalShotsCritDamage) * metaGemCritDamage; // CriticalHitDamage
            float specialCritDamage = (1 + mortalShotsCritDamage + markedForDeathCritDamage) * metaGemCritDamage; // SpecialCritDamage

            #endregion

            // pet - part 2
            #region Pet DPS Calculations

            calculatedStats.pet.calculateDPS();

            #endregion

            // shot damage calcs
            #region August 2009 AutoShot

            // scope damage only applies to autoshot, so is not added to the normalized damage
            float rangedAmmoDamage           = rangedAmmoDPS * rangedWeaponSpeed;
            float rangedAmmoDamageNormalized = rangedAmmoDPS * 2.8f;

            float damageFromRAP           = RAP / 14f * rangedWeaponSpeed;
            float damageFromRAPNormalized = RAP / 14f * 2.8f;

            float autoShotDamage = rangedWeaponDamage
                                 + rangedAmmoDamage
                                 + statsItems.WeaponDamage
                                 + damageFromRAP
                                 + calculatedStats.BasicStats.ScopeDamage;
            float autoShotDamageNormalized = rangedWeaponDamage
                                           + rangedAmmoDamageNormalized
                                           + statsItems.WeaponDamage
                                           + damageFromRAPNormalized;

            float autoShotDamageAdjust = talentDamageAdjust
                                       * targetPhysicalDebuffsDamageAdjust
                                       * ArmorDamageReduction;
            float autoShotCritAdjust = 1f * metaGemCritDamage;

            float autoShotDamageReal = CalcEffectiveDamage(autoShotDamage,
                                                           ChanceToMiss,
                                                           stats.PhysicalCrit,
                                                           autoShotCritAdjust,
                                                           autoShotDamageAdjust);

            float hunterAutoDPS = autoShotsPerSecond
                                * autoShotDamageReal
                                * (1f - viperDamagePenalty)
                                * tier7ViperDamageAdjust;

            calculatedStats.aspectBeastLostDPS = (0f - QSBaseFrequencyIncrease) * (1f - aspectUptimeHawk) * hunterAutoDPS;

            calculatedStats.AutoshotDPS = hunterAutoDPS;

            #endregion
            #region August 2009 Wild Quiver

            calculatedStats.WildQuiverDPS = 0;
            if (talents.WildQuiver > 0)
            {
                float wildQuiverProcChance = talents.WildQuiver * 0.04f;
                float wildQuiverProcFrequency = (autoShotSpeed / wildQuiverProcChance);
                float wildQuiverDamageNormal = 0.8f * (rangedWeaponDamage + statsItems.WeaponDamage + damageFromRAP);
                float wildQuiverDamageAdjust = talentDamageAdjust * partialResistDamageAdjust * (1f + targetDebuffsNature);

                float wildQuiverDamageReal = CalcEffectiveDamage(
                                                wildQuiverDamageNormal,
                                                ChanceToMiss,
                                                stats.PhysicalCrit,
                                                1f,
                                                wildQuiverDamageAdjust
                                              );

                //29-10-2009 Drizz: Added the ViperUpTIme penalty
                calculatedStats.WildQuiverDPS = (wildQuiverDamageReal / wildQuiverProcFrequency) * (1f - viperDamagePenalty);
            }

            #endregion
            #region August 2009 Steady Shot

            // base = shot_base + gear_weapon_damage + normalized_ammo_dps + (RAP * 0.1)
            //        + (rangedWeaponDamage / ranged_weapon_speed * 2.8)
            float steadyShotDamageNormal = 252
                        + statsItems.WeaponDamage
                        + rangedAmmoDamageNormalized
                        + (RAP * 0.1f)
                        + (rangedWeaponDamage / rangedWeaponSpeed * 2.8f);
            // adjust = talent_adjust * gronnstalker_bonus * glyph_of_steadyshot
            //          * sniper_training * physcial_debuffs
            // to-maybe-do: Gronnstalker set bonus
            float steadyShotDamageAdjust = talentDamageAdjust
                                            * targetPhysicalDebuffsDamageAdjust
                                            * sniperTrainingDamageAdjust
                                            * (talents.GlyphOfSteadyShot ? 1.1f : 1f);

            // ****************************************************************************
            // Drizz: 31-10-2009 Aligned the calculations with spreadsheet v92b
            // Also moved the armorReduction adjust to be multiplied after DamageReal Calc
            // Corrected from Spreadsheet changelog 91e "T9 2-set bonus only crits for spell-crit bonus damage (i.e. 50% instead of 100%), not affected by Mortal Shots"
            // This is the reason for the 0.5 multiplier and that markedForDeath is kept outside
            float steadyShotCritAdjust = metaGemCritDamage + 0.5f * mortalShotsCritDamage * (1f + metaGemCritDamage) + markedForDeathCritDamage;

            float steadyShotDamageReal = CalcEffectiveDamage(
                                            steadyShotDamageNormal,
                                            ChanceToMiss,
                                            steadyShotCritChance,
                                            steadyShotCritAdjust,
                                            steadyShotDamageAdjust
                                          );

            steadyShotDamageReal *= ArmorDamageReduction;

            calculatedStats.steadyShot.damage = steadyShotDamageReal;
            // ****************************************************************************

            //29-10-2009 Drizz: Added for PiercingShots
            float steadyShotAvgNonCritDamage = steadyShotDamageNormal * steadyShotDamageAdjust * ArmorDamageReduction;
            float steadyShotAvgCritDamage = steadyShotAvgNonCritDamage * (1f + steadyShotCritAdjust);
            //021109 Drizz: Have to add the Mangle/Trauma buff effect. 
            float steadyShotPiercingShots = (1f + targetDebuffBleed)
                                          * (character.HunterTalents.PiercingShots * 0.1f)
                                          * steadyShotCritChance * steadyShotAvgCritDamage;

            //Drizz: Add the piercingShots effect
            calculatedStats.steadyShot.damage = steadyShotDamageReal + steadyShotPiercingShots;

            #endregion
            #region August 2009 Serpent Sting

            // base_damage = 1210 + (0.2 * RAP)
            float serpentStingDamageBase = (float)Math.Round(1210 + (RAP * 0.2f), 1);

            // T9 2-piece bonus
            float serpentStingT9CritAdjust = 1;
            float serpentStingInterimBonus;
            float serpentStingCriticalHitDamage;
            // 29-10-2009 Drizz: The name in the buff have not switched from Battlegear (i.e. the name is of the Horde buff)
            // if (character.ActiveBuffsContains("Windrunner's Pursuit 2 Piece Bonus"))
            if (character.ActiveBuffsContains("Windrunner's Battlegear 2 Piece Bonus"))
            {
                // Drizz : aligned with v92b
                serpentStingInterimBonus = 0.5f + 0.5f * mortalShotsCritDamage + 0.5f;
                serpentStingCriticalHitDamage = serpentStingInterimBonus * (1f + (1f + 0.5f) * (metaGemCritDamage - 1f) / 2f + (1f + 0.5f) * (metaGemCritDamage - 1) / 2);
                serpentStingT9CritAdjust = 1f + calculatedStats.critRateOverall * serpentStingCriticalHitDamage;
            }

            double serpentStingCritAdjustment = serpentStingT9CritAdjust;

            // damage_adjust = (sting_talent_adjusts ~ noxious stings) * improved_stings * improved_tracking
            //                  + partial_resists * tier-8_2-piece_bonus * target_nature_debuffs * 100%_noxious_stings
            float serpentStingDamageAdjust = focusedFireDamageAdjust
                                                * beastWithinDamageAdjust
                                                * sancRetributionAuraDamageAdjust
                                                * blackArrowAuraDamageAdjust
                                                * ferociousInspirationDamageAdjust
                                                * noxiousStingsSerpentDamageAdjust
                                                * improvedStingsDamageAdjust
                                                * improvedTrackingDamageAdjust
                                                * partialResistDamageAdjust
                                                * serpentStingT9CritAdjust
                                                * (1f + targetDebuffsNature);

            // T8 2-piece bonus
            serpentStingDamageAdjust += statsBuffs.BonusHunter_T8_2P_SerpDmg;

            float serpentStingTicks = calculatedStats.serpentSting.duration / 3f;
            float serpentStingDamagePerTick = (float)Math.Round(serpentStingDamageBase * serpentStingDamageAdjust / 5f, 1);
            float serpentStingDamageReal = serpentStingDamagePerTick * serpentStingTicks;

            calculatedStats.serpentSting.type = Shots.SerpentSting;
            calculatedStats.serpentSting.damage = serpentStingDamageReal;

            #endregion
            #region August 2009 Aimed Shot
            // ****************************************************************************
            // Drizz: 31-10-2009 Aligned the calculations with spreadsheet v92b
            // Also moved the armorReduction adjust to be multiplied after DamageReal Calc

            // base_damage = normalized_shot + 408 (but ammo is not normalized!)
            float aimedShotDamageNormal = (rangedWeaponDamage + rangedAmmoDamage + statsItems.WeaponDamage + damageFromRAPNormalized) + 408f;

            // Corrected from Spreadsheet changelog 91e "T9 2-set bonus only crits for spell-crit bonus damage (i.e. 50% instead of 100%), not affected by Mortal Shots"
            // This is the reason for the 0.5 multiplier and that markedForDeath is kept outside
            float aimedShotCritAdjust = metaGemCritDamage + 0.5f * mortalShotsCritDamage * (1f + metaGemCritDamage) + markedForDeathCritDamage;

            // damage_adjust = talent_adjust * barrage_adjust * target_debuff_adjust * sniper_training_adjust * improved_ss_adjust
            float aimedShotDamageAdjust = talentDamageAdjust * barrageDamageAdjust * targetPhysicalDebuffsDamageAdjust
                                            * sniperTrainingDamageAdjust * ISSAimedShotDamageAdjust;

            float aimedShotDamageReal = CalcEffectiveDamage(
                                            aimedShotDamageNormal,
                                            ChanceToMiss,
                                            aimedShotCrit,
                                            aimedShotCritAdjust,
                                            aimedShotDamageAdjust
                                          );

            calculatedStats.aimedShot.damage = aimedShotDamageReal;

            aimedShotDamageReal *= ArmorDamageReduction;

            //Drizz: added for piercing shots
            float aimedShotAvgNonCritDamage = aimedShotDamageNormal * aimedShotDamageAdjust * ArmorDamageReduction;
            float aimedShotAvgCritDamage = aimedShotAvgNonCritDamage * (1f + aimedShotCritAdjust);
            //021109 Drizz: Have to add the Mangle/Trauma buff effect. 
            float aimedShotPiercingShots = (1f + targetDebuffBleed)
                                         * (character.HunterTalents.PiercingShots * 0.1f)
                                         * aimedShotCrit * aimedShotAvgCritDamage;

            //Drizz: Trying out...
            calculatedStats.aimedShot.damage = aimedShotDamageReal + aimedShotPiercingShots;

            #endregion
            #region August 2009 Explosive Shot

            // base_damage = 425 + 14% of RAP
            float explosiveShotDamageNormal = 425f + (RAP * 0.14f);

            // crit_damage = 1 + mortal_shots + gem-crit
            float explosiveShotCritAdjust = (1f + mortalShotsCritDamage) * metaGemCritDamage;

            // damage_adjust = talent_adjust * tnt * fire_debuffs * sinper_training * partial_resist
            float explosiveShotDamageAdjust = talentDamageAdjust * TNTDamageAdjust * sniperTrainingDamageAdjust
                                             * partialResistDamageAdjust * (1f + targetDebuffsFire);

            float explosiveShotDamageReal = CalcEffectiveDamage(
                                                explosiveShotDamageNormal,
                                                ChanceToMiss,
                                                explosiveShotCrit,
                                                explosiveShotCritAdjust,
                                                explosiveShotDamageAdjust
                                              );

            float explosiveShotDamagePerShot = explosiveShotDamageReal * 3f;

            calculatedStats.explosiveShot.damage = explosiveShotDamagePerShot;

            #endregion
            #region August 2009 Chimera Shot

            // base_damage = normalized_autoshot * 125%
            //float chimeraShotDamageNormal = autoShotDamageNormalized * 1.25f;
            // Drizz: Making Changes
            float chimeraShotDamageNormal = (rangedAmmoDamage + (RAP / 14f * 2.8f) + rangedWeaponDamage) * 1.25f;

            // Drizz: In the spreadsheet there is also added a row for + Weapon Damage Gear... not included here.

            // Drizz: 
            // Corrected from Spreadsheet changelog 91e "T9 2-set bonus only crits for spell-crit bonus damage (i.e. 50% instead of 100%), not affected by Mortal Shots"
            // This is the reason for the 0.5 multiplier and that markedForDeath is kept outside
            float chimeraShotCritAdjust = metaGemCritDamage + 0.5f * mortalShotsCritDamage * (1f + metaGemCritDamage) + markedForDeathCritDamage;

            // damage_adjust = talent_adjust * nature_debuffs * ISS_cs_bonus * partial_resist
            float chimeraShotDamageAdjust = talentDamageAdjust * ISSChimeraShotDamageAdjust
                                           * partialResistDamageAdjust * (1f + targetDebuffsNature);

            float chimeraShotDamageReal = CalcEffectiveDamage(
                                                chimeraShotDamageNormal,
                                                ChanceToMiss,
                                                stats.PhysicalCrit,
                                                chimeraShotCritAdjust,
                                                chimeraShotDamageAdjust
                                           );

            //Drizz: added for piercing shots
            float chimeraShotAvgNonCritDamage = chimeraShotDamageNormal * talentDamageAdjust * ISSChimeraShotDamageAdjust * (1f + targetDebuffsNature);
            float chimeraShotAvgCritDamage = chimeraShotAvgNonCritDamage * (1f + chimeraShotCritAdjust);
            // 021109 - Drizz: Had to add the Bleed Damage Multiplier
            float chimeraShotPiercingShots = (1f + targetDebuffBleed)
                                           * (character.HunterTalents.PiercingShots * 0.1f)
                                           * calculatedStats.critRateOverall
                                           * chimeraShotAvgCritDamage;

            calculatedStats.chimeraShot.damage = chimeraShotDamageReal + chimeraShotPiercingShots;

            // calculate damage from serpent sting
            // Drizz: Adding
            float chimeraShotSerpentMultiplier = improvedStingsDamageAdjust
                                                  * improvedTrackingDamageAdjust
                                                  * noxiousStingsDamageAdjust
                                                  * partialResistDamageAdjust
                                                  * (1f + targetDebuffsNature)
                                                  * (1f + stats.BonusHunter_T8_2P_SerpDmg)
                                                  * focusedFireDamageAdjust
                                                  * beastWithinDamageAdjust
                                                  * sancRetributionAuraDamageAdjust
                                                  * blackArrowAuraDamageAdjust
                                                  * ferociousInspirationArcaneDamageAdjust;

            float chimeraShotSerpentStingDamage = (float)Math.Round(serpentStingDamageBase * chimeraShotSerpentMultiplier / 5f, 1) * serpentStingTicks;

            float chimeraShotEffect;
            if (calculatedStats.serpentSting.used)
                chimeraShotEffect = chimeraShotSerpentStingDamage * 0.4f;
            else
                chimeraShotEffect = 0;

            // Drizz: Updates
            float chimeraShotSerpentCritAdjust = metaGemCritDamage + (0.5f * metaGemCritDamage + 0.5f) * mortalShotsCritDamage;
            float chimeraShotSerpentDamageAdjust = (1f - ChanceToMiss) * (1f + calculatedStats.critRateOverall * chimeraShotSerpentCritAdjust);

            float chimeraShotSerpentTotalAdjust = chimeraShotSerpentDamageAdjust * talentDamageAdjust * (1f + targetDebuffsNature);

            calculatedStats.chimeraShot.damage += chimeraShotEffect * chimeraShotSerpentTotalAdjust;

            #endregion
            #region August 2009 Arcane Shot

            // base_damage = 492 + weapon_damage_gear + (RAP * 15%)
            float arcaneShotDamageNormal = 492f + statsItems.WeaponDamage + (RAP * 0.15f);

            // Drizz:
            // Corrected from Spreadsheet changelog 91e "T9 2-set bonus only crits for spell-crit bonus damage (i.e. 50% instead of 100%), not affected by Mortal Shots"
            // This is the reason for the 0.5 multiplier and that markedForDeath is kept outside
            float arcaneShotCritAdjust = metaGemCritDamage + 0.5f * mortalShotsCritDamage * (1f + metaGemCritDamage) + markedForDeathCritDamage;

            float arcaneShotDamageAdjust = talentDamageAdjust * partialResistDamageAdjust * (1f + 0.05f * talents.ImprovedArcaneShot)
                                            * ferociousInspirationArcaneDamageAdjust * ISSArcaneShotDamageAdjust; // missing arcane_debuffs!

            float arcaneShotDamageReal = CalcEffectiveDamage(
                                            arcaneShotDamageNormal,
                                            ChanceToMiss,
                                            arcaneShotCrit,
                                            arcaneShotCritAdjust,
                                            arcaneShotDamageAdjust
                                          );

            calculatedStats.arcaneShot.damage = arcaneShotDamageReal;

            #endregion
            #region August 2009 Multi Shot

            float multiShotDamageNormal = rangedWeaponDamage + statsItems.WeaponDamage + rangedAmmoDamage
                                        + calculatedStats.BasicStats.ScopeDamage + 408f + (RAP * 0.2f);
            float multiShotDamageAdjust = talentDamageAdjust * barrageDamageAdjust * targetPhysicalDebuffsDamageAdjust
                                            * ArmorDamageReduction; // missing: pvp gloves bonus

            float multiShotDamageReal = CalcEffectiveDamage(
                                            multiShotDamageNormal,
                                            ChanceToMiss,
                                            multiShotCrit,
                                            1f,
                                            multiShotDamageAdjust
                                         );

            calculatedStats.multiShot.damage = multiShotDamageReal;
            //calculatedStats.multiShot.Dump("Multi Shot");

            #endregion
            #region August 2009 Black Arrow

            float blackArrowDamageNormal = 2765 + (RAP * 0.1f);

            // this is a long list...
            float blackArrowDamageAdjust = partialResistDamageAdjust * focusedFireDamageAdjust * beastWithinDamageAdjust
                                          * sancRetributionAuraDamageAdjust * noxiousStingsDamageAdjust
                                          * ferociousInspirationDamageAdjust * improvedTrackingDamageAdjust
                                          * rangedWeaponSpecializationDamageAdjust * markedForDeathDamageAdjust
                                          * (sniperTrainingDamageAdjust + trapMasteryDamageAdjust + TNTDamageAdjust - 2)
                                          * blackArrowSelfDamageAdjust * (1 + targetDebuffsShadow);

            float blackArrowDamage = blackArrowDamageNormal * blackArrowDamageAdjust;

            calculatedStats.blackArrow.damage = blackArrowDamage;

            #endregion
            #region August 2009 Kill Shot
            // ****************************************************************************
            // Drizz: 31-10-2009 Aligned the calculations with spreadsheet v92b
            // Also moved the armorReduction adjust to be multiplied after DamageReal Calc

            float killShotDamageNormal = (autoShotDamage * 2f) + statsItems.WeaponDamage + 650f + (RAP * 0.4f);

            // Corrected from Spreadsheet changelog 91e "T9 2-set bonus only crits for spell-crit bonus damage (i.e. 50% instead of 100%), not affected by Mortal Shots"
            // This is the reasone for the 0.5 multiplier and that markedForDeath is kept outside
            float killShotCritAdjust = metaGemCritDamage + 0.5f * mortalShotsCritDamage * (1f + metaGemCritDamage) + markedForDeathCritDamage;
            
            float killShotDamageAdjust = talentDamageAdjust * targetPhysicalDebuffsDamageAdjust * ArmorDamageReduction;

            float killShotDamageReal = CalcEffectiveDamage(
                                            killShotDamageNormal,
                                            ChanceToMiss,
                                            killShotCrit,
                                            killShotCritAdjust,
                                            killShotDamageAdjust
                                        );

            calculatedStats.killShot.damage = killShotDamageReal;

            #endregion
            #region August 2009 Silencing Shot

            float silencingShotDamageNormal = (rangedWeaponDamage + rangedAmmoDamage + damageFromRAPNormalized) * 0.5f;
            float silencingShotDamageAdjust = talentDamageAdjust * targetPhysicalDebuffsDamageAdjust * ArmorDamageReduction;
            float silencingShotCritAdjust = 1f * metaGemCritDamage;

            float silencingShotDamageReal = CalcEffectiveDamage(
                                                silencingShotDamageNormal,
                                                ChanceToMiss,
                                                stats.PhysicalCrit,
                                                silencingShotCritAdjust,
                                                silencingShotDamageAdjust
                                             );

            calculatedStats.silencingShot.damage = silencingShotDamageReal;

            #endregion
            #region August 2009 Immolation Trap

            float immolationTrapDamage = 1885f + (0.1f * RAP);
            float immolationTrapDamageAdjust = (1 - targetDebuffsFire) * partialResistDamageAdjust * trapMasteryDamageAdjust
                                              * TNTDamageAdjust * talentDamageStingAdjust;
            float immolationTrapProjectedDamage = immolationTrapDamage * immolationTrapDamageAdjust;
            float immolationTrapDamagePerTick = immolationTrapProjectedDamage / (talents.GlyphOfImmolationTrap ? 2.5f : 5f);
            float immolationTrapTicks = talents.GlyphOfImmolationTrap ? 3 : 5;

            calculatedStats.immolationTrap.damage = immolationTrapDamagePerTick * immolationTrapTicks;

            #endregion
            #region August 2009 Rapid Fire

            calculatedStats.rapidFire.damage = 0;

            #endregion
            #region October 2009 Piercing Shots
            //Drizz: Added for PiercingShots

            calculatedStats.PiercingShotsDPS = 0;
            calculatedStats.PiercingShotsDPSSteadyShot = 0;
            calculatedStats.PiercingShotsDPSAimedShot = 0;
            calculatedStats.PiercingShotsDPSChimeraShot = 0;

            if (character.HunterTalents.PiercingShots > 0)
            {
                double piercingShotsDamageDone = character.HunterTalents.PiercingShots * 0.1;
                double piercingShotsMangleOnTarget = targetDebuffBleed;
                double piercingShotsTotalModifier = piercingShotsDamageDone * piercingShotsMangleOnTarget;
                double piercingShotsSteadyShotFrequency = calculatedStats.steadyShot.freq;
                double piercingShotsSteadyShotDamageAdded = steadyShotPiercingShots;

                double piercingShotsAimedShotFrequency = calculatedStats.aimedShot.freq;
                double piercingShotsAimedShotDamageAdded = aimedShotPiercingShots;
                double piercingShotsChimeraShotFrequency = calculatedStats.chimeraShot.freq;
                double piercingShotsChimeraShotDamageAdded = chimeraShotPiercingShots;

                if (piercingShotsSteadyShotFrequency > 0)
                {
                    //calculatedStats.PiercingShotsDPSSteadyShot = piercingShotsSteadyShotDamageAdded / piercingShotsSteadyShotFrequency;
                    calculatedStats.PiercingShotsDPS += piercingShotsSteadyShotDamageAdded / piercingShotsSteadyShotFrequency;
                }
                if (piercingShotsAimedShotFrequency > 0)
                {
                    //calculatedStats.PiercingShotsDPSAimedShot = piercingShotsAimedShotDamageAdded / piercingShotsAimedShotFrequency;
                    calculatedStats.PiercingShotsDPS += piercingShotsAimedShotDamageAdded / piercingShotsAimedShotFrequency;
                }
                if (piercingShotsChimeraShotFrequency > 0)
                {
                    //calculatedStats.PiercingShotsDPSChimeraShot = piercingShotsChimeraShotDamageAdded / piercingShotsChimeraShotFrequency;
                    calculatedStats.PiercingShotsDPS += piercingShotsChimeraShotDamageAdded / piercingShotsChimeraShotFrequency;
                }

                // **************************************************************************
                // 29-10-2009 Drizz: The below is used to make easier comparisons to the spreadsheet.
                calculatedStats.PiercingShotsDPSSteadyShot = piercingShotsSteadyShotDamageAdded;
                calculatedStats.PiercingShotsDPSAimedShot = piercingShotsAimedShotDamageAdded;
                calculatedStats.PiercingShotsDPSChimeraShot = piercingShotsChimeraShotDamageAdded;
                //***************************************************************************
            }
            #endregion

            #region August 2009 On-Proc DPS

            // Bandit's Insignia
            /*if (IsWearingTrinket(character, 40371))
            {
                float banditsInsigniaDamageAverage = ((1504 + 2256) / 2) * (1 + spellCritTotal) * (1 + targetDebuffsArcane)
                                                        * (1f - 0.17f + calculatedStats.hitFromRating);
                float banditsInsigniaTimePer = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0; // T54
                float banditsInsigniaTimeBetween = banditsInsigniaTimePer > 0 ? 1f / 0.15f * banditsInsigniaTimePer + 45 : 0;
                float banditsInsigniaDPS = banditsInsigniaTimeBetween > 0 ? banditsInsigniaDamageAverage / banditsInsigniaTimeBetween : 0;

                calculatedStats.OnProcDPS += banditsInsigniaDPS;
            }

            // Gnomish Lightning Generator
            if (IsWearingTrinket(character, 41121))
            {
                float gnomishLGDamage = ((1530f + 1870f) / 2f) * (1f + spellCritTotal) * (1f + targetDebuffsFire) * (1f - 0.17f + calculatedStats.hitFromRating);
                calculatedStats.OnProcDPS += gnomishLGDamage / 60;
            }

            // Darkmoon Card: Death
            if (IsWearingTrinket(character, 42990))
            {
                float cardDeathDamage = ((1750 + 2250) / 2) * (1 + critHitPercent); // Q66
                float cardDeathTimePerShot = totalShotsPerSecond > 0 ? 1 / totalShotsPerSecond / hitChance : 0; // Q69
                float cardDeathBetweenTime = cardDeathTimePerShot > 0 ? 1f / 0.35f * cardDeathTimePerShot + 45f : 0; // Q70

                calculatedStats.OnProcDPS += cardDeathBetweenTime > 0 ? cardDeathDamage / cardDeathBetweenTime : 0;                    
            }*/

            // Hand-Mounted Pyro Rocket
            /*if (character.HandsEnchant != null && character.HandsEnchant.Id == 3603)
            {
                float pyroRocketDamage = ((1654f + 2020f) / 2f) * (1f + spellCritTotal) * (1f + targetDebuffsFire) * (1f - 0.17f + calculatedStats.hitFromRating);
                calculatedStats.OnProcDPS += pyroRocketDamage / 45;
            }*/

            // Vestige of Haldor
            /*if (IsWearingTrinket(character, 37064))
            {
                float vestigeDamage = ((1024f + 1536f) / 2f) * (1f + critHitPercent); // T40
                float vestigeTimePerShot = totalShotsPerSecond > 0 ? 1f / totalShotsPerSecond / hitChance : 0; // T43
                float vestigeBetweenTime = vestigeTimePerShot > 0 ? 1f / 0.1f * vestigeTimePerShot + 45f : 0; // T44

                calculatedStats.OnProcDPS += vestigeBetweenTime > 0 ? vestigeDamage / vestigeBetweenTime : 0;
            }*/

            calculatedStats.OnProcDPS *= (1f - viperDamagePenalty);

            #endregion
            #region August 2009 Shot Rotation
            calculatedStats.priorityRotation.viperDamagePenalty = viperDamagePenalty;
            calculatedStats.priorityRotation.calculateRotationDPS();
            calculatedStats.CustomDPS = calculatedStats.priorityRotation.DPS;
            #endregion
            #region August 2009 Kill Shot Sub-20% Usage

            float killShotCurrentFreq = calculatedStats.killShot.freq;
            float killShotPossibleFreq = calcOpts.useRotationTest ? calculatedStats.killShot.freq : calculatedStats.killShot.start_freq;
            float steadyShotCurrentFreq = calculatedStats.steadyShot.freq;

            float steadyShotNewFreq = steadyShotCurrentFreq;
            if (killShotCurrentFreq == 0f && steadyShotCurrentFreq > 0f && killShotPossibleFreq > 0f)
            {
                steadyShotNewFreq = 1f / (1f / steadyShotCurrentFreq - 1f / killShotPossibleFreq);
            }

            float oldKillShotDPS = calculatedStats.killShot.dps;
            float newKillShotDPS = killShotPossibleFreq > 0 ? calculatedStats.killShot.damage / killShotPossibleFreq : 0f;
            newKillShotDPS *= (1f - viperDamagePenalty);

            float oldSteadyShotDPS = calculatedStats.steadyShot.dps;
            float newSteadyShotDPS = steadyShotNewFreq > 0 ? calculatedStats.steadyShot.damage / steadyShotNewFreq : 0f;
            newSteadyShotDPS *= (1f - viperDamagePenalty);

            float killShotDPSGain = newKillShotDPS > 0f ? (newKillShotDPS + newSteadyShotDPS) - (oldKillShotDPS + oldSteadyShotDPS) : 0f;

            float timeSpentSubTwenty = 0;
            if (calcOpts.Duration > 0 && calcOpts.timeSpentSub20 > 0) timeSpentSubTwenty = (float)calcOpts.timeSpentSub20 / (float)calcOpts.Duration;
            if (calcOpts.bossHPPercentage < 0.2f) timeSpentSubTwenty = 1f;

            float killShotSubGain = timeSpentSubTwenty * killShotDPSGain;

            calculatedStats.killShotSub20NewSteadyFreq = steadyShotNewFreq;
            calculatedStats.killShotSub20NewDPS = newKillShotDPS;
            calculatedStats.killShotSub20NewSteadyDPS = newSteadyShotDPS;
            calculatedStats.killShotSub20Gain = killShotDPSGain;
            calculatedStats.killShotSub20TimeSpent = timeSpentSubTwenty;
            calculatedStats.killShotSub20FinalGain = killShotSubGain;

            #endregion

            calculatedStats.HunterDpsPoints = (float)(calculatedStats.AutoshotDPS
                                                    + calculatedStats.WildQuiverDPS
                                                    + calculatedStats.CustomDPS
                                                    + calculatedStats.OnProcDPS
                                                    + calculatedStats.killShotSub20FinalGain
                                                    + calculatedStats.aspectBeastLostDPS);
            calculatedStats.OverallPoints = calculatedStats.HunterDpsPoints
                                          + calculatedStats.PetDpsPoints;

            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            cacheChar = character;
            CalculationOptionsHunter calcOpts = character.CalculationOptions as CalculationOptionsHunter;
            if (calcOpts == null) { calcOpts = new CalculationOptionsHunter(); character.CalculationOptions = calcOpts; }
            HunterTalents talents = character.HunterTalents;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Hunter, character.Race);
			statsRace.PhysicalCrit += (character.Ranged != null && 
				((character.Race == CharacterRace.Dwarf && character.Ranged.Item.Type == ItemType.Gun) ||
				(character.Race == CharacterRace.Troll && character.Ranged.Item.Type == ItemType.Bow))) ?
				0.01f : 0.00f;
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsOptionsPanel = new Stats()
            {
                PhysicalCrit = StatConversion.NPC_LEVEL_CRIT_MOD[calcOpts.TargetLevel - character.Level],
                PhysicalHaste = 0.15f, // This is from what Bags used to give that got rolled into the class
            };
            {
                CharacterCalculationsHunter calculatedStats = new CharacterCalculationsHunter();
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
                if (calculatedStats.priorityRotation.containsShot(Shots.RapidFire)) {
                    statsOptionsPanel.AddSpecialEffect(new SpecialEffect(Trigger.Use,
                        new Stats() { PhysicalHaste = (talents.GlyphOfRapidFire ? 0.48f : 0.4f), },
                        15, (5 - talents.RapidKilling) * 60));
                }
            }
            Stats statsTalents = new Stats()
            {
                PhysicalHit = (talents.FocusedAim * 0.01f),
                PhysicalCrit = (1f + 0.01f * talents.LethalShots)
                             * (1f + 0.01f * talents.MasterMarksman)
                             * (1f + 0.01f * talents.KillerInstinct)
                             - 1f,
                BonusAgilityMultiplier = (1f + 0.02f * talents.CombatExperience)
                                       * (1f + 0.03f * talents.LightningReflexes)
                                       * (1f + 0.01f * talents.HuntingParty)
                                       - 1f,
                BonusIntellectMultiplier = 0.02f * talents.CombatExperience,
                BonusStaminaMultiplier = 0.02f * talents.Survivalist,
                BaseArmorMultiplier = (talents.ThickHide / 3f) * 0.10f,
                PhysicalHaste = 0.04f * talents.SerpentsSwiftness,
            };
            if(talents.MasterTactician > 0){
                SpecialEffect mt = new SpecialEffect(Trigger.PhysicalHit,
                    new Stats() { PhysicalCrit = talents.MasterTactician * 0.02f, }, 8f, 0f, 0.10f);
                statsTalents.AddSpecialEffect(mt);
            }
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents + statsOptionsPanel;
            Stats statsProcs = new Stats();

            statsTotal.Agility += statsTotal.AverageAgility;
            int targetDefence = 5 * calcOpts.TargetLevel;

            // Stamina
            float totalBSTAM = statsTotal.BonusStaminaMultiplier;
            float staBase = (float)Math.Floor((1f + totalBSTAM) * statsRace.Stamina);
            float staBonus = (float)Math.Floor((1f + totalBSTAM) * statsGearEnchantsBuffs.Stamina);
            statsTotal.Stamina = staBase + staBonus;

            // Health
            statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);
            statsTotal.Health *= 1f + statsTotal.BonusHealthMultiplier;

            // Strength
            float totalBSTRM = statsTotal.BonusStrengthMultiplier;
            float strBase    = (float)Math.Floor((1f + totalBSTRM) * statsRace.Strength);
            float strBonus   = (float)Math.Floor((1f + totalBSTRM) * statsGearEnchantsBuffs.Strength);
            statsTotal.Strength = strBase + strBonus;

            // Agility
            float totalBAGIM = statsTotal.BonusAgilityMultiplier;
            float agiBase    = (float)Math.Floor((1f + totalBAGIM) * statsRace.Agility);
            float agiBonus   = (float)Math.Floor((1f + totalBAGIM) * statsGearEnchantsBuffs.Agility);
            statsTotal.Agility = agiBase + agiBonus;

            // Intellect
            float totalBINTM = statsTotal.BonusIntellectMultiplier;
            float intBase = (float)Math.Floor((1f + totalBINTM) * statsRace.Intellect);
            float intBonus = (float)Math.Floor((1f + totalBINTM) * statsGearEnchantsBuffs.Intellect);
            statsTotal.Intellect = intBase + intBonus;

            // Armor
            statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BaseArmorMultiplier));
            statsTotal.BonusArmor += statsTotal.Agility * 2f;
            statsTotal.BonusArmor = (float)Math.Floor(statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal.Armor += statsTotal.BonusArmor;

            // Attack Power
            statsTotal.BonusAttackPowerMultiplier *= (1f + statsTotal.BonusRangedAttackPowerMultiplier);
            float totalBAPM    = statsTotal.BonusAttackPowerMultiplier;
            float apBase       = (1f + totalBAPM) * (statsRace.AttackPower + statsRace.RangedAttackPower);
            float apFromAGI    = (1f + totalBAPM) * (statsTotal.Agility);
            float apFromSTR    = (1f + totalBAPM) * (statsTotal.Strength);
            float apBonusOther = (1f + totalBAPM) * (statsGearEnchantsBuffs.AttackPower + statsGearEnchantsBuffs.RangedAttackPower);
            statsTotal.AttackPower = (float)Math.Floor(apBase + apFromAGI + apFromSTR + apBonusOther);

            // Spell Power
            float totalBSPM    = statsTotal.BonusSpellPowerMultiplier;
            float spBase       = (1f + totalBSPM) * (statsRace.SpellPower);
            float spBonusOther = (1f + totalBSPM) * (statsGearEnchantsBuffs.SpellPower);
            statsTotal.SpellPower = (float)Math.Floor(spBase + spBonusOther);

            // Crit
            statsTotal.CritRating += statsTotal.RangedCritRating;
            statsTotal.PhysicalCrit += StatConversion.GetCritFromAgility(statsTotal.Agility, character.Class)
                                     + -0.0153f
                                     + StatConversion.GetCritFromRating(statsTotal.CritRating, character.Class);

            // Haste
            statsTotal.HasteRating += statsTotal.RangedHasteRating;
            float ratingHasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, character.Class);
            statsTotal.PhysicalHaste = (1f + statsRace.PhysicalHaste) *
                                       (1f + statsItems.PhysicalHaste) *
                                       (1f + statsBuffs.PhysicalHaste) *
                                       (1f + statsTalents.PhysicalHaste) *
                                       (1f + statsOptionsPanel.PhysicalHaste) *
                                       (1f + statsTotal.RangedHaste) *
                                       (1f + ratingHasteBonus)
                                       - 1f;

            // Hit
            statsTotal.HitRating += statsTotal.RangedHitRating;
            statsTotal.PhysicalHit += StatConversion.GetHitFromRating(statsTotal.HitRating);

            // The first 20 Int = 20 Mana, while each subsequent Int = 15 Mana
            // (20-(20/15)) = 18.66666
            // spreadsheet uses 18.7, so we will too :)
            statsTotal.Mana = (float)(statsRace.Mana + 15f * (statsTotal.Intellect - 18.7f) + statsGearEnchantsBuffs.Mana);

            /*// The first 20 Stam = 20 Health, while each subsequent Stam = 10 Health, so Health = (Stam-18)*10
            // (20-(20/10)) = 18
            float healthFromBase = statsRace.Health;
            float healthFromStamina = (statsTotal.Stamina - 18) * 10;
            float healthFromGearBuffs = statsGearEnchantsBuffs.Health;
            float healthFromTalents = (healthFromBase + healthFromStamina + healthFromGearBuffs) * (character.HunterTalents.EnduranceTraining * 0.01);
            float healthSubTotal = healthFromBase + healthFromStamina + healthFromGearBuffs + healthFromTalents;
            float healthTaurenAdjust = character.Race == CharacterRace.Tauren ? 1.05 : 1;
            statsTotal.Health = (float)(healthSubTotal * healthTaurenAdjust);*/

            float attemptedAtksInterval = 1f;
            float ChanceToMiss = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[calcOpts.TargetLevel - character.Level] - statsTotal.PhysicalHit);
            float hitRate = (1f - ChanceToMiss);
            float critRate = statsTotal.PhysicalCrit;
            float bleedHitInterval = 1f;
            float dmgDoneInterval = 1f;

            statsProcs += GetSpecialEffectsStats(character, attemptedAtksInterval, hitRate, critRate,
                                    bleedHitInterval, dmgDoneInterval, statsTotal, null);

            // Base Stats
            statsProcs.Stamina  = (float)Math.Floor(statsProcs.Stamina     * (1f + totalBSTAM) * (1f + statsProcs.BonusStaminaMultiplier));
            statsProcs.Strength = (float)Math.Floor(statsProcs.Strength    * (1f + totalBSTRM) * (1f + statsProcs.BonusStrengthMultiplier));
            statsProcs.Agility  = (float)Math.Floor(statsProcs.Agility     * (1f + totalBAGIM) * (1f + statsProcs.BonusAgilityMultiplier));
            statsProcs.Agility += (float)Math.Floor(statsProcs.HighestStat * (1f + totalBAGIM) * (1f + statsProcs.BonusAgilityMultiplier));
            statsProcs.Agility += (float)Math.Floor(statsProcs.Paragon     * (1f + totalBAGIM) * (1f + statsProcs.BonusAgilityMultiplier));
            statsProcs.Health  += (float)Math.Floor(statsProcs.Stamina * 10f);

            // Armor
            statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BaseArmorMultiplier + statsProcs.BaseArmorMultiplier));
            statsProcs.BonusArmor += statsProcs.Agility * 2f;
            statsProcs.BonusArmor = (float)Math.Floor(statsProcs.BonusArmor * (1f + statsTotal.BonusArmorMultiplier + statsProcs.BonusArmorMultiplier));
            statsProcs.Armor += statsProcs.BonusArmor;
            statsProcs.BonusArmor = 0; //it's been added to Armor so kill it

            // Attack Power
            statsProcs.BonusAttackPowerMultiplier *= (1f + statsProcs.BonusRangedAttackPowerMultiplier);
            float totalBAPMProcs    = (1f + totalBAPM) * (1f + statsProcs.BonusAttackPowerMultiplier) - 1f;
            float apFromAGIProcs    = (1f + totalBAPMProcs) * (statsProcs.Agility);
            float apFromSTRProcs    = (1f + totalBAPMProcs) * (statsProcs.Strength);
            float apBonusOtherProcs = (1f + totalBAPMProcs) * (statsProcs.AttackPower + statsProcs.RangedAttackPower);
            statsProcs.AttackPower = (float)Math.Floor(apFromAGIProcs + apFromSTRProcs + apBonusOtherProcs);

            // Crit
            statsProcs.PhysicalCrit += StatConversion.GetCritFromAgility(statsProcs.Agility, character.Class);
            statsProcs.PhysicalCrit += StatConversion.GetCritFromRating(statsProcs.CritRating + statsProcs.RangedCritRating, character.Class);

            // Haste
            statsProcs.PhysicalHaste = (1f + statsProcs.PhysicalHaste)
                                     * (1f + StatConversion.GetPhysicalHasteFromRating(statsProcs.HasteRating, character.Class))
                                     - 1f;

            statsTotal += statsProcs;

            return statsTotal;
        }

        private Stats GetSpecialEffectsStats(Character Char,
            float attemptedAtkInterval, float hitRate, float critRate, float bleedHitInterval, float dmgDoneInterval,
            Stats statsTotal, Stats statsToProcess)
        {
            CalculationOptionsHunter calcOpts = Char.CalculationOptions as CalculationOptionsHunter;
            ItemInstance RangeWeap = Char.MainHand;
            float speed = (RangeWeap != null ? RangeWeap.Speed : 2.4f);
            HunterTalents talents = Char.HunterTalents;
            Stats statsProcs = new Stats();
            float fightDuration = calcOpts.Duration;
            //
            foreach (SpecialEffect effect in (statsToProcess != null ? statsToProcess.SpecialEffects() : statsTotal.SpecialEffects())) {
                float oldArp = effect.Stats.ArmorPenetrationRating;
                if (effect.Stats.ArmorPenetrationRating > 0) {
                    float arpenBuffs = 0.0f;
                    float currentArp = arpenBuffs + StatConversion.GetArmorPenetrationFromRating(statsTotal.ArmorPenetrationRating
                        + (statsToProcess != null ? statsToProcess.ArmorPenetrationRating : 0f));
                    float arpToHardCap = (1f - currentArp) * StatConversion.RATING_PER_ARMORPENETRATION;
                    if (arpToHardCap < effect.Stats.ArmorPenetrationRating) effect.Stats.ArmorPenetrationRating = arpToHardCap;
                }
                switch (effect.Trigger) {
                    case Trigger.Use:
                        Stats _stats = new Stats();
                        if (effect.Stats._rawSpecialEffectDataSize == 1 && statsToProcess == null) {
                            float uptime = effect.GetAverageUptime(0f, 1f, speed, fightDuration);
                            _stats.AddSpecialEffect(effect.Stats._rawSpecialEffectData[0]);
                            Stats _stats2 = GetSpecialEffectsStats(Char,
                                attemptedAtkInterval, hitRate, critRate, bleedHitInterval, dmgDoneInterval, statsTotal, _stats);
                            _stats = _stats2 * uptime;
                        } else {
                            _stats = effect.GetAverageStats(0f, 1f, speed, fightDuration);
                        }
                        statsProcs += _stats;
                        break;
                    case Trigger.PhysicalHit:
                        if (attemptedAtkInterval > 0f) {
                            Stats add = effect.GetAverageStats(attemptedAtkInterval, hitRate, speed, fightDuration);
                            statsProcs += add;
                        }
                        break;
                    case Trigger.MeleeCrit:
                    case Trigger.PhysicalCrit:
                        if (attemptedAtkInterval > 0f) {
                            Stats add = effect.GetAverageStats(attemptedAtkInterval, critRate, speed, fightDuration);
                            statsProcs += add;
                        }
                        break;
                    case Trigger.DoTTick:
                        if (bleedHitInterval > 0f) { statsProcs += effect.GetAverageStats(bleedHitInterval, 1f, speed, fightDuration); } // 1/sec DeepWounds, 1/3sec Rend
                        break;
                    case Trigger.DamageDone: // physical and dots
                        if (dmgDoneInterval > 0f) { statsProcs += effect.GetAverageStats(dmgDoneInterval, 1f, speed, fightDuration); }
                        break;
                }
                effect.Stats.ArmorPenetrationRating = oldArp;
            }

            return statsProcs;
        }

		#endregion //overrides

        #region Private Functions

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

        public static float CalcTrinketUptime(float duration, float cooldown, float chance, float triggersPerSecond)
        {
            float timePerTrigger = triggersPerSecond > 0 ? 1 / triggersPerSecond : 0;
            float time_between_procs = timePerTrigger > 0 ? 1 / chance * timePerTrigger + cooldown : 0;
            return time_between_procs > 0 ? duration / time_between_procs : 0;
        }

        private ShotData getShotByIndex(int index, CharacterCalculationsHunter calculatedStats)
        {
            if (index == 1) return calculatedStats.aimedShot;
            if (index == 2) return calculatedStats.arcaneShot;
            if (index == 3) return calculatedStats.multiShot;
            if (index == 4) return calculatedStats.serpentSting;
            if (index == 5) return calculatedStats.scorpidSting;
            if (index == 6) return calculatedStats.viperSting;
            if (index == 7) return calculatedStats.silencingShot;
            if (index == 8) return calculatedStats.steadyShot;
            if (index == 9) return calculatedStats.killShot;
            if (index == 10) return calculatedStats.explosiveShot;
            if (index == 11) return calculatedStats.blackArrow;
            if (index == 12) return calculatedStats.immolationTrap;
            if (index == 13) return calculatedStats.chimeraShot;
            if (index == 14) return calculatedStats.rapidFire;
            if (index == 15) return calculatedStats.readiness;
            if (index == 16) return calculatedStats.beastialWrath;
            if (index == 17) return calculatedStats.bloodFury;
            if (index == 18) return calculatedStats.berserk;
            return null;
        }

        public float GetArmorDamageReduction(CalculationOptionsHunter CalcOpts, Character Char, Stats StatS) {
                float armorReduction;
                float arpenBuffs = 0.0f;
                if (CalcOpts == null) {
                    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level, (int)StatConversion.NPC_ARMOR[CalcOpts.TargetLevel - Char.Level], StatS.ArmorPenetration, arpenBuffs, StatS.ArmorPenetrationRating)); // default is vs raid boss
                } else {
                    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level, CalcOpts.TargetArmor, StatS.ArmorPenetration, arpenBuffs, StatS.ArmorPenetrationRating));
                }

                return armorReduction;
        }
        #endregion
	}
}
