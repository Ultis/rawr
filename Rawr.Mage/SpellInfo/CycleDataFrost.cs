using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public static class FrBFB
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            Spell FrB;
            float K;
            cycle.Name = "FrBFB";

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            Spell FB = castingState.GetSpell(SpellId.FireballBF);

            // FrB      1 - brainFreeze
            // FrB-FB   brainFreeze

            float T8 = 0;

            K = 0.05f * castingState.MageTalents.BrainFreeze / (1 - T8);

            cycle.AddSpell(needsDisplayCalculations, FrB, 1);
            cycle.AddSpell(needsDisplayCalculations, FB, K);
            cycle.Calculate();
            return cycle;
        }
    }

    public static class FrBFBIL
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            Spell FrB, FrBS, FB, FBS, ILS;
            float KFrB, KFrBS, KFB, KFBS, KILS;
            cycle.Name = "FrBFBIL";

            float T8 = 0;

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

            cycle.AddSpell(needsDisplayCalculations, FrB, KFrB);
            cycle.AddSpell(needsDisplayCalculations, FB, KFB);
            cycle.AddSpell(needsDisplayCalculations, FrBS, KFrBS);
            cycle.AddSpell(needsDisplayCalculations, FBS, KFBS);
            cycle.AddSpell(needsDisplayCalculations, ILS, KILS);
            cycle.Calculate();
            return cycle;
        }
    }

    public static class FrBDFFFBIL
    {
        public static void SolveCycle(CastingState castingState, out float KFrB, out float KFFB, out float KFFBS, out float KILS, out float KDFS)
        {
            Spell FrB, FFB, FFBS, ILS, DFS;
            float RFrB = 0, RFFB = 0, RFFBS = 0, RILS = 0, RDFS = 0;
            int CFrB = 0, CFFB = 0, CFFBS = 0, CILS = 0, CDFS = 0;

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FFB = castingState.GetSpell(SpellId.FrostfireBoltBF);
            FFBS = castingState.FrozenState.GetSpell(SpellId.FrostfireBoltBF);
            ILS = castingState.FrozenState.GetSpell(SpellId.IceLance);
            DFS = castingState.FrozenState.GetSpell(SpellId.DeepFreeze);

            // FFB on FOF only, if Freeze is off cooldown only use it if FOF is off
            // IL on FOF if Freeze is off cooldown, otherwise on FOF2 only

            float bf = 0.05f * castingState.MageTalents.BrainFreeze;
            float fof = (castingState.MageTalents.FingersOfFrost == 3 ? 0.2f : 0.07f * castingState.MageTalents.FingersOfFrost);

            float dfCooldown = 0;
            float freezeCooldown = 0;
            int fofActual = 0;
            int fofRegistered = 0;
            bool bfRegistered = false;
            bool bfActual = false;

            Random rnd = new Random();

            bool stable;
            float errorMargin = 0.000001f;

            do
            {
                for (int c = 0; c < 1000000; c++)
                {
                    if (dfCooldown == 0.0f && (fofRegistered > 0 || freezeCooldown == 0))
                    {
                        CDFS++;
                        if (fofRegistered > 0)
                        {
                            bfRegistered = bfActual;
                            fofActual = Math.Max(0, fofActual - 1);
                            fofRegistered = fofActual;
                            dfCooldown = Math.Max(0, 30f - DFS.CastTime);
                            freezeCooldown = Math.Max(0, freezeCooldown - DFS.CastTime);
                        }
                        else
                        {
                            bfRegistered = bfActual;
                            fofActual = 1;
                            fofRegistered = fofActual;
                            dfCooldown = Math.Max(0, 30f - DFS.CastTime);
                            freezeCooldown = Math.Max(0, 25f - DFS.CastTime);
                        }
                    }
                    else if (bfRegistered && ((fofRegistered > 0 && freezeCooldown > 0) || (fofRegistered == 0 && freezeCooldown == 0)))
                    {
                        CFFBS++;
                        if (fofRegistered > 0)
                        {
                            bool fofProc = rnd.NextDouble() < fof;
                            bfActual = false;
                            bfRegistered = false;
                            fofActual = Math.Max(0, fofActual - 1);
                            dfCooldown = Math.Max(0, dfCooldown - FFBS.CastTime);
                            freezeCooldown = Math.Max(0, freezeCooldown - FFBS.CastTime);
                            if (fofProc)
                            {
                                fofActual++;
                            }
                            fofRegistered = fofActual;
                        }
                        else
                        {
                            bool fofProc = rnd.NextDouble() < fof;
                            bfActual = false;
                            bfRegistered = false;
                            fofActual = Math.Max(0, 2 - 1);
                            dfCooldown = Math.Max(0, dfCooldown - FFBS.CastTime);
                            freezeCooldown = Math.Max(0, 25.0f - FFBS.CastTime);
                            if (fofProc)
                            {
                                fofActual++;
                            }
                            fofRegistered = fofActual;
                        }
                    }
                    else if (fofRegistered == 2 || (fofRegistered == 1 && freezeCooldown < ILS.CastTime))
                    {
                        CILS++;
                        bfRegistered = bfActual;
                        fofActual = Math.Max(0, fofActual - 1);
                        fofRegistered = fofActual;
                        dfCooldown = Math.Max(0, dfCooldown - ILS.CastTime);
                        freezeCooldown = Math.Max(0, freezeCooldown - ILS.CastTime);
                    }
                    else if (fofRegistered == 0 && freezeCooldown == 0)
                    {
                        CILS++;
                        bfRegistered = bfActual;
                        fofActual = Math.Max(0, 2 - 1);
                        fofRegistered = fofActual;
                        dfCooldown = Math.Max(0, dfCooldown - ILS.CastTime);
                        freezeCooldown = Math.Max(0, 25.0f - ILS.CastTime);
                    }
                    else
                    {
                        CFrB++;
                        bool fofProc = rnd.NextDouble() < fof;
                        bool bfProc = rnd.NextDouble() < bf;
                        bfRegistered = bfActual;
                        bfActual = bfProc || bfActual;
                        fofRegistered = fofActual;
                        if (fofProc)
                        {
                            fofActual = Math.Min(2, fofActual + 1);
                        }
                        dfCooldown = Math.Max(0, dfCooldown - FrB.CastTime);
                        freezeCooldown = Math.Max(0, freezeCooldown - FrB.CastTime);
                    }
                }

                float total = CDFS + CFFB + CFFBS + CFrB + CILS;
                KDFS = CDFS / total;
                KFFB = CFFB / total;
                KFFBS = CFFBS / total;
                KFrB = CFrB / total;
                KILS = CILS / total;
                stable = Math.Abs(KDFS - RDFS) < errorMargin &&
                    Math.Abs(KFFB - RFFB) < errorMargin &&
                    Math.Abs(KFFBS - RFFBS) < errorMargin &&
                    Math.Abs(KFrB - RFrB) < errorMargin &&
                    Math.Abs(KILS - RILS) < errorMargin;
                RDFS = KDFS;
                RFFB = KFFB;
                RFFBS = KFFBS;
                RFrB = KFrB;
                RILS = KILS;
            } while (!stable);
        }


        public static Cycle GetSolvedCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            Spell FrB, FFB, FFBS, ILS, DFS;
            //float KFrB, KFFB, KFFBS, KILS, KDFS;
            cycle.Name = "FrBDFFFBIL";

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FFB = castingState.GetSpell(SpellId.FrostfireBoltBF);
            FFBS = castingState.FrozenState.GetSpell(SpellId.FrostfireBoltBF);
            ILS = castingState.FrozenState.GetSpell(SpellId.IceLance);
            DFS = castingState.FrozenState.GetSpell(SpellId.DeepFreeze);

            //KFrB = castingState.Solver.FrBDFFFBIL_KFrB;
            //KFFB = castingState.Solver.FrBDFFFBIL_KFFB;
            //KFFBS = castingState.Solver.FrBDFFFBIL_KFFBS;
            //KILS = castingState.Solver.FrBDFFFBIL_KILS;
            //KDFS = castingState.Solver.FrBDFFFBIL_KDFS;

            //cycle.AddSpell(needsDisplayCalculations, FrB, KFrB);
            //cycle.AddSpell(needsDisplayCalculations, FFB, KFFB);
            //cycle.AddSpell(needsDisplayCalculations, FFBS, KFFBS);
            //cycle.AddSpell(needsDisplayCalculations, ILS, KILS);
            //cycle.AddSpell(needsDisplayCalculations, DFS, KDFS);
            cycle.Calculate();
            return cycle;
        }

        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            Spell FrB, FFB, FFBS, ILS, DFS;
            float KFrB, KFrB2, KFFB, KFFBS, KILS, KDFS;
            cycle.Name = "FrBDFFFBIL";

            // S00: FOF00, BF00
            // FrB => S11    fof * bf * (1-Y)
            //        S10    fof * (1-bf) * (1-Y)
            //        S01    (1-fof) * bf * (1-Y)
            //        S00    (1-fof)*(1-bf) * (1-Y)
            // -   => S40    Y

            // S01: FOF00, BF10
            // FrB => S12    fof * (1-Y)
            //        S02    (1-fof) * (1-Y)
            // -   => S41    Y

            // S02: FOF00, BF11
            // FrB => S12    fof * (1-Y)
            //        S02    (1-fof) * (1-Y)
            // -   => S42    Y

            // S10: FOF10, BF00
            // FrB => S31    fof * bf * (1-Y)
            //        S30    fof * (1-bf) * (1-Y)
            //        S21    (1-fof) * bf * (1-Y)
            //        S20    (1-fof)*(1-bf) * (1-Y)
            // -   => S40    Y

            // S11: FOF10, BF10
            // FrB => S32    fof * (1-Y)
            //        S22    (1-fof) * (1-Y)
            // -   => S41    Y

            // S12: FOF10, BF11
            // FrB => S32    (1-fof) * (1-Y)
            //        S22    fof * (1-Y)
            // -   => S42    Y

            // S20: FOF11, BF00
            // FrB => S31    fof * bf * (1-X)
            //        S30    fof * (1-bf) * (1-X)
            //        S21    (1-fof) * bf * (1-X)
            //        S20    (1-fof)*(1-bf) * (1-X)
            // DF  => S00    X

            // S21: FOF11, BF10
            // FrB => S32    fof * (1-X)
            //        S22    (1-fof) * (1-X)
            // DF  => S02    X

            // S22: FOF11, BF11
            // FFBS => S00   (1-fof)*(1-X)
            //         S20   fof*(1-X)
            // DF   => S02   X

            // S30: FOF21, B00
            // FrB => S41    bf * (1-X)
            //        S40    (1-bf) * (1-X)
            // DF  => S20    X

            // S31: FOF21, BF10
            // FrB => S42    (1-X)
            // DF  => S22    X

            // S32: FOF21, BF11
            // FFBS => S20   (1-fof)*(1-X)
            //         S40   fof*(1-X)
            // DF   => S22   X

            // S40: FOF22, B00
            // IL  => S20    1-X
            // DF  => S20    X

            // S41: FOF22, B10
            // IL  => S22    1-X
            // DF  => S22    X

            // S42: FOF22, BF11
            // FFBS => S20   (1-fof)*(1-X)
            //         S40   fof*(1-X)
            // DF   => S22   X

            // S00 = S00 * (1-fof)*(1-bf) * (1-Y) + S20 * X + S22 * (1-fof)*(1-X)
            // S01 = S00 * (1-fof) * bf * (1-Y)
            // S02 = S01 * (1-fof) * (1-Y) + S02 * (1-fof) * (1-Y) + S21 * X + S22 * X
            // S10 = S00 * fof * (1-bf) * (1-Y)
            // S11 = S00 * fof * bf * (1-Y)
            // S12 = S01 * fof * (1-Y) + S02 * fof * (1-Y)
            // S20 = S10 * (1-fof)*(1-bf) * (1-Y) + S20 * (1-fof)*(1-bf) * (1-X) + S22 * fof*(1-X) + S30 * X + S32 * (1-fof)*(1-X) + S40 + S42 * (1-fof)*(1-X)
            // S21 = S10 * (1-fof) * bf * (1-Y) + S20 * (1-fof) * bf * (1-X)
            // S22 = S11 * (1-fof) * (1-Y) + S12 * fof * (1-Y) + S21 * (1-fof) * (1-X) + S31 * X + S32 * X + S41 + S42 * X
            // S30 = S10 * fof * (1-bf) * (1-Y) + S20 * fof * (1-bf) * (1-X)
            // S31 = S10 * fof * bf * (1-Y) + S20 * fof * bf * (1-X)
            // S32 = S11 * fof * (1-Y) + S12 * (1-fof) * (1-Y) + S21 * fof * (1-X)
            // S40 = S00 * Y + S10 * Y + S30 * (1-bf) * (1-X) + S32 * fof*(1-X) + S42 * fof*(1-X)
            // S41 = S01 * Y + S11 * Y + S30 * bf * (1-X)
            // S42 = S02 * Y + S12 * Y + S31 * (1-X)
            // S00 + S01 + S02 + S10 + S11 + S12 + S20 + S21 + S22 + S30 + S31 + S32 + S40 + S41 + S42 = 1

            // solved symbolically
            // solve([S00 = S00 * (1-fof)*(1-bf) * (1-Y) + S20 * X + S22 * (1-fof)*(1-X),S01 = S00 * (1-fof) * bf * (1-Y),S02 = S01 * (1-fof) * (1-Y) + S02 * (1-fof) * (1-Y) + S21 * X + S22 * X,S10 = S00 * fof * (1-bf) * (1-Y),S11 = S00 * fof * bf * (1-Y),S12 = S01 * fof * (1-Y) + S02 * fof * (1-Y),S20 = S10 * (1-fof)*(1-bf) * (1-Y) + S20 * (1-fof)*(1-bf) * (1-X) + S22 * fof*(1-X) + S30 * X + S32 * (1-fof)*(1-X) + S40 + S42 * (1-fof)*(1-X),S21 = S10 * (1-fof) * bf * (1-Y) + S20 * (1-fof) * bf * (1-X),S22 = S11 * (1-fof) * (1-Y) + S12 * fof * (1-Y) + S21 * (1-fof) * (1-X) + S31 * X + S32 * X + S41 + S42 * X,S30 = S10 * fof * (1-bf) * (1-Y) + S20 * fof * (1-bf) * (1-X),S31 = S10 * fof * bf * (1-Y) + S20 * fof * bf * (1-X),S32 = S11 * fof * (1-Y) + S12 * (1-fof) * (1-Y) + S21 * fof * (1-X),S40 = S00 * Y + S10 * Y + S30 * (1-bf) * (1-X) + S32 * fof*(1-X) + S42 * fof*(1-X),S41 = S01 * Y + S11 * Y + S30 * bf * (1-X),S42 = S02 * Y + S12 * Y + S31 * (1-X),S00 + S01 + S02 + S10 + S11 + S12 + S20 + S21 + S22 + S30 + S31 + S32 + S40 + S41 + S42 = 1,fof=0.2,bf=0.15], [S00,S01,S02,S10,S11,S12,S20,S21,S22,S30,S31,S32,S40,S41,S42,fof,bf]);

            // S00=((480000*X^4-6440000*X^3+6440000*X^2-480000*X)*Y^2+(-6240000*X^4+126440000*X^3-33880000*X^2-76600000*X-9720000)*Y-840000*X^4+21950000*X^3+1190000*X^2-19870000*X-2430000)
            // S02=((57600*X^4-772800*X^3+772800*X^2-57600*X)*Y^3+(1853100*X^4-3543525*X^3+10315725*X^2-10646400*X-1166400)*Y^2+(-8469000*X^4+9007950*X^3+23115150*X^2-861900*X+2332800)*Y-191700*X^4-2629125*X^3+671325*X^2-18621600*X-1166400)
            // S20=((81600*X^3-163200*X^2+81600*X)*Y^4+(-1224000*X^3-11899600*X^2+22031200*X-8907600)*Y^3+(2060400*X^3+88837500*X^2-27086200*X-63811700)*Y^2+(-775200*X^3+43676600*X^2+2752400*X-45653800)*Y-142800*X^3+4548700*X^2+2221000*X-6626900)
            // S01 = S00 * (1-fof) * bf * (1-Y)
            // S10 = S00 * fof * (1-bf) * (1-Y)
            // S11 = S00 * fof * bf * (1-Y)
            // S21 = S10 * (1-fof) * bf * (1-Y) + S20 * (1-fof) * bf * (1-X)
            // S30 = S10 * fof * (1-bf) * (1-Y) + S20 * fof * (1-bf) * (1-X)
            // S31 = S10 * fof * bf * (1-Y) + S20 * fof * bf * (1-X)
            // S12 = S01 * fof * (1-Y) + S02 * fof * (1-Y)
            // S32 = S11 * fof * (1-Y) + S12 * (1-fof) * (1-Y) + S21 * fof * (1-X)
            // S41 = S01 * Y + S11 * Y + S30 * bf * (1-X)
            // S42 = S02 * Y + S12 * Y + S31 * (1-X)
            // S22 = S11 * (1-fof) * (1-Y) + S12 * fof * (1-Y) + S21 * (1-fof) * (1-X) + S31 * X + S32 * X + S41 + S42 * X
            // S40 = S00 * Y + S10 * Y + S30 * (1-bf) * (1-X) + S32 * fof*(1-X) + S42 * fof*(1-X)

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FFB = castingState.GetSpell(SpellId.FrostfireBoltBF);
            FFBS = castingState.FrozenState.GetSpell(SpellId.FrostfireBoltBF);
            ILS = castingState.FrozenState.GetSpell(SpellId.IceLance);
            DFS = castingState.FrozenState.GetSpell(SpellId.DeepFreeze);

            float bf = 0.05f * castingState.MageTalents.BrainFreeze;
            float fof = (castingState.MageTalents.FingersOfFrost == 3 ? 0.2f : 0.07f * castingState.MageTalents.FingersOfFrost);
            float fof2 = fof * fof;
            float fof3 = fof2 * fof;
            float fof4 = fof3 * fof;
            float bf2 = bf * bf;
            float bf3 = bf2 * bf;

            // states are split into S0,S1 (fof not registered) vs S2,S3,S4 (fof registered)
            // ABBBaaBBBaaBBBaABBBaaBBBaaBBB
            // A*Y freezes in B*time(B) + A*(1-Y)*time(a)
            // A*Y <= R / 25 * (B*time(B) + A*(1-Y)*time(a))
            // A*Y <= R / 25 * B*time(B) + R / 25 * A*time(a) - R / 25 * A*time(a) * Y
            // A*Y*(1 + R / 25 * time(a)) <= R / 25 * B*time(B) + R / 25 * A*time(a)

            // Y = R/25 * (B*time(B) + A*time(a)) / (A * (1 + R / 25 * time(a)))

            // B*X*time(DF)/(B*time(B) + A*(1-Y)*time(a))=K*time(DF)/cool(DF)

            // heruistic tuning parameters
            float R = 1.05f; // overestimate because we don't have IL to eat down FOF when freeze is coming off cooldown in the model
            if (castingState.CalculationOptions.FlameOrb == 1)
            {
                fof *= 1.8f;
            }
            else if (castingState.CalculationOptions.FlameOrb == 2 && castingState.FlameOrb)
            {
                fof *= 5;
            }
            float K = 0.95f;

            // crude initial guess (fof=0.9,nonfof=0.1)
            float Y = R / 25f * (0.9f * (0.5f * DFS.CastTime + 0.5f * FrB.CastTime) + 0.1f * FrB.CastTime) / (0.1f * (1 + R / 25f * FrB.CastTime));
            float X = K * DFS.CastTime / DFS.Cooldown * (0.9f * (0.5f * DFS.CastTime + 0.5f * FrB.CastTime) + 0.1f * (1 - Y) * FrB.CastTime) / (0.9f * DFS.CastTime);

            float S00 = -((480000 * X * X * X * X - 6440000 * X * X * X + 6440000 * X * X - 480000 * X) * Y * Y + (-6240000 * X * X * X * X + 126440000 * X * X * X - 33880000 * X * X - 76600000 * X - 9720000) * Y - 840000 * X * X * X * X + 21950000 * X * X * X + 1190000 * X * X - 19870000 * X - 2430000);
            float S02 = -((57600 * X * X * X * X - 772800 * X * X * X + 772800 * X * X - 57600 * X) * Y * Y * Y + (1853100 * X * X * X * X - 3543525 * X * X * X + 10315725 * X * X - 10646400 * X - 1166400) * Y * Y + (-8469000 * X * X * X * X + 9007950 * X * X * X + 23115150 * X * X - 861900 * X + 2332800) * Y - 191700 * X * X * X * X - 2629125 * X * X * X + 671325 * X * X - 18621600 * X - 1166400);
            float S20 = -((81600 * X * X * X - 163200 * X * X + 81600 * X) * Y * Y * Y * Y + (-1224000 * X * X * X - 11899600 * X * X + 22031200 * X - 8907600) * Y * Y * Y + (2060400 * X * X * X + 88837500 * X * X - 27086200 * X - 63811700) * Y * Y + (-775200 * X * X * X + 43676600 * X * X + 2752400 * X - 45653800) * Y - 142800 * X * X * X + 4548700 * X * X + 2221000 * X - 6626900);
            float S01 = S00 * (1-fof) * bf * (1-Y);
            float S10 = S00 * fof * (1-bf) * (1-Y);
            float S11 = S00 * fof * bf * (1-Y);
            float S21 = S10 * (1-fof) * bf * (1-Y) + S20 * (1-fof) * bf * (1-X);
            float S30 = S10 * fof * (1-bf) * (1-Y) + S20 * fof * (1-bf) * (1-X);
            float S31 = S10 * fof * bf * (1-Y) + S20 * fof * bf * (1-X);
            float S12 = S01 * fof * (1-Y) + S02 * fof * (1-Y);
            float S32 = S11 * fof * (1-Y) + S12 * (1-fof) * (1-Y) + S21 * fof * (1-X);
            float S41 = S01 * Y + S11 * Y + S30 * bf * (1-X);
            float S42 = S02 * Y + S12 * Y + S31 * (1-X);
            float S22 = S11 * (1-fof) * (1-Y) + S12 * fof * (1-Y) + S21 * (1-fof) * (1-X) + S31 * X + S32 * X + S41 + S42 * X;
            float S40 = S00 * Y + S10 * Y + S30 * (1-bf) * (1-X) + S32 * fof*(1-X) + S42 * fof*(1-X);

            float div = S00 + S01 + S02 + S10 + S11 + S12 + S20 + S21 + S22 + S30 + S31 + S32 + S40 + S41 + S42;

            KFrB = ((S00 + S01 + S02 + S10 + S11 + S12) * (1 - Y) + (S20 + S21 + S30 + S31) * (1 - X)) / div;
            KFrB2 = ((S00 + S01 + S02 + S10 + S11 + S12) + (S20 + S21 + S30 + S31) * (1 - X)) / div;
            KFFB = 0 / div;
            KFFBS = ((S22 + S32 + S42) * (1 - X)) / div;
            KILS = (S40 + S41) * (1 - X) / div;
            KDFS = (S20 + S21 + S22 + S30 + S31 + S32 + S40 + S41 + S42) * X / div;

            for (int i = 0; i < 5; i++)
            {
                float T = KFrB * FrB.CastTime + KFFB * FFB.CastTime + KFFBS * FFBS.CastTime + KILS * ILS.CastTime + KDFS * DFS.CastTime;
                float T2 = KFrB2 * FrB.CastTime + KFFB * FFB.CastTime + KFFBS * FFBS.CastTime + KILS * ILS.CastTime + KDFS * DFS.CastTime;

                // better estimate
                // TODO better probabilistic model
                Y = R / 25 * T2 / ((S00 + S01 + S02 + S10 + S11 + S12) / div * (1 + R / 25 * FrB.CastTime));
                X = K / DFS.Cooldown * T / ((S20 + S21 + S22 + S30 + S31 + S32 + S40 + S41 + S42) / div);

                // recalculate shares based on revised estimate
                S00 = -((480000 * X * X * X * X - 6440000 * X * X * X + 6440000 * X * X - 480000 * X) * Y * Y + (-6240000 * X * X * X * X + 126440000 * X * X * X - 33880000 * X * X - 76600000 * X - 9720000) * Y - 840000 * X * X * X * X + 21950000 * X * X * X + 1190000 * X * X - 19870000 * X - 2430000);
                S02 = -((57600 * X * X * X * X - 772800 * X * X * X + 772800 * X * X - 57600 * X) * Y * Y * Y + (1853100 * X * X * X * X - 3543525 * X * X * X + 10315725 * X * X - 10646400 * X - 1166400) * Y * Y + (-8469000 * X * X * X * X + 9007950 * X * X * X + 23115150 * X * X - 861900 * X + 2332800) * Y - 191700 * X * X * X * X - 2629125 * X * X * X + 671325 * X * X - 18621600 * X - 1166400);
                S20 = -((81600 * X * X * X - 163200 * X * X + 81600 * X) * Y * Y * Y * Y + (-1224000 * X * X * X - 11899600 * X * X + 22031200 * X - 8907600) * Y * Y * Y + (2060400 * X * X * X + 88837500 * X * X - 27086200 * X - 63811700) * Y * Y + (-775200 * X * X * X + 43676600 * X * X + 2752400 * X - 45653800) * Y - 142800 * X * X * X + 4548700 * X * X + 2221000 * X - 6626900);
                S01 = S00 * (1 - fof) * bf * (1 - Y);
                S10 = S00 * fof * (1 - bf) * (1 - Y);
                S11 = S00 * fof * bf * (1 - Y);
                S21 = S10 * (1 - fof) * bf * (1 - Y) + S20 * (1 - fof) * bf * (1 - X);
                S30 = S10 * fof * (1 - bf) * (1 - Y) + S20 * fof * (1 - bf) * (1 - X);
                S31 = S10 * fof * bf * (1 - Y) + S20 * fof * bf * (1 - X);
                S12 = S01 * fof * (1 - Y) + S02 * fof * (1 - Y);
                S32 = S11 * fof * (1 - Y) + S12 * (1 - fof) * (1 - Y) + S21 * fof * (1 - X);
                S41 = S01 * Y + S11 * Y + S30 * bf * (1 - X);
                S42 = S02 * Y + S12 * Y + S31 * (1 - X);
                S22 = S11 * (1 - fof) * (1 - Y) + S12 * fof * (1 - Y) + S21 * (1 - fof) * (1 - X) + S31 * X + S32 * X + S41 + S42 * X;
                S40 = S00 * Y + S10 * Y + S30 * (1 - bf) * (1 - X) + S32 * fof * (1 - X) + S42 * fof * (1 - X);

                div = S00 + S01 + S02 + S10 + S11 + S12 + S20 + S21 + S22 + S30 + S31 + S32 + S40 + S41 + S42;

                KFrB = ((S00 + S01 + S02 + S10 + S11 + S12) * (1 - Y) + (S20 + S21 + S30 + S31) * (1 - X)) / div;
                KFrB2 = ((S00 + S01 + S02 + S10 + S11 + S12) + (S20 + S21 + S30 + S31) * (1 - X)) / div;
                KFFB = 0 / div;
                KFFBS = ((S22 + S32 + S42) * (1 - X)) / div;
                KILS = (S40 + S41) * (1 - X) / div;
                KDFS = (S20 + S21 + S22 + S30 + S31 + S32 + S40 + S41 + S42) * X / div;
            }

            //div = KFrB + KFFBS + KILS + KDFS;
            //KFrB /= div;
            //KFFBS /= div;
            //KILS /= div;
            //KDFS /= div;

            cycle.AddSpell(needsDisplayCalculations, FrB, KFrB);
            cycle.AddSpell(needsDisplayCalculations, FFB, KFFB);
            cycle.AddSpell(needsDisplayCalculations, FFBS, KFFBS);
            cycle.AddSpell(needsDisplayCalculations, ILS, KILS);
            cycle.AddSpell(needsDisplayCalculations, DFS, KDFS);
            cycle.Calculate();
            return cycle;
        }
    }

    public static class FrBDFFBIL
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            Spell FrB, FrBS, FB, FBS, ILS, DFS;
            float KFrB, KFrBS, KFB, KFBS, KILS, KDFS;
            cycle.Name = "FrBDFFBIL";

            //float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

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
            // FrBS-ILS => S12    X*fof * bf
            //             S10    X*fof * (1-bf)
            //             S02    X*(1-fof) * bf
            //             S00    X*(1-fof)*(1-bf)
            // FrBS-DFS => S12    (1-X)*fof*bf
            //             S10    (1-X)*fof * (1-bf)
            //             S02    (1-X)*(1-fof) * bf
            //             S00    (1-X)*(1-fof)*(1-bf)

            // S11: FOF1, BF1
            // FrBS-FBS => S10    X*fof
            //             S00    X*(1-fof)
            // FrBS-DFS => S12    (1-X)*fof
            //             S02    (1-X)*(1-fof)

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

            // S00 = (1-fof)*(1-bf) * S00 + S02 + (1-fof)*(1-bf) * S10 + X*(1-fof) * S11
            // S01 = (1-fof) * bf * S00
            // S02 = (1-fof) * S01 + (1-fof) * bf * S10 + (1-X)*(1-fof) * S11
            // S10 = fof * (1-bf) * S10 + X*fof * S11 + (1-fof)*(1-bf) * S20
            // S11 = fof * bf * S10 + (1-X)*fof * S11 + (1-fof) * bf * S20 + (1-fof) * S21
            // S20 = fof * (1-bf) * S00 + fof * (1-bf) * S20
            // S21 = fof * bf * S00 + fof * S01 + fof * bf * S20 + fof * S21
            // S00 + S01 + S02 + S10 + S11 + S20 + S21 = 1

            // solved symbolically

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FrBS = castingState.FrozenState.GetSpell(SpellId.Frostbolt);
            FB = castingState.GetSpell(SpellId.FireballBF);
            FBS = castingState.FrozenState.GetSpell(SpellId.FireballBF);
            ILS = castingState.FrozenState.GetSpell(SpellId.IceLance);
            DFS = castingState.FrozenState.GetSpell(SpellId.DeepFreeze);

            float bf = 0.05f * castingState.MageTalents.BrainFreeze;
            float fof = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);
            float fof2 = fof * fof;
            float fof3 = fof2 * fof;
            float fof4 = fof3 * fof;
            float bf2 = bf * bf;
            float bf3 = bf2 * bf;

            // shatters until deep freeze ~ Poisson
            // share of shatters that are deep freeze = sum_i=0..inf Pi / sum_i=0..inf (i+1)*Pi = 1 / (1 + mean)

            // crude initial guess
            float X = 1.0f - 1.0f / (1.0f + (DFS.Cooldown - DFS.CastTime) / (FrB.CastTime * (1 / fof + 1) + ILS.CastTime));

            float S00 = (((bf - 1) * fof3 + (2 - bf) * fof2 - fof) * X + (bf2 - 2 * bf + 1) * fof3 + (-bf2 + 4 * bf - 3) * fof2 + (3 - 2 * bf) * fof - 1);
            float S01 = -(((bf2 - bf) * fof4 + (3 * bf - 2 * bf2) * fof3 + (bf2 - 3 * bf) * fof2 + bf * fof) * X + (bf3 - 2 * bf2 + bf) * fof4 + (-2 * bf3 + 6 * bf2 - 4 * bf) * fof3 + (bf3 - 6 * bf2 + 6 * bf) * fof2 + (2 * bf2 - 4 * bf) * fof + bf);
            float S02 = (((bf2 - bf) * fof4 + (4 * bf - 3 * bf2) * fof3 + (3 * bf2 - 5 * bf) * fof2 + (2 * bf - bf2) * fof) * X + (-bf3 + 2 * bf2 - bf) * fof3 + (2 * bf3 - 3 * bf2 + bf) * fof2 + (-bf3 + bf2 + bf) * fof - bf);
            float S10 = (((bf2 - bf) * fof4 + (-bf2 + bf + 1) * fof3 + (-bf - 1) * fof2) * X + (-bf2 + 2 * bf - 1) * fof3 + (2 * bf2 - 4 * bf + 2) * fof2 + (-bf2 + 2 * bf - 1) * fof);
            float S11 = ((bf3 - 2 * bf2 + bf) * fof4 + (-bf3 + 4 * bf2 - 3 * bf) * fof3 + (5 * bf - 4 * bf2) * fof2 + (bf2 - 3 * bf) * fof);
            float S20 = -(((bf - 1) * fof3 + (1 - bf) * fof2) * X + (bf2 - 2 * bf + 1) * fof3 + (-bf2 + 3 * bf - 2) * fof2 + (1 - bf) * fof);
            float S21 = (((bf2 - bf) * fof4 + (2 * bf - bf2) * fof3 - 2 * bf * fof2) * X + (bf3 - 2 * bf2 + bf) * fof4 + (-bf3 + 4 * bf2 - 3 * bf) * fof3 + (4 * bf - 3 * bf2) * fof2 - 2 * bf * fof);

            float div = S00 + S01 + S02 + S10 + S11 + S20 + S21;            

            KFrB = (S00 + S01) / div;
            KFB = S02 / div;
            KFrBS = (S10 + S11 + S20 + S21) / div;
            KFBS = X * S11 / div;
            KILS = X * S10 / div;
            KDFS = (1 - X) * (S10 + S11) / div;

            float hasteFactor = 1.0f;

            float T = KFrB * FrB.CastTime + KFB * FB.CastTime + KFrBS * FrBS.CastTime + KFBS * FBS.CastTime + KILS * ILS.CastTime + KDFS * DFS.CastTime;
            float T0 = KFBS * FBS.CastTime + KILS * ILS.CastTime + KDFS * DFS.CastTime;
            float T1 = KFBS * FBS.CastTime + KFB * FB.CastTime;

            if (castingState.Solver.Mage2T10)
            {
                // we'll make a lot of assumptions here and just assume that 2T10 haste is uniformly distributed over all
                // spells and doesn't have an impact on state space
                // also ignore the possible refresh of 2T10
                // each proc gives 12% haste for 5 sec
                // we have on average one proc every T/T1 * FB.CastTime
                // we have some feedback loop here, speeding up the cycle increases the rate of procs
                // hastedShare = (5-FB.CastTimeAverage) / (T/T1 * FB.CastTimeAverage)
                // hastedCastShare = (5-FB.CastTimeAverage) / (T/T1 * FB.CastTime) * 1.12
                // average haste = 1 / (1-hastedCastShare*0.12/1.12)
                // TODO this is all a bunch of voodoo, redo the math when you're thinking straight
                hasteFactor = 1.0f / (1.0f - (5 - FB.CastTime) / (T / T1 * FB.CastTime) * 0.12f);

                // alternative model based on reduction to single state space and expanding for haste
                // probability of being hasted = 1 - (1-p)^(N-1)
                // where
                // K := (KFrB + KFB + KFrBS + KFBS + KILS + KDFS)
                // p = probability of haste generating spell = (KFBS + KFB) / K
                // N = average number of spells affected by haste = (haste duration - average cast time of haste generating spell) / (average cast time of hasted spells)
                //   = (5 - T1 / 1.12 / K) / (T / 1.12 / K)
                //   = (5 * 1.12 * K - T1) / T
                // hasteFactor = 1 / (((1-p)^(N-1)) * 1 + (1 - (1-p)^(N-1)) * 1/1.12)
                //             = 1.12 / (((1-p)^(N-1)) * 0.12 + 1)
                //             = 1.12 / (1 + 0.12 * (1 - (KFBS + KFB) / K)^((5 * 1.12 * K - T1) / T - 1))
                //float K = KFrB + KFB + KFrBS + KFBS + KILS + KDFS;
                //hasteFactor = 1.12f / (1.0f + 0.12f * (float)Math.Pow(1.0f - ((KFBS + KFB) / K), ((5.0f * 1.12f * K - T1) / T - 1.0f)));
            }

            if (fof > 0) // nothing new here if we don't have fof
            {
                // better estimate for percentage of shatter combos that are deep freeze
                // TODO better probabilistic model for DF percentage
                X = 1.0f - 1.0f / (1.0f + (DFS.Cooldown - DFS.CastTime / hasteFactor) / (DFS.CastTime / hasteFactor * T / T0));

                // recalculate shares based on revised estimate
                S00 = (((bf - 1) * fof3 + (2 - bf) * fof2 - fof) * X + (bf2 - 2 * bf + 1) * fof3 + (-bf2 + 4 * bf - 3) * fof2 + (3 - 2 * bf) * fof - 1);
                S01 = -(((bf2 - bf) * fof4 + (3 * bf - 2 * bf2) * fof3 + (bf2 - 3 * bf) * fof2 + bf * fof) * X + (bf3 - 2 * bf2 + bf) * fof4 + (-2 * bf3 + 6 * bf2 - 4 * bf) * fof3 + (bf3 - 6 * bf2 + 6 * bf) * fof2 + (2 * bf2 - 4 * bf) * fof + bf);
                S02 = (((bf2 - bf) * fof4 + (4 * bf - 3 * bf2) * fof3 + (3 * bf2 - 5 * bf) * fof2 + (2 * bf - bf2) * fof) * X + (-bf3 + 2 * bf2 - bf) * fof3 + (2 * bf3 - 3 * bf2 + bf) * fof2 + (-bf3 + bf2 + bf) * fof - bf);
                S10 = (((bf2 - bf) * fof4 + (-bf2 + bf + 1) * fof3 + (-bf - 1) * fof2) * X + (-bf2 + 2 * bf - 1) * fof3 + (2 * bf2 - 4 * bf + 2) * fof2 + (-bf2 + 2 * bf - 1) * fof);
                S11 = ((bf3 - 2 * bf2 + bf) * fof4 + (-bf3 + 4 * bf2 - 3 * bf) * fof3 + (5 * bf - 4 * bf2) * fof2 + (bf2 - 3 * bf) * fof);
                S20 = -(((bf - 1) * fof3 + (1 - bf) * fof2) * X + (bf2 - 2 * bf + 1) * fof3 + (-bf2 + 3 * bf - 2) * fof2 + (1 - bf) * fof);
                S21 = (((bf2 - bf) * fof4 + (2 * bf - bf2) * fof3 - 2 * bf * fof2) * X + (bf3 - 2 * bf2 + bf) * fof4 + (-bf3 + 4 * bf2 - 3 * bf) * fof3 + (4 * bf - 3 * bf2) * fof2 - 2 * bf * fof);

                div = S00 + S01 + S02 + S10 + S11 + S20 + S21;

                KFrB = (S00 + S01) / div;
                KFB = S02 / div;
                KFrBS = (S10 + S11 + S20 + S21) / div;
                KFBS = X * S11 / div;
                KILS = X * S10 / div;
                KDFS = (1 - X) * (S10 + S11) / div;
            }

            cycle.AddSpell(needsDisplayCalculations, FrB, KFrB);
            cycle.AddSpell(needsDisplayCalculations, FB, KFB);
            cycle.AddSpell(needsDisplayCalculations, FrBS, KFrBS);
            cycle.AddSpell(needsDisplayCalculations, FBS, KFBS);
            cycle.AddSpell(needsDisplayCalculations, ILS, KILS);
            cycle.AddSpell(needsDisplayCalculations, DFS, KDFS);
            cycle.CastTime /= hasteFactor; // ignores latency effects, but it'll have to do for now
            cycle.Calculate();
            return cycle;
        }
    }

    public static class FrBDFFFB
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            Spell FrB, FrBS, FFB, FFBS, DFS;
            float KFrB, KFrBS, KFFB, KFFBS, KDFS;
            cycle.Name = "FrBDFFFB";

            //float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

            // S00: FOF0, BF0
            // FrB => S21    fof * bf
            //        S20    fof * (1-bf)
            //        S01    (1-fof) * bf
            //        S00    (1-fof)*(1-bf)

            // S01: FOF0, BF1
            // FrB => S22    fof
            //        S02    (1-fof)

            // S02: FOF0, BF2
            // FFB => S20    fof
            //        S00    (1-fof)

            // S10: FOF1, BF0
            // FrBS => S21    X*fof * bf
            //         S20    X*fof * (1-bf)
            //         S01    X*(1-fof) * bf
            //         S00    X*(1-fof)*(1-bf)
            // FrBS-DFS => S12    (1-X)*fof*bf
            //             S10    (1-X)*fof * (1-bf)
            //             S02    (1-X)*(1-fof) * bf
            //             S00    (1-X)*(1-fof)*(1-bf)

            // S11: FOF1, BF1
            // FrBS-FFBS => S10    X*fof*(1-fof)
            //              S00    X*(1-fof)*(1-fof)
            //              S20    X*fof
            // FrBS-DFS => S12    (1-X)*fof
            //             S02    (1-X)*(1-fof)

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

            // S00 = (1-fof)*(1-bf) * S00 + (1-fof) * S02 + (1-fof)*(1-bf) * S10 + X*(1-fof)*(1-fof) * S11
            // S01 = (1-fof) * bf * S00 + X*(1-fof) * bf * S10
            // S02 = (1-fof) * S01 + (1-X)*(1-fof) * bf * S10 + (1-X)*(1-fof) * S11
            // S10 = (1-X)*fof * (1-bf) * S10 + X*fof*(1-fof) * S11 + (1-fof)*(1-bf) * S20
            // S11 = (1-X)*fof*bf * S10 + (1-X)*fof * S11 + (1-fof) * bf * S20 + (1-fof) * S21
            // S20 = fof * (1-bf) * S00 + fof * S02 + X*fof * (1-bf) * S10 + X*fof * S11 + fof * (1-bf) * S20
            // S21 = fof * bf * S00 + fof * S01 + X*fof * bf * S10 + fof * bf * S20 + fof * S21
            // S00 + S01 + S02 + S10 + S11 + S20 + S21 = 1

            // solved symbolically

            // S20:((bf^2*fof^4+(bf-3*bf^2)*fof^3+(2*bf^2-1)*fof^2)*X^2+(-bf^2*fof^4+(3*bf^2-bf)*fof^3+(-2*bf^2-bf+2)*fof^2+(bf-2)*fof)*X+(bf-1)*fof^2+(2-bf)*fof-1)
            // S11: ((bf^3-bf)*fof^3+(-3*bf^3+bf^2+2*bf)*fof^2+(2*bf^3-2*bf)*fof)*X+(bf-bf^3)*fof^3+(3*bf^3-bf^2-3*bf)*fof^2+(5*bf-2*bf^3)*fof-3*bf
            // S10: ((fof^2-fof)*S11*X+((1-bf)*fof+bf-1)*S20)/((bf-1)*fof*X+(1-bf)*fof-1)
            // S00: (-(bf*fof^3-2*bf*fof^2+bf*fof)*S10*X-(-fof^2+2*fof-1)*S11-(-bf*fof^2+(bf+1)*fof-1)*S10)/(bf*fof^3-3*bf*fof^2+(2*bf+1)*fof)
            // S21: ((bf*fof^2-2*bf*fof)*S10*X-bf*fof*S20+(bf*fof^2-2*bf*fof)*S00)/(fof-1)
            // S02: (1-fof)*(bf*(1-fof)*S10*X+bf*(1-fof)*S00)+(1-fof)*S11*(1-X)+bf*(1-fof)*S10*(1-X)
            // S01: (1-fof) * bf * S00 + X*(1-fof) * bf * S10

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FrBS = castingState.FrozenState.GetSpell(SpellId.Frostbolt);
            FFB = castingState.GetSpell(SpellId.FrostfireBoltBF);
            FFBS = castingState.FrozenState.GetSpell(SpellId.FrostfireBoltBF);
            DFS = castingState.FrozenState.GetSpell(SpellId.DeepFreeze);

            float bf = 0.05f * castingState.MageTalents.BrainFreeze;
            float fof = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);
            float fof2 = fof * fof;
            float fof3 = fof2 * fof;
            float fof4 = fof3 * fof;
            float bf2 = bf * bf;
            float bf3 = bf2 * bf;

            // shatters until deep freeze ~ Poisson
            // share of shatters that are deep freeze = sum_i=0..inf Pi / sum_i=0..inf (i+1)*Pi = 1 / (1 + mean)

            // crude initial guess
            float X = 1.0f - 1.0f / (1.0f + (DFS.Cooldown - DFS.CastTime) / (FrB.CastTime * (1 / fof + 1) + FFBS.CastTime));

            float S20=((bf2*fof4+(bf-3*bf2)*fof3+(2*bf2-1)*fof2)*X*X+(-bf2*fof4+(3*bf2-bf)*fof3+(-2*bf2-bf+2)*fof2+(bf-2)*fof)*X+(bf-1)*fof2+(2-bf)*fof-1);
            float S11= ((bf3-bf)*fof3+(-3*bf3+bf2+2*bf)*fof2+(2*bf3-2*bf)*fof)*X+(bf-bf3)*fof3+(3*bf3-bf2-3*bf)*fof2+(5*bf-2*bf3)*fof-3*bf;
            float S10= ((fof2-fof)*S11*X+((1-bf)*fof+bf-1)*S20)/((bf-1)*fof*X+(1-bf)*fof-1);
            float S00= (-(bf*fof3-2*bf*fof2+bf*fof)*S10*X-(-fof2+2*fof-1)*S11-(-bf*fof2+(bf+1)*fof-1)*S10)/(bf*fof3-3*bf*fof2+(2*bf+1)*fof);
            float S21= ((bf*fof2-2*bf*fof)*S10*X-bf*fof*S20+(bf*fof2-2*bf*fof)*S00)/(fof-1);
            float S02= (1-fof)*(bf*(1-fof)*S10*X+bf*(1-fof)*S00)+(1-fof)*S11*(1-X)+bf*(1-fof)*S10*(1-X);
            float S01 = (1 - fof) * bf * S00 + X * (1 - fof) * bf * S10;

            float div = S00 + S01 + S02 + S10 + S11 + S20 + S21;

            KFrB = (S00 + S01) / div;
            KFFB = S02 / div;
            KFrBS = (S10 + S11 + S20 + S21) / div;
            KFFBS = X * S11 / div;
            KDFS = (1 - X) * (S10 + S11) / div;

            float hasteFactor = 1.0f;

            float T = KFrB * FrB.CastTime + KFFB * FFB.CastTime + KFrBS * FrBS.CastTime + KFFBS * FFBS.CastTime + KDFS * DFS.CastTime;
            float T0 = KFrBS * FrBS.CastTime / 2.0f;
            float T1 = KFFBS * FFBS.CastTime + KFFB * FFB.CastTime;

            if (castingState.Solver.Mage2T10)
            {
                // we'll make a lot of assumptions here and just assume that 2T10 haste is uniformly distributed over all
                // spells and doesn't have an impact on state space
                // also ignore the possible refresh of 2T10
                // each proc gives 12% haste for 5 sec
                // we have on average one proc every T/T1 * FFB.CastTime
                // we have some feedback loop here, speeding up the cycle increases the rate of procs
                // hastedShare = (5-FFB.CastTimeAverage) / (T/T1 * FFB.CastTimeAverage)
                // hastedCastShare = (5-FFB.CastTimeAverage) / (T/T1 * FFB.CastTime) * 1.12
                // average haste = 1 / (1-hastedCastShare*0.12/1.12)
                // TODO this is all a bunch of voodoo, redo the math when you're thinking straight
                hasteFactor = 1.0f / (1.0f - (5 - FFB.CastTime) / (T / T1 * FFB.CastTime) * 0.12f);

                // alternative model based on reduction to single state space and expanding for haste
                // probability of being hasted = 1 - (1-p)^(N-1)
                // where
                // K := (KFrB + KFB + KFrBS + KFBS + KILS + KDFS)
                // p = probability of haste generating spell = (KFBS + KFB) / K
                // N = average number of spells affected by haste = (haste duration - average cast time of haste generating spell) / (average cast time of hasted spells)
                //   = (5 - T1 / 1.12 / K) / (T / 1.12 / K)
                //   = (5 * 1.12 * K - T1) / T
                // hasteFactor = 1 / (((1-p)^(N-1)) * 1 + (1 - (1-p)^(N-1)) * 1/1.12)
                //             = 1.12 / (((1-p)^(N-1)) * 0.12 + 1)
                //             = 1.12 / (1 + 0.12 * (1 - (KFBS + KFB) / K)^((5 * 1.12 * K - T1) / T - 1))
                //float K = KFrB + KFB + KFrBS + KFBS + KILS + KDFS;
                //hasteFactor = 1.12f / (1.0f + 0.12f * (float)Math.Pow(1.0f - ((KFBS + KFB) / K), ((5.0f * 1.12f * K - T1) / T - 1.0f)));
            }

            if (fof > 0) // nothing new here if we don't have fof
            {
                // better estimate for percentage of shatter combos that are deep freeze
                // TODO better probabilistic model for DF percentage
                X = 1.0f - 1.0f / (1.0f + (DFS.Cooldown - DFS.CastTime / hasteFactor) / (FrBS.CastTime / hasteFactor * T / T0));

                // recalculate shares based on revised estimate
                S20 = ((bf2 * fof4 + (bf - 3 * bf2) * fof3 + (2 * bf2 - 1) * fof2) * X * X + (-bf2 * fof4 + (3 * bf2 - bf) * fof3 + (-2 * bf2 - bf + 2) * fof2 + (bf - 2) * fof) * X + (bf - 1) * fof2 + (2 - bf) * fof - 1);
                S11 = ((bf3 - bf) * fof3 + (-3 * bf3 + bf2 + 2 * bf) * fof2 + (2 * bf3 - 2 * bf) * fof) * X + (bf - bf3) * fof3 + (3 * bf3 - bf2 - 3 * bf) * fof2 + (5 * bf - 2 * bf3) * fof - 3 * bf;
                S10 = ((fof2 - fof) * S11 * X + ((1 - bf) * fof + bf - 1) * S20) / ((bf - 1) * fof * X + (1 - bf) * fof - 1);
                S00 = (-(bf * fof3 - 2 * bf * fof2 + bf * fof) * S10 * X - (-fof2 + 2 * fof - 1) * S11 - (-bf * fof2 + (bf + 1) * fof - 1) * S10) / (bf * fof3 - 3 * bf * fof2 + (2 * bf + 1) * fof);
                S21 = ((bf * fof2 - 2 * bf * fof) * S10 * X - bf * fof * S20 + (bf * fof2 - 2 * bf * fof) * S00) / (fof - 1);
                S02 = (1 - fof) * (bf * (1 - fof) * S10 * X + bf * (1 - fof) * S00) + (1 - fof) * S11 * (1 - X) + bf * (1 - fof) * S10 * (1 - X);
                S01 = (1 - fof) * bf * S00 + X * (1 - fof) * bf * S10;

                div = S00 + S01 + S02 + S10 + S11 + S20 + S21;

                KFrB = (S00 + S01) / div;
                KFFB = S02 / div;
                KFrBS = (S10 + S11 + S20 + S21) / div;
                KFFBS = X * S11 / div;
                KDFS = (1 - X) * (S10 + S11) / div;
            }

            cycle.AddSpell(needsDisplayCalculations, FrB, KFrB);
            cycle.AddSpell(needsDisplayCalculations, FFB, KFFB);
            cycle.AddSpell(needsDisplayCalculations, FrBS, KFrBS);
            cycle.AddSpell(needsDisplayCalculations, FFBS, KFFBS);
            cycle.AddSpell(needsDisplayCalculations, DFS, KDFS);
            cycle.CastTime /= hasteFactor; // ignores latency effects, but it'll have to do for now
            cycle.Calculate();
            return cycle;
        }
    }

    public static class FrBILFB
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            Spell FrB, FrBS, FB, ILS;
            float KFrB, KFrBS, KFB, KILS;
            cycle.Name = "FrBILFB";

            float T8 = 0;

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
            // FrBS-ILS => S12    fof
            //             S02    (1-fof)

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

            // S00 = (1-fof)*(1-bf) * S00 + S02*(1-T8) + (1-fof)*(1-bf) * S10
            // S01 = (1-fof) * bf * S00
            // S02 = (1-fof) * S01 + T8 * S02 + (1-fof) * bf * S10 + (1-fof) * S11
            // S10 = fof * (1-bf) * S10 + (1-fof)*(1-bf) * S20
            // S11 = fof * bf * S10 + fof * S11 + (1-fof) * bf * S20 + (1-fof) * S21
            // S20 = fof * (1-bf) * S00 + fof * (1-bf) * S20
            // S21 = fof * bf * S00 + fof * S01 + fof * bf * S20 + fof * S21
            // S00 + S01 + S02 + S10 + S11 + S20 + S21 = 1

            // solved symbolically

            float bf = 0.05f * castingState.MageTalents.BrainFreeze;
            float fof = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);

            //S00=(((bf^2-2*bf+1)*fof^3+(-bf^2+4*bf-3)*fof^2+(3-2*bf)*fof-1)*T8+(-bf^2+2*bf-1)*fof^3+(bf^2-4*bf+3)*fof^2+(2*bf-3)*fof+1)
            //S01=-(((bf^3-2*bf^2+bf)*fof^4+(-2*bf^3+6*bf^2-4*bf)*fof^3+(bf^3-6*bf^2+6*bf)*fof^2+(2*bf^2-4*bf)*fof+bf)*T8+(-bf^3+2*bf^2-bf)*fof^4+(2*bf^3-6*bf^2+4*bf)*fof^3+(-bf^3+6*bf^2-6*bf)*fof^2+(4*bf-2*bf^2)*fof-bf)
            //S02=((bf^3-2*bf^2+bf)*fof^3+(-2*bf^3+3*bf^2-bf)*fof^2+(bf^3-bf^2-bf)*fof+bf)
            //S10=-(((bf^2-2*bf+1)*fof^3+(-2*bf^2+4*bf-2)*fof^2+(bf^2-2*bf+1)*fof)*T8+(-bf^2+2*bf-1)*fof^3+(2*bf^2-4*bf+2)*fof^2+(-bf^2+2*bf-1)*fof)
            //S11=(((bf^3-2*bf^2+bf)*fof^4+(-bf^3+4*bf^2-3*bf)*fof^3+(5*bf-4*bf^2)*fof^2+(bf^2-3*bf)*fof)*T8+(-bf^3+2*bf^2-bf)*fof^4+(bf^3-4*bf^2+3*bf)*fof^3+(4*bf^2-5*bf)*fof^2+(3*bf-bf^2)*fof)
            //S20=-(((bf^2-2*bf+1)*fof^3+(-bf^2+3*bf-2)*fof^2+(1-bf)*fof)*T8+(-bf^2+2*bf-1)*fof^3+(bf^2-3*bf+2)*fof^2+(bf-1)*fof)
            //S21=(((bf^3-2*bf^2+bf)*fof^4+(-bf^3+4*bf^2-3*bf)*fof^3+(4*bf-3*bf^2)*fof^2-2*bf*fof)*T8+(-bf^3+2*bf^2-bf)*fof^4+(bf^3-4*bf^2+3*bf)*fof^3+(3*bf^2-4*bf)*fof^2+2*bf*fof)

            float S00 = (((bf * bf - 2 * bf + 1) * fof * fof * fof + (-bf * bf + 4 * bf - 3) * fof * fof + (3 - 2 * bf) * fof - 1) * T8 + (-bf * bf + 2 * bf - 1) * fof * fof * fof + (bf * bf - 4 * bf + 3) * fof * fof + (2 * bf - 3) * fof + 1);
            float S01 = -(((bf * bf * bf - 2 * bf * bf + bf) * fof * fof * fof * fof + (-2 * bf * bf * bf + 6 * bf * bf - 4 * bf) * fof * fof * fof + (bf * bf * bf - 6 * bf * bf + 6 * bf) * fof * fof + (2 * bf * bf - 4 * bf) * fof + bf) * T8 + (-bf * bf * bf + 2 * bf * bf - bf) * fof * fof * fof * fof + (2 * bf * bf * bf - 6 * bf * bf + 4 * bf) * fof * fof * fof + (-bf * bf * bf + 6 * bf * bf - 6 * bf) * fof * fof + (4 * bf - 2 * bf * bf) * fof - bf);
            float S02 = ((bf * bf * bf - 2 * bf * bf + bf) * fof * fof * fof + (-2 * bf * bf * bf + 3 * bf * bf - bf) * fof * fof + (bf * bf * bf - bf * bf - bf) * fof + bf);
            float S10 = -(((bf * bf - 2 * bf + 1) * fof * fof * fof + (-2 * bf * bf + 4 * bf - 2) * fof * fof + (bf * bf - 2 * bf + 1) * fof) * T8 + (-bf * bf + 2 * bf - 1) * fof * fof * fof + (2 * bf * bf - 4 * bf + 2) * fof * fof + (-bf * bf + 2 * bf - 1) * fof);
            float S11 = (((bf * bf * bf - 2 * bf * bf + bf) * fof * fof * fof * fof + (-bf * bf * bf + 4 * bf * bf - 3 * bf) * fof * fof * fof + (5 * bf - 4 * bf * bf) * fof * fof + (bf * bf - 3 * bf) * fof) * T8 + (-bf * bf * bf + 2 * bf * bf - bf) * fof * fof * fof * fof + (bf * bf * bf - 4 * bf * bf + 3 * bf) * fof * fof * fof + (4 * bf * bf - 5 * bf) * fof * fof + (3 * bf - bf * bf) * fof);
            float S20 = -(((bf * bf - 2 * bf + 1) * fof * fof * fof + (-bf * bf + 3 * bf - 2) * fof * fof + (1 - bf) * fof) * T8 + (-bf * bf + 2 * bf - 1) * fof * fof * fof + (bf * bf - 3 * bf + 2) * fof * fof + (bf - 1) * fof);
            float S21 = (((bf * bf * bf - 2 * bf * bf + bf) * fof * fof * fof * fof + (-bf * bf * bf + 4 * bf * bf - 3 * bf) * fof * fof * fof + (4 * bf - 3 * bf * bf) * fof * fof - 2 * bf * fof) * T8 + (-bf * bf * bf + 2 * bf * bf - bf) * fof * fof * fof * fof + (bf * bf * bf - 4 * bf * bf + 3 * bf) * fof * fof * fof + (3 * bf * bf - 4 * bf) * fof * fof + 2 * bf * fof);

            float div = S00 + S01 + S02 + S10 + S11 + S20 + S21;

            KFrB = (S00 + S01) / div;
            KFB = S02 / div;
            KFrBS = (S10 + S11 + S20 + S21) / div;
            KILS = (S10 + S11) / div;

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FrBS = castingState.FrozenState.GetSpell(SpellId.Frostbolt);
            FB = castingState.GetSpell(SpellId.FireballBF);
            ILS = castingState.FrozenState.GetSpell(SpellId.IceLance);

            cycle.AddSpell(needsDisplayCalculations, FrB, KFrB);
            cycle.AddSpell(needsDisplayCalculations, FB, KFB);
            cycle.AddSpell(needsDisplayCalculations, FrBS, KFrBS);
            cycle.AddSpell(needsDisplayCalculations, ILS, KILS);
            cycle.Calculate();
            return cycle;
        }
    }

    public static class FrBIL
    {
        public static Cycle GetCycle(bool needsDisplayCalculations, CastingState castingState)
        {
            Cycle cycle = Cycle.New(needsDisplayCalculations, castingState);
            Spell FrB, FrBS, ILS;
            float KFrB, KFrBS, KILS;
            cycle.Name = "FrBIL";

            //float T8 = 0;

            // S00: FOF0
            // FrB => S20    fof
            //        S00    (1-fof)

            // S10: FOF1, BF0
            // FrBS-ILS => S10    fof
            //             S00    (1-fof)

            // S20: FOF0, BF0
            // FrBS => S20    fof
            //         S10    (1-fof)


            // S00 = (1-fof) * S00 + (1-fof) * S10
            // S10 = fof * S10 + (1-fof) * S20
            // S20 = fof * S00 + fof * S20
            // S00 + S10 + S20 = 1

            float fof = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);

            float S00 = (1 - fof) / (1 + fof);
            float S10 = fof / (1 + fof);
            float S20 = fof / (1 + fof);

            KFrB = S00;
            KFrBS = S10 + S20;
            KILS = S10;

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FrBS = castingState.FrozenState.GetSpell(SpellId.Frostbolt);
            ILS = castingState.FrozenState.GetSpell(SpellId.IceLance);

            cycle.AddSpell(needsDisplayCalculations, FrB, KFrB);
            cycle.AddSpell(needsDisplayCalculations, FrBS, KFrBS);
            cycle.AddSpell(needsDisplayCalculations, ILS, KILS);
            cycle.Calculate();
            return cycle;
        }
    }

    public class FrostCycleGenerator : CycleGenerator
    {
        private class State : CycleState
        {
            public bool BrainFreezeRegistered { get; set; }
            public float BrainFreezeDuration { get; set; }
            public int FingersOfFrostRegistered { get; set; }
            public int FingersOfFrostActual { get; set; }
            public bool LatentFingersOfFrostWindow { get; set; }
            public bool DeepFreezeCooldown { get; set; }
        }

        public Spell FrB, FrBS, FB, FBS, IL, ILS, DFS;

        private float BF;
        private float FOF;
        private float T8;

        private bool deepFreeze;
        private bool useLatencyCombos;

        public FrostCycleGenerator(CastingState castingState, bool useLatencyCombos, bool useDeepFreeze)
        {
            this.useLatencyCombos = useLatencyCombos;

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FrBS = castingState.FrozenState.GetSpell(SpellId.Frostbolt);
            FB = castingState.GetSpell(SpellId.FireballBF);
            FBS = castingState.FrozenState.GetSpell(SpellId.FireballBF);
            IL = castingState.GetSpell(SpellId.IceLance);
            ILS = castingState.FrozenState.GetSpell(SpellId.IceLance);
            DFS = castingState.FrozenState.GetSpell(SpellId.DeepFreeze);

            BF = 0.05f * castingState.MageTalents.BrainFreeze;
            FOF = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);
            T8 = 0;
            deepFreeze = useDeepFreeze;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(false, 0.0f, 0, 0, false, false);
        }

        protected override List<CycleControlledStateTransition> GetStateTransitions(CycleState state)
        {
            State s = (State)state;
            List<CycleControlledStateTransition> list = new List<CycleControlledStateTransition>();
            Spell FrB = null;
            Spell IL = null;
            Spell FB = null;
            Spell DF = null;
            if (s.FingersOfFrostActual > 0)
            {
                FrB = this.FrBS;
            }
            else
            {
                FrB = this.FrB;
            }
            if (s.FingersOfFrostActual > 0 || (useLatencyCombos && s.LatentFingersOfFrostWindow))
            {
                IL = this.ILS;
                FB = this.FBS;
            }
            else
            {
                IL = this.IL;
                FB = this.FB;
            }
            if (!s.DeepFreezeCooldown && (s.FingersOfFrostRegistered > 0 || (useLatencyCombos && s.LatentFingersOfFrostWindow)))
            {
                DF = this.DFS;
            }
            if (FOF > 0 && BF > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = FrB,
                    TargetState = GetState(
                        s.BrainFreezeDuration > FrB.CastTime,
                        15.0f,
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        2,
                        s.FingersOfFrostActual > 0,
                        s.DeepFreezeCooldown && s.FingersOfFrostActual > 0
                    ),
                    TransitionProbability = FOF * BF
                });
            }
            if (FOF > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = FrB,
                    TargetState = GetState(
                        s.BrainFreezeDuration > FrB.CastTime,
                        Math.Max(0.0f, s.BrainFreezeDuration - FrB.CastTime),
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        2,
                        s.FingersOfFrostActual > 0,
                        s.DeepFreezeCooldown && s.FingersOfFrostActual > 0
                    ),
                    TransitionProbability = FOF * (1 - BF)
                });
            }
            if (BF > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = FrB,
                    TargetState = GetState(
                        s.BrainFreezeDuration > FrB.CastTime,
                        15.0f,
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        s.FingersOfFrostActual > 0,
                        s.DeepFreezeCooldown && s.FingersOfFrostActual > 0
                    ),
                    TransitionProbability = (1 - FOF) * BF
                });
            }
            list.Add(new CycleControlledStateTransition()
            {
                Spell = FrB,
                TargetState = GetState(
                    s.BrainFreezeDuration > FrB.CastTime,
                    Math.Max(0.0f, s.BrainFreezeDuration - FrB.CastTime),
                    Math.Max(0, s.FingersOfFrostActual - 1),
                    Math.Max(0, s.FingersOfFrostActual - 1),
                    s.FingersOfFrostActual > 0,
                    s.DeepFreezeCooldown && s.FingersOfFrostActual > 0
                ),
                TransitionProbability = (1 - FOF) * (1 - BF)
            });
            list.Add(new CycleControlledStateTransition()
            {
                Spell = IL,
                TargetState = GetState(
                    s.BrainFreezeDuration > IL.CastTime,
                    Math.Max(0.0f, s.BrainFreezeDuration - IL.CastTime),
                    Math.Max(0, s.FingersOfFrostActual - 1),
                    Math.Max(0, s.FingersOfFrostActual - 1),
                    s.FingersOfFrostActual > 1,
                    s.DeepFreezeCooldown && s.FingersOfFrostActual > 1
                ),
                TransitionProbability = 1
            });
            if (s.BrainFreezeRegistered)
            {
                if (T8 > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = FB,
                        TargetState = GetState(
                            s.BrainFreezeDuration > FB.CastTime,
                            Math.Max(0.0f, s.BrainFreezeDuration - FB.CastTime),
                            Math.Max(0, s.FingersOfFrostActual - 1),
                            Math.Max(0, s.FingersOfFrostActual - 1),
                            s.FingersOfFrostActual > 1,
                            s.DeepFreezeCooldown && s.FingersOfFrostActual > 1
                        ),
                        TransitionProbability = T8
                    });
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = FB,
                    TargetState = GetState(
                        false,
                        0.0f,
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        s.FingersOfFrostActual > 1,
                        s.DeepFreezeCooldown && s.FingersOfFrostActual > 1
                    ),
                    TransitionProbability = 1 - T8
                });
            }
            if (DF != null && deepFreeze)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = DF,
                    TargetState = GetState(
                        s.BrainFreezeDuration > DF.CastTime,
                        Math.Max(0.0f, s.BrainFreezeDuration - DF.CastTime),
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        s.FingersOfFrostActual > 1,
                        s.FingersOfFrostActual > 1
                    ),
                    TransitionProbability = 1
                });
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(bool brainFreezeRegistered, float brainFreezeDuration, int fingersOfFrostRegistered, int fingersOfFrostActual, bool latentFingersOfFrostWindow, bool deepFreezeCooldown)
        {
            if (!useLatencyCombos)
            {
                latentFingersOfFrostWindow = false;
            }
            string name = string.Format("BF{0}{1},FOF{2}{3}({4}),DF{5}", brainFreezeDuration, brainFreezeRegistered ? "+" : "-", fingersOfFrostRegistered, latentFingersOfFrostWindow ? "+" : "-", fingersOfFrostActual, deepFreezeCooldown ? "-" : "+");
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, BrainFreezeDuration = brainFreezeDuration, BrainFreezeRegistered = brainFreezeRegistered, FingersOfFrostActual = fingersOfFrostActual, FingersOfFrostRegistered = fingersOfFrostRegistered, LatentFingersOfFrostWindow = latentFingersOfFrostWindow, DeepFreezeCooldown = deepFreezeCooldown };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (
                a.FingersOfFrostRegistered != b.FingersOfFrostRegistered || 
                a.LatentFingersOfFrostWindow != b.LatentFingersOfFrostWindow || 
                a.DeepFreezeCooldown != b.DeepFreezeCooldown ||
                a.BrainFreezeRegistered != b.BrainFreezeRegistered);
        }

        public override string StateDescription
        {
            get
            {
                return @"Cycle Code Legend: 0 = FrB, 1 = IL, 2 = FB, 3 = DF
State Descriptions: BFx+-,FOFy+-(z),DF+-
x = remaining time on Brain Freeze
+ = Brain Freeze proc visible
- = Brain Freeze proc not visible
y = visible count on Fingers of Frost
+ = ghost Fingers of Frost charge for instant available
- = ghost Fingers of Frost charge for instant not available
z = actual count on Fingers of Frost
+ = Deep Freeze not on cooldown (within single FoF only)
- = Deep Freeze on cooldown (within single FoF only)";
            }
        }
    }

    public class FrostCycleGenerator2 : CycleGenerator
    {
        private class State : CycleState
        {
            public bool BrainFreezeRegistered { get; set; }
            public bool BrainFreezeProcced { get; set; }
            public int FingersOfFrostRegistered { get; set; }
            public int FingersOfFrostActual { get; set; }
            public bool LatentFingersOfFrostWindow { get; set; }
            public float DeepFreezeCooldown { get; set; }
            public float Tier10TwoPieceDuration { get; set; }
        }

        public Spell[,] FrB, FB, IL, DF;

        private float BF;
        private float FOF;
        private float T8;
        private bool T10;

        private bool deepFreeze;
        private float deepFreezeCooldown;
        private bool useLatencyCombos;
        private bool Tier10TwoPieceCollapsed;
        private bool fofInstantsOnLastChargeOnly;
        private bool ffbBrainFreeze;

        public FrostCycleGenerator2(CastingState castingState, bool useLatencyCombos, bool useDeepFreeze, float deepFreezeCooldown, bool tier10TwoPieceCollapsed, bool fofInstantsOnLastChargeOnly, bool ffbBrainFreeze)
        {
            this.useLatencyCombos = useLatencyCombos;
            this.Tier10TwoPieceCollapsed = tier10TwoPieceCollapsed;
            this.fofInstantsOnLastChargeOnly = fofInstantsOnLastChargeOnly;
            this.deepFreezeCooldown = deepFreezeCooldown;
            this.deepFreeze = useDeepFreeze;
            this.ffbBrainFreeze = ffbBrainFreeze;

            FrB = new Spell[2, 2];
            FB = new Spell[2, 2];
            IL = new Spell[2, 2];
            DF = new Spell[2, 2];

            for (int fof = 0; fof < 2; fof++)
            {
                for (int t10 = 0; t10 < 2; t10++)
                {
                    CastingState cstate = castingState;
                    string label = "";
                    if (t10 == 1)
                    {
                        cstate = cstate.Tier10TwoPieceState;
                        label = "2T10";
                    }
                    if (fof == 1)
                    {
                        cstate = cstate.FrozenState;
                        label = label.Length > 0 ? label + "+" + "FOF" : "FOF";
                    }
                    if (label.Length > 0)
                    {
                        label += ":";
                    }
                    FrB[fof, t10] = cstate.GetSpell(SpellId.Frostbolt);
                    FrB[fof, t10].Label = label + "Frostbolt";
                    if (ffbBrainFreeze)
                    {
                        FB[fof, t10] = cstate.GetSpell(SpellId.FrostfireBoltBF);
                        FB[fof, t10].Label = label + "FrostfireBolt";
                    }
                    else
                    {
                        FB[fof, t10] = cstate.GetSpell(SpellId.FireballBF);
                        FB[fof, t10].Label = label + "Fireball";
                    }
                    IL[fof, t10] = cstate.GetSpell(SpellId.IceLance);
                    IL[fof, t10].Label = label + "IceLance";
                    DF[fof, t10] = cstate.GetSpell(SpellId.DeepFreeze);
                    DF[fof, t10].Label = label + "DeepFreeze";
                }
            }

            BF = 0.05f * castingState.MageTalents.BrainFreeze;
            FOF = (castingState.MageTalents.FingersOfFrost == 2 ? 0.15f : 0.07f * castingState.MageTalents.FingersOfFrost);
            T8 = 0;
            T10 = castingState.Solver.Mage2T10;

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(false, false, 0, 0, false, 0.0f, 0.0f);
        }

        protected override List<CycleControlledStateTransition> GetStateTransitions(CycleState state)
        {
            State s = (State)state;
            List<CycleControlledStateTransition> list = new List<CycleControlledStateTransition>();
            Spell FrB = null;
            Spell IL = null;
            Spell FB = null;
            Spell DF = null;
            int t10 = s.Tier10TwoPieceDuration > 0 ? 1 : 0;
            if (s.FingersOfFrostActual > 0)
            {
                FrB = this.FrB[1, t10];
            }
            else
            {
                FrB = this.FrB[0, t10];
            }
            if (s.FingersOfFrostActual > 0 || (useLatencyCombos && s.LatentFingersOfFrostWindow))
            {
                IL = this.IL[1, t10];
                FB = this.FB[1, t10];
            }
            else
            {
                IL = this.IL[0, t10];
                FB = this.FB[0, t10];
            }
            if (s.DeepFreezeCooldown == 0.0f && (s.FingersOfFrostRegistered > 0 || (useLatencyCombos && s.LatentFingersOfFrostWindow)))
            {
                DF = this.DF[1, t10];
            }
            if (FOF > 0 && BF > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = FrB,
                    TargetState = GetState(
                        s.BrainFreezeProcced,
                        true,
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        2,
                        s.FingersOfFrostActual > 0,
                        Math.Max(0, s.DeepFreezeCooldown - FrB.CastTime),
                        Math.Max(0, s.Tier10TwoPieceDuration - FrB.CastTime)
                    ),
                    TransitionProbability = FOF * BF
                });
            }
            if (FOF > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = FrB,
                    TargetState = GetState(
                        s.BrainFreezeProcced,
                        s.BrainFreezeProcced,
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        2,
                        s.FingersOfFrostActual > 0,
                        Math.Max(0, s.DeepFreezeCooldown - FrB.CastTime),
                        Math.Max(0, s.Tier10TwoPieceDuration - FrB.CastTime)
                    ),
                    TransitionProbability = FOF * (1 - BF)
                });
            }
            if (BF > 0)
            {
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = FrB,
                    TargetState = GetState(
                        s.BrainFreezeProcced,
                        true,
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        Math.Max(0, s.FingersOfFrostActual - 1),
                        s.FingersOfFrostActual > 0,
                        Math.Max(0, s.DeepFreezeCooldown - FrB.CastTime),
                        Math.Max(0, s.Tier10TwoPieceDuration - FrB.CastTime)
                    ),
                    TransitionProbability = (1 - FOF) * BF
                });
            }
            list.Add(new CycleControlledStateTransition()
            {
                Spell = FrB,
                TargetState = GetState(
                    s.BrainFreezeProcced,
                    s.BrainFreezeProcced,
                    Math.Max(0, s.FingersOfFrostActual - 1),
                    Math.Max(0, s.FingersOfFrostActual - 1),
                    s.FingersOfFrostActual > 0,
                    Math.Max(0, s.DeepFreezeCooldown - FrB.CastTime),
                    Math.Max(0, s.Tier10TwoPieceDuration - FrB.CastTime)
                ),
                TransitionProbability = (1 - FOF) * (1 - BF)
            });
            if (!fofInstantsOnLastChargeOnly || !(s.FingersOfFrostRegistered > 0) || (useLatencyCombos && s.LatentFingersOfFrostWindow && s.FingersOfFrostRegistered == 0) || (!useLatencyCombos && s.FingersOfFrostRegistered == 1))
            {
                if (s.FingersOfFrostRegistered > 0 || (useLatencyCombos && s.LatentFingersOfFrostWindow))
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = IL,
                        TargetState = GetState(
                            s.BrainFreezeProcced,
                            s.BrainFreezeProcced,
                            Math.Max(0, s.FingersOfFrostActual - 1),
                            Math.Max(0, s.FingersOfFrostActual - 1),
                            s.FingersOfFrostActual > 1,
                            Math.Max(0, s.DeepFreezeCooldown - IL.CastTime),
                            Math.Max(0, s.Tier10TwoPieceDuration - IL.CastTime)
                        ),
                        TransitionProbability = 1
                    });
                }
                if (s.BrainFreezeRegistered)
                {
                    if (ffbBrainFreeze)
                    {
                        // T8 not supported for FFB mode
                        if (FOF > 0)
                        {
                            list.Add(new CycleControlledStateTransition()
                            {
                                Spell = FB,
                                TargetState = GetState(
                                    false,
                                    false,
                                    Math.Max(0, s.FingersOfFrostActual - 1),
                                    2,
                                    s.FingersOfFrostActual > 1,
                                    Math.Max(0, s.DeepFreezeCooldown - FB.CastTime),
                                    Math.Max(0, T10 ? 5.0f - FB.CastTime : s.Tier10TwoPieceDuration - FB.CastTime)
                                ),
                                TransitionProbability = FOF
                            });
                        }
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = FB,
                            TargetState = GetState(
                                false,
                                false,
                                Math.Max(0, s.FingersOfFrostActual - 1),
                                Math.Max(0, s.FingersOfFrostActual - 1),
                                s.FingersOfFrostActual > 1,
                                Math.Max(0, s.DeepFreezeCooldown - FB.CastTime),
                                Math.Max(0, T10 ? 5.0f - FB.CastTime : s.Tier10TwoPieceDuration - FB.CastTime)
                            ),
                            TransitionProbability = 1 - FOF
                        });
                    }
                    else
                    {
                        if (T8 > 0)
                        {
                            list.Add(new CycleControlledStateTransition()
                            {
                                Spell = FB,
                                TargetState = GetState(
                                    true,
                                    true,
                                    Math.Max(0, s.FingersOfFrostActual - 1),
                                    Math.Max(0, s.FingersOfFrostActual - 1),
                                    s.FingersOfFrostActual > 1,
                                    Math.Max(0, s.DeepFreezeCooldown - FB.CastTime),
                                    Math.Max(0, T10 ? 5.0f - FB.CastTime : s.Tier10TwoPieceDuration - FB.CastTime)
                                ),
                                TransitionProbability = T8
                            });
                        }
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = FB,
                            TargetState = GetState(
                                false,
                                false,
                                Math.Max(0, s.FingersOfFrostActual - 1),
                                Math.Max(0, s.FingersOfFrostActual - 1),
                                s.FingersOfFrostActual > 1,
                                Math.Max(0, s.DeepFreezeCooldown - FB.CastTime),
                                Math.Max(0, T10 ? 5.0f - FB.CastTime : s.Tier10TwoPieceDuration - FB.CastTime)
                            ),
                            TransitionProbability = 1 - T8
                        });
                    }
                }
                if (DF != null && deepFreeze)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = DF,
                        TargetState = GetState(
                            s.BrainFreezeProcced,
                            s.BrainFreezeProcced,
                            Math.Max(0, s.FingersOfFrostActual - 1),
                            Math.Max(0, s.FingersOfFrostActual - 1),
                            s.FingersOfFrostActual > 1,
                            Math.Max(0, deepFreezeCooldown - DF.CastTime),
                            Math.Max(0, s.Tier10TwoPieceDuration - DF.CastTime)
                        ),
                        TransitionProbability = 1
                    });
                }
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(bool brainFreezeRegistered, bool brainFreezeProcced, int fingersOfFrostRegistered, int fingersOfFrostActual, bool latentFingersOfFrostWindow, float deepFreezeCooldown, float tier10TwoPieceDuration)
        {
            if (!useLatencyCombos)
            {
                latentFingersOfFrostWindow = false;
            }
            string name = string.Format("BF{0}{1},FOF{2}{3}({4}),DF{5},2T10={6}", brainFreezeProcced ? "+" : "-", brainFreezeRegistered ? "+" : "-", fingersOfFrostRegistered, latentFingersOfFrostWindow ? "+" : "-", fingersOfFrostActual, deepFreezeCooldown, tier10TwoPieceDuration);
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, BrainFreezeProcced = brainFreezeProcced, BrainFreezeRegistered = brainFreezeRegistered, FingersOfFrostActual = fingersOfFrostActual, FingersOfFrostRegistered = fingersOfFrostRegistered, LatentFingersOfFrostWindow = latentFingersOfFrostWindow, DeepFreezeCooldown = deepFreezeCooldown, Tier10TwoPieceDuration = tier10TwoPieceDuration };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (
                a.FingersOfFrostRegistered != b.FingersOfFrostRegistered ||
                a.LatentFingersOfFrostWindow != b.LatentFingersOfFrostWindow ||
                (a.DeepFreezeCooldown == 0 && (a.FingersOfFrostRegistered > 0 || a.LatentFingersOfFrostWindow)) != (b.DeepFreezeCooldown == 0 && (b.FingersOfFrostRegistered > 0 || b.LatentFingersOfFrostWindow)) ||
                (!Tier10TwoPieceCollapsed && (a.Tier10TwoPieceDuration != b.Tier10TwoPieceDuration)) ||
                a.BrainFreezeRegistered != b.BrainFreezeRegistered);
        }

        public override string StateDescription
        {
            get
            {
                return @"Cycle Code Legend: 0 = FrB, 1 = IL, 2 = FB, 3 = DF
State Descriptions: BF+-+-,FOFy+-(z),DFw,2T10=z
+ = Brain Freeze procced
- = Brain Freeze not procced
+ = Brain Freeze proc visible
- = Brain Freeze proc not visible
y = visible count on Fingers of Frost
+ = ghost Fingers of Frost charge for instant available
- = ghost Fingers of Frost charge for instant not available
z = actual count on Fingers of Frost
w = Deep Freeze cooldown
z = remaining duration on 2T10";
            }
        }
    }

    public class FrostCycleGeneratorBeta : CycleGenerator
    {
        private class State : CycleState
        {
            public bool BrainFreezeRegistered { get; set; }
            public bool BrainFreezeProcced { get; set; }
            public int FingersOfFrostRegistered { get; set; }
            public int FingersOfFrostActual { get; set; }
            public float DeepFreezeCooldown { get; set; }
            public float FreezeCooldown { get; set; }
        }

        public Spell FrB, FFB, FFBF, IL, DF;

        private float BF;
        private float FOF;

        private bool deepFreeze;
        private float deepFreezeCooldown;
        private bool freeze;
        private float freezeCooldown;

        public FrostCycleGeneratorBeta(CastingState castingState, bool useDeepFreeze, float deepFreezeCooldown, bool useFreeze, float freezeCooldown)
        {
            this.deepFreezeCooldown = deepFreezeCooldown;
            this.deepFreeze = useDeepFreeze;
            this.freezeCooldown = freezeCooldown;
            this.freeze = useFreeze;

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FrB.Label = "Frostbolt";
            FFB = castingState.GetSpell(SpellId.FrostfireBoltBF);
            FFB.Label = "FrostfireBolt";
            FFBF = castingState.FrozenState.GetSpell(SpellId.FrostfireBoltBF);
            FFBF.Label = "Frozen+FrostfireBolt";
            IL = castingState.FrozenState.GetSpell(SpellId.IceLance);
            IL.Label = "Frozen+IceLance";
            DF = castingState.FrozenState.GetSpell(SpellId.DeepFreeze);
            DF.Label = "Frozen+DeepFreeze";

            BF = 0.05f * castingState.MageTalents.BrainFreeze;
            FOF = (castingState.MageTalents.FingersOfFrost == 3 ? 0.2f : 0.07f * castingState.MageTalents.FingersOfFrost);

            GenerateStateDescription();
        }

        protected override CycleState GetInitialState()
        {
            return GetState(false, false, 0, 0, 0.0f, 0.0f);
        }

        protected override List<CycleControlledStateTransition> GetStateTransitions(CycleState state)
        {
            State s = (State)state;
            List<CycleControlledStateTransition> list = new List<CycleControlledStateTransition>();
            Spell FrB = null;
            Spell IL = null;
            Spell FFB = null;
            Spell DF = null;
            FrB = this.FrB;
            if (s.FingersOfFrostActual > 0 || (s.FreezeCooldown == 0 && freeze))
            {
                FFB = this.FFBF;
            }
            else
            {
                FFB = this.FFB;
            }
            if (s.FingersOfFrostRegistered > 0 || (s.FreezeCooldown == 0 && freeze))
            {
                IL = this.IL;
            }
            if (s.DeepFreezeCooldown == 0.0f && (s.FingersOfFrostRegistered > 0 || (s.FreezeCooldown == 0 && freeze)))
            {
                DF = this.DF;
            }

            if (DF != null && deepFreeze)
            {
                if (s.FingersOfFrostRegistered > 0 || !freeze)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = DF,
                        TargetState = GetState(
                            s.BrainFreezeProcced,
                            s.BrainFreezeProcced,
                            Math.Max(0, s.FingersOfFrostActual - 1),
                            Math.Max(0, s.FingersOfFrostActual - 1),
                            Math.Max(0, deepFreezeCooldown - DF.CastTime),
                            Math.Max(0, s.FreezeCooldown - DF.CastTime)
                        ),
                        TransitionProbability = 1
                    });
                }
                else
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = DF,
                        TargetState = GetState(
                            s.BrainFreezeProcced,
                            s.BrainFreezeProcced,
                            Math.Max(0, 1),
                            Math.Max(0, 1),
                            Math.Max(0, deepFreezeCooldown - DF.CastTime),
                            Math.Max(0, freezeCooldown - DF.CastTime)
                        ),
                        TransitionProbability = 1
                    });
                }
            }
            else
            {
                if (FOF > 0 && BF > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = FrB,
                        TargetState = GetState(
                            s.BrainFreezeProcced,
                            true,
                            s.FingersOfFrostActual,
                            Math.Min(2, s.FingersOfFrostActual + 1),
                            Math.Max(0, s.DeepFreezeCooldown - FrB.CastTime),
                            Math.Max(0, s.FreezeCooldown - FrB.CastTime)
                        ),
                        TransitionProbability = FOF * BF
                    });
                }
                if (FOF > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = FrB,
                        TargetState = GetState(
                            s.BrainFreezeProcced,
                            s.BrainFreezeProcced,
                            s.FingersOfFrostActual,
                            Math.Min(2, s.FingersOfFrostActual + 1),
                            Math.Max(0, s.DeepFreezeCooldown - FrB.CastTime),
                            Math.Max(0, s.FreezeCooldown - FrB.CastTime)
                        ),
                        TransitionProbability = FOF * (1 - BF)
                    });
                }
                if (BF > 0)
                {
                    list.Add(new CycleControlledStateTransition()
                    {
                        Spell = FrB,
                        TargetState = GetState(
                            s.BrainFreezeProcced,
                            true,
                            s.FingersOfFrostActual,
                            s.FingersOfFrostActual,
                            Math.Max(0, s.DeepFreezeCooldown - FrB.CastTime),
                            Math.Max(0, s.FreezeCooldown - FrB.CastTime)
                        ),
                        TransitionProbability = (1 - FOF) * BF
                    });
                }
                list.Add(new CycleControlledStateTransition()
                {
                    Spell = FrB,
                    TargetState = GetState(
                        s.BrainFreezeProcced,
                        s.BrainFreezeProcced,
                        s.FingersOfFrostActual,
                        s.FingersOfFrostActual,
                        Math.Max(0, s.DeepFreezeCooldown - FrB.CastTime),
                        Math.Max(0, s.FreezeCooldown - FrB.CastTime)
                    ),
                    TransitionProbability = (1 - FOF) * (1 - BF)
                });
                if (IL != null)
                {
                    if (s.FingersOfFrostRegistered > 0 || !freeze)
                    {
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = IL,
                            TargetState = GetState(
                                s.BrainFreezeProcced,
                                s.BrainFreezeProcced,
                                Math.Max(0, s.FingersOfFrostActual - 1),
                                Math.Max(0, s.FingersOfFrostActual - 1),
                                Math.Max(0, s.DeepFreezeCooldown - IL.CastTime),
                                Math.Max(0, s.FreezeCooldown - IL.CastTime)
                            ),
                            TransitionProbability = 1
                        });
                    }
                    else
                    {
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = IL,
                            TargetState = GetState(
                                s.BrainFreezeProcced,
                                s.BrainFreezeProcced,
                                Math.Max(0, 1),
                                Math.Max(0, 1),
                                Math.Max(0, s.DeepFreezeCooldown - IL.CastTime),
                                Math.Max(0, freezeCooldown - IL.CastTime)
                            ),
                            TransitionProbability = 1
                        });
                    }
                }
                if (s.BrainFreezeRegistered)
                {
                    if (s.FingersOfFrostRegistered > 0 || !freeze)
                    {
                        if (FOF > 0)
                        {
                            list.Add(new CycleControlledStateTransition()
                            {
                                Spell = FFB,
                                TargetState = GetState(
                                    false,
                                    false,
                                    Math.Max(0, s.FingersOfFrostActual - 1) + 1,
                                    Math.Max(0, s.FingersOfFrostActual - 1) + 1,
                                    Math.Max(0, s.DeepFreezeCooldown - FFB.CastTime),
                                    Math.Max(0, s.FreezeCooldown - FFB.CastTime)
                                ),
                                TransitionProbability = FOF
                            });
                        }
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = FFB,
                            TargetState = GetState(
                                false,
                                false,
                                Math.Max(0, s.FingersOfFrostActual - 1),
                                Math.Max(0, s.FingersOfFrostActual - 1),
                                Math.Max(0, s.DeepFreezeCooldown - FFB.CastTime),
                                Math.Max(0, s.FreezeCooldown - FFB.CastTime)
                            ),
                            TransitionProbability = 1 - FOF
                        });
                    }
                    else
                    {
                        if (FOF > 0)
                        {
                            list.Add(new CycleControlledStateTransition()
                            {
                                Spell = FFB,
                                TargetState = GetState(
                                    false,
                                    false,
                                    Math.Max(0, 2 - 1) + 1,
                                    Math.Max(0, 2 - 1) + 1,
                                    Math.Max(0, s.DeepFreezeCooldown - FFB.CastTime),
                                    Math.Max(0, freezeCooldown - FFB.CastTime)
                                ),
                                TransitionProbability = FOF
                            });
                        }
                        list.Add(new CycleControlledStateTransition()
                        {
                            Spell = FFB,
                            TargetState = GetState(
                                false,
                                false,
                                Math.Max(0, 2 - 1),
                                Math.Max(0, 2 - 1),
                                Math.Max(0, s.DeepFreezeCooldown - FFB.CastTime),
                                Math.Max(0, freezeCooldown - FFB.CastTime)
                            ),
                            TransitionProbability = 1 - FOF
                        });
                    }
                }
            }

            return list;
        }

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>();

        private State GetState(bool brainFreezeRegistered, bool brainFreezeProcced, int fingersOfFrostRegistered, int fingersOfFrostActual, float deepFreezeCooldown, float freezeCooldown)
        {
            string name = string.Format("BF{0}{1},FOF{2}({3}),DF{4},F{5}", brainFreezeProcced ? "+" : "-", brainFreezeRegistered ? "+" : "-", fingersOfFrostRegistered, fingersOfFrostActual, deepFreezeCooldown, freezeCooldown);
            State state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new State() { Name = name, BrainFreezeProcced = brainFreezeProcced, BrainFreezeRegistered = brainFreezeRegistered, FingersOfFrostActual = fingersOfFrostActual, FingersOfFrostRegistered = fingersOfFrostRegistered, DeepFreezeCooldown = deepFreezeCooldown, FreezeCooldown = freezeCooldown };
                stateDictionary[name] = state;
            }
            return state;
        }

        protected override bool CanStatesBeDistinguished(CycleState state1, CycleState state2)
        {
            State a = (State)state1;
            State b = (State)state2;
            return (
                a.FingersOfFrostRegistered != b.FingersOfFrostRegistered ||
                (a.DeepFreezeCooldown == 0 && (a.FingersOfFrostRegistered > 0)) != (b.DeepFreezeCooldown == 0 && (b.FingersOfFrostRegistered > 0)) ||
                ((a.FreezeCooldown > 0) != (b.FreezeCooldown > 0)) ||
                a.BrainFreezeRegistered != b.BrainFreezeRegistered);
        }

        public override string StateDescription
        {
            get
            {
                return @"State Descriptions: BF+-+-,FOFy(z),DFw
+ = Brain Freeze procced
- = Brain Freeze not procced
+ = Brain Freeze proc visible
- = Brain Freeze proc not visible
y = visible count on Fingers of Frost
z = actual count on Fingers of Frost
w = Deep Freeze cooldown";
            }
        }
    }
}
