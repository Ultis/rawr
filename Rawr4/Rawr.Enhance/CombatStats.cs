using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Rawr.Enhance
{
    public class CombatStats
    {

        private CalculationOptionsEnhance _calcOpts;
        private Character _character;
        private Stats _stats;
        private ShamanTalents _talents;
        private Priorities _rotation;

        private int levelDifference = 3;
        private float whiteCritDepression = 0.048f;
        private float yellowCritDepression = 0.018f;
        private float fightLength = 0f;
        private float chanceDodgeMH = 0f;
        private float chanceDodgeOH = 0f;
        private float chanceParryMH = 0f;
        private float chanceParryOH = 0f;
        private float expertiseBonusMH = 0f;
        private float expertiseBonusOH = 0f;

        private float critMultiplierMelee = 0f;
        private float critMultiplierSpell = 0f;

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
        private float overMeleeCritCap = 0f;
        
        private float unhastedMHSpeed = 0f;
        private float hastedMHSpeed = 0f;
        private float unhastedOHSpeed = 0f;
        private float hastedOHSpeed = 0f;
        private float averageFSDotTime = 18f;
        private float averageFSTickTime = 3f;

        private float secondsToFiveStack = 0f;
        private float hitsPerSOH = 0f;
        private float hitsPerSMH = 0f;
        private float hitsPerSOHSS = 0f;
        private float hitsPerSMHSS = 0f;
        private float hitsPerSWF = 0f;
        private float hitsPerSLL = 0f;

        private float flurryUptime = 1f;
        private float uWUptime = 0f;
        //private float uFUptime = 0f;
        private float edUptime = 0f;
        private float edBonusCrit = 0f;
        private float ftBonusCrit = 0f;
        private float fireTotemUptime = 0f;  //all fire totems other than searing
        private float searingTotemUptime = 0f;  //searing totem

        private float meleeAttacksPerSec = 0f;
        private float meleeCritsPerSec = 0f;
        private float spellAttacksPerSec = 0f;
        private float spellCritsPerSec = 0f;
        private float spellCastsPerSec = 0f;
        private float spellMissesPerSec = 0f;

        private float stormstrikeBonusCrit = 0f;
        private float enhance4T11 = 0f;
        private float staticShocksPerSecond = 0f;
        private float baseMana = 0f;
        private float maxMana = 0f;
        private float manaRegen = 0f;
        private float exportMeleeCritMH = 0f;
        private float exportMeleeCritOH = 0f;

        public float FightLength { get { return fightLength; } }
        public float ChanceDodgeMH { get { return chanceDodgeMH; } }
        public float ChanceDodgeOH { get { return chanceDodgeOH; } }
        public float ChanceParryMH { get { return chanceParryMH; } }
        public float ChanceParryOH { get { return chanceParryOH; } }
        public float ExpertiseBonusMH { get { return expertiseBonusMH; } }
        public float ExpertiseBonusOH { get { return expertiseBonusOH; } }

        public float NormalHitModifierMH { get { return ChanceWhiteHitMH - chanceWhiteCritMH - GlancingRate; } }
        public float CritHitModifierMH { get { return chanceWhiteCritMH * (2f * (1f + _stats.BonusCritMultiplier)); } }
        public float NormalHitModifierOH { get { return ChanceWhiteHitOH - chanceWhiteCritOH - GlancingRate; } }
        public float CritHitModifierOH { get { return chanceWhiteCritOH * (2f * (1f + _stats.BonusCritMultiplier)); } }
        public float GlancingHitModifier { get { return GlancingRate * 0.7f; } }
        public float YellowHitModifierMH { get { return ChanceYellowHitMH * (1 - chanceYellowCritMH); } }
        public float YellowHitModifierOH { get { return ChanceYellowHitOH * (1 - chanceYellowCritOH); } }
        public float YellowCritModifierMH { get { return ChanceYellowHitMH * chanceYellowCritMH; } }
        public float YellowCritModifierOH { get { return ChanceYellowHitOH * chanceYellowCritOH; } }
        public float SpellHitModifier { get { return ChanceSpellHit * (1 - chanceSpellCrit); } }
        public float SpellCritModifier { get { return ChanceSpellHit * chanceSpellCrit; } }
        public float NatureSpellHitModifier { get { return ChanceSpellHit * (1 - ChanceNatureSpellCrit); } }
        public float NatureSpellCritModifier { get { return ChanceSpellHit * ChanceNatureSpellCrit; } }
        public float LBSpellHitModifier { get { return ChanceSpellHit * (1 - ChanceLBSpellCrit); } }
        public float LBSpellCritModifier { get { return ChanceSpellHit * ChanceLBSpellCrit; } }

        public float CritMultiplierMelee { get { return critMultiplierMelee; } }
        public float CritMultiplierSpell { get { return critMultiplierSpell; } }

        public float ChanceSpellHit { get { return 1 - chanceSpellMiss; } }
        public float ChanceWhiteHitMH { get { return 1 - chanceWhiteMissMH - chanceDodgeMH; } }
        public float ChanceWhiteHitOH { get { return 1 - chanceWhiteMissOH - chanceDodgeOH; } }
        public float ChanceYellowHitMH { get { return 1 - chanceYellowMissMH; } }
        public float ChanceYellowHitOH { get { return 1 - chanceYellowMissOH; } }
        public float ChanceSpellCrit { get { return chanceSpellCrit; } }
        public float ChanceNatureSpellCrit { get { return chanceSpellCrit + stormstrikeBonusCrit; } }
        public float ChanceLBSpellCrit { get { return chanceSpellCrit + stormstrikeBonusCrit + enhance4T11; } }
        public float ChanceWhiteCritMH { get { return chanceWhiteCritMH; } }
        public float ChanceWhiteCritOH { get { return chanceWhiteCritOH; } }
        public float ChanceYellowCritMH { get { return chanceYellowCritMH; } }
        public float ChanceYellowCritOH { get { return chanceYellowCritOH; } }
        public float OverSpellHitCap { get { return overSpellHitCap; } }
        public float OverMeleeCritCap { get { return overMeleeCritCap; } }

        public float AverageDodge { get { return (ChanceDodgeMH + ChanceDodgeOH) / 2; } }
        public float AverageParry { get { return (ChanceParryMH + ChanceParryOH) / 2; } }
        public float AverageExpertise { get { return (ExpertiseBonusMH + ExpertiseBonusOH) / 2; } }
        public float AverageWhiteHitChance { get { return (ChanceWhiteHitMH + ChanceWhiteHitOH) / 2; } }
        public float AverageWhiteCritChance { get { return (ChanceWhiteCritMH + ChanceWhiteCritOH) / 2; } }
        public float AverageYellowHitChance { get { return (ChanceYellowHitMH + ChanceYellowHitOH) / 2; } }
        public float AverageYellowCritChance { get { return (ChanceYellowCritMH + ChanceYellowCritOH) / 2; } }

        public float UnhastedMHSpeed { get { return unhastedMHSpeed; } }
        public float HastedMHSpeed { get { return hastedMHSpeed; } }
        public float UnhastedOHSpeed { get { return unhastedOHSpeed; } }
        public float HastedOHSpeed { get { return hastedOHSpeed; } }

        public float SecondsToFiveStack { get { return secondsToFiveStack; } }
        public float AverageFSDotTime { get { return averageFSDotTime; } }
        public float AverageFSTickTime { get { return averageFSTickTime; } } 
        public float BaseShockSpeed { get { return 6f - .5f * _talents.Reverberation; } }
        public float BaseFireNovaSpeed { get { return 10f - 2f * _talents.ImprovedFireNova; } }
        public float StaticShockProcsPerS { get { return staticShocksPerSecond; } }
        public float StaticShockAvDuration { get { return /*StaticShockProcsPerS == 0 ? 600f : (3f / StaticShockProcsPerS)*/600f; } }  //FIXME Static Chock no longer consumes charges
        public float MultiTargetMultiplier { get { return _calcOpts.MultipleTargets ? _calcOpts.AdditionalTargets * _calcOpts.AdditionalTargetPercent : 1f; } }
            
        public float HitsPerSOH { get { return hitsPerSOH; } }
        public float HitsPerSMH { get { return hitsPerSMH; } }
        public float HitsPerSOHSS { get { return hitsPerSOHSS; } }
        public float HitsPerSMHSS { get { return hitsPerSMHSS; } }
        public float HitsPerSWF { get { return hitsPerSWF; } }
        public float HitsPerSLL { get { return hitsPerSLL; } }
        public float WFPPM { get { return hitsPerSWF == 0 ? 0f : 60f * hitsPerSWF / 2f; } } // we doubled the hits to get extra attacks for PPM we need number of WFs not number of hits
        public float FTPPM { get { return hitsPerSOH == 0 ? 0f : 60f * hitsPerSOH; } }
        public float MeleePPM { get { return (hastedMHSpeed == 0 ? 0f : 60f / hastedMHSpeed) + (hastedOHSpeed == 0 ? 0f : 60f / hastedOHSpeed); } }

        public float PecentageBehindBoss { get { return _calcOpts.InBackPerc / 100f; } }
        public float PecentageInfrontBoss { get { return 1f - _calcOpts.InBackPerc / 100f; } }

        public float EDUptime { get { return edUptime; } }
        public float EDBonusCrit { get { return edBonusCrit; } }
        public float FlurryUptime { get { return flurryUptime; } }
        public float UWUptime { get { return uWUptime; } }
        //public float UFUptime { get { return uFUptime; } }
        public float FireTotemUptime { get { return fireTotemUptime; } }
        public float SearingTotemUptime { get { return searingTotemUptime; } }
        public float FireElementalUptime { get { return getFireElementalUptime(); } }
        public float AbilityCooldown(EnhanceAbility abilityType) { return _rotation.AbilityCooldown(abilityType); }

        public float ExportMeleeCritMH { get { return exportMeleeCritMH; } } // doesn't include ED
        public float ExportMeleeCritOH { get { return exportMeleeCritOH; } } // doesn't include ED
        public float DisplayMeleeCrit { get { return AverageWhiteCritChance + whiteCritDepression; } }
        public float DisplayYellowCrit { get { return AverageYellowCritChance + yellowCritDepression; } }
        public float DisplaySpellCrit { get { return chanceSpellCrit - ftBonusCrit; } }

        public float MaxMana { get { return maxMana; } }
        public float ManaRegen { get { return manaRegen; } }
      
        public float DamageReduction {
            get { return 1f - StatConversion.GetArmorDamageReduction(_character.Level, _calcOpts.TargetArmor,
                            _stats.TargetArmorReduction, _stats.ArmorPenetration); }
        }

        public  float GlancingRate { get { return StatConversion.WHITE_GLANCE_CHANCE_CAP[levelDifference]; } }
        private float GetDPRfromExp(float Expertise) { return StatConversion.GetDodgeParryReducFromExpertise(Expertise, CharacterClass.Shaman); }
        private float DodgeChanceCap { get { return StatConversion.WHITE_DODGE_CHANCE_CAP[levelDifference]; } }
        private float ParryChanceCap { get { return StatConversion.WHITE_PARRY_CHANCE_CAP[levelDifference]; } }
        private float WhiteHitCap { get { return StatConversion.WHITE_MISS_CHANCE_CAP_DW[levelDifference]; } }
        private float YellowHitCap { get { return StatConversion.YELLOW_MISS_CHANCE_CAP[levelDifference]; } }
        private float SpellMissRate { get { return StatConversion.GetSpellMiss(-levelDifference, false); } }

        public CombatStats(Character character, Stats stats, CalculationOptionsEnhance calcOpts)
        {
            _stats = stats;
            _character = character;
            _calcOpts = calcOpts;
            _talents = _character.ShamanTalents;
            fightLength = _calcOpts.FightLength * 60f;
            levelDifference = _calcOpts.TargetLevel - _character.Level;
            if (levelDifference > 3) levelDifference = 3;
            if (levelDifference < 0) levelDifference = 0;
            whiteCritDepression = StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];// 0.03f + 0.006f * levelDifference;
            yellowCritDepression = StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];// 0.006f * levelDifference;
            UpdateCalcs(true);
            SetManaRegen();
            _rotation = new Priorities(this, _calcOpts, _character, _stats, _talents);
            _rotation.CalculateAbilities();
            UpdateCalcs(false); // second pass to revise calcs based on new ability cooldowns
        }

        #region Calculate Regen
        public void SetManaRegen()  //Check
        {
            baseMana = BaseStats.GetBaseStats(_character).Mana;
            //float spiRegen = StatConversion.GetSpiritRegenSec(_stats.Spirit, _stats.Intellect);
            float replenishRegen = _stats.Mana * _stats.ManaRestoreFromMaxManaPerSecond;
            float primalWisdomRegen = ((hitsPerSMH + hitsPerSOH) * 0.40f) * (baseMana * 0.05f);
            manaRegen = _stats.Mp5 / 5 + replenishRegen + primalWisdomRegen;
        }
        #endregion

        public void UpdateCalcs(bool firstPass)
        {
            // talents
            if (_calcOpts.PriorityInUse(EnhanceAbility.StormStrike))
            {
                stormstrikeBonusCrit = .25f * _talents.Stormstrike + (_talents.GlyphofStormstrike ? .1f : 0f);
            }
            else
            {
                stormstrikeBonusCrit = 0f;
            }
            //set bonus
            if (_stats.Enhance2T11 == 1)
            {
                enhance4T11 = .1f;
            }
            else
            {
                enhance4T11 = 0f;
            }

            critMultiplierSpell = 1.5f * (1 + _stats.BonusSpellCritMultiplier);
            critMultiplierMelee = 2f * (1 + _stats.BonusCritMultiplier);
            
            // Melee
            float hitBonus = _stats.PhysicalHit + StatConversion.GetHitFromRating(_stats.HitRating) + 0.06f;  //DualWieldSpecialization
            expertiseBonusMH = GetDPRfromExp(_stats.Expertise + StatConversion.GetExpertiseFromRating(_stats.ExpertiseRating) + BaseStats.GetRacialExpertise(_character, ItemSlot.MainHand));
            expertiseBonusOH = GetDPRfromExp(_stats.Expertise + StatConversion.GetExpertiseFromRating(_stats.ExpertiseRating) + BaseStats.GetRacialExpertise(_character, ItemSlot.OffHand));

            // Need to modify expertiseBonusMH & OH if Orc/Dwarf and have racial bonus weapons
            //Commented out as BaseStats.GetRacialExpertise is now being used
            /*if (_character.Race == CharacterRace.Orc)
            {
                ItemType mhType = _character.MainHand == null ? ItemType.None : _character.MainHand.Type;
                ItemType ohType = _character.OffHand == null ? ItemType.None : _character.OffHand.Type;
                if (mhType == ItemType.OneHandAxe || mhType == ItemType.FistWeapon)
                    expertiseBonusMH += 0.0075f;
                if (ohType == ItemType.OneHandAxe || ohType == ItemType.FistWeapon)
                    expertiseBonusOH += 0.0075f;
            }
            if (_character.Race == CharacterRace.Dwarf)
            {
                ItemType mhType = _character.MainHand == null ? ItemType.None : _character.MainHand.Type;
                ItemType ohType = _character.OffHand == null ? ItemType.None : _character.OffHand.Type;
                if (mhType == ItemType.OneHandMace)
                    expertiseBonusMH += 0.0075f;
                if (ohType == ItemType.OneHandMace)
                    expertiseBonusOH += 0.0075f;
            }*/

            float meleeCritModifier = _stats.PhysicalCrit;
            float baseMeleeCrit = StatConversion.GetCritFromRating(_stats.CritRating) +
                                  StatConversion.GetCritFromAgility(_stats.Agility, _character.Class) + .01f * _talents.Acuity;
            chanceDodgeMH = Math.Max(0f, DodgeChanceCap - expertiseBonusMH);
            chanceDodgeOH = Math.Max(0f, DodgeChanceCap - expertiseBonusOH);
            float ParryChance = ParryChanceCap - expertiseBonusMH;
            chanceParryMH = (float)Math.Max(0f, _calcOpts.InBack ? ParryChance * (1f - _calcOpts.InBackPerc / 100f) : ParryChance);
            ParryChance = ParryChanceCap - expertiseBonusOH;
            chanceParryOH = (float)Math.Max(0f, _calcOpts.InBack ? ParryChance * (1f - _calcOpts.InBackPerc / 100f) : ParryChance);
            chanceWhiteMissMH = Math.Max(0f, WhiteHitCap - hitBonus) + chanceDodgeMH + chanceParryMH;
            chanceWhiteMissOH = Math.Max(0f, WhiteHitCap - hitBonus) + chanceDodgeOH + chanceParryOH;
            chanceYellowMissMH = Math.Max(0f, YellowHitCap - hitBonus) + chanceDodgeMH + chanceParryMH; // base miss 8% now
            chanceYellowMissOH = Math.Max(0f, YellowHitCap - hitBonus) + chanceDodgeOH + chanceParryOH; // base miss 8% now
            
            // SetCritValues((1 + _stats.BonusCritChance) * (baseMeleeCrit + meleeCritModifier) + .00005f); //fudge factor for rounding
            SetCritValues(baseMeleeCrit + meleeCritModifier + .00005f); //fudge factor for rounding
            // set two export values so that ED crit isn't included
            exportMeleeCritMH = chanceWhiteCritMH + whiteCritDepression;
            exportMeleeCritOH = chanceWhiteCritOH + whiteCritDepression;

            // Spells
            ftBonusCrit = 0f;
            if (_calcOpts.MainhandImbue == "Flametongue")
                ftBonusCrit += _talents.GlyphofFlametongueWeapon ? .02f : 0f;
            if (_calcOpts.OffhandImbue == "Flametongue")
                ftBonusCrit += _talents.GlyphofFlametongueWeapon ? .02f : 0f;

            float spellCritModifier = _stats.SpellCrit + _stats.SpellCritOnTarget + ftBonusCrit;
            float hitBonusSpell = _stats.SpellHit + StatConversion.GetSpellHitFromRating(_stats.HitRating);
            chanceSpellMiss = Math.Max(0f, SpellMissRate - hitBonusSpell);
            overSpellHitCap = Math.Max(0f, hitBonusSpell - SpellMissRate);
            float baseSpellCrit = StatConversion.GetSpellCritFromRating(_stats.CritRating) +
                                  StatConversion.GetSpellCritFromIntellect(_stats.Intellect) + .01f * _talents.Acuity;
            //chanceSpellCrit = Math.Min(0.75f, (1 + _stats.BonusCritChance) * (baseSpellCrit + spellCritModifier) + .00005f); //fudge factor for rounding
            chanceSpellCrit = Math.Min(0.75f, baseSpellCrit + spellCritModifier + .00005f); //fudge factor for rounding

            float hasteBonus = StatConversion.GetHasteFromRating(_stats.HasteRating, _character.Class);
            unhastedMHSpeed = _character.MainHand == null ? 3.0f : _character.MainHand.Item.Speed;
            unhastedOHSpeed = _character.OffHand == null ? 3.0f : _character.OffHand.Item.Speed;
            float baseHastedMHSpeed = unhastedMHSpeed / (1f + hasteBonus) / (1f + _stats.PhysicalHaste);
            float baseHastedOHSpeed = unhastedOHSpeed / (1f + hasteBonus) / (1f + _stats.PhysicalHaste);
            float chanceToProcWFPerHit = .2f + (_character.ShamanTalents.GlyphofWindfuryWeapon ? .02f : 0f);
           
            //The Swing Loop
            //This is where we figure out feedback systems -- WF, MW, ED, Flurry, etc.
            //--------------
            flurryUptime = 1f;
            uWUptime = 0f;
            //uFUptime = 0f;
            edUptime = 0f;
            float stormstrikeSpeed = firstPass ? (_talents.Stormstrike == 1 ? 8f : 0f) : AbilityCooldown(EnhanceAbility.StormStrike);
            float shockSpeed = firstPass ? BaseShockSpeed : AbilityCooldown(EnhanceAbility.EarthShock);
            float lavaLashSpeed = firstPass ? 10f : AbilityCooldown(EnhanceAbility.LavaLash);
            float fireNovaSpeed = firstPass ? BaseFireNovaSpeed : AbilityCooldown(EnhanceAbility.FireNova);
            if (_calcOpts.PriorityInUse(EnhanceAbility.MagmaTotem))
                fireTotemUptime = firstPass ? 1.0f : 20f / AbilityCooldown(EnhanceAbility.MagmaTotem);
            else if (_calcOpts.PriorityInUse(EnhanceAbility.SearingTotem))
                searingTotemUptime = firstPass ? 1.0f : 60f / AbilityCooldown(EnhanceAbility.SearingTotem);
            else if (_calcOpts.PriorityInUse(EnhanceAbility.RefreshTotems)) // if no Searing or Magma totem use refresh of Flametongue totem.
                fireTotemUptime = firstPass ? 1.0f : 300f / AbilityCooldown(EnhanceAbility.RefreshTotems); 
            
            float mwPPM = 2 * _talents.MaelstromWeapon;  //Check
            float flurryHasteBonus = .10f * _talents.Flurry;
            float uWHasteBonus = .4f + .1f * _talents.ElementalWeapons;
            float edCritBonus = .03f * _talents.ElementalDevastation;
            float staticShockChance = .15f * _character.ShamanTalents.StaticShock;
            hitsPerSMHSS = 0f;
            hitsPerSOHSS = 0f;
            hitsPerSOH = 0f;
            hitsPerSMH = 0f;
            hitsPerSWF = 0f;
            if (_talents.Stormstrike == 1)
            {
                hitsPerSMHSS = (1f - chanceYellowMissMH) / stormstrikeSpeed;
                hitsPerSOHSS = (1f - 2 * chanceYellowMissOH) / stormstrikeSpeed; //OH only swings if MH connect
            }
            hitsPerSLL = lavaLashSpeed == 0 ? 0f : (1f - chanceYellowMissOH) / lavaLashSpeed;
            float swingsPerSMHMelee = 0f;
            float swingsPerSOHMelee = 0f;
            float wfProcsPerSecond = 0f;
            float mwProcsPerSecond = 0f;
            secondsToFiveStack = 10f;
            float averageMeleeCritChance = (chanceWhiteCritMH + chanceWhiteCritOH + chanceYellowCritMH + chanceYellowCritOH) / 4f;
            float averageMeleeHitChance = ((1f - chanceWhiteMissMH - chanceDodgeMH - chanceParryMH) + (1f - chanceWhiteMissOH - chanceDodgeOH - chanceParryOH)) / 2f;
            float averageMeleeMissChance = (chanceWhiteMissMH + chanceWhiteMissOH) / 2f;
            float whiteHitsPerSMH = 0f;
            float whiteHitsPerSOH = 0f;
            float moteHitsPerS = 0f;
            float yellowHitsPerSMH = 0f;
            float yellowHitsPerSOH = 0f;
            float flameTongueHitsPerSecond = 0f;
            for (int i = 0; i < 5; i++)
            {
                // float bonusHaste = (1f + (flurryUptime * flurryHasteBonus));
                float bonusHaste = 1 / (1 - flurryUptime + flurryUptime / (1 + flurryHasteBonus)) / (1 - uWUptime + uWUptime / (1 + uWHasteBonus)); // use time based not proc based flurryUptime
                hastedMHSpeed = baseHastedMHSpeed / bonusHaste;
                hastedOHSpeed = baseHastedOHSpeed / bonusHaste;
                swingsPerSMHMelee = 1f / hastedMHSpeed;
                swingsPerSOHMelee = (hastedOHSpeed == 0f) ? 0f : 1f / hastedOHSpeed;
                whiteHitsPerSMH = ChanceWhiteHitMH * swingsPerSMHMelee;
                whiteHitsPerSOH = ChanceWhiteHitOH * swingsPerSOHMelee;
                moteHitsPerS = _stats.MoteOfAnger * 2 * AverageWhiteHitChance;
                // Windfury model
                if (_calcOpts.MainhandImbue == "Windfury")
                {
                    float hitsThatProcWFPerS = whiteHitsPerSMH + hitsPerSMHSS;
                    if (unhastedOHSpeed != 0f)
                        hitsThatProcWFPerS += moteHitsPerS / 2; // half the hits will be OH and thus won't proc WF
                    else
                        hitsThatProcWFPerS += moteHitsPerS; // if no offhand then all motes will be MH weapon by definition
                    float maxExpectedWFPerFight = hitsThatProcWFPerS * chanceToProcWFPerHit * fightLength;
                    float ineligibleSeconds = maxExpectedWFPerFight * (3.25f - hastedMHSpeed);
                    float expectedWFPerFight = hitsThatProcWFPerS * chanceToProcWFPerHit * (fightLength - ineligibleSeconds);
                    wfProcsPerSecond = expectedWFPerFight / fightLength;
                    hitsPerSWF = 2f * wfProcsPerSecond * (1f - chanceYellowMissMH);
                }
                yellowHitsPerSMH = hitsPerSWF + hitsPerSMHSS;
                yellowHitsPerSOH = hitsPerSOHSS + hitsPerSLL;
                    
                //Due to attack table, a white swing has the same chance to crit as a yellow hit
// Old Flurry calc changed 10 Nov 2009
//                couldCritSwingsPerSecond = whiteHitsPerSMH + whiteHitsPerSOH + yellowHitsPerSMH + yellowHitsPerSOH;
//                float swingsThatConsumeFlurryPerSecond = swingsPerSMHMelee + swingsPerSOHMelee;
//                flurryUptime = 1f - (float)Math.Pow(1 - averageMeleeCritChance, (3 / swingsThatConsumeFlurryPerSecond) * couldCritSwingsPerSecond);  // old formulae
                flurryUptime = CalculateFlurryUptime(averageMeleeCritChance, averageMeleeHitChance, averageMeleeMissChance);
                //uWUptime = (float)Math.Max(12f / 15f, ??);  //FIXME!!!!
                uWUptime = 7.5f / 15f;  //Temp Uptime until above line is fixed

                // Maelstrom Weapon time to 5 stacks calc
                if (unhastedOHSpeed != 0f)
                {
                    hitsPerSMH = whiteHitsPerSMH + yellowHitsPerSMH + moteHitsPerS / 2;
                    hitsPerSOH = whiteHitsPerSOH + yellowHitsPerSOH + moteHitsPerS / 2;
                    mwProcsPerSecond = (mwPPM / (60f / unhastedMHSpeed)) * hitsPerSMH + (mwPPM / (60f / unhastedOHSpeed)) * hitsPerSOH;
                }
                else
                {
                    hitsPerSMH = whiteHitsPerSMH + yellowHitsPerSMH + moteHitsPerS;
                    hitsPerSOH = 0f;
                    mwProcsPerSecond = (mwPPM / (60f / unhastedMHSpeed)) * hitsPerSMH;
                }
                secondsToFiveStack = 5f / mwProcsPerSecond;

                // Elemental Devastation Uptime calc
                staticShocksPerSecond = (hitsPerSLL + hitsPerSMHSS + hitsPerSOHSS) * staticShockChance;
                flameTongueHitsPerSecond = (_calcOpts.MainhandImbue == "Flametongue" ? HitsPerSMH : 0f) +
                                    ((_calcOpts.OffhandImbue == "Flametongue") ? HitsPerSOH : 0f);
                spellAttacksPerSec = (1f / secondsToFiveStack + 1f / shockSpeed + 1f / fireNovaSpeed + staticShocksPerSecond) // + flameTongueHitsPerSecond)
                                   * (1f - chanceSpellMiss);
                float couldCritSpellsPerS = spellAttacksPerSec;
                edUptime = 1f - (float)Math.Pow(1 - chanceSpellCrit, 10 * couldCritSpellsPerS);
                averageMeleeCritChance = (chanceWhiteCritMH + chanceWhiteCritOH + chanceYellowCritMH + chanceYellowCritOH) / 4f + edUptime * edCritBonus;
            }
            float yellowAttacksPerSecond = hitsPerSWF + hitsPerSMHSS;
            if (unhastedMHSpeed != 0)
                yellowAttacksPerSecond += hitsPerSOHSS;

            // set output variables
            edBonusCrit = edUptime * edCritBonus;
            //SetCritValues((1 + _stats.BonusCritChance) * (baseMeleeCrit + meleeCritModifier) + edBonusCrit + .00005f); //fudge factor for rounding
            SetCritValues(baseMeleeCrit + meleeCritModifier + edBonusCrit + .00005f); //fudge factor for rounding
            meleeAttacksPerSec = hitsPerSMH + hitsPerSOH;
            meleeCritsPerSec = (whiteHitsPerSMH * chanceWhiteCritMH) + (whiteHitsPerSOH * chanceWhiteCritOH) + 
                               (yellowHitsPerSMH * chanceYellowCritMH) + (yellowHitsPerSOH * chanceYellowCritOH) +
                               (_stats.MoteOfAnger * 2 * AverageWhiteCritChance);
            spellCritsPerSec = spellAttacksPerSec * ChanceSpellCrit;
            spellCastsPerSec = spellAttacksPerSec;
            spellMissesPerSec = spellAttacksPerSec * chanceSpellMiss;
            chanceMeleeHit = meleeAttacksPerSec / (swingsPerSMHMelee + swingsPerSOHMelee + 2f * wfProcsPerSecond + .25f + 1f / 6f);
            maxMana = _stats.Mana;
            float spellhaste = _stats.SpellHaste + StatConversion.GetSpellHasteFromRating(_stats.HasteRating);
            averageFSDotTime = _talents.GlyphofFlameShock ? 27f : 18f;
            averageFSTickTime = 3f / (1f + spellhaste);
        }

        private void SetCritValues(float chanceCrit)
        {
            // first set max crit chance after crit depression is 76% - miss chance (ie 100% - 24% glancing - miss chance) 
            // see http://elitistjerks.com/f31/t76785-crit_depression_combat_table/
            // miss chance includes dodge & parry
            chanceWhiteCritMH = Math.Min(chanceCrit - whiteCritDepression, 1f - GlancingRate - chanceWhiteMissMH);
            chanceWhiteCritOH = Math.Min(chanceCrit - whiteCritDepression, 1f - GlancingRate - chanceWhiteMissOH);
            chanceYellowCritMH = Math.Min(chanceCrit - yellowCritDepression, 1f - chanceYellowMissMH);
            chanceYellowCritOH = Math.Min(chanceCrit - yellowCritDepression, 1f - chanceYellowMissOH);
            if (chanceCrit - whiteCritDepression > 1f - GlancingRate - chanceWhiteMissMH)
                overMeleeCritCap = chanceCrit - whiteCritDepression - (1f - GlancingRate - chanceWhiteMissMH);
            else
                overMeleeCritCap = 0f;
            // now apply min 1% crit chance
            chanceWhiteCritMH = Math.Max(0.01f, chanceWhiteCritMH);
            chanceWhiteCritOH = Math.Max(0.01f, chanceWhiteCritOH);
            chanceYellowCritMH = Math.Max(0.01f, chanceYellowCritMH);
            chanceYellowCritOH = Math.Max(0.01f, chanceYellowCritOH);
        }

        private float CalculateFlurryUptime(float c, float h, float m) // c = crit rate, h = hit rate, m = miss rate, assuming hits as noncrits only
        {
            h = h - c; // remove crits from hit figure
            float result = 1 - (float)Math.Pow(1 - c, 3) + 3 * c * m * h * h + 6 * c * m * m * h * h + 3 * c * m * m * h +
                c * m * m * m * (-6 * h * h * m * m - m * m - 3 * h * m * m + 15 * h * h * m + 2 * m + 7 * h * m - 10 * h * h - 1 - 4 * h) / (float)Math.Pow(m - 1, 3);
            return result;
        }

        private float getFireElementalUptime()
        {
            if (!_calcOpts.PriorityInUse(EnhanceAbility.FireElemental))
                return 0f;
            float cooldown = _talents.GlyphofFireElementalTotem ? 300f : 600f;
            float duration = 120f;
            float totalDuration = 0f;
            float uses = 0f;
            while (totalDuration < fightLength)
            {
                if (totalDuration + duration >= fightLength)
                {   // if next totem drop will exceed remaining fight length then only process part available
                    uses = (fightLength - totalDuration) / duration;
                    totalDuration += duration;
                }
                else
                {
                    uses++;
                    totalDuration += cooldown;
                }
            }
            return uses * duration / fightLength;
        }
        
        #region getters
        public float GetMeleeCritsPerSec()
        {
            return meleeCritsPerSec;
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
        private float _ppm = 0f;

        public DPSAnalysis(float dps, float miss, float dodge, float glancing, float crit, float PPM)
        {
            _dps = dps;
            _miss = miss;
            _dodge = dodge;
            _glancing = glancing;
            _crit = crit;
            _ppm = PPM;
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
        public float PPM { get { return _ppm; } }

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

}
