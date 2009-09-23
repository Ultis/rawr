using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public class ABSpam04MBAM : DynamicCycle
    {
        public ABSpam04MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2, K3, K4, K5, S0, S1;
            Name = "ABSpam04MBAM";

            // always ramp up to 4 AB before using MBAM

            // S0:
            // AB0-AB1-AB2-AB3-MBAM4 => S0       (1 - (1-MB)*(1-MB)*(1-MB))*(1-MB)*(1-T8)      one of the first three AB procs
            // AB0-AB1-AB2-AB3-MBAM4 => S2       (1 - (1-MB)*(1-MB)*(1-MB))*(1 - (1-T8)*(1-MB))      one of the first three AB procs
            // AB0-AB1-AB2-AB3-AB4-MBAM4 => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-MB)*(1-T8)       fourth AB procs
            // AB0-AB1-AB2-AB3-AB4-MBAM4 => S2   (1-MB)*(1-MB)*(1-MB)*MB*(1 - (1-T8)*(1-MB))       fourth AB procs
            // AB0-AB1-AB2-AB3 => S1            (1-MB)*(1-MB)*(1-MB)*(1-MB)   no procs
            // S1:
            // AB4-AB4-MBAM4 => S0           MB*(1-MB)*(1-T8)                     proc
            // AB4-AB4-MBAM4 => S2           MB*(1 - (1-T8)*(1-MB))                     proc
            // AB4 => S1                    (1-MB)                 no proc
            // S2:
            // MBAM0 => S0       (1-T8)     
            // MBAM0 => S2       T8     

            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

            // S0 = (1 - K0) * T1 * S0 + MB * T1 * S1 + (1-T8) * S2
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (1 - K0) * (1 - T1) * S0 + MB * (1 - T1) * S1 + T8 * S2
            // S0 + S1 + S2 = 1

            Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);
            Spell MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);

            float MB = 0.08f * castingState.MageTalents.MissileBarrage;
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            float T1 = (1 - MB) * (1 - T8);
            float div = (MB + K0) * T8 + MB * T1 - 2 * MB - K0;
            S0 = (MB * T8 - MB) / div;
            S1 = (K0 * T8 - K0) / div;
            float S2 = (MB * T1 - MB) / div;
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
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM0, S2);
            AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);

            Calculate();
        }
    }

    public class ABSpam4MBAM : DynamicCycle
    {
        public ABSpam4MBAM(bool needsDisplayCalculations, CastingState castingState)
            : base(needsDisplayCalculations, castingState)
        {
            float K1, K2, K3, K4, K5, S0, S1;
            Name = "ABSpam4MBAM";

            // always ramp up to 4 AB before using MBAM

            // S0:
            // AB0-AB1-AB2-AB3-MBAM => S0       (1 - (1-MB)*(1-MB)*(1-MB))*(1-MB)*(1-T8)      one of the first three AB procs
            // AB0-AB1-AB2-AB3-MBAM => S2       (1 - (1-MB)*(1-MB)*(1-MB))*(1 - (1-T8)*(1-MB))      one of the first three AB procs
            // AB0-AB1-AB2-AB3-AB4-MBAM => S0   (1-MB)*(1-MB)*(1-MB)*MB*(1-MB)*(1-T8)       fourth AB procs
            // AB0-AB1-AB2-AB3-AB4-MBAM => S2   (1-MB)*(1-MB)*(1-MB)*MB*(1 - (1-T8)*(1-MB))       fourth AB procs
            // AB0-AB1-AB2-AB3 => S1            (1-MB)*(1-MB)*(1-MB)*(1-MB)   no procs
            // S1:
            // AB4-AB4-MBAM => S0           MB*(1-MB)*(1-T8)                     proc
            // AB4-AB4-MBAM => S2           MB*(1 - (1-T8)*(1-MB))                     proc
            // AB4 => S1                    (1-MB)                 no proc
            // S2:
            // AB0-AB1-AB2-AB3-MBAM => S0       (1-MB)*(1-T8)     
            // AB0-AB1-AB2-AB3-MBAM => S2       (1 - (1-T8)*(1-MB))     

            // K0 = (1-MB)*(1-MB)*(1-MB)*(1-MB)

            // S0 = (1 - K0) * T1 * S0 + MB * T1 * S1 + T1 * S2
            // S1 = K0 * S0 + (1-MB) * S1
            // S2 = (1 - K0) * (1 - T1) * S0 + MB * (1 - T1) * S1 + (1 - T1) * S2
            // S0 + S1 + S2 = 1

            Spell MBAM4 = castingState.GetSpell(SpellId.ArcaneMissilesMB4);

            float MB = 0.08f * castingState.MageTalents.MissileBarrage;
            float T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;
            float K0 = (1 - MB) * (1 - MB) * (1 - MB) * (1 - MB);
            float T1 = (1 - MB) * (1 - T8);
            S0 = (T1 * MB) / (MB + K0 * T1);
            S1 = (K0 * T1) / (MB + K0 * T1);
            float S2 = ((1 - T1) * MB) / (MB + K0 * T1);
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
                AddSpell(needsDisplayCalculations, AB0, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB1, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB2, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB3, K1 + K2 + K3);
                AddSpell(needsDisplayCalculations, AB4, K2 + 2 * K4 + K5);
            }
            else
            {
                Spell AB = castingState.GetSpell(SpellId.ArcaneBlastRaw);
                castingState.Calculations.ArcaneBlastTemplate.AddToCycle(castingState.Calculations, this, AB, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K1 + K2 + K3, K2 + 2 * K4 + K5);
            }
            AddSpell(needsDisplayCalculations, MBAM4, K1 + K2 + K4);

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
                    AM = (s.MissileBarrageRegistered) ? MBAM0 : AM0;
                    break;
                case 1:
                    AB = AB1;
                    ABar = ABar1;
                    AM = (s.MissileBarrageRegistered) ? MBAM1 : AM1;
                    break;
                case 2:
                    AB = AB2;
                    ABar = ABar2;
                    AM = (s.MissileBarrageRegistered) ? MBAM2 : AM2;
                    break;
                case 3:
                    AB = AB3;
                    ABar = ABar3;
                    AM = (s.MissileBarrageRegistered) ? MBAM3 : AM3;
                    break;
                case 4:
                    AB = AB4;
                    ABar = ABar4;
                    AM = (s.MissileBarrageRegistered) ? MBAM4 : AM4;
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
            if (s.MissileBarrageRegistered && T8 > 0)
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
            // if MB duration is at max this means that it was a reproc from AB and it should not be consumed even if it was registered from earlier
            list.Add(new CycleControlledStateTransition()
            {
                Spell = AM,
                TargetState = GetState(
                    s.MissileBarrageRegistered && s.MissileBarrageDuration < 15.0f ? false : s.MissileBarrageDuration > AM.CastTime,
                    s.MissileBarrageRegistered && s.MissileBarrageDuration < 15.0f ? 0.0f : Math.Max(0.0f, s.MissileBarrageDuration - AM.CastTime),
                    Math.Max(0.0f, s.ArcaneBarrageCooldown - AM.CastTime),
                    0
                ),
                TransitionProbability = s.MissileBarrageRegistered ? 1.0f - T8 : 1.0f
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
    }

    class GenericArcane : Cycle
    {
        public Spell AB0, AB1, AB2, AB3, ABar0, ABar1, ABar2, ABar3, AM0, AM1, AM2, AM3, MBAM0, MBAM1, MBAM2, MBAM3;
        public double S00, S01, S02, S03, S10, S11, S12, S20, S21, S22, S30, S31, S32;
        public float KAB0, KAB1, KAB2, KAB3, KABar0, KABar1, KABar2, KABar3, KAM0, KAM1, KAM2, KAM3, KMBAM0, KMBAM1, KMBAM2, KMBAM3;
        public string SpellDistribution;

        private void AppendFormat(StringBuilder sb, string format, double weight)
        {
            if (weight > 0) sb.AppendFormat(format, weight);
        }

#if SILVERLIGHT
        public GenericArcane(string name, CastingState castingState, double X00, double X01, double X02, double X10, double X11, double X12, double X20, double X22, double X30, double X32, double X40, double X41, double X42, double X50, double X51, double X52, double X60, double X61, double X62, double X70, double X71, double X72, double X80, double X81, double X82, double X90, double X91, double X92)
#else
        public unsafe GenericArcane(string name, CastingState castingState, double X00, double X01, double X02, double X10, double X11, double X12, double X20, double X22, double X30, double X32, double X40, double X41, double X42, double X50, double X51, double X52, double X60, double X61, double X62, double X70, double X71, double X72, double X80, double X81, double X82, double X90, double X91, double X92)
#endif
            : base(castingState)
        {
            Name = name;

            const int size = 13;

            ArraySet arraySet = ArrayPool.RequestArraySet(size, size);
            LU M = new LU(size, arraySet);

            double[] X = new double[size];

            const int s00 = 0;
            const int s01 = 1;
            const int s02 = 2;
            const int s03 = 12;
            const int s10 = 3;
            const int s11 = 4;
            const int s12 = 5;
            const int s20 = 6;
            const int s21 = 7;
            const int s22 = 8;
            const int s30 = 9;
            const int s31 = 10;
            const int s32 = 11;

            double MB = 0.04 * castingState.MageTalents.MissileBarrage;
            double T8 = CalculationOptionsMage.SetBonus4T8ProcRate * castingState.BaseStats.Mage4T8;

#if SILVERLIGHT
            double[] U = arraySet.LU_U;
            double[] x = X;
            {
                M.BeginSafe();

                for (int replace = size - 1; replace >= size - 1; replace--)
                {
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            U[i * size + j] = 0;
                        }
                    }

                    //U[i * rows + j]

                    //AB0,MB0,ABar+: S00
                    //AB0       => AB1,MB0,ABar+    X00*(1-MB)
                    //AB0       => AB1,MB1,ABar+    X00*MB
                    //ABar      => AB0,MB0,ABar-    X01*(1-MB)
                    //ABar      => AB0,MB2,ABar-    X01*MB
                    //AM        => AB0,MB0,ABar+    X02
                    U[s00 * size + s00] = X02 - 1;
                    U[s01 * size + s00] = X01 * (1 - MB);
                    U[s02 * size + s00] = X01 * MB;
                    U[s10 * size + s00] = X00 * (1 - MB);
                    U[s11 * size + s00] = X00 * MB;

                    //AB0,MB0,ABar-: S01
                    //AB0       => AB1,MB0,ABar+    X20*(1-MB)
                    //AB0       => AB1,MB1,ABar+    X20*MB
                    //AM        => AB0,MB0,ABar+    X22
                    U[s00 * size + s01] = X22;
                    U[s01 * size + s01] = -1;
                    U[s10 * size + s01] = X20 * (1 - MB);
                    U[s11 * size + s01] = X20 * MB;

                    //AB0,MB2,ABar-: S02
                    //AB0       => AB1,MB2,ABar+    X30
                    //MBAM      => AB0,MB0,ABar+    X32*(1-T8)
                    //MBAM      => AB0,MB2,ABar+    X32*T8
                    U[s00 * size + s02] = X32 * (1 - T8);
                    U[s02 * size + s02] = -1;
                    U[s03 * size + s02] = X32 * T8;
                    U[s12 * size + s02] = X30;

                    //AB0,MB2,ABar+: S03
                    //AB0       => AB1,MB2,ABar+    X90
                    //ABar      => AB0,MB2,ABar-    X91
                    //MBAM      => AB0,MB0,ABar+    X92*(1-T8)
                    //MBAM      => AB0,MB2,ABar+    X92*T8
                    U[s00 * size + s03] = X92 * (1 - T8);
                    U[s02 * size + s03] = X91;
                    U[s03 * size + s03] = X92 * T8 - 1;
                    U[s12 * size + s03] = X90;

                    //AB1,MB0,ABar+: S10
                    //AB1       => AB2,MB0,ABar+    X10*(1-MB)
                    //AB1       => AB2,MB1,ABar+    X10*MB
                    //ABar1     => AB0,MB0,ABar-    X11*(1-MB)
                    //ABar1     => AB0,MB2,ABar-    X11*MB
                    //AM1       => AB0,MB0,ABar+    X12
                    U[s00 * size + s10] = X12;
                    U[s01 * size + s10] = X11 * (1 - MB);
                    U[s02 * size + s10] = X11 * MB;
                    U[s20 * size + s10] = X10 * (1 - MB);
                    U[s21 * size + s10] = X10 * MB;
                    U[s10 * size + s10] = -1;

                    //AB1,MB1,ABar+: S11
                    //AB1       => AB2,MB2,ABar+    X10
                    //ABar1     => AB0,MB2,ABar-    X11
                    //MBAM1     => AB0,MB0,ABar+    X12*(1-T8)
                    //MBAM1     => AB0,MB2,ABar+    X12*T8
                    U[s00 * size + s11] = X12 * (1 - T8);
                    U[s02 * size + s11] = X11;
                    U[s03 * size + s11] = X12 * T8;
                    U[s22 * size + s11] = X10;
                    U[s11 * size + s11] = -1;

                    //AB1,MB2,ABar+: S12
                    //AB1       => AB2,MB2,ABar+    X40
                    //ABar1     => AB0,MB2,ABar-    X41
                    //MBAM1     => AB0,MB0,ABar+    X42*(1-T8)
                    //MBAM1     => AB0,MB2,ABar+    X42*T8
                    U[s00 * size + s12] = X42 * (1 - T8);
                    U[s02 * size + s12] = X41;
                    U[s03 * size + s12] = X42 * T8;
                    U[s22 * size + s12] = X40;
                    U[s12 * size + s12] = -1;

                    //AB2,MB0,ABar+: S20
                    //AB2       => AB3,MB0,ABar+    X50*(1-MB)
                    //AB2       => AB3,MB1,ABar+    X50*MB
                    //ABar2     => AB0,MB0,ABar-    X51*(1-MB)
                    //ABar2     => AB0,MB2,ABar-    X51*MB
                    //AM2       => AB0,MB0,ABar+    X52
                    U[s00 * size + s20] = X52;
                    U[s01 * size + s20] = X51 * (1 - MB);
                    U[s02 * size + s20] = X51 * MB;
                    U[s30 * size + s20] = X50 * (1 - MB);
                    U[s31 * size + s20] = X50 * MB;
                    U[s20 * size + s20] = -1;

                    //AB2,MB1,ABar+: S21
                    //AB2       => AB3,MB2,ABar+    X50
                    //ABar2     => AB0,MB2,ABar-    X51
                    //MBAM2     => AB0,MB0,ABar+    X52*(1-T8)
                    //MBAM2     => AB0,MB2,ABar+    X52*T8
                    U[s00 * size + s21] = X52 * (1 - T8);
                    U[s02 * size + s21] = X51;
                    U[s03 * size + s21] = X52 * T8;
                    U[s32 * size + s21] = X50;
                    U[s21 * size + s21] = -1;

                    //AB2,MB2,ABar+: S22
                    //AB2       => AB3,MB2,ABar+    X60
                    //ABar2     => AB0,MB2,ABar-    X61
                    //MBAM2     => AB0,MB0,ABar+    X62*(1-T8)
                    //MBAM2     => AB0,MB2,ABar+    X62*T8
                    U[s00 * size + s22] = X62 * (1 - T8);
                    U[s02 * size + s22] = X61;
                    U[s03 * size + s22] = X62 * T8;
                    U[s32 * size + s22] = X60;
                    U[s22 * size + s22] = -1;

                    //AB3,MB0,ABar+: S30
                    //AB3       => AB3,MB0,ABar+    X70*(1-MB)
                    //AB3       => AB3,MB1,ABar+    X70*MB
                    //ABar3     => AB0,MB0,ABar-    X71*(1-MB)
                    //ABar3     => AB0,MB2,ABar-    X71*MB
                    //AM3       => AB0,MB0,ABar+    X72
                    U[s00 * size + s30] = X72;
                    U[s01 * size + s30] = X71 * (1 - MB);
                    U[s02 * size + s30] = X71 * MB;
                    U[s30 * size + s30] = X70 * (1 - MB) - 1;
                    U[s31 * size + s30] = X70 * MB;

                    //AB3,MB1,ABar+: S31
                    //AB3       => AB3,MB2,ABar+    X70
                    //ABar3     => AB0,MB2,ABar-    X71
                    //MBAM3     => AB0,MB0,ABar+    X72*(1-T8)
                    //MBAM3     => AB0,MB2,ABar+    X72*T8
                    U[s00 * size + s31] = X72 * (1 - T8);
                    U[s02 * size + s31] = X71;
                    U[s03 * size + s31] = X72 * T8;
                    U[s32 * size + s31] = X70;
                    U[s31 * size + s31] = -1;

                    //AB3,MB2,ABar+: S32
                    //AB3       => AB3,MB2,ABar+    X80
                    //ABar3     => AB0,MB2,ABar-    X81
                    //MBAM3     => AB0,MB0,ABar+    X82*(1-T8)
                    //MBAM3     => AB0,MB2,ABar+    X82*T8
                    U[s00 * size + s32] = X82 * (1 - T8);
                    U[s02 * size + s32] = X81;
                    U[s03 * size + s32] = X82 * T8;
                    U[s32 * size + s32] = X80 - 1;

                    // the above system is singular, "guess" which one is dependent and replace with sum=1
                    // since not all states are used always we'll get a singular system anyway sometimes, but in those cases the FSolve should still work ok on the nonsingular part
                    for (int i = 0; i < size; i++) x[i] = 0;

                    if (replace < size)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            U[replace * size + i] = 1;
                        }

                        x[replace] = 1;
                    }

                    M.Decompose();
                    if (!M.Singular) break;
                }
                M.FSolve(x);

                M.EndUnsafe();

                S00 = x[s00];
                S01 = x[s01];
                S02 = x[s02];
                S03 = x[s03];
                S10 = x[s10];
                S11 = x[s11];
                S12 = x[s12];
                S20 = x[s20];
                S21 = x[s21];
                S22 = x[s22];
                S30 = x[s30];
                S31 = x[s31];
                S32 = x[s32];
            }
#else
            fixed (double* U = arraySet.LU_U, x = X)
            fixed (double* sL = arraySet.LUsparseL, column = arraySet.LUcolumn, column2 = arraySet.LUcolumn2)
            fixed (int* P = arraySet.LU_P, Q = arraySet.LU_Q, LJ = arraySet.LU_LJ, sLI = arraySet.LUsparseLI, sLstart = arraySet.LUsparseLstart)
            {
                M.BeginUnsafe(U, sL, P, Q, LJ, sLI, sLstart, column, column2);

                for (int replace = size - 1; replace >= size - 1; replace--)
                {
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            U[i * size + j] = 0;
                        }
                    }

                    //U[i * rows + j]

                    //AB0,MB0,ABar+: S00
                    //AB0       => AB1,MB0,ABar+    X00*(1-MB)
                    //AB0       => AB1,MB1,ABar+    X00*MB
                    //ABar      => AB0,MB0,ABar-    X01*(1-MB)
                    //ABar      => AB0,MB2,ABar-    X01*MB
                    //AM        => AB0,MB0,ABar+    X02
                    U[s00 * size + s00] = X02 - 1;
                    U[s01 * size + s00] = X01 * (1 - MB);
                    U[s02 * size + s00] = X01 * MB;
                    U[s10 * size + s00] = X00 * (1 - MB);
                    U[s11 * size + s00] = X00 * MB;

                    //AB0,MB0,ABar-: S01
                    //AB0       => AB1,MB0,ABar+    X20*(1-MB)
                    //AB0       => AB1,MB1,ABar+    X20*MB
                    //AM        => AB0,MB0,ABar+    X22
                    U[s00 * size + s01] = X22;
                    U[s01 * size + s01] = -1;
                    U[s10 * size + s01] = X20 * (1 - MB);
                    U[s11 * size + s01] = X20 * MB;

                    //AB0,MB2,ABar-: S02
                    //AB0       => AB1,MB2,ABar+    X30
                    //MBAM      => AB0,MB0,ABar+    X32*(1-T8)
                    //MBAM      => AB0,MB2,ABar+    X32*T8
                    U[s00 * size + s02] = X32 * (1 - T8);
                    U[s02 * size + s02] = -1;
                    U[s03 * size + s02] = X32 * T8;
                    U[s12 * size + s02] = X30;

                    //AB0,MB2,ABar+: S03
                    //AB0       => AB1,MB2,ABar+    X90
                    //ABar      => AB0,MB2,ABar-    X91
                    //MBAM      => AB0,MB0,ABar+    X92*(1-T8)
                    //MBAM      => AB0,MB2,ABar+    X92*T8
                    U[s00 * size + s03] = X92 * (1 - T8);
                    U[s02 * size + s03] = X91;
                    U[s03 * size + s03] = X92 * T8 - 1;
                    U[s12 * size + s03] = X90;

                    //AB1,MB0,ABar+: S10
                    //AB1       => AB2,MB0,ABar+    X10*(1-MB)
                    //AB1       => AB2,MB1,ABar+    X10*MB
                    //ABar1     => AB0,MB0,ABar-    X11*(1-MB)
                    //ABar1     => AB0,MB2,ABar-    X11*MB
                    //AM1       => AB0,MB0,ABar+    X12
                    U[s00 * size + s10] = X12;
                    U[s01 * size + s10] = X11 * (1 - MB);
                    U[s02 * size + s10] = X11 * MB;
                    U[s20 * size + s10] = X10 * (1 - MB);
                    U[s21 * size + s10] = X10 * MB;
                    U[s10 * size + s10] = -1;

                    //AB1,MB1,ABar+: S11
                    //AB1       => AB2,MB2,ABar+    X10
                    //ABar1     => AB0,MB2,ABar-    X11
                    //MBAM1     => AB0,MB0,ABar+    X12*(1-T8)
                    //MBAM1     => AB0,MB2,ABar+    X12*T8
                    U[s00 * size + s11] = X12 * (1 - T8);
                    U[s02 * size + s11] = X11;
                    U[s03 * size + s11] = X12 * T8;
                    U[s22 * size + s11] = X10;
                    U[s11 * size + s11] = -1;

                    //AB1,MB2,ABar+: S12
                    //AB1       => AB2,MB2,ABar+    X40
                    //ABar1     => AB0,MB2,ABar-    X41
                    //MBAM1     => AB0,MB0,ABar+    X42*(1-T8)
                    //MBAM1     => AB0,MB2,ABar+    X42*T8
                    U[s00 * size + s12] = X42 * (1 - T8);
                    U[s02 * size + s12] = X41;
                    U[s03 * size + s12] = X42 * T8;
                    U[s22 * size + s12] = X40;
                    U[s12 * size + s12] = -1;

                    //AB2,MB0,ABar+: S20
                    //AB2       => AB3,MB0,ABar+    X50*(1-MB)
                    //AB2       => AB3,MB1,ABar+    X50*MB
                    //ABar2     => AB0,MB0,ABar-    X51*(1-MB)
                    //ABar2     => AB0,MB2,ABar-    X51*MB
                    //AM2       => AB0,MB0,ABar+    X52
                    U[s00 * size + s20] = X52;
                    U[s01 * size + s20] = X51 * (1 - MB);
                    U[s02 * size + s20] = X51 * MB;
                    U[s30 * size + s20] = X50 * (1 - MB);
                    U[s31 * size + s20] = X50 * MB;
                    U[s20 * size + s20] = -1;

                    //AB2,MB1,ABar+: S21
                    //AB2       => AB3,MB2,ABar+    X50
                    //ABar2     => AB0,MB2,ABar-    X51
                    //MBAM2     => AB0,MB0,ABar+    X52*(1-T8)
                    //MBAM2     => AB0,MB2,ABar+    X52*T8
                    U[s00 * size + s21] = X52 * (1 - T8);
                    U[s02 * size + s21] = X51;
                    U[s03 * size + s21] = X52 * T8;
                    U[s32 * size + s21] = X50;
                    U[s21 * size + s21] = -1;

                    //AB2,MB2,ABar+: S22
                    //AB2       => AB3,MB2,ABar+    X60
                    //ABar2     => AB0,MB2,ABar-    X61
                    //MBAM2     => AB0,MB0,ABar+    X62*(1-T8)
                    //MBAM2     => AB0,MB2,ABar+    X62*T8
                    U[s00 * size + s22] = X62 * (1 - T8);
                    U[s02 * size + s22] = X61;
                    U[s03 * size + s22] = X62 * T8;
                    U[s32 * size + s22] = X60;
                    U[s22 * size + s22] = -1;

                    //AB3,MB0,ABar+: S30
                    //AB3       => AB3,MB0,ABar+    X70*(1-MB)
                    //AB3       => AB3,MB1,ABar+    X70*MB
                    //ABar3     => AB0,MB0,ABar-    X71*(1-MB)
                    //ABar3     => AB0,MB2,ABar-    X71*MB
                    //AM3       => AB0,MB0,ABar+    X72
                    U[s00 * size + s30] = X72;
                    U[s01 * size + s30] = X71 * (1 - MB);
                    U[s02 * size + s30] = X71 * MB;
                    U[s30 * size + s30] = X70 * (1 - MB) - 1;
                    U[s31 * size + s30] = X70 * MB;

                    //AB3,MB1,ABar+: S31
                    //AB3       => AB3,MB2,ABar+    X70
                    //ABar3     => AB0,MB2,ABar-    X71
                    //MBAM3     => AB0,MB0,ABar+    X72*(1-T8)
                    //MBAM3     => AB0,MB2,ABar+    X72*T8
                    U[s00 * size + s31] = X72 * (1 - T8);
                    U[s02 * size + s31] = X71;
                    U[s03 * size + s31] = X72 * T8;
                    U[s32 * size + s31] = X70;
                    U[s31 * size + s31] = -1;

                    //AB3,MB2,ABar+: S32
                    //AB3       => AB3,MB2,ABar+    X80
                    //ABar3     => AB0,MB2,ABar-    X81
                    //MBAM3     => AB0,MB0,ABar+    X82*(1-T8)
                    //MBAM3     => AB0,MB2,ABar+    X82*T8
                    U[s00 * size + s32] = X82 * (1 - T8);
                    U[s02 * size + s32] = X81;
                    U[s03 * size + s32] = X82 * T8;
                    U[s32 * size + s32] = X80 - 1;

                    // the above system is singular, "guess" which one is dependent and replace with sum=1
                    // since not all states are used always we'll get a singular system anyway sometimes, but in those cases the FSolve should still work ok on the nonsingular part
                    for (int i = 0; i < size; i++) x[i] = 0;

                    if (replace < size)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            U[replace * size + i] = 1;
                        }

                        x[replace] = 1;
                    }

                    M.Decompose();
                    if (!M.Singular) break;
                }
                M.FSolve(x);

                M.EndUnsafe();

                S00 = x[s00];
                S01 = x[s01];
                S02 = x[s02];
                S03 = x[s03];
                S10 = x[s10];
                S11 = x[s11];
                S12 = x[s12];
                S20 = x[s20];
                S21 = x[s21];
                S22 = x[s22];
                S30 = x[s30];
                S31 = x[s31];
                S32 = x[s32];
            }
#endif

            AB0 = castingState.GetSpell(SpellId.ArcaneBlast0);
            AB1 = castingState.GetSpell(SpellId.ArcaneBlast1);
            AB2 = castingState.GetSpell(SpellId.ArcaneBlast2);
            AB3 = castingState.GetSpell(SpellId.ArcaneBlast3);
            ABar0 = castingState.GetSpell(SpellId.ArcaneBarrage);
            ABar1 = castingState.GetSpell(SpellId.ArcaneBarrage1);
            ABar2 = castingState.GetSpell(SpellId.ArcaneBarrage2);
            ABar3 = castingState.GetSpell(SpellId.ArcaneBarrage3);
            AM0 = castingState.GetSpell(SpellId.ArcaneMissiles);
            AM1 = castingState.GetSpell(SpellId.ArcaneMissiles1);
            AM2 = castingState.GetSpell(SpellId.ArcaneMissiles2);
            AM3 = castingState.GetSpell(SpellId.ArcaneMissiles3);
            MBAM0 = castingState.GetSpell(SpellId.ArcaneMissilesMB);
            MBAM1 = castingState.GetSpell(SpellId.ArcaneMissilesMB1);
            MBAM2 = castingState.GetSpell(SpellId.ArcaneMissilesMB2);
            MBAM3 = castingState.GetSpell(SpellId.ArcaneMissilesMB3);

            KAB0 = (float)(S00 * X00 + S01 * X20 + S02 * X30 + S03 * X90);
            KABar0 = (float)(S00 * X01 + S03 * X91);
            KAM0 = (float)(S00 * X02 + S01 * X22);
            KMBAM0 = (float)(S02 * X32 + S03 * X92);
            KAB1 = (float)(S10 * X10 + S11 * X10 + S12 * X40);
            KABar1 = (float)(S10 * X11 + S11 * X11 + S12 * X41);
            KAM1 = (float)(S10 * X12);
            KMBAM1 = (float)(S11 * X12 + S12 * X42);
            KAB2 = (float)(S20 * X50 + S21 * X50 + S22 * X60);
            KABar2 = (float)(S20 * X51 + S21 * X51 + S22 * X61);
            KAM2 = (float)(S20 * X52);
            KMBAM2 = (float)(S21 * X52 + S22 * X62);
            KAB3 = (float)(S30 * X70 + S31 * X70 + S32 * X80);
            KABar3 = (float)(S30 * X71 + S31 * X71 + S32 * X81);
            KAM3 = (float)(S30 * X72);
            KMBAM3 = (float)(S31 * X72 + S32 * X82);

            CastTime = KAB0 * AB0.CastTime + KABar0 * ABar0.CastTime + KAM0 * AM0.CastTime + KMBAM0 * MBAM0.CastTime + KAB1 * AB1.CastTime + KABar1 * ABar1.CastTime + KAM1 * AM1.CastTime + KMBAM1 * MBAM1.CastTime + KAB2 * AB2.CastTime + KABar2 * ABar2.CastTime + KAM2 * AM2.CastTime + KMBAM2 * MBAM2.CastTime + KAB3 * AB3.CastTime + KABar3 * ABar3.CastTime + KAM3 * AM3.CastTime + KMBAM3 * MBAM3.CastTime;
            costPerSecond = (KAB0 * AB0.CastTime * AB0.CostPerSecond + KABar0 * ABar0.CastTime * ABar0.CostPerSecond + KAM0 * AM0.CastTime * AM0.CostPerSecond + KMBAM0 * MBAM0.CastTime * MBAM0.CostPerSecond + KAB1 * AB1.CastTime * AB1.CostPerSecond + KABar1 * ABar1.CastTime * ABar1.CostPerSecond + KAM1 * AM1.CastTime * AM1.CostPerSecond + KMBAM1 * MBAM1.CastTime * MBAM1.CostPerSecond + KAB2 * AB2.CastTime * AB2.CostPerSecond + KABar2 * ABar2.CastTime * ABar2.CostPerSecond + KAM2 * AM2.CastTime * AM2.CostPerSecond + KMBAM2 * MBAM2.CastTime * MBAM2.CostPerSecond + KAB3 * AB3.CastTime * AB3.CostPerSecond + KABar3 * ABar3.CastTime * ABar3.CostPerSecond + KAM3 * AM3.CastTime * AM3.CostPerSecond + KMBAM3 * MBAM3.CastTime * MBAM3.CostPerSecond) / CastTime;
            damagePerSecond = (KAB0 * AB0.CastTime * AB0.DamagePerSecond + KABar0 * ABar0.CastTime * ABar0.DamagePerSecond + KAM0 * AM0.CastTime * AM0.DamagePerSecond + KMBAM0 * MBAM0.CastTime * MBAM0.DamagePerSecond + KAB1 * AB1.CastTime * AB1.DamagePerSecond + KABar1 * ABar1.CastTime * ABar1.DamagePerSecond + KAM1 * AM1.CastTime * AM1.DamagePerSecond + KMBAM1 * MBAM1.CastTime * MBAM1.DamagePerSecond + KAB2 * AB2.CastTime * AB2.DamagePerSecond + KABar2 * ABar2.CastTime * ABar2.DamagePerSecond + KAM2 * AM2.CastTime * AM2.DamagePerSecond + KMBAM2 * MBAM2.CastTime * MBAM2.DamagePerSecond + KAB3 * AB3.CastTime * AB3.DamagePerSecond + KABar3 * ABar3.CastTime * ABar3.DamagePerSecond + KAM3 * AM3.CastTime * AM3.DamagePerSecond + KMBAM3 * MBAM3.CastTime * MBAM3.DamagePerSecond) / CastTime;
            threatPerSecond = (KAB0 * AB0.CastTime * AB0.ThreatPerSecond + KABar0 * ABar0.CastTime * ABar0.ThreatPerSecond + KAM0 * AM0.CastTime * AM0.ThreatPerSecond + KMBAM0 * MBAM0.CastTime * MBAM0.ThreatPerSecond + KAB1 * AB1.CastTime * AB1.ThreatPerSecond + KABar1 * ABar1.CastTime * ABar1.ThreatPerSecond + KAM1 * AM1.CastTime * AM1.ThreatPerSecond + KMBAM1 * MBAM1.CastTime * MBAM1.ThreatPerSecond + KAB2 * AB2.CastTime * AB2.ThreatPerSecond + KABar2 * ABar2.CastTime * ABar2.ThreatPerSecond + KAM2 * AM2.CastTime * AM2.ThreatPerSecond + KMBAM2 * MBAM2.CastTime * MBAM2.ThreatPerSecond + KAB3 * AB3.CastTime * AB3.ThreatPerSecond + KABar3 * ABar3.CastTime * ABar3.ThreatPerSecond + KAM3 * AM3.CastTime * AM3.ThreatPerSecond + KMBAM3 * MBAM3.CastTime * MBAM3.ThreatPerSecond) / CastTime;

            StringBuilder sb = new StringBuilder();
            AppendFormat(sb, "AB0:\t{0:F}%\r\n", 100.0 * (S00 * X00 + S01 * X20 + S02 * X30));
            AppendFormat(sb, "ABar0:\t{0:F}%\r\n", 100.0 * (S00 * X01));
            AppendFormat(sb, "AM0:\t{0:F}%\r\n", 100.0 * (S00 * X02 + S01 * X22));
            AppendFormat(sb, "MBAM0:\t{0:F}%\r\n", 100.0 * (S02 * X32));

            AppendFormat(sb, "AB1:\t{0:F}%\r\n", 100.0 * (S10 * X10 + S11 * X10 + S12 * X40));
            AppendFormat(sb, "ABar1:\t{0:F}%\r\n", 100.0 * (S10 * X11 + S11 * X11 + S12 * X41));
            AppendFormat(sb, "AM1:\t{0:F}%\r\n", 100.0 * (S10 * X12));
            AppendFormat(sb, "MBAM1:\t{0:F}%\r\n", 100.0 * (S11 * X12 + S12 * X42));

            AppendFormat(sb, "AB2:\t{0:F}%\r\n", 100.0 * (S20 * X50 + S21 * X50 + S22 * X60));
            AppendFormat(sb, "ABar2:\t{0:F}%\r\n", 100.0 * (S20 * X51 + S21 * X51 + S22 * X61));
            AppendFormat(sb, "AM2:\t{0:F}%\r\n", 100.0 * (S20 * X52));
            AppendFormat(sb, "MBAM2:\t{0:F}%\r\n", 100.0 * (S21 * X52 + S22 * X62));

            AppendFormat(sb, "AB3:\t{0:F}%\r\n", 100.0 * (S30 * X70 + S31 * X70 + S32 * X80));
            AppendFormat(sb, "ABar3:\t{0:F}%\r\n", 100.0 * (S30 * X71 + S31 * X71 + S32 * X81));
            AppendFormat(sb, "AM3:\t{0:F}%\r\n", 100.0 * (S30 * X72));
            AppendFormat(sb, "MBAM3:\t{0:F}%\r\n", 100.0 * (S31 * X72 + S32 * X82));

            SpellDistribution = sb.ToString();
            ArrayPool.ReleaseArraySet(arraySet);
        }

        public override string Sequence
        {
            get
            {
                return "GenericArcane";
            }
        }

        public override void AddSpellContribution(Dictionary<string, SpellContribution> dict, float duration, float effectSpellPower)
        {
            AB0.AddSpellContribution(dict, KAB0 * AB0.CastTime / CastTime * duration, effectSpellPower);
            ABar0.AddSpellContribution(dict, KABar0 * ABar0.CastTime / CastTime * duration, effectSpellPower);
            AM0.AddSpellContribution(dict, KAM0 * AM0.CastTime / CastTime * duration, effectSpellPower);
            MBAM0.AddSpellContribution(dict, KMBAM0 * MBAM0.CastTime / CastTime * duration, effectSpellPower);
            AB1.AddSpellContribution(dict, KAB1 * AB1.CastTime / CastTime * duration, effectSpellPower);
            ABar1.AddSpellContribution(dict, KABar1 * ABar1.CastTime / CastTime * duration, effectSpellPower);
            AM1.AddSpellContribution(dict, KAM1 * AM1.CastTime / CastTime * duration, effectSpellPower);
            MBAM1.AddSpellContribution(dict, KMBAM1 * MBAM1.CastTime / CastTime * duration, effectSpellPower);
            AB2.AddSpellContribution(dict, KAB2 * AB2.CastTime / CastTime * duration, effectSpellPower);
            ABar2.AddSpellContribution(dict, KABar2 * ABar2.CastTime / CastTime * duration, effectSpellPower);
            AM2.AddSpellContribution(dict, KAM2 * AM2.CastTime / CastTime * duration, effectSpellPower);
            MBAM2.AddSpellContribution(dict, KMBAM2 * MBAM2.CastTime / CastTime * duration, effectSpellPower);
            AB3.AddSpellContribution(dict, KAB3 * AB3.CastTime / CastTime * duration, effectSpellPower);
            ABar3.AddSpellContribution(dict, KABar3 * ABar3.CastTime / CastTime * duration, effectSpellPower);
            AM3.AddSpellContribution(dict, KAM3 * AM3.CastTime / CastTime * duration, effectSpellPower);
            MBAM3.AddSpellContribution(dict, KMBAM3 * MBAM3.CastTime / CastTime * duration, effectSpellPower);
        }

        public override void AddManaSourcesContribution(Dictionary<string, float> dict, float duration)
        {
            throw new NotImplementedException();
        }

        public override void AddManaUsageContribution(Dictionary<string, float> dict, float duration)
        {
            throw new NotImplementedException();
        }
    }
}
