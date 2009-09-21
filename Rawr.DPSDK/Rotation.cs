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
        public float FrostStrike = 0f;
        public float HowlingBlast = 0f;
        public float Obliterate = 0f;
        public float DeathStrike = 0f;
        public float BloodStrike = 0f;
        public float HeartStrike = 0f;
        public float DancingRuneWeapon = 0f;
        public float Horn = 0f;
		public float GhoulFrenzy = 0f;
		public float Pestilence = 0f;

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

            RP = ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.ChillOfTheGrave) * (Obliterate)) +
                ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.Dirge) * (ScourgeStrike)) +
                ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.Dirge) * (DeathStrike)) +
                ((10 + 2.5f * talents.Dirge) * (PlagueStrike)) +
                (10 * (BloodStrike + HeartStrike + Pestilence + GhoulFrenzy)) +
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
                    ((GlyphofFS ? 32 : 40) * FrostStrike));
            }
            return RP;
        }

        public float manageRPDumping(DeathKnightTalents talents, float RP)
        {
			if (talents.DancingRuneWeapon > 0f)
			{
				RP -= curRotationDuration * (60f / 90f);
			}
			if (talents.SummonGargoyle > 0f)
			{
				RP -= curRotationDuration * (60f / 180f);
			}
			if (talents.FrostStrike > 0f)
			{
				FrostStrike = RP / (talents.GlyphofFrostStrike ? 32f : 40f);
				DeathCoil = 0f;
				RP = 0f;
			}
			else
			{
				DeathCoil = RP / 40f;
				FrostStrike = 0f;
				RP = 0f;
			}
            return RP;
        }

        public float getGCDTime()
        {
            if (presence.Equals(CalculationOptionsDPSDK.Presence.Unholy))
            {
                GCDTime = DeathCoil + IcyTouch + PlagueStrike + ScourgeStrike +
                    FrostStrike + HowlingBlast + Obliterate + DeathStrike + BloodStrike +
                    HeartStrike + Horn + Pestilence + GhoulFrenzy;
            }
            else if (presence.Equals(CalculationOptionsDPSDK.Presence.Blood))
            {
                GCDTime = 1.5f * (PlagueStrike + ScourgeStrike + FrostStrike + Obliterate + DeathStrike +
                    BloodStrike + HeartStrike);
                GCDTime += 1.5f * (DeathCoil + IcyTouch + HowlingBlast + Horn + Pestilence + GhoulFrenzy);
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
                    managedRP = true;
                    Horn = 0f;
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
                    GhoulFrenzy = 0f;
                    Pestilence = 0f;
                    presence = CalculationOptionsDPSDK.Presence.Blood;
                    break;
                case Type.Frost:
                    numDisease = 2f;
                    diseaseUptime = 100f;
                    DeathCoil = 0f;
                    IcyTouch = 0f;
                    PlagueStrike = 0f;
                    ScourgeStrike = 0f;
                    managedRP = true;
                    Horn = 0f;
                    FrostStrike = 3f;
                    HowlingBlast = 0f;
                    Obliterate = 5f;
                    BloodStrike = 1f;
                    HeartStrike = 0f;
                    DancingRuneWeapon = 0f;
                    curRotationDuration = 20f;
                    GargoyleDuration = 0f;
                    DeathStrike = 0f;
					Pestilence = 1f;
                    GhoulFrenzy = 0f;
                    presence = CalculationOptionsDPSDK.Presence.Blood;
                    break;
                case Type.Unholy:
                    numDisease = 3f;
                    diseaseUptime = 100f;
                    DeathCoil = 2f;
                    IcyTouch = 1f;
                    PlagueStrike = 1f;
                    ScourgeStrike = 1f;
                    managedRP = true;
                    FrostStrike = 0f;
                    HowlingBlast = 0f;
                    Obliterate = 0f;
                    BloodStrike = 2f;
                    HeartStrike = 0f;
                    DancingRuneWeapon = 0f;
                    curRotationDuration = 10f;
                    Horn = 0.5f;
                    GargoyleDuration = 30f;
                    DeathStrike = 0f;
                    GhoulFrenzy = 0f;
                    Pestilence = 0f;
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
