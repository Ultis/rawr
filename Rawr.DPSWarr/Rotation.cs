using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Rawr.DPSWarr.Skills;

namespace Rawr.DPSWarr {
    public class Rotation
    {
        /*public static Dictionary<string, int> ConstructionCounts = new Dictionary<string, int>() {
            { "Rotation Base", 0 },
            { "Rotation Arms", 0 },
            { "Rotation Fury", 0 },
        };*/

        // Constructors
        public Rotation()
        {
            AbilityList = new Dictionary<Type,AbilityWrapper>();
            InvalidateCache();
#if DEBUG
            //ConstructionCounts["Rotation Base"]++;
#endif
        }

        #region Variables

        public void InvalidateAbilityLists() {
            AbilityList.Clear();
            _abilList = _abilListThatDMGs = _abilListThatMaints = _abilListThatGCDs = null;
        }

        private Dictionary<Type, AbilityWrapper> _abilityList;
        private Dictionary<Type, AbilityWrapper> AbilityList {
            get { return _abilityList; }
            set { _abilityList = value; }
        }

        private List<AbilityWrapper> _abilList;
        public List<AbilityWrapper> TheAbilityList {
            get
            {
                if (_abilList == null) { _abilList = new List<AbilityWrapper>(AbilityList.Values); }
                return _abilList;
            }
        }
        private List<AbilityWrapper> _abilListThatDMGs;
        public List<AbilityWrapper> DamagingAbilities {
            get
            {
                if (_abilListThatDMGs == null) { _abilListThatDMGs = new List<AbilityWrapper>(AbilityList.Values).FindAll(e => e.IsDamaging); }
                return _abilListThatDMGs;
            }
        }
        private List<AbilityWrapper> _abilListThatMaints;
        public List<AbilityWrapper> MaintenanceAbilities {
            get
            {
                if (_abilListThatMaints == null) { _abilListThatMaints = new List<AbilityWrapper>(AbilityList.Values).FindAll(e => e.Ability.IsMaint); }
                return _abilListThatMaints;
            }
        }
        private List<AbilityWrapper> _abilListThatGCDs;
        public List<AbilityWrapper> AbilityListThatGCDs {
            get
            {
                if (_abilListThatGCDs == null) { _abilListThatGCDs = new List<AbilityWrapper>(AbilityList.Values).FindAll(e => e.Ability.UsesGCD); }
                return _abilListThatGCDs;
            }
        }

        public AbilityWrapper GetWrapper<T>() { return AbilityList[typeof(T)]; }

        public float _HPS_TTL;
        public float _DPS_TTL_O20, _DPS_TTL_U20;
        public string GCDUsage = "";
        protected CharacterCalculationsDPSWarr calcs = null;
        
        public bool _needDisplayCalcs = true;
        
        protected float TimeLostGCDsO20, TimeLostGCDsU20;
        protected float RageGainedWhileMoving;
        public float TimesStunned = 0f;
        public float TimesFeared = 0f;
        public float TimesRooted = 0f;
        public float TimesMoved = 0f;
        public float TimesSilenced = 0f;
        public float TimesDisarmed = 0f;
        
        public Skills.DeepWounds DW;

        #endregion
        #region Get/Set
        private DPSWarrCharacter dpswarrchar;
        public DPSWarrCharacter DPSWarrChar { get { return dpswarrchar; } set { dpswarrchar = value; } }

        protected float _cachedLatentGCD = 1.5f;
        public float LatentGCD { get { return _cachedLatentGCD; } }
        
        /// <summary>How many GCDs are in the rotation, based on fight duration and latency, all or if using Exec Spam then just Over 20%</summary>
        protected float NumGCDsO20 { get { return _cachedNumGCDsO20; } }
        protected float _cachedNumGCDsO20 = 0f;
        /// <summary>How many GCDs are in the rotation, based on fight duration and latency, under 20%</summary>
        protected float NumGCDsU20 { get { return _cachedNumGCDsU20; } }
        protected float _cachedNumGCDsU20 = 0f;
        /// <summary>How many GCDs are in the rotation, based on fight duration and latency, all or if using Exec Spam then just Over 20%</summary>
        protected float NumGCDsAll { get { return NumGCDsO20 + NumGCDsU20; } }

        /// <summary>How many GCDs have been used by the rotation, over 20%</summary>
        protected float GCDsUsedO20
        {
            get
            {
                float gcds = 0f;
                foreach (AbilityWrapper aw in AbilityListThatGCDs)
                {
                    gcds += aw.GCDUsageO20;// aw.numActivatesO20 * aw.ability.UseTime / LatentGCD;
                }
                return gcds;
            }
        }
        /// <summary>How many GCDs have been used by the rotation, under 20%</summary>
        protected float GCDsUsedU20
        {
            get
            {
                float gcds = 0f;
                foreach (AbilityWrapper aw in AbilityListThatGCDs)
                {
                    gcds += aw.GCDUsageU20;// aw.numActivatesU20 * aw.ability.UseTime / LatentGCD;
                }
                return gcds;
            }
        }
        /// <summary>How many GCDs have been used by the rotation (all)</summary>
        protected float GCDsUsedAll { get { return GCDsUsedO20 + GCDsUsedU20; } }
        /// <summary>How many GCDs are still available in the rotation</summary>
        protected float GCDsAvailableO20 { get { return Math.Max(0f, NumGCDsO20 - GCDsUsedO20 - TimeLostGCDsO20); } }
        /// <summary>How many GCDs are still available in the rotation sub 20%</summary>
        protected float GCDsAvailableU20 { get { return Math.Max(0f, NumGCDsU20 - GCDsUsedU20 - TimeLostGCDsU20); } }
        /// <summary>How many GCDs are still available in the rotation sub 20%</summary>
        protected float GCDsAvailableAll { get { return GCDsAvailableO20 + GCDsAvailableU20; } }
        
        #endregion
        #region Functions
        public virtual void Initialize(CharacterCalculationsDPSWarr ocalcs) {
            if (ocalcs == null) { ocalcs = new CharacterCalculationsDPSWarr(); }
            calcs = ocalcs;
            dpswarrchar.StatS = calcs.AverageStats;

            InitAbilities();

            // Whites
            calcs.Whites = DPSWarrChar.Whiteattacks;
        }
        public virtual void Initialize() { InitAbilities(); /*doIterations();*/ }

        public virtual void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalculations) {
            _needDisplayCalcs = needsDisplayCalculations;
        }

        protected virtual void InitAbilities() {
#if DEBUG
            //string info = "Before";
#endif
            try
            {
                InvalidateAbilityLists();
                // Whites
                DPSWarrChar.Whiteattacks.InvalidateCache();
                // Anti-Debuff
#if DEBUG
                //info = "Anti-Debuff";
#endif
                AddAbility(new AbilityWrapper(new Skills.HeroicFury(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.EveryManForHimself(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.EscapeArtist(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.WillOfTheForsaken(DPSWarrChar)));
                // Movement
#if DEBUG
                //info = "Movement";
#endif
                AddAbility(new AbilityWrapper(new Skills.Charge(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.Intercept(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.HeroicLeap(DPSWarrChar)));
                // Rage Generators
#if DEBUG
                //info = "Rage Generators";
#endif
                AddAbility(new AbilityWrapper(new Skills.SecondWind(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.BerserkerRage(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.DeadlyCalm(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.InnerRage(DPSWarrChar)));
                // Maintenance
#if DEBUG
                //info = "Maintenance";
#endif
                AddAbility(new AbilityWrapper(new Skills.BattleShout(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.CommandingShout(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.RallyingCry(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.DemoralizingShout(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.SunderArmor(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.Hamstring(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.EnragedRegeneration(DPSWarrChar)));
                // Periodics
#if DEBUG
                //info = "Periodics";
#endif
                AddAbility(new AbilityWrapper(new Skills.HeroicThrow(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.ShatteringThrow(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.SweepingStrikes(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.DeathWish(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.Recklessness(DPSWarrChar)));

                // Arms abilities
#if DEBUG
                //info = "Arms abilities";
#endif
                AddAbility(new AbilityWrapper(new Skills.ColossusSmash(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.Bladestorm(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.MortalStrike(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.Rend(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.ThunderClap(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.Overpower(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.TasteForBlood(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.VictoryRush(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.Cleave(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.HeroicStrike(DPSWarrChar)));
                AddAbility(new AbilityWrapper(new Skills.Execute(DPSWarrChar)));
                Slam SL = new Skills.Slam(DPSWarrChar);
                AddAbility(new AbilityWrapper(SL)); // Slam used by Bloodsurge, so its shared
                AddAbility(new AbilityWrapper(new Skills.StrikesOfOpportunity(DPSWarrChar)));

                // Fury abilities
#if DEBUG
                //info = "Fury abilities";
#endif
                Skills.Ability WW = new Skills.Whirlwind(DPSWarrChar);
                AddAbility(new AbilityWrapper(WW));
                Ability BT = new Skills.Bloodthirst(DPSWarrChar);
                AddAbility(new AbilityWrapper(BT));
                AddAbility(new AbilityWrapper(new Skills.BloodSurge(DPSWarrChar, SL/*, WW*/, BT)));
                AddAbility(new AbilityWrapper(new Skills.RagingBlow(DPSWarrChar)));

                DW = new Skills.DeepWounds(DPSWarrChar);
            } catch (Exception ex) {
                new Base.ErrorBox() {
                    Title = "Error Initializing Rotation Abilities",
                    Function = "initAbilities()",
#if DEBUG
                    //Info = info,
#endif
                    TheException = ex,
                }.Show();
            }
        }

        private void AddAbility(AbilityWrapper abilWrapper)
        {
            try {
                AbilityList.Add(abilWrapper.Ability.GetType(), abilWrapper);
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error in adding an Ability Wrapper",
                    Function = "AddAbility()",
                    TheException = ex,
                }.Show();
            }
        }

        public virtual void DoIterations() { }

        protected virtual void CalcDeepWounds() {
            // Main Hand
            float mhActivates =
                /*Yellow  */CriticalYellowsOverDurMH +
                /*White   */DPSWarrChar.Whiteattacks.MHActivatesAll * DPSWarrChar.Whiteattacks.MHAtkTable.Crit;

            // Off Hand
            float ohActivates = (DPSWarrChar.CombatFactors.useOH ?
                // No OnAttacks for OH
                /*Yellow*/CriticalYellowsOverDurOH +
                /*White */DPSWarrChar.Whiteattacks.OHActivatesAll * DPSWarrChar.Whiteattacks.OHAtkTable.Crit
                : 0f);

            // Push to the Ability
            DW.SetAllAbilityActivates(mhActivates, ohActivates); 
        }

        public void InvalidateCache()
        {
            _atkOverDursO20 = new float[,,] {
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
            };
            _atkOverDursU20 = new float[,,] {
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
            };
            _atkOverDurs = new float[,,] {
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
                { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 }, },
            };
            /*for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        _atkOverDursO20[i, j, k] = -1f;
                        _atkOverDursU20[i, j, k] = -1f;
                        _atkOverDurs[i, j, k] = -1f;
                    }
                }
            }*/
        }

        #region Attacks over Duration
        private float[,,] _atkOverDursO20 = new float[5, 3, 3];
        private float[,,] _atkOverDursU20 = new float[5, 3, 3];
        private float[,,] _atkOverDurs = new float[5, 3, 3];
        public float GetAttackOverDurationO20(SwingResult swingResult, Hand hand, AttackType attackType)
        {
            if (_atkOverDursO20[(int)swingResult, (int)hand, (int)attackType] == -1f)
            {
                SetTableO20(swingResult, hand, attackType);
            }
            return _atkOverDursO20[(int)swingResult, (int)hand, (int)attackType];
        }
        public float GetAttackOverDurationU20(SwingResult swingResult, Hand hand, AttackType attackType)
        {
            if (_atkOverDursU20[(int)swingResult, (int)hand, (int)attackType] == -1f)
            {
                SetTableU20(swingResult, hand, attackType);
            }
            return _atkOverDursU20[(int)swingResult, (int)hand, (int)attackType];
        }
        public float GetAttackOverDuration(SwingResult swingResult, Hand hand, AttackType attackType)
        {
            if (_atkOverDurs[(int)swingResult, (int)hand, (int)attackType] == -1f)
            {
                SetTable(swingResult, hand, attackType);
            }
            return _atkOverDurs[(int)swingResult, (int)hand, (int)attackType];
        }
        private void SetTableO20(SwingResult sr, Hand h, AttackType at)
        {
            float count = 0f;
            float mod;
            CombatTable table;

            if (at != AttackType.White) {
                foreach (AbilityWrapper abil in DamagingAbilities)
                {
                    if (!abil.Ability.Validated) { continue; }
                    if (h != Hand.OH) {
                        table = abil.Ability.MHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.NumActivatesO20 * abil.Ability.AvgTargets * abil.Ability.SwingsPerActivate * mod;
                    }
                    if (h != Hand.MH && DPSWarrChar.CombatFactors.useOH && abil.Ability.SwingsOffHand)
                    {
                        table = abil.Ability.OHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.NumActivatesO20 * abil.Ability.AvgTargets * mod;
                    }
                }
            }
            if (at != AttackType.Yellow) {
                if (h != Hand.OH) {
                    table = DPSWarrChar.Whiteattacks.MHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += DPSWarrChar.Whiteattacks.MHActivatesO20 * mod;
                }
                if (h != Hand.MH && DPSWarrChar.CombatFactors.useOH)
                {
                    table = DPSWarrChar.Whiteattacks.OHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += DPSWarrChar.Whiteattacks.OHActivatesO20 * mod;
                }
            }
            
            _atkOverDursO20[(int)sr, (int)h, (int)at] = count;
        }
        private void SetTableU20(SwingResult sr, Hand h, AttackType at)
        {
            float count = 0f;
            float mod;
            CombatTable table;

            if (at != AttackType.White)
            {
                foreach (AbilityWrapper abil in DamagingAbilities)
                {
                    if (!abil.Ability.Validated) { continue; }
                    if (h != Hand.OH) {
                        table = abil.Ability.MHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.NumActivatesU20 * abil.Ability.AvgTargets * abil.Ability.SwingsPerActivate * mod;
                    }
                    if (h != Hand.MH && DPSWarrChar.CombatFactors.useOH && abil.Ability.SwingsOffHand)
                    {
                        table = abil.Ability.OHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.NumActivatesU20 * abil.Ability.AvgTargets * mod;
                    }
                }
            }
            if (at != AttackType.Yellow) {
                if (h != Hand.OH) {
                    table = DPSWarrChar.Whiteattacks.MHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += DPSWarrChar.Whiteattacks.MHActivatesU20 * mod;
                }
                if (h != Hand.MH && DPSWarrChar.CombatFactors.useOH)
                {
                    table = DPSWarrChar.Whiteattacks.OHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += DPSWarrChar.Whiteattacks.OHActivatesU20 * mod;
                }
            }

            _atkOverDursU20[(int)sr, (int)h, (int)at] = count;
        }
        private void SetTable(SwingResult sr, Hand h, AttackType at)
        {
            float count = 0f;
            float mod;
            CombatTable table;

            if (at != AttackType.White)
            {
                // pulling function once to save processing
                List<AbilityWrapper> dmgAbils = DamagingAbilities;
                foreach (AbilityWrapper abil in dmgAbils)
                {
                    if (!abil.Ability.Validated)
                    {
                        continue;
                    }
                    if (h != Hand.OH)
                    {
                        table = abil.Ability.MHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.AllNumActivates * abil.Ability.AvgTargets * abil.Ability.SwingsPerActivate * mod;
                    }
                    if (h != Hand.MH && DPSWarrChar.CombatFactors.useOH && abil.Ability.SwingsOffHand)
                    {
                        table = abil.Ability.OHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.AllNumActivates * abil.Ability.AvgTargets * mod;
                    }
                }
            }
            if (at != AttackType.Yellow)
            {
                if (h != Hand.OH)
                {
                    table = DPSWarrChar.Whiteattacks.MHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += DPSWarrChar.Whiteattacks.MHActivatesAll * mod;
                }
                if (h != Hand.MH && DPSWarrChar.CombatFactors.useOH)
                {
                    table = DPSWarrChar.Whiteattacks.OHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += DPSWarrChar.Whiteattacks.OHActivatesAll * mod;
                }
            }

            _atkOverDurs[(int)sr, (int)h, (int)at] = count;
        }
        private static float GetTableFromSwingResult(SwingResult sr, CombatTable table)
        {
            if (table == null) return 0f;
            switch (sr)
            {
                case SwingResult.Attempt: return 1f;
                case SwingResult.Critical:    return table.Crit;
                case SwingResult.Dodge:   return table.Dodge;
                case SwingResult.Land:    return table.AnyLand;
                case SwingResult.Parry:   return table.Parry;
                default:                  return 0f;
            }
        }

        public float AttemptedAtksOverDurO20 { get { return GetAttackOverDurationO20(SwingResult.Attempt, Hand.Both, AttackType.Both); } }
        public float AttemptedAtksOverDurU20 { get { return GetAttackOverDurationU20(SwingResult.Attempt, Hand.Both, AttackType.Both); } }
        public float AttemptedAtksOverDur { get { return GetAttackOverDuration(SwingResult.Attempt, Hand.Both, AttackType.Both); } }

        public float AttemptedYellowsOverDur { get { return GetAttackOverDuration(SwingResult.Attempt, Hand.Both, AttackType.Yellow); } }

        public float LandedAtksOverDurO20 { get { return GetAttackOverDurationO20(SwingResult.Land, Hand.Both, AttackType.Both); } }
        public float LandedAtksOverDurU20 { get { return GetAttackOverDurationU20(SwingResult.Land, Hand.Both, AttackType.Both); } }
        public float LandedAtksOverDur { get { return GetAttackOverDuration(SwingResult.Land, Hand.Both, AttackType.Both); } }

        public float CriticalAtksOverDur { get { return GetAttackOverDuration(SwingResult.Critical, Hand.Both, AttackType.Both); } }

        public float AttemptedAtksOverDurMH { get { return GetAttackOverDuration(SwingResult.Attempt, Hand.MH, AttackType.Both); } }
        public float LandedAtksOverDurMH { get { return GetAttackOverDuration(SwingResult.Land, Hand.MH, AttackType.Both); } }

        public float AttemptedAtksOverDurOH { get { return GetAttackOverDuration(SwingResult.Attempt, Hand.OH, AttackType.Both); } }
        public float LandedAtksOverDurOH { get { return GetAttackOverDuration(SwingResult.Land, Hand.OH, AttackType.Both); } }

        public float CriticalYellowsOverDur { get { return GetAttackOverDuration(SwingResult.Critical, Hand.Both, AttackType.Both); } }
        public float LandedYellowsOverDur { get { return GetAttackOverDuration(SwingResult.Land, Hand.Both, AttackType.Both); } }

        public float CriticalYellowsOverDurMH { get { return GetAttackOverDuration(SwingResult.Critical, Hand.MH, AttackType.Yellow); } }
        public float CriticalYellowsOverDurOH { get { return GetAttackOverDuration(SwingResult.Critical, Hand.OH, AttackType.Yellow); } }

        public float DodgedAttacksOverDur { get { return GetAttackOverDuration(SwingResult.Dodge, Hand.Both, AttackType.Both); } }
        public float DodgedYellowsOverDur { get { return GetAttackOverDuration(SwingResult.Dodge, Hand.Both, AttackType.Yellow); } }

        public float ParriedAttacksOverDur { get { return GetAttackOverDuration(SwingResult.Parry, Hand.Both, AttackType.Both); } }
        public float ParriedYellowsOverDur { get { return GetAttackOverDuration(SwingResult.Parry, Hand.Both, AttackType.Yellow); } }

        public virtual float CritHSSlamOverDur {
            get {
                AbilityWrapper HS = GetWrapper<HeroicStrike>();
//                AbilityWrapper SL = GetWrapper<Slam>();
//                AbilityWrapper BS = GetWrapper<BloodSurge>();
                return HS.AllNumActivates*HS.Ability.MHAtkTable.Crit;
//                       SL.AllNumActivates * SL.Ability.MHAtkTable.Crit * SL.Ability.AvgTargets +
//                       BS.AllNumActivates * BS.Ability.MHAtkTable.Crit * BS.Ability.AvgTargets;
            }
        }

        #endregion

        /// <summary>This is used by Fury</summary>
        public float MaintainCDs {
            get {
                float cds = 0f;
                foreach (AbilityWrapper aw in MaintenanceAbilities) cds += aw.AllNumActivates;
                return cds;
            }
        }
        #endregion
        #region Rage Calcs
        static float RageMod = 2.5f / 453.3f;
        protected float _berserkerUptime = -1f;
        protected float BerserkerUptime {
            get {
                if (_berserkerUptime == -1f) {
                    _berserkerUptime = TalentsAsSpecialEffects.BerserkerRage.GetAverageUptime(0, 1f, DPSWarrChar.CombatFactors.CMHItemSpeed, (DPSWarrChar.CalcOpts.SE_UseDur ? FightDuration : 0f));
                }
                return _berserkerUptime;
            }
        }
        protected float _armorDamageReduction = -1f;
        protected float ArmorDamageReduction {
            get {
                if (_armorDamageReduction == -1f) {
                    _armorDamageReduction = StatConversion.GetArmorDamageReduction(DPSWarrChar.BossOpts.Level, DPSWarrChar.Char.Level, DPSWarrChar.StatS.Armor, 0, 0);
                }
                return _armorDamageReduction;
            }
        }
        protected virtual float RageGenOverDurIncDmg {
            get {
                float incRage = 0f;
                List<Attack> atks = DPSWarrChar.BossOpts.Attacks.FindAll(a => a.AffectsRole[PLAYER_ROLES.MeleeDPS]);
                foreach (Attack a in atks) {
                    incRage += Math.Min(100, a.DamagePerHit // Raw damage per hit
                                            * (1f - ArmorDamageReduction) // Drop out damage reduced by armor
                                            * (1f - DPSWarrChar.StatS.DamageTakenReductionMultiplier) // Drop out damage taken modifier
                                            * RageMod // The modifier that determines how much rage we get out of it
                                            * (1f + (DPSWarrChar.CalcOpts.M_BerserkerRage ? BerserkerUptime : 0f))) // Bonus from Berserker Rage

                                            * (FightDuration / a.AttackSpeed) // Number of times to occur in fight, if it were the whole fight
                                            * a.FightUptimePercent; // The uptime in the fight
                }
                return incRage;
            }
        }

        protected virtual float RageGenOverDurAngerO20 { get { return DPSWarrChar.CombatFactors.FuryStance ? 0f : (1.0f / 3.0f) * FightDurationO20; } }
        protected virtual float RageGenOverDurAngerU20 { get { return DPSWarrChar.CombatFactors.FuryStance ? 0f : (1.0f / 3.0f) * FightDurationU20; } }
        /// <summary>Anger Management is an Arms Spec Bonus in Cata, 1 rage every 3 seconds</summary>
        protected virtual float RageGenOverDurAnger { get { return RageGenOverDurAngerO20 + (DPSWarrChar.CalcOpts.M_ExecuteSpam ? RageGenOverDurAngerU20 : 0f); } }
        
        protected virtual float RageGenOverDurOtherO20 {
            get {
                float rage = RageGenOverDurAngerO20               // Anger Management Talent
                            + RageGenOverDurIncDmg * TimeOver20Perc             // Damage from the bosses
                            + (100f * DPSWarrChar.StatS.ManaorEquivRestore) * TimeOver20Perc // 0.02f becomes 2f
                            + DPSWarrChar.StatS.BonusRageGen * TimeOver20Perc;               // Bloodrage, Berserker Rage, Mighty Rage Pot

                foreach (AbilityWrapper aw in TheAbilityList) { if (aw.AllRage < 0) { rage += (-1f) * aw.RageO20; } }

                return rage;
            }
        }
        protected virtual float RageGenOverDurOtherU20 {
            get {
                float rage = RageGenOverDurAngerU20               // Anger Management Talent
                            + RageGenOverDurIncDmg * TimeUndr20Perc              // Damage from the bosses
                            + (100f * DPSWarrChar.StatS.ManaorEquivRestore) * TimeUndr20Perc  // 0.02f becomes 2f
                            + DPSWarrChar.StatS.BonusRageGen * TimeUndr20Perc;                // Bloodrage, Berserker Rage, Mighty Rage Pot

                foreach (AbilityWrapper aw in TheAbilityList) { if (aw.AllRage < 0) { rage += (-1f) * aw.RageU20; } }

                return rage;
            }
        }
        protected virtual float RageGenOverDurOther { get { return RageGenOverDurOtherO20 + (DPSWarrChar.CalcOpts.M_ExecuteSpam ? RageGenOverDurOtherU20 : 0f); } }


        /*protected float RageMOD_DeadlyCalm {
            get {
                return 1f - (CalcOpts.M_DeadlyCalm && !CombatFactors.FuryStance && Talents.DeadlyCalm > 0 ? 10f / 120f : 0f);
            }
        }*/
        protected float RageModBattleTrance {
            get {
                AbilityWrapper aw;
                if(DPSWarrChar.Talents.SingleMindedFury > 0 || DPSWarrChar.Talents.TitansGrip > 0)
                    aw = GetWrapper<Bloodthirst>();
                else
                    aw = GetWrapper<MortalStrike>();
                if (DPSWarrChar.Talents.BattleTrance == 0 || aw.AllNumActivates <= 0) { return 1f; }
                float numAffectedItems = TalentsAsSpecialEffects.BattleTrance[DPSWarrChar.Talents.BattleTrance].GetAverageProcsPerSecond(
                    FightDurationO20 / aw.NumActivatesO20, aw.Ability.MHAtkTable.AnyLand, 3.3f, FightDurationO20)
                    * FightDurationO20;
                float percAffectedVsUnAffected = numAffectedItems / (AttemptedAtksOverDurO20 * TimeOver20Perc);
                return 1f - percAffectedVsUnAffected;
            }
        }
        protected float RageModTotal { get { return /*RageMOD_DeadlyCalm * */ (1f + DPSWarrChar.StatS.RageCostMultiplier); } }

        private float _fightDur = -1f, _fightDurO20 = -1f, _fightDurU20 = -1f;
        public float FightDuration { get { if (_fightDur == -1) { _fightDur = DPSWarrChar.BossOpts.BerserkTimer; } return _fightDur; } }
        public float FightDurationO20 { get { if (_fightDurO20 == -1) { _fightDurO20 = FightDuration * TimeOver20Perc; } return _fightDurO20; } }
        public float FightDurationU20 { get { if (_fightDurU20 == -1) { _fightDurU20 = FightDuration * TimeUndr20Perc; } return _fightDurU20; } }

        private float _timeOver20Perc = -1f, _timeUndr20Perc = -1f;
        public float TimeOver20Perc { get { if (_timeOver20Perc == -1f) { _timeOver20Perc = (DPSWarrChar.CalcOpts.M_ExecuteSpam ? 1f - (float)DPSWarrChar.BossOpts.Under20Perc : 1f); } return _timeOver20Perc; } }
        public float TimeUndr20Perc { get { if (_timeUndr20Perc == -1f) { _timeUndr20Perc = (DPSWarrChar.CalcOpts.M_ExecuteSpam ?      (float)DPSWarrChar.BossOpts.Under20Perc : 0f); } return _timeUndr20Perc; } }
        // TODO: check new battle trance rage needed modification logic
        protected float RageNeededOverDurO20 {
            get
            {
                float rageModBtlTrance = RageModBattleTrance;
                float rage = 0f;
                foreach (AbilityWrapper aw in TheAbilityList) {
                    if (aw.RageO20 > 0f) {
                        // all abilities which costs more than 5 rage is affected by "battle trance"
                        if (aw.Ability.RageCost > 5f && aw.Ability.GetType() != typeof(MortalStrike) && aw.Ability.GetType() != typeof(Bloodthirst))
                            rage += aw.RageO20 * rageModBtlTrance;
                        else
                            rage += aw.RageO20;
                    }
                }
                //rage *= RageMOD_DeadlyCalm; // Deadly Calm makes your abilities cost no rage for 10 sec every 2 min.
                return rage;
            }
        }
        protected float RageNeededOverDurU20 {
            get {
                float rage = 0f;
                float rageModBtlTrance = RageModBattleTrance;
                foreach (AbilityWrapper aw in TheAbilityList)
                {
                    if (aw.RageU20 > 0f) {
                        if (aw.Ability.RageCost > 5f && aw.Ability.GetType() != typeof(MortalStrike) && aw.Ability.GetType() != typeof(Bloodthirst))
                            rage += aw.RageU20 * rageModBtlTrance;
                        else
                            rage += aw.RageU20;
                    }
                }
                //rage *= RageMOD_DeadlyCalm; // Deadly Calm makes your abilities cost no rage for 10 sec every 2 min.
                return rage;
            }
        }
        protected float RageNeededOverDur { get { return RageNeededOverDurO20 + (DPSWarrChar.CalcOpts.M_ExecuteSpam ? RageNeededOverDurU20 : 0f); } }
        #endregion

        #region AddAnItem(s)
        /// <summary>
        /// Adds every maintenance ability to the rotation
        /// </summary>
        /// <param name="totalPercTimeLost">Time lost due to Boss Handler options</param>
        /// <returns>Change in rage from these abilities</returns>
        protected float DoMaintenanceActivates(float totalPercTimeLost)
        {
            float netRage = 0f;

            foreach (AbilityWrapper aw in MaintenanceAbilities)
            {
                netRage += AddMaintenanceAbility(totalPercTimeLost, aw);
            }
            if (netRage != 0f && _needDisplayCalcs) GCDUsage += Environment.NewLine;
            return netRage;
        }

        /// <summary>
        /// Adds a maintenance ability to the rotation if it's been validated
        /// </summary>
        /// <param name="totalPercTimeLost">Time lost due to stun/fear/movement</param>
        /// <param name="abil">The ability to add</param>
        /// <returns>The final result from Abil.GetRageUseOverDur</returns>
        private float AddMaintenanceAbility(float totalPercTimeLost, AbilityWrapper aw)
        {
            if (!aw.Ability.Validated) { return 0f; }

            // If we are using Execute phase, distribute the abilities like they should be
            float Abil_GCDsO20 = Math.Min(GCDsAvailableO20, aw.Ability.Activates * (1f - totalPercTimeLost) * TimeOver20Perc);
            float Abil_GCDsU20 = Math.Min(GCDsAvailableU20, aw.Ability.Activates * (1f - totalPercTimeLost) * TimeUndr20Perc);
            aw.NumActivatesO20 = Abil_GCDsO20;
            aw.NumActivatesU20 = Abil_GCDsU20;
            if (_needDisplayCalcs && aw.AllNumActivates > 0) {
                GCDUsage += string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                    aw.AllNumActivates, aw.NumActivatesO20, aw.NumActivatesU20,
                    aw.Ability.Name, (!aw.Ability.UsesGCD ? " (Doesn't use GCDs)" : ""));
            }

            _HPS_TTL += aw.AllHPS;
            _DPS_TTL_O20 += aw.DPS_O20;
            _DPS_TTL_U20 += aw.DPS_U20;
            // This ability doesn't use rage
            if (aw.Ability.RageCost == -1) { return 0f; }
            // The ability GENERATES rage (the negative will be double-negatived and added to the rage pool)
            if (aw.Ability.RageCost <  -1) { return aw.Ability.RageCost * aw.AllNumActivates; }
            // The ability USES rage (the positive will be substracted from the rage pool) and since it uses rage, it may get a rage cost effect on it
            if (aw.Ability.RageCost >   0) { return aw.Ability.RageCost * aw.AllNumActivates * RageModTotal; }
            // If it didn't fall into those categories, something is wrong, so don't affect rage
            return 0f;
        }
        #endregion

        #region Lost Time due to Combat Factors
        /// <summary>Calculates percentage of time lost due to moving, being rooted, etc</summary>
        /// <returns>Percentage of time lost as a float</returns>
        protected float CalculateTimeLost()
        {
            TimeLostGCDsO20 = TimeLostGCDsU20 = 0;
            RageGainedWhileMoving = 0;

            float percTimeInFearRootStunMove = 0f;
            try {
                percTimeInFearRootStunMove = CalculateFearRootStunMove();
            }catch(Exception ex) {
                new Base.ErrorBox() {
                    Title = "Error Getting Time Lost Calcs",
                    Function = "CalculateTimeLost()",
                    TheException = ex,
                }.Show();
            }
            return Math.Min(1f, percTimeInFearRootStunMove);
        }
        
        private float CalculateFearRootStunMove()
        {
            #region Validation
            // If there are no ACTIVE Fears/Roots/Stuns/Moves/Sielences/Disarms, don't process
            // We aren't processing Silences yet, but I still want the code for it in place
            if (!DPSWarrChar.BossOpts.FearingTargs && !DPSWarrChar.BossOpts.RootingTargs && !DPSWarrChar.BossOpts.StunningTargs && !DPSWarrChar.BossOpts.MovingTargs /*&& !DPSWarrChar.BossOpts.SilencingTargs*/ && !DPSWarrChar.BossOpts.DisarmingTargs) { return 0f; }

            // If they are active but have counts of 0, don't process
            if (!(DPSWarrChar.BossOpts.FearingTargs && DPSWarrChar.BossOpts.Fears.Count > 0)
                && !(DPSWarrChar.BossOpts.RootingTargs && DPSWarrChar.BossOpts.Roots.Count > 0)
                && !(DPSWarrChar.BossOpts.StunningTargs && DPSWarrChar.BossOpts.Stuns.Count > 0)
                && !(DPSWarrChar.BossOpts.MovingTargs && DPSWarrChar.BossOpts.Moves.Count > 0)
                //&& !(DPSWarrChar.BossOpts.SilencingTargs && DPSWarrChar.BossOpts.Silences.Count > 0)
                && !(DPSWarrChar.BossOpts.DisarmingTargs && DPSWarrChar.BossOpts.Disarms.Count > 0))
            { return 0f; }

            // Generate the master list
            List<ImpedanceWithType> allImps = new List<ImpedanceWithType>();
            if (DPSWarrChar.BossOpts.FearingTargs && DPSWarrChar.BossOpts.Fears.Count > 0) { foreach (Impedance i in DPSWarrChar.BossOpts.Fears) { if (i.Validate && i.AffectsRole[PLAYER_ROLES.MeleeDPS]) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Fear }); } } }
            if (DPSWarrChar.BossOpts.RootingTargs && DPSWarrChar.BossOpts.Roots.Count > 0) { foreach (Impedance i in DPSWarrChar.BossOpts.Roots) { if (i.Validate && i.AffectsRole[PLAYER_ROLES.MeleeDPS]) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Root }); } } }
            if (DPSWarrChar.BossOpts.StunningTargs && DPSWarrChar.BossOpts.Stuns.Count > 0) { foreach (Impedance i in DPSWarrChar.BossOpts.Stuns) { if (i.Validate && i.AffectsRole[PLAYER_ROLES.MeleeDPS]) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Stun }); } } }
            if (DPSWarrChar.BossOpts.MovingTargs && DPSWarrChar.BossOpts.Moves.Count > 0) { foreach (Impedance i in DPSWarrChar.BossOpts.Moves) { if (i.Validate && i.AffectsRole[PLAYER_ROLES.MeleeDPS]) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Move }); } } }
            //if (DPSWarrChar.BossOpts.SilencingTargs && DPSWarrChar.BossOpts.Silences.Count > 0) { foreach (Impedance i in DPSWarrChar.BossOpts.Silences) { if (i.Validate && i.AffectsRole[PLAYER_ROLES.MeleeDPS]) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Silence }); } } }
            if (DPSWarrChar.BossOpts.DisarmingTargs && DPSWarrChar.BossOpts.Disarms.Count > 0) { foreach (Impedance i in DPSWarrChar.BossOpts.Disarms) { if (i.Validate && i.AffectsRole[PLAYER_ROLES.MeleeDPS]) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Disarm }); } } }
            
            // If the array has count 0 for some reason, don't process
            if (allImps.Count <= 0) { return 0; }
            #endregion

            #region Variable Declaration
            float[] percTimeIn_ = { 0f, 0f, 0f, 0f, 0f, 0f, };
            float[] timelostwhile_ = { 0f, 0f, 0f, 0f, 0f, 0f, };
            TimesFeared = TimesRooted = TimesStunned = TimesMoved = TimesSilenced = TimesDisarmed = 0f;
            float baseDur = 0f, acts = 0f, maxTimeRegain = 0f, chanceYouAreAffected = 1f, timelostwhileaffected = 0f;
            float MovementSpeed = 7f * (1f + DPSWarrChar.StatS.MovementSpeed); // 7 yards per sec * 1.08 (if have bonus) = 7.56
            #endregion

            #region Abilities that can Break this stuff
            // Heroic Fury (Fury Warrior): Roots
            AbilityWrapper HF = GetWrapper<HeroicFury>();          float HFMaxActs = HF.Ability.Activates; float newActsHF = 0f; float reducedDurHF = 0f; HF.NumActivatesO20 = HF.NumActivatesU20 = 0f;
            // Berserker Rage (Warrior): Fears, Stuns
            AbilityWrapper BZ = GetWrapper<BerserkerRage>();       float BZMaxActs = BZ.Ability.Activates; float newActsBZ = 0f; float reducedDurBZ = 0f; BZ.NumActivatesO20 = BZ.NumActivatesU20 = 0f;
            // Escape Artist (Gnome): Roots/Snares
            AbilityWrapper EA = GetWrapper<EscapeArtist>();        float EAMaxActs = EA.Ability.Activates; float newActsEA = 0f; float reducedDurEA = 0f; EA.NumActivatesO20 = EA.NumActivatesU20 = 0f;
            // Will of the Forsaken (Undead): Fears, Charms, Sleeps (only model Fears)
            AbilityWrapper WF = GetWrapper<WillOfTheForsaken>();   float WFMaxActs = WF.Ability.Activates; float newActsWF = 0f; float reducedDurWF = 0f; WF.NumActivatesO20 = WF.NumActivatesU20 = 0f;
            // Every Man For Himself (Human): Fears, Roots, Stuns, Charms, Sleeps (Don't model Charms/Sleeps)
            AbilityWrapper EM = GetWrapper<EveryManForHimself>();  float EMMaxActs = EM.Ability.Activates; float newActsEM = 0f; float reducedDurEM = 0f; EM.NumActivatesO20 = EM.NumActivatesU20 = 0f;
            // Heroic Leap (Warrior): Moves
            AbilityWrapper HL = GetWrapper<HeroicLeap>();          float HLMaxActs = HL.Ability.Activates; float newActsHL = 0f; float reducedDurHL = 0f; HL.NumActivatesO20 = HL.NumActivatesU20 = 0f;
            // Charge (Warrior/Arms/Juggernaught): Moves
            AbilityWrapper CH = GetWrapper<Charge>();              float CHMaxActs = CH.Ability.Activates; float newActsCH = 0f; float reducedDurCH = 0f; CH.NumActivatesO20 = CH.NumActivatesU20 = 0f;
            // Intercept (Warrior/Fury): Moves
            AbilityWrapper IN = GetWrapper<Intercept>();           float INMaxActs = IN.Ability.Activates; float newActsIN = 0f; float reducedDurIN = 0f; IN.NumActivatesO20 = IN.NumActivatesU20 = 0f;
            #endregion

            foreach (ImpedanceWithType iwt in allImps)
            {
                #region Validation and Variable Setup
                chanceYouAreAffected = iwt.imp.Chance;
                if (chanceYouAreAffected <= 0) { continue; } // If it can't affect you, skip it
                iwt.imp.FightDuration = FightDuration;
                acts = (FightDuration / (iwt.imp.Frequency < FightDuration ? iwt.imp.Frequency : FightDuration)) * iwt.imp.FightUptimePercent;
                if (acts <= 0f || float.IsNaN(acts) || float.IsInfinity(acts)) { continue; } // If it never activates or is broken, skip it
                float statReducVal = iwt.type == ImpedanceTypes.Fear ? DPSWarrChar.StatS.FearDurReduc
                                   : iwt.type == ImpedanceTypes.Root ? DPSWarrChar.StatS.SnareRootDurReduc
                                   : iwt.type == ImpedanceTypes.Stun ? DPSWarrChar.StatS.StunDurReduc
                                   : iwt.type == ImpedanceTypes.Move ? DPSWarrChar.StatS.MovementSpeed
                                   //: iwt.type == ImpedanceTypes.Silence ? DPSWarrChar.StatS.SilenceDurReduc
                                   : iwt.type == ImpedanceTypes.Disarm ? DPSWarrChar.StatS.DisarmDurReduc
                                   : 0f;
                baseDur = Math.Max(0f, (iwt.imp.Duration / 1000f * (1f - statReducVal)));
                float actsRemainingToCounter = acts;
                // We need to reset these values or they act weird
                newActsHF = newActsBZ = newActsEA = newActsWF = newActsEM = newActsHL = newActsCH = newActsIN = 0f;
                #endregion

                #region Add the acts to the approprate Times____ed
                switch (iwt.type) {
                    case ImpedanceTypes.Fear: TimesFeared += acts; break;
                    case ImpedanceTypes.Root: TimesRooted += acts; break;
                    case ImpedanceTypes.Stun: TimesStunned += acts; break;
                    case ImpedanceTypes.Move: TimesMoved += acts; break;
                    //case ImpedanceTypes.Silence: TimesSilenced += acts; break;
                    case ImpedanceTypes.Disarm: TimesDisarmed += acts; break;
                    default: continue; // If it is not one of these types, skip it
                }
                #endregion

                #region PROCESS HEROIC FURY
                if ((actsRemainingToCounter > 0) // There are acts to counter
                    && (iwt.type == ImpedanceTypes.Root) // This is a Root
                    && (DPSWarrChar.Char.WarriorTalents.HeroicFury > 0) // We have the Fury Talent (qualifier for Heroic Fury)
                    && (HFMaxActs - HF.AllNumActivates > 0f) // We haven't already used up Heroic Fury
                    && (Math.Min(0f, (HF.Ability.GCDTime - baseDur)) < 0)) // We can regain time using Heroic Fury
                {
                    maxTimeRegain = Math.Max(0f, baseDur - HF.Ability.GCDTime);
                    // Determine how many we can still use
                    float availActs     = HFMaxActs - HF.AllNumActivates;
                    newActsHF           = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    HF.NumActivatesO20 += newActsHF * TimeOver20Perc;
                    HF.NumActivatesU20 += newActsHF * TimeUndr20Perc;
                    // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                    reducedDurHF = Math.Max(0f, baseDur - maxTimeRegain);
                    float percActdVsUnActd = newActsHF / actsRemainingToCounter;
                    timelostwhileaffected += (baseDur * actsRemainingToCounter * (1f - percActdVsUnActd) * chanceYouAreAffected)
                                           + (reducedDurHF * actsRemainingToCounter * percActdVsUnActd * chanceYouAreAffected);
                    // If there are still roots left to counter, lets hope something else can put it up
                    actsRemainingToCounter -= newActsHF;
                }
                #endregion

                #region PROCESS BERSERKER RAGE
                if ((actsRemainingToCounter > 0) // There are acts to counter
                    && (iwt.type == ImpedanceTypes.Fear || iwt.type == ImpedanceTypes.Stun) // This is a Fear OR Stun
                    //&& (Char.Class == CharacterClass.Warrior) // We are a Warrior (qualifier for Berserker Rage)
                    && (BZMaxActs - BZ.AllNumActivates > 0f) // We haven't already used up Berserker Rage
                    && (Math.Min(0f, (BZ.Ability.GCDTime - baseDur)) < 0)) // We can regain time using Berserker Rage
                {
                    maxTimeRegain = Math.Max(0f, baseDur - BZ.Ability.GCDTime);
                    // Determine how many we can still use
                    float availActs     = BZMaxActs - BZ.AllNumActivates;
                    newActsBZ           = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    BZ.NumActivatesO20 += newActsBZ * TimeOver20Perc;
                    BZ.NumActivatesU20 += newActsBZ * TimeUndr20Perc;
                    // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                    reducedDurBZ = Math.Max(0f, baseDur - maxTimeRegain);
                    float percActdVsUnActd = newActsBZ / actsRemainingToCounter;
                    timelostwhileaffected += (baseDur * actsRemainingToCounter * (1f - percActdVsUnActd) * chanceYouAreAffected)
                                           + (reducedDurBZ * actsRemainingToCounter * percActdVsUnActd * chanceYouAreAffected);
                    // If there are still roots left to counter, lets hope something else can put it up
                    actsRemainingToCounter -= newActsBZ;
                }
                #endregion

                #region PROCESS ESCAPE ARTIST
                if ((actsRemainingToCounter > 0) // There are acts to counter
                    && (iwt.type == ImpedanceTypes.Root) // This is a Root
                    && (DPSWarrChar.Char.Race == CharacterRace.Gnome) // We are a Gnome (qualifier for Escape Artist)
                    && (EAMaxActs - EA.AllNumActivates > 0f) // We haven't already used up Escape Artist
                    && (Math.Min(0f, (EA.Ability.GCDTime - baseDur)) < 0)) // We can regain time using Escape Artist
                {
                    maxTimeRegain = Math.Max(0f, baseDur - EA.Ability.GCDTime);
                    // Determine how many we can still use
                    float availActs     = EAMaxActs - EA.AllNumActivates;
                    newActsEA           = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    EA.NumActivatesO20 += newActsEA * TimeOver20Perc;
                    EA.NumActivatesU20 += newActsEA * TimeUndr20Perc;
                    // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                    reducedDurEA = Math.Max(0f, baseDur - maxTimeRegain);
                    float percActdVsUnActd = newActsEA / actsRemainingToCounter;
                    timelostwhileaffected += (baseDur * actsRemainingToCounter * (1f - percActdVsUnActd) * chanceYouAreAffected)
                                           + (reducedDurEA * actsRemainingToCounter * percActdVsUnActd * chanceYouAreAffected);
                    // If there are still roots left to counter, lets hope something else can put it up
                    actsRemainingToCounter -= newActsEA;
                }
                #endregion

                #region PROCESS WILL OF THE FORSAKEN
                if ((actsRemainingToCounter > 0) // There are acts to counter
                    && (iwt.type == ImpedanceTypes.Fear) // This is a Fear
                    && (DPSWarrChar.Char.Race == CharacterRace.Undead) // We are Undead (qualifier for Will of the Forsaken)
                    && (WFMaxActs - WF.AllNumActivates > 0f) // We haven't already used up Will of the Forsaken
                    && (Math.Min(0f, (WF.Ability.GCDTime - baseDur)) < 0)) // We can regain time using Will of the Forsaken
                {
                    maxTimeRegain = Math.Max(0f, baseDur - WF.Ability.GCDTime);
                    // Determine how many we can still use
                    float availActs     = WFMaxActs - WF.AllNumActivates;
                    newActsWF           = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    WF.NumActivatesO20 += newActsWF * TimeOver20Perc;
                    WF.NumActivatesU20 += newActsWF * TimeUndr20Perc;
                    // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                    reducedDurWF = Math.Max(0f, baseDur - maxTimeRegain);
                    float percActdVsUnActd = newActsWF / actsRemainingToCounter;
                    timelostwhileaffected += (baseDur * actsRemainingToCounter * (1f - percActdVsUnActd) * chanceYouAreAffected)
                                           + (reducedDurWF * actsRemainingToCounter * percActdVsUnActd * chanceYouAreAffected);
                    // If there are still roots left to counter, lets hope something else can put it up
                    actsRemainingToCounter -= newActsWF;
                }
                #endregion

                #region PROCESS EVERY MAN FOR HIMSELF
                if ((actsRemainingToCounter > 0) // There are acts to counter
                    && (iwt.type == ImpedanceTypes.Fear || iwt.type == ImpedanceTypes.Stun || iwt.type == ImpedanceTypes.Root) // This is a Fear OR Stun OR Root
                    && (DPSWarrChar.Char.Race == CharacterRace.Human) // We are a Human (qualifier for Every Man for Himself)
                    && (EMMaxActs - EM.AllNumActivates > 0f) // We haven't already used up Every Man for Himself
                    && (Math.Min(0f, (EM.Ability.GCDTime - baseDur)) < 0)) // We can regain time using Every Man for Himself
                {
                    maxTimeRegain = Math.Max(0f, baseDur - EM.Ability.GCDTime);
                    // Determine how many we can still use
                    float availActs     = EMMaxActs - EM.AllNumActivates;
                    newActsEM           = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    EM.NumActivatesO20 += newActsEM * TimeOver20Perc;
                    EM.NumActivatesU20 += newActsEM * TimeUndr20Perc;
                    // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                    reducedDurEM = Math.Max(0f, baseDur - maxTimeRegain);
                    float percActdVsUnActd = newActsEM / actsRemainingToCounter;
                    timelostwhileaffected += (baseDur * actsRemainingToCounter * (1f - percActdVsUnActd) * chanceYouAreAffected)
                                           + (reducedDurEM * actsRemainingToCounter * percActdVsUnActd * chanceYouAreAffected);
                    // If there are still roots left to counter, lets hope something else can put it up
                    actsRemainingToCounter -= newActsEM;
                }
                #endregion

                #region PROCESS HEROIC LEAP
                if ((actsRemainingToCounter > 0) // There are acts to counter
                    && (iwt.type == ImpedanceTypes.Move) // This is a Move
                    //&& (Char.Class == CharacterClass.Warrior) // We are a Warrior (qualifier for Heroic Leap)
                    && (HLMaxActs - HL.AllNumActivates > 0f) // We haven't already used up Heroic Leap
                    && ((HL.Ability.MinRange / MovementSpeed) < baseDur) // We meet the Minimum Range requirement for Heroic Leap
                    && (Math.Min(0f, HL.Ability.GCDTime - baseDur) < 0)) // Heroic Leap's GCD Time won't be more than what we need to save
                {
                    maxTimeRegain = Math.Max(0f, Math.Min(baseDur - HL.Ability.GCDTime, HL.Ability.MaxRange / MovementSpeed - HL.Ability.GCDTime)); // calc the actual max time we can regain
                    // Determine how many we can still use
                    float availActs = HLMaxActs - HL.AllNumActivates;
                    newActsHL = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    HL.NumActivatesO20 += newActsHL * TimeOver20Perc;
                    HL.NumActivatesU20 += newActsHL * TimeUndr20Perc;
                    // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                    reducedDurHL = Math.Max(0f, baseDur - maxTimeRegain);
                    float percActdVsUnActd = newActsHL / actsRemainingToCounter;
                    timelostwhileaffected += (baseDur * actsRemainingToCounter * (1f - percActdVsUnActd) * chanceYouAreAffected)
                                           + (reducedDurHL * actsRemainingToCounter * percActdVsUnActd * chanceYouAreAffected);
                    // If there are still roots left to counter, lets hope something else can put it up
                    actsRemainingToCounter -= newActsHL;
                }
                #endregion

                #region PROCESS CHARGE
                if ((actsRemainingToCounter > 0) // There are acts to counter
                    && (iwt.type == ImpedanceTypes.Move) // This is a Move
                    && (!DPSWarrChar.CombatFactors.FuryStance && DPSWarrChar.Char.WarriorTalents.Juggernaut > 0) // We are not in Fury Stance and have the Juggernaught Talent (qualifiers for Charge)
                    && (CHMaxActs - CH.AllNumActivates > 0f) // We haven't already used up Charge
                    && ((CH.Ability.MinRange / MovementSpeed) < baseDur) // We meet the Minimum Range requirement for Charge
                    && (Math.Min(0f, (CH.Ability.GCDTime - baseDur)) < 0)) // We can regain time using Charge
                {
                    maxTimeRegain = Math.Max(0f, Math.Min(baseDur - CH.Ability.GCDTime, CH.Ability.MaxRange / MovementSpeed - CH.Ability.GCDTime)); // calc the actual max time we can regain
                    // Determine how many we can still use
                    float availActs = CHMaxActs - CH.AllNumActivates;
                    newActsCH = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    CH.NumActivatesO20 += newActsCH * TimeOver20Perc;
                    CH.NumActivatesU20 += newActsCH * TimeUndr20Perc;
                    // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                    reducedDurCH = Math.Max(0f, baseDur - maxTimeRegain);
                    float percActdVsUnActd = newActsCH / actsRemainingToCounter;
                    timelostwhileaffected += (baseDur * actsRemainingToCounter * (1f - percActdVsUnActd) * chanceYouAreAffected)
                                           + (reducedDurCH * actsRemainingToCounter * percActdVsUnActd * chanceYouAreAffected);
                    // If there are still roots left to counter, lets hope something else can put it up
                    actsRemainingToCounter -= newActsCH;
                }
                #endregion

                #region PROCESS INTERCEPT
                if ((actsRemainingToCounter > 0) // There are acts to counter
                    && (iwt.type == ImpedanceTypes.Move) // This is a Move
                    && (DPSWarrChar.CombatFactors.FuryStance) // We are in Fury Stance (qualifier for Intercept)
                    && ((INMaxActs - IN.AllNumActivates) + (HFMaxActs - HF.AllNumActivates) > 0f) // We haven't already used up Intercept, though we might be able to use Heroic Fury to reset the cooldown
                    && ((IN.Ability.MinRange / MovementSpeed) < baseDur) // We meet the Minimum Range requirement for Intercept
                    && (Math.Min(0f, (IN.Ability.GCDTime - baseDur)) < 0)) // We can regain time using Intercept
                {
                    maxTimeRegain = Math.Max(0f, Math.Min(baseDur - IN.Ability.GCDTime, IN.Ability.MaxRange / MovementSpeed - IN.Ability.GCDTime)); // calc the actual max time we can regain
                    // Determine how many we can still use
                    float availActs = INMaxActs - IN.AllNumActivates;
                    newActsIN = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    IN.NumActivatesO20 += newActsIN * TimeOver20Perc;
                    IN.NumActivatesU20 += newActsIN * TimeUndr20Perc;
                    // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                    reducedDurIN = Math.Max(0f, baseDur - maxTimeRegain);
                    float percActdVsUnActd = newActsIN / actsRemainingToCounter;
                    timelostwhileaffected += (baseDur * actsRemainingToCounter * (1f - percActdVsUnActd) * chanceYouAreAffected)
                                           + (reducedDurIN * actsRemainingToCounter * percActdVsUnActd * chanceYouAreAffected);
                    // If there are still roots left to counter, lets hope something else can put it up
                    actsRemainingToCounter -= newActsIN;
                }
                #endregion

                #region PROCESS: There are acts to counter, but we have nothing to counter them
                if (actsRemainingToCounter > 0)
                {
                    timelostwhileaffected += baseDur * actsRemainingToCounter * chanceYouAreAffected;
                }
                #endregion

                #region Add the total amount of time lost of this type to the appropriate separation
                timelostwhile_[(int)iwt.type] += timelostwhileaffected;
                #endregion

                #region Tell User how much was lost to this instance of Type
                if (_needDisplayCalcs) {
                    switch (iwt.type)
                    {
                        case ImpedanceTypes.Fear: {
                            GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Lost to Fears\n",
                                TimesFeared, TimesFeared * TimeOver20Perc, TimesFeared * TimeUndr20Perc, baseDur, TimesFeared * baseDur);
                            break;
                        }
                        case ImpedanceTypes.Root: {
                            GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Lost to Roots\n",
                                TimesRooted, TimesRooted * TimeOver20Perc, TimesRooted * TimeUndr20Perc, baseDur, TimesRooted * baseDur);
                            break;
                        }
                        case ImpedanceTypes.Stun: {
                            GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Lost to Stuns\n",
                                TimesStunned, TimesStunned * TimeOver20Perc, TimesStunned * TimeUndr20Perc, baseDur, TimesStunned * baseDur);
                            break;
                        }
                        case ImpedanceTypes.Move: {
                            GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Lost to Moves\n",
                                TimesMoved, TimesMoved * TimeOver20Perc, TimesMoved * TimeUndr20Perc, baseDur, TimesMoved * baseDur);
                            break;
                        }
                        /*case ImpedanceTypes.Silence: {
                            GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Lost to Silences\n",
                                TimesSilenced, TimesSilenced * timeOver20, TimesSilenced * timeUndr20, baseDur, TimesSilenced * baseDur);
                            break;
                        }*/
                        case ImpedanceTypes.Disarm: {
                            GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Lost to Disarms\n",
                                TimesDisarmed, TimesDisarmed * TimeOver20Perc, TimesDisarmed * TimeUndr20Perc, baseDur, TimesDisarmed * baseDur);
                            break;
                        }
                        default: { break; } // Invalid type
                    }
                }
                #endregion

                #region Tell User how much was recovered per ability
                if (_needDisplayCalcs) {
                    if (newActsHF + newActsBZ + newActsEA + newActsWF + newActsEM + newActsHL + newActsCH + newActsIN <= 0)
                    {
                        GCDUsage += "You did not recover this because either there were not enough counters or it wasn't worth countering.\n";
                    } else {
                        GCDUsage += (newActsHF > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsHF, // Total Acts
                                newActsHF * TimeOver20Perc, // Acts Over 20
                                newActsHF * TimeUndr20Perc, // Acts Under 20
                                baseDur - reducedDurHF, // Amount Recovered Per
                                newActsHF * (baseDur - reducedDurHF), // Total Amount Recovered
                                HF.Ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsBZ > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsBZ, // Total Acts, limited by its other uses
                                newActsBZ * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsBZ * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurBZ, // Amount Recovered Per
                                newActsBZ * (baseDur - reducedDurBZ), // Total Amount Recovered
                                BZ.Ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsEA > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsEA, // Total Acts, limited by its other uses
                                newActsEA * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsEA * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurEA, // Amount Recovered Per
                                newActsEA * (baseDur - reducedDurEA), // Total Amount Recovered
                                EA.Ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsWF > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsWF, // Total Acts, limited by its other uses
                                newActsWF * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsWF * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurWF, // Amount Recovered Per
                                newActsWF * (baseDur - reducedDurWF), // Total Amount Recovered
                                WF.Ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsEM > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsEM, // Total Acts, limited by its other uses
                                newActsEM * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsEM * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurEM, // Amount Recovered Per
                                newActsEM * (baseDur - reducedDurEM), // Total Amount Recovered
                                EM.Ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsHL > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsHL, // Total Acts, limited by its other uses
                                newActsHL * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsHL * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurHL, // Amount Recovered Per
                                newActsHL * (baseDur - reducedDurHL), // Total Amount Recovered
                                HL.Ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsCH > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsCH, // Total Acts, limited by its other uses
                                newActsCH * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsCH * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurCH, // Amount Recovered Per
                                newActsCH * (baseDur - reducedDurCH), // Total Amount Recovered
                                CH.Ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsIN > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsIN, // Total Acts, limited by its other uses
                                newActsIN * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsIN * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurIN, // Amount Recovered Per
                                newActsIN * (baseDur - reducedDurIN), // Total Amount Recovered
                                IN.Ability.Name) // The Name
                            : "");
                    }
                }
                #endregion

                #region Take the lost time out of GCDs
                float times = 0f;
                switch (iwt.type) {
                    case ImpedanceTypes.Fear: { times = TimesFeared; break; }
                    case ImpedanceTypes.Root: { times = TimesRooted; break; }
                    case ImpedanceTypes.Stun: { times = TimesStunned; break; }
                    case ImpedanceTypes.Move: { times = TimesMoved; break; }
                    //case ImpedanceTypes.Silence: { times = TimesSilenced; break; }
                    case ImpedanceTypes.Disarm: { times = TimesDisarmed; break; }
                    default: { break; } // Invalid type
                }
                TimeLostGCDsO20 += Math.Min(NumGCDsO20, (baseDur * times * TimeOver20Perc) / LatentGCD);
                if (newActsHF > 0) TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDurHF * newActsHF * TimeOver20Perc) / LatentGCD);
                if (newActsBZ > 0) TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDurBZ * newActsBZ * TimeOver20Perc) / LatentGCD);
                if (newActsEA > 0) TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDurEA * newActsEA * TimeOver20Perc) / LatentGCD);
                if (newActsEM > 0) TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDurEM * newActsEM * TimeOver20Perc) / LatentGCD);
                if (newActsWF > 0) TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDurWF * newActsWF * TimeOver20Perc) / LatentGCD);
                if (newActsHL > 0) TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDurHL * newActsHL * TimeOver20Perc) / LatentGCD);
                if (newActsCH > 0) TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDurCH * newActsCH * TimeOver20Perc) / LatentGCD);
                if (newActsIN > 0) TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDurIN * newActsIN * TimeOver20Perc) / LatentGCD);

                TimeLostGCDsU20 += Math.Min(NumGCDsU20, (baseDur * times * TimeUndr20Perc) / LatentGCD);
                if (newActsHF > 0) TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDurHF * newActsHF * TimeUndr20Perc) / LatentGCD);
                if (newActsBZ > 0) TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDurBZ * newActsBZ * TimeUndr20Perc) / LatentGCD);
                if (newActsEA > 0) TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDurEA * newActsEA * TimeUndr20Perc) / LatentGCD);
                if (newActsEM > 0) TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDurEM * newActsEM * TimeUndr20Perc) / LatentGCD);
                if (newActsWF > 0) TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDurWF * newActsWF * TimeUndr20Perc) / LatentGCD);
                if (newActsHL > 0) TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDurHL * newActsHL * TimeUndr20Perc) / LatentGCD);
                if (newActsCH > 0) TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDurCH * newActsCH * TimeUndr20Perc) / LatentGCD);
                if (newActsIN > 0) TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDurIN * newActsIN * TimeUndr20Perc) / LatentGCD);
                #endregion
            }

            percTimeIn_[(int)ImpedanceTypes.Fear] = timelostwhile_[(int)ImpedanceTypes.Fear] / FightDuration;
            percTimeIn_[(int)ImpedanceTypes.Root] = timelostwhile_[(int)ImpedanceTypes.Root] / FightDuration;
            percTimeIn_[(int)ImpedanceTypes.Stun] = timelostwhile_[(int)ImpedanceTypes.Stun] / FightDuration;
            percTimeIn_[(int)ImpedanceTypes.Move] = timelostwhile_[(int)ImpedanceTypes.Move] / FightDuration;
            percTimeIn_[(int)ImpedanceTypes.Silence] = timelostwhile_[(int)ImpedanceTypes.Silence] / FightDuration;
            percTimeIn_[(int)ImpedanceTypes.Disarm] = timelostwhile_[(int)ImpedanceTypes.Disarm] / FightDuration;
            
            (GetWrapper<SecondWind>().Ability as SecondWind).NumStunsOverDur = TimesRooted + TimesStunned;

            return percTimeIn_[(int)ImpedanceTypes.Fear] + percTimeIn_[(int)ImpedanceTypes.Root] + percTimeIn_[(int)ImpedanceTypes.Stun]
                + percTimeIn_[(int)ImpedanceTypes.Move] /*+ percTimeInSilence*/ + percTimeIn_[(int)ImpedanceTypes.Disarm];
        }

        #region OLD METHODS
#if FALSE
        private float CalculateFear()
        {
            float percTimeInFear = 0f;
            if (DPSWarrChar.BossOpts.FearingTargs && DPSWarrChar.BossOpts.Fears.Count > 0)
            {
                float timeUnder20 = CalcOpts.M_ExecuteSpam ? (float)DPSWarrChar.BossOpts.Under20Perc : 0f;
                float timelostwhilefeared = 0f;
                float BaseFearDur = 0f, fearActs = 0f, reducedDur = 0f,
                      MaxTimeRegain = 0f,
                      ChanceYouAreFeared = 1f;
                AbilWrapper BZ = GetWrapper<BerserkerRage>();
                float BZMaxActs = BZ.ability.Activates;
                float BZActualActs = 0f;
                AbilWrapper WF = GetWrapper<WillOfTheForsaken>();
                float WFMaxActs = WF.ability.Activates - WF.allNumActivates;
                float WFOldActs = WF.allNumActivates;
                AbilWrapper EM = GetWrapper<EveryManForHimself>();
                float EMMaxActs = EM.ability.Activates - EM.allNumActivates;
                float EMOldActs = EM.allNumActivates;
                TimesFeared = 0f;
                foreach (Impedance f in DPSWarrChar.BossOpts.Fears)
                {
                    ChanceYouAreFeared = f.Chance;
                    BaseFearDur = Math.Max(0f, (f.Duration / 1000f * (1f - StatS.FearDurReduc)));
                    fearActs = FightDuration / f.Frequency;
                    if (fearActs > 0f)
                    {
                        TimesFeared += fearActs;
                        if (BZMaxActs - BZActualActs > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseFearDur - LatentGCD - CalcOpts.React / 1000f));
                            float BZNewActs = Math.Min(BZMaxActs - BZActualActs, fearActs);
                            BZActualActs += BZNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseFearDur - MaxTimeRegain);
                            float percBZdVsUnBZd = BZNewActs / fearActs;
                            timelostwhilefeared += (reducedDur * fearActs * percBZdVsUnBZd * ChanceYouAreFeared)
                                                 + (BaseFearDur * fearActs * (1f - percBZdVsUnBZd) * ChanceYouAreFeared);
                        }
                        else if (Char.Race == CharacterRace.Human && EMMaxActs - EM.allNumActivates > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseFearDur - LatentGCD - CalcOpts.React / 1000f));

                            float availEMacts = EM.ability.Activates - EM.allNumActivates;
                            float EMNewActs = Math.Min(fearActs, availEMacts);
                            EM.numActivatesO20 += EMNewActs * (1f - timeUnder20);
                            EM.numActivatesU20 += EMNewActs * timeUnder20;
                            //_emActs = EM.allNumActivates;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseFearDur - MaxTimeRegain);
                            float percEMdVsUnEMd = EMNewActs / fearActs;
                            timelostwhilefeared += (reducedDur * fearActs * percEMdVsUnEMd * ChanceYouAreFeared)
                                                 + (BaseFearDur * fearActs * (1f - percEMdVsUnEMd) * ChanceYouAreFeared);
                        }
                        else if (Char.Race == CharacterRace.Undead && WFMaxActs - WF.allNumActivates > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseFearDur - LatentGCD - CalcOpts.React / 1000f));

                            float availWFacts = WF.ability.Activates - WF.allNumActivates;
                            float WFNewActs = Math.Min(fearActs, availWFacts);
                            WF.numActivatesO20 += WFNewActs * (1f - timeUnder20);
                            WF.numActivatesU20 += WFNewActs * timeUnder20;
                            //_wfActs = WF.allNumActivates;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseFearDur - MaxTimeRegain);
                            float percWFdVsUnWFd = WFNewActs / fearActs;
                            timelostwhilefeared += (reducedDur * fearActs * percWFdVsUnWFd * ChanceYouAreFeared)
                                                 + (BaseFearDur * fearActs * (1f - percWFdVsUnWFd) * ChanceYouAreFeared);
                        }
                        else
                        {
                            timelostwhilefeared += BaseFearDur * fearActs * ChanceYouAreFeared;
                        }
                    }
                }
                BZ.numActivatesO20 = BZActualActs * (1f - timeUnder20);
                BZ.numActivatesU20 = BZActualActs * timeUnder20;
                if (_needDisplayCalcs && TimesFeared > 0)
                {
                    GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Lost to Fears\n",
                        TimesFeared, TimesFeared * (1f - timeUnder20), TimesFeared * timeUnder20, BaseFearDur, TimesFeared * BaseFearDur);
                    GCDUsage += (BZ.allNumActivates > 0 ?
                        string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                            BZ.allNumActivates, // Total Acts
                            BZ.numActivatesO20, // Acts Over 20
                            BZ.numActivatesU20, // Acts Under 20
                            BaseFearDur - reducedDur, // Amount Recovered Per
                            BZ.allNumActivates * (BaseFearDur - reducedDur), // Total Amount Recovered
                            BZ.ability.Name) // The Name
                        : "");
                    GCDUsage += (EM.allNumActivates - EMOldActs > 0 ?
                        string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                            EM.allNumActivates - EMOldActs, // Total Acts, limited by its other uses
                            EM.numActivatesO20 - EMOldActs * (1f - timeUnder20), // Acts Over 20, limited by its other uses
                            EM.numActivatesU20 - EMOldActs * timeUnder20, // Acts Under 20, limited by its other uses
                            BaseFearDur - reducedDur, // Amount Recovered Per
                            (EM.allNumActivates - EMOldActs) * (BaseFearDur - reducedDur), // Total Amount Recovered
                            EM.ability.Name) // The Name
                        : "");
                    GCDUsage += (WF.allNumActivates - WFOldActs > 0 ?
                        string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                            WF.allNumActivates - WFOldActs, // Total Acts, limited by its other uses
                            WF.numActivatesO20 - WFOldActs * (1f - timeUnder20), // Acts Over 20, limited by its other uses
                            WF.numActivatesU20 - WFOldActs * timeUnder20, // Acts Under 20, limited by its other uses
                            BaseFearDur - reducedDur, // Amount Recovered Per
                            (WF.allNumActivates - WFOldActs) * (BaseFearDur - reducedDur), // Total Amount Recovered
                            WF.ability.Name) // The Name
                        : "");
                }
                TimeLostGCDsO20 += Math.Min(NumGCDsO20, (BaseFearDur * TimesFeared * (1f - timeUnder20)) / LatentGCD);
                TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * BZ.numActivatesO20) / LatentGCD);
                //TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * _emActs * (1f - timeUnder20)) / LatentGCD);
                //TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * _wfActs * (1f - timeUnder20)) / LatentGCD);

                TimeLostGCDsU20 += Math.Min(NumGCDsU20, (BaseFearDur * TimesFeared * timeUnder20) / LatentGCD);
                TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * BZ.numActivatesU20) / LatentGCD);
                //TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * _emActs * timeUnder20) / LatentGCD);
                //TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * _wfActs * timeUnder20) / LatentGCD);

                percTimeInFear = timelostwhilefeared / FightDuration;
            }
            
            return percTimeInFear;
        }

        private float CalculateRoot()
        {
            float percTimeInRoot = 0f;
            if (DPSWarrChar.BossOpts.RootingTargs && DPSWarrChar.BossOpts.Roots.Count > 0)
            {
                float timeUnder20 = CalcOpts.M_ExecuteSpam ? (float)DPSWarrChar.BossOpts.Under20Perc : 0f;
                float timelostwhilerooted = 0f;
                float BaseRootDur = 0f, rootActs = 0f, reducedDur = 0f,
                      MaxTimeRegain = 0f,
                      ChanceYouAreRooted = 1f;
                AbilWrapper HF = GetWrapper<HeroicFury>();          float HFMaxActs = HF.ability.Activates;                      float HFActualActs = 0f;
                AbilWrapper EM = GetWrapper<EveryManForHimself>();  float EMMaxActs = EM.ability.Activates - EM.allNumActivates; float EMOldActs = EM.allNumActivates;
                AbilWrapper EA = GetWrapper<EscapeArtist>();        float EAMaxActs = EA.ability.Activates - EA.allNumActivates; float EAOldActs = EA.allNumActivates;
                TimesRooted = 0f;
                foreach (Impedance r in DPSWarrChar.BossOpts.Roots)
                {
                    ChanceYouAreRooted = r.Chance;
                    BaseRootDur = Math.Max(0f, (r.Duration / 1000f * (1f - StatS.SnareRootDurReduc)));
                    rootActs = FightDuration / r.Frequency;
                    if (rootActs > 0f)
                    {
                        TimesRooted += rootActs;
                        if (HFMaxActs - HFActualActs > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseRootDur - LatentGCD - CalcOpts.React / 1000f));
                            float BZNewActs = Math.Min(HFMaxActs - HFActualActs, rootActs);
                            HFActualActs += BZNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseRootDur - MaxTimeRegain);
                            float percBZdVsUnBZd = BZNewActs / rootActs;
                            timelostwhilerooted += (reducedDur * rootActs * percBZdVsUnBZd * ChanceYouAreRooted)
                                                 + (BaseRootDur * rootActs * (1f - percBZdVsUnBZd) * ChanceYouAreRooted);
                        }
                        else if (Char.Race == CharacterRace.Human && EMMaxActs - EM.allNumActivates > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseRootDur - LatentGCD - CalcOpts.React / 1000f));

                            float availEMacts = EM.ability.Activates - EM.allNumActivates;
                            float EMNewActs = Math.Min(rootActs, availEMacts);
                            EM.numActivatesO20 += EMNewActs * (1f - timeUnder20);
                            EM.numActivatesU20 += EMNewActs * timeUnder20;
                            //_emActs = EM.allNumActivates;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseRootDur - MaxTimeRegain);
                            float percEMdVsUnEMd = EMNewActs / rootActs;
                            timelostwhilerooted += (reducedDur * rootActs * percEMdVsUnEMd * ChanceYouAreRooted)
                                                 + (BaseRootDur * rootActs * (1f - percEMdVsUnEMd) * ChanceYouAreRooted);
                        }
                        else if (Char.Race == CharacterRace.Gnome && EAMaxActs - EA.allNumActivates > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseRootDur - LatentGCD - CalcOpts.React / 1000f));

                            float availEAacts = EA.ability.Activates - EA.allNumActivates;
                            float EANewActs = Math.Min(rootActs, availEAacts);
                            EA.numActivatesO20 += EANewActs * (1f - timeUnder20);
                            EA.numActivatesU20 += EANewActs * timeUnder20;
                            //_eaActs = EA.allNumActivates;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseRootDur - MaxTimeRegain);
                            float percEAdVsUnEAd = EANewActs / rootActs;
                            timelostwhilerooted += (reducedDur * rootActs * percEAdVsUnEAd * ChanceYouAreRooted)
                                                 + (BaseRootDur * rootActs * (1f - percEAdVsUnEAd) * ChanceYouAreRooted);
                        }
                        else
                        {
                            timelostwhilerooted += BaseRootDur * rootActs * ChanceYouAreRooted;
                        }
                    }
                }
                HF.numActivatesO20 = HFActualActs * (1f - timeUnder20);
                HF.numActivatesU20 = HFActualActs * timeUnder20;
                if (_needDisplayCalcs && TimesRooted > 0) {
                    GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Lost to Roots\n",
                        TimesRooted, TimesRooted * (1f - timeUnder20), TimesRooted * timeUnder20, BaseRootDur, TimesRooted * BaseRootDur);
                    GCDUsage += (HF.allNumActivates > 0 ?
                        string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                            HF.allNumActivates, // Total Acts
                            HF.numActivatesO20, // Acts Over 20
                            HF.numActivatesU20, // Acts Under 20
                            BaseRootDur - reducedDur, // Amount Recovered Per
                            HF.allNumActivates * (BaseRootDur - reducedDur), // Total Amount Recovered
                            HF.ability.Name) // The Name
                        : "");
                    GCDUsage += (EM.allNumActivates - EMOldActs > 0 ?
                        string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                            EM.allNumActivates - EMOldActs, // Total Acts, limited by its other uses
                            EM.numActivatesO20 - EMOldActs * (1f - timeUnder20), // Acts Over 20, limited by its other uses
                            EM.numActivatesU20 - EMOldActs * timeUnder20, // Acts Under 20, limited by its other uses
                            BaseRootDur - reducedDur, // Amount Recovered Per
                            (EM.allNumActivates - EMOldActs) * (BaseRootDur - reducedDur), // Total Amount Recovered
                            EM.ability.Name) // The Name
                        : "");
                    GCDUsage += (EA.allNumActivates - EAOldActs > 0 ?
                        string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                            EA.allNumActivates - EAOldActs, // Total Acts, limited by its other uses
                            EA.numActivatesO20 - EAOldActs * (1f - timeUnder20), // Acts Over 20, limited by its other uses
                            EA.numActivatesU20 - EAOldActs * timeUnder20, // Acts Under 20, limited by its other uses
                            BaseRootDur - reducedDur, // Amount Recovered Per
                            (EA.allNumActivates - EAOldActs) * (BaseRootDur - reducedDur), // Total Amount Recovered
                            EA.ability.Name) // The Name
                        : "");
                }
                TimeLostGCDsO20 += Math.Min(NumGCDsO20, (BaseRootDur * TimesRooted * (1f - timeUnder20)) / LatentGCD);
                TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * HF.numActivatesO20) / LatentGCD);
                //TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * _emActs * (1f - timeUnder20)) / LatentGCD);
                //TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * _eaActs * (1f - timeUnder20)) / LatentGCD);

                TimeLostGCDsU20 += Math.Min(NumGCDsU20, (BaseRootDur * TimesRooted * timeUnder20) / LatentGCD);
                TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * HF.numActivatesU20) / LatentGCD);
                //TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * _emActs * timeUnder20) / LatentGCD);
                //TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * _eaActs * timeUnder20) / LatentGCD);

                percTimeInRoot = timelostwhilerooted / FightDuration;
            }
            SecondWind SndW = GetWrapper<SecondWind>().ability as SecondWind;
            SndW.NumStunsOverDur += TimesRooted;
            return percTimeInRoot;
        }

        private float CalculateStun()
        {
            float percTimeInStun = 0f;
            if (DPSWarrChar.BossOpts.StunningTargs && DPSWarrChar.BossOpts.Stuns.Count > 0)
            {
                float timeUnder20 = CalcOpts.M_ExecuteSpam ? (float)DPSWarrChar.BossOpts.Under20Perc : 0f;
                float timelostwhilestunned = 0f;
                float BaseStunDur = 0f, stunActs = 0f, reducedDur = 0f,
                      MaxTimeRegain = 0f,
                      ChanceYouAreStunned = 1f;
                AbilWrapper BZ = GetWrapper<BerserkerRage>();
                float BZMaxActs = BZ.ability.Activates;
                float BZActualActs = 0f;
                AbilWrapper EM = GetWrapper<EveryManForHimself>();
                float EMMaxActs = EM.ability.Activates - EM.allNumActivates;
                float EMOldActs = EM.allNumActivates;
                TimesStunned = 0f;
                foreach (Impedance s in DPSWarrChar.BossOpts.Stuns)
                {
                    ChanceYouAreStunned = s.Chance;
                    BaseStunDur = Math.Max(0f, (s.Duration / 1000f * (1f - StatS.StunDurReduc)));
                    stunActs = FightDuration / s.Frequency;
                    TimesStunned += stunActs;
                    if (stunActs > 0f)
                    {
                        if (BZMaxActs - BZActualActs > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseStunDur - LatentGCD - CalcOpts.React / 1000f));
                            float BZNewActs = Math.Min(BZMaxActs - BZActualActs, stunActs);
                            BZActualActs += BZNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseStunDur - MaxTimeRegain);
                            float percBZdVsUnBZd = BZNewActs / stunActs;
                            timelostwhilestunned += (reducedDur * stunActs * percBZdVsUnBZd * ChanceYouAreStunned)
                                                 + (BaseStunDur * stunActs * (1f - percBZdVsUnBZd) * ChanceYouAreStunned);
                        }
                        else if (Char.Race == CharacterRace.Human && EMMaxActs - EM.allNumActivates > 0f)
                        {
                            MaxTimeRegain = Math.Max(0f, (BaseStunDur - LatentGCD - CalcOpts.React / 1000f));

                            float availEMacts = EM.ability.Activates - EM.allNumActivates;
                            float EMNewActs = Math.Min(stunActs, availEMacts);
                            EM.numActivatesO20 += EMNewActs * (1f - timeUnder20);
                            EM.numActivatesU20 += EMNewActs * timeUnder20;
                            //_emActs = EM.allNumActivates;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseStunDur - MaxTimeRegain);
                            float percEMdVsUnEMd = EMNewActs / stunActs;
                            timelostwhilestunned += (reducedDur * stunActs * percEMdVsUnEMd * ChanceYouAreStunned)
                                                 + (BaseStunDur * stunActs * (1f - percEMdVsUnEMd) * ChanceYouAreStunned);
                        } else {
                            timelostwhilestunned += BaseStunDur * stunActs * ChanceYouAreStunned;
                        }
                    }
                }
                BZ.numActivatesO20 += BZActualActs * (1f - timeUnder20);
                BZ.numActivatesU20 += BZActualActs * timeUnder20;
                if (_needDisplayCalcs && TimesStunned > 0)
                {
                    GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Lost to Stuns\n",
                        TimesStunned, TimesStunned * (1f - timeUnder20), TimesStunned * timeUnder20, BaseStunDur, TimesRooted * BaseStunDur);
                    GCDUsage += (EM.allNumActivates - EMOldActs > 0 ?
                        string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                            EM.allNumActivates - EMOldActs, // Total Acts, limited by its other uses
                            EM.numActivatesO20 - EMOldActs * (1f - timeUnder20), // Acts Over 20, limited by its other uses
                            EM.numActivatesU20 - EMOldActs * timeUnder20, // Acts Under 20, limited by its other uses
                            BaseStunDur - reducedDur, // Amount Recovered Per
                            (EM.allNumActivates - EMOldActs) * (BaseStunDur - reducedDur), // Total Amount Recovered
                            EM.ability.Name) // The Name
                        : "");
                    GCDUsage += (EM.allNumActivates - EMOldActs > 0 ?
                        string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                            EM.allNumActivates - EMOldActs, // Total Acts, limited by its other uses
                            EM.numActivatesO20 - EMOldActs * (1f - timeUnder20), // Acts Over 20, limited by its other uses
                            EM.numActivatesU20 - EMOldActs * timeUnder20, // Acts Under 20, limited by its other uses
                            BaseStunDur - reducedDur, // Amount Recovered Per
                            (EM.allNumActivates - EMOldActs) * (BaseStunDur - reducedDur), // Total Amount Recovered
                            EM.ability.Name) // The Name
                        : "");
                }
                TimeLostGCDsO20 += Math.Min(NumGCDsO20, (BaseStunDur * TimesStunned * (1f - timeUnder20)) / LatentGCD);
                TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * BZ.numActivatesO20 * (1f - timeUnder20)) / LatentGCD);
                //TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * _emActs * (1f - timeUnder20)) / LatentGCD);

                TimeLostGCDsU20 += Math.Min(NumGCDsU20, (BaseStunDur * TimesStunned * timeUnder20) / LatentGCD);
                TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * BZ.numActivatesU20 * timeUnder20) / LatentGCD);
                //TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * _emActs * timeUnder20) / LatentGCD);

                percTimeInStun = timelostwhilestunned / FightDuration;

                SecondWind SndW = GetWrapper<SecondWind>().ability as SecondWind;
                SndW.NumStunsOverDur += stunActs;
            }
            
            return percTimeInStun;
        }

        private float CalculateMovement()
        {
            float percTimeInMovement = 0f;
            if (DPSWarrChar.BossOpts.MovingTargs && DPSWarrChar.BossOpts.Moves.Count > 0)
            {
                /* = Movement Speed =
                 * According to a post I found on WoWWiki, Standard (Run) Movement
                 * Speed is 7 yards per 1 sec.
                 * Cat's Swiftness (and similar) bring this to 7.56 (7x1.08)
                 * If you are moving for 5 seconds, this is 35 yards (37.8 w/ bonus)
                 * All the movement effects have a min 8 yards, so you have to be
                 * moving for 1.142857 seconds (1.08 seconds w/ bonus) before Charge
                 * would be viable. If you had to be moving more than Charge's Max
                 * Range (25 yards, editable by certain bonuses) then we'd benefit
                 * again from move speed bonuses, etc.
                 * 
                 * Charge Max = 25
                 * that's 25/7.00 = 3.571428571428571 seconds at 7.00 yards per sec
                 * that's 25/7.56 = 3.306878306873070 seconds at 7.56 yards per sec
                 * Charge (Glyph of Charge) Max = 25+5=30
                 * that's 30/7.00 = 4.285714285714286 seconds at 7.00 yards per sec
                 * that's 30/7.56 = 3.968253968253968 seconds at 7.56 yards per sec
                 * 
                 * = Now let's try and get some of those GCDs back =
                 * Let's assume that if the movement duration is longer
                 * than the before mentioned (1.142857|1.08) seconds,
                 * you are far enough away that you can use a Movement
                 * Ability (Charge, Intercept or  Intervene)
                 * Since some of these abilities are usable in combat
                 * only by talents, we have to make those checks first
                 * Since some stuff is kind of weird, we're going to
                 * enforce an ~3 sec est minimum move time before activating
                 * Charge
                 */
                float timeUnder20 = CalcOpts.M_ExecuteSpam ? (float)DPSWarrChar.BossOpts.Under20Perc : 0f;
                AbilWrapper HF = GetWrapper<HeroicFury>();
                AbilWrapper CH;
                if (CombatFactors.FuryStance) CH = GetWrapper<Intercept>();
                else CH = GetWrapper<Charge>();
                
                float MovementSpeed = 7f * (1f + StatS.MovementSpeed); // 7 yards per sec * 1.08 (if have bonus) = 7.56
                float BaseMoveDur = 0f, movedActs = 0f, reducedDur = 0f,
                      MinMovementTimeRegain = 0f, MaxMovementTimeRegain = 0f,
                      ChanceYouHaveToMove = 1f;
                float ChargeMaxActs = CH.ability.Activates;
                if (CombatFactors.FuryStance && HF.ability.Validated)
                {
                    // Heroic Fury refreshes the cooldown on Intercept
                    ChargeMaxActs += HF.ability.Activates - HF.allNumActivates;
                }
                float ChargeActualActs = 0f;
                float timelostwhilemoving = 0f;
                float moveGCDs = 0f;
                foreach (Impedance m in DPSWarrChar.BossOpts.Moves)
                {
                    ChanceYouHaveToMove = m.Chance;
                    BaseMoveDur = (m.Duration / 1000f * (1f - StatS.MovementSpeed));
                    moveGCDs += movedActs = FightDuration / m.Frequency;

                    if ((ChargeMaxActs - ChargeActualActs > 0f) && (movedActs > 0f))
                    {
                        MaxMovementTimeRegain = Math.Max(0f,
                            Math.Min((BaseMoveDur - CalcOpts.React / 1000f),
                                     (CH.ability.MaxRange / MovementSpeed - CalcOpts.React / 1000f)));
                        MinMovementTimeRegain = Math.Max(0f,
                            Math.Min((BaseMoveDur - CalcOpts.React / 1000f),
                                     (CH.ability.MinRange / MovementSpeed - CalcOpts.React / 1000f)));
                        if (BaseMoveDur >= MinMovementTimeRegain)
                        {
                            float ChargeNewActs = Math.Min(ChargeMaxActs - ChargeActualActs, movedActs);
                            ChargeActualActs += ChargeNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseMoveDur - MaxMovementTimeRegain);
                            float percChargedVsUncharged = ChargeNewActs / movedActs;
                            timelostwhilemoving += (reducedDur * movedActs * percChargedVsUncharged * ChanceYouHaveToMove)
                                                 + (BaseMoveDur * movedActs * (1f - percChargedVsUncharged) * ChanceYouHaveToMove);
                        }
                    } else if (movedActs > 0f) {
                        timelostwhilemoving += BaseMoveDur * movedActs * ChanceYouHaveToMove;
                    }
                }
                float actsCharge = ChargeActualActs;
                TimeLostGCDsO20 += Math.Min(NumGCDsO20, (BaseMoveDur * moveGCDs * (1f - timeUnder20)) / LatentGCD);
                TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * actsCharge * (1f - timeUnder20)) / LatentGCD);
                TimeLostGCDsU20 += Math.Min(NumGCDsU20, (BaseMoveDur * moveGCDs * timeUnder20) / LatentGCD);
                TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * actsCharge * timeUnder20) / LatentGCD);
                CH.numActivatesO20 = actsCharge * (1f - timeUnder20);
                CH.numActivatesU20 = actsCharge * timeUnder20;
                float actsHF = 0f;
                if (CH.allNumActivates > CH.ability.Activates) actsHF += (CH.allNumActivates - CH.ability.Activates);
                HF.numActivatesO20 += actsHF * (1f - timeUnder20);
                HF.numActivatesU20 += actsHF * timeUnder20;
                if (_needDisplayCalcs && moveGCDs > 0) {
                    GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Lost to Movement\n",
                        moveGCDs, moveGCDs * (1f - timeUnder20), moveGCDs * timeUnder20, BaseMoveDur, moveGCDs * BaseMoveDur);
                    GCDUsage += (CH.allNumActivates > 0 ?
                        string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                            CH.allNumActivates, // Total Acts
                            CH.numActivatesO20, // Acts Over 20
                            CH.numActivatesU20, // Acts Under 20
                            BaseMoveDur - reducedDur, // Amount Recovered Per
                            CH.allNumActivates * (BaseMoveDur - reducedDur), // Total Amount Recovered
                            CH.ability.Name) // The Name
                        : "");
                    GCDUsage += (actsHF > 0 ? actsHF.ToString("000.00") + " activates of " + HF.ability.Name + " to refresh " + CH.ability.Name : "");
                }
                percTimeInMovement = timelostwhilemoving / FightDuration;
            }
            return percTimeInMovement;
        }
#endif
        #endregion
        #endregion

        #region Cached Special Effects
        #endregion

        public void AddValidatedSpecialEffects(Stats statsTotal, WarriorTalents talents)
        {
            if (statsTotal == null) { statsTotal = new Stats(); }
            if (talents == null) { talents = new WarriorTalents(); }
            try
            {
                Ability ST = GetWrapper<ShatteringThrow>().Ability,
                        BTS = GetWrapper<BattleShout>().Ability,
                        CS = GetWrapper<CommandingShout>().Ability,
                        RC = GetWrapper<RallyingCry>().Ability,
                        DS = GetWrapper<DemoralizingShout>().Ability,
                        TH = GetWrapper<ThunderClap>().Ability,
                        SN = GetWrapper<SunderArmor>().Ability,
                        SW = GetWrapper<SweepingStrikes>().Ability,
                        RK = GetWrapper<Recklessness>().Ability;
                if (BTS.Validated) { statsTotal.AddSpecialEffect(TalentsAsSpecialEffects.BattleShout[talents.GlyphOfBattle ? 0 : 1][talents.BoomingVoice]); }
                if (CS.Validated) { statsTotal.AddSpecialEffect(TalentsAsSpecialEffects.CommandingShout[talents.GlyphOfCommand ? 0 : 1][talents.BoomingVoice]); }
                if (RC.Validated) { statsTotal.AddSpecialEffect(TalentsAsSpecialEffects.RallyingCry/*[talents.GlyphOfCommand ? 0 : 1][talents.BoomingVoice]*/); }
                if (DS.Validated) { statsTotal.AddSpecialEffect(TalentsAsSpecialEffects.DemoralizingShout[talents.GlyphOfDemoralizingShout ? 0 : 1]); }
                if (ST.Validated) {
                    /*try {
                        float value = (float)Math.Round(ST.MHAtkTable.AnyLand, 3);
                        if (!_SE_ShatteringThrow.ContainsKey(value)) {
                            try {
                                _SE_ShatteringThrow.Add(value, new SpecialEffect(Trigger.Use, new Stats() { TargetArmorReduction = 0.20f, }, ST.Duration, ST.Cd, ST.MHAtkTable.AnyLand));
                            } catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                        }
                        statsTotal.AddSpecialEffect(_SE_ShatteringThrow[value]);
                    } catch (Exception) { } // Do nothing, this is a Silverlight retard bug*/
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { TargetArmorReduction = 0.20f, }, ST.Duration, ST.CD, ST.MHAtkTable.AnyLand) { BypassCache = true });
                }
                if (TH.Validated) {
                    /*try {
                        float value = (int)((float)Math.Round(TH.MHAtkTable.AnyLand, 3)*1000);
                        if (!_SE_ThunderClap.ContainsKey(value)) {
                            //try {
                                _SE_ThunderClap.Add(value, new SpecialEffect(Trigger.Use, new Stats() { BossAttackSpeedMultiplier = -0.10f, }, TH.Duration, TH.Cd, TH.MHAtkTable.AnyLand));
                            //} catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                        }
                        statsTotal.AddSpecialEffect(_SE_ThunderClap[value]);
                    } catch (Exception) { } // Do nothing, this is a Silverlight retard bug*/
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BossAttackSpeedReductionMultiplier = 0.10f, }, TH.Duration, TH.CD, TH.MHAtkTable.AnyLand) { BypassCache = true });
                }
                if (SN.Validated) {
                    /*try {
                        float value = (float)Math.Round(SN.MHAtkTable.AnyLand, 3);
                        if (!_SE_SunderArmor.ContainsKey(value)) {
                            try {
                                _SE_SunderArmor.Add(value, new SpecialEffect(Trigger.Use, new Stats() { TargetArmorReduction = 0.04f, }, SN.Duration, SN.Cd, SN.MHAtkTable.AnyLand, 5));
                            } catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                        }
                        statsTotal.AddSpecialEffect(_SE_SunderArmor[value]);
                    } catch (Exception) { } // Do nothing, this is a Silverlight retard bug*/
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { TargetArmorReduction = 0.04f, }, SN.Duration, SN.CD, SN.MHAtkTable.AnyLand, 5));
                }
                float landedAtksInterval = LandedAtksOverDur / FightDuration;
                //float critRate = CriticalAtksOverDur / AttemptedAtksOverDur;
                if (SW.Validated) {
                    /*try {
                        float interval = (float)Math.Round(landedAtksInterval * 5f, 3);
                        if (!_SE_SweepingStrikes.ContainsKey(interval)) {
                            try {
                                _SE_SweepingStrikes.Add(interval, new SpecialEffect(Trigger.Use, new Stats() { BonusTargets = 1f, }, landedAtksInterval * 5f, SW.Cd));
                            } catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                        }
                        statsTotal.AddSpecialEffect(_SE_SweepingStrikes[interval]);
                    } catch (Exception) { } // Do nothing, this is a Silverlight retard bug*/
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusTargets = 1f, }, landedAtksInterval * 5f, SW.CD));
                }
                if (RK.Validated && DPSWarrChar.CombatFactors.FuryStance)
                {
                    /*try {
                        float interval = (float)Math.Round(landedAtksInterval * 3f, 3);
                        if (!_SE_Recklessness.ContainsKey(interval)) {
                            try {
                                _SE_Recklessness.Add(interval, new SpecialEffect(Trigger.Use, new Stats() { PhysicalCrit = 1f }, landedAtksInterval * 3f, RK.Cd));
                            } catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                        }
                        statsTotal.AddSpecialEffect(_SE_Recklessness[interval]);
                    } catch (Exception) { } // Do nothing, this is a Silverlight retard bug*/
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { PhysicalCrit = 1f }, landedAtksInterval * 3f, RK.CD));
                }
                /*if (talents.Flurry > 0 && CalcOpts.FuryStance)
                { // NOTE!!!! This comment is still using the old method, we need to cache values like you see above
                    //float value = talents.Flurry * 0.05f;
                    SpecialEffect flurry = new SpecialEffect(Trigger.MeleeCrit,
                        new Stats() { PhysicalHaste = talents.Flurry * 0.05f, }, landedAtksInterval * 3f, 0f);
                    statsTotal.AddSpecialEffect(flurry);
                }*/
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error in creating Special Effects Caches",
                    Function = "AddValidatedSpecialEffects()",
                    TheException = ex,
                }.Show();
            }
        }

        internal void ResetHitTables()
        {
            foreach (AbilityWrapper aw in TheAbilityList)
            {
                if (aw.Ability.CanCrit)
                {
                    aw.Ability.MHAtkTable.Reset();
                    aw.Ability.OHAtkTable.Reset();
                }
            }
        }
    }
}
