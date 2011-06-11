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
        // interpolation on haste, nodes every 5% haste
        private static float[,] CastDistribution = new float[21, 5]
        {{0.7157861f,	0f,	0.085539f,	    0.1399948f,	    0.058680058f},
        {0.721539557f,	0f,	0.08603663f,	0.136260092f,	0.056163713f},
        {0.72453934f,	0f,	0.08594114f,	0.135948941f,	0.05357056f},
        {0.7301836f,	0f,	0.0863862f,	    0.132187933f,	0.051242285f},
        {0.7330192f,	0f,	0.086870365f,	0.130937025f,	0.049173456f},
        {0.7371909f,	0f,	0.08679625f,	0.128678024f,	0.047334842f},
        {0.740539551f,	0f,	0.08730111f,	0.126491666f,	0.04566765f},
        {0.7440289f,	0f,	0.087509655f,	0.124477684f,	0.043983698f},
        {0.746264637f,	0f,	0.08748013f,	0.123845078f,	0.042410113f},
        {0.7486419f,	0f,	0.087904334f,	0.122434862f,	0.041018877f},
        {0.7515504f,	0f,	0.08799671f,	0.12071982f,	0.039733145f},
        {0.7537658f,	0f,	0.08803718f,	0.119323894f,	0.038873088f},
        {0.7553765f,	0f,	0.08810475f,	0.118425064f,	0.038093668f},
        {0.7571395f,	0f,	0.08822203f,	0.117465571f,	0.037172858f},
        {0.7580866f,	0f,	0.08829209f,	0.117605865f,	0.036015358f},
        {0.760549247f,	0f,	0.088211365f,	0.1157174f,	    0.035521943f},
        {0.760740936f,	0f,	0.088395126f,	0.1162385f,	    0.034625463f},
        {0.7634897f,	0f,	0.08835807f,	0.114246659f,	0.033905577f},
        {0.763518453f,	0f,	0.088534616f,	0.114513695f,	0.033433206f},
        {0.76615274f,	0f,	0.08868719f,	0.112837039f,	0.03232302f},
        {0.766139865f,	0f,	0.08863693f,	0.112900153f,	0.03232304f}};
        private static float[,] CastDistributionFFO = new float[21, 5]
        {{0.491966039f,	0f,	0.226755485f,	0.22519511f,	0.056083325f},
        {0.5040986f,	0f,	0.222027585f,	0.220168188f,	0.053705636f},
        {0.513947368f,	0f,	0.218099445f,	0.2164993f,	    0.051453874f},
        {0.5250243f,	0f,	0.2140254f,	    0.2115933f,	    0.049357045f},
        {0.533787966f,	0f,	0.21004355f,	0.208755672f,	0.04741281f},
        {0.543759763f,	0f,	0.206295818f,	0.204271153f,	0.04567321f},
        {0.5522804f,	0f,	0.202769175f,	0.200873256f,	0.04407718f},
        {0.55993855f,	0f,	0.199471f,	    0.1980493f,	    0.042541146f},
        {0.5681519f,	0f,	0.196498647f,	0.194225475f,	0.041123964f},
        {0.5749693f,	0f,	0.193494767f,	0.19174777f,	0.03978816f},
        {0.5822501f,	0f,	0.190802053f,	0.188389063f,	0.03855871f},
        {0.5867951f,	0f,	0.189169884f,	0.186088681f,	0.03794636f},
        {0.590621531f,	0f,	0.18746084f,	0.184543952f,	0.03737368f},
        {0.594657362f,	0f,	0.1860552f,	    0.182732135f,	0.0365553f},
        {0.5982177f,	0f,	0.184560671f,	0.1812985f,	    0.0359231f},
        {0.6013578f,	0f,	0.183169439f,	0.180305526f,	0.03516726f},
        {0.605428159f,	0f,	0.181768268f,	0.17795822f,	0.03484537f},
        {0.6080714f,	0f,	0.180478483f,	0.177521229f,	0.0339289f},
        {0.611364245f,	0f,	0.179141417f,	0.175587669f,	0.03390669f},
        {0.614726543f,	0f,	0.17822212f,	0.1742635f,	    0.03278781f},
        {0.616703749f,	0f,	0.176965117f,	0.173551589f,	0.032779545f}};
        private static float[,] CastDistributionT11 = new float[21, 5]
        {{0.7240437f,	0f,	0.08583403f,	0.136183023f,	0.05393924f},
        {0.7287668f,	0f,	0.08668169f,	0.133173227f,	0.0513783f},
        {0.7330106f,	0f,	0.08683309f,	0.13096118f,	0.049195185f},
        {0.7364949f,	0f,	0.086648904f,	0.129529446f,	0.04732675f},
        {0.7412851f,	0f,	0.08698201f,	0.126115486f,	0.04561743f},
        {0.7440329f,	0f,	0.08755244f,	0.124534026f,	0.04388061f},
        {0.7464007f,	0f,	0.08747157f,	0.124046348f,	0.04208137f},
        {0.750229061f,	0f,	0.08759505f,	0.121547796f,	0.040628064f},
        {0.752744853f,	0f,	0.08808802f,	0.119869545f,	0.039297573f},
        {0.7544785f,	0f,	0.08802639f,	0.119419083f,	0.038075987f},
        {0.7574128f,	0f,	0.08808565f,	0.117576338f,	0.036925215f},
        {0.7597843f,	0f,	0.08846713f,	0.115840048f,	0.03590857f},
        {0.7605631f,	0f,	0.08835957f,	0.116190083f,	0.034887254f},
        {0.7632261f,	0f,	0.088433415f,	0.113767356f,	0.03457312f},
        {0.763483942f,	0f,	0.08849296f,	0.114589363f,	0.03343375f},
        {0.766129851f,	0f,	0.08867436f,	0.112817913f,	0.032377873f},
        {0.7661175f,	0f,	0.08868439f,	0.112874158f,	0.03232393f},
        {0.7661879f,	0f,	0.08866255f,	0.112831093f,	0.032318477f},
        {0.766159534f,	0f,	0.08868681f,	0.112832136f,	0.032321516f},
        {0.7661285f,	0f,	0.088667646f,	0.112881459f,	0.03232237f},
        {0.766159832f,	0f,	0.08864395f,	0.112875558f,	0.032320663f}};
        private static float[,] CastDistributionT11FFO = new float[21, 5]
        {{0.5064794f,	0f,	0.220768452f,	0.219918534f,	0.05283361f},
        {0.51713f,	    0f,	0.2165683f,	    0.215657309f,	0.050644346f},
        {0.528974533f,	0f,	0.212381348f,	0.210019439f,	0.048624653f},
        {0.539946854f,	0f,	0.208183646f,	0.205216676f,	0.04665284f},
        {0.549020648f,	0f,	0.204062611f,	0.202194184f,	0.044722535f},
        {0.5578711f,	0f,	0.20053108f,	0.198691845f,	0.04290594f},
        {0.566934347f,	0f,	0.197269052f,	0.194501281f,	0.04129531f},
        {0.574531734f,	0f,	0.193768f,	    0.191471115f,	0.040229175f},
        {0.5816688f,	0f,	0.190866753f,	0.1885619f,	    0.03890256f},
        {0.5890516f,	0f,	0.188143045f,	0.185174316f,	0.037631024f},
        {0.595508635f,	0f,	0.185440764f,	0.182656482f,	0.036394123f},
        {0.599441946f,	0f,	0.183949351f,	0.180975065f,	0.035633653f},
        {0.603909254f,	0f,	0.182379887f,	0.178606138f,	0.035104755f},
        {0.6073667f,	0f,	0.180913448f,	0.177589417f,	0.034130435f},
        {0.609929562f,	0f,	0.179504737f,	0.176650032f,	0.033915684f},
        {0.614497f,	    0f,	0.178361878f,	0.174352f,	    0.032789104f},
        {0.6166332f,	0f,	0.176984042f,	0.1736029f,	    0.03277989f},
        {0.616728246f,	0f,	0.1768961f,	    0.173595279f,	0.03278041f},
        {0.6166569f,	0f,	0.176937014f,	0.173626512f,	0.032779574f},
        {0.6166615f,	0f,	0.176908925f,	0.173647508f,	0.03278206f},
        {0.61670357f,	0f,	0.176935852f,	0.1735808f,	    0.032779768f}};
        private static float[,] CastDistributionT12 = new float[21, 5]
	    {{0.706252f,	0f,	0.133225f,	0.1031823f,	    0.05734068f},
	    {0.7120382f,	0f,	0.1338209f,	0.09929287f,	0.05484801f},
	    {0.7151474f,	0f,	0.1333017f,	0.09916303f,	0.05238784f},
	    {0.7205276f,	0f,	0.1331322f,	0.09619394f,	0.0501462f},
	    {0.7235046f,	0f,	0.1340708f,	0.09431984f,	0.04810472f},
	    {0.7276107f,	0f,	0.1334465f,	0.09269033f,	0.04625241f},
	    {0.730621f,	    0f,	0.1337644f,	0.09098524f,	0.04462929f},
	    {0.7342333f,	0f,	0.1338861f,	0.08885222f,	0.04302839f},
	    {0.736442f,	    0f,	0.1335555f,	0.08849932f,	0.04150319f},
	    {0.7388258f,	0f,	0.134181f,	0.08687305f,	0.04012015f},
	    {0.7416363f,	0f,	0.1339485f,	0.08556953f,	0.03884568f},
	    {0.743588f,	    0f,	0.133672f,	0.08470091f,	0.03803909f},
	    {0.7454485f,	0f,	0.1337775f,	0.08350687f,	0.03726713f},
	    {0.7470396f,	0f,	0.1334852f,	0.08305957f,	0.03641563f},
	    {0.748008f,	    0f,	0.1338823f,	0.08276368f,	0.03534608f},
	    {0.7502965f,	0f,	0.1331005f,	0.08184797f,	0.03475505f},
	    {0.7505655f,	0f,	0.1336799f,	0.08173961f,	0.03401494f},
	    {0.7531676f,	0f,	0.1331804f,	0.08044228f,	0.03320982f},
	    {0.7531664f,	0f,	0.1334017f,	0.08058503f,	0.03284679f},
	    {0.7557487f,	0f,	0.133393f,	0.07909141f,	0.03176685f},
	    {0.7557488f,	0f,	0.1333961f,	0.07908709f,	0.03176798f}};
        private static float[,] CastDistributionT12FFO = new float[21, 5]
	    {{0.4687241f,	0f,	0.3177885f,	0.1581333f,	0.0553541f},
	    {0.4811813f,	0f,	0.3123612f,	0.153508f,	0.05294952f},
	    {0.4906897f,	0f,	0.3080013f,	0.1505895f,	0.05071948f},
	    {0.5015014f,	0f,	0.3035668f,	0.14624f,	0.0486918f},
	    {0.510736f,	    0f,	0.298745f,	0.14373f,	0.046789f},
	    {0.5207732f,	0f,	0.2941628f,	0.140029f,	0.04503507f},
	    {0.5292103f,	0f,	0.2899091f,	0.1374499f,	0.0434306f},
	    {0.5371782f,	0f,	0.2854958f,	0.1353987f,	0.04192728f},
	    {0.5452931f,	0f,	0.2817981f,	0.1323696f,	0.04053913f},
	    {0.5524104f,	0f,	0.2782257f,	0.1301366f,	0.03922724f},
	    {0.5598132f,	0f,	0.2746831f,	0.1275034f,	0.03800023f},
	    {0.5641169f,	0f,	0.2725927f,	0.125818f,	0.0374724f},
	    {0.5681915f,	0f,	0.2703645f,	0.124596f,	0.036848f},
	    {0.5721451f,	0f,	0.268714f,	0.1229821f,	0.03615874f},
	    {0.5755674f,	0f,	0.2667559f,	0.1222469f,	0.03542995f},
	    {0.5790635f,	0f,	0.2650871f,	0.120998f,	0.0348514f},
	    {0.5828797f,	0f,	0.2634758f,	0.1192735f,	0.03437104f},
	    {0.5854599f,	0f,	0.2617701f,	0.1191155f,	0.03365446f},
	    {0.5891193f,	0f,	0.2601747f,	0.1171092f,	0.03359687f},
	    {0.5922822f,	0f,	0.2586771f,	0.1165186f,	0.03252209f},
	    {0.5942183f,	0f,	0.2571276f,	0.1161443f,	0.03250973f}};

        public static void SolveCycle(CastingState castingState, bool useFFO, out float KFrB, out float KFFB, out float KFFBS, out float KILS, out float KDFS)
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

            // override this so we don't have to store so many variations (means talent graphs won't show value for this talent)
            bf = 0.3f; // testing possible 4T12 value
            fof = 0.2f;

            float dfCooldown = 0;
            float freezeCooldown = 0;
            int fofActual = 0;
            int fofRegistered = 0;
            bool bfRegistered = false;
            bool bfActual = false;
            float ffo = 1;

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
                        while (useFFO && ffo < DFS.CastTime)
                        {
                            bool fofProc = rnd.NextDouble() < fof;
                            bool bfProc = rnd.NextDouble() < bf;
                            bfActual = bfProc || bfActual;
                            if (fofProc)
                            {
                                fofActual = Math.Min(2, fofActual + 1);
                            }
                            ffo += 1;
                        }
                        if (useFFO)
                        {
                            ffo -= DFS.CastTime;
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
                        while (useFFO && ffo < FFBS.CastTime)
                        {
                            bool fofProc = rnd.NextDouble() < fof;
                            bool bfProc = rnd.NextDouble() < bf;
                            bfActual = bfProc || bfActual;
                            if (fofProc)
                            {
                                fofActual = Math.Min(2, fofActual + 1);
                            }
                            ffo += 1;
                        }
                        if (useFFO)
                        {
                            ffo -= FFBS.CastTime;
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
                        while (useFFO && ffo < ILS.CastTime)
                        {
                            bool fofProc = rnd.NextDouble() < fof;
                            bool bfProc = rnd.NextDouble() < bf;
                            bfActual = bfProc || bfActual;
                            if (fofProc)
                            {
                                fofActual = Math.Min(2, fofActual + 1);
                            }
                            ffo += 1;
                        }
                        if (useFFO)
                        {
                            ffo -= ILS.CastTime;
                        }
                    }
                    else if (fofRegistered == 0 && freezeCooldown == 0)
                    {
                        CILS++;
                        bfRegistered = bfActual;
                        fofActual = Math.Max(0, 2 - 1);
                        fofRegistered = fofActual;
                        dfCooldown = Math.Max(0, dfCooldown - ILS.CastTime);
                        freezeCooldown = Math.Max(0, 25.0f - ILS.CastTime);
                        while (useFFO && ffo < ILS.CastTime)
                        {
                            bool fofProc = rnd.NextDouble() < fof;
                            bool bfProc = rnd.NextDouble() < bf;
                            bfActual = bfProc || bfActual;
                            if (fofProc)
                            {
                                fofActual = Math.Min(2, fofActual + 1);
                            }
                            ffo += 1;
                        }
                        if (useFFO)
                        {
                            ffo -= ILS.CastTime;
                        }
                    }
                    else
                    {
                        while (useFFO && ffo < FrB.CastTime)
                        {
                            bool fofProcffo = rnd.NextDouble() < fof;
                            bool bfProcffo = rnd.NextDouble() < bf;
                            bfActual = bfProcffo || bfActual;
                            if (fofProcffo)
                            {
                                fofActual = Math.Min(2, fofActual + 1);
                            }
                            ffo += 1;
                        }
                        if (useFFO)
                        {
                            ffo -= FrB.CastTime;
                        }
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
            float KFrB, KFFB, KFFBS, KILS, KDFS;
            cycle.Name = "FrBDFFFBIL";

            FrB = castingState.GetSpell(SpellId.Frostbolt);
            FFB = castingState.GetSpell(SpellId.FrostfireBoltBF);
            FFBS = castingState.FrozenState.GetSpell(SpellId.FrostfireBoltBF);
            ILS = castingState.FrozenState.GetSpell(SpellId.IceLance);
            DFS = castingState.FrozenState.GetSpell(SpellId.DeepFreeze);

            // get the right distribution bank
            float[,] castDistribution;
            float[,] castDistributionFFO;
            if (castingState.Solver.Mage4T11)
            {
                castDistribution = CastDistributionT11;
                castDistributionFFO = CastDistributionT11FFO;
            }
            else if (castingState.Solver.Mage4T12)
            {
                castDistribution = CastDistributionT12;
                castDistributionFFO = CastDistributionT12FFO;
            }
            else
            {
                castDistribution = CastDistribution;
                castDistributionFFO = CastDistributionFFO;
            }

            // check if we have interpolation nodes ready
            float r = (castingState.CastingSpeed - 1) / 0.05f;
            int i = (int)r;
            r -= i;

            // if we're out of bounds just use the edge
            if (i + 1 >= 21)
            {
                i = 19;
                r = 1;
            }

            // uncomment this to get code for cast distributions to paste
            /*lock (castDistribution)
            {
                for (i = 0; i < 21; i++)
                {
                    CastingState state = castingState.Clone();
                    state.CastingSpeed = 1 + i * 0.05f;
                    state.ReferenceCastingState = castingState;
                    SolveCycle(state, false, out castDistribution[i, 0], out castDistribution[i, 1], out castDistribution[i, 2], out castDistribution[i, 3], out castDistribution[i, 4]);
                    System.Diagnostics.Trace.WriteLine(string.Format("\t{{{0}f,	{1}f,	{2}f,	{3}f,	{4}f}},", castDistribution[i, 0], castDistribution[i, 1], castDistribution[i, 2], castDistribution[i, 3], castDistribution[i, 4]));
                }
                Console.WriteLine();
                for (i = 0; i < 21; i++)
                {
                    CastingState state = castingState.Clone();
                    state.CastingSpeed = 1 + i * 0.05f;
                    state.ReferenceCastingState = castingState;
                    SolveCycle(state, true, out castDistribution[i, 0], out castDistribution[i, 1], out castDistribution[i, 2], out castDistribution[i, 3], out castDistribution[i, 4]);
                    System.Diagnostics.Trace.WriteLine(string.Format("\t{{{0}f,	{1}f,	{2}f,	{3}f,	{4}f}},", castDistribution[i, 0], castDistribution[i, 1], castDistribution[i, 2], castDistribution[i, 3], castDistribution[i, 4]));
                }
                Console.WriteLine();
            }*/

            if (castingState.CalculationOptions.FlameOrb == 0 || (castingState.CalculationOptions.FlameOrb == 2 && !castingState.FlameOrb))
            {
                KFrB = castDistribution[i, 0] + r * (castDistribution[i + 1, 0] - castDistribution[i, 0]);
                KFFB = castDistribution[i, 1] + r * (castDistribution[i + 1, 1] - castDistribution[i, 1]);
                KFFBS = castDistribution[i, 2] + r * (castDistribution[i + 1, 2] - castDistribution[i, 2]);
                KILS = castDistribution[i, 3] + r * (castDistribution[i + 1, 3] - castDistribution[i, 3]);
                KDFS = castDistribution[i, 4] + r * (castDistribution[i + 1, 4] - castDistribution[i, 4]);
            }
            else if (castingState.CalculationOptions.FlameOrb == 1)
            {
                KFrB = 0.75f * (castDistribution[i, 0] + r * (castDistribution[i + 1, 0] - castDistribution[i, 0])) + 0.25f * (castDistributionFFO[i, 0] + r * (castDistributionFFO[i + 1, 0] - castDistributionFFO[i, 0]));
                KFFB = 0.75f * (castDistribution[i, 1] + r * (castDistribution[i + 1, 1] - castDistribution[i, 1])) + 0.25f * (castDistributionFFO[i, 1] + r * (castDistributionFFO[i + 1, 1] - castDistributionFFO[i, 1]));
                KFFBS = 0.75f * (castDistribution[i, 2] + r * (castDistribution[i + 1, 2] - castDistribution[i, 2])) + 0.25f * (castDistributionFFO[i, 2] + r * (castDistributionFFO[i + 1, 2] - castDistributionFFO[i, 2]));
                KILS = 0.75f * (castDistribution[i, 3] + r * (castDistribution[i + 1, 3] - castDistribution[i, 3])) + 0.25f * (castDistributionFFO[i, 3] + r * (castDistributionFFO[i + 1, 3] - castDistributionFFO[i, 3]));
                KDFS = 0.75f * (castDistribution[i, 4] + r * (castDistribution[i + 1, 4] - castDistribution[i, 4])) + 0.25f * (castDistributionFFO[i, 4] + r * (castDistributionFFO[i + 1, 4] - castDistributionFFO[i, 4]));
            }
            else //if (castingState.CalculationOptions.FlameOrb == 2 && castingState.FlameOrb)
            {
                KFrB = castDistributionFFO[i, 0] + r * (castDistributionFFO[i + 1, 0] - castDistributionFFO[i, 0]);
                KFFB = castDistributionFFO[i, 1] + r * (castDistributionFFO[i + 1, 1] - castDistributionFFO[i, 1]);
                KFFBS = castDistributionFFO[i, 2] + r * (castDistributionFFO[i + 1, 2] - castDistributionFFO[i, 2]);
                KILS = castDistributionFFO[i, 3] + r * (castDistributionFFO[i + 1, 3] - castDistributionFFO[i, 3]);
                KDFS = castDistributionFFO[i, 4] + r * (castDistributionFFO[i + 1, 4] - castDistributionFFO[i, 4]);
            }

            cycle.AddSpell(needsDisplayCalculations, FrB, KFrB);
            cycle.AddSpell(needsDisplayCalculations, FFB, KFFB);
            cycle.AddSpell(needsDisplayCalculations, FFBS, KFFBS);
            cycle.AddSpell(needsDisplayCalculations, ILS, KILS);
            cycle.AddSpell(needsDisplayCalculations, DFS, KDFS);
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
