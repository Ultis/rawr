using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
    class PetCalculations
    {
        Solver solver;
        public Stats petStats;
        Character character;
        CalculationOptionsWarlock CalculationOptions;
        String Pet;
        Stats charStats;

        double baseAttackDmg;
        float baseAttackSpeed;
        double specialAttackDmg;
        float specialAttackSpeed;
        int specialCost;
        float critCoefSpecial;
        float critCoefMelee;
        double petHit;
        public float critCount;

        public PetCalculations(Stats charStats, Character character)
        {
            this.character = character;
            this.CalculationOptions = character.CalculationOptions as CalculationOptionsWarlock;
            this.Pet = CalculationOptions.Pet;

            calculatePetStats(charStats);
        }

        private void calculatePetStats(Stats charStats)
        {
            this.charStats = charStats;

            petStats = new Stats();
            //Strength
            if (Pet == "Imp") petStats.Strength = 297;
            else petStats.Strength = 314;
            petStats.Strength *= 1 + charStats.BonusStrengthMultiplier;
            //Agility
            if (Pet == "Imp") petStats.Agility = 79;
            else petStats.Agility = 90;
            petStats.Agility *= 1 + charStats.BonusAgilityMultiplier;
            //Stamina
            if (Pet == "Imp") petStats.Stamina = 118;
            else petStats.Stamina = 328;
            //petStats.Stamina += charStats.Stamina * (0.299f + (character.WarlockTalents.FelSynergy > 0 ? character.WarlockTalents.FelSynergy * 0.05f : 0));
            petStats.Stamina *= 1 + charStats.BonusStaminaMultiplier;
            if (character.WarlockTalents.FelVitality > 0 && (Pet == "Imp" || Pet == "Voidwalker" || Pet == "Succubus" || Pet == "Felhunter" || Pet == "Felguard"))
                petStats.Stamina *= 1 + character.WarlockTalents.FelVitality * 0.05f;
            //Intellect
            if (Pet == "Imp") petStats.Intellect = 369;
            else petStats.Intellect = 150;
            //petStats.Intellect += charStats.Intellect + (0.299f + (character.WarlockTalents.FelSynergy > 0 ? character.WarlockTalents.FelSynergy * 0.05f : 0));
            petStats.Intellect *= 1 + charStats.BonusIntellectMultiplier;
            if (character.WarlockTalents.FelVitality > 0 && (Pet == "Imp" || Pet == "Voidwalker" || Pet == "Succubus" || Pet == "Felhunter" || Pet == "Felguard"))
                petStats.Intellect *= 1 + character.WarlockTalents.FelVitality * 0.05f;
            //Spirit
            if (Pet == "Imp") petStats.Spirit = 367;
            else petStats.Spirit = 209;
            petStats.Spirit *= 1 + charStats.BonusSpiritMultiplier;
            //AttackPower
            if (Pet == "Imp") petStats.AttackPower = 287;
            else petStats.AttackPower = 608;
            petStats.AttackPower += (charStats.SpellPower + Math.Max(charStats.SpellShadowDamageRating, charStats.SpellFireDamageRating)) * 0.57f;
            petStats.AttackPower *= 1 + charStats.BonusAttackPowerMultiplier;
            if (Pet == "Felguard") petStats.AttackPower *= 1 + ((0.05f + (character.WarlockTalents.DemonicBrutality > 0 ? character.WarlockTalents.DemonicBrutality * 0.01f : 0)) * 10);
            if (Pet == "Felguard" && CalculationOptions.GlyphFelguard) petStats.AttackPower *= 1.2f;
            //SpellPower
            petStats.SpellPower = (charStats.SpellPower + Math.Max(charStats.SpellShadowDamageRating, charStats.SpellFireDamageRating)) * 0.1495f;
            petStats.SpellPower *= 1 + charStats.BonusSpellPowerMultiplier;
            //Mana
            int baseMana = (Pet == "Imp" ? 1175 : 1559);
            petStats.Mana = baseMana + petStats.Intellect * 11.55f;
            //Mana cost of special attack
            if (Pet == "Imp") specialCost = 180;
            else if (Pet == "Felhunter") specialCost = 131;
            else if (Pet == "Succubus") specialCost = 250;
            else if (Pet == "Felguard") specialCost = 439;
            //Crit
            petStats.PhysicalCrit = 0.0320f + petStats.Agility * 0.00019f;
            petStats.PhysicalCrit -= ((CalculationOptions.TargetLevel * 5f) * 0.0004f);
            if (character.WarlockTalents.ImprovedDemonicTactics > 0)
                petStats.PhysicalCrit += charStats.PhysicalCrit * character.WarlockTalents.ImprovedDemonicTactics * 0.1f;
            petStats.SpellCrit = 0.05f + petStats.Intellect / 460 * 0.061f;//??
            if (character.WarlockTalents.ImprovedDemonicTactics > 0)
                petStats.SpellCrit += charStats.SpellCrit * character.WarlockTalents.ImprovedDemonicTactics * 0.1f;
            if (Pet == "Imp" && character.WarlockTalents.MasterDemonologist > 0)
                petStats.SpellCrit += character.WarlockTalents.MasterDemonologist * 0.01f;
            if (Pet == "Imp" && character.WarlockTalents.DemonicEmpowerment > 0)
                petStats.SpellCrit += character.WarlockTalents.DemonicEmpowerment * 0.2f * 30f / (60f * (1 - character.WarlockTalents.Nemesis * 0.1f));

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
                specialCastTime = specialAttackSpeed = 2.5f - (character.WarlockTalents.DemonicPower > 0 ? character.WarlockTalents.DemonicPower * 0.25f : 0);
                specialHit = 2.5f / 3.5f * petStats.SpellPower + (199 + 223) / 2;
                if (character.WarlockTalents.ImprovedImp > 0)
                    specialHit *= 1 + character.WarlockTalents.ImprovedImp * 0.1f;
                if (character.WarlockTalents.UnholyPower > 0)
                    specialHit *= 1 + character.WarlockTalents.UnholyPower * 0.04f;
                if (character.WarlockTalents.MasterDemonologist > 0)
                    specialHit *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                if (character.WarlockTalents.EmpoweredImp > 0)
                    specialHit *= 1 + character.WarlockTalents.EmpoweredImp * 0.05f;
                if (CalculationOptions.GlyphImp)
                    specialHit *= 1.2f;
            }
            else if (Pet == "Felhunter")
            {
                //?? DoTs give 5% extra per DoT
                specialAttackSpeed = 6f;
                specialCastTime = 1.5f;
                specialHit = specialCastTime / 3.5f * petStats.SpellPower + (98 + 138) / 2;
            }
            else if (Pet == "Succubus")
            {
                if (character.WarlockTalents.MasterDemonologist > 0)
                    critCoefSpecial *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                specialAttackSpeed = 12f - (character.WarlockTalents.DemonicPower > 0 ? character.WarlockTalents.DemonicPower * 3 : 0);
                specialCastTime = 1.5f;
                specialHit = specialCastTime / 3.5f * petStats.SpellPower + 237;
                if (character.WarlockTalents.MasterDemonologist > 0)
                    specialHit *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
            }
            else if (Pet == "Infernal")
            {
                specialCastTime = specialAttackSpeed = 1f;
                specialHit = 1.34f * petStats.SpellPower + 40;
            }
            else if (Pet == "Felguard")
            {
                specialAttackSpeed = 6f;
                specialCastTime = 1.5f;
                specialHit = specialCastTime / 3.5f /*??*/* petStats.AttackPower + 124;
            }
            specialAttackDmg = (float)((specialHit * (1 - petStats.SpellCrit) + specialHit * critCoefSpecial * petStats.SpellCrit));
        }

        public void calcBaseDPS()
        {
            baseAttackDmg = 0;
            if (Pet == "Felhunter") baseAttackDmg = petStats.AttackPower * 4 / 35 + 330;
            else if (Pet == "Succubus") baseAttackDmg = petStats.AttackPower * 0.15 + 433;
            else if (Pet == "Voidwalker") baseAttackDmg = petStats.AttackPower * 0.1225 + 355;
            else if (Pet == "Felguard") baseAttackDmg = petStats.AttackPower * 0.18 + 520;
            else if (Pet == "Infernal") baseAttackDmg = 1040 + petStats.AttackPower * 0.13;//??
            baseAttackSpeed = 2;
            if (Pet == "Felguard" && character.WarlockTalents.DemonicEmpowerment > 0) baseAttackSpeed *= 1 - character.WarlockTalents.DemonicEmpowerment * 15f / (60f * (1 - character.WarlockTalents.Nemesis * 0.1f));

            baseAttackDmg = baseAttackDmg * (1 - petStats.PhysicalCrit) + baseAttackDmg * critCoefMelee * petStats.PhysicalCrit;
            baseAttackDmg *= petHit;
            baseAttackDmg *= 1 - (0.065f + 0.005f * (CalculationOptions.TargetLevel - 3));
            float targetArmor = 10000;//??
            baseAttackDmg *= 1f - (targetArmor / ((467.5f * character.Level) + targetArmor - 22167.5f));
            baseAttackDmg = baseAttackDmg * 0.24f * 0.7f + baseAttackDmg * 0.76f;
            if (character.WarlockTalents.UnholyPower > 0 && (Pet == "Voidwalker" || Pet == "Succubus" || Pet == "Felhunter" || Pet == "Felguard"))
                baseAttackDmg *= 1 + character.WarlockTalents.UnholyPower * 0.04;
            if (character.WarlockTalents.MasterDemonologist > 0 && Pet == "Felguard")
                baseAttackDmg *= 1 + character.WarlockTalents.MasterDemonologist * 0.01;
        }

        public float getPetDPS(Solver solver)
        {
            this.solver = solver;

            if (Pet == null || (Pet == "Felguard" && !(character.WarlockTalents.SummonFelguard > 0))) return 0;

            #region Hit
            petHit = solver.HitChance;

            if (character.Race == CharacterRace.Draenei)
                petHit += 0.01;
            /*if (options.TargetLevel == 73) // TODO: Level80
            {
                petHitChance -= (5 * .004 + .02);
            }
            else
            {
                petHitChance -= ((options.TargetLevel * 5 - 350) * .001);
            }
            */
            if (petHit > 1.0)
                petHit = 1.0;
            #endregion
            
            calcSpecialDPS();
            calcBaseDPS();

            float baseMana;
            if (Pet == "Imp") baseMana = 1175;
            else baseMana = 1559;

            double availableMana = petStats.Mana;
            availableMana += petStats.Mp5 / 5f * solver.time;
            if (CalculationOptions.FSRRatio < 100)
                availableMana += Math.Floor(StatConversion.GetSpiritRegenSec(petStats.Spirit, petStats.Intellect)) * (1f - CalculationOptions.FSRRatio / 100f) * solver.time;
            if (CalculationOptions.Replenishment > 0)
                availableMana += petStats.Mana * 0.0025f * (CalculationOptions.Replenishment / 100f) * solver.time;
            availableMana += solver.petManaGain;
            if (charStats.ManaRestoreFromBaseManaPerHit > 0)
            {
                double hitCount = solver.time / baseAttackSpeed;
                availableMana += baseMana * charStats.ManaRestoreFromBaseManaPerHit * (CalculationOptions.JoW / 100f) * hitCount * petHit;
            }
            int specialHits = 0;
            while (availableMana > 0 && specialHits + 1 <= solver.time / specialAttackSpeed && specialAttackSpeed > 0)
            {
                specialHits++;
                availableMana -= specialCost;
                if (charStats.ManaRestoreFromBaseManaPerHit > 0)
                    availableMana += baseMana * charStats.ManaRestoreFromBaseManaPerHit * (CalculationOptions.JoW / 100f) * petHit;
                if (Pet == "Felhunter" && character.WarlockTalents.ImprovedFelhunter > 0)
                    availableMana += petStats.Mana * character.WarlockTalents.ImprovedFelhunter * 0.04;
            }
            critCount = petStats.SpellCrit * specialHits;
            return (float)((baseAttackDmg > 0 ? baseAttackDmg / baseAttackSpeed : 0) + (specialAttackDmg > 0 ? specialAttackDmg * specialHits / solver.time : 0));
        }
    }
}