using System;

namespace Rawr.DPSWarr {
    public class Skills {
        public Skills(Character character,WarriorTalents talents, Stats stats, CombatFactors combatFactors,WhiteAttacks whiteStats) {
            _character = character;
            _talents = talents;
            _stats = stats;
            _combatFactors = combatFactors;
            _whiteStats = whiteStats;
            _calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
        }
        #region Global Variables
        private Character _character;
        private WarriorTalents _talents;
        private Stats _stats;
        private CombatFactors _combatFactors;
        private WhiteAttacks _whiteStats;
        private CalculationOptionsDPSWarr _calcOpts;
        public const float ROTATION_LENGTH_FURY = 8.0f;
        public const float ROTATION_LENGTH_ARMS_GLYPH = 42.0f;
        public const float ROTATION_LENGTH_ARMS_NOGLYPH = 30.0f;
        #endregion
        #region Base
        // White Damage + White Rage Generated
        public class WhiteAttacks {
            // Constructors
            public WhiteAttacks(WarriorTalents talents, Stats stats, CombatFactors combatFactors, Character character) {
                _talents = talents;
                _stats = stats;
                _combatFactors = combatFactors;
                _character = character;
                //HS_Freq = 0.0f;
            }
            // Variables
            private readonly WarriorTalents _talents;
            private readonly Stats _stats;
            private readonly CombatFactors _combatFactors;
            private readonly Character _character;
            private float HS_FREQ;
            // Get/Set
            public float HS_Freq { get { return HS_FREQ; } set { HS_FREQ = value; } }
            // Functions
            public float CalcMhWhiteDPS() {
                float wepSpeed = _combatFactors.MainHandSpeed;
                if (_combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand &&
                    (_character.OffHand != null && _combatFactors.OffHand.Slot == Item.ItemSlot.TwoHand) &&
                    _talents.TitansGrip != 1f) {
                    wepSpeed += (1.5f - (0.5f * _talents.ImprovedSlam)) / 5f;
                }
                float mhWhiteDPS = MhAvgSwingDmg();
                mhWhiteDPS /= wepSpeed;
                //mhWhiteDPS *= (1 + _combatFactors.MhCrit * _combatFactors.BonusWhiteCritDmg - (1 - _combatFactors.ProbMhWhiteHit) - (_combatFactors.GlanceChance/* - (0.24f * 0.35f)*/)); // ebs: WTF?!?
                //mhWhiteDPS *= _combatFactors.DamageReduction;
                mhWhiteDPS *= (1f - HS_Freq);
                return mhWhiteDPS;
            }
            public float CalcOhWhiteDPS() {
                float ohWhiteDPS = OhAvgSwingDmg();
                ohWhiteDPS /= _combatFactors.OffHandSpeed;
                //ohWhiteDPS *= _combatFactors.DamageReduction;
                if (_combatFactors.OffHand != null && _combatFactors.OffHand.DPS > 0 &&
                    (_combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || _talents.TitansGrip == 1)) {
                    return ohWhiteDPS;
                } else {
                    return 0f;
                }
            }
            public float MhAvgSwingDmg() {
                float mhWhiteSwing = _combatFactors.AvgMhWeaponDmg * _combatFactors.ProbMhWhiteHit;
                mhWhiteSwing += _combatFactors.AvgMhWeaponDmg * _combatFactors.MhCrit * (1+_combatFactors.BonusWhiteCritDmg);
                mhWhiteSwing += _combatFactors.AvgMhWeaponDmg * _combatFactors.GlanceChance * 0.7f;
                
                mhWhiteSwing *= _combatFactors.DamageBonus;
                mhWhiteSwing *= _combatFactors.DamageReduction;
                
                return mhWhiteSwing;
            }
            public float OhAvgSwingDmg() {
                float ohWhiteSwing = _combatFactors.AvgOhWeaponDmg * _combatFactors.ProbOhWhiteHit;
                ohWhiteSwing += _combatFactors.AvgOhWeaponDmg * _combatFactors.OhCrit * (1 + _combatFactors.BonusWhiteCritDmg);
                ohWhiteSwing += _combatFactors.AvgOhWeaponDmg * _combatFactors.GlanceChance * 0.7f;
                
                ohWhiteSwing *= _combatFactors.DamageBonus;
                ohWhiteSwing *= _combatFactors.DamageReduction;

                return ohWhiteSwing;
            }
            // Rage Calcs
            public float GetSwingRage(Item i, bool isMH) {
                // d = damage amt
                // s = weapon speed
                // f = hit factor

                float d, s, f, rage;

                rage = 0.0f;
                s = i.Speed;

                // regular hit
                d = _combatFactors.AvgWeaponDmg(i,isMH) * _combatFactors.DamageReduction;
                f = (isMH ? 3.5f : 1.75f);
                rage += RageFormula(d, s, f) * _combatFactors.ProbWhiteHit(i);

                // crit
                d *= (1f + _combatFactors.BonusWhiteCritDmg);
                f = (isMH ? 7.0f : 3.5f);
                rage += RageFormula(d, s, f) * _combatFactors.CalcCrit(i);

                // glance
                d = d / (1f + _combatFactors.BonusWhiteCritDmg) * 0.75f;
                f = (isMH ? 3.5f : 1.75f);
                rage += RageFormula(d, s, f) * _combatFactors.GlanceChance;

                // UW rage per swing
                rage += (_combatFactors.MainHand.Speed * (3f * _talents.UnbridledWrath) / 60.0f) * (1.0f - _combatFactors.WhiteMissChance);
                return rage;
            }
            // Rage generated per second
            public float whiteRageGenPerSec() {

                float MHRage = (_character.MainHand != null && _character.MainHand.MaxDamage > 0 ? GetSwingRage(_character.MainHand.Item,true) : 0f);
                float OHRage = (_character.OffHand  != null && _character.OffHand.MaxDamage  > 0 ? GetSwingRage(_character.OffHand.Item,false) : 0f);

                // Rage per Second
                MHRage /= _combatFactors.MainHandSpeed;
                OHRage /= _combatFactors.OffHandSpeed;

                float rage = MHRage + OHRage;
                
                return rage;
            }
            public float RageFormula(float d, float s, float f) {
                /* R = Rage Generated
                 * d = damage amount
                 * c = rage conversion value
                 * s = weapon speed
                 * f = hit factor */
                float c = 453.3f;
                if (_character.Level != 80) c = 0.0091107836f * _character.Level * _character.Level + 3.225598133f * _character.Level + 4.2652911f; // = ~320.6;
                float dmgRage = 7.5f * d / c;
                float rps = f * s; // 3.5rage/sec baseline
                float R = System.Math.Min((dmgRage + rps) / 2.0f, dmgRage*2.0f);
                
                //R = 3.75f * d / c + f * s / 2.0f;
                R *= (1.0f + 0.25f * _talents.EndlessRage);
                return R;
            }
        }
        // Ability class base
        public class Ability {
            // Constructors
            /// <summary>
            /// Stores information pertaining to character class abilities
            /// </summary>
            /// <param name="c">The Character to run the calculations against</param>
            /// <param name="s">The Stats to run the calculations against</param>
            /// <param name="cf">The combatFactors to run the calculations against</param>
            /// <param name="wa">The WhiteAttacks to run the calculations against</param>
            /// <param name="name">The name of the ability</param>
            /// <param name="reqmeleeweap">Abilties that require a specific talent will yield 0 dps without that talent</param>
            /// <param name="reqmeleeweap">Abilities that require a melee weapon will provide zero dps when the main hand item is null</param>
            /// <param name="reqmeleerange">This will be used for movement fights, the % of time in movement will drop this abilities dps if it requires melee distance</param>
            /// <param name="maxrange">The maximum distance for the abilities use (in yards). Currently identifying Melee Range as 5 (yards). -1 means distance doesn't affect this ability.</param>
            /// <param name="cd">The cooldown of the ability. Glyphs Affecting cooldowns should have the modifier in the constructor</param>
            /// <param name="ragecost">The amount of rage used by this ability. Glyphs which sometimes/always reduce rage cost should have the modifier in the constructor</param>
            /// <param name="casttime">The cast time required to complete this ability action. Use -1 for instant attacks.</param>
            /// <param name="stancef">The ability can be activated from Fury (Berserker) Stance</param>
            /// <param name="stancea">The ability can be activated from Arms (Battle) Stance</param>
            /// <param name="stanced">The ability can be activated from Defensive Stance</param>
            public Ability(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, string name,
                bool reqtalent, bool reqmeleeweap, bool reqmeleerange, float maxrange, 
                float cd, float ragecost, float casttime, bool stancef, bool stancea, bool stanced)
            {
                // Character related
                Char = c;
                if (Char != null) { Talents = c.WarriorTalents; } else { Talents = null; }
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                if (Char != null) { CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr; } else { CalcOpts = null; }
                // Ability Related
                Name = name;
                ReqTalent = reqtalent;
                ReqMeleeWeap = reqmeleeweap;
                ReqMeleeRange = reqmeleerange;
                MaxRange = maxrange; // In Yards 
                Cd = cd; // In Seconds
                RageCost = ragecost;
                CastTime = casttime; // In Seconds
                StanceOkFury = stancef;
                StanceOkArms = stancea;
                StanceOkDef = stanced;
            }
            public Ability() {
                // Character related
                Char = null;
                Talents = null;
                StatS = null;
                combatFactors = null;
                Whiteattacks = null;
                CalcOpts = null;
                // Ability Related
                Name = "Invalid";
                ReqTalent = false;
                ReqMeleeWeap = false;
                ReqMeleeRange = false;
                MaxRange = 5f; // In Yards 
                Cd = -1f; // In Seconds
                RageCost = -1f;
                CastTime = -1f; // In Seconds
                StanceOkFury = false;
                StanceOkArms = false;
                StanceOkDef = false;
            }
            #region Variables
            private string NAME;
            private float DAMAGEBASE;
            private float DAMAGEBONUS;
            private bool REQTALENT;
            private bool REQMELEEWEAP;
            private bool REQMELEERRANGE;
            private float MAXRANGE; // In Yards 
            private float CD; // In Seconds
            private float RAGECOST;
            private float CASTTIME; // In Seconds
            private bool STANCEOKARMS; // The ability can be used in Battle Stance
            private bool STANCEOKFURY; // The ability can be used in Berserker Stance
            private bool STANCEOKDEF;  // The ability can be used in Defensive Stance
            public float heroicStrikesPerSecond;
            public float heroicStrikePercent;
            private Character CHARACTER;
            private WarriorTalents TALENTS;
            private Stats STATS;
            private CombatFactors COMBATFACTORS;
            private WhiteAttacks WHITEATTACKS;
            private CalculationOptionsDPSWarr CALCOPTS;
            #endregion
            #region Get/Set
            public float bloodsurgeRPS { get; set; }
            public string Name { get { return NAME; } set { NAME = value; } }
            public bool ReqTalent { get { return REQTALENT; } set { REQTALENT = value; } }
            public bool ReqMeleeWeap { get { return REQMELEEWEAP; } set { REQMELEEWEAP = value; } }
            public bool ReqMeleeRange { get { return REQMELEERRANGE; } set { REQMELEERRANGE = value; } }
            public float MaxRange { get { return MAXRANGE; } set { MAXRANGE = value; } } // In Yards 
            public float Cd { get { return CD; } set { CD = value; } } // In Seconds
            public float RageCost { get { return RAGECOST; } set { RAGECOST = value; } }
            public float CastTime { get { return CASTTIME; } set { CASTTIME = value; } } // In Seconds
            public float DamageBase { get { return DAMAGEBASE; } set { DAMAGEBASE = value; } }
            public float DamageBonus { get { return DAMAGEBONUS; } set { DAMAGEBONUS = value; } }
            public bool StanceOkFury { get { return STANCEOKFURY; } set { STANCEOKFURY = value; } }
            public bool StanceOkArms { get { return STANCEOKARMS; } set { STANCEOKARMS = value; } }
            public bool StanceOkDef { get { return STANCEOKDEF; } set { STANCEOKDEF = value; } }
            public Character Char {
                get { return CHARACTER; }
                set {
                    CHARACTER = value;
                    if(CHARACTER != null){
                        Talents = CHARACTER.WarriorTalents;
                        //StatS = CalculationsDPSWarr.GetCharacterStats(CHARACTER, null);
                        combatFactors = new CombatFactors(CHARACTER, StatS);
                        //Whiteattacks = Whiteattacks;
                        CalcOpts = CHARACTER.CalculationOptions as CalculationOptionsDPSWarr;
                    } else {
                        Talents = null;
                        StatS = null;
                        combatFactors = null;
                        Whiteattacks = null;
                        CalcOpts = null;
                    }
                }
            }
            public WarriorTalents Talents { get { return TALENTS; } set { TALENTS = value; } }
            public Stats StatS { get { return STATS; } set { STATS = value; } }
            public CombatFactors combatFactors { get { return COMBATFACTORS; } set { COMBATFACTORS = value; } }
            public WhiteAttacks Whiteattacks { get { return WHITEATTACKS; } set { WHITEATTACKS = value; } }
            public CalculationOptionsDPSWarr CalcOpts { get { return CALCOPTS; } set { CALCOPTS = value; } }
            #endregion
            #region Functions
            public virtual float GetRageUsePerSecond(){return GetActivates() * RageCost / GetRotation();}
            public virtual float GetRotation() {
                if (CalcOpts.FuryStance) {
                    return ROTATION_LENGTH_FURY;
                }else{
                    if (Talents.GlyphOfRending) {
                        return ROTATION_LENGTH_ARMS_GLYPH;
                    } else {
                        return ROTATION_LENGTH_ARMS_NOGLYPH;
                    }
                }
            }
            public virtual bool GetValided() {return GetValided(0);}
            public virtual bool GetValided(int TalentLvl) {
                // Null crap is bad
                if (Char == null || CalcOpts == null || Talents == null) { return false; }
                // Talent Requirements
                if (ReqTalent && TalentLvl == 0) { return false; }
                // Need a weapon
                if (ReqMeleeWeap && (Char.MainHand == null || Char.MainHand.MaxDamage == 0)){return false;}
                // Proper Stance
                if ((CalcOpts.FuryStance && !StanceOkFury)
                    || (!CalcOpts.FuryStance && !StanceOkArms)
                    /*||( CalcOpts.DefStance  && !StanceOkDef )*/ ) { return false; }
                return true;
            }
            public virtual float GetActivates() { return GetActivates(true); } // Number of times used in rotation
            public virtual float GetActivates(bool Override) { return 0f; } // Number of times used in rotation
            public virtual float GetHealing() { return 0f; }
            public virtual float GetDamage() { return GetDamage(false); }
            public virtual float GetDamage(bool Override) { return (float)Math.Max(0f,DamageBase * (1f + DamageBonus)); }
            public virtual float GetAvgDamage() { return GetDamage() * GetActivates(); }
            public virtual float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties
                Damage *= (1f - combatFactors.YellowMissChance - combatFactors.MhDodgeChance
                    + combatFactors.MhYellowCrit * combatFactors.BonusYellowCritDmg); // Attack Table

                if (Damage < 0) { Damage = 0; } // Ensure that we are not doing negative Damage
                return Damage;
            }
            public virtual float GetAvgDamageOnUse() { return GetDamageOnUse() * GetActivates(); }
            public virtual float GetDPS() { return GetAvgDamageOnUse() / GetRotation(); }
            public virtual float GetAvgDamageOnUse(float acts) {
                float dou = GetDamageOnUse();
                return dou * acts;
            }
            public virtual float GetDPS(float acts) {
                float adou = GetAvgDamageOnUse(acts);
                float rot = GetRotation();
                return adou / rot;
            }
            public virtual float GetLandedAtksPerSecNoSS() {
                Ability MS = new MortalStrike(Char, StatS, combatFactors, Whiteattacks);
                Ability OP = new OverPower(Char, StatS, combatFactors, Whiteattacks);
                Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);

                float MS_Acts = MS.GetActivates();
                float OP_Acts = OP.GetActivates();
                float SD_Acts = SD.GetActivates();
                float SL_Acts = GetRotation() / 1.5f - MS.GetActivates() - OP.GetActivates() - SD.GetActivates();

                float Dable = MS_Acts + SD_Acts + SL_Acts;
                float nonDable = OP_Acts;

                float white = (combatFactors.ProbMhWhiteHit+combatFactors.MhCrit+combatFactors.GlanceChance)
                    * (combatFactors.MainHand.Speed/combatFactors.TotalHaste);

                float ProbYellowHit = (1f - combatFactors.WhiteMissChance - combatFactors.MhDodgeChance);
                float ProbYellowHitOP = (1f - combatFactors.WhiteMissChance);

                float result = white + (Dable * ProbYellowHit) + (nonDable * ProbYellowHitOP);

                return result;
            }
            public virtual float GetLandedAtksPerSec() {
                //Ability OP = new OverPower(Char, StatS, combatFactors, Whiteattacks);
                //Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);
                //Ability SL = new Slam(Char, StatS, combatFactors, Whiteattacks);

                //float AH31 = combatFactors.WhiteMissChance;
                //float AH39 = combatFactors.MhDodgeChance;
                //float H56 = combatFactors.TotalHaste;
                //float AH48 = 1.0452f;//Heroic Strike Difference
                //float AB46 = OP.GetActivates();
                //float M61 = 0.9455f;//SuddenDeath Overwrite Chance
                //float AG47 = -0.0452f;//Heroic Strike Freq

                float sspec = 0.00000f; //0.01f * GetLandedAtksPerSecNoSS() * 0f * Talents.SwordSpecialization * (1f - AH31 - AH39) * 54 / 60;

                return GetLandedAtksPerSecNoSS() + sspec;
            }
            public float GetCritHsSlamPerSec() {
                Ability MS = new MortalStrike(Char, StatS, combatFactors, Whiteattacks);
                Ability OP = new OverPower(Char, StatS, combatFactors, Whiteattacks);
                Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);
                Ability HS = new HeroicStrike(Char, StatS, combatFactors, Whiteattacks);

                float MS_Acts = MS.GetActivates();
                float OP_Acts = OP.GetActivates();
                float SD_Acts = SD.GetActivates();
                float HS_Acts = HS.GetActivates();
                float SL_Acts = GetRotation() / 1.5f - MS_Acts - OP_Acts - SD_Acts;

                float result = combatFactors.MhYellowCrit * (SL_Acts/HS.GetRotation() + HS_Acts/HS.GetRotation());

                return result;
            }
            #endregion
            #region Rage Calcs
            public virtual float BloodRageGainRagePerSec() {
                return (20f * (1f + 0.25f * Talents.ImprovedBloodrage)) /
                    (60f * (1f - 1.0f / 9.0f * Talents.IntensifyRage));
            }
            public virtual float AngerManagementRagePerSec() { return Talents.AngerManagement / 3.0f; }
            public virtual float ImprovedBerserkerRagePerSec() { return Talents.ImprovedBerserkerRage * 10 / (30 * (1 - 1.0f / 9.0f * Talents.IntensifyRage)); }
            public virtual float OtherRageGenPerSec() {
                // Ebs: Removed a lot of this due to crit chances already being factored in the WhiteAttacks class
                float rage;// = (14.0f / 8.0f * (1 + combatFactors.MhCrit - (1.0f - combatFactors.ProbMhWhiteHit)));
                //if (combatFactors.OffHand.DPS > 0 && (combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || Talents.TitansGrip == 1))
                //    rage += 7.0f / 8.0f * (1 + combatFactors.OhCrit - (1.0f - combatFactors.ProbOhWhiteHit));
                //rage *= combatFactors.TotalHaste;
                rage /*+*/= AngerManagementRagePerSec() + ImprovedBerserkerRagePerSec() + BloodRageGainRagePerSec();
                
                // 4pcT7
                if (StatS.DreadnaughtBonusRageProc != 0f)
                    rage += 5.0f * 0.1f * ((Talents.DeepWounds > 0f ? 1f : 0f) + (CalcOpts.FuryStance == false ? 1f / 3f : 0f));

                rage *= 1 + Talents.EndlessRage * 0.25f;

                return rage;
            }
            public virtual float neededRagePerSecond(Ability BT, Ability WW, Ability MS, Ability OP, Ability SD,
                                                    Ability SL,/*Ability BS,*/ Ability BLS, Ability SW, Ability RND)
            {
                float BTRage = BT.GetRageUsePerSecond();
                float WWRage = WW.GetRageUsePerSecond();
                float MSRage = MS.GetRageUsePerSecond();
                float OPRage = OP.GetRageUsePerSecond();
                float SDRage = SD.GetRageUsePerSecond();
                float SlamRage = SL.GetRageUsePerSecond();
                float BloodSurgeRage = bloodsurgeRPS;// BS.GetRageUsePerSecond();
                float BladestormRage = BLS.GetRageUsePerSecond();
                float SweepingRage = SW.GetRageUsePerSecond();
                float RendRage = RND.GetRageUsePerSecond();
                // Total
                float rage = BTRage + WWRage + MSRage + OPRage + SDRage + SlamRage + 
                    BloodSurgeRage + SweepingRage + BladestormRage + RendRage;
                return rage;
            }
            public virtual float neededRage() {
                Ability BT = new BloodThirst(Char, StatS, combatFactors, Whiteattacks);
                float BTRage = BT.GetRageUsePerSecond();
                //float BTRage = BloodThirstHits() * 30; // ORIGINAL LINE

                Ability WW = new WhirlWind(Char, StatS, combatFactors, Whiteattacks);
                float WWRage = WW.GetRageUsePerSecond();
                //float WWRage = WhirlWindHits() * 30; // ORIGINAL LINE

                Ability MS = new MortalStrike(Char, StatS, combatFactors, Whiteattacks);
                float MSRage = MS.GetRageUsePerSecond();
                //float MSRage = MortalStrikeHits() * 30; // ORIGINAL LINE

                Ability OP = new OverPower(Char, StatS, combatFactors, Whiteattacks);
                float OPRage = OP.GetRageUsePerSecond();
                //float OPRage = OverpowerHits() * 5; // ORIGINAL LINE

                Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);
                float SDRage = SD.GetRageUsePerSecond();
                //float SDRage = SuddenDeathHits() * 10; // ORIGINAL LINE

                Ability SL = new Slam(Char, StatS, combatFactors, Whiteattacks);
                float SlamRage = SL.GetRageUsePerSecond();
                //float SlamRage = SlamHits() * 15; // ORIGINAL LINE

                Ability BS = new BloodSurge(Char, StatS, combatFactors, Whiteattacks);
                float BloodSurgeRage = BS.GetRageUsePerSecond();
                // NO ORIGINAL LINE

                Ability BLS = new Bladestorm(Char, StatS, combatFactors, Whiteattacks);
                float BladestormRage = BLS.GetRageUsePerSecond();
                //float BladestormRage = BladestormHits() * 30; // ORIGINAL LINE

                Ability SW = new SweepingStrikes(Char, StatS, combatFactors, Whiteattacks);
                float SweepingRage = SW.GetRageUsePerSecond();
                //float SweepingRage = SweepingHits() * (_talents.GlyphOfSweepingStrikes ? 0 : 30); // ORIGINAL LINE

                Ability RND = new Rend(Char, StatS, combatFactors, Whiteattacks);
                float RendRage = RND.GetRageUsePerSecond();
                //float RendRage = RendHits() * 10; // ORIGINAL LINE

                // Total
                float rage = BTRage + WWRage + MSRage + OPRage + SDRage + SlamRage
                    + BloodSurgeRage + SweepingRage + BladestormRage + RendRage;
                return rage;
            }
            public virtual float freeRage(Ability BT, Ability WW, Ability MS, Ability OP, Ability SD,
                                         Ability SL,/*Ability BS,*/ Ability BLS, Ability SW, Ability RND)
            {
                if (Char.MainHand == null) { return 0f; }
                float white = Whiteattacks.whiteRageGenPerSec();
                float other = OtherRageGenPerSec();
                float death = SD.GetRageUsePerSecond();
                float needy = neededRagePerSecond(BT, WW, MS, OP, SD, SL,/*BS,*/ BLS, SW, RND);
                return white + other + death - needy;
            }
            public virtual float heroicStrikeRageCost() { return 0f; }
            public virtual float BloodRageGain() { return (20f + 5f * Talents.ImprovedBloodrage) / (60f * (1f - 0.11f * Talents.IntensifyRage)); }
            public virtual float AngerManagementGain() { return Talents.AngerManagement / 3.0f; }
            public virtual float ImprovedBerserkerRage() { return Talents.ImprovedBerserkerRage * 10f / (30f * (1f - 0.11f * Talents.IntensifyRage)); }
            public virtual float UnbridledWrathGain() { return Talents.UnbridledWrath * 3.0f / 60.0f; }
            public virtual float OtherRage() {
                if (Char.MainHand == null) { return 0f; }
                float rage = (14.0f / 8.0f * (1 + combatFactors.MhCrit - (1.0f - combatFactors.ProbMhWhiteHit)));
                if (combatFactors.OffHand != null && combatFactors.OffHand.DPS > 0 &&
                    (combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || Talents.TitansGrip == 1))
                    rage += 7.0f / 8.0f * (1 + combatFactors.OhCrit - (1.0f - combatFactors.ProbOhWhiteHit));
                rage *= combatFactors.TotalHaste;
                rage += AngerManagementGain() + ImprovedBerserkerRage() + BloodRageGain() + UnbridledWrathGain();
                rage *= 1 + Talents.EndlessRage * 0.25f;

                return rage;
            }
            #endregion
        }
        #endregion

        #region Direct Damage Abilities
        // Fury Abilities
        public class BloodThirst : Ability {
            // Constructors
            /// <summary>
            /// Instantly attack the target causing [AP*50/100] damage. In addition, the next 3 successful melee
            /// attacks will restore 1% health. This effect lasts 8 sec. Damage is based on your attack power.
            /// </summary>
            /// <TalentsAffecting>Bloodthirst (Requires talent), Unending Fury [+(2*Pts)% Damage]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Bloodthirst [+100% from healing effect]</GlyphsAffecting>
            public BloodThirst(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Bloodthirst";
                ReqTalent = true;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Cd = 4f; // In Seconds
                RageCost = 20f - (Talents.FocusedRage * 1f);
                StanceOkFury = true;
                DamageBase = StatS.AttackPower * 50f / 100f;
                DamageBonus = Talents.UnendingFury * 0.02f;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                if (!GetValided(Talents.Bloodthirst)) { return 0f; }
                return 2.0f; // Only have time for 3 in rotation due to clashes in BT and WW cooldown timers
            }
            public override float GetHealing() {
                // ToDo: Bloodthirst healing effect, also adding in GlyphOfBloodthirst (+100% healing)
                return StatS.Health / 100.0f * (Talents.GlyphOfBloodthirst?2f:1f);
            }
            public override float GetDamage() {if (!GetValided(Talents.Bloodthirst)) { return 0f; }return base.GetDamage();}
            public override float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties

                float Crit = 0f;
                // Add additional crit chance for Incite
                // & Range check resulting crit factor
                Crit = (float)Math.Min(1f,combatFactors.MhYellowCrit + StatS.MortalstrikeBloodthirstCritIncrease);

                Damage *= (1f - combatFactors.YellowMissChance - combatFactors.MhDodgeChance
                    + Crit * combatFactors.BonusYellowCritDmg); // Attack Table

                return (float)Math.Max(0f, Damage);
            }
        }
        public class WhirlWind : Ability {
            // Constructors
            /// <summary>
            /// In a whirlwind of steel you attack up to 4 enemies in 8 yards,    
            /// causing weapon damage from both melee weapons to each enemy.
            /// </summary>
            /// <TalentsAffecting>Improved Whirlwind [+(10*Pts)% Damage]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Whirlwind [-2 sec Cooldown]</GlyphsAffecting>
            public WhirlWind(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "WhirlWind";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Cd = 10f; // In Seconds
                RageCost = 25f - (Talents.FocusedRage * 1f);
                StanceOkFury = true;
                DamageBonus = Talents.ImprovedWhirlwind * 0.10f + Talents.UnendingFury * 0.02f;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // Actual Calcs
                //return GetRotation() / (Cd - (Talents.GlyphOfWhirlwind ? 2f : 0f));
                return 1f;
                // ORIGINAL LINE
                //return 1.0f / (10f - (_talents.GlyphOfWhirlwind ? 2f : 0f));
            }
            // Whirlwind while dual wielding executes two separate attacks; assume no offhand in base case
            public override float GetDamage(bool Override) {return GetDamage(Override, false);}
            /// <summary></summary>
            /// <param name="Override">When true, do not check for Bers Stance</param>
            /// <param name="isOffHand">When true, do calculations for off-hand damage instead of main-hand</param>
            /// <returns>Unmitigated damage of a single hit</returns>
            public float GetDamage(bool Override, bool isOffHand) {
                // Invalidators
                if (!GetValided() && !Override) { return 0f; }

                // Base Damage
                float Damage;
                if (isOffHand) {
                    if (this.Char.OffHand != null && this.Char.OffHand.Item != null) {
                        Damage = combatFactors.NormalizedOhWeaponDmg * (0.50f + Talents.DualWieldSpecialization * 0.025f);
                    }else{ Damage = 0f; }
                }else{ Damage = combatFactors.NormalizedMhWeaponDmg; }

                return (float)Math.Max(0f, Damage * (1f + DamageBonus));
            }
            public override float GetDamageOnUse() {
                float DamageMH = GetDamage(); // Base Damage
                DamageMH *= combatFactors.DamageBonus; // Global Damage Bonuses
                DamageMH *= combatFactors.DamageReduction; // Global Damage Penalties
                DamageMH *= (1 - combatFactors.YellowMissChance - combatFactors.MhDodgeChance
                    + combatFactors.MhYellowCrit * combatFactors.BonusYellowCritDmg); // Attack Table
                float DamageOH = GetDamage(false, true);
                DamageOH *= combatFactors.DamageBonus;
                DamageOH *= combatFactors.DamageReduction;
                DamageOH *= (1 - combatFactors.YellowMissChance - combatFactors.OhDodgeChance
                    + combatFactors.OhYellowCrit * combatFactors.BonusYellowCritDmg);

                float Damage = DamageMH + DamageOH;
                if (Damage < 0) { Damage = 0; } // Ensure that we are not doing negative Damage
                return Damage;
            }
        }
        public class BloodSurge : Ability {
            // Constructors
            /// <summary>
            /// Your Heroic Strike, Bloodthirst and Whirlwind hits have a (7%/13%/20%) chance of making
            /// your next Slam instant for 5 sec.
            /// </summary>
            /// <TalentsAffecting>Bloodsurge (Requires Talent) [(7%/13%/20%) chance]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public BloodSurge(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Bloodsurge";
                SL = new Slam(c, s, cf, wa);
                Whirlwind = new WhirlWind(c, s, cf, wa);
                Bloodthirst = new BloodThirst(c, s, cf, wa);
                ReqTalent = true;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Cd = 5f; // In Seconds
                RageCost = 15f - (Talents.FocusedRage * 1f);
                StanceOkFury = true;
                hsActivates = 0.0f;
            }
            // Variables
            public float hsActivates { get; set; }
            private Slam SL;
            private WhirlWind WW;
            private BloodThirst BT;
            // Get/Set
            public Slam Slam { get { return SL; } set { SL = value; } }
            public WhirlWind Whirlwind { get { return WW; } set { WW = value; } }
            public BloodThirst Bloodthirst { get { return BT; } set { BT = value; } }
            // Functions

            // Assumes one slot to slam every 8 seconds: WW/BT/Slam/BT repeat. Not optimal, but easy to do
            private float BasicFuryRotation(float chanceMHhit, float chanceOHhit, float hsActivates, float procChance){
                float chanceWeDontProc = 1f;
                chanceWeDontProc *= (1f - hsActivates * procChance * chanceMHhit);
                chanceWeDontProc *= (1f - Whirlwind.GetActivates() * procChance * chanceMHhit) * (1f - Whirlwind.GetActivates() * procChance * chanceOHhit);
                chanceWeDontProc *= (1f - Bloodthirst.GetActivates() * procChance * chanceMHhit);
                return 1f - chanceWeDontProc;
            }
            
            private float CalcSlamProcs(float chanceMHhit, float chanceOHhit, float hsActivates, float procChance) {
                float hsPercent = (hsActivates) / (GetRotation() / combatFactors.MainHandSpeed);
                float numProcs = 0.0f;
                int whiteTimer = 0;
                int WWtimer = 0;
                int BTtimer = 0;
                const int GCD = 15;
                float chanceWeDontProc = 1f; // temp value that keeps track of what the odds are we got a proc by SLAM time
                int numWW = 0;
                int numBT = 0;
                for (int timeStamp = 0; timeStamp < GetRotation()*10; timeStamp++) {
                    if (whiteTimer <= 0) {
                        chanceWeDontProc *= (1f - hsPercent * procChance * chanceMHhit);
                        whiteTimer = (int)Math.Ceiling(combatFactors.MainHandSpeed*10);
                    }
                    if (timeStamp % GCD == 0) {
                        if (WWtimer <= 0) {
                            chanceWeDontProc *= (1f - procChance*chanceMHhit) * (1f - procChance*chanceOHhit);
                            WWtimer = 80;
                            numWW++;
                        } else if (BTtimer <= 0) {
                            chanceWeDontProc *= (1f - procChance * chanceMHhit);
                            BTtimer = 40;
                            numBT++;
                        } else {
                            // We slam
                            numProcs += (1f - chanceWeDontProc);
                            chanceWeDontProc = 1f;
                        }
                    }
                    whiteTimer--;
                    WWtimer--;
                    BTtimer--;
                }
                return numProcs;
            }
            public override float GetActivates() {
                // Invalidators
                if (!GetValided(Talents.Bloodsurge)) { return 0f; }

                // Actual Calcs
                //Ability HS = new HeroicStrike(Char, StatS, combatFactors, Whiteattacks);
                
                float chance = Talents.Bloodsurge * 0.20f / 3f;
                float chanceMhHitLands = (1f - combatFactors.YellowMissChance - combatFactors.MhDodgeChance);
                float chanceOhHitLands = (1f - combatFactors.YellowMissChance - combatFactors.OhDodgeChance);
                
                float procs2 = CalcSlamProcs(chanceMhHitLands, chanceOhHitLands, hsActivates, chance);
                float procs3 = BasicFuryRotation(chanceMhHitLands, chanceOhHitLands, hsActivates, chance);

                float procs = BT.GetActivates() * chanceMhHitLands + Whirlwind.GetActivates() * chanceMhHitLands + Whirlwind.GetActivates() * chanceOhHitLands
                    + hsActivates*chanceMhHitLands;// HS.GetActivates();
                procs *= chance;
                //procs /= GetRotation();
                // procs = (procs / GetRotation()) - (chance * chance + 0.01f); // WTF is with squaring chance?
                if (procs2 < 0) { procs2 = 0; }
                if (procs2 > 1) { procs2 = 1; } // Only have 1 free GCD in the default rotation
                return procs3;

                // ORIGINAL LINES
                //float chance = _talents.Bloodsurge * 0.0666666666f;
                //float procs = 3 + 4 + ((16 / _combatFactors.MainHandSpeed) * heroicStrikePercent);
                //procs *= chance;
                //procs = (procs / 16) - (chance * chance + 0.01f);
                //if (procs < 0) { procs = 0; }
                //return procs;
            }
            public override float GetDamage() {
                if (!GetValided(Talents.Bloodsurge)) { return 0f; }
                return (float)Math.Max(0f,Slam.GetDamage(true));
            }
        }
        // Arms Abilities
        public class MortalStrike : Ability {
            // Constructors
            /// <summary>
            /// A vicious strike that deals weapon damage plus 380 and wounds the target, reducing
            /// the effectiveness of any healing by 50% for 10 sec.
            /// </summary>
            /// <TalentsAffecting>
            /// Mortal Strike (Requires Talent),
            /// Improved Mortal Strike [+(10-ROUNDUP(10/3*Pts))% damage, -(1/3*Pts) sec cooldown]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Mortal Strike [+10% damage]</GlyphsAffecting>
            public MortalStrike(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Mortal Strike";
                ReqTalent = true;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Cd = 6f; // In Seconds
                RageCost = 30f;
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                DamageBase = combatFactors.NormalizedMhWeaponDmg + 380f;
                DamageBonus = Talents.ImprovedMortalStrike / 3f * 0.1f +
                              (Talents.GlyphOfMortalStrike ? 0.1f : 0f);
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                if (!GetValided(Talents.MortalStrike)) { return 0f; }
                return (float)Math.Max(0f,GetRotation() / (Cd - (Talents.ImprovedMortalStrike / 3.0f)));
            }
            public override float GetDamage() {if (!GetValided(Talents.MortalStrike)) { return 0f; }return base.GetDamage();}
            public override float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties

                float Crit = 0f;
                Crit = combatFactors.MhYellowCrit + StatS.MortalstrikeBloodthirstCritIncrease; // Add additional crit chance for Incite
                if (Crit > 1f) { Crit = 1f; } //Range check resulting crit factor

                Damage *= (1f - combatFactors.YellowMissChance - combatFactors.MhDodgeChance
                    + Crit * combatFactors.BonusYellowCritDmg); // Attack Table

                return (float)Math.Max(0f,Damage);
            }
        }
        public class Suddendeath : Ability {
            // Constructors
            /// <summary>
            /// Your melee hits have a (3*Pts)% chance of allowing the use of Execute regardless of
            /// the target's Health state. This Execute only uses up to 30 total rage. In addition,
            /// you keep at least (3/7/10) rage after using Execute.
            /// </summary>
            /// <TalentsAffecting>Sudden Death (Requires Talent) [(3*Pts)% chance to proc and (3/7/10) rage kept after],
            /// Improved Execute [-(2.5*Pts) rage cost]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Execute [Execute acts as if it had 10 additional rage]</GlyphsAffecting>
            public Suddendeath(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "SuddenDeath";
                Execute = new Execute(c, s, cf, wa);
                RageCost = Execute.RageCost;
                ReqTalent = true;
                ReqMeleeWeap = Execute.ReqMeleeWeap;
                ReqMeleeRange = Execute.ReqMeleeRange;
                Cd = Execute.Cd;
                StanceOkArms = true;
            }
            // Variables
            private Execute EXECUTE;
            // Get/Set
            public Execute Execute { get { return EXECUTE; } set { EXECUTE = value; } }
            // Functions
            public override float GetActivates() { return GetActivates(true); }
            public override float GetActivates(bool Override) {
                // Invalidators
                if (!GetValided(Talents.SuddenDeath)) { return 0f; }

                // ACTUAL CALCS
                float talent = 3f * Talents.SuddenDeath / 100f;
                float hitspersec = (Override ? 1f : GetLandedAtksPerSec());
                float latency = (1.5f);
                //float mod = 100f;
                float SD_GCDS = talent * hitspersec * latency /* * mod*/;
                // END ACTUAL CALCS */
                /*
                float fightDuration = CalcOpts.Duration;
                float hasteBonus = StatConversion.GetPhysicalHasteFromRating(combatFactors.TotalHaste, Character.CharacterClass.Warrior);
                hasteBonus = (1f + hasteBonus) * (1f + combatFactors.TotalHaste) * (1f + StatS.Bloodlust * 40f / fightDuration) - 1f;
                float meleeHitsPerSecond = 1f / 1.5f;
                if (Char.MainHand != null && Char.MainHand.Speed > 0f){ meleeHitsPerSecond += (1f / Char.MainHand.Speed) * (1f + hasteBonus); }
                if (Char.OffHand  != null && Char.OffHand.Speed  > 0f){ meleeHitsPerSecond += (1f / Char.OffHand.Speed ) * (1f + hasteBonus); }
                float meleeHitInterval = 1f / meleeHitsPerSecond;
                float baseWeaponSpeed = Char.MainHand != null ? Char.MainHand.Speed : 3f;

                SpecialEffect SuddenDeath = new SpecialEffect(Trigger.MeleeHit,
                    new Stats() { BonusCritChance = 0f, },
                    1f, 1.5f, Talents.SuddenDeath * (3f / 100f));
                float procs = SuddenDeath.GetAverageProcsPerSecond(meleeHitInterval, 1f, baseWeaponSpeed, CalcOpts.Duration);

                return procs * 1.5f;//*/
                return SD_GCDS;
            }
            public override float GetDamage() {
                if (!GetValided(Talents.SuddenDeath)) { return 0f; }

                float Damage = Execute.GetDamage(true);

                return (float)Math.Max(0f,Damage);
            }
        }
        public class OverPower : Ability {
            // Constructors
            /// <summary>
            /// Instantly overpower the enemy, causing weapon damage plus 125. Only usable after the target dodges.
            /// The Overpower cannot be blocked, dodged or parried.
            /// </summary>
            /// <TalentsAffecting>Improved Overpower [+(25*Pts)% Crit Chance],
            /// Unrelenting Assault [-(2*Pts) sec cooldown, +(10*Pts)% Damage.]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Overpower [Can proc when parried]</GlyphsAffecting>
            public OverPower(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Overpower";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Cd = 5f - (2f * Talents.UnrelentingAssault); // In Seconds
                RageCost = 5f - (Talents.FocusedRage * 1f);
                StanceOkArms = true;
                DamageBase = combatFactors.NormalizedMhWeaponDmg;
                DamageBonus = (0.1f * Talents.UnrelentingAssault);
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // ACTUAL CALCS
                Ability SL = new Slam(Char, StatS, combatFactors, Whiteattacks);
                float GCDPerc = (Talents.TasteForBlood == 0 ? 0f : (1.5f - 0.5f * Talents.UnrelentingAssault / 1000f) / ((Talents.TasteForBlood > 1f) ? 6f : 9f));

                float cd = 1f, result = 0;
                
                if(combatFactors.MhDodgeChance + (Talents.GlyphOfOverpower?combatFactors.MhParryChance:0f) <= 0f && Talents.TasteForBlood == 0f){
                    // No TasteForBlood talent and no chance to activate otherwise
                    cd = 0f;
                }else if(Talents.TasteForBlood == 0f){
                    // No TasteForBlood talent and but chance to activate via parry or dodge
                    cd = 1f / (
                        (combatFactors.MhDodgeChance + (Talents.GlyphOfOverpower ? combatFactors.MhParryChance : 0f)) * (1f / combatFactors.MainHandSpeed) +
                        /*0.01f * GetLandedAtksPerSecNoSS() * combatFactors.MhExpertise * Talents.SwordSpecialization * 54f / 60f +
                        0.03f * GCDPerc * GetLandedAtksPerSec() +*/ // Removed this because it's causing stack overflow (try a fury spec in arms stance)
                        1f / (5f / 1000f)//+
                        //1f / /*AB49 Slam Proc GCD % 0.071227f*/ SL.GetActivates()
                     );
                }// TODO: TasteForBlood talent AND chance to activate otherwise
                else if(Talents.TasteForBlood > 0f){
                    // TasteForBlood talent and NO chance to activate otherwise
                    cd = 6f / (1f / 3f * Talents.TasteForBlood);
                }

                result = GetRotation() / cd;
                // END ACTUAL CALCS

                return result;
            }
            public override float GetDamageOnUse() {
                if (!GetValided()) { return 0f; }
                float Damage = GetDamage(); // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties
                Damage *=
                    (1f - combatFactors.YellowMissChance // cant be dodged
                    + (System.Math.Min(1f-combatFactors.YellowMissChance,(combatFactors.MhYellowCrit + 0.25f * Talents.ImprovedOverpower))
                      * combatFactors.BonusYellowCritDmg)); // Attack Table
                return (float)Math.Max(0f,Damage);
            }
        }
        public class Bladestorm : Ability {
            // Constructors
            /// <summary>
            /// Instantly Whirlwind up to 4 nearby targets and for the next 6 sec you will
            /// perform a whirlwind attack every 1 sec. While under the effects of Bladestorm, you can move but cannot
            /// perform any other abilities but you do not feel pity or remorse or fear and you cannot be stopped
            /// unless killed.
            /// </summary>
            /// <TalentsAffecting>Bladestorm [Requires talent to use Ability]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Bladestorm [Cooldown Reduced by 15 sec]</GlyphsAffecting>
            public Bladestorm(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Bladestorm";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                Cd = 90f; // In Seconds
                RageCost = 25f - (Talents.FocusedRage * 1f);
                CastTime = 6f; // In Seconds // Channeled
                StanceOkFury = true;
                StanceOkArms = true;
                StanceOkDef = true;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided() || Talents.Bladestorm == 0) { return 0f; }

                // Actual Calcs
                return GetRotation() / (Cd - (Talents.GlyphOfBladestorm ? 15f : 0f));
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.Bladestorm == 0) { return 0f; }

                // Base Damage
                Ability WW = new WhirlWind(Char, StatS, combatFactors, Whiteattacks);
                float Damage = WW.GetDamage(true);

                // Talents/Glyphs Affecting

                // Spread this damage over rotaion length (turns it into DPS)
                //Damage /= GetRotation();

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage * 6f; // it WW's 6 times
            }
        }
        public class Swordspec : Ability {
            // Constructors
            /// <summary>
            /// Gives a (1*Pts)% chance to get an extra attack on the same target after hitting
            /// your target with your Sword. This effect cannot occur more than once every 6 seconds.
            /// </summary>
            /// <TalentsAffecting>Sword Specialization (Requires Talent)</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public Swordspec(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Sword Specialization";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                Cd = 6f; // In Seconds
                RageCost = -1f;
                CastTime = -1f; // In Seconds
                StanceOkFury = true;
                StanceOkArms = true;
                StanceOkDef = true;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided() || Talents.SwordSpecialization == 0) { return 0f; }
                
                Ability SL = new Slam(Char, StatS, combatFactors, Whiteattacks);
                Ability MS = new MortalStrike(Char, StatS, combatFactors, Whiteattacks);
                Ability OP = new OverPower(Char, StatS, combatFactors, Whiteattacks);
                Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);
                // Actual Calcs
                if (combatFactors.MainHand.Type != Item.ItemType.TwoHandSword) { return 0.0f; }
                float wepSpeed = combatFactors.MainHandSpeed;
                if (combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand && Talents.TitansGrip != 1) {
                    wepSpeed += (1.5f - (0.5f * Talents.ImprovedSlam)) / 5;
                }
                float whiteHits = (1 / wepSpeed);
                float attacks = 0.01f * Talents.SwordSpecialization;
                attacks *= (MS.GetActivates() + SL.GetActivates() + OP.GetActivates() + SD.GetActivates() + whiteHits);
                return attacks;
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.SwordSpecialization == 0) { return 0f; }

                // Base Damage
                float Damage = combatFactors.AvgMhWeaponDmg;

                // Talents/Glyphs Affecting

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;

                // ORIGINAL LINES
                //float damage = SwordSpecHits() * _combatFactors.AvgMhWeaponDmg;
                //damage *= (1 + _combatFactors.MhCrit * _combatFactors.BonusWhiteCritDmg);
                //return damage * _combatFactors.DamageReduction;
            }
            public override float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties
                Damage *= (1 - combatFactors.YellowMissChance - combatFactors.MhDodgeChance
                    + combatFactors.MhYellowCrit * combatFactors.BonusYellowCritDmg); // Attack Table

                if (Damage < 0) { Damage = 0; } // Ensure that we are not doing negative Damage
                return Damage;
            }
        }
        public class SweepingStrikes : Ability {
            // Constructors
            /// <summary>
            /// Your next 5 melee attacks strike an additional nearby opponent.
            /// </summary>
            /// <TalentsAffecting>Sweeping Strikes [Requires Talent]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Sweeping Strikes [-30 Rage cost]</GlyphsAffecting>
            public SweepingStrikes(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Sweeping Strikes";
                ReqMeleeWeap = false;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                Cd = 30f; // In Seconds
                RageCost = 30f/*-(Talents.GlyphofSweepingStrikes?:30f:0f)*/ - (Talents.FocusedRage * 1f);
                CastTime = -1f; // In Seconds
                StanceOkFury = true;
                StanceOkArms = true;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided() || Talents.SweepingStrikes == 0 || !CalcOpts.MultipleTargets) { return 0f; }

                // Actual Calcs
                return GetRotation() / Cd;
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.SweepingStrikes == 0 || !CalcOpts.MultipleTargets) { return 0f; }

                // Base Damage
                float DamageMod = 2;
                float Damage = 1 * DamageMod;

                // Talents/Glyphs Affecting

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;
            }
            public override float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties
                Damage *= (1 - combatFactors.YellowMissChance - combatFactors.MhDodgeChance
                    + combatFactors.MhYellowCrit * combatFactors.BonusYellowCritDmg); // Attack Table

                if (Damage < 0) { Damage = 0; } // Ensure that we are not doing negative Damage
                return Damage;
            }
        }
        public class ThunderClap : Ability {
            // Constructors
            /// <summary>
            /// Blasts nearby enemies increasing the time between their attacks by 10% for 30 sec
            /// and doing [300+AP*0.12] damage to them. Damage increased by attack power.
            /// This ability causes additional threat.
            /// </summary>
            /// <TalentsAffecting>Improved Thunder Clap [Reduces rage cost by (1/2/4) points, increases the damage by
            /// (10*Pts)% and the slowing effect by an additional (ROUNDUP(10-10/3*Pts))%.]
            /// Incite [+(5*Pts)% Critical Strike chance]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Thunder Clap [+2 yards to radius]</GlyphsAffecting>
            public ThunderClap(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Thunder Clap";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f + (Talents.GlyphOfThunderClap ? 2f : 0f); // In Yards 
                Cd = 6f; // In Seconds
                RageCost = 20f - (Talents.FocusedRage * 1f);
                CastTime = -1f; // In Seconds
                StanceOkFury = false;
                StanceOkArms = true;
                StanceOkDef = true;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetDamage() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // Base Damage
                float Damage = 300 + StatS.AttackPower * 0.12f;

                // Talents/Glyphs Affecting

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;
            }
            public override float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties

                float Crit = combatFactors.MhYellowCrit + (Talents.Incite * 0.05f);

                Damage *= (1f - combatFactors.YellowMissChance - combatFactors.MhDodgeChance
                    + Crit * combatFactors.BonusYellowCritDmg); // Attack Table

                if (Damage < 0) { Damage = 0; } // Ensure that we are not doing negative Damage
                return Damage;
            }
        }
        public class Execute : Ability {
            // Constructors
            /// <summary>
            /// Attempt to finish off a wounded foe, causing (1456+AP*0.2) damage and converting each
            /// extra point of rage into 38 additional damage. Only usable on enemies that have less
            /// than 20% health.
            /// </summary>
            /// <TalentsAffecting>Improved Execute [Reduces the rage cost of your Execute ability by (2.5/5).]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Execute [Your Execute ability acts as if it has 10 additional rage.]</GlyphsAffecting>
            public Execute(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Execute";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                RageCost = 15f - (Talents.ImprovedExecute * 2.5f) - (Talents.FocusedRage * 1f);
                StanceOkFury = StanceOkArms = true;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {if (!GetValided()) { return 0f; }return 0f;}
            public override float GetDamage() { return GetDamage(false); }
            public override float GetDamage(bool Override) {
                if (!Override && !GetValided()) { return 0f; }

                float freerage = (float)System.Math.Max(0f,freeRage(
                    new BloodThirst(Char,StatS,combatFactors,Whiteattacks),
                    new WhirlWind(Char, StatS, combatFactors, Whiteattacks),
                    new MortalStrike(Char, StatS, combatFactors, Whiteattacks),
                    new OverPower(Char, StatS, combatFactors, Whiteattacks),
                    new Suddendeath(Char, StatS, combatFactors, Whiteattacks),
                    new Slam(Char, StatS, combatFactors, Whiteattacks),
                    new Bladestorm(Char, StatS, combatFactors, Whiteattacks),
                    new SweepingStrikes(Char, StatS, combatFactors, Whiteattacks),
                    new Rend(Char, StatS, combatFactors, Whiteattacks)));
                if (Override && freerage <= (RageCost - (Talents.ImprovedExecute * 2.5f))) {
                    freerage = RageCost - (Talents.ImprovedExecute * 2.5f);
                }else if (freerage <= 0f) {
                    return 0.0f; // No Free Rage = 0 damage
                }
                float executeRage = freerage * GetRotation();
                if (Override && executeRage > 30f) { executeRage = 30f; }
                executeRage += (Talents.GlyphOfExecution ? 10.00f : 0.00f);
                executeRage -= RageCost;

                float Damage = 1456f + StatS.AttackPower * 0.2f + executeRage * 38f;

                return (float)Math.Max(0f,Damage);
            }
        }
        public class Slam : Ability {
            // Constructors
            /// <summary>Slams the opponent, causing weapon damage plus 250.</summary>
            /// <TalentsAffecting>Improved Slam [Reduces cast time of your Slam ability by (0.5/1) sec.]</TalentsAffecting>
            /// <SetsAffecting>T7 Deadnaught Battlegear 2 Pc [+10% Damage]</SetsAffecting>
            public Slam(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Slam";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                RageCost = 15f - (Talents.FocusedRage * 1f);
                CastTime = 1.5f - (Talents.ImprovedSlam * 0.5f); // In Seconds
                StanceOkArms = StanceOkDef = true;
                DamageBase = combatFactors.AvgMhWeaponDmg + 250;
                DamageBonus = (Talents.UnendingFury * 0.02f) + (StatS.BonusSlamDamage);
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {if (!GetValided()) { return 0f; }return 0f;}
            public override float GetDamage() { return GetDamage(false); }
            public override float GetDamage(bool Override) {
                if (!Override && !GetValided()) { return 0f; }
                return DamageBase * (1f + DamageBonus);
            }
        }
        // Prot Abilities
        public class ShieldSlam : Ability {
            /// <summary>
            /// Instant, 6 sec cd, 20 Rage, Melee Range, Shields (Any)
            /// Slam the target with your shield, causing 990 to 1040 damage, modified by you shield block
            /// value, and dispels 1 magic effect on the target. Also causes a high amount of threat.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        public class Revenge : Ability {
            /// <summary>
            /// Instant, 1 sec cd, 5 Rage, Melee Range, Melee Weapon (Def)
            /// Instantly counterattack the enemy for 2399 to 2787 damage. Revenge is only usable after the
            /// warrior blocks, dodges or parries an attack.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        public class ConcussionBlow : Ability {
            /// <summary>
            /// Instant, 30 sec cd, 12 Rage, Melee Range, Melee Weapon (Any)
            /// Stuns the opponent for 5 sec and deals 2419 damage (based upon attack power).
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        public class Devastate : Ability {
            /// <summary>
            /// Instant, No Cd, 12 Rage, Melee Range, 1h Melee Weapon (Any)
            /// Sunder the target's armor causing the Sunder Armor effect. In addition, causes 50% of weapon
            /// damage plus 101 for each application of Sunder Armor on the target. The Sunder Armor effect
            /// can stack up to 5 times.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        public class Shockwave : Ability {
            /// <summary>
            /// Instant, 20 sec Cd, 12 Rage, (Any)
            /// Sends a wave of force in front of the warrior, causing 2419 damage (based upon attack power)
            /// and stunning all enemy targets within 10 yards in a frontal cone for 4 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        public class MockingBlow : Ability {
            /// <summary>
            /// Instant, 1 min Cooldown, 10 Rage, Melee Range, Melee Weapon, (Battle/Zerker)
            /// A mocking attack that causes weapon damage, a moderate amount of threat and forces the
            /// target to focus attacks on you for 6 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        // PvP Abilities
        public class Pummel : Ability {
            /// <summary>
            /// Instant, 10 sec Cd, 10 Rage, Melee Range, (Zerker)
            /// Pummel the target, interupting spellcasting and preventing any spell in that school
            /// from being cast for 4 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        // Mixed Abilities
        public class VictoryRush : Ability{
            /// <summary>
            /// Instant, No Cd, No Rage, Melee Range, (Battle, Zerker)
            /// Instant attack the target causing 1424 damage. Can only be used within 25 sec after you
            /// kill an enemy that yields experience or honor. Damage is based on your attack power.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        public class HeroicThrow : Ability {
            /// <summary>
            /// Instant, 1 min Cd, 30 yd, Melee Weapon (Any)
            /// Throws your weapon at the enemy causing 1595 dmg (based upon attack power). This ability
            /// causes high threat.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            ///  - (Talents.FocusedRage * 1f)
        }
        #endregion
        #region OnAttack Abilities
        public class OnAttack : Ability {
            // Constructors
            public OnAttack() { }
            // Variables
            // Get/Set
            // Functions
        };
        public class HeroicStrike : OnAttack {
            // Constructors
            /// <summary>
            /// A strong attack that increases melee damage by 495 and causes a high amount of
            /// threat. Causes 173.25 additional damage against Dazed targets
            /// </summary>
            /// <TalentsAffecting>Improved Heroic Strike [Reduces the rage cost of your Heroic Strike ability by (1/2/3)]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Heroic Strike [You gain 10 rage when you critically strike with your Heroic Strike ability.]</GlyphsAffecting>
            public HeroicStrike(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Heroic Strike";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                Cd = 0f/*(MHWeapon!=null?MHWeaponSpeed:0)*/; // In Seconds
                RageCost = 15f - (Talents.ImprovedHeroicStrike * 1f) - (Talents.FocusedRage * 1f);
                CastTime = 0f; // In Seconds // Replaces a white hit
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                bloodsurgeRPS = 0.0f;
                DamageBase = combatFactors.AvgMhWeaponDmg + 495f;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                if (!GetValided()) { return 0f; }

                // HS per second
                float hsHits = (/*rageModifier +*/ freeRage(
                    new BloodThirst(Char, StatS, combatFactors, Whiteattacks),
                    new WhirlWind(Char, StatS, combatFactors, Whiteattacks),
                    new MortalStrike(Char, StatS, combatFactors, Whiteattacks),
                    new OverPower(Char, StatS, combatFactors, Whiteattacks),
                    new Suddendeath(Char, StatS, combatFactors, Whiteattacks),
                    new Slam(Char, StatS, combatFactors, Whiteattacks),
                    new Bladestorm(Char, StatS, combatFactors, Whiteattacks),
                    new SweepingStrikes(Char, StatS, combatFactors, Whiteattacks),
                    new Rend(Char, StatS, combatFactors, Whiteattacks))
                    / heroicStrikeRageCost());

                if (hsHits < 0) { hsHits = 0; }
                heroicStrikesPerSecond = hsHits;
                
                return hsHits * GetRotation();
            }
            public override float heroicStrikeRageCost() {
                if (!GetValided()) { return 0f; }
                float rageCost = this.RageCost;
                if (Talents.GlyphOfHeroicStrike) { rageCost -= 10.0f * combatFactors.MhCrit; } // Glyph bonus rage on crit
                rageCost += Whiteattacks.GetSwingRage(combatFactors.MainHand, true);
                return rageCost;
            }
            public override float GetDamageOnUse() {
                if (!GetValided()) { return 0f; }
                float Damage = GetDamage(); // Base Damage

                float Crit = 0f;
                Crit = combatFactors.MhYellowCrit + (Talents.Incite * 0.05f); // Add additional crit chance for Incite
                if (Crit > 1f) { Crit = 1f; } //Range check resulting crit factor

                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties
                Damage *= (1 - combatFactors.YellowMissChance - combatFactors.MhDodgeChance
                    + Crit * combatFactors.BonusYellowCritDmg); // Attack Table

                return (float)Math.Max(0f,Damage);
            }
        }
        public class Cleave : OnAttack {
            // Constructors
            /// <summary>
            /// A sweeping attack that does your weapon damage plus 222 to the target and his nearest ally.
            /// </summary>
            /// <TalentsAffecting>Improved Cleave [+(40*Pts)% Damage]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Cleaving [+1 targets hit]</GlyphsAffecting>
            public Cleave(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Cleave";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                RageCost = 20f - (Talents.FocusedRage * 1f);
                //Talents.GlyphOfCleaving;
                CastTime = 0f; // In Seconds // Replaces a white hit
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                bloodsurgeRPS = 0.0f;
                DamageBase = combatFactors.AvgMhWeaponDmg + 222f;
                DamageBonus = Talents.ImprovedCleave * 0.40f;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // HS per second
                float hsHits = (/*rageModifier +*/ freeRage(
                    new BloodThirst(Char, StatS, combatFactors, Whiteattacks),
                    new WhirlWind(Char, StatS, combatFactors, Whiteattacks),
                    new MortalStrike(Char, StatS, combatFactors, Whiteattacks),
                    new OverPower(Char, StatS, combatFactors, Whiteattacks),
                    new Suddendeath(Char, StatS, combatFactors, Whiteattacks),
                    new Slam(Char, StatS, combatFactors, Whiteattacks),
                    new Bladestorm(Char, StatS, combatFactors, Whiteattacks),
                    new SweepingStrikes(Char, StatS, combatFactors, Whiteattacks),
                    new Rend(Char, StatS, combatFactors, Whiteattacks)
                    ) / heroicStrikeRageCost());

                if (hsHits < 0) { hsHits = 0; }
                heroicStrikesPerSecond = hsHits;
                
                return hsHits * GetRotation();
            }
            public override float heroicStrikeRageCost() {
                if (!GetValided()) { return 0f; }
                return RageCost + Whiteattacks.GetSwingRage(combatFactors.MainHand, true);
            }
            public override float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage

                float Crit = 0f;
                Crit = combatFactors.MhYellowCrit + (Talents.Incite * 0.05f); // Add additional crit chance for Incite
                if (Crit > 1f) { Crit = 1f; } //Range check resulting crit factor

                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties
                Damage *= (1 - combatFactors.YellowMissChance - combatFactors.MhDodgeChance
                    + Crit * combatFactors.BonusYellowCritDmg); // Attack Table

                return (float)Math.Max(0f,Damage);
            }
        }
        #endregion
        #region DoT Abilities
        public class DoT : Ability {
            // Constructors
            public DoT(){}
            // Variables
            private float DURATION; // In Seconds
            private float TIMEBTWNTICKS; // In Seconds
            // Get/Set
            public float Duration      { get { return DURATION;      } set { DURATION      = value; } } // In Seconds
            public float TimeBtwnTicks { get { return TIMEBTWNTICKS; } set { TIMEBTWNTICKS = value; } } // In Seconds
            // Functions
            public virtual float GetTickSize() { return 0f; }
            public virtual float GetTTLTickingTime() { return Duration; }
            public virtual float GetTickLength() { return TimeBtwnTicks; }
            public virtual float GetNumTicks() { return GetTTLTickingTime() / GetTickLength(); }
            public virtual float GetDmgOverTickingTime() { return GetTickSize() * GetNumTicks(); }
            public virtual float GetDmgOverTickingTime(float acts) { return GetTickSize() * (GetNumTicks() * acts); }
            public override float GetDPS(float acts) {
                float dmgonuse = GetTickSize();
                float numticks = GetNumTicks()*acts;
                float rot = GetRotation();
                float result = GetDmgOverTickingTime(acts) / rot;
                return result;
            }
            public override float GetDPS()
            {
                return GetTickSize() / GetTickLength();
            }
        }
        public class Rend : DoT {
            // Constructors
            /// <summary>
            /// Wounds the target causing them to bleed for 380 damage plus an additional
            /// (0.2*5*MWB+mwb/2+AP/14*MWS) (based on weapon damage) over 15 sec. If used while your
            /// target is above 75% health, Rend does 35% more damage.
            /// </summary>
            /// <TalentsAffecting>Improved Rend [+(10*Pts)% Bleed Damage], Trauma [+(15*Pts)% Bleed Damage]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Rending [+2 damage ticks]</GlyphsAffecting>
            public Rend(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Rend";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Duration = 15f + (Talents.GlyphOfRending ? 6f : 0f); // In Seconds
                TimeBtwnTicks = 3f; // In Seconds
                RageCost = 10f - (Talents.FocusedRage * 1f);
                StanceOkArms = StanceOkDef = true;
                DamageBase = 380f;
                DamageBonus = 0.10f * Talents.ImprovedRend + 0.15f * Talents.Trauma;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                if (!GetValided()) { return 0f; }
                float RendGCDs = GetRotation() / GetTTLTickingTime();
                return RendGCDs;
            }
            public override float GetTickSize() {
                if (!GetValided()) { return 0f; }

                float DmgBonusBase = ((StatS.AttackPower * combatFactors.MainHand.Speed) / 14f + (combatFactors.MainHand.MaxDamage + combatFactors.MainHand.MinDamage) / 2f) * (743f / 300000f);
                float DmgBonusO75 = 0.25f * 1.35f * DmgBonusBase;
                float DmgBonusU75 = 0.75f * 1.00f * DmgBonusBase;
                float DmgMod = (1f + StatS.BonusBleedDamageMultiplier + DamageBonus);

                float TickSize = (DamageBase + DmgBonusO75 + DmgBonusU75) * DmgMod;
                return TickSize;
            }
        }
        public class DeepWounds : DoT {
            // Constructors
            /// <summary>
            /// Your critical strikes cause the opponent to bleed, dealing (16*Pts)% of your melee weapon's
            /// average damage over 6 sec.
            /// </summary>
            /// <TalentsAffecting>Deep Wounds (Requires Talent) [(16*Pts)% damage dealt]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public DeepWounds(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Deep Wounds";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Duration = 6f; // In Seconds
                TimeBtwnTicks = 1f; // In Seconds
                StanceOkFury = StanceOkArms = StanceOkDef  = true;
                mhActivates = ohActivates = 0;
            }
            // Variables
            private float mhActivates, ohActivates;
            public void SetAllAbilityActivates(float mh, float oh) { 
                mhActivates = mh * combatFactors.MhYellowCrit + GetRotation() / combatFactors.MainHandSpeed * combatFactors.MhCrit; 
                ohActivates = oh * combatFactors.OhYellowCrit + GetRotation() / combatFactors.OffHandSpeed * combatFactors.OhCrit; 
            }
            // Get/Set
            // Functions
            public override float GetActivates() {
                if (!GetValided()) { return 0f; }
                //SetAllAbilityActivates(0,0);
                //return mhActivates + ohActivates;
                float DWsacts = GetRotation() / GetTTLTickingTime();
                return DWsacts;
            }
            public override float GetTickSize() {
                if (!GetValided(Talents.DeepWounds)) { return 0f; }

                // doing it this way because Deep Wounds triggering off of a MH crit and Deep Wounds triggering off of an OH crit do diff damage.
                // GetDamage is doing the average damage of a deep wounds trigger
                float Damage = combatFactors.AvgMhWeaponDmg * (0.16f * Talents.DeepWounds) * mhActivates / (mhActivates + ohActivates) +
                               combatFactors.AvgOhWeaponDmg * (0.16f * Talents.DeepWounds) * ohActivates / (mhActivates + ohActivates);

                Damage *= (1f + StatS.BonusBleedDamageMultiplier);
                Damage *= combatFactors.DamageBonus;
                if (Talents.TitansGrip == 1 && Char.OffHand != null && Char.OffHand.Slot == Item.ItemSlot.TwoHand) { Damage *= 0.9f; } // Titan's Grip penalty, since we're not modifying by combatFactors.DamageReduction

                // Ensure that we are not doing negative Damage
                return (float)Math.Max(0f,Damage);
            }
        }
        #endregion
        #region BuffEffect Abilities
        public class BuffEffect : Ability {
            // Constructors
            public BuffEffect(){}
            // Variables
            private SpecialEffect EFFECT;
            private float DURATION; // In Seconds
            // Get/Set
            public SpecialEffect Effect { get { return EFFECT; } set { EFFECT = value; } }
            public float Duration { get { return DURATION; } set { DURATION = value; } } // In Seconds
            // Functions
        }
        public class Trauma : BuffEffect {
            // Constructors
            /// <summary>
            /// Your melee critical strikes increase the effectiveness of Bleed Effects on the
            /// target by (15*Pts)% for 15 sec.
            /// </summary>
            /// <TalentsAffecting>Trauma (Requires Talent)</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public Trauma(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Trauma";
                ReqMeleeWeap = false;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                Cd = -1f; // In Seconds
                Duration = 15f; // In Seconds
                RageCost = -1f;
                CastTime = -1f; // In Seconds
                StanceOkFury = true;
                StanceOkArms = true;
                StanceOkDef = true;
                Effect = new SpecialEffect(Trigger.MeleeCrit, StatS, Duration, 0f);
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                if (!GetValided() || Talents.Trauma == 0) { return 0f; }
                // Chance to activate on every GCD
                return (GetRotation() / 1.5f) * combatFactors.MhCrit;
            }
        }
        public class BerserkerRage : BuffEffect{
            /// <summary>
            /// Instant, 30 sec Cd, Self, (Any)
            /// The warrior enters a berserker rage, becoming immune to Fear, Sap and Incapacitate effects
            /// and generating extra tage when taking damage. Lasts 10 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        public class EnragedRegeneration : BuffEffect{
            /// <summary>
            /// Instant, 3 min Cd, 15 Rage, Self, (Any)
            /// You regenerate 30% of your total health over 10 sec. This ability requires an Enrage effect,
            /// consumes all Enrage effects and prevents any from affecting you for the full duration.
            /// </summary>
        }
        public class Bloodrage : BuffEffect{
            /// <summary>
            /// Instant, 1 min cd, Self (Any)
            /// Generates 10 rage at the cost of health and then generates an additional 10 rage over 10 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        public class BattleShout : BuffEffect {
            // Constructors
            /// <summary>
            /// The warrior shouts, increasing attack power of all raid and party members within 20 yards by 548. Lasts 2 min.
            /// </summary>
            /// <TalentsAffecting>Booming Voice [+(25*Pts)% AoE and Duration],
            /// Commanding Presence [+(5*Pts)% to the AP Bonus]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Battle [+1 min duration]</GlyphsAffecting>
            public BattleShout(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;Talents = c.WarriorTalents;StatS = s;combatFactors = cf;Whiteattacks = wa;CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                //
                Name = "Battle Shout";
                MaxRange = 20f * (1f + Talents.BoomingVoice * 0.25f); // In Yards 
                Duration = (2f+(Talents.GlyphOfBattle?1f:0f))*60f * (1f + Talents.BoomingVoice * 0.25f);
                RageCost = 10f;
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = (528f*(1f+Talents.CommandingPresence*0.05f)), },
                    Duration, Duration);
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                if (!GetValided()) { return 0f; }
                //Effect.GetAverageProcsPerSecond(...);
                float result = GetRotation() / Duration;
                return result;
            }
        }
        public class ShatteringThrow : BuffEffect {
            // Constructors
            /// <summary>
            /// Throws your weapon at the enemy causing (12+AP*0.50) damage (based on attack power),
            /// reducing the armor on the target by 20% for 10 sec or removing any invulnerabilities.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public ShatteringThrow(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Shattering Throw";
                ReqMeleeWeap = true;
                ReqMeleeRange = false;
                MaxRange = 30f; // In Yards 
                Cd = 2f * 60f; // In Seconds
                Duration = 10f;
                RageCost = 25f;
                CastTime = 1.5f; // In Seconds
                StanceOkFury = false;
                StanceOkArms = true;
                StanceOkDef = false;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.20f, },
                    Duration, Cd);
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                float result = GetRotation() / Cd;

                return result;
            }
        }
        public class DeathWish : BuffEffect {
            // Constructors
            /// <summary>
            /// When activated you become enraged, increasing your physical damage by 20% but increasing
            /// all damage taken by 5%. Lasts 30 sec.
            /// </summary>
            /// <TalentsAffecting>Death Wish [Requires Talent], Intensify Rage [-(1/9*Pts)% Cooldown]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public DeathWish(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Death Wish";
                ReqTalent = true;
                MaxRange = 20f; // In Yards 
                Cd = 3f * 60f * (1f - 1f/9f * Talents.IntensifyRage); // In Seconds
                Duration = 30f;
                RageCost = 10f;
                StanceOkArms = true;
                Effect = new SpecialEffect(Trigger.Use,
                        new Stats() { BonusDamageMultiplier = 0.20f, DamageTakenMultiplier = 0.05f, },
                        Duration, Cd);
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                if (!GetValided(Talents.DeathWish)) { return 0f; }
                //Effect.GetAverageProcsPerSecond(...);
                float result = GetRotation() / Cd;
                return result;
            }
        }
        public class Recklessness : BuffEffect {
            // Constructors
            /// <summary>
            /// Your next 3 special ability attacks have an additional 100% to critically hit
            /// but all damage taken is increased by 20%. Lasts 12 sec.
            /// </summary>
            /// <TalentsAffecting>Booming Voice [Increases the area of effect and duration of your Battle Shout,
            /// Demoralizing Shout and Commanding Shout by (0.25/.050)%]\n
            /// Commanding Presence [Increases the melee attack power bonus of your Battle Shout and health bonus
            /// of your Commanding Shout by (0.05*Pts)%]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Battle [Increases the duration of your Battle Shout ability by 1 min.]</GlyphsAffecting>
            public Recklessness(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Recklessness";
                MaxRange = 5f; // In Yards 
                Cd = 5f * 60f * (1f - 1f/9f*Talents.IntensifyRage); // In Seconds
                Duration = 12f; // In Seconds
                StanceOkFury = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats { PhysicalCrit = 1f, DamageTakenMultiplier = 0.20f, },
                    Duration, Cd);
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                if (!GetValided()) { return 0f; }
                //Effect.GetAverageProcsPerSecond(...);
                float result = GetRotation() / Cd;
                return result;
            }
        }
        #endregion
        #region DeBuff Abilities
        public class Hamstring : Ability{
            /// <summary>
            /// Instant, No cd, 10 Rage, Melee Range, Melee Weapon, (Battle/Zerker)
            /// Maims the enemy, reducing movement speed by 50% for 15 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        #endregion
        #region Anti-Debuff Abilities
        public class EveryManForHimself : Ability {
            /// <summary>
            /// Instant, 2 Min Cooldown, 0 Rage, Self (Any)
            /// Removes all movement impairing effects and all effects which cause loss of control of
            /// your character. This effect shares a cooldown with other similar effects.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        #endregion
        #region Movement Abilities
        public class Charge : Ability{
            /// <summary>
            /// Instant, 20 sec cd, 0 Rage, 8-25 yds, (Battle)
            /// Charge an enemy, generate 25 rage, and stun it for 1.50 sec. Cannot be used in combat.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        public class Intercept : Ability{
            /// <summary>
            /// Instant, 30 sec Cd, 10 Rage, 8-25 yds, (Zerker)
            /// Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        public class Intervene : Ability{
            /// <summary>
            /// Instant, 30 sec Cd, 10 Rage, 8-25 yds, (Def)
            /// Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        #endregion
        #region Other Abilities
        public class Retaliation : Ability {
            /// <summary>
            /// Instant, 5 Min Cd, No Rage, Melee Range, Melee Weapon, (Battle)
            /// Instantly counterattack any enemy that strikes you in melee for 12 sec. Melee attacks
            /// made from behind cannot be counterattacked. A maximum of 20 attacks will cause retaliation.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
        }
        #endregion
    }
}

