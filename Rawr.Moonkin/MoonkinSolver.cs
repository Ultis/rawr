using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    // The interface public class to the rest of Rawr.  Provides a single Solve method that runs all the calculations.
    public class MoonkinSolver
    {
        private const int NUM_SPELL_DETAILS = 17;
        // A list of all currently active proc effects.
        public List<ProcEffect> procEffects;
        public static float BaseMana = 18635f;
        public static float OOC_PROC_CHANCE = 0.0583f;
        public static float EUPHORIA_PERCENT = 0.08f;
        public static float DRAGONWRATH_PROC_RATE = 0.11f;
        public static float ECLIPSE_BASE = 0.25f;

        #region Cast Distributions
        
        public static double[,] CastDistribution = new double[21, 8] {
{0.146870915028848,	0.218473879280155,	0.0462401088400018,	0.0370504436628126,	0.188704043057695,	0.265899560803597,	0.0572716505622004,	0.039488500899468,},
{0.14766108032111,	0.219332517323234,	0.0470564031996256,	0.0372856339637232,	0.189340108797777,	0.266674440255254,	0.053336142301307,	0.0393127555718448,},
{0.153151990008103,	0.21990592107663,	0.0408188593468499,	0.0372936766585363,	0.189681386158838,	0.266819803541383,	0.0530874772400197,	0.039240042993109,},
{0.153804697546538,	0.2207099666662,	0.0386799152492424,	0.0372259958975479,	0.190470578217262,	0.271859762965403,	0.0481626919383665,	0.0390856164988852,},
{0.154534018570709,	0.22111777729999,	0.0402291772241524,	0.0375145865283392,	0.191121040551878,	0.272374075475106,	0.0443382610676791,	0.0387703435741695,},
{0.159282838860255,	0.224522352414512,	0.0309892107909201,	0.0373785790758112,	0.191479080306969,	0.272443932888101,	0.0451479727376098,	0.0387553467800751,},
{0.159581387713945,	0.222768723429335,	0.0344920944219636,	0.0375692522292907,	0.191672974816555,	0.275593075728467,	0.0396638494862716,	0.0386579717482491,},
{0.160557528069633,	0.223207888214756,	0.0328852165432135,	0.0375814015759688,	0.191965853235217,	0.27562191099119,	0.0395130571203669,	0.0386664913724333,},
{0.160145175386056,	0.226427571658022,	0.02939845390781,	0.0374613687680708,	0.191887069453371,	0.277118780809628,	0.0389454775884527,	0.0386156315326331,},
{0.164577251456784,	0.226348173523568,	0.0273645773916264,	0.0377342628654714,	0.192588847546987,	0.275506378552168,	0.0375286840372756,	0.0383511860205644,},
{0.16433000932828,	0.227236959168885,	0.025538293068109,	0.0375476335642191,	0.192244898329082,	0.278312305493182,	0.0363488314507479,	0.0384404586526027,},
{0.164942705167348,	0.227664521611107,	0.024460463396622,	0.037604953440743,	0.192763889745136,	0.278980924266597,	0.0352107306991715,	0.038371321089546,},
{0.164975300545384,	0.228130539435868,	0.025748570495172,	0.0377782530716703,	0.193070744667339,	0.28002439427304,	0.0321049196481388,	0.038166856233753,},
{0.167793262887505,	0.229037768875379,	0.0211381842727311,	0.0376222470294895,	0.192981665409106,	0.280471266443393,	0.0326182591097568,	0.0383367579298858,},
{0.168166483127242,	0.228991234398322,	0.0225887760528162,	0.0378682658552541,	0.193558273272773,	0.281202055964355,	0.029606563708362,	0.0380178810332892,},
{0.168701172314815,	0.229378997019732,	0.0211568623684946,	0.0378081322509998,	0.193935757304302,	0.281511208796403,	0.0293900364481919,	0.0381173755644252,},
{0.169568452870639,	0.232498387785699,	0.0155572944697852,	0.0375710435044336,	0.193477146595183,	0.280377526087265,	0.0326966066482553,	0.0382530479914882,},
{0.169771399987173,	0.230882772976225,	0.0187015431346923,	0.0377485481893393,	0.193863253780333,	0.282763719595443,	0.0281936697109578,	0.0380746151750637,},
{0.169909993923591,	0.232416185824445,	0.0161390732962462,	0.0376751506328873,	0.193750676491765,	0.283074743561871,	0.0289220928848875,	0.0381116138487198,},
{0.170317155744802,	0.233172107842085,	0.0146307445962018,	0.0376142943378223,	0.19426319525816,	0.283113980416548,	0.0287275441077092,	0.0381605271058789,},
{0.170754670983033,	0.233231593101096,	0.0153362472170702,	0.0377388967359551,	0.194716410734661,	0.283819218264373,	0.0265464096156782,	0.0378561356649963,},
};

        public static double[,] T12CastDistribution = new double[21, 8] {
{0.131417767336708,	0.199737034383142,	0.0438497284048121,	0.0334541350395419,	0.203175227574246,	0.28505940925912,	0.0608840306765956,	0.042421912265673,},
{0.131291860907573,	0.201802042281276,	0.0443244695807248,	0.0337158959317629,	0.20387955862192,	0.28656114540061,	0.0563863454142234,	0.0420380687852483,},
{0.136733971258007,	0.202604643569961,	0.0366285970430171,	0.0336481565299879,	0.204086021242996,	0.28632824658856,	0.0576734552687276,	0.0422967401183508,},
{0.136738924671549,	0.204295665451648,	0.0344741303038865,	0.0336029788535144,	0.20456786155274,	0.291061581680907,	0.0531433794627315,	0.0421153238421909,},
{0.138331030087742,	0.20461408603263,	0.0358457090947836,	0.0339418464939563,	0.205530358085729,	0.290863503444111,	0.049106758170525,	0.0417665424983787,},
{0.141293057129612,	0.208387175035701,	0.0281413454179658,	0.0338508865984508,	0.205785744125146,	0.292200575444721,	0.0486955921302775,	0.0416454993098231,},
{0.141858331076016,	0.206659472897971,	0.031485124086248,	0.0340651408640957,	0.205996525490413,	0.295330234814136,	0.0431111195052693,	0.0414939160054197,},
{0.141979203445325,	0.207286755890202,	0.0303498597596568,	0.0340703856866646,	0.206130904194779,	0.296191029240452,	0.0425817583213089,	0.0414099820589851,},
{0.141431563919644,	0.210546571130501,	0.0264409376166908,	0.0338913711248064,	0.206112406370442,	0.297862268132159,	0.0422058006789283,	0.0415089893371534,},
{0.145360566341378,	0.210564619473241,	0.0250992392869744,	0.0342027078767661,	0.206867465848259,	0.29627110797917,	0.0404504977447801,	0.0411836898718824,},
{0.145371935016149,	0.211495057338325,	0.0233570069397467,	0.034046086646593,	0.206455841496377,	0.298937457486895,	0.0390154017856766,	0.0413211008645715,},
{0.1456007166117,	0.211008458535989,	0.0231792096049731,	0.0340705702163556,	0.207235440833196,	0.300552194609902,	0.0371893148255708,	0.0411639910890598,},
{0.145891661244521,	0.211839861669812,	0.0242557906571839,	0.0343192177729586,	0.20768912592129,	0.301165833134762,	0.0339108904236012,	0.0409275945363509,},
{0.148465263209604,	0.213136398793856,	0.0190745524813106,	0.0340820244537817,	0.207424251613871,	0.301430617179203,	0.0351341694601658,	0.0412525999699117,},
{0.148758318936064,	0.21322190987188,	0.0206276505882868,	0.0343516600809878,	0.207913984705659,	0.302199340552056,	0.0319545615254239,	0.0409724551875831,},
{0.15009312277774,	0.21324876966446,	0.0186256021852492,	0.0342675512721985,	0.208497996341561,	0.301967805495615,	0.0321123662512584,	0.0411866487851536,},
{0.149337908987019,	0.215902026674528,	0.0156295099272223,	0.0341337245423637,	0.208026266639132,	0.30259345063056,	0.0334718496323392,	0.0409052382778387,},
{0.149732571321586,	0.214394321015648,	0.018417667112219,	0.0342893326368169,	0.208413638643908,	0.305014318437234,	0.0290017782127378,	0.040736346634948,},
{0.15055613523192,	0.21582621126264,	0.0152318276706539,	0.0342144905273114,	0.208578024818691,	0.304206112795635,	0.0305372470282059,	0.0408498389997947,},
{0.150571884065685,	0.216788907677473,	0.0137523115988662,	0.0341453251844189,	0.208861225179235,	0.304948626380684,	0.030105990132846,	0.0408257073098219,},
{0.150892038632357,	0.217047317549536,	0.0145323741563374,	0.0342785168033393,	0.209127849771313,	0.30554859399366,	0.0279137639298043,	0.0406595245563825,},
};

        public static double[] BaseRotationDurations = new double[21] { 56.5739180626449, 53.8210306222131, 51.3600164891103, 49.0344131129915, 46.9841489677816, 44.99334587337, 43.2757521247403, 41.6676660366715, 40.105920214558, 38.7491570429366, 37.4252285850539, 36.271410582689, 35.1873740128298, 34.166559435486, 33.2061971293205, 32.2835279362925, 31.4132301835548, 30.6206034510746, 29.8440694863477, 29.0980087629725, 28.4079560290723, };

        public static double[] T12RotationDurations = new double[21] { 52.64053596476, 50.0582046768898, 47.7453085997329, 45.5945127924301, 43.7003286728987, 41.8322055206568, 40.2416082635732, 38.7387471301205, 37.281510217123, 36.0054439537007, 34.7933789851398, 33.6983359727604, 32.6943425097696, 31.7300804685204, 30.8470559904772, 29.9812135278665, 29.1835275635565, 28.4469839226811, 27.7269078149251, 27.0361579630371, 26.4028932919365, };

        public static string[] CastDistributionSpells = new string[8] { "Starfire", "Wrath", "Starsurge", "Shooting Stars", "Starfire (Eclipse)", "Wrath (Eclipse)", "Starsurge (Eclipse)", "Shooting Stars (Eclipse)" };

        #endregion

        // A list of all the damage spells
        private Spell[] _spellData = null;
        private Spell[] SpellData
        {
            get
            {
                if (_spellData == null)
                {
                    _spellData = new Spell[] {
                        new Spell()
                        {
                            Name = "SF",
                            BaseDamage = (1214f + 1514f) / 2.0f,
                            SpellDamageModifier = 1.0f,
                            BaseCastTime = 3.2f,
                            BaseManaCost = (float)(int)(BaseMana * 0.11f),
                            DotEffect = null,
                            School = SpellSchool.Arcane,
                            BaseEnergy = 20
                        },
                        new Spell()
                        {
                            Name = "MF",
                            BaseDamage = (197.0f + 239.0f) / 2.0f,
                            SpellDamageModifier = 0.18f,
                            BaseCastTime = 1.5f,
                            BaseManaCost = (float)(int)(BaseMana * 0.09f),
                            DotEffect = new DotEffect()
                                {
                                    BaseDuration = 12.0f,
                                    BaseTickLength = 2.0f,
                                    TickDamage = 93.0f,
                                    SpellDamageModifierPerTick = 0.18f
                                },
                            School = SpellSchool.Arcane
                        },
                        new Spell()
                        {
                            Name = "W",
                            BaseDamage = (831f + 937f) / 2.0f,
                            SpellDamageModifier = 2.5f/3.5f,
                            BaseCastTime = 2.5f,
                            BaseManaCost = (float)(int)(BaseMana * 0.09f),
                            DotEffect = null,
                            School = SpellSchool.Nature,
                            BaseEnergy = 40/3f
                        },
                        new Spell()
                        {
                            Name = "IS",
                            BaseDamage = 0.0f,
                            SpellDamageModifier = 0.0f,
                            BaseCastTime = 1.5f,
                            BaseManaCost = (float)(int)(BaseMana * 0.08f),
                            DotEffect = new DotEffect()
                            {
                                BaseDuration = 12.0f,
                                BaseTickLength = 2.0f,
                                TickDamage = 136.0f,
                                SpellDamageModifierPerTick = 0.13f
                            },
                            School = SpellSchool.Nature
                        },
                        new Spell()
                        {
                            Name = "SS",
                            BaseDamage = (1018 + 1404) / 2f,
                            SpellDamageModifier = 1.228f,
                            BaseCastTime = 2.0f,
                            BaseManaCost = (float)(int)(BaseMana * 0.11f),
                            DotEffect = null,
                            School = SpellSchool.Spellstorm,
                            BaseEnergy = 15
                        }
                    };
                }
                return _spellData;
            }
        }
        public Spell Starfire
        {
            get
            {
                return SpellData[0];
            }
        }
        public Spell Moonfire
        {
            get
            {
                return SpellData[1];
            }
        }
        public Spell Wrath
        {
            get
            {
                return SpellData[2];
            }
        }
        public Spell InsectSwarm
        {
            get
            {
                return SpellData[3];
            }
        }
        public Spell Starsurge
        {
            get
            {
                return SpellData[4];
            }
        }
        private void ResetSpellList()
        {
            // Since the property rebuilding the array is based on this variable being null, this effectively forces a refresh
            _spellData = null;
        }

        // The spell rotations themselves.
        private SpellRotation[] rotations = null;
        public SpellRotation[] Rotations
        {
            get
            {
                if (rotations == null)
                {
                    rotations = new SpellRotation[37];
                    RecreateRotations();
                }
                return rotations;
            }
        }

        // Results data from the calculations, which will be sent to the UI.
        RotationData[] cachedResults = new RotationData[36];

        public void Solve(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            DruidTalents talents = character.DruidTalents;
            procEffects = new List<ProcEffect>();
            UpdateSpells(character, ref calcs);

            float trinketDPS = 0.0f;
            float baseSpellPower = calcs.SpellPower;
            float baseHit = 1 - Math.Max(0, calcs.SpellHitCap - calcs.SpellHit);
            float baseCrit = calcs.SpellCrit;
            float baseHaste = calcs.SpellHaste;
            float baseMastery = calcs.Mastery;
            float sub35PercentTime = (float)(character.BossOptions.Under20Perc + character.BossOptions.Under35Perc);

            BuildProcList(calcs);

            float maxDamageDone = 0.0f, maxBurstDamageDone = 0.0f;
            SpellRotation maxBurstRotation = Rotations[0];
            SpellRotation maxRotation = Rotations[0];

            float manaPool = GetEffectiveManaPool(character, calcOpts, calcs);

            float manaGained = manaPool - calcs.BasicStats.Mana;

            float oldArcaneMultiplier = calcs.BasicStats.BonusArcaneDamageMultiplier;
            float oldNatureMultiplier = calcs.BasicStats.BonusNatureDamageMultiplier;

            int rotationIndex = 1;
            foreach (SpellRotation rot in Rotations)
            {
                if (rot.RotationData.Name == "None") continue;
                rot.Solver = this;

                // Reset variables modified in the pre-loop to base values
                float currentSpellPower = baseSpellPower;
                float currentCrit = baseCrit + StatConversion.NPC_LEVEL_SPELL_CRIT_MOD[character.BossOptions.Level - character.Level];
                float currentHaste = baseHaste;
                float currentMastery = baseMastery;
                float currentTrinketDPS = trinketDPS;
                calcs.BasicStats.BonusArcaneDamageMultiplier = oldArcaneMultiplier;
                calcs.BasicStats.BonusNatureDamageMultiplier = oldNatureMultiplier;
                float accumulatedDamage = 0.0f;
                float totalUpTime = 0.0f;
                float[] spellDetails = new float[NUM_SPELL_DETAILS];
                List<ProcEffect> activatedEffects = new List<ProcEffect>();
                List<ProcEffect> alwaysUpEffects = new List<ProcEffect>();

                float baselineDPS = rot.DamageDone(character, calcs, calcOpts.TreantLifespan, currentSpellPower, baseHit, currentCrit, currentHaste, currentMastery, calcOpts.Latency);

                // Calculate spell power/spell damage modifying trinkets in a separate pre-loop
                // Add spell crit effects here as well, since they no longer affect timing
                foreach (ProcEffect proc in procEffects)
                {
                    bool handled = false;
                    if (proc.Effect.Stats.SpellPower > 0 || proc.Effect.Stats.CritRating > 0 || proc.Effect.Stats.MasteryRating > 0)
                    {
                        handled = true;
                        float procSpellPower = proc.Effect.Stats.SpellPower;
                        float procSpellCrit = StatConversion.GetSpellCritFromRating(proc.Effect.Stats.CritRating);
                        float procMastery = StatConversion.GetMasteryFromRating(proc.Effect.Stats.MasteryRating);

                        float triggerInterval = 0.0f, triggerChance = 1.0f;
                        switch (proc.Effect.Trigger)
                        {
                            case Trigger.DamageDone:
                            case Trigger.DamageOrHealingDone:
                                triggerInterval = ((rot.RotationData.Duration / rot.RotationData.CastCount) + (rot.RotationData.Duration / (rot.RotationData.MoonfireTicks + rot.RotationData.InsectSwarmTicks))) / 2.0f;
                                break;
                            case Trigger.Use:
                                break;
                            case Trigger.SpellHit:
                            case Trigger.DamageSpellHit:
                                triggerInterval = rot.RotationData.Duration / rot.RotationData.CastCount;
                                triggerChance = baseHit;
                                break;
                            case Trigger.SpellCrit:
                            case Trigger.DamageSpellCrit:
                                triggerInterval = rot.RotationData.Duration / (rot.RotationData.CastCount - rot.RotationData.InsectSwarmCasts);
                                triggerChance = baseCrit;
                                break;
                            case Trigger.SpellCast:
                            case Trigger.DamageSpellCast:
                                triggerInterval = rot.RotationData.Duration / rot.RotationData.CastCount;
                                break;
                            case Trigger.MoonfireCast:
                                triggerInterval = rot.RotationData.Duration / rot.RotationData.MoonfireCasts;
                                break;
                            case Trigger.DoTTick:
                                triggerInterval = rot.RotationData.Duration / (rot.RotationData.InsectSwarmTicks + rot.RotationData.MoonfireTicks);
                                break;
                            case Trigger.MoonfireTick:
                                triggerInterval = rot.RotationData.Duration / rot.RotationData.MoonfireTicks;
                                break;
                            case Trigger.InsectSwarmTick:
                                triggerInterval = rot.RotationData.Duration / rot.RotationData.InsectSwarmTicks;
                                break;
                            default:
                                triggerChance = 0.0f;
                                break;
                        }
                        if (triggerChance > 0)
                        {
                            float durationMultiplier = proc.Effect.LimitedToExecutePhase ? sub35PercentTime : 1f;
                            currentSpellPower += (proc.Effect.MaxStack > 1 ? proc.Effect.GetAverageStackSize(triggerInterval, triggerChance, 3.0f, character.BossOptions.BerserkTimer * 60.0f * durationMultiplier) : 1) *
                            proc.Effect.GetAverageUptime(triggerInterval, triggerChance, 3.0f, character.BossOptions.BerserkTimer * 60.0f) * procSpellPower * durationMultiplier;
                            currentCrit += (proc.Effect.MaxStack > 1 ? proc.Effect.GetAverageStackSize(triggerInterval, triggerChance, 3.0f, character.BossOptions.BerserkTimer * 60.0f * durationMultiplier) : 1) *
                                proc.Effect.GetAverageUptime(triggerInterval, triggerChance, 3.0f, character.BossOptions.BerserkTimer * 60.0f) * procSpellCrit * durationMultiplier;
                            currentMastery += (proc.Effect.MaxStack > 1 ? proc.Effect.GetAverageStackSize(triggerInterval, triggerChance, 3.0f, character.BossOptions.BerserkTimer * 60.0f * durationMultiplier) : 1) *
                                proc.Effect.GetAverageUptime(triggerInterval, triggerChance, 3.0f, character.BossOptions.BerserkTimer * 60.0f) * procMastery * durationMultiplier;
                        }
                    }
                    // 2T10 (both if statements, which is why it isn't else-if)
                    if (proc.Effect.Stats.BonusArcaneDamageMultiplier > 0)
                    {
                        handled = true;
                        calcs.BasicStats.BonusArcaneDamageMultiplier += proc.Effect.GetAverageUptime(rot.RotationData.Duration / rot.RotationData.CastCount, 1f, 3.0f, character.BossOptions.BerserkTimer * 60.0f) * proc.Effect.Stats.BonusArcaneDamageMultiplier;
                    }
                    if (proc.Effect.Stats.BonusNatureDamageMultiplier > 0)
                    {
                        handled = true;
                        calcs.BasicStats.BonusNatureDamageMultiplier += proc.Effect.GetAverageUptime(rot.RotationData.Duration / rot.RotationData.CastCount, 1f, 3.0f, character.BossOptions.BerserkTimer * 60.0f) * proc.Effect.Stats.BonusNatureDamageMultiplier;
                    }
                    // This area reserved for Dragonwrath, Tarecgosa's Rest
                    // Variable Pulse Lightning Capacitor
                    // This might catch some other effects, I probably need a better way to differentiate
                    if (proc.Effect.Trigger == Trigger.DamageSpellCrit && proc.Effect.Stats.NatureDamage > 0)
                    {
                        float procInterval = rot.RotationData.Duration / (rot.RotationData.CastCount - rot.RotationData.InsectSwarmCasts + rot.RotationData.DotTicks);
                        currentTrinketDPS += proc.Effect.GetAverageProcsPerSecond(procInterval, currentCrit, 3.0f, character.BossOptions.BerserkTimer * 60.0f) * proc.Effect.Stats.NatureDamage;
                    }
                    // Nested special effects
                    if (proc.Effect.Stats._rawSpecialEffectDataSize > 0)
                    {
                        handled = true;
                        SpecialEffect childEffect = proc.Effect.Stats._rawSpecialEffectData[0];
                        // Heart of Ignacious
                        if (childEffect.Stats.SpellPower > 0)
                        {
                            float averageStack = childEffect.GetAverageStackSize(rot.RotationData.Duration / rot.RotationData.CastCount, baseHit, 3.0f, proc.Effect.Duration);
                            currentSpellPower += childEffect.Stats.SpellPower * averageStack * proc.Effect.GetAverageUptime(rot.RotationData.Duration / rot.RotationData.CastCount, baseHit);
                        }
                        // 4T11
                        if (childEffect.Stats.SpellCrit != 0)
                        {
                            float maxStack = proc.Effect.Stats.SpellCrit;
                            float numNegativeStacks = childEffect.GetAverageStackSize(rot.RotationData.Duration / (rot.RotationData.CastCount - rot.RotationData.InsectSwarmCasts), Math.Min(1.0f, baseCrit + maxStack), 3.0f, proc.Effect.Duration);
                            float averageNegativeValue = childEffect.Stats.SpellCrit * numNegativeStacks;
                            float averageCrit = maxStack + averageNegativeValue;
                            currentCrit += averageCrit * proc.Effect.GetAverageUptime(rot.RotationData.Duration / 2f, 1f, 3.0f, character.BossOptions.BerserkTimer * 60.0f);
                        }
                    }
                    if (!handled)
                    {
                        if (proc.CalculateDPS != null)
                        {
                            accumulatedDamage += proc.CalculateDPS(rot, calcs, character.BossOptions.BerserkTimer, currentSpellPower, baseHit, currentCrit, currentHaste) * rot.RotationData.Duration;
                        }
                        if (proc.Activate != null)
                        {
                            float upTime = proc.UpTime(rot, calcs, character.BossOptions.BerserkTimer, (float)(character.BossOptions.Under35Perc + character.BossOptions.Under20Perc));
                            // Procs with 100% uptime should be activated and not put into the combination loop
                            if (upTime == 1)
                            {
                                alwaysUpEffects.Add(proc);
                                proc.Activate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste, ref currentMastery);
                            }
                            // Procs with uptime 0 < x < 100 should be activated
                            else if (upTime > 0)
                                activatedEffects.Add(proc);
                        }
                        if (proc.CalculateMP5 != null)
                        {
                            manaGained += proc.CalculateMP5(rot, calcs, character.BossOptions.BerserkTimer, currentSpellPower, baseHit, currentCrit, currentHaste) / 5.0f * character.BossOptions.BerserkTimer * 60.0f;
                        }
                    }
                }
                // Calculate stat-boosting trinkets, taking into effect interactions with other stat-boosting procs
                int sign = 1;
                float[] cachedDamages = new float[1 << activatedEffects.Count];
                float[] cachedUptimes = new float[1 << activatedEffects.Count];
                float[,] cachedDetails = new float[1 << activatedEffects.Count, NUM_SPELL_DETAILS];
                List<int> calculatedPairs = new List<int>();
                // Iterate over the entire set of trinket combinations (each trinket by itself, 2 at a time, ...)
                for (int i = 1; i <= activatedEffects.Count; ++i)
                {
                    // Create a new combination generator for this "level" of trinket interaction
                    CombinationGenerator gen = new CombinationGenerator(activatedEffects.Count, i);
                    // Iterate over all combinations
                    while (gen.HasNext())
                    {
                        float tempUpTime = 1.0f;
                        int[] vals = gen.GetNext();
                        int pairs = 0;
                        int lengthCounter = 0;
                        // Activate the trinkets, calculate the damage and uptime, then deactivate them
                        foreach (int idx in vals)
                        {
                            pairs |= 1 << idx;
                            ++lengthCounter;
                            activatedEffects[idx].Activate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste, ref currentMastery);
                        }
                        currentCrit = (float)Math.Min(1.0f, currentCrit);
                        float tempDPS = rot.DamageDone(character, calcs, calcOpts.TreantLifespan, currentSpellPower, baseHit, currentCrit, currentHaste, currentMastery, calcOpts.Latency) / rot.RotationData.Duration;
                        spellDetails[0] = rot.RotationData.StarfireAvgHit;
                        spellDetails[1] = rot.RotationData.WrathAvgHit;
                        spellDetails[2] = rot.RotationData.MoonfireAvgHit;
                        spellDetails[3] = rot.RotationData.InsectSwarmAvgHit;
                        spellDetails[4] = rot.RotationData.StarSurgeAvgHit;
                        spellDetails[5] = rot.RotationData.StarfireAvgCast;
                        spellDetails[6] = rot.RotationData.WrathAvgCast;
                        spellDetails[7] = rot.RotationData.MoonfireAvgCast;
                        spellDetails[8] = rot.RotationData.InsectSwarmAvgCast;
                        spellDetails[9] = rot.RotationData.StarSurgeAvgCast;
                        spellDetails[10] = rot.RotationData.AverageInstantCast;
                        spellDetails[11] = rot.RotationData.StarfireAvgEnergy;
                        spellDetails[12] = rot.RotationData.WrathAvgEnergy;
                        spellDetails[13] = rot.RotationData.StarSurgeAvgEnergy;
                        spellDetails[14] = rot.RotationData.TreantDamage;
                        spellDetails[15] = rot.RotationData.StarfallDamage;
                        spellDetails[16] = rot.RotationData.MushroomDamage;
                        foreach (int idx in vals)
                        {
                            tempUpTime *= activatedEffects[idx].UpTime(rot, calcs, character.BossOptions.BerserkTimer, (float)(character.BossOptions.Under35Perc + character.BossOptions.Under20Perc));
                            activatedEffects[idx].Deactivate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste, ref currentMastery);
                        }
                        if (tempUpTime == 0) continue;
                        // Adjust previous probability tables by the current factor
                        // At the end of the algorithm, this ensures that the probability table will contain the individual
                        // probabilities of each effect or set of effects.
                        // These adjustments only need to be made for higher levels of the table, and if the current probability is > 0.
                        if (lengthCounter > 1)
                        {
                            foreach (int subset in calculatedPairs)
                            {
                                // Truly a subset?
                                if ((pairs & subset) != subset)
                                {
                                    continue;
                                }

                                // Calculate the "layer" of the current subset by getting the set bit count.
                                int subsetLength = 0;
                                for (int j = subset; j > 0; ++subsetLength)
                                {
                                    j &= --j;
                                }

                                // Set the sign of the operation: Evenly separated layers are added, oddly separated layers are subtracted
                                int newSign = ((lengthCounter - subsetLength) % 2 == 0) ? 1 : -1;

                                // Adjust by current uptime * sign of operation.
                                cachedUptimes[subset] += newSign * tempUpTime;
                            }
                        }
                        // Cache the results to be calculated later
                        cachedUptimes[pairs] = tempUpTime;
                        cachedDamages[pairs] = tempDPS;
                        for (int idx = 0; idx < NUM_SPELL_DETAILS; ++idx)
                        {
                            cachedDetails[pairs, idx] = spellDetails[idx];
                        }
                        calculatedPairs.Add(pairs);
                        totalUpTime += sign * tempUpTime;
                    }
                    sign = -sign;
                }
                float accumulatedDPS = 0.0f;
                Array.Clear(spellDetails, 0, spellDetails.Length);
                // Apply the above-calculated probabilities to the previously stored damage calculations and add to the result.
                for (int idx = 0; idx < cachedUptimes.Length; ++idx)
                {
                    if (cachedUptimes[idx] == 0) continue;
                    accumulatedDPS += cachedUptimes[idx] * cachedDamages[idx];
                    for (int i = 0; i < NUM_SPELL_DETAILS; ++i)
                    {
                        spellDetails[i] += cachedUptimes[idx] * cachedDetails[idx,i];
                    }
                }
                float damageDone = rot.DamageDone(character, calcs, calcOpts.TreantLifespan, currentSpellPower, baseHit, currentCrit, currentHaste, currentMastery, calcOpts.Latency);
                accumulatedDPS += (1 - totalUpTime) * damageDone / rot.RotationData.Duration;
                spellDetails[0] += (1 - totalUpTime) * rot.RotationData.StarfireAvgHit;
                spellDetails[1] += (1 - totalUpTime) * rot.RotationData.WrathAvgHit;
                spellDetails[2] += (1 - totalUpTime) * rot.RotationData.MoonfireAvgHit;
                spellDetails[3] += (1 - totalUpTime) * rot.RotationData.InsectSwarmAvgHit;
                spellDetails[4] += (1 - totalUpTime) * rot.RotationData.StarSurgeAvgHit;
                spellDetails[5] += (1 - totalUpTime) * rot.RotationData.StarfireAvgCast;
                spellDetails[6] += (1 - totalUpTime) * rot.RotationData.WrathAvgCast;
                spellDetails[7] += (1 - totalUpTime) * rot.RotationData.MoonfireAvgCast;
                spellDetails[8] += (1 - totalUpTime) * rot.RotationData.InsectSwarmAvgCast;
                spellDetails[9] += (1 - totalUpTime) * rot.RotationData.StarSurgeAvgCast;
                spellDetails[10] += (1 - totalUpTime) * rot.RotationData.AverageInstantCast;
                spellDetails[11] += (1 - totalUpTime) * rot.RotationData.StarfireAvgEnergy;
                spellDetails[12] += (1 - totalUpTime) * rot.RotationData.WrathAvgEnergy;
                spellDetails[13] += (1 - totalUpTime) * rot.RotationData.StarSurgeAvgEnergy;
                spellDetails[14] += (1 - totalUpTime) * rot.RotationData.TreantDamage;
                spellDetails[15] += (1 - totalUpTime) * rot.RotationData.StarfallDamage;
                spellDetails[16] += (1 - totalUpTime) * rot.RotationData.MushroomDamage;

                float burstDPS = accumulatedDPS + accumulatedDamage / rot.RotationData.Duration;
                float sustainedDPS = burstDPS;

                // Mana calcs:
                // Main rotation - all spells
                // Movement rotation - Lunar Shower MF, IS, Shooting Stars procs, and Starfall only
                rot.RotationData.ManaGained += manaGained / (character.BossOptions.BerserkTimer * 60.0f) * rot.RotationData.Duration;
                float timeToOOM = manaPool / ((rot.RotationData.ManaUsed - rot.RotationData.ManaGained) / rot.RotationData.Duration);
                if (timeToOOM <= 0) timeToOOM = character.BossOptions.BerserkTimer * 60.0f;   // Happens when ManaUsed is less than 0
                if (timeToOOM < character.BossOptions.BerserkTimer * 60.0f)
                {
                    rot.RotationData.TimeToOOM = new TimeSpan(0, (int)(timeToOOM / 60), (int)(timeToOOM % 60));
                    sustainedDPS = burstDPS * timeToOOM / (character.BossOptions.BerserkTimer * 60.0f);
                }
                
                burstDPS += currentTrinketDPS;
                sustainedDPS += currentTrinketDPS;

                rot.RotationData.SustainedDPS = sustainedDPS;
                rot.RotationData.BurstDPS = burstDPS;
                rot.RotationData.StarfireAvgHit = spellDetails[0];
                rot.RotationData.WrathAvgHit = spellDetails[1];
                rot.RotationData.MoonfireAvgHit = spellDetails[2];
                rot.RotationData.InsectSwarmAvgHit = spellDetails[3];
                rot.RotationData.StarSurgeAvgHit = spellDetails[4];
                rot.RotationData.StarfireAvgCast = spellDetails[5];
                rot.RotationData.WrathAvgCast = spellDetails[6];
                rot.RotationData.MoonfireAvgCast = spellDetails[7];
                rot.RotationData.InsectSwarmAvgCast = spellDetails[8];
                rot.RotationData.StarSurgeAvgCast = spellDetails[9];
                rot.RotationData.AverageInstantCast = spellDetails[10];
                rot.RotationData.StarfireAvgEnergy = spellDetails[11];
                rot.RotationData.WrathAvgEnergy = spellDetails[12];
                rot.RotationData.StarSurgeAvgEnergy = spellDetails[13];
                rot.RotationData.TreantDamage = spellDetails[14];
                rot.RotationData.StarfallDamage = spellDetails[15];
                rot.RotationData.MushroomDamage = spellDetails[16];

                // Update the sustained DPS rotation if any one of the following three cases is true:
                // 1) No user rotation is selected and sustained DPS is maximum
                // 2) A user rotation is selected, Eclipse is not present, and the user rotation matches the current rotation
                // 3) A user rotation is selected, Eclipse is present, and the user rotation's dot spells matches this rotation's
                if ((calcOpts.UserRotation == "None" && sustainedDPS > maxDamageDone) || rot.RotationData.Name == calcOpts.UserRotation)
                {
                    maxDamageDone = sustainedDPS;
                    maxRotation = rot;
                }
                if (burstDPS > maxBurstDamageDone)
                {
                    maxBurstDamageDone = burstDPS;
                    maxBurstRotation = rot;
                }
                cachedResults[rotationIndex - 1] = rot.RotationData;

                // Deactivate always-up procs
                foreach (ProcEffect proc in alwaysUpEffects)
                {
                    proc.Deactivate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste, ref currentMastery);
                }

                ++rotationIndex;
            }
            // Present the findings to the user.
            calcs.SelectedRotation = maxRotation.RotationData;
            calcs.BurstRotation = maxBurstRotation.RotationData;
            calcs.SubPoints = new float[] { maxBurstDamageDone, maxDamageDone };
            calcs.OverallPoints = calcs.SubPoints[0] + calcs.SubPoints[1];
            calcs.Rotations = cachedResults;
        }

        // Create proc effect calculations for proc-based trinkets.
        private void BuildProcList(CharacterCalculationsMoonkin calcs)
        {
            // Implement a new handler for each special effect in the calculations stats
            foreach (SpecialEffect effect in calcs.BasicStats.SpecialEffects())
            {
                procEffects.Add(new ProcEffect(effect));
            }
        }

        // Non-rotation-specific mana calculations
        private float GetEffectiveManaPool(Character character, CalculationOptionsMoonkin calcOpts, CharacterCalculationsMoonkin calcs)
        {
            float fightLength = character.BossOptions.BerserkTimer * 60.0f;

            float innervateCooldown = 180;

            // Mana/5 calculations
            float totalManaRegen = calcs.ManaRegen * fightLength;

            // Mana pot calculations
            float manaRestoredByPots = 0.0f;
            foreach (Buff b in character.ActiveBuffs)
            {
                if (b.Stats.ManaRestore > 0)
                {
                    manaRestoredByPots = b.Stats.ManaRestore;
                    break;
                }
            }

            // Innervate calculations
            float innervateDelay = calcOpts.InnervateDelay * 60.0f;
            int numInnervates = (calcOpts.Innervate && fightLength - innervateDelay > 0) ? ((int)(fightLength - innervateDelay) / (int)innervateCooldown + 1) : 0;
            float totalInnervateMana = numInnervates * 0.2f * calcs.BasicStats.Mana;
            totalInnervateMana *= 1 + 0.15f * character.DruidTalents.Dreamstate;

            // Replenishment calculations
            float replenishmentPerTick = calcs.BasicStats.Mana * calcs.BasicStats.ManaRestoreFromMaxManaPerSecond;
            float replenishmentMana = calcOpts.ReplenishmentUptime * replenishmentPerTick * character.BossOptions.BerserkTimer * 60;

            return calcs.BasicStats.Mana + totalInnervateMana + totalManaRegen + manaRestoredByPots + replenishmentMana;
        }

        private void RecreateRotations()
        {
            rotations[0] = new SpellRotation() { RotationData = new RotationData() { Name = "None" } };
            for (int mfMode = 0; mfMode < 2; ++mfMode)
            {
                for (int isMode = 0; isMode < 2; ++isMode)
                {
                    for (int sfMode = 0; sfMode < 3; ++sfMode)
                    {
                        for (int wmMode = 0; wmMode < 3; ++wmMode)
                        {
                            int index = 1 + (wmMode + 3 * sfMode + 9 * isMode + 18 * mfMode);
                            DotMode mfModeEnum = (DotMode)mfMode;
                            DotMode isModeEnum = (DotMode)isMode;
                            StarfallMode sfModeEnum = (StarfallMode)sfMode;
                            MushroomMode wmModeEnum = (MushroomMode)wmMode;
                            string name = String.Format("MF {0} IS {1} SF {2} WM {3}",
                                mfModeEnum.ToString(),
                                isModeEnum.ToString(),
                                sfModeEnum.ToString(),
                                wmModeEnum.ToString());
                            rotations[index] = new SpellRotation()
                            {
                                RotationData = new RotationData()
                                {
                                    Name = name,
                                    MoonfireRefreshMode = mfModeEnum,
                                    InsectSwarmRefreshMode = isModeEnum,
                                    StarfallCastMode = sfModeEnum,
                                    WildMushroomCastMode = wmModeEnum
                                }
                            };
                        }
                    }
                }
            }
        }

        // Add talented effects to the spells
        private void UpdateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            DruidTalents talents = character.DruidTalents;
            StatsMoonkin stats = calcs.BasicStats;

            switch (talents.StarlightWrath)
            {
                case 1:
                    Starfire.BaseCastTime -= 0.15f;
                    Wrath.BaseCastTime -= 0.15f;
                    break;
                case 2:
                    Starfire.BaseCastTime -= 0.25f;
                    Wrath.BaseCastTime -= 0.25f;
                    break;
                case 3:
                    Starfire.BaseCastTime -= 0.5f;
                    Wrath.BaseCastTime -= 0.5f;
                    break;
                default:
                    break;
            }

            float moonfireDotGlyph = talents.GlyphOfMoonfire ? 0.2f : 0.0f;
            float insectSwarmGlyph = talents.GlyphOfInsectSwarm ? 0.3f : 0.0f;
            // Add spell-specific damage
            // Moonfire, Insect Swarm: glyphs
            Moonfire.DotEffect.AllDamageModifier *= 1 + moonfireDotGlyph;
            InsectSwarm.DotEffect.AllDamageModifier *= 1 + insectSwarmGlyph;
            // Moonfire: Direct damage +(0.03 * Blessing of the Grove)
            Moonfire.AllDamageModifier *= 1 + 0.03f * talents.BlessingOfTheGrove;
            // Moonfire, Insect Swarm: +2/4/6 seconds for Genesis
            Moonfire.DotEffect.BaseDuration += 2f * talents.Genesis;
            InsectSwarm.DotEffect.BaseDuration += 2f * talents.Genesis;
            // Wrath: 10% for glyph
            Wrath.AllDamageModifier *= 1 + (talents.GlyphOfWrath ? 0.1f : 0f);

            // Add spell-specific critical strike damage
            // Burning Shadowspirit Diamond
            float baseCritMultiplier = 1.5f * (1 + stats.BonusCritDamageMultiplier);
            float moonfuryMultiplier = baseCritMultiplier + (baseCritMultiplier - 1);
            Starfire.CriticalDamageModifier = Wrath.CriticalDamageModifier = Moonfire.CriticalDamageModifier = InsectSwarm.CriticalDamageModifier = moonfuryMultiplier;
            Starsurge.CriticalDamageModifier = moonfuryMultiplier;

            // Reduce spell-specific mana costs
            // Shard of Woe (Mana cost -405)
            Starfire.BaseManaCost -= calcs.BasicStats.SpellsManaCostReduction;
            Moonfire.BaseManaCost -= calcs.BasicStats.SpellsManaCostReduction;
            Wrath.BaseManaCost -= calcs.BasicStats.SpellsManaCostReduction + calcs.BasicStats.NatureSpellsManaCostReduction;
            InsectSwarm.BaseManaCost -= calcs.BasicStats.SpellsManaCostReduction + calcs.BasicStats.NatureSpellsManaCostReduction;
            Starsurge.BaseManaCost -= calcs.BasicStats.SpellsManaCostReduction;
            // All spells: Mana cost -(0.03 * Moonglow)
            Starfire.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Moonfire.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Wrath.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            InsectSwarm.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Starsurge.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);

            // Add set bonuses
            Moonfire.CriticalChanceModifier += stats.BonusCritChanceMoonfire;
            InsectSwarm.CriticalChanceModifier += stats.BonusCritChanceInsectSwarm;

            // Dragonwrath, Tarecgosa's Rest: X% chance on damaging spell cast to proc a duplicate version of the spell.
            // If it duplicates a DoT tick, it fires Wrath of Tarecgosa for an equivalent amount of damage.
            // Wrath, Starfire, and Starsurge will duplicate the Eclipse energy gained.
            if (calcs.BasicStats.DragonwrathProc > 0)
            {
                Starfire.AllDamageModifier += MoonkinSolver.DRAGONWRATH_PROC_RATE;
                Wrath.AllDamageModifier += MoonkinSolver.DRAGONWRATH_PROC_RATE;
                Starsurge.AllDamageModifier += MoonkinSolver.DRAGONWRATH_PROC_RATE;
                Moonfire.AllDamageModifier += MoonkinSolver.DRAGONWRATH_PROC_RATE;
                Moonfire.DotEffect.AllDamageModifier += MoonkinSolver.DRAGONWRATH_PROC_RATE;
                InsectSwarm.DotEffect.AllDamageModifier += MoonkinSolver.DRAGONWRATH_PROC_RATE;
            }

            // PTR changes go here
            if (((CalculationOptionsMoonkin)character.CalculationOptions).PTRMode)
            {
            }
        }
    }
}
