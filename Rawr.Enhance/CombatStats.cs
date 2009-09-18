using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Rawr.Enhance
{
    class CombatStats
    {

        private CalculationOptionsEnhance _calcOpts = new CalculationOptionsEnhance();
        private Character _character = new Character();
        private Stats _stats = new Stats();
        private ShamanTalents _talents = new ShamanTalents();

        private float glancingRate = 0.24f;
        private float whiteCritDepression = 0.048f;
        private float yellowCritDepression = 0.018f;
        private float bloodlustHaste = 0f;
        private float chanceCrit = 0f;
        private float chanceDodgeMH = 0f;
        private float chanceDodgeOH = 0f;
        private float expertiseBonusMH = 0f;
        private float expertiseBonusOH = 0f;

        float critMultiplierMelee = 0f;
        float critMultiplierSpell = 0f;

        private float chanceSpellMiss = 0f;
        private float chanceWhiteMissMH = 0f;
        private float chanceYellowMissMH = 0f;
        private float chanceYellowCritMH = 0f;
        private float chanceWhiteMissOH = 0f;
        private float chanceYellowMissOH = 0f;
        private float chanceYellowCritOH = 0f;
        private float chanceWhiteCritMH = 0f;
        private float chanceWhiteCritOH = 0f;
        private float chanceSpellCrit = 0f;
        private float chanceMeleeHit = 0f;
        private float overSpellHitCap = 0f;
        
        private float unhastedMHSpeed = 0f;
        private float hastedMHSpeed = 0f;
        private float unhastedOHSpeed = 0f;
        private float hastedOHSpeed = 0f;

        private float secondsToFiveStack = 0f;
        private float hitsPerSOH = 0f;
        private float hitsPerSMH = 0f;
        private float hitsPerSOHSS = 0f;
        private float hitsPerSMHSS = 0f;
        private float hitsPerSWF = 0f;
        private float hitsPerSLL = 0f;

        private float flurryUptime = 1f;
        private float edUptime = 0f;
        private float edBonusCrit = 0f;
        private float ftBonusCrit = 0f;
        private float urUptime = 0f;

        private float meleeAttacksPerSec = 0f;
        private float meleeCritsPerSec = 0f;
        private float spellAttacksPerSec = 0f;
        private float spellCritsPerSec = 0f;
        private float spellCastsPerSec = 0f;
        private float spellMissesPerSec = 0f;

        private float callOfThunder = 0f;
        private float staticShocksPerSecond = 0f;

        private List<Ability> abilities = new List<Ability>();

        public CombatStats(Character character, Stats stats)
        {
            _stats = stats;
            _character = character;
            _calcOpts = _character.CalculationOptions as CalculationOptionsEnhance;
            _talents = _character.ShamanTalents;
            UpdateCalcs();
            SetupAbilities();
            CalculateAbilities();
            UpdateCalcs(); // second pass to revise calcs based on new ability cooldowns
        }

        public float GlancingRate { get { return glancingRate; } }
        public float FightLength { get { return _calcOpts.FightLength * 60f; } }
        public float BloodlustHaste { get { return bloodlustHaste; } }
        public float ChanceDodgeMH { get { return chanceDodgeMH; } }
        public float ChanceDodgeOH { get { return chanceDodgeOH; } }
        public float ExpertiseBonusMH { get { return expertiseBonusMH; } }
        public float ExpertiseBonusOH { get { return expertiseBonusOH; } }

        public float NormalHitModifier { get { return 1f - AverageWhiteCrit - glancingRate; } }
        public float CritHitModifier { get { return AverageWhiteCrit * (2f * (1f + _stats.BonusCritMultiplier)); } }
        public float GlancingHitModifier { get { return glancingRate * 0.7f; } }
        public float YellowHitModifierMH { get { return ChanceYellowHitMH * (1 - chanceYellowCritMH); } }
        public float YellowHitModifierOH { get { return ChanceYellowHitOH * (1 - chanceYellowCritOH); } }
        public float YellowCritModifierMH { get { return ChanceYellowHitMH * chanceYellowCritMH; } }
        public float YellowCritModifierOH { get { return ChanceYellowHitOH * chanceYellowCritOH; } }
        public float SpellHitModifier { get { return ChanceSpellHit * (1 - chanceSpellCrit); } }
        public float SpellCritModifier { get { return ChanceSpellHit * chanceSpellCrit; } }
        public float LBHitModifier { get { return ChanceSpellHit * (1 - chanceSpellCrit - callOfThunder); } }
        public float LBCritModifier { get { return ChanceSpellHit * (chanceSpellCrit + callOfThunder); } }

        public float CritMultiplierMelee { get { return critMultiplierMelee; } }
        public float CritMultiplierSpell { get { return critMultiplierSpell; } }

        public float ChanceSpellHit { get { return 1 - chanceSpellMiss; } }
        public float ChanceWhiteHitMH { get { return 1 - chanceWhiteMissMH; } }
        public float ChanceWhiteHitOH { get { return 1 - chanceWhiteMissOH; } }
        public float ChanceYellowHitMH { get { return 1 - chanceYellowMissMH; } }
        public float ChanceYellowHitOH { get { return 1 - chanceYellowMissOH; } }
        public float ChanceSpellCrit { get { return chanceSpellCrit; } }
        public float ChanceWhiteCritMH { get { return chanceWhiteCritMH; } }
        public float ChanceWhiteCritOH { get { return chanceWhiteCritOH; } }
        public float ChanceYellowCritMH { get { return chanceYellowCritMH; } }
        public float ChanceYellowCritOH { get { return chanceYellowCritOH; } }
        public float ChanceMeleeHit { get { return chanceMeleeHit; } }
        public float ChanceMeleeCrit { get { return chanceCrit; } }
        public float OverSpellHitCap { get { return overSpellHitCap; } }

        public float AverageDodge { get { return (ChanceDodgeMH + ChanceDodgeOH) / 2; } }
        public float AverageExpertise { get { return (ExpertiseBonusMH + ExpertiseBonusOH) / 2; } }
        public float AverageWhiteHit { get { return (ChanceWhiteHitMH + ChanceWhiteHitOH) / 2; } }
        public float AverageWhiteCrit { get { return (ChanceWhiteCritMH + ChanceWhiteCritOH) / 2; } }
        public float AverageYellowHit { get { return (ChanceYellowHitMH + ChanceYellowHitOH) / 2; } }
        public float AverageYellowCrit { get { return (ChanceYellowCritMH + ChanceYellowCritOH) / 2; } }

        public float UnhastedMHSpeed { get { return unhastedMHSpeed; } }
        public float HastedMHSpeed { get { return hastedMHSpeed; } }
        public float UnhastedOHSpeed { get { return unhastedOHSpeed; } }
        public float HastedOHSpeed { get { return hastedOHSpeed; } }

        public float SecondsToFiveStack { get { return secondsToFiveStack; } }
        public float BaseShockSpeed { get { return 6f - .2f * _character.ShamanTalents.Reverberation; } }
        public float StaticShockProcsPerS { get { return staticShocksPerSecond; } }
        public float StaticShockAvDuration { get { return StaticShockProcsPerS == 0 ? 600f : ((3f + 2f * _character.ShamanTalents.StaticShock) / StaticShockProcsPerS); } }
            
        public float HitsPerSOH { get { return hitsPerSOH; } }
        public float HitsPerSMH { get { return hitsPerSMH; } }
        public float HitsPerSOHSS { get { return hitsPerSOHSS; } }
        public float HitsPerSMHSS { get { return hitsPerSMHSS; } }
        public float HitsPerSWF { get { return hitsPerSWF; } }
        public float HitsPerSLL { get { return hitsPerSLL; } }

        public float URUptime { get { return urUptime; } }
        public float EDUptime { get { return edUptime; } }
        public float EDBonusCrit { get { return edBonusCrit; } }
        public float FlurryUptime { get { return flurryUptime; } }

        public float DisplayMeleeCrit { get { return chanceCrit; } }
        public float DisplayYellowCrit { get { return AverageYellowCrit + yellowCritDepression; } }
        public float DisplaySpellCrit { get { return chanceSpellCrit - ftBonusCrit; } }
      
        public float DamageReduction {
            get { return 1f - StatConversion.GetArmorDamageReduction(_character.Level, _calcOpts.TargetArmor, _stats.ArmorPenetration, 0f, _stats.ArmorPenetrationRating); }
        }

        private static readonly float SPELL_MISS  = 0.17f;

        private void SetupAbilities()
        {
            int priority = 0;
            float gcd = Math.Max(1.0f, 1.5f * (1f - StatConversion.GetSpellHasteFromRating(_stats.HasteRating)));
            if (_talents.FeralSpirit == 1)
                abilities.Add(new Ability("Feral Spirits", 180f, 1.5f, ++priority));
            if (_talents.MaelstromWeapon > 0)
                abilities.Add(new Ability("Lightning Bolt", SecondsToFiveStack, gcd, ++priority));
            if (_talents.Stormstrike == 1)
                abilities.Add(new Ability("Stormstrike", 8f, 1.5f, ++priority));
            if (_character.ShamanTalents.GlyphofShocking)
                abilities.Add(new Ability("Earth Shock", BaseShockSpeed, 1.0f, ++priority));
            else
                abilities.Add(new Ability("Earth Shock", BaseShockSpeed, gcd, ++priority));
            if (_talents.LavaLash == 1)
                abilities.Add(new Ability("Lava Lash", 6f, 1.5f, ++priority));
            if (_talents.StaticShock > 0)
                abilities.Add(new Ability("Lightning Shield", StaticShockAvDuration, gcd, ++priority));
            if (_calcOpts.Magma)
                abilities.Add(new Ability("Magma Totem", 20f, 1.0f, ++priority));
            else
                abilities.Add(new Ability("Searing Totem", 60f, 1.0f, ++priority));
            abilities.Add(new Ability("Refresh Totems", 300f, 1.0f, ++priority)); // patch 3.2 takes just 1 second GCD to refresh totems.
            abilities.Sort();
        }

        private void CalculateAbilities()
        {
            float gcd = 1.5f;
            string name = "";
            for (float timeElapsed = 0f; timeElapsed < FightLength; timeElapsed += gcd)
            {
                gcd = 0.1f; // set GCD to small value step for dead time as dead time doesn't use a GCD its just waiting time
                name = "deadtime";
                foreach (Ability ability in abilities)
                {
                    if (ability.OffCooldown(timeElapsed))
                    {
                        ability.AddUse(timeElapsed, _calcOpts.AverageLag / 1000f);
                        gcd = ability.GCD;
                        name = ability.Name;
                        break;
                    }
                }
 /*
                   System.Diagnostics.Debug.Print("Time: {0} - FS {1}, {2} - LB {3}, {4} - SS {5}, {6} - ES {7}, {8} - LL {9}, {10} - LS {11}, {12} - MT {13}, {14} - used {15}",
                   timeElapsed,
                   abilities[0].Uses, abilities[0].CooldownOver,
                   abilities[1].Uses, abilities[1].CooldownOver,
                   abilities[2].Uses, abilities[2].CooldownOver,
                   abilities[3].Uses, abilities[3].CooldownOver,
                   abilities[4].Uses, abilities[4].CooldownOver,
                   abilities[5].Uses, abilities[5].CooldownOver,
                   abilities[6].Uses, abilities[6].CooldownOver, name);
 */
            }
            // at this stage abilities now contains the number of procs per fight for each ability.
        }

        public float AbilityCooldown(string name)
        {
            foreach (Ability ability in abilities)
            {
                if (ability.Name.Equals(name))
                    return ability.Uses == 0 ? ability.Duration : FightLength / ability.Uses;
            }
            return FightLength;
        }

        public void UpdateCalcs()
        {
            // talents
            callOfThunder = .05f * _character.ShamanTalents.CallOfThunder;
            critMultiplierMelee = 2f * (1 + _stats.BonusCritMultiplier);
            critMultiplierSpell = (1.5f + .1f * _character.ShamanTalents.ElementalFury) * (1 + _stats.BonusSpellCritMultiplier);
            
            // Melee
            float hitBonus = _stats.PhysicalHit + StatConversion.GetHitFromRating(_stats.HitRating) + 0.02f * _talents.DualWieldSpecialization;
            expertiseBonusMH = StatConversion.GetDodgeParryReducFromExpertise(_stats.Expertise + StatConversion.GetExpertiseFromRating(_stats.ExpertiseRating));
            expertiseBonusOH = StatConversion.GetDodgeParryReducFromExpertise(_stats.Expertise + StatConversion.GetExpertiseFromRating(_stats.ExpertiseRating));

            // Need to modify expertiseBonusMH & OH if Orc and have racial bonus weapons
            if (_character.Race == CharacterRace.Orc)
            {
                ItemType mhType = _character.MainHand == null ? ItemType.None : _character.MainHand.Type;
                ItemType ohType = _character.OffHand == null ? ItemType.None : _character.OffHand.Type;
                if (mhType == ItemType.OneHandAxe || mhType == ItemType.FistWeapon) // patch 3.2 includes fists
                    expertiseBonusMH += 0.0125f;
                if (ohType == ItemType.OneHandAxe || ohType == ItemType.FistWeapon) // patch 3.2 includes fists
                    expertiseBonusOH += 0.0125f;
            }

            float meleeCritModifier = _stats.PhysicalCrit;
            float baseMeleeCrit = StatConversion.GetCritFromRating(_stats.CritMeleeRating + _stats.CritRating) + 
                                  StatConversion.GetCritFromAgility(_stats.Agility, _character.Class) + .01f * _talents.ThunderingStrikes;
            chanceCrit         = Math.Min(1 - glancingRate, (1 + _stats.BonusCritChance) * (baseMeleeCrit + meleeCritModifier) + .00005f); //fudge factor for rounding
            chanceDodgeMH      = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[_calcOpts.TargetLevel - _character.Level] - expertiseBonusMH);
            chanceDodgeOH      = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[_calcOpts.TargetLevel - _character.Level] - expertiseBonusOH);
            chanceWhiteMissMH  = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP_DW[_calcOpts.TargetLevel - _character.Level] - hitBonus) + chanceDodgeMH;
            chanceWhiteMissOH  = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP_DW[_calcOpts.TargetLevel - _character.Level] - hitBonus) + chanceDodgeOH;
            chanceYellowMissMH = Math.Max(0f, StatConversion.YELLOW_MISS_CHANCE_CAP[_calcOpts.TargetLevel - _character.Level] - hitBonus) + chanceDodgeMH; // base miss 8% now
            chanceYellowMissOH = Math.Max(0f, StatConversion.YELLOW_MISS_CHANCE_CAP[_calcOpts.TargetLevel - _character.Level] - hitBonus) + chanceDodgeOH; // base miss 8% now
            chanceWhiteCritMH  = Math.Min(chanceCrit - whiteCritDepression , 1f - glancingRate - chanceWhiteMissMH);
            chanceWhiteCritOH  = Math.Min(chanceCrit - whiteCritDepression , 1f - glancingRate - chanceWhiteMissOH);
            chanceYellowCritMH = Math.Min(chanceCrit - yellowCritDepression, 1f - chanceYellowMissMH);
            chanceYellowCritOH = Math.Min(chanceCrit - yellowCritDepression, 1f - chanceYellowMissOH);

            // Spells
            ftBonusCrit = 0f;
            if (_calcOpts.MainhandImbue == "Flametongue")
                ftBonusCrit += _talents.GlyphofFlametongueWeapon ? .02f : 0f;
            if (_calcOpts.OffhandImbue == "Flametongue" && _talents.DualWield == 1)
                ftBonusCrit += _talents.GlyphofFlametongueWeapon ? .02f : 0f;
        
            float spellCritModifier = _stats.SpellCrit + ftBonusCrit;
            float hitBonusSpell = _stats.SpellHit + StatConversion.GetSpellHitFromRating(_stats.HitRating);
            chanceSpellMiss = Math.Max(0f, SPELL_MISS - hitBonusSpell);
            overSpellHitCap = Math.Max(0f, hitBonusSpell - SPELL_MISS);
            float baseSpellCrit = StatConversion.GetSpellCritFromRating(_stats.CritRating) + 
                                  StatConversion.GetSpellCritFromIntellect(_stats.Intellect) + .01f * _talents.ThunderingStrikes;
            chanceSpellCrit = Math.Min(0.75f, (1 + _stats.BonusCritChance) * (baseSpellCrit + spellCritModifier) + .00005f); //fudge factor for rounding

            float hasteBonus = StatConversion.GetHasteFromRating(_stats.HasteRating, _character.Class);
            unhastedMHSpeed = _character.MainHand == null ? 3.0f : _character.MainHand.Item.Speed;
            unhastedOHSpeed = _character.OffHand == null ? 3.0f : _character.OffHand.Item.Speed;
           
            float baseHastedMHSpeed = unhastedMHSpeed / (1f + hasteBonus) / (1f + _stats.PhysicalHaste);
            float baseHastedOHSpeed = unhastedOHSpeed / (1f + hasteBonus) / (1f + _stats.PhysicalHaste);

            // Only MH WF for now
            float chanceToProcWFPerHit = .2f + (_character.ShamanTalents.GlyphofWindfuryWeapon ? .02f : 0f);

            //The Swing Loop
            //This is where we figure out feedback systems -- WF, MW, ED, Flurry, etc.
            //It's also where we'll figure out GCD interference when we model that.
            //--------------
            flurryUptime = 1f;
            edUptime = 0f;
            urUptime = 0f;
            float stormstrikeSpeed = AbilityCooldown("Stormstrike");
            float shockSpeed = AbilityCooldown("Earth Shock");
            float lavaLashSpeed = AbilityCooldown("Lava Lash");
            if (stormstrikeSpeed == FightLength && _talents.Stormstrike == 1) // first time round loop so abilities not initialised
            {
                stormstrikeSpeed = 8f;
                shockSpeed = BaseShockSpeed;
                lavaLashSpeed = 6f;
            }
            float mwPPM = 2 * _talents.MaelstromWeapon * (1 + _stats.BonusMWFreq);
            float flurryHasteBonus = .05f * _talents.Flurry + _stats.BonusFlurryHaste;
            float edCritBonus = .03f * _talents.ElementalDevastation;
            float bloodlustUptime = 40f / FightLength;
            bloodlustHaste = 1 + (bloodlustUptime * _stats.Bloodlust);
            hastedMHSpeed = baseHastedMHSpeed / bloodlustHaste;
            hastedOHSpeed = baseHastedOHSpeed / bloodlustHaste;
            hitsPerSMHSS = (1f - chanceYellowMissMH) / stormstrikeSpeed;
            hitsPerSOHSS = _character.ShamanTalents.DualWield == 1 ? ((1f - 2 * chanceYellowMissOH) / stormstrikeSpeed) : 0f; //OH only swings if MH connects
            hitsPerSLL = (1f - chanceYellowMissOH) / lavaLashSpeed;
            float swingsPerSMHMelee = 0f;
            float swingsPerSOHMelee = 0f;
            float wfProcsPerSecond = 0f;
            float mwProcsPerSecond = 0f;
            secondsToFiveStack = 10f;
            float averageMeleeCritChance = (chanceYellowCritMH + chanceYellowCritOH)/2;
            float couldCritSwingsPerSecond = 0f;
            hitsPerSOH = 0f;
            hitsPerSMH = 0f;
            hitsPerSWF = 0f;
            for (int i = 0; i < 5; i++)
            {
                float bonusHaste = (1f + (flurryUptime * flurryHasteBonus)) * bloodlustHaste;
                hastedMHSpeed = baseHastedMHSpeed / bonusHaste;
                hastedOHSpeed = baseHastedOHSpeed / bonusHaste;
                swingsPerSMHMelee = 1f / hastedMHSpeed;
                swingsPerSOHMelee = hastedOHSpeed == 0f ? 0f : 1f / hastedOHSpeed;
                
                float hitsThatProcWFPerS = (1f - chanceWhiteMissMH) * swingsPerSMHMelee + hitsPerSMHSS;

                // new WF model - slighly curved Windfury Society
                float maxExpectedWFPerFight = hitsThatProcWFPerS * chanceToProcWFPerHit * FightLength;
                float ineligibleSeconds = maxExpectedWFPerFight * (3f - hastedMHSpeed);
                float expectedWFPerFight = hitsThatProcWFPerS * chanceToProcWFPerHit * (FightLength - ineligibleSeconds);
                wfProcsPerSecond = expectedWFPerFight / FightLength;
                hitsPerSWF = 2f * wfProcsPerSecond * (1f - chanceYellowMissMH);
                
                //Due to attack table, a white swing has the same chance to crit as a yellow hit
                couldCritSwingsPerSecond = swingsPerSMHMelee + swingsPerSOHMelee + hitsPerSMHSS + hitsPerSOHSS + hitsPerSLL + hitsPerSWF;
                float swingsThatConsumeFlurryPerSecond = swingsPerSMHMelee + swingsPerSOHMelee;
                flurryUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, (3 / swingsThatConsumeFlurryPerSecond) * couldCritSwingsPerSecond);

                hitsPerSMH = swingsPerSMHMelee * (1f - chanceWhiteMissMH - chanceDodgeMH) + hitsPerSWF + hitsPerSMHSS;
                mwProcsPerSecond = (mwPPM / (60f / unhastedMHSpeed)) * hitsPerSMH;
                if (_character.ShamanTalents.DualWield == 1 && unhastedOHSpeed != 0f)
                {
                    hitsPerSOH = swingsPerSOHMelee * (1f - chanceWhiteMissOH - chanceDodgeOH) + hitsPerSOHSS + hitsPerSLL;
                    mwProcsPerSecond += (mwPPM / (60f / unhastedOHSpeed)) * hitsPerSOH;
                }
                secondsToFiveStack = 5 / mwProcsPerSecond;

                spellAttacksPerSec = 1 / secondsToFiveStack + 1 / shockSpeed;
                float couldCritSpellsPerS = spellAttacksPerSec * (1f - chanceSpellMiss);
                edUptime = 1f - (float)Math.Pow(1 - chanceSpellCrit, 10 * couldCritSpellsPerS);
                float averageYellowCrit = (chanceYellowCritMH + chanceYellowCritOH) / 2;
                averageMeleeCritChance = averageYellowCrit + edUptime * edCritBonus;
            }
            urUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, 10 * couldCritSwingsPerSecond);
            float yellowAttacksPerSecond = hitsPerSWF + hitsPerSMHSS;
            if (_character.ShamanTalents.DualWield == 1 && unhastedMHSpeed != 0)
                yellowAttacksPerSecond += hitsPerSOHSS;

            // set output variables
            edBonusCrit = edUptime * edCritBonus;
            chanceWhiteCritMH += edBonusCrit;
            chanceWhiteCritOH += edBonusCrit;
            chanceYellowCritMH += edBonusCrit; 
            chanceYellowCritOH += edBonusCrit;
            meleeAttacksPerSec = hitsPerSMH + hitsPerSOH;
            meleeCritsPerSec = (hitsPerSMH * chanceWhiteCritMH) + (hitsPerSOH * chanceWhiteCritOH);
            spellAttacksPerSec = 1f / secondsToFiveStack + 1f / shockSpeed;
            if (_calcOpts.MainhandImbue == "Flametongue")  // flametongue weapon imbue is spell damage so we have extra spell attacks per second with it imbued.
                spellAttacksPerSec += hitsPerSMH;
            if (_calcOpts.OffhandImbue == "Flametongue" && _talents.DualWield == 1)
                spellAttacksPerSec += hitsPerSOH;
            spellCritsPerSec = spellAttacksPerSec * ChanceSpellCrit;
            spellCastsPerSec = spellAttacksPerSec;
            spellMissesPerSec = spellAttacksPerSec * chanceSpellMiss;
            chanceMeleeHit = meleeAttacksPerSec / (swingsPerSMHMelee + swingsPerSOHMelee + 2f * wfProcsPerSecond + .25f + 1f/6f);
            float staticShockChance = (.02f * _character.ShamanTalents.StaticShock + (_stats.Enhance2T9 == 1f ? 0.03f : 0f));
            staticShocksPerSecond = (HitsPerSMH + HitsPerSOH) * staticShockChance;
        }

        #region getters
        public float GetMeleeCritsPerSec()
        {
            return  meleeCritsPerSec;
        }

        public float GetMeleeAttacksPerSec()
        {
            return meleeAttacksPerSec;
        }

        public float GetSpellCritsPerSec()
        {
            return spellCritsPerSec;
        }

        public float GetSpellAttacksPerSec()
        {
            return spellAttacksPerSec;
        }

        public float GetSpellCastsPerSec()
        {
            return spellCastsPerSec;
        }
 
        public float GetSpellMissesPerSec()
        {
            return spellMissesPerSec;
        }
        #endregion
    }

    #region DPSAnalysis
    public class DPSAnalysis
    {
        private float _dps = 0f;
        private float _miss = -1f;
        private float _dodge = -1f;
        private float _glancing = -1f;
        private float _hit = -1f;
        private float _crit = -1f;

        public DPSAnalysis(float dps, float miss, float dodge, float glancing, float crit)
        {
            _dps = dps;
            _miss = miss;
            _dodge = dodge;
            _glancing = glancing;
            _crit = crit;
            _hit = 1f;
            if (miss > 0)
                _hit -= miss;  // only need to subtract miss from hit as at this point miss includes dodge
            if (dodge > 0)
                _miss -= dodge;  // so if we have dodge then the true miss is less dodge
            if (glancing > 0)
                _hit -= glancing;
            if (crit > 0)
                _hit -= crit;
        }

        public float dps { get { return _dps; } }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_dps.ToString("F2", CultureInfo.InvariantCulture) + "*");
            if (_miss >= 0)
                sb.AppendLine("Miss            " + (100f * _miss).ToString("F2", CultureInfo.InvariantCulture) + "%");
            if (_dodge >= 0)
                sb.AppendLine("Dodge        " + (100f * _dodge).ToString("F2", CultureInfo.InvariantCulture) + "%");
            if (_glancing >= 0)
                sb.AppendLine("Glancing     " + (100f * _glancing).ToString("F2", CultureInfo.InvariantCulture) + "%");
            if (_hit >= 0)
                sb.AppendLine("Normal Hit  " + (100f * _hit).ToString("F2", CultureInfo.InvariantCulture) + "%");
            if (_crit >= 0)
                sb.AppendLine("Crit Hit       " + (100f * _crit).ToString("F2", CultureInfo.InvariantCulture) + "%");
            return sb.ToString();
        }
    }
    #endregion

    #region Ability class
    public class Ability : IComparable<Ability>
    {
        private string _name;
        private float _duration;
        private int _priority;
        private float _cooldownOver;
        private int _uses;
        private float _gcd = 1.5f;

        public Ability(string name, float duration, float gcd, int priority)
        {
            _name = name;
            _duration = duration;
            _priority = priority;
            _gcd = gcd;
            _cooldownOver = 0f;
            _uses = 0;
        }

        public string Name { get { return _name; } }
        public float Duration { get { return _duration; } }
        public float GCD { get { return _gcd; } }
        public float CooldownOver { get { return _cooldownOver; } }
        public int Uses { get { return _uses; } }

        public void AddUse(float useTime, float lag)
        {
            _uses++;
            _cooldownOver = useTime + _duration + lag;
        }

        public bool OffCooldown(float starttime)
        {
            return starttime >= _cooldownOver;
        }

        public int CompareTo(Ability other)
        {
            return _priority.CompareTo(other._priority);
        }

    }
    #endregion
}
