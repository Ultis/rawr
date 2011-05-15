using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public static class MageWardCycle
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, Cycle baseCycle)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "Mage Ward+" + baseCycle.Name;

            Spell MageWard = castingState.GetSpell(SpellId.MageWard);

            // 1 ward every 30 seconds

            cycle.AreaEffect = baseCycle.AreaEffect;
            cycle.AddSpell(needsDisplayCalculations, MageWard, 1);
            cycle.AddCycle(needsDisplayCalculations, baseCycle, (30 - MageWard.CastTime) / baseCycle.CastTime);
            cycle.Calculate();

            cycle.Note = baseCycle.Note;

            return cycle;
        }
    }

    public static class MirrorImageCycle
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState, Cycle baseCycle)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = baseCycle.Name;
            cycle.AreaEffect = baseCycle.AreaEffect;

            // uptime
            float fightDuration = castingState.CalculationOptions.FightDuration;
            float effectDuration = Solver.MirrorImageDuration;
            float effectCooldown = Solver.MirrorImageCooldown;
            int activations = 0;
            float total;
            if (fightDuration < effectDuration)
            {
                total = fightDuration;
                activations = 1;
            }
            else
            {
                total = effectDuration;
                activations = 1;
                fightDuration -= effectDuration;
                int count = (int)(fightDuration / effectCooldown);
                total += effectDuration * count;
                activations += count;
                fightDuration -= effectCooldown * count;
                fightDuration -= effectCooldown - effectDuration;
                if (fightDuration > 0) 
                {
                    total += fightDuration;
                    activations++;
                }
            }          

            Spell mirrorImage = castingState.GetSpell(SpellId.MirrorImage);
           
            // activations * gcd in fightDuration
            float gcd = castingState.Solver.BaseGlobalCooldown + castingState.CalculationOptions.LatencyGCD;

            cycle.AddCycle(needsDisplayCalculations, baseCycle, (castingState.CalculationOptions.FightDuration - activations * gcd) / baseCycle.CastTime);
            cycle.CastTime += activations * gcd;
            cycle.costPerSecond += activations * (int)(0.10 * SpellTemplate.BaseMana[castingState.CalculationOptions.PlayerLevel]);
            //effectDamagePerSecond += (mirrorImage.AverageDamage + spellPower * mirrorImage.DamagePerSpellPower) / mirrorImage.CastTime;
            cycle.damagePerSecond += total * mirrorImage.AverageDamage / mirrorImage.CastTime;
            cycle.DpsPerSpellPower += total * mirrorImage.DamagePerSpellPower / mirrorImage.CastTime;
            cycle.Calculate();

            cycle.Note = baseCycle.Note;

            return cycle;
        }
    }

    public static class AE4AB
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AE4AB";

            Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
            Spell AE4 = castingState.GetSpell(SpellId.ArcaneExplosion4);

            // AEx4-AB

            // 6 seconds on AB debuff - time to refresh AB
            int aeCount = (int)((6.0f - AB4.CastTime) / AE4.CastTime);

            cycle.AddSpell(needsDisplayCalculations, AE4, aeCount);
            cycle.AddSpell(needsDisplayCalculations, AB4, 1);
            cycle.Calculate();

            cycle.AreaEffect = true;

            return cycle;
        }
    }

    public static class AERampAB
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AERampAB";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
            Spell AE1 = castingState.GetSpell(SpellId.ArcaneExplosion1);
            Spell AE2 = castingState.GetSpell(SpellId.ArcaneExplosion2);
            Spell AE3 = castingState.GetSpell(SpellId.ArcaneExplosion3);
            Spell AE4 = castingState.GetSpell(SpellId.ArcaneExplosion4);

            // ABx2-AEx4-AB-AEx4-AB-AEx6

            // 6 seconds on AB debuff - time to refresh AB
            int ae1Count = (int)((6.0f - AB1.CastTime) / AE1.CastTime);
            int ae2Count = (int)((6.0f - AB2.CastTime) / AE2.CastTime);
            int ae3Count = (int)((6.0f - AB3.CastTime) / AE3.CastTime);
            int ae4Count = (int)Math.Ceiling(6.0f / AE4.CastTime);

            cycle.AddSpell(needsDisplayCalculations, AB0, 1);
            cycle.AddSpell(needsDisplayCalculations, AE1, ae1Count);
            cycle.AddSpell(needsDisplayCalculations, AB1, 1);
            cycle.AddSpell(needsDisplayCalculations, AE2, ae2Count);
            cycle.AddSpell(needsDisplayCalculations, AB2, 1);
            cycle.AddSpell(needsDisplayCalculations, AE3, ae3Count);
            cycle.AddSpell(needsDisplayCalculations, AB3, 1);
            cycle.AddSpell(needsDisplayCalculations, AE4, ae4Count);
            cycle.Calculate();

            cycle.AreaEffect = true;

            return cycle;
        }
    }

    public static class ABAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "ABAM";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB1);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to AB-AM
                cycle.AddSpell(needsDisplayCalculations, AB, 1);
                cycle.AddSpell(needsDisplayCalculations, AM, 1);
                cycle.Calculate();
            }
            else
            {
                // S0: no proc
                // AB-AM    => S0   (1 - MB)
                // AB-MBAM  => S0   MB * (1 - T8)
                // AB-MBAM  => S1   MB * T8

                // S1: proc
                // AB-MBAM  => S0   (1 - T8)
                // AB-MBAM  => S1   T8

                // S0 = S0 * (1 - MB + MB * (1 - T8)) + S1 * (1 - T8)
                // S1 = S0 * MB * T8 + S1 * T8
                // S0 + S1 = 1

                float S0 = (1 - T8) / (1 - (1 - MB) * T8);
                float K1 = 1 - MB;
                
                cycle.AddSpell(needsDisplayCalculations, AB, 1);
                cycle.AddSpell(needsDisplayCalculations, AM, S0 * K1);
                cycle.AddSpell(needsDisplayCalculations, MBAM, 1 - S0 * K1);

                cycle.Calculate();
            }
            return cycle;
        }
    }

    public static class AB3AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB3AM";

            // S0: no proc
            // AB0-AB1-AB2-AM3    => S0   (1 - MB) * (1 - MB) * (1 - MB)
            // AB0-AB1-AB2-MBAM   => S0   (1 - (1 - MB) * (1 - MB) * (1 - MB)) * (1 - T8)
            // AB0-AB1-AB2-MBAM   => S1   (1 - (1 - MB) * (1 - MB) * (1 - MB)) * T8

            // S1: proc
            // AB0-AB1-AB2-MBAM   => S0   (1 - T8)
            // AB0-AB1-AB2-MBAM   => S1   T8

            // S1 = S0 * (1 - (1 - MB) * (1 - MB) * (1 - MB)) * T8 + S1 * T8
            // S0 + S1 = 1

            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K1 = (1 - MB) * (1 - MB) * (1 - MB);
            float S0 = (1 - T8) / (1 - K1 * T8);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                cycle.AddSpell(needsDisplayCalculations, AB0, 1);
                cycle.AddSpell(needsDisplayCalculations, AB1, 1);
                cycle.AddSpell(needsDisplayCalculations, AB2, 1);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, 1, 1, 1, 0);
            }
            cycle.AddSpell(needsDisplayCalculations, AM3, S0 * K1);
            cycle.AddSpell(needsDisplayCalculations, MBAM3, 1 - S0 * K1);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB4AM234MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB4AM234MBAM";

            // S0: no proc
            // AB0-AB1-AB2-AB3-AM4    => S0    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            // AB0-AB1-AB2-AB3-MBAM4   => S0   (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB)) * (1 - T8)
            // AB0-AB1-AB2-AB3-MBAM4   => S1   (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB)) * T8
            // AB0-AB1-AB2-MBAM3   => S0       (1 - MB) * MB * (1 - T8)
            // AB0-AB1-AB2-MBAM3   => S1       (1 - MB) * MB * T8
            // AB0-AB1-MBAM2   => S0           MB * (1 - T8)
            // AB0-AB1-MBAM2   => S1           MB * T8

            // S1: proc
            // AB0-AB1-MBAM2   => S0   (1 - T8)
            // AB0-AB1-MBAM2   => S1   T8

            // K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            // S1 = S0 * (1 - K0) * T8 + S1 * T8
            // S0 + S1 = 1

            // S1 = S0 * (1 - K0) * T8 / (1 - T8)
            // S0 = (1 - T8) / (1 - K0 * T8)
            // S1 = (1 - K0) * T8 / (1 - K0 * T8)


            Spell AM4 = castingState.GetSpell(SpellId.ArcaneMissiles4);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);

            float MB = 0.08f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            float S0 = (1 - T8) / (1 - K0 * T8);
            float S1 = (1 - K0) * T8 / (1 - K0 * T8);
            float K1 = S0 * K0;
            float K2 = S0 * (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            float K3 = S0 * (1 - MB) * MB;
            float K4 = S0 * MB + S1;

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                cycle.AddSpell(needsDisplayCalculations, AB0, 1);
                cycle.AddSpell(needsDisplayCalculations, AB1, 1);
                cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, 1, 1, K1 + K2 + K3, K1 + K2);
            }
            cycle.AddSpell(needsDisplayCalculations, AM4, K1);
            cycle.AddSpell(needsDisplayCalculations, MBAM4, K2);
            cycle.AddSpell(needsDisplayCalculations, MBAM3, K3);
            cycle.AddSpell(needsDisplayCalculations, MBAM2, K4);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB4AM0234MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB4AM0234MBAM";

            if (castingState.Solver.Mage2T10)
            {
                // doesn't support haste transferring over several loops

                // S0:
                // AB0-AB1-AB2-AB3-AM4 => S1     (1-MB)*(1-MB)*(1-MB)*(1-MB)
                // AB0-AB1-AB2-AB3-MBAM4 => S0   (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))
                // AB0-AB1-AB2-MBAM3 => S0       (1-MB)*MB
                // AB0-AB1-MBAM2 => S0           MB
                // S1:
                // AB0-AB1-AB2-AB3-AM4 => S1     (1-MB)*(1-MB)*(1-MB)*(1-MB)
                // AB0-AB1-AB2-AB3-MBAM4 => S0   (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))
                // AB0-AB1-AB2-MBAM3 => S0       (1-MB)*MB
                // AB0-AB1-MBAM2 => S0           MB


                // K0 := (1-MB)*(1-MB)*(1-MB)*(1-MB)
                // MC := 1-MB

                // S1 = K0
                // S0 = 1 - K0


                Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                Spell AM4 = castingState.GetSpell(SpellId.ArcaneMissiles4);
                Spell MBAM2 = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2);

                float m2T10time = 5.0f - castingState.CalculationOptions.LatencyChannel;
                Spell AB0A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0A.CastTime;
                Spell AB1A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1A.CastTime;
                Spell MBAM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2A.CastTime;
                Spell MBAM3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB3) : castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AB3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                m2T10time -= AB3A.CastTime;
                Spell MBAM4A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB4) : castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                Spell AM4A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles4) : castingState.GetSpell(SpellId.ArcaneMissiles4);

                Spell AB0C = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1C = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell MBAM2C = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2C = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell MBAM3C = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AB3C = castingState.GetSpell(SpellId.ArcaneBlast3);
                Spell MBAM4C = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                Spell AM4C = castingState.GetSpell(SpellId.ArcaneMissiles4);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float MC = 1 - MB;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                float K2 = (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
                float K3 = (1 - MB) * MB;

                float S0 = 1 - K0;
                float S1 = K0;

                cycle.AddSpell(needsDisplayCalculations, AB0A, S0);
                cycle.AddSpell(needsDisplayCalculations, AB1A, S0);
                cycle.AddSpell(needsDisplayCalculations, AB2A, S0 * MC);
                cycle.AddSpell(needsDisplayCalculations, AB3A, S0 * MC * MC);
                cycle.AddSpell(needsDisplayCalculations, MBAM2A, S0 * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM3A, S0 * K3);
                cycle.AddSpell(needsDisplayCalculations, MBAM4A, S0 * K2);
                cycle.AddSpell(needsDisplayCalculations, AM4A, S0 * K0);

                cycle.AddSpell(needsDisplayCalculations, AB0C, S1);
                cycle.AddSpell(needsDisplayCalculations, AB1C, S1);
                cycle.AddSpell(needsDisplayCalculations, AB2C, S1 * MC);
                cycle.AddSpell(needsDisplayCalculations, AB3C, S1 * MC * MC);
                cycle.AddSpell(needsDisplayCalculations, MBAM2C, S1 * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM3C, S1 * K3);
                cycle.AddSpell(needsDisplayCalculations, MBAM4C, S1 * K2);
                cycle.AddSpell(needsDisplayCalculations, AM4C, S1 * K0);
            }
            else
            {
                // S0: no proc
                // AB0-AB1-AB2-AB3-AM4    => S0    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-AB3-MBAM4   => S0   (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB)) * (1 - T8)
                // AB0-AB1-AB2-AB3-MBAM4   => S1   (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB)) * T8
                // AB0-AB1-AB2-MBAM3   => S0       (1 - MB) * MB * (1 - T8)
                // AB0-AB1-AB2-MBAM3   => S1       (1 - MB) * MB * T8
                // AB0-AB1-MBAM2   => S0           MB * (1 - T8)
                // AB0-AB1-MBAM2   => S1           MB * T8

                // S1: proc
                // MBAM0   => S0   (1 - T8)
                // MBAM0   => S1   T8

                // K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
                // S1 = S0 * (1 - K0) * T8 + S1 * T8
                // S0 + S1 = 1

                // S1 = S0 * (1 - K0) * T8 / (1 - T8)
                // S0 = (1 - T8) / (1 - K0 * T8)
                // S1 = (1 - K0) * T8 / (1 - K0 * T8)


                Spell AM4 = castingState.GetSpell(SpellId.ArcaneMissiles4);
                Spell MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);
                Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float T8 = 0;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                float S0 = (1 - T8) / (1 - K0 * T8);
                float S1 = (1 - K0) * T8 / (1 - K0 * T8);
                float K1 = S0 * K0;
                float K2 = S0 * (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
                float K3 = S0 * (1 - MB) * MB;
                float K4 = S0 * MB;

                if (needsDisplayCalculations)
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                    Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                    cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                    cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                    cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2);
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, S0, S0, K1 + K2 + K3, K1 + K2);
                }
                cycle.AddSpell(needsDisplayCalculations, AM4, K1);
                cycle.AddSpell(needsDisplayCalculations, MBAM4, K2);
                cycle.AddSpell(needsDisplayCalculations, MBAM3, K3);
                cycle.AddSpell(needsDisplayCalculations, MBAM2, K4);
                cycle.AddSpell(needsDisplayCalculations, MBAM0, S1);
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB3AM23MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB3AM23MBAM";

            // S0: no proc
            // AB0-AB1-AB2-AM3    => S0    (1 - MB) * (1 - MB) * (1 - MB)
            // AB0-AB1-AB2-MBAM3   => S0   (1 - MB) * (1 - (1 - MB) * (1 - MB)) * (1 - T8)
            // AB0-AB1-AB2-MBAM3   => S1   (1 - MB) * (1 - (1 - MB) * (1 - MB)) * T8
            // AB0-AB1-MBAM2   => S0       MB * (1 - T8)
            // AB0-AB1-MBAM2   => S1       MB * T8

            // S1: proc
            // AB0-AB1-MBAM2   => S0   (1 - T8)
            // AB0-AB1-MBAM2   => S1   T8

            // K0 = (1 - MB) * (1 - MB) * (1 - MB)
            // S1 = S0 * (1 - K0) * T8 + S1 * T8
            // S0 + S1 = 1

            // S1 = S0 * (1 - K0) * T8 / (1 - T8)
            // S0 = (1 - T8) / (1 - K0 * T8)
            // S1 = (1 - K0) * T8 / (1 - K0 * T8)


            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            float MB = 0.08f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB);
            float S0 = (1 - T8) / (1 - K0 * T8);
            float S1 = (1 - K0) * T8 / (1 - K0 * T8);
            float K1 = S0 * K0;
            float K2 = S0 * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            float K4 = S0 * MB + S1;

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                cycle.AddSpell(needsDisplayCalculations, AB0, 1);
                cycle.AddSpell(needsDisplayCalculations, AB1, 1);
                cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, 1, 1, K1 + K2, 0);
            }
            cycle.AddSpell(needsDisplayCalculations, AM3, K1);
            cycle.AddSpell(needsDisplayCalculations, MBAM3, K2);
            cycle.AddSpell(needsDisplayCalculations, MBAM2, K4);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB2ABar12AMABABar
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB2ABar12AMABABar";

            if (castingState.Solver.Mage2T10)
            {
                // doesn't support haste transferring over several loops

                // S0:
                // AB0-AB1-ABar            => S0    (1-MB)*(1-MB)*(1-MB)
                // AB0-AB1-AM-AB0-ABar     => S0    (1-MB)*(1-MB)*MB
                // AB0-AM-AB0-ABar         => S0    (1 - (1-MB)*(1-MB))

                Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
                Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell AB0T = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0);
                Spell ABarT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBarrage);
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);

                float MB = 0.4f;
                float K2 = (1 - MB) * (1 - MB);
                float K3 = K2 * (1 - MB);
                float K4 = K2 * K2;

                cycle.AddSpell(needsDisplayCalculations, AB0, 1);
                cycle.AddSpell(needsDisplayCalculations, AB1, K2);
                cycle.AddSpell(needsDisplayCalculations, ABar, K3);
                cycle.AddSpell(needsDisplayCalculations, AM, 1 - K3);
                cycle.AddSpell(needsDisplayCalculations, AB0T, 1 - K3);
                cycle.AddSpell(needsDisplayCalculations, ABarT, 1 - K3);
                if (AB0.CastTime + AB1.CastTime + ABar.CastTime < ABar.Cooldown)
                {
                    cycle.AddPause(ABar.Cooldown - AB0.CastTime - AB1.CastTime - ABar.CastTime, K3);
                }
                if (AB0.CastTime + AM.CastTime + AB0T.CastTime + ABarT.CastTime < ABar.Cooldown)
                {
                    cycle.AddPause(ABar.Cooldown - AB0.CastTime - AM.CastTime - AB0T.CastTime - ABarT.CastTime, 1 - K2);
                }
            }
            else
            {
                Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
                Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);

                float MB = 0.4f;
                float K2 = (1 - MB) * (1 - MB);
                float K3 = K2 * (1 - MB);
                float K4 = K2 * K2;

                cycle.AddSpell(needsDisplayCalculations, AB0, 2 - K3);
                cycle.AddSpell(needsDisplayCalculations, AB1, K2);
                cycle.AddSpell(needsDisplayCalculations, ABar, K3 + 1 - K3);
                cycle.AddSpell(needsDisplayCalculations, AM, 1 - K3);
                if (AB0.CastTime + AB1.CastTime + ABar.CastTime < ABar.Cooldown)
                {
                    cycle.AddPause(ABar.Cooldown - AB0.CastTime - AB1.CastTime - ABar.CastTime, K3);
                }
                if (AM.CastTime + 2 * AB0.CastTime + ABar.CastTime < ABar.Cooldown)
                {
                    cycle.AddPause(ABar.Cooldown - AM.CastTime - 2 * AB0.CastTime - ABar.CastTime, 1 - K2);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABABar1AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "ABABar1AM";

            // S0:
            // AB0-ABar         => S0    (1-MB)*(1-MB)
            // AB0-ABar-AB0-AM  => S0    (1-MB)*MB
            // AB0-AM           => S0    MB

            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);

            float MB = 0.4f;
            float K2 = (1 - MB) * (1 - MB);
            float K3 = MB * (1 - MB);

            cycle.AddSpell(needsDisplayCalculations, AB0, 1 + K3);
            cycle.AddSpell(needsDisplayCalculations, ABar, 1 - MB);
            cycle.AddSpell(needsDisplayCalculations, AM, 1 - K2);
            cycle.AddPause(ABar.Cooldown - ABar.CastTime - AB0.CastTime, 1 - MB);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB2ABar02AMABABar
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB2ABar02AMABABar";

            if (castingState.Solver.Mage2T10)
            {
                // doesn't support haste transferring over several loops

                // S0:
                // AB0-AB1-ABar            => S0    (1-MB)*(1-MB)*(1-MB)
                // AB0-AB1-AM-AB0-ABar     => S0    (1-MB)*(1-MB)*MB
                // AM-AB0-ABar             => S0    (1 - (1-MB)*(1-MB))

                Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
                Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);                
                Spell AB0T = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0);
                Spell ABarT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBarrage);
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);

                float MB = 0.4f;
                float K2 = (1 - MB) * (1 - MB);
                float K3 = K2 * (1 - MB);
                float K4 = K2 * K2;

                cycle.AddSpell(needsDisplayCalculations, AB0, K2);
                cycle.AddSpell(needsDisplayCalculations, AB1, K2);
                cycle.AddSpell(needsDisplayCalculations, ABar, K3);
                cycle.AddSpell(needsDisplayCalculations, AM, 1 - K3);
                cycle.AddSpell(needsDisplayCalculations, AB0T, 1 - K3);
                cycle.AddSpell(needsDisplayCalculations, ABarT, 1 - K3);
                if (AB0.CastTime + AB1.CastTime + ABar.CastTime < ABar.Cooldown)
                {
                    cycle.AddPause(ABar.Cooldown - AB0.CastTime - AB1.CastTime - ABar.CastTime, K3);
                }
                if (AM.CastTime + AB0T.CastTime + ABarT.CastTime < ABar.Cooldown)
                {
                    cycle.AddPause(ABar.Cooldown - AM.CastTime - AB0T.CastTime - ABarT.CastTime, 1 - K2);
                }
            }
            else
            {
                Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
                Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);

                float MB = 0.4f;
                float K2 = (1 - MB) * (1 - MB);
                float K3 = K2 * (1 - MB);
                float K4 = K2 * K2;

                cycle.AddSpell(needsDisplayCalculations, AB0, K2 + 1 - K3);
                cycle.AddSpell(needsDisplayCalculations, AB1, K2);
                cycle.AddSpell(needsDisplayCalculations, ABar, K3 + 1 - K3);
                cycle.AddSpell(needsDisplayCalculations, AM, 1 - K3);
                if (AB0.CastTime + AB1.CastTime + ABar.CastTime < ABar.Cooldown)
                {
                    cycle.AddPause(ABar.Cooldown - AB0.CastTime - AB1.CastTime - ABar.CastTime, K3);
                }
                if (AM.CastTime + AB0.CastTime + ABar.CastTime < ABar.Cooldown)
                {
                    cycle.AddPause(ABar.Cooldown - AM.CastTime - AB0.CastTime - ABar.CastTime, 1 - K2);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB23ABar023AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB23ABar023AM";

            if (castingState.Solver.Mage2T10)
            {
                // doesn't support haste transferring over several loops

                // S0:
                // AB0-AB1-ABar        => S1    (1 - MB)
                // AB0-AB1-AM          => S0    MB
                // S1:
                // AB0-AB1-AB2-ABar    => S1    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-AM      => S0    (1 - MB) * (1 - MB) * (1 - MB) * MB
                // AB0-AB1-AM          => S0    (1 - MB) * (1 - MB) * MB
                // AM                  => S0    (1 - (1 - MB) * (1 - MB))

                // K2 := (1-MB)*(1-MB)
                // K4 := K2 * K2

                // S0 = MB * S0 + (1-K4) * S1
                // S1 = (1-MB) * S0 + K4 * S1
                // S0 + S1 = 1

                // S0 = (K4-1)/(K4+MB-2),   

                Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
                Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell AMT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles);

                float m2T10time = 5.0f - castingState.CalculationOptions.LatencyChannel;
                Spell AB0A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0A.CastTime;
                Spell AB1A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1A.CastTime;
                Spell AM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles) : castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell ABar2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBarrage) : castingState.GetSpell(SpellId.ArcaneBarrage);

                Spell AB0C = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1C = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AMC = castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell AB2C = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell ABarC = castingState.GetSpell(SpellId.ArcaneBarrage);

                float MB = 0.4f;
                float K2 = (1 - MB) * (1 - MB);
                float K4 = K2 * K2;

                float S0 = (K4 - 1) / (K4 + MB - 2);
                float S1 = 1 - S0;

                cycle.AddSpell(needsDisplayCalculations, AB0A, S0);
                cycle.AddSpell(needsDisplayCalculations, AB1A, S0);
                cycle.AddSpell(needsDisplayCalculations, AM2A, S0 * MB);
                cycle.AddSpell(needsDisplayCalculations, ABar2A, S0 * (1 - MB));

                cycle.AddSpell(needsDisplayCalculations, AB0C, S1 * K2);
                cycle.AddSpell(needsDisplayCalculations, AB1C, S1 * K2);
                cycle.AddSpell(needsDisplayCalculations, AB2C, S1 * K2 * (1 - MB));
                cycle.AddSpell(needsDisplayCalculations, AMC, S1 * (1 - K4));
                cycle.AddSpell(needsDisplayCalculations, ABarC, S1 * K4);
            }
            else
            {
                Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);

                float MB = 0.4f;
                float K2 = (1 - MB) * (1 - MB);
                float K4 = K2 * K2;
                float ABcast;

                float S0 = (K4 - 1) / (K4 + MB - 2);
                float S1 = 1 - S0;

                if (needsDisplayCalculations)
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                    cycle.AddSpell(needsDisplayCalculations, AB0, S0 + S1 * K2);
                    cycle.AddSpell(needsDisplayCalculations, AB1, S0 + S1 * K2);
                    cycle.AddSpell(needsDisplayCalculations, AB2, S1 * K2 * (1 - MB));
                    ABcast = AB0.CastTime;
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, S0 + S1 * K2, S0 + S1 * K2, S1 * K2 * (1 - MB), 0);
                    ABcast = AB.CastTime;
                }
                cycle.AddSpell(needsDisplayCalculations, AM, S0 * MB + S1 * (1 - K4));
                cycle.AddSpell(needsDisplayCalculations, ABar, S0 * (1 - MB) + S1 * K4);
                // a bit overkill on pause, but make sure we respect the ABar cooldown
                if (3 * ABcast + ABar.CastTime < ABar.Cooldown)
                {
                    cycle.AddPause(ABar.Cooldown - 3 * ABcast - ABar.CastTime, S0 * (1 - MB) + S1 * K4);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB3ABar023AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB3ABar023AM";

            if (castingState.Solver.Mage2T10)
            {
                // doesn't support haste transferring over several loops

                // S0:
                // AB0-AB1-AB2-ABar    => S1    (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-AM      => S0    (1 - MB) * MB
                // AB0-AB1-AM          => S0    MB
                // S1:
                // AB0-AB1-AB2-ABar    => S1    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-AM      => S0    (1 - MB) * (1 - MB) * (1 - MB) * MB
                // AB0-AB1-AM          => S0    (1 - MB) * (1 - MB) * MB
                // AM                  => S0    (1 - (1 - MB) * (1 - MB))

                // K2 := (1-MB)*(1-MB)
                // K4 := K2 * K2

                // S0 = (1 - K2) * S0 + (1 - K4) * S1
                // S1 = K2 * S0 + K4 * S1
                // S0 + S1 = 1

                // S0 = (1-K4)/(1-K4+K2)

                Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
                Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell AMT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles);

                float m2T10time = 5.0f - castingState.CalculationOptions.LatencyChannel;
                Spell AB0A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0A.CastTime;
                Spell AB1A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1A.CastTime;
                Spell AM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles) : castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell AB2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2A.CastTime;
                Spell AM3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles) : castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell ABar3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBarrage) : castingState.GetSpell(SpellId.ArcaneBarrage);

                Spell AB0C = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1C = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AMC = castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell AB2C = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell ABarC = castingState.GetSpell(SpellId.ArcaneBarrage);

                float MB = 0.4f;
                float K2 = (1 - MB) * (1 - MB);
                float K4 = K2 * K2;

                float S0 = (1 - K4) / (1 - K4 + K2);
                float S1 = 1 - S0;

                cycle.AddSpell(needsDisplayCalculations, AB0A, S0);
                cycle.AddSpell(needsDisplayCalculations, AB1A, S0);
                cycle.AddSpell(needsDisplayCalculations, AB2A, S0 * (1 - MB));
                cycle.AddSpell(needsDisplayCalculations, AM2A, S0 * MB);
                cycle.AddSpell(needsDisplayCalculations, AM3A, S0 * (1 - MB) * MB);
                cycle.AddSpell(needsDisplayCalculations, ABar3A, S0 * K2);

                cycle.AddSpell(needsDisplayCalculations, AB0C, S1 * K2);
                cycle.AddSpell(needsDisplayCalculations, AB1C, S1 * K2);
                cycle.AddSpell(needsDisplayCalculations, AB2C, S1 * K2 * (1 - MB));
                cycle.AddSpell(needsDisplayCalculations, AMC, S1 * (1 - K4));
                cycle.AddSpell(needsDisplayCalculations, ABarC, S1 * K4);
            }
            else
            {
                // S0:
                // AB0-AB1-AB2-ABar    => S1    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-ABar-AM => S0    (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
                // AB0-AB1-AB2-AM      => S0    (1 - MB) * MB
                // AB0-AB1-AM          => S0    MB

                Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
                Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);

                float MB = 0.4f;
                float K2 = (1 - MB) * (1 - MB);
                float K4 = K2 * K2;
                float ABcast;

                if (needsDisplayCalculations)
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                    cycle.AddSpell(needsDisplayCalculations, AB0, 1);
                    cycle.AddSpell(needsDisplayCalculations, AB1, 1);
                    cycle.AddSpell(needsDisplayCalculations, AB2, 1 - MB);
                    ABcast = AB0.CastTime;
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, 1, 1, 1 - MB, 0);
                    ABcast = AB.CastTime;
                }
                cycle.AddSpell(needsDisplayCalculations, AM, 1 - K4);
                cycle.AddSpell(needsDisplayCalculations, ABar, K2);
                // a bit overkill on pause, but make sure we respect the ABar cooldown
                if (3 * ABcast + ABar.CastTime < ABar.Cooldown)
                {
                    cycle.AddPause(ABar.Cooldown - 3 * ABcast - ABar.CastTime, K2);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB4ABar1234AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB4ABar1234AM";

            // S0:
            // AB0-AB1-AB2-AB3-ABar        => S0    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            // AB0-AB1-AB2-AB3-ABar-AB0-AM => S0    (1 - MB) * (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
            // AB0-AB1-AB2-AB3-AM          => S0    (1 - MB) * (1 - MB) * MB
            // AB0-AB1-AB2-AM              => S0    (1 - MB) * MB
            // AB0-AB1-AM                  => S0    MB

            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);

            float MB = 0.4f;
            float K2 = (1 - MB) * (1 - MB);
            float K3 = K2 * (1 - MB);
            float K4 = K2 * K2;
            float K0 = K3 * (1 - K2);
            float K5 = K3 * K2;
            float ABcast;

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                cycle.AddSpell(needsDisplayCalculations, AB0, 1 + K0);
                cycle.AddSpell(needsDisplayCalculations, AB1, 1);
                cycle.AddSpell(needsDisplayCalculations, AB2, 1 - MB);
                cycle.AddSpell(needsDisplayCalculations, AB3, K2);
                ABcast = AB0.CastTime;
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, 1 + K0, 1, 1 - MB, K2);
                ABcast = AB.CastTime;
            }
            cycle.AddSpell(needsDisplayCalculations, AM, 1 - K5);
            cycle.AddSpell(needsDisplayCalculations, ABar, K3);
            // a bit overkill on pause, but make sure we respect the ABar cooldown
            if (4 * ABcast + ABar.CastTime < ABar.Cooldown)
            {
                cycle.AddPause(ABar.Cooldown - 4 * ABcast - ABar.CastTime, K3);
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB4ABar34AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB4ABar34AM";

            // S0:
            // AB0-AB1-AB2-AB3-ABar                => S0    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            // AB0-AB1-AB2-AB3-ABar-AB0-AB1-AB2-AM => S0    (1 - MB) * (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
            // AB0-AB1-AB2-AB3-AM                  => S0    (1 - MB) * (1 - MB) * MB
            // AB0-AB1-AB2-AM                      => S0    (1 - (1 - MB) * (1 - MB))

            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);

            float MB = 0.4f;
            float K2 = (1 - MB) * (1 - MB);
            float K3 = K2 * (1 - MB);
            float K4 = K2 * K2;
            float K0 = K3 * (1 - K2);
            float K5 = K3 * K2;
            float ABcast;

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                cycle.AddSpell(needsDisplayCalculations, AB0, 1 + K0);
                cycle.AddSpell(needsDisplayCalculations, AB1, 1 + K0);
                cycle.AddSpell(needsDisplayCalculations, AB2, 1 + K0);
                cycle.AddSpell(needsDisplayCalculations, AB3, K2);
                ABcast = AB0.CastTime;
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, 1 + K0, 1 + K0, 1 + K0, K2);
                ABcast = AB.CastTime;
            }
            cycle.AddSpell(needsDisplayCalculations, AM, 1 - K5);
            cycle.AddSpell(needsDisplayCalculations, ABar, K3);
            // a bit overkill on pause, but make sure we respect the ABar cooldown
            if (4 * ABcast + ABar.CastTime < ABar.Cooldown)
            {
                cycle.AddPause(ABar.Cooldown - 4 * ABcast - ABar.CastTime, K3);
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB4ABar4AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB4ABar4AM";

            // S0:
            // AB0-AB1-AB2-AB3-ABar                    => S0    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            // AB0-AB1-AB2-AB3-ABar-AB0-AB1-AB2-AB3-AM => S0    (1 - MB) * (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
            // AB0-AB1-AB2-AB3-AM                      => S0    (1 - (1 - MB) * (1 - MB) * (1 - MB))

            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);

            float MB = 0.4f;
            float K2 = (1 - MB) * (1 - MB);
            float K3 = K2 * (1 - MB);
            float K4 = K2 * K2;
            float K0 = K3 * (1 - K2);
            float K5 = K3 * K2;
            float ABcast;

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                cycle.AddSpell(needsDisplayCalculations, AB0, 1 + K0);
                cycle.AddSpell(needsDisplayCalculations, AB1, 1 + K0);
                cycle.AddSpell(needsDisplayCalculations, AB2, 1 + K0);
                cycle.AddSpell(needsDisplayCalculations, AB3, 1 + K0);
                ABcast = AB0.CastTime;
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, 1 + K0, 1 + K0, 1 + K0, 1 + K0);
                ABcast = AB.CastTime;
            }
            cycle.AddSpell(needsDisplayCalculations, AM, 1 - K5);
            cycle.AddSpell(needsDisplayCalculations, ABar, K3);
            // a bit overkill on pause, but make sure we respect the ABar cooldown
            if (4 * ABcast + ABar.CastTime < ABar.Cooldown)
            {
                cycle.AddPause(ABar.Cooldown - 4 * ABcast - ABar.CastTime, K3);
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB3ABar123AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB3ABar123AM";

            // S0:
            // AB0-AB1-AB2-ABar        => S0    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            // AB0-AB1-AB2-ABar-AB0-AM => S0    (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
            // AB0-AB1-AB2-AM          => S0    (1 - MB) * MB
            // AB0-AB1-AM              => S0    MB

            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);

            float MB = 0.4f;
            float K2 = (1 - MB) * (1 - MB);
            float K3 = K2 * (1 - MB);
            float K4 = K2 * K2;
            float K0 = K2 * (1 - K2);
            float ABcast;

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                cycle.AddSpell(needsDisplayCalculations, AB0, 1 + K0);
                cycle.AddSpell(needsDisplayCalculations, AB1, 1);
                cycle.AddSpell(needsDisplayCalculations, AB2, 1 - MB);
                ABcast = AB0.CastTime;
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, 1 + K0, 1, 1 - MB, 0);
                ABcast = AB.CastTime;
            }
            cycle.AddSpell(needsDisplayCalculations, AM, 1 - K4);
            cycle.AddSpell(needsDisplayCalculations, ABar, K2);
            // a bit overkill on pause, but make sure we respect the ABar cooldown
            if (3 * ABcast + ABar.CastTime < ABar.Cooldown)
            {
                cycle.AddPause(ABar.Cooldown - 3 * ABcast - ABar.CastTime, K2);
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB3AM023MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB3AM023MBAM";

            if (castingState.Solver.Mage2T10)
            {
                // doesn't support haste transferring over several loops

                // S0:
                // AB0-AB1-AB2-AM3    => S1    (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-MBAM3   => S0   (1 - MB) * (1 - (1 - MB) * (1 - MB))
                // AB0-AB1-MBAM2   => S0       MB
                // S1:
                // AB0-AB1-AB2-AM3    => S1    (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-MBAM3   => S0   (1 - MB) * (1 - (1 - MB) * (1 - MB))
                // AB0-AB1-MBAM2   => S0       MB

                // K0 := (1-MB)*(1-MB)*(1-MB)

                // MC := 1-MB

                // K2 := (1 - MB) * (1 - (1 - MB) * (1 - MB))

                // S0 = 1 - K0
                // S1 = K0


                Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
                Spell MBAM2 = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2);

                float m2T10time = 5.0f - castingState.CalculationOptions.LatencyChannel;
                Spell AB0A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0A.CastTime;
                Spell AB1A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1A.CastTime;
                Spell MBAM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2A.CastTime;
                Spell MBAM3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB3) : castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AM3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles3) : castingState.GetSpell(SpellId.ArcaneMissiles3);

                Spell AB0C = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1C = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell MBAM2C = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2C = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell MBAM3C = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AM3C = castingState.GetSpell(SpellId.ArcaneMissiles3);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float MC = 1 - MB;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB);
                float K2 = (1 - MB) * (1 - (1 - MB) * (1 - MB));

                float S0 = 1 - K0;
                float S1 = K0;

                cycle.AddSpell(needsDisplayCalculations, AB0A, S0);
                cycle.AddSpell(needsDisplayCalculations, AB1A, S0);
                cycle.AddSpell(needsDisplayCalculations, AB2A, S0 * MC);
                cycle.AddSpell(needsDisplayCalculations, MBAM2A, S0 * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM3A, S0 * K2);
                cycle.AddSpell(needsDisplayCalculations, AM3A, S0 * K0);

                cycle.AddSpell(needsDisplayCalculations, AB0C, S1);
                cycle.AddSpell(needsDisplayCalculations, AB1C, S1);
                cycle.AddSpell(needsDisplayCalculations, AB2C, S1 * MC);
                cycle.AddSpell(needsDisplayCalculations, MBAM2C, S1 * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM3C, S1 * K2);
                cycle.AddSpell(needsDisplayCalculations, AM3C, S1 * K0);
            }
            else
            {
                // S0: no proc
                // AB0-AB1-AB2-AM3    => S0    (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-MBAM3   => S0   (1 - MB) * (1 - (1 - MB) * (1 - MB)) * (1 - T8)
                // AB0-AB1-AB2-MBAM3   => S1   (1 - MB) * (1 - (1 - MB) * (1 - MB)) * T8
                // AB0-AB1-MBAM2   => S0       MB * (1 - T8)
                // AB0-AB1-MBAM2   => S1       MB * T8

                // S1: proc
                // MBAM0   => S0   (1 - T8)
                // MBAM0   => S1   T8

                // K0 = (1 - MB) * (1 - MB) * (1 - MB)
                // S1 = S0 * (1 - K0) * T8 + S1 * T8
                // S0 + S1 = 1

                // S1 = S0 * (1 - K0) * T8 / (1 - T8)
                // S0 = (1 - T8) / (1 - K0 * T8)
                // S1 = (1 - K0) * T8 / (1 - K0 * T8)


                Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
                Spell MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);
                Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float T8 = 0;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB);
                float S0 = (1 - T8) / (1 - K0 * T8);
                float S1 = (1 - K0) * T8 / (1 - K0 * T8);
                float K1 = S0 * K0;
                float K2 = S0 * (1 - MB) * (1 - (1 - MB) * (1 - MB));
                float K4 = S0 * MB;

                if (needsDisplayCalculations)
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                    cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                    cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                    cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2);
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, S0, S0, K1 + K2, 0);
                }
                cycle.AddSpell(needsDisplayCalculations, AM3, K1);
                cycle.AddSpell(needsDisplayCalculations, MBAM3, K2);
                cycle.AddSpell(needsDisplayCalculations, MBAM2, K4);
                cycle.AddSpell(needsDisplayCalculations, MBAM0, S1);
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB2AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB2AM";

            if (castingState.Solver.Mage2T10)
            {
                // doesn't support haste transferring over several loops

                // S0:
                // AB0-AB1-AM2    => S1   (1 - MB) * (1 - MB)
                // AB0-AB1-MBAM2   => S0   (1 - (1 - MB) * (1 - MB))
                // S1:
                // AB0-AB1-AM2    => S1   (1 - MB) * (1 - MB)
                // AB0-AB1-MBAM2   => S0   (1 - (1 - MB) * (1 - MB))

                // K0 := (1-MB)*(1-MB)

                // MC := 1-MB

                // S0 = 1 - K0
                // S1 = K0

                Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
                Spell MBAM2H = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2);

                float m2T10time = 5.0f - castingState.CalculationOptions.LatencyChannel;
                Spell AB0A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0A.CastTime;
                Spell AB1A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1A.CastTime;
                Spell MBAM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles2) : castingState.GetSpell(SpellId.ArcaneMissiles2);

                Spell AB0C = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1C = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell MBAM2C = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AM2C = castingState.GetSpell(SpellId.ArcaneMissiles2);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float MC = 1 - MB;
                float K0 = (1 - MB) * (1 - MB);

                float S0 = 1 - K0;
                float S1 = K0;

                cycle.AddSpell(needsDisplayCalculations, AB0A, S0);
                cycle.AddSpell(needsDisplayCalculations, AB1A, S0);
                cycle.AddSpell(needsDisplayCalculations, MBAM2A, S0 * (1 - K0));
                cycle.AddSpell(needsDisplayCalculations, AM2A, S0 * K0);

                cycle.AddSpell(needsDisplayCalculations, AB0C, S1);
                cycle.AddSpell(needsDisplayCalculations, AB1C, S1);
                cycle.AddSpell(needsDisplayCalculations, MBAM2C, S1 * (1 - K0));
                cycle.AddSpell(needsDisplayCalculations, AM2C, S1 * K0);
            }
            else
            {
                // S0: no proc
                // AB0-AB1-AM2    => S0   (1 - MB) * (1 - MB)
                // AB0-AB1-MBAM   => S0   (1 - (1 - MB) * (1 - MB)) * (1 - T8)
                // AB0-AB1-MBAM   => S1   (1 - (1 - MB) * (1 - MB)) * T8

                // S1: proc
                // AB0-AB1-MBAM   => S0   (1 - T8)
                // AB0-AB1-MBAM   => S1   T8

                // S1 = S0 * (1 - (1 - MB) * (1 - MB)) * T8 + S1 * T8
                // S0 + S1 = 1

                Spell AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
                Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float T8 = 0;
                float K1 = (1 - MB) * (1 - MB);
                float S0 = (1 - T8) / (1 - K1 * T8);

                if (needsDisplayCalculations)
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    cycle.AddSpell(needsDisplayCalculations, AB0, 1);
                    cycle.AddSpell(needsDisplayCalculations, AB1, 1);
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, 1, 1, 0, 0);
                }
                cycle.AddSpell(needsDisplayCalculations, AM2, S0 * K1);
                cycle.AddSpell(needsDisplayCalculations, MBAM2, 1 - S0 * K1);
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB3AMABar
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB3AMABar";

            // S0: no proc
            // ABar-AB0-AB1-AB2-AM3    => S0   (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            // ABar-AB0-AB1-AB2-MBAM   => S0   (1 - (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)) * (1 - T8)
            // ABar-AB0-AB1-AB2-MBAM   => S1   (1 - (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)) * T8

            // S1: proc
            // ABar-AB0-AB1-AB2-MBAM   => S0   (1 - T8)
            // ABar-AB0-AB1-AB2-MBAM   => S1   T8

            // S1 = S0 * (1 - K1) * T8 + S1 * T8
            // S0 + S1 = 1

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            float S0 = (1 - T8) / (1 - K1 * T8);

            cycle.AddSpell(needsDisplayCalculations, AB0, 1);
            cycle.AddSpell(needsDisplayCalculations, AB1, 1);
            cycle.AddSpell(needsDisplayCalculations, AB2, 1);
            cycle.AddSpell(needsDisplayCalculations, AM3, S0 * K1);
            cycle.AddSpell(needsDisplayCalculations, MBAM3, 1 - S0 * K1);
            cycle.AddSpell(needsDisplayCalculations, ABar, 1);

            cycle.Calculate();
            return cycle;
        }
    }

    class AB3AM2MBAM : Cycle
    {
        public AB3AM2MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "AB3AM2MBAM";

            // S0: no proc
            // AB0-AB1-AB2-AM3    => S0   (1 - MB) * (1 - MB) * (1 - MB)
            // AB0-AB1-AB2-MBAM3   => S0   (1 - MB) * (1 - (1 - MB) * (1 - MB)) * (1 - T8)
            // AB0-AB1-AB2-MBAM3   => S1   (1 - MB) * (1 - (1 - MB) * (1 - MB)) * T8
            // AB0-AB1-MBAM2   => S0   MB * (1 - T8)
            // AB0-AB1-MBAM2   => S1   MB * T8

            // S1: proc
            // AB0-AB1-MBAM2   => S0   (1 - T8)
            // AB0-AB1-MBAM2   => S1   T8

            // S1 = S0 * (1 - (1 - MB) * (1 - MB) * (1 - MB)) * T8 + S1 * T8
            // S0 + S1 = 1

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K1 = (1 - MB) * (1 - MB) * (1 - MB);
            float K2 = (1 - MB) * (1 - (1 - MB) * (1 - MB));
            float S0 = (1 - T8) / (1 - K1 * T8);

            AddSpell(needsDisplayCalculations, AB0, 1);
            AddSpell(needsDisplayCalculations, AB1, 1);
            AddSpell(needsDisplayCalculations, AB2, S0 * (1 - MB));
            AddSpell(needsDisplayCalculations, AM3, S0 * K1);
            AddSpell(needsDisplayCalculations, MBAM3, S0 * K2);
            AddSpell(needsDisplayCalculations, MBAM2, 1 - S0 * (1 - MB));

            Calculate();
        }
    }

    class AB3AMABar2C : Cycle
    {
        public AB3AMABar2C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "AB3AMABar2C";

            // S0: no proc
            // ABar-AB0-AB1-AB2-AM3     => S0   (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            // ABar-AB0-AB1-AB2-MBAM3   => S0   (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB)) * (1 - T8)
            // ABar-AB0-AB1-AB2-MBAM3   => S1   (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB)) * T8
            // ABar-AB0-AB1-MBAM2       => S0   (1 - (1 - MB) * (1 - MB)) * (1 - T8)
            // ABar-AB0-AB1-MBAM2       => S1   (1 - (1 - MB) * (1 - MB)) * T8

            // S1: proc
            // ABar-AB0-AB1-MBAM2   => S0   (1 - T8)
            // ABar-AB0-AB1-MBAM2   => S1   T8

            // S1 = S0 * (1 - (1 - MB) * (1 - MB) * (1 - MB)) * T8 + S1 * T8
            // S0 + S1 = 1

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            float K2 = (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            float K3 = (1 - MB) * (1 - MB);
            float S0 = (1 - T8) / (1 - K1 * T8);

            AddSpell(needsDisplayCalculations, AB0, 1);
            AddSpell(needsDisplayCalculations, AB1, 1);
            AddSpell(needsDisplayCalculations, AB2, S0 * K3);
            AddSpell(needsDisplayCalculations, AM3, S0 * K1);
            AddSpell(needsDisplayCalculations, MBAM3, S0 * K2);
            AddSpell(needsDisplayCalculations, MBAM2, 1 - S0 * K3);
            AddSpell(needsDisplayCalculations, ABar, 1);

            Calculate();
        }
    }

    class ABABar0C : Cycle
    {
        public ABABar0C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "ABABar0C";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to AB-ABar
                AddSpell(needsDisplayCalculations, AB, 1);
                if (AB.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB.CastTime - ABar.CastTime, 1);
                AddSpell(needsDisplayCalculations, ABar1, 1);

                Calculate();
            }
            else
            {
                // S0: no proc
                // AB-ABar1    => S0   (1 - MB) * (1 - MB)
                // AB-ABar1    => S1   1 - (1 - MB) * (1 - MB)

                // S1: proc
                // MBAM-ABar  => S0   (1 - T8) * (1 - MB)
                // MBAM-ABar  => S1   1 - (1 - T8) * (1 - MB)

                // S0 = S0 * (1 - MB) * (1 - MB) + S1 * (1 - T8) * (1 - MB)
                // S0 + S1 = 1

                float K1 = (1 - MB) * (1 - MB);
                float K2 = (1 - T8) * (1 - MB);
                float S0 = K2 / (K2 - K1 + 1);

                //AB-ABar 0.8 * 0.8
                AddSpell(needsDisplayCalculations, ABar1, S0);
                AddSpell(needsDisplayCalculations, AB, S0);
                if (AB.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB.CastTime - ABar.CastTime, S0);

                //ABar-MBAM
                AddSpell(needsDisplayCalculations, ABar, 1 - S0);
                AddSpell(needsDisplayCalculations, MBAM, 1 - S0);
                if (MBAM.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime, 1 - S0);

                Calculate();
            }
        }
    }

    class ABABar1C : Cycle
    {
        public ABABar1C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "ABABar1C";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;

            // S0
            // AB-ABar1 => S0                  (1-MB)*(1-MB)
            //          => S1                  1 - (1-MB)*(1-MB)

            // S1
            // AB-MBAM1-ABar => S0             (1-T8)*(1-MB)
            //               => S1             1 - (1-T8)*(1-MB)

            // S0 = S0 * (1 - MB) * (1 - MB) + S1 * (1 - T8) * (1 - MB)
            // S0 + S1 = 1

            float K1 = (1 - MB) * (1 - MB);
            float K2 = (1 - T8) * (1 - MB);
            float S0 = K2 / (K2 - K1 + 1);
            float S1 = 1 - S0;

            //AB-ABar1
            AddSpell(needsDisplayCalculations, AB, S0);
            if (AB.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB.CastTime - ABar.CastTime, S0);
            AddSpell(needsDisplayCalculations, ABar1, S0);

            //AB-MBAM1-ABar
            AddSpell(needsDisplayCalculations, AB, S1);
            AddSpell(needsDisplayCalculations, MBAM1, S1);
            if (AB.CastTime + MBAM1.CastTime + ABar.CastTime < 3.0)
            {
                AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM1.CastTime - AB.CastTime - ABar.CastTime, S1);
                AddSpell(needsDisplayCalculations, ABar, S1);
            }
            else
            {
                AddSpell(needsDisplayCalculations, ABar, S1);
            }

            Calculate();
        }
    }

    class ABABar0MBAM : Cycle
    {
        public ABABar0MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "ABABar0MBAM";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;

            // S0
            // AB-ABar1 => S0                  (1-MB)*(1-MB)
            //          => S1                  1 - (1-MB)*(1-MB)

            // S1
            // MBAM => S0             (1-T8)
            //      => S1             T8

            // S0 = S0 * (1 - MB) * (1 - MB) + S1 * (1 - T8) * (1 - MB)
            // S0 + S1 = 1

            float K1 = (1 - MB) * (1 - MB);
            float K2 = (1 - T8);
            float S0 = K2 / (K2 - K1 + 1);
            float S1 = 1 - S0;

            AddSpell(needsDisplayCalculations, AB, S0);
            if (AB.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB.CastTime - ABar1.CastTime, S0);
            AddSpell(needsDisplayCalculations, ABar1, S0);

            AddSpell(needsDisplayCalculations, MBAM, S1);

            Calculate();
        }
    }

    class ABSpam3C : Cycle
    {
        public ABSpam3C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2, K3, K4, K5, S0, S1;
            Name = "ABSpam3C";

            // always ramp up to 3 AB before using MBAM-ABar

            // S0: (we always enter S0 with ABar, take into account)
            // ABar-AB0-AB1-AB2-MBAM => S0       (1 - (1-MB)*(1-MB)*(1-MB))*(1-T8)      one of the first two AB or ABar procs
            // ABar-AB0-AB1-AB2-MBAM => S2       (1 - (1-MB)*(1-MB)*(1-MB))*T8      one of the first two AB or ABar procs
            // ABar-AB0-AB1-AB2-AB3-MBAM => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-T8)       third AB procs
            // ABar-AB0-AB1-AB2-AB3-MBAM => S2   (1-MB)*(1-MB)*(1-MB)*MB*T8       third AB procs
            // ABar-AB0-AB1-AB2 => S1            (1-MB)*(1-MB)*(1-MB)*(1-MB)   no procs
            // S1:
            // AB3-AB3-MBAM => S0           MB*(1-T8)              proc
            // AB3-AB3-MBAM => S2           MB*T8                  proc
            // AB3 => S1                    (1-MB)                 no proc
            // S2:
            // ABar-AB0-AB1-AB2-MBAM => S0       (1-T8)      
            // ABar-AB0-AB1-AB2-MBAM => S2       T8      


            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)
            // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
            // S0 + S1 + S2 = 1

            // S0 = (MB*T8-MB)/(K0*T8-MB-K0)
            // S1 = (K0*T8-K0)/(K0*T8-MB-K0)
            // S2 = -(MB*T8)/(K0*T8-MB-K0)

            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            //Spell MBAM3C = castingState.GetSpell(SpellId.ArcaneMissilesMB3Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            //Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            //Spell ABar3C = castingState.GetSpell(SpellId.ArcaneBarrage3Combo);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
            S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
            float S2 = -(MB * T8) / (K0 * T8 - MB - K0);
            K1 = S0 * (1 - (1 - MB) * (1 - MB) * (1 - MB)) + S2;
            K2 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);
            AddSpell(needsDisplayCalculations, ABar, K1 + K2 + K4);

            Calculate();
        }
    }

    class ABSpam03C : Cycle
    {
        public ABSpam03C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2, K3, K4, K5, K6, S0, S1;
            Name = "ABSpam03C";

            // S0: (we always enter S0 with ABar, take into account)

            // ABar-MBAM => S0                   MB*(1-T8)                     ABar procs
            // ABar-MBAM => S2                   MB*T8                         ABar procs
            // ABar-AB0-AB1-AB2-MBAM => S0       (1-MB)*(1 - (1-MB)*(1-MB))*(1-T8)    one of the first two AB procs
            // ABar-AB0-AB1-AB2-MBAM => S2       (1-MB)*(1 - (1-MB)*(1-MB))*T8    one of the first two AB procs
            // ABar-AB0-AB1-AB2-AB3-MBAM => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-T8)       third AB procs
            // ABar-AB0-AB1-AB2-AB3-MBAM => S2   (1-MB)*(1-MB)*(1-MB)*MB*T8       third AB procs
            // ABar-AB0-AB1-AB2 => S1            (1-MB)*(1-MB)*(1-MB)*(1-MB)   no procs
            // S1:
            // AB3-AB3-MBAM => S0           MB*(1-T8)                     proc
            // AB3-AB3-MBAM => S2           MB*T8                     proc
            // AB3 => S1                    (1-MB)                 no proc
            // S2:
            // ABar-MBAM => S0                   (1-T8)                        
            // ABar-MBAM => S2                   T8                            

            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)
            // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
            // S0 + S1 + S2 = 1


            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
            S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
            float S2 = -(MB * T8) / (K0 * T8 - MB - K0);

            K6 = S0 * MB + S2;
            K1 = S0 * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K2 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM, K6);
            if (MBAM.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime, K6);
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);
            AddSpell(needsDisplayCalculations, ABar, K1 + K2 + K4 + K6);

            Calculate();
        }
    }

    public class ABSpam03MBAM : Cycle
    {
        public ABSpam03MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2, K3, K4, K5, S0, S1, S2;
            Name = "ABSpam03MBAM";

            // S0:

            // AB0-AB1-AB2-MBAM => S0       (1 - (1-MB)*(1-MB))*(1-T8)    one of the first two AB procs
            // AB0-AB1-AB2-MBAM => S2       (1 - (1-MB)*(1-MB))*T8    one of the first two AB procs
            // AB0-AB1-AB2-AB3-MBAM => S0   (1-MB)*(1-MB)*MB*(1-T8)       third AB procs
            // AB0-AB1-AB2-AB3-MBAM => S2   (1-MB)*(1-MB)*MB*T8       third AB procs
            // AB0-AB1-AB2 => S1            (1-MB)*(1-MB)*(1-MB)   no procs
            // S1:
            // AB3-AB3-MBAM => S0           MB*(1-T8)                     proc
            // AB3-AB3-MBAM => S2           MB*T8                     proc
            // AB3 => S1                    (1-MB)                 no proc
            // S2:
            // MBAM => S0                   (1-T8)                        
            // MBAM => S2                   T8                            

            // K0 = (1-MB)*(1-MB)*(1-MB)
            // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
            // S0 + S1 + S2 = 1


            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB);
            S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
            S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
            S2 = -(MB * T8) / (K0 * T8 - MB - K0);

            K1 = S0 * (1 - (1 - MB) * (1 - MB));
            K2 = S0 * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * (1 - MB) * (1 - MB) * (1 - MB);
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM, S2);
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);

            Calculate();
        }
    }

    public class ABSpam3MBAM : Cycle
    {
        public ABSpam3MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2, K3, K4, K5, S0, S1;
            Name = "ABSpam3MBAM";

            // always ramp up to 3 AB before using MBAM

            // S0:
            // AB0-AB1-AB2-MBAM => S0       (1 - (1-MB)*(1-MB))*(1-T8)      one of the first two AB procs
            // AB0-AB1-AB2-MBAM => S2       (1 - (1-MB)*(1-MB))*T8      one of the first two AB procs
            // AB0-AB1-AB2-AB3-MBAM => S0   (1-MB)*(1-MB)*MB*(1-T8)       third AB procs
            // AB0-AB1-AB2-AB3-MBAM => S2   (1-MB)*(1-MB)*MB*T8       third AB procs
            // AB0-AB1-AB2 => S1            (1-MB)*(1-MB)*(1-MB)   no procs
            // S1:
            // AB3-AB3-MBAM => S0           MB*(1-T8)                     proc
            // AB3-AB3-MBAM => S2           MB*T8                     proc
            // AB3 => S1                    (1-MB)                 no proc
            // S2:
            // AB0-AB1-AB2-MBAM => S0       (1-T8)     
            // AB0-AB1-AB2-MBAM => S2       T8     

            // K0 = (1-MB)*(1-MB)*(1-MB)

            // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
            // S0 + S1 + S2 = 1

            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB);
            S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
            S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
            float S2 = -(MB * T8) / (K0 * T8 - MB - K0);
            K1 = S0 * (1 - (1 - MB) * (1 - MB)) + S2;
            K2 = S0 * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * (1 - MB) * (1 - MB) * (1 - MB);
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);

            Calculate();
        }
    }

    public static class ABSpam4MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, S0, S1;
            cycle.Name = "ABSpam4MBAM";

            // always ramp up to 4 AB before using MBAM

            // S0:
            // AB0-AB1-AB2-AB3-MBAM => S0       (1 - (1-MB)*(1-MB)*(1-MB))*(1-T8)      one of the first three AB procs
            // AB0-AB1-AB2-AB3-MBAM => S2       (1 - (1-MB)*(1-MB)*(1-MB))*T8      one of the first three AB procs
            // AB0-AB1-AB2-AB3-AB4-MBAM => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-T8)       fourth AB procs
            // AB0-AB1-AB2-AB3-AB4-MBAM => S2   (1-MB)*(1-MB)*(1-MB)*MB*T8       fourth AB procs
            // AB0-AB1-AB2-AB3 => S1            (1-MB)*(1-MB)*(1-MB)*(1-MB)   no procs
            // S1:
            // AB4-AB4-MBAM => S0           MB*(1-T8)                     proc
            // AB4-AB4-MBAM => S2           MB*T8                     proc
            // AB4 => S1                    (1-MB)                 no proc
            // S2:
            // AB0-AB1-AB2-AB3-MBAM => S0       (1-T8)     
            // AB0-AB1-AB2-AB3-MBAM => S2       T8     

            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

            // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
            // S0 + S1 + S2 = 1

            // for 2T10 we assume that the buff always drops by the time we get to 4 stacks

            Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);

            float MB = 0.08f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
            S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
            float S2 = -(MB * T8) / (K0 * T8 - MB - K0);
            K1 = S0 * (1 - (1 - MB) * (1 - MB) * (1 - MB)) + S2;
            K2 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * K0;
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            if (needsDisplayCalculations)
            {
                if (castingState.Solver.Mage2T10)
                {
                    float m2T10time = 5.0f - MBAM4.CastTime;
                    Spell AB0 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                    m2T10time -= AB0.CastTime;
                    Spell AB1 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                    m2T10time -= AB1.CastTime;
                    Spell AB2 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                    m2T10time -= AB2.CastTime;
                    Spell AB3 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                    Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                    cycle.AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);
                }
                else
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                    Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                    Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                    cycle.AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);
                }
            }
            else
            {
                if (castingState.Solver.Mage2T10)
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell ABT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlastRaw);
                    float m2T10time = 5.0f - MBAM4.CastTime;
                    float ab0 = 0.0f, ab1 = 0.0f, ab2 = 0.0f, ab3 = 0.0f, abt0 = 0.0f, abt1 = 0.0f, abt2 = 0.0f, abt3 = 0.0f;
                    if (m2T10time > 0.0f)
                    {
                        abt0 = K1 + K2 + K3;
                        m2T10time -= ABT.CastTime;
                    }
                    else
                    {
                        ab0 = K1 + K2 + K3;
                        m2T10time -= AB.CastTime;
                    }
                    if (m2T10time > 0.0f)
                    {
                        abt1 = K1 + K2 + K3;
                        m2T10time -= ABT.CastTime;
                    }
                    else
                    {
                        ab1 = K1 + K2 + K3;
                        m2T10time -= AB.CastTime;
                    }
                    if (m2T10time > 0.0f)
                    {
                        abt2 = K1 + K2 + K3;
                        m2T10time -= ABT.CastTime;
                    }
                    else
                    {
                        ab2 = K1 + K2 + K3;
                        m2T10time -= AB.CastTime;
                    }
                    if (m2T10time > 0.0f)
                    {
                        abt3 = K1 + K2 + K3;
                        m2T10time -= ABT.CastTime;
                    }
                    else
                    {
                        ab3 = K1 + K2 + K3;
                        m2T10time -= AB.CastTime;
                    }
                    castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, ab0, ab1, ab2, ab3, K2 + 2 * K4 + K5);
                    castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, ABT, abt0, abt1, abt2, abt3);
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
                }
            }
            cycle.AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam04MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, S0, S1;
            cycle.Name = "ABSpam04MBAM";

            if (castingState.Solver.Mage2T10)
            {
                // 5 - channelLatency - n * ABT > 0
                // (5 - channelLatency) / ABT > n

                // S0:
                // AB0-AB1-AB2-AB3-MBAM => S0          (1 - (1-MB)*(1-MB)*(1-MB))       one of the first three AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM => S0      (1-MB)*(1-MB)*(1-MB)*MB          fourth AB procs
                // AB0-AB1-AB2-AB3-AB4-AB4-MBAM => S0  (1-MB)*(1-MB)*(1-MB)*(1-MB)*MB   fifth AB procs
                // ...
                // AB0-...-AB4 => S1                   (1-MB)*...*(1-MB)                no procs in first n,4
                // S1:
                // AB4-AB4-MBAM => S0   MB      proc
                // AB4 => S1            (1-MB)  no proc

                // K0 = (1-MB)^n

                // S0 = (1 - K0) * S0 + MB * S1
                // S1 = K0 * S0 + (1-MB) * S1
                // S0 + S1 = 1

                // S0 = MB / K0 * S1
                // S1 = K0 / (K0 + MB)
                // S0 = MB / (K0 + MB)

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;

                if (needsDisplayCalculations)
                {
                    float m2T10time = 5.0f - castingState.CalculationOptions.LatencyChannel;
                    Spell AB0 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                    m2T10time -= AB0.CastTime;
                    Spell AB1 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                    m2T10time -= AB1.CastTime;
                    Spell AB2 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                    m2T10time -= AB2.CastTime;
                    Spell AB3 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                    m2T10time -= AB2.CastTime;
                    Spell AB4 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast4) : castingState.GetSpell(SpellId.ArcaneBlast4);
                    Spell ABn = AB4;
                    Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                    Spell MBAM4T = MBAM4;
                    if (m2T10time > 0.0f)
                    {
                        ABn = castingState.GetSpell(SpellId.ArcaneBlast4);
                        MBAM4T = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB4);
                    }
                    float m = (float)Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / AB0.CastTime);
                    float MBI = 1 - MB;
                    float MBI2 = MBI * MBI;
                    float n;
                    float K0;
                    if (m > 4)
                    {
                        n = m;
                        K0 = (float)Math.Pow(MBI, n);
                    }
                    else
                    {
                        n = 4;
                        K0 = MBI2 * MBI2;
                    }
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    //n=8:

                    //AB0-AB1-AB2-AB3-MBAM4T                         1-(1-MB)^3
                    //AB0-AB1-AB2-AB3-AB4-MBAM4T                     (1-MB)^3*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-MBAM4T                 (1-MB)^4*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-MBAM4T             (1-MB)^5*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4-MBAM4          (1-MB)^6*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4-AB4n-MBAM4     (1-MB)^7*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4                (1-MB)^8

                    //AB4n: S0 * (1-MB)^(n-1)*MB
                    //MBAM4T: S0 * (1-(1-MB)^3 + (1-MB)^3*MB + ... + (1-MB)^(n-3)*MB)
                    //        = S0 * (2 - (1-MB)^3 - (1-MB)^(n-5))
                    //MBAM4: S0 * ((1-MB)^(n-2)*MB + (1-MB)^(n-1)*MB)
                    //        = S0 * (1-MB)^(n-2)*MB * (2-MB)
                    //AB4: S0 * (k^3 - k^(n-1))/MB

                    if (n <= 4)
                    {
                        cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB2, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB3, S0);
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * MBI2 * MBI * MB + S1 * (1 + MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (1 - K0) + S1 * MB);
                    }
                    else
                    {
                        cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB2, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB3, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB4, S0 * (float)(Math.Pow(1- MB, 3) - Math.Pow(1 - MB, n - 1)) / MB);
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * (float)Math.Pow(1 - MB, n - 1) * MB + S1 * (1 + MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (float)Math.Pow(1 - MB, n - 2) * MB * (2 - MB) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, MBAM4T, S0 * (float)(2 - Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 5)));
                    }
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell ABT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

                    float m = (float)Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / ABT.CastTime);
                    float MBI = 1 - MB;
                    float MBI2 = MBI * MBI;
                    float MBI3 = MBI2 * MBI;
                    float n;
                    float K0;
                    if (m > 4)
                    {
                        n = m;
                        if (n == 5)
                        {
                            K0 = MBI3 * MBI2;
                        }
                        else
                        {
                            K0 = (float)Math.Pow(MBI, n);
                        }
                    }
                    else
                    {
                        n = 4;
                        K0 = MBI2 * MBI2;
                    }
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    Solver solver = castingState.Solver;

                    float ab4, abt4;
                    if (m > 4)
                    {
                        float k2 = K0 / MBI2;
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM, 0, 0, 0, 0, S0 * k2 * MB * (2 - MB) + S1 * MB);
                        Spell MBAMT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB);
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAMT, 0, 0, 0, 0, S0 * (2 - MBI3 - k2 / MBI3));
                        float k = k2 * MBI;
                        ab4 = S0 * k * MB + S1 * (1 + MB);
                        abt4 = S0 * (MBI3 - k) / MB;
                    }
                    else
                    {
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM, 0, 0, 0, 0, S0 * (1 - K0) + S1 * MB);
                        ab4 = S0 * MBI3 * MB + S1 * (1 + MB);
                        abt4 = 0;
                    }
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, (m < 1) ? S0 : 0, (m < 2) ? S0 : 0, (m < 3) ? S0 : 0, (m < 4) ? S0 : 0, ab4);
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, ABT, (m >= 1) ? S0 : 0, (m >= 2) ? S0 : 0, (m >= 3) ? S0 : 0, (m >= 4) ? S0 : 0, abt4);
                }
            }
            else
            {
                // always ramp up to 4 AB before using MBAM

                // S0:
                // AB0-AB1-AB2-AB3-MBAM => S0       (1 - (1-MB)*(1-MB)*(1-MB))*(1-T8)      one of the first three AB procs
                // AB0-AB1-AB2-AB3-MBAM => S2       (1 - (1-MB)*(1-MB)*(1-MB))*T8      one of the first three AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-T8)       fourth AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM => S2   (1-MB)*(1-MB)*(1-MB)*MB*T8       fourth AB procs
                // AB0-AB1-AB2-AB3 => S1            (1-MB)*(1-MB)*(1-MB)*(1-MB)   no procs
                // S1:
                // AB4-AB4-MBAM => S0           MB*(1-T8)                     proc
                // AB4-AB4-MBAM => S2           MB*T8                     proc
                // AB4 => S1                    (1-MB)                 no proc
                // S2:
                // MBAM => S0       (1-T8)     
                // MBAM => S2       T8     

                // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

                // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
                // S1 = K0 * S0 + (1-MB) * S1
                // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
                // S0 + S1 + S2 = 1

                Spell MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float T8 = 0;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
                S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
                float S2 = -(MB * T8) / (K0 * T8 - MB - K0);
                K1 = S0 * (1 - (1 - MB) * (1 - MB) * (1 - MB));
                K2 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB;
                K3 = S0 * K0;
                K4 = S1 * MB;
                K5 = S1 * (1 - MB);

                if (needsDisplayCalculations)
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                    Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                    Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                    cycle.AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);

                    Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                    cycle.AddSpell(needsDisplayCalculations, MBAM0, S2);
                    cycle.AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Solver solver = castingState.Solver;
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
                    solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM0, S2, 0, 0, 0, K1 + K2 + K4);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam24MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, K6, S0, S1;
            cycle.Name = "ABSpam24MBAM";

            // always ramp up to 4 AB before using MBAM

            // S0:
            // AB0-AB1-MBAM2 => S0               MB*(1-T8)                         first AB procs
            // AB0-AB1-MBAM2 => S2               MB*T8                             first AB procs
            // AB0-AB1-AB2-AB3-MBAM4 => S0       (1 - (1-MB)*(1-MB))*(1-MB)*(1-T8) 2nd or 3rd AB procs
            // AB0-AB1-AB2-AB3-MBAM4 => S2       (1 - (1-MB)*(1-MB))*(1-MB)*T8     2nd or 3rd AB procs
            // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-T8)    fourth AB procs
            // AB0-AB1-AB2-AB3-AB4-MBAM4 => S2   (1-MB)*(1-MB)*(1-MB)*MB*T8        fourth AB procs
            // AB0-AB1-AB2-AB3 => S1            (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
            // S1:
            // AB4-AB4-MBAM4 => S0           MB*(1-T8)                     proc
            // AB4-AB4-MBAM4 => S2           MB*T8                     proc
            // AB4 => S1                    (1-MB)                 no proc
            // S2:
            // AB0-AB1-MBAM2 => S0       (1-T8)     
            // AB0-AB1-MBAM2 => S2       T8     

            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

            // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
            // S0 + S1 + S2 = 1

            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);

            float MB = 0.08f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
            S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
            float S2 = -(MB * T8) / (K0 * T8 - MB - K0);
            K6 = S0 * MB + S2;
            K1 = S0 * (1 - (1 - MB) * (1 - MB)) * (1 - MB);
            K2 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB;
            K3 = S0 * K0;
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                cycle.AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3 + K6);
                cycle.AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3 + K6);
                cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                cycle.AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, K1 + K2 + K3 + K6, K1 + K2 + K3 + K6, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            cycle.AddSpell(needsDisplayCalculations, MBAM2, K6);
            cycle.AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam024MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, K6, S0, S1;
            cycle.Name = "ABSpam024MBAM";

            if (castingState.Solver.Mage2T10)
            {
                // 5 - channelLatency - n * ABT > 0
                // (5 - channelLatency) / ABT > n

                // S0:
                // AB0-AB1-MBAM => S0                  MB                               first AB procs
                // AB0-AB1-AB2-AB3-MBAM => S0          (1-MB)(1 - (1-MB)*(1-MB))        second or third AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM => S0      (1-MB)*(1-MB)*(1-MB)*MB          fourth AB procs
                // AB0-AB1-AB2-AB3-AB4-AB4-MBAM => S0  (1-MB)*(1-MB)*(1-MB)*(1-MB)*MB   fifth AB procs
                // ...
                // AB0-...-AB4 => S1                   (1-MB)*...*(1-MB)                no procs in first n,4
                // S1:
                // AB4-AB4-MBAM => S0   MB      proc
                // AB4 => S1            (1-MB)  no proc

                // K0 = (1-MB)^n

                // S0 = (1 - K0) * S0 + MB * S1
                // S1 = K0 * S0 + (1-MB) * S1
                // S0 + S1 = 1

                // S0 = MB / K0 * S1
                // S1 = K0 / (K0 + MB)
                // S0 = MB / (K0 + MB)

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;

                if (needsDisplayCalculations)
                {
                    float m2T10time = 5.0f - castingState.CalculationOptions.LatencyChannel;
                    Spell AB0 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                    m2T10time -= AB0.CastTime;
                    Spell AB1 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                    m2T10time -= AB1.CastTime;
                    Spell AB2 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                    Spell MBAM2 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                    m2T10time -= AB2.CastTime;
                    Spell AB3 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                    m2T10time -= AB2.CastTime;
                    Spell AB4 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast4) : castingState.GetSpell(SpellId.ArcaneBlast4);
                    Spell ABn = AB4;
                    Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                    Spell MBAM4T = MBAM4;
                    if (m2T10time > 0.0f)
                    {
                        ABn = castingState.GetSpell(SpellId.ArcaneBlast4);
                        MBAM4T = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB4);
                    }
                    float m = (float)Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / AB0.CastTime);
                    float n = Math.Max(4, m);
                    float K0 = (float)Math.Pow(1 - MB, n);
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    //n=8:

                    //AB0-AB1-MBAM2T                                 MB
                    //AB0-AB1-AB2-AB3-MBAM4T                         (1-MB)*(1-(1-MB)^2)
                    //AB0-AB1-AB2-AB3-AB4-MBAM4T                     (1-MB)^3*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-MBAM4T                 (1-MB)^4*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-MBAM4T             (1-MB)^5*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4-MBAM4          (1-MB)^6*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4-AB4n-MBAM4     (1-MB)^7*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4                (1-MB)^8

                    //AB4n: S0 * (1-MB)^(n-1)*MB
                    //MBAM4T: S0 * (1-(1-MB)^3 + (1-MB)^3*MB + ... + (1-MB)^(n-3)*MB)
                    //        = S0 * (2 - (1-MB)^3 - (1-MB)^(n-5))
                    //MBAM4: S0 * ((1-MB)^(n-2)*MB + (1-MB)^(n-1)*MB)
                    //        = S0 * (1-MB)^(n-2)*MB * (2-MB)
                    //AB4: S0 * (k^3 - k^(n-1))/MB

                    if (n <= 4)
                    {
                        cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB2, S0 * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, AB3, S0 * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB + S1 * (1 + MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (1 - K0 - MB) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, MBAM2, S0 * MB);
                    }
                    else
                    {
                        cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB2, S0 * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, AB3, S0 * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, AB4, S0 * (float)(Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 1)) / MB);
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * (float)Math.Pow(1 - MB, n - 1) * MB + S1 * (1 + MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (float)Math.Pow(1 - MB, n - 2) * MB * (2 - MB) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, MBAM4T, S0 * (float)(2 - Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 5) - MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM2, S0 * MB);
                    }
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell ABT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
                    Spell MBAMT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB);
                    float m = (float)Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / ABT.CastTime);
                    float n = Math.Max(4, m);
                    float K0 = (float)Math.Pow(1 - MB, n);
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    Solver solver = castingState.Solver;
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, (m < 1) ? S0 : 0, (m < 2) ? S0 : 0, (m < 3) ? S0 * (1 - MB) : 0, (m < 4) ? S0 * (1 - MB) : 0, (m <= 4) ? (S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB + S1 * (1 + MB)) : (float)(S0 * Math.Pow(1 - MB, n - 1) * MB + S1 * (1 + MB)));
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, ABT, (m >= 1) ? S0 : 0, (m >= 2) ? S0 : 0, (m >= 3) ? S0 * (1 - MB) : 0, (m >= 4) ? S0 * (1 - MB) : 0, (m <= 4) ? 0 : (float)(S0 * (Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 1)) / MB));

                    if (n <= 4)
                    {
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM, 0, 0, (m < 2) ? S0 * MB : 0, 0, S0 * (1 - K0 - MB) + S1 * MB);
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAMT, 0, 0, (m >= 2) ? S0 * MB : 0, 0, 0);
                    }
                    else
                    {
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM, 0, 0, 0, 0, S0 * (float)Math.Pow(1 - MB, n - 2) * MB * (2 - MB) + S1 * MB);
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAMT, 0, 0, S0 * MB, 0, S0 * (float)(2 - Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 5) - MB));
                    }
                }
            }
            else
            {
                // S0:
                // AB0-AB1-MBAM2 => S0               MB*(1-T8)                         first AB procs
                // AB0-AB1-MBAM2 => S2               MB*T8                             first AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S0       (1 - (1-MB)*(1-MB))*(1-MB)*(1-T8) 2nd or 3rd AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S2       (1 - (1-MB)*(1-MB))*(1-MB)*T8     2nd or 3rd AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-T8)    fourth AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S2   (1-MB)*(1-MB)*(1-MB)*MB*T8        fourth AB procs
                // AB0-AB1-AB2-AB3 => S1            (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
                // S1:
                // AB4-AB4-MBAM4 => S0           MB*(1-T8)                     proc
                // AB4-AB4-MBAM4 => S2           MB*T8                     proc
                // AB4 => S1                    (1-MB)                 no proc
                // S2:
                // MBAM0 => S0       (1-T8)     
                // MBAM0 => S2       T8     

                // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

                // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
                // S1 = K0 * S0 + (1-MB) * S1
                // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
                // S0 + S1 + S2 = 1

                Spell MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float T8 = 0;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
                S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
                float S2 = -(MB * T8) / (K0 * T8 - MB - K0);
                K6 = S0 * MB;
                K1 = S0 * (1 - (1 - MB) * (1 - MB)) * (1 - MB);
                K2 = S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB;
                K3 = S0 * K0;
                K4 = S1 * MB;
                K5 = S1 * (1 - MB);

                if (needsDisplayCalculations)
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                    Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                    Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                    cycle.AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3 + K6);
                    cycle.AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3 + K6);
                    cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);

                    Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                    Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                    cycle.AddSpell(needsDisplayCalculations, MBAM0, S2);
                    cycle.AddSpell(needsDisplayCalculations, MBAM2, K6);
                    cycle.AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Solver solver = castingState.Solver;
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, K1 + K2 + K3 + K6, K1 + K2 + K3 + K6, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
                    solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM0, S2, 0, K6, 0, K1 + K2 + K4);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam034MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, K7, S0, S1;
            cycle.Name = "ABSpam034MBAM";

            if (castingState.Solver.Mage2T10)
            {
                // 5 - channelLatency - n * ABT > 0
                // (5 - channelLatency) / ABT > n

                // S0:
                // AB0-AB1-AB2-MBAM3 => S0              1 - (1-MB)*(1-MB)                first or second AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S0          (1-MB)*(1-MB)*MB                 third AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0      (1-MB)*(1-MB)*(1-MB)*MB          fourth AB procs
                // AB0-AB1-AB2-AB3-AB4-AB4-MBAM4 => S0  (1-MB)*(1-MB)*(1-MB)*(1-MB)*MB   fifth AB procs
                // ...
                // AB0-...-AB4 => S1                    (1-MB)*...*(1-MB)                 no procs in first n,4
                // S1:
                // AB4-AB4-MBAM4 => S0   MB      proc
                // AB4 => S1            (1-MB)  no proc

                // K0 = (1-MB)^n

                // S0 = (1 - K0) * S0 + MB * S1
                // S1 = K0 * S0 + (1-MB) * S1
                // S0 + S1 = 1

                // S0 = MB / K0 * S1
                // S1 = K0 / (K0 + MB)
                // S0 = MB / (K0 + MB)

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;

                if (needsDisplayCalculations)
                {
                    float m2T10time = 5.0f - castingState.CalculationOptions.LatencyChannel;
                    Spell AB0 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                    m2T10time -= AB0.CastTime;
                    Spell AB1 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                    m2T10time -= AB1.CastTime;
                    Spell AB2 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                    m2T10time -= AB2.CastTime;
                    Spell AB3 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                    Spell MBAM3 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB3) : castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                    m2T10time -= AB2.CastTime;
                    Spell AB4 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast4) : castingState.GetSpell(SpellId.ArcaneBlast4);
                    Spell ABn = AB4;
                    Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                    Spell MBAM4T = MBAM4;
                    if (m2T10time > 0.0f)
                    {
                        ABn = castingState.GetSpell(SpellId.ArcaneBlast4);
                        MBAM4T = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB4);
                    }
                    float m = (float)Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / AB0.CastTime);
                    float MBI = 1 - MB;
                    float MBI2 = MBI * MBI;
                    float n;
                    float K0;
                    if (m > 4)
                    {
                        n = m;
                        K0 = (float)Math.Pow(MBI, n);
                    }
                    else
                    {
                        n = 4;
                        K0 = MBI2 * MBI2;
                    }
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    //n=8:

                    //AB0-AB1-AB2-MBAM3T                             1 - (1-MB)^2
                    //AB0-AB1-AB2-AB3-MBAM4T                         (1-MB)^2*MB
                    //AB0-AB1-AB2-AB3-AB4-MBAM4T                     (1-MB)^3*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-MBAM4T                 (1-MB)^4*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-MBAM4T             (1-MB)^5*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4-MBAM4          (1-MB)^6*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4-AB4n-MBAM4     (1-MB)^7*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4                (1-MB)^8

                    //AB4n: S0 * (1-MB)^(n-1)*MB
                    //MBAM4T: S0 * (1-(1-MB)^3 + (1-MB)^3*MB + ... + (1-MB)^(n-3)*MB)
                    //        = S0 * (2 - (1-MB)^3 - (1-MB)^(n-5))
                    //MBAM4: S0 * ((1-MB)^(n-2)*MB + (1-MB)^(n-1)*MB)
                    //        = S0 * (1-MB)^(n-2)*MB * (2-MB)
                    //AB4: S0 * (k^3 - k^(n-1))/MB

                    if (n <= 4)
                    {
                        cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB2, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB3, S0 * MBI2);
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * MBI2 * MBI * MB + S1 * (1 + MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (MBI2 - K0) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, MBAM3, S0 * (1 - MBI2));
                    }
                    else
                    {
                        cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB2, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB3, S0 * MBI2);
                        cycle.AddSpell(needsDisplayCalculations, AB4, S0 * (float)(Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 1)) / MB);
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * (float)Math.Pow(1 - MB, n - 1) * MB + S1 * (1 + MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (float)Math.Pow(1 - MB, n - 2) * MB * (2 - MB) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, MBAM4T, S0 * (float)(2 - Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 5) - MB - MB * (1 - MB)));
                        cycle.AddSpell(needsDisplayCalculations, MBAM3, S0 * (1 - MBI2));
                    }
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell ABT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
                    Spell MBAMT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB);
                    float m = (float)Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / ABT.CastTime);
                    float MBI = 1 - MB;
                    float MBI2 = MBI * MBI;
                    float MBI3 = MBI2 * MBI;
                    float n;
                    float K0;
                    if (m > 4)
                    {
                        n = m;
                        K0 = (float)Math.Pow(MBI, n);
                    }
                    else
                    {
                        n = 4;
                        K0 = MBI2 * MBI2;
                    }
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    Solver solver = castingState.Solver;

                    float ab4, abt4;
                    if (m > 4)
                    {
                        float k2 = K0 / MBI2;
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM, 0, 0, 0, 0, S0 * k2 * MB * (2 - MB) + S1 * MB);
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAMT, 0, 0, 0, S0 * (1 - MBI2), S0 * (2 - MBI3 - k2 / MBI3 - MB - MB * MBI));
                        float k = k2 * MBI;
                        ab4 = S0 * k * MB + S1 * (1 + MB);
                        abt4 = S0 * (MBI3 - k) / MB;
                    }
                    else
                    {
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM, 0, 0, 0, (m < 4) ? S0 * (1 - MBI2) : 0, S0 * (MBI2 - K0) + S1 * MB);
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAMT, 0, 0, 0, (m >= 4) ? S0 * (1 - MBI2) : 0, 0);
                        ab4 = S0 * MBI3 * MB + S1 * (1 + MB);
                        abt4 = 0;
                    }
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, (m < 1) ? S0 : 0, (m < 2) ? S0 : 0, (m < 3) ? S0 : 0, (m < 4) ? S0 * MBI2 : 0, ab4);
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, ABT, (m >= 1) ? S0 : 0, (m >= 2) ? S0 : 0, (m >= 3) ? S0 : 0, (m >= 4) ? S0 * MBI2 : 0, abt4);
                }
            }
            else
            {

                // S0:
                // AB0-AB1-AB2-MBAM3 => S0           (1-(1-MB)*(1-MB))*(1-T8)                  first or 2nd AB procs
                // AB0-AB1-AB2-MBAM3 => S2           (1-(1-MB)*(1-MB))*T8                      first or 2nd AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S0       (1-MB)*(1-MB)*MB*(1-T8)           3rd AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S2       (1-MB)*(1-MB)*MB*T8               3rd AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-T8)    fourth AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S2   (1-MB)*(1-MB)*(1-MB)*MB*T8        fourth AB procs
                // AB0-AB1-AB2-AB3 => S1             (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
                // S1:
                // AB4-AB4-MBAM4 => S0           MB*(1-T8)                     proc
                // AB4-AB4-MBAM4 => S2           MB*T8                     proc
                // AB4 => S1                    (1-MB)                 no proc
                // S2:
                // MBAM0 => S0       (1-T8)     
                // MBAM0 => S2       T8     

                // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

                // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
                // S1 = K0 * S0 + (1-MB) * S1
                // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
                // S0 + S1 + S2 = 1

                Spell MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float T8 = 0;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
                S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
                float S2 = -(MB * T8) / (K0 * T8 - MB - K0);
                K7 = S0 * (1 - (1 - MB) * (1 - MB));
                K1 = S0 * MB * (1 - MB) * (1 - MB);
                K2 = S0 * MB * (1 - MB) * (1 - MB) * (1 - MB);
                K3 = S0 * K0;
                K4 = S1 * MB;
                K5 = S1 * (1 - MB);

                if (needsDisplayCalculations)
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                    Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                    Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                    cycle.AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3 + K7);
                    cycle.AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3 + K7);
                    cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3 + K7);
                    cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);

                    Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                    Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                    cycle.AddSpell(needsDisplayCalculations, MBAM0, S2);
                    cycle.AddSpell(needsDisplayCalculations, MBAM3, K7);
                    cycle.AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Solver solver = castingState.Solver;
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, K1 + K2 + K3 + K7, K1 + K2 + K3 + K7, K1 + K2 + K3 + K7, K1 + K2 + K3, K2 + 2 * K4 + K5);
                    solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM0, S2, 0, 0, K7, K1 + K2 + K4);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ArcaneManaNeutral
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            List<Cycle> cycles = new List<Cycle>();
            cycles.Add(castingState.GetCycle(CycleId.ArcaneBlastSpam));
            cycles.Add(castingState.GetCycle(CycleId.ABSpam4AM));
            cycles.Add(castingState.GetCycle(CycleId.ABSpam34AM));
            cycles.Add(castingState.GetCycle(CycleId.ABSpam234AM));
            cycles.Add(castingState.GetCycle(CycleId.AB3ABar123AM));
            cycles.Add(castingState.GetCycle(CycleId.AB4ABar1234AM));
            cycles.Add(castingState.GetCycle(CycleId.AB4ABar34AM));
            cycles.Add(castingState.GetCycle(CycleId.AB4ABar4AM));
            cycles.Add(castingState.GetCycle(CycleId.AB3ABar023AM));
            cycles.Add(castingState.GetCycle(CycleId.AB23ABar023AM));
            cycles.Add(castingState.GetCycle(CycleId.AB2ABar02AMABABar));
            cycles.Add(castingState.GetCycle(CycleId.ABSpam0234AMABar));
            cycles.Add(castingState.GetCycle(CycleId.ABSpam0234AMABABar));
            cycles.Add(castingState.GetCycle(CycleId.AB2ABar2AMABar0AMABABar));
            cycles.Add(castingState.GetCycle(CycleId.ABABar1AM));

            cycles.Sort((c1, c2) => c1.ManaPerSecond.CompareTo(c2.ManaPerSecond));

            if (cycles[0].ManaPerSecond > 0)
            {
                return cycles[0];
            }

            int i = 0;
            while (i < cycles.Count)
            {
                float maxDpm = 0;
                int maxj = -1;
                for (int j = i + 1; j < cycles.Count; j++)
                {
                    float dpm = (cycles[j].DamagePerSecond - cycles[i].DamagePerSecond) / (cycles[j].ManaPerSecond - cycles[i].ManaPerSecond);
                    if (dpm > maxDpm)
                    {
                        maxDpm = dpm;
                        maxj = j;
                    }
                }
                if (maxj != -1)
                {
                    if (cycles[maxj].ManaPerSecond >= 0)
                    {
                        // mps1 + k * (mps2 - mps1)
                        float k = -cycles[i].ManaPerSecond / (cycles[maxj].ManaPerSecond - cycles[i].ManaPerSecond);
                        Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
                        cycle.Name = "ArcaneManaNeutral";
                        cycle.Note = string.Format("Mix {0:F}% {1} and {2:F}% {3}", 100 * (1 - k), cycles[i].Name, 100 * k, cycles[maxj].Name);
                        cycle.AddCycle(needsDisplayCalculations, cycles[i], (1 - k) / cycles[i].CastTime);
                        cycle.AddCycle(needsDisplayCalculations, cycles[maxj], k / cycles[maxj].CastTime);
                        cycle.DpmConversion = maxDpm;
                        cycle.Calculate();
                        return cycle;
                    }
                    i = maxj;
                }
                else
                {
                    // we've run out of cycles
                    return cycles[i];
                }
            }
            return null;
        }
    }

    public static class ABSpam0234AMABar
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "ABSpam0234AMABar";

            // ABar on cooldown only, assume we get into S0 from ABar, we'll incorrectly skip ABar sometimes
            // but it should be close enough, redo the math later if it's not
            // S0:
            // AB0-AB1-AM => S2/0               MB                         first AB procs
            // AB0-AB1-AB2-AM => S2/0           (1-MB)*MB                  2nd AB procs
            // AB0-AB1-AB2-AB3-AM => S2/0       (1-MB)*(1-MB)*MB           3rd AB procs
            // AB0-AB1-AB2-AB3-AB4-AM => S2/0   (1-MB)*(1-MB)*(1-MB)*MB    fourth AB procs
            // AB0-AB1-AB2-AB3 => S1               (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
            // S1:
            // AB4-AB4-AM => S2   MB      proc
            // AB4 => S1                  (1-MB)  no proc
            // S2:
            // ABar => S0     (1-MB)
            // ABar-AM => S0  MB

            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)
            // K1 = (1-MB)*..*(1-MB) depending on which cases fall into cooldown block

            // S0 = (1 - K1) * S0 + S2
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (K1 - K0) * S0 + MB * S1
            // S0 + S1 + S2 = 1

            // S0=MB/(K1*MB+MB+K0)
            // S1=K0/(K1*MB+MB+K0)
            // S2=(K1*MB)/(K1*MB+MB+K0)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
            Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);

            float MB = 0.4f;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            float K1;

            float cool = ABar.Cooldown;
            cool -= ABar.CastTime;
            cool -= AB0.CastTime;
            cool -= AB1.CastTime;
            cool -= AM.CastTime;
            if (cool < 0)
            {
                K1 = 1;
            }
            else
            {
                cool -= AB2.CastTime;
                if (cool < 0)
                {
                    K1 = (1 - MB);
                }
                else
                {
                    cool -= AB3.CastTime;
                    if (cool < 0)
                    {
                        K1 = (1 - MB) * (1 - MB);
                    }
                    else
                    {
                        cool -= AB4.CastTime;
                        if (cool < 0)
                        {
                            K1 = (1 - MB) * (1 - MB) * (1 - MB);
                        }
                        else
                        {
                            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                        }
                    }
                }
            }

            float S0 = MB / (K1 * MB + MB + K0);
            float S1 = K0 / (K1 * MB + MB + K0);
            float S2 = (K1 * MB) / (K1 * MB + MB + K0);

            cycle.AddSpell(needsDisplayCalculations, AB0, S0);
            cycle.AddSpell(needsDisplayCalculations, AB1, S0);
            cycle.AddSpell(needsDisplayCalculations, AB2, S0 * (1 - MB));
            cycle.AddSpell(needsDisplayCalculations, AB3, S0 * (1 - MB) * (1 - MB));
            cycle.AddSpell(needsDisplayCalculations, AB4, S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB + S1 * (1 + MB));
            cycle.AddSpell(needsDisplayCalculations, AM, S0 * (1 - K0) + S1 * MB + S2 * MB);
            cycle.AddSpell(needsDisplayCalculations, ABar, S2);

            cycle.Calculate();
            return cycle;
        }
    }

    /*public static class AB2ABar02AMABABar
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB2ABar02AMABABar";

            // S0: no proc
            // AB0-AB1-ABar     => S0    (1-MB)*(1-MB)*(1-MB)         nothing procs
            //                  => S1    (1-MB)*(1 - (1-MB)*(1-MB))   AB0 doesn't proc, AB1 or ABar procs
            // AB0-AB1  => S1    MB                    AB0 procs
            // S1: proc
            // AM-AB0-ABar      => S0    (1-MB)*(1-MB)       nothing procs
            //                  => S1    1 - (1-MB)*(1-MB)   either AB0 or ABar procs

            // K0 = (1-MB)*(1-MB)*(1-MB)
            // K1 = (1-MB)*(1-MB)

            // S0 = S0 * K0 + S1 * K1
            // S1 = S0 * (1 - K0) + S1 * (1 - K1)
            // S0 + S1 = 1

            // S0 = K1/(K1-K0+1)
            // S1 = 1 - S0

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);

            float MB = 0.4f;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB);
            float K1 = (1 - MB) * (1 - MB);
            float S0 = K1 / (K1 - K0 + 1);
            float S1 = 1 - S0;

            cycle.AddSpell(needsDisplayCalculations, AB0, 1);
            cycle.AddSpell(needsDisplayCalculations, AB1, S0);
            cycle.AddSpell(needsDisplayCalculations, AM, S1);
            cycle.AddSpell(needsDisplayCalculations, ABar, S0 * (1 - MB) + S1);

            float pause = ABar.Cooldown - 2 * AB0.CastTime - AB1.CastTime - AM.CastTime - ABar.CastTime;
            if (pause > 0)
            {
                cycle.AddPause(pause, S1 * K1);
            }
            pause = ABar.Cooldown - AB0.CastTime - AB1.CastTime - ABar.CastTime;
            if (pause > 0)
            {
                cycle.AddPause(pause, S0 * (1 - MB));
            }
            pause = ABar.Cooldown - AB0.CastTime - AM.CastTime - ABar.CastTime;
            if (pause > 0)
            {
                cycle.AddPause(pause, S1 * (1 - K1));
            }

            cycle.Calculate();
            return cycle;
        }
    }*/

    public static class AB2ABar2AMABar0AMABABar
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB2ABar2AMABar0AMABABar";

            // S0: no proc
            // AB0-AB1-ABar     => S0    (1-MB)*(1-MB)*(1-MB)         nothing procs
            //                  => S1    (1-MB)*(1 - (1-MB)*(1-MB))   AB0 doesn't proc, AB1 or ABar procs
            // AB0-AB1-AM-ABar  => S0    MB*(1-MB)                    AB0 procs, ABar doesn't proc
            //                  => S1    MB*MB                        AB0 and ABar both proc
            // S1: proc
            // AM-AB0-ABar      => S0    (1-MB)*(1-MB)       nothing procs
            //                  => S1    1 - (1-MB)*(1-MB)   either AB0 or ABar procs

            // S0 = (1 - MB) * (1 - MB) / (1 - MB*MB*(1-MB))
            // S1 = 1 - S0

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);

            float MB = 0.4f;
            float S0 = (1 - MB) * (1 - MB) / (1 - MB * MB * (1 - MB));
            float S1 = 1 - S0;

            cycle.AddSpell(needsDisplayCalculations, AB0, 1);
            cycle.AddSpell(needsDisplayCalculations, AB1, S0);
            cycle.AddSpell(needsDisplayCalculations, AM, S0 * MB + S1);
            cycle.AddSpell(needsDisplayCalculations, ABar, 1);

            float pause = ABar.Cooldown - AB0.CastTime - AB1.CastTime - AM.CastTime - ABar.CastTime;
            if (pause > 0)
            {
                cycle.AddPause(pause, S0 * MB);
            }
            pause = ABar.Cooldown - AB0.CastTime - AB1.CastTime - ABar.CastTime;
            if (pause > 0)
            {
                cycle.AddPause(pause, S0 * (1 - MB));
            }
            pause = ABar.Cooldown - AB0.CastTime - AM.CastTime - ABar.CastTime;
            if (pause > 0)
            {
                cycle.AddPause(pause, S1);
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam0234AMABABar
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "ABSpam0234AM[AB]ABar";

            // ABar on cooldown only, assume we get into S0 from ABar, we'll incorrectly skip ABar sometimes
            // but it should be close enough, redo the math later if it's not
            // S0:
            // AB0-AB1-AM => S3/2/0               MB                         first AB procs
            // AB0-AB1-AB2-AM => S3/2/0           (1-MB)*MB                  2nd AB procs
            // AB0-AB1-AB2-AB3-AM => S3/2/0       (1-MB)*(1-MB)*MB           3rd AB procs
            // AB0-AB1-AB2-AB3-AB4-AM => S3/2/0   (1-MB)*(1-MB)*(1-MB)*MB    fourth AB procs
            // AB0-AB1-AB2-AB3 => S1              (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
            // S1:
            // AB4-AB4-AM => S2   MB      proc
            // AB4 => S1                  (1-MB)  no proc
            // S2:
            // ABar => S0     (1-MB)
            // ABar-AM => S0/3  MB
            // S3:
            // AB0-ABar => S0     (1-MB)*(1-MB)
            // AB0-ABar-AM => S0/3  1 - (1-MB)*(1-MB)

            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)
            // K1 = (1-MB)*..*(1-MB) depending on which cases fall into S2
            // K2 = (1-MB)*..*(1-MB) depending on which cases fall into S3
            // K3 = (1-MB)*(1-MB)

            // S0 = (1 - K2) * S0 + S2 + S3
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (K1 - K0) * S0 + MB * S1
            // S3 = (K2 - K1) * S0
            // S0 + S1 + S2 + S3 = 1

            // S0=MB/(K2*MB+MB+K0)
            // S1=K0/(K2*MB+MB+K0)
            // S2=(K1*MB)/(K2*MB+MB+K0)
            // S3=(K2*MB-K1*MB)/(K2*MB+MB+K0)

            // low haste
            // S0 = (1 - K2) * S0 + S2*(1-MB) + S3*K3
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (K1 - K0) * S0 + MB * S1
            // S3 = (K2 - K1) * S0 + S2 * MB + S3 * (1 - K3)

            // S0 = K3*MB
            // S1 = K0*K3
            // S2 = K1*K3*MB
            // S3 = K1*MB^2+(K2-K1)*MB

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
            Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);

            float MB = 0.4f;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            float K1, K2;

            float cool = ABar.Cooldown;
            cool -= ABar.CastTime;
            cool -= AB0.CastTime;
            cool -= AB1.CastTime;
            cool -= AM.CastTime;
            if (cool < 0)
            {
                K1 = 1;
            }
            else
            {
                cool -= AB2.CastTime;
                if (cool < 0)
                {
                    K1 = (1 - MB);
                }
                else
                {
                    cool -= AB3.CastTime;
                    if (cool < 0)
                    {
                        K1 = (1 - MB) * (1 - MB);
                    }
                    else
                    {
                        cool -= AB4.CastTime;
                        if (cool < 0)
                        {
                            K1 = (1 - MB) * (1 - MB) * (1 - MB);
                        }
                        else
                        {
                            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                        }
                    }
                }
            }

            cool = ABar.Cooldown;
            cool -= ABar.CastTime;
            cool -= 2 * AB0.CastTime;
            cool -= AB1.CastTime;
            cool -= AM.CastTime;
            if (cool < 0)
            {
                K2 = 1;
            }
            else
            {
                cool -= AB2.CastTime;
                if (cool < 0)
                {
                    K2 = (1 - MB);
                }
                else
                {
                    cool -= AB3.CastTime;
                    if (cool < 0)
                    {
                        K2 = (1 - MB) * (1 - MB);
                    }
                    else
                    {
                        cool -= AB4.CastTime;
                        if (cool < 0)
                        {
                            K2 = (1 - MB) * (1 - MB) * (1 - MB);
                        }
                        else
                        {
                            K2 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                        }
                    }
                }
            }

            float S0, S1, S2, S3;

            if (AB0.CastTime + AM.CastTime + ABar.CastTime < ABar.Cooldown)
            {
                S0 = MB / (K2 * MB + MB + K0);
                S1 = K0 / (K2 * MB + MB + K0);
                S2 = (K1 * MB) / (K2 * MB + MB + K0);
                S3 = (K2 * MB - K1 * MB) / (K2 * MB + MB + K0);
            }
            else
            {
                float K3 = (1 - MB) * (1 - MB);
                S0 = K3 * MB;
                S1 = K0 * K3;
                S2 = K1 * K3 * MB;
                S3 = K1 * MB * MB + (K2 - K1) * MB;
            }

            cycle.AddSpell(needsDisplayCalculations, AB0, S0 + S3);
            cycle.AddSpell(needsDisplayCalculations, AB1, S0);
            cycle.AddSpell(needsDisplayCalculations, AB2, S0 * (1 - MB));
            cycle.AddSpell(needsDisplayCalculations, AB3, S0 * (1 - MB) * (1 - MB));
            cycle.AddSpell(needsDisplayCalculations, AB4, S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB + S1 * (1 + MB));
            cycle.AddSpell(needsDisplayCalculations, AM, S0 * (1 - K0) + S1 * MB + S2 * MB + S3 * (1 - (1 - MB) * (1 - MB)));
            cycle.AddSpell(needsDisplayCalculations, ABar, S2 + S3);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam234AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, K6, K7, S0, S1;
            cycle.Name = "ABSpam234AM";

            if (castingState.Solver.Mage2T10)
            {
                // 5 - channelLatency - n * ABT > 0
                // (5 - channelLatency) / ABT > n

                // S0:
                // AB0-AB1-AM => S0                  MB                               first AB procs
                // AB0-AB1-AB2-AM => S0              (1-MB)*MB                        second AB procs
                // AB0-AB1-AB2-AB3-AM => S0          (1-MB)*(1-MB)*MB                 third AB procs
                // AB0-AB1-AB2-AB3-AB4-AM => S0      (1-MB)*(1-MB)*(1-MB)*MB          fourth AB procs
                // AB0-AB1-AB2-AB3-AB4-AB4-AM => S0  (1-MB)*(1-MB)*(1-MB)*(1-MB)*MB   fifth AB procs
                // ...
                // AB0-...-AB4 => S1                    (1-MB)*...*(1-MB)                 no procs in first n,4
                // S1:
                // AB4-AB4-AM => S0   MB      proc
                // AB4 => S1            (1-MB)  no proc

                // K0 = (1-MB)^n

                // S0 = (1 - K0) * S0 + MB * S1
                // S1 = K0 * S0 + (1-MB) * S1
                // S0 + S1 = 1

                // S0 = MB / K0 * S1
                // S1 = K0 / (K0 + MB)
                // S0 = MB / (K0 + MB)

                float MB = 0.4f;

                if (needsDisplayCalculations)
                {
                    float m2T10time = 5.0f - castingState.CalculationOptions.LatencyChannel;
                    Spell AB0 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                    m2T10time -= AB0.CastTime;
                    Spell AB1 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                    m2T10time -= AB1.CastTime;
                    Spell AB2 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                    Spell AM2 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles) : castingState.GetSpell(SpellId.ArcaneMissiles);
                    m2T10time -= AB2.CastTime;
                    Spell AB3 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                    Spell AM3 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles) : castingState.GetSpell(SpellId.ArcaneMissiles);
                    m2T10time -= AB2.CastTime;
                    Spell AB4 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast4) : castingState.GetSpell(SpellId.ArcaneBlast4);
                    Spell ABn = AB4;
                    Spell AM4 = castingState.GetSpell(SpellId.ArcaneMissiles);
                    Spell AM4T = AM4;
                    if (m2T10time > 0.0f)
                    {
                        ABn = castingState.GetSpell(SpellId.ArcaneBlast4);
                        AM4T = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles);
                    }
                    float m = (float)Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / AB0.CastTime);
                    float n = Math.Max(4, m);
                    float K0 = (float)Math.Pow(1 - MB, n);
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    //n=8:

                    //AB0-AB1-MBAM2T                                 MB
                    //AB0-AB1-AB2-MBAM3T                             (1-MB)^1*MB
                    //AB0-AB1-AB2-AB3-MBAM4T                         (1-MB)^2*MB
                    //AB0-AB1-AB2-AB3-AB4-MBAM4T                     (1-MB)^3*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-MBAM4T                 (1-MB)^4*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-MBAM4T             (1-MB)^5*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4-MBAM4          (1-MB)^6*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4-AB4n-MBAM4     (1-MB)^7*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4                (1-MB)^8

                    //AB4n: S0 * (1-MB)^(n-1)*MB
                    //MBAM4T: S0 * (1-(1-MB)^3 + (1-MB)^3*MB + ... + (1-MB)^(n-3)*MB)
                    //        = S0 * (2 - (1-MB)^3 - (1-MB)^(n-5))
                    //MBAM4: S0 * ((1-MB)^(n-2)*MB + (1-MB)^(n-1)*MB)
                    //        = S0 * (1-MB)^(n-2)*MB * (2-MB)
                    //AB4: S0 * (k^3 - k^(n-1))/MB

                    if (n <= 4)
                    {
                        cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB2, S0 * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, AB3, S0 * (1 - MB - MB * (1 - MB)));
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB + S1 * (1 + MB));
                        cycle.AddSpell(needsDisplayCalculations, AM4, S0 * (1 - K0 - MB - MB * (1 - MB)) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, AM3, S0 * MB * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, AM2, S0 * MB);
                    }
                    else
                    {
                        cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB2, S0 * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, AB3, S0 * (1 - MB - MB * (1 - MB)));
                        cycle.AddSpell(needsDisplayCalculations, AB4, S0 * (float)(Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 1)) / MB);
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * (float)Math.Pow(1 - MB, n - 1) * MB + S1 * (1 + MB));
                        cycle.AddSpell(needsDisplayCalculations, AM4, S0 * (float)Math.Pow(1 - MB, n - 2) * MB * (2 - MB) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, AM4T, S0 * (float)(2 - Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 5) - MB - MB * (1 - MB)));
                        cycle.AddSpell(needsDisplayCalculations, AM3, S0 * MB * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, AM2, S0 * MB);
                    }
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell ABT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
                    Spell AMT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles);
                    float m = (float)Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / ABT.CastTime);
                    float n = Math.Max(4, m);
                    float K0 = (float)Math.Pow(1 - MB, n);
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    Solver solver = castingState.Solver;
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, (m < 1) ? S0 : 0, (m < 2) ? S0 : 0, (m < 3) ? S0 * (1 - MB) : 0, (m < 4) ? S0 * (1 - MB - MB * (1 - MB)) : 0, (m <= 4) ? (S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB + S1 * (1 + MB)) : (float)(S0 * Math.Pow(1 - MB, n - 1) * MB + S1 * (1 + MB)));
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, ABT, (m >= 1) ? S0 : 0, (m >= 2) ? S0 : 0, (m >= 3) ? S0 * (1 - MB) : 0, (m >= 4) ? S0 * (1 - MB - MB * (1 - MB)) : 0, (m <= 4) ? 0 : (float)(S0 * (Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 1)) / MB));

                    if (n <= 4)
                    {
                        cycle.AddSpell(needsDisplayCalculations, AM, ((m < 3) ? S0 * MB : 0) + ((m < 4) ? S0 * MB * (1 - MB) : 0) + S0 * (1 - K0 - MB - MB * (1 - MB)) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, AMT, ((m >= 3) ? S0 * MB : 0) + ((m >= 4) ? S0 * MB * (1 - MB) : 0));
                    }
                    else
                    {
                        cycle.AddSpell(needsDisplayCalculations, AM, S0 * (float)Math.Pow(1 - MB, n - 2) * MB * (2 - MB) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, AMT, S0 * MB + S0 * MB * (1 - MB) + S0 * (float)(2 - Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 5) - MB - MB * (1 - MB)));
                    }
                }
            }
            else
            {

                // S0:
                // AB0-AB1-AM => S0               MB                         first AB procs
                // AB0-AB1-AB2-AM => S0           (1-MB)*MB                  2nd AB procs
                // AB0-AB1-AB2-AB3-AM => S0       (1-MB)*(1-MB)*MB           3rd AB procs
                // AB0-AB1-AB2-AB3-AB4-AM => S0   (1-MB)*(1-MB)*(1-MB)*MB    fourth AB procs
                // AB0-AB1-AB2-AB3 => S1          (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
                // S1:
                // AB4-AB4-AM => S0           MB                     proc
                // AB4 => S1                  (1-MB)                 no proc

                // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

                // S0 = (1 - K0) * S0 + MB * S1
                // S1 = K0 * S0 + (1-MB) * S1
                // S0 + S1 = 1

                Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);

                float MB = 0.4f;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                S0 = MB / (MB + K0);
                S1 = K0 / (MB + K0);
                K6 = S0 * MB;
                K7 = S0 * MB * (1 - MB);
                K1 = S0 * MB * (1 - MB) * (1 - MB);
                K2 = S0 * MB * (1 - MB) * (1 - MB) * (1 - MB);
                K3 = S0 * K0;
                K4 = S1 * MB;
                K5 = S1 * (1 - MB);

                if (needsDisplayCalculations)
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                    Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                    Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                    cycle.AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3 + K6 + K7);
                    cycle.AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3 + K6 + K7);
                    cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3 + K7);
                    cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);

                    cycle.AddSpell(needsDisplayCalculations, AM, S0 * (1 - K0) + K4);
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Solver solver = castingState.Solver;
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K7, K1 + K2 + K3, K2 + 2 * K4 + K5);
                    cycle.AddSpell(needsDisplayCalculations, AM, S0 * (1 - K0) + K4);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam34AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            float S0, S1;
            cycle.Name = "ABSpam34AM";

            // S0:
            // AB0-AB1-AB2-AM => S0           1 - (1-MB)*(1-MB)          1st or 2nd AB procs
            // AB0-AB1-AB2-AB3-AM => S0       (1-MB)*(1-MB)*MB           3rd AB procs
            // AB0-AB1-AB2-AB3-AB4-AM => S0   (1-MB)*(1-MB)*(1-MB)*MB    fourth AB procs
            // AB0-AB1-AB2-AB3 => S1          (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
            // S1:
            // AB4-AB4-AM => S0           MB                     proc
            // AB4 => S1                  (1-MB)                 no proc

            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

            // S0 = (1 - K0) * S0 + MB * S1
            // S1 = K0 * S0 + (1-MB) * S1
            // S0 + S1 = 1

            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);

            float MB = 0.4f;
            float K1 = (1 - MB) * (1 - MB);
            float K0 = K1 * K1;            
            S0 = MB / (MB + K0);
            S1 = K0 / (MB + K0);
            float K2 = S0 * K1 * (1 - MB) * MB + S1 * (1 + MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                cycle.AddSpell(needsDisplayCalculations, AB2, S0);
                cycle.AddSpell(needsDisplayCalculations, AB3, S0 * K1);
                cycle.AddSpell(needsDisplayCalculations, AB4, K2);

                cycle.AddSpell(needsDisplayCalculations, AM, S0 * (1 - K0) + S1 * MB);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                Solver solver = castingState.Solver;
                solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, S0, S0, S0, S0 * K1, K2);
                cycle.AddSpell(needsDisplayCalculations, AM, S0 * (1 - K0) + S1 * MB);
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam4AM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            float S0, S1;
            cycle.Name = "ABSpam4AM";

            // S0:
            // AB0-AB1-AB2-AB3-AM => S0       1 - (1-MB)*(1-MB)*(1-MB)   1st, 2nd or 3rd AB procs
            // AB0-AB1-AB2-AB3-AB4-AM => S0   (1-MB)*(1-MB)*(1-MB)*MB    fourth AB procs
            // AB0-AB1-AB2-AB3 => S1          (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
            // S1:
            // AB4-AB4-AM => S0           MB                     proc
            // AB4 => S1                  (1-MB)                 no proc

            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

            // S0 = (1 - K0) * S0 + MB * S1
            // S1 = K0 * S0 + (1-MB) * S1
            // S0 + S1 = 1

            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);

            float MB = 0.4f;
            float K1 = (1 - MB) * (1 - MB);
            float K0 = K1 * K1;
            S0 = MB / (MB + K0);
            S1 = K0 / (MB + K0);
            float K2 = S0 * K1 * (1 - MB) * MB + S1 * (1 + MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                cycle.AddSpell(needsDisplayCalculations, AB2, S0);
                cycle.AddSpell(needsDisplayCalculations, AB3, S0);
                cycle.AddSpell(needsDisplayCalculations, AB4, K2);

                cycle.AddSpell(needsDisplayCalculations, AM, S0 * (1 - K0) + S1 * MB);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                Solver solver = castingState.Solver;
                solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, S0, S0, S0, S0, K2);
                cycle.AddSpell(needsDisplayCalculations, AM, S0 * (1 - K0) + S1 * MB);
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam0234MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, K6, K7, S0, S1;
            cycle.Name = "ABSpam0234MBAM";

            if (castingState.Solver.Mage2T10)
            {
                // 5 - channelLatency - n * ABT > 0
                // (5 - channelLatency) / ABT > n

                // S0:
                // AB0-AB1-MBAM2 => S0                  MB                               first AB procs
                // AB0-AB1-AB2-MBAM3 => S0              (1-MB)*MB                        second AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S0          (1-MB)*(1-MB)*MB                 third AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0      (1-MB)*(1-MB)*(1-MB)*MB          fourth AB procs
                // AB0-AB1-AB2-AB3-AB4-AB4-MBAM4 => S0  (1-MB)*(1-MB)*(1-MB)*(1-MB)*MB   fifth AB procs
                // ...
                // AB0-...-AB4 => S1                    (1-MB)*...*(1-MB)                 no procs in first n,4
                // S1:
                // AB4-AB4-MBAM4 => S0   MB      proc
                // AB4 => S1            (1-MB)  no proc

                // K0 = (1-MB)^n

                // S0 = (1 - K0) * S0 + MB * S1
                // S1 = K0 * S0 + (1-MB) * S1
                // S0 + S1 = 1

                // S0 = MB / K0 * S1
                // S1 = K0 / (K0 + MB)
                // S0 = MB / (K0 + MB)

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;

                if (needsDisplayCalculations)
                {
                    float m2T10time = 5.0f - castingState.CalculationOptions.LatencyChannel;
                    Spell AB0 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                    m2T10time -= AB0.CastTime;
                    Spell AB1 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                    m2T10time -= AB1.CastTime;
                    Spell AB2 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                    Spell MBAM2 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                    m2T10time -= AB2.CastTime;
                    Spell AB3 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                    Spell MBAM3 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB3) : castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                    m2T10time -= AB2.CastTime;
                    Spell AB4 = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast4) : castingState.GetSpell(SpellId.ArcaneBlast4);
                    Spell ABn = AB4;
                    Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                    Spell MBAM4T = MBAM4;
                    if (m2T10time > 0.0f)
                    {
                        ABn = castingState.GetSpell(SpellId.ArcaneBlast4);
                        MBAM4T = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB4);
                    }
                    float m = (float)Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / AB0.CastTime);
                    float n = Math.Max(4, m);
                    float K0 = (float)Math.Pow(1 - MB, n);
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    //n=8:

                    //AB0-AB1-MBAM2T                                 MB
                    //AB0-AB1-AB2-MBAM3T                             (1-MB)^1*MB
                    //AB0-AB1-AB2-AB3-MBAM4T                         (1-MB)^2*MB
                    //AB0-AB1-AB2-AB3-AB4-MBAM4T                     (1-MB)^3*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-MBAM4T                 (1-MB)^4*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-MBAM4T             (1-MB)^5*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4-MBAM4          (1-MB)^6*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4-AB4n-MBAM4     (1-MB)^7*MB
                    //AB0-AB1-AB2-AB3-AB4-AB4-AB4-AB4                (1-MB)^8

                    //AB4n: S0 * (1-MB)^(n-1)*MB
                    //MBAM4T: S0 * (1-(1-MB)^3 + (1-MB)^3*MB + ... + (1-MB)^(n-3)*MB)
                    //        = S0 * (2 - (1-MB)^3 - (1-MB)^(n-5))
                    //MBAM4: S0 * ((1-MB)^(n-2)*MB + (1-MB)^(n-1)*MB)
                    //        = S0 * (1-MB)^(n-2)*MB * (2-MB)
                    //AB4: S0 * (k^3 - k^(n-1))/MB

                    if (n <= 4)
                    {
                        cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB2, S0 * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, AB3, S0 * (1 - MB - MB * (1 - MB)));
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB + S1 * (1 + MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (1 - K0 - MB - MB * (1 - MB)) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, MBAM3, S0 * MB * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM2, S0 * MB);
                    }
                    else
                    {
                        cycle.AddSpell(needsDisplayCalculations, AB0, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB1, S0);
                        cycle.AddSpell(needsDisplayCalculations, AB2, S0 * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, AB3, S0 * (1 - MB - MB * (1 - MB)));
                        cycle.AddSpell(needsDisplayCalculations, AB4, S0 * (float)(Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 1)) / MB);
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * (float)Math.Pow(1 - MB, n - 1) * MB + S1 * (1 + MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (float)Math.Pow(1 - MB, n - 2) * MB * (2 - MB) + S1 * MB);
                        cycle.AddSpell(needsDisplayCalculations, MBAM4T, S0 * (float)(2 - Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 5) - MB - MB * (1 - MB)));
                        cycle.AddSpell(needsDisplayCalculations, MBAM3, S0 * MB * (1 - MB));
                        cycle.AddSpell(needsDisplayCalculations, MBAM2, S0 * MB);
                    }
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell ABT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlastRaw);
                    Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
                    Spell MBAMT = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB);
                    float m = (float)Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / ABT.CastTime);
                    float n = Math.Max(4, m);
                    float K0 = (float)Math.Pow(1 - MB, n);
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    Solver solver = castingState.Solver;
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, (m < 1) ? S0 : 0, (m < 2) ? S0 : 0, (m < 3) ? S0 * (1 - MB) : 0, (m < 4) ? S0 * (1 - MB - MB * (1 - MB)) : 0, (m <= 4) ? (S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB + S1 * (1 + MB)) : (float)(S0 * Math.Pow(1 - MB, n - 1) * MB + S1 * (1 + MB)));
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, ABT, (m >= 1) ? S0 : 0, (m >= 2) ? S0 : 0, (m >= 3) ? S0 * (1 - MB) : 0, (m >= 4) ? S0 * (1 - MB - MB * (1 - MB)) : 0, (m <= 4) ? 0 : (float)(S0 * (Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 1)) / MB));

                    if (n <= 4)
                    {
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM, 0, 0, (m < 3) ? S0 * MB : 0, (m < 4) ? S0 * MB * (1 - MB) : 0, S0 * (1 - K0 - MB - MB * (1 - MB)) + S1 * MB);
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAMT, 0, 0, (m >= 3) ? S0 * MB : 0, (m >= 4) ? S0 * MB * (1 - MB) : 0, 0);
                    }
                    else
                    {
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM, 0, 0, 0, 0, S0 * (float)Math.Pow(1 - MB, n - 2) * MB * (2 - MB) + S1 * MB);
                        solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAMT, 0, 0, S0 * MB, S0 * MB * (1 - MB), S0 * (float)(2 - Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 5) - MB - MB * (1 - MB)));
                    }
                }
            }
            else
            {

                // S0:
                // AB0-AB1-MBAM2 => S0               MB*(1-T8)                         first AB procs
                // AB0-AB1-MBAM2 => S2               MB*T8                             first AB procs
                // AB0-AB1-AB2-MBAM3 => S0           (1-MB)*MB*(1-T8)                  2nd AB procs
                // AB0-AB1-AB2-MBAM3 => S2           (1-MB)*MB*T8                      2nd AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S0       (1-MB)*(1-MB)*MB*(1-T8)           3rd AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S2       (1-MB)*(1-MB)*MB*T8               3rd AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-T8)    fourth AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S2   (1-MB)*(1-MB)*(1-MB)*MB*T8        fourth AB procs
                // AB0-AB1-AB2-AB3 => S1             (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
                // S1:
                // AB4-AB4-MBAM4 => S0           MB*(1-T8)                     proc
                // AB4-AB4-MBAM4 => S2           MB*T8                     proc
                // AB4 => S1                    (1-MB)                 no proc
                // S2:
                // MBAM0 => S0       (1-T8)     
                // MBAM0 => S2       T8     

                // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

                // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
                // S1 = K0 * S0 + (1-MB) * S1
                // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
                // S0 + S1 + S2 = 1

                Spell MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float T8 = 0;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
                S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
                float S2 = -(MB * T8) / (K0 * T8 - MB - K0);
                K6 = S0 * MB;
                K7 = S0 * MB * (1 - MB);
                K1 = S0 * MB * (1 - MB) * (1 - MB);
                K2 = S0 * MB * (1 - MB) * (1 - MB) * (1 - MB);
                K3 = S0 * K0;
                K4 = S1 * MB;
                K5 = S1 * (1 - MB);

                if (needsDisplayCalculations)
                {
                    Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                    Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                    Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                    Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                    Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                    cycle.AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3 + K6 + K7);
                    cycle.AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3 + K6 + K7);
                    cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3 + K7);
                    cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                    cycle.AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);

                    Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                    Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                    Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                    cycle.AddSpell(needsDisplayCalculations, MBAM0, S2);
                    cycle.AddSpell(needsDisplayCalculations, MBAM2, K6);
                    cycle.AddSpell(needsDisplayCalculations, MBAM3, K7);
                    cycle.AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    Solver solver = castingState.Solver;
                    solver.ArcaneBlastTemplate.AddToCycle(solver, cycle, AB, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K7, K1 + K2 + K3, K2 + 2 * K4 + K5);
                    solver.ArcaneMissilesTemplate.AddToCycle(solver, cycle, MBAM0, S2, 0, K6, K7, K1 + K2 + K4);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam234MBAM
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, K6, K7, S0, S1;
            cycle.Name = "ABSpam234MBAM";

            // always ramp up to 4 AB before using MBAM

            // S0:
            // AB0-AB1-MBAM2 => S0               MB*(1-T8)                         first AB procs
            // AB0-AB1-MBAM2 => S2               MB*T8                             first AB procs
            // AB0-AB1-AB2-MBAM3 => S0           (1-MB)*MB*(1-T8)                  2nd AB procs
            // AB0-AB1-AB2-MBAM3 => S2           (1-MB)*MB*T8                      2nd AB procs
            // AB0-AB1-AB2-AB3-MBAM4 => S0       (1-MB)*(1-MB)*MB*(1-T8)           3rd AB procs
            // AB0-AB1-AB2-AB3-MBAM4 => S2       (1-MB)*(1-MB)*MB*T8               3rd AB procs
            // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-T8)    fourth AB procs
            // AB0-AB1-AB2-AB3-AB4-MBAM4 => S2   (1-MB)*(1-MB)*(1-MB)*MB*T8        fourth AB procs
            // AB0-AB1-AB2-AB3 => S1             (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
            // S1:
            // AB4-AB4-MBAM4 => S0           MB*(1-T8)                     proc
            // AB4-AB4-MBAM4 => S2           MB*T8                     proc
            // AB4 => S1                    (1-MB)                 no proc
            // S2:
            // AB0-AB1-MBAM2 => S0       (1-T8)     
            // AB0-AB1-MBAM2 => S2       T8     

            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

            // S0 = (1 - K0) * (1 - T8) * S0 + MB * (1 - T8) * S1 + (1 - T8) * S2
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (1 - K0) * T8 * S0 + MB * T8 * S1 + T8 * S2
            // S0 + S1 + S2 = 1

            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);

            float MB = 0.08f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            S0 = (MB * T8 - MB) / (K0 * T8 - MB - K0);
            S1 = (K0 * T8 - K0) / (K0 * T8 - MB - K0);
            float S2 = -(MB * T8) / (K0 * T8 - MB - K0);
            K6 = S0 * MB + S2;
            K7 = S0 * MB * (1 - MB);
            K1 = S0 * MB * (1 - MB) * (1 - MB);
            K2 = S0 * MB * (1 - MB) * (1 - MB) * (1 - MB);
            K3 = S0 * K0;
            K4 = S1 * MB;
            K5 = S1 * (1 - MB);

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
                cycle.AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3 + K6 + K7);
                cycle.AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3 + K6 + K7);
                cycle.AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3 + K7);
                cycle.AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                cycle.AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, cycle, AB, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K7, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            cycle.AddSpell(needsDisplayCalculations, MBAM2, K6);
            cycle.AddSpell(needsDisplayCalculations, MBAM3, K7);
            cycle.AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);

            cycle.Calculate();
            return cycle;
        }
    }

    class AB2ABar2C : Cycle
    {
        public AB2ABar2C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2;
            Name = "AB2ABar2C";

            // S0: no proc at start
            // AB0-AB1-ABar2          => S0     (1-MB)*(1-MB)*(1-MB)
            //                        => S1     (1-MB)*(1 - (1-MB)*(1-MB))
            // AB0-AB1-MBAM2-ABar2    => S0     MB*(1-T8)*(1-MB)
            //                        => S1     MB*(1 - (1-T8)*(1-MB))
            // S1: proc at start
            // AB0-AB1-MBAM2-ABar2    => S0     (1-T8)*(1-MB)
            //                        => S1     1 - (1-T8)*(1-MB)

            // S0 = S0 * ((1-MB)*(1-MB)*(1-MB) + MB*(1-T8)*(1-MB)) + S1 * (1-T8)*(1-MB)
            // S1 = S0 * ((1-MB)*(1 - (1-MB)*(1-MB)) + MB*(1 - (1-T8)*(1-MB))) + S1 * (1 - (1-T8)*(1-MB))
            // S0 + S1 = 1

            // P1 = (1-MB)*(1-MB)
            // P2 = (1-T8)*(1-MB)
            // S0 = S0 * ((1-MB)*P1 + MB*P2) + S1 * P2

            // S0 = P2/(1 + (1-MB)*(P2 - P1))

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float P1 = (1 - MB) * (1 - MB);
            float P2 = (1 - T8) * (1 - MB);
            float S0 = P2 / (1 + (1 - MB) * (P2 - P1));
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB);
            K2 = S0 * MB + S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            AddSpell(needsDisplayCalculations, AB1, K1 + K2);
            AddSpell(needsDisplayCalculations, ABar2, K1);
            AddSpell(needsDisplayCalculations, MBAM2, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class AB2ABar2MBAM : Cycle
    {
        public AB2ABar2MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2;
            Name = "AB2ABar2MBAM";

            // S0: no proc at start

            // AB0-AB1-ABar2          => S0     (1-MB)*(1-MB)*(1-MB)
            //                        => S1     (1-MB)*(1 - (1-MB)*(1-MB))
            // AB0-AB1-MBAM2          => S0     MB*(1-T8)
            //                        => S1     MB*T8
            // S1: proc at start
            // AB0-AB1-MBAM2          => S0     1-T8
            //                        => S1     T8

            // P1 = (1-MB)*(1-MB)
            // P2 = (1-T8)
            // S0 = S0 * ((1-MB)*P1 + MB*P2) + S1 * P2

            // S0 = P2/(1 + (1-MB)*(P2 - P1))

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float P1 = (1 - MB) * (1 - MB);
            float P2 = (1 - T8);
            float S0 = P2 / (1 + (1 - MB) * (P2 - P1));
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB);
            K2 = S0 * MB + S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            AddSpell(needsDisplayCalculations, AB1, K1 + K2);
            AddSpell(needsDisplayCalculations, ABar2, K1);
            AddSpell(needsDisplayCalculations, MBAM2, K2);

            Calculate();
        }
    }

    class AB2ABar3C : Cycle
    {
        public AB2ABar3C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2;
            Name = "AB2ABar3C";

            // S0: no proc at start
            // AB0-AB1-ABar2              => S0     (1-MB)*(1-MB)*(1-MB)
            //                            => S1     (1-MB)*(1 - (1-MB)*(1-MB))
            // AB0-AB1-AB2-MBAM3-ABar     => S0     MB*(1-T8)*(1-MB)
            //                            => S1     MB*(1 - (1-T8)*(1-MB))
            // S1: proc at start
            // AB0-AB1-AB2-MBAM3-ABar     => S0     (1-T8)*(1-MB)
            //                            => S1     1 - (1-T8)*(1-MB)

            // P1 = (1-MB)*(1-MB)
            // P2 = (1-T8)*(1-MB)
            // S0 = S0 * ((1-MB)*P1 + MB*P2) + S1 * P2

            // S0 = P2/(1 + (1-MB)*(P2 - P1))

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float P1 = (1 - MB) * (1 - MB);
            float P2 = (1 - T8) * (1 - MB);
            float S0 = P2 / (1 + (1 - MB) * (P2 - P1));
            float S1 = 1 - S0;
            K1 = S0 * (1 - MB);
            K2 = S0 * MB + S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            AddSpell(needsDisplayCalculations, AB1, K1 + K2);
            AddSpell(needsDisplayCalculations, ABar2, K1);
            AddSpell(needsDisplayCalculations, AB2, K2);
            AddSpell(needsDisplayCalculations, MBAM3, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class ABABar3C : Cycle
    {
        public ABABar3C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2;
            Name = "ABABar3C";

            // S0: no proc at start
            // AB0-ABar1                  => S0     (1-MB)*(1-MB)
            //                            => S1     (1 - (1-MB)*(1-MB))
            // S1: proc at start
            // AB0-AB1-AB2-MBAM3-ABar     => S0     (1-T8)(1-MB)
            //                            => S1     (1 - (1-T8)(1-MB))

            // P1 = (1-MB)*(1-MB)
            // P2 = (1-T8)*(1-MB)
            // S0 = S0 * P1 + S1 * P2

            // S0 = P2/(P2-P1+1)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float P1 = (1 - MB) * (1 - MB);
            float P2 = (1 - T8) * (1 - MB);
            float S0 = P2 / (P2 - P1 + 1);
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            if (AB0.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - ABar1.CastTime, K1);
            AddSpell(needsDisplayCalculations, ABar1, K1);
            AddSpell(needsDisplayCalculations, AB1, K2);
            AddSpell(needsDisplayCalculations, AB2, K2);
            AddSpell(needsDisplayCalculations, MBAM3, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class ABABar2C : Cycle
    {
        public ABABar2C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2;
            Name = "ABABar2C";

            // S0: no proc at start
            // AB0-ABar1                  => S0     (1-MB)*(1-MB)
            //                            => S1     (1 - (1-MB)*(1-MB))
            // S1: proc at start
            // AB0-AB1-MBAM2-ABar         => S0     (1-T8)*(1-MB)
            //                            => S1     (1 - (1-T8)*(1-MB))

            // P1 = (1-MB)*(1-MB)
            // P2 = (1-T8)*(1-MB)
            // S0 = S0 * P1 + S1 * P2

            // S0 = P2/(P2-P1+1)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float P1 = (1 - MB) * (1 - MB);
            float P2 = (1 - T8) * (1 - MB);
            float S0 = P2 / (P2 - P1 + 1);
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            if (AB0.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - ABar1.CastTime, K1);
            AddSpell(needsDisplayCalculations, ABar1, K1);
            AddSpell(needsDisplayCalculations, AB1, K2);
            AddSpell(needsDisplayCalculations, MBAM2, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class ABABar2MBAM : Cycle
    {
        public ABABar2MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2;
            Name = "ABABar2MBAM";

            // S0: no proc at start
            // AB0-ABar1                  => S0     (1-MB)*(1-MB)
            //                            => S1     (1 - (1-MB)*(1-MB))
            // S1: proc at start
            // AB0-AB1-MBAM2              => S0     (1-T8)
            //                            => S1     T8

            // P1 = (1-MB)*(1-MB)
            // P2 = (1-T8)
            // S0 = S0 * P1 + S1 * P2

            // S0 = P2/(P2-P1+1)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float P1 = (1 - MB) * (1 - MB);
            float P2 = (1 - T8);
            float S0 = P2 / (P2 - P1 + 1);
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            if (AB0.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - ABar1.CastTime, K1);
            AddSpell(needsDisplayCalculations, ABar1, K1);
            AddSpell(needsDisplayCalculations, AB1, K2);
            AddSpell(needsDisplayCalculations, MBAM2, K2);

            Calculate();
        }
    }

    class ABABar1MBAM : Cycle
    {
        public ABABar1MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2;
            Name = "ABABar1MBAM";

            // S0: no proc at start
            // AB0-ABar1                  => S0     (1-MB)*(1-MB)
            //                            => S1     (1 - (1-MB)*(1-MB))
            // S1: proc at start
            // AB0-MBAM1                  => S0     1-T8    
            //                            => S1     T8

            // P1 = (1-MB)*(1-MB)
            // P2 = (1-T8)
            // S0 = S0 * P1 + S1 * P2

            // S0 = P2/(P2-P1+1)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float P1 = (1 - MB) * (1 - MB);
            float P2 = (1 - T8);
            float S0 = P2 / (P2 - P1 + 1);
            float S1 = 1 - S0;
            K1 = S0;
            K2 = S1;

            AddSpell(needsDisplayCalculations, AB0, K1 + K2);
            if (AB0.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - ABar1.CastTime, K1);
            AddSpell(needsDisplayCalculations, ABar1, K1);
            AddSpell(needsDisplayCalculations, MBAM1, K2);

            Calculate();
        }
    }

    class AB3ABar3C : Cycle
    {
        public AB3ABar3C(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2;
            Name = "AB3ABar3C";

            // S0: no proc at start
            // AB0-AB1-AB2-ABar3          => S0     (1-MB)*(1-MB)*(1-MB)*(1-MB)
            //                            => S1     (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))
            // AB0-AB1-AB2-MBAM3-ABar3C   => S0     (1 - (1-MB)*(1-MB))*(1-T8)*(1-MB)
            //                            => S1     (1 - (1-MB)*(1-MB))*(1 - (1-T8)*(1-MB))
            // S1: proc at start
            // AB0-AB1-AB2-MBAM3-ABar3C   => S0     (1-T8)*(1-MB)
            //                            => S1     (1 - (1-T8)*(1-MB))

            // P1 = (1-MB)*(1-MB)
            // P2 = (1-T8)*(1-MB)

            // S0 = S0 * (P1 * P1 + (1 - P1) * P2) + S1 * P2

            // S0 = P2/(P1*(P2-P1)+1)


            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float P1 = (1 - MB) * (1 - MB);
            float P2 = (1 - T8) * (1 - MB);
            float S0 = P2 / (P1 * (P2 - P1) + 1);
            float S1 = 1 - S0;
            K1 = S0 * P1;
            K2 = S0 * (1 - P1) + S1;

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, this, AB, K1 + K2, K1 + K2, K1 + K2, 0);
            }
            AddSpell(needsDisplayCalculations, ABar3, K1);
            AddSpell(needsDisplayCalculations, MBAM3, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class AB3ABar3MBAM : Cycle
    {
        public AB3ABar3MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2;
            Name = "AB3ABar3MBAM";

            // S0: no proc at start
            // AB0-AB1-AB2-ABar3          => S0     (1-MB)*(1-MB)*(1-MB)*(1-MB)
            //                            => S1     (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))
            // AB0-AB1-AB2-MBAM3          => S0     (1 - (1-MB)*(1-MB))*(1-T8)
            //                            => S1     (1 - (1-MB)*(1-MB))*T8
            // S1: proc at start
            // AB0-AB1-AB2-MBAM3          => S0     1-T8
            //                            => S1     T8

            // P1 = (1-MB)*(1-MB)
            // P2 = (1-T8)

            // S0 = S0 * (P1 * P1 + (1 - P1) * P2) + S1 * P2

            // S0 = P2/(P1*(P2-P1)+1)

            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = 0;
            float P1 = (1 - MB) * (1 - MB);
            float P2 = (1 - T8);
            float S0 = P2 / (P1 * (P2 - P1) + 1);
            float S1 = 1 - S0;
            K1 = S0 * P1;
            K2 = S0 * (1 - P1) + S1;

            if (needsDisplayCalculations)
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
                AddSpell(needsDisplayCalculations, AB0, K1 + K2);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Solver.ArcaneBlastTemplate.AddToCycle(castingState.Solver, this, AB, K1 + K2, K1 + K2, K1 + K2, 0);
            }
            AddSpell(needsDisplayCalculations, ABar3, K1);
            AddSpell(needsDisplayCalculations, MBAM3, K2);

            Calculate();
        }
    }

    public class ArcaneCycleGenerator : CycleGenerator
    {
        private class State : CycleState
        {
            public bool MissileBarrageRegistered { get; set; }
            public float MissileBarrageDuration { get; set; }
            public bool ArcaneMissilesRegistered { get; set; }
            public bool ArcaneMissilesProcced { get; set; }
            public float ArcaneBarrageCooldown { get; set; }
            public int ArcaneBlastStack { get; set; }
            public float Tier10TwoPieceDuration { get; set; }
        }

        public Spell[,] AB, ABar, AM, MBAM;

        private float ABMB;
        private float MB;
        private float AMProc;
        private float T8;
        private int maxStack;
        private bool T10;
        private float channelLatency;
        private bool beta;

        private bool ABarAllowed;
        private bool ABarOnCooldownOnly;
        private bool MBDurationCollapsed;
        private bool AMDurationCollapsed;
        private bool ABarCooldownCollapsed;
        private bool Tier10TwoPieceCollapsed;        

        public ArcaneCycleGenerator(CastingState castingState, bool ABarAllowed, bool ABarOnCooldownOnly, bool MBDurationCollapsed, bool ABarCooldownCollapsed, bool Tier10TwoPieceCollapsed, bool AMDurationCollapsed)
        {
            this.ABarAllowed = ABarAllowed;
            this.ABarOnCooldownOnly = ABarOnCooldownOnly;
            this.MBDurationCollapsed = MBDurationCollapsed;
            this.ABarCooldownCollapsed = ABarCooldownCollapsed;
            this.Tier10TwoPieceCollapsed = Tier10TwoPieceCollapsed;
            this.AMDurationCollapsed = AMDurationCollapsed;

            var calc = castingState.Solver;
            maxStack = 4;

            AB = new Spell[maxStack + 1, 2];
            ABar = new Spell[maxStack + 1, 2];
            AM = new Spell[maxStack + 1, 2];
            MBAM = new Spell[maxStack + 1, 2];

            for (int stack = 0; stack <= maxStack; stack++)
            {
                AB[stack, 0] = calc.ArcaneBlastTemplate.GetSpell(castingState, stack);
                AB[stack, 0].Label = "ArcaneBlast" + stack;
                ABar[stack, 0] = calc.ArcaneBarrageTemplate.GetSpell(castingState, stack);
                ABar[stack, 0].Label = "ArcaneBarrage" + stack;
                AM[stack, 0] = calc.ArcaneMissilesTemplate.GetSpell(castingState, false, stack);
                AM[stack, 0].Label = "ArcaneMissiles" + stack;
                MBAM[stack, 0] = calc.ArcaneMissilesTemplate.GetSpell(castingState, true, stack);
                MBAM[stack, 0].Label = "MBAM" + stack;
                AB[stack, 1] = calc.ArcaneBlastTemplate.GetSpell(castingState.Tier10TwoPieceState, stack);
                AB[stack, 1].Label = "2T10:ArcaneBlast" + stack;
                ABar[stack, 1] = calc.ArcaneBarrageTemplate.GetSpell(castingState.Tier10TwoPieceState, stack);
                ABar[stack, 1].Label = "2T10:ArcaneBarrage" + stack;
                AM[stack, 1] = calc.ArcaneMissilesTemplate.GetSpell(castingState.Tier10TwoPieceState, false, stack);
                AM[stack, 1].Label = "2T10:ArcaneMissiles" + stack;
                MBAM[stack, 1] = calc.ArcaneMissilesTemplate.GetSpell(castingState.Tier10TwoPieceState, true, stack);
                MBAM[stack, 1].Label = "2T10:MBAM" + stack;
            }

            ABMB = 0.08f * castingState.MageTalents.MissileBarrage;
            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            T8 = 0;
            T10 = castingState.Solver.Mage2T10;
            channelLatency = castingState.CalculationOptions.LatencyChannel;
            beta = true;
            AMProc = 0.3f;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(false, 0.0f, 0.0f, 0, 0.0f, false, false);
        }

        protected override List<CycleControlledStateTransition> GetStateTransitions(CycleState state)
        {
            State s = (State)state;
            List<CycleControlledStateTransition> list = new List<CycleControlledStateTransition>();
            Spell AB = null;
            Spell AM = null;
            Spell ABar = null;
            AB = this.AB[s.ArcaneBlastStack, s.Tier10TwoPieceDuration > 0 ? 1 : 0];
            ABar = this.ABar[s.ArcaneBlastStack, s.Tier10TwoPieceDuration - s.ArcaneBarrageCooldown > 0 ? 1 : 0];
            if (s.MissileBarrageDuration > 0)
            {
                AM = this.MBAM[s.ArcaneBlastStack, s.Tier10TwoPieceDuration > 0 ? 1 : 0];
            }
            else
            {
                AM = this.AM[s.ArcaneBlastStack, s.Tier10TwoPieceDuration > 0 ? 1 : 0];
            }
            if (MB > 0)
            {
                if (AMProc > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = AB,
                        TargetState = GetState(
                            s.MissileBarrageDuration > AB.CastTime,
                            15.0f,
                            Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                            Math.Min(maxStack, s.ArcaneBlastStack + 1),
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - AB.CastTime),
                            s.ArcaneMissilesProcced,
                            true
                        ),
                        TransitionProbability = ABMB * AMProc
                    });
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    TargetState = GetState(
                        s.MissileBarrageDuration > AB.CastTime,
                        15.0f,
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                        Math.Min(maxStack, s.ArcaneBlastStack + 1),
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - AB.CastTime),
                        s.ArcaneMissilesProcced,
                        s.ArcaneMissilesProcced
                    ),
                    TransitionProbability = ABMB * (1 - AMProc)
                });
            }
            if (AMProc > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    TargetState = GetState(
                        s.MissileBarrageDuration > AB.CastTime,
                        Math.Max(0.0f, s.MissileBarrageDuration - AB.CastTime),
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                        Math.Min(maxStack, s.ArcaneBlastStack + 1),
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - AB.CastTime),
                        s.ArcaneMissilesProcced,
                        true
                    ),
                    TransitionProbability = (1.0f - ABMB) * AMProc
                });
            }
            list.Add(new CycleControlledStateTransition()
            {
                Spell = AB,
                TargetState = GetState(
                    s.MissileBarrageDuration > AB.CastTime,
                    Math.Max(0.0f, s.MissileBarrageDuration - AB.CastTime),
                    Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                    Math.Min(maxStack, s.ArcaneBlastStack + 1),
                    Math.Max(0.0f, s.Tier10TwoPieceDuration - AB.CastTime),
                    s.ArcaneMissilesProcced,
                    s.ArcaneMissilesProcced
                ),
                TransitionProbability = (1.0f - ABMB) * (1 - AMProc)
            });
            if (s.MissileBarrageDuration > 0 && T8 > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AM,
                    TargetState = GetState(
                        s.MissileBarrageDuration > AM.CastTime,
                        Math.Max(0.0f, s.MissileBarrageDuration - AM.CastTime),
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                        0,
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - AM.CastTime), // cannot have 4T8 and 2T10 at the same time
                        false,
                        false
                    ),
                    TransitionProbability = T8
                });
            }
            if (!beta || s.ArcaneMissilesRegistered || s.MissileBarrageRegistered)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AM,
                    TargetState = GetState(
                        false,
                        0.0f,
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                        0,
                        Math.Max(0.0f, (s.MissileBarrageDuration > 0 && T10) ? 5.0f - channelLatency : s.Tier10TwoPieceDuration - AM.CastTime),
                        false,
                        false
                    ),
                    TransitionProbability = s.MissileBarrageDuration > 0 ? 1.0f - T8 : 1.0f
                });
            }
            if (ABarAllowed && (!ABarOnCooldownOnly || s.ArcaneBarrageCooldown == 0))
            {
                if (MB > 0)
                {
                    if (AMProc > 0)
                    {
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = ABar,
                            Pause = s.ArcaneBarrageCooldown,
                            TargetState = GetState(
                                true,
                                15.0f - ABar.CastTime,
                                3.0f - ABar.CastTime,
                                0,
                                Math.Max(0.0f, s.Tier10TwoPieceDuration - ABar.CastTime - s.ArcaneBarrageCooldown),
                                s.ArcaneMissilesProcced,
                                true    
                            ),
                            TransitionProbability = MB * AMProc
                        });
                    }
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = ABar,
                        Pause = s.ArcaneBarrageCooldown,
                        TargetState = GetState(
                            true,
                            15.0f - ABar.CastTime,
                            3.0f - ABar.CastTime,
                            0,
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - ABar.CastTime - s.ArcaneBarrageCooldown),
                            s.ArcaneMissilesProcced,
                            s.ArcaneMissilesProcced
                        ),
                        TransitionProbability = MB * (1 - AMProc)
                    });
                }
                if (AMProc > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = ABar,
                        Pause = s.ArcaneBarrageCooldown,
                        TargetState = GetState(
                            s.MissileBarrageDuration > ABar.CastTime,
                            Math.Max(0.0f, s.MissileBarrageDuration - ABar.CastTime),
                            3.0f - ABar.CastTime,
                            0,
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - ABar.CastTime - s.ArcaneBarrageCooldown),
                            s.ArcaneMissilesProcced,
                            true
                        ),
                        TransitionProbability = (1.0f - MB) * AMProc
                    });
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = ABar,
                    Pause = s.ArcaneBarrageCooldown,
                    TargetState = GetState(
                        s.MissileBarrageDuration > ABar.CastTime,
                        Math.Max(0.0f, s.MissileBarrageDuration - ABar.CastTime),
                        3.0f - ABar.CastTime,
                        0,
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - ABar.CastTime - s.ArcaneBarrageCooldown),
                        s.ArcaneMissilesProcced,
                        s.ArcaneMissilesProcced
                    ),
                    TransitionProbability = (1.0f - MB) * (1 - AMProc)
                });
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(bool missileBarrageRegistered, float missileBarrageDuration, float arcaneBarrageCooldown, int arcaneBlastStack, float tier10TwoPieceDuration, bool arcaneMissilesRegistered, bool arcaneMissilesProcced)
        {
            string name;
            if (beta)
            {
                name = string.Format("AB{0},ABar{1},MB{2}{3},AM{5}{6},2T10={4}", arcaneBlastStack, arcaneBarrageCooldown, missileBarrageDuration, missileBarrageRegistered ? "+" : "-", tier10TwoPieceDuration, arcaneMissilesProcced ? "+" : "-", arcaneMissilesRegistered ? "+" : "-");
            }
            else
            {
                name = string.Format("AB{0},ABar{1},MB{2}{3},2T10={4}", arcaneBlastStack, arcaneBarrageCooldown, missileBarrageDuration, missileBarrageRegistered ? "+" : "-", tier10TwoPieceDuration);
            }
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, MissileBarrageRegistered = missileBarrageRegistered, MissileBarrageDuration = missileBarrageDuration, ArcaneBarrageCooldown = arcaneBarrageCooldown, ArcaneBlastStack = arcaneBlastStack, Tier10TwoPieceDuration = tier10TwoPieceDuration, ArcaneMissilesProcced = arcaneMissilesProcced, ArcaneMissilesRegistered = arcaneMissilesRegistered };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (a.ArcaneBlastStack != b.ArcaneBlastStack ||
                (!ABarCooldownCollapsed && a.ArcaneBarrageCooldown != b.ArcaneBarrageCooldown) ||
                ((a.ArcaneBarrageCooldown > 0) != (b.ArcaneBarrageCooldown > 0)) ||
                a.MissileBarrageRegistered != b.MissileBarrageRegistered
                || (!Tier10TwoPieceCollapsed && a.Tier10TwoPieceDuration != b.Tier10TwoPieceDuration)
                || (!MBDurationCollapsed && a.MissileBarrageRegistered == true && b.MissileBarrageRegistered == true && a.MissileBarrageDuration != b.MissileBarrageDuration)
                || a.ArcaneMissilesRegistered != b.ArcaneMissilesRegistered
                /*|| (!AMDurationCollapsed && a.ArcaneMissilesRegistered == true && b.ArcaneMissilesRegistered == true && a.ArcaneMissilesDuration != b.ArcaneMissilesDuration)*/);
        }

        public override string StateDescription
        {
            get
            {
                return @"Cycle Code Legend:
State Descriptions: ABx,ABary,MBz+-,2T10=w
x = number of AB stacks
y = remaining cooldown on Arcane Barrage
z = remaining time on Missile Barrage
+ = Missile Barrage proc visible
- = Missile Barrage proc not visible
w = remaining time on 2T10 effect";
            }
        }
    }

    public class ArcaneCycleGeneratorBeta : CycleGenerator
    {
        private class State : CycleState
        {
            public bool ArcaneMissilesRegistered { get; set; }
            public bool ArcaneMissilesProcced { get; set; }
            public float ArcaneBarrageCooldown { get; set; }
            public int ArcaneBlastStack { get; set; }
        }

        public Spell[] AB;
        public Spell ABar, AM;

        private float AMProc;
        private int maxStack;
        private float channelLatency;

        private bool ABarAllowed;
        private bool ABarOnCooldownOnly;
        private bool ABarCooldownCollapsed;

        public ArcaneCycleGeneratorBeta(CastingState castingState, bool ABarAllowed, bool ABarOnCooldownOnly, bool ABarCooldownCollapsed)
        {
            this.ABarAllowed = ABarAllowed;
            this.ABarOnCooldownOnly = ABarOnCooldownOnly;
            this.ABarCooldownCollapsed = ABarCooldownCollapsed;

            var calc = castingState.Solver;
            maxStack = 4;

            AB = new Spell[maxStack + 1];

            for (int stack = 0; stack <= maxStack; stack++)
            {
                AB[stack] = calc.ArcaneBlastTemplate.GetSpell(castingState, stack);
                AB[stack].Label = "ArcaneBlast" + stack;
            }
            ABar = calc.ArcaneBarrageTemplate.GetSpell(castingState, 0);
            ABar.Label = "ArcaneBarrage";
            AM = calc.ArcaneMissilesTemplate.GetSpell(castingState, false, 0);
            AM.Label = "ArcaneMissiles";


            channelLatency = castingState.CalculationOptions.LatencyChannel;
            AMProc = 0.4f;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(0.0f, 0, false, false);
        }

        protected override List<CycleControlledStateTransition> GetStateTransitions(CycleState state)
        {
            State s = (State)state;
            List<CycleControlledStateTransition> list = new List<CycleControlledStateTransition>();
            Spell AB = null;
            Spell AM = null;
            Spell ABar = null;
            AB = this.AB[s.ArcaneBlastStack];
            ABar = this.ABar;
            if (s.ArcaneMissilesRegistered)
            {
                AM = this.AM;
            }
            if (AMProc > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    TargetState = GetState(Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime), Math.Min(maxStack, s.ArcaneBlastStack + 1), s.ArcaneMissilesProcced, true),
                    TransitionProbability = AMProc
                });
            }
            list.Add(new CycleControlledStateTransition()
            {
                Spell = AB,
                TargetState = GetState(Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime), Math.Min(maxStack, s.ArcaneBlastStack + 1), s.ArcaneMissilesProcced, s.ArcaneMissilesProcced),
                TransitionProbability = (1 - AMProc)
            });
            if (s.ArcaneMissilesRegistered)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AM,
                    TargetState = GetState(Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime), 0, false, false),
                    TransitionProbability = 1.0f
                });
            }
            if (ABarAllowed && (!ABarOnCooldownOnly || s.ArcaneBarrageCooldown == 0.0))
            {
                if (AMProc > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = ABar,
                        Pause = s.ArcaneBarrageCooldown,
                        TargetState = GetState(ABar.Cooldown - ABar.CastTime, 0, true, true),
                        TransitionProbability = AMProc
                    });
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = ABar,
                    Pause = s.ArcaneBarrageCooldown,
                    TargetState = GetState(ABar.Cooldown - ABar.CastTime, 0, s.ArcaneMissilesProcced, s.ArcaneMissilesProcced),
                    TransitionProbability = (1 - AMProc)
                });
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(float arcaneBarrageCooldown, int arcaneBlastStack, bool arcaneMissilesRegistered, bool arcaneMissilesProcced)
        {
            string name;
            name = string.Format("AB{0},ABar{1},AM{2}{3}", arcaneBlastStack, arcaneBarrageCooldown, arcaneMissilesProcced ? "+" : "-", arcaneMissilesRegistered ? "+" : "-");
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, ArcaneBarrageCooldown = arcaneBarrageCooldown, ArcaneBlastStack = arcaneBlastStack, ArcaneMissilesProcced = arcaneMissilesProcced, ArcaneMissilesRegistered = arcaneMissilesRegistered };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (a.ArcaneBlastStack != b.ArcaneBlastStack ||
                (!ABarCooldownCollapsed && a.ArcaneBarrageCooldown != b.ArcaneBarrageCooldown) ||
                ((a.ArcaneBarrageCooldown > 0) != (b.ArcaneBarrageCooldown > 0)) ||
                a.ArcaneMissilesRegistered != b.ArcaneMissilesRegistered);
        }

        public override string StateDescription
        {
            get
            {
                return @"Cycle Code Legend:
State Descriptions: ABx,ABary,AM+-
x = number of AB stacks
y = remaining cooldown on Arcane Barrage
+ = Arcane Missiles proc visible
- = Arcane Missiles proc not visible";
            }
        }
    }

    public class ArcaneAOECycleGenerator : CycleGenerator
    {
        private class State : CycleState
        {
            public bool ArcaneMissilesRegistered { get; set; }
            public bool ArcaneMissilesProcced { get; set; }
            public float ArcaneBarrageCooldown { get; set; }
            public int ArcaneBlastStack { get; set; }
            public float ArcaneBlastDuration { get; set; }
        }

        public Spell[] AB, AE, ABT;
        public Spell ABar, AM, Bliz;

        //private float AMProc;
        private int maxStack;
        private float channelLatency;

        private bool ABarAllowed;
        private bool ABarOnCooldownOnly;
        private bool ABarCooldownCollapsed;
        private CastingState castingState;

        public ArcaneAOECycleGenerator(CastingState castingState, bool ABarAllowed, bool ABarOnCooldownOnly, bool ABarCooldownCollapsed)
        {
            this.castingState = castingState;
            this.ABarAllowed = ABarAllowed;
            this.ABarOnCooldownOnly = ABarOnCooldownOnly;
            this.ABarCooldownCollapsed = ABarCooldownCollapsed;

            var calc = castingState.Solver;
            maxStack = 4;

            AB = new Spell[maxStack + 1];
            ABT = new Spell[maxStack + 1];
            AE = new Spell[maxStack + 1];

            for (int stack = 0; stack <= maxStack; stack++)
            {
                AB[stack] = calc.ArcaneBlastTemplate.GetSpell(castingState, stack);
                AB[stack].Label = "ArcaneBlast" + stack;
                ABT[stack] = calc.ArcaneBlastTemplate.GetSpell(castingState, 0, stack);
                ABT[stack].Label = "ArcaneBlast" + stack + "*";
                AE[stack] = calc.ArcaneExplosionTemplate.GetSpell(castingState, stack);
                AE[stack].Label = "ArcaneExplosion" + stack;
            }
            ABar = calc.ArcaneBarrageTemplate.GetSpell(castingState, 0);
            ABar.Label = "ArcaneBarrage";
            AM = calc.ArcaneMissilesTemplate.GetSpell(castingState, false, 0);
            AM.Label = "ArcaneMissiles";
            Bliz = calc.BlizzardTemplate.GetSpell(castingState);
            Bliz.Label = "Blizzard";


            channelLatency = castingState.CalculationOptions.LatencyChannel;
            //AMProc = 0.4f;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(0.0f, 0, 0.0f, false, false);
        }

        protected override List<CycleControlledStateTransition> GetStateTransitions(CycleState state)
        {
            State s = (State)state;
            List<CycleControlledStateTransition> list = new List<CycleControlledStateTransition>();
            Spell AB = null;
            Spell AM = null;
            Spell ABar = null;
            Spell AE = null;
            AB = this.AB[s.ArcaneBlastStack];
            if (s.ArcaneBlastDuration > 0 && AB.CastTime > s.ArcaneBlastDuration)
            {
                AB = this.ABT[s.ArcaneBlastStack];
            }
            AE = this.AE[s.ArcaneBlastStack];
            ABar = this.ABar;
            //if (s.ArcaneMissilesRegistered)
            {
                AM = this.AM;
            }
            /*if (AMProc > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    TargetState = GetState(
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                        Math.Min(maxStack, AB.CastTime < s.ArcaneBlastDuration ? s.ArcaneBlastStack + 1 : 1),
                        6.0f,
                        s.ArcaneMissilesProcced, 
                        true),
                    TransitionProbability = AMProc
                });
            }*/
            // only use AB on full AB debuff duration or when it would run out if we delay
            //if (s.ArcaneBlastDuration == 0.0f || s.ArcaneBlastDuration == 6.0f || (s.ArcaneBlastDuration - AB.CastTime > 0 && s.ArcaneBlastDuration - AE.CastTime - AB.CastTime < 0))
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    TargetState = GetState(
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                        Math.Min(maxStack, (AB.CastTime < s.ArcaneBlastDuration) ? s.ArcaneBlastStack + 1 : 1),
                        6.0f,
                        s.ArcaneMissilesProcced,
                        s.ArcaneMissilesProcced),
                    TransitionProbability = 1/*(1 - AMProc)*/
                });
            }
            /*if (s.ArcaneMissilesRegistered)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AM,
                    TargetState = GetState(
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                        0,
                        0.0f,
                        false,
                        false),
                    TransitionProbability = 1.0f
                });
            }*/
            if (ABarAllowed && (!ABarOnCooldownOnly || s.ArcaneBarrageCooldown == 0.0))
            {
                /*if (AMProc > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = ABar,
                        Pause = s.ArcaneBarrageCooldown,
                        TargetState = GetState(
                            ABar.Cooldown - ABar.CastTime,
                            0,
                            0.0f,
                            true,
                            true),
                        TransitionProbability = AMProc
                    });
                }*/
                /*list.Add(new CycleControlledStateTransition()
                {
                    Spell = ABar,
                    Pause = s.ArcaneBarrageCooldown,
                    TargetState = GetState(
                        ABar.Cooldown - ABar.CastTime,
                        0,
                        0.0f,
                        s.ArcaneMissilesProcced,
                        s.ArcaneMissilesProcced),
                    TransitionProbability = (1 - AMProc)
                });*/
            }
            /*if (AMProc > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AE,
                    TargetState = GetState(
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AE.CastTime),
                        Math.Min(maxStack, AE.CastTime < s.ArcaneBlastDuration ? s.ArcaneBlastStack : 0),
                        Math.Max(0.0f, s.ArcaneBlastDuration - AE.CastTime),
                        true,
                        true),
                    TransitionProbability = AMProc
                });
            }*/
            list.Add(new CycleControlledStateTransition()
            {
                Spell = AE,
                TargetState = GetState(
                    Math.Max(0.0f, s.ArcaneBarrageCooldown - AE.CastTime),
                    Math.Min(maxStack, AE.CastTime < s.ArcaneBlastDuration ? s.ArcaneBlastStack : 0),
                    Math.Max(0.0f, s.ArcaneBlastDuration - AE.CastTime),
                    s.ArcaneMissilesProcced,
                    s.ArcaneMissilesProcced),
                TransitionProbability = 1/*(1 - AMProc)*/
            });
            if ((s.ArcaneBlastDuration == 0.0 || s.ArcaneBlastDuration == 6.0) && s.ArcaneBlastStack <= 1)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = Bliz,
                    TargetState = GetState(
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - Bliz.CastTime),
                        Math.Min(maxStack, Bliz.CastTime < s.ArcaneBlastDuration ? s.ArcaneBlastStack : 0),
                        Math.Max(0.0f, s.ArcaneBlastDuration - Bliz.CastTime),
                        s.ArcaneMissilesProcced,
                        s.ArcaneMissilesProcced),
                    TransitionProbability = 1
                });
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(float arcaneBarrageCooldown, int arcaneBlastStack, float arcaneBlastDuration, bool arcaneMissilesRegistered, bool arcaneMissilesProcced)
        {
            string name;
            name = string.Format("AB{0}:{4},ABar{1},AM{2}{3}", arcaneBlastStack, arcaneBarrageCooldown, arcaneMissilesProcced ? "+" : "-", arcaneMissilesRegistered ? "+" : "-", arcaneBlastDuration);
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, ArcaneBarrageCooldown = arcaneBarrageCooldown, ArcaneBlastStack = arcaneBlastStack, ArcaneBlastDuration = arcaneBlastDuration, ArcaneMissilesProcced = arcaneMissilesProcced, ArcaneMissilesRegistered = arcaneMissilesRegistered };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (a.ArcaneBlastStack != b.ArcaneBlastStack ||
                (!ABarCooldownCollapsed && a.ArcaneBarrageCooldown != b.ArcaneBarrageCooldown) ||
                ((a.ArcaneBarrageCooldown > 0) != (b.ArcaneBarrageCooldown > 0)) ||
                a.ArcaneMissilesRegistered != b.ArcaneMissilesRegistered ||
                a.ArcaneBlastDuration != b.ArcaneBlastDuration);
        }

        public override string StateDescription
        {
            get
            {
                return @"Cycle Code Legend:
State Descriptions: ABx:d,ABary,AM+-
x = number of AB stacks
d = duration on AB stack
y = remaining cooldown on Arcane Barrage
+ = Arcane Missiles proc visible
- = Arcane Missiles proc not visible";
            }
        }
    }

    public class ArcaneMovementCycleGenerator : CycleGenerator
    {
        private class State : CycleState
        {
            public bool MissileBarrageRegistered { get; set; }
            public float MissileBarrageDuration { get; set; }
            public float ArcaneBarrageCooldown { get; set; }
            public int ArcaneBlastStack { get; set; }
            public float Tier10TwoPieceDuration { get; set; }
            public float MovementDuration { get; set; }
        }

        public Spell[,] AB, ABar, AM, MBAM;

        private float ABMB;
        private float MB;
        private float T8;
        private int maxStack;
        private bool T10;
        private float channelLatency;

        private bool ABarAllowed;
        private bool ABarOnCooldownOnly;
        private bool MBDurationCollapsed;
        private bool ABarCooldownCollapsed;
        private bool Tier10TwoPieceCollapsed;

        private float movementEventRate;
        private float movementDuration;
        private bool instantsOnMovementOnly;

        public ArcaneMovementCycleGenerator(CastingState castingState, float movementEventRate, float movementDuration, bool ABarAllowed, bool ABarOnCooldownOnly, bool instantsOnMovementOnly, bool MBDurationCollapsed, bool ABarCooldownCollapsed, bool Tier10TwoPieceCollapsed)
        {
            this.ABarAllowed = ABarAllowed;
            this.ABarOnCooldownOnly = ABarOnCooldownOnly;
            this.MBDurationCollapsed = MBDurationCollapsed;
            this.ABarCooldownCollapsed = ABarCooldownCollapsed;
            this.Tier10TwoPieceCollapsed = Tier10TwoPieceCollapsed;
            this.movementEventRate = movementEventRate;
            this.movementDuration = movementDuration;
            this.instantsOnMovementOnly = instantsOnMovementOnly;

            var calc = castingState.Solver;
            maxStack = 4;

            AB = new Spell[maxStack + 1, 2];
            ABar = new Spell[maxStack + 1, 2];
            AM = new Spell[maxStack + 1, 2];
            MBAM = new Spell[maxStack + 1, 2];

            for (int stack = 0; stack <= maxStack; stack++)
            {
                AB[stack, 0] = calc.ArcaneBlastTemplate.GetSpell(castingState, stack);
                AB[stack, 0].Label = "ArcaneBlast" + stack;
                ABar[stack, 0] = calc.ArcaneBarrageTemplate.GetSpell(castingState, stack);
                ABar[stack, 0].Label = "ArcaneBarrage" + stack;
                AM[stack, 0] = calc.ArcaneMissilesTemplate.GetSpell(castingState, false, stack);
                AM[stack, 0].Label = "ArcaneMissiles" + stack;
                MBAM[stack, 0] = calc.ArcaneMissilesTemplate.GetSpell(castingState, true, stack);
                MBAM[stack, 0].Label = "MBAM" + stack;
                AB[stack, 1] = calc.ArcaneBlastTemplate.GetSpell(castingState.Tier10TwoPieceState, stack);
                AB[stack, 1].Label = "2T10:ArcaneBlast" + stack;
                ABar[stack, 1] = calc.ArcaneBarrageTemplate.GetSpell(castingState.Tier10TwoPieceState, stack);
                ABar[stack, 1].Label = "2T10:ArcaneBarrage" + stack;
                AM[stack, 1] = calc.ArcaneMissilesTemplate.GetSpell(castingState.Tier10TwoPieceState, false, stack);
                AM[stack, 1].Label = "2T10:ArcaneMissiles" + stack;
                MBAM[stack, 1] = calc.ArcaneMissilesTemplate.GetSpell(castingState.Tier10TwoPieceState, true, stack);
                MBAM[stack, 1].Label = "2T10:MBAM" + stack;
            }

            ABMB = 0.08f * castingState.MageTalents.MissileBarrage;
            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            T8 = 0;
            T10 = castingState.Solver.Mage2T10;
            channelLatency = castingState.CalculationOptions.LatencyChannel;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(false, 0.0f, 0.0f, 0, 0.0f, 0.0f);
        }

        protected override List<CycleControlledStateTransition> GetStateTransitions(CycleState state)
        {
            State s = (State)state;
            List<CycleControlledStateTransition> list = new List<CycleControlledStateTransition>();
            Spell AB = null;
            Spell AM = null;
            Spell ABar = null;
            AB = this.AB[s.ArcaneBlastStack, s.Tier10TwoPieceDuration > 0 ? 1 : 0];
            ABar = this.ABar[s.ArcaneBlastStack, s.Tier10TwoPieceDuration - s.ArcaneBarrageCooldown > 0 ? 1 : 0];
            if (s.MissileBarrageDuration > 0)
            {
                AM = this.MBAM[s.ArcaneBlastStack, s.Tier10TwoPieceDuration > 0 ? 1 : 0];
            }
            else
            {
                AM = this.AM[s.ArcaneBlastStack, s.Tier10TwoPieceDuration > 0 ? 1 : 0];
            }
            if (s.MovementDuration == 0.0f)
            {
                float move = (float)(1.0 - Math.Exp(-movementEventRate * AB.CastTime));
                if (s.ArcaneBarrageCooldown > AB.CastTime)
                {
                    move = 0.0f;
                }
                if (MB > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = AB,
                        TargetState = GetState(
                            s.MissileBarrageDuration > AB.CastTime,
                            15.0f,
                            Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                            Math.Min(maxStack, s.ArcaneBlastStack + 1),
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - AB.CastTime),
                            0.0f
                        ),
                        TransitionProbability = ABMB * (1.0f - move)
                    });
                    if (move > 0)
                    {
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = AB,
                            TargetState = GetState(
                                s.MissileBarrageDuration > AB.CastTime,
                                15.0f,
                                Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                                Math.Min(maxStack, s.ArcaneBlastStack + 1),
                                Math.Max(0.0f, s.Tier10TwoPieceDuration - AB.CastTime),
                                movementDuration
                            ),
                            TransitionProbability = ABMB * move
                        });
                    }
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    TargetState = GetState(
                        s.MissileBarrageDuration > AB.CastTime,
                        Math.Max(0.0f, s.MissileBarrageDuration - AB.CastTime),
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                        Math.Min(maxStack, s.ArcaneBlastStack + 1),
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - AB.CastTime),
                        0.0f
                    ),
                    TransitionProbability = (1.0f - ABMB) * (1.0f - move)
                });
                if (move > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = AB,
                        TargetState = GetState(
                            s.MissileBarrageDuration > AB.CastTime,
                            Math.Max(0.0f, s.MissileBarrageDuration - AB.CastTime),
                            Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                            Math.Min(maxStack, s.ArcaneBlastStack + 1),
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - AB.CastTime),
                            movementDuration
                        ),
                        TransitionProbability = (1.0f - ABMB) * move
                    });
                }
                move = (float)(1.0 - Math.Exp(-movementEventRate * AM.CastTime));
                if (s.ArcaneBarrageCooldown > AM.CastTime)
                {
                    move = 0.0f;
                }
                if (s.MissileBarrageDuration > 0 && T8 > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = AM,
                        TargetState = GetState(
                            s.MissileBarrageDuration > AM.CastTime,
                            Math.Max(0.0f, s.MissileBarrageDuration - AM.CastTime),
                            Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                            0,
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - AM.CastTime), // cannot have 4T8 and 2T10 at the same time
                            0.0f
                        ),
                        TransitionProbability = T8 * (1.0f - move)
                    });
                    if (move > 0)
                    {
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = AM,
                            TargetState = GetState(
                                s.MissileBarrageDuration > AM.CastTime,
                                Math.Max(0.0f, s.MissileBarrageDuration - AM.CastTime),
                                Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                                0,
                                Math.Max(0.0f, s.Tier10TwoPieceDuration - AM.CastTime), // cannot have 4T8 and 2T10 at the same time
                                movementDuration
                            ),
                            TransitionProbability = T8 * move
                        });
                    }
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AM,
                    TargetState = GetState(
                        false,
                        0.0f,
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                        0,
                        Math.Max(0.0f, (s.MissileBarrageDuration > 0 && T10) ? 5.0f - channelLatency : s.Tier10TwoPieceDuration - AM.CastTime),
                        0.0f
                    ),
                    TransitionProbability = (s.MissileBarrageDuration > 0 ? 1.0f - T8 : 1.0f) * (1.0f - move)
                });
                if (move > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = AM,
                        TargetState = GetState(
                            false,
                            0.0f,
                            Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                            0,
                            Math.Max(0.0f, (s.MissileBarrageDuration > 0 && T10) ? 5.0f - channelLatency : s.Tier10TwoPieceDuration - AM.CastTime),
                            movementDuration
                        ),
                        TransitionProbability = (s.MissileBarrageDuration > 0 ? 1.0f - T8 : 1.0f) * move
                    });
                }
            }
            if (ABarAllowed && (!ABarOnCooldownOnly || s.ArcaneBarrageCooldown == 0) && (s.MovementDuration > 0 || !instantsOnMovementOnly))
            {
                float move = (float)(1.0 - Math.Exp(-movementEventRate * ABar.CastTime));
                move = 0.0f;
                if (MB > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = ABar,
                        Pause = s.ArcaneBarrageCooldown,
                        TargetState = GetState(
                            true,
                            15.0f - ABar.CastTime,
                            3.0f - ABar.CastTime,
                            0,
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - ABar.CastTime - s.ArcaneBarrageCooldown),
                            Math.Max(0.0f, s.MovementDuration - ABar.CastTime - s.ArcaneBarrageCooldown)
                        ),
                        TransitionProbability = MB * (1.0f - move)
                    });
                    /*list.Add(new CycleControlledStateTransition()
                    {
                        Spell = ABar,
                        Pause = s.ArcaneBarrageCooldown,
                        TargetState = GetState(
                            true,
                            15.0f - ABar.CastTime,
                            3.0f - ABar.CastTime,
                            0,
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - ABar.CastTime - s.ArcaneBarrageCooldown),
                            movementDuration
                        ),
                        TransitionProbability = MB * move
                    });*/
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = ABar,
                    Pause = s.ArcaneBarrageCooldown,
                    TargetState = GetState(
                        s.MissileBarrageDuration > ABar.CastTime,
                        Math.Max(0.0f, s.MissileBarrageDuration - ABar.CastTime),
                        3.0f - ABar.CastTime,
                        0,
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - ABar.CastTime - s.ArcaneBarrageCooldown),
                        Math.Max(0.0f, s.MovementDuration - ABar.CastTime - s.ArcaneBarrageCooldown)
                    ),
                    TransitionProbability = (1.0f - MB) * (1.0f - move)
                });
                /*list.Add(new CycleControlledStateTransition()
                {
                    Spell = ABar,
                    Pause = s.ArcaneBarrageCooldown,
                    TargetState = GetState(
                        s.MissileBarrageDuration > ABar.CastTime,
                        Math.Max(0.0f, s.MissileBarrageDuration - ABar.CastTime),
                        3.0f - ABar.CastTime,
                        0,
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - ABar.CastTime - s.ArcaneBarrageCooldown),
                        movementDuration
                    ),
                    TransitionProbability = (1.0f - MB) * move
                });*/
            }
            if (s.MovementDuration > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = null,
                    Pause = s.MovementDuration,
                    TargetState = GetState(
                        s.MissileBarrageDuration > s.MovementDuration,
                        Math.Max(0.0f, s.MissileBarrageDuration - s.MovementDuration),
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - s.MovementDuration),
                        s.ArcaneBlastStack,
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - s.MovementDuration),
                        0.0f
                    ),
                    TransitionProbability = 1.0f
                });
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(bool missileBarrageRegistered, float missileBarrageDuration, float arcaneBarrageCooldown, int arcaneBlastStack, float tier10TwoPieceDuration, float movementDuration)
        {
            string name = string.Format("AB{0},ABar{1},MB{2}{3},2T10={4},Move={5}", arcaneBlastStack, arcaneBarrageCooldown, missileBarrageDuration, missileBarrageRegistered ? "+" : "-", tier10TwoPieceDuration, movementDuration);
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, MissileBarrageRegistered = missileBarrageRegistered, MissileBarrageDuration = missileBarrageDuration, ArcaneBarrageCooldown = arcaneBarrageCooldown, ArcaneBlastStack = arcaneBlastStack, Tier10TwoPieceDuration = tier10TwoPieceDuration, MovementDuration = movementDuration };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (a.ArcaneBlastStack != b.ArcaneBlastStack ||
                (!ABarCooldownCollapsed && a.ArcaneBarrageCooldown != b.ArcaneBarrageCooldown) ||
                (((a.ArcaneBarrageCooldown > 0) != (b.ArcaneBarrageCooldown > 0)) && (a.MovementDuration > 0 || !instantsOnMovementOnly)) ||
                a.MissileBarrageRegistered != b.MissileBarrageRegistered
                || (!Tier10TwoPieceCollapsed && a.Tier10TwoPieceDuration != b.Tier10TwoPieceDuration)
                || (!MBDurationCollapsed && a.MissileBarrageRegistered == true && b.MissileBarrageRegistered == true && a.MissileBarrageDuration != b.MissileBarrageDuration)
                || (a.MovementDuration != b.MovementDuration));
        }

        public override string StateDescription
        {
            get
            {
                return @"State Descriptions: ABx,ABary,MBz+-,2T10=w,Move=m
x = number of AB stacks
y = remaining cooldown on Arcane Barrage
z = remaining time on Missile Barrage
+ = Missile Barrage proc visible
- = Missile Barrage proc not visible
w = remaining time on 2T10 effect
m = remaining movement time";
            }
        }
    }

    public class ArcaneInterruptMovementCycleGenerator : CycleGenerator
    {
        private class State : CycleState
        {
            public bool MissileBarrageRegistered { get; set; }
            public float MissileBarrageDuration { get; set; }
            public float ArcaneBarrageCooldown { get; set; }
            public int ArcaneBlastStack { get; set; }
            public float ArcaneBlastDuration { get; set; }
            public float MovementDuration { get; set; }
        }

        public Spell AB0, AB1, AB2, AB3, AB4, ABar0, ABar1, ABar2, ABar3, ABar4, AM0, AM1, AM2, AM3, AM4, MBAM0, MBAM1, MBAM2, MBAM3, MBAM4;
        public Spell[,] AMT = new Spell[5, 5];
        public Spell[,] MBAMT = new Spell[5, 5];
        public Spell ABNull, FB;

        private float ABMB;
        private float MB;
        private float T8;
        private int maxStack;

        private float movementEventRate;
        private float movementDuration;
        private CastingState castingState;

        private float ABI;
        private float ABI2;
        private float ABI0;
        private float ABarI;
        private float ABarI0;
        private float[] AMI = new float[6];
        private float[] MBAMI = new float[6];
        private float AMI0, MBAMI0;

        public ArcaneInterruptMovementCycleGenerator(CastingState castingState, float movementEventRate, float movementDuration)
        {
            this.castingState = castingState;

            AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
            AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);
            ABar0 = castingState.GetSpell(SpellId.ArcaneBarrage);
            ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            ABar4 = castingState.GetSpell(SpellId.ArcaneBarrage4);
            AM0 = castingState.GetSpell(SpellId.ArcaneMissiles);
            AM1 = castingState.GetSpell(SpellId.ArcaneMissiles1);
            AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
            AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            AM4 = castingState.GetSpell(SpellId.ArcaneMissiles4);
            MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);
            MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);

            for (int ab = 0; ab < 5; ab++)
            {
                for (int t = 0; t < 5; t++)
                {
                    AMT[ab, t] = castingState.Solver.ArcaneMissilesTemplate.GetSpell(castingState, false, ab);
                    MBAMT[ab, t] = castingState.Solver.ArcaneMissilesTemplate.GetSpell(castingState, true, ab);
                }
            }
            ABNull = new Spell(castingState.Solver.ArcaneBlastTemplate);
            FB = castingState.GetSpell(SpellId.FireBlast);

            this.movementEventRate = movementEventRate;
            this.movementDuration = movementDuration;

            ABI = (float)(1.0 - Math.Exp(-movementEventRate * (AB0.CastTime - castingState.CalculationOptions.LatencyCast)));
            ABI2 = (float)((1 - ABI) * (1.0 - Math.Exp(-movementEventRate * castingState.CalculationOptions.LatencyCast)));
            ABI0 = (float)Math.Exp(-movementEventRate * AB0.CastTime);

            ABarI = (float)(1.0 - Math.Exp(-movementEventRate * ABar0.CastTime));
            ABarI0 = (float)Math.Exp(-movementEventRate * ABar0.CastTime);

            AMI[0] = (float)(1.0 - Math.Exp(-movementEventRate * (AM0.CastTime - castingState.CalculationOptions.LatencyChannel) / 5.0));
            MBAMI[0] = (float)(1.0 - Math.Exp(-movementEventRate * (MBAM0.CastTime - castingState.CalculationOptions.LatencyChannel) / 5.0));
            for (int t = 1; t < 5; t++)
            {
                AMI[t] = (float)(Math.Exp(-movementEventRate * (AM0.CastTime - castingState.CalculationOptions.LatencyChannel) * t / 5.0) * (1.0 - Math.Exp(-movementEventRate * (AM0.CastTime - castingState.CalculationOptions.LatencyChannel) / 5.0)));
                MBAMI[t] = (float)(Math.Exp(-movementEventRate * (MBAM0.CastTime - castingState.CalculationOptions.LatencyChannel) * t / 5.0) * (1.0 - Math.Exp(-movementEventRate * (MBAM0.CastTime - castingState.CalculationOptions.LatencyChannel) / 5.0)));
            }
            AMI[5] = (float)(Math.Exp(-movementEventRate * (AM0.CastTime - castingState.CalculationOptions.LatencyChannel)) * (1.0 - Math.Exp(-movementEventRate * castingState.CalculationOptions.LatencyChannel)));
            MBAMI[5] = (float)(Math.Exp(-movementEventRate * (AM0.CastTime - castingState.CalculationOptions.LatencyChannel)) * (1.0 - Math.Exp(-movementEventRate * castingState.CalculationOptions.LatencyChannel)));
            AMI0 = (float)Math.Exp(-movementEventRate * AM0.CastTime);
            MBAMI0 = (float)Math.Exp(-movementEventRate * MBAM0.CastTime);

            ABMB = 0.08f * castingState.MageTalents.MissileBarrage;
            maxStack = 4;
            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            T8 = 0;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(false, 0.0f, 0.0f, 0, 0.0f, 0.0f);
        }

        protected override List<CycleControlledStateTransition> GetStateTransitions(CycleState state)
        {
            State s = (State)state;
            List<CycleControlledStateTransition> list = new List<CycleControlledStateTransition>();
            Spell AB = null;
            Spell AM = null;
            Spell ABar = null;
            switch (s.ArcaneBlastStack)
            {
                case 0:
                    AB = AB0;
                    ABar = ABar0;
                    AM = (s.MissileBarrageDuration > 0) ? MBAM0 : AM0;
                    break;
                case 1:
                    AB = AB1;
                    ABar = ABar1;
                    AM = (s.MissileBarrageDuration > 0) ? MBAM1 : AM1;
                    break;
                case 2:
                    AB = AB2;
                    ABar = ABar2;
                    AM = (s.MissileBarrageDuration > 0) ? MBAM2 : AM2;
                    break;
                case 3:
                    AB = AB3;
                    ABar = ABar3;
                    AM = (s.MissileBarrageDuration > 0) ? MBAM3 : AM3;
                    break;
                case 4:
                    AB = AB4;
                    ABar = ABar4;
                    AM = (s.MissileBarrageDuration > 0) ? MBAM4 : AM4;
                    break;
            }
            if (s.MovementDuration == 0.0f)
            {
                float d = (AB.CastTime - castingState.CalculationOptions.LatencyCast) / 2.0f;
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = ABNull,
                    Pause = d,
                    TargetState = GetState(
                        s.MissileBarrageDuration > d,
                        Math.Max(0.0f, s.MissileBarrageDuration - d),
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - d),
                        s.ArcaneBlastDuration - d > 0 ? s.ArcaneBlastStack : 0,
                        Math.Max(s.ArcaneBlastDuration - d, 0.0f),
                        movementDuration
                    ),
                    TransitionProbability = ABI
                });
                d = AB.CastTime - castingState.CalculationOptions.LatencyCast / 2.0f;
                if (MB > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = AB,
                        Pause = -castingState.CalculationOptions.LatencyCast / 2.0f,
                        TargetState = GetState(
                            s.MissileBarrageDuration > AB.CastTime,
                            15.0f,
                            Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                            Math.Min(maxStack, s.ArcaneBlastStack + 1),
                            6.0f,
                            movementDuration
                        ),
                        TransitionProbability = ABI2 * ABMB
                    });
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = AB,
                        TargetState = GetState(
                            s.MissileBarrageDuration > AB.CastTime,
                            15.0f,
                            Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                            Math.Min(maxStack, s.ArcaneBlastStack + 1),
                            6.0f,
                            0.0f
                        ),
                        TransitionProbability = ABI0 * ABMB
                    });
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    Pause = -castingState.CalculationOptions.LatencyCast / 2.0f,
                    TargetState = GetState(
                        s.MissileBarrageDuration > AB.CastTime,
                        Math.Max(0.0f, s.MissileBarrageDuration - AB.CastTime),
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                        Math.Min(maxStack, s.ArcaneBlastStack + 1),
                        6.0f,
                        movementDuration
                    ),
                    TransitionProbability = ABI2 * (1.0f - ABMB)
                });
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    TargetState = GetState(
                        s.MissileBarrageDuration > AB.CastTime,
                        Math.Max(0.0f, s.MissileBarrageDuration - AB.CastTime),
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                        Math.Min(maxStack, s.ArcaneBlastStack + 1),
                        6.0f,
                        0.0f
                    ),
                    TransitionProbability = ABI0 * (1.0f - ABMB)
                });
                //if (s.ArcaneBarrageCooldown == 0)
                {
                    float p = (float)(1.0 - Math.Exp(-movementEventRate * (ABar0.CastTime + s.ArcaneBarrageCooldown)));
                    if (MB > 0)
                    {
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = ABar,
                            Pause = s.ArcaneBarrageCooldown,
                            TargetState = GetState(
                                true,
                                15.0f - ABar.CastTime,
                                3.0f - ABar.CastTime,
                                0,
                                0.0f,
                                movementDuration - (ABar.CastTime + s.ArcaneBarrageCooldown) / 2.0f
                            ),
                            TransitionProbability = p * MB
                        });
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = ABar,
                            Pause = s.ArcaneBarrageCooldown,
                            TargetState = GetState(
                                true,
                                15.0f - ABar.CastTime,
                                3.0f - ABar.CastTime,
                                0,
                                0.0f,
                                0.0f
                            ),
                            TransitionProbability = (1 - p) * MB
                        });
                    }
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = ABar,
                        Pause = s.ArcaneBarrageCooldown,
                        TargetState = GetState(
                            s.MissileBarrageDuration > ABar.CastTime,
                            Math.Max(0.0f, s.MissileBarrageDuration - ABar.CastTime),
                            3.0f - ABar.CastTime,
                            0,
                            0.0f,
                            movementDuration - (ABar.CastTime + s.ArcaneBarrageCooldown) / 2.0f
                        ),
                        TransitionProbability = p * (1.0f - MB)
                    });
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = ABar,
                        Pause = s.ArcaneBarrageCooldown,
                        TargetState = GetState(
                            s.MissileBarrageDuration > ABar.CastTime,
                            Math.Max(0.0f, s.MissileBarrageDuration - ABar.CastTime),
                            3.0f - ABar.CastTime,
                            0,
                            0.0f,
                            0.0f
                        ),
                        TransitionProbability = (1 - p) * (1.0f - MB)
                    });
                }
                float[] I = (s.MissileBarrageDuration > 0) ? MBAMI : AMI;
                Spell[,] T = (s.MissileBarrageDuration > 0) ? MBAMT : AMT;
                for (int t = 0; t < 5; t++)
                {
                    d = (t + 0.5f) * (AM.CastTime - castingState.CalculationOptions.LatencyChannel) / 5.0f;
                    if (s.MissileBarrageDuration > 0 && T8 > 0)
                    {
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = T[s.ArcaneBlastStack, t],
                            Pause = -castingState.CalculationOptions.LatencyChannel + (AM.CastTime - castingState.CalculationOptions.LatencyChannel) / 10.0f,
                            TargetState = GetState(
                                s.MissileBarrageDuration > d,
                                Math.Max(0.0f, s.MissileBarrageDuration - d),
                                Math.Max(0.0f, s.ArcaneBarrageCooldown - d),
                                0,
                                0.0f,
                                movementDuration
                            ),
                            TransitionProbability = I[t] * T8
                        });
                    }
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = T[s.ArcaneBlastStack, t],
                        Pause = -castingState.CalculationOptions.LatencyChannel + (AM.CastTime - castingState.CalculationOptions.LatencyChannel) / 10.0f,
                        TargetState = GetState(
                            false,
                            0.0f,
                            Math.Max(0.0f, s.ArcaneBarrageCooldown - d),
                            0,
                            0.0f,
                            movementDuration
                        ),
                        TransitionProbability = I[t] * (s.MissileBarrageDuration > 0 ? 1.0f - T8 : 1.0f)
                    });
                }
                d = AM.CastTime - castingState.CalculationOptions.LatencyChannel / 2.0f;
                if (s.MissileBarrageDuration > 0 && T8 > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = AM,
                        Pause = -castingState.CalculationOptions.LatencyChannel / 2,
                        TargetState = GetState(
                            s.MissileBarrageDuration > d,
                            Math.Max(0.0f, s.MissileBarrageDuration - d),
                            Math.Max(0.0f, s.ArcaneBarrageCooldown - d),
                            0,
                            0.0f,
                            movementDuration
                        ),
                        TransitionProbability = I[5] * T8
                    });
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AM,
                    Pause = -castingState.CalculationOptions.LatencyChannel / 2,
                    TargetState = GetState(
                        false,
                        0.0f,
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - d),
                        0,
                        0.0f,
                        movementDuration
                    ),
                    TransitionProbability = I[5] * (s.MissileBarrageDuration > 0 ? 1.0f - T8 : 1.0f)
                });
                if (s.MissileBarrageDuration > 0 && T8 > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = AM,
                        TargetState = GetState(
                            s.MissileBarrageDuration > AM.CastTime,
                            Math.Max(0.0f, s.MissileBarrageDuration - AM.CastTime),
                            Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                            0,
                            0.0f,
                            0.0f
                        ),
                        TransitionProbability = T8
                    });
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AM,
                    TargetState = GetState(
                        false,
                        0.0f,
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                        0,
                        0.0f,
                        0.0f
                    ),
                    TransitionProbability = s.MissileBarrageDuration > 0 ? 1.0f - T8 : 1.0f
                });
            }
            else
            {
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(bool missileBarrageRegistered, float missileBarrageDuration, float arcaneBarrageCooldown, int arcaneBlastStack, float arcaneBlastDuration, float movementDuration)
        {
            string name = string.Format("AB{0}({4}),ABar{1},MB{2}{3},M{5}", arcaneBlastStack, arcaneBarrageCooldown, missileBarrageDuration, missileBarrageRegistered ? "+" : "-", arcaneBlastDuration, movementDuration);
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, MissileBarrageRegistered = missileBarrageRegistered, MissileBarrageDuration = missileBarrageDuration, ArcaneBarrageCooldown = arcaneBarrageCooldown, ArcaneBlastStack = arcaneBlastStack, ArcaneBlastDuration = arcaneBlastDuration, MovementDuration = movementDuration };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (a.ArcaneBlastStack != b.ArcaneBlastStack || a.ArcaneBarrageCooldown != b.ArcaneBarrageCooldown || a.MissileBarrageRegistered != b.MissileBarrageRegistered || a.ArcaneBlastDuration != b.ArcaneBlastDuration || a.MovementDuration != b.MovementDuration /* || (a.MissileBarrageRegistered == true && b.MissileBarrageRegistered == true && a.MissileBarrageDuration != b.MissileBarrageDuration)*/);
        }

        public override string StateDescription
        {
            get
            {
                return @"Cycle Code Legend: 0 = AB, 1 = ABar, 2 = AM
State Descriptions: ABx,ABary,MBz+-
x = number of AB stacks
y = remaining cooldown on Arcane Barrage
z = remaining time on Missile Barrage
+ = Missile Barrage proc visible
- = Missile Barrage proc not visible";
            }
        }
    }
}
