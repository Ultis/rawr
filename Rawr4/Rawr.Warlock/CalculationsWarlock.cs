using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rawr.Warlock
{
    [Rawr.Calculations.RawrModelInfo("Warlock", "Spell_Nature_FaerieFire", CharacterClass.Warlock)]
    public class CalculationsWarlock : CalculationsBase
    {
        // Basic Model Functionality
        public override CharacterClass TargetClass { get { return CharacterClass.Warlock; } }
        private CalculationOptionsPanelWarlock _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelWarlock();
                }
                return _calculationOptionsPanel;
            }
        }
        public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return new ComparisonCalculationWarlock();
        }
        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsWarlock();
        }
        public const float AVG_UNHASTED_CAST_TIME = 2f; // total SWAG
        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd("Fel Armor");
        }
        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                {
                    _characterDisplayCalculationLabels = new string[] {
                        "Simulation:Personal DPS",
                        "Simulation:Pet DPS",
                        "Simulation:Total DPS",
                        "Basic Stats:Health",
                        "Basic Stats:Mana",
                        "Basic Stats:Bonus Damage",
                        "Basic Stats:Hit Rating",
                        "Basic Stats:Crit Chance",
                        "Basic Stats:Average Haste",
                        "Basic Stats:Mastery",
                        "Pet Stats:Pet Stamina",
                        "Pet Stats:Pet Intellect",
                        "Pet Stats:Pet Health",
                        "Affliction:Corruption",
                        "Affliction:Bane Of Agony",
                        "Affliction:Bane Of Doom",
                        "Affliction:Curse Of The Elements",
                        "Affliction:Drain Life",
                        "Affliction:Drain Soul",
                        "Affliction:Haunt",
                        "Affliction:Life Tap",
                        "Affliction:Unstable Affliction",
                        "Demonology:Immolation Aura",
                        "Destruction:Chaos Bolt",
                        "Destruction:Conflagrate",
                        "Destruction:Fel Flame",
                        "Destruction:Immolate",
                        "Destruction:Incinerate",
                        "Destruction:Incinerate (Under Backdraft)",
                        "Destruction:Incinerate (Under Molten Core)",
                        "Destruction:Searing Pain",
                        "Destruction:Soul Fire",
                        "Destruction:Shadow Bolt",
                        "Destruction:Shadow Bolt (Instant)",
                        "Destruction:Shadowburn"
                    };
                }
                return _characterDisplayCalculationLabels;
            }
        }
        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                {
                    _optimizableCalculationLabels = new string[] { "Miss Chance"};
                }
                return _optimizableCalculationLabels;
            }
        }
        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] { };
                }
                return _customChartNames;
            }
        }
        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("DPS", Color.FromArgb(255, 255, 0, 0));
                    _subPointNameColors.Add("Pet DPS", Color.FromArgb(255, 0, 0, 255));
                }
                return _subPointNameColors;
            }
        }

        // Basic Calcuations
        public override ICalculationOptionBase DeserializeDataObject(string xml) 
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsWarlock));
            StringReader reader = new StringReader(xml);
            CalculationOptionsWarlock calcOpts = serializer.Deserialize(reader) as CalculationOptionsWarlock;
            return calcOpts;
        }
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item addlItem, bool _u1, bool _u2, bool _u3)
        {
            return new CharacterCalculationsWarlock(character, GetCharacterStats(character, addlItem), GetPetBuffStats(character));
        }
        private Stats GetPetBuffStats(Character character)
        {
            List<Buff> buffs = new List<Buff>();
            foreach (Buff buff in character.ActiveBuffs)
            {
                string group = buff.Group;
                if (   group != "Profession Buffs"
                    && group != "Set Bonuses"
                    && group != "Food"
                    && group != "Potion"
                    && group != "Elixirs and Flasks"
                    && group != "Focus Magic, Spell Critical Strike Chance")
                {
                    buffs.Add(buff);
                }
            }
            Stats stats = GetBuffsStats(buffs);
            var options = character.CalculationOptions as CalculationOptionsWarlock;
            ApplyPetsRaidBuff(stats, options.Pet, character.WarlockTalents, character.ActiveBuffs, options);
            return stats;
        }
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            WarlockTalents talents = character.WarlockTalents;
            CalculationOptionsWarlock options = character.CalculationOptions as CalculationOptionsWarlock;
            Stats stats = BaseStats.GetBaseStats(character);
            
            AccumulateItemStats(stats, character, additionalItem);
            AccumulateBuffsStats(stats, character.ActiveBuffs);
            ApplyPetsRaidBuff(stats, options.Pet, talents, character.ActiveBuffs, options);

            float[] demonicEmbraceValues = { 0f, .04f, .07f, .1f };
            Stats statsTalents = new Stats {
                BonusStaminaMultiplier = demonicEmbraceValues[talents.DemonicEmbrace], //Demonic Embrace
                BonusIntellectMultiplier = options.PlayerLevel > 50 ? .05f : 0 // Nethermancy
            };

            if (talents.Eradication > 0)
            {
                float[] eradicationValues = { 0f, .06f, .12f, .20f };
                statsTalents.AddSpecialEffect(
                    new SpecialEffect(
                        Trigger.CorruptionTick,
                        new Stats() {
                            SpellHaste = eradicationValues[talents.Eradication]
                        },
                        10f, //duration
                        0f,  //cooldown
                        .06f)); //chance
            }

            stats.Accumulate(statsTalents);
            stats.ManaRestoreFromMaxManaPerSecond
                = Math.Max(
                    stats.ManaRestoreFromMaxManaPerSecond,
                    .001f * Spell.CalcUprate(talents.SoulLeech > 0 ? 1f : 0f, 15f, options.Duration * 1.1f));
            return stats;
        }
        private void ApplyPetsRaidBuff(Stats stats, string pet, WarlockTalents talents, List<Buff> activeBuffs, CalculationOptionsWarlock options)
        {
            stats.Health += CalcPetHealthBuff(pet, talents, activeBuffs, options);
            stats.Mana += CalcPetManaBuff(pet, talents, activeBuffs, options);
            stats.Mp5 += CalcPetMP5Buff(pet, talents, activeBuffs, options);
        }
        private static float[] buffBaseValues = { 125f, 308f, 338f, 375f, 407f, 443f };
        public static float CalcPetHealthBuff(string pet, WarlockTalents talents, List<Buff> activeBuffs, CalculationOptionsWarlock options)
        {
            if (!pet.Equals("Imp"))
            {
                return 0f;
            }

            //spell ID 6307, effect ID 2190
            float SCALE = 1.3200000525f;
            return StatUtils.GetBuffEffect(activeBuffs, SCALE * buffBaseValues[options.PlayerLevel - 80], "Health", s => s.Health);
        }
        public static float CalcPetManaBuff(string pet, WarlockTalents talents, List<Buff> activeBuffs, CalculationOptionsWarlock options)
        {
            if (!pet.Equals("Felhunter"))
            {
                return 0f;
            }

            //spell ID 54424, effect ID 47202
            float SCALE = 4.8000001907f;
            return StatUtils.GetBuffEffect(activeBuffs, SCALE * buffBaseValues[options.PlayerLevel - 80], "Mana", s => s.Mana);
        }
        public static float CalcPetMP5Buff(string pet, WarlockTalents talents, List<Buff> activeBuffs, CalculationOptionsWarlock options)
        {
            if (!pet.Equals("Felhunter"))
            {
                return 0f;
            }

            //spell ID 54424, effect ID 47203
            float SCALE = 0.7360000014f;
            return StatUtils.GetBuffEffect(activeBuffs, SCALE * buffBaseValues[options.PlayerLevel - 80], "Mana Regeneration", s => s.Mp5);
        }
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }

        // Relevancy
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(6) { 
                        ItemType.None, ItemType.Cloth, ItemType.Dagger, ItemType.Wand, ItemType.OneHandSword, ItemType.Staff 
                    };
                }
                return _relevantItemTypes;
            }
        }
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                const int CATAUN = 0;
                const int CATARA = 1;
                const int CATAJC = 2;
                //const int CATAEP = 3; // compiler warning
                const int CATACG = 4;

                //Red
                int[] brilliant = { 52084, 52207, 52257, 0, 0 }; //int

                //Yellow
                int[] fractured = { 52094, 52219, 52269, 0, 59480 }; //mastery
                int[] quick     = { 52093, 52232, 52268, 0, 59479 }; //haste
                int[] smooth    = { 52091, 52241, 52266, 0, 59478 }; //crit

                //Blue
                int[] rigid     = { 52089, 52235, 52264, 0, 59493 }; //hit

                //Purple
                int[] veiled    = { 52104, 52217, 0, 0, 0 }; // int/hit

                //Orange
                int[] reckless  = { 52113, 52208, 0, 0, 0 }; //int/haste
                int[] potent    = { 52239, 52239, 0, 0, 0 }; //int/crit
                int[] artful    = { 52117, 52205, 0, 0, 0 }; //int/mast

                //Green
                int[] lightning = { 52125, 52225, 0, 0, 0 }; // haste/hit
                int[] piercing  = { 52122, 52228, 0, 0, 0 }; // crit/hit
                int[] senseis   = { 52128, 52237, 0, 0, 0 }; // mast/hit

                //Meta
                const int CATAMETA = 0;
                int[] ember   = { 52296 };
                int[] chaotic = { 52291 };
                int[] burning = { 68780 };

                return new List<GemmingTemplate>
                {
                    #region uncommon - cata
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //Max SP - Ember
                        RedId = brilliant[CATAUN], YellowId = brilliant[CATAUN], BlueId = brilliant[CATAUN],
                        PrismaticId = brilliant[CATAUN], MetaId = burning[CATAMETA], CogwheelId = quick[CATACG]
                    },
                    new GemmingTemplate
                    {
                        Model = "Warlock", Group = "Uncommon", //Matching - Ember
                        RedId = brilliant[CATAUN], YellowId = reckless[CATAUN], BlueId = veiled[CATAUN],
                        PrismaticId = brilliant[CATAUN], MetaId = burning[CATAMETA], CogwheelId = quick[CATACG]
                    },
                    #endregion

                    #region rare - cata
				    new GemmingTemplate
				    {
                        Enabled = true,
                        Model = "Warlock", Group = "Rare", //Max SP - Ember
				        RedId = brilliant[CATARA], YellowId = brilliant[CATARA], BlueId = brilliant[CATARA],
                        PrismaticId = brilliant[CATARA], MetaId = burning[CATAMETA], CogwheelId = quick[CATACG]
                    },
				    new GemmingTemplate
				    {
                        Enabled = true,
                        Model = "Warlock", Group = "Rare", //SP/Hit - Ember
				        RedId = brilliant[CATARA], YellowId = reckless[CATARA], BlueId = veiled[CATARA],
                        PrismaticId = brilliant[CATARA], MetaId = burning[CATAMETA], CogwheelId = quick[CATACG]
                    },
                    #endregion

                    /*
                    #region epic - cata
                    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //Max SP - Ember
				        RedId = brilliant[CATAEP], YellowId = brilliant[CATAEP], BlueId = brilliant[CATAEP],
                        PrismaticId = brilliant[CATAEP], MetaId = burning[CATAMETA], CogwheelId = quick[CATACG]
                    },
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Epic", Enabled = true, //SP/Hit - Ember
				        RedId = brilliant[CATAEP], YellowId = reckless[CATAEP], BlueId = veiled[CATAEP],
                        PrismaticId = brilliant[CATAEP], MetaId = burning[CATAMETA], CogwheelId = quick[CATACG]
                    },
                    #endregion
                    */

                    #region jeweler - cata
				    new GemmingTemplate
				    {
                        Model = "Warlock", Group = "Jeweler", //Max SP - Ember
				        RedId = brilliant[CATAJC], YellowId = brilliant[CATAJC], BlueId = brilliant[CATAJC],
                        PrismaticId = brilliant[CATAJC], MetaId = burning[CATAMETA], CogwheelId = quick[CATACG]
                    },
                    #endregion
                };
            }
        }
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats {
                SpellPower = stats.SpellPower,
                Intellect = stats.Intellect,
                HitRating = stats.HitRating,
                SpellHit = stats.SpellHit,
                HasteRating = stats.HasteRating,
                SpellHaste = stats.SpellHaste,
                CritRating = stats.CritRating,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                MasteryRating = stats.MasteryRating,

                ShadowDamage = stats.ShadowDamage,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                FireDamage = stats.FireDamage,
                SpellFireDamageRating = stats.SpellFireDamageRating,

                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,

                Warlock2T7 = stats.Warlock2T7,
                Warlock4T7 = stats.Warlock4T7,
                Warlock2T8 = stats.Warlock2T8,
                Warlock4T8 = stats.Warlock4T8,
                Warlock2T9 = stats.Warlock2T9,
                Warlock4T9 = stats.Warlock4T9,
                Warlock2T10 = stats.Warlock2T10,
                Warlock4T10 = stats.Warlock4T10,
                Warlock2T11 = stats.Warlock2T11,
                Warlock4T11 = stats.Warlock4T11,

                Stamina = stats.Stamina,
                Health = stats.Health,
                Mana = stats.Mana,
                Mp5 = stats.Mp5,

                HighestStat = stats.HighestStat,                                    //trinket - darkmoon card: greatness
                ManaRestoreFromBaseManaPPM = stats.ManaRestoreFromBaseManaPPM,      //paladin buff: judgement of wisdom
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,    //replenishment
                BonusManaPotion = stats.BonusManaPotion,                            //triggered when a mana pot is consumed
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,        //Bracing Eathsiege Diamond (metagem) effect
                ManaRestore = stats.ManaRestore,                                    //quite a few items that restore mana on spell cast or crit. Also used to model replenishment.
                SpellsManaReduction = stats.SpellsManaReduction,                    //spark of hope -> http://www.wowhead.com/?item=45703
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (RelevantTrinket(effect))
                {
                    s.AddSpecialEffect(effect);
                }
            }
            return s;
        }
        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            if (buff == null) { return false; }
            if (!buff.AllowedClasses.Contains(CharacterClass.Warlock)
                || (buff.Group != null && buff.Group.Equals("Spell Sensitivity")))
            {
                return false;
            }
            if (character != null
                && Rawr.Properties.GeneralSettings.Default.HideProfEnchants
                && !character.HasProfession(buff.Professions))
            {
                return false;
            }
            Stats stats = buff.GetTotalStats();
            return HasRelevantStats(stats)
                || stats.Strength > 0
                || stats.Agility > 0
                || stats.AttackPower > 0
                || stats.BonusAttackPowerMultiplier > 0
                || stats.PhysicalCrit > 0
                || stats.PhysicalHaste > 0
                || stats.ArmorPenetration > 0
                || stats.TargetArmorReduction > 0
                || stats.BonusPhysicalDamageMultiplier > 0;
        }
        protected bool RelevantTrinket(SpecialEffect effect)
        {
            if (   effect.Trigger == Trigger.Use
                || effect.Trigger == Trigger.DamageSpellCast
                || effect.Trigger == Trigger.DamageSpellCrit
                || effect.Trigger == Trigger.DamageSpellHit
                || effect.Trigger == Trigger.SpellCast
                || effect.Trigger == Trigger.SpellCrit
                || effect.Trigger == Trigger.SpellHit
                || effect.Trigger == Trigger.SpellMiss
                || effect.Trigger == Trigger.DoTTick
                || effect.Trigger == Trigger.DamageDone
                || effect.Trigger == Trigger.DamageOrHealingDone)
            {
                return _HasRelevantStats(effect.Stats);
            }
            return false;
        }
        public override bool HasRelevantStats(Stats stats)
        {
            if (HasIgnoreStats(stats))
            {
                return false;
            }

            // this will allow Tiny Abomination in a Jar (passive Hit), but also Hurricane (melee proc or spell proc)
            bool isRelevant = HasWarlockStats(stats) || HasCommonStats(stats);
            foreach (SpecialEffect se in stats.SpecialEffects())
            {
                isRelevant |= RelevantTrinket(se);
            }
            return isRelevant;
        }
        protected bool HasWarlockStats(Stats stats)
        {
            // These stats automatically count as relevant.
            return (stats.SpellPower
                  + stats.Intellect
                  + stats.ShadowDamage + stats.SpellShadowDamageRating
                  + stats.FireDamage + stats.SpellFireDamageRating
                  + stats.BonusIntellectMultiplier
                  + stats.BonusDamageMultiplier + stats.BonusShadowDamageMultiplier + stats.BonusFireDamageMultiplier
                  + stats.Warlock2T7 + stats.Warlock4T7
                  + stats.Warlock2T8 + stats.Warlock4T8
                  + stats.Warlock2T9 + stats.Warlock4T9
                  + stats.Warlock2T10 + stats.Warlock4T10
                  + stats.Warlock2T11 + stats.Warlock4T11 > 0);
        }
        protected bool HasCommonStats(Stats stats)
        {
            // These stats are only relevant if none of the ignore stats were found.
            // That way Str + Crit, etc. are rejected, but an item with only Hit + Crit, etc. would be accepted.
            return (stats.Stamina + stats.Health
                  + stats.HitRating + stats.SpellHit
                  + stats.HasteRating + stats.SpellHaste
                  + stats.CritRating + stats.SpellCrit + stats.SpellCritOnTarget + stats.BonusSpellCritMultiplier
                  + stats.MasteryRating
                  + stats.Mana + stats.Mp5
                  + stats.HighestStat                     //darkmoon card: greatness
                  + stats.SpellsManaReduction             //spark of hope -> http://www.wowhead.com/?item=45703
                  + stats.BonusManaPotion                 //triggered when a mana pot is consumed
                  + stats.ManaRestoreFromBaseManaPPM      //judgement of wisdom
                  + stats.ManaRestoreFromMaxManaPerSecond //replenishment sources
                  + stats.ManaRestore                     //quite a few items that restore mana on spell cast or crit. Also used to model replenishment.
                  + stats.ThreatReductionMultiplier       //bracing earthsiege diamond (metagem) effect
            ) > 0;
        }
        protected bool HasIgnoreStats(Stats stats)
        {
            // These stats automatically count as irrelevant.
            return (stats.Resilience
                  + stats.Agility
                  + stats.ArmorPenetration + stats.TargetArmorReduction
                  + stats.Strength + stats.AttackPower
                  + stats.Expertise + stats.ExpertiseRating
                  + stats.Dodge + stats.DodgeRating
                  + stats.Parry + stats.ParryRating
             > 0);
        }
        protected bool _HasRelevantStats(Stats stats)
        {
            return HasWarlockStats(stats) || (!HasIgnoreStats(stats) && HasCommonStats(stats));
        }
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            if (slot == ItemSlot.Ranged) return false;
            if (slot == ItemSlot.OffHand) return (enchant.Id == 4091);
            return base.EnchantFitsInSlot(enchant, character, slot);
        }
        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }
        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs
                    = new List<string>{
                        "Glyph of Metamorphosis",
                        "Glyph of Corruption",
                        "Glyph of Life Tap",
                        "Glyph of Bane of Agony",
                        "Glyph of Lash of Pain",
                        "Glyph of Shadowburn",
                        "Glyph of Unstable Affliction",
                        "Glyph of Haunt",
                        "Glyph of Chaos Bolt",
                        "Glyph of Immolate",
                        "Glyph of Incinerate",
                        "Glyph of Conflagrate",
                        "Glyph of Imp",
                        "Glyph of Felguard",
                        "Glyph of Shadow Bolt"};
            }
            return _relevantGlyphs;
        }
    }
}