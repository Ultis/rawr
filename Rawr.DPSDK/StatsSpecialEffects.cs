using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DK;

namespace Rawr.DPSDK
{
    public class StatsSpecialEffects
    {
//        public Character character;
//        public Stats stats;
        public DKCombatTable combatTable;
        public Rotation m_Rot;
        private BossOptions m_bo;
        private Dictionary<Trigger, float> triggerIntervals;
        private Dictionary<Trigger, float> triggerChances;
        private float unhastedAttackSpeed;

        public StatsSpecialEffects(DKCombatTable t, Rotation rot, BossOptions bo)
        {
            if (rot.ml_Rot == null || rot.ml_Rot.Count == 0)
            {
#if DEBUG
                throw new Exception("Invalid or Incomplete rotation.");
#endif
            }
            combatTable = t;
            m_Rot = rot;
            m_bo = bo;

            triggerIntervals = new Dictionary<Trigger, float>();
            // Chance that trigger of correct type is produced (for example for
            /// SpellCrit trigger you would set triggerInterval to average time between hits and set
            /// triggerChance to crit chance)
            triggerChances = new Dictionary<Trigger, float>();
            unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
            float unhastedAttackSpeedOH = (combatTable.OH != null ? combatTable.OH.baseSpeed : 2.0f);
            float unhastedAttackSpeedSpells = (combatTable.OH != null ? combatTable.OH.baseSpeed : 2.0f);

            #region Use
            triggerIntervals.Add(Trigger.Use, 0);
            triggerChances.Add(Trigger.Use, 1);
            #endregion
            #region Basic Hit
            float fMeleeHitTriggerInterval = (1f / ((m_Rot.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f)));
            float fMeleeHitTriggerInterval1ofDW = (1f / ((m_Rot.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f)));
            float fPhysicalHitChance = 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial) * (1f - combatTable.totalMHMiss);
            triggerIntervals.Add(Trigger.MeleeHit, fMeleeHitTriggerInterval);
            triggerIntervals.Add(Trigger.PhysicalHit, fMeleeHitTriggerInterval);
            triggerIntervals.Add(Trigger.MeleeAttack, fMeleeHitTriggerInterval);
            triggerIntervals.Add(Trigger.PhysicalAttack, fMeleeHitTriggerInterval);
            triggerChances.Add(Trigger.MeleeHit, fPhysicalHitChance);
            triggerChances.Add(Trigger.PhysicalHit, fPhysicalHitChance);
            triggerChances.Add(Trigger.MeleeAttack, fPhysicalHitChance);
            triggerChances.Add(Trigger.PhysicalAttack, fPhysicalHitChance);
            // TODO: interval would be quicker since it should include DOTTick interval.
            triggerIntervals.Add(Trigger.DamageDone, fMeleeHitTriggerInterval);
            triggerChances.Add(Trigger.DamageDone, fPhysicalHitChance);
            triggerIntervals.Add(Trigger.DamageOrHealingDone, fMeleeHitTriggerInterval);
            triggerChances.Add(Trigger.DamageOrHealingDone, fPhysicalHitChance);
            #endregion
            #region Special Hit
            triggerIntervals.Add(Trigger.CurrentHandHit, fMeleeHitTriggerInterval1ofDW);
            triggerChances.Add(Trigger.CurrentHandHit, fPhysicalHitChance);
            triggerIntervals.Add(Trigger.MainHandHit, fMeleeHitTriggerInterval1ofDW);
            triggerChances.Add(Trigger.MainHandHit, fPhysicalHitChance);
            float fMeleeHitTriggerIntervalOH = (combatTable.OH == null ? 0 : fMeleeHitTriggerInterval1ofDW);
            triggerIntervals.Add(Trigger.OffHandHit, fMeleeHitTriggerIntervalOH);
            triggerChances.Add(Trigger.OffHandHit, fPhysicalHitChance);
            #endregion
            #region Basic Crit
            triggerIntervals.Add(Trigger.MeleeCrit, fMeleeHitTriggerInterval);
            triggerChances.Add(Trigger.MeleeCrit, combatTable.physCrits);
            triggerIntervals.Add(Trigger.PhysicalCrit, fMeleeHitTriggerInterval);
            triggerChances.Add(Trigger.PhysicalCrit, combatTable.physCrits);
            #endregion

            #region Spell Hit
            float fSpellHitInterval = 0;
            fSpellHitInterval = 1f / m_Rot.getSpellSpecialsPerSecond();
            float fSpellHitChance = 0;
            float fSpellCritChance = 0;
            switch (m_Rot.curRotationType)
            {
                case Rotation.Type.Unholy:
                {
                    // Unholy
                    fSpellHitChance = m_Rot.GetAbilityOfType(DKability.DeathCoil).HitChance;
                    fSpellCritChance = m_Rot.GetAbilityOfType(DKability.DeathCoil).CritChance;
                    break;
                }
                case Rotation.Type.Frost:
                {
                    if (m_Rot.Contains(DKability.HowlingBlast))
                    {
                        fSpellHitChance = m_Rot.GetAbilityOfType(DKability.HowlingBlast).HitChance;
                        fSpellCritChance = m_Rot.GetAbilityOfType(DKability.HowlingBlast).CritChance;
                    }
                    else
                    {
                        fSpellHitChance = m_Rot.GetAbilityOfType(DKability.IcyTouch).HitChance;
                        fSpellCritChance = m_Rot.GetAbilityOfType(DKability.IcyTouch).CritChance;
                    }
                    break;
                }
                case Rotation.Type.Blood:
                {
                    fSpellHitChance = m_Rot.GetAbilityOfType(DKability.IcyTouch).HitChance;
                    fSpellCritChance = m_Rot.GetAbilityOfType(DKability.IcyTouch).CritChance;
                    break;
                }
            }
            triggerIntervals.Add(Trigger.DamageSpellCast, fSpellHitInterval);
            triggerChances.Add(Trigger.DamageSpellCast, fSpellHitChance);
            triggerIntervals.Add(Trigger.SpellCast, fSpellHitInterval);
            triggerChances.Add(Trigger.SpellCast, fSpellHitChance);
            triggerIntervals.Add(Trigger.DamageSpellHit, fSpellHitInterval);
            triggerChances.Add(Trigger.DamageSpellHit, fSpellHitChance);
            triggerIntervals.Add(Trigger.SpellHit, fSpellHitInterval);
            triggerChances.Add(Trigger.SpellHit, fSpellHitChance);
            #endregion
            #region Spell Crit
            triggerIntervals.Add(Trigger.SpellCrit, fSpellHitInterval);
            triggerChances.Add(Trigger.SpellCrit, fSpellCritChance);
            triggerIntervals.Add(Trigger.DamageSpellCrit, fSpellHitInterval);
            triggerChances.Add(Trigger.DamageSpellCrit, fSpellCritChance);
            #endregion

            #region Specific Strikes
//            triggerIntervals.Add(Trigger.BloodStrikeHit, 0);
//            triggerChances.Add(Trigger.BloodStrikeHit, 0);
            if (m_Rot.HasTrigger(Trigger.BloodStrikeHit))
            {
                triggerIntervals.Add(Trigger.BloodStrikeHit, m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.BloodStrikeHit) * (combatTable.DW ? 2f : 1f)));
                triggerChances.Add(Trigger.BloodStrikeHit, m_Rot.GetAbilityOfType(DKability.BloodStrike).HitChance);
            }
//            triggerIntervals.Add(Trigger.HeartStrikeHit, 0);
//            triggerChances.Add(Trigger.HeartStrikeHit, 0);
            if (m_Rot.HasTrigger(Trigger.HeartStrikeHit))
            {
                triggerIntervals.Add(Trigger.HeartStrikeHit, m_Rot.CurRotationDuration / m_Rot.CountTrigger(Trigger.HeartStrikeHit));
                triggerChances.Add(Trigger.HeartStrikeHit, m_Rot.GetAbilityOfType(DKability.HeartStrike).HitChance); 
            }
//            triggerIntervals.Add(Trigger.ObliterateHit, 0);
//            triggerChances.Add(Trigger.ObliterateHit, 0);
            if (m_Rot.HasTrigger(Trigger.ObliterateHit))
            {
                triggerIntervals.Add(Trigger.ObliterateHit, m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.ObliterateHit) * (combatTable.DW ? 2f : 1f)));
                triggerChances.Add(Trigger.ObliterateHit, m_Rot.GetAbilityOfType(DKability.Obliterate).HitChance);
            }
//            triggerIntervals.Add(Trigger.ScourgeStrikeHit, 0);
//            triggerChances.Add(Trigger.ScourgeStrikeHit, 0);
            if (m_Rot.HasTrigger(Trigger.ScourgeStrikeHit))
            {
                triggerIntervals.Add(Trigger.ScourgeStrikeHit, m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.ScourgeStrikeHit) * (combatTable.DW ? 2f : 1f)));
                triggerChances.Add(Trigger.ScourgeStrikeHit, m_Rot.GetAbilityOfType(DKability.ScourgeStrike).HitChance);
            }
//            triggerIntervals.Add(Trigger.DeathStrikeHit, 0);
//            triggerChances.Add(Trigger.DeathStrikeHit, 0);
            if (m_Rot.HasTrigger(Trigger.DeathStrikeHit))
            {
                triggerIntervals.Add(Trigger.DeathStrikeHit, m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.DeathStrikeHit) * (combatTable.DW ? 2f : 1f)));
                triggerChances.Add(Trigger.DeathStrikeHit, m_Rot.GetAbilityOfType(DKability.DeathStrike).HitChance);
            }
//            triggerIntervals.Add(Trigger.PlagueStrikeHit, 0);
//            triggerChances.Add(Trigger.PlagueStrikeHit, 0);
            if (m_Rot.HasTrigger(Trigger.PlagueStrikeHit))
            {
                triggerIntervals.Add(Trigger.PlagueStrikeHit, m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.PlagueStrikeHit) * (combatTable.DW ? 2f : 1f)));
                triggerChances.Add(Trigger.PlagueStrikeHit, m_Rot.GetAbilityOfType(DKability.PlagueStrike).HitChance);
            }
//            triggerIntervals.Add(Trigger.IcyTouchHit, 0);
//            triggerChances.Add(Trigger.IcyTouchHit, 0);
            if (m_Rot.HasTrigger(Trigger.IcyTouchHit))
            {
                triggerIntervals.Add(Trigger.IcyTouchHit, m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.IcyTouchHit) * (combatTable.DW ? 2f : 1f)));
                triggerChances.Add(Trigger.IcyTouchHit, m_Rot.GetAbilityOfType(DKability.IcyTouch).HitChance);
            }
//            triggerIntervals.Add(Trigger.RuneStrikeHit, 0);
//            triggerChances.Add(Trigger.RuneStrikeHit, 0);
            if (m_Rot.HasTrigger(Trigger.RuneStrikeHit))
            {
                triggerIntervals.Add(Trigger.RuneStrikeHit, m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.RuneStrikeHit) * (combatTable.DW ? 2f : 1f)));
                triggerChances.Add(Trigger.RuneStrikeHit, m_Rot.GetAbilityOfType(DKability.RuneStrike).HitChance); 
            }
            #endregion

            #region Misc Offensive
            triggerIntervals.Add(Trigger.DeathRuneGained, (m_Rot.m_DeathRunes > 0) ? m_Rot.CurRotationDuration / (m_Rot.m_DeathRunes) : 0);
            triggerChances.Add(Trigger.DeathRuneGained, 1);
            triggerIntervals.Add(Trigger.KillingMachine, (combatTable.m_CState.m_Talents.KillingMachine > 0) ? ( 60 / (5 * combatTable.m_CState.m_Talents.KillingMachine / 3 ) ) : 0); // KM is a PPM
            triggerChances.Add(Trigger.KillingMachine, 1);
            triggerIntervals.Add(Trigger.DoTTick, 1); // TODO: assumes 2 diseases.  but w/ Blood & unholy's chance for a 3rd plus UB could also tick.... should be dynamic.
            triggerChances.Add(Trigger.DoTTick, 1);
            #endregion

            #region Defensive
            triggerChances.Add(Trigger.DamageParried, Math.Min(1f, m_Rot.m_CT.m_CState.m_Stats.EffectiveParry));
            triggerIntervals.Add(Trigger.DamageParried, m_bo.DynamicCompiler_FilteredAttacks(m_bo.GetFilteredAttackList(AVOIDANCE_TYPES.Parry)).AttackSpeed);
            float fAvoidance = (m_Rot.m_CT.m_CState.m_Stats.EffectiveParry
                + m_Rot.m_CT.m_CState.m_Stats.Dodge
                + m_Rot.m_CT.m_CState.m_Stats.Miss);
            triggerChances.Add(Trigger.DamageAvoided, Math.Min(1f, fAvoidance));
            triggerIntervals.Add(Trigger.DamageAvoided, m_bo.DynamicCompiler_FilteredAttacks(m_bo.GetFilteredAttackList(
                AVOIDANCE_TYPES.Parry | AVOIDANCE_TYPES.Block | AVOIDANCE_TYPES.Dodge | AVOIDANCE_TYPES.Miss)).AttackSpeed);
            triggerChances.Add(Trigger.DamageTaken, Math.Min(1f, 1f - fAvoidance));
            triggerIntervals.Add(Trigger.DamageTaken, m_bo.DynamicCompiler_Attacks.AttackSpeed);
            triggerChances.Add(Trigger.DamageTakenPhysical, Math.Min(1f, 1f - fAvoidance));
            triggerIntervals.Add(Trigger.DamageTakenPhysical, m_bo.DynamicCompiler_FilteredAttacks(m_bo.GetFilteredAttackList(ItemDamageType.Physical)).AttackSpeed);
            triggerChances.Add(Trigger.DamageTakenPutsMeBelow35PercHealth, 0.35f);
            triggerIntervals.Add(Trigger.DamageTakenPutsMeBelow35PercHealth, m_bo.DynamicCompiler_Attacks.AttackSpeed);
            triggerChances.Add(Trigger.DamageTakenMagical, 1);
            List<Attack> attacks = new List<Attack>();
            foreach (ItemDamageType i in EnumHelper.GetValues(typeof(ItemDamageType)))
            {
                if (i != ItemDamageType.Physical)
                {
                    foreach (Attack a in m_bo.GetFilteredAttackList(i))
                        attacks.Add(a);
                }
            }
            triggerIntervals.Add(Trigger.DamageTakenMagical, m_bo.DynamicCompiler_FilteredAttacks(attacks).AttackSpeed);


            #endregion
        }

        /// <summary>
        /// With Triggers already setup, just pass into the Accumulate function and return the values
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public StatsDK getSpecialEffects(SpecialEffect effect)
        {
            StatsDK statsAverage = new StatsDK();
            triggerIntervals[Trigger.Use] = effect.Cooldown;
            if (float.IsInfinity(effect.Cooldown)) triggerIntervals[Trigger.Use] = m_bo.BerserkTimer;

            effect.AccumulateAverageStats(statsAverage, triggerIntervals, triggerChances, unhastedAttackSpeed, m_bo.BerserkTimer);
            return statsAverage;
        }
    }
}
