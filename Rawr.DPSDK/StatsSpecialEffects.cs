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
        }


        public StatsDK getSpecialEffects(SpecialEffect effect)
        {
            StatsDK statsAverage = new StatsDK();
            if (effect.Trigger == Trigger.Use)
            {
                foreach (SpecialEffect e in effect.Stats.SpecialEffects())
                {
                    statsAverage.Accumulate(this.getSpecialEffects(e), (effect.Duration / effect.Cooldown));
                }

                statsAverage.Accumulate(effect.GetAverageStats(0, 1, 3, m_bo.BerserkTimer));
            }
            else
            {
                double trigger = 0f;
                float chance = 1f;
                float unhastedAttackSpeed = 2f;
                switch (effect.Trigger)
                {
                    case Trigger.MeleeCrit:
                    case Trigger.PhysicalCrit:
                        trigger = (1f / ((m_Rot.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f)));
                        chance *= combatTable.physCrits;
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.MeleeAttack:
                    case Trigger.MeleeHit:
                    case Trigger.PhysicalHit:
                        trigger = (1f / ((m_Rot.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f)));
                        chance *= 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial) * (1f - combatTable.totalMHMiss);
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.CurrentHandHit:
                        trigger = (1f / (m_Rot.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f));
                        chance *= 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial) * (1f - combatTable.totalMHMiss);
                        // TODO: need to know if this is MH or OH.
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.MainHandHit:
                        trigger = (1f / (m_Rot.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f));
                        chance *= 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial) * (1f - combatTable.totalMHMiss);
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.OffHandHit:
                        trigger = (1f / (m_Rot.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f));
                        chance *= 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial) * (1f - combatTable.totalMHMiss);
                        unhastedAttackSpeed = (combatTable.OH != null ? combatTable.OH.baseSpeed : 2.0f);
                        break;
                    case Trigger.DamageDone:
                        trigger = 1f / (((m_Rot.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + m_Rot.getSpellSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f));
                        chance *= (1f - (combatTable.missedSpecial + combatTable.dodgedSpecial)) * (1f - combatTable.totalMHMiss);
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.DamageOrHealingDone:
                        // Need to add Self Healing
                        trigger = 1f / (((m_Rot.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + m_Rot.getSpellSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f));
                        chance *= (1f - (combatTable.missedSpecial + combatTable.dodgedSpecial)) * (1f - combatTable.totalMHMiss);
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        trigger = 1f / m_Rot.getSpellSpecialsPerSecond();
                        chance *= 1f - combatTable.spellResist;
                        unhastedAttackSpeed = m_Rot.getSpellSpecialsPerSecond();
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        trigger = 1f / m_Rot.getSpellSpecialsPerSecond();
                        chance *= combatTable.spellCrits;
                        unhastedAttackSpeed = m_Rot.getSpellSpecialsPerSecond();
                        break;
                    case Trigger.BloodStrikeHit:
                        trigger = m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.BloodStrikeHit) * (combatTable.DW ? 2f : 1f));
                        chance *= m_Rot.GetAbilityOfType(DKability.BloodStrike).HitChance; 
                        break;
                    case Trigger.HeartStrikeHit:
                        trigger = m_Rot.CurRotationDuration / m_Rot.CountTrigger(Trigger.HeartStrikeHit);
                        chance *= m_Rot.GetAbilityOfType(DKability.BloodStrike).HitChance; 
                        break;
                    case Trigger.ObliterateHit:
                        trigger = m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.ObliterateHit) * (combatTable.DW ? 2f : 1f));
                        chance *= m_Rot.GetAbilityOfType(DKability.BloodStrike).HitChance; 
                        break;
                    case Trigger.ScourgeStrikeHit:
                        trigger = m_Rot.CurRotationDuration / m_Rot.CountTrigger(Trigger.ScourgeStrikeHit);
                        chance *= m_Rot.GetAbilityOfType(DKability.BloodStrike).HitChance; 
                        break;
                    case Trigger.DeathStrikeHit:
                        trigger = m_Rot.CurRotationDuration / m_Rot.CountTrigger(Trigger.DeathStrikeHit);
                        chance *= m_Rot.GetAbilityOfType(DKability.BloodStrike).HitChance; 
                        break;
                    case Trigger.PlagueStrikeHit:
                        trigger = m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.PlagueStrikeHit) * (combatTable.DW ? 2f : 1f));
                        chance *= m_Rot.GetAbilityOfType(DKability.BloodStrike).HitChance; 
                        break;
                    case Trigger.DeathRuneGained:
                        if (m_Rot.m_DeathRunes > 0)
                            trigger = m_Rot.CurRotationDuration / (m_Rot.m_DeathRunes);
                        chance *= 1f;
                        break;
                    case Trigger.DoTTick:
                        // TODO: check the tick rate from the specific FF & BP instances in Rot.
//                        trigger = m_Rot.NumDisease;
                        chance *= 1f;
                        break;
                    // TankDK triggers:
                    case Trigger.DamageAvoided:
                        chance *= Math.Min(1f, m_Rot.m_CT.m_CState.m_Stats.EffectiveParry + m_Rot.m_CT.m_CState.m_Stats.Miss + m_Rot.m_CT.m_CState.m_Stats.Dodge);
                        trigger = m_bo.DynamicCompiler_FilteredAttacks(m_bo.GetFilteredAttackList(ItemDamageType.Physical)).AttackSpeed;
                        break;
                    case Trigger.DamageTaken:
                        chance *= 1f - Math.Min(1f, m_Rot.m_CT.m_CState.m_Stats.EffectiveParry + m_Rot.m_CT.m_CState.m_Stats.Miss + m_Rot.m_CT.m_CState.m_Stats.Dodge);
                        trigger = m_bo.DynamicCompiler_Attacks.AttackSpeed;
                        break;
                    case Trigger.DamageTakenMagical:
                        chance *= 1f;
                        List<Attack> attacks = new List<Attack>();
                        foreach (ItemDamageType i in EnumHelper.GetValues(typeof(ItemDamageType)))
                        {
                            if (i != ItemDamageType.Physical)
                            {
                                foreach (Attack a in m_bo.GetFilteredAttackList(i))
                                    attacks.Add(a);
                            }
                        }
                        trigger = m_bo.DynamicCompiler_FilteredAttacks(attacks).AttackSpeed;
                        break;
                    case Trigger.DamageTakenPhysical:
                        chance *= (1f - Math.Min(1f, m_Rot.m_CT.m_CState.m_Stats.EffectiveParry + m_Rot.m_CT.m_CState.m_Stats.Miss + m_Rot.m_CT.m_CState.m_Stats.Dodge));
                        trigger = m_bo.DynamicCompiler_FilteredAttacks(m_bo.GetFilteredAttackList(ItemDamageType.Physical)).AttackSpeed;
                        break;
                    case Trigger.DamageTakenPutsMeBelow35PercHealth:
                        // TODO: Update this to use BossOptions.  Fights like Chimeron, this woul be HUGE.
                        chance *= (1f - Math.Min(1f, m_Rot.m_CT.m_CState.m_Stats.EffectiveParry + m_Rot.m_CT.m_CState.m_Stats.Miss + m_Rot.m_CT.m_CState.m_Stats.Dodge)) * .35f;
                        trigger = m_bo.DynamicCompiler_Attacks.AttackSpeed;
                        break;
                    case Trigger.DamageParried:
                        chance *= Math.Min(1f, m_Rot.m_CT.m_CState.m_Stats.EffectiveParry);
                        trigger = m_bo.DynamicCompiler_FilteredAttacks(m_bo.GetFilteredAttackList(ItemDamageType.Physical)).AttackSpeed;
                        break;
                }
                if (effect.Chance < 0)
                {
                    chance *= effect.GetChance(unhastedAttackSpeed);
                }
                foreach (SpecialEffect e in effect.Stats.SpecialEffects())
                {
                    statsAverage.Accumulate(this.getSpecialEffects(e));
                }

                if (effect.MaxStack > 1)
                {
                    float timeToMax = (float)Math.Min(m_bo.BerserkTimer, effect.GetChance(unhastedAttackSpeed) * trigger * effect.MaxStack);
                    float buffDuration = m_bo.BerserkTimer;
                    if (effect.Stats.AttackPower == 250f || effect.Stats.AttackPower == 215f || effect.Stats.HasteRating == 57f || effect.Stats.HasteRating == 64f)
                    {
                        buffDuration = 20f;
                    }
                    if (timeToMax * .5f > buffDuration)
                    {
                        timeToMax = 2 * buffDuration;
                    }
                    statsAverage.Accumulate(effect.Stats, effect.GetAverageStackSize((float)trigger, chance, unhastedAttackSpeed, buffDuration));
                }
                else
                {
                    effect.AccumulateAverageStats(statsAverage, (float)trigger, chance, unhastedAttackSpeed, m_bo.BerserkTimer);
                }
            }
            return statsAverage;
        }
    }
}
