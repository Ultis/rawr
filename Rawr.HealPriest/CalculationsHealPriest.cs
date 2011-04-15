using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Rawr.HealPriest
{
    [Rawr.Calculations.RawrModelInfo("HealPriest", "Spell_Holy_Renew", CharacterClass.Priest)]
    public class CalculationsHealPriest : CalculationsBase
    {
        #region Variables and Properties
        #endregion

        #region Gemming Templates
        protected string[] gemmingGroups = { "Common", "Rare", "Epic", "Jeweller" };
        protected string gemmingEngineer = "Engineering";
        protected string gemmingClass = "HealPriest";

        protected int GetRightGem(int[] gemList, int index)
        {
            if (gemList[index] == -1)
            {
                switch (index)
                {       // Do some twistarounds incase a special type of gem is missing.
                    case 0: return gemList[1];      // Common
                    case 1: return gemList[0];      // Rare
                    case 2: break;                  // Epic NYI
                    case 3: return gemList[1];      // JC
                    default: return -1;
                }
            }
            return gemList[index];
        }
        protected List<GemmingTemplate> MakeGemmingTemplatesFor(int[] red, int[] yellow, int[] blue, int[] prismatic, int[] meta, int enabler)
        {
            List<GemmingTemplate> gtRes = new List<GemmingTemplate>();

            for (int x = 0; x < gemmingGroups.Length; x++)
            {
                GemmingTemplate gt = new GemmingTemplate();
                gt.Model = gemmingClass;
                gt.Group = gemmingGroups[x];
                
                gt.RedId = GetRightGem(red, x);
                gt.YellowId = GetRightGem(yellow, x);
                gt.BlueId = GetRightGem(blue, x);
                gt.PrismaticId = prismatic[x];          // Always use the suggested gem in prismatic, as you would use JC gems in proper sockets
                gt.MetaId = meta[0];              
                gt.Enabled = (enabler == x);

                if (gt.RedId > 0 && gt.YellowId > 0 && gt.BlueId > 0 && gt.PrismaticId > 0 && gt.MetaId > 0)
                    gtRes.Add(gt);
            }

            return gtRes;
        }

        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Interesting Gem Choices for a Holy & Discipline Priest
                // Common, Rare, Epic, Jeweller, Engineer
                #region gems
                // Red
                int[] brilliant = { 52084, 52207, -1, 52257, -1 }; // +int

                // Yellow
                int[] fractured = { 52094, 52219, -1, 52269, 59480 }; // +mastery
                int[] mystic = { 52092, 52226, -1, 52267, 68660 }; // +resilience
                int[] quick = { 52093, 52232, -1, 52268, 59479 }; // +haste
                int[] smooth = { 52091, 52241, -1, 52266, 59478 }; // +crit

                // Orange
                int[] artful = { 52117, 52205, -1, -1, -1 }; // +int/+mastery
                int[] potent = { 52110, 52239, -1, -1, -1 }; // +int/+crit
                int[] reckless = { 52113, 52208, -1, -1, -1 }; // +int/+haste
                //int[] veiled = { 52104, 52217, -1, -1, -1 }; // +int/+hit
                int[] willfull = { -1, 68356, -1, -1, -1 }; // +int/+resilience


                // Green
                int[] forceful = { 52124, 52218, -1, -1, -1 }; // +haste/+sta
                int[] jagged = { -1, 52223, -1, -1, -1 }; // +crit/+stamina
                int[] lightning = { -1, 52225, -1, -1, -1 }; // +haste/+hit
                //int[] piercing = { 52122, 52228, -1, -1, -1 }; // +crit/+hit
                int[] puissant = { 52126, 52231, -1, -1, -1 }; // +mastery/+stamina
                //int[] senseis = { 52128, 52237, -1, -1, -1 }; // +mastery/+hit
                int[] steady = { 52123, 52245, -1, -1, -1 }; // +resilience/+stamina
                int[] vidid = { -1, 68741, -1, -1, -1 }; // +resilience/+spell pen
                int[] zen = { 52127, 52250, -1, -1, -1 }; // +mastery/+spirit

                // Blue
                // int[] rigid = { 52089, 52235, -1, 52264, 59493 }; // +hit
                int[] solid = { 52086, 52242, -1, 52261, -1 }; // +stamina
                int[] sparkling = { 52087, 52244, -1, 52262, 59496 }; // +spirit
                int[] stormy = { 52088, 52246, -1, 52263, -1 }; // +spell pen

                // Purple
                int[] purified = { 52100, 52236, -1, -1, -1 }; // +int/+spirit
                int[] timeless = { 52098, 52236, -1, -1, -1 }; // +int/+stamina
                //int[] veiled = { 52104, 52217, -1, -1, -1 }; // +int/hit

                // Meta
                int[] ember = { 52296 }; // +int/+2% int
                int[] fleet = { 52289 }; // +mastery/run speed
                int[] revitalizing = { 52297 }; // +mp5/+3% heal effect
                #endregion
                // Red, Yellow, Blue, Prismatic, Meta
                List<GemmingTemplate> gtList = new List<GemmingTemplate>();

                // Pure Intellect
                gtList.AddRange(MakeGemmingTemplatesFor(brilliant, brilliant, brilliant, brilliant, ember, 1));
                // Intellect > Haste and Spirit
                gtList.AddRange(MakeGemmingTemplatesFor(brilliant, reckless, purified, brilliant, ember, 1));
                // Intellect > Mastery and Spirit
                gtList.AddRange(MakeGemmingTemplatesFor(brilliant, artful, zen, brilliant, ember, 1));
                // Haste > Intellect and Spirit
                gtList.AddRange(MakeGemmingTemplatesFor(reckless, quick, purified, quick, ember, -1));
                // Mastery > Intellect and Spirit
                gtList.AddRange(MakeGemmingTemplatesFor(artful, fractured, zen, fractured, ember, -1));
                // Intellect > Crit & Spirit + Revitalizing
                gtList.AddRange(MakeGemmingTemplatesFor(brilliant, potent, purified, brilliant, revitalizing, 1));
                // Intellect > Haste & Spirit + Revitalizing
                gtList.AddRange(MakeGemmingTemplatesFor(brilliant, reckless, purified, brilliant, revitalizing, 1));

                gtList.Add(new GemmingTemplate()
                {
                    Model = gemmingClass,
                    Group = gemmingEngineer,
                    RedId = -1,
                    YellowId = -1,
                    BlueId = -1,
                    CogwheelId = fractured[4],
                    Cogwheel2Id = sparkling[4],
                    MetaId = ember[0],
                    Enabled = false,
                });
                gtList.Add(new GemmingTemplate()
                {
                    Model = gemmingClass,
                    Group = gemmingEngineer,
                    RedId = -1,
                    YellowId = -1,
                    BlueId = -1,
                    CogwheelId = fractured[4],
                    Cogwheel2Id = quick[4],
                    MetaId = ember[0],
                    Enabled = false,
                });
                gtList.Add(new GemmingTemplate()
                {
                    Model = gemmingClass,
                    Group = gemmingEngineer,
                    RedId = -1,
                    YellowId = -1,
                    BlueId = -1,
                    CogwheelId = sparkling[4],
                    Cogwheel2Id = quick[4],
                    MetaId = ember[0],
                    Enabled = false,
                });


                return gtList;
            }
        }

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffsAdd("Arcane Brilliance (Mana)");
            character.ActiveBuffsAdd("Blessing of Kings");
            character.ActiveBuffsAdd("Blessing of Might (Mp5)");
            character.ActiveBuffsAdd("Devotion Aura");
            character.ActiveBuffsAdd("Flask of the Draconic Mind");
            character.ActiveBuffsAdd("Inner Fire");
            character.ActiveBuffsAdd("Intellect Food");
            character.ActiveBuffsAdd("Power Word: Fortitude");
            character.ActiveBuffsAdd("Rampage");
            character.ActiveBuffsAdd("Resistance Aura");
            character.ActiveBuffsAdd("Totemic Wrath");
            character.ActiveBuffsAdd("Vampiric Touch");
            character.ActiveBuffsAdd("Wrath of Air Totem");
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                _subPointNameColors = new Dictionary<string, Color>();
                switch (_currentChartName)
                {
                    case "MP5 Sources":
                        _subPointNameColors.Add(string.Format("MP5 Sources ({0} total)", _currentChartTotal.ToString("0")), Color.FromArgb(255, 0, 0, 255));
                        break;
                    case "Spell HpS":
                        _subPointNameColors.Add("HpS", Color.FromArgb(255, 255, 0, 0));
                        break;
                    case "Spell HpM":
                        _subPointNameColors.Add("HpM", Color.FromArgb(255, 255, 0, 0));
                        break;
                    default:
                        _subPointNameColors.Add("HPS-Burst", Color.FromArgb(255, 255, 0, 0));
                        _subPointNameColors.Add("HPS-Sustained", Color.FromArgb(255, 0, 0, 255));
                        _subPointNameColors.Add("Survivability", Color.FromArgb(255, 0, 128, 0));
                        break;
                }
                _currentChartName = null;
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
                    "General:Health",
                    "General:Mana",
                    "General:Item Level",
//                    "General:Speed",
                    "Spell:Spell Power",
                    "Spell:Haste",
                    "Spell:Hit",
                    "Spell:Penetration",
                    "Spell:Mana Regen",
                    "Spell:Combat Regen",
                    "Spell:Crit Chance",
                    "Spell:Mastery",
                    "Model:Role",
                    "Model:Burst*This is the HPS you are expected to have if you are not limited by Mana.\r\nIn Custom Role, this displays your HPS when you dump all spells in 1 stream.",
                    "Model:Sustained*This is the HPS are expected to have when restricted by Mana.\r\nIf this value is lower than your Burst HPS, you are running out of mana in the simulation.\r\nIn Custom Role, this displays your HPS over the length of the fight, adjusted by the amount of mana available.",
                    "Holy Spells:Heal",
                    "Holy Spells:Binding Heal",
                    "Holy Spells:Greater Heal",
                    "Holy Spells:Flash Heal",
                    "Holy Spells:Renew",
                    "Holy Spells:ProM*Prayer of Mending",
                    "Holy Spells:ProM 5 Hits*Prayer of Mending hitting 5 targets",
                    "Holy Spells:PWS*Power Word Shield",
                    "Holy Spells:ProH*Prayer of Healing",
                    "Holy Spells:Holy Nova",
                    "Holy Spells:Lightwell",
                    "Holy Spells:CoH*Circle of Healing",
                    "Holy Spells:Penance",
                    "Holy Spells:HW Serenity*Holy Word Serenity",
                    "Holy Spells:HW Sanctuary*Holy Word Sanctuary",
                    "Holy Spells:Gift of the Naaru",
                    "Holy Spells:Divine Hymn",
                    "Holy Spells:Resurrection",
                    "Shadow Spells:Halleluja",
                    "Attributes:Strength",
                    "Attributes:Agility",
                    "Attributes:Stamina",
                    "Attributes:Intellect",
                    "Attributes:Spirit",
                    "Defense:Armor",
                    "Defense:Dodge",
                    "Defense:Resilience",
                    "Resistance:Arcane",
                    "Resistance:Fire",
                    "Resistance:Nature",
                    "Resistance:Frost",
                    "Resistance:Shadow",
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
                    "Resilience",
                    "Mana",
                    "Mana Regen",
                    "Combat Regen",
                    "Haste Rating",
                    "Mastery Rating",
                    "Haste %",
                    "Renew Ticks",
                    "Crit Rating",
                    //"Spell Crit %",
                    //"PW:Shield",
                    //"GHeal Avg",
                    //"FHeal Avg",
                    //"CoH Avg",
                    "Armor",
                    "Arcane Resistance",
                    "Fire Resistance",
                    "Frost Resistance",
                    "Nature Resistance",
                    "Shadow Resistance",
                    };
                return _optimizableCalculationLabels;
            }
        }

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel { get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelHealPriest()); } }

        public override CharacterClass TargetClass { get { return CharacterClass.Priest; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHealPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHealPriest(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHealPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml.Replace("CalculationOptionsPriest", "CalculationOptionsHealPriest").Replace("CalculationOptionsHolyPriest", "CalculationOptionsHealPriest"));
            CalculationOptionsHealPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsHealPriest;
            return calcOpts;
        }

        #endregion

        #region Relevancy
        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Circle of Healing");
                _relevantGlyphs.Add("Glyph of Flash Heal");
                _relevantGlyphs.Add("Glyph of Guardian Spirit");
                _relevantGlyphs.Add("Glyph of Holy Nova");
                _relevantGlyphs.Add("Glyph of Hymn of Hope");
                _relevantGlyphs.Add("Glyph of Inner Fire");
                _relevantGlyphs.Add("Glyph of Lightwell");
                _relevantGlyphs.Add("Glyph of Mass Dispel");
                _relevantGlyphs.Add("Glyph of Penance");
                _relevantGlyphs.Add("Glyph of Power Word: Shield");
                _relevantGlyphs.Add("Glyph of Prayer of Healing");
                _relevantGlyphs.Add("Glyph of Renew");
                _relevantGlyphs.Add("Glyph of Fading");
                _relevantGlyphs.Add("Glyph of Power Word: Barrier");
                _relevantGlyphs.Add("Glyph of Prayer of Mending");
                _relevantGlyphs.Add("Glyph of Divine Accuracy");
                _relevantGlyphs.Add("Glyph of Smite");

            }
            return _relevantGlyphs;
        }

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]{
                        ItemType.None,
                        ItemType.Cloth,
                        ItemType.Dagger,
                        ItemType.Wand,
                        ItemType.OneHandMace,
                        ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot)
        {
            if (slot == ItemSlot.Ranged) return false;
            if (enchant.ShieldsOnly) return false;
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique)
        {
            if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                Health = stats.Health,
                Mana = stats.Mana,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,

                SpellHaste = stats.SpellHaste,
                SpellCrit = stats.SpellCrit,

                Resilience = stats.Resilience,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                MasteryRating = stats.MasteryRating,

                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                BonusManaMultiplier = stats.BonusManaMultiplier,
                BonusCritHealMultiplier = stats.BonusCritHealMultiplier,

                //SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                HealingReceivedMultiplier = stats.HealingReceivedMultiplier,
                BonusHealingDoneMultiplier = stats.BonusHealingDoneMultiplier,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusManaPotionEffectMultiplier = stats.BonusManaPotionEffectMultiplier,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                PriestInnerFire = stats.PriestInnerFire,
                Healed = stats.Healed,

                ManaRestore = stats.ManaRestore,
                SpellsManaCostReduction = stats.SpellsManaCostReduction,
                HolySpellsManaCostReduction = stats.HolySpellsManaCostReduction,
                HighestStat = stats.HighestStat,
                ShieldFromHealedProc = stats.ShieldFromHealedProc,

                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Agility = stats.Agility,
                Strength = stats.Strength,
                ArcaneResistance = stats.ArcaneResistance,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                FireResistance = stats.FireResistance,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistance = stats.FrostResistance,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                NatureResistance = stats.NatureResistance,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                ShadowResistance = stats.ShadowResistance,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,

                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                MovementSpeed = stats.MovementSpeed,
            };

            foreach (SpecialEffect se in stats.SpecialEffects())
                if (RelevantTrinket(se))
                    s.AddSpecialEffect(se);
            return s;
        }

        protected bool RelevantTrinket(SpecialEffect se)
        {
            if (se.Trigger == Trigger.HealingSpellCast
                || se.Trigger == Trigger.HealingSpellCrit
                || se.Trigger == Trigger.HealingSpellHit
                || se.Trigger == Trigger.SpellCast
                || se.Trigger == Trigger.SpellCrit
                || se.Trigger == Trigger.Use)
            {
                return _HasRelevantStats(se.Stats) || (se.Stats._rawSpecialEffectDataSize > 0 && _HasRelevantStats(se.Stats._rawSpecialEffectData[0].Stats));
            }
            return false;
        }

        protected bool _HasRelevantStats(Stats stats)
        {
            bool Yes = (
                stats.Intellect + stats.Spirit + stats.Mana + stats.Mp5 + stats.SpellPower
                + stats.SpellHaste + stats.SpellCrit + stats.HasteRating + stats.CritRating + stats.MasteryRating
                + stats.BonusSpellPowerMultiplier
                + stats.BonusIntellectMultiplier + stats.BonusSpiritMultiplier + stats.BonusManaMultiplier + stats.BonusCritHealMultiplier
                + stats.HealingReceivedMultiplier + stats.BonusHealingDoneMultiplier + stats.BonusManaPotionEffectMultiplier
                + stats.ManaRestoreFromMaxManaPerSecond + stats.PriestInnerFire
                + stats.Healed

                + stats.ManaRestore + stats.SpellsManaCostReduction + stats.HolySpellsManaCostReduction + stats.HighestStat
                + stats.ShieldFromHealedProc
            ) != 0;

            bool Maybe = (
                stats.Stamina + stats.Health + stats.Resilience
                + stats.Armor + stats.BonusArmor + stats.Agility +
                + stats.SpellHit + stats.HitRating
                + stats.ArcaneResistance + stats.ArcaneResistanceBuff
                + stats.FireResistance + stats.FireResistanceBuff
                + stats.FrostResistance + stats.FrostResistanceBuff
                + stats.NatureResistance + stats.NatureResistanceBuff
                + stats.ShadowResistance + stats.ShadowResistanceBuff
                + stats.SnareRootDurReduc + stats.FearDurReduc + stats.StunDurReduc + stats.MovementSpeed
            ) != 0;

            bool No = (
                stats.Strength + stats.AttackPower
                + stats.ArmorPenetration + stats.TargetArmorReduction
                + stats.ExpertiseRating
                + stats.Dodge + stats.DodgeRating
                + stats.Parry + stats.ParryRating
                + stats.PhysicalHit
            ) != 0;

            return Yes || (Maybe && !No);
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool isRelevant = _HasRelevantStats(stats);

            foreach (SpecialEffect se in stats.SpecialEffects())
                isRelevant |= RelevantTrinket(se);
            return isRelevant;
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsHealPriest calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            //float hasRelevantBuff;

            #region Passive Ability Auto-Fixing
            // Removes the Trueshot Aura Buff and it's equivalents Unleashed Rage and Abomination's Might if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            /*{
                hasRelevantBuff = character.HunterTalents.TrueshotAura;
                Buff a = Buff.GetBuffByName("Trueshot Aura");
                Buff b = Buff.GetBuffByName("Unleashed Rage");
                Buff c = Buff.GetBuffByName("Abomination's Might");
                if (hasRelevantBuff > 0)
                {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); removedBuffs.Add(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); removedBuffs.Add(b); }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); removedBuffs.Add(c); }
                }
            }
            // Removes the Hunter's Mark Buff and it's Children 'Glyphed', 'Improved' and 'Both' if you are
            // maintaining it yourself. We are now calculating this internally for better accuracy and to provide
            // value to relevant talents
            {
                hasRelevantBuff =  character.HunterTalents.ImprovedHuntersMark
                                + (character.HunterTalents.GlyphOfHuntersMark ? 1 : 0);
                Buff a = Buff.GetBuffByName("Hunter's Mark");
                Buff b = Buff.GetBuffByName("Glyphed Hunter's Mark");
                Buff c = Buff.GetBuffByName("Improved Hunter's Mark");
                Buff d = Buff.GetBuffByName("Improved and Glyphed Hunter's Mark");
                // Since we are doing base Hunter's mark ourselves, we still don't want to double-dip
                if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); /*removedBuffs.Add(a);*//* }
                // If we have an enhanced Hunter's Mark, kill the Buff
                if (hasRelevantBuff > 0) {
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); /*removedBuffs.Add(b);*//* }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); /*removedBuffs.Add(c);*//* }
                    if (character.ActiveBuffs.Contains(d)) { character.ActiveBuffs.Remove(d); /*removedBuffs.Add(c);*//* }
                }
            }*/
            #endregion

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs, character.SetBonusCount);

            foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs) {
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }

        #endregion

        #region Custom Charts

        private string _currentChartName = null;
        private float _currentChartTotal = 0;

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { "Mana Regen Sources" }; // , "Spell HpS", "Spell HpM", "Spell AoE HpS", "Spell AoE HpM"}; //, "Relative Stat Values" };
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            ComparisonCalculationBase comparison;
            CalculationOptionsHealPriest calcOpts = character.CalculationOptions as CalculationOptionsHealPriest;
            if (calcOpts == null)
                return null;
            CharacterCalculationsHealPriest calcs = GetCharacterCalculations(character) as CharacterCalculationsHealPriest;
            if (calcs == null)
                return null;

            _currentChartTotal = 0;
            switch (chartName)
            {
                case "Mana Regen Sources":
                    _currentChartName = chartName;
                    PriestSolver solver = new PriestSolverDisciplineRaid(calcs, calcOpts, true);
                    solver.Solve();
                    
                    foreach (ManaSource manaSource in solver.ManaSources)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = manaSource.Name;
                        comparison.SubPoints[1] = manaSource.Value * 5f; // Convert to Mp5
                        _currentChartTotal += comparison.SubPoints[1];
                        comparison.OverallPoints = comparison.SubPoints[1];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                #region old old old
                /*case "Spell AoE HpS":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHealPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 5));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new HolyNova(p.BasicStats, character, 5));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if(spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpS;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Spell AoE HpM":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHealPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 5));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 5));
                    spellList.Add(new HolyNova(p.BasicStats, character, 5));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if (spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpM;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Spell HpS":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHealPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 1));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new HolyNova(p.BasicStats, character, 1));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if (spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpS;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Spell HpM":
                    _currentChartName = chartName;
                    p = GetCharacterCalculations(character) as CharacterCalculationsHealPriest;
                    spellList = new List<Spell>();
                    spellList.Add(new Renew(p.BasicStats, character));
                    spellList.Add(new FlashHeal(p.BasicStats, character));
                    spellList.Add(new Heal(p.BasicStats, character));
                    spellList.Add(new PrayerOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new BindingHeal(p.BasicStats, character));
                    spellList.Add(new PrayerOfMending(p.BasicStats, character, 1));
                    spellList.Add(new CircleOfHealing(p.BasicStats, character, 1));
                    spellList.Add(new HolyNova(p.BasicStats, character, 1));
                    spellList.Add(new Penance(p.BasicStats, character));
                    spellList.Add(new PowerWordShield(p.BasicStats, character));

                    foreach (Spell spell in spellList)
                    {
                        if (spell.AvgHeal == 0)
                            continue;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.Equipped = false;
                        comparison.SubPoints[0] = spell.HpM;
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                    */
                #endregion
                default:
                    _currentChartName = null;
                    return new ComparisonCalculationBase[0];
            }
        }

        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsHealPriest calc = new CharacterCalculationsHealPriest();
            if (character == null) { return calc; }
            CalculationOptionsHealPriest calcOpts = character.CalculationOptions as CalculationOptionsHealPriest;
            if (calcOpts == null) { return calc; }
            //
            Stats stats = GetCharacterStats(character, additionalItem);

            calc.BasicStats = stats;
            calc.Character = character;

            PriestSolver solver = new PriestSolverDisciplineRaid(calc, calcOpts, needsDisplayCalculations);
            solver.Solve();
            
            return calc;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            
            CalculationOptionsHealPriest calcOpts = character.CalculationOptions as CalculationOptionsHealPriest;
            Stats statsRace = BaseStats.GetBaseStats(character);
            statsRace.BonusIntellectMultiplier = 0.05f;     // Cloth bonus
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character, calcOpts);

            ePriestSpec ps = PriestSpec.GetPriestSpec(character.PriestTalents);
            if (ps == ePriestSpec.Spec_ERROR)
                throw new Exception("Unpossible Talent Spec!");

            Stats statsTalents = new Stats()
            {
                BonusIntellectMultiplier = (ps == ePriestSpec.Spec_Disc) ? 0.15f : 0f,
                BonusHealingDoneMultiplier = (ps == ePriestSpec.Spec_Holy) ? 0.15f : 0f,
                SpellHaste = character.PriestTalents.Darkness * 0.01f,
                SpellCombatManaRegeneration = 0.5f + character.PriestTalents.HolyConcentration * 0.15f,
            };

            Stats statsTotal = statsBaseGear + statsBuffs + statsRace + statsTalents;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor((statsTotal.Intellect) * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.SpellPower += (statsTotal.PriestInnerFire > 0 ? PriestInformation.GetInnerFireSpellPowerBonus(character) : 0) + (statsTotal.Intellect - 10);
            statsTotal.SpellPower *= (1f + statsTotal.BonusSpellPowerMultiplier);
            statsTotal.Mana += StatConversion.GetManaFromIntellect(statsTotal.Intellect);
            statsTotal.Mana *= (1f + statsTotal.BonusManaMultiplier);
            statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);
            statsTotal.Health = (float)Math.Floor(statsTotal.Health * (1f + statsTotal.BonusHealthMultiplier));
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect)
                + StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            statsTotal.SpellHaste = (1f + statsTotal.SpellHaste) * (1f + StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating)) - 1f;
            statsTotal.Armor *= (1 + (statsTotal.PriestInnerFire > 0 ? PriestInformation.GetInnerFireArmorBonus(character) : 0));
            return statsTotal;
        }

    }
}
