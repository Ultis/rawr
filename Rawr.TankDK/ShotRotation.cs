using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    public class Rotation
    {
        // I need FullCharacterStats
        public Stats m_FullStats = null;
        
        // Initial Code taken from DPSDK
        public Type curRotationType = Type.Frost;
        
        public float curRotationDuration = 0f;  // rotation duration in seconds

        //disease info
        public float avgDiseaseMult = 0f;
        public float numDisease = 0f;
        public float diseaseUptime = 0f;
        public float GargoyleDuration = 0f;
        
        //ability number of times per rotation used 
        public float DeathCoil = 0f;
        public float IcyTouch = 0f;
        public float PlagueStrike = 0f;
        public float FrostFever = 0f;
        public float BloodPlague = 0f;
        public float ScourgeStrike = 0f;
        public float UnholyBlight = 0f;
        public float FrostStrike = 0f;
        public float HowlingBlast = 0f;
        public float Obliterate = 0f;
        public float DeathStrike = 0f;
        public float BloodStrike = 0f;
        public float HeartStrike = 0f;
        public float DancingRuneWeapon = 0f;
        public float Horn = 0f;
        public float Pestilence = 0f;
        public float BloodBoil = 0f;
        public float DeathNDecay = 0f;
        // Defensive Rotations
        public float BoneShield = 0f;
        public float IceBoundFortitude = 0f;
        public float UnbreakableArmor = 0f;
        public float RuneStrike = 0f;

        public Boolean fourT7 = false;
        public Boolean GlyphofIT = false;
        public Boolean GlyphofFS = false;
        public Boolean managedRP = false;
        public float GCDTime;
        public float RP;

        public enum Type
        {            
            Custom, Blood, Frost, Unholy
        }

        public float getMeleeSpecialsPerSecond()
        {
            float temp;
            temp = PlagueStrike + ScourgeStrike + FrostStrike + Obliterate + DeathStrike + BloodStrike + HeartStrike;
            temp = temp / curRotationDuration;
            return temp;
        }

        public float getSpellSpecialsPerSecond()
        {
            float temp;
            temp = DeathCoil + IcyTouch + HowlingBlast + DeathNDecay + BloodBoil;
            temp = temp / curRotationDuration;
            return temp;
        }


        public float getRP(DeathKnightTalents talents, Character character)
        {
            fourT7 = character.ActiveBuffsContains("Scourgeborne Battlegear 4 Piece Bonus");
            this.GlyphofIT = talents.GlyphofIcyTouch;
            this.GlyphofFS = talents.GlyphofFrostStrike;

            RP = ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.ChillOfTheGrave + 2.5f * talents.Dirge) * (Obliterate)) +
                ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.Dirge) * (ScourgeStrike)) +
                ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.Dirge) * (DeathStrike)) +
                ((15 * (DeathNDecay)) +
                ((10 + 2.5f * talents.Dirge) * (PlagueStrike)) +
                (10 * (BloodStrike + HeartStrike + BloodBoil)) +
                ((10 + (GlyphofIT ? 10 : 0) + 2.5f * talents.ChillOfTheGrave) * (IcyTouch)) +
                ((15 + 2.5f * talents.ChillOfTheGrave) * HowlingBlast) +
                (10 * Horn) +
                ((curRotationDuration / 5f)*talents.Butchery));

            if (managedRP)
            {
                RP = manageRPDumping(talents, RP);
            }
            else
            {
                RP -= ((40 * DeathCoil) +
                    ((GlyphofFS ? 32 : 40) * FrostStrike) 
                    + (40 * UnholyBlight)
                    + (20 * RuneStrike));
            }
            return RP;
        }

        public float manageRPDumping(DeathKnightTalents talents, float RP)
        {
            RuneStrike = RP / 20f;
            // RuneStrikes count is also based on Dodge & Parry chance:
            RuneStrike *= (m_FullStats.Dodge + m_FullStats.Parry);
            // Remove the RS shots from the pool of available RP.
            RP -= (RuneStrike * 20f);
            
            if (talents.FrostStrike > 0f)
            {
                FrostStrike = RP / (talents.GlyphofFrostStrike ? 32f : 40f);
                DeathCoil = 0f;
                UnholyBlight = 0f;
                RP = 0f;
            }
            else if (talents.UnholyBlight > 0f)
            {
                UnholyBlight = curRotationDuration / (talents.GlyphofUnholyBlight ? 30f : 20f);
                RP -= UnholyBlight * 40f;
                DeathCoil = RP / 40f;
                FrostStrike = 0f;
                RP = 0f;
            }
            else
            {
                DeathCoil = RP / 40f;
                FrostStrike = 0f;
                UnholyBlight = 0f;
                RP = 0f;
            }

            return RP;
        }

        public float getGCDTime()
        {
           // TODO: Update rotation to just factor in the number of strikes minus haste.
            GCDTime = GetGCDHasted() * (PlagueStrike + ScourgeStrike + FrostStrike + Obliterate + DeathStrike +
                BloodStrike + HeartStrike);
            GCDTime += GetGCDHasted() * (DeathCoil + IcyTouch + UnholyBlight + HowlingBlast + Horn + DeathNDecay + BloodBoil + Pestilence);
            return GCDTime;
        }
        /// <summary>
        /// How fast is the GCD with the current character's stats?
        /// </summary>
        /// <param name="s">Total Stats for the character.</param>
        /// <returns>Duration of the GCD in seconds.</returns>
        public float GetGCDHasted()
        {
            float fHR = m_FullStats.HasteRating;
            float fPH = m_FullStats.PhysicalHaste;
            if (null == m_FullStats) 
            {
                fHR = 0f;
                fPH = 0f;
            }
            float fNormalGCD = 1.5f;
            float fPercHaste = StatConversion.GetHasteFromRating(fHR, Character.CharacterClass.DeathKnight) + fPH;
            float fHastedGCD = Math.Max( 1f, fNormalGCD/(1f + fPercHaste) );
            return fHastedGCD;
        }

        public void setRotation(Type t)
        {
            curRotationType = t;
            switch (curRotationType)
            {
                case Type.Blood:
                    numDisease = 2f;
                    diseaseUptime = 100f;
                    DeathCoil = 3f;
                    IcyTouch = 1f;
                    PlagueStrike = 1f;
                    ScourgeStrike = 0f;
                    UnholyBlight = 0f;
                    FrostStrike = 0f;
                    HowlingBlast = 0f;
                    Obliterate = 0f;
                    DeathStrike = 2f;
                    BloodStrike = 0f;
                    HeartStrike = 6f;
                    curRotationDuration = 20f;
                    DancingRuneWeapon = 190f;
                    GargoyleDuration = 0f;
                    DeathStrike = 2f;
                    break;
                case Type.Frost:
                    numDisease = 2f;
                    diseaseUptime = 100f;
                    DeathCoil = 0f;
                    IcyTouch = 2f;
                    PlagueStrike = 2f;
                    ScourgeStrike = 0f;
                    UnholyBlight = 0f;
                    FrostStrike = 2f;
                    HowlingBlast = 2f;
                    Obliterate = 4f;
                    BloodStrike = 2f;
                    HeartStrike = 0f;
                    DancingRuneWeapon = 0f;
                    curRotationDuration = 20f;
                    GargoyleDuration = 0f;
                    DeathStrike = 0f;
                    RuneStrike = 3f;
                    break;
                case Type.Unholy:
                    numDisease = 3f;
                    diseaseUptime = 100f;
                    DeathCoil = 1f;
                    IcyTouch = 1f;
                    PlagueStrike = 1f;
                    ScourgeStrike = 4f;
                    UnholyBlight = 1f;
                    FrostStrike = 0f;
                    HowlingBlast = 0f;
                    Obliterate = 0f;
                    BloodStrike = 2f;
                    HeartStrike = 0f;
                    DancingRuneWeapon = 0f;
                    curRotationDuration = 20f;
                    GargoyleDuration = 30f;
                    DeathStrike = 0f;
                    break;
                case Type.Custom:
                   /* numDisease = 0f;
                    diseaseUptime = 0f;
                    DeathCoil = 0f;
                    IcyTouch = 0f;
                    PlagueStrike = 0f;
                    ScourgeStrike = 0f;
                    UnholyBlight = 0f;
                    FrostStrike = 0f;
                    HowlingBlast = 0f;
                    Obliterate = 0f;
                    BloodStrike = 0f;
                    HeartStrike = 0f;
                    DancingRuneWeapon = 0f;
                    curRotationDuration = 0f;
                    GargoyleDuration = 0f;
                    presence = CalculationOptionsDPSDK.Presence.Blood;*/
                    break;
            }
        }
    }
}
