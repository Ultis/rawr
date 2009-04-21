using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    class FrBFB : DynamicCycle
    {
        public FrBFB(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FrB;
            float K;
            Name = "FrBFB";

            FrB = castingState.GetSpell(SpellId.FrostboltFOF);
            Spell FB = castingState.GetSpell(SpellId.FireballBF);
            sequence = "Frostbolt";

            // FrB      1 - brainFreeze
            // FrB-FB   brainFreeze

            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

            K = 0.05f * castingState.MageTalents.BrainFreeze / (1 - T8);

            AddSpell(needsDisplayCalculations, FrB, 1);
            AddSpell(needsDisplayCalculations, FB, K);
            Calculate();
        }
    }

    class FrBFBIL : DynamicCycle
    {
        public FrBFBIL(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            Spell FrB, FrBS, FB, FBS, ILS;
            float KFrB, KFrBS, KFB, KFBS, KILS;
            Name = "FrBFBIL";

            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

            // S00: FOF0, BF0
            // FrB => S21    fof * bf
            //        S20    fof * (1-bf)
            //        S01    (1-fof) * bf
            //        S00    (1-fof)*(1-bf)

            // S01: FOF0, BF1
            // FrB => S22    fof
            //        S02    (1-fof)

            // S02: FOF0, BF2
            // FB => S00    (1-T8)
            //    => S02    T8

            // S10: FOF1, BF0
            // FrBS-ILS => S12    fof * bf
            //             S10    fof * (1-bf)
            //             S02    (1-fof) * bf
            //             S00    (1-fof)*(1-bf)

            // S11: FOF1, BF1
            // FrBS-FBS => S10    fof*(1-T8)
            //             S11    fof*T8
            //             S00    (1-fof)*(1-T8)
            //             S02    (1-fof)*T8

            // S12 = S11

            // S20: FOF0, BF0
            // FrBS => S21    fof * bf
            //         S20    fof * (1-bf)
            //         S11    (1-fof) * bf
            //         S10    (1-fof)*(1-bf)

            // S21: FOF0, BF1
            // FrBS => S22    fof
            //         S12    (1-fof)

            // S22 = S21

            // S00 = (1-fof)*(1-bf) * S00 + S02*(1-T8) + (1-fof)*(1-bf) * S10 + (1-fof)*(1-T8) * S11
            // S01 = (1-fof) * bf * S00
            // S02 = (1-fof) * S01 + T8 * S02 + (1-fof) * bf * S10 + (1-fof)*T8 * S11
            // S10 = fof * (1-bf) * S10 + fof*(1-T8) * S11 + (1-fof)*(1-bf) * S20
            // S11 = fof * bf * S10 + fof*T8 * S11 + (1-fof) * bf * S20 + (1-fof) * S21
            // S20 = fof * (1-bf) * S00 + fof * (1-bf) * S20
            // S21 = fof * bf * S00 + fof * S01 + fof * bf * S20 + fof * S21
            // S00 + S01 + S02 + S10 + S11 + S20 + S21 = 1

            // solved symbolically

            float bf = 0.05f * castingState.MageTalents.BrainFreeze;
            float fof = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);

            //float div = ((bf * bf * bf - bf) * fof * fof * fof * fof + (3 * bf - bf * bf * bf) * fof * fof * fof + (bf * bf * bf - 4 * bf + 1) * fof * fof * fof + (-bf * bf * bf - 2 * bf * bf + 2 * bf) * fof - 2 * bf - 1);
            //float S00 = ((bf * bf - bf) * fof * fof * fof + (-bf * bf + 3 * bf - 1) * fof * fof + (2 - 2 * bf) * fof - 1) / div;
            //float S01 = -((bf * bf * bf - bf * bf) * fof * fof * fof * fof + (-2 * bf * bf * bf + 4 * bf * bf - bf) * fof * fof * fof + (bf * bf * bf - 5 * bf * bf + 3 * bf) * fof * fof + (2 * bf * bf - 3 * bf) * fof + bf) / div;
            //float S02 = ((bf * bf - bf) * fof * fof * fof * fof + (-bf * bf * bf - bf * bf + 3 * bf) * fof * fof * fof + (2 * bf * bf * bf - 4 * bf) * fof * fof + (3 * bf - bf * bf * bf) * fof - bf) / div;
            //float S10 = ((bf * bf - bf) * fof * fof * fof * fof + (3 * bf - 2 * bf * bf) * fof * fof * fof + (2 * bf * bf - 5 * bf + 1) * fof * fof + (-bf * bf + 2 * bf - 1) * fof) / div;
            //float S11 = ((bf * bf * bf - 2 * bf * bf + bf) * fof * fof * fof * fof + (-bf * bf * bf + 4 * bf * bf - 3 * bf) * fof * fof * fof + (5 * bf - 4 * bf * bf) * fof * fof + (bf * bf - 3 * bf) * fof) / div;
            //float S20 = -((bf * bf - bf) * fof * fof * fof + (-bf * bf + 2 * bf - 1) * fof * fof + (1 - bf) * fof) / div;
            //float S21 = ((bf * bf * bf - bf * bf) * fof * fof * fof * fof + (-bf * bf * bf + 3 * bf * bf - bf) * fof * fof * fof + (2 * bf - 3 * bf * bf) * fof * fof - 2 * bf * fof) / div;

            float S00 = (fof - 1) * ((bf - 1) * fof + 1) * (T8 - 1) * (fof * (T8 - bf) - 1);
            float S01 = -bf * (fof - 1) * (fof - 1) * ((bf - 1) * fof + 1) * (T8 - 1) * (fof * (T8 - bf) - 1);
            float S02 = -bf * (fof - 1) * (fof * ((fof - 1) * ((bf - 1) * fof - bf + 2) * T8 - fof * ((bf - 1) * fof - bf * bf + 2) - bf * bf + 2) - 1);
            float S10 = fof * (T8 - 1) * (fof * ((bf * (fof - 1) - 1) * ((bf - 1) * fof + 1) * T8 - bf * (fof * ((bf - 1) * fof - 2 * bf + 3) + 2 * bf - 5) - 1) + (bf - 1) * (bf - 1));
            float S11 = -bf * fof * (fof * ((bf - 1) * fof * ((bf - 1) * fof - bf + 3) - 4 * bf + 5) + bf - 3) * (T8 - 1);
            float S20 = -(bf - 1) * (fof - 1) * fof * (T8 - 1) * (fof * (T8 - bf) - 1);
            float S21 = bf * fof * (fof * ((bf - 1) * fof - bf + 2) - 2) * (T8 - 1) * (fof * (T8 - bf) - 1);

            float div = S00 + S01 + S02 + S10 + S11 + S20 + S21;

            KFrB = (S00 + S01) / div;
            KFB = S02 / div;
            KFrBS = (S10 + S11 + S20 + S21) / div;
            KFBS = S11 / div;
            KILS = S10 / div;

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FrBS = castingState.FrozenState.GetSpell(SpellId.Frostbolt);
            FB = castingState.GetSpell(SpellId.FireballBF);
            FBS = castingState.FrozenState.GetSpell(SpellId.FireballBF);
            ILS = castingState.FrozenState.GetSpell(SpellId.IceLance);
            sequence = "Frostbolt";

            AddSpell(needsDisplayCalculations, FrB, KFrB);
            AddSpell(needsDisplayCalculations, FB, KFB);
            AddSpell(needsDisplayCalculations, FrBS, KFrBS);
            AddSpell(needsDisplayCalculations, FBS, KFBS);
            AddSpell(needsDisplayCalculations, ILS, KILS);
            Calculate();
        }
    }
}
