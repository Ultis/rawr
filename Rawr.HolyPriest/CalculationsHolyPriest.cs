using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.HolyPriest
{
	[Rawr.Calculations.RawrModelInfo("HolyPriest", "Spell_Holy_Renew", Character.CharacterClass.Priest)]
	public class CalculationsHolyPriest : CalculationsBase 
    {
        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Priest; } }

        private string _currentChartName = null;
        
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                switch (_currentChartName)
                {
                    case "Spell HpS":
                        _subPointNameColors.Add("HpS", System.Drawing.Color.Red);
                        break;
                    case "Spell HpM":
                        _subPointNameColors.Add("HpM", System.Drawing.Color.Red);
                        break;
                    default:
                        _subPointNameColors.Add("HPS-Burst", System.Drawing.Color.Red);
                        _subPointNameColors.Add("HPS-Sustained", System.Drawing.Color.Blue);
                        _subPointNameColors.Add("Survivability", System.Drawing.Color.Green);
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
					"Basic Stats:Health",
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
                    "Basic Stats:Resilience",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
                    "Basic Stats:Spell Power",
					"Basic Stats:Mp5",
					"Basic Stats:Regen InFSR",
					"Basic Stats:Regen OutFSR",
                    "Basic Stats:Spell Crit",
					"Basic Stats:Healing Crit",
					"Basic Stats:Spell Haste",
                    "Spells:Greater Heal",
                    "Spells:Flash Heal",
				    "Spells:Binding Heal",
                    "Spells:Renew",
                    "Spells:Prayer of Mending",
                    "Spells:Power Word Shield",
                    "Spells:PoH",
				    "Spells:Holy Nova",
                    "Spells:Lightwell",
				    "Spells:CoH",
                    "Spells:Penance",
                    "Spells:Gift of the Naaru"
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelHolyPriest();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { "Spell HpS", "Spell HpM", "Spell AoE HpS", "Spell AoE HpM", "Relative Stat Values" };
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationHolyPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsHolyPriest(); }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]{
                        Item.ItemType.None,
                        Item.ItemType.Cloth,
                        Item.ItemType.Dagger,
                        Item.ItemType.Wand,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        private float Calc_GreaterHeal_Burst(Stats stats, Char character)
        {

        
            return 0;
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            Stats statsRace = GetRaceStats(character);
            CharacterCalculationsHolyPriest calculatedStats = new CharacterCalculationsHolyPriest();
            CalculationOptionsPriest calculationOptions = character.CalculationOptions as CalculationOptionsPriest;
            if (calculationOptions == null)
                return null;

            calculatedStats.Race = character.Race;
            calculatedStats.BasicStats = stats;
            calculatedStats.Character = character;

            calculatedStats.SpiritRegen = (float)Math.Floor(5 * (0.001f + 0.0093271 * calculatedStats.BasicStats.Spirit * Math.Sqrt(calculatedStats.BasicStats.Intellect)));
            calculatedStats.RegenInFSR = calculatedStats.SpiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration;
            calculatedStats.RegenOutFSR = calculatedStats.SpiritRegen;

            int Rotation = calculationOptions.Rotation;
            if (Rotation == 0) // OOOH MAGIC TANK ROTATION!!!
            {
                if (character.PriestTalents.Penance > 0)
                {
                    if (character.PriestTalents.DivineFury < 5)
                        Rotation = 8; // Disc-MT, Using Flash Heal instead of GH
                    else
                        Rotation = 7; // Disc-MT
                }
                else
                    Rotation = 5; // Holy-MT
            }
            else if (Rotation == 1) // Raid rotation
            {
                if (character.PriestTalents.Penance > 0)
                    Rotation = 9; // Disc-Raid (PW:S/Penance/Flash)
                else if (character.PriestTalents.CircleOfHealing > 0)
                    Rotation = 6; // Holy-Raid (CoH/FH)
                else
                    Rotation = 3; // Fallback to Flash Heal raid.

            }

            Stats simstats = calculatedStats.BasicStats.Clone();

            Stats UseProcs = new Stats();

            // Pre calc Procs (Power boosting Procs)
            if (calculationOptions.ModelProcs)
            {
                if (simstats.SpiritFor20SecOnUse2Min > 0)
                    // Trinkets with Use: Increases Spirit with. (Like Earring of Soulful Meditation / Bangle of Endless blessings)
                    UseProcs.Spirit += simstats.SpiritFor20SecOnUse2Min * 20f / 120f;              
//                if (simstats.BangleProc > 0)
                    // Bangle of Endless Blessings. Use: 130 spirit over 20 seconds. 120 sec cd.
                    //UseProcs.Spirit += 130f * 20f / 120f;              
                if (simstats.SpellPowerFor15SecOnUse2Min > 0)
                    UseProcs.SpellPower += simstats.SpellPowerFor15SecOnUse2Min * 15f / 120f;
                if (simstats.SpellPowerFor15SecOnUse90Sec > 0)
                    UseProcs.SpellPower += simstats.SpellPowerFor15SecOnUse90Sec * 15f / 90f;
                if (simstats.SpellPowerFor20SecOnUse2Min > 0)
                    UseProcs.SpellPower += simstats.SpellPowerFor20SecOnUse2Min * 20f / 120f;
                if (simstats.HasteRatingFor20SecOnUse5Min > 0)
                    UseProcs.SpellHaste += simstats.HasteRatingFor20SecOnUse5Min * 20f / 300f / 15.77f / 100f;
                if (simstats.HasteRatingFor20SecOnUse2Min > 0)
                    UseProcs.SpellHaste += simstats.HasteRatingFor20SecOnUse2Min * 20f / 120f / 15.77f / 100f;
            }

            /*     
                        + calculatedStats.BasicStats.MementoProc * 3f * 5f / (45f + 9.5f * 2f)
                        + calculatedStats.BasicStats.ManaregenFor8SecOnUse5Min * 5f * (8f * (1 - calculatedStats.BasicStats.HasteRating / 15.7f / 100f)) / (60f * 5f)
                        + (calculatedStats.BasicStats.Mp5OnCastFor20SecOnUse2Min > 0 ? 588f * 5f / 120f : 0)
                        + (calculatedStats.BasicStats.ManaregenOver20SecOnUse3Min * 5f / 180f)
                        + (calculatedStats.BasicStats.ManaregenOver20SecOnUse5Min * 5f / 300f)
                        + (calculatedStats.BasicStats.ManacostReduceWithin15OnHealingCast / (2.0f * 50f)) * 5f
                        + (calculatedStats.BasicStats.FullManaRegenFor15SecOnSpellcast > 0?(((calculatedStats.RegenOutFSR - calculatedStats.RegenInFSR) / 5f) * 15f / 125f) * 5f: 0)
                        + (calculatedStats.BasicStats.BangleProc > 0 ? (((calculatedStats.RegenOutFSR - calculatedStats.RegenInFSR) / 5f) * 0.25f * 15f / 125f) * 5f : 0);
           */


            UseProcs.Spirit = (float)Math.Round(UseProcs.Spirit * (1 + simstats.BonusSpiritMultiplier));
            UseProcs.SpellPower += (float)Math.Round(UseProcs.Spirit * simstats.SpellDamageFromSpiritPercentage);

            simstats += UseProcs;

            // MP5 from Replenishment
            simstats.Mp5 += calculatedStats.BasicStats.Mana * 0.0025f * calculationOptions.Replenishment / 100f * 5;

            // MP5 from Shadowfiend (15 second duration, 1.5 attack speed = 10 attacks @ 4% regen = total 40%)
            // Cooldown is 5 minutes - talents in shadow.
            simstats.Mp5 += (simstats.Mana * 0.4f * calculationOptions.Shadowfiend / 100f)
                / ((5f - character.PriestTalents.VeiledShadows * 1f) * 60f) * 5f;

            // Insightful Earthstorm Diamond.
            float metaSpellCostReduction = simstats.ManaRestorePerCast_5_15 * 0.05f;
            float hcchance = (character.PriestTalents.HolyConcentration * 0.1f + character.PriestTalents.ImprovedHolyConcentration * .05f)
                * (simstats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f);
            float ihcastshasted = 2f * hcchance - (float)Math.Pow(hcchance, 2f);
            float ihchaste = character.PriestTalents.ImprovedHolyConcentration * 0.1f;
            float solchance = (character.PriestTalents.HolySpecialization * 0.01f + simstats.SpellCrit) * character.PriestTalents.SurgeOfLight * 0.25f;
            float sol5chance = 1f - (float)Math.Pow(1f - solchance, 5);
            float serendipityconst = calculationOptions.Serendipity / 100f * character.PriestTalents.Serendipity * 0.25f / 3f;
            float raptureconst = 0.01035f * simstats.Mana / statsRace.Mana * character.PriestTalents.Rapture / 5f * calculationOptions.Rapture / 100f;
            float healmultiplier = (1 + character.PriestTalents.TestOfFaith * 0.02f * calculationOptions.TestOfFaith / 100f) * (1 + character.PriestTalents.Grace * 0.03f) * (1 + simstats.HealingReceivedMultiplier);

            // Test of Faith gives 2-6% extra crit on targets below 50%.
            simstats.SpellCrit += character.PriestTalents.TestOfFaith * 0.02f * calculationOptions.TestOfFaith / 100f;

            // Add on Renewed Hope crit for Disc Maintank Rotation.
            if (Rotation == 7)
                simstats.SpellCrit += character.PriestTalents.RenewedHope * 0.02f;

            //Spell spell;
            GreaterHeal gh = GreaterHeal.GetAllCommonRanks(simstats, character)[0] as GreaterHeal;
            FlashHeal fh = FlashHeal.GetAllCommonRanks(simstats, character)[0] as FlashHeal;
            CircleOfHealing coh = CircleOfHealing.GetAllCommonRanks(simstats, character, 5)[0] as CircleOfHealing;
            Penance penance = Penance.GetAllCommonRanks(simstats, character)[0] as Penance;
            PowerWordShield pws = PowerWordShield.GetAllCommonRanks(simstats, character)[0] as PowerWordShield;
            PrayerOfMending prom_1 = PrayerOfMending.GetAllCommonRanks(simstats, character, 1)[0] as PrayerOfMending;
            PrayerOfMending prom_2 = PrayerOfMending.GetAllCommonRanks(simstats, character, 2)[0] as PrayerOfMending;
            PrayerOfMending prom_3 = PrayerOfMending.GetAllCommonRanks(simstats, character, 3)[0] as PrayerOfMending;
            Renew renew = Renew.GetAllCommonRanks(simstats, character)[0] as Renew;

            // Surge of Light Flash Heal (cannot crit, is free)
            FlashHeal fh_sol = FlashHeal.GetAllCommonRanks(simstats, character)[0] as FlashHeal;
            fh_sol.SurgeOfLight();
            
            // Improved Holy Concentration Haste
            simstats.SpellHaste += ihchaste;
            GreaterHeal gh_hc = GreaterHeal.GetAllCommonRanks(simstats, character)[0] as GreaterHeal; 
            FlashHeal fh_hc = FlashHeal.GetAllCommonRanks(simstats, character)[0] as FlashHeal;
            FlashHeal fh_hc_sol = FlashHeal.GetAllCommonRanks(simstats, character)[0] as FlashHeal;
            fh_hc_sol.SurgeOfLight();
            simstats.SpellHaste -= ihchaste;

            // Borrowed Time Haste
            simstats.SpellHaste += character.PriestTalents.BorrowedTime * 0.05f;
            GreaterHeal gh_bt = GreaterHeal.GetAllCommonRanks(simstats, character)[0] as GreaterHeal;
            FlashHeal fh_bt = FlashHeal.GetAllCommonRanks(simstats, character)[0] as FlashHeal;
            Penance penance_bt = Penance.GetAllCommonRanks(simstats, character)[0] as Penance;
            simstats.SpellHaste -= character.PriestTalents.BorrowedTime * 0.05f;

            List<Spell> sr = new List<Spell>();
            switch (Rotation)
            {
                case 2:     // Greater Heal Spam
                    sr.Add(gh);
                    break;
                case 3:     // Flash Heal Spam
                    sr.Add(fh);
                    break;
                case 4:     // Circle of Healing Spam
                    sr.Add(coh);
                    break;
                case 5:     // Holy MT Healing, renew + prom + ghx5 repeat
                    sr.Add(renew);      // 1.5s 1.5  -13.5 -??.?
                    sr.Add(prom_1);     // 1.5s 3.0  -12.0 -8.5
                    sr.Add(gh);         // 2.5s 5.5  -9.5  -6
                    sr.Add(gh);         // 2.5s 8.0  -7.0  -3.5
                    sr.Add(gh);         // 2.5s 10.5 -4.5  -1.0
                    sr.Add(gh);         // 2.5s 13.0 -2    -??
                    sr.Add(gh);         // 2.5s 15.5 -??   -??   Although, adjusted for haste and improved holy conc, this gets better and better.
                    break;
                case 6:     // Holy Raid Healing, prom, cohx2, fh, cohx2, fh
                    sr.Add(prom_3);     // 1.5s 1.5 -8.5
                    sr.Add(coh);        // 1.5s 3.0 -7.0
                    sr.Add(coh);        // 1.5s 4.5 -5.5
                    sr.Add(fh);         // 1.5s 6.0 -4.0
                    sr.Add(coh);        // 1.5s 7.5 -2.5
                    sr.Add(coh);        // 1.5s 9.0 -1.0
                    sr.Add(fh);         // 1.5s 10.5 - ?
                    // Repeat
                    break;
                case 7:     // Disc MT Healing, pws, penance, prom, gh, penance
                    sr.Add(pws);        // 1.5s 1.5  -15.0 -2.5 -??   -??
                    sr.Add(penance_bt); // 1.5s 3.0  -13.5 -1.0 -??   -6.5
                    sr.Add(prom_1);     // 1.5s 4.5  -12.0 -??  -8.5  -5.0
                    sr.Add(gh);         // 2.5s 7.0  -9.5  -??  -6.0  -2.5
                    sr.Add(gh);         // 2.5s 9.5  -7.0  -??  -3.5  -??
                    sr.Add(penance);    // 2.0s 11.5 -5.0  -??  -1.5  -6.0
                    sr.Add(gh);         // 2.5s 14.0 -2.5  -??  -??   -3.5
                    sr.Add(prom_1);     // 1.5s 17.0 -1.0  -??  -8.5  -2.0
                    sr.Add(gh);         // 2.5s 19.5 -1.5  -??  -6.0  -??
                    sr.Add(penance);    // 2.0s 20.5 -0.5  -??  -5.0  -6.0
                    sr.Add(gh);         // 2.5s 23.0 -??   -??  -3.5  -3.5
                    // repeat
                    break;
                case 8:     // Disc MT Healing, pws, penance, prom, fh - Does not have Divine Fury.
                    sr.Add(pws);        // 1.5s 1.5  -15.0 -2.5 -??   -??
                    sr.Add(penance_bt); // 1.5s 3.0  -13.5 -1.0 -??   -6.5
                    sr.Add(prom_1);     // 1.5s 4.5  -12.0 -??  -8.5  -5.0
                    sr.Add(fh);         // 1.5s 6.0  -10.5 -??  -7.0  -3.5
                    sr.Add(fh);         // 1.5s 7.5  -9.0  -??  -5.5  -2.0
                    sr.Add(fh);         // 1.5s 9.0  -7.5  -??  -4.0  -0.5
                    sr.Add(fh);         // 1.5s 10.5 -6.0  -??  -2.5  -??
                    sr.Add(penance);    // 2.0s 12.5 -4.0  -??  -0.5  -6.0
                    sr.Add(fh);         // 1.5s 14.0 -2.5  -??  -0.5  -4.5
                    sr.Add(fh);         // 1.5s 15.5 -1.0  -??  -0.5  -3.0
                    sr.Add(prom_1);     // 1.5s 12.5 -4.0  -??  -8.5  -1.5
                    sr.Add(fh);         // 1.5s 14.0 -2.5  -??  -7.0  -??
                    sr.Add(penance);    // 2.0s 16.0 -0.5  -??  -5.0  -6.0
                    sr.Add(fh);         // 1.5s 19.0 -??   -??  -3.5  -4.5
                    sr.Add(fh);         // 1.5s 19.0 -??   -??  -3.5  -3.0
                    // repeat
                    break;
                case 9:     // Disc Raid Healing, pws, penance, prom, pw:s, fh, fh
                    sr.Add(pws);        // 1.5  1.5  -2.5  -??   -??
                    sr.Add(penance_bt); // 1.5  3.0  -1.0  -8.0  -??
                    sr.Add(prom_3);     // 1.5  4.5  -??   -6.5  -8.5
                    sr.Add(pws);        // 1.5  6.0  -2.5  -5.0  -7.0
                    sr.Add(fh_bt);      // 1.5  7.5  -1.0  -3.5  -5.5
                    sr.Add(fh);         // 1.5  9.0  -??   -2.0  -4.0
                    // repeat
                    break;
                default:
                    break;
            }

/*            float avgcastlen = 0;
            for (int x = 0; x < sr.Count; x++)
            {
                if (sr[x] == gh || sr[x] == gh_bt)
                    avgcastlen += sr[x].CastTime * (1f - ihcastshasted) + gh_hc.CastTime * ihcastshasted;
                if (sr[x] == fh || sr[x] == fh_bt)
                    avgcastlen += sr[x].CastTime * (1f - ihcastshasted) + fh_hc.CastTime * ihcastshasted;


            }
            */
            float manacost = 0, cyclelen = 0, healamount = 0, solctr = 0, castctr = 0;
            for (int x = 0; x < sr.Count; x++)
            {
                float mcost = 0, absorb = 0, heal = 0, rheal = 0, clen = 0;
                if (sr[x] == gh || sr[x] == gh_bt)
                {   // Greater Heal (A Borrowed Time GHeal cannot also be improved Holy conc hasted, so this works)
                    clen = sr[x].CastTime * (1f - ihcastshasted) + gh_hc.CastTime * ihcastshasted;
                    rheal = sr[x].AvgTotHeal * healmultiplier;
                    absorb = sr[x].AvgCrit * healmultiplier * sr[x].CritChance * character.PriestTalents.DivineAegis * 0.1f;
                    solctr = 1f - (1f - solctr) * (1f - solchance * (1f - hcchance));
                    mcost = sr[x].ManaCost;
                    mcost -= mcost * hcchance;
                    mcost -= mcost * serendipityconst;
                    mcost -= simstats.ManaGainOnGreaterHealOverheal * (1f - calculationOptions.Serendipity / 100f);
                    castctr++;
                }
                else if (sr[x] == fh || sr[x] == fh_bt)
                {   // Flash Heal (Same applies to FH as GHeal with regards to borrowed time)
                    clen = sr[x].CastTime * (1f - hcchance) + fh_hc.CastTime * hcchance;
                    rheal = sr[x].AvgTotHeal * healmultiplier;
                    absorb = sr[x].AvgCrit * healmultiplier * sr[x].CritChance * character.PriestTalents.DivineAegis * 0.1f;
                    solctr = 1f - (1f - solctr) * (1f - solchance * (1f - hcchance));
                    mcost = sr[x].ManaCost;
                    mcost -= mcost * hcchance;
                    mcost -= mcost * solctr;
                    solctr = 0;
                    mcost -= mcost * serendipityconst;
                    castctr++;
                }
                else if (sr[x] == penance || sr[x] == penance_bt)
                {
                    clen = sr[x].CastTime;
                    rheal = sr[x].AvgTotHeal * healmultiplier;
                    absorb = sr[x].AvgCrit * healmultiplier * sr[x].CritChance * character.PriestTalents.DivineAegis * 0.1f;
                    mcost = sr[x].ManaCost;
                    castctr += 3; // Penance counts as 3 casts for some purposes.
                }
                else if (sr[x] == coh)
                {   // Circle of Healing
                    clen = coh.GlobalCooldown;
                    heal = coh.AvgTotHeal * healmultiplier;
                    solctr = 1f - (1f - solctr) * (1f - sol5chance);
                    mcost = coh.ManaCost;
                    castctr += sr[x].Targets;
                }
                else if (sr[x] == renew)
                {   // Renew
                    clen = renew.GlobalCooldown;
                    heal = renew.AvgTotHeal * healmultiplier;
                    mcost = renew.ManaCost;
                    castctr++;
                }
                else if (sr[x] == pws)
                {
                    clen = pws.GlobalCooldown;
                    absorb = pws.AvgTotHeal;
                    mcost = pws.ManaCost;
                    castctr++;
                }
                else if (sr[x] == prom_1 || sr[x] == prom_2 || sr[x] == prom_3)
                {
                    clen = sr[x].GlobalCooldown;
                    heal = sr[x].AvgTotHeal * healmultiplier;
                    absorb = sr[x].AvgCrit * healmultiplier * sr[x].CritChance * character.PriestTalents.DivineAegis * 0.1f;
                    mcost = sr[x].ManaCost;
                    castctr += sr[x].Targets;
                }
                cyclelen += clen;
                healamount += heal + rheal + absorb;
                manacost += mcost;
                manacost -= (rheal + absorb) * raptureconst;
                manacost -= metaSpellCostReduction;
            }

            float avgcastlen = cyclelen / castctr;
            if (calculationOptions.ModelProcs)
            {
                if (simstats.BangleProc > 0)
                    // 15% mana regen over 15 sec. Calculate this as 1 PPM. (45s cd, 15% proc chance.)
                    //simstats.SpellCombatManaRegeneration += 0.15f * 15f / (45f + (100f / 15f * avgcastlen));
                    simstats.SpellCombatManaRegeneration += 0.15f * 15f / 60f * (1f - (float)Math.Pow(1f - 0.15f, 15f / avgcastlen));
                if (simstats.FullManaRegenFor15SecOnSpellcast > 0)
                    // Blue Dragon. 2% chance to proc on cast, no known internal cooldown. calculate as the chance to have procced during its duration. 2% proc/cast.
                    simstats.SpellCombatManaRegeneration += (1f - simstats.SpellCombatManaRegeneration) * (1f - (float)Math.Pow(1f - 0.02f, 15f / avgcastlen));

                if (simstats.Mp5OnCastFor20SecOnUse2Min > 0)
                    simstats.Mp5 += (20f / avgcastlen) * 21f / 2f * 20f / 120f;
                if (simstats.MementoProc > 0)
                    simstats.Mp5 += simstats.MementoProc * 3f * 5f / (45f + 15f * (1f - (float)Math.Pow(1f - 0.1f, 15f / avgcastlen)));
                if (simstats.ManacostReduceWithin15OnUse1Min > 0)
                    manacost -= simstats.ManacostReduceWithin15OnUse1Min * (float)Math.Floor(15f / cyclelen * sr.Count) / 60f;
                if (simstats.ManacostReduceWithin15OnHealingCast > 0)
                    simstats.Mp5 += simstats.ManacostReduceWithin15OnHealingCast * (1f - (float)Math.Pow(1f - 0.02f, castctr)) * 5f / cyclelen;
                if (simstats.ManaregenFor8SecOnUse5Min > 0)
                    simstats.Mp5 += simstats.ManaregenFor8SecOnUse5Min * 8f / 300f * 5f;
            }

            float manapotion = ((calculationOptions.ManaPotion) ? (1800f + 3000f) / 2f : 0) * (1f + simstats.BonusManaPotion);
            // We put in our averaged values (use/procs) here.
            float simregen = (float)Math.Floor(0.001f + 0.0093271 * simstats.Spirit * Math.Sqrt(simstats.Intellect));
            // Calculate regen in FSR/oFSR and add on mana + mana potion as MP5 based on fight length.
            float periodicRegenInFSR = (simregen * simstats.SpellCombatManaRegeneration
                + (simstats.Mana + manapotion) / calculationOptions.FightLength / 60f
                + simstats.Mp5 / 5); // MP1
            float periodicRegenOutFSR = (simregen
                + (simstats.Mana + manapotion) / calculationOptions.FightLength / 60f
                + simstats.Mp5 / 5); // MP1
            float manareg = calculationOptions.FSRRatio / 100f * periodicRegenInFSR + (1f - calculationOptions.FSRRatio / 100f) * periodicRegenOutFSR;

            // Burst is simply heal amount given infinite mana
            calculatedStats.HPSBurstPoints = healamount / cyclelen;
            // Sustained is limited by how much mana you regenerate over the time it would take to cast the spells, divided by the cost.
            if (manareg * cyclelen > manacost) // Regenerating more mana than we can use. Dont make user believe this is an upgrade.
                calculatedStats.HPSSustainPoints = calculatedStats.HPSBurstPoints;
            else
                calculatedStats.HPSSustainPoints = calculatedStats.HPSBurstPoints * manareg * cyclelen / manacost;
            // If opponent has 25% crit, each 39.42308044 resilience gives -1% damage from dots and -1% chance to be crit. Also reduces crits by 2%.
            // This effectively means you gain 12.5% extra health from removing 12.5% dot and 12.5% crits at resilience cap (492.5 (39.42308044*12.5))
            // In addition, the remaining 12.5% crits are reduced by 25% (12.5%*200%damage*75% = 18.75%)
            // At resilience cap I'd say that your hp's are scaled by 1.125*1.1875 = ~30%. Probably wrong but good enough.
            calculatedStats.SurvivabilityPoints = calculatedStats.BasicStats.Health * calculationOptions.Survivability / 100f * (1 + 0.3f * calculatedStats.BasicStats.Resilience / 492.7885f);

            calculatedStats.OverallPoints = calculatedStats.HPSBurstPoints + calculatedStats.HPSSustainPoints + calculatedStats.SurvivabilityPoints;

            return calculatedStats;
        }

        public Stats GetRaceStats(Character character)
        {
            switch (character.Race)
            {
                case Character.CharacterRace.NightElf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 57f,
                        Intellect = 145f,
                        Spirit = 151f
                    };
                case Character.CharacterRace.Dwarf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 61f,
                        Intellect = 144f,
                        Spirit = 150f
                    };
                case Character.CharacterRace.Draenei:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 57f,
                        Intellect = 146f,
                        Spirit = 153f
                    };
                case Character.CharacterRace.Human:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 58f,
                        Intellect = 145f,
                        Spirit = 152f,
                        BonusSpiritMultiplier = 0.03f
                    };
                case Character.CharacterRace.BloodElf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 56f,
                        Intellect = 149f,
                        Spirit = 150f
                    };
                case Character.CharacterRace.Troll:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 59f,
                        Intellect = 141f,
                        Spirit = 152f
                    };
                case Character.CharacterRace.Undead:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 59f,
                        Intellect = 143f,
                        Spirit = 156f,
                    };
            }
            return new Stats();
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTalents = new Stats()
            {
                BonusStaminaMultiplier = character.PriestTalents.Enlightenment * 0.01f,
                BonusSpiritMultiplier = (1 + character.PriestTalents.Enlightenment * 0.01f) * (1f + character.PriestTalents.SpiritOfRedemption * 0.05f) - 1f,
                BonusIntellectMultiplier = character.PriestTalents.MentalStrength * 0.03f,
                SpellDamageFromSpiritPercentage = character.PriestTalents.SpiritualGuidance * 0.05f + character.PriestTalents.TwistedFaith * 0.02f,
                SpellHaste = character.PriestTalents.Enlightenment * 0.01f,
                SpellCombatManaRegeneration = character.PriestTalents.Meditation * 0.1f
            };

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace + statsTalents;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
			statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit));
            statsTotal.Mana += (statsTotal.Intellect - 20f) * 15f + 20f;
            statsTotal.Health += statsTotal.Stamina * 10f;
            statsTotal.SpellCrit += (statsTotal.Intellect / 80f / 100f) + (statsTotal.CritRating / 22.07692337F / 100f) + 0.0124f;
            statsTotal.SpellHaste += (statsTotal.HasteRating / 15.76923275f / 100f);
                 
            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            ComparisonCalculationBase comparison;
            CharacterCalculationsHolyPriest p;
            List<Spell>[] spellList;

            switch (chartName)
            {
                case "Spell AoE HpS":
                    _currentChartName = "Spell AoE HpS";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 5),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 5),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)
                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if(spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpS;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();
                case "Spell AoE HpM":
                    _currentChartName = "Spell AoE HpM";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 5),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 5),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 5),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)
                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if (spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpM;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();
                case "Spell HpS":
                    _currentChartName = "Spell HpS";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 1),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 1),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)

                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if (spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpS;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();
                case "Spell HpM":
                    _currentChartName = "Spell HpM";
                    p = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    spellList = new List<Spell>[] {
                                                Renew.GetAllCommonRanks(p.BasicStats, character),
                                                FlashHeal.GetAllCommonRanks(p.BasicStats, character), 
                                                GreaterHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                BindingHeal.GetAllCommonRanks(p.BasicStats, character),
                                                PrayerOfMending.GetAllCommonRanks(p.BasicStats, character, 1),
                                                CircleOfHealing.GetAllCommonRanks(p.BasicStats, character, 1),
                                                HolyNova.GetAllCommonRanks(p.BasicStats, character, 1),
                                                Penance.GetAllCommonRanks(p.BasicStats, character),
                                                PowerWordShield.GetAllCommonRanks(p.BasicStats, character)
                                            };

                    foreach (List<Spell> spells in spellList)
                    {
                        if (spells[0].AvgHeal == 0)
                            continue;

                        for (int i = 0; i < spells.Count; i++)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = spells[i].Name + " [Rank " + spells[i].Rank + "]";
                            comparison.Equipped = false;
                            comparison.SubPoints[0] = spells[i].HpM;
                            comparison.OverallPoints = comparison.SubPoints[0];
                            comparisonList.Add(comparison);
                        }
                    }

                    return comparisonList.ToArray();

                case "Relative Stat Values":
                    CharacterCalculationsHolyPriest calcsBase = GetCharacterCalculations(character) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsIntellect = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSpirit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsMP5 = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Mp5 = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSpellPower = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsHaste = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsCrit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsSta = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 50 } }) as CharacterCalculationsHolyPriest;
                    CharacterCalculationsHolyPriest calcsRes = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 50 } }) as CharacterCalculationsHolyPriest;

                    return new ComparisonCalculationBase[] {
                        new ComparisonCalculationHolyPriest() { Name = "1 Intellect",
                            OverallPoints = (calcsIntellect.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsIntellect.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsIntellect.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsIntellect.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Spirit",
                            OverallPoints = (calcsSpirit.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSpirit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSpirit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSpirit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 MP5",
                            OverallPoints = (calcsMP5.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsMP5.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsMP5.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsMP5.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Spell Power",
                            OverallPoints = (calcsSpellPower.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSpellPower.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSpellPower.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSpellPower.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Haste",
                            OverallPoints = (calcsHaste.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsHaste.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsHaste.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsHaste.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Crit",
                            OverallPoints = (calcsCrit.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsCrit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsCrit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsCrit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Stamina",
                            OverallPoints = (calcsSta.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsSta.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsSta.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsSta.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationHolyPriest() { Name = "1 Resilience",
                            OverallPoints = (calcsRes.OverallPoints - calcsBase.OverallPoints) / 50,
                            HealPoints = (calcsRes.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            RegenPoints = (calcsRes.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            HastePoints = (calcsRes.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        }};
                default:
                    _currentChartName = null;
                    return new ComparisonCalculationBase[0];
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                HasteRating = stats.HasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                Resilience = stats.Resilience,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                SpellHaste = stats.SpellHaste,
                SpellCrit = stats.SpellCrit,
                HealingReceivedMultiplier = stats.HealingReceivedMultiplier,
                BonusManaPotion = stats.BonusManaPotion,
                MementoProc = stats.MementoProc,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                BonusPoHManaCostReductionMultiplier = stats.BonusPoHManaCostReductionMultiplier,
                BonusGHHealingMultiplier = stats.BonusGHHealingMultiplier,
                ManaregenFor8SecOnUse5Min = stats.ManaregenFor8SecOnUse5Min,
                HealingDoneFor15SecOnUse2Min = stats.HealingDoneFor15SecOnUse2Min,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaregenOver20SecOnUse3Min = stats.ManaregenOver20SecOnUse3Min,
                ManaregenOver20SecOnUse5Min = stats.ManaregenOver20SecOnUse5Min,
                ManacostReduceWithin15OnHealingCast = stats.ManacostReduceWithin15OnHealingCast,
                FullManaRegenFor15SecOnSpellcast = stats.FullManaRegenFor15SecOnSpellcast,
                BangleProc = stats.BangleProc
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Intellect + stats.Spirit + stats.Resilience + stats.Mp5 + stats.SpellPower + stats.CritRating
                + stats.HasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.SpellHaste + stats.SpellCrit + stats.HealingReceivedMultiplier
                + stats.BonusManaPotion + stats.MementoProc + stats.ManaregenFor8SecOnUse5Min
                + stats.BonusPoHManaCostReductionMultiplier + stats.SpellCombatManaRegeneration
                + stats.HealingDoneFor15SecOnUse2Min + stats.SpellPowerFor20SecOnUse2Min
                + stats.SpellPowerFor15SecOnUse90Sec + stats.SpiritFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse2Min + stats.Mp5OnCastFor20SecOnUse2Min
                + stats.ManaregenOver20SecOnUse3Min + stats.ManaregenOver20SecOnUse5Min
                + stats.ManacostReduceWithin15OnHealingCast + stats.FullManaRegenFor15SecOnSpellcast
                + stats.BangleProc) > 0;
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsPriest;
            return calcOpts;
        }
    }
}
