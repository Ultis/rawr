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

        public enum Type
        {            
            Custom, Blood, Frost, Unholy
        }

        public Rotation()
        {
            setRotation(Type.Unholy);
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
