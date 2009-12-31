using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public static class ABAM
    {
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "ABAM";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB1);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

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
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, 1, 1, 1, 0);
            }
            cycle.AddSpell(needsDisplayCalculations, AM3, S0 * K1);
            cycle.AddSpell(needsDisplayCalculations, MBAM3, 1 - S0 * K1);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB4AM234MBAM
    {
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, 1, 1, K1 + K2 + K3, K1 + K2);
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
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB4AM0234MBAM";

            if (castingState.BaseStats.Mage2T10 > 0)
            {
                // doesn't support haste transferring over several loops

                // S0A-nonhasted:
                // AB0-AB1-AB2-AB3-AM4    => S1    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-AB3-MBAM4   => S0AB   (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
                // AB0-AB1-AB2-MBAM3   => S0AB       (1 - MB) * MB
                // AB0-AB1-MBAM2   => S0AB           MB
                // S0B-hasted:
                // AB0-AB1-AB2-AB3-AM4    => S1    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-AB3-MBAM4   => S0AB   (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
                // AB0-AB1-AB2-MBAM3   => S0AB       (1 - MB) * MB
                // AB0-AB1-MBAM2   => S0AB           MB
                // S1-no 2T10:
                // AB0-AB1-AB2-AB3-AM4    => S1    (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-AB3-MBAM4   => S0A   (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
                // AB0-AB1-AB2-MBAM3   => S0A       (1 - MB) * MB
                // AB0-AB1-MBAM2   => S0A           MB

                // K0 := (1-MB)*(1-MB)*(1-MB)*(1-MB)

                // MC := 1-MB

                // K2 := (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
                // K3 := (1 - MB) * MB

                // S0A = S0A * (K2 * X2A + K3 * X3A + MB * X4A) + S0B * (K2 * X2B + K3 * X3B + MB * X4B) + S1 * (1 - K0)
                // S0B = S0A * (K2 * Y2A + K3 * Y3A + MB * Y4A) + S0B * (K2 * Y2B + K3 * Y3B + MB * Y4B)
                // S0AB = 1 - K0
                // S1 = K0

                // S0B = 1 - K0 - S0A

                // S0A * (1 - (K2 * (X2A-X2B) + K3 * (X3A-X3B) + MB * (X4A-X4B))) = (1 - K0) * (K2 * X2B + K3 * X3B + MB * X4B) + K0 * (1 - K0)
                // S0A = ((1 - K0) * (K2 * X2B + K3 * X3B + MB * X4B) + K0 * (1 - K0)) / (1 - (K2 * (X2A-X2B) + K3 * (X3A-X3B) + MB * (X4A-X4B)))


                Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                Spell AM4 = castingState.GetSpell(SpellId.ArcaneMissiles4);
                Spell MBAM2 = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2);

                float m2T10time = 5.0f - MBAM4.CastTime;
                Spell AB0A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0A.CastTime;
                Spell AB1A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1A.CastTime;
                float X4A = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2A.CastTime;
                float X3A = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB3) : castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AB3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                m2T10time -= AB3A.CastTime;
                float X2A = (m2T10time > 0.0f) ? 0 : 1;
                float X1A = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM4A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB4) : castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                Spell AM4A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles4) : castingState.GetSpell(SpellId.ArcaneMissiles4);

                m2T10time = 5.0f - MBAM2.CastTime;
                Spell AB0B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0B.CastTime;
                Spell AB1B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1B.CastTime;
                float X4B = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM2B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2B.CastTime;
                float X3B = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM3B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB3) : castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AB3B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                m2T10time -= AB3B.CastTime;
                float X2B = (m2T10time > 0.0f) ? 0 : 1;
                float X1B = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM4B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB4) : castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                Spell AM4B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles4) : castingState.GetSpell(SpellId.ArcaneMissiles4);

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

                float S0A = ((1 - K0) * (K2 * X2B + K3 * X3B + MB * X4B) + K0 * (1 - K0)) / (1 - (K2 * (X2A - X2B) + K3 * (X3A - X3B) + MB * (X4A - X4B)));
                float S0B = 1 - K0 - S0A;
                float S1 = K0;

                cycle.AddSpell(needsDisplayCalculations, AB0A, S0A);
                cycle.AddSpell(needsDisplayCalculations, AB1A, S0A);
                cycle.AddSpell(needsDisplayCalculations, AB2A, S0A * MC);
                cycle.AddSpell(needsDisplayCalculations, AB3A, S0A * MC * MC);
                cycle.AddSpell(needsDisplayCalculations, MBAM2A, S0A * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM3A, S0A * K3);
                cycle.AddSpell(needsDisplayCalculations, MBAM4A, S0A * K2);
                cycle.AddSpell(needsDisplayCalculations, AM4A, S0A * K0);

                cycle.AddSpell(needsDisplayCalculations, AB0B, S0B);
                cycle.AddSpell(needsDisplayCalculations, AB1B, S0B);
                cycle.AddSpell(needsDisplayCalculations, AB2B, S0B * MC);
                cycle.AddSpell(needsDisplayCalculations, AB3B, S0B * MC * MC);
                cycle.AddSpell(needsDisplayCalculations, MBAM2B, S0B * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM3B, S0B * K3);
                cycle.AddSpell(needsDisplayCalculations, MBAM4B, S0B * K2);
                cycle.AddSpell(needsDisplayCalculations, AM4B, S0B * K0);

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
                float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                    castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, S0, S0, K1 + K2 + K3, K1 + K2);
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
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, 1, 1, K1 + K2, 0);
            }
            cycle.AddSpell(needsDisplayCalculations, AM3, K1);
            cycle.AddSpell(needsDisplayCalculations, MBAM3, K2);
            cycle.AddSpell(needsDisplayCalculations, MBAM2, K4);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class AB3AM023MBAM
    {
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB3AM023MBAM";

            if (castingState.BaseStats.Mage2T10 > 0)
            {
                // doesn't support haste transferring over several loops

                // S0A-nonhasted:
                // AB0-AB1-AB2-AM3    => S1    (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-MBAM3   => S0AB   (1 - MB) * (1 - (1 - MB) * (1 - MB))
                // AB0-AB1-MBAM2   => S0AB       MB
                // S0B-hasted:
                // AB0-AB1-AB2-AM3    => S1    (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-MBAM3   => S0AB   (1 - MB) * (1 - (1 - MB) * (1 - MB))
                // AB0-AB1-MBAM2   => S0AB       MB
                // S1-no 2T10:
                // AB0-AB1-AB2-AM3    => S1    (1 - MB) * (1 - MB) * (1 - MB)
                // AB0-AB1-AB2-MBAM3   => S0A   (1 - MB) * (1 - (1 - MB) * (1 - MB))
                // AB0-AB1-MBAM2   => S0A       MB

                // K0 := (1-MB)*(1-MB)*(1-MB)

                // MC := 1-MB

                // K2 := (1 - MB) * (1 - (1 - MB) * (1 - MB))

                // S0A = S0A * (K2 * X2A + MB * X3A) + S0B * (K2 * X2B + MB * X3B) + S1 * (1 - K0)
                // S0B = S0A * (K2 * Y2A + MB * Y3A) + S0B * (K2 * Y2B + MB * Y3B)
                // S0AB = 1 - K0
                // S1 = K0

                // S0B = 1 - K0 - S0A

                // S0A = S0A * (K2 * (X2A-X2B) + MB * (X3A-X3B)) + (1 - K0) * (K2 * X2B + MB * X3B) + S1 * (1 - K0)
                // S0A * (1 - (K2 * (X2A-X2B) + MB * (X3A-X3B))) = (1 - K0) * (K2 * X2B + MB * X3B) + K0 * (1 - K0)
                // S0A = ((1 - K0) * (K2 * X2B + MB * X3B) + K0 * (1 - K0)) / (1 - (K2 * (X2A-X2B) + MB * (X3A-X3B)))


                Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
                Spell MBAM2 = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2);

                float m2T10time = 5.0f - MBAM3.CastTime;
                Spell AB0A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0A.CastTime;
                Spell AB1A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1A.CastTime;
                float X3A = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2A.CastTime;
                float X2A = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB3) : castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AM3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles3) : castingState.GetSpell(SpellId.ArcaneMissiles3);

                m2T10time = 5.0f - MBAM2.CastTime;
                Spell AB0B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0B.CastTime;
                Spell AB1B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1B.CastTime;
                float X3B = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM2B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2B.CastTime;
                float X2B = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM3B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB3) : castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AM3B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles3) : castingState.GetSpell(SpellId.ArcaneMissiles3);

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

                float S0A = ((1 - K0) * (K2 * X2B + MB * X3B) + K0 * (1 - K0)) / (1 - (K2 * (X2A - X2B) + MB * (X3A - X3B)));
                float S0B = 1 - K0 - S0A;
                float S1 = K0;

                cycle.AddSpell(needsDisplayCalculations, AB0A, S0A);
                cycle.AddSpell(needsDisplayCalculations, AB1A, S0A);
                cycle.AddSpell(needsDisplayCalculations, AB2A, S0A * MC);
                cycle.AddSpell(needsDisplayCalculations, MBAM2A, S0A * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM3A, S0A * K2);
                cycle.AddSpell(needsDisplayCalculations, AM3A, S0A * K0);

                cycle.AddSpell(needsDisplayCalculations, AB0B, S0B);
                cycle.AddSpell(needsDisplayCalculations, AB1B, S0B);
                cycle.AddSpell(needsDisplayCalculations, AB2B, S0B * MC);
                cycle.AddSpell(needsDisplayCalculations, MBAM2B, S0B * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM3B, S0B * K2);
                cycle.AddSpell(needsDisplayCalculations, AM3B, S0B * K0);

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
                float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                    castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, S0, S0, K1 + K2, 0);
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
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
            cycle.Name = "AB2AM";

            if (castingState.BaseStats.Mage2T10 > 0)
            {
                // doesn't support haste transferring over several loops

                // S0A-nonhasted:
                // AB0-AB1-AM2    => S1   (1 - MB) * (1 - MB)
                // AB0-AB1-MBAM2   => S0AB   (1 - (1 - MB) * (1 - MB))
                // S0B-hasted:
                // AB0-AB1-AM2    => S1   (1 - MB) * (1 - MB)
                // AB0-AB1-MBAM2   => S0AB   (1 - (1 - MB) * (1 - MB))
                // S1-no 2T10:
                // AB0-AB1-AM2    => S1   (1 - MB) * (1 - MB)
                // AB0-AB1-MBAM2   => S0A   (1 - (1 - MB) * (1 - MB))

                // K0 := (1-MB)*(1-MB)

                // MC := 1-MB

                // S0A = S0A * (1 - K0) * XA + S0B * (1 - K0) * XB + S1 * (1 - K0)
                // S0B = S0A * (1 - K0) * YA + S0B * (1 - K0) * YB
                // S0 = 1 - K0
                // S1 = K0

                // S0B = 1 - K0 - S0A

                // S0A = S0A * (1 - K0) * (XA - XB) + (1 - K0) * (1 - K0) * XB + K0 * (1 - K0)
                // S0A * (1 - (1 - K0) * (XA - XB)) = (1 - K0) * (1 - K0) * XB + K0 * (1 - K0)
                // S0A = ((1 - K0) * (1 - K0) * XB + K0 * (1 - K0)) / (1 - (1 - K0) * (XA - XB))

                Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
                Spell MBAM2H = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2);

                float m2T10time = 5.0f - MBAM2.CastTime;
                Spell AB0A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0A.CastTime;
                Spell AB1A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1A.CastTime;
                float XA = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles2) : castingState.GetSpell(SpellId.ArcaneMissiles2);

                m2T10time = 5.0f - MBAM2H.CastTime;
                Spell AB0B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0B.CastTime;
                Spell AB1B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1B.CastTime;
                float XB = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM2B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AM2B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissiles2) : castingState.GetSpell(SpellId.ArcaneMissiles2);

                Spell AB0C = castingState.GetSpell(SpellId.ArcaneBlast0);
                Spell AB1C = castingState.GetSpell(SpellId.ArcaneBlast1);
                Spell MBAM2C = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AM2C = castingState.GetSpell(SpellId.ArcaneMissiles2);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float MC = 1 - MB;
                float K0 = (1 - MB) * (1 - MB);

                float S0A = ((1 - K0) * (1 - K0) * XB + K0 * (1 - K0)) / (1 - (1 - K0) * (XA - XB));
                float S0B = 1 - K0 - S0A;
                float S1 = K0;

                cycle.AddSpell(needsDisplayCalculations, AB0A, S0A);
                cycle.AddSpell(needsDisplayCalculations, AB1A, S0A);
                cycle.AddSpell(needsDisplayCalculations, MBAM2A, S0A * (1 - K0));
                cycle.AddSpell(needsDisplayCalculations, AM2A, S0A * K0);

                cycle.AddSpell(needsDisplayCalculations, AB0B, S0B);
                cycle.AddSpell(needsDisplayCalculations, AB1B, S0B);
                cycle.AddSpell(needsDisplayCalculations, MBAM2B, S0B * (1 - K0));
                cycle.AddSpell(needsDisplayCalculations, AM2B, S0B * K0);

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
                float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                    castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, 1, 1, 0, 0);
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
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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

    class AB3AM2MBAM : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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

    class AB3AMABar2C : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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

    class ABABar0C : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

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

    class ABABar1C : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

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

    class ABABar0MBAM : DynamicCycle
    {
        public ABABar0MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "ABABar0MBAM";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

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

    class ABSpam3C : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);
            AddSpell(needsDisplayCalculations, ABar, K1 + K2 + K4);

            Calculate();
        }
    }

    class ABSpam03C : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM, K6);
            if (MBAM.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime, K6);
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);
            AddSpell(needsDisplayCalculations, ABar, K1 + K2 + K4 + K6);

            Calculate();
        }
    }

    public class ABSpam03MBAM : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM, S2);
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);

            Calculate();
        }
    }

    public class ABSpam3MBAM : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM3, K1 + K2 + K4);

            Calculate();
        }
    }

    public static class ABSpam4MBAM
    {
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                if (castingState.BaseStats.Mage2T10 > 0)
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
                if (castingState.BaseStats.Mage2T10 > 0)
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
                    castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, ab0, ab1, ab2, ab3, K2 + 2 * K4 + K5);
                    castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, ABT, abt0, abt1, abt2, abt3);
                }
                else
                {
                    Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                    castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
                }
            }
            cycle.AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam04MBAM
    {
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, S0, S1;
            cycle.Name = "ABSpam04MBAM";

            if (castingState.BaseStats.Mage2T10 > 0)
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
                    float n = (float)Math.Max(4, Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / AB0.CastTime));
                    float K0 = (float)Math.Pow(1 - MB, n);
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
                        cycle.AddSpell(needsDisplayCalculations, ABn, S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB + S1 * (1 + MB));
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
                    Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);

                    float n = (float)Math.Max(4, Math.Ceiling((5f - castingState.CalculationOptions.LatencyChannel) / ABT.CastTime));
                    float K0 = (float)Math.Pow(1 - MB, n);
                    S0 = MB / (MB + K0);
                    S1 = K0 / (MB + K0);

                    CharacterCalculationsMage calc = castingState.Calculations;
                    calc.ArcaneBlastTemplate.AddToCycle(calc, cycle, AB, (n < 1) ? S0 : 0, (n < 2) ? S0 : 0, (n < 3) ? S0 : 0, (n < 4) ? S0 : 0, (n <= 4) ? (S0 * (1 - MB) * (1 - MB) * (1 - MB) * MB + S1 * (1 + MB)) : (float)(S0 * Math.Pow(1 - MB, n - 1) * MB + S1 * (1 + MB)));
                    calc.ArcaneBlastTemplate.AddToCycle(calc, cycle, ABT, (n >= 1) ? S0 : 0, (n >= 2) ? S0 : 0, (n >= 3) ? S0 : 0, (n >= 4) ? S0 : 0, (n <= 4) ? 0 : (float)(S0 * (Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 1)) / MB));

                    if (n <= 4)
                    {
                        cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (1 - K0) + S1 * MB);
                    }
                    else
                    {
                        cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (float)Math.Pow(1 - MB, n - 2) * MB * (2 - MB) + S1 * MB);
                    }
                    if (n >= 5)
                    {
                        Spell MBAM4T = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB4);
                        cycle.AddSpell(needsDisplayCalculations, MBAM4T, S0 * (float)(2 - Math.Pow(1 - MB, 3) - Math.Pow(1 - MB, n - 5)));
                    }

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
                float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                    CharacterCalculationsMage calc = castingState.Calculations;
                    calc.ArcaneBlastTemplate.AddToCycle(calc, cycle, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
                    calc.ArcaneMissilesTemplate.AddToCycle(calc, cycle, MBAM0, S2, 0, 0, 0, K1 + K2 + K4);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam24MBAM
    {
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, K1 + K2 + K3 + K6, K1 + K2 + K3 + K6, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            cycle.AddSpell(needsDisplayCalculations, MBAM2, K6);
            cycle.AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam024MBAM
    {
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, K6, S0, S1;
            cycle.Name = "ABSpam024MBAM";

            if (castingState.BaseStats.Mage2T10 > 0)
            {
                // needs a completely different state graph because the MBAM can be either hasted or not hasted
                // but we don't need to worry about T8, can't have 4T8 and 2T10 together
                // still assume that at 4 stack 2T10 is always out to simplify things a bit

                // S0A-honhasted:
                // AB0-AB1-MBAM2 => S0A               MB*X                         first AB procs
                // AB0-AB1-MBAM2 => S0B               MB*(1-X)                     first AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S0A       (1 - (1-MB)*(1-MB))*(1-MB) 2nd or 3rd AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0A   (1-MB)*(1-MB)*(1-MB)*MB    fourth AB procs
                // AB0-AB1-AB2-AB3 => S1            (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
                // S0B-hasted:
                // AB0-AB1-MBAM2 => S0A               MB*Y                         first AB procs
                // AB0-AB1-MBAM2 => S0B               MB*(1-Y)                     first AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S0A       (1 - (1-MB)*(1-MB))*(1-MB) 2nd or 3rd AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0A   (1-MB)*(1-MB)*(1-MB)*MB    fourth AB procs
                // AB0-AB1-AB2-AB3 => S1            (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
                // S1:
                // AB4-AB4-MBAM4 => S0A           MB                    proc
                // AB4 => S1                    (1-MB)                 no proc

                // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

                // S0A = S0A * (MB*X + (1-MB)*(1 - (1-MB)*(1-MB)*(1-MB)) + S0B * (MB*Y + (1-MB)*(1 - (1-MB)*(1-MB)*(1-MB)) + S1 * MB
                // S0B = S0A * MB * (1-X) + S0B * MB * (1-Y)
                // S1 = K0 * S0A + K0 * S0B + (1-MB)*S1

                // S0 = S0A + S0B
                // S1 = K0 * S0 + (1-MB)*S1
                // S0 + S1 = 1

                // S0 + K0 * S0 = 1
                // S0 = MB / (MB + K0)
                // S1 = K0 / (MB + K0)

                // S0B = S0A * MB * (1-X) + S0B * MB * (1-Y)
                // S0B * (1 - MB * (1-Y)) = S0A * MB * (1-X)
                // S0B = S0A * MB * (1-X) / (1 - MB * (1-Y))

                // S0A + S0B = S0A * (1 + MB * (Y-X)) / (1 - MB * (1-Y)) = MB / (MB + K0)
                // S0A = (1 - MB * (1-Y))*MB / ((1 + MB * (Y-X))*(MB + K0))

                Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                Spell MBAM2 = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2);

                float m2T10time = 5.0f - MBAM4.CastTime;
                Spell AB0A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0A.CastTime;
                Spell AB1A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1A.CastTime;
                float X = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2A.CastTime;
                Spell AB3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);

                m2T10time = 5.0f - MBAM2.CastTime;
                Spell AB0B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0B.CastTime;
                Spell AB1B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1B.CastTime;
                float Y = (m2T10time > 0.0f) ? 0 : 1;
                Spell MBAM2B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2B.CastTime;
                Spell AB3B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                S0 = MB / (MB + K0);
                S1 = 1 - S0;
                float S0A = (1 - MB * (1 - Y)) * MB / ((1 + MB * (Y - X)) * (MB + K0));
                float S0B = S0 - S0A;

                K1 = (1 - (1 - MB) * (1 - MB)) * (1 - MB);
                K2 = (1 - MB) * (1 - MB) * (1 - MB) * MB;

                cycle.AddSpell(needsDisplayCalculations, AB0A, S0A);
                cycle.AddSpell(needsDisplayCalculations, AB1A, S0A);
                cycle.AddSpell(needsDisplayCalculations, AB2A, S0A * (1 - MB));
                cycle.AddSpell(needsDisplayCalculations, AB3A, S0A * (1 - MB));
                cycle.AddSpell(needsDisplayCalculations, AB0B, S0B);
                cycle.AddSpell(needsDisplayCalculations, AB1B, S0B);
                cycle.AddSpell(needsDisplayCalculations, AB2B, S0B * (1 - MB));
                cycle.AddSpell(needsDisplayCalculations, AB3B, S0B * (1 - MB));
                cycle.AddSpell(needsDisplayCalculations, AB4, S0 * K2 + S1 * (MB + 1));
                cycle.AddSpell(needsDisplayCalculations, MBAM2A, S0A * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM2B, S0B * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * (K1 + K2) + S1 * MB);
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
                float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                    CharacterCalculationsMage calc = castingState.Calculations;
                    calc.ArcaneBlastTemplate.AddToCycle(calc, cycle, AB, K1 + K2 + K3 + K6, K1 + K2 + K3 + K6, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
                    calc.ArcaneMissilesTemplate.AddToCycle(calc, cycle, MBAM0, S2, 0, K6, 0, K1 + K2 + K4);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam0234MBAM
    {
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
            float K1, K2, K3, K4, K5, K6, K7, S0, S1;
            cycle.Name = "ABSpam0234MBAM";

            if (castingState.BaseStats.Mage2T10 > 0)
            {
                // needs a completely different state graph because the MBAM can be either hasted or not hasted
                // but we don't need to worry about T8, can't have 4T8 and 2T10 together
                // still assume that at 4 stack 2T10 is always out to simplify things a bit

                // S0A-nonhasted:
                // AB0-AB1-MBAM2 => S0A               MB*X0                        first AB procs
                // AB0-AB1-MBAM2 => S0B               MB*(1-X0)                        first AB procs
                // AB0-AB1-AB2-MBAM3 => S0A           (1-MB)*MB*X1                  2nd AB procs
                // AB0-AB1-AB2-MBAM3 => S0B           (1-MB)*MB*(1-X1)                  2nd AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S0A       (1-MB)*(1-MB)*MB           3rd AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0A   (1-MB)*(1-MB)*(1-MB)*MB    fourth AB procs
                // AB0-AB1-AB2-AB3 => S1             (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
                // S0B-hasted:
                // AB0-AB1-MBAM2 => S0A               MB*X2                        first AB procs
                // AB0-AB1-MBAM2 => S0B               MB*(1-X2)                        first AB procs
                // AB0-AB1-AB2-MBAM3 => S0A           (1-MB)*MB*X3                  2nd AB procs
                // AB0-AB1-AB2-MBAM3 => S0B           (1-MB)*MB*(1-X3)                  2nd AB procs
                // AB0-AB1-AB2-AB3-MBAM4 => S0A       (1-MB)*(1-MB)*MB           3rd AB procs
                // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0A   (1-MB)*(1-MB)*(1-MB)*MB    fourth AB procs
                // AB0-AB1-AB2-AB3 => S1             (1-MB)*(1-MB)*(1-MB)*(1-MB)       no procs
                // S1:
                // AB4-AB4-MBAM4 => S0A           MB                     proc
                // AB4 => S1                    (1-MB)                 no proc

                // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

                // MC := 1-MB

                // S0A = S0A * (MB*X0 + MC*MB*X1 + MC*MC*(1 - MC*MC)) + S0B * (MB*X2 + MC*MB*X3 + MC*MC*(1 - MC*MC)) + S1 * MB
                // S0B = S0A * (MB*Y0 + MC*MB*Y1) + S0B * (MB*Y2 + MC*MB*Y3)
                // S1 = K0 * S0A + K0 * S0B + (1-MB)*S1

                // S0 = S0A + S0B
                // S1 = K0 * S0 + (1-MB)*S1
                // S0 + S1 = 1

                // S0 + K0 * S0 = 1
                // S0 = MB / (MB + K0)
                // S1 = K0 / (MB + K0)

                // S0B = S0A * (MB*Y0 + MC*MB*Y1) + S0B * (MB*Y2 + MC*MB*Y3)
                // S0B * (1 - MB*Y2 - MC*MB*Y3) = S0A * (MB*Y0 + MC*MB*Y1)
                // S0B = S0A * (MB*Y0 + MC*MB*Y1) / (1 - MB*Y2 - MC*MB*Y3)

                // S0A + S0B = S0A * (1 - MB*Y2 - MC*MB*Y3 + MB*Y0 + MC*MB*Y1) / (1 - MB*Y2 - MC*MB*Y3) = MB / (MB + K0)
                // S0A = MB * (1 - MB*Y2 - MC*MB*Y3) / ((MB + K0) * (1 - MB*Y2 - MC*MB*Y3 + MB*Y0 + MC*MB*Y1))

                Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
                Spell MBAM2 = castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2);

                float m2T10time = 5.0f - MBAM4.CastTime;
                Spell AB0A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0A.CastTime;
                Spell AB1A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1A.CastTime;
                float Y0 = (m2T10time > 0.0f) ? 1 : 0;
                Spell MBAM2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2A.CastTime;
                float Y1 = (m2T10time > 0.0f) ? 1 : 0;
                Spell MBAM3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB3) : castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AB3A = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);
                Spell AB4 = castingState.GetSpell(SpellId.ArcaneBlast4);

                m2T10time = 5.0f - MBAM2.CastTime;
                Spell AB0B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast0) : castingState.GetSpell(SpellId.ArcaneBlast0);
                m2T10time -= AB0B.CastTime;
                Spell AB1B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast1) : castingState.GetSpell(SpellId.ArcaneBlast1);
                m2T10time -= AB1B.CastTime;
                float Y2 = (m2T10time > 0.0f) ? 1 : 0;
                Spell MBAM2B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB2) : castingState.GetSpell(SpellId.ArcaneMissilesMB2);
                Spell AB2B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast2) : castingState.GetSpell(SpellId.ArcaneBlast2);
                m2T10time -= AB2B.CastTime;
                float Y3 = (m2T10time > 0.0f) ? 1 : 0;
                Spell MBAM3B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneMissilesMB3) : castingState.GetSpell(SpellId.ArcaneMissilesMB3);
                Spell AB3B = (m2T10time > 0.0f) ? castingState.Tier10TwoPieceState.GetSpell(SpellId.ArcaneBlast3) : castingState.GetSpell(SpellId.ArcaneBlast3);

                float MB = 0.08f * castingState.MageTalents.MissileBarrage;
                float MC = 1 - MB;
                float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
                S0 = MB / (MB + K0);
                S1 = 1 - S0;
                float S0A = MB * (1 - MB * Y2 - MC * MB * Y3) / ((MB + K0) * (1 - MB * Y2 - MC * MB * Y3 + MB * Y0 + MC * MB * Y1));
                float S0B = S0 - S0A;

                K1 = (1 - MB) * MB;
                K2 = (1 - MB) * (1 - MB) * MB;
                K3 = (1 - MB) * (1 - MB) * (1 - MB) * MB;
                K4 = MC * MC * (1 - MC * MC);

                cycle.AddSpell(needsDisplayCalculations, AB0A, S0A);
                cycle.AddSpell(needsDisplayCalculations, AB1A, S0A);
                cycle.AddSpell(needsDisplayCalculations, AB2A, S0A * MC);
                cycle.AddSpell(needsDisplayCalculations, AB3A, S0A * MC * MC);
                cycle.AddSpell(needsDisplayCalculations, AB0B, S0B);
                cycle.AddSpell(needsDisplayCalculations, AB1B, S0B);
                cycle.AddSpell(needsDisplayCalculations, AB2B, S0B * MC);
                cycle.AddSpell(needsDisplayCalculations, AB3B, S0B * MC * MC);
                cycle.AddSpell(needsDisplayCalculations, AB4, S0 * K3 + S1 * (MB + 1));
                cycle.AddSpell(needsDisplayCalculations, MBAM2A, S0A * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM2B, S0B * MB);
                cycle.AddSpell(needsDisplayCalculations, MBAM3A, S0A * K1);
                cycle.AddSpell(needsDisplayCalculations, MBAM3B, S0B * K1);
                cycle.AddSpell(needsDisplayCalculations, MBAM4, S0 * K4 + S1 * MB);
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
                float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                    CharacterCalculationsMage calc = castingState.Calculations;
                    calc.ArcaneBlastTemplate.AddToCycle(calc, cycle, AB, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K7, K1 + K2 + K3, K2 + 2 * K4 + K5);
                    calc.ArcaneMissilesTemplate.AddToCycle(calc, cycle, MBAM0, S2, 0, K6, K7, K1 + K2 + K4);
                }
            }

            cycle.Calculate();
            return cycle;
        }
    }

    public static class ABSpam234MBAM
    {
        public static DynamicCycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            DynamicCycle cycle = DynamicCycle.New(needsDisplayCalculations, castingState);
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K7, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            cycle.AddSpell(needsDisplayCalculations, MBAM2, K6);
            cycle.AddSpell(needsDisplayCalculations, MBAM3, K7);
            cycle.AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);

            cycle.Calculate();
            return cycle;
        }
    }

    class AB2ABar2C : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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

    class AB2ABar2MBAM : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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

    class AB2ABar3C : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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

    class ABABar3C : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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

    class ABABar2C : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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

    class ABABar2MBAM : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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

    class ABABar1MBAM : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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

    class AB3ABar3C : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, K1 + K2, K1 + K2, K1 + K2, 0);
            }
            AddSpell(needsDisplayCalculations, ABar3, K1);
            AddSpell(needsDisplayCalculations, MBAM3, K2);
            AddSpell(needsDisplayCalculations, ABar, K2);

            Calculate();
        }
    }

    class AB3ABar3MBAM : DynamicCycle
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
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
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
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, K1 + K2, K1 + K2, K1 + K2, 0);
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
            public float ArcaneBarrageCooldown { get; set; }
            public int ArcaneBlastStack { get; set; }
            public float Tier10TwoPieceDuration { get; set; }
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

        public ArcaneCycleGenerator(CastingState castingState, bool ABarAllowed, bool ABarOnCooldownOnly, bool MBDurationCollapsed, bool ABarCooldownCollapsed, bool Tier10TwoPieceCollapsed)
        {
            this.ABarAllowed = ABarAllowed;
            this.ABarOnCooldownOnly = ABarOnCooldownOnly;
            this.MBDurationCollapsed = MBDurationCollapsed;
            this.ABarCooldownCollapsed = ABarCooldownCollapsed;
            this.Tier10TwoPieceCollapsed = Tier10TwoPieceCollapsed;

            var calc = castingState.Calculations;
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
            T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
            T10 = castingState.BaseStats.Mage2T10 > 0;
            channelLatency = castingState.CalculationOptions.LatencyChannel;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(false, 0.0f, 0.0f, 0, 0.0f);
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
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    TargetState = GetState(
                        s.MissileBarrageDuration > AB.CastTime,
                        15.0f,
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                        Math.Min(maxStack, s.ArcaneBlastStack + 1),
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - AB.CastTime)
                    ),
                    TransitionProbability = ABMB
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
                    Math.Max(0.0f, s.Tier10TwoPieceDuration - AB.CastTime)
                ),
                TransitionProbability = 1.0f - ABMB
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
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - AM.CastTime) // cannot have 4T8 and 2T10 at the same time
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
                    Math.Max(0.0f, (s.MissileBarrageDuration > 0 && T10) ? 5.0f - channelLatency : s.Tier10TwoPieceDuration - AM.CastTime)
                ),
                TransitionProbability = s.MissileBarrageDuration > 0 ? 1.0f - T8 : 1.0f
            });
            if (ABarAllowed && (!ABarOnCooldownOnly || s.ArcaneBarrageCooldown == 0))
            {
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
                            Math.Max(0.0f, s.Tier10TwoPieceDuration - ABar.CastTime - s.ArcaneBarrageCooldown)
                        ),
                        TransitionProbability = MB
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
                        Math.Max(0.0f, s.Tier10TwoPieceDuration - ABar.CastTime - s.ArcaneBarrageCooldown)
                    ),
                    TransitionProbability = 1.0f - MB
                });
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(bool missileBarrageRegistered, float missileBarrageDuration, float arcaneBarrageCooldown, int arcaneBlastStack, float tier10TwoPieceDuration)
        {
            string name = string.Format("AB{0},ABar{1},MB{2}{3},2T10={4}", arcaneBlastStack, arcaneBarrageCooldown, missileBarrageDuration, missileBarrageRegistered ? "+" : "-", tier10TwoPieceDuration);
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, MissileBarrageRegistered = missileBarrageRegistered, MissileBarrageDuration = missileBarrageDuration, ArcaneBarrageCooldown = arcaneBarrageCooldown, ArcaneBlastStack = arcaneBlastStack, Tier10TwoPieceDuration = tier10TwoPieceDuration };
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
                || (!MBDurationCollapsed && a.MissileBarrageRegistered == true && b.MissileBarrageRegistered == true && a.MissileBarrageDuration != b.MissileBarrageDuration));
        }

        public override string StateDescription
        {
            get
            {
                return @"Cycle Code Legend: 0 = AB, 1 = AM, 2 = ABar
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

    public class ArcaneMovementCycleGenerator : CycleGenerator
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

        public ArcaneMovementCycleGenerator(CastingState castingState, float movementEventRate, float movementDuration)
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
                    AMT[ab, t] = castingState.Calculations.ArcaneMissilesTemplate.GetSpell(castingState, false, ab, t);
                    MBAMT[ab, t] = castingState.Calculations.ArcaneMissilesTemplate.GetSpell(castingState, true, ab, t);
                }
            }
            ABNull = new Spell(castingState.Calculations.ArcaneBlastTemplate);
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
            T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

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
