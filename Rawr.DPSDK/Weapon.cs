using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DPSDK;

namespace Rawr.DK {
    public class Weapon {
        public float baseSpeed, baseDamage, hastedSpeed, mitigation, effectiveExpertise, chanceDodged, chanceParried, chanceMissed, DPS, damage;
        public bool twohander = false;

        public Weapon (Item i, Stats stats, CalculationOptionsDPSDK calcOpts, BossOptions bossOpts, DeathKnightTalents talents, float expertise) {
            if (stats == null || calcOpts == null) { return; }

            if (i == null) {
                i = new Item();
                i.Speed = 2.0f;
                i.MinDamage = 0;
                i.MaxDamage = 0;
            }
            else
            {
                twohander = (i.Slot == ItemSlot.TwoHand);
            }

            effectiveExpertise = expertise;

            float fightDuration = bossOpts.BerserkTimer;

            if (i == null) { return; }

            baseSpeed = i.Speed;
            baseDamage = (float)(i.MinDamage + i.MaxDamage) / 2f + stats.WeaponDamage;

            #region Attack Speed
            {
                hastedSpeed = baseSpeed / ((1f + (StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.DeathKnight))) * (1 + stats.PhysicalHaste));
                hastedSpeed /= 1f + 0.05f * (float)talents.ImprovedIcyTalons;
            }
            #endregion

            #region Dodge
            {
                chanceDodged = StatConversion.WHITE_DODGE_CHANCE_CAP[bossOpts.Level - 85];
                chanceDodged -= StatConversion.GetDodgeParryReducFromExpertise(effectiveExpertise);
                chanceDodged = Math.Max(chanceDodged, 0f);
            }
            #endregion

            #region Parry
            {
                chanceParried = StatConversion.WHITE_PARRY_CHANCE_CAP[bossOpts.Level - 85];
                chanceParried -= StatConversion.GetDodgeParryReducFromExpertise(effectiveExpertise);
                chanceParried = Math.Max(chanceParried, 0f);
            }
            #endregion

            #region Miss
            {
                chanceMissed = StatConversion.WHITE_MISS_CHANCE_CAP[bossOpts.Level - 85];
                chanceMissed -= stats.PhysicalHit;
                chanceMissed = Math.Max(chanceMissed, 0f);
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
