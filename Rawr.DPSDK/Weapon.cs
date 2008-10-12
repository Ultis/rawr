using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.DPSDK
{
    public class Weapon
    {
        public float baseSpeed, baseDamage, hastedSpeed, mitigation, effectiveExpertise, chanceDodged, DPS, damage;

        public Weapon (Item i, Stats stats, CalculationOptionsDPSDK calcOpts, float expertise)
        {
            effectiveExpertise = expertise;
            float fightDuration = calcOpts.FightLength * 60;

            if (i == null) return;

            baseSpeed = i.Speed;
            baseDamage = (i.MinDamage + i.MaxDamage) / 2f + stats.WeaponDamage;


            #region Attack Speed
            {
                hastedSpeed = baseSpeed / (1f + (stats.HasteRating / 1576f));

                if (calcOpts.Bloodlust)
                {
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
                chanceDodged = .065f;
                chanceDodged -= effectiveExpertise * .0025f;
                if (chanceDodged < 0f) chanceDodged = 0f;
            }
            #endregion

            #region White Damage
            {
                // White damage per hit.  Basic white hits are use elsewhere.
                damage = baseDamage + (stats.AttackPower / 14.0f) * baseSpeed;
                DPS = damage / hastedSpeed;
            }
            #endregion
        }
    }
}
