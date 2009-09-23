using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    class ABAM : DynamicCycle
    {
        public ABAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "ABAM";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles1);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB1);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to AB-AM
                AddSpell(needsDisplayCalculations, AB, 1);
                AddSpell(needsDisplayCalculations, AM, 1);
                Calculate();
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

                AddSpell(needsDisplayCalculations, AB, 1);
                AddSpell(needsDisplayCalculations, AM, S0 * K1);
                AddSpell(needsDisplayCalculations, MBAM, 1 - S0 * K1);

                Calculate();
            }
        }
    }

    public class AB3AM : DynamicCycle
    {
        public AB3AM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "AB3AM";

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
                AddSpell(needsDisplayCalculations, AB0, 1);
                AddSpell(needsDisplayCalculations, AB1, 1);
                AddSpell(needsDisplayCalculations, AB2, 1);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, 1, 1, 1, 0);
            }
            AddSpell(needsDisplayCalculations, AM3, S0 * K1);
            AddSpell(needsDisplayCalculations, MBAM3, 1 - S0 * K1);

            Calculate();
        }
    }

    public class AB4AM234MBAM : DynamicCycle
    {
        public AB4AM234MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "AB4AM234MBAM";

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
                AddSpell(needsDisplayCalculations, AB0, 1);
                AddSpell(needsDisplayCalculations, AB1, 1);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K1 + K2);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, 1, 1, K1 + K2 + K3, K1 + K2);
            }
            AddSpell(needsDisplayCalculations, AM4, K1);
            AddSpell(needsDisplayCalculations, MBAM4, K2);
            AddSpell(needsDisplayCalculations, MBAM3, K3);
            AddSpell(needsDisplayCalculations, MBAM2, K4);

            Calculate();
        }
    }

    public class AB3AM23MBAM : DynamicCycle
    {
        public AB3AM23MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "AB3AM23MBAM";

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
                AddSpell(needsDisplayCalculations, AB0, 1);
                AddSpell(needsDisplayCalculations, AB1, 1);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, 1, 1, K1 + K2, 0);
            }
            AddSpell(needsDisplayCalculations, AM3, K1);
            AddSpell(needsDisplayCalculations, MBAM3, K2);
            AddSpell(needsDisplayCalculations, MBAM2, K4);

            Calculate();
        }
    }

    class AB2AM : DynamicCycle
    {
        public AB2AM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "AB2AM";

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
                AddSpell(needsDisplayCalculations, AB0, 1);
                AddSpell(needsDisplayCalculations, AB1, 1);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, 1, 1, 0, 0);
            }
            AddSpell(needsDisplayCalculations, AM2, S0 * K1);
            AddSpell(needsDisplayCalculations, MBAM2, 1 - S0 * K1);

            Calculate();
        }
    }

    class AB3AMABar : DynamicCycle
    {
        public AB3AMABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Name = "AB3AMABar";

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

            AddSpell(needsDisplayCalculations, AB0, 1);
            AddSpell(needsDisplayCalculations, AB1, 1);
            AddSpell(needsDisplayCalculations, AB2, 1);
            AddSpell(needsDisplayCalculations, AM3, S0 * K1);
            AddSpell(needsDisplayCalculations, MBAM3, 1 - S0 * K1);
            AddSpell(needsDisplayCalculations, ABar, 1);

            Calculate();
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

    public class ABSpam24MBAM : DynamicCycle
    {
        public ABSpam24MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2, K3, K4, K5, K6, S0, S1;
            Name = "ABSpam24MBAM";

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
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3 + K6);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3 + K6);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, K1 + K2 + K3 + K6, K1 + K2 + K3 + K6, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM2, K6);
            AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);

            Calculate();
        }
    }

    public class ABSpam234MBAM : DynamicCycle
    {
        public ABSpam234MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2, K3, K4, K5, K6, K7, S0, S1;
            Name = "ABSpam234MBAM";

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
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3 + K6 + K7);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3 + K6 + K7);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3 + K7);
                AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K6 + K7, K1 + K2 + K3 + K7, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM2, K6);
            AddSpell(needsDisplayCalculations, MBAM3, K7);
            AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);

            Calculate();
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

    /*class ArcaneMissilesCC : Cycle
    {
        Spell AMc1;
        Spell AM10;
        Spell AM11;
        float CC;

        public ArcaneMissilesCC(CastingState castingState) : base(castingState)
        {
            Name = "Arcane Missiles CC";

            //AM?1-AM11-AM11-...=0.9*0.1*...
            //...
            //AM?1-AM10=0.9

            //TIME = T * [1 + 1/0.9]
            //DAMAGE = AM?1 + AM10 + 0.1/0.9*AM11

            CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

            AMc1 = new ArcaneMissiles(castingState, false, true, false, true, 0, 5);
            AM10 = new ArcaneMissiles(castingState, false, false, true, false, 0, 5);
            AM11 = new ArcaneMissiles(castingState, false, false, true, true, 0, 5);

            CastProcs = AMc1.CastProcs * (1 + 1 / (1 - CC));
            CastTime = AMc1.CastTime * (1 + 1 / (1 - CC));
            HitProcs = AMc1.HitProcs * (1 + 1 / (1 - CC));
            costPerSecond = (AMc1.CostPerSecond + AM10.CostPerSecond + CC / (1 - CC) * AM11.CostPerSecond) / (1 + 1 / (1 - CC));
            damagePerSecond = (AMc1.DamagePerSecond + AM10.DamagePerSecond + CC / (1 - CC) * AM11.DamagePerSecond) / (1 + 1 / (1 - CC));
            threatPerSecond = (AMc1.ThreatPerSecond + AM10.ThreatPerSecond + CC / (1 - CC) * AM11.ThreatPerSecond) / (1 + 1 / (1 - CC));
            //ManaRegenPerSecond = (AMc1.ManaRegenPerSecond + AM10.ManaRegenPerSecond + CC / (1 - CC) * AM11.ManaRegenPerSecond) / (1 + 1 / (1 - CC)); // we only use it indirectly in spell cycles that recompute oo5sr
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            AMc1.AddSpellContribution(dict, duration * 1 / (1 + 1 / (1 - CC)));
            AM10.AddSpellContribution(dict, duration * 1 / (1 + 1 / (1 - CC)));
            AM11.AddSpellContribution(dict, duration * CC / (1 - CC) / (1 + 1 / (1 - CC)));
        }

        public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            AMc1.AddManaUsageContribution(dict, duration * 1 / (1 + 1 / (1 - CC)));
            AM10.AddManaUsageContribution(dict, duration * 1 / (1 + 1 / (1 - CC)));
            AM11.AddManaUsageContribution(dict, duration * CC / (1 - CC) / (1 + 1 / (1 - CC)));
        }
    }*/

    /*class ABAMP : StaticCycle
    {
        public ABAMP(CastingState castingState) : base(3)
        {
            Name = "ABAMP";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast10);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);

            AddSpell(AB, castingState);
            AddSpell(AM, castingState);
            AddPause(8 - AM.CastTime - AB.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }*/

    class ABP : StaticCycle
    {
        public ABP(CastingState castingState)
            : base(3)
        {
            Name = "ABP";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);

            AddSpell(AB, castingState);
            AddPause(3 - AB.CastTime + castingState.CalculationOptions.LatencyGCD);

            Calculate(castingState);
        }
    }

    class ABarAM : DynamicCycle
    {
        public ABarAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            Name = "ABarAM";

            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to ABar-AM
                AddSpell(needsDisplayCalculations, ABar, 1);
                AddSpell(needsDisplayCalculations, AM, 1);

                Calculate();
            }
            else
            {
                //AB-AM 0.85
                AddSpell(needsDisplayCalculations, ABar, 1 - MB);
                AddSpell(needsDisplayCalculations, AM, 1 - MB);

                //AB-MBAM 0.15
                AddSpell(needsDisplayCalculations, ABar, MB);
                AddSpell(needsDisplayCalculations, MBAM, MB);
                if (ABar.CastTime + MBAM.CastTime < 3.0) AddPause(3.0f - ABar.CastTime - MBAM.CastTime, MB);

                Calculate();
            }
        }
    }

    class ABABarSlow : DynamicCycle
    {
        public ABABarSlow(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float MB;
            float X;
            float S0;
            float S1;
            // TODO not updated for 3.0.8 mode, consider deprecated?
            Name = "ABABarSlow";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell ABar = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell Slow = castingState.GetSpell(SpellId.Slow);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            //S0
            //AB-Pause-ABar     1 - X  (1 - MB) * (1 - MB) => S0, 1 - (1 - MB) * (1 - MB) => S1
            //AB-Slow-ABar      X

            //S1
            //MBAM-Pause-ABar   1 - X  (1 - MB) => S0, MB => S1
            //MBAM-Slow-ABar    X

            //S0 + S1 = 1
            //S0a = (1 - X) * ((1 - MB) * (1 - MB) * S0 + (1 - MB) * S1)
            //S0b = X * ((1 - MB) * (1 - MB) * S0 + (1 - MB) * S1)
            //S1a = (1 - X) * ((1 - (1 - MB) * (1 - MB)) * S0 + MB * S1)
            //S1b = X * ((1 - (1 - MB) * (1 - MB)) * S0 + MB * S1)

            //S0 = (1 - MB) * (1 - MB) * S0 + (1 - MB) * S1
            //S1 = (1 - (1 - MB) * (1 - MB)) * S0 + MB * S1

            //S0 * (1 - (1 - MB) * (1 - MB) + (1 - MB)) = (1 - MB)

            //S0 = (1 - MB) / (1 + (1 - MB) * MB)
            //S1 = (2 - MB) * MB / (1 + (1 - MB) * MB)

            //time casting slow / all time casting = time(Slow) / 15

            //value = (1 - X) * S0 * (value(AB) + value(ABar) + value(Pause)) + X * S0 * (value(AB) + value(ABar) + value(Slow)) + (1 - X) * S1 * (value(MBAM) + value(ABar) + value(Pause)) * X * S1 * (value(MBAM) + value(ABar) + value(Slow))
            //value = S0 * value(AB-ABar) + X * S0 * value(Slow) + S1 * value(MBAM-ABar) + X * S1 * value(Slow) + (1 - X) * value(Pause)
            //value = S0 * value(AB-ABar) + S1 * value(MBAM-ABar) + X * value(Slow) + (1 - X) * value(Pause)

            // X = (S0 * time(AB-ABar) + S1 * time(MBAM-ABar) + time(Pause)) / (15 - time(Slow) + time(Pause))

            S0 = (1 - MB) / (1 + (1 - MB) * MB);
            S1 = (2 - MB) * MB / (1 + (1 - MB) * MB);
            float Pause = 0.0f;
            if (AB.CastTime + ABar.CastTime < 3.0) Pause = 3.0f + castingState.CalculationOptions.LatencyGCD - AB.CastTime - ABar.CastTime;
            X = (S0 * (AB.CastTime + ABar.CastTime) + S1 * (MBAM.CastTime + ABar.CastTime) + Pause) / (15.0f - Slow.CastTime + Pause);

            //AB-ABar
            AddSpell(needsDisplayCalculations, ABar, (1 - X) * S0);
            AddSpell(needsDisplayCalculations, AB, (1 - X) * S0);
            if (AB.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB.CastTime - ABar.CastTime, (1 - X) * S0);

            //ABar-MBAM
            AddSpell(needsDisplayCalculations, ABar, (1 - X) * S1);
            AddSpell(needsDisplayCalculations, MBAM, (1 - X) * S1);
            if (MBAM.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime, (1 - X) * S1);

            //AB-ABar-Slow
            AddSpell(needsDisplayCalculations, ABar, X * S0);
            AddSpell(needsDisplayCalculations, AB, X * S0);
            AddSpell(needsDisplayCalculations, Slow, X * S0);
            if (AB.CastTime + ABar.CastTime + Slow.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB.CastTime - ABar.CastTime - Slow.CastTime, X * S0);

            //ABar-MBAM-Slow
            AddSpell(needsDisplayCalculations, ABar, X * S1);
            AddSpell(needsDisplayCalculations, MBAM, X * S1);
            AddSpell(needsDisplayCalculations, Slow, X * S1);
            if (MBAM.CastTime + ABar.CastTime + Slow.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime - Slow.CastTime, X * S1);

            Calculate();
        }
    }

    class FBABarSlow : DynamicCycle
    {
        public FBABarSlow(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            StaticCycle chain4;
            float MB;
            float X;
            float S0;
            float S1;
            Name = "FBABarSlow";
            AffectedByFlameCap = true;

            Spell FB = castingState.MaintainSnareState.GetSpell(SpellId.Fireball);
            Spell ABar = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell Slow = castingState.GetSpell(SpellId.Slow);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            S0 = (1 - MB) / (1 + (1 - MB) * MB);
            S1 = (2 - MB) * MB / (1 + (1 - MB) * MB);
            float Pause = 0.0f;
            if (FB.CastTime + ABar.CastTime < 3.0) Pause = 3.0f + castingState.CalculationOptions.LatencyGCD - FB.CastTime - ABar.CastTime;
            X = (S0 * (FB.CastTime + ABar.CastTime) + S1 * (MBAM.CastTime + ABar.CastTime) + Pause) / (15.0f - Slow.CastTime + Pause);

            //AB-ABar
            chain1 = new StaticCycle(2);
            chain1.AddSpell(ABar, castingState);
            chain1.AddSpell(FB, castingState);
            if (FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FB.CastTime - ABar.CastTime);
            chain1.Calculate(castingState);

            //ABar-MBAM
            chain2 = new StaticCycle(2);
            chain2.AddSpell(ABar, castingState);
            chain2.AddSpell(MBAM, castingState);
            if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime);
            chain2.Calculate(castingState);

            //AB-ABar-Slow
            chain3 = new StaticCycle(3);
            chain3.AddSpell(ABar, castingState);
            chain3.AddSpell(FB, castingState);
            chain3.AddSpell(Slow, castingState);
            if (FB.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FB.CastTime - ABar.CastTime - Slow.CastTime);
            chain3.Calculate(castingState);

            //ABar-MBAM-Slow
            chain4 = new StaticCycle(3);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.AddSpell(Slow, castingState);
            if (MBAM.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime - Slow.CastTime);
            chain4.Calculate(castingState);

            commonChain = chain1;

            AddCycle(needsDisplayCalculations, chain1, (1 - X) * S0);
            AddCycle(needsDisplayCalculations, chain2, (1 - X) * S1);
            AddCycle(needsDisplayCalculations, chain3, X * S0);
            AddCycle(needsDisplayCalculations, chain4, X * S1);
            Calculate();
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FrBABarSlow : DynamicCycle
    {
        public FrBABarSlow(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            StaticCycle chain4;
            float MB;
            float X;
            float S0;
            float S1;
            Name = "FrBABarSlow";

            Spell FrB = castingState.MaintainSnareState.GetSpell(SpellId.Frostbolt);
            Spell ABar = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.MaintainSnareState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell Slow = castingState.GetSpell(SpellId.Slow);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            S0 = (1 - MB) / (1 + (1 - MB) * MB);
            S1 = (2 - MB) * MB / (1 + (1 - MB) * MB);
            float Pause = 0.0f;
            if (FrB.CastTime + ABar.CastTime < 3.0) Pause = 3.0f + castingState.CalculationOptions.LatencyGCD - FrB.CastTime - ABar.CastTime;
            X = (S0 * (FrB.CastTime + ABar.CastTime) + S1 * (MBAM.CastTime + ABar.CastTime) + Pause) / (15.0f - Slow.CastTime + Pause);

            //AB-ABar
            chain1 = new StaticCycle(2);
            chain1.AddSpell(ABar, castingState);
            chain1.AddSpell(FrB, castingState);
            if (FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FrB.CastTime - ABar.CastTime);
            chain1.Calculate(castingState);

            //ABar-MBAM
            chain2 = new StaticCycle(2);
            chain2.AddSpell(ABar, castingState);
            chain2.AddSpell(MBAM, castingState);
            if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime);
            chain2.Calculate(castingState);

            //AB-ABar-Slow
            chain3 = new StaticCycle(3);
            chain3.AddSpell(ABar, castingState);
            chain3.AddSpell(FrB, castingState);
            chain3.AddSpell(Slow, castingState);
            if (FrB.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FrB.CastTime - ABar.CastTime - Slow.CastTime);
            chain3.Calculate(castingState);

            //ABar-MBAM-Slow
            chain4 = new StaticCycle(3);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.AddSpell(Slow, castingState);
            if (MBAM.CastTime + ABar.CastTime + Slow.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime - Slow.CastTime);
            chain4.Calculate(castingState);

            commonChain = chain1;

            AddCycle(needsDisplayCalculations, chain1, (1 - X) * S0);
            AddCycle(needsDisplayCalculations, chain2, (1 - X) * S1);
            AddCycle(needsDisplayCalculations, chain3, X * S0);
            AddCycle(needsDisplayCalculations, chain4, X * S1);
            Calculate();
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class ABABarY : DynamicCycle
    {
        public ABABarY(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB;
            float S0, S1;
            Name = "ABABarY";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            // S0
            // AB-ABar1 => S0             (1-MB)*(1-MB)
            //          => S1             1 - (1-MB)*(1-MB)

            // S1
            // AB-MBAM1 => S0             1

            // S0 + S1 = 1
            // S0 = S0 * (1-MB)*(1-MB) + S1
            // S1 = S0 * (1 - (1-MB)*(1-MB))

            // 1 = S0 * (2 - (1-MB)*(1-MB))
            // S0 = 1 / (2 - (1-MB)*(1-MB))
            // S1 = (1 - (1-MB)*(1-MB)) / (2 - (1-MB)*(1-MB))

            // value = S0 * value(AB-ABar1) + S1 * value(AB-MBAM1-ABar)

            S0 = 1 / (2 - (1 - MB) * (1 - MB));
            S1 = 1 - S0;

            //AB-ABar1
            chain1 = new StaticCycle(2);
            chain1.AddSpell(AB, castingState);
            if (AB.CastTime + ABar1.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB.CastTime - ABar1.CastTime); // this might not actually be needed if we're transitioning from S1, but we're assuming this cycle is used under low haste conditions only
            chain1.AddSpell(ABar1, castingState);
            chain1.Calculate(castingState);

            //AB-MBAM1
            chain2 = new StaticCycle(3);
            chain2.AddSpell(AB, castingState);
            chain2.AddSpell(MBAM1, castingState);
            chain2.Calculate(castingState);

            AddCycle(needsDisplayCalculations, chain1, S0);
            AddCycle(needsDisplayCalculations, chain2, S1);
            Calculate();
        }
    }

    class AB2ABarMBAM : DynamicCycle
    {
        public AB2ABarMBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2, K3, K4;
            Name = "AB2ABarMBAM";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            //S0:
            //AB0-AB1-ABar2            (1 - MB) * (1 - MB) * (1 - MB)
            //AB0-AB1-ABar2-MBAM       (1 - MB) * (1 - (1 - MB) * (1 - MB))
            //AB0-AB1-MBAM2-ABar       MB * (1 - MB)
            //AB0-AB1-MBAM2-ABar-MBAM  MB * MB


            K1 = (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = MB * (1 - MB);
            K4 = MB * MB;

            //AB0-AB1-ABar2
            AddSpell(needsDisplayCalculations, AB0, K1);
            AddSpell(needsDisplayCalculations, AB1, K1);
            if (AB0.CastTime + AB1.CastTime + ABar2.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - ABar2.CastTime, K1);
            AddSpell(needsDisplayCalculations, ABar2, K1);

            //AB0-AB1-ABar2-MBAM
            AddSpell(needsDisplayCalculations, AB0, K2);
            AddSpell(needsDisplayCalculations, AB1, K2);
            if (AB0.CastTime + AB1.CastTime + ABar2.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - ABar2.CastTime, K2);
            AddSpell(needsDisplayCalculations, ABar2, K2);
            AddSpell(needsDisplayCalculations, MBAM, K2);

            //AB0-AB1-MBAM2-ABar
            AddSpell(needsDisplayCalculations, AB0, K3);
            AddSpell(needsDisplayCalculations, AB1, K3);
            AddSpell(needsDisplayCalculations, MBAM2, K3);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - ABar.CastTime - MBAM2.CastTime, K3);
            AddSpell(needsDisplayCalculations, ABar, K3);

            //AB0-AB1-MBAM2-ABar
            AddSpell(needsDisplayCalculations, AB0, K4);
            AddSpell(needsDisplayCalculations, AB1, K4);
            AddSpell(needsDisplayCalculations, MBAM2, K4);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - ABar.CastTime - MBAM2.CastTime, K4);
            AddSpell(needsDisplayCalculations, ABar, K4);
            AddSpell(needsDisplayCalculations, MBAM, K4);

            Calculate();
        }
    }

    class AB3ABar : DynamicCycle
    {
        public AB3ABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            StaticCycle chain4;
            StaticCycle chain5;
            StaticCycle chain6;
            float K1, K2, K3, K4, K5, K6;
            Name = "AB3ABar";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            //S0:
            //AB0-AB1-AB2-ABar3                  (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            //AB0-AB1-AB2-ABar3-MBAM             (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
            //AB0-AB1-AB2-MBAM3-ABar             (1 - MB) * MB * (1 - MB)
            //AB0-AB1-AB2-MBAM3-ABar-MBAM        (1 - MB) * MB * MB
            //AB0-AB1-MBAM2-ABar                 MB * (1 - MB)
            //AB0-AB1-MBAM2-ABar-MBAM            MB * MB

            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = (1 - MB) * MB * (1 - MB);
            K4 = (1 - MB) * MB * MB;
            K5 = MB * (1 - MB);
            K6 = MB * MB;

            //AB0-AB1-AB2-ABar3
            chain1 = new StaticCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-AB2-ABar3-MBAM
            chain2 = new StaticCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain2.AddSpell(ABar3, castingState);
            chain2.AddSpell(MBAM, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar
            chain3 = new StaticCycle(5);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain3.AddSpell(ABar, castingState);
            chain3.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar-MBAM
            chain4 = new StaticCycle(6);
            chain4.AddSpell(AB0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar
            chain5 = new StaticCycle(4);
            chain5.AddSpell(AB0, castingState);
            chain5.AddSpell(AB1, castingState);
            chain5.AddSpell(MBAM2, castingState);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) chain5.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - MBAM2.CastTime - ABar.CastTime);
            chain5.AddSpell(ABar, castingState);
            chain5.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar-MBAM
            chain6 = new StaticCycle(4);
            chain6.AddSpell(AB0, castingState);
            chain6.AddSpell(AB1, castingState);
            chain6.AddSpell(MBAM2, castingState);
            if (AB0.CastTime + AB1.CastTime + MBAM2.CastTime + ABar.CastTime < 3.0) chain6.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - MBAM2.CastTime - ABar.CastTime);
            chain6.AddSpell(ABar, castingState);
            chain6.AddSpell(MBAM, castingState);
            chain6.Calculate(castingState);

            commonChain = chain1;

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            AddCycle(needsDisplayCalculations, chain3, K3);
            AddCycle(needsDisplayCalculations, chain4, K4);
            AddCycle(needsDisplayCalculations, chain5, K5);
            AddCycle(needsDisplayCalculations, chain6, K6);
            Calculate();
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class AB3ABarX : DynamicCycle
    {
        public AB3ABarX(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            StaticCycle chain4;
            float K1, K2, K3, K4;
            Name = "AB3ABarX";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            //S0:
            //AB0-AB1-AB2-ABar3                  (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            //AB0-AB1-AB2-ABar3-MBAM             (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
            //AB0-AB1-AB2-MBAM3-ABar             (1 - (1 - MB) * (1 - MB)) * (1 - MB)
            //AB0-AB1-AB2-MBAM3-ABar-MBAM        (1 - (1 - MB) * (1 - MB)) * MB

            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = (1 - (1 - MB) * (1 - MB)) * (1 - MB);
            K4 = (1 - (1 - MB) * (1 - MB)) * MB;

            //AB0-AB1-AB2-ABar3
            chain1 = new StaticCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-AB2-ABar3-MBAM
            chain2 = new StaticCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain2.AddSpell(ABar3, castingState);
            chain2.AddSpell(MBAM, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar
            chain3 = new StaticCycle(5);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain3.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain3.AddSpell(ABar, castingState);
            chain3.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar-MBAM
            chain4 = new StaticCycle(6);
            chain4.AddSpell(AB0, castingState);
            chain4.AddSpell(AB1, castingState);
            chain4.AddSpell(AB2, castingState);
            chain4.AddSpell(MBAM3, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + MBAM3.CastTime + ABar.CastTime < 3.0) chain4.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - AB2.CastTime - MBAM3.CastTime - ABar.CastTime);
            chain4.AddSpell(ABar, castingState);
            chain4.AddSpell(MBAM, castingState);
            chain4.Calculate(castingState);

            commonChain = chain1;

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            AddCycle(needsDisplayCalculations, chain3, K3);
            AddCycle(needsDisplayCalculations, chain4, K4);
            Calculate();
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class AB3ABarY : DynamicCycle
    {
        public AB3ABarY(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            StaticCycle chain5;
            float K1, K2, K3, K5;
            Name = "AB3ABarY";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            //S0:
            //AB0-AB1-AB2-ABar3                  (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB)
            //AB0-AB1-AB2-ABar3-MBAM             (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB))
            //AB0-AB1-AB2-MBAM3                  (1 - MB) * MB
            //AB0-AB1-MBAM2                      MB

            K1 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - MB) * (1 - (1 - MB) * (1 - MB));
            K3 = (1 - MB) * MB;
            K5 = MB;

            //AB0-AB1-AB2-ABar3
            chain1 = new StaticCycle(4);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain1.AddSpell(ABar3, castingState);
            chain1.Calculate(castingState);

            //AB0-AB1-AB2-ABar3-MBAM
            chain2 = new StaticCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            if (AB0.CastTime + AB1.CastTime + AB2.CastTime + ABar3.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - AB2.CastTime - ABar3.CastTime);
            chain2.AddSpell(ABar3, castingState);
            chain2.AddSpell(MBAM, castingState);
            chain2.Calculate(castingState);

            //AB0-AB1-AB2-MBAM3-ABar
            chain3 = new StaticCycle(5);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            chain3.Calculate(castingState);

            //AB0-AB1-MBAM2-ABar
            chain5 = new StaticCycle(4);
            chain5.AddSpell(AB0, castingState);
            chain5.AddSpell(AB1, castingState);
            chain5.AddSpell(MBAM2, castingState);
            chain5.Calculate(castingState);

            commonChain = chain1;

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            AddCycle(needsDisplayCalculations, chain3, K3);
            AddCycle(needsDisplayCalculations, chain5, K5);
            Calculate();
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class AB2ABar : DynamicCycle
    {
        public AB2ABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float K;
            Name = "AB2ABar";

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            {
                if (MB == 0.0f)
                {
                    Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
                    // if we don't have barrage then this degenerates to AB-AB-ABar
                    chain1 = new StaticCycle(3);
                    chain1.AddSpell(AB0, castingState);
                    chain1.AddSpell(AB1, castingState);
                    if (AB3.CastTime + AB3.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB3.CastTime - AB3.CastTime - ABar.CastTime);
                    chain1.AddSpell(ABar2, castingState);
                    chain1.Calculate(castingState);

                    commonChain = chain1;

                    AddCycle(needsDisplayCalculations, chain1, 1);

                    CastTime = chain1.CastTime;
                    costPerSecond = chain1.costPerSecond;
                    damagePerSecond = chain1.damagePerSecond;
                    threatPerSecond = chain1.threatPerSecond;
                }
                else
                {
                    //S0:
                    //AB0-AB1-ABar2 => S0 1 - MB3
                    //              => S1  MB3

                    //S1:
                    //MBAM-AB0-ABar1 => S0 1 - MB2
                    //               => S1  MB2


                    // S0 + S1 = 1
                    // S0 = S0 * (1 - MB3) + S1 * (1 - MB2)
                    // S1 = S0 * MB3 + S1 * MB2
                    // S1 * (1 - MB2) = S0 * MB3
                    // S1 * (1 - MB2) = MB3 - S1 * MB3
                    // S1 * (1 - MB2 + MB3) = MB3
                    // K2 := S1 = MB3 / (1 - MB2 + MB3)
                    // K1 := S0 = 1 - S1

                    K = (1 - MB) * (1 - MB) * (1 - MB) / (1 - (1 - MB) * (1 - MB) + (1 - MB) * (1 - MB) * (1 - MB));

                    //AB0-AB1-ABar2
                    chain1 = new StaticCycle(3);
                    chain1.AddSpell(AB0, castingState);
                    chain1.AddSpell(AB1, castingState);
                    if (AB0.CastTime + AB1.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB0.CastTime - AB1.CastTime - ABar.CastTime);
                    chain1.AddSpell(ABar2, castingState);
                    chain1.Calculate(castingState);

                    //MBAM-AB0-ABar1
                    chain2 = new StaticCycle(3);
                    chain2.AddSpell(MBAM, castingState);
                    chain2.AddSpell(AB0, castingState);
                    if (MBAM.CastTime + AB0.CastTime + ABar1.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - AB0.CastTime - ABar1.CastTime);
                    chain2.AddSpell(ABar1, castingState);
                    chain2.Calculate(castingState);

                    commonChain = chain1;

                    AddCycle(needsDisplayCalculations, chain1, 1 - K);
                    AddCycle(needsDisplayCalculations, chain2, K);
                    Calculate();
                }
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FB2ABar : DynamicCycle
    {
        public FB2ABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float K;
            Name = "FB2ABar";
            AffectedByFlameCap = true;

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0f)
            {
                // if we don't have barrage then this degenerates to AB-AB-ABar
                chain1 = new StaticCycle(3);
                chain1.AddSpell(FB, castingState);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FB.CastTime - FB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                AddCycle(needsDisplayCalculations, chain1, 1);

                CastTime = chain1.CastTime;
                costPerSecond = chain1.costPerSecond;
                damagePerSecond = chain1.damagePerSecond;
                threatPerSecond = chain1.threatPerSecond;
            }
            else
            {
                // A: MBAM-AB-ABar  (1-MB)*(1-MB) => B, 1 - (1-MB)*(1-MB) => A
                // B: AB-AB-ABar    (1-MB)*(1-MB)*(1-MB) => B, 1 - (1-MB)*(1-MB)*(1-MB) => A

                // A + B = 1
                // A = (1 - (1-MB)*(1-MB)) * A + (1 - (1-MB)*(1-MB)*(1-MB)) * B
                // B = (1-MB)*(1-MB) * A + (1-MB)*(1-MB)*(1-MB) * B

                // B * (1 + (1-MB)*(1-MB) - (1-MB)*(1-MB)*(1-MB)) = (1-MB)*(1-MB)
                // B = (1-MB)*(1-MB) / [1 + (1-MB)*(1-MB)*MB]

                //AB-ABar 0.8 * 0.8
                chain1 = new StaticCycle(3);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FB, castingState);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FB.CastTime - FB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                //ABar-MBAM
                chain2 = new StaticCycle(3);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.AddSpell(FB, castingState);
                if (FB.CastTime + MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FB.CastTime - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain1;

                K = 1 - (1 - MB) * (1 - MB) / (1 + (1 - MB) * (1 - MB) * MB);

                AddCycle(needsDisplayCalculations, chain1, 1 - K);
                AddCycle(needsDisplayCalculations, chain2, K);
                Calculate();
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FrB2ABar : DynamicCycle
    {
        public FrB2ABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float K;
            Name = "FrB2ABar";

            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0f)
            {
                // if we don't have barrage then this degenerates to AB-AB-ABar
                chain1 = new StaticCycle(3);
                chain1.AddSpell(FrB, castingState);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FrB.CastTime - FrB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                AddCycle(needsDisplayCalculations, chain1, 1);

                CastTime = chain1.CastTime;
                costPerSecond = chain1.costPerSecond;
                damagePerSecond = chain1.damagePerSecond;
                threatPerSecond = chain1.threatPerSecond;
            }
            else
            {
                // A: MBAM-AB-ABar  (1-MB)*(1-MB) => B, 1 - (1-MB)*(1-MB) => A
                // B: AB-AB-ABar    (1-MB)*(1-MB)*(1-MB) => B, 1 - (1-MB)*(1-MB)*(1-MB) => A

                // A + B = 1
                // A = (1 - (1-MB)*(1-MB)) * A + (1 - (1-MB)*(1-MB)*(1-MB)) * B
                // B = (1-MB)*(1-MB) * A + (1-MB)*(1-MB)*(1-MB) * B

                // B * (1 + (1-MB)*(1-MB) - (1-MB)*(1-MB)*(1-MB)) = (1-MB)*(1-MB)
                // B = (1-MB)*(1-MB) / [1 + (1-MB)*(1-MB)*MB]

                //AB-ABar 0.8 * 0.8
                chain1 = new StaticCycle(3);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FrB, castingState);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FrB.CastTime - FrB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                //ABar-MBAM
                chain2 = new StaticCycle(3);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                chain2.AddSpell(FrB, castingState);
                if (FrB.CastTime + MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FrB.CastTime - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain1;

                K = 1 - (1 - MB) * (1 - MB) / (1 + (1 - MB) * (1 - MB) * MB);

                AddCycle(needsDisplayCalculations, chain1, 1 - K);
                AddCycle(needsDisplayCalculations, chain2, K);
                Calculate();
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FBABar : DynamicCycle
    {
        public FBABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB;
            Name = "FBABar";
            AffectedByFlameCap = true;

            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FB-ABar
                chain1 = new StaticCycle(2);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                AddCycle(needsDisplayCalculations, chain1, 1);

                CastTime = chain1.CastTime;
                costPerSecond = chain1.costPerSecond;
                damagePerSecond = chain1.damagePerSecond;
                threatPerSecond = chain1.threatPerSecond;
            }
            else
            {
                chain1 = new StaticCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FB, castingState);
                if (FB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                chain2 = new StaticCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain2;

                AddCycle(needsDisplayCalculations, chain1, 1 - MB);
                AddCycle(needsDisplayCalculations, chain2, (2 - MB) * MB);
                Calculate();
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FrBABar : DynamicCycle
    {
        public FrBABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB;
            Name = "FrBABar";

            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FrB-ABar
                chain1 = new StaticCycle(2);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FrB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                AddCycle(needsDisplayCalculations, chain1, 1);

                CastTime = chain1.CastTime;
                costPerSecond = chain1.costPerSecond;
                damagePerSecond = chain1.damagePerSecond;
                threatPerSecond = chain1.threatPerSecond;
            }
            else
            {
                chain1 = new StaticCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FrB, castingState);
                if (FrB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FrB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                chain2 = new StaticCycle(2);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain2;

                AddCycle(needsDisplayCalculations, chain1, 1 - MB);
                AddCycle(needsDisplayCalculations, chain2, (2 - MB) * MB);
                Calculate();
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    class FFBABar : DynamicCycle
    {
        public FFBABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB;
            Name = "FFBABar";
            AffectedByFlameCap = true;

            Spell FFB = castingState.GetSpell(SpellId.FrostfireBolt);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;

            if (MB == 0.0)
            {
                // if we don't have barrage then this degenerates to FB-ABar
                chain1 = new StaticCycle(2);
                chain1.AddSpell(FFB, castingState);
                if (FFB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FFB.CastTime - ABar.CastTime);
                chain1.AddSpell(ABar, castingState);
                chain1.Calculate(castingState);

                commonChain = chain1;

                AddCycle(needsDisplayCalculations, chain1, 1);

                CastTime = chain1.CastTime;
                costPerSecond = chain1.costPerSecond;
                damagePerSecond = chain1.damagePerSecond;
                threatPerSecond = chain1.threatPerSecond;
            }
            else
            {
                chain1 = new StaticCycle(2);
                chain1.AddSpell(ABar, castingState);
                chain1.AddSpell(FFB, castingState);
                if (FFB.CastTime + ABar.CastTime < 3.0) chain1.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - FFB.CastTime - ABar.CastTime);
                chain1.Calculate(castingState);

                chain2 = new StaticCycle(4);
                chain2.AddSpell(ABar, castingState);
                chain2.AddSpell(MBAM, castingState);
                if (MBAM.CastTime + ABar.CastTime < 3.0) chain2.AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - MBAM.CastTime - ABar.CastTime);
                chain2.Calculate(castingState);

                commonChain = chain2;

                AddCycle(needsDisplayCalculations, chain1, 1 - MB);
                AddCycle(needsDisplayCalculations, chain2, (2 - MB) * MB);
                Calculate();
            }
        }

        private StaticCycle commonChain;

        public override string Sequence
        {
            get
            {
                return commonChain.Sequence;
            }
        }
    }

    /*class AB : Cycle
    {
        Spell AB3;
        StaticCycle chain1;
        StaticCycle chain3;
        StaticCycle chain4;
        Spell AB0M;
        float hit, k21, k31, k41;

        public AB(CastingState castingState) : base(castingState)
        {
            Name = "Arcane Blast";

            // main cycle is AB3 spam
            // spell miss causes debuff reset

            // RAMP =
            // AB0-AB1-AB2           hit*hit*hit = k1
            // AB0-AB1-miss-RAMP hit*hit*(1-hit) = k2
            // AB0-miss-RAMP     hit*(1-hit)     = k3
            // miss-RAMP         (1-hit)         = k4

            // RAMP = k1 * (AB0+AB1+AB2) + k2 * (AB0+AB1+AB2M + RAMP) + k3 * (AB0+AB1M + RAMP) + k4 * (AB0M + RAMP)
            // RAMP * (1 - k2 - k3 - k4) = k1 * (AB0+AB1+AB2) + k2 * (AB0+AB1+AB2M) + k3 * (AB0+AB1M) + k4 * (AB0M)
            // RAMP = (AB0+AB1+AB2) + k2 / k1 * (AB0+AB1+AB2M) + k3 / k1 * (AB0+AB1M) + k4 / k1 * (AB0M)

            // AB3           hit
            // AB3M-RAMP     (1-hit)

            AB3 = castingState.GetSpell(SpellId.ArcaneBlast33);
            hit = AB3.HitRate;

            if (AB3.HitRate >= 1.0f || 2 * AB3.CastTime < 3.0)
            {
                // if we have enough hit this is just AB3
                // if we have enough haste to get 2 ABs in 3 sec then assume we get to chain cast, can refine this if desired
                CastTime = AB3.CastTime;
                costPerSecond = AB3.CostPerSecond;
                damagePerSecond = AB3.DamagePerSecond;
                threatPerSecond = AB3.ThreatPerSecond;
            }
            else
            {
                Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0Hit);
                Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1Hit);
                Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2Hit);
                AB3 = castingState.GetSpell(SpellId.ArcaneBlast3Hit);
                AB0M = castingState.GetSpell(SpellId.ArcaneBlast0Miss);
                Spell AB1M = castingState.GetSpell(SpellId.ArcaneBlast1Miss);
                Spell AB2M = castingState.GetSpell(SpellId.ArcaneBlast2Miss);
                Spell AB3M = castingState.GetSpell(SpellId.ArcaneBlast3Miss);

                // AB3 hit

                // AB3M-RAMP (1-hit)
                chain1 = new StaticCycle(4);
                chain1.AddSpell(AB3M, castingState);
                chain1.AddSpell(AB0, castingState);
                chain1.AddSpell(AB1, castingState);
                chain1.AddSpell(AB2, castingState);
                chain1.Calculate(castingState);

                chain3 = new StaticCycle(3);
                chain3.AddSpell(AB0, castingState);
                chain3.AddSpell(AB1, castingState);
                chain3.AddSpell(AB2M, castingState);
                chain3.Calculate(castingState);

                chain4 = new StaticCycle(2);
                chain4.AddSpell(AB0, castingState);
                chain4.AddSpell(AB1M, castingState);
                chain4.Calculate(castingState);

                k21 = (1 - hit) / hit;
                k31 = (1 - hit) / hit / hit;
                k41 = (1 - hit) / hit / hit / hit;

                CastTime = hit * AB3.CastTime + (1 - hit) * (chain1.CastTime + k21 * chain3.CastTime + k31 * chain4.CastTime + k41 * AB0M.CastTime);
                costPerSecond = (hit * AB3.CostPerSecond * AB3.CastTime + (1 - hit) * (chain1.costPerSecond * chain1.CastTime + k21 * chain3.costPerSecond * chain3.CastTime + k31 * chain4.costPerSecond * chain4.CastTime + k41 * AB0M.CostPerSecond * AB0M.CastTime)) / CastTime;
                damagePerSecond = (hit * AB3.DamagePerSecond * AB3.CastTime + (1 - hit) * (chain1.damagePerSecond * chain1.CastTime + k21 * chain3.damagePerSecond * chain3.CastTime + k31 * chain4.damagePerSecond * chain4.CastTime + k41 * AB0M.DamagePerSecond * AB0M.CastTime)) / CastTime;
                threatPerSecond = (hit * AB3.ThreatPerSecond * AB3.CastTime + (1 - hit) * (chain1.threatPerSecond * chain1.CastTime + k21 * chain3.threatPerSecond * chain3.CastTime + k31 * chain4.threatPerSecond * chain4.CastTime + k41 * AB0M.ThreatPerSecond * AB0M.CastTime)) / CastTime;
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration)
        {
            if (AB3.HitRate >= 1.0f || 2 * AB3.CastTime < 3.0)
            {
                AB3.AddSpellContribution(dict, duration);
            }
            else
            {
                AB3.AddSpellContribution(dict, duration * hit * AB3.CastTime / CastTime);
                chain1.AddSpellContribution(dict, duration * (1 - hit) * chain1.CastTime / CastTime);
                chain3.AddSpellContribution(dict, duration * (1 - hit) * k21 * chain3.CastTime / CastTime);
                chain4.AddSpellContribution(dict, duration * (1 - hit) * k31 * chain4.CastTime / CastTime);
                AB0M.AddSpellContribution(dict, duration * (1 - hit) * k41 * AB0M.CastTime / CastTime);
            }
        }

        public override void AddManaSourcesContribution(Dictionary<string, float> dict, float duration)
        {
            throw new NotImplementedException();
        }

        public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            throw new NotImplementedException();
        }
    }*/

    class ABSpamMBAM : DynamicCycle
    {
        public ABSpamMBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell AB3;
            float MB, MB3, MB4, MB5, hit, miss;
            Name = "ABSpamMBAM";

            // main cycle is AB3 spam
            // on MB we change into ramp up mode

            // RAMP =
            // AB0-AB1-AB2           0.85*0.85*0.85 = k1
            // AB0-AB1-AB2-(AB3-)MBAM-RAMP 0.85*0.85*0.15 = k2
            // AB0-AB1-(AB2-)MBAM-RAMP     0.85*0.15      = k3
            // AB0-(AB1-)MBAM-RAMP         0.15           = k4

            // RAMP = k1 * (AB0+AB1+AB2) + k2 * (AB0+AB1+AB2+MBAM + RAMP) + k3 * (AB0+AB1+MBAM + RAMP) + k4 * (AB0+MBAM + RAMP)
            // RAMP * (1 - k2 - k3 - k4) = k1 * (AB0+AB1+AB2) + k2 * (AB0+AB1+AB2+MBAM) + k3 * (AB0+AB1+MBAM) + k4 * (AB0+MBAM)
            // RAMP = (AB0+AB1+AB2) + k2 / k1 * (AB0+AB1+AB2+MBAM) + k3 / k1 * (AB0+AB1+MBAM) + k4 / k1 * (AB0+MBAM)

            // RAMP =
            // AB0H-AB1H-AB2H                 0.85*hit*0.85*hit*0.85*hit = k1
            // AB0H-AB1H-AB2H-(AB3-)MBAM-RAMP 0.85*hit*0.85*hit*0.15*hit = k2
            // AB0H-AB1H-(AB2-)MBAM-RAMP      0.85*hit*0.15*hit          = k3
            // AB0H-(AB1-)MBAM-RAMP           0.15*hit                   = k4
            // AB0H-AB1H-AB2M-RAMP            0.85*hit*0.85*hit*miss     = k5
            // AB0H-AB1M-RAMP                 0.85*hit*miss              = k6
            // AB0M-RAMP                      miss                       = k7

            // RAMP = k1 * (AB0H+AB1H+AB2H) + k2 * (AB0H+AB1H+AB2H+AB3+MBAM + RAMP) + k3 * (AB0H+AB1H+AB2+MBAM + RAMP) + k4 * (AB0H+AB1+MBAM + RAMP) + k5 * (AB0H+AB1H+AB2M + RAMP) + k6 * (AB0H+AB1M + RAMP) + k7 * (AB0M + RAMP)
            // RAMP = (AB0H+AB1H+AB2H) + k2 / k1 * (AB0H+AB1H+AB2H+AB3+MBAM) + k3 / k1 * (AB0H+AB1H+AB2+MBAM) + k4 / k1 * (AB0H+AB1+MBAM) + k5 / k1 * (AB0H+AB1H+AB2M) + k6 / k1 * (AB0H+AB1M) + k7 / k1 * (AB0M)

            // AB3H                 0.85*hit
            // AB3H-(AB3-)MBAM-RAMP 0.15*hit
            // AB3M-RAMP            (1-hit)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
            Spell MBAM = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            hit = AB3.HitRate;
            miss = 1 - hit;

            if (MB == 0.0)
            {
                // TODO take hit rate into account
                // if we don't have barrage then this degenerates to AB

                AddSpell(needsDisplayCalculations, AB3, 1);
                Calculate();
            }
            else
            {
                MB3 = MB / (1 - MB);
                MB4 = MB / (1 - MB) / (1 - MB);
                MB5 = MB / (1 - MB) / (1 - MB) / (1 - MB);

                //AB3 0.85

                //AB3-MBAM-RAMP 0.15
                AddSpell(needsDisplayCalculations, AB3, MB);
                AddSpell(needsDisplayCalculations, AB3, MB); // account for latency
                AddSpell(needsDisplayCalculations, MBAM3, MB);
                AddSpell(needsDisplayCalculations, AB0, MB);
                AddSpell(needsDisplayCalculations, AB1, MB);
                AddSpell(needsDisplayCalculations, AB2, MB);

                AddSpell(needsDisplayCalculations, AB0, MB * MB3);
                AddSpell(needsDisplayCalculations, AB1, MB * MB3);
                AddSpell(needsDisplayCalculations, AB2, MB * MB3);
                AddSpell(needsDisplayCalculations, AB3, MB * MB3); // account for latency
                AddSpell(needsDisplayCalculations, MBAM3, MB * MB3);

                AddSpell(needsDisplayCalculations, AB0, MB * MB4);
                AddSpell(needsDisplayCalculations, AB1, MB * MB4);
                AddSpell(needsDisplayCalculations, AB2, MB * MB4); // account for latency
                AddSpell(needsDisplayCalculations, MBAM3, MB * MB4);

                AddSpell(needsDisplayCalculations, AB0, MB * MB5);
                AddSpell(needsDisplayCalculations, AB1, MB * MB5); // account for latency
                AddSpell(needsDisplayCalculations, MBAM2, MB * MB5);

                AddSpell(needsDisplayCalculations, AB3, 1 - MB);

                Calculate();
            }
        }
    }

    class AB32AMABar : DynamicCycle
    {
        public AB32AMABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            StaticCycle chain3;
            float MB, K1, K2, K3;
            Name = "AB32AMABar";

            // ABar-AB0-AB1-MBAM       1 - (1-MB)*(1-MB)
            // ABar-AB0-AB1-AB2-AM     (1-MB)*(1-MB)*(1-MB)*(1-MB)
            // ABar-AB0-AB1-AB2-MBAM   (1-MB)*(1-MB)*(1 - (1-MB)*(1-MB))

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            //Spell AM3C = castingState.GetSpell(SpellId.ArcaneMissiles3Clipped);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            //Spell MBAM2C = castingState.GetSpell(SpellId.ArcaneMissilesMB2Clipped);
            Spell MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);
            //Spell MBAM3C = castingState.GetSpell(SpellId.ArcaneMissilesMB3Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            //Spell ABar2C = castingState.GetSpell(SpellId.ArcaneBarrage2Combo);
            Spell ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            //Spell ABar3C = castingState.GetSpell(SpellId.ArcaneBarrage3Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = 1 - (1 - MB) * (1 - MB);
            K2 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            K3 = (1 - (1 - MB) * (1 - MB)) * (1 - MB) * (1 - MB);

            chain1 = new StaticCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(MBAM2, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new StaticCycle(5);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AB2, castingState);
            chain2.AddSpell(AM3, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            chain3 = new StaticCycle(6);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AB1, castingState);
            chain3.AddSpell(AB2, castingState);
            chain3.AddSpell(MBAM3, castingState);
            chain3.AddSpell(ABar, castingState);
            chain3.Calculate(castingState);

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            AddCycle(needsDisplayCalculations, chain3, K3);
            Calculate();
        }
    }

    class AB2AMABar : DynamicCycle
    {
        public AB2AMABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB, K1, K2;
            Name = "AB2AMABar";

            // ABar-AB0-AB1-AM     (1-MB)*(1-MB)*(1-MB)
            // ABar-AB0-AB1-MBAM   1 - (1-MB)*(1-MB)*(1-MB)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
            //Spell AM2C = castingState.GetSpell(SpellId.ArcaneMissiles2Clipped);
            Spell MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            //Spell MBAM2C = castingState.GetSpell(SpellId.ArcaneMissilesMB2Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            //Spell ABar2C = castingState.GetSpell(SpellId.ArcaneBarrage2Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB) * (1 - MB);
            K2 = (1 - (1 - MB) * (1 - MB) * (1 - MB));

            chain1 = new StaticCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AM2, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new StaticCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(MBAM2, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            Calculate();
        }
    }

    class ABAMABar : DynamicCycle
    {
        public ABAMABar(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            StaticCycle chain1;
            StaticCycle chain2;
            float MB, K1, K2;
            Name = "ABAMABar";

            // ABar-AB0-AM     (1-MB)*(1-MB)
            // ABar-AB0-MBAM   1 - (1-MB)*(1-MB)

            Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell AM1 = castingState.GetSpell(SpellId.ArcaneMissiles1);
            //Spell AM1C = castingState.GetSpell(SpellId.ArcaneMissiles1Clipped);
            Spell MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);
            //Spell MBAM1C = castingState.GetSpell(SpellId.ArcaneMissilesMB1Clipped);
            Spell ABar = castingState.GetSpell(SpellId.ArcaneBarrage);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            //Spell ABar1C = castingState.GetSpell(SpellId.ArcaneBarrage1Combo);

            MB = 0.04f * castingState.MageTalents.MissileBarrage;
            K1 = (1 - MB) * (1 - MB);
            K2 = (1 - (1 - MB) * (1 - MB));

            chain1 = new StaticCycle(5);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AM1, castingState);
            chain1.AddSpell(ABar, castingState);
            chain1.Calculate(castingState);

            chain2 = new StaticCycle(6);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(MBAM1, castingState);
            chain2.AddSpell(ABar, castingState);
            chain2.Calculate(castingState);

            AddCycle(needsDisplayCalculations, chain1, K1);
            AddCycle(needsDisplayCalculations, chain2, K2);
            Calculate();
        }
    }

    /*class AB3AMSc : StaticCycle
    {
        public AB3AMSc(CastingState castingState) : base(12)
        {
            Name = "AB3AMSc";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB01 = castingState.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = castingState.GetSpell(SpellId.ArcaneBlast12);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(AB01, castingState);
            AddSpell(AB12, castingState);
            AddSpell(AM, castingState);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= Sc.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }*/

    class ABABar : StaticCycle
    {
        public ABABar(CastingState castingState)
            : base(3)
        {
            Name = "ABABar";

            Spell AB = castingState.GetSpell(SpellId.ArcaneBlast0);
            Spell ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);

            AddSpell(AB, castingState);
            if (AB.CastTime + ABar1.CastTime < 3.0) AddPause(3.0f + castingState.CalculationOptions.LatencyGCD - AB.CastTime - ABar1.CastTime);
            AddSpell(ABar1, castingState);

            Calculate(castingState);
        }
    }

    /*class ABAM3Sc : StaticCycle
    {
        public ABAM3Sc(CastingState castingState) : base(14)
        {
            Name = "ABAM3Sc";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB11, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB22, castingState);
            AddSpell(AM, castingState);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= Sc.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABAM3Sc2 : StaticCycle
    {
        public ABAM3Sc2(CastingState castingState) : base(14)
        {
            Name = "ABAM3Sc2";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB11, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB22, castingState);
            AddSpell(AM, castingState);
            float gap = 8 - AM.CastTime;
            while (gap >= AB30.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABAM3FrB : StaticCycle
    {
        public ABAM3FrB(CastingState castingState) : base(14)
        {
            Name = "ABAM3FrB";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB11, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB22, castingState);
            AddSpell(AM, castingState);
            float gap = 8 - AM.CastTime;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABAM3FrB2 : StaticCycle
    {
        public ABAM3FrB2(CastingState castingState) : base(14)
        {
            Name = "ABAM3FrB2";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB11, castingState);
            AddSpell(AM, castingState);
            AddSpell(AB22, castingState);
            AddSpell(AM, castingState);
            float gap = 8 - AM.CastTime;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class AB3FrB : StaticCycle
    {
        public AB3FrB(CastingState castingState) : base(11)
        {
            Name = "AB3FrB";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB01 = castingState.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = castingState.GetSpell(SpellId.ArcaneBlast12);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, castingState);
            AddSpell(AB01, castingState);
            AddSpell(AB12, castingState);
            float gap = 8;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABFrB : StaticCycle
    {
        public ABFrB(CastingState castingState)
            : base(13)
        {
            Name = "ABFrB";

            Spell AB10 = castingState.GetSpell(SpellId.ArcaneBlast10);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB10, castingState);
            float gap = 8;
            while (gap - AB10.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB10.CastTime < gap) AddPause(gap - AB10.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABFrB3FrB : StaticCycle
    {
        public ABFrB3FrB(CastingState castingState) : base(13)
        {
            Name = "ABFrB3FrB";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB11, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB22, castingState);
            float gap = 8;
            while (gap - AB30.CastTime >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABFrB3FrB2 : StaticCycle
    {
        public ABFrB3FrB2(CastingState castingState) : base(13)
        {
            Name = "ABFrB3FrB2";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

            AddSpell(AB30, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB11, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB22, castingState);
            float gap = 8;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABFrB3FrBSc : StaticCycle
    {
        public ABFrB3FrBSc(CastingState castingState) : base(13)
        {
            Name = "ABFrB3FrBSc";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB11, castingState);
            AddSpell(FrB, castingState);
            AddSpell(AB22, castingState);
            float gap = 8;
            while (gap >= FrB.CastTime)
            {
                AddSpell(FrB, castingState);
                gap -= FrB.CastTime;
            }
            while (gap >= Sc.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class ABFB3FBSc : StaticCycle
    {
        public ABFB3FBSc(CastingState castingState) : base(13)
        {
            Name = "ABFB3FBSc";
            AffectedByFlameCap = true;

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB11 = castingState.GetSpell(SpellId.ArcaneBlast1);
            Spell AB22 = castingState.GetSpell(SpellId.ArcaneBlast2);
            Spell FB = castingState.GetSpell(SpellId.Fireball);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(FB, castingState);
            AddSpell(AB11, castingState);
            AddSpell(FB, castingState);
            AddSpell(AB22, castingState);
            float gap = 8;
            while (gap >= FB.CastTime)
            {
                AddSpell(FB, castingState);
                gap -= FB.CastTime;
            }
            while (gap >= Sc.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }

    class AB3Sc : StaticCycle
    {
        public AB3Sc(CastingState castingState) : base(11)
        {
            Name = "AB3Sc";

            Spell AB30 = castingState.GetSpell(SpellId.ArcaneBlast30);
            Spell AB01 = castingState.GetSpell(SpellId.ArcaneBlast01);
            Spell AB12 = castingState.GetSpell(SpellId.ArcaneBlast12);
            Spell Sc = castingState.GetSpell(SpellId.Scorch);

            AddSpell(AB30, castingState);
            AddSpell(AB01, castingState);
            AddSpell(AB12, castingState);
            float gap = 8;
            while (gap >= Sc.CastTime)
            {
                AddSpell(Sc, castingState);
                gap -= Sc.CastTime;
            }
            if (AB30.CastTime < gap) AddPause(gap - AB30.CastTime + castingState.Latency);

            Calculate(castingState);
        }
    }*/

    class ABABarSc : DynamicCycle
    {
        public ABABarSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle ABABar;
            Spell Sc;
            float X;
            Name = "ABABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            ABABar = castingState.GetCycle(CycleId.ABABar0C);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = ABABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap));

            AddCycle(needsDisplayCalculations, ABABar, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class ABABarCSc : DynamicCycle
    {
        public ABABarCSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle ABABarC;
            Spell Sc;
            float X;
            Name = "ABABarCSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            ABABarC = castingState.GetCycle(CycleId.ABABar1C);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = ABABarC.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABarC.CastTime * (1 - gap));

            AddCycle(needsDisplayCalculations, ABABarC, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class ABAMABarSc : DynamicCycle
    {
        public ABAMABarSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle ABAMABar;
            Spell Sc;
            float X;
            Name = "ABAMABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            ABAMABar = castingState.GetCycle(CycleId.ABAMABar);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = ABAMABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + ABAMABar.CastTime * (1 - gap));

            AddCycle(needsDisplayCalculations, ABAMABar, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class AB3AMABarSc : DynamicCycle
    {
        public AB3AMABarSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle AB3AMABar;
            Spell Sc;
            float X;
            Name = "AB3AMABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            AB3AMABar = castingState.GetCycle(CycleId.AB3AMABar);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = AB3AMABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + AB3AMABar.CastTime * (1 - gap));

            AddCycle(needsDisplayCalculations, AB3AMABar, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class AB3ABarCSc : DynamicCycle
    {
        public AB3ABarCSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle AB3ABarC;
            Spell Sc;
            float X;
            Name = "AB3ABarCSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            AB3ABarC = castingState.GetCycle(CycleId.AB3ABar3C);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = AB3ABarC.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + AB3ABarC.CastTime * (1 - gap));

            AddCycle(needsDisplayCalculations, AB3ABarC, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    class AB3MBAMABarSc : DynamicCycle
    {
        public AB3MBAMABarSc(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Cycle AB3MBAMABar;
            Spell Sc;
            float X;
            Name = "AB3MBAMABarSc";
            ProvidesScorch = true;
            AffectedByFlameCap = true;

            AB3MBAMABar = castingState.GetCycle(CycleId.ABSpam3C);
            Sc = castingState.GetSpell(SpellId.Scorch);
            sequence = AB3MBAMABar.Sequence;

            int averageScorchesNeeded = (int)Math.Ceiling(3f / (float)castingState.MageTalents.ImprovedScorch);
            int extraScorches = 1;
            if (Sc.HitRate >= 1.0) extraScorches = 0;
            if (castingState.MageTalents.GlyphOfImprovedScorch)
            {
                averageScorchesNeeded = 1;
                extraScorches = 0;
            }

            float gap = (30.0f - (averageScorchesNeeded + extraScorches) * Sc.CastTime) / (30.0f - extraScorches * Sc.CastTime);
            if (castingState.MageTalents.ImprovedScorch == 0)
            {
                ProvidesScorch = false;
                gap = 1.0f;
            }

            // gap = X * ABABar.CastTime / (X * ABABar.CastTime + (1 - X) * Sc.CastTime)
            // gap * (X * ABABar.CastTime + (1 - X) * Sc.CastTime) = X * ABABar.CastTime
            // gap * X * ABABar.CastTime + gap * Sc.CastTime - gap * X * Sc.CastTime = X * ABABar.CastTime
            // X * (gap * ABABar.CastTime - gap * Sc.CastTime - ABABar.CastTime) + gap * Sc.CastTime = 0
            // X = gap * Sc.CastTime / (gap * Sc.CastTime + ABABar.CastTime * (1 - gap))

            X = gap * Sc.CastTime / (gap * Sc.CastTime + AB3MBAMABar.CastTime * (1 - gap));

            AddCycle(needsDisplayCalculations, AB3MBAMABar, X);
            AddSpell(needsDisplayCalculations, Sc, 1 - X);
            Calculate();
        }
    }

    /*class ABAM3ScCCAM : DynamicCycle
{
    StaticCycle chain1;
    StaticCycle chain2;
    StaticCycle chain3;
    StaticCycle chain4;
    float CC;

    public ABAM3ScCCAM(CastingState castingState) : base(4)
    {
        Name = "ABAM3ScCC";

        //AMCC-AB0                       0.1
        //AM?0-AB1-AMCC-AB0              0.9*0.1
        //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
        //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

        //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
        //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
        //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
        //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

        Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
        Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
        Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
        Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
        Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
        Spell Sc0 = castingState.GetSpell(SpellId.ScorchNoCC);

        Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
        Spell Sc = castingState.GetSpell(SpellId.Scorch);

        CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

        //AMCC-AB0                       0.1
        chain1 = new StaticCycle(2);
        chain1.AddSpell(AMCC, castingState);
        chain1.AddSpell(AB0, castingState);
        chain1.Calculate(castingState);

        //AM?0-AB1-AMCC-AB0              0.9*0.1
        chain2 = new StaticCycle(4);
        chain2.AddSpell(AMc0, castingState);
        chain2.AddSpell(AB1, castingState);
        chain2.AddSpell(AMCC, castingState);
        chain2.AddSpell(AB0, castingState);
        chain2.Calculate(castingState);

        //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
        chain3 = new StaticCycle(6);
        chain3.AddSpell(AMc0, castingState);
        chain3.AddSpell(AB1, castingState);
        chain3.AddSpell(AMc0, castingState);
        chain3.AddSpell(AB2, castingState);
        chain3.AddSpell(AMCC, castingState);
        chain3.AddSpell(AB0, castingState);
        chain3.Calculate(castingState);

        //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
        chain4 = new StaticCycle(13);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(AB1, castingState);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(AB2, castingState);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(Sc0, castingState);
        float gap = 8 - AMc0.CastTime - Sc0.CastTime;
        while (gap - AB3.CastTime >= Sc.CastTime)
        {
            chain4.AddSpell(Sc, castingState);
            gap -= Sc.CastTime;
        }
        if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + castingState.Latency);
        chain4.AddSpell(AB3, castingState);
        chain4.Calculate(castingState);

        Cycle[0] = chain1;
        Cycle[1] = chain2;
        Cycle[2] = chain3;
        Cycle[3] = chain4;
        Weight[0] = CC;
        Weight[1] = CC * (1 - CC);
        Weight[2] = CC * (1 - CC) * (1 - CC);
        Weight[3] = (1 - CC) * (1 - CC) * (1 - CC);
        Calculate();

        commonChain = chain4;
    }

    private StaticCycle commonChain;

    public override string Sequence
    {
        get
        {
            return commonChain.Sequence;
        }
    }
}

class ABAM3Sc2CCAM : DynamicCycle
{
    StaticCycle chain1;
    StaticCycle chain2;
    StaticCycle chain3;
    StaticCycle chain4;
    float CC;

    public ABAM3Sc2CCAM(CastingState castingState) : base(4)
    {
        Name = "ABAM3Sc2CC";

        //AMCC-AB0                       0.1
        //AM?0-AB1-AMCC-AB0              0.9*0.1
        //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
        //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

        //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
        //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
        //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
        //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

        Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
        Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
        Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
        Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
        Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
        Spell Sc0 = castingState.GetSpell(SpellId.ScorchNoCC);

        Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
        Spell Sc = castingState.GetSpell(SpellId.Scorch);

        CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

        //AMCC-AB0                       0.1
        chain1 = new StaticCycle(2);
        chain1.AddSpell(AMCC, castingState);
        chain1.AddSpell(AB0, castingState);
        chain1.Calculate(castingState);

        //AM?0-AB1-AMCC-AB0              0.9*0.1
        chain2 = new StaticCycle(4);
        chain2.AddSpell(AMc0, castingState);
        chain2.AddSpell(AB1, castingState);
        chain2.AddSpell(AMCC, castingState);
        chain2.AddSpell(AB0, castingState);
        chain2.Calculate(castingState);

        //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
        chain3 = new StaticCycle(13);
        chain3.AddSpell(AMc0, castingState);
        chain3.AddSpell(AB1, castingState);
        chain3.AddSpell(AMc0, castingState);
        chain3.AddSpell(AB2, castingState);
        chain3.AddSpell(AMCC, castingState);
        chain3.AddSpell(AB0, castingState);
        chain3.Calculate(castingState);

        //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
        chain4 = new StaticCycle();
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(AB1, castingState);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(AB2, castingState);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(Sc0, castingState);
        float gap = 8 - AMc0.CastTime - Sc0.CastTime;
        while (gap >= Sc.CastTime)
        {
            chain4.AddSpell(Sc, castingState);
            gap -= Sc.CastTime;
        }
        if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + castingState.Latency);
        chain4.AddSpell(AB3, castingState);
        chain4.Calculate(castingState);

        Cycle[0] = chain1;
        Cycle[1] = chain2;
        Cycle[2] = chain3;
        Cycle[3] = chain4;
        Weight[0] = CC;
        Weight[1] = CC * (1 - CC);
        Weight[2] = CC * (1 - CC) * (1 - CC);
        Weight[3] = (1 - CC) * (1 - CC) * (1 - CC);
        Calculate();

        commonChain = chain4;
    }

    private StaticCycle commonChain;

    public override string Sequence
    {
        get
        {
            return commonChain.Sequence;
        }
    }
}

class ABAM3FrBCCAM : DynamicCycle
{
    StaticCycle chain1;
    StaticCycle chain2;
    StaticCycle chain3;
    StaticCycle chain4;
    float CC;

    public ABAM3FrBCCAM(CastingState castingState) : base(4)
    {
        Name = "ABAM3FrBCC";

        //AMCC-AB0                       0.1
        //AM?0-AB1-AMCC-AB0              0.9*0.1
        //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
        //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

        //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
        //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
        //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
        //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

        Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
        Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
        Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
        Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
        Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
        Spell FrB0 = castingState.GetSpell(SpellId.FrostboltNoCC);

        Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
        Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

        CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

        //AMCC-AB0                       0.1
        chain1 = new StaticCycle(2);
        chain1.AddSpell(AMCC, castingState);
        chain1.AddSpell(AB0, castingState);
        chain1.Calculate(castingState);

        //AM?0-AB1-AMCC-AB0              0.9*0.1
        chain2 = new StaticCycle(4);
        chain2.AddSpell(AMc0, castingState);
        chain2.AddSpell(AB1, castingState);
        chain2.AddSpell(AMCC, castingState);
        chain2.AddSpell(AB0, castingState);
        chain2.Calculate(castingState);

        //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
        chain3 = new StaticCycle(6);
        chain3.AddSpell(AMc0, castingState);
        chain3.AddSpell(AB1, castingState);
        chain3.AddSpell(AMc0, castingState);
        chain3.AddSpell(AB2, castingState);
        chain3.AddSpell(AMCC, castingState);
        chain3.AddSpell(AB0, castingState);
        chain3.Calculate(castingState);

        //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
        chain4 = new StaticCycle(13);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(AB1, castingState);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(AB2, castingState);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(FrB0, castingState);
        float gap = 8 - AMc0.CastTime - FrB0.CastTime;
        while (gap - AB3.CastTime >= FrB.CastTime)
        {
            chain4.AddSpell(FrB, castingState);
            gap -= FrB.CastTime;
        }
        if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + castingState.Latency);
        chain4.AddSpell(AB3, castingState);
        chain4.Calculate(castingState);

        Cycle[0] = chain1;
        Cycle[1] = chain2;
        Cycle[2] = chain3;
        Cycle[3] = chain4;
        Weight[0] = CC;
        Weight[1] = CC * (1 - CC);
        Weight[2] = CC * (1 - CC) * (1 - CC);
        Weight[3] = (1 - CC) * (1 - CC) * (1 - CC);
        Calculate();

        commonChain = chain4;
    }

    private StaticCycle commonChain;

    public override string Sequence
    {
        get
        {
            return commonChain.Sequence;
        }
    }
}

class ABAM3FrBCCAMFail : DynamicCycle
{
    StaticCycle chain1;
    StaticCycle chain2;
    StaticCycle chain3;
    StaticCycle chain4;
    float CC;

    public ABAM3FrBCCAMFail(CastingState castingState) : base(4)
    {
        Name = "ABAM3FrBCCFail";

        //AMCC-AB0                       0.1
        //AM?0-AB1-AMCC-AB0              0.9*0.1
        //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
        //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

        //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
        //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
        //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
        //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

        Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
        Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
        Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
        Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
        Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
        Spell FrB0 = castingState.GetSpell(SpellId.FrostboltNoCC);

        Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast00);
        Spell FrB = castingState.GetSpell(SpellId.Frostbolt);

        CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

        //AMCC-AB0                       0.1
        chain1 = new StaticCycle(2);
        chain1.AddSpell(AMCC, castingState);
        chain1.AddSpell(AB0, castingState);
        chain1.Calculate(castingState);

        //AM?0-AB1-AMCC-AB0              0.9*0.1
        chain2 = new StaticCycle(4);
        chain2.AddSpell(AMc0, castingState);
        chain2.AddSpell(AB1, castingState);
        chain2.AddSpell(AMCC, castingState);
        chain2.AddSpell(AB0, castingState);
        chain2.Calculate(castingState);

        //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
        chain3 = new StaticCycle(6);
        chain3.AddSpell(AMc0, castingState);
        chain3.AddSpell(AB1, castingState);
        chain3.AddSpell(AMc0, castingState);
        chain3.AddSpell(AB2, castingState);
        chain3.AddSpell(AMCC, castingState);
        chain3.AddSpell(AB0, castingState);
        chain3.Calculate(castingState);

        //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
        chain4 = new StaticCycle(13);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(AB1, castingState);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(AB2, castingState);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(FrB0, castingState);
        float gap = 8 - AMc0.CastTime - FrB0.CastTime;
        while (gap - AB3.CastTime >= FrB.CastTime)
        {
            chain4.AddSpell(FrB, castingState);
            gap -= FrB.CastTime;
        }
        if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + castingState.Latency);
        chain4.AddSpell(AB3, castingState);
        chain4.Calculate(castingState);

        Cycle[0] = chain1;
        Cycle[1] = chain2;
        Cycle[2] = chain3;
        Cycle[3] = chain4;
        Weight[0] = CC;
        Weight[1] = CC * (1 - CC);
        Weight[2] = CC * (1 - CC) * (1 - CC);
        Weight[3] = (1 - CC) * (1 - CC) * (1 - CC);
        Calculate();

        commonChain = chain4;
    }

    private StaticCycle commonChain;

    public override string Sequence
    {
        get
        {
            return commonChain.Sequence;
        }
    }
}

class ABAM3FrBScCCAM : DynamicCycle
{
    StaticCycle chain1;
    StaticCycle chain2;
    StaticCycle chain3;
    StaticCycle chain4;
    float CC;

    public ABAM3FrBScCCAM(CastingState castingState) : base(4)
    {
        Name = "ABAM3FrBScCC";

        //AMCC-AB0                       0.1
        //AM?0-AB1-AMCC-AB0              0.9*0.1
        //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
        //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9

        //TIME = 0.1*[AMCC+AB0] + 0.9*0.1*[AM+AMCC+AB0+AB1] + 0.9*0.9*0.1*[2*AM+AMCC+AB0+AB1+AB2] + 0.9*0.9*0.9*[3*AM+AB1+AB2+AB3?]
        //     = [0.1 + 0.9*0.1 + 0.9*0.9*0.1]*[AMCC+AB0] + [0.9*0.1 + 2*0.9*0.9*0.1 + 3*0.9*0.9*0.9]*AM + 0.9*AB1 + 0.9*0.9*AB2 + 0.9*0.9*0.9*[S+AB3?]
        //     = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]
        //DAMAGE = 0.271*[AMCC+AB0] + 2.439*AM + 0.9*AB1 + 0.81*AB2 + 0.729*[S+AB3?]

        Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
        Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
        Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
        Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
        Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
        Spell FrB0 = castingState.GetSpell(SpellId.FrostboltNoCC);

        Spell AM = castingState.GetSpell(SpellId.ArcaneMissiles);
        Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast30);
        Spell FrB = castingState.GetSpell(SpellId.Frostbolt);
        Spell Sc = castingState.GetSpell(SpellId.Scorch);

        CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

        //AMCC-AB0                       0.1
        chain1 = new StaticCycle(2);
        chain1.AddSpell(AMCC, castingState);
        chain1.AddSpell(AB0, castingState);
        chain1.Calculate(castingState);

        //AM?0-AB1-AMCC-AB0              0.9*0.1
        chain2 = new StaticCycle(4);
        chain2.AddSpell(AMc0, castingState);
        chain2.AddSpell(AB1, castingState);
        chain2.AddSpell(AMCC, castingState);
        chain2.AddSpell(AB0, castingState);
        chain2.Calculate(castingState);

        //AM?0-AB1-AM?0-AB2-AMCC-AB0     0.9*0.9*0.1
        chain3 = new StaticCycle(6);
        chain3.AddSpell(AMc0, castingState);
        chain3.AddSpell(AB1, castingState);
        chain3.AddSpell(AMc0, castingState);
        chain3.AddSpell(AB2, castingState);
        chain3.AddSpell(AMCC, castingState);
        chain3.AddSpell(AB0, castingState);
        chain3.Calculate(castingState);

        //AM?0-AB1-AM?0-AB2-AM?0-S-AB3?  0.9*0.9*0.9
        chain4 = new StaticCycle(13);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(AB1, castingState);
        chain4.AddSpell(AMc0, castingState);
        chain4.AddSpell(AB2, castingState);
        chain4.AddSpell(AMc0, castingState);
        float gap = 8 - AMc0.CastTime;
        bool extraAM = false;
        while (gap >= AM.CastTime)
        {
            chain4.AddSpell(AM, castingState);
            gap -= AM.CastTime;
            extraAM = true;
        }
        if (!extraAM)
        {
            chain4.AddSpell(FrB0, castingState);
            gap -= FrB0.CastTime;
        }
        while (gap >= FrB.CastTime)
        {
            chain4.AddSpell(FrB, castingState);
            gap -= FrB.CastTime;
        }
        while (gap >= Sc.CastTime)
        {
            chain4.AddSpell(Sc, castingState);
            gap -= Sc.CastTime;
        }
        if (AB3.CastTime < gap) chain4.AddPause(gap - AB3.CastTime + castingState.Latency);
        chain4.AddSpell(AB3, castingState);
        chain4.Calculate(castingState);

        Cycle[0] = chain1;
        Cycle[1] = chain2;
        Cycle[2] = chain3;
        Cycle[3] = chain4;
        Weight[0] = CC;
        Weight[1] = CC * (1 - CC);
        Weight[2] = CC * (1 - CC) * (1 - CC);
        Weight[3] = (1 - CC) * (1 - CC) * (1 - CC);
        Calculate();

        commonChain = chain4;
    }

    private StaticCycle commonChain;

    public override string Sequence
    {
        get
        {
            return commonChain.Sequence;
        }
    }
}

class ABAMCCAM : DynamicCycle
{
    StaticCycle chain1;
    StaticCycle chain2;
    float CC;

    public ABAMCCAM(CastingState castingState) : base(2)
    {
        Name = "ABAMCC";

        //AMCC-AB00-AB01-AB12-AB23       0.1
        //AM?0-AB33                      0.9

        Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
        Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
        Spell AB00 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
        Spell AB01 = castingState.GetSpell(SpellId.ArcaneBlast01);
        Spell AB12 = castingState.GetSpell(SpellId.ArcaneBlast12);
        Spell AB23 = castingState.GetSpell(SpellId.ArcaneBlast23);
        Spell AB33 = castingState.GetSpell(SpellId.ArcaneBlast33NoCC);

        CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

        if (CC == 0)
        {
            // if we don't have clearcasting then this degenerates to AMc0-AB33
            chain1 = new StaticCycle(2);
            chain1.AddSpell(AMc0, castingState);
            chain1.AddSpell(AB33, castingState);
            chain1.Calculate(castingState);

            Cycle[0] = chain1;
            Weight[0] = 1;

            CastTime = chain1.CastTime;
            CostPerSecond = chain1.CostPerSecond;
            DamagePerSecond = chain1.DamagePerSecond;
            ThreatPerSecond = chain1.ThreatPerSecond;

            commonChain = chain1;
        }
        else
        {

            //AMCC-AB00-AB01-AB12-AB23       0.1
            chain1 = new StaticCycle(5);
            chain1.AddSpell(AMCC, castingState);
            chain1.AddSpell(AB00, castingState);
            chain1.AddSpell(AB01, castingState);
            chain1.AddSpell(AB12, castingState);
            chain1.AddSpell(AB23, castingState);
            chain1.Calculate(castingState);

            //AM?0-AB33                      0.9
            chain2 = new StaticCycle(2);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB33, castingState);
            chain2.Calculate(castingState);

            Cycle[0] = chain1;
            Cycle[1] = chain2;
            Weight[0] = CC;
            Weight[1] = (1 - CC);
            Calculate();

            commonChain = chain2;
        }
    }

    private StaticCycle commonChain;

    public override string Sequence
    {
        get
        {
            return commonChain.Sequence;
        }
    }
}

class ABAM3CCAM : DynamicCycle
{
    StaticCycle chain1;
    StaticCycle chain2;
    StaticCycle chain3;
    float CC;

    public ABAM3CCAM(CastingState castingState) : base(3)
    {
        Name = "ABAM3CC";

        //AM?0-AB33-AMCC subcycle
        //starts with 3 AB debuffs, alternate AM-AB until AM procs CC, then AM chain and stop

        //AM?0-AB33-AM?0-AB33-...=0.1*0.9*...
        //...
        //AM?0-AB33-AM?0-AB33-AMCC=0.1*0.9*0.9
        //AM?0-AB33-AMCC=0.1*0.9
        //AMCC=0.1

        //V = AMCC + 0.1*0.9*AM?0AB33 + 0.1*0.9*0.9*2*AM?0AB33 + ... + 0.1*0.9^n*n*AM?0AB33 + ...
        //  = AMCC + 0.1*AM?0AB33 * sum_1_inf n*0.9^n
        //  = AMCC + 9*AM?0AB33

        // it is on average equivalent to (AM?0-AB33)x9+AMCC cycle


        //AB00-AM?0-AB11-AM?0-AB22-[(AM?0-AB33)x9+AMCC]       0.9*0.9
        //AB00-AM?0-AB11-AMCC                                 0.9*0.1
        //AB00-AMCC                                           0.1

        Spell AMc0 = castingState.GetSpell(SpellId.ArcaneMissilesNoProc);
        Spell AMCC = castingState.GetCycle(SpellId.ArcaneMissilesCC);
        Spell AB0 = castingState.GetSpell(SpellId.ArcaneBlast00NoCC);
        Spell AB1 = castingState.GetSpell(SpellId.ArcaneBlast11NoCC);
        Spell AB2 = castingState.GetSpell(SpellId.ArcaneBlast22NoCC);
        Spell AB3 = castingState.GetSpell(SpellId.ArcaneBlast33NoCC);

        CC = 0.02f * castingState.MageTalents.ArcaneConcentration;

        if (CC == 0)
        {
            // if we don't have clearcasting then this degenerates to AMc0-AB33
            chain1 = new StaticCycle(2);
            chain1.AddSpell(AMc0, castingState);
            chain1.AddSpell(AB3, castingState);
            chain1.Calculate(castingState);

            Cycle[0] = chain1;
            Weight[0] = 1;

            CastTime = chain1.CastTime;
            CostPerSecond = chain1.CostPerSecond;
            DamagePerSecond = chain1.DamagePerSecond;
            ThreatPerSecond = chain1.ThreatPerSecond;

            commonChain = chain1;
        }
        else
        {
            //AB00-AM?0-AB11-AM?0-AB22-[(AM?0-AB33)x9+AMCC]       0.9*0.9
            chain1 = new StaticCycle(24);
            chain1.AddSpell(AB0, castingState);
            chain1.AddSpell(AMc0, castingState);
            chain1.AddSpell(AB1, castingState);
            chain1.AddSpell(AMc0, castingState);
            chain1.AddSpell(AB2, castingState);
            for (int i = 0; i < (int)((1 - CC) / CC); i++)
            {
                chain1.AddSpell(AMc0, castingState);
                chain1.AddSpell(AB3, castingState);
            }
            chain1.AddSpell(AMCC, castingState);
            chain1.Calculate(castingState);

            //AB00-AM?0-AB11-AMCC                                 0.9*0.1
            chain2 = new StaticCycle(4);
            chain2.AddSpell(AB0, castingState);
            chain2.AddSpell(AMc0, castingState);
            chain2.AddSpell(AB1, castingState);
            chain2.AddSpell(AMCC, castingState);
            chain2.Calculate(castingState);

            //AB00-AMCC                                           0.1
            chain3 = new StaticCycle(2);
            chain3.AddSpell(AB0, castingState);
            chain3.AddSpell(AMCC, castingState);
            chain3.Calculate(castingState);

            Cycle[0] = chain1;
            Cycle[1] = chain2;
            Cycle[2] = chain3;
            Weight[0] = (1 - CC) * (1 - CC);
            Weight[1] = CC * (1 - CC);
            Weight[2] = CC;
            Calculate();

            commonChain = chain3;
        }
    }

    private StaticCycle commonChain;

    public override string Sequence
    {
        get
        {
            return commonChain.Sequence;
        }
    }
}*/
}
