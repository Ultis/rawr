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
        private float fMHWeaponSwingSpeed = 2.0f, fOHWeaponSwingSpeed = 2.0f;

        private int nCurrentRage = 0;
        private bool bBloodSurgeProcced = false, bSuddenDeathProcced = false, bOverpowerProcced = false;
        private int nFlurryActive = 0;
        private int nDeathWishCoolDown = 0;
        private int nDeathWishActive = -1;
        private int nBladeStormCoolDown = 0;
        
        private int nWhirlWindCoolDown = 0;
        private int nBloodthirstCoolDown = 0;
        private int nMortalStrikeCoolDown = 0;
        private int nOverpowerCoolDown = 0;
        private int nRendCharges = 0, nRendCoolDown = 0, nAngerManagementCoolDown = 0;

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
        public float nExecuteDamage = 0, nMortalStrikeDamage = 0, nOverpowerDamage = 0, nRendDamage = 0;
        
        public int nFlurryActiveCounter = 0;
        public int nHits = 0, nCrits = 0, nMisses = 0, nDodges = 0, nYellowMisses = 0;
        public int nDreadnoughtPieces = 0;
        
        private Stats stats;
        private CharacterCalculationsDPSWarr calc;
        private CalculationOptionsDPSWarr options;
        private WarriorTalents talents;
        private Character character;

        public void CheckSetBonus(Item a_item)
        {
            if (a_item != null && a_item.Name.Contains("Dreadnaught") && a_item.Stats.DefenseRating == 0)
                ++nDreadnoughtPieces;
        }

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
            CheckSetBonus( character.Head );
            CheckSetBonus(character.Shoulders);
            CheckSetBonus(character.Chest);
            CheckSetBonus(character.Hands);
            CheckSetBonus(character.Legs);

            fDamageModifier = 1.0f + (0.02f * talents.TwoHandedWeaponSpecialization) + stats.BonusPhysicalDamageMultiplier + (talents.WreckingCrew * 0.02f);
            float fHaste = stats.HasteRating;

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
            if (character.OffHand != null && talents.TitansGrip > 0)
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
            nExecuteDamage = 0;
            nMortalStrikeDamage = 0;
            nOverpowerDamage = 0; 
            nRendDamage = 0;
            nFlurryActiveCounter = 0;
            nHits = 0;
            nCrits=0;
            nMisses = 0;
            nDodges = 0;
            nYellowMisses = 0;
        }


        public void Reset()
        {
            fMHSwingTime = fMHSwingTimeNormal;
            fOHSwingTime = fOHSwingTimeNormal;
            bBloodSurgeProcced = false;
            bSuddenDeathProcced = false;
            nFlurryActive = -1;
            fSwingTimerMH = 0.0f;
            fSwingTimerOH = 0.0f;
            nDeathWishCoolDown = 0;
            nDeathWishActive = -1;
            nBloodthirstCoolDown = 0;
            nWhirlWindCoolDown = 0;
            nBloodlustCoolDown = 0;
            nMortalStrikeCoolDown = 0;
            nOverpowerCoolDown = 0;
            nRendCoolDown = 0;
            nRendCharges = 0;
            nBladeStormCoolDown = 0;
            nAngerManagementCoolDown = 0;

            nBloodlustActive = 0;
            fGCDTimer = 0.0f;
            bHeroicStrikeOnNextSwing = false;
            fDamageModifier = 1.0f + (0.02f * talents.TwoHandedWeaponSpecialization) + stats.BonusPhysicalDamageMultiplier + (talents.WreckingCrew * 0.02f);
        }

        public void DoRotation()
        {
            Reset();
            int nFightLenIterations = options.FightLength * 10;
            int nExecuteRange = (int)((float)nFightLenIterations * 0.8f);
            for (int nCurTime = 0; nCurTime < nFightLenIterations; ++nCurTime)
            {
                DoWhiteHitCalcs();
                if (options.ExecuteSpam && nCurTime > nExecuteRange)
                    DoExecuteSpamRotation();
                else if (options.SimMode == 0)
                    DoFuryRotation1();
                else if (options.SimMode == 1)
                    DoFuryRotation2();
                else if (options.SimMode == 2)
                    DoArmsRotation();
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
            if (talents.AngerManagement > 0)
            {
                if (nAngerManagementCoolDown > 0)
                    --nAngerManagementCoolDown;
                if (nAngerManagementCoolDown == 0)
                {
                    nAngerManagementCoolDown = (int)(3f / fTimeStep);
                    ++nCurrentRage;
                }
            }
            fSwingTimerOH += fTimeStep;
            if (fGCDTimer > 0)
                fGCDTimer -= fTimeStep;
            CheckCoolDowns();
        }

        public void CheckCoolDowns()
        {
            if (nDeathWishCoolDown > 0)
                --nDeathWishCoolDown;
            if (nDeathWishActive > 0)
                --nDeathWishActive;
            if (nBloodlustCoolDown > 0)
                --nBloodlustCoolDown;
            if (nBloodlustActive > 0)
                --nBloodlustActive;
            if (nDeathWishActive == 0)
            {
                fDamageModifier -= 0.2f;
                nDeathWishActive = -1;
            }
            if (nBloodlustCoolDown <= 0 && stats.Bloodlust > 0)
            {
                nBloodlustCoolDown = (int)(300.0f / fTimeStep);
                nBloodlustActive = (int)(45.0f / fTimeStep);
            }
            if (nCurrentRage > 10 && nDeathWishCoolDown <= 0 && talents.DeathWish > 0)
            {
                // Activate DeathWish
                fDamageModifier += 0.2f;
                nDeathWishCoolDown = (int)((180.0f - talents.IntensifyRage * 20.0f) / fTimeStep);
                nDeathWishActive = (int)(30.0f / fTimeStep);
                nCurrentRage -= 10;
                InvokeGCD();
            }
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
                bOverpowerProcced = true;
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
        
        public ETypeOfHit rollYellow(float a_fAdditionalCritChance )
        {
            double dRand = rnd.NextDouble(); // Zahl zwischen 0.0 und 1.0
            if (dRand <= fchanceToMissYellow)
            {
                ++nYellowMisses;
                return ETypeOfHit.eMiss;
            }
            else if (dRand <= fchanceToMissYellow + fchanceToDodge)
            {
                bOverpowerProcced = true;
                return ETypeOfHit.eDodge;
            }
            else if (dRand <= fchanceToMissYellow + fchanceToDodge + fchanceToCrit + a_fAdditionalCritChance)
                return ETypeOfHit.eCrit;
            return ETypeOfHit.eHit;
        }
        
        public void DoMainHandWhiteHit()
        {
            ETypeOfHit type = ETypeOfHit.eHit;
            if (bHeroicStrikeOnNextSwing)
                type = rollYellow(talents.Incite * 0.05f);
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
                nTempDamageDone += fBaseDamage * (2f + talents.Impale * 0.1f + stats.BonusCritMultiplier * 2.0f);
                GenerateDeepWoundMainHand();
                nFlurryActive = 4;
                if (bHeroicStrikeOnNextSwing && options.GlyphOfHeroicStrike)
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

            PossibleSuddenDeathProc();
/*            if (talents.SwordSpecialization > 0 && nSwordCoolDown <= 0 )
            {
                
            }*/
        }

        public void DoOffHandWhiteHit()
        {
            if (character.OffHand == null || talents.TitansGrip == 0 )
                return;
            ETypeOfHit type = rollWhite();
            float nTempDamageDone = 0;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += favgBaseOffHandWeaponHit;
            else if (type == ETypeOfHit.eGlance)
                nTempDamageDone += (favgBaseOffHandWeaponHit * 0.65f);
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += favgBaseOffHandWeaponHit * (2f + talents.Impale * 0.1f + stats.BonusCritMultiplier * 2.0f);
                GenerateDeepWoundOffHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier;
            GenerateRage(nTempDamageDone, character.OffHand.Speed, type == ETypeOfHit.eCrit ? 2.5f : 1.25f);
            nWhiteDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;

            PossibleSuddenDeathProc();
        }

        public bool DoWhirlWindMainHandHit(bool a_bOffHandCrit)
        {
            ETypeOfHit type = rollYellow(0.0f);
            float nTempDamageDone = 0;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += favgBaseMainHandWeaponHitNormalized;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += favgBaseMainHandWeaponHitNormalized * (2f + talents.Impale * 0.1f + stats.BonusCritMultiplier * 2.0f);
                if (!a_bOffHandCrit) // When both crit only offhand wounds tick
                    GenerateDeepWoundMainHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier * (1f + talents.ImprovedWhirlwind * 0.1f + talents.UnendingFury * 0.02f);
            nWhirlwindDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
            PossibleSuddenDeathProc();
            return type == ETypeOfHit.eCrit;
        }

        public bool DoWhirlWindOffHandHit()
        {
            ETypeOfHit type = rollYellow(0.0f);
            float nTempDamageDone = 0;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += favgBaseOffHandWeaponHitNormalized;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += favgBaseOffHandWeaponHitNormalized * (2f + talents.Impale * 0.1f + stats.BonusCritMultiplier * 2.0f);
                GenerateDeepWoundOffHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier * (1f + talents.ImprovedWhirlwind * 0.1f + talents.UnendingFury * 0.02f); 
            nWhirlwindDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
            PossibleSuddenDeathProc();
            return type == ETypeOfHit.eCrit;
        }

        public void DoBloodthirstHit()
        {
            if (talents.Bloodthirst == 0)
                return;
            ETypeOfHit type = rollYellow(0.0f);
            float nTempDamageDone = 0;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += stats.AttackPower / 2.0f;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += (stats.AttackPower / 2.0f) * (2f + talents.Impale * 0.1f + stats.BonusCritMultiplier * 2.0f);
                GenerateDeepWoundMainHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier * (1f + talents.UnendingFury * 0.02f); ;
            nBloodthirstDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
        }

        public void DoExecuteHit( float a_fRage )
        {
            ETypeOfHit type = rollYellow(0.0f);
            float nTempDamageDone = 0;
            float fbaseDamage = (stats.AttackPower * 0.2f) + 1456f + 38f * a_fRage;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += fbaseDamage;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += fbaseDamage * (2f + talents.Impale * 0.1f + stats.BonusCritMultiplier * 2.0f);
                GenerateDeepWoundMainHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier;
            nExecuteDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
            PossibleSuddenDeathProc();
        }

        public void SlamHit()
        {
            ETypeOfHit type = rollYellow(0.0f);
            float nTempDamageDone = 0;
            float fBaseDamage = favgBaseMainHandWeaponHit + 250f;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += fBaseDamage;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += fBaseDamage * (2f + talents.Impale * 0.1f + stats.BonusCritMultiplier * 2.0f);
                GenerateDeepWoundMainHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier;
            if (nDreadnoughtPieces >= 2)
                nTempDamageDone *= 1.1f;
            nSlamDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
        }

        public void MortalStrikeHit()
        {
            if (talents.MortalStrike == 0)
                return;
            ETypeOfHit type = rollYellow(0.0f);
            float nTempDamageDone = 0;
            float fBaseDamage = favgBaseMainHandWeaponHitNormalized + 380f;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += fBaseDamage;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += fBaseDamage * (2f + talents.Impale * 0.1f + stats.BonusCritMultiplier * 2.0f);
                GenerateDeepWoundMainHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier * ( 1f + (options.GlyphOfMortalStrike ? 0.1f : 0.0f) + talents.ImprovedMortalStrike * 0.0333f);
            nMortalStrikeDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
        }

        public void OverpowerHit()
        {
            ETypeOfHit type = rollYellow(talents.ImprovedOverpower * 0.25f);
            float nTempDamageDone = 0;
            float fBaseDamage = favgBaseMainHandWeaponHitNormalized + 380f;
            if (type == ETypeOfHit.eHit)
                nTempDamageDone += fBaseDamage;
            else if (type == ETypeOfHit.eCrit)
            {
                nTempDamageDone += fBaseDamage * (2f + talents.Impale * 0.1f + stats.BonusCritMultiplier * 2.0f);
                GenerateDeepWoundMainHand();
                nFlurryActive = 3;
            }
            nTempDamageDone *= fMitigation * fDamageModifier;
            nOverpowerDamage += nTempDamageDone;
            nDamageDone += nTempDamageDone;
        }
        
        public void GenerateDeepWoundMainHand()
        {
            float nDamage = (favgBaseMainHandWeaponHit * talents.DeepWounds * 0.16f * fDamageModifier * (1f + stats.BonusBleedDamageMultiplier)); // Assuming Mangle present
            if (nDeathWishActive > 0)
                nDamage *= 1.2f;
            nDeepWoundDamage += nDamage;
            nDamageDone += nDamage;
        }

        public void GenerateDeepWoundOffHand()
        {
            float nDamage = (favgBaseOffHandWeaponHit * talents.DeepWounds * 0.16f * fDamageModifier * (1f+stats.BonusBleedDamageMultiplier));
            if (nDeathWishActive > 0)
                nDamage *= 1.2f;
            nDeepWoundDamage += nDamage;
            nDamageDone += nDamage;
        }

        public void DoRendTick()
        {
            // 380 + [ 7.14% of MWS * AP + 50% of MWB + 50% of mwb ]
            float nDamage = (380f + (stats.AttackPower * 0.0714f * fMHWeaponSwingSpeed) + favgBaseMainHandWeaponHit) * fDamageModifier * (1f + stats.BonusBleedDamageMultiplier) / 5f;
            nDamage *= 1f + (talents.ImprovedRend * 0.1f);
            nRendDamage += nDamage;
            nDamageDone += nDamage;
        }

        public void GenerateRage( float a_fDamage, float a_fWeaponSpeed, float a_fWeaponFactor )
        {
            // ((Damage Dealt) / (Rage Conversion at Your Level) * 7.5 + (Weapon Speed * Factor))/2;
            if (nCurrentRage < 0)
                nCurrentRage = 0;
            int nRageGen = (int)((a_fDamage / 320.62f * 7.5f + a_fWeaponSpeed * a_fWeaponFactor) / 2f);
            if (talents.UnbridledWrath > 0)
            {
                float fChance = 0.16f * talents.UnbridledWrath;
                if (rnd.NextDouble() < fChance)
                    nRageGen += 1;
            }
            if (talents.EndlessRage > 0)
                nRageGen = (int) (1.25f * (float)nRageGen);
            nCurrentRage += nRageGen;
            if (nCurrentRage > 100)
                nCurrentRage = 100;
        }

        public void PossibleBloodSurgeProc()
        {
            if (talents.Bloodsurge == 3)
            {
                if (rnd.NextDouble() < 0.2f)
                    bBloodSurgeProcced = true;
            }
            else if( talents.Bloodsurge > 0)
            {
                if(rnd.NextDouble() < (0.07f * talents.Bloodsurge))
                    bBloodSurgeProcced = true;
            }
        }

        public void PossibleSuddenDeathProc()
        {
            if (talents.SuddenDeath > 0)
            {
                if (rnd.NextDouble() < (0.03f * talents.SuddenDeath))
                    bSuddenDeathProcced = true;
            }
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
            if (nWhirlWindCoolDown > 0)
                --nWhirlWindCoolDown;
            if (nBloodthirstCoolDown > 0)
                --nBloodthirstCoolDown;

            bool bOnGCD = fGCDTimer > 0.0f;
            if (bOnGCD)
                return;
            int nHSRageCost = 15 - talents.ImprovedHeroicStrike;

            if (nCurrentRage > 25 && nWhirlWindCoolDown <= 0 )
            {
                if( options.GlyphOfWhirlwind )
                    nWhirlWindCoolDown = (int)(8.0f / fTimeStep);
                else
                    nWhirlWindCoolDown = (int)(10.0f / fTimeStep);
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
        public void DoFuryRotation2()
        {
            if (nWhirlWindCoolDown > 0)
                --nWhirlWindCoolDown;
            if (nBloodthirstCoolDown > 0)
                --nBloodthirstCoolDown;

            bool bOnGCD = fGCDTimer > 0.0f;
            if (bOnGCD)
                return;
            int nHSRageCost = 15 - talents.ImprovedHeroicStrike;

            if (nCurrentRage > 15 && bBloodSurgeProcced)
            {
                // slam!
                nCurrentRage -= 15;
                bBloodSurgeProcced = false;
                SlamHit();
                InvokeGCD();
            }
            else if (nCurrentRage > 25 && nWhirlWindCoolDown <= 0)
            {
                if (options.GlyphOfWhirlwind)
                    nWhirlWindCoolDown = (int)(8.0f / fTimeStep);
                else
                    nWhirlWindCoolDown = (int)(10.0f / fTimeStep);
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
            else if (nCurrentRage > nHSRageCost && nCurrentRage > options.HeroicStrikeRage && !bHeroicStrikeOnNextSwing)
            {
                nCurrentRage -= nHSRageCost;
                bHeroicStrikeOnNextSwing = true;
            }
        }
        public void DoArmsRotation()
        {
            if (nMortalStrikeCoolDown > 0)
                --nMortalStrikeCoolDown;
            if (nOverpowerCoolDown > 0)
                --nOverpowerCoolDown;
            if (nRendCoolDown > 0)
                --nRendCoolDown;
            if (nRendCoolDown == 0 && nRendCharges > 0)
            {
                nRendCoolDown = (int)(3f / fTimeStep);
                --nRendCharges;
                DoRendTick();
                if (talents.TasteForBlood > 0)
                {
                    float fTasteForBlood = talents.TasteForBlood * 0.1f;
                    if (rnd.NextDouble() < fTasteForBlood)
                        bOverpowerProcced = true;
                }
            }
            bool bOnGCD = fGCDTimer > 0.0f;
            if (bOnGCD)
                return;
            int nExecuteRageCost = 15;
            if (talents.ImprovedExecute == 1)
                nExecuteRageCost -= 2;
            if (talents.ImprovedExecute == 2)
                nExecuteRageCost -= 5;
            if (nCurrentRage > 10 && nRendCharges <= 0)
            {
                nRendCharges = 5 + (options.GlyphOfRend ? 1 : 0);
                nRendCoolDown = (int)(3f / fTimeStep);
                nCurrentRage -= 10;
                InvokeGCD();
            }
            else if (nCurrentRage > 25 && nBladeStormCoolDown <= 0 && talents.Bladestorm > 0)
            {
                nBladeStormCoolDown = (int)( 90f / fTimeStep);
                nCurrentRage -= 25;
                for (int i = 0; i < 6; ++i)
                {
                    DoWhirlWindMainHandHit(false);
                    PossibleSuddenDeathProc();
                }
                fGCDTimer = 6.0f;
            }
            else if (nCurrentRage > nExecuteRageCost && bSuddenDeathProcced)
            {
                nCurrentRage -= nExecuteRageCost;
                bSuddenDeathProcced = false;
                int nRageUsed = System.Math.Max(nCurrentRage, 30);
                DoExecuteHit(nRageUsed + (options.GlyphOfExecute ? 10f : 0f));
                nCurrentRage -= nRageUsed;
                if (nCurrentRage < 10)
                    nCurrentRage = 10;
                InvokeGCD();
                PossibleSuddenDeathProc();
            }
            else if (nCurrentRage > 30 && nMortalStrikeCoolDown <= 0)
            {
                nMortalStrikeCoolDown = (int)((6.0f - talents.ImprovedMortalStrike * 0.333f) / fTimeStep);
                nCurrentRage -= 30;
                MortalStrikeHit();
                InvokeGCD();
                PossibleSuddenDeathProc();
            }
            else if (nCurrentRage > 5 && nOverpowerCoolDown <= 0 && bOverpowerProcced)
            {
                nOverpowerCoolDown = (int)((5.0f - talents.UnrelentingAssault * 2.0f) / fTimeStep);
                bOverpowerProcced = false;
                nCurrentRage -= 5;
                OverpowerHit();
                fGCDTimer = 1.0f;
                PossibleSuddenDeathProc();
            }
            else if (nCurrentRage > 15 && talents.ImprovedSlam > 0 && nCurrentRage > options.HeroicStrikeRage )
            {
                // slam!
                nCurrentRage -= 15;
                fSwingTimerMH -= (1.5f - talents.ImprovedSlam * 0.5f);
                fSwingTimerOH -= (1.5f - talents.ImprovedSlam * 0.5f);
                SlamHit();
                InvokeGCD();
                PossibleSuddenDeathProc();
            }

        }

        public void DoExecuteSpamRotation()
        {
            bool bOnGCD = fGCDTimer > 0.0f;
            if (bOnGCD)
                return;
            int nExecuteRageCost = 15;
            if (talents.ImprovedExecute == 1)
                nExecuteRageCost -= 2;
            if (talents.ImprovedExecute == 2)
                nExecuteRageCost -= 5;
            if ( nCurrentRage > nExecuteRageCost )
            {
                nCurrentRage -= nExecuteRageCost;
                DoExecuteHit(nCurrentRage + (options.GlyphOfExecute ? 10f : 0f));
                nCurrentRage = 0;
                InvokeGCD();
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
					"Basic Stats:Expertise",
					"Basic Stats:Haste",
					"Basic Stats:Armor Penetration Rating",
					"Basic Stats:Armor Mitigation",
					"Basic Stats:Flurry Uptime",
                    "DPS Result:Total DPS",
                    "DPS Breakdown:White DPS",
                    "DPS Breakdown:Heroic Strike DPS",
                    "DPS Breakdown:Deep Wounds DPS",
                    "DPS Breakdown:Whirlwind DPS",
                    "DPS Breakdown:Bloodthirst DPS",
                    "DPS Breakdown:Slam DPS",
                    "DPS Breakdown:Execute DPS",
                    "DPS Breakdown:Mortal Strike DPS",
                    "DPS Breakdown:Overpower DPS",
                    "DPS Breakdown:Rend DPS",
                    "Simulation Results (200xFight Length):White Hits",
                    "Simulation Results (200xFight Length):White Crits",
                    "Simulation Results (200xFight Length):White Dodges",
                    "Simulation Results (200xFight Length):White Misses"
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
                        Item.ItemType.TwoHandSword
					}));
            }
        }

        public override bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot)
        {
            if (character.CurrentTalents is WarriorTalents)
            {
                CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;

                if (item.ItemLevel < 200 && !item.IsGem && item.ItemLevel > 0 && calcOpts.HideLowQualityItems )
                    return false;
                if ((item.ItemLevel < 80 || (item.Quality == Item.ItemQuality.Epic && !item.IsJewelersGem)) && item.IsGem && calcOpts.HideLowQualityItems)
                    return false;

                WarriorTalents talents = (WarriorTalents)character.CurrentTalents;
                if (talents.TitansGrip > 0 &&
                    (item.Type == Item.ItemType.TwoHandAxe || item.Type == Item.ItemType.TwoHandSword || item.Type == Item.ItemType.TwoHandMace) &&
                    slot == Character.CharacterSlot.OffHand)
                {
                    return true;
                }
                if (talents.TitansGrip > 0 && slot == Character.CharacterSlot.MainHand && item.Type == Item.ItemType.Polearm)
                    return false;
            }
            return item.FitsInSlot(slot);
        }

        public override bool IncludeOffHandInCalculations(Character character)
        {
            if (character.CurrentTalents is WarriorTalents )
            {
                WarriorTalents talents = (WarriorTalents)character.CurrentTalents;
                if (talents.TitansGrip > 0)
                    return true;
            }
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

            if (!(character.CurrentTalents is WarriorTalents))
                return calcs;

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
                calcs.MSDPSPoints += calcData.nMortalStrikeDamage / calcOpts.FightLength / nIterations;
                calcs.ExecuteDPSPoints += calcData.nExecuteDamage / calcOpts.FightLength / nIterations;
                calcs.OverpowerDPSPoints += calcData.nOverpowerDamage / calcOpts.FightLength / nIterations;
                calcs.RendDPSPoints += calcData.nRendDamage / calcOpts.FightLength / nIterations;
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
            if (!(character.CurrentTalents is WarriorTalents))
                return statsBaseGear;
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
            statsTotal.Armor = statsRace.Armor + statsGearEnchantsBuffs.Armor + statsGearEnchantsBuffs.BonusArmor + statsRace.BonusArmor;
            float fBerserkAPProc = 0.0f;
            if (statsGearEnchantsBuffs.BerserkingProc > 0.0f)
            {
                if (character.MainHandEnchant.Name == "Berserking")
                {
                    float fBerserkingUptime = 0.4f;
                    statsTotal.Armor *= (1f - (0.05f * fBerserkingUptime));
                    fBerserkAPProc = 400f * fBerserkingUptime;
                }
                if (character.OffHandEnchant.Name == "Berserking")
                {
                    float fBerserkingUptime = 0.3f;
                    statsTotal.Armor *= (1f - (0.05f * fBerserkingUptime));
                    fBerserkAPProc += 400f * fBerserkingUptime;
                }
            }
            
            statsTotal.Strength = (strBase + (float)Math.Floor((strBase * statsBuffs.BonusStrengthMultiplier) + strBonus * (1 + statsBuffs.BonusStrengthMultiplier)));
            if (talents.StrengthOfArms > 0)
                statsTotal.Strength *= 1f + (talents.StrengthOfArms * 0.02f);

            statsTotal.Stamina = (staBase + (float)Math.Round((staBase * statsBuffs.BonusStaminaMultiplier) + staBonus * (1 + statsBuffs.BonusStaminaMultiplier)));
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + ((statsTotal.Stamina-staBase) * 10f))));
            statsTotal.AttackPower = (float)Math.Floor((statsRace.AttackPower + statsGearEnchantsBuffs.AttackPower + fBerserkAPProc +
                statsTotal.Armor / 180.0f * talents.ArmoredToTheTeeth + (statsTotal.Strength * 2)) * (1f + statsTotal.BonusAttackPowerMultiplier));

            statsTotal.AttackPower *= (1.0f + talents.ImprovedBerserkerStance * 0.02f);

            statsTotal.CritRating = statsRace.PhysicalCrit + statsGearEnchantsBuffs.CritRating;
            statsTotal.CritRating += ((statsTotal.Agility / fAgiPerCritPercent) * fCritRatingPerPercent);
            statsTotal.CritRating += fCritRatingPerPercent * 3.2f; // added 3.2%, +5% lotp, +3% retpally, -4.8% boss crit chance reduction.
            
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
                statsTotal.ArmorPenetrationRating += (fArmorPen * talents.MaceSpecialization * 3f);
            }
                        
			CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            statsTotal.CritRating += (fCritRatingPerPercent * talents.Cruelty);
            statsTotal.CritRating += (fCritRatingPerPercent * 3);// Berserker Stance

            statsTotal.HitRating = statsRace.HitRating + statsGearEnchantsBuffs.HitRating;
			statsTotal.HitRating += (fHitRatingPerPercent * talents.Precision);
            statsTotal.HitRating += statsGearEnchantsBuffs.PhysicalHit * 100f * fHitRatingPerPercent;

            statsTotal.ExpertiseRating = (statsRace.Expertise * fExpertiseRatingPerPercent) + statsGearEnchantsBuffs.ExpertiseRating;
            statsTotal.ExpertiseRating += talents.WeaponMastery * fExpertiseRatingPerPercent * 4f;
            statsTotal.ExpertiseRating += talents.StrengthOfArms * fExpertiseRatingPerPercent * 2f;

            statsTotal.HasteRating = statsRace.HasteRating + statsGearEnchantsBuffs.HasteRating;
            statsTotal.HasteRating += (statsGearEnchantsBuffs.PhysicalHaste + statsRace.PhysicalHaste) * 100.0f * fHastePerPercent;
            statsTotal.HasteRating += talents.BloodFrenzy * 3f * fHastePerPercent;

            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;
            statsTotal.BonusBleedDamageMultiplier = statsGearEnchantsBuffs.BonusBleedDamageMultiplier;
            statsTotal.Bloodlust = statsGearEnchantsBuffs.Bloodlust;
            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;
            statsTotal.BonusDamageMultiplier = statsGearEnchantsBuffs.BonusDamageMultiplier;

            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;
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
                        new Item() { Stats = new Stats() { Strength = 20 } },
                        new Item() { Stats = new Stats() { Agility = 20 } },
                        new Item() { Stats = new Stats() { HitRating = 20 } },
                        new Item() { Stats = new Stats() { HasteRating = 20 } },
                        new Item() { Stats = new Stats() { CritRating = 20 } },
                        new Item() { Stats = new Stats() { ArmorPenetrationRating = 20 } },
                        new Item() { Stats = new Stats() { AttackPower = 40 } },
                        new Item() { Stats = new Stats() { ExpertiseRating = 20 } }
                    };
                    string[] statList = new string[] {
                        "20 Strength",
                        "20 Agility",
                        "20 Hit Rating",
                        "20 Haste Rating",
                        "20 Crit Rating",
                        "20 Armor Penetration Rating",
                        "40 Attack Power",
                        "20 Expertise Rating"
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
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Agility = stats.Agility,
                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                CritRating = stats.CritRating,
                HitRating = stats.HitRating,
                Stamina = stats.Stamina,
                HasteRating = stats.HasteRating,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                ArmorPenetrationRating = stats.ArmorPenetrationRating,
                BloodlustProc = stats.BloodlustProc,
                WeaponDamage = stats.WeaponDamage,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                Health = stats.Health,
                ExposeWeakness = stats.ExposeWeakness,
                Bloodlust = stats.Bloodlust,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                MongooseProc = stats.MongooseProc,
                BerserkingProc = stats.BerserkingProc
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            if (stats.SpellPower > 0)
                return false;
            return ((stats.Strength +
                 stats.Agility +
                 stats.Armor +
                 stats.BonusArmor +
                 stats.AttackPower +
                 stats.ArmorPenetration +
                 stats.ArmorPenetrationRating +
                 stats.ExpertiseRating +
                 stats.HasteRating +
                 stats.HitRating +
                 stats.CritRating +
                 stats.BonusBleedDamageMultiplier +
                 stats.BonusStrengthMultiplier +
                 stats.BonusAttackPowerMultiplier +
                 stats.BonusPhysicalDamageMultiplier +
                 stats.BonusCritMultiplier +
                 stats.Bloodlust +
                 stats.ExposeWeakness +
                 stats.PhysicalHit + 
                 stats.MongooseProc +
                 stats.PhysicalHaste + 
                 stats.BerserkingProc +
                 stats.WeaponDamage) > 0); 
        }

    }
}
