using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSDK
{
    public class Rotation
    {
        public Type curRotationType = Type.Blood;
        public CalculationOptionsDPSDK.Presence presence = CalculationOptionsDPSDK.Presence.Blood;
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

        public Boolean fourT7 = false;
        public Boolean GlyphofIT = false;
        public Boolean GlyphofFS = false;
        public Boolean managedRP = false;
        public Boolean PTRCalcs = false;
		public Boolean TAT = false;
        public float GCDTime;
        public float RP;

        public enum Type
        {            
            Custom, Blood, Frost, Unholy
        }

        public Rotation()
        {
            setRotation(Type.Unholy);
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
            temp = DeathCoil + IcyTouch + HowlingBlast;
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
                ((10 + 2.5f * talents.Dirge) * (PlagueStrike)) +
                (10 * (BloodStrike + HeartStrike)) +
                ((10 + (GlyphofIT ? 10 : 0) + 2.5f * talents.ChillOfTheGrave) * (IcyTouch)) +
                ((15 + 2.5f * talents.ChillOfTheGrave) * HowlingBlast) +
                (10 * Horn) +
                ((curRotationDuration / 5f)*talents.Butchery);

            if (managedRP)
            {
                RP = manageRPDumping(talents, RP);
            }
            else
            {
                RP -= ((40 * DeathCoil) +
                    ((GlyphofFS ? 32 : 40) * FrostStrike) + (40 * UnholyBlight));
            }
            return RP;
        }

        public float manageRPDumping(DeathKnightTalents talents, float RP)
        {
            #region non-ptr
            if (!PTRCalcs)
            {
                if (talents.DancingRuneWeapon > 0f)
                {
                    RP = RP - curRotationDuration * ((100f + 15f * talents.RunicPowerMastery) / 90f);
                }
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
            }
            #endregion
            #region 3.2 PTR
            else
            {
                if (talents.DancingRuneWeapon > 0f)
                {
                    RP -= curRotationDuration * (60f / 90f);
                }
                if (talents.SummonGargoyle > 0f)
                {
                    RP -= curRotationDuration * (60f / 180f);
                    GargoyleDuration = 30f;
                }
                if (talents.FrostStrike > 0f)
                {
                    FrostStrike = RP / (talents.GlyphofFrostStrike ? 32f : 40f);
                    DeathCoil = 0f;
                    UnholyBlight = 0f;
                    RP = 0f;
                }
                else
                {
                    DeathCoil = RP / 40f;
                    FrostStrike = 0f;
                    UnholyBlight = 0f;
                    RP = 0f;
                }
            }
            #endregion
            return RP;
        }

        public float getGCDTime()
        {
            if (presence.Equals(CalculationOptionsDPSDK.Presence.Unholy))
            {
                GCDTime = DeathCoil + IcyTouch + PlagueStrike + ScourgeStrike + UnholyBlight +
                    FrostStrike + HowlingBlast + Obliterate + DeathStrike + BloodStrike +
                    HeartStrike + Horn;
            }
            else if (presence.Equals(CalculationOptionsDPSDK.Presence.Blood))
            {
                GCDTime = 1.5f * (PlagueStrike + ScourgeStrike + FrostStrike + Obliterate + DeathStrike +
                    BloodStrike + HeartStrike);
                GCDTime += 1.5f * (DeathCoil + IcyTouch + UnholyBlight + HowlingBlast + Horn);
                // this does not currently account for haste, and I don't think it is possible in the current design.
            }
            return GCDTime;
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
                    presence = CalculationOptionsDPSDK.Presence.Blood;
                    break;
                case Type.Frost:
                    numDisease = 1f;
                    diseaseUptime = 100f;
                    DeathCoil = 0f;
                    IcyTouch = 1f;
                    PlagueStrike = 0f;
                    ScourgeStrike = 0f;
                    UnholyBlight = 0f;
                    FrostStrike = 3f;
                    HowlingBlast = 0f;
                    Obliterate = 2f;
                    BloodStrike = 1f;
                    HeartStrike = 0f;
                    DancingRuneWeapon = 0f;
                    curRotationDuration = 10f;
                    GargoyleDuration = 0f;
                    DeathStrike = 0f;
                    presence = CalculationOptionsDPSDK.Presence.Blood;
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
                    presence = CalculationOptionsDPSDK.Presence.Blood;
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
