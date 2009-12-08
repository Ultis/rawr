using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSDK
{
    class StatsSpecialEffects
    {
        public Character character;
        public Stats stats;
        public CombatTable combatTable;

        public StatsSpecialEffects(Character c, Stats s, CombatTable t)
        {
            character = c;
            stats = s;
            combatTable = t;
        }


        public Stats getSpecialEffects(CalculationOptionsDPSDK calcOpts, SpecialEffect effect)
        {
            Stats statsAverage = new Stats();
            Rotation rotation = calcOpts.rotation;
            if (effect.Trigger == Trigger.Use)
            {
                effect.AccumulateAverageStats(statsAverage);
                foreach (SpecialEffect e in effect.Stats.SpecialEffects())
                {
                    statsAverage.Accumulate(this.getSpecialEffects(calcOpts, e), (effect.Duration / effect.Cooldown));
                }
            }
            else
            {
                float trigger = 0f;
                float chance = 0f;
                float unhastedAttackSpeed = 2f;
                switch (effect.Trigger)
                {
                    case Trigger.MeleeCrit:
                    case Trigger.PhysicalCrit:
                        trigger = (1f / ((rotation.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f)));
                        chance = combatTable.physCrits;
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.MeleeHit:
                    case Trigger.PhysicalHit:
                    case Trigger.DamageDone:
                        trigger = (1f / ((rotation.getMeleeSpecialsPerSecond() * (combatTable.DW ? 2f : 1f)) + (combatTable.combinedSwingTime != 0 ? 1f / combatTable.combinedSwingTime : 0.5f)));
                        chance = 1f - (combatTable.missedSpecial + combatTable.dodgedSpecial);
                        unhastedAttackSpeed = (combatTable.MH != null ? combatTable.MH.baseSpeed : 2.0f);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        trigger = 1f /rotation.getSpellSpecialsPerSecond();
                        chance = 1f - combatTable.spellResist;
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        trigger = 1f /rotation.getSpellSpecialsPerSecond();
                        chance = combatTable.spellCrits;
                        break;
                    case Trigger.BloodStrikeHit:
                        trigger = rotation.CurRotationDuration / (rotation.BloodStrike * (combatTable.DW ? 2f : 1f));
                        chance = 1f;
                        break;
                    case Trigger.HeartStrikeHit:
                        trigger = rotation.CurRotationDuration / rotation.HeartStrike;
                        chance = 1f;
                        break;
                    case Trigger.BloodStrikeOrHeartStrikeHit :
                        trigger = rotation.CurRotationDuration / ((rotation.BloodStrike + rotation.HeartStrike) * (combatTable.DW ? 2f : 1f));
                        chance = 1f;
                        break;
                    case Trigger.ObliterateHit:
                        trigger = rotation.CurRotationDuration / (rotation.Obliterate * (combatTable.DW ? 2f : 1f));
                        chance = 1f;
                        break;
                    case Trigger.ScourgeStrikeHit:
                        trigger = rotation.CurRotationDuration / rotation.ScourgeStrike;
                        chance = 1f;
                        break;
                    case Trigger.DeathStrikeHit:
                        trigger = rotation.CurRotationDuration / rotation.DeathStrike;
                        chance = 1f;
                        break;
                    case Trigger.PlagueStrikeHit:
                        trigger = rotation.CurRotationDuration / (rotation.PlagueStrike * (combatTable.DW ? 2f : 1f));
                        chance = 1f;
                        break;
                    case Trigger.DoTTick:
                        trigger = (rotation.BloodPlague + rotation.FrostFever) / 3;
                        chance = 1f;
                        break;

                }
                foreach (SpecialEffect e in effect.Stats.SpecialEffects())
                {
                    statsAverage.Accumulate(this.getSpecialEffects(calcOpts, e));
                }
                if (effect.MaxStack > 1)
                {
                    float timeToMax = (float)Math.Min(calcOpts.FightLength * 60, effect.GetChance(unhastedAttackSpeed) * trigger * effect.MaxStack);
                    float buffDuration = calcOpts.FightLength * 60f;
                    if (effect.Stats.AttackPower == 250f || effect.Stats.AttackPower == 215f || effect.Stats.HasteRating == 57f || effect.Stats.HasteRating == 64f)
                    {
                        buffDuration = 20f;
                    }
                    if (timeToMax * .5f > buffDuration)
                    {
                        timeToMax = 2 * buffDuration;
                    }
                    statsAverage.Accumulate(effect.Stats, effect.GetAverageStackSize(trigger, chance, unhastedAttackSpeed, buffDuration));
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
