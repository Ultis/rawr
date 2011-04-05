using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK 
{
    class AbilityDK_WhiteSwing : AbilityDK_Base
    {
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
            this.RunicPower = (int)((10f * CS.m_Talents.MightOfTheFrozenWastes * .15f) + ((CS.m_Talents.ScentOfBlood * .15f) * (CS.m_Stats.Parry + CS.m_Stats.Dodge))); // Should be 1.5 per point.
            this.wMH = CS.MH;
            this.wOH = CS.OH;
        }

        public override float DamageMultiplierModifer
        {
            get
            {
                float BCBChance = (CState.m_Talents.BloodCakedBlade * .1f);
                float BCBDamMult = .25f + (.125f * CState.m_uDiseaseCount);
                float DMM = ((1 + BCBChance) * (1 + BCBDamMult) * (1 + CState.m_Stats.BonusWhiteDamageMultiplier) * (1 + CState.m_Stats.BonusFrostWeaponDamage)  - 1);
                return base.DamageMultiplierModifer + DMM;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }

        override public float GetTotalDamage()
        {
            if (null == this.wMH && null == this.wOH)
            {
                return 0;
            }
            // Start w/ getting the base damage values.
            float iDamage = this.GetTickDamage();

            // Factor in max value for Crit, Hit, Glancing
            float glancechance = .24f;
            float misschance = 1 - HitChance;
            iDamage = (float)iDamage * (1f + (float)Math.Min(CritChance, 1f - (glancechance + misschance))) * (float)Math.Min(1f, HitChance) * (0.94f)/* Glancing */;
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
                if (null != this.wOH)
                {
                    WDam += (uint)(this.wOH.damage * this.fWeaponDamageModifier);
                    WDam /= 2; // Average out the damage between the 2 weapons.
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
                float ChanceToHit = 1;
                // Determine Dodge chance
                float fDodgeChanceForTarget = wMH.chanceDodged;
                // Determine Parry Chance  (Only for Tank... Since only they should be in front of the target.)
                float fParryChanceForTarget = wMH.chanceParried;
                // Determine Miss Chance
                float fMissChance = wMH.chanceMissed;
                if (wOH != null)
                {
                    fDodgeChanceForTarget = (fDodgeChanceForTarget + wOH.chanceDodged) / 2;
                    fParryChanceForTarget = (fParryChanceForTarget + wOH.chanceParried)/2;
                    fMissChance = (fMissChance + wOH.chanceMissed)/2;
                }
                ChanceToHit -= Math.Max(0, fMissChance);
                ChanceToHit -= Math.Max(0, fDodgeChanceForTarget);
                if (CState != null && !CState.m_bAttackingFromBehind)
                    ChanceToHit -= Math.Max(0, fParryChanceForTarget);
                return ChanceToHit;
            }
        }

        private float combinedSwingTime
        {
            get
            {
                if (wMH != null)
                {
                    if (wOH != null)
                    {
                        return 1 / ((1 / wMH.hastedSpeed) + (1 / wOH.hastedSpeed));
                    }
                    else
                        return wMH.hastedSpeed;
                }
                else
                    // Just throw in some default bare-hand weapon time as the default.
                    return 2.0f;
            }
        }
    }
}
