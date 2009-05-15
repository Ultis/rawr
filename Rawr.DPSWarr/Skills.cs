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
        //private float heroicStrikePercent;
        //private float heroicStrikesPerSecond;
        public const float ROTATION_LENGTH_FURY = 16.0f;
        public const float ROTATION_LENGTH_ARMS = 30.0f;
        #endregion

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
            /// <param name="cd">The cooldown of the ability. Glyphs Affecting cooldowns should have the modifier in the constructor</param>
            /// <param name="ragecost">The amount of rage used by this ability. Glyphs which sometimes/always reduce rage cost should have the modifier in the constructor</param>
            /// <param name="casttime">The cast time required to complete this ability action. Use -1 for instant attacks.</param>
            /// <param name="stancef">The ability can be activated from Fury (Berserker) Stance</param>
            /// <param name="stancea">The ability can be activated from Arms (Battle) Stance</param>
            /// <param name="stanced">The ability can be activated from Defensive Stance</param>
            public Ability(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, string name, string desc,
                bool reqmeleeweap, bool reqmeleerange, float maxrange, string tlntsafctg, string glphsafctg,
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
                Cd = -1; // In Seconds
                RageCost = -1;
                CastTime = -1; // In Seconds
                StanceOkFury = false;
                StanceOkArms = false;
                StanceOkDef = false;
            }
            // Variables
            private string NAME;
            private string DESC;
            private bool REQMELEEWEAP;
            private bool REQMELEERRANGE;
            private float MAXRANGE; // In Yards 
            private string TLNTSAFCTG;
            private string GLPHSAFCTG;
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
            // Get/Set
            public string Name { get { return NAME; } set { NAME = value; } }
            public string Desc { get { return DESC; } set { DESC = value; } }
            public bool ReqMeleeWeap { get { return REQMELEEWEAP; } set { REQMELEEWEAP = value; } }
            public bool ReqMeleeRange { get { return REQMELEERRANGE; } set { REQMELEERRANGE = value; } }
            public float MaxRange { get { return MAXRANGE; } set { MAXRANGE = value; } } // In Yards 
            public string TlntsAfctg { get { return TLNTSAFCTG; } set { TLNTSAFCTG = value; } }
            public string GlphsAfctg { get { return GLPHSAFCTG; } set { GLPHSAFCTG = value; } }
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
            // Functions
            public virtual float GetRageUsePerSecond(){return GetActivates() * RageCost / GetRotation();}
            public virtual float GetRotation() {
                if (CalcOpts.FuryStance) {
                    return ROTATION_LENGTH_FURY;
                }else{
                    return ROTATION_LENGTH_ARMS;
                }
            }
            public virtual bool GetValided() {
                // Invalidators
                if (Char == null || CalcOpts == null || Talents == null) { return false; }
                if (ReqMeleeWeap && (Char.MainHand == null || Char.MainHand.MaxDamage == 0)){return false;}
                if ((CalcOpts.FuryStance && !StanceOkFury)
                    || (!CalcOpts.FuryStance && !StanceOkArms)
                    /*||( CalcOpts.DefStance  && !StanceOkDef )*/ ) { return false; }
                return true;
            }
            public virtual float GetActivates() { return 0f; } // Number of times used in rotation
            public virtual float GetHealing() { return 0f; }
            public virtual float GetDamage() { return GetDamage(false); }
            public virtual float GetDamage(bool Override) { return 0f; }
            public virtual float GetAvgDamage() { return GetDamage() * GetActivates(); }
            public virtual float GetDamageOnUse() { return 0f; }
            public virtual float GetAvgDamageOnUse() { return GetDamageOnUse() * GetActivates(); }
            public virtual float GetDPS() { return GetAvgDamageOnUse() / GetRotation(); }
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
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Bloodthirst (Requires talent to use ability)\n
Unending Fury [Increases the damage done by your Slam, Whirlwind and Bloodthirst abilities by (2/4/6/8/10)%.]";
                GlphsAfctg = @"Glyph of Bloodthirst [+100% from healing effect]";
                Cd = 5; // In Seconds
                RageCost = 30;
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
                if (!GetValided()||Talents.Bloodthirst==0) { return 0f; }

                // Actual Calcs
                return 3f;
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
                if (!GetValided()||Talents.Bloodthirst==0) { return 0f; }

                // Base Damage
                float Damage = (StatS.AttackPower * 50.0f / 100f);
                
                // Talents Affecting
                Damage *= (1 + Talents.UnendingFury * 0.02f);

                // Spread this damage over rotation length (turns it into DPS)
                // Damage /= GetRotation();

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }
                
                return Damage;

                // ORIGINAL LINES
                //float btDamage = _stats.AttackPower * 0.5f * _combatFactors.DamageBonus * (1 + _talents.UnendingFury * 0.02f);
                //btDamage *= (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg);
                //btDamage *= BloodThirstHits();

                //if (btDamage < 0) { btDamage = 0; }
                //return _combatFactors.DamageReduction * btDamage * _talents.Bloodthirst;
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
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Improved Whirlwind [Damage increased by (10%/20%)]";
                GlphsAfctg = @"Glyph of Whirlwind [Cooldown Reduced by 2 sec.]";
                Cd = 10; // In Seconds
                RageCost = 25;
                CastTime = -1; // In Seconds
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
            public override float GetDamage(bool Override) {
                return GetDamage(Override, false);
            }

            // <param name="Override">When true, do not check for Bers Stance</param>
            // <param name="isOffHand">When true, do calculations for off-hand damage instead of main-hand</param>
            public float GetDamage(bool Override, bool isOffHand) {
                // Invalidators
                if (!GetValided() && !Override) { return 0f; }

                // Base Damage
                float Damage;
                if (isOffHand)
                {
                    if (this.Char.OffHand != null && this.Char.OffHand.Item != null) Damage = combatFactors.NormalizedOhWeaponDmg * (0.50f + Talents.DualWieldSpecialization * 0.025f);
                    else Damage = 0f;
                }
                else Damage = combatFactors.NormalizedMhWeaponDmg;
                //Damage += combatFactors.NormalizedOhWeaponDmg * (0.50f + Talents.DualWieldSpecialization * 0.025f);

                // Talents Affecting
                Damage *= (1.00f + Talents.ImprovedWhirlwind * 0.10f);
                Damage *= (1.00f + Talents.UnendingFury * 0.02f);

                // Spread this damage over rotaion length (turns it into DPS)
                //Damage /= GetRotation();

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;

                // ORIGINAL LINES
                //float wwDamage = _combatFactors.DamageBonus * (1.00f + 0.1f * _talents.ImprovedWhirlwind) * (1.00f + _talents.UnendingFury * 0.02f);
                //wwDamage *= (_combatFactors.NormalizedMhWeaponDmg * (1f - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance
                //             + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg) +
                //             (0.50f + _talents.DualWieldSpecialization * 0.025f) * _combatFactors.NormalizedOhWeaponDmg *
                //             (1.00f - _combatFactors.YellowMissChance - _combatFactors.OhDodgeChance
                //             + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg));
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
                StanceOkArms = true;
                StanceOkDef  = true;
                hsActivates = 0.0f;
            }
            // Variables
            public float hsActivates { get; set; }
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided() || Talents.Bloodsurge == 0) { return 0f; }

                // Actual Calcs
                Ability BT = new BloodThirst(Char, StatS, combatFactors, Whiteattacks);
                Ability WW = new WhirlWind(Char, StatS, combatFactors, Whiteattacks);
                //Ability HS = new HeroicStrike(Char, StatS, combatFactors, Whiteattacks);
                
                float chance = Talents.Bloodsurge * 0.0666666667f;
                float procs = BT.GetActivates() + WW.GetActivates() + hsActivates;// HS.GetActivates();
                procs *= chance;
                //procs /= GetRotation();
                // procs = (procs / GetRotation()) - (chance * chance + 0.01f); // WTF is with squaring chance?
                if (procs < 0) { procs = 0; }
                if (procs > 5) { procs = 5; } // Only have 5 free GCDs in the rotation
                return procs;

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
                return (float)System.Math.Floor(GetRotation() / (Cd - (Talents.ImprovedMortalStrike * 0.33334f)));
                // ORIGINAL LINE
                //return (1.0f / (5f - .334f * _talents.ImprovedMortalStrike));
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.MortalStrike == 0) { return 0f; }

                // Base Damage
                float Damage = combatFactors.NormalizedMhWeaponDmg + 380;

                // Talents/Glyphs Affecting
                Damage *= (1f + Talents.ImprovedMortalStrike * 0.0333334f);
                Damage *= (1f + (Talents.GlyphOfMortalStrike ? 0.10f : 0f));

                // Spread this damage over rotaion length (turns it into DPS)
                //Damage /= GetRotation();

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;

                // ORIGINAL LINES
                //float msDamage = _combatFactors.DamageBonus * (1.1f) * (1 + 0.0333333f * _talents.ImprovedMortalStrike);
                //msDamage *= (_combatFactors.NormalizedMhWeaponDmg * (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance
                //             + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg) + 380);
                //msDamage *= MortalStrikeHits();
                //msDamage *= (_talents.GlyphOfMortalStrike ? 1.10f : 1.00f);
                //return msDamage * _combatFactors.DamageReduction;
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
        public class Rend : Ability {
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
                // Actual Calcs
                return GetRotation() / (15f + (Talents.GlyphOfRending ? 6f : 0f));
                // ORIGINAL LINE
                //return 6.0f / (15f + (_talents.GlyphOfRending ? 6f : 0f));
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                Ability TR = new Trauma(Char, StatS, combatFactors, Whiteattacks);
                // Base Damage
                float DamageMod = 1 + Talents.ImprovedRend * 0.1f; // +10% or 20% from Improved Rend Talent
                DamageMod *= combatFactors.DamageBonus; // From talents, thins like Wrecking crew are taken into account here
                DamageMod *= 1 + (Talents.GlyphOfRending ? (2.00f / 7.00f) : 0.00f); // Glyph of Rending adds 6 seconds (2 more ticks from 5 to new ttl of 7)
                DamageMod *= 1 + Talents.Trauma * 0.15f * TR.GetActivates();
                DamageMod *= 1 + StatS.BonusBleedDamageMultiplier; // This is supposed to be other bleed effects similar to trauma, selected on the Buffs Tab
                float Damage = (380+combatFactors.AvgMhWeaponDmg) * DamageMod; // Rend Dmg

                // Talents/Glyphs Affecting

                // Spread this damage over rotation length (turns it into damage per tick)
                Damage /= (Cd+(Talents.GlyphOfRending ? 6f : 0f)/CastTime);

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage; // divide by 18 because of the uptime maybe? I don't get this part

                // ORIGINAL LINES
                //float DamageMod = (1 + (0.35f * 0.25f)); // Bonus Dmg over 75% HP // I cant find any evidence to support this claim
                //DamageMod *= 1 + 0.1f * Talents.ImprovedRend; // +10% or 20% from Improved Rend Talent
                //DamageMod *= combatFactors.DamageBonus; // From gear and stuff
                //DamageMod *= 1 + ((Talents != null && Talents.GlyphOfRending) ? (2.00f / 7.00f) : 0.00f); // Glyph of Rending adds 6 seconds (2 more ticks from 5 to new ttl of 7)
                //DamageMod *= 1 + Talents.Trauma * 0.15f * TR.GetActivates();
                //DamageMod *= 1 + StatS.BonusBleedDamageMultiplier; // This is supposed to be other bleed effects similar to trauma
                //float Damage = 380 * DamageMod; // Base Rend Dmg
                //Damage += DamageMod * combatFactors.AvgMhWeaponDmg; // Add in Weapon dmg
                //if (Damage < 0) { Damage = 0; }
                //return Damage / 18; // divide by 18 because of the uptime maybe? I don't get this part
            }
            public override float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage
                //Damage *= combatFactors.DamageBonus; // Global Damage Bonuses // No other bonuses to damage (see getdamage)
                //Damage *= combatFactors.DamageReduction; // Global Damage Penalties // Set value, Ignores Armor
                //Damage *= (1 - combatFactors.YellowMissChance - combatFactors.MhDodgeChance); // Attack Table, does not crit

                if (Damage < 0) { Damage = 0; } // Ensure that we are not doing negative Damage
                return Damage;
            }
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
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Sudden Death (Requires talent to use ability) (Talent modifies Chance to proc and rage kept after)\n
Improved Execute [Reduces the rage cost of your Execute ability by (2.5/5)]";
                GlphsAfctg = @"Glyph of Execute [Ability acts as if it had 10 additional rage]";
                Cd = 6f; // In Seconds
                RageCost = 15; // 30 is the max it will use, glyph will take this to 40, cast cost will drop from 15 to 10 with max ImpExec
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
                if (!GetValided() || Talents.SuddenDeath == 0) { return 0f; }

                Ability SL = new Slam(Char, StatS, combatFactors, Whiteattacks);
                Ability MS = new Mortalstrike(Char, StatS, combatFactors, Whiteattacks);

                // Actual Calcs
                double chance = 0.03 * Talents.SuddenDeath;
                double totalHits = MS.GetRotation() / combatFactors.MainHandSpeed;
                totalHits -= 0.5 * (SL.GetActivates() * SL.GetRotation());
                totalHits += (SL.GetActivates() + MS.GetActivates()) * MS.GetRotation();
                float hits = (1f - (float)System.Math.Pow(1 - chance, totalHits)) / MS.GetRotation();
                return 2 * hits;
                
                // ORIGINAL LINES
                //double chance = 0.03 * Talents.SuddenDeath;
                //double totalHits = MS.GetRotation() / combatFactors.MainHandSpeed;
                //totalHits -= 0.5 * (SL.GetActivates() * SL.GetRotation());
                //totalHits += (SL.GetActivates() + MS.GetActivates()) * MS.GetRotation();
                //float hits = (1f - (float)System.Math.Pow(1 - chance, totalHits)) / MS.GetRotation();
                //return 2 * hits;
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.MortalStrike == 0) { return 0f; }

                Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);
                // Base Damage
                float freerage = freeRage(); if (freerage < 0) { return 0.0f; }                 // No Free Rage = 0 damage
                float executeRage = freerage * GetRotation();
                executeRage -= (Cd - 2.5f * Talents.ImprovedExecute);
                if (executeRage > 30) { executeRage = 30; }
                executeRage += (Talents.GlyphOfExecution ? 10.00f : 0.00f);
                float executeDamage = 0;
                executeDamage *= ((StatS.AttackPower * 0.2f) + 1456 + executeRage * 38);
                executeDamage *= SD.GetActivates();

                // Talents/Glyphs Affecting

                // Spread this damage over rotaion length (turns it into DPS)
                executeDamage /= GetRotation();

                // Ensure that we are not doing negative Damage
                if (executeDamage < 0) { executeDamage = 0; }

                return executeDamage;
            }
            public float neededRage() {
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
            public float freeRage() {
                Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);
                return Whiteattacks.whiteRageGenPerSec() + OtherRage() + SD.GetRageUsePerSecond() - neededRage();
            }
            public float BloodRageGain() { return (20 + 5 * Talents.ImprovedBloodrage) / (60 * (1 - 0.11f * Talents.IntensifyRage)); }
            public float AngerManagementGain() { return Talents.AngerManagement / 3.0f; }
            public float ImprovedBerserkerRage() { return Talents.ImprovedBerserkerRage * 10 / (30 * (1 - 0.11f * Talents.IntensifyRage)); }
            public float UnbridledWrathGain() { return Talents.UnbridledWrath * 3.0f / 60.0f; }
            public float OtherRage() {
                float rage = (14.0f / 8.0f * (1 + combatFactors.MhCrit - (1.0f - combatFactors.ProbMhWhiteHit)));
                if (combatFactors.OffHand.DPS > 0 && (combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || Talents.TitansGrip == 1))
                    rage += 7.0f / 8.0f * (1 + combatFactors.OhCrit - (1.0f - combatFactors.ProbOhWhiteHit));
                rage *= combatFactors.TotalHaste;
                rage += AngerManagementGain() + ImprovedBerserkerRage() + BloodRageGain() + UnbridledWrathGain();
                rage *= 1 + Talents.EndlessRage * 0.25f;

                return rage;
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
                /*double chance; float procs;
                { // TasteFoorBlood
                    chance = 0.33334f * Talents.TasteForBlood;
                    procs = (1f - (float)System.Math.Pow(1 - chance, 6))/* / GetRotation()*//*;
                }
                return (float)System.Math.Floor(procs);*/

                // ORIGINAL LINES
                //double chance; float procs;
                //{ // TasteFoorBlood
                //    chance = 0.1 * _talents.TasteForBlood;
                //    procs = (1f - (float)System.Math.Pow(1 - chance, 6)) / 18;
                //}
                //return procs;

                Ability SL = new Slam(Char, StatS, combatFactors, Whiteattacks);
                // LANDSOUL's VERSION
                if(combatFactors.MhDodgeChance<=0f&&Talents.TasteForBlood==0f){
                    return 0f;//999999f;
                }else{
                    if (Talents.TasteForBlood == 0f) {
                        return 1f/
                            (combatFactors.MhDodgeChance*(1f/combatFactors.MainHandSpeed)+
                             0.01f*(/*R66 Landed attacks per second no SwdSpk*/0.8868f)*combatFactors.MhExpertise*Talents.SwordSpecialization*54f/60f+
                             0.03f*(/*AR7 Overpower GCD Percentage*/0.1392f)*(/*M61 Exe overwrite chance*/0.9430f)*(/*R68 Landed attacks per second*/0.8868f)+
                             1f/(5f+(/*N4 Latency 115f*/0f+/*N3 React 220f*/0f)/1000f)+
                             1f / /*AB49 Slam Proc GCD % 0.071227f*/ SL.GetActivates());
                    }else{
                        if (Talents.TasteForBlood == 2f || Talents.TasteForBlood == 3f) {
                            return 6f;
                        }else{
                            return 9f;
                        }
                    }
                }

            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // Base Damage
                //float Damage = combatFactors.NormalizedMhWeaponDmg;

                // Talents/Glyphs Affecting
                //Damage *= (1f + Talents.UnrelentingAssault * 0.10f);

                // Spread this damage over rotaion length (turns it into DPS)
                //Damage /= GetRotation();

                // Ensure that we are not doing negative Damage
                //if (Damage < 0) { Damage = 0; }

                //return Damage;

                // ORIGINAL LINES
                //float opCrit = _combatFactors.MhYellowCrit + 0.25f * Talents.ImprovedOverpower;
                //if (opCrit > 1) { opCrit = 1; }
                //float overpowerDamage = _combatFactors.DamageBonus;
                //overpowerDamage *= (_combatFactors.NormalizedMhWeaponDmg * (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance
                //             + opCrit * _combatFactors.BonusYellowCritDmg));
                //return overpowerDamage * _combatFactors.DamageReduction * (1 + _talents.UnrelentingAssault * 0.10f);

                // LANDSOUL's VERSION
                return /* C64* */                                      // C64  Damage reduction
                /* AB54* */                                     // AB54 % Damage Bonus
                //(1f-combatFactors.HitPercent+(combatFactors.MhYellowCrit+0.25f*Talents.ImprovedOverpower>1f?1f:combatFactors.MhYellowCrit+0.25f*Talents.ImprovedOverpower)*combatFactors.BonusYellowCritDmg)* // AH31 miss chance, AH36 Yellow Crit %, AB60 Crit Bonus Dmg % 1.2720
                (combatFactors.AvgMhWeaponDmg*combatFactors.MainHandSpeed+StatS.AttackPower/14f*3.3f)*                  // U120 MHNormDPS, S120 MHSpd, Z55 AvgAP, T120 MHNormSpd
                (1f+0.1f*Talents.UnrelentingAssault) /                              // AR6 Unrelenting Assault
                GetActivates();                                      // OP Hits
            }
            public override float GetDamageOnUse() {
                float Damage = GetDamage(); // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties
                Damage *= (1 - combatFactors.YellowMissChance // cant be dodged
                    + (combatFactors.MhYellowCrit + 0.25f * Talents.ImprovedOverpower) * combatFactors.BonusYellowCritDmg); // Attack Table

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
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Bladestorm [Requires talent to use Ability]";
                GlphsAfctg = @"Glyph of Bladestorm [Cooldown Reduced by 15 sec]";
                Cd = 90f; // In Seconds
                RageCost = 25;
                CastTime = 6; // In Seconds // Channeled
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
                return GetRotation() / (Cd - (Talents.GlyphOfBladestorm ? 15 : 0));
                // ORIGINAL LINE
                //return (6.0f / (90 - (_talents.GlyphOfBladestorm ? 15 : 0))) * _talents.Bladestorm;
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

                return Damage;

                // ORIGINAL LINES
                //float wwDamage = _combatFactors.DamageBonus;
                //wwDamage *= (_combatFactors.NormalizedMhWeaponDmg * (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance
                //             + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg));
                //wwDamage *= BladestormHits();
                //if (wwDamage < 0)
                //    wwDamage = 0;
                //return _combatFactors.DamageReduction * wwDamage;
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

                // ORIGINAL LINE
                //Ability SL = new Slam(_character, _stats, _combatFactors);
                //Ability MS = new Mortalstrike(_character, _stats, _combatFactors);
                //if (_combatFactors.MainHand.Type != Item.ItemType.TwoHandSword) { return 0.0f; }
                //float missChance = (1 - _combatFactors.WhiteMissChance - _combatFactors.MhDodgeChance);
                //float wepSpeed = _combatFactors.MainHandSpeed;
                //if (_combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand && _talents.TitansGrip != 1) {
                //  wepSpeed += (1.5f - (0.5f * _talents.ImprovedSlam)) / 5;
                //}
                //float whiteHits = missChance * (1 / wepSpeed);
                //float attacks = 0.01f * _talents.SwordSpecialization;
                //attacks *= missChance;
                //attacks *= (MS.GetActivates() + SL.GetActivates() + OverpowerHits() + SuddenDeathHits() + whiteHits);
                //return attacks;
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.SwordSpecialization == 0) { return 0f; }

                // Base Damage
                float Damage = combatFactors.AvgMhWeaponDmg;

                // Talents/Glyphs Affecting

                // Spread this damage over rotation length (turns it into DPS)
                Damage /= GetRotation();

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

                // ORIGINAL LINE
                //if (_calcOpts!=null && _calcOpts.MultipleTargets) {
                //  return 6.0f / 30 * _talents.SweepingStrikes;
                //}else{
                //  return 0.0f;
                //}
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided() || Talents.MortalStrike == 0) { return 0f; }

                // Base Damage
                float DamageMod = 2;
                float Damage = 1 * DamageMod;

                // Talents/Glyphs Affecting

                // Spread this damage over rotaion length (turns it into DPS)
                Damage /= GetRotation();

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
                return combatFactors.MhCrit /* * (1 - (15.0f/60.0f)) */;
                // last part is to calc uptime but not sure if that's accurate
                //return GetRotation() / (1.5f - (Talents.ImprovedSlam * 0.5f)) / 5;
                
                // ORIGINAL LINE
                //return _combatFactors.MhCrit * _combatFactors.ProbMhWhiteHit /* * (1 - (15.0f/60.0f))*/;
                // last part is to calc uptime but not sure if that's accurate
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
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Improved Slam [Reduces cast time of your Slam ability by (0.5/1) sec.]";
                GlphsAfctg = @"";
                Cd = -1; // In Seconds
                RageCost = 15;
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

                // Actual Calcs
                return GetRotation() / (1.5f - (Talents.ImprovedSlam * 0.5f)) / 5;
                
                // ORIGINAL LINE
                //return (_talents.ImprovedSlam == 2 ? (1.5f /*- (0.5f * _talents.ImprovedSlam)*/ / 5) : 0);
            }
            public override float GetDamage()
            {
                return GetDamage(false);
            }
            public override float GetDamage(bool Override) {
                // Invalidators
                if (!Override && !GetValided()) { return 0f; }

                // Base Damage
                float Damage = combatFactors.AvgMhWeaponDmg + 250;

                // Talents Affecting
                Damage *= (1f + StatS.BonusSlamDamage);
                Damage *= (1f + Talents.UnendingFury * 0.02f);

                // Spread this damage over rotation length (turns it into DPS)
                //Damage /= GetRotation();

                // Ensure that we are not doing negative Damage
                if (Damage < 0) { Damage = 0; }

                return Damage;

                // ORIGINAL LINES
                //float slamDamage = _combatFactors.DamageBonus * (1 + _stats.BonusSlamDamage);
                //slamDamage *= (_combatFactors.AvgMhWeaponDmg * (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance
                //             + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg) + 250);
                //slamDamage *= SlamHits();
                //return slamDamage * _combatFactors.DamageReduction;
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
                StanceOkArms = true;
                StanceOkDef = true;
                bloodsurgeRPS = 0.0f;
            }
            // Variables
            public float bloodsurgeRPS { get; set; }
            // Get/Set
            // Functions
            public override float GetActivates() {
                // Invalidators
                if (!GetValided()) { return 0f; }

                // HS per second
                float hsHits = (1 - combatFactors.YellowMissChance - combatFactors.MhDodgeChance);
                hsHits *= (/*rageModifier +*/ freeRage() / heroicStrikeRageCost());

                if (hsHits < 0) { hsHits = 0; }
                heroicStrikesPerSecond = hsHits;

                return hsHits * GetRotation();

                // ORIGINAL LINE
                //return (_talents.ImprovedSlam == 2 ? (1.5f /*- (0.5f * _talents.ImprovedSlam)*/ / 5) : 0);
            }
            public float BloodRageGainRagePerSec() { return (20 + 5 * Talents.ImprovedBloodrage) / (60 * (1 - 1.0f/9.0f * Talents.IntensifyRage)); }
            public float AngerManagementRagePerSec() { return Talents.AngerManagement / 3.0f; }
            public float ImprovedBerserkerRagePerSec() { return Talents.ImprovedBerserkerRage * 10 / (30 * (1 - 1.0f/9.0f * Talents.IntensifyRage)); }
            public float OtherRageGenPerSec() {
                // Ebs: Removed a lot of this due to crit chances already being factored in the WhiteAttacks class
                float rage;// = (14.0f / 8.0f * (1 + combatFactors.MhCrit - (1.0f - combatFactors.ProbMhWhiteHit)));
                //if (combatFactors.OffHand.DPS > 0 && (combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || Talents.TitansGrip == 1))
                //    rage += 7.0f / 8.0f * (1 + combatFactors.OhCrit - (1.0f - combatFactors.ProbOhWhiteHit));
                //rage *= combatFactors.TotalHaste;
                rage /*+*/= AngerManagementRagePerSec() + ImprovedBerserkerRagePerSec() + BloodRageGainRagePerSec();
                rage *= 1 + Talents.EndlessRage * 0.25f;

                return rage;
            }
            public float neededRagePerSecond() {
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

                //Ability BS = new BloodSurge(Char, StatS, combatFactors, Whiteattacks);
                float BloodSurgeRage = bloodsurgeRPS;// BS.GetRageUsePerSecond();
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
            public float freeRage() {
                Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);
                return Whiteattacks.whiteRageGenPerSec() + OtherRageGenPerSec() + SD.GetRageUsePerSecond() - neededRagePerSecond();
            }
            public float heroicStrikeRageCost() {

                float rageCost = this.RageCost;
                rageCost -= Talents.ImprovedHeroicStrike; // Imp HS
                if (Talents.GlyphOfHeroicStrike) rageCost -= 10.0f * combatFactors.MhCrit; // Glyph bonus rage on crit
                rageCost += Whiteattacks.GetMHSwingRage();
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
                float damageIncrease = 495f;
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
                MaxRange = 5; // In Yards 
                TlntsAfctg = @"Deep Wounds (Requires talent to use Ability) [(16/32/48)% damage dealt]";
                GlphsAfctg = @"";
                Cd = 6; // In Seconds // 6 seconds to sim falloff
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
                if (!GetValided()) { return 0f; }

                // Actual Calcs
                return StatS.PhysicalCrit;

                // ORIGINAL LINE
                //NONE
            }
            public override float GetDamage() {
                // Invalidators
                if (!GetValided()) { return 0f; }




                // Ablities
                Ability BT = new BloodThirst(Char, StatS, combatFactors, Whiteattacks);
                Ability WW = new WhirlWind(Char, StatS, combatFactors, Whiteattacks);
                Ability SL = new Slam(Char, StatS, combatFactors, Whiteattacks);
                Ability MS = new Mortalstrike(Char, StatS, combatFactors, Whiteattacks);
                Ability TR = new Trauma(Char, StatS, combatFactors, Whiteattacks);
                Ability OP = new OverPower(Char, StatS, combatFactors, Whiteattacks);
                Ability SD = new Suddendeath(Char, StatS, combatFactors, Whiteattacks);
                Ability BLS = new Bladestorm(Char, StatS, combatFactors, Whiteattacks);
                Ability SS = new Swordspec(Char, StatS, combatFactors, Whiteattacks);
                //
                float mhCrits = (1 / combatFactors.MainHandSpeed) * combatFactors.MhCrit * (1 - heroicStrikePercent);
                float ohCrits = (1 / combatFactors.OffHandSpeed) * combatFactors.OhCrit;

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

                return deepWoundsDamage;
                
                
                
                
                // Spread this damage over rotation length (turns it into DPS)
                //Damage /= GetRotation();

                // Ensure that we are not doing negative Damage
                //if (Damage < 0) { Damage = 0; }

                //return Damage;
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

        #region Rage Calculations
        // Returns the value for every one second
        public float neededRage() {
            Ability BT = new BloodThirst(_character, _stats, _combatFactors, _whiteStats);
            float BTRage = BT.GetRageUsePerSecond();
            //float BTRage = BloodThirstHits() * 30; // ORIGINAL LINE

            Ability WW = new WhirlWind(_character, _stats, _combatFactors, _whiteStats);
            float WWRage = WW.GetRageUsePerSecond();
            //float WWRage = WhirlWindHits() * 30; // ORIGINAL LINE

            Ability MS = new Mortalstrike(_character, _stats, _combatFactors, _whiteStats);
            float MSRage = MS.GetRageUsePerSecond();
            //float MSRage = MortalStrikeHits() * 30; // ORIGINAL LINE

            Ability OP = new OverPower(_character, _stats, _combatFactors, _whiteStats);
            float OPRage = OP.GetRageUsePerSecond();
            //float OPRage = OverpowerHits() * 5; // ORIGINAL LINE

            Ability SD = new Suddendeath(_character, _stats, _combatFactors, _whiteStats);
            float SDRage = SD.GetRageUsePerSecond();
            //float SDRage = SuddenDeathHits() * 10; // ORIGINAL LINE

            Ability SL = new Slam(_character, _stats, _combatFactors, _whiteStats);
            float SlamRage = SL.GetRageUsePerSecond();
            //float SlamRage = SlamHits() * 15; // ORIGINAL LINE

            Ability BS = new BloodSurge(_character, _stats, _combatFactors, _whiteStats);
            float BloodSurgeRage = BS.GetRageUsePerSecond();
            // NO ORIGINAL LINE

            Ability BLS = new Bladestorm(_character, _stats, _combatFactors, _whiteStats);
            float BladestormRage = BLS.GetRageUsePerSecond();
            //float BladestormRage = BladestormHits() * 30; // ORIGINAL LINE

            Ability SW = new SweepingStrikes(_character, _stats, _combatFactors, _whiteStats);
            float SweepingRage = SW.GetRageUsePerSecond();
            //float SweepingRage = SweepingHits() * (_talents.GlyphOfSweepingStrikes ? 0 : 30); // ORIGINAL LINE

            Ability RND = new Rend(_character, _stats, _combatFactors, _whiteStats);
            float RendRage = RND.GetRageUsePerSecond();
            //float RendRage = RendHits() * 10; // ORIGINAL LINE

            // Total
            float rage = BTRage + WWRage + MSRage + OPRage + SDRage + SlamRage
                + BloodSurgeRage + SweepingRage + BladestormRage + RendRage;
            return rage;  
        }
        // Returns the value for every one second
        public float BloodRageGain() {return (20+5*_talents.ImprovedBloodrage)/(60*(1-0.11f*_talents.IntensifyRage));}
        // Returns the value for every one second
        public float AngerManagementGain() {return _talents.AngerManagement / 3.0f;}
        // Returns the value for every one second
        public float UnbridledWrathGain() { return _talents.UnbridledWrath*3.0f / 60.0f; }
        // Returns the value for every one second
        public float ImprovedBerserkerRage() {return _talents.ImprovedBerserkerRage * 10 / (30 * (1 - 0.11f * _talents.IntensifyRage));}
        // Returns the value for every one second
        public float OtherRage() {
            float rage = (14.0f / 8.0f*(1+_combatFactors.MhCrit-(1.0f-_combatFactors.ProbMhWhiteHit)));
            if(_combatFactors.OffHand.DPS > 0 && (_combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || _talents.TitansGrip == 1))
                rage += 7.0f/8.0f*(1+_combatFactors.OhCrit-(1.0f-_combatFactors.ProbOhWhiteHit));
            rage *= _combatFactors.TotalHaste;
            rage += AngerManagementGain() + ImprovedBerserkerRage() + BloodRageGain() + UnbridledWrathGain();
            rage *= 1 + _talents.EndlessRage * 0.25f;

            return rage;
        }
        public float freeRage() {
            Ability SD = new Suddendeath(_character, _stats, _combatFactors, _whiteStats);
            return _whiteStats.whiteRageGenPerSec() + OtherRage() + SD.GetRageUsePerSecond() - neededRage();
        }
        public float heroicStrikeRage() {
            //MHAverageDamage*ArmorDeal*15/4/cVal*(1+mhCritBonus*mhCrit-glanceChance*glanceReduc-whiteMiss-dodgeMH)+7/2*(1+mhCrit-whiteMiss-whiteDodge)
            float rage = _combatFactors.AvgMhWeaponDmg*_combatFactors.DamageReduction*15.0f/4/320.6f;
            rage *= (1.0f + _combatFactors.MhCrit * _combatFactors.BonusWhiteCritDmg
                            - (1.0f - _combatFactors.ProbMhWhiteHit) - (0.25f * 0.35f));
            rage += 7.0f / 2.0f * (1 + _combatFactors.MhCrit - (1.0f - _combatFactors.ProbMhWhiteHit));
            //int modNumber = 0; if (_talents.GlyphOfHeroicStrike) { modNumber = 10; }
            //rage += 1.0f * (1 + _combatFactors.MhCrit - (1.0f - _combatFactors.ProbMhWhiteHit)) * modNumber;
            
            return rage;
        }
        #endregion
    }
}

