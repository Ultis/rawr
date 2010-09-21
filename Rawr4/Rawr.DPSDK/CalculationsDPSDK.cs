using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;
using Rawr.DK;

namespace Rawr.DPSDK
{
    [Rawr.Calculations.RawrModelInfo("DPSDK", "spell_deathknight_classicon", CharacterClass.DeathKnight)]
    public class CalculationsDPSDK : CalculationsBase
    {
        public float BonusMaxRunicPower = 0f;

        enum ability
        {
            // Basic
            White,
            // Melee abilities
            BCB,
            BloodStrike,
            DeathStrike,
            FesteringStrike,
            FrostStrike,
            HeartStrike,
            NecroticStrike,
            Obliterate,
            PlagueStrike,
            RuneStrike,
            ScourgeStrike,
            // Ranged/AOE abilities
            BloodBoil,
            DeathCoil,
            DeathNDecay,
            HowlingBlast,
            IcyTouch,
            // Others
            BloodParasite,
            BloodPlague,
            DRW,
            FrostFever,
            Gargoyle,
            Ghoul,
            UnholyBlight,
            WanderingPlague,
            OtherPhysical,
            OtherHoly,
            OtherArcane,
            OtherFire,
            OtherFrost,
            OtherNature,
            OtherShadow,
        }
        private float[] dpsSub = new float[EnumHelper.GetCount(typeof(ability))];
        public float Mastery { get; set; }
        public float BonusRPMultiplier { get; set; }
        public float BonusRuneRegeneration { get; set; }

        //public static double hawut = new Random().NextDouble() * DateTime.Now.ToOADate();
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs for DPSDKs

                //Red
                int[] bold = { 39900, 39996, 40111, 42142 };
                int[] fractured = { 39909, 40002, 40117, 42153 };

                //Purple
                int[] sovereign = { 39934, 40022, 40129 };

                //Orange
                int[] inscribed = { 39947, 40037, 40142 };

                // Prismatic 
                int[] tear = { 42702, 42702, 49110 };

                //Meta
                int chaotic = 41285;
                int relentless = 41398;

                return new List<GemmingTemplate>()
				{
					new GemmingTemplate() { Model = "DPSDK", Group = "Rare", //Max Strength
						RedId = bold[1], YellowId = bold[1], BlueId = bold[1], PrismaticId = bold[1], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Rare", //Max Arp
						RedId = fractured[1], YellowId = fractured[1], BlueId = fractured[1], PrismaticId = fractured[1], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Rare", //Strength
						RedId = bold[1], YellowId = inscribed[1], BlueId = sovereign[1], PrismaticId = bold[1], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Rare", //Relentless
						RedId = bold[1], YellowId = bold[1], BlueId = bold[1], PrismaticId = tear[1], MetaId = relentless },
						
					new GemmingTemplate() { Model = "DPSDK", Group = "Epic", Enabled = true, //Max Strength
						RedId = bold[2], YellowId = bold[2], BlueId = bold[2], PrismaticId = bold[2], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Rare", //Max Arp
						RedId = fractured[2], YellowId = fractured[2], BlueId = fractured[2], PrismaticId = fractured[2], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Epic", Enabled = true, //Strength
						RedId = bold[2], YellowId = inscribed[2], BlueId = sovereign[2], PrismaticId = bold[2], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Epic", Enabled = true, //Relentless
						RedId = bold[2], YellowId = inscribed[2], BlueId = sovereign[2], PrismaticId = tear[2], MetaId = relentless },

					new GemmingTemplate() { Model = "DPSDK", Group = "Jeweler", //Max Strength
						RedId = bold[3], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[3], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Rare", //Max Arp
						RedId = fractured[3], YellowId = fractured[3], BlueId = fractured[3], PrismaticId = fractured[3], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSDK", Group = "Jeweler", //Strength
						RedId = bold[2], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[2], MetaId = chaotic },
				};
            }
        }

        public static float AddStatMultiplierStat(float statMultiplier, float newValue)
        {
            float updatedStatModifier = ((1 + statMultiplier) * (1 + newValue)) - 1f;
            return updatedStatModifier;
        }
        
        private Dictionary<string, Color> _subPointNameColors = null;
        /// <summary>
        /// Dictionary<string, Color> that includes the names of each rating which your model will use,
        /// and a color for each. These colors will be used in the charts.
        /// 
        /// EXAMPLE: 
        /// subPointNameColors = new Dictionary<string, System.Drawing.Color>();
        /// subPointNameColors.Add("Mitigation", System.Drawing.Colors.Red);
        /// subPointNameColors.Add("Survival", System.Drawing.Colors.Blue);
        /// </summary>
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("DPS", System.Windows.Media.Color.FromArgb(255,0,0,255));
                }
                return _subPointNameColors;
            }
        }


        private string[] _characterDisplayCalculationLabels = null;
        /// <summary>
        /// An array of strings which will be used to build the calculation display.
        /// Each string must be in the format of "Heading:Label". Heading will be used as the
        /// text of the group box containing all labels that have the same Heading.
        /// Label will be the label of that calculation, and may be appended with '*' followed by
        /// a description of that calculation which will be displayed in a tooltip for that label.
        /// Label (without the tooltip string) must be unique.
        /// 
        /// EXAMPLE:
        /// characterDisplayCalculationLabels = new string[]
        /// {
        ///		"Basic Stats:Health",
        ///		"Basic Stats:Armor",
        ///		"Advanced Stats:Dodge",
        ///		"Advanced Stats:Miss*Chance to be missed"
        /// };
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    List<string> labels = new List<string>(new string[] {
                        "Basic Stats:Health",
					    "Basic Stats:Strength",
					    "Basic Stats:Agility",
					    "Basic Stats:Attack Power",
					    "Basic Stats:Crit Rating",
					    "Basic Stats:Hit Rating",
					    "Basic Stats:Expertise",
					    "Basic Stats:Haste Rating",
                        "Basic Stats:Armor",
                        "Basic Stats:Mastery",
					    "Advanced Stats:Weapon Damage*Damage before misses and mitigation",
					    "Advanced Stats:Attack Speed",
					    "Advanced Stats:Crit Chance",
					    "Advanced Stats:Avoided Attacks",
					    "Advanced Stats:Enemy Mitigation",
                        "DPS Breakdown:White",
                        "DPS Breakdown:BCB*Blood Caked Blade",
                        "DPS Breakdown:Necrosis",
                        "DPS Breakdown:Death Coil",
                        "DPS Breakdown:Icy Touch",
                        "DPS Breakdown:Plague Strike",
                        "DPS Breakdown:Frost Fever",
                        "DPS Breakdown:Blood Plague",
                        "DPS Breakdown:Scourge Strike",
                        "DPS Breakdown:Unholy Blight",
                        "DPS Breakdown:Frost Strike",
                        "DPS Breakdown:Howling Blast",
                        "DPS Breakdown:Obliterate",
                        "DPS Breakdown:Death Strike",
                        "DPS Breakdown:Blood Strike",
                        "DPS Breakdown:Heart Strike",
                        "DPS Breakdown:DRW*Dancing Rune Weapon",
                        "DPS Breakdown:Gargoyle",
                        "DPS Breakdown:Wandering Plague",
                        "DPS Breakdown:Ghoul",
                        "DPS Breakdown:Bloodparasite",
                        "DPS Breakdown:Other",
                        "DPS Breakdown:Total DPS",
                    });
                    _characterDisplayCalculationLabels = labels.ToArray();
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) 
                { 
                    _customChartNames = new string[] 
                        { 
                            "Item Budget (10 point steps)", 
                            "Item Budget (25 point steps)", 
                            "Item Budget (50 point steps)", 
                            "Item Budget (100 point steps)",
                            "Presences"
                        }; }
                return _customChartNames;
            }
        }

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelDPSDK()); }
        }

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes {
            get {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<ItemType>(new ItemType[] {
						ItemType.None,
                        ItemType.Leather,
                        ItemType.Mail,
                        ItemType.Plate,
                        ItemType.Sigil,
                        ItemType.Polearm,
                        ItemType.TwoHandAxe,
                        ItemType.TwoHandMace,
                        ItemType.TwoHandSword,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword
					}));
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.DeathKnight; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationDPSDK(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsDPSDK(); }
        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsDPSDK));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsDPSDK calcOpts = serializer.Deserialize(reader) as CalculationOptionsDPSDK;
            return calcOpts;
        }

        /// <summary>
        /// GetCharacterCalculations is the primary method of each model, where a majority of the calculations
        /// and formulae will be used. GetCharacterCalculations should call GetCharacterStats(), and based on
        /// those total stats for the character, and any calculationoptions on the character, perform all the 
        /// calculations required to come up with the final calculations defined in 
        /// CharacterDisplayCalculationLabels, including an Overall rating, and all Sub ratings defined in 
        /// SubPointNameColors.
        /// </summary>
        /// <param name="character">The character to perform calculations for.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A custom CharacterCalculations object which inherits from CharacterCalculationsBase,
        /// containing all of the final calculations defined in CharacterDisplayCalculationLabels. See
        /// CharacterCalculationsBase comments for more details.</returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            Stats stats = new Stats();
            CharacterCalculationsDPSDK calcs = new CharacterCalculationsDPSDK();
            DeathKnightTalents talents = character.DeathKnightTalents;

            // Setup initial Boss data.
            // Get Boss from BossOptions data.
            BossOptions hBossOptions = new BossOptions();
            int targetLevel = hBossOptions.Level;

            stats = GetCharacterStats(character, additionalItem);
            calcs.BasicStats = stats;

            CombatTable2 combatTable = new CombatTable2(character, stats, calcs, calcOpts/*, additionalItem*/);

            calcs.OverallPoints = calcs.DPSPoints;

            return calcs;
        }

        public CharacterCalculationsBase GetCharacterCalculationsExp(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // Setup what we need.
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            int targetLevel = calcOpts.TargetLevel; // This needs to get from BossHandler.

            Stats stats = new Stats();

            CharacterCalculationsDPSDK calcs = new CharacterCalculationsDPSDK();
            // Build up Character Stats to do math

            // Perform the calculations
            CombatTable combatTable = new CombatTable(character, calcs, stats, calcOpts);


            // Output the results.

            return calcs;
        }

        private Stats GetRaceStats(Character character) 
        {
            return BaseStats.GetBaseStats(character.Level, CharacterClass.DeathKnight, character.Race);
        }

        public Stats GetPresenceStats(CalculationOptionsDPSDK.Presence p,  DeathKnightTalents t)
        {
            BonusRPMultiplier = 0;
            BonusRuneRegeneration = 0f;
            Stats PresenceStats = new Stats();
            switch(p)
            {
                case CalculationOptionsDPSDK.Presence.Blood:
                {
                    if (t.ImprovedBloodPresence > 0)
                        PresenceStats.CritChanceReduction -= 0.03f * t.ImprovedBloodPresence;
                    else if (t.ImprovedFrostPresence > 0)
                        BonusRPMultiplier += .02f * t.ImprovedFrostPresence;
                    else if (t.ImprovedUnholyPresence == 1)
                        PresenceStats.MovementSpeed += .08f;
                    else if (t.ImprovedUnholyPresence == 2)
                        PresenceStats.MovementSpeed += .15f;
                    PresenceStats.BonusStaminaMultiplier += 0.08f;
                    PresenceStats.BaseArmorMultiplier += 0.6f;
                    PresenceStats.DamageTakenMultiplier -= 0.08f;
                    // Threat bonus.
                    PresenceStats.ThreatIncreaseMultiplier += 2f; // TODO: NOT VERIFIED AT ALL
                    break;
                }
                case CalculationOptionsDPSDK.Presence.Frost:
                {
                    if (t.ImprovedBloodPresence > 0)
                        PresenceStats.DamageTakenMultiplier -= 0.02f * t.ImprovedBloodPresence;
                    else if (t.ImprovedUnholyPresence == 1)
                        PresenceStats.MovementSpeed += .08f;
                    else if (t.ImprovedUnholyPresence == 2)
                        PresenceStats.MovementSpeed += .15f;
                    PresenceStats.BonusDamageMultiplier += 0.1f;
                    BonusRPMultiplier += 0.1f;  // TODO: Put this into a stats object or something.
                    PresenceStats.ThreatReductionMultiplier += .20f; // Wowhead has this as effect #3
                    break;
                }
                case CalculationOptionsDPSDK.Presence.Unholy:
                {
                    if (t.ImprovedBloodPresence > 0)
                        PresenceStats.DamageTakenMultiplier -= 0.02f * t.ImprovedBloodPresence;
                    else if (t.ImprovedFrostPresence > 0)
                        BonusRPMultiplier += .02f * t.ImprovedFrostPresence;
                    PresenceStats.PhysicalHaste += .1f;
                    BonusRuneRegeneration += .1f;
                    PresenceStats.MovementSpeed += .15f;
                    PresenceStats.ThreatReductionMultiplier += .20f; // Wowhead has this as effect #3
                    break;
                }
            }
            return PresenceStats;
        }

        /// <summary>
        /// GetCharacterStats is the 2nd-most calculation intensive method in a model. Here the model will
        /// combine all of the information about the character, including race, gear, enchants, buffs,
        /// calculationoptions, etc., to form a single combined Stats object. Three of the methods below
        /// can be called from this method to help total up stats: GetItemStats(character, additionalItem),
        /// GetEnchantsStats(character), and GetBuffsStats(character.ActiveBuffs).
        /// </summary>
        /// <param name="character">The character whose stats should be totaled.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A Stats object containing the final totaled values of all character stats.</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem) {
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            DeathKnightTalents talents = character.DeathKnightTalents;

            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);

            // Filter out the duplicate non-stacking Rune Enchants:
            if (character.OffHandEnchant == Enchant.FindEnchant(3368, ItemSlot.OneHand, character)
                && character.MainHandEnchant == character.OffHandEnchant)
            {
                bool bFC1Found = false;
                bool bFC2Found = false;
                foreach (SpecialEffect se1 in statsBaseGear.SpecialEffects())
                {
                    // if we've already found them, and we're seeing them again, then remove these repeats.
                    if (bFC1Found && _SE_FC1.Stats.Equals(se1.Stats) && _SE_FC1.Trigger.Equals(se1.Trigger))
                        statsBaseGear.RemoveSpecialEffect(se1);
                    else if (bFC2Found && _SE_FC2.Stats.Equals(se1.Stats) && _SE_FC2.Trigger.Equals(se1.Trigger))
                        statsBaseGear.RemoveSpecialEffect(se1);
                    else if (_SE_FC1.Stats.Equals(se1.Stats) && _SE_FC1.Trigger.Equals(se1.Trigger))
                        bFC1Found = true;
                    else if (_SE_FC2.Stats.Equals(se1.Stats) && _SE_FC2.Trigger.Equals(se1.Trigger))
                        bFC2Found = true;
                }
            }

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsPresence = GetPresenceStats(calcOpts.CurrentPresence, talents);
            Stats statsTalents = GetTalentStats(character);

            Stats statsTotal = new Stats();
            statsTotal.Accumulate(statsBaseGear);
            statsTotal.Accumulate(statsBuffs);
            statsTotal.Accumulate(statsRace);
            statsTotal.Accumulate(statsPresence);
            statsTotal.Accumulate(statsTalents);

            statsTotal = GetRelevantStats(statsTotal);
            statsTotal.Expertise += (float)StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating);

            StatsSpecialEffects se = new StatsSpecialEffects(character, statsTotal, new CombatTable(character, statsTotal, calcOpts));
            Stats statSE = new Stats();
            foreach (SpecialEffect e in statsTotal.SpecialEffects())
            {
                // There are some multi-level special effects that need to be factored in.
                foreach (SpecialEffect ee in e.Stats.SpecialEffects())
                {
                    e.Stats = se.getSpecialEffects(calcOpts, ee);
                }
                statSE.Accumulate(se.getSpecialEffects(calcOpts, e));
            }

            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    se = new StatsSpecialEffects(character, statsTotal, new CombatTable(character, statsTotal, calcOpts));
                    statsTotal.Accumulate(se.getSpecialEffects(calcOpts, effect));
                }
            }

            statsTotal.Strength += statsTotal.HighestStat + statsTotal.Paragon + statsTotal.DeathbringerProc/3;
            statsTotal.HasteRating += statsTotal.DeathbringerProc/3;
            statsTotal.CritRating += statsTotal.DeathbringerProc/3;

            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Strength = (float)Math.Floor(statsTotal.Strength * (1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor(statsTotal.Spirit * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Health = (float)Math.Floor(statsTotal.Health + (statsTotal.Stamina * 10f));
            statsTotal.Mana = (float)Math.Floor(statsTotal.Mana + (statsTotal.Intellect * 15f));
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower + statsTotal.Strength * 2);
            statsTotal.Armor = (float)Math.Floor(StatConversion.GetArmorFromAgility(statsTotal.Agility) +
                                StatConversion.ApplyMultiplier(statsTotal.Armor, statsTotal.BaseArmorMultiplier) +
                                StatConversion.ApplyMultiplier(statsTotal.BonusArmor, statsTotal.BonusArmorMultiplier));

            statsTotal.BonusSpellPowerMultiplier++;
            statsTotal.BonusFrostDamageMultiplier++;
            statsTotal.BonusShadowDamageMultiplier++;

            statsTotal.AttackPower *= 1f + statsTotal.BonusAttackPowerMultiplier;

            statsTotal.BonusPhysicalDamageMultiplier++;

            return (statsTotal);
        }

        /*
        public Stats GetCharacterStatsMaximum(Character character, Item additionalItem, float abilityCooldown)
        {
            CalculationOptionsDPSDK calcOpts = character.CalculationOptions as CalculationOptionsDPSDK;
            DeathKnightTalents talents = character.DeathKnightTalents;
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsTalents = GetTalentStats(character);

            Stats statsTotal = new Stats();
            Stats statsGearEnchantsBuffs = new Stats();

            statsGearEnchantsBuffs = statsBaseGear + statsBuffs + statsRace + statsTalents;

            statsTotal = GetRelevantStats(statsGearEnchantsBuffs);
            statsTotal.Expertise += (float)StatConversion.GetExpertiseFromRating(statsGearEnchantsBuffs.ExpertiseRating);

            StatsSpecialEffects se = new StatsSpecialEffects(character, statsTotal, new CombatTable(character, statsTotal, calcOpts));
            int temp = 0;
            Stats statsTemp = new Stats();
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                statsTemp.AddSpecialEffect(effect);
            }
            foreach (SpecialEffect effect in statsTemp.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    if (effect.Trigger == Trigger.Use)
                    {
                        float uptimeMult = 0f;
                        if (effect.Cooldown > abilityCooldown)
                        {
                            for (int i = 0; i * effect.Cooldown < calcOpts.FightLength * 60f; i++)
                            {
                                if (i * effect.Cooldown % abilityCooldown == 0)
                                {
                                    uptimeMult++;
                                }
                            }
                            uptimeMult /= calcOpts.FightLength * 60f / abilityCooldown;
                            statsTotal += effect.Stats * uptimeMult;
                        }
                    }
                    else
                    {
                        statsTotal.AddSpecialEffect(effect);
                        temp++;
                    }
                }
            }
            return (statsTotal);
        }
        */

        public Stats GetTalentStats(Character c)
        {
            // TODO: This will eventually be in the common area.
            Stats TalentStats = new Stats();

            AccumulateTalents(TalentStats, c);

            return TalentStats;
        }

        public Rotation.Type TalentFocus(DeathKnightTalents t)
        {
            Rotation.Type curRotationType = Rotation.Type.Custom;
            const int indexBlood = 0; // start index of Blood Talents.
            const int indexFrost = 19; // start index of Frost Talents.
            const int indexUnholy = indexFrost + 19; // start index of Unholy Talents.
            //const int sizeUnholy = 20;
            int[] TalentCounter = new int[4];
            int index = indexBlood;
            foreach (int i in t.Data)
            {
                if (i > 0)
                {
                    // Blood
                    if (index < indexFrost)
                        TalentCounter[(int)Rotation.Type.Blood] += i;
                    // Frost
                    else if ((indexFrost <= index) && (index < indexUnholy))
                    {
                        TalentCounter[(int)Rotation.Type.Frost] += i;
                    }
                    // Unholy
                    else if (index >= indexUnholy)
                    {
                        TalentCounter[(int)Rotation.Type.Unholy] += i;
                    }
                }
                index++;
            }
            if ((TalentCounter[(int)Rotation.Type.Blood] > TalentCounter[(int)Rotation.Type.Frost]) && (TalentCounter[(int)Rotation.Type.Blood] > TalentCounter[(int)Rotation.Type.Unholy]))
            {
                // Blood
                curRotationType = Rotation.Type.Blood;
            }
            else if ((TalentCounter[(int)Rotation.Type.Frost] > TalentCounter[(int)Rotation.Type.Blood]) && (TalentCounter[(int)Rotation.Type.Frost] > TalentCounter[(int)Rotation.Type.Unholy]))
            {
                // Frost
                curRotationType = Rotation.Type.Frost;
            }
            else if ((TalentCounter[(int)Rotation.Type.Unholy] > TalentCounter[(int)Rotation.Type.Frost]) && (TalentCounter[(int)Rotation.Type.Unholy] > TalentCounter[(int)Rotation.Type.Blood]))
            {
                // Unholy
                curRotationType = Rotation.Type.Unholy;
            }
            return curRotationType;
        }

        /// <summary>Build the talent effects.</summary>
        private void AccumulateTalents(Stats FullCharacterStats, Character character)
        {
            Stats newStats = new Stats();

            // Which talent tree focus?
            #region Talent Speciality
            Rotation.Type r = TalentFocus(character.DeathKnightTalents);
            switch (r)
            {
                case Rotation.Type.Blood:
                    {
                        // Special abilities for being blood
                        // Heart Strike
                        // Veteran of the third war
                        // Stamina +9%
                        FullCharacterStats.BonusStaminaMultiplier += .09f;
                        // Expertise +6
                        FullCharacterStats.Expertise += 6;
                        // Blood Rites
                        // Whenever you hit with Death Strike or Obliterate, the Frost and Unholy Runes 
                        // will become Death Runes when they activate.  Death Runes count as a Blood, Frost or Unholy Rune.
                        // Vengence
                        // Each time you take damage, you gain 5% of the damage taken as attack power, up to a maximum of 10% of your health.
                        // Mastery: Blood Shield
                        // Each Time you heal yourself w/ DS you gain a shield worth 50% of the amount healed
                        // Each Point of Mastery increases the shield by 6.25%
                        break;
                    }
                case Rotation.Type.Frost:
                    {
                        // Special abilities for being Frost
                        // Frost Strike
                        // Icy Talons
                        // Melee Attack speed +20%
                        FullCharacterStats.PhysicalHaste += .2f;
                        // Blood of the North
                        // Whenever you hit with Blood Strike or Pest, your blood rune will become a death rune.
                        // Mastery: Frozen Heart
                        // Increases all frost damage by 16%.  
                        FullCharacterStats.BonusFrostDamageMultiplier += .16f;
                        // Each point of mastery increases frost damage by an additional 2.0%
                        break;
                    }
                case Rotation.Type.Unholy:
                    {
                        // Special abilities for being Unholy
                        // Scourge Strike
                        // Master of Ghouls
                        // Reduces the CD on Raise dead by 60 sec.
                        // The ghoul summoned is considered your pet w/o a limited duration.
                        // Reaping
                        // Whenever you hit with Blood strike, pestilence, or Festering strike, the runes spent will 
                        // become death runes when they activate.
                        // Unholy Might
                        // Str +15%
                        FullCharacterStats.BonusStrengthMultiplier += .15f;
                        // Mastery: Blightcaller.
                        // Increases the damage done by your diseases by 32%.
                        FullCharacterStats.BonusDiseaseDamageMultiplier += .32f;
                        // Each point of mastery increases disease damage by an additional 4.0%
                        break;
                    }
            }
            #endregion

            #region Blood Talents
            // Butchery
            // 1RPp5 per Point
            if (character.DeathKnightTalents.Butchery > 0)
            {
                FullCharacterStats.RPp5 += 1 * character.DeathKnightTalents.Butchery;
            }

            // Blade Barrier
            // Reduce damage by 2% per point for 10 sec.
            if (character.DeathKnightTalents.BladeBarrier > 0)
            {
                // If you don't have your Blood Runes on CD, you're doing it wrong. 
                FullCharacterStats.DamageTakenMultiplier -= (.02f * character.DeathKnightTalents.BladeBarrier);
            }

            // Bladed Armor
            // 2 AP per point per 180 Armor
            if (character.DeathKnightTalents.BladedArmor > 0)
            {
                // If you don't have your Blood Runes on CD, you're doing it wrong. 
                FullCharacterStats.AttackPower += (2 * character.DeathKnightTalents.BladedArmor) * (FullCharacterStats.Armor / 180);
            }

            // Blood-Caked Blade
            // 10% chance per point to cause Blood-Caked strike
            
            // Scent of Blood
            // 15% after Dodge, Parry or damage received causing 1 melee hit per point to generate 10 runic power.
            // TODO: setup RP gains.

            if (r == Rotation.Type.Blood)
            {
                // Abominations Might
                // increase AP by 5%/10% of raid.
                // 1% per point increase to str.
                if (character.DeathKnightTalents.AbominationsMight > 0)
                {
                    // This happens no matter what:
                    FullCharacterStats.BonusStrengthMultiplier += (0.01f * character.DeathKnightTalents.AbominationsMight);
                    // This happens only if there isn't Trueshot Aura/Unleashed Rage/Abom's might  available:
                    if (!(character.ActiveBuffsContains("Trueshot Aura") || character.ActiveBuffsContains("Unleashed Rage") || character.ActiveBuffsContains("Abomination's Might")))
                    {
                        FullCharacterStats.BonusAttackPowerMultiplier += (.05f * character.DeathKnightTalents.AbominationsMight);
                    }
                }

                // Bone Shield
                // 3 Bones 
                // Takes 20% less damage from all sources
                // Does 2% more damage to target
                // Each damaging attack consumes a bone.
                // Lasts 5 mins

                // Toughness
                // Increases Armor Value from items by 3% per point.
                if (character.DeathKnightTalents.Toughness > 0)
                {
                    FullCharacterStats.BaseArmorMultiplier = AddStatMultiplierStat(FullCharacterStats.BaseArmorMultiplier, (.03f * character.DeathKnightTalents.Toughness));
                }

                // Hand of Doom
                // Reduces the CD for Strangulate by 30/60 sec.

                // Sanguine Fortitude
                // Buff's IBF:
                // While Active, your IBF reduces Damage taken by 15/30% and costs 50/100% less RP to activate.
                // CD duration? 3min suggested on pwnwear.
                // Cost?  This is a CD stacker.
                // TODO: look into CD stacking code./max v average values.

                // Blood Parasite
                // Melee Attacks have 5% chance of spawning a blood worm.
                // Blood worm attacks your enemies, gorging itself on blood
                // until it bursts to heal nearby allies. Lasts 20 sec.
                if (character.DeathKnightTalents.BloodParasite > 0)
                {
                    // TODO: figure out how much damage the worms do.
                    // TODO: figure out how much healing the worms do.
                    float fDamageDone = 100f;
                    float fBWAttackSpeed = 2f;
                    float fBWDuration = 20f;
                    float key = (fDamageDone * fBWDuration / fBWAttackSpeed);
                    // note, while this only creates one Dictionary entry and may seem like a waste
                    // I left it open like this so that your above TODO for figuring out how much damage the worms do will make this part dynamic
                    if (!_SE_Bloodworms.ContainsKey(key))
                    {
                        _SE_Bloodworms.Add(key, new SpecialEffect[] {
                            null,
                            new SpecialEffect(Trigger.PhysicalHit, new Stats() { Healed = ((fDamageDone * fBWDuration / fBWAttackSpeed) * 1.5f) }, fBWDuration, 0, .05f * 1),
                            new SpecialEffect(Trigger.PhysicalHit, new Stats() { Healed = ((fDamageDone * fBWDuration / fBWAttackSpeed) * 1.5f) }, fBWDuration, 0, .05f * 2),
                        });
                    }
                    FullCharacterStats.AddSpecialEffect(_SE_Bloodworms[key][character.DeathKnightTalents.BloodParasite]);
                }

                // Improved Blood Presence
                // Reduces chance to be critically hit while in blood presence by 3/6%
                // In addition while in Frost or Unholy, retain the 2/4% Damage reduction. 
                // Implemented in GetPresenceStats()

                // Will of the Necropolis
                // Damage that takes you below 35% health or while at less than 35% is reduced by 5% per point.  
                if (character.DeathKnightTalents.WillOfTheNecropolis > 0)
                {
                    // Need to factor in the damage taken aspect of the trigger.
                    // Using the assumption that the tank will be at < 35% health about that % of the time.
                    FullCharacterStats.AddSpecialEffect(_SE_WillOfTheNecropolis[character.DeathKnightTalents.WillOfTheNecropolis]);
                }

                // Rune Tap
                // Convert 1 BR to 10% health.
                /*
                if (character.DeathKnightTalents.RuneTap > 0)
                {
                    newStats = new Stats();
                    float fCD = 60f;
                    newStats.Healed = (GetCurrentHealth(FullCharacterStats) * .1f);
                    // Improved Rune Tap.
                    // increases the health provided by RT by 33% per point. and lowers the CD by 10 sec per point
                    fCD -= (10f * character.DeathKnightTalents.ImprovedRuneTap);
                    newStats.Healed += (newStats.Healed * (character.DeathKnightTalents.ImprovedRuneTap / 3f));
                    FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, 0, fCD));
                }
                */

                // Vampiric Blood
                // temp 15% of max health and
                // increases health generated by 35% for 10 sec.
                // 1 min CD. as of 3.2.2
                /*
                if (character.DeathKnightTalents.VampiricBlood > 0)
                {
                    // Also copy above, but it's commented out.
                    newStats = new Stats();
                    newStats.Health = (GetCurrentHealth(FullCharacterStats) * 0.15f);
                    newStats.HealingReceivedMultiplier += 0.35f;

                    float fVBCD = 60f;
                    if (m_bT9_4PC) fVBCD -= 10f;
                    float fVBDur = 10f;
                    if (character.DeathKnightTalents.GlyphofVampiricBlood == true)
                    {
                        fVBDur += 5f;
                    }
                    FullCharacterStats.AddSpecialEffect(new SpecialEffect(Trigger.Use, newStats, fVBDur, fVBCD));
                }
                */

                // Improved Death Strike
                if (character.DeathKnightTalents.ImprovedDeathStrike > 0)
                {
                    FullCharacterStats.BonusDeathStrikeCrit += (.03f * character.DeathKnightTalents.ImprovedDeathStrike);
                    FullCharacterStats.BonusDeathStrikeDamage += (.15f * character.DeathKnightTalents.ImprovedDeathStrike);
                }

                // Crimson Scourge 
                // Increases the damage dealt by your Blood Boil by 20/40%, and when you Plague Strike a target that is already
                // infected with your Blood Plague, there is a 50/100% chance that your next Blood Boil will consume no runes.
                if (character.DeathKnightTalents.CrimsonScourge > 0)
                {
                    // TODO: Implement in Combat Table.
                }

                // Dancing Rune Weapon
                // TODO: Re-Implement this.
            }
            #endregion

            #region Frost Talents

            // Runic Power Mastery
            // Increases Max RP by 10 per point
            if (character.DeathKnightTalents.RunicPowerMastery > 0)
            {
                BonusMaxRunicPower += 10 * character.DeathKnightTalents.RunicPowerMastery;
            }

            // Icy Reach
            // Increases range of IT & CoI and HB by 5 yards per point.

            // Nerves of Cold Steel
            // Increase hit w/ 1H weapons by 1% per point
            // Increase damage done by off hand weapons by 8/16/25% per point
            // Implement in combat shot roation

            // Annihilation
            // +15% Oblit damage per point.
            if (character.DeathKnightTalents.Annihilation > 0)
            {
                FullCharacterStats.BonusObliterateMultiplier += (0.15f * character.DeathKnightTalents.Annihilation);
            }
            
            // Lichborne
            // for 10 sec, immune to charm, fear, sleep
            // CD 2 Mins

            // On a Pale Horse
            // Reduce dur of Movement slowing effects by 15% per point
            // increase mount speed by 10% per point
            if (character.DeathKnightTalents.OnAPaleHorse > 0)
            { }

            // Endless Winter
            // Mind Freeze RP cost is reduced by 50% per point.
            if (character.DeathKnightTalents.EndlessWinter > 0)
            { }

            if (r == Rotation.Type.Frost)
            {
                // Merciless Combat
                // addtional 6% per point damage for IT, HB, Oblit, and FS
                // on targets of less than 35% health.

                // Chill of the Grave
                // CoI, HB, IT and Oblit generate 5 RP per point.

                // Killing Machine
                // Melee attacks have a chance to make IT, OB, or FS a crit.
                // increased proc per point.

                // Rime
                // Oblit has a 15% per point your next IT or HB consumes no runes

                // Pillar of Frost
                // 1 min CD, 20 sec dur
                // Str +20%
                if (character.DeathKnightTalents.PillarOfFrost > 0)
                    FullCharacterStats.AddSpecialEffect( _SE_PillarOfFrost );

                // Improved Icy Talons
                // increases the melee haste of the group/raid by 20%
                // increases your haste by 5% all the time.
                if (character.DeathKnightTalents.ImprovedIcyTalons > 0)
                {
                    FullCharacterStats.PhysicalHaste += 0.05f;
                    if (!character.ActiveBuffsContains("Improved Icy Talons")
                        && !character.ActiveBuffsContains("Windfury Totem"))
                    {
                        FullCharacterStats.PhysicalHaste += .2f;
                    }
                }

                // Brittle Bones:
                // Str +2% per point
                // FF chills the bones of its victims increasing damage taken by 2% per point.
                if (character.DeathKnightTalents.BrittleBones > 0)
                {
                    FullCharacterStats.BonusStrengthMultiplier += .02f * character.DeathKnightTalents.BrittleBones;
                    FullCharacterStats.BonusDamageMultiplier += .02f * character.DeathKnightTalents.BrittleBones;
                }

                // Chilblains
                // FF victimes are movement reduced 25% per point

                // Hungering Cold
                // Spell that freezes all enemies w/ 10 yards.


                // Improved Frost Presence
                // Increases your bonus damage while in Frost Presence by an additional 2% per point.  
                // In addition, while in Blood Presence or Unholy Presence, you retain 2% per point increased runic power generation from Frost Presence.
                if (character.DeathKnightTalents.ImprovedFrostPresence > 0)
                {
                    FullCharacterStats.BonusDamageMultiplier += (0.02f * character.DeathKnightTalents.ImprovedFrostPresence);
                }

                // Threat of Thassarian: 
                // When dual-wielding, your Death Strikes, Obliterates, Plague Strikes, 
                // Blood Strikes and Frost Strikes and Rune Strike (as of 3.2.2) have a 30/60/100% chance 
                // to also deal damage with your off-hand weapon. 

                // Might of the Frozen Wastes
                // When wielding a two-handed weapon, your autoattacks have a 15% chance to generate 10 Runic Power.

                // Howling Blast.
            }
            #endregion

            #region UnHoly Talents
            // Unholy Command
            // reduces CD of DG by 5 sec per point

            // Virulence
            // Increases Spell hit +1% per point
            if (character.DeathKnightTalents.Virulence > 0)
            {
                FullCharacterStats.SpellHit += 0.02f * character.DeathKnightTalents.Virulence;
            }

            // Epidemic
            // Increases Duration of BP and FF by 4 sec per point

            // Desecration
            // PS and SS cause Desecrated Ground effect.
            // Targets are slowed by 25% per point
            
            // Resilient Infection
            // When your diseases are dispelled you have a 50/100% to activate a 
            // Frost rune if FF was dispelled
            // Unholy rune if BP was dispelled

            // Morbidity
            // increases dam & healing of DC by 5% per point
            // increases dam of DnD by 10% sec per point

            if (r == Rotation.Type.Unholy)
            {
                // Runic Corruption
                // Reduces the cost of your Death Coil by 3 per point, and causes 
                // your Runic Empowerment ability to no longer refresh a depleted 
                // rune, but instead to increase your rune regeneration rate by 50/100% for 3 sec.

                // Unholy Frenzy
                // Induces a friendly unit into a killing frenzy for 30 sec.  
                // The target is Enraged, which increases their melee and ranged haste by 20%, 
                // but causes them to lose health equal to 2% of their maximum health every 3 sec.

                // Contagion
                // Increases the damage of your diseases spread via Pestilence by 50/100%.
                if (character.DeathKnightTalents.Contagion > 0)
                    FullCharacterStats.BonusDiseaseDamageMultiplier += .5f * character.DeathKnightTalents.Contagion;

                // Shadow Infusion
                // When you cast Death Coil, you have a 33% per point chance to empower your active Ghoul, 
                // increasing its damage dealt by 10% for 30 sec.  Stacks up to 5 times.

                // Magic Suppression
                // AMS absorbs additional 8, 16, 25% of spell damage.

                // Rage of Rivendare
                // Increases the damage of your Plague Strike, Scourge Strike, and Festering Strike abilities by 15% per point.

                // Unholy Blight
                // Causes the victims of your Death Coil to be surrounded by a vile swarm of unholy insects, 
                // taking 10% of the damage done by the Death Coil over 10 sec, and preventing any diseases on the victim from being dispelled.

                // AntiMagic Zone
                // Creates a zone where party/raid members take 75% less spell damage
                // Lasts 10 secs or X damage.  
                if (character.DeathKnightTalents.AntiMagicZone > 0)
                {
                    FullCharacterStats.AddSpecialEffect(_SE_AntiMagicZone);
                }

                // Improved Unholy Presence
                // Grants you an additional 2% haste while in Unholy Presence.  
                // In addition, while in Blood Presence or Frost Presence, you retain 8% increased movement speed from Unholy Presence.
                if (character.DeathKnightTalents.ImprovedUnholyPresence > 0)
                    FullCharacterStats.PhysicalHaste += .025f * character.DeathKnightTalents.ImprovedUnholyPresence;

                // Dark Transformation
                // Consume 5 charges of Shadow Infusion on your Ghoul to transform it into a powerful 
                // undead monstrosity for 30 sec.  The Ghoul's abilities are empowered and take on new 
                // functions while the transformation is active.

                // Ebon Plaguebringer
                // Your Plague Strike, Icy Touch, Chains of Ice, and Outbreak abilities also infect 
                // their target with Ebon Plague, which increases damage taken from your diseases 
                // by 15/30% and all magic damage taken by an additional 8%.
                if (character.DeathKnightTalents.EbonPlaguebringer > 0)
                {
                    if (!character.ActiveBuffsContains("Earth and Moon")
                        && !character.ActiveBuffsContains("Curse of the Elements")
                        && !character.ActiveBuffsContains("Ebon Plaguebringer"))
                    {
                        float fBonus = .08f;
                        FullCharacterStats.BonusArcaneDamageMultiplier += fBonus;
                        FullCharacterStats.BonusFireDamageMultiplier += fBonus;
                        FullCharacterStats.BonusFrostDamageMultiplier += fBonus;
                        FullCharacterStats.BonusHolyDamageMultiplier += fBonus;
                        FullCharacterStats.BonusNatureDamageMultiplier += fBonus;
                        FullCharacterStats.BonusShadowDamageMultiplier += fBonus;
                    }
                }

                // Sudden Doom
                // Your auto attacks have a 5% per point chance to make your next Death Coil cost no runic power.

                // Summon Gargoyle
            }
            #endregion
        }

        #region Custom Charts
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsDPSDK baseCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;
            float fMultiplier = 1f;

            baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSDK;

            string[] statList = new string[] 
            {
                "Strength",
                "Agility",
                "Attack Power",
                "Crit Rating",
                "Hit Rating",
                "Expertise Rating",
                "Haste Rating",
                "Mastery Rating",
            };

            switch (chartName)
            {

                //"Item Budget (10 point steps)","Item Budget (25 point steps)","Item Budget(50 point steps)","Item Budget (100 point steps)"
                case "Item Budget (10 point steps)":
                    {
                        fMultiplier = 1f;
                        break;
                    }
                case "Item Budget (25 point steps)":
                    {
                        fMultiplier = 2.5f;
                        break;
                    }
                case "Item Budget (50 point steps)":
                    {
                        fMultiplier = 5f;
                        break;
                    }
                case "Item Budget (100 point steps)":
                    {
                        fMultiplier = 10f;
                        break;
                    }
                case "Presences":
                    {
                        string[] listPresence = new string[] {
                            "None",
                            "Blood",
                            "Unholy",
                            "Frost",
                        };

                        // Set this to have no presence enabled.
                        Character baseCharacter = character.Clone();
                        (baseCharacter.CalculationOptions as CalculationOptionsDPSDK).CurrentPresence = CalculationOptionsDPSDK.Presence.None;
                        // replacing pre-factored base calc since this is different than the Item budget lists. 
                        baseCalc = GetCharacterCalculations(baseCharacter, null, true, false, false) as CharacterCalculationsDPSDK;

                        // Set these to have the key presence enabled.
                        for (int index = 1; index < listPresence.Length; index++)
                        {
                            (character.CalculationOptions as CalculationOptionsDPSDK).CurrentPresence = (CalculationOptionsDPSDK.Presence)index;
                            
                            calc = GetCharacterCalculations(character, null, false, true, true) as CharacterCalculationsDPSDK;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = listPresence[index];
                            comparison.Equipped = false;
                            comparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                            subPoints = new float[calc.SubPoints.Length];
                            for (int i = 0; i < calc.SubPoints.Length; i++)
                            {
                                subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                            }
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }
                        return comparisonList.ToArray();
                    }
                default:
                    return new ComparisonCalculationBase[0];
            }

            // Item Budget list. since it's used multiple times.
            Item[] itemList = new Item[] 
            {
                new Item() { Stats = new Stats() { Strength = 10 * fMultiplier } },
                new Item() { Stats = new Stats() { Agility = 10 * fMultiplier } },
                new Item() { Stats = new Stats() { AttackPower = 20 * fMultiplier } },
                new Item() { Stats = new Stats() { CritRating = 10 * fMultiplier } },
                new Item() { Stats = new Stats() { HitRating = 10 * fMultiplier } },
                new Item() { Stats = new Stats() { ExpertiseRating = 10 * fMultiplier } },
                new Item() { Stats = new Stats() { HasteRating = 10 * fMultiplier } },
                new Item() { Stats = new Stats() { MasteryRating = 10 * fMultiplier } },
            };
            // Do the math.
            for (int index = 0; index < itemList.Length; index++)
            {
                calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsDPSDK;

                comparison = CreateNewComparisonCalculation();
                comparison.Name = statList[index];
                comparison.Equipped = false;
                comparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                subPoints = new float[calc.SubPoints.Length];
                for (int i = 0; i < calc.SubPoints.Length; i++)
                {
                    subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                }
                comparison.SubPoints = subPoints;

                comparisonList.Add(comparison);
            }
            return comparisonList.ToArray();
        }
        #endregion

        #region Relevant Stats?
        public override bool IsItemRelevant(Item item)
        {
            if (item.Slot == ItemSlot.OffHand /*  ||
                (item.Slot == ItemSlot.Ranged && item.Type != ItemType.Sigil) */)
                return false;
            return base.IsItemRelevant(item);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                // Core stats
                Strength = stats.Strength,
                Agility = stats.Agility,
                Stamina = stats.Stamina,
                ExpertiseRating = stats.ExpertiseRating,
                AttackPower = stats.AttackPower,
                MasteryRating = stats.MasteryRating,
                // Other Base Stats
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Resilience = stats.Resilience,
//                Mastery = stats.Mastery,

                // Secondary Stats
                Health = stats.Health,
                SpellHaste = stats.SpellHaste,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                SpellCrit = stats.SpellCrit,
                SpellCritOnTarget = stats.SpellCritOnTarget,
                SpellHit = stats.SpellHit,
                SpellPenetration = stats.SpellPenetration,

                // Damage stats
                WeaponDamage = stats.WeaponDamage,
                PhysicalDamage = stats.PhysicalDamage,
                ShadowDamage = stats.ShadowDamage,
                ArcaneDamage = stats.ArcaneDamage,
                FireDamage = stats.FireDamage,
                FrostDamage = stats.FrostDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,

                // Bonus to stat
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,

                // Bonus to Damage
                // *Damage
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,
                BonusRuneStrikeMultiplier = stats.BonusRuneStrikeMultiplier,
                BonusObliterateMultiplier = stats.BonusObliterateMultiplier,
                BonusHeartStrikeMultiplier = stats.BonusHeartStrikeMultiplier,
                BonusScourgeStrikeMultiplier = stats.BonusScourgeStrikeMultiplier,
                // +Damage
                BonusFrostWeaponDamage = stats.BonusFrostWeaponDamage,
                BonusScourgeStrikeDamage = stats.BonusScourgeStrikeDamage,
                BonusBloodStrikeDamage = stats.BonusBloodStrikeDamage,
                BonusDeathCoilDamage = stats.BonusDeathCoilDamage, 
                BonusDeathStrikeDamage =  stats.BonusDeathStrikeDamage,
                BonusFrostStrikeDamage = stats.BonusFrostStrikeDamage,
                BonusHeartStrikeDamage = stats.BonusHeartStrikeDamage,
                BonusIcyTouchDamage = stats.BonusIcyTouchDamage,
                BonusObliterateDamage = stats.BonusObliterateDamage,
                BonusPerDiseaseBloodStrikeDamage = stats.BonusPerDiseaseBloodStrikeDamage,
                BonusPerDiseaseHeartStrikeDamage = stats.BonusPerDiseaseHeartStrikeDamage,
                BonusPerDiseaseObliterateDamage = stats.BonusPerDiseaseObliterateDamage,
                BonusPerDiseaseScourgeStrikeDamage = stats.BonusPerDiseaseScourgeStrikeDamage,
                // Crit
                BonusDeathCoilCrit = stats.BonusDeathCoilCrit,
                BonusDeathStrikeCrit = stats.BonusDeathStrikeCrit,
                BonusFrostStrikeCrit = stats.BonusFrostStrikeCrit,
                BonusObliterateCrit = stats.BonusObliterateCrit,
                BonusPlagueStrikeCrit = stats.BonusPlagueStrikeCrit,
                BonusScourgeStrikeCrit = stats.BonusScourgeStrikeCrit,
                // RP
                BonusRPFromDeathStrike = stats.BonusRPFromDeathStrike,
                BonusRPFromObliterate = stats.BonusRPFromObliterate,
                BonusRPFromScourgeStrike = stats.BonusRPFromScourgeStrike, 
                // Other
                CinderglacierProc = stats.CinderglacierProc,
                DiseasesCanCrit = stats.DiseasesCanCrit,
                HighestStat = stats.HighestStat,
                Paragon = stats.Paragon,
                DeathbringerProc = stats.DeathbringerProc, 
            };

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    if (effect.Trigger == Trigger.DamageDone ||
                        effect.Trigger == Trigger.DamageOrHealingDone ||
                        effect.Trigger == Trigger.DamageSpellCast ||
                        effect.Trigger == Trigger.DamageSpellCrit ||
                        effect.Trigger == Trigger.DamageSpellHit ||
                        effect.Trigger == Trigger.SpellCast ||
                        effect.Trigger == Trigger.SpellCrit ||
                        effect.Trigger == Trigger.SpellHit ||
                        effect.Trigger == Trigger.DoTTick ||
                        effect.Trigger == Trigger.MeleeCrit ||
                        effect.Trigger == Trigger.MeleeHit ||
                        effect.Trigger == Trigger.CurrentHandHit ||
                        effect.Trigger == Trigger.MainHandHit ||
                        effect.Trigger == Trigger.OffHandHit ||
                        effect.Trigger == Trigger.PhysicalCrit ||
                        effect.Trigger == Trigger.PhysicalHit ||
                        effect.Trigger == Trigger.BloodStrikeHit ||
                        effect.Trigger == Trigger.HeartStrikeHit ||
                        effect.Trigger == Trigger.BloodStrikeOrHeartStrikeHit ||
                        effect.Trigger == Trigger.ScourgeStrikeHit ||
                        effect.Trigger == Trigger.ObliterateHit ||
                        effect.Trigger == Trigger.DeathStrikeHit ||
                        effect.Trigger == Trigger.IcyTouchHit ||
                        effect.Trigger == Trigger.PlagueStrikeHit ||
                        effect.Trigger == Trigger.RuneStrikeHit ||
                        effect.Trigger == Trigger.Use)
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }

            return s;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool bRelevant = false;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (relevantStats(effect.Stats))
                {
                    if (effect.Trigger == Trigger.DamageDone ||
                        effect.Trigger == Trigger.DamageOrHealingDone ||
                        effect.Trigger == Trigger.DamageSpellCast ||
                        effect.Trigger == Trigger.DamageSpellCrit ||
                        effect.Trigger == Trigger.DamageSpellHit ||
                        effect.Trigger == Trigger.SpellCast ||
                        effect.Trigger == Trigger.SpellCrit ||
                        effect.Trigger == Trigger.SpellHit ||
                        effect.Trigger == Trigger.DoTTick ||
                        effect.Trigger == Trigger.MeleeCrit ||
                        effect.Trigger == Trigger.MeleeHit ||
                        effect.Trigger == Trigger.CurrentHandHit ||
                        effect.Trigger == Trigger.MainHandHit ||
                        effect.Trigger == Trigger.OffHandHit ||
                        effect.Trigger == Trigger.PhysicalCrit ||
                        effect.Trigger == Trigger.PhysicalHit ||
                        effect.Trigger == Trigger.BloodStrikeHit ||
                        effect.Trigger == Trigger.HeartStrikeHit ||
                        effect.Trigger == Trigger.BloodStrikeOrHeartStrikeHit ||
                        effect.Trigger == Trigger.ScourgeStrikeHit ||
                        effect.Trigger == Trigger.ObliterateHit ||
                        effect.Trigger == Trigger.DeathStrikeHit ||
                        effect.Trigger == Trigger.IcyTouchHit ||
                        effect.Trigger == Trigger.PlagueStrikeHit ||
                        effect.Trigger == Trigger.RuneStrikeHit ||
                        effect.Trigger == Trigger.Use)
                    {
                        foreach (SpecialEffect e in effect.Stats.SpecialEffects())
                        {
                            if (!bRelevant)
                                bRelevant = HasRelevantStats(e.Stats);
                        }
                        if (!bRelevant)
                            bRelevant = relevantStats(effect.Stats);
                        
                    }
                }
            }
            if (!bRelevant)
                bRelevant = relevantStats(stats);
            return bRelevant;
        }

        private bool relevantStats(Stats stats)
        {
            bool bResults = false;
            // Core stats
            bResults |= (stats.Strength != 0);
            bResults |= (stats.Agility != 0);
            bResults |= (stats.Stamina != 0);
            bResults |= (stats.ExpertiseRating != 0);
            bResults |= (stats.AttackPower != 0);
            bResults |= (stats.MasteryRating != 0);
            bool bHasCore = bResults; // if the above stats are 0, lets make sure we're not bringing in caster gear below.
            // Other Base Stats
            bResults |= (stats.HasteRating != 0);
            bResults |= (stats.HitRating != 0);
            bResults |= (stats.CritRating != 0);
            bResults |= (stats.Armor != 0);
            bResults |= (stats.BonusArmor != 0);
            bResults |= (stats.Resilience != 0);
//            bResults |= (Mastery != 0); // TODO: May update the stats object to include this.

            // Secondary Stats
            bResults |= (stats.Health != 0);
            bResults |= (stats.SpellHaste != 0);
            bResults |= (stats.PhysicalCrit != 0);
            bResults |= (stats.PhysicalHaste != 0);
            bResults |= (stats.PhysicalHit != 0);
            bResults |= (stats.SpellCrit != 0);
            bResults |= (stats.SpellCritOnTarget != 0);
            bResults |= (stats.SpellHit != 0);
            bResults |= (stats.SpellHaste != 0);
            bResults |= (stats.SpellPenetration != 0);

            // Damage stats
            bResults |= (stats.WeaponDamage != 0);
            bResults |= (stats.PhysicalDamage != 0);
            bResults |= (stats.ShadowDamage != 0);
            bResults |= (stats.ArcaneDamage != 0);
            bResults |= (stats.FireDamage != 0);
            bResults |= (stats.FrostDamage) != 0;
            bResults |= (stats.HolyDamage) != 0;
            bResults |= (stats.NatureDamage) != 0;

            // Bonus to stat
            bResults |= (stats.BonusStrengthMultiplier != 0);
            bResults |= ( stats.BonusStaminaMultiplier != 0);
            bResults |= ( stats.BonusAgilityMultiplier != 0);
            bResults |= ( stats.BonusCritMultiplier != 0);
            bResults |= (stats.BonusAttackPowerMultiplier != 0);

            // Bonus to Damage
            // *Damage
            bResults |= (stats.BonusDamageMultiplier != 0);
            bResults |= (stats.BonusPhysicalDamageMultiplier != 0);
            bResults |= ( stats.BonusShadowDamageMultiplier != 0);
            bResults |= ( stats.BonusFrostDamageMultiplier != 0);
            bResults |= ( stats.BonusDiseaseDamageMultiplier  != 0);
            bResults |= (stats.BonusRuneStrikeMultiplier != 0);
            bResults |= (stats.BonusObliterateMultiplier != 0);
            bResults |= (stats.BonusHeartStrikeMultiplier != 0);
            bResults |= (stats.BonusScourgeStrikeMultiplier != 0);
            // +Damage
            bResults |= (stats.BonusFrostWeaponDamage != 0);
            bResults |= (stats.BonusScourgeStrikeDamage != 0);
            bResults |= (stats.BonusBloodStrikeDamage != 0);
            bResults |= ( stats.BonusDeathCoilDamage != 0); 
            bResults |= ( stats.BonusDeathStrikeDamage != 0);  
            bResults |= ( stats.BonusFrostStrikeDamage   != 0); 
            bResults |= ( stats.BonusHeartStrikeDamage != 0);  
            bResults |= ( stats.BonusIcyTouchDamage != 0);  
            bResults |= ( stats.BonusObliterateDamage != 0);
            bResults |= (stats.BonusPerDiseaseBloodStrikeDamage != 0);
            bResults |= (stats.BonusPerDiseaseHeartStrikeDamage != 0);
            bResults |= (stats.BonusPerDiseaseObliterateDamage != 0);
            bResults |= (stats.BonusPerDiseaseScourgeStrikeDamage != 0);
            // Crit
            bResults |= (stats.BonusCritMultiplier != 0);
            bResults |= (stats.BonusDeathCoilCrit != 0);
            bResults |= ( stats.BonusDeathStrikeCrit != 0);
            bResults |= ( stats.BonusFrostStrikeCrit != 0); 
            bResults |= ( stats.BonusObliterateCrit != 0); 
            bResults |= ( stats.BonusPlagueStrikeCrit != 0);
            bResults |= (stats.BonusScourgeStrikeCrit != 0);
            // RP
            bResults |= ( stats.BonusRPFromDeathStrike != 0);
            bResults |= ( stats.BonusRPFromObliterate != 0);
            bResults |= ( stats.BonusRPFromScourgeStrike != 0); 
            // Other
            bResults |= ( stats.CinderglacierProc != 0); 
            bResults |= ( stats.DiseasesCanCrit != 0); 
            bResults |= ( stats.HighestStat != 0); 
            bResults |= ( stats.Paragon != 0); 
            bResults |= ( stats.DeathbringerProc != 0); 

            // Filter out caster gear:
            if (!bHasCore & bResults)
                // Let's make sure that if we've got some stats that may be interesting
            {
                bResults = !((stats.Intellect != 0)
                    || (stats.Spirit != 0)
                    || (stats.Mp5 != 0)
                    || (stats.SpellPower != 0)
                    || (stats.Mana != 0)
                    );
            }
            return bResults;
        }
        #endregion

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                        "Health",
                        "Crit Rating",
                        "Expertise Rating",
                        "Hit Rating",
                        "Haste Rating",
                        "Target Miss %",
                        "Target Dodge %",
                        "Resilience",
                        "Spell Penetration"
                    };

                return _optimizableCalculationLabels;
            }
        }

        #region Static SpecialEffects
        // Talent: Spell Deflection
        private static Dictionary<float, SpecialEffect[]> _SE_SpellDeflection = new Dictionary<float, SpecialEffect[]>();
        // Gear: T10 4P
        private static readonly SpecialEffect _SE_T10_4P = new SpecialEffect(Trigger.Use, new Stats() { DamageTakenMultiplier = -0.12f }, 10f, 60f);
        // Enchant: Rune of Fallen Crusader
        private static readonly SpecialEffect _SE_FC1 = new SpecialEffect(Trigger.DamageDone, new Stats() { BonusStrengthMultiplier = .15f }, 15f, 0f, -2f, 1);
        private static readonly SpecialEffect _SE_FC2 = new SpecialEffect(Trigger.DamageDone, new Stats() { HealthRestoreFromMaxHealth = .03f }, 0, 0f, -2f, 1);
        private static readonly SpecialEffect[][] _SE_VampiricBlood = new SpecialEffect[][] {
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, null, 10 + 0 * 5, 60f - (false ? 0 : 10)), new SpecialEffect(Trigger.Use, null, 10 + 0 * 5, 60f - (true ? 0 : 10)),},
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, null, 10 + 1 * 5, 60f - (false ? 0 : 10)), new SpecialEffect(Trigger.Use, null, 10 + 1 * 5, 60f - (true ? 0 : 10)),},
        };
        // Talent: Rune Tap
        private static readonly SpecialEffect[] _SE_RuneTap = new SpecialEffect[] {
            new SpecialEffect(Trigger.Use, null, 0, 60f - 10 * 0),
            new SpecialEffect(Trigger.Use, null, 0, 60f - 10 * 1),
            new SpecialEffect(Trigger.Use, null, 0, 60f - 10 * 2),
            new SpecialEffect(Trigger.Use, null, 0, 60f - 10 * 3),
        };
        // Talent Bloody Vengence.
        private static readonly SpecialEffect[] _SE_BloodyVengeance1 = new SpecialEffect[] {
            null,
            new SpecialEffect(Trigger.DamageSpellCrit, new Stats() { BonusPhysicalDamageMultiplier = .01f * 0 }, 30, 0, 1, 3),
            new SpecialEffect(Trigger.DamageSpellCrit, new Stats() { BonusPhysicalDamageMultiplier = .01f * 1 }, 30, 0, 1, 3),
            new SpecialEffect(Trigger.DamageSpellCrit, new Stats() { BonusPhysicalDamageMultiplier = .01f * 2 }, 30, 0, 1, 3),
            new SpecialEffect(Trigger.DamageSpellCrit, new Stats() { BonusPhysicalDamageMultiplier = .01f * 3 }, 30, 0, 1, 3),
        };
        private static readonly SpecialEffect[] _SE_BloodyVengeance2 = new SpecialEffect[] {
            null,
            new SpecialEffect(Trigger.MeleeCrit, new Stats() { BonusPhysicalDamageMultiplier = .01f * 0 }, 30, 0, 1, 3),
            new SpecialEffect(Trigger.MeleeCrit, new Stats() { BonusPhysicalDamageMultiplier = .01f * 1 }, 30, 0, 1, 3),
            new SpecialEffect(Trigger.MeleeCrit, new Stats() { BonusPhysicalDamageMultiplier = .01f * 2 }, 30, 0, 1, 3),
            new SpecialEffect(Trigger.MeleeCrit, new Stats() { BonusPhysicalDamageMultiplier = .01f * 3 }, 30, 0, 1, 3),
        };
        private static Dictionary<float, SpecialEffect[]> _SE_Bloodworms = new Dictionary<float, SpecialEffect[]>();
        private static readonly SpecialEffect[] _SE_WillOfTheNecropolis = new SpecialEffect[] {
            null,
            new SpecialEffect(Trigger.DamageTaken, new Stats() { DamageTakenMultiplier = -(0.08f * 1) }, 0, 0, 0.45f),
            new SpecialEffect(Trigger.DamageTaken, new Stats() { DamageTakenMultiplier = -(0.08f * 2) }, 0, 0, 0.45f),
            new SpecialEffect(Trigger.DamageTaken, new Stats() { DamageTakenMultiplier = -(0.08f * 3) }, 0, 0, 0.45f),
        };
        public static readonly SpecialEffect[][] _SE_UnbreakableArmor = new SpecialEffect[][] {
            new SpecialEffect[] {
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (false ? .20f : 0f), BonusArmorMultiplier = .25f + (false ? .20f : 0f) }, 20f, 60f - 0 * 10f),
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (true  ? .20f : 0f), BonusArmorMultiplier = .25f + (true  ? .20f : 0f) }, 20f, 60f - 0 * 10f),
            },
            new SpecialEffect[] {
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (false ? .20f : 0f), BonusArmorMultiplier = .25f + (false ? .20f : 0f) }, 20f, 60f - 1 * 10f),
                    new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = 0.20f, BaseArmorMultiplier = .25f + (true  ? .20f : 0f), BonusArmorMultiplier = .25f + (true  ? .20f : 0f) }, 20f, 60f - 1 * 10f),
            },
        };
        public static readonly SpecialEffect _SE_AntiMagicZone = new SpecialEffect(Trigger.Use, new Stats() { SpellDamageTakenMultiplier = -0.75f }, 10f, 2f * 60f);

        public static readonly SpecialEffect _SE_PillarOfFrost = new SpecialEffect(Trigger.Use, new Stats() { BonusStrengthMultiplier = .2f }, 20f, 60);
        #endregion
    }
}
