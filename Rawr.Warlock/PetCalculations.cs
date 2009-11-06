using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock 
{
    class PetCalculations 
    {
        public PetCalculations(Stats charStats, Character Char) 
        {
            character = Char;
            talents = Char.WarlockTalents;
            CalculationOptions = character.CalculationOptions as CalculationOptionsWarlock;
            Pet = CalculationOptions.Pet;

            calculatePetStats(charStats);
        }
        #region Variables
        Solver solver;
        public Stats petStats;
        Character character;
        CalculationOptionsWarlock CalculationOptions;
        WarlockTalents talents;
        String Pet;
        Stats charStats;

        float baseAttackDmg;
        float baseAttackSpeed;
        float specialAttackDmg;
        float specialAttackSpeed;
        int specialCost;
        float critCoefSpecial;
        float critCoefMelee;
        float petHit;
        float baseMana; 
        public float critCount;
        #endregion

        //TODO: refactor 
        private void calculatePetStats(Stats charStats) 
        {
            this.charStats = charStats;

            bool isImpVoidSucHuntOrGuard = (Pet == "Imp" || Pet == "Voidwalker" || Pet == "Succubus" || Pet == "Felhunter" || Pet == "Felguard");

            petStats = new Stats();

            #region Buffs
            //get buffs before calculations for double dipping of benifts
            Stats buffs = (new CalculationsWarlock()).GetBuffStats(character);
            //remove player only buffs
            if (character.ActiveBuffsContains("Focus Magic")) buffs -= Buff.GetBuffByName("Focus Magic").Stats;
            //remove elixir and flask/food buff
            foreach (Buff buff in character.ActiveBuffs)
            {
                if (buff.Group == "Elixirs and Flasks")
                {
                    buffs -= buff.Stats;
                    //TODO: Remove improvement from applying to pet as well
                    //foreach (Buff ImpBuff in buff.Improvements) buffs -= ImpBuff.Stats; ;
                }
                if (buff.Group == "Food") buffs -= buff.Stats;
            }




            petStats += buffs;
            #endregion

            petStats.Strength += (Pet == "Imp" ? 297 : (Pet == "Infernal" ? 331 : 314)) * (1f + charStats.BonusStrengthMultiplier);
            petStats.Agility += (Pet == "Imp" ? 79 : (Pet == "Infernal" ? 113 : 90)) * (1f + charStats.BonusAgilityMultiplier);
            petStats.Stamina += ((Pet == "Imp" ? 118 : (Pet == "Infernal" ? 361 : 328)) + (charStats.Stamina * 0.3f)) * (1f + charStats.BonusStaminaMultiplier) * (talents.FelVitality > 0 && isImpVoidSucHuntOrGuard ? (1f + talents.FelVitality * 0.05f) : 1f);
            petStats.Intellect += ((Pet == "Imp" ? 424 : (Pet == "Infernal" ? 65 : 150)) + (charStats.Intellect * 0.3f)) * (talents.FelVitality > 0 && isImpVoidSucHuntOrGuard ? (1f + talents.FelVitality * 0.05f) : 1f);
            petStats.Spirit += (Pet == "Imp" ? 367 : (Pet == "Infernal" ? 109 : 209)) * (1f + charStats.BonusSpiritMultiplier);

            //AttackPower 57% SP to AP + 2xStr
            //(Pet == "Imp" ? 287 : 608) OLD
            petStats.AttackPower -= 20
                                  - (((charStats.SpellPower + Math.Max(charStats.SpellShadowDamageRating, charStats.SpellFireDamageRating)) * 0.57f) + petStats.Strength * 2) * (1f + charStats.BonusAttackPowerMultiplier);
            if (Pet == "Felguard") 
            {
                petStats.AttackPower *= 1f + ((0.05f + (talents.DemonicBrutality > 0 ? talents.DemonicBrutality * 0.01f : 0)) * 10);
                if (talents.GlyphFelguard) { petStats.AttackPower *= 1.2f; }
            };
            
            //SpellPower 15% char SP to pet SP
            petStats.SpellPower += ((charStats.SpellPower + Math.Max(charStats.SpellShadowDamageRating, charStats.SpellFireDamageRating)) * 0.15f) * (1f + charStats.BonusSpellPowerMultiplier);
            
            #region Crit
            petStats.PhysicalCrit += 0.0320f + StatConversion.GetPhysicalCritFromAgility(petStats.Agility, CharacterClass.Warlock) + (charStats.Warlock2T9);
            petStats.PhysicalCrit += StatConversion.NPC_LEVEL_CRIT_MOD[CalculationOptions.TargetLevel - character.Level];
            petStats.SpellCrit += 0.0092f + StatConversion.GetSpellCritFromIntellect(petStats.Intellect) + (charStats.Warlock2T9);
            
            if (talents.ImprovedDemonicTactics > 0) 
            { 
                petStats.PhysicalCrit += charStats.PhysicalCrit * talents.ImprovedDemonicTactics * 0.1f; 
                petStats.SpellCrit += charStats.SpellCrit * talents.ImprovedDemonicTactics * 0.1f;
            }

            critCoefSpecial = 1.5f;
            critCoefMelee = 2;
            #endregion

            #region Mana
            
            if (Pet == "Imp") {
                baseMana = 1175; 
            } else if (Pet == "Succubus" || Pet == "Felhunter" || Pet == "Voidwalker") {
                baseMana = 1559;
            } else if (Pet == "Felguard") {
                baseMana = 3331;
            } else if (Pet == "Doomguard") {
                baseMana = 3000;
            } else {
                baseMana = 0;
            }

            petStats.Mana += baseMana + petStats.Intellect * (Pet == "Imp" ? 4.8f : 10.8f);
            petStats.Mp5 += (Pet == "Imp" ?  -257 : -55) 
                          + (Pet == "Imp" ? 5f/6f * petStats.Intellect : 2f/3f * petStats.Intellect); //Mp5 from int
            #endregion


        }

        private void calcSpecialDPS() 
        {
            float specialHit = 0;
            float specialCastTime = 0;
            specialAttackDmg = 0;
            specialAttackSpeed = 0;

            #region Mana Cost
            if (Pet == "Imp") specialCost = 180;
            else if (Pet == "Felhunter") specialCost = (int)Math.Floor(baseMana * 0.03f);
            else if (Pet == "Succubus") specialCost = 250;
            else if (Pet == "Felguard") specialCost = (int)Math.Floor(baseMana * 0.1f);
            else if (Pet == "Infernal") specialCost = 0;
            #endregion

            if (Pet == "Imp") 
            {
                petStats.SpellCrit += talents.MasterDemonologist * 0.01f 
                                    + talents.DemonicEmpowerment * 0.2f * 30f / (60f * (1 - talents.Nemesis * 0.1f));
                
                specialCastTime = specialAttackSpeed = 2.5f - (talents.DemonicPower > 0 ? talents.DemonicPower * 0.25f : 0);
                //Improved Imp only applies to base dmg of the spell
                specialHit = 2.5f / 3.5f * petStats.SpellPower + (199 + 223) * (1f + talents.ImprovedImp * 0.1f) / 2;
                //Damage Multipliers
                specialHit *= (1 + talents.EmpoweredImp * 0.05f + (talents.GlyphImp ? 0.2f : 0))
                            * (1 + talents.MasterDemonologist * 0.01f)
                            * (1 + talents.UnholyPower * 0.04f)
                            * (1 + petStats.BonusFireDamageMultiplier);
            } else if (Pet == "Felhunter") {
                //?? DoTs give 5% extra per DoT
                specialAttackSpeed = 6f;
                specialHit = 1.5f / 3.5f * petStats.SpellPower + (98 + 138) / 2;
                if (talents.UnholyPower > 0) { specialHit *= 1f + talents.UnholyPower * 0.04f; }
            } else if (Pet == "Succubus") {
                specialAttackSpeed = 12f - (talents.DemonicPower > 0 ? talents.DemonicPower * 3 : 0);
                specialHit = 1.5f / 3.5f * petStats.SpellPower + 237;
                //+20% damage + 5% dmg + 5% spell crit  
                if (talents.MasterDemonologist > 0) { petStats.SpellCrit += 1f + talents.MasterDemonologist * 0.01f; specialHit *= 1 + talents.MasterDemonologist * 0.01f; }
                if (talents.UnholyPower > 0) { specialHit *= 1f + talents.UnholyPower * 0.04f; }
            } else if (Pet == "Infernal") {
                //2 sec periodic aura
                specialCastTime = specialAttackSpeed = 2f;
                specialHit = 0.20f * petStats.SpellPower + 40;
            }
            else if (Pet == "Felguard")
            {
                //cleave treated as physical
                specialAttackSpeed = 6f;
                specialHit  = (412.5f + (2f * petStats.AttackPower / 14.0f) + 124f );
                //damage mods
                specialHit *= (1f - (StatConversion.WHITE_DODGE_CHANCE_CAP[CalculationOptions.TargetLevel - character.Level] * (1 - petHit)))
                            * (1f - StatConversion.GetArmorDamageReduction(character.Level, StatConversion.NPC_ARMOR[CalculationOptions.TargetLevel - character.Level], petStats.ArmorPenetration, 0f, 0f))
                            * (petHit)
                            * (1 + petStats.BonusPhysicalDamageMultiplier)
                            * (1 + talents.MasterDemonologist * 0.01f) * (1 + talents.UnholyPower * 0.04f);
            }
            //felguard uses physical crit and multiplier for crit
            specialAttackDmg = specialHit * (1f + ((Pet == "Felguard" ? critCoefMelee : critCoefSpecial) - 1f) * (Pet == "Felguard" ? petStats.PhysicalCrit : petStats.SpellCrit));
        }

        public void calcBaseDPS() 
        {
            
            #region Base Melee Damage
            baseAttackDmg = 0;
            if      (Pet == "Felhunter")  { baseAttackDmg = (412.5f + (2f * petStats.AttackPower / 14.0f)) * 0.8f; }
            else if (Pet == "Succubus") { baseAttackDmg = (412.5f + (2f * petStats.AttackPower / 14.0f)) * 1.05f; }
            else if (Pet == "Voidwalker") { baseAttackDmg = (412.5f + (2f * petStats.AttackPower / 14.0f)) * 0.86f; }
            else if (Pet == "Felguard")   { baseAttackDmg = (412.5f + (2f * petStats.AttackPower / 14.0f)) * 1.05f; }
            else if (Pet == "Infernal") { baseAttackDmg = (412.5f + (2f * petStats.AttackPower / 14.0f)) * 3.2f; } //??
            else if (Pet == "Doomguard") { baseAttackDmg = (412.5f + (2f * petStats.AttackPower / 14.0f)) * 1.98f; } //??
            #endregion

            #region Attack Speed
            baseAttackSpeed = 2;
            if (Pet == "Felguard" && talents.DemonicEmpowerment > 0) 
            { 
                baseAttackSpeed *= 1f - talents.DemonicEmpowerment * 15f / (60f * (1f - talents.Nemesis * 0.1f));
            }
            #endregion

            #region Damage Multipliers
            baseAttackDmg *= (1 + petStats.BonusPhysicalDamageMultiplier)
                           * (1f + (critCoefMelee - 1f) * petStats.PhysicalCrit);
            if (talents.UnholyPower > 0 && (Pet == "Voidwalker" || Pet == "Succubus" || Pet == "Felhunter" || Pet == "Felguard")) { baseAttackDmg *= 1f + talents.UnholyPower * 0.04f; }
            if (talents.MasterDemonologist > 0 && Pet == "Felguard") { baseAttackDmg *= 1f + talents.MasterDemonologist * 0.01f; }
            #endregion

            #region Damage Reducers
            baseAttackDmg *= petHit
                           * (1f - (StatConversion.WHITE_DODGE_CHANCE_CAP[CalculationOptions.TargetLevel-character.Level] * (1 - petHit)))
                           * (1f - StatConversion.GetArmorDamageReduction(character.Level, StatConversion.NPC_ARMOR[CalculationOptions.TargetLevel - character.Level], petStats.ArmorPenetration, 0f, 0f))
                           * ((1f - StatConversion.WHITE_GLANCE_CHANCE_CAP[CalculationOptions.TargetLevel - character.Level]) + 
                                   StatConversion.WHITE_GLANCE_CHANCE_CAP[CalculationOptions.TargetLevel - character.Level] * 0.7f);
            #endregion

        }

        public float getPetDPS(Solver solv) 
        {
            solver = solv;

            if (Pet == null || (Pet == "Felguard" && !(talents.SummonFelguard > 0))) { return 0; }

            #region Hit
            if (character.Race == CharacterRace.Draenei) { petHit += 0.01f; }
            petHit = Math.Min(1f,solver.HitChance/100f);
            #endregion
            
            calcSpecialDPS();
            calcBaseDPS();

            #region Mana Gains

            //initial mana
            float availableMana = petStats.Mana;

            //mana gained from Mp5
            availableMana += petStats.Mp5 / 5f * solver.time;

            //replenishment
            if (CalculationOptions.Replenishment > 0) 
            { 
                availableMana += petStats.Mana * 0.002f * (CalculationOptions.Replenishment / 100f) * solver.time; 
            }

            //ISL + Mana Feed
            availableMana += solver.petManaGain;

            //JoW
            if (charStats.ManaRestoreFromBaseManaPPM > 0) 
            {
                availableMana += baseMana * 0.005f * (CalculationOptions.JoW / 100f) * solver.time;
            }

            if (Pet == "Felhunter" && talents.ImprovedFelhunter > 0) availableMana += petStats.Mana * talents.ImprovedFelhunter * 0.04f * solver.time / specialAttackSpeed;
            #endregion

            int specialHits = 0;
            //calculated number of special hits based on limiting factor (time or mana)
            if (specialAttackSpeed > 0 && specialCost > 0)
            {
                specialHits = (int)Math.Floor(Math.Min(availableMana / specialCost, solver.time / specialAttackSpeed));
            } else if (specialAttackSpeed > 0) {
                specialHits = (int)Math.Floor(solver.time / specialAttackSpeed);
            }

            //used for Empowered Imp calc.
            critCount = petStats.SpellCrit * specialHits * petHit;

            return (baseAttackDmg > 0 ? baseAttackDmg / baseAttackSpeed : 0) + (specialAttackDmg > 0 ? specialAttackDmg * specialHits / solver.time : 0);
        }
    }
}