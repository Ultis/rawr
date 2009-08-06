using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    class PetCalculations
    {
    
        Character character;
        CharacterCalculationsHunter calculatedStats;
        CalculationOptionsHunter options;
        Stats statsBuffs;
        Stats basegear;
        PetFamily family;
        public Stats petStats;

        double armorReduction;

        double specialAttackSpeed;
        double whiteAttackSpeed;

        public double ferociousInspirationUptime;
        private List<double> freqs = new List<double>();

        public PetCalculations(Character character, CharacterCalculationsHunter calculatedStats, CalculationOptionsHunter options, Stats statsBuffs, PetFamily petfamily, Stats gearStats)
        {
            this.character = character;
            this.calculatedStats = calculatedStats;
            this.options = options;
            this.statsBuffs = statsBuffs;
            this.basegear = gearStats;
            this.family = petfamily;

            int targetArmor = options.TargetArmor;
            this.armorReduction = 1f - StatConversion.GetArmorDamageReduction(character.Level, targetArmor,
                statsBuffs.ArmorPenetration, 0f, statsBuffs.ArmorPenetrationRating);

            calculatePetStats();
        }

        private void calculatePetStats()
        {
            //All calculations in this function and the functions it calls are based off 
            //Shandara's DPS Spreadsheet and will be updated from that sheet and should 
            //continue to be updated from that sheet until it is no longer actively 
            //maintained.
            //Shandara DPS Spreadhseet Info:
            //Version: 0.87c
            //Release date: April 28, 2009
            //Forum: http://elitistjerks.com/f74/t30710-wotlk_dps_spreadsheet/

            petStats = new Stats()
            {
                Agility = 113,
                Strength = 331,
                Stamina = 361,
                Intellect = 65,
                Spirit = 109
            };

            petStats.Strength += statsBuffs.Strength;
            petStats.Strength *= 1.0f + statsBuffs.BonusStrengthMultiplier;

            #region Hit
            petStats.PhysicalHit = 0.95f;
            petStats.PhysicalHit += (float)(calculatedStats.BasicStats.HitRating / HunterRatings.HIT_RATING_PER_PERCENT) / 100f;
            petStats.PhysicalHit += character.HunterTalents.FocusedAim * 0.01f;
            petStats.PhysicalHit -= (options.TargetLevel - 80) / 100;
            if (character.Race == CharacterRace.Draenei)
            {
                petStats.PhysicalHit += 0.01f;
            }

            if (petStats.PhysicalHit > 1)
                petStats.PhysicalHit = 1.0f;
            #endregion

            #region Physical Crit

            petStats.Agility += statsBuffs.Agility;
            petStats.Agility *= 1.0f + statsBuffs.BonusAgilityMultiplier;

            petStats.PhysicalCrit = 0.032f;
            petStats.PhysicalCrit += (float)(petStats.Agility / (62.5 * 100));
            petStats.PhysicalCrit += options.petSpidersBite * 0.03f;
            petStats.PhysicalCrit += (.02f * character.HunterTalents.Ferocity);
            petStats.PhysicalCrit += calculatedStats.BasicStats.BonusPetCritChance;

            if (options.TargetLevel == 83)
            {
                float critdepression = 0.0f;
                if ((options.TargetLevel * 5) - 400 > 10)
                    critdepression = 0.03f;
                critdepression += (options.TargetLevel - 80) * 0.006f;
                petStats.PhysicalCrit -= critdepression;
            }
            else
                petStats.PhysicalCrit -= (float)(400 - (5 * options.TargetLevel)) * 0.04f / 100;
            #endregion

            #region Attack Speed
            getWhiteAttackSpeed();
            getSpecialAttackSpeed();
            #endregion

            #region AP
            
            petStats.AttackPower = (petStats.Strength - 10) * 2;
            petStats.AttackPower += calculatedStats.BasicStats.RangedAttackPower * 0.22f;
            petStats.AttackPower *= 1.0f + 0.1f * character.HunterTalents.TrueshotAura;
            petStats.AttackPower *= 1.0f + getRabidProcEffect(petStats.PhysicalHit);
            petStats.AttackPower *= 1.0f + getSerenityEffect();
            petStats.AttackPower += getFuriousHowlEffect();
            petStats.AttackPower += getT8Bonus();

            if (options.Aspect == Aspect.Beast)
                petStats.AttackPower *= 1.1f;
            else if (options.Aspect == Aspect.Hawk)
            {
                petStats.AttackPower += (300 + (character.HunterTalents.AspectMastery * 90));
            }

            petStats.AttackPower += (float)Math.Floor(basegear.Stamina * (0.1 * character.HunterTalents.HunterVsWild));

            float callofthewildboost = 0.0f;
            if (options.petCallOfTheWild > 0)
            {
                callofthewildboost = (float)(20.0 / 300.0 * (1 - character.HunterTalents.Longevity * 0.1)) * 0.1f;
            }
            petStats.AttackPower += callofthewildboost;
            #endregion

            calculatedStats.PetStats = petStats;
        }

        private float getRabidProcEffect(double physhit)
        {
            if (options.petRabid > 0)
            {
                float frequency = 45.0f * (1.0f - character.HunterTalents.Longevity * 0.1f);
                float uptime = 20.0f / frequency;
                float targetdodge = (0.05f + (options.TargetLevel - 80) * 0.005f) - character.HunterTalents.AnimalHandler * 0.0125f;
                float chancetoapply = (float)(0.5f * (physhit - targetdodge));
                float avgattackstofull = 5 / chancetoapply;
                float timetofull = (float)(avgattackstofull * specialAttackSpeed);

                float result = 0;
                if (timetofull > 20)
                {
                    result = (float)(specialAttackSpeed * chancetoapply * 0.5f);
                }
                else
                {
                    result = (timetofull / 20) * 0.125f;
                    result += ((20 - timetofull) / 20) * 0.25f;
                }
                return result;
            }
            else
                return 0.0f;
        }

        private float getSerenityEffect()
        {
            if (family == PetFamily.Moth)
            {
                float cd = 60 * (1 - character.HunterTalents.Longevity * 0.1f);
                return 0.1f * (15 / cd);
            }
            else
                return 0.0f;
        }

        private int getFuriousHowlEffect()
        {
            if (family == PetFamily.Wolf)
            {
                return 158;
            }
            else
                return 0;
        }

        private float getCobraEffect()
        {
            if (character.HunterTalents.CobraStrikes > 0)
            {
                float procchance = character.HunterTalents.CobraStrikes * 0.2f;
                float twospecialtime = (float)specialAttackSpeed * 2f;
                float exp = 0.0f;
                if (calculatedStats.priorityRotation.containsShot(Shots.ArcaneShot))
                {
                    exp += 1.0f / 6.0f;
                }
                if (calculatedStats.priorityRotation.containsShot(Shots.SteadyShot))
                {
                    exp += 1.0f / 2.5f;
                }
                float critchance = 0.3566f + statsBuffs.BonusSteadyShotCrit + (character.HunterTalents.SurvivalInstincts * 0.02f);
                float basecrit = 1 - (1 - critchance * procchance);
                return (float)Math.Pow(basecrit , critchance) * twospecialtime;
            }
            else
                return 0.0f;
        }

        private static Dictionary<PetAttacks, PetSkill> skillLibrary = new Dictionary<PetAttacks, PetSkill>() 
        { 
            {PetAttacks.Growl, new PetSkill(5, 15, -1, 3)},
            {PetAttacks.Bite, new PetSkill(1.25, 25, -1, 0)},
            {PetAttacks.SonicBlast, new PetSkill(60, 0, -1, 2)},
            {PetAttacks.Wolverine, new PetSkill(0, 0, -1, 9)},
            {PetAttacks.Claw, new PetSkill(1.25, 25, -1, 0)},
            {PetAttacks.Swipe, new PetSkill(5, 20, -1, 1, 90, 126)},
            {PetAttacks.Thunderstomp, new PetSkill(10, 20, 1, 2, 236, 334)},
            {PetAttacks.Snatch, new PetSkill(60, 20, -1, 3)},
            {PetAttacks.Gore, new PetSkill(10, 20, -1, 1, 122, 164)},
            {PetAttacks.Screech, new PetSkill(10, 20, -1, 1, 85, 129)},
            {PetAttacks.Rabid, new PetSkill(45, 0, 1, 3)},
            {PetAttacks.Rake, new PetSkill(10, 20, -1, 1, 47, 67)},
            {PetAttacks.Frost, new PetSkill(10, 20, -1, 2, 128, 172)},
            {PetAttacks.Lava, new PetSkill(10, 20, -1, 2, 128, 172)},
            {PetAttacks.Pin, new PetSkill(60, 0, -1, 3)},
            {PetAttacks.Attitude, new PetSkill(120, 0, -1, 3)},
            {PetAttacks.Monstrous, new PetSkill(10, 20, -1, 1, 91, 123)},
            {PetAttacks.FireBreath, new PetSkill(10, 20, -1, 5)},
            {PetAttacks.Tendon, new PetSkill(20, 20, -1, 1, 49, 69)},
            {PetAttacks.Pummel, new PetSkill(30, 20, -1, 3)},
            {PetAttacks.Serenity, new PetSkill(60, 0, -1, 3)},
            {PetAttacks.Shock, new PetSkill(40, 0, -1, 2, 64, 86)},
            {PetAttacks.Savage, new PetSkill(60, 20, -1, 4)},
            {PetAttacks.Ravage, new PetSkill(40, 0, -1, 1, 85, 129)},
            {PetAttacks.Smack, new PetSkill(1.25, 25, -1, 0)},
            {PetAttacks.Stampede, new PetSkill(60, 0, -1, 3)},
            {PetAttacks.Scorpid, new PetSkill(10, 20, -1, 8)},
            {PetAttacks.Poison, new PetSkill(10, 20, -1, 6)},
            {PetAttacks.Web, new PetSkill(120, 0, -1, 3)},
            {PetAttacks.Spirit, new PetSkill(10, 20, -1, 7)},
            {PetAttacks.Spore, new PetSkill(10, 20, -1, 2)},
            {PetAttacks.Dust, new PetSkill(40, 20, -1, 1, 85, 129)},
            {PetAttacks.Shell, new PetSkill(180, 0, -1, 3)},
            {PetAttacks.Warp, new PetSkill(15, 0, -1, 3)},
            {PetAttacks.Sting, new PetSkill(6, 20, -1, 2, 64, 86)},
            {PetAttacks.LightningBreath, new PetSkill(10, 20, -1, 2, 80, 120)},
            {PetAttacks.Howl, new PetSkill(40, 20, -1, 3)},
            {PetAttacks.Acid, new PetSkill(10, 20, -1, 2, 124, 176)},
       };

        private void getSpecialAttackSpeed()
        {
            //Add an attack to the rotation if it isn't None and isn't already in the rotation

            if (options.PetPriority1 != PetAttacks.None)
                skills.Add(skillLibrary[options.PetPriority1]);

            if (options.PetPriority2 != PetAttacks.None && options.PetPriority2 != options.PetPriority1)
                skills.Add(skillLibrary[options.PetPriority2]);

            if (options.PetPriority3 != PetAttacks.None && options.PetPriority3 != options.PetPriority1 && options.PetPriority3 != options.PetPriority2)
                skills.Add(skillLibrary[options.PetPriority3]);

            if (options.PetPriority4 != PetAttacks.None && options.PetPriority4 != options.PetPriority1 && options.PetPriority4 != options.PetPriority2 && options.PetPriority4 != options.PetPriority3)
                skills.Add(skillLibrary[options.PetPriority4]);
            
            int i = 0;
            double L = 1;
            double N = 0;
            double M = 0;
            double R = getFocus();
            double Q = 0;

            foreach (PetSkill S in skills)
            {
                
                double actualCD = S.CD;
                if (S.Type > 0)
                    actualCD *= (1 - (character.HunterTalents.Longevity * 0.1f));

                double K = actualCD - (actualCD % 1.25f);
                if (actualCD % 1.25 == 0)
                    K += 1.25;

                if (i > 0)
                    L = L - N;

                if (actualCD >= 30)
                    M = 0;
                else
                {
                    if (K > 0)
                        M = 1.25 / K;
                    else
                        M = 0;
                }
                N = 1 - L + M;

                double O = 0;
                if (S.Talent != 0)
                {
                    if (N > 0)
                        O = 1.25 / N;
                    else
                    {
                        if (actualCD >= 30)
                            O = actualCD;
                        else
                            O = 0;
                    }
                }

                if (i > 0)
                    R = R - Q;

                if (O > 0)
                    Q = S.Cost / O;
                else
                    Q = 0;

                if (options.petOwlsFocus > 0)
                {
                    double OwlReduction = 1/(1 / (options.petOwlsFocus * 0.15)) + 1;
                    Q *= (1-OwlReduction);
                }

                double Freq = 0;

                if (R > Q)
                    Freq = O;
                else
                {
                    if (Q > 0)
                        Freq = O * (Q / R);
                    else
                        Freq = 0;
                }

                if (Freq > 0)
                {
                    specialAttackSpeed += 1 / Freq;
                }
                freqs.Add(Freq);
            }
            specialAttackSpeed = 1/specialAttackSpeed;
        }

        private List<PetSkill> skills = new List<PetSkill>();
        
        private double getSpecialDPS(double bonus, double critmiss, double damageFromAP)
        {
            if (skills.Count == 0)
                return 0;
            else
                return getSpecialDPSFromSkills(bonus, critmiss, damageFromAP);
        }

        private double getSpecialDPSFromSkills(double bonus, double critmiss, double damageFromAP)
        {
            double dps = 0;
            int i = 0;

            foreach (PetSkill S in skills)
            {
                switch (skills[i].Type)
                {
                    case 0:
                        dps += getFocusDumpDPS(freqs[i], bonus, critmiss, damageFromAP);
                        break;
                    case 1:
                        dps += getPhysicalSpecialDPS(freqs[i], bonus, critmiss, damageFromAP, S.Min, S.Max);
                        break;
                    case 2:
                        dps += getSpellSpecialDPS(freqs[i], bonus, S.Min, S.Max);
                        break;
                    case 3:
                        break; //Non-damaging, Calculations still done because we need the info for the next skill in the rotation
                    case 4:
                        dps += getSavageRendDPS(freqs[i], bonus, damageFromAP);
                        break;
                    case 5:
                        dps += getFireBreathDPS(freqs[i], bonus);
                        break;
                    case 6:
                        dps += getPoisonSpitDps(freqs[i], bonus);
                        break;
                    case 7: 
                        dps += getSpiritSmackDPS(freqs[i], bonus);
                        break;
                    case 8:
                        dps += getScorpidPoionDPS(freqs[i], bonus);
                        break;
                }
                i++;
            }

            return dps;
        }

        private double getScorpidPoionDPS(double freq, double bonus)
        {
            double avgDmg = 110 + calculatedStats.BasicStats.AttackPower * 0.0429;
            double critResist = (petStats.PhysicalCrit * 1.5 + 1) * 0.935;
            double dmg = avgDmg * bonus * critResist;
            return (dmg / 10) * freq;

        }

        private double getSpiritSmackDPS(double freq, double bonus)
        {
            double avgDmg = 120 + calculatedStats.BasicStats.AttackPower * 0.0429;
            double critResist = (petStats.PhysicalCrit * 1.5 + 1) * 0.935;
            double dmg = avgDmg * bonus * critResist;
            double dotDmg = 57 + calculatedStats.BasicStats.AttackPower * 0.0429;
            dotDmg *= bonus * critResist;
            return (dmg + dotDmg) / freq;
        }

        private double getPoisonSpitDps(double Freq, double bonus)
        {
            double avgDmg = 120 + calculatedStats.BasicStats.AttackPower * 0.0429;
            double critResist = (petStats.PhysicalCrit * 1.5 + 1) * 0.935;
            double dmg = avgDmg * bonus * critResist;
            return (dmg / 10) / Freq;
        }

        private double getFireBreathDPS(double Freq, double bonus)
        {
            double dotWithAP = 50 + calculatedStats.BasicStats.AttackPower * 0.0429;
            double initialHit = 50 + calculatedStats.BasicStats.AttackPower * 0.0429;
            double critResist = (petStats.PhysicalCrit * 1.5 + 1) * 0.935;
            dotWithAP *= bonus * critResist;
            return (initialHit + dotWithAP) / Freq;
        }

        private double getSavageRendDPS(double freq, double bonus, double damageFromAP)
        {
            double dmgPer = ((59 + 83) / 2) + damageFromAP;
            dmgPer *= bonus;
            double armorMit = 1 - (options.TargetArmor / (options.TargetArmor - 22167.5 + (467.5 * options.TargetLevel)));
            dmgPer *= armorMit;
            double bleedDmg = 24 + damageFromAP;
            bleedDmg *= bonus;
            bleedDmg *= armorMit;
            double totalDmg = dmgPer + (bleedDmg * 3);
            return totalDmg / freq;

        }

        private double getFocusDumpDPS(double freq, double bonus, double critmiss, double damageFromAP)
        {
            double avgPer = ((168 - 118) / 2) + 118;
            double avgDamage = avgPer + damageFromAP;
            double armorMit = options.TargetArmor / (options.TargetArmor - 22167.5 + (467.5 * options.TargetLevel));
            double totalDamage = avgDamage * bonus * (1 - armorMit) * critmiss;
            return totalDamage / freq;
        }

        private double getSpellSpecialDPS(double freq, double bonus,int min, int max)
        {
            double avgPer = ((max - min) / 2) + min;
            double avgDamage = avgPer + (petStats.AttackPower * 0.0429);
            double critResist = (petStats.PhysicalCrit * 1.5 + 1) * 0.935;
            double totalDamage = avgDamage * critResist * bonus;
            return totalDamage / freq;
        }

        private double getPhysicalSpecialDPS(double freq, double bonus, double critmiss, double damageFromAP, int min, int max)
        {
            double avgPer = ((max - min) / 2) + max;
            double avgDamage = avgPer + damageFromAP;
            double armorMit = options.TargetArmor / (options.TargetArmor - 22167.5 + (467.5 * options.TargetLevel));
            double totalDamage = avgDamage * bonus * (1 - armorMit) * critmiss;
            return totalDamage / freq;
        }

        private double getFerociousInspirationEffect()
        {
            ferociousInspirationUptime = 1.0 - Math.Pow(1.0 - calculatedStats.PetStats.PhysicalCrit, 10.0 / whiteAttackSpeed + 10.0 / specialAttackSpeed);
            double benefit = 0.01f * character.HunterTalents.FerociousInspiration;
            return ferociousInspirationUptime * benefit;
        }

        private void getWhiteAttackSpeed()
        {
            double spdMod = 1.0f + character.HunterTalents.SerpentsSwiftness * 0.04;
            spdMod *= 1.3f; //Net effect of one heroism
            spdMod *= 1.0f + options.petCobraReflexes * 0.15f;
            spdMod *= 1.0f + (statsBuffs.HasteRating / HunterRatings.HASTE_RATING_PER_PERCENT);

            double spd = 2.0 / spdMod;
            spd /= 1 + getFrenzyEffect(spd);
            whiteAttackSpeed = spd;
        }

        private double getFrenzyEffect(double spd)
        {
            double chancetocause = petStats.CritRating * (1 / spd / ((1 / spd) + (1 / specialAttackSpeed)));
            chancetocause += petStats.CritRating * (1 / spd / ((1 / spd) + (1 / specialAttackSpeed)));
            chancetocause *= character.HunterTalents.Frenzy * 0.2;

            double effectivespd = spd / 1.3f;
            double numattacks = Math.Floor(8 / effectivespd) + (8 / specialAttackSpeed);
            double chancetonotbein = Math.Pow(1 - chancetocause, numattacks);
            return (1 - chancetonotbein) * 0.3f;
        }

        private double getBestialWrathEffect()
        {
            if (character.HunterTalents.BestialWrath >= 1)
            {
                double cd = 120;
                cd *= 1.0f - character.HunterTalents.Longevity * 0.1f;
                double uptime = 18 / cd;
                return uptime * 0.5f;
            }
            else
                return 0.0f;
            
        }

        private double getMonstrousBiteEffect()
        {
            if (family == PetFamily.Devilsaur)
            {
                double tagetdodge = (0.05 + (options.TargetLevel - 80) * 0.005) - character.HunterTalents.AnimalHandler * 0.0125;
                double chancetoapply = petStats.PhysicalHit - tagetdodge;
                double cd = 10 * (1 - character.HunterTalents.Longevity * 0.1f);
                double chancestoapply = Math.Floor(11 / cd);
                double falloffchance = 1 - chancetoapply;
                double avgTimToInc = 10f;
                double timeToMax = avgTimToInc + (12 * falloffchance);
                double[] timeToReach = new double[4];
                double[] chanceToMax = new double[4];
                double[] timeSpent = new double[4];

                for (int i = 0; i < 4; i++)
                    timeToReach[i] = (double)10f * i;

                chanceToMax[0] = 0.0f;
                chanceToMax[1] = falloffchance;
                chanceToMax[2] = (1 - chanceToMax[1]) * falloffchance;
                chanceToMax[3] = (1 - chanceToMax[2]) * falloffchance;

                timeSpent[0] = timeToReach[1];
                timeSpent[1] = timeToMax * (1 - chanceToMax[0]);
                timeSpent[2] = timeToMax * (1 - chanceToMax[0] + chanceToMax[1]);
                timeSpent[3] = timeToMax * (1 - chanceToMax[0] + chanceToMax[1] + chanceToMax[2]);

                double totaltime = 0.0f;

                for (int i = 0; i < 4; i++)
                {
                    totaltime += timeSpent[i];
                }

                double total = 0.0f;

                for (int i = 0; i < 4; i++)
                {
                    total += (0.03 * i) * (timeSpent[i] / totaltime);
                }

                return total;
            }
            else
                return 0.0f;
        }

        private double getSavageRendEffect()
        {
            if (family == PetFamily.Raptor)
            {
                double timeBetweenProcs = 60 / petStats.PhysicalCrit;
                double uptime = 30 / timeBetweenProcs;
                return uptime * 0.1f;
            }
            else
                return 0.0f;
        }

        private double getFeedingFrenzyEffect()
        {
            return (options.petFeedingFrenzy * 0.08) * 0.35f;
        }

        private double getGlancingBlows()
        {
            int diff = 400 - (options.TargetLevel * 5);
            if (diff < 0)
                diff = 0;
            double chance = 0.0f;
            if (diff >= 15)
                chance = 0.25;
            else
                chance = (diff * 0.01) + 0.1f;

            double lowEnd = 1.3 - 0.05 * diff;
            if (lowEnd > 0.91)
                lowEnd = 0.91;
            double hiEnd = 1.2 * 0.03 * diff;
            if (hiEnd > 0.99)
                hiEnd = 0.99;
            double avgDamage = lowEnd + hiEnd / 2;
            return 1 - (chance * (1 - avgDamage));
        }

        public float getDPS()
        {
            double avgPetDmg = ((78 - 52) / 2) + 52;
            double damageFromAP = Math.Floor(petStats.AttackPower * 0.07);
            double targetDodge = 0.05 + (options.TargetLevel - 80) * 0.005;
            double critMissAdjustment = (2 * petStats.PhysicalCrit) + (petStats.PhysicalHit - petStats.PhysicalCrit - targetDodge);
            double damageAdjustment = 1.0f + character.HunterTalents.UnleashedFury * 0.03;
            damageAdjustment *= 1.0f + options.petSharkAttack * 0.03;
            damageAdjustment *= 1.0f + character.HunterTalents.KindredSpirits * 0.04;
            damageAdjustment *= 1.0f + getT7Bonus();
            damageAdjustment *= 1.0f + options.petSpikedCollar * 0.03;
            damageAdjustment *= 1.0f + getFerociousInspirationEffect();
            damageAdjustment *= 1.0f + getBestialWrathEffect();
            damageAdjustment *= 1.0f + getMonstrousBiteEffect();
            damageAdjustment *= 1.0f + getSavageRendEffect();
            damageAdjustment *= 1.0f + getFeedingFrenzyEffect();
            damageAdjustment *= 1.25f; //Happy pet bonus
            damageAdjustment *= 1.05f; //All families do +5% damage

            ferociousInspirationUptime = 1.0 - Math.Pow(1.0 - calculatedStats.PetStats.PhysicalCrit, 10.0 / whiteAttackSpeed + 10.0 / specialAttackSpeed);

            double totalDamageAdjustment = critMissAdjustment * damageAdjustment;
            double cobraAdjusment = 1 - (options.petCobraReflexes * 0.075f);
            double targeArmorReduction = options.TargetArmor / (options.TargetArmor - 22167.5 + (467.5 * options.TargetLevel));
            double glancingBlows = getGlancingBlows();

            double totalDamage = (avgPetDmg + damageFromAP) * totalDamageAdjustment * cobraAdjusment * (1 - targeArmorReduction) * glancingBlows;

            calculatedStats.PetBaseDPS = totalDamage / whiteAttackSpeed;
            calculatedStats.PetSpecialDPS = getSpecialDPS(damageAdjustment, critMissAdjustment, damageFromAP);
            
            return (float)(calculatedStats.PetBaseDPS + calculatedStats.PetSpecialDPS);
        }

        public double getT7Bonus()
        {
            int i = 0;

            if (character._head != null && character._chest != null && character._legs != null && character._shoulders != null && character._hands != null)
            {

                if (character._head.Contains("Cryptstalker Battlegear"))
                    i++;

                if (character._chest.Contains("Cryptstalker Battlegear"))
                    i++;

                if (character._legs.Contains("Cryptstalker Battlegear"))
                    i++;

                if (character._shoulders.Contains("Cryptstalker Battlegear"))
                    i++;

                if (character._hands.Contains("Cryptstalker Battlegear"))
                    i++;

                if (i >= 2)
                    return 0.05f;
                else
                    return 0.0f;
            }
            else
                return 0.0f;
        }

        public int getT8Bonus()
        {
            int i = 0;

            if (character._head != null && character._chest != null && character._legs != null && character._shoulders != null && character._hands != null)
            {

                if (character._head.Contains("Scourgestalker Battlegear"))
                    i++;

                if (character._chest.Contains("Scourgestalker Battlegear"))
                    i++;

                if (character._legs.Contains("Scourgestalker Battlegear"))
                    i++;

                if (character._shoulders.Contains("Scourgestalker Battlegear"))
                    i++;

                if (character._hands.Contains("Scourgestalker Battlegear"))
                    i++;

                //only procs if steady shot is being used
                if (calculatedStats.priorityRotation.containsShot(Shots.SteadyShot) && i >= 4)
                    //15s duration of 600AP 45s cooldown = ~33.33%uptime = 200AP average for hunter = 44AP average for pet
                    return 44;
                else
                    return 0;
            }
            else
                return 0;
        }

        public double getFocus()
        {
            double focus = (24.0 + 12.0 * character.HunterTalents.BestialDiscipline) / 4.0;
            double shotsPerSecond = 1.0 / calculatedStats.BaseAttackSpeed + 1.0 / 1.5;
            focus += shotsPerSecond * calculatedStats.BasicStats.PhysicalCrit * character.HunterTalents.GoForTheThroat * 25.0;
            
            return focus;
        }
    }
}
