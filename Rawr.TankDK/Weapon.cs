using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK {
    public class Weapon {
        public float baseSpeed, baseDamage, hastedSpeed, mitigation, effectiveExpertise, chanceDodged, DPS, damage;
        public Weapon (Item i, Stats stats, CalculationOptionsTankDK calcOpts, float expertise) {
            if (stats == null || calcOpts == null || calcOpts.talents == null) { return; }

            if (i == null) {
                i = new Item();
                i.Speed = 2f;
                i.MinDamage = 0;
                i.MaxDamage = 0;
            }

            effectiveExpertise = expertise;
            float fightDuration = calcOpts.FightLength * 60f;

            if (i == null) { return; }

            baseSpeed = i.Speed;
            baseDamage = (float)(i.MinDamage + i.MaxDamage) / 2f + stats.WeaponDamage;


            #region Attack Speed
            {
                hastedSpeed = baseSpeed / (1f + (StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.DeathKnight)) + stats.PhysicalHaste);
                hastedSpeed /= 1f + 0.05f * (float)calcOpts.talents.ImprovedIcyTalons;

                if (calcOpts.Bloodlust) {
                    //float bloodlustUptime = (calcOpts.Bloodlust * 40f);

                    //if (bloodlustUptime > fightDuration) bloodlustUptime = 1f;
                    //else bloodlustUptime /= fightDuration;

                    float numLust = fightDuration % 300f;  // bloodlust changed in 3.0, can only have one every 5 minutes.
                    float fullLustDur = (numLust - 1) * 300f + 40f;
                    if (fightDuration < fullLustDur) // if the last lust doesn't go its full duration
                    {
                        float lastLustFraction = (fullLustDur - fightDuration) / 40f;
                        numLust -= 1f;
                        numLust += lastLustFraction;
                    }

                    float bloodlustUptime = (numLust * 40f) / fightDuration;

                    hastedSpeed /= 1f + (0.3f * bloodlustUptime);
                }
            }
            #endregion

            #region Dodge
            {
                chanceDodged = StatConversion.WHITE_DODGE_CHANCE_CAP[calcOpts.TargetLevel-80];
                chanceDodged -= effectiveExpertise / 400f;
                if (chanceDodged < 0f) chanceDodged = 0f;
            }
            #endregion

            #region White Damage
            {
                // White damage per hit.  Basic white hits are use elsewhere.
                damage = baseDamage + (stats.AttackPower / 14f) * baseSpeed;
                DPS = 0f;
                if (hastedSpeed > 0) { DPS = damage / hastedSpeed; }
            }
            #endregion
        }
    }
}
