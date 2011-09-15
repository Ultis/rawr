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
{ 0.147972547827501, 0.21964030823292, 0.0471277942618362, 0.0365214175344861, 0.189724364711169, 0.267105153877004, 0.0534875410294136, 0.0384200532598154, },
{ 0.153383573434764, 0.220205964181827, 0.0408963175831069, 0.0366507841329187, 0.190066703738765, 0.267260044507214, 0.0531674420998709, 0.0383684082279934, },
{ 0.15394178211712, 0.221149187452673, 0.0388454641242777, 0.0366076462547075, 0.190354941496311, 0.271349712577298, 0.0493764297588739, 0.0383741106906023, },
{ 0.154197317545749, 0.221099535812449, 0.0414877992202946, 0.0368833936193381, 0.191197395958511, 0.272705020516954, 0.0444360273007098, 0.0379928854273749, },
{ 0.155513711168498, 0.221558470455288, 0.0396575665910419, 0.0369219689106836, 0.191741255359368, 0.273198708626145, 0.0433526773808967, 0.0380551365125762, },
{ 0.160088561191856, 0.222068067554852, 0.03339234671619, 0.0369259868554051, 0.192062600330414, 0.275495599972689, 0.041886832110365, 0.0380793335556814, },
{ 0.16078177785651, 0.223429831668864, 0.0329132791421944, 0.0370624882745512, 0.192249007704817, 0.275941705863852, 0.0395935012770806, 0.0380277989042521, },
{ 0.160355209996209, 0.226599787503615, 0.0294358986450322, 0.0370030483193831, 0.192184950092727, 0.27742386905424, 0.0389919390495083, 0.0380048545782623, },
{ 0.16477453357262, 0.226528882308364, 0.0273951007122285, 0.0373034001571801, 0.192823552525402, 0.275831668198111, 0.0375739274876915, 0.0377683344948853, },
{ 0.164811775613307, 0.225225216564663, 0.0285337862905437, 0.0372934316934983, 0.192944545039392, 0.278347866700834, 0.0351056789914011, 0.0377372109559531, },
{ 0.16452294100256, 0.227743961770316, 0.0247631546606655, 0.0371691884027013, 0.192707165092783, 0.279580470528305, 0.0356452838525187, 0.0378673845276752, },
{ 0.165174495108237, 0.228037102214199, 0.0256713662234801, 0.0373328651871732, 0.193078295106086, 0.280170592858398, 0.0327970519013161, 0.037737815902729, },
{ 0.168019669628017, 0.228754822659194, 0.0220044247030498, 0.0373659334210236, 0.193522403268996, 0.278341765008663, 0.0342372881915699, 0.0377532106930406, },
{ 0.168020048262701, 0.229130717778369, 0.0209342140544335, 0.0373062252885696, 0.193410386280601, 0.280945682021529, 0.032462615512292, 0.0377896385158185, },
{ 0.168592756639369, 0.2289677683221, 0.0225618354801386, 0.0374991319226597, 0.194054118694916, 0.282122209149572, 0.0286206138723258, 0.0375811362522947, },
{ 0.168665819494342, 0.231494390925341, 0.0184850714880919, 0.0373710894671475, 0.193784208629266, 0.282032239753201, 0.0303901134904416, 0.0377766325603543, },
{ 0.169927584602455, 0.230987816870709, 0.0187043720133462, 0.037455874915825, 0.194044406272916, 0.282979850652004, 0.0282771453241495, 0.0376224976017779, },
{ 0.170434746692412, 0.231004842585928, 0.0179035260028586, 0.0374406436316484, 0.194409416084807, 0.283363954392048, 0.0277413241011018, 0.0377010863728051, },
{ 0.170447493143391, 0.232997160853456, 0.0150123084371002, 0.0373643719023339, 0.194322457161072, 0.283338866507193, 0.028809051007882, 0.0377078619853355, },
{ 0.170693548422362, 0.233265618727916, 0.0156964601583541, 0.0374755809561798, 0.194672608597671, 0.283818455386464, 0.0269011767216554, 0.0374761568279721, },
{ 0.170884404055612, 0.23269466298796, 0.0157352393612175, 0.0374457519042864, 0.194678192907212, 0.285375265849019, 0.0256091378515697, 0.0375769391539339, },
};

        public static double[,] T12CastDistribution = new double[21, 8] {
{ 0.131360708204151, 0.202206960560984, 0.0443502040286031, 0.0330105821796299, 0.204308035894447, 0.287115665260536, 0.0565365683081252, 0.0411107551919473, },
{ 0.136796299361651, 0.202978885444207, 0.0366615601073297, 0.0330790118010881, 0.204492355850545, 0.286850509315792, 0.0578160177664852, 0.0413252126349974, },
{ 0.136332968729295, 0.205151122953059, 0.0345085762462012, 0.0330243153981219, 0.204545596411637, 0.290797615236494, 0.0544304167342263, 0.0412092607799544, },
{ 0.136876882913975, 0.204981153567644, 0.0374607085613747, 0.0333710588908561, 0.205219848133578, 0.292152151115799, 0.0489615902601676, 0.0409764812357424, },
{ 0.138667907151598, 0.205625669825198, 0.0352152721363608, 0.0334150660528786, 0.205882947873219, 0.291883277175363, 0.0483529655256682, 0.0409567764947764, },
{ 0.142352548916959, 0.205707947438881, 0.0306070589775495, 0.0334689748260268, 0.206472902116663, 0.295282578706084, 0.0452212922400547, 0.040886578453838, },
{ 0.142128110033529, 0.207539414903175, 0.0304078081536245, 0.0335901066754861, 0.206425452303909, 0.29654048062197, 0.0426169345466137, 0.0407515859318389, },
{ 0.141561623104624, 0.210755113887993, 0.0265053321728384, 0.0334874856948391, 0.206427951146807, 0.298195712837822, 0.0422163610056273, 0.040850394431242, },
{ 0.145532628587289, 0.210740768769626, 0.0251648343486998, 0.0338226490534999, 0.207120172959164, 0.296610345448287, 0.0404462746984684, 0.0405622326336333, },
{ 0.145812097836374, 0.208720539340434, 0.0272949579723617, 0.0338623842446354, 0.207453627404957, 0.299568679711162, 0.0367898027136725, 0.0404978219526899, },
{ 0.145648323060127, 0.211308720642897, 0.023574313087215, 0.0337435475096945, 0.207152397752604, 0.300307490489021, 0.0376387062604629, 0.040626399317125, },
{ 0.145995088071707, 0.211883665471076, 0.0244905176188703, 0.0339424314310878, 0.207694588462452, 0.301360910832368, 0.0341941442046541, 0.0404386313771882, },
{ 0.148955251416148, 0.212739822188719, 0.0198655717694575, 0.0338807413058098, 0.208107267509304, 0.298972000241404, 0.0368832257759555, 0.0405960123947106, },
{ 0.148658595160073, 0.213474931848763, 0.0188055219844972, 0.0338155911982621, 0.207850036129407, 0.301741066139717, 0.0349981041630528, 0.0406560472603043, },
{ 0.149225365097322, 0.212793969132282, 0.0209870691448656, 0.0340395168224352, 0.208504953080233, 0.303496360572059, 0.0304692473652271, 0.040483413051528, },
{ 0.149463729887622, 0.216041594524873, 0.0156322527352055, 0.0338593922656224, 0.208176242113801, 0.302567730628502, 0.0335689104835377, 0.0406900393025507, },
{ 0.149873414052799, 0.21451678770166, 0.0184152061587109, 0.0340269257209738, 0.208589787441217, 0.305234417647402, 0.0290426483269094, 0.0403007896832088, },
{ 0.150873965233453, 0.215343896088077, 0.0159035342102702, 0.0339699562545958, 0.20883843385003, 0.30434265898983, 0.030241958254157, 0.040485494041663, },
{ 0.150940204214511, 0.21603138871525, 0.0146641816655525, 0.0339399691931871, 0.209235834978917, 0.305094547922825, 0.0297751194559566, 0.0403187326536771, },
{ 0.150822606435416, 0.216998432759052, 0.0149621726459412, 0.0340519454915029, 0.209091272042001, 0.305682469339689, 0.028101490974537, 0.0402895901790646, },
{ 0.151670272521422, 0.216055134081458, 0.0148518201585697, 0.034038556288055, 0.209423868294565, 0.306645582119686, 0.026949186646592, 0.0403655541212052, },
};

        public static double[] BaseRotationDurations = new double[21] { 55.3465526921378, 52.7732509404359, 50.3551028595971, 48.167168746111, 46.1703439619733, 44.3445234114005, 42.6428503630685, 41.0259546847715, 39.618290111547, 38.266255956289, 36.9698187567448, 35.8480740314186, 34.7939731625764, 33.8045271001925, 32.872250031962, 31.9698735063964, 31.1349877380572, 30.328301272985, 29.5691361098869, 28.8611930505838, 28.1879276832318, };

        public static double[] T12RotationDurations = new double[21] { 51.469666215483, 49.0556696732626, 46.806472852096, 44.7901482777298, 42.9488863265477, 41.23233083726, 39.6466182907634, 38.1387919410011, 36.8139909730218, 35.5687966231073, 34.3689523216068, 33.3085755449296, 32.3179474730659, 31.4033718025863, 30.5307316965001, 29.6872230428616, 28.9265351513266, 28.1804216594053, 27.4713141801098, 26.8221294792511, 26.1930624266338, };

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
