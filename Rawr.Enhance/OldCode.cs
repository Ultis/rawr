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
