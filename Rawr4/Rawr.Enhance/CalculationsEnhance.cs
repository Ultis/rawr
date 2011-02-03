using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Rawr.Enhance
{
    [Rawr.Calculations.RawrModelInfo("Enhance", "inv_jewelry_talisman_04", CharacterClass.Shaman)]
    public class CalculationsEnhance : CalculationsBase
    {
        private string VERSION = "4.0.0.0";
        #region Gemming Template
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                //Meta
                int chaotic = 52291;
                int fleet = 52289;
                int relentless = 41398;

                if (_defaultGemmingTemplates == null)
                {
                    Gemming gemming = new Gemming();
                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Uncommon", 0, fleet, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Uncommon", 0, chaotic, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Uncommon", 0, relentless, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Rare", 1, fleet, true));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Rare", 1, chaotic, true));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Rare", 1, relentless, true));
                    //_defaultGemmingTemplates.AddRange(gemming.addTemplates("Epic", 2, fleet, false));
                    //_defaultGemmingTemplates.AddRange(gemming.addTemplates("Epic", 2, chaotic, false));
                    //_defaultGemmingTemplates.AddRange(gemming.addTemplates("Epic", 2, relentless, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Jeweler", 3, fleet, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Jeweler", 3, chaotic, false));
                    _defaultGemmingTemplates.AddRange(gemming.addTemplates("Jeweler", 3, relentless, false));
                }
                return _defaultGemmingTemplates;
            }
        }
        #endregion

        #region Display labels
        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
                    "Summary:DPS Points*Your total expected DPS with this kit and selected glyphs and buffs",
                    "Summary:Survivability Points*Assumes basic 2% of total health as Survivability",
                    "Summary:Overall Points*This is the sum of Total DPS and Survivability. If you want sort items by DPS only select DPS from the sort dropdown top right",
                    "Basic Stats:Health",
                    "Basic Stats:Mana",
                    "Basic Stats:Strength",
                    "Basic Stats:Agility",
                    "Basic Stats:Intellect",
                    "Basic Stats:Spirit",
                    "Basic Stats:Attack Power",
                    "Basic Stats:Spell Power",
                    "Basic Stats:Mastery",
                    "Basic Stats:Haste Rating",
                    "Basic Stats:Total Expertise",
                    "Basic Stats:Hit Rating",
                    "Basic Stats:White Hit",
                    "Basic Stats:Yellow Hit",
                    "Basic Stats:Spell Hit",
                    "Basic Stats:Melee Crit",
                    "Basic Stats:Spell Crit",
                    "Complex Stats:Avoided Attacks*The percentage of your attacks that fail to land.",
                    "Complex Stats:Avg MH Speed",
                    "Complex Stats:Avg OH Speed",
                    "Complex Stats:Armor Mitigation",
                    "Complex Stats:Flurry Uptime",
                    "Complex Stats:ED Uptime*Elemental Devastation Uptime percentage",
                    "Complex Stats:Avg Time to 5 Stack*Average time it takes to get 5 stacks of Maelstrom Weapon.",
                    "Complex Stats:MH Enchant Uptime",
                    "Complex Stats:OH Enchant Uptime",
                    "Complex Stats:Trinket 1 Uptime",
                    "Complex Stats:Trinket 2 Uptime",
                    "Complex Stats:Fire Totem Uptime",
                    "Complex Stats:Mana Regen*In combat Mp5",
                    "Attacks:White Damage",
                    "Attacks:Windfury Attack",
                    "Attacks:Flametongue Attack",
                    "Attacks:Stormstrike",
                    "Attacks:Lava Lash",
                    "Attacks:Searing/Magma Totem",
                    "Attacks:Earth Shock",
                    "Attacks:Flame Shock",
                    "Attacks:Lightning Bolt",
                    "Attacks:Unleash Wind",
                    "Attacks:Unleash Flame",
                    "Attacks:Lightning Shield",
                    "Attacks:Chain Lightning",
                    "Attacks:Fire Nova",
                    "Attacks:Fire Elemental",
                    "Attacks:Spirit Wolf",
                    "Attacks:Other",
                    "Attacks:Total DPS",
                    "Module Version:Enhance Version"
                    /*"Summary:DPS Points*Your total expected DPS with this kit and selected glyphs and buffs",
                    "Summary:Survivability Points*Assumes basic 2% of total health as Survivability",
                    "Summary:Overall Points*This is the sum of Total DPS and Survivability. If you want sort items by DPS only select DPS from the sort dropdown top right",
                    #region Stats as they appear on your character screen out of combat
                    "Base Stats:Health",
                    "Base Stats:Mana",
                    "Base Stats:Strength",
                    "Base Stats:Agility",
                    "Base Stats:Stamina",
                    "Base Stats:Intellect",
                    "Base Stats:Spirit",
                    "Base Stats:Mastery",
                    "Melee:Damage",
                    "Melee:DPS",
                    "Melee:Attack Power",
                    "Melee:Speed",
                    "Melee:Haste",
                    "Melee:Hit",
                    "Melee:Crit",
                    "Melee:Expertise",
                    "Spell:Spell Power",
                    "Spell:Haste",
                    "Spell:Hit",
                    "Spell:Crit",
                    "Spell:Combat Regen",
                    //"Basic Stats:White Hit",
                    //"Basic Stats:Yellow Hit",
                    #endregion
                    #region Averaged Stats in combat
                    //"Base Stats:Health",
                    //"Base Stats:Mana",
                    //"Base Stats:Strength",
                    "Combat Stats:Avg Agility",
                    //"Base Stats:Stamina",
                    //"Base Stats:Intellect",
                    //"Base Stats:Spirit",
                    "Combat Stats:Avg Mastery",
                    //"Melee:Damage",
                    //"Melee:DPS",
                    "Combat Stats:Avg Attack Power",
                    "Combat Stats:Avg Melee Speed",
                    "Combat Stats:Avg Melee Haste",
                    "Combat Stats:Avg Melee Hit",
                    "Combat Stats:Avg Melee Crit",
                    "Combat Stats:Avg Expertise",
                    "Combat Stats:Avg Spell Power",
                    "Combat Stats:Avg Spell Haste",
                    "Combat Stats:Avg Spell Hit",
                    "Combat Stats:Avg Spell Crit",
                    "Combat Stats:Avg Combat Regen*Includes Primal Wisdon regen",
                    "Complex Stats:Avoided Attacks*The percentage of your attacks that fail to land.",
                    "Complex Stats:Armor Mitigation",
                    "Complex Stats:Flurry Uptime",
                    "Complex Stats:ED Uptime*Elemental Devastation Uptime percentage",
                    "Complex Stats:Avg Time to 5 Stack*Average time it takes to get 5 stacks of Maelstrom Weapon.",
                    "Complex Stats:MH Enchant Uptime",
                    "Complex Stats:OH Enchant Uptime",
                    "Complex Stats:Trinket 1 Uptime",
                    "Complex Stats:Trinket 2 Uptime",
                    "Complex Stats:Fire Totem Uptime",
                    #endregion
                    #region Attacks Breakdown
                    "Attacks:White Damage",
                    "Attacks:Windfury Attack",
                    "Attacks:Flametongue Attack",
                    "Attacks:Stormstrike",
                    "Attacks:Lava Lash",
                    "Attacks:Searing/Magma Totem",
                    "Attacks:Earth Shock",
                    "Attacks:Flame Shock",
                    "Attacks:Lightning Bolt",
                    "Attacks:Unleash Wind",
                    "Attacks:Unleash Flame",
                    "Attacks:Lightning Shield",
                    "Attacks:Chain Lightning",
                    "Attacks:Fire Nova",
                    "Attacks:Fire Elemental",
                    "Attacks:Spirit Wolf",
                    "Attacks:Other",
                    "Attacks:Total DPS",
                    #endregion
                    //"Module Version:Enhance Version"*/
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
                    "Missed Attacks",
                    "Dodged Attacks",
                    //"Parried Attacks",
                    "Avoided Attacks",
                    "Health"
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
                    "Combat Table (White)",
                    "Combat Table (Yellow)",
                    "Combat Table (Spell)",
                    "Relative Gem Values",
                    "MH Weapon Speeds",
                    "OH Weapon Speeds",
                    };
                return _customChartNames;
            }
        }
        #endregion 

        #region Overrides
#if RAWR3 || RAWR4
        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                    _calculationOptionsPanel = new CalculationOptionsPanelEnhance();
                return _calculationOptionsPanel;
            }
        }
#else
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                    _calculationOptionsPanel = new CalculationOptionsPanelEnhance();
                return _calculationOptionsPanel;
            }
        }
#endif

#if RAWR3 || RAWR4
        private Dictionary<string, System.Windows.Media.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Windows.Media.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("DPS", System.Windows.Media.Color.FromArgb(255, 160, 0, 224));
                    _subPointNameColors.Add("Survivability", System.Windows.Media.Color.FromArgb(255, 64, 128, 32));
                }
                return _subPointNameColors;
            }
        }
#else
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                        _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                        _subPointNameColors.Add("DPS", System.Drawing.Color.FromArgb(160, 0, 224));
                        _subPointNameColors.Add("Survivability", System.Drawing.Color.FromArgb(64, 128, 32));
                }
                return _subPointNameColors;
            }
        }
#endif
        public override CharacterClass TargetClass { get { return CharacterClass.Shaman; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationEnhance(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsEnhance(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsEnhance));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsEnhance calcOpts = serializer.Deserialize(reader) as CalculationOptionsEnhance;
            return calcOpts;
        }
        #endregion

        #region Main Calculations
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            // First things first, we need to ensure that we aren't using bad data
            CharacterCalculationsEnhance calc = new CharacterCalculationsEnhance();
            if (character == null) { return calc; }
            CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            if (calcOpts == null) { return calc; }
            //
            #region Applied Stats
            Stats stats = GetCharacterStats(character, additionalItem);
            calc.BasicStats = stats;
            calc.BuffStats = GetBuffsStats(character.ActiveBuffs);
            Item noBuffs = RemoveAddedBuffs(calc.BuffStats);
            calc.EnhSimStats = GetCharacterStats(character, noBuffs);
            calc.TargetLevel = calcOpts.TargetLevel;
            calc.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
            float initialAP = stats.AttackPower;

            bool MailSpecialization = character.Head != null && character.Head.Type == ItemType.Mail &&
                                character.Shoulders != null && character.Shoulders.Type == ItemType.Mail &&
                                character.Chest != null && character.Chest.Type == ItemType.Mail &&
                                character.Wrist != null && character.Wrist.Type == ItemType.Mail &&
                                character.Hands != null && character.Hands.Type == ItemType.Mail &&
                                character.Waist != null && character.Waist.Type == ItemType.Mail &&
                                character.Legs != null && character.Legs.Type == ItemType.Mail &&
                                character.Feet != null && character.Feet.Type == ItemType.Mail;
            stats.BonusAgilityMultiplier += (MailSpecialization ? .05f : 0);

            // deal with Special Effects - for now add into stats regardless of effect later need to be more precise
            StatsSpecialEffects se = new StatsSpecialEffects(character, stats, calcOpts);
            stats.Accumulate(se.getSpecialEffects());
            //Set up some talent variables
            float concussionMultiplier = 1f + .02f * character.ShamanTalents.Concussion;
            float shieldBonus = 1f + .05f * character.ShamanTalents.ImprovedShields;
            float callofFlameBonus = 1f + .1f * character.ShamanTalents.CallOfFlame;
            float fireNovaBonus = 1f + .1f * character.ShamanTalents.ImprovedFireNova;
            float mentalQuickness = .5f;  //AP -> SP conversion
            float windfuryWeaponBonus = 4430f + stats.BonusWFAttackPower;  //WFAP (Check)
            float windfuryDamageBonus = 1f;
            float elemPrec = character.ShamanTalents.ElementalPrecision * 0.01f; //additive or multi
            switch (character.ShamanTalents.ElementalWeapons)
            {
                case 1: windfuryDamageBonus = 1.20f; break;
                case 2: windfuryDamageBonus = 1.40f; break;
            }
            float focusedStrikes = 1f;
            switch (character.ShamanTalents.FocusedStrikes)
            {
                case 1: focusedStrikes = 1.15f; break;
                case 2: focusedStrikes = 1.30f; break;
                case 3: focusedStrikes = 1.45f; break;
            }
            float unleashedRage = 0f;
            switch (character.ShamanTalents.UnleashedRage)
            {
                case 1: unleashedRage = .05f; break;
                case 2: unleashedRage = .10f; break;
            }

            // Tier Bonuses
            float enhance2T11 = 0f;
            if (stats.Enhance2T11 == 1)
            {
                enhance2T11 = 0.1f;
            }

            float FTspellpower = (float)Math.Floor((float)(748f * (1 + character.ShamanTalents.ElementalWeapons * .2f)));  //FT SP Bonus (Check) 
            if (calcOpts.MainhandImbue == "Flametongue")
                stats.SpellPower += FTspellpower;
            if (calcOpts.OffhandImbue == "Flametongue")
                stats.SpellPower += FTspellpower;

            float addedAttackPower = stats.AttackPower - initialAP;
            float MQSpellPower = mentalQuickness * addedAttackPower * (1 + stats.BonusAttackPowerMultiplier);
            // make sure to add in the spellpower from MQ gained from all the bonus AP added in this section
            stats.SpellPower += MQSpellPower * (1 + stats.BonusSpellPowerMultiplier);
            // also add in bonus attack power
            stats.AttackPower += addedAttackPower * stats.BonusAttackPowerMultiplier;
            #endregion

            #region Damage Model
            ////////////////////////////
            // Main calculation Block //
            ////////////////////////////

            CombatStats cs = new CombatStats(character, stats, calcOpts); // calculate the combat stats using modified stats

            // only apply unleashed rage talent if not already applied Unleashed Rage buff.
            if (!character.ActiveBuffsContains("Unleashed Rage") && 
                !character.ActiveBuffsContains("Trueshot Aura") && 
                !character.ActiveBuffsContains("Abomination's Might"))
            {
                float URattackPower = (calc.BuffStats.BonusAttackPowerMultiplier == .1f) ? 0f :
                                                        (stats.AttackPower * unleashedRage);
                stats.AttackPower += URattackPower; // no need to multiply by bonus attack power as the whole point is its zero if we need to add Unleashed rage
                stats.SpellPower += mentalQuickness * URattackPower * (1f + stats.BonusSpellPowerMultiplier);
            }

            // assign basic variables for calcs
            float attackPower = stats.AttackPower;
            float spellPower = stats.SpellPower;
            float mastery = 1f + ((8f + StatConversion.GetMasteryFromRating(stats.MasteryRating)) * 0.025f);
            float wdpsMH = character.MainHand == null ? 46.3f : (stats.WeaponDamage + (character.MainHand.MinDamage + character.MainHand.MaxDamage) / 2f) / character.MainHand.Speed;
            float wdpsOH = character.OffHand == null ? 46.3f : (stats.WeaponDamage + (character.OffHand.MinDamage + character.OffHand.MaxDamage) / 2f) / character.OffHand.Speed;
            float dualWieldSpecialization = .06f; //Hit portion of Dual Wield
            //float AP_SP_Ratio = (spellPower - 274f - 211f) / attackPower;  //CATA where do these values come from (274, 211)? 211 Flametongue sp? 274 Flametongue damage?
            float bonusPhysicalDamage = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusPhysicalDamageMultiplier);
            float bonusFireDamage = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusFireDamageMultiplier);
            float bonusNatureDamage = (1f + stats.BonusDamageMultiplier) * (1f + stats.BonusNatureDamageMultiplier);
            float bonusSSDamage = stats.BonusSSDamage;
            int baseResistance = Math.Max((calcOpts.TargetLevel - character.Level) * 5, 0);
            float bossFireResistance = 1f - ((baseResistance + calcOpts.TargetFireResistance) / (character.Level * 5f)) * .75f;
            float bossNatureResistance = 1f - ((baseResistance + calcOpts.TargetNatureResistance) / (character.Level * 5f)) * .75f;

            #endregion

            #region Individual DPS
            #region Melee DPS
            float APDPS = (attackPower / 14f);
            float adjustedMHDPS = (wdpsMH + APDPS);
            float adjustedOHDPS = 0f;
            float dpsOHMeleeTotal = 0f;
            float dpsMoteOfAnger = 0f;

            float dpsMHMeleeNormal = adjustedMHDPS * cs.NormalHitModifierMH;
            float dpsMHMeleeCrits = adjustedMHDPS * cs.CritHitModifierMH;
            float dpsMHMeleeGlances = adjustedMHDPS * cs.GlancingHitModifier;
            float meleeMultipliers = cs.DamageReduction * bonusPhysicalDamage;

            float dpsMHMeleeTotal = ((dpsMHMeleeNormal + dpsMHMeleeCrits + dpsMHMeleeGlances) * cs.UnhastedMHSpeed / cs.HastedMHSpeed) * meleeMultipliers;

            if (cs.HastedOHSpeed != 0)  //(Check) if needed
            {
                adjustedOHDPS = (wdpsOH + APDPS) * .5f;
                float dpsOHMeleeNormal = adjustedOHDPS * cs.NormalHitModifierOH;
                float dpsOHMeleeCrits = adjustedOHDPS * cs.CritHitModifierOH;
                float dpsOHMeleeGlances = adjustedOHDPS * cs.GlancingHitModifier;
                dpsOHMeleeTotal = ((dpsOHMeleeNormal + dpsOHMeleeCrits + dpsOHMeleeGlances) * cs.UnhastedOHSpeed / cs.HastedOHSpeed) * meleeMultipliers;
            }

            // Generic MH & OH damage values used for SS, LL & WF
            float damageMHSwing = adjustedMHDPS * cs.UnhastedMHSpeed;
            float damageOHSwing = adjustedOHDPS * cs.UnhastedOHSpeed;

            if (cs.HastedOHSpeed != 0)  //(Check) if needed
                dpsMoteOfAnger = (damageMHSwing + damageOHSwing) / 2 * stats.MoteOfAnger;
            else
                dpsMoteOfAnger = damageMHSwing * stats.MoteOfAnger;

            float dpsMelee = dpsMHMeleeTotal + dpsOHMeleeTotal + dpsMoteOfAnger;
            #endregion

            #region Stormstrike DPS
            float dpsSS = 0f;
            if (character.ShamanTalents.Stormstrike == 1 && calcOpts.PriorityInUse(EnhanceAbility.StormStrike) && character.MainHand != null)
            {
                float swingDPSMH = (damageMHSwing + bonusSSDamage) * 1.25f * cs.HitsPerSMHSS;
                float swingDPSOH = (damageOHSwing + bonusSSDamage) * 1.25f * cs.HitsPerSOHSS;
                float SSnormal = (swingDPSMH * cs.YellowHitModifierMH) + (swingDPSOH * cs.YellowHitModifierOH);
                float SScrit = ((swingDPSMH * cs.YellowCritModifierMH) + (swingDPSOH * cs.YellowCritModifierOH)) * cs.CritMultiplierMelee;
                dpsSS = (SSnormal + SScrit) * cs.DamageReduction * focusedStrikes * (1f + enhance2T11) * bonusNatureDamage * bossNatureResistance;
            }
            #endregion

            #region Lavalash DPS
            //200% Weapon Damage; 40% bonus if FT imbued; 15/30% from talent; 10/20% per searing flames stack (max 5); 20% bonus from glyph; 20% bonus from Mastery(base) + 2.5%  per mastery
            float dpsLL = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.LavaLash) && character.OffHand != null)
            {
                float impLL = character.ShamanTalents.ImprovedLavaLash * 0.15f;
                float searingFlames = 0f;
                float flametongue = 0f;
                float glyphLL = 0f;
                if (calcOpts.PriorityInUse(EnhanceAbility.SearingTotem) && character.ShamanTalents.SearingFlames != 0)
                {
                    searingFlames = character.ShamanTalents.ImprovedLavaLash * 5f * 0.1f; //5f = number of stacks of searing flames (takes app. 8.25s to hit 5 stacks, LL CD is 10s).
                }
                if (calcOpts.OffhandImbue == "Flametongue")
                {
                    flametongue = .4f;
                }
                if (character.ShamanTalents.GlyphofLavaLash)
                {
                    glyphLL = .2f;
                }
                float lavalashDPS = damageOHSwing * cs.HitsPerSLL;
                float LLnormal = lavalashDPS * cs.YellowHitModifierOH;
                float LLcrit = lavalashDPS * cs.YellowCritModifierOH * cs.CritMultiplierMelee;
                dpsLL = (LLnormal + LLcrit) * (2f + searingFlames) * (1f + glyphLL + impLL) * (1f + flametongue) * (1f + enhance2T11) * mastery * bonusFireDamage * bossFireResistance;
            }
            #endregion

            #region Earth Shock DPS
            float dpsES = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.EarthShock))
            {
                float damageESBase = 931f;
                float coefES = .3858f;
                float damageES = concussionMultiplier * (damageESBase + coefES * spellPower);
                float shockdps = damageES / cs.AbilityCooldown(EnhanceAbility.EarthShock);
                float shockNormal = shockdps * cs.NatureSpellHitModifier;
                float shockCrit = shockdps * cs.NatureSpellCritModifier * cs.CritMultiplierSpell;
                dpsES = (shockNormal + shockCrit) * mastery * bonusNatureDamage * bossNatureResistance;
            }
            #endregion

            #region Flame Shock DPS
            float dpsFS = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.FlameShock))
            {
                float FSBaseNumTick = 18f / 3f;
                float damageFSBase = 531f;
                float damageFSDoTTickBase = 852f / FSBaseNumTick;
                float FSNumTick = cs.AverageFSDotTime / cs.AverageFSTickTime;
                float coefFS = 1.5f / 3.5f / 2f;
                float coefFSDoT = .6f;
                float damageFS = (damageFSBase + coefFS * spellPower) * concussionMultiplier;
                float damageFTDoT = ((damageFSDoTTickBase * FSNumTick) + coefFSDoT * spellPower) * concussionMultiplier;
                float usesCooldown = cs.AbilityCooldown(EnhanceAbility.FlameShock);
                float flameShockdps = damageFS / usesCooldown;
                float flameShockDoTdps = damageFTDoT / usesCooldown;
                float flameShockNormal = (flameShockdps + flameShockDoTdps) * cs.SpellHitModifier;
                float flameShockCrit = (flameShockdps + flameShockDoTdps) * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFS = (flameShockNormal + flameShockCrit) * mastery * bonusFireDamage * bossFireResistance;
            }
            #endregion

            #region Lightning Bolt DPS
            float dpsLB = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.LightningBolt))
            {
                float damageLBBase = 770f;
                float coefLB = .7143f;
                // LightningSpellPower is for totem of hex/the void/ancestral guidance
                float damageLB = concussionMultiplier * (damageLBBase + coefLB * (spellPower + stats.LightningSpellPower));
                float lbdps = damageLB / cs.AbilityCooldown(EnhanceAbility.LightningBolt);
                float lbNormal = lbdps * /*cs.LBSpellHitModifier*/cs.NatureSpellHitModifier;
                float lbCrit = lbdps * /*cs.LBSpellCritModifier*/cs.NatureSpellCritModifier * cs.CritMultiplierSpell;
                dpsLB = (lbNormal + lbCrit) * mastery * bonusNatureDamage * bossNatureResistance;
                if (character.ShamanTalents.GlyphofLightningBolt)
                    dpsLB *= 1.04f; // 4% bonus dmg if Lightning Bolt Glyph
            }
            #endregion

            #region Chain Lightning DPS
            //Single Target for now.  Add in Multi target later.  FIXME
            float dpsCL = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.ChainLightning))
            {
                float damageCLBase = 1092f;
                float coefCL = 0.5714f;
                float damageCL = concussionMultiplier * (damageCLBase + coefCL * spellPower);
                float cldps = damageCL / cs.AbilityCooldown(EnhanceAbility.ChainLightning);
                float clNormal = cldps * cs.NatureSpellHitModifier;
                float clCrit = cldps * cs.NatureSpellCritModifier * cs.CritMultiplierSpell;
                dpsCL = (clNormal + clCrit) * mastery * bonusNatureDamage * bossNatureResistance;
            }
            #endregion

            #region Windfury DPS
            float dpsWF = 0f;
            if (calcOpts.MainhandImbue == "Windfury" && character.MainHand != null)
            {
                float damageWFHit = damageMHSwing + (windfuryWeaponBonus / 14 * cs.UnhastedMHSpeed);
                float WFdps = damageWFHit * cs.HitsPerSWF;
                float WFnormal = WFdps * cs.YellowHitModifierMH;
                float WFcrit = WFdps * cs.YellowCritModifierMH * cs.CritMultiplierMelee;
                dpsWF = (WFnormal + WFcrit) * cs.DamageReduction * bonusPhysicalDamage * windfuryDamageBonus;
            }
            #endregion

            #region Lightning Shield DPS
            float dpsLS = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.LightningShield))
            {
                float damageLSBase = 391f;
                float damageLSCoef = 0.267f; // co-efficient from EnhSim
                float damageLS = shieldBonus * (damageLSBase + damageLSCoef * spellPower);
                float lsdps = damageLS * cs.StaticShockProcsPerS;
                float lsNormal = lsdps * cs.NatureSpellHitModifier;
                float lsCrit = lsdps * cs.NatureSpellCritModifier * cs.CritMultiplierSpell;
                dpsLS = (lsNormal + lsCrit) * mastery * bonusNatureDamage * bossNatureResistance;
            }
            #endregion

            #region Fire Totem DPS
            float dpsFireTotem = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.MagmaTotem))
            {
                float damageFireTotem = (268f + .067f * spellPower) * callofFlameBonus;
                float FireTotemdps = damageFireTotem / 2f * cs.FireTotemUptime;
                float FireTotemNormal = FireTotemdps * cs.SpellHitModifier;
                float FireTotemCrit = FireTotemdps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFireTotem = (FireTotemNormal + FireTotemCrit) * mastery * bonusFireDamage * bossFireResistance * cs.MultiTargetMultiplier;
            }
            else if (calcOpts.PriorityInUse(EnhanceAbility.SearingTotem))
            {
                float damageFireTotem = (96f + .1667f * spellPower) * callofFlameBonus;
                float FireTotemdps = damageFireTotem / 1.65f * cs.SearingTotemUptime;
                float FireTotemNormal = FireTotemdps * cs.SpellHitModifier;
                float FireTotemCrit = FireTotemdps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFireTotem = (FireTotemNormal + FireTotemCrit) * mastery * bonusFireDamage * bossFireResistance;
            }
            dpsFireTotem *= (1f - cs.FireElementalUptime);
            #endregion

            #region Fire Nova DPS
            float dpsFireNova = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.FireNova))
            {
                float damageFireNova = (686.0f + 0.143f * spellPower) * callofFlameBonus * fireNovaBonus;
                float FireNovadps = (damageFireNova / cs.AbilityCooldown(EnhanceAbility.FireNova)) * cs.FireTotemUptime;
                float FireNovaNormal = FireNovadps * cs.SpellHitModifier;
                float FireNovaCrit = FireNovadps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFireNova = (FireNovaNormal + FireNovaCrit) * mastery * bonusFireDamage * bossFireResistance * cs.MultiTargetMultiplier;
            }
            #endregion

            #region Flametongue Weapon DPS
            float dpsFT = 0f;
            /*if (calcOpts.MainhandImbue == "Flametongue")
            {
                float damageFTBase = 306f * cs.UnhastedOHSpeed / 4.0f;
                float damageFTCoef = 0.15396f * cs.UnhastedOHSpeed;
                float damageFT = damageFTBase + damageFTCoef * attackPower;
                float FTdps = damageFT * (cs.HitsPerSOH - cs.HitsPerSLL);
                float FTNormal = FTdps * cs.SpellHitModifier;
                float FTCrit = FTdps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFT += (FTNormal + FTCrit) * mastery * bonusFireDamage * bossFireResistance;
            }*/
            if (calcOpts.OffhandImbue == "Flametongue" && character.OffHand != null)
            {
                float damageFTBase = 306f * cs.UnhastedOHSpeed / 4.0f;
                float damageFTCoef = 0.15396f * cs.UnhastedOHSpeed;
                float damageFT = damageFTBase + damageFTCoef * attackPower;
                float FTdps = damageFT * (cs.HitsPerSOH - cs.HitsPerSLL);
                float FTNormal = FTdps * cs.SpellHitModifier;
                float FTCrit = FTdps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsFT += (FTNormal + FTCrit) * mastery * bonusFireDamage * bossFireResistance;
            }
            #endregion

            #region Unleash Elements DPS
            #region Unleash Windfury
            float dpsUW = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.UnleashElements) && calcOpts.MainhandImbue == "Windfury" && character.MainHand != null)
            {
                float damageUWHit = damageMHSwing * 1.25f;
                float UWdps = damageUWHit / cs.AbilityCooldown(EnhanceAbility.UnleashElements);
                float UWnormal = UWdps * cs.YellowCritModifierMH;
                float UWcrit = UWdps * cs.YellowCritModifierMH * cs.CritMultiplierMelee;
                dpsUW = (UWnormal + UWcrit) * cs.DamageReduction * bonusPhysicalDamage;
            }
            #endregion
            #region Unleash Flametongue
            float dpsUF = 0f;
            if (calcOpts.PriorityInUse(EnhanceAbility.UnleashElements) && calcOpts.OffhandImbue == "Flametongue" && character.OffHand != null)
            {
                float damageUFBase = 1070f;
                float damageUFCoef = 0.43f;
                float damageUF = damageUFBase + damageUFCoef * spellPower;
                float UFdps = damageUF / cs.AbilityCooldown(EnhanceAbility.UnleashElements);
                float UFnormal = UFdps * cs.SpellHitModifier;
                float UFcrit = UFdps * cs.SpellCritModifier * cs.CritMultiplierSpell;
                dpsUF = (UFnormal + UFcrit) * mastery * bonusFireDamage * bossFireResistance;
            }
            #endregion
            #endregion

            #region Pet calculations
            // needed for pets - spirit wolves and Fire Elemental
            bool critDebuff = character.ActiveBuffsContains("Heart of the Crusader") ||
                              character.ActiveBuffsContains("Master Poisioner") ||
                              character.ActiveBuffsContains("Totem of Wrath");
            bool critBuff = character.ActiveBuffsContains("Leader of the Pack") ||
                            character.ActiveBuffsContains("Rampage");
            float critbuffs = (critDebuff ? 0.03f : 0f) + (critBuff ? 0.05f : 0f);
            float meleeHitBonus = stats.PhysicalHit + StatConversion.GetHitFromRating(stats.HitRating) + dualWieldSpecialization;
            float petMeleeMissRate = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[calcOpts.TargetLevel - character.Level] - meleeHitBonus) + cs.AverageDodge;
            float petMeleeMultipliers = cs.DamageReduction * bonusPhysicalDamage;
            #endregion

            #region Doggies!
            // TTT article suggests 300-450 dps while the dogs are up plus 30% of AP
            // my analysis reveals they get 31% of shaman AP + 2 * their STR and base 206.17 dps.
            float dpsDogs = 0f;
            if (character.ShamanTalents.FeralSpirit == 1 && calcOpts.PriorityInUse(EnhanceAbility.FeralSpirits))
            {
                float FSglyphAP = character.ShamanTalents.GlyphofFeralSpirit ? attackPower * .3f : 0f;
                float soeBuff = character.ActiveBuffsContains("Strength of Earth Totem") ? 155f : 0f;  //(Check)
                float dogsStr = 331f + soeBuff; // base str = 331 plus SoE totem if active giving extra 178 str buff
                float dogsAgi = 113f + soeBuff; 
                float dogsAP = ((dogsStr * 2f - 20f) + .31f * attackPower + FSglyphAP) * (1f + unleashedRage);
                float dogsCrit = (StatConversion.GetCritFromAgility(dogsAgi, CharacterClass.Shaman) + critbuffs) * (1 + stats.BonusCritMultiplier);
                float dogsBaseSpeed = 1.5f;
                float dogsHitsPerS = 1f / (dogsBaseSpeed / (1f + stats.PhysicalHaste));
                float dogsBaseDamage = (206.17f + dogsAP / 14f) * dogsBaseSpeed;

                float dogsMeleeNormal = dogsBaseDamage * (1 - petMeleeMissRate - dogsCrit - cs.GlancingRate);
                float dogsMeleeCrits = dogsBaseDamage * dogsCrit * cs.CritMultiplierMelee;
                float dogsMeleeGlances = dogsBaseDamage * cs.GlancingHitModifier;

                float dogsTotalDamage = dogsMeleeNormal + dogsMeleeCrits + dogsMeleeGlances;

                dpsDogs = 2 * (30f / 120f) * dogsTotalDamage * dogsHitsPerS * petMeleeMultipliers;
                calc.SpiritWolf = new DPSAnalysis(dpsDogs, petMeleeMissRate, cs.AverageDodge, cs.GlancingRate, dogsCrit, 60f / cs.AbilityCooldown(EnhanceAbility.FeralSpirits));
            }
            else 
            { 
                calc.SpiritWolf = new DPSAnalysis(0, 0, 0, 0, 0, 0);
            }
            #endregion

            #region Fire Elemental
            if (calcOpts.PriorityInUse(EnhanceAbility.FireElemental))
            {
                float spellHitBonus = stats.SpellHit + StatConversion.GetHitFromRating(stats.HitRating);
                float petSpellMissRate = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[calcOpts.TargetLevel - character.Level] - spellHitBonus);
                float petSpellMultipliers = bonusFireDamage * bossFireResistance * callofFlameBonus;
                float petCritRate = critbuffs * (1 + stats.BonusCritMultiplier);
                calc.FireElemental = new FireElemental(attackPower, spellPower, stats.Intellect, cs, 
                        petCritRate, petMeleeMissRate, petMeleeMultipliers, petSpellMissRate, petSpellMultipliers);
            }
            else
                calc.FireElemental = new FireElemental(0, 0, 0, cs, 0, 0, 0, 0, 0);
            float dpsFireElemental = calc.FireElemental.getDPS();
            #endregion
            #endregion

            #region Set CalculatedStats
            calc.DPSPoints = dpsMelee + dpsSS + dpsLL + dpsES + dpsFS + dpsLB + dpsCL + dpsWF + dpsLS + dpsFireTotem + dpsFireNova + dpsFT + dpsDogs + dpsFireElemental;
            calc.SurvivabilityPoints = stats.Health * 0.02f;
            calc.OverallPoints = calc.DPSPoints + calc.SurvivabilityPoints;
            calc.DodgedAttacks = cs.AverageDodge * 100f;
            calc.ParriedAttacks = cs.AverageParry * 100f;
            calc.MissedAttacks = (1 - cs.AverageWhiteHitChance) * 100f;
            calc.AvoidedAttacks = calc.MissedAttacks + calc.DodgedAttacks + calc.ParriedAttacks;
            calc.YellowHit = (float)Math.Floor((float)(cs.AverageYellowHitChance * 10000f)) / 100f;
            calc.SpellHit = (float)Math.Floor((float)(cs.ChanceSpellHit * 10000f)) / 100f;
            calc.OverSpellHitCap = (float)Math.Floor((float)(cs.OverSpellHitCap * 10000f)) / 100f;
            calc.OverMeleeCritCap = (float)Math.Floor((float)(cs.OverMeleeCritCap * 10000f)) / 100f;
            calc.WhiteHit = (float)Math.Floor((float)(cs.AverageWhiteHitChance * 10000f)) / 100f;
            calc.MeleeCrit = (float)Math.Floor((float)((cs.DisplayMeleeCrit)) * 10000f) / 100f;
            calc.YellowCrit = (float)Math.Floor((float)((cs.DisplayYellowCrit)) * 10000f) / 100f;
            calc.SpellCrit = (float)Math.Floor((float)(cs.ChanceSpellCrit * 10000f)) / 100f;
            calc.GlancingBlows = cs.GlancingRate * 100f;
            calc.ArmorMitigation = (1f - cs.DamageReduction) * 100f;
            calc.MasteryRating = stats.MasteryRating;  //CATA FIXME!!
            calc.AttackPower = attackPower;
            calc.SpellPower = spellPower;
            calc.AvMHSpeed = cs.HastedMHSpeed;
            calc.AvOHSpeed = cs.HastedOHSpeed;
            calc.EDBonusCrit = cs.EDBonusCrit * 100f;
            calc.EDUptime = cs.EDUptime * 100f;
            calc.FlurryUptime = cs.FlurryUptime * 100f;
            calc.SecondsTo5Stack = cs.SecondsToFiveStack;
            calc.MHEnchantUptime = se.GetMHUptime() * 100f;
            calc.OHEnchantUptime = se.GetOHUptime() * 100f;
            calc.Trinket1Uptime = se.GetUptime(character.Trinket1) * 100f;
            calc.Trinket2Uptime = se.GetUptime(character.Trinket2) * 100f;
            calc.FireTotemUptime = (cs.FireTotemUptime + cs.SearingTotemUptime) * 100f;
            calc.ManaRegen = cs.ManaRegen * 5f;
            
            calc.TotalExpertiseMH = (float) Math.Floor(cs.ExpertiseBonusMH * 400f);
            calc.TotalExpertiseOH = (float) Math.Floor(cs.ExpertiseBonusOH * 400f);
            
            calc.SwingDamage = new DPSAnalysis(dpsMelee, 1 - cs.AverageWhiteHitChance, cs.AverageDodge, cs.GlancingRate, cs.AverageWhiteCritChance, cs.MeleePPM);
            calc.Stormstrike = new DPSAnalysis(dpsSS, 1 - cs.AverageYellowHitChance, cs.AverageDodge, -1, cs.AverageYellowCritChance, 60f / cs.AbilityCooldown(EnhanceAbility.StormStrike));
            calc.LavaLash = new DPSAnalysis(dpsLL, 1 - cs.ChanceYellowHitOH, cs.ChanceDodgeOH, -1, cs.ChanceYellowCritOH, 60f / cs.AbilityCooldown(EnhanceAbility.LavaLash));
            calc.EarthShock = new DPSAnalysis(dpsES, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceNatureSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.EarthShock));
            calc.FlameShock = new DPSAnalysis(dpsFS, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.FlameShock));
            calc.LightningBolt = new DPSAnalysis(dpsLB, 1 - cs.ChanceSpellHit, -1, -1, /*cs.ChanceLBSpellCrit*/cs.ChanceNatureSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.LightningBolt));
            calc.WindfuryAttack = new DPSAnalysis(dpsWF, 1 - cs.ChanceYellowHitMH, cs.ChanceDodgeMH, -1, cs.ChanceYellowCritMH, cs.WFPPM);
            calc.LightningShield = new DPSAnalysis(dpsLS, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceNatureSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.LightningShield));
            calc.ChainLightning = new DPSAnalysis(dpsCL, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceNatureSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.ChainLightning));
            calc.SearingMagma = new DPSAnalysis(dpsFireTotem, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit,
                calcOpts.Magma ? 60f / cs.AbilityCooldown(EnhanceAbility.MagmaTotem) : 60f / cs.AbilityCooldown(EnhanceAbility.SearingTotem));
            calc.FlameTongueAttack = new DPSAnalysis(dpsFT, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit, cs.FTPPM);
            calc.FireNova = new DPSAnalysis(dpsFireNova, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.FireNova));
            calc.UnleashWind = new DPSAnalysis(dpsUW, 1 - cs.ChanceYellowHitMH, cs.ChanceDodgeMH, -1, cs.ChanceYellowCritMH, 60f / cs.AbilityCooldown(EnhanceAbility.UnleashElements));
            calc.UnleashFlame = new DPSAnalysis(dpsUF, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit, 60f / cs.AbilityCooldown(EnhanceAbility.UnleashElements));
            calc.Other = new DPSAnalysis(0f, 1 - cs.ChanceSpellHit, -1, -1, cs.ChanceSpellCrit, 0f);

#if RAWR3 || RAWR4
            calc.Version = VERSION;
#else
            calculatedStats.Version = typeof(CalculationsEnhance).Assembly.GetName().Version.ToString();
#endif
            #endregion
            return calc;
        }
        #endregion

        #region Get Character Stats
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsBase = BaseStats.GetBaseStats(character); 
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsGearEnchantsBuffs = statsBaseGear + statsBuffs;

            float agiBase = (float)Math.Floor((float)(statsBase.Agility));
            float agiBonus = (float)Math.Floor((float)(statsGearEnchantsBuffs.Agility));
            float strBase = (float)Math.Floor((float)(statsBase.Strength));
            float strBonus = (float)Math.Floor((float)(statsGearEnchantsBuffs.Strength));
            float intBase = (float)Math.Floor((float)(statsBase.Intellect));
            float intBonus = (float)Math.Floor((float)(statsGearEnchantsBuffs.Intellect));
            float staBase = (float)Math.Floor((float)(statsBase.Stamina));  
            float staBonus = (float)Math.Floor((float)(statsGearEnchantsBuffs.Stamina));
            float spiBase = (float)Math.Floor((float)(statsBase.Spirit));  
            float spiBonus = (float)Math.Floor((float)(statsGearEnchantsBuffs.Spirit));
                        
            Stats statsTotal = GetRelevantStats(statsBase + statsGearEnchantsBuffs);
            statsTotal.BonusIntellectMultiplier = ((1 + statsBase.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusSpiritMultiplier = ((1 + statsBase.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsBase.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsBase.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsBase.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsBase.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusSpellPowerMultiplier = ((1 + statsBase.BonusSpellPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpellPowerMultiplier)) - 1;
            statsTotal.BonusCritMultiplier = ((1 + statsBase.BonusCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusCritMultiplier)) - 1;
            statsTotal.BonusSpellCritMultiplier = ((1 + statsBase.BonusSpellCritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpellCritMultiplier)) - 1;
            statsTotal.BonusPhysicalDamageMultiplier = ((1 + statsBase.BonusPhysicalDamageMultiplier) * (1 + statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier)) - 1;
            statsTotal.BonusNatureDamageMultiplier = ((1 + statsBase.BonusNatureDamageMultiplier) * (1 + statsGearEnchantsBuffs.BonusNatureDamageMultiplier)) - 1;
            statsTotal.BonusFireDamageMultiplier = ((1 + statsBase.BonusFireDamageMultiplier) * (1 + statsGearEnchantsBuffs.BonusFireDamageMultiplier)) - 1;
            statsTotal.BonusHealthMultiplier = ((1 + statsBase.BonusHealthMultiplier) * (1 + statsGearEnchantsBuffs.BonusHealthMultiplier)) - 1;
            statsTotal.BonusManaMultiplier = ((1 + statsBase.BonusManaMultiplier) * (1 + statsGearEnchantsBuffs.BonusManaMultiplier)) - 1;
            statsTotal.PhysicalHaste = ((1 + statsBase.PhysicalHaste) * (1 + statsGearEnchantsBuffs.PhysicalHaste)) - 1;
            statsTotal.SpellHaste = ((1 + statsBase.SpellHaste) * (1 + statsGearEnchantsBuffs.SpellHaste)) - 1;
            statsTotal.MasteryRating = ((1 + statsBase.MasteryRating) * (1 + statsGearEnchantsBuffs.MasteryRating)) - 1;
            
            statsTotal.Agility =   (float)Math.Floor((float)((agiBase + agiBonus) * (1 + statsTotal.BonusAgilityMultiplier)));
            statsTotal.Strength =  (float)Math.Floor((float)((strBase + strBonus) * (1 + statsTotal.BonusStrengthMultiplier)));
            statsTotal.Stamina =   (float)Math.Floor((float)((staBase + staBonus) * (1 + statsTotal.BonusStaminaMultiplier)));
            statsTotal.Intellect = (float)Math.Floor((float)((intBase + intBonus) * (1 + statsTotal.BonusIntellectMultiplier)));
            statsTotal.Spirit =    (float)Math.Floor((float)((spiBase + spiBonus) * (1 + statsTotal.BonusSpiritMultiplier)));
            statsTotal.Health = statsBase.Health + statsGearEnchantsBuffs.Health + StatConversion.GetHealthFromStamina(statsTotal.Stamina);
            statsTotal.Mana =   statsBase.Mana   + statsGearEnchantsBuffs.Mana   + StatConversion.GetManaFromIntellect(statsTotal.Intellect);
            statsTotal.Health = (float)Math.Floor((float)(statsTotal.Health * (1 + statsTotal.BonusHealthMultiplier)));
            statsTotal.Mana   = (float)Math.Floor((float)(statsTotal.Mana   * (1 + statsTotal.BonusManaMultiplier)));
            statsTotal.Expertise += 4 * character.ShamanTalents.UnleashedRage;

            statsTotal.AttackPower += statsTotal.Strength + 2f * statsTotal.Agility;  //(Check)
            statsTotal.AttackPower = statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier);

            float SPfromAP = statsTotal.AttackPower * .5f;  //Mental Quickness
            statsTotal.SpellPower += SPfromAP + statsTotal.Intellect;
            statsTotal.SpellPower = statsTotal.SpellPower * (1f + statsTotal.BonusSpellPowerMultiplier);

            return statsTotal;
        }
        #endregion

        #region Buff Functions
        /*public override void SetDefaults(Character character)
        {
            // add shaman buffs
            character.ActiveBuffsAdd("Strength of Earth Totem");
            character.ActiveBuffsAdd("Heroism/Bloodlust");
            character.ActiveBuffsAdd("Windfury Totem");

            // add other raid buffs
            character.ActiveBuffsAdd("Blessing of Wisdom");
            character.ActiveBuffsAdd("Improved Blessing of Wisdom");
            character.ActiveBuffsAdd("Blessing of Might");
            character.ActiveBuffsAdd("Improved Blessing of Might");
            character.ActiveBuffsAdd("Sanctified Retribution");
            character.ActiveBuffsAdd("Blessing of Sanctuary");
            character.ActiveBuffsAdd("Swift Retribution");
            character.ActiveBuffsAdd("Arcane Intellect");
            character.ActiveBuffsAdd("Unleashed Rage");
            character.ActiveBuffsAdd("Commanding Shout");
            character.ActiveBuffsAdd("Commanding Presence (Health)");
            character.ActiveBuffsAdd("Leader of the Pack");
            character.ActiveBuffsAdd("Elemental Oath");
            character.ActiveBuffsAdd("Wrath of Air Totem");
            character.ActiveBuffsAdd("Totem of Wrath (Spell Power)");
            character.ActiveBuffsAdd("Power Word: Fortitude");
            character.ActiveBuffsAdd("Improved Power Word: Fortitude");
            character.ActiveBuffsAdd("Mark of the Wild");
            character.ActiveBuffsAdd("Improved Mark of the Wild");
            character.ActiveBuffsAdd("Blessing of Kings");
            character.ActiveBuffsAdd("Blessing of Kings (Str/Sta Bonus)");
            character.ActiveBuffsAdd("Sunder Armor");
            character.ActiveBuffsAdd("Faerie Fire");
            character.ActiveBuffsAdd("Heart of the Crusader");
            character.ActiveBuffsAdd("Blood Frenzy");
            character.ActiveBuffsAdd("Improved Scorch");
            character.ActiveBuffsAdd("Curse of the Elements");
            character.ActiveBuffsAdd("Hunting Party");
            character.ActiveBuffsAdd("Flask of Endless Rage");
            character.ActiveBuffsAdd("Potion of Speed");
            character.ActiveBuffsAdd("Fish Feast");

            if (character.HasProfession(Profession.Alchemy))
            {
                character.ActiveBuffsAdd(("Flask of Endless Rage (Mixology)"));
            }

            character.EnforceGemRequirements = true; // set default to be true for Enhancement Shaman
        }*/

        private Item RemoveAddedBuffs(Stats addedBuffs)
        {
            Item result = new Item();
            result.Stats = addedBuffs * -1;
            // to this point works fine for additive stats doesn't work for multiplicative ones.
            result.Stats.BonusAgilityMultiplier = 1 / (1 - result.Stats.BonusAgilityMultiplier) - 1;
            result.Stats.BonusStrengthMultiplier = 1 / (1 - result.Stats.BonusStrengthMultiplier) - 1;
            result.Stats.BonusIntellectMultiplier = 1 / (1 - result.Stats.BonusIntellectMultiplier) - 1;
            result.Stats.BonusSpiritMultiplier = 1 / (1 - result.Stats.BonusSpiritMultiplier) - 1;
            result.Stats.BonusAttackPowerMultiplier = 1 / (1 - result.Stats.BonusAttackPowerMultiplier) - 1;
            result.Stats.BonusSpellPowerMultiplier = 1 / (1 - result.Stats.BonusSpellPowerMultiplier) - 1;
            result.Stats.BonusCritMultiplier = 1 / (1 - result.Stats.BonusCritMultiplier) - 1;
            result.Stats.BonusSpellCritMultiplier = 1 / (1 - result.Stats.BonusSpellCritMultiplier) - 1;
            result.Stats.BonusDamageMultiplier = 1 / (1 - result.Stats.BonusDamageMultiplier) - 1;
            result.Stats.BonusPhysicalDamageMultiplier = 1 / (1 - result.Stats.BonusPhysicalDamageMultiplier) - 1;
            result.Stats.BonusFireDamageMultiplier = 1 / (1 - result.Stats.BonusFireDamageMultiplier) - 1;
            result.Stats.BonusNatureDamageMultiplier = 1 / (1 - result.Stats.BonusNatureDamageMultiplier) - 1;
            result.Stats.BonusHealthMultiplier = 1 / (1 - result.Stats.BonusHealthMultiplier) - 1;
            result.Stats.BonusManaMultiplier = 1 / (1 - result.Stats.BonusManaMultiplier) - 1;
            result.Stats.PhysicalHaste = 1 / (1 - result.Stats.PhysicalHaste) - 1;
            result.Stats.SpellHaste = 1 / (1 - result.Stats.SpellHaste) - 1;
            return result; 
        }
        #endregion

        #region RelevantGlyphs
        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Feral Spirit");
                _relevantGlyphs.Add("Glyph of Fire Elemental Totem");
                _relevantGlyphs.Add("Glyph of Flame Shock");
                _relevantGlyphs.Add("Glyph of Flametongue Weapon");
                _relevantGlyphs.Add("Glyph of Lava Lash");
                _relevantGlyphs.Add("Glyph of Lightning Bolt");
                _relevantGlyphs.Add("Glyph of Shocking");
                _relevantGlyphs.Add("Glyph of Stormstrike");
                _relevantGlyphs.Add("Glyph of Windfury Weapon");
                _relevantGlyphs.Add("Glyph of Chain Lightning");
            }
            return _relevantGlyphs;
        }
        #endregion

        #region Relevant Stats
        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]
                    {
                        ItemType.None,
                        //ItemType.Cloth,
                        //ItemType.Leather,
                        ItemType.Mail,
                        ItemType.Totem,ItemType.Relic,
                    //	ItemType.Staff,
                    //	ItemType.TwoHandMace, // Removed two handed options so as not to screw up recommendations
                    //  ItemType.TwoHandAxe,  // Two handers are simply NOT viable for Enhancement Shamans
                    //  ItemType.Dagger,      // Nor are daggers
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.FistWeapon
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override bool IsItemRelevant(Item item)
        {
            if ((item.Slot == ItemSlot.Ranged && (item.Type != ItemType.Totem && item.Type != ItemType.Relic))) 
                return false;
            if (item.Slot == ItemSlot.OffHand && item.Type == ItemType.None)
                return false;
            return base.IsItemRelevant(item);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
                {
                    Agility = stats.Agility,
                    Strength = stats.Strength,
                    Intellect = stats.Intellect,
                    Spirit = stats.Spirit,
                    AttackPower = stats.AttackPower,
                    CritRating = stats.CritRating,
                    HitRating = stats.HitRating,
                    Stamina = stats.Stamina,
                    HasteRating = stats.HasteRating,
                    Expertise = stats.Expertise,
                    ExpertiseRating = stats.ExpertiseRating,
                    MasteryRating = stats.MasteryRating,
                    TargetArmorReduction = stats.TargetArmorReduction,
                    WeaponDamage = stats.WeaponDamage,
                    BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                    BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                    BonusCritMultiplier = stats.BonusCritMultiplier,
                    BonusDamageMultiplier = stats.BonusDamageMultiplier,
                    BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                    BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                    BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                    BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                    BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                    BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                    BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                    BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                    BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                    BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                    BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                    BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                    BonusHealthMultiplier = stats.BonusHealthMultiplier,
                    BonusManaMultiplier = stats.BonusManaMultiplier,
                    Health = stats.Health,
                    Mana = stats.Mana,
                    SpellPower = stats.SpellPower,
                    LightningSpellPower = stats.LightningSpellPower,
                    BonusSSDamage = stats.BonusSSDamage,
                    BonusWFAttackPower = stats.BonusWFAttackPower,
                    HighestStat = stats.HighestStat,
                    Paragon = stats.Paragon,
                    Enhance2T11 = stats.Enhance2T11,
                    Enhance4T11 = stats.Enhance4T11,
                    PhysicalHit = stats.PhysicalHit,
                    PhysicalHaste = stats.PhysicalHaste,
                    PhysicalCrit = stats.PhysicalCrit,
                    SpellHit = stats.SpellHit,
                    SpellHaste = stats.SpellHaste,
                    SpellCrit = stats.SpellCrit,
                    SpellCritOnTarget = stats.SpellCritOnTarget,
                    Mp5 = stats.Mp5,
                    ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                    ManaRestoreFromBaseManaPPM = stats.ManaRestoreFromBaseManaPPM,
                    DeathbringerProc = stats.DeathbringerProc,
                    MoteOfAnger = stats.MoteOfAnger,
                    SnareRootDurReduc = stats.SnareRootDurReduc,
                    FearDurReduc = stats.FearDurReduc,
                    StunDurReduc = stats.StunDurReduc,
                    MovementSpeed = stats.MovementSpeed,
                    ShadowDamage = stats.ShadowDamage,
                    ArcaneDamage = stats.ArcaneDamage,
                    HolyDamage = stats.HolyDamage,
                    NatureDamage = stats.NatureDamage,
                    FrostDamage = stats.FrostDamage,
                    FireDamage = stats.FireDamage,
                };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantTrigger(effect.Trigger))
                {
                    if (relevantStats(effect.Stats))
                        s.AddSpecialEffect(effect);
                    else 
                    {
                        foreach (SpecialEffect subEffect in effect.Stats.SpecialEffects())
                            if (HasRelevantTrigger(subEffect.Trigger) && relevantStats(subEffect.Stats))
                                s.AddSpecialEffect(effect);
                    }
                }
            }
            return s;
        }

        public bool HasRelevantTrigger(Trigger trigger)
        {
            return (trigger == Trigger.Use ||
                    trigger == Trigger.SpellHit ||
                    trigger == Trigger.SpellCrit ||
                    trigger == Trigger.SpellCast ||
                    trigger == Trigger.DamageSpellHit ||
                    trigger == Trigger.DamageSpellCrit ||
                    trigger == Trigger.DamageSpellCast ||
                    trigger == Trigger.MeleeHit ||
                    trigger == Trigger.MeleeCrit ||
                    trigger == Trigger.MeleeAttack ||
                    trigger == Trigger.PhysicalHit ||
                    trigger == Trigger.PhysicalCrit ||
                    trigger == Trigger.WhiteHit ||
                    trigger == Trigger.WhiteCrit ||
                    trigger == Trigger.WhiteAttack ||
                    trigger == Trigger.DamageDone ||
                    trigger == Trigger.DamageOrHealingDone ||
                    trigger == Trigger.ShamanLightningBolt ||
                    trigger == Trigger.ShamanLavaLash ||
                    trigger == Trigger.ShamanShock ||
                    trigger == Trigger.ShamanStormStrike ||
                    trigger == Trigger.ShamanShamanisticRage ||
                    trigger == Trigger.ShamanFlameShockDoTTick ||
                    trigger == Trigger.DoTTick);
        }

        public override bool IsBuffRelevant(Buff buff, Character character)
        {
            if (buff.Name.StartsWith("Amplify Magic"))
                return false;
            if (buff.AllowedClasses.Contains(CharacterClass.Shaman))
                return base.IsBuffRelevant(buff, character);
            else
                return false;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            if (relevantStats(stats))
                return true;
            if (irrelevantStats(stats)) 
                return false;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantTrigger(effect.Trigger))
                    foreach (SpecialEffect subEffect in effect.Stats.SpecialEffects())
                    {
                        if (HasRelevantTrigger(subEffect.Trigger) && relevantStats(subEffect.Stats))
                            return true;
                    }
                    if (relevantStats(effect.Stats))
                        return true;
            }
            return false;
        }

        private bool relevantStats(Stats stats)
        {
            return (stats.Agility + stats.Intellect + stats.Stamina + stats.Strength + stats.Spirit +
                stats.AttackPower + stats.SpellPower + stats.Mana + stats.WeaponDamage + stats.Health +
                stats.MasteryRating + stats.TargetArmorReduction +
                stats.Expertise + stats.ExpertiseRating + stats.HasteRating + stats.CritRating + stats.HitRating + 
                stats.BonusAgilityMultiplier + stats.BonusAttackPowerMultiplier + stats.BonusCritMultiplier + 
                stats.BonusStrengthMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusIntellectMultiplier + 
                stats.BonusSpiritMultiplier + stats.BonusDamageMultiplier + stats.BonusPhysicalDamageMultiplier + 
                stats.BonusNatureDamageMultiplier + stats.BonusFireDamageMultiplier + stats.BonusSpellCritMultiplier +
                stats.BonusHealthMultiplier + stats.BonusManaMultiplier + stats.DeathbringerProc +
                stats.PhysicalCrit + stats.PhysicalHaste + stats.PhysicalHit + stats.Paragon + stats.BonusShadowDamageMultiplier +
                stats.SpellCrit + stats.SpellCritOnTarget + stats.SpellHaste + stats.SpellHit + stats.HighestStat +
                stats.LightningSpellPower + stats.BonusWFAttackPower + stats.BonusSSDamage + stats.MoteOfAnger +
                stats.NatureDamage + stats.FireDamage + stats.FrostDamage + stats.ArcaneDamage + stats.HolyDamage + stats.ShadowDamage +  
                stats.Mp5 + stats.ManaRestoreFromMaxManaPerSecond + stats.ManaRestoreFromBaseManaPPM +
                stats.SnareRootDurReduc + stats.FearDurReduc + stats.StunDurReduc + stats.MovementSpeed) != 0;
        }

        private bool irrelevantStats(Stats stats)
        {
            return (stats.TigersFuryCooldownReduction + stats.BonusRageGen) > 0;
        }
        #endregion

        #region Custom Chart Data
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Combat Table (White)":
                    CharacterCalculationsEnhance currentCalculationsEnhanceWhite = GetCharacterCalculations(character) as CharacterCalculationsEnhance;
                    ComparisonCalculationEnhance calcMissWhite = new ComparisonCalculationEnhance() { Name = "    Miss    " };
                    ComparisonCalculationEnhance calcDodgeWhite = new ComparisonCalculationEnhance() { Name = "   Dodge   " };
                    ComparisonCalculationEnhance calcCritWhite = new ComparisonCalculationEnhance() { Name = "  Crit  " };
                    ComparisonCalculationEnhance calcGlanceWhite = new ComparisonCalculationEnhance() { Name = " Glance " };
                    ComparisonCalculationEnhance calcHitWhite = new ComparisonCalculationEnhance() { Name = "Hit" };
                    if (currentCalculationsEnhanceWhite != null)
                    {
                        calcMissWhite.SubPoints = new float[2];
                        calcMissWhite.DPSPoints = 0;
                        calcMissWhite.OverallPoints = calcMissWhite.SubPoints[1] = 100 - currentCalculationsEnhanceWhite.WhiteHit;
                        calcDodgeWhite.OverallPoints = calcDodgeWhite.DPSPoints = currentCalculationsEnhanceWhite.DodgedAttacks;
                        calcCritWhite.SubPoints = new float[2];
                        calcCritWhite.SubPoints[1] = currentCalculationsEnhanceWhite.OverMeleeCritCap;
                        calcCritWhite.OverallPoints = currentCalculationsEnhanceWhite.MeleeCrit - 4.8f;
                        calcCritWhite.DPSPoints = calcCritWhite.OverallPoints - calcCritWhite.SubPoints[1];
                        calcGlanceWhite.OverallPoints = calcGlanceWhite.DPSPoints = currentCalculationsEnhanceWhite.GlancingBlows;
                        calcHitWhite.OverallPoints = calcHitWhite.DPSPoints = 100f - calcMissWhite.OverallPoints 
                                                                                   - calcDodgeWhite.OverallPoints 
                                                                                   - calcCritWhite.DPSPoints 
                                                                                   - calcGlanceWhite.OverallPoints;
                    }
                    return new ComparisonCalculationBase[] { calcMissWhite, calcDodgeWhite, calcCritWhite, calcGlanceWhite, calcHitWhite };

                case "Combat Table (Yellow)":
                    CharacterCalculationsEnhance currentCalculationsEnhanceYellow = GetCharacterCalculations(character) as CharacterCalculationsEnhance;
                    ComparisonCalculationEnhance calcMissYellow = new ComparisonCalculationEnhance() { Name = "    Miss    " };
                    ComparisonCalculationEnhance calcDodgeYellow = new ComparisonCalculationEnhance() { Name = "   Dodge   " };
                    ComparisonCalculationEnhance calcCritYellow = new ComparisonCalculationEnhance() { Name = "  Crit  " };
                    ComparisonCalculationEnhance calcHitYellow = new ComparisonCalculationEnhance() { Name = "Hit" };
                    if (currentCalculationsEnhanceYellow != null)
                    {
                        calcMissYellow.OverallPoints = calcMissYellow.DPSPoints = 100f - currentCalculationsEnhanceYellow.YellowHit;
                        calcDodgeYellow.OverallPoints = calcDodgeYellow.DPSPoints = currentCalculationsEnhanceYellow.DodgedAttacks;
                        calcCritYellow.OverallPoints = calcCritYellow.DPSPoints = currentCalculationsEnhanceYellow.YellowCrit;
                        calcHitYellow.OverallPoints = calcHitYellow.DPSPoints = (100f - calcMissYellow.OverallPoints -
                        calcDodgeYellow.OverallPoints - calcCritYellow.OverallPoints);
                    }
                    return new ComparisonCalculationBase[] { calcMissYellow, calcDodgeYellow, calcCritYellow, calcHitYellow };

                case "Combat Table (Spell)":
                    CharacterCalculationsEnhance currentCalculationsEnhanceSpell = GetCharacterCalculations(character) as CharacterCalculationsEnhance;
                    ComparisonCalculationEnhance calcMissSpell = new ComparisonCalculationEnhance() { Name = "    Miss    " };
                    ComparisonCalculationEnhance calcCritSpell = new ComparisonCalculationEnhance() { Name = "  Crit  " };
                    ComparisonCalculationEnhance calcHitSpell = new ComparisonCalculationEnhance() { Name = "Hit" };
                    if (currentCalculationsEnhanceSpell != null)
                    {
                        calcMissSpell.OverallPoints = calcMissSpell.DPSPoints = 100f - currentCalculationsEnhanceSpell.SpellHit;
                        calcCritSpell.OverallPoints = calcCritSpell.DPSPoints = currentCalculationsEnhanceSpell.SpellCrit;
                        calcHitSpell.OverallPoints = calcHitSpell.DPSPoints = (100f - calcMissSpell.OverallPoints - calcCritSpell.OverallPoints);
                    }
                    return new ComparisonCalculationBase[] { calcMissSpell, calcCritSpell, calcHitSpell };

                case "Relative Gem Values":
                    float dpsBase = GetCharacterCalculations(character).OverallPoints;
                    float dpsStr = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { Strength = 40 } }).OverallPoints - dpsBase);
                    float dpsAgi = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { Agility = 40 } }).OverallPoints - dpsBase);
                    float dpsInt = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { Intellect = 40 } }).OverallPoints - dpsBase);
                    float dpsCrit = (GetCharacterCalculations(character, new Item()  { Stats = new Stats() { CritRating = 40 } }).OverallPoints - dpsBase);
                    float dpsExp = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { ExpertiseRating = 40 } }).OverallPoints - dpsBase);
                    float dpsHaste = (GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 40 } }).OverallPoints - dpsBase);
                    float dpsHit = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { HitRating = 40 } }).OverallPoints - dpsBase);
                    float dpsDmg = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { WeaponDamage = 1 } }).OverallPoints - dpsBase);
                    float dpsMast = (GetCharacterCalculations(character, new Item()  { Stats = new Stats() { MasteryRating = 40 } }).OverallPoints - dpsBase);
                    //float dpsSta = (GetCharacterCalculations(character, new Item()   { Stats = new Stats() { Stamina = 60 } }).OverallPoints - dpsBase);

                    return new ComparisonCalculationBase[] { 
                        //new ComparisonCalculationEnhance() { Name = "60 Stamina", OverallPoints = dpsSta, DPSPoints = dpsSta },
                        new ComparisonCalculationEnhance() { Name = "40 Agility", OverallPoints = dpsAgi, DPSPoints = dpsAgi },
                        new ComparisonCalculationEnhance() { Name = "40 Strength", OverallPoints = dpsStr, DPSPoints = dpsStr },
                        new ComparisonCalculationEnhance() { Name = "40 Intellect", OverallPoints = dpsInt, DPSPoints = dpsInt },
                        new ComparisonCalculationEnhance() { Name = "40 Crit Rating", OverallPoints = dpsCrit, DPSPoints = dpsCrit },
                        new ComparisonCalculationEnhance() { Name = "40 Expertise Rating", OverallPoints = dpsExp, DPSPoints = dpsExp },
                        new ComparisonCalculationEnhance() { Name = "40 Haste Rating", OverallPoints = dpsHaste, DPSPoints = dpsHaste },
                        new ComparisonCalculationEnhance() { Name = "40 Hit Rating", OverallPoints = dpsHit, DPSPoints = dpsHit },
                        new ComparisonCalculationEnhance() { Name = "40 Mastery Rating", OverallPoints = dpsMast, DPSPoints = dpsMast }
                    };

                case "MH Weapon Speeds":
                    if (character.MainHand == null)
                        return new ComparisonCalculationBase[0];
                    ComparisonCalculationBase MHonePointFour = CheckWeaponSpeedEffect(character, 1.4f, true);
                    ComparisonCalculationBase MHonePointFive = CheckWeaponSpeedEffect(character, 1.5f, true);
                    ComparisonCalculationBase MHonePointSix = CheckWeaponSpeedEffect(character, 1.6f, true);
                    ComparisonCalculationBase MHonePointSeven = CheckWeaponSpeedEffect(character, 1.7f, true);
                    ComparisonCalculationBase MHonePointEight = CheckWeaponSpeedEffect(character, 1.8f, true);
                    ComparisonCalculationBase MHtwoPointThree = CheckWeaponSpeedEffect(character, 2.3f, true);
                    ComparisonCalculationBase MHtwoPointFour = CheckWeaponSpeedEffect(character, 2.4f, true);
                    ComparisonCalculationBase MHtwoPointFive = CheckWeaponSpeedEffect(character, 2.5f, true);
                    ComparisonCalculationBase MHtwoPointSix = CheckWeaponSpeedEffect(character, 2.6f, true);
                    ComparisonCalculationBase MHtwoPointSeven = CheckWeaponSpeedEffect(character, 2.7f, true);
                    ComparisonCalculationBase MHtwoPointEight = CheckWeaponSpeedEffect(character, 2.8f, true);
                    return new ComparisonCalculationBase[] { MHonePointFour, MHonePointFive, MHonePointSix, MHonePointSeven, MHonePointEight, 
                                                             MHtwoPointThree, MHtwoPointFour, MHtwoPointFive, MHtwoPointSix, MHtwoPointSeven, MHtwoPointEight };

                case "OH Weapon Speeds":
                    if (character.OffHand == null)
                        return new ComparisonCalculationBase[0];
                    ComparisonCalculationBase OHonePointFour = CheckWeaponSpeedEffect(character, 1.4f, false);
                    ComparisonCalculationBase OHonePointFive = CheckWeaponSpeedEffect(character, 1.5f, false);
                    ComparisonCalculationBase OHonePointSix = CheckWeaponSpeedEffect(character, 1.6f, false);
                    ComparisonCalculationBase OHonePointSeven = CheckWeaponSpeedEffect(character, 1.7f, false);
                    ComparisonCalculationBase OHonePointEight = CheckWeaponSpeedEffect(character, 1.8f, false);
                    ComparisonCalculationBase OHtwoPointThree = CheckWeaponSpeedEffect(character, 2.3f, false);
                    ComparisonCalculationBase OHtwoPointFour = CheckWeaponSpeedEffect(character, 2.4f, false);
                    ComparisonCalculationBase OHtwoPointFive = CheckWeaponSpeedEffect(character, 2.5f, false);
                    ComparisonCalculationBase OHtwoPointSix = CheckWeaponSpeedEffect(character, 2.6f, false);
                    ComparisonCalculationBase OHtwoPointSeven = CheckWeaponSpeedEffect(character, 2.7f, false);
                    ComparisonCalculationBase OHtwoPointEight = CheckWeaponSpeedEffect(character, 2.8f, false);
                    return new ComparisonCalculationBase[] { OHonePointFour, OHonePointFive, OHonePointSix, OHonePointSeven, OHonePointEight, 
                                                             OHtwoPointThree, OHtwoPointFour, OHtwoPointFive, OHtwoPointSix, OHtwoPointSeven, OHtwoPointEight };

                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        private ComparisonCalculationBase CheckWeaponSpeedEffect(Character character, float newSpeed, bool mainHand)
        {
            float baseSpeed = 0f;
            int minDamage = 0;
            int maxDamage = 0;
            Item newWeapon;
            CharacterCalculationsBase baseCalc = Calculations.GetCharacterCalculations(character);
            Character deltaChar = character.Clone();

            if (mainHand)
            {
                newWeapon = deltaChar.MainHand.Item.Clone();
                baseSpeed = deltaChar.MainHand.Speed;
                minDamage = deltaChar.MainHand.MinDamage;
                maxDamage = deltaChar.MainHand.MaxDamage;
            }
            else
            {
                newWeapon = deltaChar.OffHand.Item.Clone();
                baseSpeed = deltaChar.OffHand.Speed;
                minDamage = deltaChar.OffHand.MinDamage;
                maxDamage = deltaChar.OffHand.MaxDamage;
            }
            newWeapon.MinDamage = (int)Math.Round(minDamage / baseSpeed * newSpeed);
            newWeapon.MaxDamage = (int)Math.Round(maxDamage / baseSpeed * newSpeed);
            newWeapon.Speed = newSpeed;
            String speed = newSpeed.ToString() + " Speed";
            deltaChar.IsLoading = true; // forces item instance to avoid invalidating and reloading from cache
            if (mainHand)
                deltaChar.MainHand = new ItemInstance(newWeapon, character.MainHand.Gem1, character.MainHand.Gem2, character.MainHand.Gem3, character.MainHand.Enchant);
            else
                deltaChar.OffHand = new ItemInstance(newWeapon, deltaChar.OffHand.Gem1, deltaChar.OffHand.Gem2, deltaChar.OffHand.Gem3, deltaChar.OffHand.Enchant);
            ComparisonCalculationBase result = Calculations.GetCharacterComparisonCalculations(baseCalc, deltaChar, speed, baseSpeed == newWeapon.Speed);
            deltaChar.IsLoading = false;
            result.Item = null;
            return result;
        }

        #endregion
    }
}
