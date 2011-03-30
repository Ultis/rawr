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


        public StatsDK getSpecialEffects(SpecialEffect effect, bool bFromTankDK = false)
        {
            StatsDK statsAverage = new StatsDK();
            if (effect.Trigger == Trigger.Use)
            {
                foreach (SpecialEffect e in effect.Stats.SpecialEffects())
                {
                    statsAverage.Accumulate(this.getSpecialEffects(e), (effect.Duration / effect.Cooldown));
                }
                if (bFromTankDK)
                    statsAverage.Accumulate(effect.Stats);
                else
                    statsAverage.Accumulate(effect.GetAverageStats());
            }
            else
            {
                double trigger = 0f;
                float chance = 0f;
                float unhastedAttackSpeed = 2f;
                switch (effect.Trigger)
                {
                    case Trigger.MeleeCrit:
                    case Trigger.PhysicalCrit:
                        trigger = (1f / ((m_Rot.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f)));
                        chance = combatTable.physCrits;
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.MeleeAttack:
                    case Trigger.MeleeHit:
                    case Trigger.PhysicalHit:
                        trigger = (1f / ((m_Rot.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f)));
                        chance = 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial) * (1f - combatTable.totalMHMiss);
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.CurrentHandHit:
                        trigger = (1f / (m_Rot.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f));
                        chance = 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial) * (1f - combatTable.totalMHMiss);
                        // TODO: need to know if this is MH or OH.
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.MainHandHit:
                        trigger = (1f / (m_Rot.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f));
                        chance = 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial) * (1f - combatTable.totalMHMiss);
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.OffHandHit:
                        trigger = (1f / (m_Rot.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f));
                        chance = 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial) * (1f - combatTable.totalMHMiss);
                        unhastedAttackSpeed = (combatTable.OH != null ? combatTable.OH.baseSpeed : 2.0f);
                        break;
                    case Trigger.DamageDone:
                        trigger = 1f / (((m_Rot.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + m_Rot.getSpellSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f));
                        chance = (1f - (combatTable.missedSpecial + combatTable.dodgedSpecial)) * (1f - combatTable.totalMHMiss);
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.DamageOrHealingDone:
                        // Need to add Self Healing
                        trigger = 1f / (((m_Rot.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + m_Rot.getSpellSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f));
                        chance = (1f - (combatTable.missedSpecial + combatTable.dodgedSpecial)) * (1f - combatTable.totalMHMiss);
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        trigger = 1f / m_Rot.getSpellSpecialsPerSecond();
                        chance = 1f - combatTable.spellResist;
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        trigger = 1f / m_Rot.getSpellSpecialsPerSecond();
                        chance = combatTable.spellCrits;
                        break;
                    case Trigger.BloodStrikeHit:
                        trigger = m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.BloodStrikeHit) * (combatTable.DW ? 2f : 1f));
                        chance = 1f; 
                        break;
                    case Trigger.HeartStrikeHit:
                        trigger = m_Rot.CurRotationDuration / m_Rot.CountTrigger(Trigger.HeartStrikeHit);
                        chance = 1f;
                        break;
                    case Trigger.ObliterateHit:
                        trigger = m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.ObliterateHit) * (combatTable.DW ? 2f : 1f));
                        chance = 1f;
                        break;
                    case Trigger.ScourgeStrikeHit:
                        trigger = m_Rot.CurRotationDuration / m_Rot.CountTrigger(Trigger.ScourgeStrikeHit);
                        chance = 1f;
                        break;
                    case Trigger.DeathStrikeHit:
                        trigger = m_Rot.CurRotationDuration / m_Rot.CountTrigger(Trigger.DeathStrikeHit);
                        chance = 1f;
                        break;
                    case Trigger.PlagueStrikeHit:
                        trigger = m_Rot.CurRotationDuration / (m_Rot.CountTrigger(Trigger.PlagueStrikeHit) * (combatTable.DW ? 2f : 1f));
                        chance = 1f;
                        break;
                    case Trigger.DeathRuneGained:
                        if (m_Rot.m_DeathRunes > 0)
                            trigger = m_Rot.CurRotationDuration / (m_Rot.m_DeathRunes);
                        chance = 1f;
                        break;
                    case Trigger.DoTTick:
                        // TODO: check the tick rate from the specific FF & BP instances in Rot.
//                        trigger = m_Rot.NumDisease;
                        chance = 1f;
                        break;
                    // TankDK triggers:
                    case Trigger.DamageAvoided:
                        break;
                    case Trigger.DamageTaken:
                        break;
                    case Trigger.DamageTakenMagical:
                        break;
                    case Trigger.DamageTakenPhysical:
                        break;
                    case Trigger.DamageParried:
                        break;
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
