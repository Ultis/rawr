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
        public const float ROTATION_LENGTH_FURY = 16.0f;
        public const float ROTATION_LENGTH_ARMS_GLYPH = 42.0f;
        public const float ROTATION_LENGTH_ARMS_NOGLYPH = 30.0f;
        #endregion

        // White Damage + White Rage Generated
        public class WhiteAttacks {
            // Constructors
            public WhiteAttacks(WarriorTalents talents, Stats stats, CombatFactors combatFactors, Character character) {
                _talents = talents;
                _stats = stats;
                _combatFactors = combatFactors;
                _character = character;
                HS_Freq = 0.0f;
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
                if (_combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand && _talents.TitansGrip != 1f) {
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
                if (_combatFactors.OffHand != null && _combatFactors.OffHand.DPS > 0 && (_combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || _talents.TitansGrip == 1)) {
                    return ohWhiteDPS;
                } else {
                    return 0f;
                }
            }
            public float MhAvgSwingDmg()
            {
                float mhWhiteSwing = _combatFactors.AvgMhWeaponDmg * _combatFactors.ProbMhWhiteHit;
                mhWhiteSwing += _combatFactors.AvgMhWeaponDmg * _combatFactors.MhCrit * (1+_combatFactors.BonusWhiteCritDmg);
                mhWhiteSwing += _combatFactors.AvgMhWeaponDmg * _combatFactors.GlanceChance * 0.7f;
                
                mhWhiteSwing *= _combatFactors.DamageBonus;
                mhWhiteSwing *= _combatFactors.DamageReduction;
                
                return mhWhiteSwing;
                
            }
            public float OhAvgSwingDmg()
            {
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
                const float c = 453.3f;//0.0091107836f * 80f * 80f + 3.225598133f * 80f + 4.2652911f; // = ~320.6
                float R = 0f;
                R = 3.75f * d / c + f * s / 2.0f;
                R *= (1.0f + 0.25f * _talents.EndlessRage);
                return R;
            }
        }
        // Abilities
        public class Ability {
            // Constructors
            /// <summary>
            /// Ability: Stores information pertaining to character class abilities
            /// </summary>
            /// <param name="c">The Character to run the calculations against</param>
            /// <param name="s">The Stats to run the calculations against</param>
            /// <param name="cf">The combatFactors to run the calculations against</param>
            /// <param name="wa">The WhiteAttacks to run the calculations against</param>
            /// <param name="name">The name of the ability</param>
            /// <param name="desc">The full description of the ability (and it's ranks)</param>
            /// <param name="reqmeleeweap">Abilities that require a melee weapon will provide zero dps when the main hand item is null</param>
            /// <param name="reqmeleerange">This will be used for movement fights, the % of time in movement will drop this abilities dps if it requires melee distance</param>
            /// <param name="maxrange">The maximum distance for the abilities use (in yards). Currently identifying Melee Range as 5 (yards). -1 means distance doesn't affect this ability.</param>
            /// <param name="tlntsafctg">Talents Affecting: The name of the talent [and it's effect (staged:1/2/3/4/5)]</param>
            /// <param name="glphsafctg">Glyphs Affecting: The name of the glyph [and it's effect]</param>
            /// <param name="setsafctg">Sets Affecting: The name of the set [and it's effect]</param>
            /// <param name="cd">The cooldown of the ability. Glyphs Affecting cooldowns should have the modifier in the constructor</param>
            /// <param name="ragecost">The amount of rage used by this ability. Glyphs which sometimes/always reduce rage cost should have the modifier in the constructor</param>
            /// <param name="casttime">The cast time required to complete this ability action. Use -1 for instant attacks.</param>
            /// <param name="stancef">The ability can be activated from Fury (Berserker) Stance</param>
            /// <param name="stancea">The ability can be activated from Arms (Battle) Stance</param>
            /// <param name="stanced">The ability can be activated from Defensive Stance</param>
            public Ability(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, string name, string desc,
                bool reqmeleeweap, bool reqmeleerange, float maxrange, string tlntsafctg, string glphsafctg, string setsafctg,
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
                Desc = desc;
                ReqMeleeWeap = reqmeleeweap;
                ReqMeleeRange = reqmeleerange;
                MaxRange = maxrange; // In Yards 
                TlntsAfctg = tlntsafctg;
                GlphsAfctg = glphsafctg;
                SetsAfctg = setsafctg;
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
                Desc = "Invalid";
                ReqMeleeWeap = false;
                ReqMeleeRange = false;
                MaxRange = -1; // In Yards 
                TlntsAfctg = "Invalid";
                GlphsAfctg = "Invalid";
                SetsAfctg = "Invalid";
                Cd = -1; // In Seconds
                RageCost = -1;
                CastTime = -1; // In Seconds
                StanceOkFury = false;
                StanceOkArms = false;
                StanceOkDef = false;
            }
            #region Variables
            private string NAME;
            private string DESC;
            private bool REQMELEEWEAP;
            private bool REQMELEERRANGE;
            private float MAXRANGE; // In Yards 
            private string TLNTSAFCTG;
            private string GLPHSAFCTG;
            private string SETSAFCTG;
            private float CD; // In Seconds
            private float RAGECOST;
            private float CASTTIME; // In Seconds
            private bool STANCEOKFURY; // The ability can be used in Fury Stance
            private bool STANCEOKARMS; // The ability can be used in Arms Stance
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
            public string Desc { get { return DESC; } set { DESC = value; } }
            public bool ReqMeleeWeap { get { return REQMELEEWEAP; } set { REQMELEEWEAP = value; } }
            public bool ReqMeleeRange { get { return REQMELEERRANGE; } set { REQMELEERRANGE = value; } }
            public float MaxRange { get { return MAXRANGE; } set { MAXRANGE = value; } } // In Yards 
            public string TlntsAfctg { get { return TLNTSAFCTG; } set { TLNTSAFCTG = value; } }
            public string GlphsAfctg { get { return GLPHSAFCTG; } set { GLPHSAFCTG = value; } }
            public string SetsAfctg { get { return SETSAFCTG; } set { SETSAFCTG = value; } }
            public float Cd { get { return CD; } set { CD = value; } } // In Seconds
            public float RageCost { get { return RAGECOST; } set { RAGECOST = value; } }
            public float CastTime { get { return CASTTIME; } set { CASTTIME = value; } }
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
            public virtual bool  GetValided() {
                // Invalidators
                if (Char == null || CalcOpts == null || Talents == null) { return false; }
                if (ReqMeleeWeap && (Char.MainHand == null || Char.MainHand.MaxDamage == 0)){return false;}
                if ((CalcOpts.FuryStance && !StanceOkFury)
                    || (!CalcOpts.FuryStance && !StanceOkArms)
                    /*||( CalcOpts.DefStance  && !StanceOkDef )*/ ) { return false; }
                return true;
            }
            public virtual float GetActivates() { return GetActivates(true); } // Number of times used in rotation
            public virtual float GetActivates(bool Override) { return 0f; } // Number of times used in rotation
            public virtual float GetHealing() { return 0f; }
            public virtual float GetDamage() { return GetDamage(false); }
            public virtual float GetDamage(bool Override) { return 0f; }
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
                Ability MS = new Mortalstrike(Char, StatS, combatFactors, Whiteattacks);
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
            #endregion
            #region Rage Calcs
            public virtual float BloodRageGainRagePerSec() { return (20 + 5 * Talents.ImprovedBloodrage) / (60 * (1 - 1.0f / 9.0f * Talents.IntensifyRage)); }
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
            public virtual float neededRagePerSecond() {
                Ability BT = new BloodThirst(   Char, StatS, combatFactors, Whiteattacks); float BTRage = BT.GetRageUsePerSecond();
                Ability WW = new WhirlWind(     Char, StatS, combatFactors, Whiteattacks); float WWRage = WW.GetRageUsePerSecond();
                Ability MS = new Mortalstrike(  Char, StatS, combatFactors, Whiteattacks); float MSRage = MS.GetRageUsePerSecond();
                Ability OP = new OverPower(     Char, StatS, combatFactors, Whiteattacks); float OPRage = OP.GetRageUsePerSecond();
                Ability SD = new Suddendeath(   Char, StatS, combatFactors, Whiteattacks); float SDRage = SD.GetRageUsePerSecond();
                Ability SL = new Slam(          Char, StatS, combatFactors, Whiteattacks); float SlamRage = SL.GetRageUsePerSecond();
                /*Ability BS = new BloodSurge(Char, StatS, combatFactors, Whiteattacks);*/ float BloodSurgeRage = bloodsurgeRPS;// BS.GetRageUsePerSecond();
                Ability BLS = new Bladestorm(   Char, StatS, combatFactors, Whiteattacks); float BladestormRage = BLS.GetRageUsePerSecond();
                Ability SW = new SweepingStrikes(Char,StatS, combatFactors, Whiteattacks); float SweepingRage = SW.GetRageUsePerSecond();
                Ability RND = new Rend(         Char, StatS, combatFactors, Whiteattacks); float RendRage = RND.GetRageUsePerSecond();
                // Total
                float rage = BTRage + WWRage + MSRage + OPRage + SDRage + SlamRage + BloodSurgeRage + SweepingRage + BladestormRage + RendRage;
                return rage;
            }
            public virtual float neededRage() {
                Ability BT = new BloodThirst(Char, StatS, combatFactors, Whiteattacks);
                float BTRage = BT.GetRageUsePerSecond();
                //float BTRage = BloodThirstHits() * 30; // ORIGINAL LINE

                Ability WW = new WhirlWind(Char, StatS, combatFactors, Whiteattacks);
                float WWRage = WW.GetRageUsePerSecond();
                //float WWRage = WhirlWindHits() * 30; // ORIGINAL LINE

                Ability MS = new Mortalstrike(Char, StatS, combatFactors, Whiteattacks);
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
            public virtual float freeRage() {
                Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);
                float white = Whiteattacks.whiteRageGenPerSec();
                float other = OtherRageGenPerSec();
                float death = SD.GetRageUsePerSecond();
                float needy = neededRagePerSecond();
                return white + other + death - needy;
            }
            public virtual float heroicStrikeRageCost() { return 0f; }
            public virtual float BloodRageGain() { return (20f + 5f * Talents.ImprovedBloodrage) / (60f * (1f - 0.11f * Talents.IntensifyRage)); }
            public virtual float AngerManagementGain() { return Talents.AngerManagement / 3.0f; }
            public virtual float ImprovedBerserkerRage() { return Talents.ImprovedBerserkerRage * 10f / (30f * (1f - 0.11f * Talents.IntensifyRage)); }
            public virtual float UnbridledWrathGain() { return Talents.UnbridledWrath * 3.0f / 60.0f; }
            public virtual float OtherRage() {
                float rage = (14.0f / 8.0f * (1 + combatFactors.MhCrit - (1.0f - combatFactors.ProbMhWhiteHit)));
                if (combatFactors.OffHand != null && combatFactors.OffHand.DPS > 0 && (combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || Talents.TitansGrip == 1))
                    rage += 7.0f / 8.0f * (1 + combatFactors.OhCrit - (1.0f - combatFactors.ProbOhWhiteHit));
                rage *= combatFactors.TotalHaste;
                rage += AngerManagementGain() + ImprovedBerserkerRage() + BloodRageGain() + UnbridledWrathGain();
                rage *= 1 + Talents.EndlessRage * 0.25f;

                return rage;
            }
            #endregion
        }
        public class DoT : Ability {
            // Constructors
            public DoT(){}
            // Variables
            // Get/Set
            // Functions
            public virtual float GetTickSize() { return 0f; }
            public virtual float GetTTLTickingTime() { return Cd; }
            public virtual float GetTickLength() { return CastTime; }
            public virtual float GetNumTicks() { return GetTTLTickingTime() / GetTickLength(); }
            public virtual float GetDmgOverTickingTime() { return GetTickSize() * GetNumTicks(); }
            public virtual float GetDmgOverTickingTime(float acts) { return GetTickSize() * (GetNumTicks() * acts); }
            public override float GetDPS(float acts) {
                float dmgonuse = GetTickSize();
                float numticks = GetNumTicks()*acts;
                float rot = GetRotation();
                float result = GetDmgOverTickingTime(acts) / rot;
                return result;
                //return GetRotation() / GetDmgOverTickingTime();
            }
        }
        // Fury Abilities
        public class BloodThirst : Ability {
            // Constructors
            public BloodThirst(Character c,Stats s,CombatFactors cf,WhiteAttacks wa){
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Bloodthirst";
                Desc = @"Instantly attack the target causing [AP*50/100] damage. In addition, the next 3 successful melee
attacks will restore 1% health. This effect lasts 8 sec. Damage is based on your attack power";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                TlntsAfctg = @"Bloodthirst (Requires talent to use ability)\n
Unending Fury [Increases the damage done by your Slam, Whirlwind and Bloodthirst abilities by (2/4/6/8/10)%.]";
                GlphsAfctg = @"Glyph of Bloodthirst [+100% from healing effect]";
                Cd = 5f; // In Seconds
                RageCost = 30f;
                CastTime = -1f; // In Seconds
                StanceOkFury = true;
                StanceOkArms = false;
                StanceOkDef = false;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()||Talents.Bloodthirst==0f) { return 0f; }
                return 3.0f; // Only have time for 3 in rotation due to clashes in BT and WW cooldown timers
                // Actual Calcs
                //return GetRotation() / Cd;
                // ORIGINAL LINE
                //return (3.0f / 16) * Talents.Bloodthirst;
            }
            public override float GetHealing() {
                // ToDo: Bloodthirst healing effect, also adding in GlyphOfBloodthirst (+100% healing)
                return StatS.Health / 100.0f * (Talents.GlyphOfBloodthirst?2f:1f);
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided()||Talents.Bloodthirst==0f) { return 0f; }

                // Base Damage
                float Damage = (StatS.AttackPower * 50.0f / 100f);
                
                // Talents Affecting
                Damage *= (1.00f + Talents.UnendingFury * 0.02f);

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

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }
                return Damage;
            }
        }
        public class WhirlWind : Ability {
            // Constructors
            public WhirlWind(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "WhirlWind";
                Desc = @"In a whirlwind of steel you attack up to 4 enemies in 8 yards, causing weapon damage from
both melee weapons to each enemy.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                TlntsAfctg = @"Improved Whirlwind [Damage increased by (10%/20%)]";
                GlphsAfctg = @"Glyph of Whirlwind [Cooldown Reduced by 2 sec.]";
                Cd = 10f; // In Seconds
                RageCost = 25f;
                CastTime = -1f; // In Seconds
                StanceOkFury = true;
                StanceOkArms = false;
                StanceOkDef = false;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // Actual Calcs
                //return GetRotation() / (Cd - (Talents.GlyphOfWhirlwind ? 2f : 0f));
                return 2f;
                // ORIGINAL LINE
                //return 1.0f / (10f - (_talents.GlyphOfWhirlwind ? 2f : 0f));
            }
            // Whirlwind while dual wielding executes two separate attacks; assume no offhand in base case
            public override float GetDamage(bool Override) {return GetDamage(Override, false);}
            /// <summary>
            /// 
            /// </summary>
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
                    } else { Damage = 0f; }
                } else { Damage = combatFactors.NormalizedMhWeaponDmg; }

                // Talents Affecting
                Damage *= (1.00f + Talents.ImprovedWhirlwind * 0.10f);
                if (!Override) { Damage *= (1.00f + Talents.UnendingFury * 0.02f); }

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;
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
            public BloodSurge(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "BloodSurge";
                Desc = @"Your Heroic Strike, Bloodthirst and Whirlwind hits have a (7%/13%/20%) chance of making
your next Slam instant for 5 sec.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Bloodsurge (Requires Talent to use ability) [(7%/13%/20%) chance]";
                GlphsAfctg = @"";
                Cd = 5; // In Seconds
                RageCost = 15;
                CastTime = -1; // In Seconds
                StanceOkFury = true;
                StanceOkArms = false;
                StanceOkDef  = false;
                hsActivates = 0.0f;
            }
            // Variables
            public float hsActivates { get; set; }
            // Get/Set
            // Functions
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
                        whiteTimer = (int)System.Math.Ceiling(combatFactors.MainHandSpeed*10);
                    }
                    if (timeStamp % GCD == 0) {
                        if (WWtimer <= 0) {
                            chanceWeDontProc *= (1f - procChance*chanceMHhit) * (1f - procChance*chanceOHhit);
                            WWtimer = 80;
                            numWW++;
                        } else if (BTtimer <= 0) {
                            chanceWeDontProc *= (1f - procChance * chanceMHhit);
                            BTtimer = 50;
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
                if (!GetValided() || Talents.Bloodsurge == 0) { return 0f; }

                // Actual Calcs
                Ability BT = new BloodThirst(Char, StatS, combatFactors, Whiteattacks);
                Ability WW = new WhirlWind(Char, StatS, combatFactors, Whiteattacks);
                //Ability HS = new HeroicStrike(Char, StatS, combatFactors, Whiteattacks);
                
                float chance = Talents.Bloodsurge * 0.20f / 3f;
                float chanceMhHitLands = (1f - combatFactors.YellowMissChance - combatFactors.MhDodgeChance);
                float chanceOhHitLands = (1f - combatFactors.YellowMissChance - combatFactors.OhDodgeChance);
                
                float procs2 = CalcSlamProcs(chanceMhHitLands, chanceOhHitLands, hsActivates, chance);

                float procs = BT.GetActivates()*chanceMhHitLands + WW.GetActivates()*chanceMhHitLands + WW.GetActivates()*chanceOhHitLands
                    + hsActivates*chanceMhHitLands;// HS.GetActivates();
                procs *= chance;
                //procs /= GetRotation();
                // procs = (procs / GetRotation()) - (chance * chance + 0.01f); // WTF is with squaring chance?
                if (procs < 0) { procs = 0; }
                if (procs > 5) { procs = 5; } // Only have 5 free GCDs in the rotation
                return procs2;

                // ORIGINAL LINES
                //float chance = _talents.Bloodsurge * 0.0666666666f;
                //float procs = 3 + 4 + ((16 / _combatFactors.MainHandSpeed) * heroicStrikePercent);
                //procs *= chance;
                //procs = (procs / 16) - (chance * chance + 0.01f);
                //if (procs < 0) { procs = 0; }
                //return procs;
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.Bloodsurge == 0) { return 0f; }

                // Base Damage
                Ability SL = new Slam(Char, StatS, combatFactors, Whiteattacks);
                float Damage = SL.GetDamage(true);
                //Damage *= (combatFactors.AvgMhWeaponDmg + combatFactors.DamageBonus * 250f);

                // Talents Affecting
                //Damage *= (1f + StatS.BonusSlamDamage);
                //Damage *= (1f + Talents.UnendingFury * 0.02f);

                // Spread this damage over rotaion length (turns it into DPS)
                //Damage *= GetRotation();

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;

                // ORIGINAL LINES
                //float slamDamage = BloodsurgeProcs() / (1 - _combatFactors.MhDodgeChance - _combatFactors.YellowMissChance);
                //slamDamage *= (1 - _combatFactors.MhDodgeChance - _combatFactors.YellowMissChance + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg);
                //slamDamage *= _combatFactors.DamageReduction * (1 + _stats.BonusSlamDamage) * (1 + 0.02f * _talents.UnendingFury) * (_combatFactors.AvgMhWeaponDmg + (_combatFactors.DamageBonus * 250));
                //if (slamDamage < 0) { slamDamage = 0; }
                //return slamDamage;
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
        // Arms Abilities
        public class Mortalstrike : Ability {
            // Constructors
            public Mortalstrike(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Mortal Strike";
                Desc = @"A vicious strike that deals weapon damage plus 380 and wounds the target, reducing
the effectiveness of any healing by 50% for 10 sec.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                TlntsAfctg = @"Mortal Strike (Requires Talent to use Ability)\n
Improved Mortal Strike [Increases the dmage caused by your Mortal Strike ability by (3%/6%/10%) and reduces
the cooldown by (0.333/.666/1) sec.]";
                GlphsAfctg = @"Glyph of Mortal Strike [+10% to this ability's Damage]";
                Cd = 6f; // In Seconds
                RageCost = 30f;
                CastTime = -1f; // In Seconds
                StanceOkFury = true;
                StanceOkArms = true;
                StanceOkDef  = true;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided() || Talents.MortalStrike == 0) { return 0f; }

                // Actual Calcs
                return GetRotation() / (Cd - (Talents.ImprovedMortalStrike / 3.0f));
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.MortalStrike == 0) { return 0f; }

                // Base Damage
                float Damage = combatFactors.NormalizedMhWeaponDmg + 380;

                // Talents/Glyphs Affecting
                Damage *= (1f + Talents.ImprovedMortalStrike / 3f * 0.10f);
                Damage *= (1f + (Talents.GlyphOfMortalStrike ? 0.10f : 0f));

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;
            }
        }
        public class Rend : DoT {
            // Constructors
            public Rend(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Rend";
                Desc = @"Wounds the target causing them to bleed for 380 damage plus an additional (0.2*5*MWB+mwb/2+AP/14*MWS)
(based on weapon damage) over 15 sec. If used while your target is above 75% health, Rend does 35% more damage.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                TlntsAfctg = @"Improved Rend [Increases the Bleed Damage done by your Rend ability by (10/20)%]\n
Trauma [Your melee critical strikes increase the effectiveness of Bleed effects on the target by (15/30)% for 15 sec.]";
                GlphsAfctg = @"Glyph of Rending [+6 sec to bleed time (damage is added to, not spread further)]";
                Cd = 15f; // In Seconds // adding cd time to sim not using it again until it falls off
                RageCost = 10f;
                CastTime = 3f; // In Seconds // adding cast time to sim the ticks
                StanceOkFury = false;
                StanceOkArms = true;
                StanceOkDef = true;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }
                float RendGCDs = GetRotation() / GetTTLTickingTime();
                return RendGCDs;
            }
            public override float GetTickSize() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                float DmgBase = 380f;
                float DmgBonusO75 = 0.25f*1.35f*((StatS.AttackPower * combatFactors.MainHand.Speed) / 14f + (combatFactors.MainHand.MaxDamage+combatFactors.MainHand.MinDamage)/2f) * (743f / 300000f);
                float DmgBonusU75 = 0.75f*1.00f*((StatS.AttackPower * combatFactors.MainHand.Speed) / 14f + (combatFactors.MainHand.MaxDamage + combatFactors.MainHand.MinDamage) / 2f) * (743f / 300000f);
                float DmgMod = (1f + StatS.BonusBleedDamageMultiplier);
                DmgMod *= 1f + 0.1f * Talents.ImprovedRend;

                float TickSize = (DmgBase + DmgBonusO75 + DmgBonusU75) * DmgMod;
                return TickSize;
            }
            public override float GetTTLTickingTime() { return (Cd + (Talents.GlyphOfRending ? 6f : 0f)); }
        }
        public class Suddendeath : Ability {
            // Constructors
            public Suddendeath(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "SuddenDeath";
                Desc = @"Your melee hits have a (3/6/9)% chance of allowing the use of Execute regardless of
the target's Health state. This Execute only uses up to 30 total rage. In addition, you keep at least
(3/7/10) rage after using Execute.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                TlntsAfctg = @"Sudden Death (Requires talent to use ability) (Talent modifies Chance to proc and rage kept after)\n
Improved Execute [Reduces the rage cost of your Execute ability by (2.5/5)]";
                GlphsAfctg = @"Glyph of Execute [Ability acts as if it had 10 additional rage]";
                Cd = 6f; // In Seconds
                RageCost = 15f; // 30 is the max it will use, glyph will take this to 40, cast cost will drop from 15 to 10 with max ImpExec
                CastTime = -1f; // In Seconds
                StanceOkFury = true;
                StanceOkArms = true;
                StanceOkDef = false;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() { return GetActivates(true); }
            public override float GetActivates(bool Override) {
                // Invalidators
                if (!GetValided() || Talents.SuddenDeath == 0) { return 0f; }

                // ACTUAL CALCS
                float talent = 3f * Talents.SuddenDeath / 100f;
                float hitspersec = (Override ? 1f : GetLandedAtksPerSec());
                float latency = (1.5f);
                //float mod = 100f;
                float SD_GCDS = talent * hitspersec * latency /* * mod*/;
                // END ACTUAL CALCS */

                /*float fightDuration = CalcOpts.Duration;
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

                return procs * 1.5f;*/
                return SD_GCDS;
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.SuddenDeath == 0) { return 0f; }

                Ability Ex = new Execute(Char, StatS, combatFactors, Whiteattacks);
                float Damage = Ex.GetDamage(true);

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;
            }
        }
        public class OverPower : Ability {
            // Constructors
            public OverPower(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Overpower";
                Desc = @"Instantly overpower the enemy, causing weapon damage plus 125. Only usable after the target dodges.
The Overpower cannot be blocked, dodged or parried.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                TlntsAfctg = @"Improved Overpower [Increases the critical strike chance of your overpower ability by (25%/50%)]\n
Unrelenting Assault [Reduces the cooldown of your Overpower and Revenge abilities by (2/4) sec and
increases the damage done by both abilities by (10/20)%. In addition, if you strike a player with Overpower
while they are casting, their magical damage and healing will be reduced by (25/50)% for 6 sec.]";
                GlphsAfctg = @"Glyph of Overpower [+100% chance to enable your Overpower when your attacks are parried.]";
                Cd = 5f; // In Seconds
                RageCost = 5f;
                CastTime = -1f; // In Seconds
                StanceOkFury = false;
                StanceOkArms = true;
                StanceOkDef = false;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // Actual Calcs
                {
                    // TODO: 5 sec cooldown - (_talents.UnrelentingAssault*2.0f)
                }
                {
                    // TODO: GlyphofOverpower "100% chance to activate when attacks are parried"
                    // thereby making it so that it will activate on both Dodges and Parries instead
                    // of Dodges alone, which are pretty much invalidated by the Expertise mechanic
                }
                // ACTUAL CALCS
                Ability SL = new Slam(Char, StatS, combatFactors, Whiteattacks);
                float GCDPerc = (Talents.TasteForBlood == 0 ? 0f : (1.5f - 0.5f * Talents.UnrelentingAssault / 1000f) / ((Talents.TasteForBlood > 1f) ? 6f : 9f));

                float OP_GCDs = GetRotation() / 
                    (((combatFactors.MhDodgeChance <= 0f /*&& (Talents.GlyphOfOverpower && combatFactors.MhParryChance <= 0f)*/) && Talents.TasteForBlood == 0f) ?
                        0f
                    :
                        ((Talents.TasteForBlood == 0f) ? 1f /
                            (
                                (combatFactors.MhDodgeChance /*+ (Talents.GlyphOfOverpower ? combatFactors.MhParryChance : 0f)*/) * (1f / combatFactors.MainHandSpeed) +
                                0.01f * GetLandedAtksPerSecNoSS() * combatFactors.MhExpertise * Talents.SwordSpecialization * 54f / 60f +
                                0.03f * GCDPerc * GetLandedAtksPerSec() +
                                1f / (5f / 1000f)//+
                                //1f / /*AB49 Slam Proc GCD % 0.071227f*/ SL.GetActivates()
                             )
                        :
                        ((Talents.TasteForBlood > 1f) ? 6f : 9f ) ))
                ;
                // END ACTUAL CALCS

                return OP_GCDs;
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                return (combatFactors.NormalizedMhWeaponDmg) * (1f + 0.1f * Talents.UnrelentingAssault);
            }
            public override float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties
                Damage *=
                    (1f - combatFactors.YellowMissChance // cant be dodged
                    + (System.Math.Min(1f-combatFactors.YellowMissChance,(combatFactors.MhYellowCrit + 0.25f * Talents.ImprovedOverpower))
                      * combatFactors.BonusYellowCritDmg)); // Attack Table

                if (Damage < 0) { Damage = 0; } // Ensure that we are not doing negative Damage
                return Damage;
            }
        }
        public class Bladestorm : Ability {
            // Constructors
            public Bladestorm(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Bladestorm";
                Desc = @"Instantly Whirlwind up to 4 nearby targets and for the next 6 sec you will
perform a whirlwind attack every 1 sec. While under the effects of Bladestorm, you can move but cannot
perform any other abilities but you do not feel pity or remorse or fear and you cannot be stopped
unless killed.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                TlntsAfctg = @"Bladestorm [Requires talent to use Ability]";
                GlphsAfctg = @"Glyph of Bladestorm [Cooldown Reduced by 15 sec]";
                Cd = 90f; // In Seconds
                RageCost = 25f;
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
            public Swordspec(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Sword Specialization";
                Desc = @"Gives a (1/2/3/4/5)% chance to get an extra attack on the same target after hitting
your target with your Sword. This effect cannot occur more than once every 6 seconds.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Sword Specialization (Requires talent to use Ability)";
                GlphsAfctg = @"";
                Cd = 6f; // In Seconds
                RageCost = -1;
                CastTime = -1; // In Seconds
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
                Ability MS = new Mortalstrike(Char, StatS, combatFactors, Whiteattacks);
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
            public SweepingStrikes(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Sweeping Strikes";
                Desc = @"Your next 5 melee attacks strike an additional nearby opponent.";
                ReqMeleeWeap = false;
                ReqMeleeRange = true;
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Sweeping Strikes [Requires talent to use ability]";
                GlphsAfctg = @"Glyph of Mortal Strike [+10% to this ability's Damage]";
                Cd = 30f; // In Seconds
                RageCost = 30f/*-(Talents.GlyphofSweepingStrikes?:30f:0f)*/;
                CastTime = -1; // In Seconds
                StanceOkFury = true;
                StanceOkArms = true;
                StanceOkDef = false;
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
        public class Trauma : Ability {
            // Constructors
            public Trauma(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Trauma";
                Desc = @"Your melee critical strikes increase the effectiveness of Bleed Effects on the
target by (15/30)% for 15 sec.";
                ReqMeleeWeap = false;
                ReqMeleeRange = true;
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Trauma [Requires Talent to use ability]";
                GlphsAfctg = @"";
                Cd = -1; // In Seconds
                RageCost = -1;
                CastTime = -1; // In Seconds
                StanceOkFury = true;
                StanceOkArms = true;
                StanceOkDef = true;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided() || Talents.Trauma == 0) { return 0f; }

                // Actual Calcs
                return combatFactors.MhCrit;
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // Base Damage
                float Damage = Talents.Trauma * 0.15f;

                // Talents Affecting

                // Spread this damage over rotation length (turns it into DPS)

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;
            }
            public override float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage

                if (Damage < 0) { Damage = 0; } // Ensure that we are not doing negative Damage
                return Damage;
            }
        }
        // Mixed Abilities
        public class Execute : Ability {
            // Constructors
            public Execute(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Execute";
                Desc = @"Attempt to finish off a wounded foe, causing (1456+AP*0.2) damage and converting each
extra point of rage into 38 additional damage. Only usable on enemies that have less than 20% health.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                TlntsAfctg = @"Improved Execute [Reduces the rage cost of your Execute ability by (2.5/5).]";
                GlphsAfctg = @"Glyph of Execute [Your Execute ability acts as if it has 10 additional rage.]";
                Cd = -1f; // In Seconds
                RageCost = 15f;
                CastTime = -1f; // In Seconds
                StanceOkFury = true;
                StanceOkArms = true;
                StanceOkDef = false;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                return 0f;
            }
            public override float GetDamage() { return GetDamage(false); }
            public override float GetDamage(bool Override) {
                // Invalidators
                if (!Override && !GetValided()) { return 0f; }

                float freerage = freeRage();
                if (Override && freerage <= (RageCost - (Talents.ImprovedExecute * 0.25f))) {
                    freerage = RageCost - (Talents.ImprovedExecute * 0.25f);
                }else if (freerage <= 0f) {
                    return 0.0f; // No Free Rage = 0 damage
                }
                float executeRage = freerage * GetRotation();
                executeRage -= (Cd - 2.5f * Talents.ImprovedExecute);
                if (Override && executeRage > 30f) { executeRage = 30f; }
                executeRage += (Talents.GlyphOfExecution ? 10.00f : 0.00f);

                float Damage = 1456f + StatS.AttackPower * 0.2f + executeRage * 38f;

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;
            }
        }
        public class Slam : Ability {
            // Constructors
            public Slam(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Slam";
                Desc = @"Slams the opponent, causing weapon damage plus 250.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                TlntsAfctg = @"Improved Slam [Reduces cast time of your Slam ability by (0.5/1) sec.]";
                GlphsAfctg = @"";
                SetsAfctg = @"Deadnaught Battlegear 2 Pc [+10% Slam Damage]";
                Cd = -1f; // In Seconds
                RageCost = 15f;
                CastTime = 1.5f; // In Seconds
                StanceOkFury = false;
                StanceOkArms = true;
                StanceOkDef = true;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                //Ability MS = new Mortalstrike(Char,StatS,combatFactors,Whiteattacks);
                //Ability OP = new OverPower(Char,StatS,combatFactors,Whiteattacks);
                //Ability SD = new Suddendeath(Char,StatS,combatFactors,Whiteattacks);
                //Ability RND = new Rend(Char,StatS,combatFactors,Whiteattacks);
                //float MS_A = MS.GetActivates();
                //float RND_A = RND.GetActivates();
                //float OP_A = OP.GetActivates();
                //float SD_A = SD.GetActivates();
                //float GCDPerc = (float)System.Math.Max(0f, GetRotation() / 1.5f - MS_A - OP_A - SD_A - RND_A);
                //float result = (float)System.Math.Floor(GCDPerc);

                return 0f;
                //return result;
            }
            public override float GetDamage() { return GetDamage(false); }
            public override float GetDamage(bool Override) {
                // Invalidators
                if (!Override && !GetValided()) { return 0f; }

                float bonus =
                    (1f +
                    StatS.BonusSlamDamage +// 2 Pc T7 Set bonus
                    (!Override?0.02f*Talents.UnendingFury:0f));// Talents
                float dmg = bonus * (combatFactors.AvgMhWeaponDmg + 250);
                return dmg;
            }
        }
        public class HeroicStrike : Ability {
            // Constructors
            public HeroicStrike(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Heroic Strike";
                Desc = @"A strong attack that increases melee damage by 495 and causes a high amount of threat.
Causes 173.25 additional damage against Dazed targets";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Improved Heroic Strike [Reduces the rage cost of your Heroic Strike ability by (1/2/3)]";
                GlphsAfctg = @"Glyph of Heroic Strike [You gain 10 rage when you critically strike with your Heroic Strike ability.]";
                Cd = 0/*(MHWeapon!=null?MHWeaponSpeed:0)*/; // In Seconds
                RageCost = 15/*-(_talents.ImprovedHeroicStrike*1f)*/;
                CastTime = 0; // In Seconds // Replaces a white hit
                StanceOkFury = true;
                StanceOkArms = false;
                StanceOkDef = true;
                bloodsurgeRPS = 0.0f;
            }
            // Variables
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // HS per second
                float hsHits = (/*rageModifier +*/ freeRage() / heroicStrikeRageCost());

                if (hsHits < 0) { hsHits = 0; }
                heroicStrikesPerSecond = hsHits;
                
                return hsHits * GetRotation();

                // ORIGINAL LINE
                //return (_talents.ImprovedSlam == 2 ? (1.5f /*- (0.5f * _talents.ImprovedSlam)*/ / 5) : 0);
            }
            public override float heroicStrikeRageCost() {
                if (!GetValided()) { return 0f; }
                float rageCost = this.RageCost;
                rageCost -= Talents.ImprovedHeroicStrike; // Imp HS
                if (Talents.GlyphOfHeroicStrike) rageCost -= 10.0f * combatFactors.MhCrit; // Glyph bonus rage on crit
                rageCost += Whiteattacks.GetSwingRage(combatFactors.MainHand, true);
                return rageCost;
                // Ebs: Removing this and replacing with the WhiteAttacks call
                /*
                //MHAverageDamage*ArmorDeal*15/4/cVal*(1+mhCritBonus*mhCrit-glanceChance*glanceReduc-whiteMiss-dodgeMH)+7/2*(1+mhCrit-whiteMiss-whiteDodge)
                float rage = combatFactors.AvgMhWeaponDmg * combatFactors.DamageReduction * 15.0f / 4 / 320.6f;
                rage *= (1.0f + combatFactors.MhCrit * combatFactors.BonusWhiteCritDmg
                                - (1.0f - combatFactors.ProbMhWhiteHit) - (0.25f * 0.35f));
                rage += 7.0f / 2.0f * (1 + combatFactors.MhCrit - (1.0f - combatFactors.ProbMhWhiteHit));
                //int modNumber = 0; if (_talents.GlyphOfHeroicStrike) { modNumber = 10; }
                //rage += 1.0f * (1 + _combatFactors.MhCrit - (1.0f - _combatFactors.ProbMhWhiteHit)) * modNumber;

                return rage; */
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // Base Damage
                // Ebs: removed this, the ability only deals 495 base damage (since it replaces a white attack)
                //heroicStrikePercent = combatFactors.MainHandSpeed /* * HeroicStrikeHits(0)*/;
                /*if (heroicStrikePercent > 1) { heroicStrikePercent = 1; }
                float damageIncrease = heroicStrikePercent * (495 + combatFactors.AvgMhWeaponDmg * (0.25f * 0.35f));
                */
                float damageIncrease = combatFactors.AvgMhWeaponDmg + 495f;
                // Talents Affecting

                // Spread this damage over rotation length (turns it into DPS)
                //damageIncrease /= GetRotation();

                // Ensure that we are not doing negative Damage
                if (damageIncrease < 0) { damageIncrease = 0; }

                return damageIncrease;

                // ORIGINAL LINES
                //heroicStrikePercent = _combatFactors.MainHandSpeed * HeroicStrikeHits(0);
                //if (heroicStrikePercent > 1) { heroicStrikePercent = 1; }
                //float damageIncrease = heroicStrikePercent * _combatFactors.DamageReduction * ((_combatFactors.DamageBonus * 495)
                //                       + _combatFactors.DamageReduction * _combatFactors.AvgMhWeaponDmg * (((_combatFactors.MhYellowCrit) - (_combatFactors.MhCrit)) *
                //                       (1 + (_combatFactors.BonusYellowCritDmg - _combatFactors.BonusWhiteCritDmg)) + (_combatFactors.WhiteMissChance - _combatFactors.YellowMissChance) + (0.25f * 0.35f)));
                //if (damageIncrease < 0) { damageIncrease = 0; }
                //return damageIncrease;
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
        public class DeepWounds : Ability {
            // Constructors
            public DeepWounds(Character c, Stats s, CombatFactors cf,WhiteAttacks wa) {
                Char = c;
                Talents = c.WarriorTalents;
                StatS = s;
                combatFactors = cf;
                Whiteattacks = wa;
                CalcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                Name = "Deep Wounds";
                Desc = @"Your critical strikes cause the opponent to bleed, dealing (16/32/48)% of your melee weapon's
average damage over 6 sec.";
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                MaxRange = 5f; // In Yards 
                TlntsAfctg = @"Deep Wounds (Requires talent to use Ability) [(16/32/48)% damage dealt]";
                GlphsAfctg = @"";
                Cd = 6f; // In Seconds // 6 seconds to sim falloff
                RageCost = -1f;
                CastTime = 1f; // In Seconds // Simulating Tick time
                StanceOkFury = true;
                StanceOkArms = true;
                StanceOkDef = true;
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
                // Invalidators
                if (!GetValided()) { return 0f; }

                // Actual Calcs
                return mhActivates + ohActivates;

                // ORIGINAL LINE
                //NONE
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.DeepWounds == 0) { return 0f; }




                // Abilities

                /* float mhCrits = (1f / combatFactors.MainHandSpeed) * combatFactors.MhCrit * (1f - heroicStrikePercent);
                float ohCrits = (1f / combatFactors.OffHandSpeed) * combatFactors.OhCrit;

                #region Fury Deep Wounds
                float heroicCrits = (1 / combatFactors.MainHandSpeed) * combatFactors.MhYellowCrit * heroicStrikePercent;
                float bloodThirstCrits = BT.GetActivates() * combatFactors.MhYellowCrit;
                float whirlWindCrits = WW.GetActivates() * combatFactors.MhYellowCrit;

                float bloodsurgeCrits = 0;
                if (Talents.Bloodsurge > 1) {
                    bloodsurgeCrits = (0.2f / 3 * Talents.Bloodsurge)
                        * (3 * (1 - combatFactors.YellowMissChance - combatFactors.MhDodgeChance) + 7 * heroicStrikesPerSecond)
                        / 10 * combatFactors.MhCrit;
                }
                #endregion

                #region Arms Deep Wounds
                float mortalStrikeCrits = MS.GetActivates() * combatFactors.MhYellowCrit;
                float overPowerCrits = OP.GetActivates() * (combatFactors.MhYellowCrit + (Talents.ImprovedOverpower * 0.25f) > 1 ? 1.0f : (combatFactors.MhYellowCrit + (Talents.ImprovedOverpower * 0.25f)));
                float suddenDeathCrits = SD.GetActivates() * combatFactors.MhYellowCrit;
                float slamCrits = SL.GetActivates() * combatFactors.MhYellowCrit;
                float bladestormCrits = BLS.GetActivates() * combatFactors.MhYellowCrit;
                float swordspecCrits = SS.GetActivates() * combatFactors.MhCrit;
                #endregion

                float deepWoundsDamage = combatFactors.AvgMhWeaponDmg * (mhCrits + heroicCrits + bloodThirstCrits + whirlWindCrits + bloodsurgeCrits
                                                                        + mortalStrikeCrits + overPowerCrits + suddenDeathCrits + slamCrits + bladestormCrits + swordspecCrits);
                deepWoundsDamage += combatFactors.AvgOhWeaponDmg * ohCrits;
                deepWoundsDamage *= 1 + StatS.BonusBleedDamageMultiplier;
                deepWoundsDamage *= 0.16f * Talents.DeepWounds;
                deepWoundsDamage *= 1 + Talents.Trauma * 0.15f * TR.GetActivates();
                deepWoundsDamage *= combatFactors.DamageBonus;

                return deepWoundsDamage;*/
                
                
                
                
                // Spread this damage over rotation length (turns it into DPS)
                //Damage /= GetRotation();

                // Ensure that we are not doing negative Damage
                //if (Damage < 0) { Damage = 0; }

                //return Damage;

                // LANDSOUL'S VERSION
                //Ability BT = new BloodThirst(Char, StatS, combatFactors, Whiteattacks);
                //Ability WW = new WhirlWind(Char, StatS, combatFactors, Whiteattacks);
                //Ability SL = new Slam(Char, StatS, combatFactors, Whiteattacks);
                //Ability MS = new Mortalstrike(Char, StatS, combatFactors, Whiteattacks);
                //Ability TR = new Trauma(Char, StatS, combatFactors, Whiteattacks);
                //Ability OP = new OverPower(Char, StatS, combatFactors, Whiteattacks);
                //Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);
                //Ability BLS = new Bladestorm(Char, StatS, combatFactors, Whiteattacks);
                //Ability SS = new Swordspec(Char, StatS, combatFactors, Whiteattacks);

                //float mhCrits = (1f / combatFactors.MainHandSpeed) * combatFactors.MhCrit * (1f - heroicStrikePercent);
                //float ohCrits = (1f / combatFactors.OffHandSpeed) * combatFactors.OhCrit;

                //float avgwpdmg = (/*AB54 %DIM*/1.122f * ((StatS.AttackPower / 14f + combatFactors.AvgMhWeaponDmg) * combatFactors.MainHandSpeed) */*AB56 %wepmod*/1.06f);

                //float critspersec = 
                ///*MeleeMH*/		    /* 0.078588f*/	(1f/combatFactors.TotalHaste)*(combatFactors.MhCrit)*/*AH48 Heroic Strike Difference*/1.0452f+(0.01f*/*AH24 sword exp bonus*/0f*Talents.IntensifyRage*/*R66 landed hits/sec no SS 0.9367f*/GetLandedAtksPerSecNoSS())*54f/60f*(combatFactors.MhCrit) + 
                ///*Mortal Strike*/	/* 0.042531f*/	(combatFactors.MhYellowCrit+0.1f*/*C40 4 pc T8 set bonus*/0f)/(MS.GetActivates()) +
                ///*Execute*/		    /* 0.019085f*/	SD.GetActivates()*combatFactors.MhYellowCrit +
                ///*Overpower*/		/* 0.123237f*/	(OP.GetActivates()==0f?0f:(combatFactors.MhYellowCrit+0.25f*Talents.ImprovedOverpower>1f?1f:combatFactors.MhYellowCrit+0.25f*Talents.ImprovedOverpower)/6f) +
                //// /*Heroics*/		/*-0.003395f*/	+
                ///*Slam*/		    /* 0.043861f*/SL.GetActivates()*combatFactors.MhYellowCrit

                //;

                // doing it this way because Deep Wounds triggering off of a MH crit and Deep Wounds triggering off of an OH crit do diff damage.
                // GetDamage is doing the average damage of a deep wounds trigger
                float damage = combatFactors.AvgMhWeaponDmg * (0.16f * Talents.DeepWounds) * mhActivates / (mhActivates+ohActivates) +
                    combatFactors.AvgOhWeaponDmg * (0.16f * Talents.DeepWounds) * ohActivates / (mhActivates+ohActivates);
                
                
                return damage;
            }
            public override float GetDamageOnUse() {
                // Invalidators
                if (Talents.DeepWounds == 0) { return 0f; }
                float Damage = GetDamage(); // Base Damage
                //Damage *= combatFactors.DamageBonus; // DW is only affected by % effects in the sense that MHWeaponDmg is, which is already covered
                //Damage *= combatFactors.DamageReduction; // Global Damage Penalties
                Damage *= (1f + StatS.BonusBleedDamageMultiplier);
                Damage *= combatFactors.DamageBonus; // Avg_hWeaponDmg no longer has DamageBonus in, we were double-dipping
                if (Talents.TitansGrip == 1) Damage *= 0.9f; // Titan's Grip penalty, since we're not modifying by combatFactors.DamageReduction

                if (Damage < 0) { Damage = 0; } // Ensure that we are not doing negative Damage
                return Damage;
            }
            public float GetTTLTickingTime() { return Cd; }
            public float GetNumTicks() { return GetTTLTickingTime() / CastTime; }
            //public override float GetAvgDamageOnUse() { return GetDamageOnUse() * GetActivates(); }
            public override float GetDPS() {
                //float dmgonuse = GetDamageOnUse();
                //float numticks = GetNumTicks();
                //float rot = GetRotation();
                //float result = (dmgonuse / numticks)/* / rot*/;
                float result = GetDamageOnUse() * GetActivates() / GetRotation();
                
                return result;
            }
        }
        // Arms Rotation
        public float _MS_PerHit  = 0f;public float _MS_DPS  = 0f;public float _MS_GCDs  = 0f;public float _MS_GCDsD  = 0f;
        public float _RD_PerHit  = 0f;public float _RD_DPS  = 0f;public float _RD_GCDs  = 0f;public float _RD_GCDsD  = 0f;
        public float _OP_PerHit  = 0f;public float _OP_DPS  = 0f;public float _OP_GCDs  = 0f;public float _OP_GCDsD  = 0f;
        public float _SD_PerHit  = 0f;public float _SD_DPS  = 0f;public float _SD_GCDs  = 0f;public float _SD_GCDsD  = 0f;
        public float _SL_PerHit  = 0f;public float _SL_DPS  = 0f;public float _SL_GCDs  = 0f;public float _SL_GCDsD  = 0f;
        public float _BLS_PerHit = 0f;public float _BLS_DPS = 0f;public float _BLS_GCDs = 0f;public float _BLS_GCDsD = 0f;
        public float _DW_PerHit  = 0f;public float _DW_DPS  = 0f;
        public float MakeRotationandDoDPS_Arms() {
            // Starting Numbers
            float DPS_TTL = 0f;
            float rotation = (_talents.GlyphOfRending ? ROTATION_LENGTH_ARMS_GLYPH:ROTATION_LENGTH_ARMS_NOGLYPH);
            float duration = _calcOpts.Duration;
            float NumGCDs = rotation / 1.5f;
            float GCDsused = 0f;
            float availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);

            // White DPS
            DPS_TTL += _whiteStats.CalcMhWhiteDPS();

            // Passive DPS (occurs regardless)
            /*Ability DW = new DeepWounds(_character, _stats, _combatFactors, _whiteStats);
            _DW_PerHit = DW.GetDamageOnUse();
            _DW_DPS = DW.GetDPS();
            DPS_TTL += _DW_DPS;*/
            // DW is being handled in GetCharacterCalcs right now
            
            // Periodic DPS (run only once every few rotations)
            Ability BLS = new Bladestorm(_character, _stats, _combatFactors, _whiteStats);
            float BLS_GCDs = BLS.GetActivates();
            if (BLS_GCDs > availGCDs) { BLS_GCDs = availGCDs; }
            _BLS_GCDs = BLS_GCDs; _BLS_GCDsD = _BLS_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, BLS_GCDs*4f); // the *4 is because it is channeled over 6 secs (4 GCD's consumed from 1 activate)
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _BLS_DPS = BLS.GetDPS(BLS_GCDs);
            DPS_TTL += _BLS_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 1 : Mortal Strike on every CD
            Ability MS = new Mortalstrike(_character, _stats, _combatFactors, _whiteStats);
            //float MS_GCDs = (float)System.Math.Floor(MS.GetActivates());
            float MS_GCDs = MS.GetActivates();
            if (MS_GCDs > availGCDs) { MS_GCDs = availGCDs; }
            _MS_GCDs = MS_GCDs; _MS_GCDsD = _MS_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, MS_GCDs);
            availGCDs = (float)System.Math.Max(0f,NumGCDs - GCDsused);
            _MS_DPS = MS.GetDPS(MS_GCDs);
            DPS_TTL += _MS_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 2 : Rend on every tick off
            DoT RND = new Rend(_character, _stats, _combatFactors, _whiteStats);
            //float RND_GCDs = (float)System.Math.Floor(RND.GetActivates());
            float RND_GCDs = RND.GetActivates();
            if (RND_GCDs > availGCDs) { RND_GCDs = availGCDs; }
            _RD_GCDs = RND_GCDs; _RD_GCDsD = _RD_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, RND_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _RD_DPS = RND.GetDPS(RND_GCDs);
            DPS_TTL += _RD_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 3 : Taste for Blood Proc (Do Overpower) if available
            Ability OP = new OverPower(_character, _stats, _combatFactors, _whiteStats);
            //float OP_GCDs = (float)System.Math.Floor(OP.GetActivates());
            float OP_GCDs = OP.GetActivates();
            if (OP_GCDs > availGCDs) { OP_GCDs = availGCDs; }
            _OP_GCDs = OP_GCDs; _OP_GCDsD = _OP_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, OP_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _OP_DPS = OP.GetDPS(OP_GCDs);
            DPS_TTL += _OP_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 4 : Sudden Death Proc (Do Execute) if available
            Ability SD = new Suddendeath(_character, _stats, _combatFactors, _whiteStats);
            //float SD_GCDs = (float)System.Math.Floor(SD.GetActivates(false));
            float SD_GCDs = SD.GetActivates(false);
            if (SD_GCDs > availGCDs) { SD_GCDs = availGCDs; }
            _SD_GCDs = SD_GCDs; _SD_GCDsD = _SD_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, SD_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _SD_DPS = SD.GetDPS(SD_GCDs);
            DPS_TTL += _SD_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 5 : Slam for remainder of GCDs
            Ability SL = new Slam(_character, _stats, _combatFactors, _whiteStats);
            float SL_GCDs = availGCDs;
            if (SL_GCDs > availGCDs) { SL_GCDs = availGCDs; }
            _SL_GCDs = SL_GCDs; _SL_GCDsD = _SL_GCDs * duration / rotation;
            GCDsused += (float)System.Math.Min(NumGCDs, SL_GCDs);
            availGCDs = (float)System.Math.Max(0f, NumGCDs - GCDsused);
            _SL_DPS = SL.GetDPS(SL_GCDs);
            DPS_TTL += _SL_DPS;
            if (availGCDs <= 0f) { return DPS_TTL; }

            // Priority 6 : Heroic Strike, never

            // Return result
            return DPS_TTL;
        }
    }
}

