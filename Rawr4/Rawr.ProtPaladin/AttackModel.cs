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

            #region Custom rotation logic
            
            float usageAvengersShield = 0f;
            float usageHolyWrath = 0f;
            float usageConsecration = 0f;
            float usageHammerOfWrath = 0f;

            float grandCrusaderChance = 1f - (float)Math.Pow(1f - (Character.PaladinTalents.GrandCrusader * 0.1f), 3f);
            float twoConsecutiveGrandCrusaders = (float)Math.Pow(grandCrusaderChance, 2f);

            #region Avenger's Shield...

            if (CalcOpts.RankAvengersShield == 1)
            {
                usageAvengersShield = 1f / (1f + (1f - grandCrusaderChance));

                #region Avenger's Shield, Holy Wrath...

                if (CalcOpts.RankHolyWrath == 2)
                {
                    usageHolyWrath = 1f - usageAvengersShield;
                }

                #endregion

                #region Avenger's Shield, Consecration...

                else if (CalcOpts.RankConsecration == 2)
                {
                    usageConsecration = (1f - usageAvengersShield) * (1f / (1f + (1f - twoConsecutiveGrandCrusaders)));

                    #region Avenger's Shield, Consecration, Holy Wrath, Hammer of Wrath

                    if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 1f - usageAvengersShield - usageConsecration;
                    }

                    #endregion
                    
                    #region Avenger's Shield, Consecration, Hammer of Wrath, Holy Wrath

                    else if (CalcOpts.RankHammerOfWrath == 3)
                    {
                        usageHammerOfWrath = 0.2f * (1f - usageAvengersShield - usageConsecration);
                        usageHolyWrath = 1f - usageAvengersShield - usageConsecration - usageHammerOfWrath;
                    }

                    #endregion
                }

                #endregion

                #region Avenger's Shield, Hammer of Wrath...

                else if (CalcOpts.RankHammerOfWrath == 2) 
                {
                    usageHammerOfWrath = 0.2f * (1f - usageAvengersShield);

                    #region Avenger's Shield, Hammer of Wrath, Holy Wrath, Consecration

                    if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 1f - usageAvengersShield - usageHammerOfWrath;
                    }

                    #endregion

                    #region Avenger's Shield, Hammer of Wrath, Consecration, Holy Wrath
                    
                    else if (CalcOpts.RankConsecration == 3)
                    {
                        usageConsecration = (1f - usageAvengersShield - usageHammerOfWrath) * (1f / (1f + (1f - twoConsecutiveGrandCrusaders))); ;
                        usageHolyWrath = 1f - usageAvengersShield - usageConsecration - usageHammerOfWrath;
                    }

                    #endregion

                }

                #endregion

            }
            
            #endregion

            #region Holy Wrath...

            else if (CalcOpts.RankHolyWrath == 1)
            {
                usageHolyWrath = 0.5f;

                #region Holy Wrath, Avenger's Shield, n/a, n/a

                if (CalcOpts.RankAvengersShield == 2)
                {
                    usageAvengersShield = 0.5f;
                }

                #endregion

                #region Holy Wrath, Consecration...

                else if (CalcOpts.RankConsecration == 2)
                {
                    usageConsecration = 0.25f;

                    #region Holy Wrath, Consecration, Avenger's Shield, Hammer of Wrath

                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.25f;
                    }
                        
                    #endregion

                    #region Holy Wrath, Consecration, Hammer of Wrath, Avenger's Shield
                        
                    else if (CalcOpts.RankHammerOfWrath == 3)
                    {
                        usageHammerOfWrath = 0.05f;
                        usageAvengersShield = 0.2f;
                    }
                    
                    #endregion
                }

                #endregion

                #region Holy Wrath, Hammer of Wrath...

                else if (CalcOpts.RankHammerOfWrath == 2)
                {
                    usageHammerOfWrath = 0.1f;

                    #region Holy Wrath, Hammer of Wrath, Avenger's Shield, Consecration

                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.4f;
                    }

                    #endregion

                    #region Holy Wrath, Hammer of Wrath, Consecration, Avenger's Shield

                    else if (CalcOpts.RankConsecration == 3)
                    {
                        usageConsecration = 0.2f;
                        usageAvengersShield = 0.2f;
                    }

                    #endregion
                
                }

                #endregion
            }
            
            #endregion

            #region Consecration...
            
            else if (CalcOpts.RankConsecration == 1)
            {
                usageConsecration = 0.25f;

                #region Consecration, Avenger's Shield...

                if (CalcOpts.RankAvengersShield == 2)
                {
                    usageAvengersShield = 0.5f + 0.25f * (float)Math.Pow(grandCrusaderChance, 2f);

                    #region Consecration, Avenger's Shield, Holy Wrath, Hammer of Wrath

                    if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 1f - usageConsecration - usageAvengersShield;
                    }

                    #endregion

                    #region Consecration, Avenger's Shield, Hammer of Wrath, Holy Wrath

                    else if (CalcOpts.RankHammerOfWrath == 3)
                    {
                        usageHammerOfWrath = 0.2f * (1f - usageConsecration - usageAvengersShield);
                        usageHolyWrath = 1f - usageConsecration - usageAvengersShield - usageHammerOfWrath;
                    }

                    #endregion
                    
                }

                #endregion

                #region Consecration, Holy Wrath...

                else if (CalcOpts.RankHolyWrath == 2)
                {
                    usageHolyWrath = 0.5f;

                    #region Consecration, Holy Wrath, Avenger's Shield, Hammer of Wrath

                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.25f;
                    }

                    #endregion

                    #region Consecration, Holy Wrath, Hammer of Wrath, Avenger's Shield

                    else if (CalcOpts.RankHammerOfWrath == 3)
                    {
                        usageHammerOfWrath = 0.05f;
                        usageAvengersShield = 0.2f;
                    }

                    #endregion

                }

                #endregion

                #region Consecration, Hammer of Wrath...

                else if (CalcOpts.RankHammerOfWrath == 2)
                {
                    usageHammerOfWrath = 0.15f;

                    #region Consecration, Hammer of Wrath, Avenger's Shield, Holy Wrath

                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.6f * (0.5f + 0.25f * (float)Math.Pow(grandCrusaderChance, 2f));
                        usageHolyWrath = 1f - usageConsecration - usageHammerOfWrath - usageAvengersShield;
                    }

                    #endregion

                    #region Consecration, Hammer of Wrath, Holy Wrath, Avenger's Shield

                    else if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 0.3f;
                        usageAvengersShield = 0.3f;
                    }

                    #endregion

                }

                #endregion

            }

            #endregion

            #region Hammer of Wrath...

            else if (CalcOpts.RankHammerOfWrath == 1)
            {
                usageHammerOfWrath = 0.2f;

                #region Hammer of Wrath, Avenger's Shield...

                if (CalcOpts.RankAvengersShield == 2)
                {
                    usageAvengersShield = 0.8f * (1f / (1f + (1f - grandCrusaderChance)));

                    #region Hammer of Wrath, Avenger's Shield, Holy Wrath, Consecration

                    if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 1f - usageAvengersShield;
                    }

                    #endregion

                    #region Hammer of Wrath, Avenger's Shield, Consecration, Holy Wrath

                    else if (CalcOpts.RankConsecration == 3)
                    {
                        usageConsecration = (1f - usageHammerOfWrath - usageAvengersShield) * (1f / (1f + (1f - twoConsecutiveGrandCrusaders)));
                        usageHolyWrath = 1f - usageAvengersShield - usageConsecration;
                    }

                    #endregion

                }

                #endregion

                #region Hammer of Wrath, Holy Wrath...

                else if (CalcOpts.RankHolyWrath == 2)
                {
                    usageHolyWrath = 0.4f;

                    #region Hammer of Wrath, Holy Wrath, Avenger's Shield, Consecration

                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.4f;
                    }

                    #endregion

                    #region Hammer of Wrath, Holy Wrath, Consecration, Avenger's Shield

                    else if (CalcOpts.RankConsecration == 3)
                    {
                        usageConsecration = 0.2f;
                        usageAvengersShield = 0.2f;
                    }

                    #endregion

                }

                #endregion

                #region Hammer of Wrath, Consecration...

                else if (CalcOpts.RankConsecration == 2)
                {
                    usageConsecration = 0.2f;

                    #region Hammer of Wrath, Consecration, Avenger's Shield, Holy Wrath

                    if (CalcOpts.RankAvengersShield == 3)
                    {
                        usageAvengersShield = 0.8f * (0.5f + 0.25f * (float)Math.Pow(grandCrusaderChance, 2f));
                        usageHolyWrath = 1f - usageHammerOfWrath - usageConsecration - usageAvengersShield;
                    }

                    #endregion

                    #region Hammer of Wrath, Consecration, Holy Wrath, Avenger's Shield

                    else if (CalcOpts.RankHolyWrath == 3)
                    {
                        usageHolyWrath = 0.4f;
                        usageAvengersShield = 0.2f;
                    }

                    #endregion

                }

                #endregion

            }

            #endregion

            #endregion

            #region Model "A", Crusader Strike, Hammer of the Righteous, and Hammer of Righteous procs

            if (CalcOpts.UseAoE)
            {
                /*
                 * Hammer of the Righteous actually causes two attacks.
                 * The first attack is the 30% weapon damage done to the primary target.
                 * The second attack is the AoE holy damage done to all targets within 8 yards.
                 * 
                 * If the first attack does not connect, the second attack is never caused.
                 * The second attack has its own chance to miss.
                 * 
                 * This means that the AoE attack has two "chances" to miss, either by the first attack being avoided or by the second attack being missed.
                 * 
                 * This is why the second attack, which already has its own attack table, is multiplied by the hit chance of the first attack.
                 */
                modelThreat += 3.0f * (Abilities[Ability.HammerOfTheRighteous].Threat
                                       + Abilities[Ability.HammerOfTheRighteousProc].Threat * Abilities[Ability.HammerOfTheRighteous].AttackTable.AnyHit);
                modelDamage += 3.0f * (Abilities[Ability.HammerOfTheRighteous].Damage
                                       + Abilities[Ability.HammerOfTheRighteousProc].Damage * Abilities[Ability.HammerOfTheRighteous].AttackTable.AnyHit);
                modelCrits  += 3.0f * (Abilities[Ability.HammerOfTheRighteous].CritPercentage
                                       + Abilities[Ability.HammerOfTheRighteousProc].CritPercentage * Abilities[Ability.HammerOfTheRighteous].AttackTable.AnyHit);
            }
            else if(Abilities.ContainsKey(Ability.CrusaderStrike)) // fail check
            {
                modelThreat += 3.0f * Abilities[Ability.CrusaderStrike].Threat;
                modelDamage += 3.0f * Abilities[Ability.CrusaderStrike].Damage;
                modelCrits  += 3.0f * Abilities[Ability.CrusaderStrike].CritPercentage;
            }

            #endregion

            #region Model "B", Judgement of Righteousness and Judgement of Truth

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

            #endregion

            #region Model "C", Avenger's Shield, Holy Wrath, Consecration, and Hammer of Wrath

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

            #endregion

            #region Model "D", Shield of the Righteous

            modelThreat = Abilities[Ability.ShieldOfTheRighteous].Threat;
            modelDamage = Abilities[Ability.ShieldOfTheRighteous].Damage;
            modelCrits  = Abilities[Ability.ShieldOfTheRighteous].CritPercentage;

            #endregion

            #region White Damage, including Reckoning procs

            float reckoningUptime = 1f - (float)Math.Pow((1f - 0.02f * Character.PaladinTalents.Reckoning * DefendTable.Block), (Math.Min(8f, 4f * ParryModel.WeaponSpeed) / CalcOpts.BossAttackSpeed));
            float weaponSwings = modelLength / ParryModel.WeaponSpeed / (1 - reckoningUptime);
            modelThreat += Abilities[Ability.MeleeSwing].Threat * weaponSwings;
            modelDamage += Abilities[Ability.MeleeSwing].Damage * weaponSwings;
            modelCrits += Abilities[Ability.MeleeSwing].CritPercentage * weaponSwings;

            #endregion

            #region Seal procs, from melee hits, judgements, and crusader strikes

            float weaponHits = weaponSwings * Abilities[Ability.MeleeSwing].AttackTable.AnyHit; // Only count melee hits that landed
            weaponHits += (CalcOpts.UseAoE ? 0f : Abilities.ContainsKey(Ability.CrusaderStrike) ? 3f * Abilities[Ability.CrusaderStrike].AttackTable.AnyHit : 0f); // Only add Crusader Strikes that hit
            switch (CalcOpts.SealChoice) {
                // Seal of Righteousness
                case "Seal of Righteousness":				
                    weaponHits  += Abilities[Ability.JudgementOfRighteousness].AttackTable.AnyHit; // Only add Judgements that hit
                    
                    modelThreat += Abilities[Ability.SealOfRighteousness].Threat * weaponHits;
                    modelDamage += Abilities[Ability.SealOfRighteousness].Damage * weaponHits;
                    modelCrits  += Abilities[Ability.SealOfRighteousness].CritPercentage * weaponHits;
                    break;
                //Seal of Truth Mode
                case "Seal of Truth":
                    weaponHits  += Abilities[Ability.JudgementOfTruth].AttackTable.AnyHit; // Only add Judgements that hit

                    modelThreat += Abilities[Ability.SealOfTruth].Threat * weaponHits;
                    modelDamage += Abilities[Ability.SealOfTruth].Damage * weaponHits;
                    modelCrits  += Abilities[Ability.SealOfTruth].CritPercentage * weaponHits;

                    // Censure ticks, one every 3 seconds
                    modelThreat += modelLength / 3f * Abilities[Ability.CensureTick].Threat;
                    modelDamage += modelLength / 3f * Abilities[Ability.CensureTick].Damage;
                    modelCrits += modelLength / 3f * Abilities[Ability.CensureTick].CritPercentage;
                    break;
            }

            #endregion

            float attackerHits = DefendTable.AnyHit * (modelLength / CalcOpts.BossAttackSpeed);

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
