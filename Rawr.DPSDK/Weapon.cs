using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DPSDK;

namespace Rawr.DK {
    public class Weapon {
        public float baseSpeed, baseDamage, hastedSpeed, mitigation, effectiveExpertise, chanceDodged, chanceParried, chanceMissed, DPS, damage;
        public bool twohander = false;
        public CharacterSlot hand;

        public Weapon (Item i, Stats stats, CalculationOptionsDPSDK calcOpts, BossOptions bossOpts, DeathKnightTalents talents, float expertise, CharacterSlot hand) {
            if (stats == null || calcOpts == null || !(hand == CharacterSlot.MainHand || hand == CharacterSlot.OffHand )) { return; }

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

            if (i == null) { return; }

            baseSpeed = i.Speed;
            baseDamage = (float)(i.MinDamage + i.MaxDamage) / 2f + stats.WeaponDamage;

            #region Attack Speed
            {
                hastedSpeed = baseSpeed / (1 + stats.PhysicalHaste);
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
                if (!twohander)
                    chanceMissed = StatConversion.WHITE_MISS_CHANCE_CAP_DW[bossOpts.Level - 85];
                chanceMissed -= stats.PhysicalHit;
                chanceMissed = Math.Max(chanceMissed, 0f);
            }
            #endregion

#if DEBUG
            if (chanceDodged < 0
                || chanceParried < 0
                || chanceMissed < 0)
                throw new Exception("Chance to hit out of range.");
#endif
            #region White Damage
            {
                // White damage per hit.  Basic white hits are use elsewhere.
                float baseDPS = baseDamage / baseSpeed;
                damage = (baseDPS + (stats.AttackPower / 14.0f)) * baseSpeed;
                DPS = damage / hastedSpeed;
                if (hand == CharacterSlot.OffHand)
                {
                    damage /= 2;
                    DPS /= 2;
                    if (talents.NervesOfColdSteel > 0)
                    {
                        damage *= 1f + (.25f * (talents.NervesOfColdSteel / 3f));
                        DPS *= 1f + (.25f * (talents.NervesOfColdSteel / 3f)); ;
                    }
                }
            }
            #endregion
        }
    }
}
