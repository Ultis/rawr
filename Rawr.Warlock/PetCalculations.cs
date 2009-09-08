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
        public float critCount;
        #endregion

        //TODO: refactor 
        private void calculatePetStats(Stats charStats) 
        {
            this.charStats = charStats;

            bool isImpVoidSucHuntOrGuard = (Pet == "Imp" || Pet == "Voidwalker" || Pet == "Succubus" || Pet == "Felhunter" || Pet == "Felguard");

            petStats = new Stats();
            petStats.Strength  = (Pet == "Imp" ? 297 : 314) * (1f + charStats.BonusStrengthMultiplier );
            petStats.Agility   = (Pet == "Imp" ?  79 :  90) * (1f + charStats.BonusAgilityMultiplier  );
            petStats.Stamina   = (Pet == "Imp" ? 118 : 328) * (1f + charStats.BonusStaminaMultiplier  ) * (talents.FelVitality > 0 && isImpVoidSucHuntOrGuard ? (1f + talents.FelVitality * 0.05f) : 1f);//petStats.Stamina   += charStats.Stamina   * (0.299f + (talents.FelSynergy > 0 ? talents.FelSynergy * 0.05f : 0));
            petStats.Intellect = (Pet == "Imp" ? 369 : 150) * (1f + charStats.BonusIntellectMultiplier) * (talents.FelVitality > 0 && isImpVoidSucHuntOrGuard ? (1f + talents.FelVitality * 0.05f) : 1f);//petStats.Intellect += charStats.Intellect * (0.299f + (talents.FelSynergy > 0 ? talents.FelSynergy * 0.05f : 0));
            petStats.Spirit    = (Pet == "Imp" ? 367 : 209) * (1f + charStats.BonusSpiritMultiplier   );

            //AttackPower
            petStats.AttackPower = ((Pet == "Imp" ? 287 : 608) + ((charStats.SpellPower + Math.Max(charStats.SpellShadowDamageRating, charStats.SpellFireDamageRating)) * 0.57f)) * (1f + charStats.BonusAttackPowerMultiplier);
            if (Pet == "Felguard") 
            {
                petStats.AttackPower *= 1f + ((0.05f + (talents.DemonicBrutality > 0 ? talents.DemonicBrutality * 0.01f : 0)) * 10);
                if (talents.GlyphFelguard) { petStats.AttackPower *= 1.2f; }
            };
            
            //SpellPower
            petStats.SpellPower = ((charStats.SpellPower + Math.Max(charStats.SpellShadowDamageRating, charStats.SpellFireDamageRating)) * 0.1495f) * (1f + charStats.BonusSpellPowerMultiplier);
            
            //Mana
            petStats.Mana = (Pet == "Imp" ? 1175 : 1559) + petStats.Intellect * 11.55f;

            //Mana cost of special attack
            if      (Pet == "Imp")       specialCost = 180;
            else if (Pet == "Felhunter") specialCost = 131;
            else if (Pet == "Succubus")  specialCost = 250;
            else if (Pet == "Felguard")  specialCost = 439;
            
            //Crit
            petStats.PhysicalCrit = 0.0320f + (petStats.Agility * 0.00019f) * (1 + charStats.Warlock2T9);//TODO: seems ok but need to verify
            petStats.PhysicalCrit -= ((CalculationOptions.TargetLevel * 5f) * 0.0004f);// TODO Replace with STatConvers.NPC_CRIT_MOD[CalculationOptions.TargetLevel-character.Level] but need to verify first
            if (talents.ImprovedDemonicTactics > 0) 
            { 
                petStats.PhysicalCrit += charStats.PhysicalCrit * talents.ImprovedDemonicTactics * 0.1f; 
            }
            petStats.SpellCrit = 0.05f + (petStats.Intellect / 460 * 0.061f) * (1 + charStats.Warlock2T9);//TODO: need to verify
            if (talents.ImprovedDemonicTactics > 0) 
            {
                petStats.SpellCrit += charStats.SpellCrit * talents.ImprovedDemonicTactics * 0.1f;
            }
            if (Pet == "Imp" && talents.MasterDemonologist > 0) 
            { 
                petStats.SpellCrit += talents.MasterDemonologist * 0.01f; 
            }
            if (Pet == "Imp" && talents.DemonicEmpowerment > 0) 
            { 
                petStats.SpellCrit += talents.DemonicEmpowerment * 0.2f * 30f / (60f * (1 - talents.Nemesis * 0.1f));
            }

            critCoefSpecial = 1.5f;
            if (Pet == "Felguard") critCoefSpecial = 2;
            critCoefMelee = 2;

            Stats petStats2 = (new CalculationsWarlock()).GetBuffStats(character);
            petStats += petStats2;
        }

        private void calcSpecialDPS() 
        {
            float specialHit = 0;
            float specialCastTime = 0;
            specialAttackDmg = 0;
            specialAttackSpeed = 0;
            if (Pet == "Imp") 
            {
                specialCastTime = specialAttackSpeed = 2.5f - (talents.DemonicPower > 0 ? talents.DemonicPower * 0.25f : 0);
                specialHit = 2.5f / 3.5f * petStats.SpellPower + (199 + 223) / 2;
                if (talents.ImprovedImp         > 0) { specialHit *= 1f + talents.ImprovedImp * 0.1f; }
                if (talents.UnholyPower         > 0) { specialHit *= 1f + talents.UnholyPower * 0.04f; }
                if (talents.MasterDemonologist  > 0) { specialHit *= 1f + talents.MasterDemonologist * 0.01f; }
                if (talents.EmpoweredImp        > 0) { specialHit *= 1f + talents.EmpoweredImp * 0.05f; }
                if (talents.GlyphImp               ) { specialHit *= 1f + 0.2f; }
            } else if (Pet == "Felhunter") {
                //?? DoTs give 5% extra per DoT
                specialAttackSpeed = 6f;
                specialCastTime = 1.5f;
                specialHit = specialCastTime / 3.5f * petStats.SpellPower + (98 + 138) / 2;
            } else if (Pet == "Succubus") {
                if (talents.MasterDemonologist > 0) { critCoefSpecial *= 1f + talents.MasterDemonologist * 0.01f; }
                specialAttackSpeed = 12f - (talents.DemonicPower > 0 ? talents.DemonicPower * 3 : 0);
                specialCastTime = 1.5f;
                specialHit = specialCastTime / 3.5f * petStats.SpellPower + 237;
                if (talents.MasterDemonologist > 0) { specialHit *= 1 + talents.MasterDemonologist * 0.01f; }
            } else if (Pet == "Infernal") {
                specialCastTime = specialAttackSpeed = 1f;
                specialHit = 1.34f * petStats.SpellPower + 40;
            } else if (Pet == "Felguard") {
                specialAttackSpeed = 6f;
                specialCastTime = 1.5f;
                specialHit = specialCastTime / 3.5f /*??*/* petStats.AttackPower + 124;
            }
            specialAttackDmg = (float)((specialHit * (1 - petStats.SpellCrit) + specialHit * critCoefSpecial * petStats.SpellCrit));
        }

        public void calcBaseDPS() 
        {
            baseAttackDmg = 0;
            if      (Pet == "Felhunter")  { baseAttackDmg = petStats.AttackPower * 4f/35f  +  330f; }
            else if (Pet == "Succubus")   { baseAttackDmg = petStats.AttackPower * 0.1500f +  433f; }
            else if (Pet == "Voidwalker") { baseAttackDmg = petStats.AttackPower * 0.1225f +  355f; }
            else if (Pet == "Felguard")   { baseAttackDmg = petStats.AttackPower * 0.1800f +  520f; }
            else if (Pet == "Infernal")   { baseAttackDmg = petStats.AttackPower * 0.1300f + 1040f; }//??
            baseAttackSpeed = 2;
            if (Pet == "Felguard" && talents.DemonicEmpowerment > 0) 
            { 
                baseAttackSpeed *= 1f - talents.DemonicEmpowerment * 15f / (60f * (1f - talents.Nemesis * 0.1f)); 
            }

            baseAttackDmg = baseAttackDmg * (1f - petStats.PhysicalCrit) + baseAttackDmg * critCoefMelee * petStats.PhysicalCrit;
            baseAttackDmg *= petHit;
            baseAttackDmg *= 1f - StatConversion.WHITE_DODGE_CHANCE_CAP[CalculationOptions.TargetLevel-character.Level]; // Assuming this means dodge
            float targetArmor = 10000;//??
            baseAttackDmg *= 1f - (targetArmor / ((467.5f * character.Level) + targetArmor - 22167.5f));
            float damageNormal = baseAttackDmg * (1f - StatConversion.WHITE_GLANCE_CHANCE_CAP[CalculationOptions.TargetLevel - character.Level]);
            float damageGlance = baseAttackDmg * StatConversion.WHITE_GLANCE_CHANCE_CAP[CalculationOptions.TargetLevel - character.Level] * 0.7f;
            baseAttackDmg = damageNormal + damageGlance;
            if (talents.UnholyPower > 0 && (Pet == "Voidwalker" || Pet == "Succubus" || Pet == "Felhunter" || Pet == "Felguard")) { baseAttackDmg *= 1f + talents.UnholyPower * 0.04f; }
            if (talents.MasterDemonologist > 0 && Pet == "Felguard") { baseAttackDmg *= 1f + talents.MasterDemonologist * 0.01f; }
        }

        public float getPetDPS(Solver solv) 
        {
            solver = solv;

            if (Pet == null || (Pet == "Felguard" && !(talents.SummonFelguard > 0))) { return 0; }

            #region Hit
            petHit = solver.HitChance;
            if (character.Race == CharacterRace.Draenei) { petHit += 0.01f; }
            petHit = Math.Min(1f,petHit);
            #endregion
            
            calcSpecialDPS();
            calcBaseDPS();

            float baseMana = (Pet == "Imp" ? 1175 : 1559);

            float availableMana = petStats.Mana;
            availableMana += petStats.Mp5 / 5f * solver.time;
            if (CalculationOptions.Replenishment > 0) 
            { 
                availableMana += petStats.Mana * 0.0025f * (CalculationOptions.Replenishment / 100f) * solver.time; 
            }
            availableMana += solver.petManaGain;
            if (charStats.ManaRestoreFromBaseManaPPM > 0) 
            {
                float hitCount = solver.time / baseAttackSpeed;
                availableMana += baseMana * charStats.ManaRestoreFromBaseManaPPM * (CalculationOptions.JoW / 100f) * hitCount * petHit;
            }
            int specialHits = 0;
            while (availableMana > 0 && specialHits + 1 <= solver.time / specialAttackSpeed && specialAttackSpeed > 0) 
            {
                specialHits++;
                availableMana -= specialCost;
                if (charStats.ManaRestoreFromBaseManaPPM > 0) 
                { 
                    availableMana += baseMana * charStats.ManaRestoreFromBaseManaPPM * (CalculationOptions.JoW / 100f) * petHit; 
                }
                if (Pet == "Felhunter" && talents.ImprovedFelhunter > 0) 
                { 
                    availableMana += petStats.Mana * talents.ImprovedFelhunter * 0.04f; 
                }
            }
            critCount = petStats.SpellCrit * specialHits;
            return (baseAttackDmg > 0 ? baseAttackDmg / baseAttackSpeed : 0) + (specialAttackDmg > 0 ? specialAttackDmg * specialHits / solver.time : 0);
        }
    }
}