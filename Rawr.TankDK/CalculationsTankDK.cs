using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;
using Rawr.DK;

namespace Rawr.TankDK
{
    public struct TankDKChar
    {
        public Character Char;
        public CalculationOptionsTankDK calcOpts;
        public BossOptions bo;
        //public CombatTable ct;
        //public Rotation Rot;
    }

    public enum SurvivalSub : int { Physical, Bleed, Magic }
    public enum MitigationSub : int 
    { 
        Crit, Haste, Avoidance,  // Damage Avoided
        DamageReduction, Magic, AMS, Armor, Impedences, // Damage Reduced.
        Heals, // Damage Removed.
    }

    [Rawr.Calculations.RawrModelInfo("TankDK", "spell_deathknight_darkconviction", CharacterClass.DeathKnight)]
    public class CalculationsTankDK : CalculationsBase
    {
        #region Gems
        enum GemQuality
        {
            Uncommon,
            Rare,
            Epic,
            Jewelcraft
        }

        private static void fixArray(int[] thearray)
        {
            if (thearray[0] == 0) return; // Nothing to do, they are all 0
            if (thearray[1] == 0) thearray[1] = thearray[0]; // There was a Green, but no Blue
            if (thearray[2] == 0) thearray[2] = thearray[1]; // There was a Blue (or Green as set above), but no Purple
            if (thearray[3] == 0) thearray[3] = thearray[2]; // There was a Purple (or Blue/Green as set above), but no Jewel
        }
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs for TankDKs
                //Red
                //                    UC     Rare   Epic   JC
                int[] flashing = { 52083, 52216, 0, 52259 }; // +parry 
                fixArray(flashing);

                //Purple
                int[] defenders = { 52097, 52210, 0, 0}; // parry+Stam
                fixArray(defenders);
                int[] retaliating = { 52103, 52234, 0, 0 }; // parry+hit
                fixArray(retaliating);

                //Blue
                int[] solid = { 52086, 52242, 0, 52261 }; // +Stam
                fixArray(solid);

                //Green
                int[] puissant = { 52126, 52231, 0, 0}; // +mast +Stam
                fixArray(puissant);
                int[] regal = { 52119, 52233, 0, 0}; // +dodge, Stam
                fixArray(regal);
                int[] nimble = { 52120, 52227, 0, 0 }; // +dodge, hit
                fixArray(nimble);

                //Yellow
                int[] fractured = { 52094, 52219, 0, 52269 }; // +Mastery
                fixArray(fractured);
                int[] subtle = { 52090, 52247, 0, 52265 }; // +Dodge
                fixArray(subtle);

                //Orange
                int[] fine = { 52116, 52215, 0, 0 }; // +parry +mast
                fixArray(fine);
                int[] polished = { 52106, 52229, 0, 0 }; // +agi dodge
                fixArray(polished);
                int[] resolute = { 52107, 52249, 0, 0 }; // +exp dodge
                fixArray(resolute);

                //Meta
                int austere = 52294;
                int effulgent = 52295;
                int fleet = 52289;

                // Prismatic:
//                int nightmare = 49110;

                // Cogwheels
                int[] cog_exp = { 59489, 59489, 59489, 59489 }; fixArray(cog_exp);
                int[] cog_hit = { 59493, 59493, 59493, 59493 }; fixArray(cog_hit);
                int[] cog_mst = { 59480, 59480, 59480, 59480 }; fixArray(cog_mst);
                int[] cog_crt = { 59478, 59478, 59478, 59478 }; fixArray(cog_crt);
                int[] cog_has = { 59479, 59479, 59479, 59479 }; fixArray(cog_has);
                int[] cog_pry = { 59491, 59491, 59491, 59491 }; fixArray(cog_pry);
                int[] cog_ddg = { 59477, 59477, 59477, 59477 }; fixArray(cog_ddg);
                int[] cog_spr = { 59496, 59496, 59496, 59496 }; fixArray(cog_spr);

                return new List<GemmingTemplate>() {
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Avoidance Stam
                        RedId = flashing[0], YellowId = subtle[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = austere, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Mastery 
                        RedId = fine[0], YellowId = fractured[0], BlueId = puissant[0], PrismaticId = fractured[0], MetaId = fleet, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Dodge
                        RedId = polished[0], YellowId = subtle[0], BlueId = regal[0], PrismaticId = subtle[0], MetaId = austere, HydraulicId = 0  },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Parry
                        RedId = flashing[0], YellowId = fine[0], BlueId = defenders[0], PrismaticId = flashing[0], MetaId = austere, HydraulicId = 0  },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Stamina
                        RedId = defenders[0], YellowId = puissant[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = effulgent, HydraulicId = 0  },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = false, // Max Stamina
                        RedId = solid[0], YellowId = solid[0], BlueId = solid[0], PrismaticId = solid[0], MetaId = austere, HydraulicId = 0  },
                        
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Avoidance Stam
                        RedId = flashing[1], YellowId = subtle[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = austere, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, // Mastery 
                        RedId = flashing[1], YellowId = fractured[1], BlueId = puissant[1], PrismaticId = fractured[1], MetaId = fleet, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, // Dodge
                        RedId = defenders[1], YellowId = subtle[1], BlueId = regal[1], PrismaticId = subtle[1], MetaId = austere, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Uncommon", Enabled = true, // Parry
                        RedId = flashing[1], YellowId = fine[1], BlueId = defenders[1], PrismaticId = flashing[1], MetaId = austere, HydraulicId = 0  },
                    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = true, // Stamina
                        RedId = defenders[1], YellowId = puissant[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = effulgent, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Rare", Enabled = false, // Max Stamina
                        RedId = solid[1], YellowId = solid[1], BlueId = solid[1], PrismaticId = solid[1], MetaId = austere, HydraulicId = 0 },

/*                    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Defense 
                        RedId = stalwart[2], YellowId = thick[2], BlueId = enduring[2], PrismaticId = thick[2], MetaId = austere },
                    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Dodge
                        RedId = subtle[2], YellowId = stalwart[2], BlueId = regal[2], PrismaticId = subtle[2], MetaId = austere },
                    new GemmingTemplate() { Model = "TankDK", Group = "Epic", Enabled = true, //Max Stamina
                        RedId = solid[2], YellowId = solid[2], BlueId = solid[2], PrismaticId = solid[2], MetaId = austere },
                    new GemmingTemplate() { Model = "TankDK", Group = "Epic",Enabled = true,  //Stamina
                        RedId = regal[2], YellowId = enduring[2], BlueId = solid[2], PrismaticId = nightmare, MetaId = austere },
*/
                    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Max Mastery
                        RedId = fractured[3], YellowId = fractured[3], BlueId = fractured[3], PrismaticId = fractured[3], MetaId = fleet, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Max Dodge
                        RedId = subtle[3], YellowId = subtle[3], BlueId = subtle[3], PrismaticId = subtle[3], MetaId = austere, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Max parry
                        RedId = flashing[3], YellowId = flashing[3], BlueId = flashing[3], PrismaticId = flashing[3], MetaId = austere, HydraulicId = 0 },
                    new GemmingTemplate() { Model = "TankDK", Group = "Jeweler", //Max Stamina
                        RedId = solid[3], YellowId = solid[3], BlueId = solid[3], PrismaticId = solid[3], MetaId = effulgent, HydraulicId = 0 },

                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_hit[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_mst[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_crt[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_pry[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },

                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_mst[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_crt[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_pry[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },

                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_crt[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_pry[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },
                            
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_has[0], MetaId = austere, },
                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_ddg[0], MetaId = austere, },

                            new GemmingTemplate() { Model = "TankDK", Group = "Cogwheels", Enabled = false, CogwheelId = cog_ddg[0], Cogwheel2Id = cog_has[0], MetaId = austere, },

 
                };
            }
        }
        #endregion

        public static int HitResultCount = EnumHelper.GetCount(typeof(HitResult));



        private string[] _characterDisplayCalculationLabels = null;
        /// <summary>
        /// An array of strings which will be used to build the calculation display.
        /// Each string must be in the format of "Heading:Label". Heading will be used as the
        /// text of the group box containing all labels that have the same Heading.
        /// Label will be the label of that calculation, and may be appended with '*' followed by
        /// a description of that calculation which will be displayed in a tooltip for that label.
        /// Label (without the tooltip string) must be unique.
        /// 
        /// EXAMPLE:
        /// characterDisplayCalculationLabels = new string[]
        /// {
        ///		"Basic Stats:Health",
        ///		"Basic Stats:Armor",
        ///		"Advanced Stats:Dodge",
        ///		"Advanced Stats:Miss*Chance to be missed"
        /// };
        /// </summary>
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                {
                    List<string> labels = new List<string>(new string[] {
                        @"Summary:Survival Points*Survival Points represents the total raw damage 
(pre-Mitigation) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers, but 
keeping it roughly even with Mitigation Points is a good 
way to maintain 'enough' as you progress. If you find that 
you are being killed by burst damage, focus on Survival Points.",
                        @"Summary:Mitigation Points*Mitigation Points represent the amount of damage you avoid, 
on average per second, through avoidance stats (Miss, Dodge, Parry) along 
with ways to improve survivablity, +heal or self healing, ability 
cooldowns.  It is directly relative to your Damage Taken. 
Ideally, you want to maximize Mitigation Points, while maintaining 
'enough' Survival Points (see Survival Points). If you find 
yourself dying due to healers running OOM, or being too busy 
healing you and letting other raid members die, then focus on 
Mitigation Points.  Represented in Damage per Second multiplied
by MitigationWeight (seconds).  Note: Subvalues do NOT represent
all mitigation sources, just common ones.",
                        @"Summary:Burst Points*Burst is a new idea that represents the 
mitigation of Burst damage that can be taken during a given 
encounter.  Specifically, this is focused around Death Strike
heals, Blood Shield and all On-Use Cooldowns.",
                        @"Summary:Threat Points*Threat Points represent how much threat is capable for the current 
gear/talent/rotation setup.  Threat points are represented in Threat per second and assume Vengeance is at maximum.",
                        @"Summary:Overall Points*Overall Points are a sum of Mitigation, Survival and Threat Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation or Survival 
Points individually may be important.",

                        "Basic Stats:Strength",
                        "Basic Stats:Agility",
                        "Basic Stats:Stamina",
                        "Basic Stats:Mastery",
                        "Basic Stats:Attack Power",
                        "Basic Stats:Crit Rating",
                        "Basic Stats:Hit Rating",
                        "Basic Stats:Expertise",
                        "Basic Stats:Haste Rating",
                        "Basic Stats:Health*Including Blood Presence",
                        "Basic Stats:Armor*Including Blood Presence",
                        "Basic Stats:Resilience",

                        "Advanced Stats:Miss*After Diminishing Returns",
                        "Advanced Stats:Dodge*After Diminishing Returns",
                        "Advanced Stats:Parry*After Diminishing Returns",
                        "Advanced Stats:Total Avoidance*Miss + Dodge + Parry",
                        "Advanced Stats:Armor Damage Reduction",
                        "Advanced Stats:Magic Damage Reduction*Currently Magic Resistance Only.",
                        "Advanced Stats:Reaction Time*The time healers have to react to a particularly high damage burst before the next potential burst.",
                        "Advanced Stats:Burst Time*Enhanced time-to-live calculation that factors avoidance and survival.",

                        "Threat Stats:Target Miss*Chance to miss the target",
                        "Threat Stats:Target Dodge*Chance the target dodges",
                        "Threat Stats:Target Parry*Chance the target parries",
                        "Threat Stats:Total Threat*Raw Total Threat Generated by the specified rotation",

                        "Damage Data:DPS*DPS done for given rotation",
                        "Damage Data:Rotation Time*Duration of the total rotation cycle",
                        "Damage Data:Blood*Number of Runes consumed",
                        "Damage Data:Frost*Number of Runes consumed",
                        "Damage Data:Unholy*Number of Runes consumed",
                        "Damage Data:Death*Number of Runes consumed",
                        "Damage Data:Runic Power*Amount of Runic Power used by rotation.\nNegative values mean more RP generated than consumed.",
                        "Damage Data:RE Runes*Number of Runes Generated by Runic Empowerment.",

                        "Combat Data:DTPS*Damage Taken Per Sec averaged over the whole fight.",
                        "Combat Data:HPS*Heals Per Sec from Death Strike",
                        "Combat Data:DPS Avoided*Physical incoming DPS reduced only by Miss, Dodge, Parry.",
                        "Combat Data:DPS Reduced By Armor*Physical incoming DPS reduced only by armor.",
                        "Combat Data:Death Strike*Total healing from Death Strikes only assuming full Berserk Timer for current boss.",
                        "Combat Data:Blood Shield*Total Absorbs from Blood Shield assuming full Berserk Timer for current boss.",

                    });
                    _characterDisplayCalculationLabels = labels.ToArray();
                }
                return _characterDisplayCalculationLabels;
            }
        }

        /// <summary>
        /// An array of strings which define what calculations (in addition to the subpoint ratings)
        /// will be available to the optimizer
        /// </summary>
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                return new string[] {
                    "Chance to be Crit",
                    "Avoidance %",
                    "Damage Reduction %",
                    "% Chance to Hit",
                    "Target Parry %",
                    "Target Dodge %",
                    "Armor",
                    "Health",
                    "Hit Rating",
                    "Reaction Time",
                    "Burst Time",
                    "Resilience",
                    "Spell Penetration",
                    "DPS",
                };
            }
        }

        /// <summary>
        /// GetCharacterCalculations is the primary method of each model, where a majority of the calculations
        /// and formulae will be used. GetCharacterCalculations should call GetCharacterStats(), and based on
        /// those total stats for the character, and any calculationoptions on the character, perform all the 
        /// calculations required to come up with the final calculations defined in 
        /// CharacterDisplayCalculationLabels, including an Overall rating, and all Sub ratings defined in 
        /// SubPointNameColors.
        /// </summary>
        /// <param name="character">The character to perform calculations for.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A custom CharacterCalculations object which inherits from CharacterCalculationsBase,
        /// containing all of the final calculations defined in CharacterDisplayCalculationLabels. See
        /// CharacterCalculationsBase comments for more details.</returns>
        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            #region Setup
            CharacterCalculationsTankDK basecalcs = new CharacterCalculationsTankDK();
            CharacterCalculationsTankDK calcs = new CharacterCalculationsTankDK();
            TankDKChar TDK = new TankDKChar();

            if (character == null) { return calcs; }
            TDK.calcOpts = character.CalculationOptions as CalculationOptionsTankDK;
            if (TDK.calcOpts == null) { return calcs; }
            TDK.Char = character;
            TDK.bo = character.BossOptions;
            // Make sure there is at least one attack in the list.  
            // If there's not, add a Default Melee Attack for processing  
            if (TDK.bo.Attacks.Count < 1)
            {
                TDK.Char.IsLoading = true;
                TDK.bo.DamagingTargs = true;
                TDK.bo.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                TDK.Char.IsLoading = false;
            }
            // Make sure there is a default melee attack  
            // If the above processed, there will be one so this won't have to process  
            // If the above didn't process and there isn't one, add one now  
            if (TDK.bo.DefaultMeleeAttack == null)
            {
                TDK.Char.IsLoading = true;
                TDK.bo.DamagingTargs = true;
                TDK.bo.Attacks.Add(BossHandler.ADefaultMeleeAttack);
                TDK.Char.IsLoading = false;
            }
            // Since the above forced there to be an attack it's safe to do this without a null check  
            // Attack bossAttack = TDK.bo.DefaultMeleeAttack;
            #endregion

            #region Stats
            // Get base stats that will be used for paperdoll:
            StatsDK stats = GetCharacterStats(TDK.Char, additionalItem) as StatsDK;
            // validate that we get a stats object;
            if (null == stats) { return calcs; }

            // This is the point that SHOULD have the right values according to the paper-doll.
            StatsDK sPaperDoll = stats.Clone() as StatsDK;
            #endregion

            #region Evaluation
            Rawr.DPSDK.CharacterCalculationsDPSDK DPSCalcs = new Rawr.DPSDK.CharacterCalculationsDPSDK();
            Rawr.DPSDK.CalculationOptionsDPSDK DPSopts = new Rawr.DPSDK.CalculationOptionsDPSDK();
            DPSopts.presence = Presence.Blood;
            DKCombatTable ct = new DKCombatTable(TDK.Char, stats, DPSCalcs, DPSopts, TDK.bo);
            Rotation rot = new Rotation(ct, true);
            rot.PRE_BloodDiseased();
            // Base calculation values.  This will give us Mitigation, and Survival w/ base stats.
            basecalcs = GetCharacterCalculations(TDK, stats, rot);
            // Setup max values w/ everything turned on.
            stats = GetCharacterStats(TDK.Char, additionalItem, StatType.Maximum, TDK, rot);
            calcs.SEStats = stats.Clone() as StatsDK;
            ct = new DKCombatTable(TDK.Char, stats, DPSCalcs, DPSopts, TDK.bo);
            rot = new Rotation(ct, true);
            rot.PRE_BloodDiseased();

            calcs = GetCharacterCalculations(TDK, stats, rot);
            // Survival
            calcs.Burst = calcs.Survival - basecalcs.Survival;
            calcs.PhysicalSurvival = basecalcs.PhysicalSurvival;
            calcs.MagicSurvival = basecalcs.MagicSurvival;
            calcs.BleedSurvival = basecalcs.BleedSurvival;

            // Mitigation
            calcs.Burst += calcs.Mitigation - basecalcs.Mitigation;
            calcs.Mitigation = basecalcs.Mitigation;
            calcs.MitigationWeight = TDK.calcOpts.MitigationWeight;

            #region **** Burst: DS & Blood Shield ****
            float DSDam = calcs.DTPS * 5f;
            float minDSHeal = stats.Health * .07f;
            float DamDSHeal = (DSDam * .15f) * (1 + .15f * TDK.Char.DeathKnightTalents.ImprovedDeathStrike); // DS heals for avg damage over the last 5 secs.
            float DSHeal = Math.Max(minDSHeal, DamDSHeal);
            calcs.DSHeal = DSHeal;
            calcs.DSOverHeal = DSHeal * TDK.calcOpts.pOverHealing;
            calcs.DSCount = TDK.bo.BerserkTimer * rot.m_DSperSec;
            float BloodShield = (DSHeal * .5f) * (1 + (stats.Mastery * .0625f));
            calcs.BShield = BloodShield;
            // Get HitChance value from the DS in the rotation.
            AbilityDK_Base DS = rot.GetAbilityOfType(DKability.DeathStrike) as AbilityDK_Base;
            float DSHealsPSec = (DSHeal * rot.m_DSperSec * (1f - TDK.calcOpts.pOverHealing) * (DS.HitChance));
            calcs.TotalDShealed = DSHealsPSec * TDK.bo.BerserkTimer;
            float BShieldPSec = BloodShield * rot.m_DSperSec; // A new shield w/ each DS.
            calcs.TotalBShield = BShieldPSec * TDK.bo.BerserkTimer;
            calcs.Burst += BloodShield + (DSHeal * 1f - TDK.calcOpts.pOverHealing);
            calcs.HPS += DSHealsPSec;
            calcs.DTPS -= BShieldPSec;
            calcs.BurstWeight = TDK.calcOpts.BurstWeight;

            #endregion

            #endregion

            #region Key Data Validation
            if (float.IsNaN(calcs.Threat) ||
                float.IsNaN(calcs.Survival) ||
                float.IsNaN(calcs.Burst) ||
                float.IsNaN(calcs.Mitigation) ||
                //				float.IsNaN(calcs.BurstTime) ||
                //				float.IsNaN(calcs.ReactionTime) ||
                float.IsNaN(calcs.OverallPoints))
            {
#if DEBUG
                throw new Exception("One of the Subpoints are Invalid.");
#endif
            }
            #endregion

            #region Display only work
            calcs.Miss = sPaperDoll.Miss;
            calcs.Dodge = sPaperDoll.Dodge;
            calcs.Parry = sPaperDoll.Parry;
            calcs.cType = TDK.calcOpts.cType;
            if (TDK.calcOpts.cType == CalculationType.Burst)
            {
                _subPointNameColors = _subPointNameColors_Burst;
            }
            else
            {
                _subPointNameColors = _subPointNameColors_SMT;
            }

            calcs.BasicStats = sPaperDoll;
            calcs.SEStats = stats;
            // The full character data.
            calcs.TargetLevel = TDK.bo.Level;

            calcs.Resilience = stats.Resilience;
            if (null != rot.m_CT.MH)
            {
                calcs.TargetDodge = rot.m_CT.MH.chanceDodged;
                calcs.TargetMiss = rot.m_CT.MH.chanceMissed;
                calcs.TargetParry = rot.m_CT.MH.chanceParried;
                calcs.Expertise = rot.m_CT.m_Calcs.MHExpertise;
            }
            #endregion
            return calcs;
        }

        private CharacterCalculationsTankDK GetCharacterCalculations(TankDKChar TDK, StatsDK stats, Rotation rot)
        {
            CharacterCalculationsTankDK calcs = new CharacterCalculationsTankDK();

            // Level differences.
            int iLevelDiff = Math.Max(TDK.bo.Level - TDK.Char.Level, 0);

            float fChanceToGetHit = 1f - (stats.Miss + stats.Dodge + stats.EffectiveParry);
            float ArmorDamageReduction = (float)StatConversion.GetArmorDamageReduction(TDK.bo.Level, stats.Armor, 0f, 0f);

            #region **** Setup Fight parameters ****

            // Get the values of each type of damage in %.
            // So first we get each type of damage in the same units: DPS.
            // Get the total DPS.

            float[] fCurrentDTPS = new float[EnumHelper.GetCount(typeof(SurvivalSub))];
            fCurrentDTPS[(int)SurvivalSub.Physical] = 0f;
            fCurrentDTPS[(int)SurvivalSub.Bleed] = 0f;
            fCurrentDTPS[(int)SurvivalSub.Magic] = 0f;

            float fTotalDPS = 0;
            // Factor the segments out.
            float fPhyDamPercent = 1;
            float fBleedDamPercent = 0;
            float fMagicDamPercent = 0;

            float fAvoidanceTotal = 1f - fChanceToGetHit;

            // We want to start getting the Boss Handler stuff going on.
            // Setup initial Boss data.
            // How much of what kind of damage does this boss deal with?
            #region ** Incoming Boss Damage **
            // Let's make sure this is even valid.
            
            foreach (Attack a in TDK.bo.Attacks) {
                if (a.AffectsRole[PLAYER_ROLES.MainTank]
                    || a.AffectsRole[PLAYER_ROLES.OffTank]
                    || a.AffectsRole[PLAYER_ROLES.TertiaryTank])
                {
                    // Bleeds vs Magic vs Physical
                    if (a.DamageType == ItemDamageType.Physical) {
                        // Bleed or Physical
                        // Need to figure out how to determine bleed vs. physical hits.
                        // Also need to balance out the physical hits and balance the hit rate.
                        if (!a.Avoidable) 
                        {
                            fCurrentDTPS[(int)SurvivalSub.Bleed] += GetDPS(a.DamagePerHit, a.AttackSpeed);
                        } 
                        else 
                        {
                            fCurrentDTPS[(int)SurvivalSub.Physical] += GetDPS(a.DamagePerHit, a.AttackSpeed);
                        }
                    } 
                    else 
                    {
                        // Magic
                        fCurrentDTPS[(int)SurvivalSub.Magic] += GetDPS(a.DamagePerHit, a.AttackSpeed);
                    }
                }
            }
            fTotalDPS = 0;
            fTotalDPS += fCurrentDTPS[(int)SurvivalSub.Physical];
            fTotalDPS += fCurrentDTPS[(int)SurvivalSub.Bleed];
            fTotalDPS += fCurrentDTPS[(int)SurvivalSub.Magic];

            if (fTotalDPS > 0)
            {
                fPhyDamPercent = fCurrentDTPS[(int)SurvivalSub.Physical] / fTotalDPS;
                fBleedDamPercent = fCurrentDTPS[(int)SurvivalSub.Bleed] / fTotalDPS;
                fMagicDamPercent = fCurrentDTPS[(int)SurvivalSub.Magic] / fTotalDPS;
            }

            float[] fDamagePct = new float[] {fPhyDamPercent, fBleedDamPercent, fMagicDamPercent};

            #endregion

            // Set the Fight Duration to no larger than the Berserk Timer
            // Question: What is the units for Berserk & Speed Timer? MS/S/M?
            // Does the boss have parry haste?
            bool bParryHaste = TDK.bo.DefaultMeleeAttack != null ? TDK.bo.DefaultMeleeAttack.UseParryHaste : false;

            #endregion

            #region ***** Survival Rating *****
            // Magical damage:
            // if there is a max resistance, then it's likely they are stacking for that resistance.  So factor in that Max resistance.
            float fMaxResist = Math.Max(stats.ArcaneResistance, stats.FireResistance);
            fMaxResist = Math.Max(fMaxResist, stats.FrostResistance);
            fMaxResist = Math.Max(fMaxResist, stats.NatureResistance);
            fMaxResist = Math.Max(fMaxResist, stats.ShadowResistance);

            float fMagicDR = StatConversion.GetAverageResistance(TDK.bo.Level, TDK.Char.Level, fMaxResist, 0f);
            calcs.MagicDamageReduction = fMagicDR;

            float[] SurvivalResults = new float [EnumHelper.GetCount(typeof(SurvivalSub))];
            SurvivalResults = GetSurvival(ref TDK, stats, fDamagePct, ArmorDamageReduction, fMagicDR);

            calcs.ArmorDamageReduction = ArmorDamageReduction;
            calcs.PhysicalSurvival = SurvivalResults[(int)SurvivalSub.Physical];
            calcs.BleedSurvival = SurvivalResults[(int)SurvivalSub.Bleed];
            calcs.MagicSurvival = SurvivalResults[(int)SurvivalSub.Magic];
            calcs.SurvivalWeight = TDK.calcOpts.SurvivalWeight;

            #endregion

            #region ***** Threat Rating *****
            calcs.RotationTime = rot.CurRotationDuration; // Display the rot in secs.
            calcs.Threat = rot.m_TPS;
            calcs.DPS = rot.m_DPS;
            calcs.Blood = rot.m_BloodRunes;
            calcs.Frost = rot.m_FrostRunes;
            calcs.Unholy = rot.m_UnholyRunes;
            calcs.Death = rot.m_DeathRunes;
            calcs.RP = rot.m_RunicPower;
            calcs.TotalThreat = (int)rot.TotalThreat;

            calcs.ThreatWeight = TDK.calcOpts.ThreatWeight;
            TDK.calcOpts.szRotReport = rot.ReportRotation();

            #endregion

            float fPercentCritMitigation = (stats.CritChanceReduction / .06f);

            float[] fCurrentMitigation = GetMitigation(ref TDK, stats, rot, fPercentCritMitigation, ArmorDamageReduction, fCurrentDTPS, fMagicDR);
            calcs.MagicDamageReduction += fCurrentMitigation[(int)MitigationSub.AMS];
            calcs.ArmorMitigation = fCurrentMitigation[(int)MitigationSub.Armor];
            calcs.AvoidanceMitigation = fCurrentMitigation[(int)MitigationSub.Avoidance];
            calcs.CritMitigation = fCurrentMitigation[(int)MitigationSub.Crit];
            calcs.DamageTakenMitigation = fCurrentMitigation[(int)MitigationSub.DamageReduction];
            calcs.DamageTakenMitigation += fCurrentMitigation[(int)MitigationSub.Haste];
            calcs.HealsMitigation = fCurrentMitigation[(int)MitigationSub.Heals];
            calcs.ImpedenceMitigation = fCurrentMitigation[(int)MitigationSub.Impedences];
            calcs.MagicDamageReduction += fCurrentMitigation[(int)MitigationSub.Magic];

            calcs.Crit = (.06f - stats.CritChanceReduction);
            calcs.DTPS = 0;
            foreach (float f in fCurrentDTPS)
            {
                calcs.DTPS += f;        
            }
            foreach (float f in fCurrentMitigation)
            {
                calcs.Mitigation += f;
            }

            #region ** Burst/Reaction Time **
/*            float fEffectiveHealth = fPhysicalSurvival + fBleedSurvival + fMagicalSurvival;
            // The next 2 returns are in swing count.
            float fReactionSwingCount = GetReactionTime(fAvoidanceTotal);
            // TODO: Update this w/ the Boss-handler info. 
            float DPH = 0;
            if (null != TDK.bo.DefaultMeleeAttack)
            {
                DPH = TDK.bo.DefaultMeleeAttack.DamagePerHit;
            }
            float fBurstSwingCount = GetBurstTime(fAvoidanceTotal, fEffectiveHealth, DPH);

            // Get how long that actually will be on Average.
            calcs.ReactionTime = fReactionSwingCount * TDK.bo.DynamicCompiler_Attacks.AttackSpeed * (1 + fBossAttackSpeedReduction);
            calcs.BurstTime = fBurstSwingCount * TDK.bo.DynamicCompiler_Attacks.AttackSpeed * (1 + fBossAttackSpeedReduction);

            // Total damage avoided between bursts.
            //            float fBurstDamage = fBurstSwingCount * fPerShotPhysical;
            //            float fBurstDPS = fBurstDamage / fBossAverageAttackSpeed;
            //            float fReactionDamage = fReactionSwingCount * fPerShotPhysical;
            */
            #endregion

            return calcs;
        }


        #region Character Stats
        /// <summary>
        /// GetCharacterStats is the 2nd-most calculation intensive method in a model. Here the model will
        /// combine all of the information about the character, including race, gear, enchants, buffs,
        /// calculationoptions, etc., to form a single combined Stats object. Three of the methods below
        /// can be called from this method to help total up stats: GetItemStats(character, additionalItem),
        /// GetEnchantsStats(character), and GetBuffsStats(character.ActiveBuffs).
        /// </summary>
        /// <param name="character">The character whose stats should be totaled.</param>
        /// <param name="additionalItem">An additional item to treat the character as wearing.
        /// This is used for gems, which don't have a slot on the character to fit in, so are just
        /// added onto the character, in order to get gem calculations.</param>
        /// <returns>A Stats object containing the final totaled values of all character stats.</returns>
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            return GetCharacterStats(character, additionalItem, StatType.Unbuffed, new TankDKChar());
        }

        /// <summary>
        /// Get Character Stats for multiple calls.  Allowing means by which to stack different sets/Special effects.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="additionalItem"></param>
        /// <param name="sType">Enum describing which set of stats we want.</param>
        /// <returns></returns>
        private StatsDK GetCharacterStats(Character character, Item additionalItem, StatType sType, TankDKChar TDK, Rotation rot = null)
        {
            StatsDK statsTotal = new StatsDK();
            if (null == character.CalculationOptions)
            {
                // Possibly put some error text here.
                return statsTotal;
            }
            // Warning TDK can be blank at this point.
            TDK.Char = character;
            TDK.calcOpts = character.CalculationOptions as CalculationOptionsTankDK;
            TDK.bo = character.BossOptions;

            // Start populating data w/ Basic racial & class baseline.
            Stats BStats = BaseStats.GetBaseStats(character);
            statsTotal.Accumulate(BStats);
            statsTotal.BaseAgility = BStats.Agility;

            AccumulateItemStats(statsTotal, character, additionalItem);
            AccumulateBuffsStats(statsTotal, character.ActiveBuffs); // includes set bonuses.

            #region T11
            foreach (Buff b in character.ActiveBuffs)
            {
                if (b.Name == "Magma Plated Battlearmor (T11) 2 Piece Bonus")
                    statsTotal.b2T11_Tank = true;
                if (b.Name == "Magma Plated Battlearmor (T11) 4 Piece Bonus")
                    statsTotal.b4T11_Tank = true;
            }
            if (statsTotal.b2T11_Tank)
                statsTotal.AddSpecialEffect(_SE_IBF[1]);
            else
                statsTotal.AddSpecialEffect(_SE_IBF[0]);
            #endregion

            #region Filter out the duplicate Fallen Crusader Runes:
            if (character.OffHand != null
                && character.OffHandEnchant != null
                && character.OffHandEnchant == Enchant.FindEnchant(3368, ItemSlot.OneHand, character)
                && character.MainHandEnchant == character.OffHandEnchant)
            {
                bool bFC1Found = false;
                bool bFC2Found = false;
                foreach (SpecialEffect se1 in statsTotal.SpecialEffects())
                {
                    // if we've already found them, and we're seeing them again, then remove these repeats.
                    if (bFC1Found && se1.Equals(_SE_FC1))
                        statsTotal.RemoveSpecialEffect(se1);
                    else if (bFC2Found && se1.Equals(_SE_FC2))
                        statsTotal.RemoveSpecialEffect(se1);
                    else if (se1.Equals(_SE_FC1))
                        bFC1Found = true;
                    else if (se1.Equals(_SE_FC2))
                        bFC2Found = true;
                }
            }
            #endregion

            Rawr.DPSDK.CalculationsDPSDK.AccumulateTalents(statsTotal, character);
            Rawr.DPSDK.CalculationsDPSDK.AccumulatePresenceStats(statsTotal, Presence.Blood, character.DeathKnightTalents);

            // Stack only the info we care about.
            statsTotal = GetRelevantStatsLocal(statsTotal);

            /* At this point, we're combined all the data from gear and talents and all that happy jazz.
             * However, we haven't applied any special effects nor have we applied any multipliers.
             * Many special effects are now getting dependant upon combat info (rotations).
             */
            StatsDK PreRatingsBase = statsTotal.Clone() as StatsDK;
            // Apply the ratings to actual stats.
            ProcessRatings(statsTotal);
            ProcessAvoidance(statsTotal, TDK.bo.Level, TDK.Char, PreRatingsBase);
            statsTotal.EffectiveParry = 0;
            if (character.MainHand != null || character.OffHand != null)
            {
                statsTotal.EffectiveParry = statsTotal.Parry;
            }
            float fChanceToGetHit = 1f - (statsTotal.Miss + statsTotal.Dodge + statsTotal.EffectiveParry);

            // Now comes the special handling for secondary stats passes that are dependant upon Boss & Rotation values.
            if (sType != StatType.Unbuffed
                && (null != TDK.bo && null != rot)) // Make sure we have the rotation and Boss info.
            {

                #region Special Effects
                #region Talent: Bone Shield
                if (character.DeathKnightTalents.BoneShield > 0)
                {
                    int BSStacks = 4;  // The number of bones by default.  
                    float BoneLossRate = Math.Max(2f, TDK.bo.DynamicCompiler_Attacks.AttackSpeed / fChanceToGetHit);  // 2 sec internal cooldown on loosing bones so the DK can't get spammed to death.  
                    float moveVal = character.DeathKnightTalents.GlyphofBoneShield ? 0.15f : 0f;
                    SpecialEffect primary = new SpecialEffect(Trigger.Use,
                        new Stats() { DamageTakenMultiplier = -0.20f, BonusDamageMultiplier = 0.02f, MovementSpeed = moveVal, },
                        BoneLossRate * BSStacks, 60);
                    statsTotal.AddSpecialEffect(primary);
                }
                #endregion
                #region Vengeance
                // Vengence has the chance to increase AP.
                int iVengenceMax = (int)(statsTotal.Stamina + (BaseStats.GetBaseStats(character).Health) * .1);
                int iAttackPowerMax = (int)statsTotal.AttackPower + iVengenceMax;
                float mitigatedDPS = TDK.bo.GetDPSByType(ATTACK_TYPES.AT_MELEE, 0, statsTotal.DamageTakenMultiplier,
                    0, .14f, statsTotal.Miss, statsTotal.Dodge, statsTotal.EffectiveParry, 0, 0,
                    statsTotal.ArcaneResistance, statsTotal.FireResistance, statsTotal.FrostResistance, statsTotal.NatureResistance, statsTotal.ShadowResistance);
                mitigatedDPS = mitigatedDPS * (1 - (float)StatConversion.GetArmorDamageReduction(TDK.bo.Level, statsTotal.Armor, 0f, 0f));
                float APStackSingle = mitigatedDPS * 0.05f * TDK.bo.DynamicCompiler_Attacks.AttackSpeed;
                int APStackCountMax = (int)Math.Floor(iVengenceMax / APStackSingle);
                SpecialEffect seVeng = new SpecialEffect(Trigger.DamageTaken,
                    new Stats() { AttackPower = APStackSingle },
                    2 * 10,
                    0,
                    1,
                    APStackCountMax);
                statsTotal.VengenceAttackPower = seVeng.GetAverageStats().AttackPower;
                statsTotal.AttackPower += statsTotal.VengenceAttackPower * TDK.calcOpts.VengeanceWeight;
                #endregion
                statsTotal.AddSpecialEffect(_SE_DeathPact);
                // For now we just factor them in once.
                Rawr.DPSDK.StatsSpecialEffects sse = new Rawr.DPSDK.StatsSpecialEffects(rot.m_CT, rot, TDK.bo);
                Stats statSE = new Stats();
                Stats statOnUse = new Stats();
                foreach (SpecialEffect e in statsTotal.SpecialEffects())
                {
                    // We want the Special Effects that are OnUse to be separate 
                    // so they can be factored into Burst subvalue.
                    if (e.Trigger == Trigger.Use)
                    {
                        // There are some multi-level special effects that need to be factored in.
                        foreach (SpecialEffect ee in e.Stats.SpecialEffects())
                        {
                            e.Stats = sse.getSpecialEffects(ee);
                        }
                        // Only add in the OnUse triggers when we're trying to get our max stats.
                        if (sType == StatType.Maximum)
                            statSE.Accumulate(sse.getSpecialEffects(e));
                    }
                    else
                    {
                        // There are some multi-level special effects that need to be factored in.
                        foreach (SpecialEffect ee in e.Stats.SpecialEffects())
                        {
                            e.Stats = sse.getSpecialEffects(ee);
                        }
                        statSE.Accumulate(sse.getSpecialEffects(e));
                    }
                }
                // Darkmoon card greatness procs
                if (statSE.HighestStat > 0 || statSE.Paragon > 0)
                {
                    if (statSE.Strength >= statSE.Agility) { statSE.Strength += statSE.HighestStat + statSE.Paragon; }
                    else if (statSE.Agility > statSE.Strength) { statSE.Agility += statSE.HighestStat + statSE.Paragon; }
                    statSE.HighestStat = 0;
                    statSE.Paragon = 0;
                }

                // Any Modifiers from stats need to be applied to statSE
                statSE.Strength = StatConversion.ApplyMultiplier(statSE.Strength, statsTotal.BonusStrengthMultiplier);
                statSE.Agility = StatConversion.ApplyMultiplier(statSE.Agility, statsTotal.BonusAgilityMultiplier);
                statSE.Stamina = StatConversion.ApplyMultiplier(statSE.Stamina, statsTotal.BonusStaminaMultiplier);
                //            statSE.Stamina = (float)Math.Floor(statSE.Stamina);
                statSE.Armor = StatConversion.ApplyMultiplier(statSE.Armor, statsTotal.BaseArmorMultiplier);
                statSE.AttackPower = StatConversion.ApplyMultiplier(statSE.AttackPower, statsTotal.BonusAttackPowerMultiplier);
                statSE.BonusArmor = StatConversion.ApplyMultiplier(statSE.BonusArmor, statsTotal.BonusArmorMultiplier);

                statSE.Armor += statSE.BonusArmor;
                statSE.Health += StatConversion.GetHealthFromStamina(statSE.Stamina) + statSE.BattlemasterHealthProc;
                StatConversion.ApplyMultiplier(statSE.Health, statsTotal.BonusHealthMultiplier);
                if (character.DeathKnightTalents.BladedArmor > 0)
                {
                    statSE.AttackPower += (statSE.Armor / 180f) * (float)character.DeathKnightTalents.BladedArmor;
                }
                statSE.AttackPower += StatConversion.ApplyMultiplier((statSE.Strength * 2), statsTotal.BonusAttackPowerMultiplier);
                statSE.ParryRating += statSE.Strength * 0.25f;

                // Any Modifiers from statSE need to be applied to stats
                statsTotal.Strength = StatConversion.ApplyMultiplier(statsTotal.Strength, statSE.BonusStrengthMultiplier);
                statsTotal.Agility = StatConversion.ApplyMultiplier(statsTotal.Agility, statSE.BonusAgilityMultiplier);
                statsTotal.Stamina = StatConversion.ApplyMultiplier(statsTotal.Stamina, statSE.BonusStaminaMultiplier);
                //            stats.Stamina = (float)Math.Floor(stats.Stamina);
                statsTotal.Armor = StatConversion.ApplyMultiplier(statsTotal.Armor, statSE.BaseArmorMultiplier);
                statsTotal.AttackPower = StatConversion.ApplyMultiplier(statsTotal.AttackPower, statSE.BonusAttackPowerMultiplier);
                statsTotal.BonusArmor = StatConversion.ApplyMultiplier(statsTotal.BonusArmor, statSE.BonusArmorMultiplier);

                statsTotal.Accumulate(statSE);
#if DEBUG
                if (float.IsNaN(statsTotal.Stamina))
                    throw new Exception("Something very wrong in stats.");
#endif
                #endregion // Special effects

            }
            // Apply the Multipliers
            ProcessStatModifiers(statsTotal, character.DeathKnightTalents.BladedArmor, character);
            ProcessAvoidance(statsTotal, TDK.bo.Level, TDK.Char, PreRatingsBase);
            if (statsTotal.Mastery < 8)
            {
                throw new Exception("Mastery over-written during GetCharacterStats");
            }

            return (statsTotal);
        }

        public StatsDK GetBuffsStats(Character character, CalculationOptionsTankDK calcOpts)
        {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            StatsDK statsBuffs = GetBuffsStats(character.ActiveBuffs) as StatsDK;

            foreach (Buff b in removedBuffs)
            {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs)
            {
                character.ActiveBuffs.Remove(b);
            }

            foreach (Buff b in character.ActiveBuffs)
            {
//                statsBuffs.b2T11_Tank;
//                statsBuffs.b4T11_Tank;
            }
            return statsBuffs;
        }

        /// <summary>
        /// Process the Stat modifier values 
        /// </summary>
        /// <param name="statsTotal">[in/out] Stats object for the total character stats.</param>
        /// <param name="iBladedArmor">[in] character.talent.BladedArmor</param>
        private void ProcessStatModifiers(StatsDK statsTotal, int iBladedArmor, Character c)
        {
            statsTotal.Strength = StatConversion.ApplyMultiplier(statsTotal.Strength, statsTotal.BonusStrengthMultiplier);
            statsTotal.Agility = StatConversion.ApplyMultiplier(statsTotal.Agility, statsTotal.BonusAgilityMultiplier);
            // The stamina value is floor in game for the calculation
            statsTotal.Stamina = StatConversion.ApplyMultiplier(statsTotal.Stamina, statsTotal.BonusStaminaMultiplier);
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina);
            statsTotal.Armor = StatConversion.ApplyMultiplier(statsTotal.Armor, statsTotal.BaseArmorMultiplier);
            statsTotal.AttackPower = StatConversion.ApplyMultiplier(statsTotal.AttackPower, statsTotal.BonusAttackPowerMultiplier);
            statsTotal.BonusArmor = StatConversion.ApplyMultiplier(statsTotal.BonusArmor, statsTotal.BonusArmorMultiplier);

            statsTotal.Armor += statsTotal.BonusArmor;
            statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);

            StatConversion.ApplyMultiplier(statsTotal.Health, statsTotal.BonusHealthMultiplier);

            // Talent: BladedArmor //////////////////////////////////////////////////////////////
            if (iBladedArmor > 0)
            {
                statsTotal.AttackPower += (statsTotal.Armor / 180f) * (float)iBladedArmor;
            }
            // AP, crit, etc.  already being factored in w/ multiplier.
            statsTotal.AttackPower += StatConversion.ApplyMultiplier((statsTotal.Strength * 2), statsTotal.BonusAttackPowerMultiplier);
            statsTotal.ParryRating += (statsTotal.Strength - BaseStats.GetBaseStats(c).Strength) * 0.25f;
            statsTotal.Dodge += StatConversion.GetDodgeFromAgility(statsTotal.Agility, CharacterClass.DeathKnight);
        }

        /// <summary>
        /// Process All the ratings score to their base values.
        /// </summary>
        /// <param name="s"></param>
        private void ProcessRatings(StatsDK statsTotal)
        {
            // Expertise Rating -> Expertise:
            statsTotal.Expertise += StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating);
            // Mastery Rating
            if (statsTotal.Mastery < 8) statsTotal.Mastery += 8;  // Incase the Mastery is getting filtered out.
            statsTotal.Mastery += StatConversion.GetMasteryFromRating(statsTotal.MasteryRating);

            statsTotal.PhysicalHit += StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating);
            statsTotal.PhysicalCrit += StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating);
            statsTotal.PhysicalCrit += StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, CharacterClass.DeathKnight);
            statsTotal.PhysicalHaste += StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.DeathKnight);

            statsTotal.SpellHit += StatConversion.GetSpellHitFromRating(statsTotal.HitRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromRating(statsTotal.CritRating);
            statsTotal.SpellCrit += StatConversion.GetSpellCritFromIntellect(statsTotal.Intellect);
            statsTotal.SpellCrit += statsTotal.SpellCritOnTarget;
            statsTotal.SpellHaste += StatConversion.GetSpellHasteFromRating(statsTotal.HasteRating, CharacterClass.DeathKnight);
        }

        private void ProcessAvoidance(StatsDK statsTotal, int iTargetLevel, Character c, StatsDK PreRatingsBase)
        {
            // Key point to get avoidance bases.  
            // Ratings with change with multiple passes, 
            // but the actual values need to return back to the base.
            statsTotal.Miss = PreRatingsBase.Miss;
            statsTotal.Dodge = PreRatingsBase.Dodge;
            statsTotal.Parry = PreRatingsBase.Parry;
            // Get all the character avoidance numbers including deminishing returns.
            // Iterate through each hit type. and use fAvoidance array w/ the hitresult enum.
            float[] fAvoidance = new float[HitResultCount];
            for (uint i = 0; i < HitResultCount; i++)
            {
                // GetDRAvoidanceChance returns a dec. percentage.
                // Since CurrentAvoidance is a percent, need to multiply by 100.
                fAvoidance[i] = (StatConversion.GetDRAvoidanceChance(c, statsTotal, (HitResult)i, iTargetLevel));
            }

            // So let's populate the miss, dodge and parry values for the UI display as well as pulling them out of the avoidance number.
            statsTotal.Miss = Math.Min((StatConversion.CAP_MISSED[(int)CharacterClass.DeathKnight] / 100), fAvoidance[(int)HitResult.Miss]);
            statsTotal.Dodge = Math.Min((StatConversion.CAP_DODGE[(int)CharacterClass.DeathKnight] / 100), fAvoidance[(int)HitResult.Dodge]);
            statsTotal.Parry = Math.Min((StatConversion.CAP_PARRY[(int)CharacterClass.DeathKnight] / 100), fAvoidance[(int)HitResult.Parry]);
        }
        #endregion

        #region Relevant Stats
        /// <summary>
        /// Filters a Stats object to just the stats relevant to the model.
        /// </summary>
        /// <param name="stats">A complete Stats object containing all stats.</param>
        /// <returns>A filtered Stats object containing only the stats relevant to the model.</returns>
        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Strength = stats.Strength,
                Agility = stats.Agility,
                BaseAgility = stats.BaseAgility,
                Stamina = stats.Stamina,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                Health = stats.Health,
                BattlemasterHealthProc = stats.BattlemasterHealthProc,

                HighestStat = stats.HighestStat,
                Paragon = stats.Paragon,

                ParryRating = stats.ParryRating,
                DodgeRating = stats.DodgeRating,
                CritChanceReduction = stats.CritChanceReduction,

                Dodge = stats.Dodge,
                Parry = stats.Parry,
                Miss = stats.Miss,

                Resilience = stats.Resilience,
                SpellPenetration = stats.SpellPenetration,

                DamageAbsorbed = stats.DamageAbsorbed,
                AttackPower = stats.AttackPower,
                HitRating = stats.HitRating,
                CritRating = stats.CritRating,
                MasteryRating = stats.MasteryRating,
                ExpertiseRating = stats.ExpertiseRating,
                Expertise = stats.Expertise,
                HasteRating = stats.HasteRating,
                WeaponDamage = stats.WeaponDamage,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                SpellHit = stats.SpellHit,

                Healed = stats.Healed,
                HealthRestore = stats.HealthRestore,
                HealthRestoreFromMaxHealth = stats.HealthRestoreFromMaxHealth,
                Hp5 = stats.Hp5,

                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                BonusSpellCritDamageMultiplier = stats.BonusSpellCritDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                DamageTakenMultiplier = stats.DamageTakenMultiplier,
                SpellDamageTakenMultiplier = stats.SpellDamageTakenMultiplier,
                PhysicalDamageTakenMultiplier = stats.PhysicalDamageTakenMultiplier,
                BossPhysicalDamageDealtMultiplier = stats.BossPhysicalDamageDealtMultiplier,
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,

                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,

                // General Damage Mods.
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusDiseaseDamageMultiplier = stats.BonusDiseaseDamageMultiplier,

                // Ability mods.
                BonusDamageBloodStrike = stats.BonusDamageBloodStrike,
                BonusDamageDeathCoil = stats.BonusDamageDeathCoil,
                BonusDamageDeathStrike = stats.BonusDamageDeathStrike,
                BonusDamageFrostStrike = stats.BonusDamageFrostStrike,
                BonusDamageHeartStrike = stats.BonusDamageHeartStrike,
                BonusDamageIcyTouch = stats.BonusDamageIcyTouch,
                BonusDamageObliterate = stats.BonusDamageObliterate,
                BonusDamageScourgeStrike = stats.BonusDamageScourgeStrike,
                BonusFrostWeaponDamage = stats.BonusFrostWeaponDamage,

                BonusCritChanceDeathCoil = stats.BonusCritChanceDeathCoil,
                BonusCritChanceFrostStrike = stats.BonusCritChanceFrostStrike,
                BonusCritChanceObliterate = stats.BonusCritChanceObliterate,

                AntiMagicShellDamageReduction = stats.AntiMagicShellDamageReduction,

                BonusHealingReceived = stats.BonusHealingReceived,
                RPp5 = stats.RPp5,

                // Resistances
                ArcaneResistance = stats.ArcaneResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                NatureResistance = stats.NatureResistance,
                ShadowResistance = stats.ShadowResistance,

                // Damage Procs
                ArcaneDamage = stats.ArcaneDamage,
                FireDamage = stats.FireDamage,
                FrostDamage = stats.FrostDamage,
                ShadowDamage = stats.ShadowDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,

                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                StunDurReduc = stats.StunDurReduc,
                MovementSpeed = stats.MovementSpeed,
                TargetArmorReduction = stats.TargetArmorReduction,
            };

            // Also bringing in the trigger events from DPSDK - 
            // Since I'm going to move the +Def bonus for the Sigil of the Unfaltering Knight
            // To a special effect.  Also there are alot of OnUse and OnEquip special effects
            // That probably aren't being taken into effect.
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (HasRelevantStats(effect.Stats))
                {
                    s.AddSpecialEffect(effect);
                }
            }
            return s;
        }

        private StatsDK GetRelevantStatsLocal(Stats stats)
        {
            StatsDK s = new StatsDK();
            s.Accumulate(GetRelevantStats(stats));
            return s;
        }

        /// <summary>
        /// Tests whether there are positive relevant stats in the Stats object.
        /// </summary>
        /// <param name="stats">The complete Stats object containing all stats.</param>
        /// <returns>True if any of the non-Zero stats in the Stats are relevant.  
        /// I realize that there aren't many stats that have negative values, but for completeness.</returns>
        public override bool HasRelevantStats(Stats stats)
        {
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                // if it has a DK specific trigger, then just return true.
                if (effect.Trigger == Trigger.BloodStrikeHit ||
                    effect.Trigger == Trigger.DeathRuneGained ||
                    effect.Trigger == Trigger.DeathStrikeHit ||
                    effect.Trigger == Trigger.HeartStrikeHit ||
                    effect.Trigger == Trigger.IcyTouchHit ||
                    effect.Trigger == Trigger.ObliterateHit ||
                    effect.Trigger == Trigger.PlagueStrikeHit ||
                    effect.Trigger == Trigger.RuneStrikeHit ||
                    effect.Trigger == Trigger.ScourgeStrikeHit
                    )
                {
                    return true;
                }
                else if (effect.Trigger == Trigger.DamageDone ||
                    effect.Trigger == Trigger.DamageOrHealingDone ||
                    effect.Trigger == Trigger.DamageTaken ||
                    effect.Trigger == Trigger.DamageTakenMagical ||
                    effect.Trigger == Trigger.DamageTakenPhysical ||
                    effect.Trigger == Trigger.DamageSpellCast ||
                    effect.Trigger == Trigger.DamageSpellCrit ||
                    effect.Trigger == Trigger.DamageSpellHit ||
                    effect.Trigger == Trigger.DamageTakenPutsMeBelow35PercHealth ||
                    effect.Trigger == Trigger.SpellCast ||
                    effect.Trigger == Trigger.SpellCrit ||
                    effect.Trigger == Trigger.SpellHit ||
                    effect.Trigger == Trigger.DoTTick ||
                    effect.Trigger == Trigger.MeleeCrit ||
                    effect.Trigger == Trigger.MeleeHit ||
                    effect.Trigger == Trigger.OffHandHit ||
                    effect.Trigger == Trigger.CurrentHandHit ||
                    effect.Trigger == Trigger.MainHandHit ||
                    effect.Trigger == Trigger.PhysicalCrit ||
                    effect.Trigger == Trigger.PhysicalHit ||
                    effect.Trigger == Trigger.Use)
                {
                    return relevantStats(effect.Stats);
                }
            }
            return relevantStats(stats);
        }

        /// <summary>
        /// Helper function for HasRelevantStats() function of the base class.
        /// </summary>
        /// <param name="stats"></param>
        /// <returns>true == the stats object has interesting things for this model.</returns>
        private bool relevantStats(Stats stats)
        {
            bool bResults = false;
            // Core stats
            bResults |= (stats.Strength != 0);
            // Defense stats
            bResults |= (stats.DodgeRating != 0);
            bResults |= (stats.ParryRating != 0);

            bResults |= (stats.Dodge != 0);
            bResults |= (stats.Parry != 0);
            bResults |= (stats.Miss != 0);
            bool bHasCore = bResults; // if the above stats are 0, lets make sure we're not bringing in caster gear below.

            bResults |= (stats.Agility != 0);
            bResults |= (stats.Stamina != 0);
            bResults |= (stats.Armor != 0);
            bResults |= (stats.BonusArmor != 0);
            bResults |= (stats.Health != 0);
            bResults |= (stats.BattlemasterHealthProc != 0);

            bResults |= (stats.HighestStat != 0);
            bResults |= (stats.Paragon != 0);

            bResults |= (stats.DamageAbsorbed != 0);

            bResults |= (stats.Resilience != 0);
            bResults |= (stats.SpellPenetration != 0);

            // Offense stats
            bResults |= (stats.AttackPower != 0);
            bResults |= (stats.HitRating != 0);
            bResults |= (stats.CritRating != 0);
            bResults |= (stats.ExpertiseRating != 0);
            bResults |= (stats.Expertise != 0);
            bResults |= (stats.MasteryRating != 0);
            bResults |= (stats.HasteRating != 0);
            bResults |= (stats.WeaponDamage != 0);
            bResults |= (stats.PhysicalCrit != 0);
            bResults |= (stats.PhysicalHaste != 0);
            bResults |= (stats.PhysicalHit != 0);
            bResults |= (stats.SpellHit != 0);

            bResults |= (stats.Healed != 0);
            bResults |= (stats.HealthRestore != 0);
            bResults |= (stats.HealthRestoreFromMaxHealth != 0);
            bResults |= (stats.Hp5 != 0);

            // Bonus to stats
            bResults |= (stats.BonusArmorMultiplier != 0);
            bResults |= (stats.BaseArmorMultiplier != 0);
            bResults |= (stats.BonusHealthMultiplier != 0);
            bResults |= (stats.BonusStrengthMultiplier != 0);
            bResults |= (stats.BonusStaminaMultiplier != 0);
            bResults |= (stats.BonusAgilityMultiplier != 0);
            bResults |= (stats.BonusCritDamageMultiplier != 0);
            bResults |= (stats.BonusSpellCritDamageMultiplier != 0);
            bResults |= (stats.BonusAttackPowerMultiplier != 0);
            bResults |= (stats.BonusPhysicalDamageMultiplier != 0);
            bResults |= (stats.BonusDamageMultiplier != 0);
            bResults |= (stats.DamageTakenMultiplier != 0);
            bResults |= (stats.SpellDamageTakenMultiplier != 0);
            bResults |= (stats.PhysicalDamageTakenMultiplier != 0);
            bResults |= (stats.BossPhysicalDamageDealtMultiplier != 0);
            bResults |= (stats.ThreatIncreaseMultiplier != 0);
            bResults |= (stats.ThreatReductionMultiplier != 0);
            bResults |= (stats.BonusWhiteDamageMultiplier != 0);

            // Damage Multipliers:
            bResults |= (stats.BonusShadowDamageMultiplier != 0);
            bResults |= (stats.BonusFrostDamageMultiplier != 0);
            bResults |= (stats.BonusDiseaseDamageMultiplier != 0);

            // Bulk Damage:
            bResults |= (stats.BonusDamageBloodStrike != 0);
            bResults |= (stats.BonusDamageDeathCoil != 0);
            bResults |= (stats.BonusDamageDeathStrike != 0);
            bResults |= (stats.BonusDamageFrostStrike != 0);
            bResults |= (stats.BonusDamageHeartStrike != 0);
            bResults |= (stats.BonusDamageIcyTouch != 0);
            bResults |= (stats.BonusDamageObliterate != 0);
            bResults |= (stats.BonusDamageScourgeStrike != 0);
            bResults |= (stats.BonusFrostWeaponDamage != 0);

            // Others
            bResults |= (stats.BonusCritChanceDeathCoil != 0);
            bResults |= (stats.BonusCritChanceFrostStrike != 0);
            bResults |= (stats.BonusCritChanceObliterate != 0);
            bResults |= (stats.AntiMagicShellDamageReduction != 0);
            bResults |= (stats.BonusHealingReceived != 0);
            bResults |= (stats.RPp5 != 0);

            // Resistances
            bResults |= (stats.ArcaneResistance != 0);
            bResults |= (stats.FireResistance != 0);
            bResults |= (stats.FrostResistance != 0);
            bResults |= (stats.NatureResistance != 0);
            bResults |= (stats.ShadowResistance != 0);

            // Damage Procs
            bResults |= (stats.ArcaneDamage != 0);
            bResults |= (stats.FireDamage != 0);
            bResults |= (stats.FrostDamage != 0);
            bResults |= (stats.ShadowDamage != 0);
            bResults |= (stats.HolyDamage != 0);
            bResults |= (stats.NatureDamage != 0);

            // BossHandler
            bResults |= (stats.SnareRootDurReduc != 0);
            bResults |= (stats.FearDurReduc != 0);
            bResults |= (stats.StunDurReduc != 0);
            bResults |= (stats.MovementSpeed != 0);
            bResults |= (stats.TargetArmorReduction != 0);

            // Filter out caster gear:
            if (!bHasCore & bResults)
                // Let's make sure that if we've got some stats that may be interesting
            {
                /*
                bResults = !(
                    (stats.Intellect != 0)
                    || (stats.Spirit != 0)
                    || (stats.Mp5 != 0)
                    || (stats.ManaRestore != 0)
                    || (stats.SpellPower != 0)
                    || (stats.Mana != 0)
                    || (stats.BonusIntellectMultiplier != 0)
                    || (stats.BonusSpiritMultiplier != 0)
                    || (stats.SpellPenetration != 0)
                    || (stats.BonusManaMultiplier != 0)                                
                    );
                 */
            }

            return bResults;
        }

        public override bool IsItemRelevant(Item item) 
        {
            if (item.Slot == ItemSlot.Ranged && (item.Type != ItemType.Sigil && item.Type != ItemType.Relic)) { return false; }
            return base.IsItemRelevant(item);
        }
        #endregion

        #region Evaluations And Ratings
        private static float[] GetSurvival(ref TankDKChar TDK, StatsDK stats, float[] DamagePercentages, float ArmorDamageReduction, float fMagicDR)
        {
            // For right now Survival Rating == Effective Health will be HP + Armor/Resistance mitigation values.
            // Everything else is really mitigating damage based on RNG.

            // The health bonus from Frost presence is now include in the character by default.
            float fPhysicalSurvival = stats.Health;
            float fBleedSurvival = stats.Health;
            float fMagicalSurvival = stats.Health;

            // Physical damage:
            fPhysicalSurvival = GetEffectiveHealth(stats.Health, ArmorDamageReduction, DamagePercentages[(int)SurvivalSub.Physical]);

            // Bleed damage:
            fBleedSurvival = GetEffectiveHealth(stats.Health, 0, DamagePercentages[(int)SurvivalSub.Physical]);
            
            // Magic Damage:
            fMagicalSurvival = GetEffectiveHealth(stats.Health, fMagicDR, DamagePercentages[(int)SurvivalSub.Physical]);

            // Since Armor plays a role in Survival, so shall the other damage taken adjusters.
            // Note, it's expected that (at least for tanks) that DamageTakenMultiplier will be Negative.
            // So the next line should INCREASE survival because 
            // fPhysicalSurvival * (1 - [some negative value] * (1 - [0 or some negative value])
            // will look like:
            // fPhysicalSurvival * 1.xx * 1.xx
            fPhysicalSurvival = fPhysicalSurvival * (1 - stats.DamageTakenMultiplier) * (1 - stats.PhysicalDamageTakenMultiplier);
            fBleedSurvival = fBleedSurvival * (1 - stats.DamageTakenMultiplier) * (1 - stats.PhysicalDamageTakenMultiplier);
            fMagicalSurvival = fMagicalSurvival * (1 - stats.DamageTakenMultiplier) * (1 - stats.SpellDamageTakenMultiplier);
            float[] SurvivalResults = new float[EnumHelper.GetCount(typeof(SurvivalSub))];
            SurvivalResults[(int)SurvivalSub.Physical] = fPhysicalSurvival;
            SurvivalResults[(int)SurvivalSub.Bleed] = fBleedSurvival;
            SurvivalResults[(int)SurvivalSub.Magic] = fMagicalSurvival;
            return SurvivalResults;
        }

        private static float[] GetMitigation(ref TankDKChar TDK, StatsDK stats, Rotation rot, float fPercentCritMitigation, float ArmorDamageReduction, float[] fCurrentDTPS, float fMagicDR)
        {
            float[] fTotalMitigation = new float[EnumHelper.GetCount(typeof(MitigationSub))];
            // Ensure the CurrentDTPS structure is the right size.
            if (fCurrentDTPS.Length < EnumHelper.GetCount(typeof(SurvivalSub)))
            {
                fCurrentDTPS = new float[EnumHelper.GetCount(typeof(SurvivalSub))];
            }

            float fSegmentMitigation = 0f;
            float fSegmentDPS = 0f;
            float fPhyDamageDPS = fCurrentDTPS[(int)SurvivalSub.Physical];
            float fMagicDamageDPS = fCurrentDTPS[(int)SurvivalSub.Magic];
            float fBleedDamageDPS = fCurrentDTPS[(int)SurvivalSub.Bleed];
            #region *** Damage Avoided (Crit, Haste, Avoidance) ***
            #region ** Crit Mitigation **
            // Crit mitigation:
            // Crit mitigation work for Physical damage only.
            float fCritMultiplier = .12f;
            // Bleeds can't crit.
            // Neither can spells from bosses.  (As per a Loading screen ToolTip.)
            float fCritDPS = fPhyDamageDPS * fCritMultiplier;
            fSegmentMitigation = (fCritDPS * fPercentCritMitigation);
            // Add in the value of crit mitigation.
            fTotalMitigation[(int)MitigationSub.Crit] = fSegmentMitigation;
            // The max damage at this point needs to include crit.
            fCurrentDTPS[(int)SurvivalSub.Physical] = fCurrentDTPS[(int)SurvivalSub.Physical] + (fCritDPS - fSegmentMitigation);
            #endregion
            #region ** Haste Mitigation **
            // Placeholder for comparing differing DPS values related to haste.
            float fNewIncPhysDPS = 0;
            // Let's just look at Imp Icy Touch 
            #region Improved Icy Touch
            // Get the new slowed AttackSpeed based on ImpIcyTouch
            // Factor in the base slow caused by FF (14% base).
            float fBossAttackSpeedReduction = 0.0f;

            if (rot.Contains(DKability.IcyTouch)
                || rot.Contains(DKability.FrostFever))
            {
                fBossAttackSpeedReduction = 0.14f;
            }
            // Figure out what the new Physical DPS should be based on that.
            fSegmentDPS = TDK.bo.GetDPSByType(ATTACK_TYPES.AT_MELEE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            fNewIncPhysDPS = TDK.bo.GetDPSByType(ATTACK_TYPES.AT_MELEE, 0, 0, 0, fBossAttackSpeedReduction, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            // Send the difference to the Mitigation value.
            fSegmentMitigation = fSegmentDPS - fNewIncPhysDPS;
            fTotalMitigation[(int)MitigationSub.Haste] += fSegmentMitigation;
            fCurrentDTPS[(int)SurvivalSub.Physical] -= fSegmentMitigation;
            #endregion

            #endregion
            #region ** Avoidance Mitigation **
            // Let's see how much damage was avoided.
            // Raise the total mitgation by that amount.

            fSegmentDPS = fNewIncPhysDPS;
            fNewIncPhysDPS = TDK.bo.GetDPSByType(ATTACK_TYPES.AT_MELEE, 0, 0, 0, fBossAttackSpeedReduction, stats.Miss, stats.Dodge, stats.EffectiveParry, 0, 0, 0, 0, 0, 0, 0);
            fSegmentMitigation = fSegmentDPS - fNewIncPhysDPS;
            fTotalMitigation[(int)MitigationSub.Avoidance] += fSegmentMitigation;
            fCurrentDTPS[(int)SurvivalSub.Physical] -= fSegmentMitigation;
            #endregion
            #endregion

            #region *** Damage Reduced (AMS, Armor, Magic Resist, DamageTaken Modifiers) ***
            #region ** Anti-Magic Shell **
            // TODO: This is a CD, so would only be in BURST.
            // Anti-Magic Shell. ////////////////////////////////////////////////////////
            // Talent: MagicSuppression increases AMS by 8/16/25% per point.
            // Glyph: GlyphofAntiMagicShell increases AMS by 2 sec.
            // AMS has a 45 sec CD.
            float amsDuration = (5f + (TDK.Char.DeathKnightTalents.GlyphofAntiMagicShell == true ? 2f : 0f));
            float amsUptimePct = amsDuration / 45f;
            // AMS reduces damage taken by 75% up to a max of 50% health.
            float amsReduction = 0.75f * (1f + TDK.Char.DeathKnightTalents.MagicSuppression * 0.08f + (TDK.Char.DeathKnightTalents.MagicSuppression == 3 ? 0.01f : 0f));
            float amsReductionMax = stats.Health * 0.5f;
            // up to 50% of health means that the amdDRvalue equates to the raw damage points removed.  
            // This means that toon health and INC damage values from the options pane are going to affect this quite a bit.
            float amsDRvalue = (Math.Min(amsReductionMax, (fMagicDamageDPS * amsDuration) * amsReduction) * amsUptimePct);
            // Raise the TotalMitigation by that amount.
            fCurrentDTPS[(int)SurvivalSub.Magic] -= amsDRvalue;
            fTotalMitigation[(int)MitigationSub.AMS] += amsDRvalue;
            #endregion
            #region ** Armor Damage Mitigation **
            // For any physical only damage reductions. 
            // Factor in armor Damage Reduction
            fSegmentMitigation = fPhyDamageDPS * ArmorDamageReduction;
//            calcs.ArmorMitigation = fSegmentMitigation;
            fTotalMitigation[(int)MitigationSub.Armor] += fSegmentMitigation;
            fCurrentDTPS[(int)SurvivalSub.Physical] -= (fCurrentDTPS[(int)SurvivalSub.Physical] * ArmorDamageReduction);
            #endregion
            #region ** Resistance Damage Mitigation **
            // For any physical only damage reductions. 
            // Factor in armor Damage Reduction
            fSegmentMitigation = fMagicDamageDPS * fMagicDR;
            fTotalMitigation[(int)MitigationSub.Magic] += fSegmentMitigation;
            fCurrentDTPS[(int)SurvivalSub.Magic] -= fCurrentDTPS[(int)SurvivalSub.Magic] * fMagicDR;
            #endregion
            #region ** Damage Taken Mitigation **
            fTotalMitigation[(int)MitigationSub.DamageReduction] += Math.Abs(fMagicDamageDPS * stats.DamageTakenMultiplier) + Math.Abs(fMagicDamageDPS * stats.SpellDamageTakenMultiplier);
            fTotalMitigation[(int)MitigationSub.DamageReduction] += Math.Abs(fBleedDamageDPS * stats.DamageTakenMultiplier) + Math.Abs(fBleedDamageDPS * stats.PhysicalDamageTakenMultiplier);
            fTotalMitigation[(int)MitigationSub.DamageReduction] += Math.Abs(fPhyDamageDPS * stats.DamageTakenMultiplier) + Math.Abs(fPhyDamageDPS * stats.PhysicalDamageTakenMultiplier);
            
            fCurrentDTPS[(int)SurvivalSub.Magic] -= Math.Abs(fCurrentDTPS[(int)SurvivalSub.Magic] * stats.DamageTakenMultiplier) + Math.Abs(fCurrentDTPS[(int)SurvivalSub.Magic] * stats.SpellDamageTakenMultiplier);
            fCurrentDTPS[(int)SurvivalSub.Bleed] -= Math.Abs(fCurrentDTPS[(int)SurvivalSub.Bleed] * stats.DamageTakenMultiplier) + Math.Abs(fCurrentDTPS[(int)SurvivalSub.Bleed] * stats.PhysicalDamageTakenMultiplier);
            fCurrentDTPS[(int)SurvivalSub.Physical] -= Math.Abs(fCurrentDTPS[(int)SurvivalSub.Physical] * stats.DamageTakenMultiplier) + Math.Abs(fCurrentDTPS[(int)SurvivalSub.Physical] * stats.PhysicalDamageTakenMultiplier);
            #endregion
            #endregion

            #region ** Boss Handler Mitigation (Impedences) **
            float TotalDuration = 0;
            float ImprovedDuration = 0;
            float ImpedenceMitigation = 0;
            #region ** Fear Based Mitigation (fear reduction) **
            if (TDK.bo.Fears.Count > 0)
            {
                foreach (Impedance m in TDK.bo.Fears)
                {
                    TotalDuration += m.Duration;
                }
                ImprovedDuration = TotalDuration / (1 + stats.FearDurReduc);
                fSegmentMitigation = (TotalDuration - ImprovedDuration) /* Add some multiplier to this */;
                ImpedenceMitigation += fSegmentMitigation;
            }
            #endregion

            #region ** Movement Based Mitigation (run speed) **
            if (TDK.bo.Moves.Count > 0)
            {
                TotalDuration = 0;
                foreach (Impedance m in TDK.bo.Moves)
                {
                    TotalDuration += m.Duration;
                }
                ImprovedDuration = TotalDuration / (1 + stats.MovementSpeed);
                fSegmentMitigation = (TotalDuration - ImprovedDuration) /* Add some multiplier to this */;
                ImpedenceMitigation += fSegmentMitigation;
            }
            #endregion

            #region ** Disarm Based Mitigation (Disarm reduction) **
            if (TDK.bo.Disarms.Count > 0)
            {
                TotalDuration = 0;
                foreach (Impedance m in TDK.bo.Disarms)
                {
                    TotalDuration += m.Duration;
                }
                ImprovedDuration = TotalDuration / (1 + stats.DisarmDurReduc);
                fSegmentMitigation = (TotalDuration - ImprovedDuration) /* Add some multiplier to this */;
                ImpedenceMitigation += fSegmentMitigation;
            }
            #endregion

            #region ** Stun Based Mitigation (stun reduction) **
            if (TDK.bo.Stuns.Count > 0)
            {
                TotalDuration = 0;
                foreach (Impedance m in TDK.bo.Stuns)
                {
                    TotalDuration += m.Duration;
                }
                ImprovedDuration = TotalDuration / (1 + stats.StunDurReduc);
                fSegmentMitigation = (TotalDuration - ImprovedDuration) /* Add some multiplier to this */;
                ImpedenceMitigation += fSegmentMitigation;
            }
            #endregion
            // calcs.ImpedenceMitigation = ImpedenceMitigation;
            fTotalMitigation[(int)MitigationSub.Impedences] += ImpedenceMitigation;
            #endregion

            #region ** Heals **
            fSegmentMitigation = StatConversion.ApplyMultiplier(stats.Healed, stats.HealingReceivedMultiplier);
            fSegmentMitigation += (StatConversion.ApplyMultiplier(stats.Hp5, stats.HealingReceivedMultiplier) / 5);
            fSegmentMitigation += StatConversion.ApplyMultiplier(stats.HealthRestore, stats.HealingReceivedMultiplier);
            // Health Returned by other sources than DS:
            if (stats.HealthRestoreFromMaxHealth > 0)
                fSegmentMitigation += (stats.HealthRestoreFromMaxHealth * stats.Health);
            fTotalMitigation[(int)MitigationSub.Heals] = fSegmentMitigation;
            #endregion

            return fTotalMitigation;
        }

        /// <summary>Evaluate how many swings until the tank is next hit.</summary>
        /// <param name="PercAvoidance">a float that is a 0-1 value for % of total avoidance (Dodge + Parry + Miss)</param>
        /// <returns>Float of how many swings until the next hit. Should be > 1</returns>
        private float GetReactionTime(float PercAvoidance)
        {
            float fReactionTime = 0f;
            // check args.
            if (PercAvoidance < 0f || PercAvoidance > 1f) { return 0f; }// error
            fReactionTime = 1f / (1f - PercAvoidance);
            return fReactionTime;
        }

        /// <summary>
        /// Evaluate how many swings until the tank dies.
        /// </summary>
        /// <param name="PercAvoidance">a float values of Total avoidance 0-1 as a decimal percentage.</param>
        /// <param name="EffectiveHealth">Survival score</param>
        /// <param name="RawPerHit">What's the raw unmitigated damage coming in.</param>
        /// <returns>the number of hits until death.</returns>
        private float GetBurstTime(float PercAvoidance, float EffectiveHealth, float RawPerHit)
        {
            float fBurstTime = 0f;
            // check args.
            if (PercAvoidance < 0 || PercAvoidance > 1) { return 0f; } // error

            float fHvH = (EffectiveHealth / RawPerHit);

            fBurstTime = (1f / PercAvoidance) * ((1f / (float)Math.Pow((1f - PercAvoidance), fHvH)) - 1f);

            return fBurstTime;
        }

        /// <summary>
        /// Get the value for a sub-component of Survival
        /// </summary>
        /// <param name="fHealth">Current HP</param>
        /// <param name="fDR">Damage Reduction rate</param>
        /// <param name="PercValue">% value of the survival rank. valid range 0-1</param>
        /// <returns></returns>
        private static float GetEffectiveHealth(float fHealth, float fDR, float PercValue)
        {
            // TotalSurvival == sum(Survival for each school)
            // Survival = (Health / (1 - DR)) * % damage inc from that school
            if (0f <= PercValue && PercValue <= 1f && fDR < 1f)
                return (fHealth / (1 - fDR)) * PercValue;
            else
                return 0;
        }

        private float GetDPS(float fPerUnitDamage, float fDamFrequency)
        {
            if (fDamFrequency > 0)
                return fPerUnitDamage / fDamFrequency;
            return 0f;
        }
        #endregion 

        #region Static SpecialEffects
        private static readonly SpecialEffect _SE_T10_4P = new SpecialEffect(Trigger.Use, new Stats() { DamageTakenMultiplier = -0.12f }, 10f, 60f);
        private static readonly SpecialEffect _SE_FC1 = new SpecialEffect(Trigger.DamageDone, new Stats() { BonusStrengthMultiplier = .15f }, 15f, 0f, -2f, 1);
        private static readonly SpecialEffect _SE_FC2 = new SpecialEffect(Trigger.DamageDone, new Stats() { HealthRestoreFromMaxHealth = .03f }, 0, 0f, -2f, 1);
        private static readonly SpecialEffect[] _SE_IBF = new SpecialEffect[] 
            {   new SpecialEffect(Trigger.Use, new Stats() { StunDurReduc = 1f, DamageTakenMultiplier = -.2f }, 12 * 1.0f, 3 * 60  ), // Default IBF
                new SpecialEffect(Trigger.Use, new Stats() { StunDurReduc = 1f, DamageTakenMultiplier = -.2f }, 12 * 1.5f, 3 * 60  ), // IBF w/ 4T11
            };
        private static readonly SpecialEffect _SE_AntiMagicZone = new SpecialEffect(Trigger.Use, new Stats() { SpellDamageTakenMultiplier = -0.75f }, 10f, 2f * 60f);
        private static readonly SpecialEffect _SE_RuneTap = new SpecialEffect(Trigger.Use, new Stats() { HealthRestoreFromMaxHealth = .1f }, 0, 30f);
        private static readonly SpecialEffect _SE_DeathPact = new SpecialEffect(Trigger.Use, new Stats() { HealthRestoreFromMaxHealth = .25f }, 0, 60 * 2f);
        #endregion

        public override void SetDefaults(Character character)
        {
            // Need a Boss Attack
            character.BossOptions.InBack = false;
            character.BossOptions.DamagingTargs = true;
            character.BossOptions.Attacks.Add(BossHandler.ADefaultMeleeAttack);
        }

        #region Low Traffic Overrides

        private string[] _customChartNames = null;
        /// <summary>
        /// The names of all custom charts provided by the model, if any.
        /// </summary>
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] { };
                }
                return _customChartNames;
            }
        }

        /// <summary>
        /// Gets data to fill a custom chart, based on the chart name, as defined in CustomChartNames.
        /// </summary>
        /// <param name="character">The character to build the chart for.</param>
        /// <param name="chartName">The name of the custom chart to get data for.</param>
        /// <returns>The data for the custom chart.</returns>
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            return new ComparisonCalculationBase[0];
        }
        /// <summary>Deserializes the model's CalculationOptions data object from xml</summary>
        /// <param name="xml">The serialized xml representing the model's CalculationOptions data object.</param>
        /// <returns>The model's CalculationOptions data object.</returns>
        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsTankDK));
            StringReader reader = new StringReader(xml);
            CalculationOptionsTankDK calcOpts = serializer.Deserialize(reader) as CalculationOptionsTankDK;
            return calcOpts;
        }

        private ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel
        {
            get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelTankDK()); }
        }

        private List<ItemType> _relevantItemTypes = null;
        /// <summary>
        /// List<ItemType> containing all of the ItemTypes relevant to this model. Typically this
        /// means all types of armor/weapons that the intended class is able to use, but may also
        /// be trimmed down further if some aren't typically used. ItemType.None should almost
        /// always be included, because that type includes items with no proficiancy requirement, such
        /// as rings, necklaces, cloaks, held in off hand items, etc.
        /// </summary>
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<ItemType>(new ItemType[]
                    {
                        ItemType.None,
                        ItemType.Plate,
                        ItemType.Sigil,
                        ItemType.Relic,
                        ItemType.Polearm,
                        ItemType.TwoHandAxe,
                        ItemType.TwoHandMace,
                        ItemType.TwoHandSword,
                        ItemType.OneHandAxe,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword
                    });
                }
                return _relevantItemTypes;
            }
        }
        /// <summary>
        /// Character class that this model is for.
        /// </summary>
        public override CharacterClass TargetClass { get { return CharacterClass.DeathKnight; } }
        /// <summary>
        /// Method to get a new instance of the model's custom ComparisonCalculation class.
        /// </summary>
        /// <returns>A new instance of the model's custom ComparisonCalculation class, 
        /// which inherits from ComparisonCalculationBase</returns>
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationTankDK(); }
        /// <summary>
        /// Method to get a new instance of the model's custom CharacterCalculations class.
        /// </summary>
        /// <returns>A new instance of the model's custom CharacterCalculations class, 
        /// which inherits from CharacterCalculationsBase</returns>
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsTankDK(); }

        #endregion

        #region SubPointColors
        private Dictionary<string, Color> _subPointNameColors = null;
        private Dictionary<string, Color> _subPointNameColors_SMT = new Dictionary<string, Color>();
        private Dictionary<string, Color> _subPointNameColors_Burst = new Dictionary<string, Color>();

        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    return _subPointNameColors_SMT;
                }
                return _subPointNameColors;
            }
        }

        public CalculationsTankDK()
        {
            _subPointNameColors_SMT.Add("Survival", Color.FromArgb(255, 0, 0, 255));
            _subPointNameColors_SMT.Add("Mitigation", Color.FromArgb(255, 255, 0, 0));
            _subPointNameColors_SMT.Add("Burst", Color.FromArgb(255, 128, 0, 255));
            _subPointNameColors_SMT.Add("Threat", Color.FromArgb(255, 0, 255, 0));

            _subPointNameColors_Burst.Add("Burst Time", Color.FromArgb(255, 0, 0, 255));
            _subPointNameColors_Burst.Add("Reaction Time", Color.FromArgb(255, 255, 0, 0));
            _subPointNameColors_Burst.Add("Threat", Color.FromArgb(255, 0, 255, 0));

            _subPointNameColors = _subPointNameColors_SMT;
        }
        #endregion
    }
}
