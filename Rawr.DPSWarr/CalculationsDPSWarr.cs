using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
    class CalculationsDPSWarrData
    {
        private float fGCDTimer = 0.0f;
        private float fSwingTimerMH = 0.0f, fSwingTimerOH = 0.0f, fMHSwingTimeUnderFlurry = 0.0f, fOHSwingTimeUnderFlurry = 0.0f;
        private float fMHSwingTimeNormal = 0.0f, fOHSwingTimeNormal = 0.0f, fMHSwingTimeUnderFlurryAndBL = 0.0f, fOHSwingTimeUnderFlurryAndBL = 0.0f;
        private float fMHSwingTimeUnderBL = 0.0f, fOHSwingTimeUnderBL = 0.0f;
        private int nCurrentRage = 0;
        private bool bBloodSurgeProcced = false;
        private int nFlurryActive = 0;
        private int nDeathWishCoolDown = 0;
        private int nDeathWishActive = -1;
        private int nWhirlWindCoolDown = 0;
        private int nBloodthirstCoolDown = 0;
        private int nBloodlustCoolDown = 0;
        private int nBloodlustActive = 0;

        private bool bHeroicStrikeOnNextSwing = false;
        
        private float fMHSwingTime = 0.0f;
        private float fOHSwingTime = 0.0f;
        private float favgBaseMainHandWeaponHit = 0.0f, favgBaseMainHandWeaponHitNormalized = 0.0f;
        private float favgBaseOffHandWeaponHit = 0.0f, favgBaseOffHandWeaponHitNormalized = 0.0f;
        private float fchanceToMiss = 0.08f;
        private float fchanceToMissYellow = 0.08f;
        private float fchanceToDodge = 0.065f;
        private float fchanceToCrit = 0.0f;
        private float fchanceToGlance = 0.25f;
        private float fMitigation = 0.0f;

        public float fTimeStep = 0.1f;
        public float fDamageModifier = 1.0f;
        public float nDamageDone = 0;
        public float nDeepWoundDamage = 0;
        public float nWhiteDamage = 0, nWhirlwindDamage = 0, nBloodthirstDamage = 0, nHeroicStrikeDamage = 0, nSlamDamage = 0;
        public int nFlurryActiveCounter = 0;
        public int nHits = 0, nCrits=0, nMisses=0, nDodges=0;
        
        private Stats stats;
        private CharacterCalculationsDPSWarr calc;
        private CalculationOptionsDPSWarr options;
        private WarriorTalents talents;
        private Character character;

        public void PreCalc( Character a_character, Stats a_stats, CharacterCalculationsDPSWarr a_calcs, CalculationOptionsDPSWarr a_calcOpts, WarriorTalents a_talents )
        {
            stats = a_stats;
            calc = a_calcs;
            options = a_calcOpts;
            talents = a_talents;
            character = a_character;

            float bossArmor = options.TargetArmor;
            float bossArmorDebuffed = bossArmor - stats.ArmorPenetration;

            float totalArP = bossArmorDebuffed * stats.ArmorPenetrationRating / CalculationsDPSWarr.fArmorPen / 100.0f;
            float modifiedTargetArmor = bossArmorDebuffed - totalArP;
            fMitigation = 1.0f - modifiedTargetArmor / (modifiedTargetArmor + 15232.5f);

            a_calcs.ArmorMitigation = (1.0f - fMitigation) * 100.0f;

            fDamageModifier = 1.0f + (0.02f * talents.TwoHandedWeaponSpecialization) + stats.BonusPhysicalDamageMultiplier;
            float fHaste = stats.HasteRating;

            float fMHWeaponSwingSpeed = 2.0f, fOHWeaponSwingSpeed = 2.0f;
            if (character.MainHand != null)
            {
                favgBaseMainHandWeaponHit = (character.MainHand.MinDamage + character.MainHand.MaxDamage + stats.WeaponDamage * 2f) / 2.0f;
                fMHWeaponSwingSpeed = character.MainHand.Speed;
            }
            favgBaseMainHandWeaponHitNormalized = favgBaseMainHandWeaponHit;
            fMHSwingTimeNormal = fMHSwingTime = fMHWeaponSwingSpeed / (1 + (fHaste / CalculationsDPSWarr.fHastePerPercent / 100.0f));
            fMHSwingTimeUnderFlurry = fMHSwingTimeNormal / (1f + 0.05f * talents.Flurry);
            fMHSwingTimeUnderFlurryAndBL = fMHSwingTimeUnderFlurry / 1.3f;
            fMHSwingTimeUnderBL = fMHSwingTimeNormal / 1.3f;
            
            favgBaseMainHandWeaponHit += stats.AttackPower / 14.0f * fMHWeaponSwingSpeed;
            favgBaseMainHandWeaponHitNormalized += stats.AttackPower / 14.0f * 3.3f;
            if (character.OffHand != null)
            {
                favgBaseOffHandWeaponHit = (character.OffHand.MinDamage + character.OffHand.MaxDamage + stats.WeaponDamage * 2f) / 2.0f;
                fOHWeaponSwingSpeed = character.OffHand.Speed;
                fchanceToMiss += 0.19f; // Dual Wield Miss Rate
            }
            favgBaseOffHandWeaponHitNormalized = favgBaseOffHandWeaponHit;
            fOHSwingTimeNormal = fOHSwingTime = fOHWeaponSwingSpeed / (1 + (fHaste / CalculationsDPSWarr.fHastePerPercent / 100.0f));
            fOHSwingTimeUnderFlurry = fOHSwingTimeNormal / (1f + 0.05f * talents.Flurry);
            fOHSwingTimeUnderFlurryAndBL = fOHSwingTimeUnderFlurry / 1.3f;
            fOHSwingTimeUnderBL = fOHSwingTimeNormal * 1.3f;

            favgBaseOffHandWeaponHit += stats.AttackPower / 14.0f * fOHWeaponSwingSpeed;
            favgBaseOffHandWeaponHit *= (0.5f + 0.025f * talents.DualWieldSpecialization);
            favgBaseOffHandWeaponHitNormalized += stats.AttackPower / 14.0f * 3.3f;
            favgBaseOffHandWeaponHitNormalized *= (0.5f + 0.025f * talents.DualWieldSpecialization);
            
            fchanceToCrit = stats.CritRating / CalculationsDPSWarr.fCritRatingPerPercent / 100.0f;
            fchanceToDodge -= (stats.ExpertiseRating / CalculationsDPSWarr.fExpertiseRatingPerPercent) * 0.25f / 100.0f;
            if (fchanceToDodge < 0.0f)
                fchanceToDodge = 0.0f;
            fchanceToMiss -= stats.HitRating / CalculationsDPSWarr.fHitRatingPerPercent / 100.0f;
            fchanceToMissYellow -= stats.HitRating / CalculationsDPSWarr.fHitRatingPerPercent / 100.0f;
            if (fchanceToMiss < 0.0f)
                fchanceToMiss = 0.0f;
            if (fchanceToMissYellow < 0.0f)
                fchanceToMissYellow = 0.0f;
        }

        public void ResetDamageDealt()
        {
            nDamageDone = 0;
            nDeepWoundDamage = 0;
            nWhiteDamage = 0;
            nWhirlwindDamage = 0;
            nBloodthirstDamage = 0;
            nHeroicStrikeDamage = 0;
            nSlamDamage = 0;
            nFlurryActiveCounter = 0;
            nHits = 0;
            nCrits=0;
            nMisses = 0;
            nDodges = 0;
        }


        public void Reset()
        {
            fMHSwingTime = fMHSwingTimeNormal;
            fOHSwingTime = fOHSwingTimeNormal;
            bBloodSurgeProcced = false;
            nFlurryActive = -1;
            fSwingTimerMH = 0.0f;
            fSwingTimerOH = 0.0f;
            nDeathWishCoolDown = 0;
            nDeathWishActive = -1;
            nBloodthirstCoolDown = 0;
            nWhirlWindCoolDown = 0;
            nBloodlustCoolDown = 0;
            nBloodlustActive = 0;
            fGCDTimer = 0.0f;
            bHeroicStrikeOnNextSwing = false;
            fDamageModifier = 1.0f + (0.02f * talents.TwoHandedWeaponSpecialization) + stats.BonusPhysicalDamageMultiplier;
        }

        public void DoRotation()
        {
            Reset();
            int nFightLenIterations = options.FightLength * 10;
            for (int nCurTime = 0; nCurTime < nFightLenIterations; ++nCurTime)
            {
                DoWhiteHitCalcs();
                if (options.SimMode == 0)
                    DoFuryRotation1();
            }
        }

        public void DoWhiteHitCalcs()
        {
            if (nFlurryActive > 0)
            {
                if (nBloodlustActive > 0)
                {
                    fMHSwingTime = fMHSwingTimeUnderFlurryAndBL;
                    fOHSwingTime = fOHSwingTimeUnderFlurryAndBL;
                }
                else
                {
                    fMHSwingTime = fMHSwingTimeUnderFlurry;
                    fOHSwingTime = fOHSwingTimeUnderFlurry;
               }
            }
            else if (nFlurryActive == 0)
            {
                if (nBloodlustActive > 0)
                {
                    fMHSwingTime = fMHSwingTimeUnderBL;
                    fOHSwingTime = fOHSwingTimeUnderBL;
                }
                else
                {
                    fMHSwingTime = fMHSwingTimeNormal;
                    fOHSwingTime = fOHSwingTimeNormal;
                }
                nFlurryActive = -1;
            }
            if (nFlurryActive > 0)
                ++nFlurryActiveCounter;
            if (fSwingTimerMH > fMHSwingTime)
            {
                --nFlurryActive;
                DoMainHandWhiteHit();
                fSwingTimerMH -= fMHSwingTime;
            }
            fSwingTimerMH += fTimeStep;
            if (fSwingTimerOH > fOHSwingTime)
            {
                --nFlurryActive;
                DoOffHandWhiteHit();
                fSwingTimerOH -= fOHSwingTime;
            }
            fSwingTimerOH += fTimeStep;
        }

        public enum ETypeOfHit
        {
            eHit,
            eCrit,
            eDodge,
            eMiss,
            eGlance
        };

        public Random rnd = new Random(123);

        public ETypeOfHit rollWhite()
        {
            double dRand = rnd.NextDouble(); // Zahl zwischen 0.0 und 1.0
            if (dRand < fchanceToMiss)
            {
                ++nMisses;
                return ETypeOfHit.eMiss;
            }
            else if (dRand < fchanceToMiss + fchanceToDodge)
            {
                ++nDodges;
                return ETypeOfHit.eDodge;
            }
            else if (dRand < fchanceToMiss + fchanceToDodge + fchanceToGlance)
            {
                ++nHits;
                return ETypeOfHit.eGlance;
            }
            else if (dRand < fchanceToMiss + fchanceToDodge + fchanceToGlance + fchanceToCrit)
            {
                ++nCrits;
                return ETypeOfHit.eCrit;
            }
            ++nHits;
            return ETypeOfHit.eHit;
        }
        

        public ETypeOfHit rollYellow()
        {
            double dRand = rnd.NextDouble(); // Zahl zwischen 0.0 und 1.0
            if (dRand <= fchanceToMissYellow)
                return ETypeOfHit.eMiss;
            else if (dRand <= fchanceToMissYellow + fchanceToDodge)
                return ETypeOfHit.eDodge;
            else if (dRand <= fchanceToMissYellow + fchanceToDodge + fchanceToCrit)
                return ETypeOfHit.eCrit;
            return ETypeOfHit.eHit;
        }
        
        public void DoMainHandWhiteHit()
        {
            ETypeOfHit type = ETypeOfHit.eHit;
            if (bHeroicStrikeOnNextSwing)
                type = rollYellow();
            else
                type = rollWhite();

            float fBaseDamage = favgBaseMainHandWeaponHit;
            if(bHeroicStrikeOnNextSwing)
                fBaseDamage += 495f;

            float nTempDamageDone = 0;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += fBaseDamage;
            else if (type == ETypeOfHit.eGlance)
                nTempDamageDone += (fBaseDamage * 0.65f);
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += fBaseDamage * (2f + talents.Impale * 0.1f + stats.CritBonusDamage * 2.0f);
                GenerateDeepWoundMainHand();
                nFlurryActive = 4;
                if (bHeroicStrikeOnNextSwing)
                    nCurrentRage += 10;
            }
            nTempDamageDone *= fMitigation * fDamageModifier;
            if (!bHeroicStrikeOnNextSwing)
                GenerateRage(nTempDamageDone, character.MainHand != null ? character.MainHand.Speed : 2.0f, type == ETypeOfHit.eCrit ? 5f : 2.5f);

            if (bHeroicStrikeOnNextSwing)
            {
                nHeroicStrikeDamage += nTempDamageDone;
                PossibleBloodSurgeProc();
            }
            else
                nWhiteDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
            bHeroicStrikeOnNextSwing = false;
        }

        public void DoOffHandWhiteHit()
        {
            if (character.OffHand == null)
                return;
            ETypeOfHit type = rollWhite();
            float nTempDamageDone = 0;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += favgBaseOffHandWeaponHit;
            else if (type == ETypeOfHit.eGlance)
                nTempDamageDone += (favgBaseOffHandWeaponHit * 0.65f);
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += favgBaseOffHandWeaponHit * (2f + talents.Impale * 0.1f + stats.CritBonusDamage * 2.0f);
                GenerateDeepWoundOffHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier;
            GenerateRage(nTempDamageDone, character.OffHand.Speed, type == ETypeOfHit.eCrit ? 2.5f : 1.25f);
            nWhiteDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
        }

        public bool DoWhirlWindMainHandHit(bool a_bOffHandCrit)
        {
            ETypeOfHit type = rollYellow();
            float nTempDamageDone = 0;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += favgBaseMainHandWeaponHitNormalized;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += favgBaseMainHandWeaponHitNormalized * (2f + talents.Impale * 0.1f + stats.CritBonusDamage * 2.0f);
                if (!a_bOffHandCrit) // When both crit only offhand wounds tick
                    GenerateDeepWoundMainHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier * (1f + talents.ImprovedWhirlwind * 0.1f + talents.UnendingFury * 0.02f);
            nWhirlwindDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
            return type == ETypeOfHit.eCrit;
        }

        public bool DoWhirlWindOffHandHit()
        {
            ETypeOfHit type = rollYellow();
            float nTempDamageDone = 0;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += favgBaseOffHandWeaponHitNormalized;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += favgBaseOffHandWeaponHitNormalized * (2f + talents.Impale * 0.1f + stats.CritBonusDamage * 2.0f);
                GenerateDeepWoundOffHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier * (1f + talents.ImprovedWhirlwind * 0.1f + talents.UnendingFury * 0.02f); 
            nWhirlwindDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
            return type == ETypeOfHit.eCrit;
        }

        public void DoBloodthirstHit()
        {
            if (talents.Bloodthirst == 0)
                return;
            ETypeOfHit type = rollYellow();
            float nTempDamageDone = 0;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += stats.AttackPower / 2.0f;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += (stats.AttackPower / 2.0f) * (2f + talents.Impale * 0.1f + stats.CritBonusDamage * 2.0f);
                GenerateDeepWoundMainHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier * (1f + talents.UnendingFury * 0.02f); ;
            nBloodthirstDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
        }

        public void SlamHit()
        {
            ETypeOfHit type = rollYellow();
            float nTempDamageDone = 0;
            float fBaseDamage = favgBaseMainHandWeaponHit + 250f;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += fBaseDamage;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += fBaseDamage * (2f + talents.Impale * 0.1f + stats.CritBonusDamage * 2.0f);
                GenerateDeepWoundMainHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier;
            nSlamDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
        }
        
        public void GenerateDeepWoundMainHand()
        {
            float nDamage = (favgBaseMainHandWeaponHit * talents.DeepWounds * 0.16f * fDamageModifier * 1.3f); // Assuming Mangle present
            nDeepWoundDamage += nDamage;
            nDamageDone += nDamage;
        }

        public void GenerateDeepWoundOffHand()
        {
            float nDamage = (favgBaseOffHandWeaponHit * talents.DeepWounds * 0.16f * fDamageModifier * 1.3f);
            nDeepWoundDamage += nDamage;
            nDamageDone += nDamage;
        }

        public void GenerateRage( float a_fDamage, float a_fWeaponSpeed, float a_fWeaponFactor )
        {
            // ((Damage Dealt) / (Rage Conversion at Your Level) * 7.5 + (Weapon Speed * Factor))/2;
            if (nCurrentRage < 0)
                nCurrentRage = 0;
            nCurrentRage += (int)((a_fDamage / 320.62f * 7.5f + a_fWeaponSpeed * a_fWeaponFactor) / 2f);
            if (nCurrentRage > 100)
                nCurrentRage = 0;
        }

        public void PossibleBloodSurgeProc()
        {
            if (rnd.NextDouble() < 0.1)
                bBloodSurgeProcced = true;
        }

        public bool OnGCD()
        {
            return fGCDTimer > 0.0f;
        }

        public void InvokeGCD()
        {
            fGCDTimer = 1.5f;
        }

        public void DoFuryRotation1()
        {
            if (nDeathWishCoolDown > 0)
                --nDeathWishCoolDown;
            if (nDeathWishActive > 0)
                --nDeathWishActive;
            if (nWhirlWindCoolDown > 0)
                --nWhirlWindCoolDown;
            if (nBloodthirstCoolDown > 0)
                --nBloodthirstCoolDown;
            if (nBloodlustCoolDown > 0)
                --nBloodlustCoolDown;
            if (nBloodlustActive > 0)
                --nBloodlustActive;
            if (fGCDTimer > 0)
                fGCDTimer -= fTimeStep;
            if (nDeathWishActive == 0)
            {
                fDamageModifier -= 0.2f;
                nDeathWishActive = -1;
            }
            bool bOnGCD = fGCDTimer > 0.0f;
            if (bOnGCD)
                return;
            if (nBloodlustCoolDown <= 0 && stats.Bloodlust > 0)
            {
                nBloodlustCoolDown = (int)(300.0f / fTimeStep);
                nBloodlustActive = (int)(45.0f / fTimeStep);
            }
            int nHSRageCost = 15 - talents.ImprovedHeroicStrike;
            if (nCurrentRage > 10 && nDeathWishCoolDown <= 0 && talents.DeathWish > 0)
            {
                // Activate DeathWish
                fDamageModifier += 0.2f;
                nDeathWishCoolDown = (int)((180.0f - talents.IntensifyRage * 20.0f) / fTimeStep);
                nDeathWishActive = (int)(30.0f / fTimeStep);
                nCurrentRage -= 10;
                InvokeGCD();
            }
            else if (nCurrentRage > 25 && nWhirlWindCoolDown <= 0 )
            {
                nWhirlWindCoolDown = (int)(8.0f / fTimeStep);
                nCurrentRage -= 25;
                bool bCrit = DoWhirlWindOffHandHit();
                DoWhirlWindMainHandHit(bCrit);
                InvokeGCD();
                PossibleBloodSurgeProc();
            }
            else if (nCurrentRage > 30 && nBloodthirstCoolDown <= 0)
            {
                nBloodthirstCoolDown = (int)(5.0f / fTimeStep);
                nCurrentRage -= 30;
                DoBloodthirstHit();
                InvokeGCD();
                PossibleBloodSurgeProc();
            }
            else if (nCurrentRage > 15 && bBloodSurgeProcced)
            {
                // slam!
                nCurrentRage -= 15;
                bBloodSurgeProcced = false;
                SlamHit();
                InvokeGCD();
            }
            else if( nCurrentRage > nHSRageCost && nCurrentRage > options.HeroicStrikeRage && !bHeroicStrikeOnNextSwing )
            {
                nCurrentRage -= nHSRageCost;
                bHeroicStrikeOnNextSwing = true;
            }
        }
    }
    
	[Rawr.Calculations.RawrModelInfo("DPSWarr", "Ability_Rogue_Ambush", Character.CharacterClass.Warrior)]
	class CalculationsDPSWarr : CalculationsBase
    {
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        /// <summary>
        /// Dictionary<string, Color> that includes the names of each rating which your model will use,
        /// and a color for each. These colors will be used in the charts.
        /// 
        /// EXAMPLE: 
        /// subPointNameColors = new Dictionary<string, System.Drawing.Color>();
        /// subPointNameColors.Add("Mitigation", System.Drawing.Colors.Red);
        /// subPointNameColors.Add("Survival", System.Drawing.Colors.Blue);
        /// </summary>
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {

                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", System.Drawing.Color.Blue);
                }
                return _subPointNameColors;
            }
        }


        private string[] _characterDisplayCalculationLabels = null;
        /// <summary>
        /// An array of strings which will be used to build the calculation display.
        /// Each string must be in the format of "Heading:Label". Heading will be used as the
        /// text of the group box containing all labels that have the same Heading.
        /// Label will be the label of that calculation, and may be appended with '*' followed by
        /// a description of that calculation which will be displayed in a tooltip for that label.
        /// Label (without the tooltip string) must be unique.
        /// 
        /// EXAMPLE:
        /// characterDisplayCalculationLabels = new string[]
        /// {
        ///		"Basic Stats:Health",
        ///		"Basic Stats:Armor",
        ///		"Advanced Stats:Dodge",
        ///		"Advanced Stats:Miss*Chance to be missed"
        /// };
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                {
                    List<string> labels = new List<string>(new string[]
                    {
                    "Basic Stats:Health",
                    "Basic Stats:Armor",
					"Basic Stats:Attack Power",
					"Basic Stats:Agility",
					"Basic Stats:Strength",
					"Basic Stats:Crit",
					"Basic Stats:Hit",
					"Basic Stats:Expertise Rating",
					"Basic Stats:Haste Rating",
					"Basic Stats:Armor Penetration Rating",
					"Basic Stats:Armor Mitigation",
					"Basic Stats:Flurry Uptime",
                    "DPS Breakdown:White DPS",
                    "DPS Breakdown:Heroic Strike DPS",
                    "DPS Breakdown:Deep Wounds DPS",
                    "DPS Breakdown:Whirlwind DPS",
                    "DPS Breakdown:Bloodthirst DPS",
                    "DPS Breakdown:Slam DPS",
                    "DPS Breakdown:Total DPS",
                    "Attack Table:White Hits",
                    "Attack Table:White Crits",
                    "Attack Table:White Dodges",
                    "Attack Table:White Misses"
                    });
                    _characterDisplayCalculationLabels = labels.ToArray();
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] { "Item Budget" };
                }
                return _customChartNames;
            }
        }


        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelDPSWarr()); }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                return _relevantItemTypes ?? (_relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
                        Item.ItemType.Leather,
                        Item.ItemType.Mail,
                        Item.ItemType.Plate,
                        Item.ItemType.Crossbow,
                        Item.ItemType.Bow,
                        Item.ItemType.Gun,
                        Item.ItemType.Thrown,
                        Item.ItemType.Polearm,
                        Item.ItemType.TwoHandAxe,
                        Item.ItemType.TwoHandMace,
                        Item.ItemType.TwoHandSword,
                        Item.ItemType.OneHandAxe,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.OneHandSword
					}));
            }
        }

        public override bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot)
        {
            WarriorTalents talents = (WarriorTalents)character.CurrentTalents;
            if (talents.TitansGrip > 0 && 
                (item.Type == Item.ItemType.TwoHandAxe || item.Type == Item.ItemType.TwoHandSword || item.Type == Item.ItemType.TwoHandMace) &&
                slot == Character.CharacterSlot.OffHand )
            {
                return true;
            }
            return item.FitsInSlot(slot);
        }

        public override bool IncludeOffHandInCalculations(Character character)
        {
            WarriorTalents talents = (WarriorTalents)character.CurrentTalents;
            if (talents.TitansGrip > 0 )
                return true;
            return false;
        }


		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Warrior; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation()
        {
            return  new ComparisonCalculationDPSWarr();
        }

        public override CharacterCalculationsBase CreateNewCharacterCalculations()
        {
            return new CharacterCalculationsDPSWarr();
        }

		public override ICalculationOptionBase DeserializeDataObject(string xml)
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSWarr));
			System.IO.StringReader reader = new System.IO.StringReader(xml);
			CalculationOptionsDPSWarr calcOpts = serializer.Deserialize(reader) as CalculationOptionsDPSWarr;
			return calcOpts;
		}

        public static float fCritRatingPerPercent = 45.905982906f;
        public static float fHitRatingPerPercent = 32.78998779f;
        public static float fExpertiseRatingPerPercent = 8.1974969475f;
        public static float fAgiPerCritPercent = 62.5f;
        public static float fHastePerPercent = 32.78998779f;
        public static float fArmorPen = 15.3952985511f;

        /// <summary>
        /// GetCharacterCalculations is the primary method of each model, where a majority of the calculations
        /// and formulae will be used. GetCharacterCalculations should call GetCharacterStats(), and based on
        /// those total stats for the character, and any calculationoptions on the character, perform all the 
        /// calculations required to come up with the final calculations defined in 
        /// CharacterDisplayCalculationLabels, including an Overall rating, and all Sub ratings defined in 
        /// SubPointNameColors.
        /// </summary>
        /// <param name="character">The character to perform calculations for.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A custom CharacterCalculations object which inherits from CharacterCalculationsBase,
        /// containing all of the final calculations defined in CharacterDisplayCalculationLabels. See
        /// CharacterCalculationsBase comments for more details.</returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            Stats stats = GetCharacterStats( character, additionalItem );
            
            CharacterCalculationsDPSWarr calcs = new CharacterCalculationsDPSWarr();
            calcs.BasicStats = stats;

			CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            WarriorTalents talents = (WarriorTalents)character.CurrentTalents;

            CalculationsDPSWarrData calcData = new CalculationsDPSWarrData();
            calcData.fTimeStep = 0.1f;
            calcData.PreCalc(character, stats, calcs, calcOpts, talents);
            float nIterations = 200f;
            for (int i = 0; i < nIterations; ++i)
            {
                calcData.ResetDamageDealt();
                calcData.DoRotation();

                calcs.FlurryUptime += (float)calcData.nFlurryActiveCounter * calcData.fTimeStep / calcOpts.FightLength * 100.0f / nIterations;

                calcs.SlamDPSPoints += calcData.nSlamDamage / calcOpts.FightLength / nIterations;
                calcs.HSDPSPoints += calcData.nHeroicStrikeDamage / calcOpts.FightLength / nIterations;
                calcs.BTDPSPoints += calcData.nBloodthirstDamage / calcOpts.FightLength / nIterations;
                calcs.WWDPSPoints += calcData.nWhirlwindDamage / calcOpts.FightLength / nIterations;
                calcs.WhiteDPSPoints += calcData.nWhiteDamage / calcOpts.FightLength / nIterations;
                calcs.DeepWoundsDPSPoints += calcData.nDeepWoundDamage / calcOpts.FightLength / nIterations;
                calcs.MissedAttacks += (float)calcData.nMisses / nIterations;
                calcs.DodgedAttacks += (float)calcData.nDodges / nIterations;
                calcs.WhiteCrit += (float)calcData.nCrits / nIterations;
                calcs.WhiteHits += (float)calcData.nHits / nIterations;
                calcs.DPSPoints += calcData.nDamageDone / calcOpts.FightLength / nIterations;
            }
            calcs.SubPoints = new float[] { calcs.DPSPoints };
            calcs.OverallPoints = calcs.DPSPoints;
            return calcs;
        }

        #region Warrior Race Stats
        private static float[,] BaseWarriorRaceStats = new float[,] 
		{
							//	Strength,	Agility,	Stamina
            /*Human*/		{	174f,	    113f,	    159f,   },
            /*Orc*/			{	178f,		110f,		162f,	},
            /*Dwarf*/		{	176f,	    109f,	    162f,   },
			/*Night Elf*/	{	142f,	    101f,	    132f,   },
	        /*Undead*/		{	174f,	    111f,	    160f,   },
			/*Tauren*/		{	180f,		108f,		162f,	},
	        /*Gnome*/		{	170f,	    116f,	    159f,   },
			/*Troll*/		{	175f,	    115f,	    160f,   },	
			/*BloodElf*/	{	0f,		    0f,		    0f,	    },
			/*Draenei*/		{	176f,		110f,		159f,	},
		};

        private Stats GetRaceStats(Character character)
        {
            Stats statsRace;
            switch (character.Race)
            {
                case Character.CharacterRace.Human:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[0, 0],
                        Agility = (float)BaseWarriorRaceStats[0, 1],
                        Stamina = (float)BaseWarriorRaceStats[0, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    if ((character.MainHand != null) &&
                        ((character.MainHand.Type == Item.ItemType.TwoHandSword) ||
                         (character.MainHand.Type == Item.ItemType.TwoHandMace)))
                    {
                        statsRace.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.Orc:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[1, 0],
                        Agility = (float)BaseWarriorRaceStats[1, 1],
                        Stamina = (float)BaseWarriorRaceStats[1, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };

                    if ((character.MainHand != null) &&
                        (character.MainHand.Type == Item.ItemType.TwoHandAxe))
                    {
                        statsRace.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.Dwarf:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[2, 0],
                        Agility = (float)BaseWarriorRaceStats[2, 1],
                        Stamina = (float)BaseWarriorRaceStats[2, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.NightElf:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[3, 0],
                        Agility = (float)BaseWarriorRaceStats[3, 1],
                        Stamina = (float)BaseWarriorRaceStats[3, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 1f + 0.75f,
                    };
                    break;
                case Character.CharacterRace.Undead:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[4, 0],
                        Agility = (float)BaseWarriorRaceStats[4, 1],
                        Stamina = (float)BaseWarriorRaceStats[4, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Tauren:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[5, 0],
                        Agility = (float)BaseWarriorRaceStats[5, 1],
                        Stamina = (float)BaseWarriorRaceStats[5, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        BonusHealthMultiplier = 0.05f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Gnome:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[6, 0],
                        Agility = (float)BaseWarriorRaceStats[6, 1],
                        Stamina = (float)BaseWarriorRaceStats[6, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Troll:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[7, 0],
                        Agility = (float)BaseWarriorRaceStats[7, 1],
                        Stamina = (float)BaseWarriorRaceStats[7, 2],
                        PhysicalCrit = 1.18f * 22.08f,
                        AttackPower = 190f,
                        Dodge = 0.75f,
                    };
                    break;
                case Character.CharacterRace.Draenei:
                    statsRace = new Stats()
                    {
                        Health = 4264f,
                        Strength = (float)BaseWarriorRaceStats[9, 0],
                        Agility = (float)BaseWarriorRaceStats[9, 1],
                        Stamina = (float)BaseWarriorRaceStats[9, 2],
                        PhysicalCrit = 1.18f*22.08f,
                        AttackPower = 190f,
                        PhysicalHit = 1f,
                        Dodge = 0.75f,
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            return statsRace;
        }
        #endregion

        /// <summary>
        /// GetCharacterStats is the 2nd-most calculation intensive method in a model. Here the model will
        /// combine all of the information about the character, including race, gear, enchants, buffs,
        /// calculationoptions, etc., to form a single combined Stats object. Three of the methods below
        /// can be called from this method to help total up stats: GetItemStats(character, additionalItem),
        /// GetEnchantsStats(character), and GetBuffsStats(character.ActiveBuffs).
        /// </summary>
        /// <param name="character">The character whose stats should be totaled.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A Stats object containing the final totaled values of all character stats.</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character); 

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            WarriorTalents talents = (WarriorTalents)character.CurrentTalents;

            //base
            Stats statsGearEnchantsBuffs = statsBaseGear + statsEnchants + statsBuffs;
            //TalentTree tree = character.AllTalents;
            float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
            float staBonus = (float)Math.Floor(statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier));

            Stats statsTotal = new Stats();
            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;


            statsTotal.Agility = (agiBase + (float)Math.Floor((agiBase * statsBuffs.BonusAgilityMultiplier) + agiBonus * (1 + statsBuffs.BonusAgilityMultiplier)));
            statsTotal.Armor = statsRace.Armor + statsGearEnchantsBuffs.Armor;
            statsTotal.Strength = (strBase + (float)Math.Floor((strBase * statsBuffs.BonusStrengthMultiplier) + strBonus * (1 + statsBuffs.BonusStrengthMultiplier)));
            statsTotal.Stamina = (staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier)));
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina-staBase) * 10f))));
            statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower +
                statsTotal.Armor / 180.0f * talents.ArmoredToTheTeeth + (statsTotal.Strength * 2)) * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.AttackPower *= (1.0f + talents.ImprovedBerserkerStance * 0.02f);

            statsTotal.CritRating = statsRace.PhysicalCrit + statsGearEnchantsBuffs.CritRating;
            statsTotal.CritRating += ((statsTotal.Agility / fAgiPerCritPercent) * fCritRatingPerPercent);
            statsTotal.CritRating += statsBuffs.LotPCritRating;
            statsTotal.CritRating += fCritRatingPerPercent * 8.0f;
            
            /*Check if axe, if so assume poleaxe spec
              -This allows easier comparison between weapon specs
             */
            if ((character.MainHand != null) &&
                ((character.MainHand.Type == Item.ItemType.TwoHandAxe)
                || (character.MainHand.Type == Item.ItemType.Polearm)) )
            {
                statsTotal.CritRating += (fCritRatingPerPercent * talents.PoleaxeSpecialization);
            }

            statsTotal.ArmorPenetrationRating = statsRace.ArmorPenetrationRating + statsGearEnchantsBuffs.ArmorPenetrationRating;
            statsTotal.ArmorPenetration = statsRace.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            if ((character.MainHand != null) &&
                character.MainHand.Type == Item.ItemType.TwoHandMace)
            {
                statsTotal.ArmorPenetrationRating += (fArmorPen * talents.MaceSpecialization * 3);
            }
                        
			CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            statsTotal.CritRating += (fCritRatingPerPercent * talents.Cruelty);
            statsTotal.CritRating += (fCritRatingPerPercent * 3);// Berserker Stance

            statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
			statsTotal.HitRating += (fHitRatingPerPercent * talents.Precision);

            statsTotal.ExpertiseRating = (statsRace.Expertise * fExpertiseRatingPerPercent) + statsGearEnchantsBuffs.ExpertiseRating;
            statsTotal.ExpertiseRating += talents.WeaponMastery * fExpertiseRatingPerPercent * 4f;

            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;

            statsTotal.DodgeRating = statsRace.DodgeRating + statsGearEnchantsBuffs.DodgeRating;
            statsTotal.DodgeRating = ((statsTotal.Agility / 20f) * 18.92f);
            statsTotal.ParryRating = statsRace.ParryRating + statsGearEnchantsBuffs.ParryRating;

            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;
            statsTotal.Bloodlust = statsGearEnchantsBuffs.Bloodlust;
            statsTotal.DrumsOfBattle = statsGearEnchantsBuffs.DrumsOfBattle;
            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;
            statsTotal.BonusDamageMultiplier = statsGearEnchantsBuffs.BonusDamageMultiplier;
            statsTotal.WindfuryAPBonus = statsGearEnchantsBuffs.WindfuryAPBonus;
            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;
            statsTotal.ShatteredSunMightProc = statsGearEnchantsBuffs.ShatteredSunMightProc;
            return (statsTotal);

           
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsDPSWarr baseCalc,  calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Item Budget":
                    Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { Strength = 16 } },
                        new Item() { Stats = new Stats() { Agility = 16 } },
                        new Item() { Stats = new Stats() { HitRating = 16 } },
                        new Item() { Stats = new Stats() { HasteRating = 16 } },
                        new Item() { Stats = new Stats() { CritRating = 16 } },
                        new Item() { Stats = new Stats() { ArmorPenetrationRating = 16 } },
                        new Item() { Stats = new Stats() { AttackPower = 32 } },
                        new Item() { Stats = new Stats() { ExpertiseRating = 16 } }
                    };
                    string[] statList = new string[] {
                        "16 Strength",
                        "16 Agility",
                        "16 Hit Rating",
                        "16 Haste Rating",
                        "16 Crit Rating",
                        "16 Armor Penetration Rating",
                        "32 Attack Power",
                        "16 Expertise Rating"
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSWarr;


                    for (int index = 0; index < statList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsDPSWarr;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = statList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                        }
                        comparison.SubPoints = subPoints;

                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }

        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Health = stats.Health,
                Stamina = stats.Stamina,
                Strength = stats.Strength,
                Agility = stats.Agility,
                AttackPower = stats.AttackPower,
				HitRating = stats.HitRating,
				CritRating = stats.CritRating,
				LotPCritRating = stats.LotPCritRating,
                HasteRating = stats.HasteRating,
                ArmorPenetrationRating = stats.ArmorPenetrationRating,
                ExpertiseRating = stats.ExpertiseRating,
                WeaponDamage = stats.WeaponDamage,
                Bloodlust = stats.Bloodlust,
                DrumsOfBattle = stats.DrumsOfBattle,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusCrusaderStrikeDamageMultiplier = stats.BonusCrusaderStrikeDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                WindfuryAPBonus = stats.WindfuryAPBonus,
                Armor = stats.Armor
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return ((stats.Strength +
                 stats.Agility +
                 stats.AttackPower +
                 stats.ArmorPenetration +
                 stats.ExpertiseRating +
                 stats.HasteRating +
                 stats.HitRating +
                 stats.CritRating +
                 stats.LotPCritRating +
                 stats.BonusStrengthMultiplier +
                 stats.BonusAttackPowerMultiplier +
                 stats.BonusPhysicalDamageMultiplier +
                 stats.BonusCritMultiplier +
                 stats.BonusCrusaderStrikeDamageMultiplier +
                 stats.WindfuryAPBonus +
                 stats.Bloodlust +
                 stats.ExposeWeakness +
                 stats.DrumsOfBattle +
                 stats.WeaponDamage) > 0); 
        }

    }
}
