using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSDK
{
    public class Rotation
    {
        public Type curRotationType = Type.Blood;
        public CalculationOptionsDPSDK.Presence presence = CalculationOptionsDPSDK.Presence.Blood;
        private float _curRotationDuration = 0f;  // rotation duration in seconds

        public float CurRotationDuration
        {
            get { return _curRotationDuration; }
            set { _curRotationDuration = value; }
        }

        //disease info
        private float _avgDiseaseMult = 0f;

        public float AvgDiseaseMult
        {
            get { return _avgDiseaseMult; }
            set { _avgDiseaseMult = value; }
        }
        private float _numDisease = 0f;

        public float NumDisease
        {
            get { return _numDisease; }
            set { _numDisease = value; }
        }
        private float _diseaseUptime = 0f;

        public float DiseaseUptime
        {
            get { return _diseaseUptime; }
            set { _diseaseUptime = value; }
        }
        private float _gargoyleDuration = 0f;

        public float GargoyleDuration
        {
            get { return _gargoyleDuration; }
            set { _gargoyleDuration = value; }
        }
        
        private float _deathCoil = 0f;

        public float DeathCoil
        {
            get { return _deathCoil; }
            set { _deathCoil = value; }
        }
        private float _icyTouch = 0f;

        public float IcyTouch
        {
            get { return _icyTouch; }
            set { _icyTouch = value; }
        }
        private float _plagueStrike = 0f;

        public float PlagueStrike
        {
            get { return _plagueStrike; }
            set { _plagueStrike = value; }
        }
        private float _frostFever = 0f;

        public float FrostFever
        {
            get { return _frostFever; }
            set { _frostFever = value; }
        }
        private float _bloodPlague = 0f;

        public float BloodPlague
        {
            get { return _bloodPlague; }
            set { _bloodPlague = value; }
        }
        private float _scourgeStrike = 0f;

        public float ScourgeStrike
        {
            get { return _scourgeStrike; }
            set { _scourgeStrike = value; }
        }
        private float _frostStrike = 0f;

        public float FrostStrike
        {
            get { return _frostStrike; }
            set { _frostStrike = value; }
        }
        private float _howlingBlast = 0f;

        public float HowlingBlast
        {
            get { return _howlingBlast; }
            set { _howlingBlast = value; }
        }
        private float _obliterate = 0f;

        public float Obliterate
        {
            get { return _obliterate; }
            set { _obliterate = value; }
        }
        private float _deathStrike = 0f;

        public float DeathStrike
        {
            get { return _deathStrike; }
            set { _deathStrike = value; }
        }
        private float _bloodStrike = 0f;

        public float BloodStrike
        {
            get { return _bloodStrike; }
            set { _bloodStrike = value; }
        }
        private float _heartStrike = 0f;

        public float HeartStrike
        {
            get { return _heartStrike; }
            set { _heartStrike = value; }
        }
        private float _dancingRuneWeapon = 0f;

        public float DancingRuneWeapon
        {
            get { return _dancingRuneWeapon; }
            set { _dancingRuneWeapon = value; }
        }
        private float _horn = 0f;

        public float Horn
        {
            get { return _horn; }
            set { _horn = value; }
        }
        private float _ghoulFrenzy = 0f;

        public float GhoulFrenzy
        {
            get { return _ghoulFrenzy; }
            set { _ghoulFrenzy = value; }
        }
        private float _pestilence = 0f;

        public float Pestilence
        {
            get { return _pestilence; }
            set { _pestilence = value; }
        }

        private Boolean _managedRP = false;

        public Boolean ManagedRP
        {
            get { return _managedRP; }
            set { _managedRP = value; }
        }
        private Boolean _pTRCalcs = false;

        public Boolean PTRCalcs
        {
            get { return _pTRCalcs; }
            set { _pTRCalcs = value; }
        }
        private float _gCDTime;

        public float GCDTime
        {
            get { return _gCDTime; }
            set { _gCDTime = value; }
        }
        private float _rP;

        public float RP
        {
            get { return _rP; }
            set { _rP = value; }
        }

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
            temp = temp / CurRotationDuration;
            return temp;
        }

        public float getSpellSpecialsPerSecond()
        {
            float temp;
            temp = DeathCoil + IcyTouch + HowlingBlast;
            temp = temp / CurRotationDuration;
            return temp;
        }


        public float getRP(DeathKnightTalents talents, Character character)
        {
            bool fourT7 = character.ActiveBuffsContains("Scourgeborne Battlegear 4 Piece Bonus");
            bool GlyphofIT = talents.GlyphofIcyTouch;
            bool GlyphofFS = talents.GlyphofFrostStrike;

            RP = ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.ChillOfTheGrave) * (Obliterate)) +
                ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.Dirge) * (ScourgeStrike)) +
                ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.Dirge) * (DeathStrike)) +
                ((10 + 2.5f * talents.Dirge) * (PlagueStrike)) +
                (10 * (BloodStrike + HeartStrike + Pestilence + GhoulFrenzy)) +
                ((10 + (GlyphofIT && !PTRCalcs ? 10 : 0) + 2.5f * talents.ChillOfTheGrave) * (IcyTouch)) +
                ((15 + 2.5f * talents.ChillOfTheGrave) * HowlingBlast) +
                (10 * Horn) +
                ((CurRotationDuration / 5f)*talents.Butchery);

            if (ManagedRP)
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
				RP -= CurRotationDuration * (60f / 90f);
			}
			if (talents.SummonGargoyle > 0f)
			{
				RP -= CurRotationDuration * (60f / 180f);
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
                    NumDisease = 2f;
                    DiseaseUptime = 100f;
                    DeathCoil = 3f;
                    IcyTouch = 1f;
                    PlagueStrike = 1f;
                    ScourgeStrike = 0f;
                    ManagedRP = true;
                    Horn = 0f;
                    FrostStrike = 0f;
                    HowlingBlast = 0f;
                    Obliterate = 0f;
                    DeathStrike = 2f;
                    BloodStrike = 0f;
                    HeartStrike = 6f;
                    CurRotationDuration = 20f;
                    DancingRuneWeapon = 190f;
                    GargoyleDuration = 0f;
                    DeathStrike = 2f;
                    GhoulFrenzy = 0f;
                    Pestilence = 0f;
                    presence = CalculationOptionsDPSDK.Presence.Blood;
                    break;
                case Type.Frost:
                    NumDisease = 2f;
                    DiseaseUptime = 100f;
                    DeathCoil = 0f;
                    IcyTouch = 0f;
                    PlagueStrike = 0f;
                    ScourgeStrike = 0f;
                    ManagedRP = true;
                    Horn = 0f;
                    FrostStrike = 3f;
                    HowlingBlast = 0f;
                    Obliterate = 5f;
                    BloodStrike = 1f;
                    HeartStrike = 0f;
                    DancingRuneWeapon = 0f;
                    CurRotationDuration = 20f;
                    GargoyleDuration = 0f;
                    DeathStrike = 0f;
					Pestilence = 1f;
                    GhoulFrenzy = 0f;
                    presence = CalculationOptionsDPSDK.Presence.Blood;
                    break;
                case Type.Unholy:
                    NumDisease = 3f;
                    DiseaseUptime = 100f;
                    DeathCoil = 2f;
                    IcyTouch = 1f;
                    PlagueStrike = 1f;
                    ScourgeStrike = 1f;
                    ManagedRP = true;
                    FrostStrike = 0f;
                    HowlingBlast = 0f;
                    Obliterate = 0f;
                    BloodStrike = 2f;
                    HeartStrike = 0f;
                    DancingRuneWeapon = 0f;
                    CurRotationDuration = 10f;
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
