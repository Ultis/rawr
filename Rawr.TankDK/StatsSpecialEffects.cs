using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    class StatsSpecialEffects
    {
        //        public Character character;
        public Stats stats;
        public CombatTable combatTable;
        public StatsSpecialEffects(Character c, Stats s, CombatTable t)
        {
            // It doesn't actually use the character or stats object being passed in.
            //character = c;
            stats = s;
            combatTable = t;
        }

        public Stats getSpecialEffects(CalculationOptionsTankDK calcOpts, SpecialEffect effect)
        {
            Stats statsAverage = new Stats();
            Rotation rRotation = calcOpts.m_Rotation;
            if (effect.Trigger == Trigger.Use)
            {
                statsAverage.Accumulate(effect.GetAverageStats());
            }
            else
            {
                float trigger = 0f;
                float chance = effect.Chance;
                float unhastedAttackSpeed = 2f;
                switch (effect.Trigger)
                {
                    case Trigger.MeleeCrit:
                    case Trigger.PhysicalCrit:
                        trigger = (1f / rRotation.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f);
                        chance = combatTable.physCrits * effect.Chance;
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.MeleeHit:
                    case Trigger.PhysicalHit:
                        trigger = (1f / rRotation.getMeleeSpecialsPerSecond()) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f);
                        chance = effect.Chance * (1f - (combatTable.missedSpecial + combatTable.dodgedSpecial));
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        trigger = 1f / rRotation.getSpellSpecialsPerSecond();
                        chance = 1f - combatTable.spellResist;
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        trigger = 1f / rRotation.getSpellSpecialsPerSecond();
                        chance = combatTable.spellCrits * effect.Chance;
                        break;
                    case Trigger.DoTTick:
                        trigger = (rRotation.BloodPlague + rRotation.FrostFever) / 3;
                        break;
                    case Trigger.DamageTaken:
                        trigger = calcOpts.BossAttackSpeed;
                        chance *= 1f - (stats.Dodge + stats.Parry + stats.Miss);
                        unhastedAttackSpeed = calcOpts.BossAttackSpeed;
                        break;
                    //////////////////////////////////
                    // DK specific triggers:
                    case Trigger.BloodStrikeHit:
                    case Trigger.HeartStrikeHit:
                        trigger = rRotation.curRotationDuration / (rRotation.BloodStrike + rRotation.HeartStrike);
                        break;
                    case Trigger.PlagueStrikeHit:
                        trigger = rRotation.curRotationDuration / rRotation.PlagueStrike;
                        break;
                    case Trigger.RuneStrikeHit:
                        trigger = rRotation.curRotationDuration / rRotation.RuneStrike;
                        break;
                    case Trigger.IcyTouchHit:
                        trigger = rRotation.curRotationDuration / rRotation.IcyTouch;
                        break;
                    case Trigger.DeathStrikeHit:
                        trigger = rRotation.curRotationDuration / rRotation.DeathStrike;
                        break;
                    case Trigger.ObliterateHit:
                        trigger = rRotation.curRotationDuration / rRotation.Obliterate;
                        break;
                    case Trigger.ScourgeStrikeHit:
                        trigger = rRotation.curRotationDuration / rRotation.ScourgeStrike;
                        break;
                }
                if (effect.MaxStack > 1)
                {
                    float timeToMax = (float)Math.Min(calcOpts.FightLength * 60, effect.GetChance(unhastedAttackSpeed) * trigger * effect.MaxStack);
                    statsAverage += effect.Stats * (effect.MaxStack * (((calcOpts.FightLength * 60) - .5f * timeToMax) / (calcOpts.FightLength * 60)));
                }
                else
                {
                    effect.AccumulateAverageStats(statsAverage, trigger, chance, unhastedAttackSpeed, calcOpts.FightLength * 60);
                }
            }

            return statsAverage;
        }
    }
}
