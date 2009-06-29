using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    class CombatTable
    {
        public Character character;
        public CharacterCalculationsTankDK calcs;
        public DeathKnightTalents talents;
        public Stats stats;
        public CalculationOptionsTankDK calcOpts;

        public Weapon MH, OH;

        public float combinedSwingTime;

        public float physCrits, 
            missedSpecial, dodgedSpecial, 
            spellCrits, spellResist, 
            totalMHMiss, totalOHMiss,
            realDuration, totalMeleeAbilities, totalParryableAbilities, 
        totalSpellAbilities, normalizationFactor;
        private float fDuration;
        public float m_fRSCount = 0f;
        public Rotation.Type rotType;

        public CombatTable(Character c, Stats stats, CalculationOptionsTankDK calcOpts) :
            this(c, new CharacterCalculationsTankDK(), stats, calcOpts)
        {
        }

        public CombatTable(Character c, CharacterCalculationsTankDK calcs, Stats stats, CalculationOptionsTankDK calcOpts)
        {
            this.character = c;
            this.calcs = calcs;
            this.calcOpts = calcOpts;
            this.calcOpts.talents = character.DeathKnightTalents;
            this.talents = character.DeathKnightTalents;
            this.stats = stats;
            totalMeleeAbilities = 0f;
            totalSpellAbilities = 0f;

            totalMeleeAbilities = calcOpts.m_Rotation.PlagueStrike + calcOpts.m_Rotation.ScourgeStrike +
                calcOpts.m_Rotation.Obliterate + calcOpts.m_Rotation.BloodStrike + calcOpts.m_Rotation.HeartStrike +
                calcOpts.m_Rotation.FrostStrike + calcOpts.m_Rotation.DeathStrike;

            totalSpellAbilities = calcOpts.m_Rotation.DeathCoil + calcOpts.m_Rotation.IcyTouch + calcOpts.m_Rotation.HowlingBlast
                + calcOpts.m_Rotation.UnholyBlight + calcOpts.m_Rotation.Pestilence + calcOpts.m_Rotation.Horn + calcOpts.m_Rotation.DeathNDecay
                + calcOpts.m_Rotation.BoneShield;

            totalParryableAbilities = calcOpts.m_Rotation.PlagueStrike + calcOpts.m_Rotation.ScourgeStrike +
                calcOpts.m_Rotation.Obliterate + calcOpts.m_Rotation.BloodStrike + calcOpts.m_Rotation.HeartStrike;

            Weapons();
            CritsAndResists();
            fDuration = calcOpts.m_Rotation.getGCDTime();

            rotType = calcOpts.m_Rotation.curRotationType;
        }

        public void CritsAndResists()
        {
            #region Crits, Resists
            {
                // Attack Rolltable (DW):
                // 27.0% miss     (8.0% with 2H)
                //  6.5% dodge
                // 24.0% glancing (75% hit-dmg)
                // xx.x% crit
                // remaining = hit

                // Crit: Base .65%
                physCrits = .0065f;
                physCrits += stats.PhysicalCrit;
                calcs.CritChance = physCrits;

                float chanceAvoided = 0.335f;

                float chanceDodged = 0.065f;

                calcs.DodgedMHAttacks = MH.chanceDodged;
                calcs.DodgedOHAttacks = OH.chanceDodged;

                if (character.MainHand != null)
                {
                    chanceDodged = MH.chanceDodged;
                }

                if (character.OffHand != null)
                {
                    if (character.MainHand != null)
                    {
                        chanceDodged += OH.chanceDodged;
                        chanceDodged /= 2;
                    }
                    else
                    {
                        chanceDodged = OH.chanceDodged;
                    }
                }

                calcs.TargetDodge = chanceDodged;

                float chanceMiss = .08f;
                if (character.OffHand != null)
                {
                    chanceMiss = .27f;
                }
                chanceMiss -= stats.PhysicalHit;
                chanceMiss = Math.Max(0f, chanceMiss);
                calcs.TargetMiss = chanceMiss;

                chanceAvoided = chanceDodged + chanceMiss;
                calcs.AvoidedAttacks = chanceDodged + chanceMiss;

                chanceDodged = MH.chanceDodged;
                missedSpecial = chanceMiss;
                dodgedSpecial = chanceDodged;
                // calcs.MissedAttacks = chanceMiss           

                spellCrits = 0f;
                spellCrits += stats.SpellCrit;
                calcs.SpellCritChance = spellCrits;

                // Resists: Base 17%
                spellResist = .17f;
                spellResist -= stats.SpellHit;
                spellResist = Math.Max(0f, spellResist);

                // Total physical misses
                totalMHMiss = calcs.DodgedMHAttacks + chanceMiss;
                totalOHMiss = calcs.DodgedOHAttacks + chanceMiss;
                realDuration = calcOpts.m_Rotation.curRotationDuration;
                realDuration += ((totalMeleeAbilities - calcOpts.m_Rotation.FrostStrike) * chanceDodged * (1.5f)) +
                    ((totalMeleeAbilities - calcOpts.m_Rotation.FrostStrike) * chanceMiss * (1.5f)) +
                    ((calcOpts.m_Rotation.IcyTouch * spellResist * (((1.5f) / (1 + (StatConversion.GetHasteFromRating(stats.HasteRating, Character.CharacterClass.DeathKnight)) + stats.SpellHaste)) <= 1.0f ? 1.0f : (((1.5f) / (1 + (StatConversion.GetHasteFromRating(stats.HasteRating, Character.CharacterClass.DeathKnight)) + stats.SpellHaste)))))); 
                //still need to implement spellhaste here
            }
            #endregion
        }

        public void Weapons()
        {
            float MHExpertise = stats.Expertise;
            float OHExpertise = stats.Expertise;

            if (character.Race == Character.CharacterRace.Dwarf)
            {
                if (character.MainHand != null &&
                    (character.MainHand.Item.Type == Item.ItemType.OneHandMace ||
                     character.MainHand.Item.Type == Item.ItemType.TwoHandMace))
                {
                    MHExpertise += 5f;
                }

                if (character.OffHand != null && character.OffHand.Item.Type == Item.ItemType.OneHandMace)
                {
                    OHExpertise += 5f;
                }
            }
            else if (character.Race == Character.CharacterRace.Orc)
            {
                if (character.MainHand != null &&
                    (character.MainHand.Item.Type == Item.ItemType.OneHandAxe ||
                     character.MainHand.Item.Type == Item.ItemType.TwoHandAxe))
                {
                    MHExpertise += 5f;
                }

                if (character.OffHand != null && character.OffHand.Item.Type == Item.ItemType.OneHandAxe)
                {
                    OHExpertise += 5f;
                }
            }
            if (character.Race == Character.CharacterRace.Human)
            {
                if (character.MainHand != null &&
                    (character.MainHand.Item.Type == Item.ItemType.OneHandSword ||
                     character.MainHand.Item.Type == Item.ItemType.TwoHandSword ||
                     character.MainHand.Item.Type == Item.ItemType.OneHandMace ||
                     character.MainHand.Item.Type == Item.ItemType.TwoHandMace))
                {
                    MHExpertise += 3f;
                }

                if (character.OffHand != null &&
                    (character.OffHand.Item.Type == Item.ItemType.OneHandSword ||
                    character.OffHand.Item.Type == Item.ItemType.OneHandMace))
                {
                    OHExpertise += 3f;
                }
            }


            MH = new Weapon(null, stats, calcOpts, 0f);
            OH = new Weapon(null, stats, calcOpts, 0f);

            if (character.MainHand != null)
            {
                MH = new Weapon(character.MainHand.Item, stats, calcOpts, MHExpertise);
                calcs.MHAttackSpeed = MH.hastedSpeed;
                calcs.MHWeaponDamage = MH.damage;
                calcs.MHExpertise = MH.effectiveExpertise;
                if (character.MainHand.Item.Type == Item.ItemType.TwoHandAxe
                    || character.MainHand.Item.Type == Item.ItemType.TwoHandMace
                    || character.MainHand.Item.Type == Item.ItemType.TwoHandSword
                    || character.MainHand.Item.Type == Item.ItemType.Polearm)
                {
                    normalizationFactor = 3.3f;
                    MH.damage *= 1f + .02f * talents.TwoHandedWeaponSpecialization;
                    combinedSwingTime = MH.hastedSpeed;
                    calcs.OHAttackSpeed = 0f;
                    calcs.OHWeaponDamage = 0f;
                    calcs.OHExpertise = 0f;
                }
                else normalizationFactor = 2.4f;
            }

            if (character.OffHand != null)
            {
                OH = new Weapon(character.OffHand.Item, stats, calcOpts, OHExpertise);

                float OHMult = .05f * (float)talents.NervesOfColdSteel;
                OH.damage *= .5f + OHMult;

                //need this for weapon swing procs
                //combinedSwingTime = 1f / MH.hastedSpeed + 1f / OH.hastedSpeed;
                //combinedSwingTime = 1f / combinedSwingTime;
                combinedSwingTime = (MH.hastedSpeed + OH.hastedSpeed) / 4;
                calcs.OHAttackSpeed = OH.hastedSpeed;
                calcs.OHWeaponDamage = OH.damage;
                calcs.OHExpertise = OH.effectiveExpertise;
            }

            if (character.MainHand == null && character.OffHand == null)
            {
                combinedSwingTime = 2f;
                normalizationFactor = 2.4f;
            }
        }

        public float GetTotalThreat()
        {
            bool DW = (character.MainHand != null
                        && character.OffHand != null
                        && character.MainHand.Type != Item.ItemType.TwoHandAxe
                        && character.MainHand.Type != Item.ItemType.TwoHandMace
                        && character.MainHand.Type != Item.ItemType.TwoHandSword
                        && character.MainHand.Type != Item.ItemType.Polearm);

            //DPS Subgroups
            float fDamWhite = 0f;
            float fDamBCB = 0f;
            float fDamNecrosis = 0f;
            float fDamDeathCoil = 0f;
            float fDamIcyTouch = 0f;
            float fDamPlagueStrike = 0f;
            float fDamFrostFever = 0f;
            float fDamBloodPlague = 0f;
            float fDamScourgeStrike = 0f;
            float fDamUnholyBlight = 0f;
            float fDamFrostStrike = 0f;
            float fDamHowlingBlast = 0f;
            float fDamDeathNDecay = 0f;
            float fDamObliterate = 0f;
            float fDamDeathStrike = 0f;
            float fDamBloodStrike = 0f;
            float fDamHeartStrike = 0f;
            float fDamDancingRuneWeapon = 0f;
            float fDamWanderingPlague = 0f;
            float fDamWPFromFF = 0f;
            float fDamWPFromBP = 0f;
            float fDamOtherShadow = 0f;
            float fDamOtherArcane = 0f;
            float fDamOtherFrost = 0f;
            float fDamBloodworms = 0f;
            float fDamRuneStrike = 0f;

            float missedSpecial = 0f;

            float fDamWhiteBeforeArmor = 0f;
            float fDamWhiteMinusGlancing = 0f;
            float fightDuration = calcOpts.FightLength * 60;
            float mitigation;
            float KMRatio = 0f;
            float CinderglacierMultiplier = 1f;

            float MHExpertise = stats.Expertise;
            float OHExpertise = stats.Expertise;

            //damage multipliers
            float spellPowerMult = 1f + stats.BonusSpellPowerMultiplier;
            float frostSpellPowerMult = 1f + stats.BonusSpellPowerMultiplier + Math.Max((stats.BonusFrostDamageMultiplier - stats.BonusShadowDamageMultiplier), 0f); //implement razorice here later

            float physPowerMult = 1f + stats.BonusPhysicalDamageMultiplier;
            // Covers all % physical damage increases.  Blood Frenzy, FI.
            float partialResist = 0.94f; // Average of 6% damage lost to partial resists on spells

            /* Coefficients As of 3.1:
             * http://www.tankspot.com/forums/f200/41215-dk-attack-power-coefficients.html
             * Attack Power Coefficients table:
             * Ability 	Coefficient 
             * Blood Boil	0.04
                Blood Plague	0.055
                Bloodworms	0.006
                Corpse Explosion	0.0475
                Death and Decay	0.0475
                Death Coil	0.15
                Frost Fever	0.055
                Howling Blast	0.1
                Summon Gargoyle	0.4
                Icy Touch	0.1
                Pestilence	0.04
                Strangulate	0.06
                Unholy Blight	0.013
            */

            //spell AP multipliers, for diseases its per tick
            float BloodBoilAPMult = .04f;
            float BloodPlagueAPMult = 0.055f;
            float CorpseExplosionAPMult = .0475f;
            float DeathCoilAPMult = 0.15f;
            float DeathNDecayAPMult = .0475f;
            float FrostFeverAPMult = 0.055f;
            float GargoyleAPMult = 0.4f;
            float HowlingBlastAPMult = 0.1f;
            float IcyTouchAPMult = 0.1f;
            float StrangulateAPMult = .06f;
            float UnholyBlightAPMult = 0.013f;

            //for estimating rotation pushback

            calcOpts.m_Rotation.avgDiseaseMult = calcOpts.m_Rotation.numDisease * (calcOpts.m_Rotation.diseaseUptime / 100);
            float commandMult = 0f;

            if (calcOpts.m_Rotation.managedRP)
            {
                calcOpts.m_Rotation.getRP(talents, character);
            }

            /* Threat table as of 3.0.8
             * http://www.tankspot.com/forums/f200/40485-death-knight-threat-values.html
            Anti-Magic Shell ___________________  0
            Anti-Magic Shell RP gain ___________  5 per RP (approx), unaffected by presence.  See * below.
            Anti-Magic Zone ____________________  0
            Abomination's Might ________________  0
            Blood Aura _________________________  0
            Blood Boil _________________________  damage
            Blood Caked Blade __________________  damage
            Blood Plague _______________________  damage
            Blood Presence reactive healing ____  0
            Blood Strike _______________________  damage
            Blood Tap __________________________  0
            Bloody Vengeance ___________________  0
            Bloodworms _________________________  0 The DK gains no threat from BW's damage or healing.  See ** below.
            Bone Shield ________________________  0
            Butchery RP gains on kills _________  5 per RP, unaffected by presence
            Chains of Ice ______________________  226
            Chilblains (all ranks) _____________  2
            Chill of the Grave RP gains ________  5 per RP, unaffected by presence
            Corpse Explosion ___________________  damage
            Crypt Fever ________________________  0
            Dancing Rune Weapon ________________  0
            Death and Decay ____________________  damage × 1.90
            Deathchill _________________________  55, split between all mobs
            Death Coil _________________________  damage
            Death Coil healing _________________  healing × 0.5 × presence, split between all mobs
            Death Grip _________________________  110
            Death Pact _________________________  0
            Death Strike _______________________  damage
            Death Strike healing _______________  healing × 0.5 × presence, split between all mobs
            Desecration ________________________  0
            Dirge RP gains _____________________  5 per RP, unaffected by presence
            Ebon Plaguebringer _________________  0
            Empower Rune Weapon ________________  0
            Frost Fever ________________________  damage
            Frost Strike _______________________  damage
            Heart Strike _______________________  damage
            Horn of Winter _____________________  75 ÷ number of units buffed, split between mobs
            Howling Blast ______________________  damage
            Hungering Cold _____________________  112, split between all mobs affected
            Hysteria ___________________________  55 (the DK gains the threat, not the target)
            Icebound Fortitude _________________  0
            Icy Talons _________________________  0
            Improved Icy Talons ________________  0
            Icy Touch __________________________  damage
            Killing Maching ____________________  0
            Lichborne __________________________  110, split between all mobs
            Mark of Blood ______________________  0
            Mind Freeze ________________________  0
            Obliterate _________________________  damage
            Pestilence _________________________  damage + (100 split between all mobs hit)
            Plague Strike ______________________  damage
            Raise Dead _________________________  57, split between mobs
            Rime _______________________________  0
            Rune Strike ________________________  damage × 1.5
            Rune Tap ___________________________  healing × 0.5 + 55, split between all mobs
            Scent of Blood _____________________  0
            Scent of Blood RP gains ____________  0
            Scourge Strike _____________________  damage + 120
            Strangulate ________________________  158 on application, damage at the end
            Unholy Blight ______________________  damage
            Unholy Strength ____________________  healing × 0.5 × presence, split between all mobs
            Unbreakable Armor __________________  0
            Vampiric Blood _____________________  0
            Vendetta healing ___________________  0
            Wandering Plague ___________________  damage
            */
            #region Impurity Application
            {
                // TALENT: Impurity
                float impurityMult = 1f + (.05f * (float)talents.Impurity);

                HowlingBlastAPMult *= impurityMult;
                IcyTouchAPMult *= impurityMult;
                FrostFeverAPMult *= impurityMult;
                BloodPlagueAPMult *= impurityMult;
                DeathCoilAPMult *= impurityMult;
                UnholyBlightAPMult *= impurityMult;
                GargoyleAPMult *= impurityMult;
            }
            #endregion

            #region racials
            {
                if (character.Race == Character.CharacterRace.Orc)
                {
                    commandMult += .05f;
                }
            }
            #endregion

            #region Killing Machine
            {
                float KMPpM = (1f * talents.KillingMachine) * (1f + stats.PhysicalHaste); // KM Procs per Minute (Defined "1 per point" by Blizzard) influenced by Phys. Haste

                float KMPpR = KMPpM / (60 / calcOpts.m_Rotation.curRotationDuration);
                float totalAbilities = calcOpts.m_Rotation.FrostStrike + calcOpts.m_Rotation.IcyTouch + calcOpts.m_Rotation.HowlingBlast;
                KMRatio = KMPpR / totalAbilities;
            }
            #endregion

            #region Cinderglacier
            {
                float shadowFrostAbilitiesPerSecond = ((calcOpts.m_Rotation.DeathCoil + calcOpts.m_Rotation.FrostStrike +
                        calcOpts.m_Rotation.ScourgeStrike + calcOpts.m_Rotation.IcyTouch + calcOpts.m_Rotation.HowlingBlast) /
                        this.realDuration);

                CinderglacierMultiplier *= 1f + (0.2f / (shadowFrostAbilitiesPerSecond / stats.CinderglacierProc));
            }
            #endregion

            #region Mitigation
            {
                float targetArmor = calcOpts.BossArmor, totalArP = stats.ArmorPenetration;

                mitigation = 1f - StatConversion.GetArmorDamageReduction(character.Level, targetArmor,
                stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating);

                calcs.EnemyMitigation = 1f - mitigation;
                calcs.EffectiveArmor = mitigation;
            }
            #endregion

            #region White Dmg
            {
                float MHDam = 0f, OHDam = 0f;
                #region Main Hand
                {
                    float fDamMHglancing = (0.24f * this.MH.damage) * 0.75f;
                    float fDamMHBeforeArmor = ((this.MH.damage * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + this.physCrits)) + fDamMHglancing;
                    fDamWhiteMinusGlancing = fDamMHBeforeArmor - fDamMHglancing;
                    fDamWhiteBeforeArmor = fDamMHBeforeArmor;
                    MHDam = fDamMHBeforeArmor * mitigation;
                    MHDam *= fDuration / MH.hastedSpeed;
                }
                #endregion

                #region Off Hand
                if (DW || (character.MainHand == null && character.OffHand != null))
                {
                    float fDamOHglancing = (0.24f * this.OH.damage) * 0.75f;
                    float fDamOHBeforeArmor = ((this.OH.damage * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + this.physCrits)) + fDamOHglancing;
                    fDamWhiteMinusGlancing += fDamOHBeforeArmor - fDamOHglancing;
                    fDamWhiteBeforeArmor += fDamOHBeforeArmor;
                    OHDam = fDamOHBeforeArmor * mitigation;
                    OHDam *= fDuration / MH.hastedSpeed;
                }
                #endregion

                fDamWhite = MHDam + OHDam;
            }
            #endregion

            #region Necrosis
            {
                fDamNecrosis = fDamWhiteMinusGlancing * (.04f * (float)talents.Necrosis); // doesn't proc off Glancings
            }
            #endregion

            #region Blood Caked Blade
            {
                float fDamMHBCB = 0f;
                float fDamOHBCB = 0f;
                if ((this.OH.damage != 0) && (DW || this.MH.damage == 0))
                {
                    fDamOHBCB = this.OH.damage * (.25f + .125f * calcOpts.m_Rotation.avgDiseaseMult);
                }
                if (this.MH.damage != 0)
                {
                    fDamMHBCB = this.MH.damage * (.25f + .125f * calcOpts.m_Rotation.avgDiseaseMult);
                }
                fDamBCB = fDamMHBCB + fDamOHBCB;
                fDamBCB *= .1f * (float)talents.BloodCakedBlade;
            }
            #endregion

            #region Death Coil
            {
                if (calcOpts.m_Rotation.DeathCoil > 0f)
                {
                    float DCCD = this.realDuration / (calcOpts.m_Rotation.DeathCoil + (0.05f * (float)talents.SuddenDoom * calcOpts.m_Rotation.HeartStrike));
                    float DCDmg = 443f + (DeathCoilAPMult * stats.AttackPower) + stats.BonusDeathCoilDamage;
                    fDamDeathCoil = DCDmg / DCCD * calcOpts.m_Rotation.DeathCoil;
                    float DCCritDmgMult = .5f * (2f + stats.CritBonusDamage);
                    float DCCrit = 1f + ((this.spellCrits + stats.BonusDeathCoilCrit) * DCCritDmgMult);
                    fDamDeathCoil *= DCCrit;

                    fDamDeathCoil *= 1f + (.05f * (float)talents.Morbidity) + (talents.GlyphofDarkDeath ? .15f : 0f);
                }
            }
            #endregion

            #region Icy Touch
            // this seems to handle crit strangely.
            // additionally, looks like it's missing some multipliers? maybe they're applied later
            {
                if (calcOpts.m_Rotation.IcyTouch > 0f)
                {
                    float addedCritFromKM = KMRatio;
                    float ITCD = this.realDuration / calcOpts.m_Rotation.IcyTouch;
                    float ITDmg = 236f + (IcyTouchAPMult * stats.AttackPower) + stats.BonusIcyTouchDamage;
                    ITDmg *= 1f + .1f * (float)talents.ImprovedIcyTouch;
                    // Total damage for IT is ITdam * # of ITs
                    fDamIcyTouch = ITDmg * calcOpts.m_Rotation.IcyTouch;
                    float ITCritDmgMult = .5f * (2f + stats.CritBonusDamage);
                    float ITCrit = 1f + ((this.spellCrits + addedCritFromKM + (.05f * (float)talents.Rime)) * ITCritDmgMult);
                    fDamIcyTouch *= ITCrit;

                    // TODO: Add any IT triggers here.
                }
            }
            #endregion

            #region Plague Strike
            {
                if (calcOpts.m_Rotation.PlagueStrike > 0f)
                {
                    float PSCD = this.realDuration / calcOpts.m_Rotation.PlagueStrike;
                    float PSDmg = (this.MH.baseDamage + ((stats.AttackPower / 14f) * this.normalizationFactor)) * .5f + 189f;
                    fDamPlagueStrike = PSDmg * calcOpts.m_Rotation.PlagueStrike;
                    float PSCritDmgMult = 1f + (.15f * (float)talents.ViciousStrikes);
                    float PSCrit = 1f + ((this.physCrits + (.03f * (float)talents.ViciousStrikes) + stats.BonusPlagueStrikeCrit) * PSCritDmgMult);
                    fDamPlagueStrike *= PSCrit;

                    fDamPlagueStrike *= (talents.GlyphofPlagueStrike ? 1.2f : 1f);

                    fDamPlagueStrike *= 1f + (.1f * (float)talents.Outbreak);

                    // TODO: Add PS triggers here.
                }
            }
            #endregion

            #region Frost Fever
            {
                if (calcOpts.m_Rotation.IcyTouch > 0f || (talents.GlyphofHowlingBlast && calcOpts.m_Rotation.HowlingBlast > 0f) || (talents.GlyphofDisease))
                {
                    // Frost Fever is renewed with every Icy Touch and starts a new cd
                    float ITCD = calcOpts.m_Rotation.curRotationDuration / (calcOpts.m_Rotation.IcyTouch + (talents.GlyphofHowlingBlast ? calcOpts.m_Rotation.HowlingBlast : 0f));
                    float FFCD = 3f / (calcOpts.m_Rotation.diseaseUptime / 100);
                    int tempF = (int)Math.Floor(ITCD / FFCD);
                    FFCD = ((ITCD - ((float)tempF * FFCD)) / ((float)tempF + 1f)) + FFCD;
                    float FFDmg = FrostFeverAPMult * stats.AttackPower + 25.6f;
                    fDamFrostFever = FFDmg / FFCD * fDuration;
                    fDamWPFromFF = fDamFrostFever * this.physCrits * fDuration;

                    // TODO: this is the issue w/ the Glyph of HB vs. the Sigil of the Unfaltering Knight problems come from.
                    // Need to update that based on solid shot rotation work.
                }
            }
            #endregion

            #region Blood Plague
            {
                if (calcOpts.m_Rotation.PlagueStrike > 0f || talents.GlyphofPestilence)
                {
                    // Blood Plague is renewed with every Plague Strike and starts a new cd
                    float PSCD = calcOpts.m_Rotation.curRotationDuration / calcOpts.m_Rotation.PlagueStrike;
                    float BPCD = 3f / (calcOpts.m_Rotation.diseaseUptime / 100);
                    int tempF = (int)Math.Floor(PSCD / BPCD);
                    BPCD = ((PSCD - ((float)tempF * BPCD)) / ((float)tempF + 1f)) + BPCD;
                    float BPDmg = BloodPlagueAPMult * stats.AttackPower + 31.1f;
                    fDamBloodPlague = BPDmg / BPCD * fDuration;
                    fDamWPFromBP = fDamBloodPlague * this.physCrits * fDuration;
                }
            }
            #endregion

            #region Wandering Plague
            {
                fDamWanderingPlague = fDamWPFromBP + fDamWPFromFF;
                fDamWanderingPlague *= (1f / 3f) * (float)talents.WanderingPlague;
                // Since the damage is spread across all nearby targets, let's add that in.
                fDamWanderingPlague *= calcOpts.uNumberTargets; 
            }
            #endregion

            #region Scourge Strike
            {
                if (talents.ScourgeStrike > 0 && calcOpts.m_Rotation.ScourgeStrike > 0f)
                {
                    float SSCD = this.realDuration / calcOpts.m_Rotation.ScourgeStrike;
                    float SSDmg = ((this.MH.baseDamage + ((stats.AttackPower / 14f) * this.normalizationFactor)) * .45f) + 357.188f +
                        stats.BonusScourgeStrikeDamage;
                    SSDmg *= 1f + 0.11f * calcOpts.m_Rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseScourgeStrikeDamage);
                    SSDmg += 120; // Bonus threat
                    fDamScourgeStrike = SSDmg / SSCD * fDuration;
                    float SSCritDmgMult = 1f + (.15f * (float)talents.ViciousStrikes) + stats.CritBonusDamage;
                    float SSCrit = 1f + ((this.physCrits + (.03f * (float)talents.ViciousStrikes) + stats.BonusScourgeStrikeCrit) * SSCritDmgMult);
                    fDamScourgeStrike *= SSCrit;
                    fDamScourgeStrike *= 1f + (.0666666666666666666f * (float)talents.Outbreak);
                }
            }
            #endregion

            #region Unholy Blight
            {
                //The cooldown on this 1 second and I assume 100% uptime
                float UBDmg = UnholyBlightAPMult * stats.AttackPower + 37;
                fDamUnholyBlight = UBDmg * (1f + this.spellCrits);
                fDamUnholyBlight *= (float)talents.UnholyBlight;
                fDamUnholyBlight *= fDuration;
            }
            #endregion

            #region Frost Strike
            {
                if (talents.FrostStrike > 0 && calcOpts.m_Rotation.FrostStrike > 0f)
                {
                    float addedCritFromKM = KMRatio;
                    float FSCD = this.realDuration / calcOpts.m_Rotation.FrostStrike;
                    float FSDmg = (this.MH.baseDamage + ((stats.AttackPower / 14f) * this.normalizationFactor)) * .6f +
                        150f + stats.BonusFrostStrikeDamage;
                    fDamFrostStrike = FSDmg / FSCD * calcOpts.m_Rotation.FrostStrike;
                    float FSCritDmgMult = 1f + (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage;
                    float FSCrit = 1f + ((this.physCrits + addedCritFromKM + stats.BonusFrostStrikeCrit) * FSCritDmgMult);
                    fDamFrostStrike *= FSCrit;
                    fDamFrostStrike *= 1f + .03f * talents.BloodOfTheNorth;
                }
            }
            #endregion

            #region Trinket direct-damage procs, razorice damage, etc
            {
                fDamOtherArcane = stats.ArcaneDamage;
                fDamOtherShadow = stats.ShadowDamage;

                if (this.MH != null)
                {
                    float dpsMHglancing = (0.24f * this.MH.DPS) * 0.75f;
                    float dpsMHBeforeArmor = ((this.MH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + this.physCrits)) + dpsMHglancing;
                    fDamOtherFrost += (dpsMHBeforeArmor - dpsMHglancing) * stats.BonusFrostWeaponDamage;   // presumably doesn't proc off of glancings, like necrosis
                }

                if (this.OH != null)
                {
                    float dpsOHglancing = (0.24f * this.OH.DPS) * 0.75f;
                    float dpsOHBeforeArmor = ((this.OH.DPS * (1f - calcs.AvoidedAttacks - 0.24f)) * (1f + this.physCrits)) + dpsOHglancing;
                    fDamOtherFrost += (dpsOHBeforeArmor - dpsOHglancing) * stats.BonusFrostWeaponDamage;
                }

                float OtherCritDmgMult = .5f * (1f + stats.CritBonusDamage);
                float OtherCrit = 1f + ((this.spellCrits) * OtherCritDmgMult);
                fDamOtherArcane *= OtherCrit;
                fDamOtherShadow *= OtherCrit;
            }
            #endregion

            #region Howling Blast
            {
                if (talents.HowlingBlast > 0 && calcOpts.m_Rotation.HowlingBlast > 0f)
                {
                    float addedCritFromKM = KMRatio;
                    float HBCD = this.realDuration / calcOpts.m_Rotation.HowlingBlast;
                    float HBDmg = 540 + HowlingBlastAPMult * stats.AttackPower;
                    fDamHowlingBlast = HBDmg / HBCD * calcOpts.m_Rotation.HowlingBlast;
                    float HBCritDmgMult = .5f * (2f + (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage);
                    float HBCrit = 1f + ((this.spellCrits + addedCritFromKM) * HBCritDmgMult);
                    fDamHowlingBlast *= HBCrit;
                    // Multiply this by the number of targets hit.
                    fDamHowlingBlast *= calcOpts.uNumberTargets;
                }
            }
            #endregion

            #region Death n Decay
            {
                if (calcOpts.m_Rotation.DeathNDecay > 0f)
                {
                    float DNDCD = 30f - (talents.Morbidity * 5f); // 30 sec cool down modified by Morbidity
                    float DNDDur = 10f; // 10 sec duration
                    float DNDDmg = 62 + DeathNDecayAPMult * stats.AttackPower * DNDDur * (talents.GlyphofDeathandDecay ? 1.2f : 1f);
                    fDamDeathNDecay = DNDDmg * (Math.Min(calcOpts.m_Rotation.DeathNDecay, (fDuration / DNDCD)));
                    float DNDCritDmgMult = .5f * (2f + stats.CritBonusDamage);
                    float DNDCrit = 1f + (this.spellCrits * DNDCritDmgMult);
                    fDamDeathNDecay *= DNDCrit;
                    // Threat: damage × 1.90
                    fDamDeathNDecay *= 1.9f;
                    // Multiply this by the number of targets hit.
                    fDamDeathNDecay *= calcOpts.uNumberTargets;
                }
            }
            #endregion

            #region Obliterate
            {
                if (calcOpts.m_Rotation.Obliterate > 0f)
                {
                    // this is missing +crit chance from rime
                    float OblitCD = this.realDuration / calcOpts.m_Rotation.Obliterate;
                    float OblitDmg = ((this.MH.baseDamage + ((stats.AttackPower / 14f) * this.normalizationFactor)) *
                        0.8f) + stats.BonusObliterateDamage;
                    OblitDmg *= 1f + 0.125f * (float)calcOpts.m_Rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseObliterateDamage);
                    fDamObliterate = OblitDmg / OblitCD;
                    float OblitCritDmgMult = 1f + (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage;
                    float OblitCrit = 1f + ((this.physCrits +
                        (.03f * (float)talents.Subversion) +
                        (0.05f * (float)talents.Rime) +
                        stats.BonusObliterateCrit) * OblitCritDmgMult);
                    fDamObliterate *= OblitCrit;
                    fDamObliterate *= (talents.GlyphofObliterate ? 1.2f : 1f);
                }
            }
            #endregion

            #region Death Strike
            {
                if (calcOpts.m_Rotation.DeathStrike > 0f)
                {
                    // TODO: this is missing +crit chance from rime
                    float DSCD = this.realDuration / calcOpts.m_Rotation.DeathStrike;
                    // TODO: This should be changed to make use of the new glyph stats:
                    float DSDmg = ((this.MH.baseDamage + ((stats.AttackPower / 14f) * this.normalizationFactor)) * 0.75f) + 222.75f + stats.BonusDeathStrikeDamage;
                    DSDmg *= 1f + 0.15f * (float)talents.ImprovedDeathStrike;
                    DSDmg *= (talents.GlyphofDeathStrike ? 1.25f : 1f);
                    fDamDeathStrike = DSDmg / DSCD * calcOpts.m_Rotation.DeathStrike;
                    float DSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine) + stats.CritBonusDamage;
                    float DSCrit = 1f + ((this.physCrits +
                        (.03f * (float)talents.ImprovedDeathStrike) +
                        stats.BonusDeathStrikeCrit) * DSCritDmgMult);
                    fDamDeathStrike *= DSCrit; // threat from Damage.
                    fDamDeathStrike *= ((stats.Health * .05f * calcOpts.m_Rotation.numDisease) / calcOpts.uNumberTargets / 2); // threat from Healing divided by number of targets.
                }
            }
            #endregion

            #region Blood Strike
            {
                if (calcOpts.m_Rotation.BloodStrike > 0f)
                {
                    float BSCD = this.realDuration / calcOpts.m_Rotation.BloodStrike;
                    float BSDmg = ((this.MH.baseDamage + ((stats.AttackPower / 14f) * this.normalizationFactor)) *
                        0.4f) + 305.6f + stats.BonusBloodStrikeDamage;
                    BSDmg *= 1f + 0.125f * (float)calcOpts.m_Rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseBloodStrikeDamage);
                    fDamBloodStrike = BSDmg / BSCD * calcOpts.m_Rotation.BloodStrike;
                    float BSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine);
                    BSCritDmgMult += (.15f * (float)talents.GuileOfGorefiend) + stats.CritBonusDamage;
                    float BSCrit = 1f + ((this.physCrits + (.03f * (float)talents.Subversion)) * BSCritDmgMult);
                    fDamBloodStrike *= BSCrit;
                    fDamBloodStrike *= 1f + (.03f * (float)talents.BloodOfTheNorth);
                    fDamBloodStrike *= 1f + (.15f * (float)talents.BloodyStrikes);
                }
            }
            #endregion

            #region Heart Strike
            {
                if (talents.HeartStrike > 0 && calcOpts.m_Rotation.HeartStrike > 0f)
                {
                    float HSCD = this.realDuration / calcOpts.m_Rotation.HeartStrike;
                    float HSDmg = ((this.MH.baseDamage + ((stats.AttackPower / 14f) * this.normalizationFactor)) *
                        0.5f) + 368f + stats.BonusHeartStrikeDamage;
                    HSDmg *= 1f + 0.1f * (float)calcOpts.m_Rotation.avgDiseaseMult * (1f + stats.BonusPerDiseaseHeartStrikeDamage);
                    fDamHeartStrike = HSDmg / HSCD * calcOpts.m_Rotation.HeartStrike;
                    //float HSCrit = 1f + combatTable.physCrits + ( .03f * (float)talents.Subversion );
                    float HSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine) + stats.CritBonusDamage;
                    float HSCrit = 1f + ((this.physCrits + (.03f * (float)talents.Subversion)) * HSCritDmgMult);
                    fDamHeartStrike *= HSCrit * Math.Min(2, calcOpts.uNumberTargets);
                    fDamHeartStrike *= 1f + (.15f * (float)talents.BloodyStrikes);
                }
            }
            #endregion

            #region Rune Strike
            {
                float RSDmg = (this.MH.baseDamage + ((stats.AttackPower / 14f) * this.normalizationFactor)) + (150 * stats.AttackPower * 10 / 10000);
                RSDmg *= (1f + stats.BonusRuneStrikeMultiplier); // Two T8.
                // what's the threat modifier?
                RSDmg *= 1.5f;
                float RSCritDmgMult = 1f + (.15f * (float)talents.MightOfMograine) + stats.CritBonusDamage + (talents.GlyphofRuneStrike ? .01f : 0f);
                float RSCrit = 1f + ((this.physCrits) * RSCritDmgMult);
                // How many RS do we get?
                // No more than the number of white swings
                m_fRSCount = Math.Min((fDuration / MH.hastedSpeed), (calcOpts.m_Rotation.RP / 20f) * (stats.Dodge + stats.Parry));
                if (m_fRSCount == 0)
                {
                    m_fRSCount = calcOpts.m_Rotation.RuneStrike;
                }
                else
                {
                    // Update the rotation with this number. 
                    calcOpts.m_Rotation.RuneStrike = m_fRSCount;
                }
                fDamRuneStrike = RSDmg * m_fRSCount;
                fDamRuneStrike *= RSCrit;
            }
            #endregion

            float BCBMult = 1f;
            float BloodPlagueMult = 1f;
            float BloodStrikeMult = 1f;
            float DeathCoilMult = 1f;
            float DancingRuneWeaponMult = 1f;
            float FrostFeverMult = 1f;
            float FrostStrikeMult = 1f;
            float BloodwormsMult = 1f + commandMult;
            float HeartStrikeMult = 1f;
            float HowlingBlastMult = 1f;
            float DeathNDecayMult = 1f;
            float IcyTouchMult = 1f;
            float NecrosisMult = 1f;
            float ObliterateMult = 1f;
            float DeathStrikeMult = 1f;
            float PlagueStrikeMult = 1f;
            float ScourgeStrikeMult = 1f;
            float UnholyBlightMult = 1f;
            float WhiteMult = 1f;
            float WanderingPlagueMult = 1f;
            float otherShadowMult = 1f;
            float otherArcaneMult = 1f;
            float otherFrostMult = 1f;

            #region Apply Physical Mitigation
            {
                float physMit = mitigation;
                physMit *= 1f + (!DW ? .02f * talents.TwoHandedWeaponSpecialization : 0f);

                fDamBCB *= physMit;
                fDamBloodStrike *= physMit;
                fDamHeartStrike *= physMit;
                fDamObliterate *= physMit;
                fDamDeathStrike *= physMit;
                fDamPlagueStrike *= physMit;
                fDamRuneStrike *= physMit;

                WhiteMult += physPowerMult - 1f;
                BCBMult += physPowerMult - 1f;
                BloodStrikeMult += physPowerMult - 1f;
                HeartStrikeMult += physPowerMult - 1f;
                ObliterateMult += physPowerMult - 1f;
                DeathStrikeMult += physPowerMult - 1f;
                PlagueStrikeMult += physPowerMult - 1f;
            }
            #endregion

            #region Apply Elemental Strike Mitigation
            {
                float strikeMit = /*missedSpecial **/ partialResist;
                strikeMit *= (!DW ? 1f + .02f * talents.TwoHandedWeaponSpecialization : 1f);

                fDamScourgeStrike *= strikeMit;
                fDamFrostStrike *= strikeMit * (1f - missedSpecial);

                ScourgeStrikeMult += spellPowerMult - 1f;
                FrostStrikeMult += frostSpellPowerMult - 1f;
            }
            #endregion

            #region Apply Magical Mitigation
            {
                // some of this applies to necrosis, I wonder if it is ever accounted for
                float magicMit = partialResist /** combatTable.spellResist*/;
                // magicMit = 1f - magicMit;

                fDamNecrosis *= magicMit;
                fDamBloodPlague *= magicMit;
                fDamDeathCoil *= magicMit * (1 - this.spellResist);
                fDamFrostFever *= magicMit;
                fDamHowlingBlast *= magicMit * (1 - this.spellResist);
                fDamIcyTouch *= magicMit;
                fDamUnholyBlight *= magicMit * (1 - this.spellResist);


                NecrosisMult += spellPowerMult - 1f;
                BloodPlagueMult += spellPowerMult - 1f;
                DeathCoilMult += spellPowerMult - 1f;
                FrostFeverMult += frostSpellPowerMult - 1f;
                HowlingBlastMult += frostSpellPowerMult - 1f;
                IcyTouchMult += frostSpellPowerMult - 1f;
                UnholyBlightMult += spellPowerMult - 1f;
                otherShadowMult += spellPowerMult - 1f;
                otherArcaneMult += spellPowerMult - 1f;
                otherFrostMult += frostSpellPowerMult - 1f;
            }
            #endregion

            #region Cinderglacier multipliers
            {
                DeathCoilMult *= CinderglacierMultiplier;
                HowlingBlastMult *= CinderglacierMultiplier;
                IcyTouchMult *= CinderglacierMultiplier;
                ScourgeStrikeMult *= CinderglacierMultiplier;
                FrostStrikeMult *= CinderglacierMultiplier;
            }
            #endregion

            #region Apply Multi-Ability Talent Multipliers
            {
                float BloodyVengeanceMult = .03f * (float)talents.BloodyVengeance;
                BCBMult *= 1 + BloodyVengeanceMult;
                BloodStrikeMult *= 1 + BloodyVengeanceMult;
                HeartStrikeMult *= 1 + BloodyVengeanceMult;
                ObliterateMult *= 1 + BloodyVengeanceMult;
                DeathStrikeMult *= 1 + BloodyVengeanceMult;
                PlagueStrikeMult *= 1 + BloodyVengeanceMult;
                WhiteMult *= 1 + BloodyVengeanceMult;

                float HysteriaCoeff = .3f / 6f; // current uptime is 16.666...%
                float HysteriaMult = HysteriaCoeff * (float)talents.Hysteria;
                BCBMult *= 1 + HysteriaMult;
                BloodStrikeMult *= 1 + HysteriaMult;
                HeartStrikeMult *= 1 + HysteriaMult;
                ObliterateMult *= 1 + HysteriaMult;
                DeathStrikeMult *= 1 + HysteriaMult;
                PlagueStrikeMult *= 1 + HysteriaMult;
                WhiteMult *= 1 + HysteriaMult;

                float BlackIceMult = .02f * (float)talents.BlackIce;
                FrostFeverMult *= 1 + BlackIceMult;
                HowlingBlastMult *= 1 + BlackIceMult;
                IcyTouchMult *= 1 + BlackIceMult;
                FrostStrikeMult *= 1 + BlackIceMult;
                DeathCoilMult *= 1 + BlackIceMult;
                ScourgeStrikeMult *= 1 + BlackIceMult;
                BloodPlagueMult *= 1 + BlackIceMult;
                otherShadowMult *= 1 + BlackIceMult;
                otherFrostMult *= 1 + BlackIceMult;

                float MercilessCombatMult = .315f * 0.06f * (float)talents.MercilessCombat;   // The last 35% of a Boss don't take 35% of the fight-time...say .315 (10% faster)
                ObliterateMult *= 1 + MercilessCombatMult;
                HowlingBlastMult *= 1 + MercilessCombatMult;
                IcyTouchMult *= 1 + MercilessCombatMult;
                FrostStrikeMult *= 1 + MercilessCombatMult;

                float GlacierRot = .0666666666666f * (float)talents.GlacierRot;
                HowlingBlastMult *= 1 + GlacierRot;
                IcyTouchMult *= 1 + GlacierRot;
                FrostStrikeMult *= 1 + GlacierRot;


                float CryptFeverMult = .1f * (float)talents.CryptFever;
                float CryptFeverBuff = stats.BonusDiseaseDamageMultiplier;
                CryptFeverMult = Math.Max(CryptFeverMult, CryptFeverBuff);
                FrostFeverMult *= 1 + CryptFeverMult;
                BloodPlagueMult *= 1 + CryptFeverMult;
                UnholyBlightMult *= 1 + CryptFeverMult;

                float DesecrationMult = .01f * (float)talents.Desecration;  //the new desecration is basically a flat 1% per point
                BCBMult *= 1 + DesecrationMult;
                BloodPlagueMult *= 1 + DesecrationMult;
                BloodStrikeMult *= 1 + DesecrationMult;
                DeathCoilMult *= 1 + DesecrationMult;
                DancingRuneWeaponMult *= 1 + DesecrationMult;
                FrostFeverMult *= 1 + DesecrationMult;
                FrostStrikeMult *= 1 + DesecrationMult;
                HeartStrikeMult *= 1 + DesecrationMult;
                HowlingBlastMult *= 1 + DesecrationMult;
                DeathNDecayMult *= 1 + DesecrationMult;
                IcyTouchMult *= 1 + DesecrationMult;
                NecrosisMult *= 1 + DesecrationMult;
                ObliterateMult *= 1 + DesecrationMult;
                DeathStrikeMult *= 1 + DesecrationMult;
                PlagueStrikeMult *= 1 + DesecrationMult;
                ScourgeStrikeMult *= 1 + DesecrationMult;
                UnholyBlightMult *= 1 + DesecrationMult;
                WhiteMult *= 1 + DesecrationMult;
                otherShadowMult *= 1 + DesecrationMult;
                otherArcaneMult *= 1 + DesecrationMult;
                otherFrostMult *= 1 + DesecrationMult;

                if ((float)talents.BoneShield >= 1f)
                {
                    float BoneMult = .02f;
                    BCBMult *= 1 + BoneMult;
                    BloodPlagueMult *= 1 + BoneMult;
                    BloodStrikeMult *= 1 + BoneMult;
                    DeathCoilMult *= 1 + BoneMult;
                    DancingRuneWeaponMult *= 1 + BoneMult;
                    FrostFeverMult *= 1 + BoneMult;
                    FrostStrikeMult *= 1 + BoneMult;
                    HeartStrikeMult *= 1 + BoneMult;
                    HowlingBlastMult *= 1 + BoneMult;
                    DeathNDecayMult *= 1 + BoneMult;
                    IcyTouchMult *= 1 + BoneMult;
                    NecrosisMult *= 1 + BoneMult;
                    ObliterateMult *= 1 + BoneMult;
                    DeathStrikeMult *= 1 + BoneMult;
                    PlagueStrikeMult *= 1 + BoneMult;
                    ScourgeStrikeMult *= 1 + BoneMult;
                    UnholyBlightMult *= 1 + BoneMult;
                    WhiteMult *= 1 + BoneMult;
                    otherShadowMult *= 1 + BoneMult;
                    otherArcaneMult *= 1 + BoneMult;
                    otherFrostMult *= 1 + BoneMult;
                }
            }
            #endregion

            //feed variables for output
            calcs.BCBDPS = fDamBCB * BCBMult;
            calcs.BloodPlagueDPS = fDamBloodPlague * BloodPlagueMult;
            calcs.BloodStrikeDPS = fDamBloodStrike * BloodStrikeMult;
            calcs.DeathCoilDPS = fDamDeathCoil * DeathCoilMult;
            calcs.DRWDPS = fDamDancingRuneWeapon * DancingRuneWeaponMult;
            calcs.FrostFeverDPS = fDamFrostFever * FrostFeverMult;
            calcs.FrostStrikeDPS = fDamFrostStrike * FrostStrikeMult;
            calcs.BloodwormsDPS = fDamBloodworms * BloodwormsMult;
            calcs.HeartStrikeDPS = fDamHeartStrike * HeartStrikeMult;
            calcs.HowlingBlastDPS = fDamHowlingBlast * HowlingBlastMult;
            calcs.DeathNDecayDPS = fDamDeathNDecay * DeathNDecayMult;
            calcs.IcyTouchDPS = fDamIcyTouch * IcyTouchMult;
            calcs.NecrosisDPS = fDamNecrosis * NecrosisMult;
            calcs.ObliterateDPS = fDamObliterate * ObliterateMult;
            calcs.DeathStrikeDPS = fDamDeathStrike * DeathStrikeMult;
            calcs.PlagueStrikeDPS = fDamPlagueStrike * PlagueStrikeMult;
            calcs.ScourgeStrikeDPS = fDamScourgeStrike * ScourgeStrikeMult;
            calcs.UnholyBlightDPS = fDamUnholyBlight * UnholyBlightMult;
            calcs.WhiteDPS = fDamWhite * WhiteMult;
            calcs.WanderingPlagueDPS = fDamWanderingPlague * WanderingPlagueMult;
            calcs.RuneStrikeDPS = fDamRuneStrike;
            calcs.OtherDPS = fDamOtherShadow * otherShadowMult +
                fDamOtherArcane * otherArcaneMult +
                fDamOtherFrost * otherFrostMult;

            float DPSPoints = 0;
            DPSPoints += calcs.BCBDPS;
            DPSPoints += calcs.BloodPlagueDPS;
            DPSPoints += calcs.BloodStrikeDPS;
            DPSPoints += calcs.DeathCoilDPS;
            DPSPoints += calcs.FrostFeverDPS;
            DPSPoints += calcs.FrostStrikeDPS;
            DPSPoints += calcs.WanderingPlagueDPS;
            DPSPoints += calcs.HeartStrikeDPS;
            DPSPoints += calcs.HowlingBlastDPS;
            DPSPoints += calcs.IcyTouchDPS;
            DPSPoints += calcs.NecrosisDPS;
            DPSPoints += calcs.ObliterateDPS;
            DPSPoints += calcs.DeathStrikeDPS;
            DPSPoints += calcs.PlagueStrikeDPS;
            DPSPoints += calcs.ScourgeStrikeDPS;
            DPSPoints += calcs.UnholyBlightDPS;
            DPSPoints += calcs.WhiteDPS;
            DPSPoints += calcs.OtherDPS;
            DPSPoints += calcs.BloodwormsDPS;
            DPSPoints += calcs.RuneStrikeDPS;
            DPSPoints += calcs.DeathNDecayDPS;

            DPSPoints *= 2.0735f;

            return DPSPoints;
        }
    }
}
