using System;

namespace Rawr.Retribution
{
    /// <summary>
    /// Base ability class
    /// </summary>
    public abstract class AbilityBase
    {
       /// <summary>
       /// WoW item/spell Id
       /// </summary>
       public abstract int Id { get; }
       /// <summary>
       /// Indicatees the amount of time between abilities uses. use a -1 to indicate not applicable
       /// </summary>
       public abstract float CooldownDuration { get; }
       /// <summary>
       /// used when ability is on cooldown to indicate remaining ticks
       /// </summary>
       public abstract float CooldownRemaining { get; set; }
       public abstract DamageType DamageType { get; }
       /// <summary>
       /// causes effects of ability to be fired
       /// </summary>
       /// <returns></returns>
       public abstract double Fire();
    }
    public sealed class AutoAttack : AbilityBase
    {
       
       public override int Id { get { return (int)PallyConstants.CS_ID; } }
       public override float CooldownRemaining { get; set; }
       public override float CooldownDuration { get { return CurrentState.BaseWeaponSpeed; } }
       public override DamageType DamageType { get { return DamageType.Physical; } }
       /// <summary>
       /// ((AP/14) * Weaponspeed) + Weapon damage
       /// </summary>
       /// <returns></returns>
       public override double Fire()
       {
           return (CurrentState.DpsPerAttackPower() * CurrentState.BaseWeaponSpeed) + CurrentState.BaseWeaponDamage;
       }
    }

    public sealed class AvengingWrath : AbilityBase
    {
       private float TickDuration = PallyConstants.AW_DURATION;
       private int TickCounter;
       public override int Id { get { return PallyConstants.AW_ID; }}
       public override float CooldownDuration{get { return PallyConstants.AW_COOLDOWN; }}
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType{get { return DamageType.Holy; }}
       public override double Fire()
       {
           throw new NotImplementedException();
       }
    }
    /// <summary>
    /// Handles the Paladin DOT ability Consecration
    /// </summary>
    public sealed class Consecration : AbilityBase
    {
       /// <summary>
       /// Consecration specific. determines how many seconds damage is applied
       /// </summary>
       public float TickDuration { get; set; }
       /// <summary>
       /// Consecration specific. Keeps track of how many ticks have elapsed
       /// </summary>
       private int TickCounter;
       /// <summary>
       /// cooldown with applicable modifiers
       /// </summary>
       private float modifiedCooldown;
       /// <summary>
       /// init a new consecration spell for this cycle
       /// </summary>
       /// <param name="tickModifier">some glyphs may alter tick duration. use a negative number to lower duration</param>
       /// <param name="cooldownModifier">some glyphs may alter spell cooldown. use a negative number to lower cooldown</param>
       public Consecration(float tickModifier, float cooldownModifier)
       {
           TickDuration = tickModifier;
           modifiedCooldown = CooldownDuration + cooldownModifier;
           TickCounter = 0;
       }
       /// <summary>
       /// Initialized to default values (10 sec
       /// </summary>
       public Consecration()
       {
           TickDuration = PallyConstants.CONS_DEFAULT_DURATION;
           modifiedCooldown = CooldownDuration;
           TickCounter = 0;
       }
       /** overrides **/
       public override int Id { get { return (int)PallyConstants.CONS_ID; } }
       public override float CooldownDuration { get { return PallyConstants.CONS_COOLDOWN; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType { get { return DamageType.Holy; } }
       /// <summary>
       /// (810 + .26 *  AP + .26 SP) / 10
       /// </summary>
       /// <returns></returns>
       public override double Fire()
       {
           //Check to see if we can consecrate
           if (CooldownRemaining > 0)
           {
               //nope, decrement
               CooldownRemaining -= 1;
               return -1;
           }
           // make sure we have some ticks left
           if (TickCounter < TickDuration)
           {
               // damage per tick
               return ( PallyConstants.CONS_BASE_DMG +
                   ( PallyConstants.CONS_COEFF * CurrentState.AttackPower) +
                   ( PallyConstants.CONS_COEFF * CurrentState.SpellPower)) / TickDuration;
           }
           //otherwise nothing
           //reset cooldown tick timer.  this may make it so the cooldown doesnt start untikl after last damage tick
           CooldownRemaining = modifiedCooldown;
           return -1;
       }

       
    }
    public sealed class CrusaderStrike : AbilityBase
    {
       public override int Id { get { return PallyConstants.CS_ID; } }
       public override float CooldownDuration { get { return PallyConstants.CS_COOLDOWN; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType{get { return DamageType.Physical; }}
       /// <summary>
       /// (((AP/14) * 3.3 ) + Weapon damage) * 115%
       /// </summary>
       /// <returns>Damage produced from strike, or -1 for error</returns>
       public override double Fire()
       {
           if (CooldownRemaining > 0)
           {
               CooldownRemaining -= 1;
               return -1;
           }
           
           //reset cooldown tick timer
           CooldownRemaining = CooldownDuration;
           //Damage
           return ((CurrentState.DpsPerAttackPower() * PallyConstants.CS_COEFF) + CurrentState.BaseWeaponDamage) * PallyConstants.CS_DMG_BONUS;
       }
    }
    public sealed class Exorcism : AbilityBase
    {
       public override int Id { get { return PallyConstants.EXO_ID; } }
       public override float CooldownDuration { get { return PallyConstants.EXO_COOLDOWN; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType { get { return DamageType.Holy; } }
       /// <summary>
       /// (2591 + 2891 / 2) + (0.344 * cond($gt($SP, AP, $SP, AP))))
       /// Coefficient: 34.4% of the higher of AP or SP
       /// </summary>
       /// <returns>damage done if successful, -1 if on cooldown</returns>
       public override double Fire()
       {
           if (CooldownRemaining > 0)
           {
               CooldownRemaining -= 1;
               return -1;
           }
           //reset cooldown tick timer
           CooldownRemaining = CooldownDuration;
           float modifier = Math.Max(CurrentState.BaseAttackPower, CurrentState.SpellPower);
           double retVal = PallyConstants.EXO_AVG_DMG + (PallyConstants.EXO_DMG_BONUS * modifier);
           return retVal;
       }
    }
    public sealed class HammerOfWrath : AbilityBase
    {
       public override int Id { get { return PallyConstants.HOW_ID; } }
       public override float CooldownDuration { get { return PallyConstants.HOW_COOLDOWN; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType {get { return DamageType.Holy; }}
       public override double Fire()
       {
           if (CooldownRemaining > 0)
           {
               CooldownRemaining -= 1;
               return -1;
           }
           //reset cooldown tick timer
           CooldownRemaining = CooldownDuration;
           return PallyConstants.HOW_AVG_DMG;
       }
    }
    public sealed class HolyWrath : AbilityBase
    {
       public override int Id { get { return (int)PallyConstants.HOLY_WRATH_ID; } }
       public override float CooldownDuration { get { return PallyConstants.HOLY_WRATH_COOLDOWN; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType { get { return DamageType.Holy; } }
       /// <summary>
       /// (0.61 * holy power + 2435)
       /// </summary>
       /// <returns></returns>
       public override double Fire()
       {
           if (CooldownRemaining > 0)
           {
               CooldownRemaining -= 1;
               return -1;
           }
           //reset cooldown tick timer
           CooldownRemaining = CooldownDuration;
           return PallyConstants.HOLY_WRATH_COEFF * CurrentState.HolyPower + PallyConstants.HOLY_WRATH_BASE_DMG;
       }
    }
    #region Judgements
    public sealed class JudgementOfInsight : AbilityBase
    {
       public override int Id { get { return PallyConstants.JOI_ID; } }
       public override float CooldownDuration { get { return PallyConstants.JUDGE_COOLDOWN; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType { get { return DamageType.Holy; } }
       /// <summary>
       /// (1 + 0.25 * holy power.16 * AP)
       /// </summary>
       /// <returns></returns>
       public override double Fire()
       {
           if (CooldownRemaining > 0)
           {
               CooldownRemaining -= 1;
               return -1;
           }
           //reset cooldown tick timer
           CooldownRemaining = CooldownDuration;
           return 1 + ((PallyConstants.JOI_HOLY_COEFF * CurrentState.HolyPower) * (PallyConstants.JOI_AP_COEFF * CurrentState.AttackPower));
       }
    }
    public sealed class JudgementOfJustiice : AbilityBase
    {
       public override int Id { get { return PallyConstants.SOJ_ID; } }
       public override float CooldownDuration { get { return PallyConstants.JUDGE_COOLDOWN; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType { get { return DamageType.Holy; } }
       /// <summary>
       /// (MWS * (0.005 * AP.01 * holy power)
       /// </summary>
       /// <returns></returns>
       public override double Fire()
       {
           if (CooldownRemaining > 0)
           {
               CooldownRemaining -= 1;
               return -1;
           }
           //reset cooldown tick timer
           CooldownRemaining = CooldownDuration;
           return CurrentState.WeaponSpeed *
               ((PallyConstants.SOJ_AP_COEFF * CurrentState.AttackPower) *
               (PallyConstants.SOJ_HOLY_COEFF * CurrentState.HolyPower));
       }
    }
    public sealed class JudgementOfTruth : AbilityBase //TODO Censure
    {
       public override int Id { get { return (int)PallyConstants.JOT_ID; } }
       public override float CooldownDuration { get { return PallyConstants.JUDGE_COOLDOWN; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType { get { return DamageType.Holy; } }
       /// <summary>
       /// (1 + 14.21% AP + 22.29% SP) * (100% + (10% * Censure Stacks)). At 5 stacks this is (1.5 + 21.315% AP + 33.435% SP)
       /// </summary>
       /// <returns></returns>
       public override double Fire()
       {
           if (CooldownRemaining > 0)
           {
               CooldownRemaining -= 1;
               return -1;
           }
           //reset cooldown tick timer
           CooldownRemaining = CooldownDuration;
           double retVal = (1 + 0.223 * CurrentState.HolyPower + .142 * CurrentState.AttackPower) + (.10 * CurrentState.CensureCount);
           return retVal;
       }
    }
    #endregion
    #region Seals
    
    //Note: Seal of Insight is not a damage dealer, so it is ignored
    public sealed class SealOfJustice : AbilityBase
    {
       public override int Id { get { return PallyConstants.SOJ_ID; } }
       public override float CooldownDuration { get { return -1; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType{get { return DamageType.Holy; } }
       /// <summary>
       /// MWS * ((0.005 * AP) * (.01 * holy power))
       /// </summary>
       /// <returns></returns>
       public override double Fire()
       {
           return CurrentState.WeaponSpeed *
               ((PallyConstants.SOJ_AP_COEFF * CurrentState.AttackPower) *
               (PallyConstants.SOJ_HOLY_COEFF * CurrentState.HolyPower));
       }
    }
    public sealed class SealOfRighteousness : AbilityBase
    {
       public override int Id { get { return (int)PallyConstants.SOT_ID; } }
       public override float CooldownDuration { get { return -1; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType {get { throw new NotImplementedException(); }}
       /// <summary>
       /// MWS * (0.011 * AP.022 * holy power) * 100 / 100)
       /// </summary>
       /// <returns>Damage from this attack</returns>
       public override double Fire()
       {
           if (CooldownRemaining > 0)
           {
               CooldownRemaining -= 1;
               return -1;
           }
           return CurrentState.BaseWeaponSpeed *
               (.011 * CurrentState.BaseAttackPower) +
               (.022 * CurrentState.HolyPower);
       }
    }
    /// <summary>
    /// raw abitity without censure stacks
    /// </summary>
    public sealed class SealOfTruth : AbilityBase  //TODO Censure
    {
       public override int Id { get { return (int)PallyConstants.SOT_ID; } }
       public override float CooldownDuration { get { return -1; } }
       public override double Fire()
       {
           if (CooldownRemaining > 0)
           {
               CooldownRemaining -= 1;
               return -1;
           }
           /**
            *   EJ:        (((AP/14) * Weaponspeed) + Weapon damage) * 15%
            *   Tooltip:   ((0.01 * holy power.0193 * AP) * 5 * 100 / 100)
            */
           throw new NotImplementedException();
       }
       public override float CooldownRemaining
       {
           get
           {
               throw new NotImplementedException();
           }
           set
           {
               throw new NotImplementedException();
           }
       }
       public override DamageType DamageType
       {
           get { throw new NotImplementedException(); }
       }
    }
    #endregion

    public sealed class TemplarsVerdict : AbilityBase
    {
       public float HolyModifier;
       /** overrides **/
       public override int Id { get { return (int)PallyConstants.TV_ID; } }
       public override float CooldownDuration { get { return -1; } }
       public override float CooldownRemaining { get; set; }
       public override DamageType DamageType { get { return DamageType.Physical; } }
       /// <summary>
       /// (((AP/14) * Weaponspeed) + Weapon damage) * modifier
       /// </summary>
       /// <returns>Damage done based on stacks of holy power</returns>
       public override double Fire()
       {
           switch (CurrentState.HolyPower)
           {
               case 1:
                   HolyModifier = PallyConstants.TV_ONE_STK;
                   break;
               case 2:
                   HolyModifier = PallyConstants.TV_TWO_STK;
                       break;
               case 3:
                       HolyModifier = PallyConstants.TV_THREE_STK;
                       break;
               default:
                       HolyModifier = -1;
                       break;
           }
           if (HolyModifier == -1) { return -1; }
           return ((CurrentState.DpsPerAttackPower() * CurrentState.BaseWeaponSpeed) + CurrentState.BaseWeaponDamage) * HolyModifier;
       }
    }
    }
}
