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
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, cycle, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
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
        }

        public Spell AB0, AB1, AB2, AB3, AB4, ABar0, ABar1, ABar2, ABar3, ABar4, AM0, AM1, AM2, AM3, AM4, MBAM0, MBAM1, MBAM2, MBAM3, MBAM4;

        private float ABMB;
        private float MB;
        private float T8;
        private int maxStack;

        public ArcaneCycleGenerator(CastingState castingState)
        {
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

            ABMB = 0.08f * castingState.MageTalents.MissileBarrage;
            maxStack = 4;
            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(false, 0.0f, 0.0f, 0);
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
            if (MB > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = AB,
                    TargetState = GetState(
                        s.MissileBarrageDuration > AB.CastTime,
                        15.0f,
                        Math.Max(0.0f, s.ArcaneBarrageCooldown - AB.CastTime),
                        Math.Min(maxStack, s.ArcaneBlastStack + 1)
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
                    Math.Min(maxStack, s.ArcaneBlastStack + 1)
                ),
                TransitionProbability = 1.0f - ABMB
            });
            //if (s.ArcaneBarrageCooldown == 0)
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
                            0
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
                        0
                    ),
                    TransitionProbability = 1.0f - MB
                });
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
                        0
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
                    0
                ),
                TransitionProbability = s.MissileBarrageDuration > 0 ? 1.0f - T8 : 1.0f
            });

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(bool missileBarrageRegistered, float missileBarrageDuration, float arcaneBarrageCooldown, int arcaneBlastStack)
        {
            string name = string.Format("AB{0},ABar{1},MB{2}{3}", arcaneBlastStack, arcaneBarrageCooldown, missileBarrageDuration, missileBarrageRegistered ? "+" : "-");
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, MissileBarrageRegistered = missileBarrageRegistered, MissileBarrageDuration = missileBarrageDuration, ArcaneBarrageCooldown = arcaneBarrageCooldown, ArcaneBlastStack = arcaneBlastStack };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (a.ArcaneBlastStack != b.ArcaneBlastStack || a.ArcaneBarrageCooldown != b.ArcaneBarrageCooldown || a.MissileBarrageRegistered != b.MissileBarrageRegistered/* || (a.MissileBarrageRegistered == true && b.MissileBarrageRegistered == true && a.MissileBarrageDuration != b.MissileBarrageDuration)*/);
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
