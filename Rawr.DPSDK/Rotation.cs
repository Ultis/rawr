using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.DPSDK
{
    public class Rotation
    {
        public Type curRotationType = Type.Blood;
        public float curRotationDuration = 0f;  // rotation duration in seconds

        //disease info
        public float avgDiseaseMult = 0f;
        public float numDisease = 0f;
        public float diseaseUptime = 0f;
        
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
                    diseaseUptime = 1f;
                    DeathCoil = 2f;
                    IcyTouch = 2f;
                    PlagueStrike = 2f;
                    ScourgeStrike = 0f;
                    UnholyBlight = 0f;
                    FrostStrike = 0f;
                    HowlingBlast = 0f;
                    Obliterate = 1f;
                    BloodStrike = 0f;
                    HeartStrike = 6f;
                    curRotationDuration = 16f;
                    DancingRuneWeapon = 190f;
                    break;
                case Type.Frost:
                    numDisease = 2f;
                    diseaseUptime = 1f;
                    DeathCoil = 2f;
                    IcyTouch = 2f;
                    PlagueStrike = 2f;
                    ScourgeStrike = 0f;
                    UnholyBlight = 0f;
                    FrostStrike = 0f;
                    HowlingBlast = 0f;
                    Obliterate = 0f;
                    BloodStrike = 0f;
                    HeartStrike = 0f;
                    DancingRuneWeapon = 0f;
                    curRotationDuration = 16f;
                    break;
                case Type.Unholy:
                    numDisease = 3f;
                    diseaseUptime = 1f;
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
                    curRotationDuration = 16f;
                    break;
                case Type.Custom:
                    numDisease = 0f;
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
                    break;
            }

            avgDiseaseMult = numDisease * diseaseUptime;
        }
    }
}
