using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    public class AttackModel
    {
        private Character Character;
        private CalculationOptionsProtPaladin CalcOpts;
        private BossOptions BossOpts;
        private Stats Stats;
        private DefendTable DefendTable;
        private ParryModel ParryModel;

        public AbilityModelList Abilities = new AbilityModelList();

        public string Name { get; private set; }
        public string Description { get; private set; }
        public float ThreatPerSecond { get; private set; }
        public float DamagePerSecond { get; private set; }
        public float AttackerHitsPerSecond { get; private set; }

        private void Calculate() {
            float modelLength = 0.0f;
            float modelThreat = 0.0f;
            float modelDamage = 0.0f;
            float modelCrits = 0.0f;

            Name        = "Basic";
            Description = "9-3-9 Rotation";
            modelLength = 9.0f;

            /*
             * This is the rotation priority system for Cataclysm protection paladins.
             * The 9-3-9 rotation used is:     A - B - A - C - A - D
             * Where:
             * A = Crusader Strike for single target, Hammer of the Righteous for AoE
             * B = Judgement
             * C = Determined by Priority System
             * D = Shield of the Righteous // TODO: Allow option for inquisition
             * The code below is modeling "C", which can be one of Avenger's Shield, Holy Wrath, Consecration, or Hammer of Wrath.
             * The priority is setup by the user in the options panel, defaulting to AS > HW > Con > HoW.
             */
            float usageAvengersShield = 0f;
            float usageHolyWrath = 0f;
            float usageConsecration = 0f;
            float usageHammerOfWrath = 0f;

            float grandCrusaderChance = 1f - (float)Math.Pow(1f - (Character.PaladinTalents.GrandCrusader * 0.1f), 3f);

            if (CalcOpts.RankAvengersShield == 1)
            {
                usageAvengersShield = 1f / (1f + (1f - grandCrusaderChance));

                if (CalcOpts.RankHolyWrath == 2)
                {
                    usageHolyWrath = 1f - usageAvengersShield;
                }
                else if (CalcOpts.RankConsecration == 2)
                {
                    usageConsecration = Math.Min(0.25f, 1f - usageAvengersShield); // TODO: This and others like it are a VERY loose estimate, need to figure out exact probability
                    if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 1f - usageAvengersShield - usageConsecration;
                    }
                    else if (CalcOpts.RankHammerOfWrath == 3)
                    {
                        usageHammerOfWrath = 0.2f * (1f - usageAvengersShield - usageConsecration);
                        usageHolyWrath = 1f - usageAvengersShield - usageConsecration - usageHammerOfWrath;
                    }
                }
                else if (CalcOpts.RankHammerOfWrath == 2) 
                {
                    usageHammerOfWrath = 0.2f * (1f - usageAvengersShield);
                    if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 1f - usageAvengersShield - usageHammerOfWrath;
                    }
                    else if (CalcOpts.RankConsecration == 3)
                    {
                        usageConsecration = 0.8f * Math.Min(0.25f, 1f - usageAvengersShield);
                        usageHolyWrath = 1f - usageAvengersShield - usageConsecration - usageHammerOfWrath;
                    }
                }
            }
            else if (CalcOpts.RankHolyWrath == 1)
            {
                usageHolyWrath = 0.5f;
                if (CalcOpts.RankAvengersShield == 2)
                {
                    usageAvengersShield = 0.5f;
                }
                else if (CalcOpts.RankConsecration == 2)
                {
                    usageConsecration = 0.25f;
                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.25f;
                    }
                    else if (CalcOpts.RankHammerOfWrath == 3)
                    {
                        usageHammerOfWrath = 0.05f;
                        usageAvengersShield = 0.2f;
                    }
                }
                else if (CalcOpts.RankHammerOfWrath == 2)
                {
                    usageHammerOfWrath = 0.1f;
                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.4f;
                    }
                    else if (CalcOpts.RankConsecration == 3)
                    {
                        usageConsecration = 0.2f;
                        usageAvengersShield = 0.2f;
                    }
                }
            }
            else if (CalcOpts.RankConsecration == 1)
            {
                usageConsecration = 0.25f;
                if (CalcOpts.RankAvengersShield == 2)
                {
                    usageAvengersShield = 0.5f + 0.25f * (float)Math.Pow(grandCrusaderChance, 2f);
                    if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 1f - usageConsecration - usageAvengersShield;
                    }
                    else if (CalcOpts.RankHammerOfWrath == 3)
                    {
                        usageHammerOfWrath = 0.2f * (1f - usageConsecration - usageAvengersShield);
                        usageHolyWrath = 1f - usageConsecration - usageAvengersShield - usageHammerOfWrath;
                    }
                }
                else if (CalcOpts.RankHolyWrath == 2)
                {
                    usageHolyWrath = 0.5f;
                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.25f;
                    }
                    else if (CalcOpts.RankHammerOfWrath == 3)
                    {
                        usageHammerOfWrath = 0.05f;
                        usageAvengersShield = 0.2f;
                    }
                }
                else if (CalcOpts.RankHammerOfWrath == 2)
                {
                    usageHammerOfWrath = 0.15f;
                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.6f * (0.5f + 0.25f * (float)Math.Pow(grandCrusaderChance, 2f));
                        usageHolyWrath = 1f - usageConsecration - usageHammerOfWrath - usageAvengersShield;
                    }
                    else if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 0.3f;
                        usageAvengersShield = 0.3f;
                    }
                }
            }
            else if (CalcOpts.RankHammerOfWrath == 1)
            {
                usageHammerOfWrath = 0.2f;
                if (CalcOpts.RankAvengersShield == 2)
                {
                    usageAvengersShield = 0.8f * (1f / (1f + (1f - grandCrusaderChance)));

                    if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 1f - usageAvengersShield;
                    }
                    else if (CalcOpts.RankConsecration == 3)
                    {
                        usageConsecration = 0.8f * Math.Min(0.25f, 1f - usageAvengersShield); // TODO: This and others like it are a VERY loose estimate, need to figure out exact probability
                        usageHolyWrath = 1f - usageAvengersShield - usageConsecration;
                    }
                }
                else if (CalcOpts.RankHolyWrath == 2)
                {
                    usageHolyWrath = 0.4f;
                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.4f;
                    }
                    else if (CalcOpts.RankConsecration == 3)
                    {
                        usageConsecration = 0.2f;
                        usageAvengersShield = 0.2f;
                    }
                }
                else if (CalcOpts.RankConsecration == 2)
                {
                    usageConsecration = 0.2f;
                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.8f * (0.5f + 0.25f * (float)Math.Pow(grandCrusaderChance, 2f));
                        usageHolyWrath = 1f - usageHammerOfWrath - usageConsecration - usageAvengersShield;
                    }
                    else if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 0.4f;
                        usageAvengersShield = 0.2f;
                    }
                }
            }

            // Model "A"
            if (CalcOpts.UseAoE)
            {
                modelThreat += 3.0f * (Abilities[Ability.HammerOfTheRighteous].Threat
                                       + Abilities[Ability.HammerOfTheRighteousProc].Threat);
                modelDamage += 3.0f * (Abilities[Ability.HammerOfTheRighteous].Damage
                                       + Abilities[Ability.HammerOfTheRighteousProc].Damage);
                modelCrits  += 3.0f * (Abilities[Ability.HammerOfTheRighteous].CritPercentage
                                       + Abilities[Ability.HammerOfTheRighteousProc].CritPercentage);
            }
            else
            {
                modelThreat += 3.0f * Abilities[Ability.CrusaderStrike].Threat;
                modelDamage += 3.0f * Abilities[Ability.CrusaderStrike].Damage;
                modelCrits  += 3.0f * Abilities[Ability.CrusaderStrike].CritPercentage;
            }

            // Model "B"
            if (CalcOpts.SealChoice == "Truth")
            {
                modelThreat += Abilities[Ability.JudgementOfTruth].Threat;
                modelDamage += Abilities[Ability.JudgementOfTruth].Damage;
                modelCrits  += Abilities[Ability.JudgementOfTruth].CritPercentage;
            }
            else
            {
                modelThreat += Abilities[Ability.JudgementOfRighteousness].Threat;
                modelDamage += Abilities[Ability.JudgementOfRighteousness].Damage;
                modelCrits  += Abilities[Ability.JudgementOfRighteousness].CritPercentage;
            }

            // Model "C"
            modelThreat += usageAvengersShield * Abilities[Ability.AvengersShield].Threat;
            modelThreat += usageHolyWrath * Abilities[Ability.HolyWrath].Threat;
            modelThreat += usageConsecration * Abilities[Ability.Consecration].Threat;
            modelThreat += usageHammerOfWrath * Abilities[Ability.HammerOfWrath].Threat;

            modelDamage += usageAvengersShield * Abilities[Ability.AvengersShield].Damage;
            modelDamage += usageHolyWrath * Abilities[Ability.HolyWrath].Damage;
            modelDamage += usageConsecration * Abilities[Ability.Consecration].Damage;
            modelDamage += usageHammerOfWrath * Abilities[Ability.HammerOfWrath].Damage;

            modelCrits  += usageAvengersShield * Abilities[Ability.AvengersShield].CritPercentage;
            modelCrits  += usageHolyWrath * Abilities[Ability.HolyWrath].CritPercentage;
            modelCrits  += usageConsecration * Abilities[Ability.Consecration].CritPercentage;
            modelCrits  += usageHammerOfWrath * Abilities[Ability.HammerOfWrath].CritPercentage;

            // Model "D"
            modelThreat = Abilities[Ability.ShieldOfTheRighteous].Threat;
            modelDamage = Abilities[Ability.ShieldOfTheRighteous].Damage;
            modelCrits  = Abilities[Ability.ShieldOfTheRighteous].CritPercentage;

            // White Damage
            float reckoningUptime = 1f - (float)Math.Pow((1f - 0.02f * Character.PaladinTalents.Reckoning * DefendTable.AnyHit), (Math.Min(8f, 4f * ParryModel.WeaponSpeed) / ParryModel.BossAttackSpeed));
            float weaponSwings = modelLength / ParryModel.WeaponSpeed / (1 - reckoningUptime);
            modelThreat += Abilities[Ability.MeleeSwing].Threat * weaponSwings;
            modelDamage += Abilities[Ability.MeleeSwing].Damage * weaponSwings;
            modelCrits += Abilities[Ability.MeleeSwing].CritPercentage * weaponSwings;
            
            // Seals
            float weaponHits = weaponSwings * (1f - Abilities[Ability.MeleeSwing].AttackTable.AnyMiss); // Only count melee hits that landed
            weaponHits += (CalcOpts.UseAoE ? 0f : 3f * (1f - Abilities[Ability.CrusaderStrike].AttackTable.AnyMiss)); // Only add Crusader Strikes that hit
            switch (CalcOpts.SealChoice) {
                // Seal of Righteousness
                case "Seal of Righteousness":				
                    weaponHits  += (1f - Abilities[Ability.JudgementOfRighteousness].AttackTable.AnyMiss); // Only add Judgements that hit
                    
                    modelThreat += Abilities[Ability.SealOfRighteousness].Threat * weaponHits;
                    modelDamage += Abilities[Ability.SealOfRighteousness].Damage * weaponHits;
                    modelCrits  += Abilities[Ability.SealOfRighteousness].CritPercentage * weaponHits;
                    break;
                //Seal of Truth Mode
                case "Seal of Truth":
                    weaponHits  += (1f - Abilities[Ability.JudgementOfTruth].AttackTable.AnyMiss); // Only add Judgements that hit

                    modelThreat += Abilities[Ability.SealOfTruth].Threat * weaponHits;
                    modelDamage += Abilities[Ability.SealOfTruth].Damage * weaponHits;
                    modelCrits  += Abilities[Ability.SealOfTruth].CritPercentage * weaponHits;

                    //Censure (Seal of Truth DOT)
                    modelThreat += Abilities[Ability.CensureTick].Threat;
                    modelDamage += Abilities[Ability.CensureTick].Damage;
                    modelCrits  += Abilities[Ability.CensureTick].CritPercentage;
                    break;
            }

            float attackerHits = DefendTable.AnyHit * (modelLength / ParryModel.BossAttackSpeed);

            ThreatPerSecond = modelThreat / modelLength;
            DamagePerSecond = modelDamage / modelLength;
            AttackerHitsPerSecond = attackerHits / modelLength;
        }

        public AttackModel(Character character, Stats stats, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts)
        {
            Character        = character;
            CalcOpts         = calcOpts;
            BossOpts         = bossOpts;
            Stats            = stats;
            DefendTable      = new DefendTable(character, stats, calcOpts, bossOpts);
            ParryModel       = new ParryModel(character, stats, calcOpts, bossOpts);
            
            Abilities.Add(Ability.MeleeSwing, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.ShieldOfTheRighteous, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.HammerOfTheRighteous, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.SealOfTruth, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.CensureTick, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.JudgementOfTruth, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.SealOfRighteousness, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.JudgementOfRighteousness, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.HammerOfWrath, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.AvengersShield, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.RetributionAura, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.HolyWrath, character, stats, calcOpts, bossOpts);
            Abilities.Add(Ability.Consecration, character, stats, calcOpts, bossOpts);

            Calculate();
        }
    }
}
