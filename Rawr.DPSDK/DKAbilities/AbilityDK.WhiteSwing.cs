using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK 
{
    // Changed this from average between MH and OH, to each swing for each weapon.
    class AbilityDK_WhiteSwing : AbilityDK_Base
    {
        public static float glancechance = .23f;
        public bool m_bIsOffHand = false;

        public AbilityDK_WhiteSwing(CombatState CS)
        {
            this.CState = CS;
            this.szName = "White Swing";
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1;
            this.bTriggersGCD = false;
            this.AbilityIndex = (int)DKability.White;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.RunicPower = -1 * ((int)((10f * CS.m_Talents.MightOfTheFrozenWastes * .15f) + ((CS.m_Talents.ScentOfBlood * .15f) * (CS.m_Stats.Parry + CS.m_Stats.Dodge)))); // Should be 1.5 per point.
            this.wMH = CS.MH;
            this.wOH = null;
        }

        public void UpdateCombatState(CombatState CS, bool IsOffHand)
        {
            base.UpdateCombatState(CS);
            this.RunicPower = -1 * ((int)((10f * CS.m_Talents.MightOfTheFrozenWastes * .15f) + ((CS.m_Talents.ScentOfBlood * .15f) * (CS.m_Stats.Parry + CS.m_Stats.Dodge)))); // Should be 1.5 per point.
            m_bIsOffHand = IsOffHand;
            if (m_bIsOffHand)
            {
                this.wMH = CS.OH;
                this.wOH = null;
                this.AbilityIndex = (int)DKability.WhiteOH;
                this.szName += " OH";
            }
        }

        public override float DamageMultiplierModifer
        {
            get
            {
                float BCBChance = (CState.m_Talents.BloodCakedBlade * .1f);
                float BCBDamMult = .25f + (.125f * CState.m_uDiseaseCount);
                float DMM = ((1 + BCBChance * BCBDamMult) * (1 + CState.m_Stats.BonusWhiteDamageMultiplier) * (1 + CState.m_Stats.BonusFrostWeaponDamage)  - 1);
                return base.DamageMultiplierModifer + DMM;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }

        override public float GetTotalDamage()
        {
            if (null == this.wMH)
            {
                return 0;
            }
            // Start w/ getting the base damage values.
            float iDamage = this.GetTickDamage();
            float reducedHitChance = HitChance;
            if ((glancechance + CritChance + HitChance) > 1f)
                reducedHitChance = HitChance - (glancechance + CritChance + HitChance - 1);

            // Factor in max value for Crit, Hit, Glancing
            float glancedamage = iDamage * .5f * glancechance;
            float critdamage = iDamage * CritChance * 2;
            float hitdamage = iDamage * reducedHitChance;
            iDamage = critdamage + hitdamage + glancedamage;
            if (wMH.twohander)
                iDamage *= (1f + .04f * CState.m_Talents.MightOfTheFrozenWastes);
            return iDamage;
        }

        override public uint uBaseDamage
        {
            get
            {
                uint WDam = 0;
                // Handle non-weapon based effects:
                if (null != this.wMH)
                {
                    WDam = (uint)(this.wMH.damage * this.fWeaponDamageModifier);
                }
                return (uint)WDam;
            }
            set
            {
                // Setup so that we can just set a base damage w/o having to 
                // manually set Min & Max all the time.
                uMaxDamage = uMinDamage = value;
            }
        }

        override public float GetDPS()
        {
            float dps = (float)TotalDamage / combinedSwingTime;
            return dps;
        }

        override public float GetTPS()
        {
            float dps = (float)TotalThreat / combinedSwingTime;
            return dps;
        }

        public override float HitChance
        {
            get
            {
                // Determine Miss Chance
                float fMissChance = wMH.chanceMissed;
                float ChanceToHit = 1 - Math.Max(0, fMissChance);
                ChanceToHit -= glancechance;
                // Determine Dodge chance
                float fDodgeChanceForTarget = wMH.chanceDodged;
                // Determine Parry Chance  (Only for Tank... Since only they should be in front of the target.)
                float fParryChanceForTarget = wMH.chanceParried;
                ChanceToHit -= Math.Max(0, fDodgeChanceForTarget);
                if (CState != null && !CState.m_bAttackingFromBehind)
                    ChanceToHit -= Math.Max(0, fParryChanceForTarget);
#if DEBUG
                if (ChanceToHit < 0 || ChanceToHit > 1)
                    throw new Exception("Chance to hit out of range.");
#endif
                return Math.Max(0, Math.Min(1,ChanceToHit));
            }
        }

        public override float MissChance
        {
            get
            {
                return wMH.chanceMissed;
            }
        }

        private float combinedSwingTime
        {
            get
            {
                if (wMH != null)
                {
                    return wMH.hastedSpeed;
                }
                else
                    // Just throw in some default bare-hand weapon time as the default.
                    return 2.0f;
            }
        }
    }
}
