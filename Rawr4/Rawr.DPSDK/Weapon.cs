using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DPSDK;

namespace Rawr.DK {
    public class Weapon {
        public float baseSpeed, baseDamage, hastedSpeed, mitigation, effectiveExpertise, chanceDodged, DPS, damage;

        public Weapon (Item i, Stats stats, CalculationOptionsDPSDK calcOpts, DeathKnightTalents talents, float expertise) {
            if (stats == null || calcOpts == null) { return; }

            if (i == null) {
                i = new Item();
                i.Speed = 2.0f;
                i.MinDamage = 0;
                i.MaxDamage = 0;
            }

            effectiveExpertise = expertise;
            BossOptions bo = new BossOptions();

            float fightDuration = bo.BerserkTimer;

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
                chanceDodged = StatConversion.WHITE_DODGE_CHANCE_CAP[bo.Level-85];
                chanceDodged -= StatConversion.GetDodgeParryReducFromExpertise(effectiveExpertise);
                if (chanceDodged < 0f) { chanceDodged = 0f; }
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
