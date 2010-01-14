using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK {
    public class Rotation {
        // I need FullCharacterStats
        // May need to pull these from the character rather than creating a new stats object here.
        // however these are the stats that are being saved in the XML under the TANKDK area.
        // I only need the Dodge & Parry & Haste Values.
        public float m_fDodge = 0f;
        public float m_fParry = 0f;
        public float m_fPhysicalHaste = 0f;
//        public Stats m_FullStats = new Stats();
        
        public Type curRotationType = Type.Frost;
        
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
        public float FrostStrike = 0f;
        public float HowlingBlast = 0f;
        public float Obliterate = 0f;
        public float DeathStrike = 0f;
        public float BloodStrike = 0f;
        public float HeartStrike = 0f;
        public float HornOfWinter = 0f;
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

        public Rotation( ) 
        {
            this.setRotation(this.curRotationType); 
        }

        public Rotation(DeathKnightTalents t)
        {
            if (t != null)
                this.GetRotationByTalents(t);
            else
                this.setRotation(this.curRotationType); 
        }

        public enum Type { Custom, Blood, Frost, Unholy }

        public float getMeleeSpecialsPerSecond() 
        {
            float temp;
            temp = PlagueStrike + ScourgeStrike + FrostStrike + Obliterate + DeathStrike + BloodStrike + HeartStrike;
            // As per Bloodysorc, BCB should probably be included in this.
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
            this.GlyphofFS = talents.GlyphofFrostStrike;

            // Abilities that grant RP:
            RP = ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.ChillOfTheGrave) * Obliterate) +
                 ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.Dirge) * ScourgeStrike) +
                 ((15 + (fourT7 ? 5 : 0) + 2.5f * talents.Dirge) * DeathStrike) +
                 ((10 + 2.5f * talents.Dirge) * PlagueStrike) +
                 ((10 + 2.5f * talents.ChillOfTheGrave) * IcyTouch) +
                ((15 + 2.5f * talents.ChillOfTheGrave) * HowlingBlast) +
                  (15 * DeathNDecay) +
                  (10 * (BloodStrike + HeartStrike + BloodBoil + HornOfWinter)) +
                 ((curRotationDuration / 5f) * talents.Butchery);


            if (managedRP)
            {
                RP = manageRPDumping(talents, RP);
            } 
            else 
            {
                RP -= ((40 * DeathCoil)
                    + ((GlyphofFS ? 32 : 40) * FrostStrike) 
                    + (20 * RuneStrike));
            }
            return RP;
        }

        /// <summary>
        /// Use abilities that have Runic Power to dump the available RP per rotation.
        /// Primarily focused on RuneStrike.
        /// </summary>
        /// <param name="talents">What are the given talents for this character?</param>
        /// <param name="RP">How much RP generated per rotation?</param>
        /// <returns>Amount of RP remaining after use.</returns>
        public float manageRPDumping(DeathKnightTalents talents, float RP)
        {
            // We need to get the # of boss attacks from the mitigation section to 
            // figure out what is the chance the # of RS we can see.
            // PossibleRS = AverageBossAttacksPerRotation * (Dodge% + Parry%)
            // ActualRS = Math.Min(MaxRSBySpeed, PossibleRS);
            // RuneStrikes count is also based on Dodge & Parry chance:
            // RuneStrike *= (m_FullStats.Dodge + m_FullStats.Parry);
            // Remove the RS shots from the pool of available RP.
            // RP -= (RuneStrike * 20f);*/            

            #region Old Code: using FS/DC for RP dumps.
            /*
            if (talents.FrostStrike > 0f) 
            {
                FrostStrike = RP / (talents.GlyphofFrostStrike ? 32f : 40f);
                DeathCoil = 0f;
                RuneStrike = 0f;
                RP = 0f;
            } 
            else 
            {
                DeathCoil = RP / 40f;
                FrostStrike = 0f;
                RuneStrike = 0f;
                RP = 0f;
            }
            */
            #endregion

            // How many RS limited by RP.
            float fMax_ByRP = RP / 20f;
            // How many RS limited by weapon speed.
            // TODO: figure out best way to get weapon speed into the rotation.
            // float fMax_BySpeed = curRotationDuration / MH.Speed;

            // So for right now, we just setup how many RS's we get, and 
            // Zero out all the rest of the values.
            RuneStrike = fMax_ByRP;
            RP = 0;
            FrostStrike = 0;
            DeathCoil = 0;
            RP = 0;


            return RP;
        }

        public float getRotationDuration() { return getGCDTime(); }
        public float getGCDTime() {
            GCDTime = GetGCDHasted() * (PlagueStrike + ScourgeStrike + FrostStrike + Obliterate + DeathStrike +
                BloodStrike + HeartStrike);
            GCDTime += GetGCDHasted() * (DeathCoil + IcyTouch + HowlingBlast + HornOfWinter + DeathNDecay + BloodBoil + Pestilence);
            return GCDTime;
        }
        /// <summary>
        /// How fast is the GCD with the current character's stats?
        /// </summary>
        /// <param name="s">Total Stats for the character.</param>
        /// <returns>Duration of the GCD in seconds.</returns>
        public float GetGCDHasted() {
            float fNormalGCD = 1.5f;
            float fHastedGCD = Math.Max( 1f, fNormalGCD/(1f + this.m_fPhysicalHaste) );
            return fHastedGCD;
        }

        // Rotations set by Skeleton Jack's 3.2 build and rotation notes.
        public void setRotation(Type t) {
            curRotationType = t;
            switch (curRotationType) {
                case Type.Blood:
                    numDisease = 2f;
                    diseaseUptime = 100f;
                    DeathCoil = 2f;
                    IcyTouch = 1f;
                    PlagueStrike = 1f;
                    ScourgeStrike = 0f;
                    FrostStrike = 0f;
                    HowlingBlast = 0f;
                    Obliterate = 0f;
                    DeathStrike = 2f;
                    BloodStrike = 0f;
                    HeartStrike = 6f;
                    Pestilence = 0f;
                    RuneStrike = 2f;
                    curRotationDuration = 20f;
                    break;
                case Type.Frost:
                    numDisease = 2f;
                    diseaseUptime = 100f;
                    DeathCoil = 0f;
                    IcyTouch = 2f;
                    PlagueStrike = 2f;
                    ScourgeStrike = 0f;
                    FrostStrike = 1f;
                    HowlingBlast = 0f;
                    Obliterate = 3f;
                    BloodStrike = 2f;
                    HeartStrike = 0f;
                    DeathStrike = 0f;
                    RuneStrike = 2f;
                    Pestilence = 0f;
                    curRotationDuration = 20f;
                    break;
                case Type.Unholy:
                    numDisease = 3f;
                    diseaseUptime = 100f;
                    DeathCoil = 0f;
                    IcyTouch = 1f;
                    PlagueStrike = 1f;
                    ScourgeStrike = 4f;
                    FrostStrike = 0f;
                    HowlingBlast = 0f;
                    Obliterate = 0f;
                    BloodStrike = 2f;
                    HeartStrike = 0f;
                    curRotationDuration = 20f;
                    DeathStrike = 0f;
                    RuneStrike = 2f;
                    Pestilence = 0f;
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
                    */
                    break;
            }
        }

        public void GetRotationByTalents(DeathKnightTalents t)
        {
            // Frost
            if (t.HowlingBlast > 0)
            {
                curRotationType = Type.Frost;

                // Default Frost Rotation.
                numDisease = 2f;
                diseaseUptime = 100f;
                DeathCoil = 0f;
                IcyTouch = 2f;
                PlagueStrike = 2f;
                ScourgeStrike = 0f;
                FrostStrike = 1f;
                HowlingBlast = 0f;
                Obliterate = 3f;
                BloodStrike = 2f;
                HeartStrike = 0f;
                DeathStrike = 0f;
                RuneStrike = 3f;
                Pestilence = 0f;
                curRotationDuration = 20f;

                if (t.GlyphofHowlingBlast)
                {
                    // Single Disease Glyphed HB rotation
                    numDisease = 1f;
                    IcyTouch = 1f;
                    PlagueStrike = 1f;
                    HowlingBlast = 1f;
                }
                if (t.Rime > 0)
                {
                    // Additional HB proc based on Rime.
                    HowlingBlast += .5f;
                }
            }
            else if (t.HeartStrike > 0)
            // Blood
            {
                curRotationType = Type.Blood;

                numDisease = 2f;
                diseaseUptime = 100f;
                DeathCoil = 2f;
                IcyTouch = 1f;
                PlagueStrike = 1f;
                ScourgeStrike = 0f;
                FrostStrike = 0f;
                HowlingBlast = 0f;
                Obliterate = 0f;
                DeathStrike = 2f;
                BloodStrike = 0f;
                HeartStrike = 6f;
                Pestilence = 0f;
                RuneStrike = 2f;
                curRotationDuration = 20f;
            }
            else if (t.EbonPlaguebringer > 0)
            // UnHoly
            {
                curRotationType = Type.Unholy;

                numDisease = 3f;
                diseaseUptime = 100f;
                DeathCoil = 0f;
                IcyTouch = 1f;
                PlagueStrike = 1f;
                ScourgeStrike = 4f;
                FrostStrike = 0f;
                HowlingBlast = 0f;
                Obliterate = 0f;
                BloodStrike = 2f;
                HeartStrike = 0f;
                curRotationDuration = 20f;
                DeathStrike = 0f;
                RuneStrike = 2f;
                Pestilence = 0f;
            }
            else
            // Unknown/custom build.
            {
                curRotationType = Type.Custom;
            }
        }

    }
}
