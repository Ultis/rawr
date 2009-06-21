#region Old Enchants
/*                   
            if (stats.MongooseProc > 0 | stats.BerserkingProc > 0)
            {
                if (character.MainHandEnchant != null)
                {
                    float whiteAttacksPerSecond = swingsPerSMHMelee * (1f - chanceWhiteMiss - chanceDodge);
                    if (character.MainHandEnchant.Id == 2673) // Mongoose Enchant
                    {
                        float timeBetweenMongooseProcs = 60f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float mongooseUptime = 15f / timeBetweenMongooseProcs;
                        float mongooseAgility = 120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier);
                        chanceCrit = Math.Min(0.75f, chanceCrit + StatConversion.GetCritFromAgility(mongooseAgility, character.Class));
                        attackPower += mongooseAgility * (1 + stats.BonusAttackPowerMultiplier);
                        basecs.HastedMHSpeed /= 1f + (0.02f * mongooseUptime);
                    }
                    if (character.MainHandEnchant.Id == 3789) // Berserker Enchant
                    {
                        float timeBetweenBerserkingProcs = 45f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float berserkingUptime = 15f / timeBetweenBerserkingProcs;
                        attackPower += 400f * berserkingUptime * (1 + stats.BonusAttackPowerMultiplier);
                    }
                }
                if (character.OffHandEnchant != null && character.ShamanTalents.DualWield == 1)
                {
                    float whiteAttacksPerSecond = swingsPerSOHMelee * (1f - chanceWhiteMiss - chanceDodge);
                    if (character.OffHandEnchant.Id == 2673)  // Mongoose Enchant
                    {
                        float timeBetweenMongooseProcs = 60f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float mongooseUptime = 15f / timeBetweenMongooseProcs;
                        float mongooseAgility = 120f * mongooseUptime * (1 + stats.BonusAgilityMultiplier);
                        chanceCrit = Math.Min(0.75f, chanceCrit + StatConversion.GetCritFromAgility(mongooseAgility, character.Class));
                        attackPower += mongooseAgility * (1 + stats.BonusAttackPowerMultiplier);
                        basecs.HastedOHSpeed /= 1f + (0.02f * mongooseUptime);
                    }
                    if (character.OffHandEnchant.Id == 3789) // Berserker Enchant
                    {
                        float timeBetweenBerserkingProcs = 45f / (whiteAttacksPerSecond + yellowAttacksPerSecond);
                        float berserkingUptime = 15f / timeBetweenBerserkingProcs;
                        attackPower += 400f * berserkingUptime * (1 + stats.BonusAttackPowerMultiplier);
                    }
                }
            }
 */
#endregion

        #region Get Race Stats
/*
        private Stats GetRaceStats(Character character)
        {
            Stats statsRace = new Stats()
            {
                Mana = 4116f,
                AttackPower = 140f,
                SpellCrit = 0.0220f, 
                PhysicalCrit = 0.0292f
            };

            switch (character.Race)
            {
                case Character.CharacterRace.Draenei:
                    statsRace.Health = 6305f;
                    statsRace.Strength = 121f;
                    statsRace.Agility = 71f;
                    statsRace.Stamina = 135f;
                    statsRace.Intellect = 129f;
                    statsRace.Spirit = 145f;
                    break;

                case Character.CharacterRace.Tauren:
                    statsRace.Health = 6313f;
                    statsRace.BonusStaminaMultiplier = .05f;
                    statsRace.Strength = 125f;
                    statsRace.Agility = 69f;
                    statsRace.Stamina = 138f;
                    statsRace.Intellect = 123f;
                    statsRace.Spirit = 145f;
                    break;

                case Character.CharacterRace.Orc:
                    statsRace.Health = 6305f;
                    statsRace.Strength = 123f;
                    statsRace.Agility = 71f;
                    statsRace.Stamina = 138f;
                    statsRace.Intellect = 125f;
                    statsRace.Spirit = 146f;
                    break;

                case Character.CharacterRace.Troll:
                    statsRace.Health = 6305f;
                    statsRace.Strength = 121f;
                    statsRace.Agility = 76f;
                    statsRace.Stamina = 137f;
                    statsRace.Intellect = 124f;
                    statsRace.Spirit = 144f;
                    break;
            }
            return statsRace;
        }
*/
        #endregion

/*

        private Stats ApplyTalents(Character character, Stats stats, Stats gear) // also includes basic class benefits
        {
            if (gear != null)
            {
                int AK = character.ShamanTalents.AncestralKnowledge;
                float intBase = (float)Math.Floor((float)(stats.Intellect * (1 + stats.BonusIntellectMultiplier) * (1 + .02f * AK))); // added fudge factor because apparently Visual Studio can't multiply 125 * 1.04 to get 130.
                float intBonus = (float)Math.Floor((float)(gear.Intellect * (1 + gear.BonusIntellectMultiplier) * (1 + .02f * AK)));
                stats += gear;
                stats.Intellect = (float)Math.Floor((float)(intBase + intBonus));
            }
            
            stats.Mana += 15f * stats.Intellect;
            stats.Health += 10f * stats.Stamina;
            stats.Expertise += 3 * character.ShamanTalents.UnleashedRage;
            
            int MQ = character.ShamanTalents.MentalQuickness;
            stats.AttackPower += AddAPFromStrAgiInt(character, stats.Strength, stats.Agility, stats.Intellect); 
            stats.AttackPower = (float)Math.Floor((float)(stats.AttackPower * (1f + stats.BonusAttackPowerMultiplier)));
            stats.SpellPower = (float)Math.Floor((float)(stats.SpellPower + (stats.AttackPower * .1f * MQ * (1f + stats.BonusSpellPowerMultiplier))));
            return stats;
        }
*/

// old WF model - aka Flat Windfury Society
/* 
float windfuryTimeToFirstHit = hastedMHSpeed - (3 % hastedMHSpeed);
//later -- //windfuryTimeToFirstHit = hasted
wfProcsPerSecond = 1f / (3f + windfuryTimeToFirstHit + ((avgHitsToProcWF - 1) * hitsThatProcWFPerS));
*/

/*
CombatStats.cs
        private void CalculateAbilities()
        {
            _gcd = Math.Max(1.0f, 1.5f * (1f - StatConversion.GetSpellHasteFromRating(_stats.HasteRating)));
            int deadTimes = 0;
            string name = "";
            for (float timeElapsed = 0f; timeElapsed < FightLength; timeElapsed += _gcd)
            {
                bool abilityUsed = false;
                foreach (Ability ability in abilities)
                {
                    if (ability.OffCooldown(timeElapsed))
                    {
                        ability.AddUse(timeElapsed, _calcOpts.AverageLag / 1000f);
                        abilityUsed = true;
                        name = ability.Name;
                        break;
                    }
                }
                if (!abilityUsed)
                {
                    deadTimes++;
                    name = "Deadtime";
                }
//                System.Diagnostics.Debug.Print("Time: {0} - FS {1}, {2} - LB {3}, {4} - SS {5}, {6} - ES {7}, {8} - LL {9}, {10} - LS {11}, {12} - MT {13}, {14} - used {15}",
//                    timeElapsed, 
//                    abilities[0].Uses, abilities[0].CooldownOver,
//                    abilities[1].Uses, abilities[1].CooldownOver,
//                    abilities[2].Uses, abilities[2].CooldownOver,
//                    abilities[3].Uses, abilities[3].CooldownOver,
//                    abilities[4].Uses, abilities[4].CooldownOver,
//                    abilities[5].Uses, abilities[5].CooldownOver,
//                    abilities[6].Uses, abilities[6].CooldownOver, name); 
            }
            // at this stage abilities now contains the number of procs per fight for each ability.
        }
*/
