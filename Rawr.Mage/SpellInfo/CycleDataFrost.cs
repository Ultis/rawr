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
            StaticCycle chain2;
            float K;
            Name = "FrBFB";

            FrB = castingState.GetSpell(SpellId.FrostboltFOF);
            Spell FB = castingState.GetSpell(SpellId.FireballBF);
            sequence = "Frostbolt";

            // FrB      1 - brainFreeze
            // FrB-FB   brainFreeze

            chain2 = new StaticCycle(2);
            chain2.AddSpell(FrB, castingState);
            chain2.AddSpell(FB, castingState);
            chain2.Calculate(castingState);

            K = 0.05f * castingState.MageTalents.BrainFreeze;

            AddSpell(needsDisplayCalculations, FrB, 1 - K);
            AddCycle(needsDisplayCalculations, chain2, K);
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

            // S00: FOF0, BF0
            // FrB => S21    fof * bf
            //        S20    fof * (1-bf)
            //        S01    (1-fof) * bf
            //        S00    (1-fof)*(1-bf)

            // S01: FOF0, BF1
            // FrB => S22    fof
            //        S02    (1-fof)

            // S02: FOF0, BF2
            // FB => S00    1

            // S10: FOF1, BF0
            // FrBS-ILS => S12    fof * bf
            //             S10    fof * (1-bf)
            //             S02    (1-fof) * bf
            //             S00    (1-fof)*(1-bf)

            // S11: FOF1, BF1
            // FrBS-FBS => S10    fof
            //             S00    (1-fof)

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

            // S00 = (1-fof)*(1-bf) * S00 + S02 + (1-fof)*(1-bf) * S10 + (1-fof) * S11
            // S01 = (1-fof) * bf * S00
            // S02 = (1-fof) * S01 + (1-fof) * bf * S10
            // S10 = fof * (1-bf) * S10 + fof * S11 + (1-fof)*(1-bf) * S20
            // S11 = fof * bf * S10 + (1-fof) * bf * S20 + (1-fof) * S21
            // S20 = fof * (1-bf) * S00 + fof * (1-bf) * S20
            // S21 = fof * bf * S00 + fof * S01 + fof * bf * S20 + fof * S21
            // S00 + S01 + S02 + S10 + S11 + S20 + S21 = 1

            // solved symbolically

            float bf = 0.05f * castingState.MageTalents.BrainFreeze;
            float fof = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);

            float div = ((bf * bf * bf - bf) * fof * fof * fof * fof + (3 * bf - bf * bf * bf) * fof * fof * fof + (bf * bf * bf - 4 * bf + 1) * fof * fof * fof + (-bf * bf * bf - 2 * bf * bf + 2 * bf) * fof - 2 * bf - 1);
            float S00 = ((bf * bf - bf) * fof * fof * fof + (-bf * bf + 3 * bf - 1) * fof * fof + (2 - 2 * bf) * fof - 1) / div;
            float S01 = -((bf * bf * bf - bf * bf) * fof * fof * fof * fof + (-2 * bf * bf * bf + 4 * bf * bf - bf) * fof * fof * fof + (bf * bf * bf - 5 * bf * bf + 3 * bf) * fof * fof + (2 * bf * bf - 3 * bf) * fof + bf) / div;
            float S02 = ((bf * bf - bf) * fof * fof * fof * fof + (-bf * bf * bf - bf * bf + 3 * bf) * fof * fof * fof + (2 * bf * bf * bf - 4 * bf) * fof * fof + (3 * bf - bf * bf * bf) * fof - bf) / div;
            float S10 = ((bf * bf - bf) * fof * fof * fof * fof + (3 * bf - 2 * bf * bf) * fof * fof * fof + (2 * bf * bf - 5 * bf + 1) * fof * fof + (-bf * bf + 2 * bf - 1) * fof) / div;
            float S11 = ((bf * bf * bf - 2 * bf * bf + bf) * fof * fof * fof * fof + (-bf * bf * bf + 4 * bf * bf - 3 * bf) * fof * fof * fof + (5 * bf - 4 * bf * bf) * fof * fof + (bf * bf - 3 * bf) * fof) / div;
            float S20 = -((bf * bf - bf) * fof * fof * fof + (-bf * bf + 2 * bf - 1) * fof * fof + (1 - bf) * fof) / div;
            float S21 = ((bf * bf * bf - bf * bf) * fof * fof * fof * fof + (-bf * bf * bf + 3 * bf * bf - bf) * fof * fof * fof + (2 * bf - 3 * bf * bf) * fof * fof - 2 * bf * fof) / div;

            KFrB = S00 + S01;
            KFB = S02;
            KFrBS = S10 + S11 + S20 + S21;
            KFBS = S11;
            KILS = S10;

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
