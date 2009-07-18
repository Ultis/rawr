using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSDK
{
    class CombatTable
    {
        public Character character;
        public CharacterCalculationsDPSDK calcs;
        public DeathKnightTalents talents;
        public Stats stats;
        public CalculationOptionsDPSDK calcOpts;

        public Weapon MH, OH;
        public bool DW;

        public float combinedSwingTime;

        public float physCrits, hitBonus, 
            missedSpecial, dodgedSpecial, 
            spellCrits, spellResist, 
            totalMHMiss, totalOHMiss,
            realDuration, totalMeleeAbilities,
        totalSpellAbilities, normalizationFactor;

        public CombatTable(Character c, Stats stats, CalculationOptionsDPSDK calcOpts) :
            this(c, new CharacterCalculationsDPSDK(), stats, calcOpts)
        {
        }

        public CombatTable(Character c, CharacterCalculationsDPSDK calcs, Stats stats, CalculationOptionsDPSDK calcOpts)
        {
            character = c;
            this.calcs = calcs;
            talents = character.DeathKnightTalents;
            this.calcOpts = calcOpts;
            this.stats = stats;
            totalMeleeAbilities = 0f;
            totalSpellAbilities = 0f;


#if SILVERLIGHT
            if (calcOpts.rotation == null)
            {
                calcOpts.rotation = new Rotation();
                calcOpts.rotation.setRotation(Rotation.Type.Blood);
            }
#endif

            totalMeleeAbilities = calcOpts.rotation.PlagueStrike + calcOpts.rotation.ScourgeStrike +
                calcOpts.rotation.Obliterate + calcOpts.rotation.BloodStrike + calcOpts.rotation.HeartStrike +
                calcOpts.rotation.FrostStrike;

            totalSpellAbilities = calcOpts.rotation.DeathCoil + calcOpts.rotation.IcyTouch + calcOpts.rotation.HowlingBlast;

            hitBonus = .01f * (float)talents.NervesOfColdSteel;
            Weapons();
            CritsAndResists();
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
                physCrits += StatConversion.GetPhysicalCritFromRating(stats.CritRating);
                physCrits += StatConversion.GetPhysicalCritFromAgility(stats.Agility, CharacterClass.DeathKnight);
                physCrits += .01f * (float)(talents.DarkConviction + talents.EbonPlaguebringer + talents.Annihilation);
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
                    if (DW)
                    {
                        chanceDodged += OH.chanceDodged;
                        chanceDodged /= 2;
                    }
                    else if (character.MainHand == null )
                    {
                        chanceDodged = OH.chanceDodged;
                    }
                }

                calcs.DodgedAttacks = chanceDodged;

                float chanceMiss = 0f;
                if (DW || (character.MainHand == null && character.OffHand != null)) chanceMiss = .27f;
                else chanceMiss = .08f;
                chanceMiss -= StatConversion.GetPhysicalHitFromRating(stats.HitRating);
                chanceMiss -= hitBonus;
                chanceMiss -= stats.PhysicalHit;
                if (chanceMiss < 0f) chanceMiss = 0f;
                calcs.MissedAttacks = chanceMiss;

                chanceAvoided = chanceDodged + chanceMiss;
                calcs.AvoidedAttacks = chanceDodged + chanceMiss;

                chanceMiss = .08f;
                chanceMiss -= StatConversion.GetPhysicalHitFromRating(stats.HitRating);
                chanceMiss -= hitBonus;
                chanceMiss -= stats.PhysicalHit;
                if (chanceMiss < 0f) chanceMiss = 0f;
                chanceDodged = MH.chanceDodged;
                missedSpecial = chanceMiss;
                dodgedSpecial = chanceDodged;
                // calcs.MissedAttacks = chanceMiss           

                spellCrits = 0f;
                spellCrits += StatConversion.GetSpellCritFromRating(stats.CritRating);
                spellCrits += stats.SpellCrit;
                spellCrits += .01f * (float)(talents.DarkConviction + talents.EbonPlaguebringer);
                calcs.SpellCritChance = spellCrits;

                // Resists: Base 17%
                spellResist = .17f;
                spellResist -= StatConversion.GetSpellHitFromRating(stats.HitRating);
                spellResist -= hitBonus + (.01f * talents.Virulence);
                spellResist -= stats.SpellHit;
                if (spellResist < 0f) spellResist = 0f;

                // Total physical misses
                totalMHMiss = calcs.DodgedMHAttacks + chanceMiss;
                totalOHMiss = calcs.DodgedOHAttacks + chanceMiss;
                realDuration = calcOpts.rotation.curRotationDuration;
                float foo = (((calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Blood ? 1.5f : 1.0f) / (1 + (StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.DeathKnight)) + stats.SpellHaste)));
                realDuration += ((totalMeleeAbilities - calcOpts.rotation.FrostStrike) * chanceDodged * (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Blood ? 1.5f : 1.0f)) +
                    ((totalMeleeAbilities - calcOpts.rotation.FrostStrike) * chanceMiss * (calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Blood ? 1.5f : 1.0f)) +
                    ((calcOpts.rotation.IcyTouch * spellResist * (((calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Blood ? 1.5f : 1.0f) / (1 + (StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.DeathKnight)) + stats.SpellHaste)) <= 1.0f ? 1.0f : (((calcOpts.rotation.presence == CalculationOptionsDPSDK.Presence.Blood ? 1.5f : 1.0f) / (1 + (StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.DeathKnight)) + stats.SpellHaste)))))); //still need to implement spellhaste here
            }
            #endregion
        }

        public void Weapons(){

            float MHExpertise = stats.Expertise;
            float OHExpertise = stats.Expertise;

            if (character.Race == CharacterRace.Dwarf)
            {
                if (character.MainHand != null &&
                    (character.MainHand.Item.Type == ItemType.OneHandMace ||
                     character.MainHand.Item.Type == ItemType.TwoHandMace))
                {
                    MHExpertise += 5f;
                }

                if (character.OffHand != null && character.OffHand.Item.Type == ItemType.OneHandMace)
                {
                    OHExpertise += 5f;
                }
            }
            else if (character.Race == CharacterRace.Orc)
            {
                if (character.MainHand != null &&
                    (character.MainHand.Item.Type == ItemType.OneHandAxe ||
                     character.MainHand.Item.Type == ItemType.TwoHandAxe))
                {
                    MHExpertise += 5f;
                }

                if (character.OffHand != null && character.OffHand.Item.Type == ItemType.OneHandAxe)
                {
                    OHExpertise += 5f;
                }
            }
            if (character.Race == CharacterRace.Human)
            {
                if (character.MainHand != null &&
                    (character.MainHand.Item.Type == ItemType.OneHandSword ||
                     character.MainHand.Item.Type == ItemType.TwoHandSword ||
                     character.MainHand.Item.Type == ItemType.OneHandMace ||
                     character.MainHand.Item.Type == ItemType.TwoHandMace))
                {
                    MHExpertise += 3f;
                }

                if (character.OffHand != null &&
                    (character.OffHand.Item.Type == ItemType.OneHandSword ||
                    character.OffHand.Item.Type == ItemType.OneHandMace))
                {
                    OHExpertise += 3f;
                }
            }


            MH = new Weapon(null, stats, calcOpts, 0f);
            OH = new Weapon(null, null, null, 0f);

            DW = character.MainHand != null && character.OffHand != null &&
                character.MainHand.Slot != ItemSlot.TwoHand;

            if (character.MainHand != null)
            {
                MH = new Weapon(character.MainHand.Item, stats, calcOpts, MHExpertise);
                calcs.MHAttackSpeed = MH.hastedSpeed;
                calcs.MHWeaponDamage = MH.damage;
                calcs.MHExpertise = MH.effectiveExpertise;
            }

            if (character.OffHand != null)
            {
                OH = new Weapon(character.OffHand.Item, stats, calcOpts, OHExpertise);

               // float OHMult = .05f * (float)talents.NervesOfColdSteel;
               // OH.damage *= .5f + OHMult;

                calcs.OHAttackSpeed = OH.hastedSpeed;
                calcs.OHWeaponDamage = OH.damage;
                calcs.OHExpertise = OH.effectiveExpertise;
            }

            // MH-only
            if ((character.MainHand != null) && (! DW))
            {
                if (character.MainHand.Item.Type == ItemType.TwoHandAxe
                    || character.MainHand.Item.Type == ItemType.TwoHandMace
                    || character.MainHand.Item.Type == ItemType.TwoHandSword
                    || character.MainHand.Item.Type == ItemType.Polearm)
                {
                    normalizationFactor = 3.3f;
                    MH.damage *= 1f + .02f * talents.TwoHandedWeaponSpecialization;
                }
                else normalizationFactor = 2.4f;

                combinedSwingTime = MH.hastedSpeed;
                calcs.OHAttackSpeed = 0f;
                calcs.OHWeaponDamage = 0f;
                calcs.OHExpertise = 0f;
            }
            // DW or no MH
            else if (character.OffHand != null)
            {
                // need this for weapon swing procs
                // combinedSwingTime = 1f / MH.hastedSpeed + 1f / OH.hastedSpeed;
                // combinedSwingTime = 1f / combinedSwingTime;
                combinedSwingTime = (MH.hastedSpeed + OH.hastedSpeed) / 4;
                normalizationFactor = 2.4f;
            } 
            // Unarmed
            else if (character.MainHand == null && character.OffHand == null)
            {
                combinedSwingTime = 2f;
                normalizationFactor = 2.4f;
            }
        }
    }
}
