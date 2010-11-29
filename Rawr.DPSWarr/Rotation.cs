using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Rawr.DPSWarr.Skills;

namespace Rawr.DPSWarr {
    public class Rotation {
        public class AbilWrapper
        {
            public AbilWrapper(Skills.Ability abil) {
                ability = abil;
                _cachedIsDamaging = ability.DamageOverride + ability.DamageOnUseOverride > 0f;
            }
            public Skills.Ability ability { get; set; }
            protected bool _cachedIsDamaging = false;
            public bool isDamaging { get { return _cachedIsDamaging; } }
            // Over 20%
            private float _numActivatesO20 = 0f;
            public float numActivatesO20 {
                get { return _numActivatesO20; }
                set { if (_numActivatesO20 != value) { _numActivatesO20 = value; gcdUsageO20 = -1f; } }
            }
            public float RageO20 { get { return ability.RageCost == -1 ? 0f : ability.GetRageUseOverDur(numActivatesO20); } }
            public float DPSO20 { get { return ability.GetDPS(numActivatesO20, ability.TimeOver20Perc); } }
            public float HPSO20 { get { return ability.GetHPS(numActivatesO20, ability.TimeOver20Perc); } }
            private float gcdUsageO20 = -1f;
            public float GCDUsageO20 {
                get {
                    if (gcdUsageO20 == -1f && ability.UsesGCD) {
                        gcdUsageO20 = numActivatesO20 * (ability.UseTime / LatentGCD);
                    }
                    return gcdUsageO20;
                }
            }
            // Under 20%
            private float _numActivatesU20 = 0f;
            public float numActivatesU20 {
                get { return _numActivatesU20; }
                set { if (_numActivatesU20 != value) { _numActivatesU20 = value; gcdUsageU20 = -1f; } }
            }
            public float RageU20 { get { return ability.RageCost == -1 ? 0f : ability.GetRageUseOverDur(numActivatesU20); } }
            public float DPSU20 { get { return ability.GetDPS(numActivatesU20, ability.TimeUndr20Perc); } }
            public float HPSU20 { get { return ability.GetHPS(numActivatesU20, ability.TimeUndr20Perc); } }
            private float gcdUsageU20 = -1f;
            public float GCDUsageU20 {
                get {
                    if (gcdUsageU20 == -1f && ability.UsesGCD) {
                        gcdUsageU20 = numActivatesU20 * (ability.UseTime / LatentGCD);
                    }
                    return gcdUsageU20;
                }
            }
            private static float _cachedLatentGCD = 1.5f;
            public static float LatentGCD { get { return _cachedLatentGCD; } set { _cachedLatentGCD = value; } }
            // Total
            public float allNumActivates { get { return numActivatesO20 + numActivatesU20; } }
            public float allRage { get { return RageO20 + RageU20; } }
            public float allDPS {
                get {
                    float dpsO20 = DPSO20;
                    float dpsU20 = DPSU20;
                    if (dpsO20 > 0 && dpsU20 > 0) { return (dpsO20 + dpsU20) / 2f; }
                    if (dpsU20 > 0) { return dpsU20; }
                    if (dpsO20 > 0) { return dpsO20; }
                    return 0f;
                    //return DPSO20 + DPSU20 > 0 ? (DPSO20 + DPSU20) / 2f : DPSU20 > 0 ? DPSU20 : DPSO20;
                }
            }
            public float allHPS {
                get {
                    float hpsO20 = HPSO20;
                    float hpsU20 = HPSU20;
                    if (hpsO20 > 0 && hpsU20 > 0) { return (hpsO20 + hpsU20) / 2f; }
                    if (hpsU20 > 0) { return hpsU20; }
                    if (hpsO20 > 0) { return hpsO20; }
                    return 0f;
                    //return HPSO20 + HPSU20 > 0 ? (HPSO20 + HPSU20) / 2f : HPSO20;
                }
            }

            public override string ToString()
            {
                if (ability == null) return "NULLed";
                return string.Format("{0} : Rage {1:0.##} : DPS {2:0.##} : HPS {3:0.##}", ability.Name, allRage == 0 ? "<None>" : allRage.ToString(), allDPS, allHPS);
            }
        }
        // Constructors
        public Rotation()
        {
            Char = null;
            StatS = null;
            Talents = null;
            CombatFactors = null;
            CalcOpts = null;
            BossOpts = null;

            AbilityList = new Dictionary<Type,AbilWrapper>();
            InvalidateCache();
        }

        #region Variables

        public void InvalidateAbilityLists() {
            AbilityList.Clear();
            _abilList = _abilListThatDMGs = _abilListThatMaints = _abilListThatGCDs = null;
        }

        private Dictionary<Type, AbilWrapper> _abilityList;
        private Dictionary<Type, AbilWrapper> AbilityList {
            get { return _abilityList; }
            set { _abilityList = value; }
        }

        private List<AbilWrapper> _abilList;
        public List<AbilWrapper> GetAbilityList() {
            if (_abilList == null) { _abilList = new List<AbilWrapper>(AbilityList.Values); }
            return _abilList;
        }
        private List<AbilWrapper> _abilListThatDMGs;
        public List<AbilWrapper> GetDamagingAbilities() {
            if (_abilListThatDMGs == null) { _abilListThatDMGs = new List<AbilWrapper>(AbilityList.Values).FindAll(e => e.isDamaging); }
            return _abilListThatDMGs;
        }
        private List<AbilWrapper> _abilListThatMaints;
        public List<AbilWrapper> GetMaintenanceAbilities() {
            if (_abilListThatMaints == null) { _abilListThatMaints = new List<AbilWrapper>(AbilityList.Values).FindAll(e => e.ability.isMaint); }
            return _abilListThatMaints;
        }
        private List<AbilWrapper> _abilListThatGCDs;
        public List<AbilWrapper> GetAbilityListThatGCDs() {
            if (_abilListThatGCDs == null) { _abilListThatGCDs = new List<AbilWrapper>(AbilityList.Values).FindAll(e => e.ability.UsesGCD); }
            return _abilListThatGCDs;
        }

        public AbilWrapper GetWrapper<T>() { return AbilityList[typeof(T)]; }

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
        //public float TimesSilenced = 0f;
        public float TimesDisarmed = 0f;
        
        public Skills.DeepWounds DW;

        #endregion
        #region Get/Set
        protected Character Char { get; set; }
        protected WarriorTalents Talents { get; set; }
        protected Stats StatS { get; set; }
        protected CombatFactors CombatFactors { get; set; }
        public Skills.WhiteAttacks WhiteAtks { get; protected set; }
        protected CalculationOptionsDPSWarr CalcOpts { get; set; }
        protected BossOptions BossOpts { get; set; }

        protected float _cachedLatentGCD = 1.5f;
        public float LatentGCD { get { return _cachedLatentGCD; } }
        
        /// <summary>
        /// How many GCDs are in the rotation, based on fight duration and latency, all or if using Exec Spam then just Over 20%
        /// </summary>
        protected float NumGCDsO20 { get { return _cachedNumGCDsO20; } }
        protected float _cachedNumGCDsO20 = 0f;
        /// <summary>
        /// How many GCDs are in the rotation, based on fight duration and latency, under 20%
        /// </summary>
        protected float NumGCDsU20 { get { return _cachedNumGCDsU20; } }
        protected float _cachedNumGCDsU20 = 0f;
        /// <summary>
        /// How many GCDs are in the rotation, based on fight duration and latency, all or if using Exec Spam then just Over 20%
        /// </summary>
        protected float NumGCDsAll { get { return NumGCDsO20 + NumGCDsU20; } }

        /// <summary>
        /// How many GCDs have been used by the rotation, over 20%
        /// </summary>
        protected float GCDsUsedO20
        {
            get
            {
                float gcds = 0f;
                foreach (AbilWrapper aw in GetAbilityListThatGCDs())
                {
                    gcds += aw.GCDUsageO20;// aw.numActivatesO20 * aw.ability.UseTime / LatentGCD;
                }
                return gcds;
            }
        }
        /// <summary>
        /// How many GCDs have been used by the rotation, under 20%
        /// </summary>
        protected float GCDsUsedU20
        {
            get
            {
                float gcds = 0f;
                foreach (AbilWrapper aw in GetAbilityListThatGCDs())
                {
                    gcds += aw.GCDUsageU20;// aw.numActivatesU20 * aw.ability.UseTime / LatentGCD;
                }
                return gcds;
            }
        }
        /// <summary>
        /// How many GCDs have been used by the rotation (all)
        /// </summary>
        protected float GCDsUsedAll { get { return GCDsUsedO20 + GCDsUsedU20; } }

        /// <summary>
        /// How many GCDs are still available in the rotation
        /// </summary>
        protected float GCDsAvailableO20 { get { return Math.Max(0f, NumGCDsO20 - GCDsUsedO20 - TimeLostGCDsO20); } }
        /// <summary>
        /// How many GCDs are still available in the rotation sub 20%
        /// </summary>
        protected float GCDsAvailableU20 { get { return Math.Max(0f, NumGCDsU20 - GCDsUsedU20 - TimeLostGCDsU20); } }
        /// <summary>
        /// How many GCDs are still available in the rotation sub 20%
        /// </summary>
        protected float GCDsAvailableAll { get { return GCDsAvailableO20 + GCDsAvailableU20; } }
        
        #endregion
        #region Functions
        public virtual void Initialize(CharacterCalculationsDPSWarr calcs) {
            this.calcs = calcs;
            StatS = calcs.AverageStats;

            initAbilities();

            // Whites
            calcs.Whites = WhiteAtks;
        }
        public virtual void Initialize() { initAbilities(); /*doIterations();*/ }

        public virtual void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalculations) {
            _needDisplayCalcs = needsDisplayCalculations;
        }

        protected virtual void initAbilities() {
            string info = "Before";
            try {
                InvalidateAbilityLists();
                // Whites
                WhiteAtks.InvalidateCache();
                // Anti-Debuff
                info = "Anti-Debuff";
                AddAbility(new AbilWrapper(new Skills.HeroicFury(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.EveryManForHimself(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.EscapeArtist(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.WillOfTheForsaken(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                // Movement
                info = "Movement";
                AddAbility(new AbilWrapper(new Skills.Charge(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.Intercept(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.HeroicLeap(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                // Rage Generators
                info = "Rage Generators";
                AddAbility(new AbilWrapper(new Skills.SecondWind(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.BerserkerRage(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.DeadlyCalm(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.InnerRage(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                // Maintenance
                info = "Maintenance";
                AddAbility(new AbilWrapper(new Skills.BattleShout(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.CommandingShout(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.DemoralizingShout(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.SunderArmor(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.Hamstring(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.EnragedRegeneration(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                // Periodics
                info = "Periodics";
                AddAbility(new AbilWrapper(new Skills.ShatteringThrow(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.SweepingStrikes(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.DeathWish(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.Recklessness(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));

                // Arms abilities
                info = "Arms abilities";
                AddAbility(new AbilWrapper(new Skills.ColossusSmash(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.Bladestorm(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.MortalStrike(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.Rend(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.ThunderClap(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.OverPower(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.TasteForBlood(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.VictoryRush(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.Cleave(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.HeroicStrike(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                AddAbility(new AbilWrapper(new Skills.Execute(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
                Slam SL = new Skills.Slam(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts);
                AddAbility(new AbilWrapper(SL)); // Slam used by Bloodsurge, WW used by Bladestorm, so they're shared
                AddAbility(new AbilWrapper(new Skills.StrikesOfOpportunity(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));

                // Fury abilities
                info = "Fury abilities";
                Skills.Ability WW = new Skills.WhirlWind(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts);
                AddAbility(new AbilWrapper(WW));
                Ability BT = new Skills.BloodThirst(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts);
                AddAbility(new AbilWrapper(BT));
                AddAbility(new AbilWrapper(new Skills.BloodSurge(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts, SL, WW, BT)));
                AddAbility(new AbilWrapper(new Skills.RagingBlow(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));

                DW = new Skills.DeepWounds(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts);
            } catch (Exception ex) {
                Base.ErrorBox eb = new Base.ErrorBox("Error Initalizing Rotation Abilities", ex, "initAbilities()", info);
                eb.Show();
            }
        }

        private void AddAbility(AbilWrapper abilWrapper)
        {
            try {
                AbilityList.Add(abilWrapper.ability.GetType(), abilWrapper);
            } catch (Exception ex) {
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error in adding an Ability Wrapper",
                    ex.Message, ex.InnerException,
                    "AddAbility(...)", "No Additional Info", ex.StackTrace);
                eb.Show();
            }
        }

        public virtual void doIterations() { }

        protected virtual void calcDeepWounds() {
            // Main Hand
            float mhActivates =
                /*Yellow  */CriticalYellowsOverDurMH + 
                /*White   */WhiteAtks.MhActivates * WhiteAtks.MHAtkTable.Crit;

            // Off Hand
            float ohActivates = (CombatFactors.useOH ?
                // No OnAttacks for OH
                /*Yellow*/CriticalYellowsOverDurOH +
                /*White */WhiteAtks.OhActivates * WhiteAtks.OHAtkTable.Crit
                : 0f);

            // Push to the Ability
            DW.SetAllAbilityActivates(mhActivates, ohActivates); 
        }

        public void InvalidateCache()
        {
            for (int i = 0; i < 5; i++)
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
            }
        }

        #region Attacks over Duration
        public enum SwingResult : int { Attempt=0, Land, Crit, Parry, Dodge };
        public enum Hand : int { MH=0, OH, Both };
        public enum AttackType : int { Yellow=0, White, Both };
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
                foreach (AbilWrapper abil in GetDamagingAbilities())
                {
                    if (!abil.ability.Validated) { continue; }
                    if (h != Hand.OH) {
                        table = abil.ability.MHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.numActivatesO20 * abil.ability.AvgTargets * abil.ability.SwingsPerActivate * mod;
                    }
                    if (h != Hand.MH && CombatFactors.useOH && abil.ability.SwingsOffHand) {
                        table = abil.ability.OHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.numActivatesO20 * abil.ability.AvgTargets * mod;
                    }
                }
            }
            if (at != AttackType.Yellow) {
                if (h != Hand.OH) {
                    table = WhiteAtks.MHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += WhiteAtks.MhActivatesO20 * mod;
                }
                if (h != Hand.MH && CombatFactors.useOH) {
                    table = WhiteAtks.OHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += WhiteAtks.OhActivates * mod;
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
                foreach (AbilWrapper abil in GetDamagingAbilities())
                {
                    if (!abil.ability.Validated) { continue; }
                    if (h != Hand.OH) {
                        table = abil.ability.MHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.numActivatesU20 * abil.ability.AvgTargets * abil.ability.SwingsPerActivate * mod;
                    }
                    if (h != Hand.MH && CombatFactors.useOH && abil.ability.SwingsOffHand) {
                        table = abil.ability.OHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.numActivatesU20 * abil.ability.AvgTargets * mod;
                    }
                }
            }
            if (at != AttackType.Yellow) {
                if (h != Hand.OH) {
                    table = WhiteAtks.MHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += WhiteAtks.MhActivatesU20 * mod;
                }
                if (h != Hand.MH && CombatFactors.useOH) {
                    table = WhiteAtks.OHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += WhiteAtks.OhActivates * mod;
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
                foreach (AbilWrapper abil in GetDamagingAbilities())
                {
                    if (!abil.ability.Validated)
                    {
                        continue;
                    }
                    if (h != Hand.OH)
                    {
                        table = abil.ability.MHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.allNumActivates * abil.ability.AvgTargets * abil.ability.SwingsPerActivate * mod;
                    }
                    if (h != Hand.MH && CombatFactors.useOH && abil.ability.SwingsOffHand)
                    {
                        table = abil.ability.OHAtkTable;
                        mod = GetTableFromSwingResult(sr, table);
                        count += abil.allNumActivates * abil.ability.AvgTargets * mod;
                    }
                }
            }
            if (at != AttackType.Yellow)
            {
                if (h != Hand.OH)
                {
                    table = WhiteAtks.MHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += WhiteAtks.MhActivates * mod;
                }
                if (h != Hand.MH && CombatFactors.useOH)
                {
                    table = WhiteAtks.OHAtkTable;
                    mod = GetTableFromSwingResult(sr, table);
                    count += WhiteAtks.OhActivates * mod;
                }
            }

            _atkOverDurs[(int)sr, (int)h, (int)at] = count;
        }
        private float GetTableFromSwingResult(SwingResult sr, CombatTable table)
        {
            if (table == null) return 0f;
            switch (sr)
            {
                case SwingResult.Attempt: return 1f;
                case SwingResult.Crit:    return table.Crit;
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

        public float CriticalAtksOverDur { get { return GetAttackOverDuration(SwingResult.Crit, Hand.Both, AttackType.Both); } }

        public float AttemptedAtksOverDurMH { get { return GetAttackOverDuration(SwingResult.Attempt, Hand.MH, AttackType.Both); } }
        public float LandedAtksOverDurMH { get { return GetAttackOverDuration(SwingResult.Land, Hand.MH, AttackType.Both); } }

        public float AttemptedAtksOverDurOH { get { return GetAttackOverDuration(SwingResult.Attempt, Hand.OH, AttackType.Both); } }
        public float LandedAtksOverDurOH { get { return GetAttackOverDuration(SwingResult.Land, Hand.OH, AttackType.Both); } }

        public float CriticalYellowsOverDur { get { return GetAttackOverDuration(SwingResult.Crit, Hand.Both, AttackType.Both); } }
        public float LandedYellowsOverDur { get { return GetAttackOverDuration(SwingResult.Land, Hand.Both, AttackType.Both); } }

        public float CriticalYellowsOverDurMH { get { return GetAttackOverDuration(SwingResult.Crit, Hand.MH, AttackType.Yellow); } }
        public float CriticalYellowsOverDurOH { get { return GetAttackOverDuration(SwingResult.Crit, Hand.OH, AttackType.Yellow); } }

        public float DodgedAttacksOverDur { get { return GetAttackOverDuration(SwingResult.Dodge, Hand.Both, AttackType.Both); } }
        public float DodgedYellowsOverDur { get { return GetAttackOverDuration(SwingResult.Dodge, Hand.Both, AttackType.Yellow); } }

        public float ParriedAttacksOverDur { get { return GetAttackOverDuration(SwingResult.Parry, Hand.Both, AttackType.Both); } }
        public float ParriedYellowsOverDur { get { return GetAttackOverDuration(SwingResult.Parry, Hand.Both, AttackType.Yellow); } }

        public virtual float CritHsSlamOverDur {
            get {
                AbilWrapper HS = GetWrapper<HeroicStrike>();
                AbilWrapper SL = GetWrapper<Slam>();
                AbilWrapper BS = GetWrapper<BloodSurge>();
                return HS.allNumActivates * HS.ability.MHAtkTable.Crit * HS.ability.AvgTargets +
                       SL.allNumActivates * SL.ability.MHAtkTable.Crit * SL.ability.AvgTargets +
                       BS.allNumActivates * BS.ability.MHAtkTable.Crit * BS.ability.AvgTargets;
            }
        }

        #endregion

        /// <summary>This is used by Fury</summary>
        public float MaintainCDs {
            get {
                float cds = 0f;
                foreach (AbilWrapper aw in GetMaintenanceAbilities()) cds += aw.allNumActivates;
                return cds;
            }
        }
        #endregion
        #region Rage Calcs
        protected virtual float RageGenOverDur_IncDmg {
            get {
                // Invalidate bad things
                List<Attack> aoeAtks = BossOpts.GetFilteredAttackList(ATTACK_TYPES.AT_AOE);
                Attack dynAoE = BossOpts.DynamicCompiler_FilteredAttacks(aoeAtks);
                if (aoeAtks.Count > 0 || dynAoE.AttackSpeed <= 0 || dynAoE.DamagePerHit <= 0) { return 0f; }
                float RageMod = 2.5f / 453.3f;
                float damagePerSec = 0f;
                float freq =
                    dynAoE.AttackSpeed;
                float dmg =
                    dynAoE.DamagePerHit
                    * (1f + StatS.DamageTakenMultiplier) + StatS.BossAttackPower / 14f;
                float acts = FightDuration / freq;
                // Add Berserker Rage's
                float zerkerMOD = 1f;
                if (CalcOpts.M_BerserkerRage) {
                    float upTime = _SE_ZERKERDUMMY.GetAverageUptime(0, 1f, CombatFactors._c_mhItemSpeed, (CalcOpts.SE_UseDur ? FightDuration : 0f));
                    zerkerMOD *= (1f + upTime);
                }
                float dmgCap = 100f / (RageMod * zerkerMOD); // Can't get any more rage than 100 at any given time
                damagePerSec = (acts * Math.Min(dmgCap, dmg)) / FightDuration;
                
                return (damagePerSec * RageMod * zerkerMOD) * FightDuration;
            }
        }

        protected virtual float RageGenOverDur_AngerO20 { get { return CombatFactors.FuryStance ? 0f : (1.0f / 3.0f) * FightDurationO20; } }
        protected virtual float RageGenOverDur_AngerU20 { get { return CombatFactors.FuryStance ? 0f : (1.0f / 3.0f) * FightDurationU20; } }
        /// <summary>
        /// Anger Management is an Arms Spec Bonus in Cata, 1 rage every 3 seconds
        /// </summary>
        protected virtual float RageGenOverDur_Anger { get { return RageGenOverDur_AngerO20 + (CalcOpts.M_ExecuteSpam ? RageGenOverDur_AngerU20 : 0f); } }
        
        protected virtual float RageGenOverDur_OtherO20 {
            get {
                float rage = RageGenOverDur_AngerO20               // Anger Management Talent
                            + RageGenOverDur_IncDmg * TimeOver20Perc             // Damage from the bosses
                            + (100f * StatS.ManaorEquivRestore) * TimeOver20Perc // 0.02f becomes 2f
                            + StatS.BonusRageGen * TimeOver20Perc;               // Bloodrage, Berserker Rage, Mighty Rage Pot

                foreach (AbilWrapper aw in GetAbilityList()) { if (aw.allRage < 0) { rage += (-1f) * aw.RageO20; } }

                return rage;
            }
        }
        protected virtual float RageGenOverDur_OtherU20 {
            get {
                float rage = RageGenOverDur_AngerU20               // Anger Management Talent
                            + RageGenOverDur_IncDmg * TimeUndr20Perc              // Damage from the bosses
                            + (100f * StatS.ManaorEquivRestore) * TimeUndr20Perc  // 0.02f becomes 2f
                            + StatS.BonusRageGen * TimeUndr20Perc;                // Bloodrage, Berserker Rage, Mighty Rage Pot

                foreach (AbilWrapper aw in GetAbilityList()) { if (aw.allRage < 0) { rage += (-1f) * aw.RageU20; } }

                return rage;
            }
        }
        protected virtual float RageGenOverDur_Other { get { return RageGenOverDur_OtherO20 + (CalcOpts.M_ExecuteSpam ? RageGenOverDur_OtherU20 : 0f); } }


        protected float RageMOD_DeadlyCalm { get { return 1f - (CalcOpts.M_DeadlyCalm && Talents.DeadlyCalm > 0 ? 10f / 120f : 0f); } }
        private static SpecialEffect[] _SE_BattleTrance = new SpecialEffect[] {
            null,
            new SpecialEffect(Trigger.Use, null, 0f, 0f, 0.05f * 1f),
            new SpecialEffect(Trigger.Use, null, 0f, 0f, 0.05f * 2f),
            new SpecialEffect(Trigger.Use, null, 0f, 0f, 0.05f * 3f),
        };
        protected float RageMOD_BattleTrance {
            get {
                AbilWrapper ms = GetWrapper<MortalStrike>();
                if (Talents.BattleTrance == 0 || ms.allNumActivates <= 0) { return 1f; }
                float numAffectedItems = _SE_BattleTrance[Talents.BattleTrance].GetAverageProcsPerSecond(
                    FightDurationO20 / ms.allNumActivates, ms.ability.MHAtkTable.AnyLand, 3.3f, FightDurationO20)
                    * FightDurationO20;
                float percAffectedVsUnAffected = numAffectedItems / (AttemptedAtksOverDurMH * TimeOver20Perc);
                return 1f - percAffectedVsUnAffected;
            }
        }
        protected float RageMOD_Total { get { return RageMOD_DeadlyCalm * (1f + StatS.RageCostMultiplier); } }

        private float _fightDur = -1f, _fightDurO20 = -1f, _fightDurU20 = -1f;
        public float FightDuration { get { if (_fightDur == -1) { _fightDur = BossOpts.BerserkTimer; } return _fightDur; } }
        public float FightDurationO20 { get { if (_fightDurO20 == -1) { _fightDurO20 = FightDuration * TimeOver20Perc; } return _fightDurO20; } }
        public float FightDurationU20 { get { if (_fightDurU20 == -1) { _fightDurU20 = FightDuration * TimeUndr20Perc; } return _fightDurU20; } }

        private float _timeOver20Perc = -1f, _timeUndr20Perc = -1f;
        public float TimeOver20Perc { get { if (_timeOver20Perc == -1f) { _timeOver20Perc = (CalcOpts.M_ExecuteSpam ? 1f - (float)BossOpts.Under20Perc : 1f); } return _timeOver20Perc; } }
        public float TimeUndr20Perc { get { if (_timeUndr20Perc == -1f) { _timeUndr20Perc = (CalcOpts.M_ExecuteSpam ?      (float)BossOpts.Under20Perc : 0f); } return _timeUndr20Perc; } }

        protected float RageNeededOverDurO20 {
            get {
                float rage = 0f;
                foreach (AbilWrapper aw in GetAbilityList()) {
                    if (aw.RageO20 > 0f) {
                        if (aw.ability.GetType() == typeof(MortalStrike)
                            || aw.ability.GetType() == typeof(BloodThirst))
                        {
                            rage += aw.RageO20 * (1f - Talents.BattleTrance * 0.05f);
                        } else
                            rage += aw.RageO20;
                    }
                }
                rage *= RageMOD_DeadlyCalm; // Deadly Calm makes your abilities cost no rage for 10 sec every 2 min.
                return rage;
            }
        }
        protected float RageNeededOverDurU20 {
            get {
                float rage = 0f;
                foreach (AbilWrapper aw in GetAbilityList()) {
                    if (aw.RageU20 > 0f) {
                        if (aw.ability.GetType() == typeof(MortalStrike)
                            || aw.ability.GetType() == typeof(BloodThirst))
                        {
                            rage += aw.RageU20 * (1f - Talents.BattleTrance * 0.05f);
                        }
                        else
                            rage += aw.RageU20;
                    }
                }
                rage *= RageMOD_DeadlyCalm; // Deadly Calm makes your abilities cost no rage for 10 sec every 2 min.
                return rage;
            }
        }
        protected float RageNeededOverDur { get { return RageNeededOverDurO20 + (CalcOpts.M_ExecuteSpam ? RageNeededOverDurU20 : 0f); } }
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

            foreach (AbilWrapper aw in GetMaintenanceAbilities())
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
        private float AddMaintenanceAbility(float totalPercTimeLost, AbilWrapper aw)
        {
            if (!aw.ability.Validated) { return 0f; }

            // If we are using Execute phase, distribute the abilities like they should be
            float Abil_GCDsO20 = Math.Min(GCDsAvailableO20, aw.ability.Activates * (1f - totalPercTimeLost) * TimeOver20Perc);
            float Abil_GCDsU20 = Math.Min(GCDsAvailableU20, aw.ability.Activates * (1f - totalPercTimeLost) * TimeUndr20Perc);
            aw.numActivatesO20 = Abil_GCDsO20;
            aw.numActivatesU20 = Abil_GCDsU20;
            if (_needDisplayCalcs && aw.allNumActivates > 0) {
                GCDUsage += string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                    aw.allNumActivates, aw.numActivatesO20, aw.numActivatesU20,
                    aw.ability.Name, (!aw.ability.UsesGCD ? " (Doesn't use GCDs)" : ""));
            }

            _HPS_TTL += aw.allHPS;
            _DPS_TTL_O20 += aw.DPSO20;
            _DPS_TTL_U20 += aw.DPSU20;
            // This ability doesn't use rage
            if (aw.ability.RageCost == -1) { return 0f; }
            // The ability GENERATES rage (the negative will be double-negatived and added to the rage pool)
            if (aw.ability.RageCost <  -1) { return aw.ability.RageCost * aw.allNumActivates; }
            // The ability USES rage (the positive will be substracted from the rage pool) and since it uses rage, it may get a rage cost effect on it
            if (aw.ability.RageCost >   0) { return aw.ability.RageCost * aw.allNumActivates * RageMOD_Total; }
            // If it didn't fall into those categories, something is wrong, so don't affect rage
            return 0f;
        }
        #endregion

        #region Lost Time due to Combat Factors
        /// <summary>
        /// Calculates percentage of time lost due to moving, being rooted, etc
        /// </summary>
        /// <param name="MS">Placeholder right now for juggernaut handling.  Fury should pass null</param>
        /// <returns>Percentage of time lost as a float</returns>
        protected float CalculateTimeLost()
        {
            //_emActs = 0f; //_emRecovery = 0f; _emRecoveryTotal = 0f;
            TimeLostGCDsO20 = TimeLostGCDsU20 = 0;
            RageGainedWhileMoving = 0;

            float percTimeInFearRootStunMove = 0f;
            try {
                percTimeInFearRootStunMove = CalculateFearRootStunMove();
            }catch(Exception ex) {
                Base.ErrorBox eb = new Base.ErrorBox("Error Getting Time Lost Calcs", ex, "CalculateTimeLost()");
                eb.Show();
            }
            //float percTimeInFear = CalculateFear();
            //float percTimeInStun = CalculateStun();
            //float percTimeInRoot = CalculateRoot();
            //float percTimeInMovement = CalculateMovement();
            // We should be doing silences too, but since they only affect shouts and thunderclap, it will have to be handled differently
            //return Math.Min(1f, percTimeInStun + percTimeInMovement + percTimeInFear + percTimeInRoot);
            return Math.Min(1f, percTimeInFearRootStunMove);
        }

        public enum ImpedanceTypes { Fear, Root, Stun, Move, /*Silence,*/ Disarm };
        public struct ImpedanceWithType {
            public Impedance imp;
            public ImpedanceTypes type;
        }

        private float CalculateFearRootStunMove()
        {
            #region Validation
            // If there are no ACTIVE Fears/Roots/Stuns/Moves/Sielences/Disarms, don't process
            // We aren't processing Silences yet, but I still want the code for it in place
            if (!BossOpts.FearingTargs && !BossOpts.RootingTargs && !BossOpts.StunningTargs && !BossOpts.MovingTargs /*&& !BossOpts.SilencingTargs*/ && !BossOpts.DisarmingTargs) { return 0f; }

            // If they are active but have counts of 0, don't process
            if (!(BossOpts.FearingTargs && BossOpts.Fears.Count > 0)
                && !(BossOpts.RootingTargs && BossOpts.Roots.Count > 0)
                && !(BossOpts.StunningTargs && BossOpts.Stuns.Count > 0)
                && !(BossOpts.MovingTargs && BossOpts.Moves.Count > 0)
                //&& !(BossOpts.SilencingTargs && BossOpts.Silences.Count > 0)
                && !(BossOpts.DisarmingTargs && BossOpts.Disarms.Count > 0))
            { return 0f; }

            // Generate the master list
            List<ImpedanceWithType> allImps = new List<ImpedanceWithType>();
            if (BossOpts.FearingTargs   && BossOpts.Fears.Count > 0) { foreach (Impedance i in BossOpts.Fears) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Fear }); } }
            if (BossOpts.RootingTargs   && BossOpts.Roots.Count > 0) { foreach (Impedance i in BossOpts.Roots) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Root }); } }
            if (BossOpts.StunningTargs  && BossOpts.Stuns.Count > 0) { foreach (Impedance i in BossOpts.Stuns) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Stun }); } }
            if (BossOpts.MovingTargs    && BossOpts.Moves.Count > 0) { foreach (Impedance i in BossOpts.Moves) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Move }); } }
            //if (BossOpts.SilencingTargs && BossOpts.Silences.Count > 0) { foreach (Impedance i in BossOpts.Silences) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Silence }); } }
            if (BossOpts.DisarmingTargs && BossOpts.Disarms.Count > 0) { foreach (Impedance i in BossOpts.Disarms) { allImps.Add(new ImpedanceWithType { imp = i, type = ImpedanceTypes.Disarm }); } }
            
            // If the array has count 0 for some reason, don't process
            if (allImps.Count <= 0) { return 0; }
            #endregion

            #region Variable Declaration
            float percTimeInFear = 0f, percTimeInRoot = 0f, percTimeInStun = 0f, percTimeInMove = 0f, /*percTimeInSilence = 0f,*/ percTimeInDisarm = 0f;
            float timelostwhilefeared = 0f, timelostwhilerooted = 0f, timelostwhilestunned = 0f, timelostwhilemoving = 0f, /*timelostwhilesilenced = 0f,*/ timelostwhiledisarmed = 0f;
            TimesFeared = TimesRooted = TimesStunned = TimesMoved /*= TimesSilenced*/ = TimesDisarmed = 0f;
            float baseDur = 0f, acts = 0f, maxTimeRegain = 0f, chanceYouAreAffected = 1f, timelostwhileaffected = 0f;
            float MovementSpeed = 7f * (1f + StatS.MovementSpeed); // 7 yards per sec * 1.08 (if have bonus) = 7.56
            #endregion

            #region Abilities that can Break this stuff
            // Heroic Fury (Fury Warrior): Roots
            AbilWrapper HF = GetWrapper<HeroicFury>();          float HFMaxActs = HF.ability.Activates; float newActsHF = 0f; float reducedDurHF = 0f; HF.numActivatesO20 = HF.numActivatesU20 = 0f;
            // Berserker Rage (Warrior): Fears, Stuns
            AbilWrapper BZ = GetWrapper<BerserkerRage>();       float BZMaxActs = BZ.ability.Activates; float newActsBZ = 0f; float reducedDurBZ = 0f; BZ.numActivatesO20 = BZ.numActivatesU20 = 0f;
            // Escape Artist (Gnome): Roots/Snares
            AbilWrapper EA = GetWrapper<EscapeArtist>();        float EAMaxActs = EA.ability.Activates; float newActsEA = 0f; float reducedDurEA = 0f; EA.numActivatesO20 = EA.numActivatesU20 = 0f;
            // Will of the Forsaken (Undead): Fears, Charms, Sleeps (only model Fears)
            AbilWrapper WF = GetWrapper<WillOfTheForsaken>();   float WFMaxActs = WF.ability.Activates; float newActsWF = 0f; float reducedDurWF = 0f; WF.numActivatesO20 = WF.numActivatesU20 = 0f;
            // Every Man For Himself (Human): Fears, Roots, Stuns, Charms, Sleeps (Don't model Charms/Sleeps)
            AbilWrapper EM = GetWrapper<EveryManForHimself>();  float EMMaxActs = EM.ability.Activates; float newActsEM = 0f; float reducedDurEM = 0f; EM.numActivatesO20 = EM.numActivatesU20 = 0f;
            // Heroic Leap (Warrior): Moves
            AbilWrapper HL = GetWrapper<HeroicLeap>();          float HLMaxActs = HL.ability.Activates; float newActsHL = 0f; float reducedDurHL = 0f; HL.numActivatesO20 = HL.numActivatesU20 = 0f;
            // Charge (Warrior/Arms/Juggernaught): Moves
            AbilWrapper CH = GetWrapper<Charge>();              float CHMaxActs = CH.ability.Activates; float newActsCH = 0f; float reducedDurCH = 0f; CH.numActivatesO20 = CH.numActivatesU20 = 0f;
            // Intercept (Warrior/Fury): Moves
            AbilWrapper IN = GetWrapper<Intercept>();           float INMaxActs = IN.ability.Activates; float newActsIN = 0f; float reducedDurIN = 0f; IN.numActivatesO20 = IN.numActivatesU20 = 0f;
            #endregion

            foreach (ImpedanceWithType iwt in allImps)
            {
                #region Validation and Variable Setup
                chanceYouAreAffected = iwt.imp.Chance;
                if (chanceYouAreAffected <= 0) { continue; } // If it can't affect you, skip it
                acts = FightDuration / (iwt.imp.Frequency < FightDuration ? iwt.imp.Frequency : FightDuration);
                if (acts <= 0f || float.IsNaN(acts) || float.IsInfinity(acts)) { continue; } // If it never activates or is broken, skip it
                float statReducVal = iwt.type == ImpedanceTypes.Fear ? StatS.FearDurReduc
                                   : iwt.type == ImpedanceTypes.Root ? StatS.SnareRootDurReduc
                                   : iwt.type == ImpedanceTypes.Stun ? StatS.StunDurReduc
                                   : iwt.type == ImpedanceTypes.Move ? StatS.MovementSpeed
                                   //: iwt.type == ImpedanceTypes.Silence ? StatS.SilenceDurReduc
                                   : iwt.type == ImpedanceTypes.Disarm ? StatS.DisarmDurReduc
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
                    && (Char.WarriorTalents.HeroicFury > 0) // We have the Fury Talent (qualifier for Heroic Fury)
                    && (HFMaxActs - HF.allNumActivates > 0f) // We haven't already used up Heroic Fury
                    && (Math.Min(0f, (HF.ability.GCDTime - baseDur)) < 0)) // We can regain time using Heroic Fury
                {
                    maxTimeRegain = Math.Max(0f, baseDur - HF.ability.GCDTime);
                    // Determine how many we can still use
                    float availActs     = HFMaxActs - HF.allNumActivates;
                    newActsHF           = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    HF.numActivatesO20 += newActsHF * TimeOver20Perc;
                    HF.numActivatesU20 += newActsHF * TimeUndr20Perc;
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
                    && (BZMaxActs - BZ.allNumActivates > 0f) // We haven't already used up Berserker Rage
                    && (Math.Min(0f, (BZ.ability.GCDTime - baseDur)) < 0)) // We can regain time using Berserker Rage
                {
                    maxTimeRegain = Math.Max(0f, baseDur - BZ.ability.GCDTime);
                    // Determine how many we can still use
                    float availActs     = BZMaxActs - BZ.allNumActivates;
                    newActsBZ           = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    BZ.numActivatesO20 += newActsBZ * TimeOver20Perc;
                    BZ.numActivatesU20 += newActsBZ * TimeUndr20Perc;
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
                    && (Char.Race == CharacterRace.Gnome) // We are a Gnome (qualifier for Escape Artist)
                    && (EAMaxActs - EA.allNumActivates > 0f) // We haven't already used up Escape Artist
                    && (Math.Min(0f, (EA.ability.GCDTime - baseDur)) < 0)) // We can regain time using Escape Artist
                {
                    maxTimeRegain = Math.Max(0f, baseDur - EA.ability.GCDTime);
                    // Determine how many we can still use
                    float availActs     = EAMaxActs - EA.allNumActivates;
                    newActsEA           = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    EA.numActivatesO20 += newActsEA * TimeOver20Perc;
                    EA.numActivatesU20 += newActsEA * TimeUndr20Perc;
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
                    && (Char.Race == CharacterRace.Undead) // We are Undead (qualifier for Will of the Forsaken)
                    && (WFMaxActs - WF.allNumActivates > 0f) // We haven't already used up Will of the Forsaken
                    && (Math.Min(0f, (WF.ability.GCDTime - baseDur)) < 0)) // We can regain time using Will of the Forsaken
                {
                    maxTimeRegain = Math.Max(0f, baseDur - WF.ability.GCDTime);
                    // Determine how many we can still use
                    float availActs     = WFMaxActs - WF.allNumActivates;
                    newActsWF           = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    WF.numActivatesO20 += newActsWF * TimeOver20Perc;
                    WF.numActivatesU20 += newActsWF * TimeUndr20Perc;
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
                    && (Char.Race == CharacterRace.Human) // We are a Human (qualifier for Every Man for Himself)
                    && (EMMaxActs - EM.allNumActivates > 0f) // We haven't already used up Every Man for Himself
                    && (Math.Min(0f, (EM.ability.GCDTime - baseDur)) < 0)) // We can regain time using Every Man for Himself
                {
                    maxTimeRegain = Math.Max(0f, baseDur - EM.ability.GCDTime);
                    // Determine how many we can still use
                    float availActs     = EMMaxActs - EM.allNumActivates;
                    newActsEM           = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    EM.numActivatesO20 += newActsEM * TimeOver20Perc;
                    EM.numActivatesU20 += newActsEM * TimeUndr20Perc;
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
                    && (HLMaxActs - HL.allNumActivates > 0f) // We haven't already used up Heroic Leap
                    && ((HL.ability.MinRange / MovementSpeed) < baseDur) // We meet the Minimum Range requirement for Heroic Leap
                    && (Math.Min(0f, HL.ability.GCDTime - baseDur) < 0)) // Heroic Leap's GCD Time won't be more than what we need to save
                {
                    maxTimeRegain = Math.Max(0f, Math.Min(baseDur - HL.ability.GCDTime, HL.ability.MaxRange / MovementSpeed - HL.ability.GCDTime)); // calc the actual max time we can regain
                    // Determine how many we can still use
                    float availActs = HLMaxActs - HL.allNumActivates;
                    newActsHL = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    HL.numActivatesO20 += newActsHL * TimeOver20Perc;
                    HL.numActivatesU20 += newActsHL * TimeUndr20Perc;
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
                    && (!CombatFactors.FuryStance && Char.WarriorTalents.Juggernaut > 0 ) // We are not in Fury Stance and have the Juggernaught Talent (qualifiers for Charge)
                    && (CHMaxActs - CH.allNumActivates > 0f) // We haven't already used up Charge
                    && ((CH.ability.MinRange / MovementSpeed) < baseDur) // We meet the Minimum Range requirement for Charge
                    && (Math.Min(0f, (CH.ability.GCDTime - baseDur)) < 0)) // We can regain time using Charge
                {
                    maxTimeRegain = Math.Max(0f, Math.Min(baseDur - CH.ability.GCDTime, CH.ability.MaxRange / MovementSpeed - CH.ability.GCDTime)); // calc the actual max time we can regain
                    // Determine how many we can still use
                    float availActs = CHMaxActs - CH.allNumActivates;
                    newActsCH = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    CH.numActivatesO20 += newActsCH * TimeOver20Perc;
                    CH.numActivatesU20 += newActsCH * TimeUndr20Perc;
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
                    && (CombatFactors.FuryStance) // We are in Fury Stance (qualifier for Intercept)
                    && ((INMaxActs - IN.allNumActivates) + (HFMaxActs - HF.allNumActivates) > 0f) // We haven't already used up Intercept, though we might be able to use Heroic Fury to reset the cooldown
                    && ((IN.ability.MinRange / MovementSpeed) < baseDur) // We meet the Minimum Range requirement for Intercept
                    && (Math.Min(0f, (IN.ability.GCDTime - baseDur)) < 0)) // We can regain time using Intercept
                {
                    maxTimeRegain = Math.Max(0f, Math.Min(baseDur - IN.ability.GCDTime, IN.ability.MaxRange / MovementSpeed - IN.ability.GCDTime)); // calc the actual max time we can regain
                    // Determine how many we can still use
                    float availActs = INMaxActs - IN.allNumActivates;
                    newActsIN = Math.Min(actsRemainingToCounter, availActs);
                    // Assign those as split to above and below Execute Phase
                    IN.numActivatesO20 += newActsIN * TimeOver20Perc;
                    IN.numActivatesU20 += newActsIN * TimeUndr20Perc;
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
                switch (iwt.type)
                {
                    case ImpedanceTypes.Fear: timelostwhilefeared += timelostwhileaffected; break;
                    case ImpedanceTypes.Root: timelostwhilerooted += timelostwhileaffected; break;
                    case ImpedanceTypes.Stun: timelostwhilestunned += timelostwhileaffected; break;
                    case ImpedanceTypes.Move: timelostwhilemoving += timelostwhileaffected; break;
                    //case ImpedanceTypes.Silence: timelostwhilesilenced += timelostwhileaffected; break;
                    case ImpedanceTypes.Disarm: timelostwhiledisarmed += timelostwhileaffected; break;
                    default: break;
                }
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
                                HF.ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsBZ > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsBZ, // Total Acts, limited by its other uses
                                newActsBZ * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsBZ * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurBZ, // Amount Recovered Per
                                newActsBZ * (baseDur - reducedDurBZ), // Total Amount Recovered
                                BZ.ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsEA > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsEA, // Total Acts, limited by its other uses
                                newActsEA * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsEA * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurEA, // Amount Recovered Per
                                newActsEA * (baseDur - reducedDurEA), // Total Amount Recovered
                                EA.ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsWF > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsWF, // Total Acts, limited by its other uses
                                newActsWF * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsWF * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurWF, // Amount Recovered Per
                                newActsWF * (baseDur - reducedDurWF), // Total Amount Recovered
                                WF.ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsEM > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsEM, // Total Acts, limited by its other uses
                                newActsEM * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsEM * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurEM, // Amount Recovered Per
                                newActsEM * (baseDur - reducedDurEM), // Total Amount Recovered
                                EM.ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsHL > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsHL, // Total Acts, limited by its other uses
                                newActsHL * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsHL * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurHL, // Amount Recovered Per
                                newActsHL * (baseDur - reducedDurHL), // Total Amount Recovered
                                HL.ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsCH > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsCH, // Total Acts, limited by its other uses
                                newActsCH * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsCH * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurCH, // Amount Recovered Per
                                newActsCH * (baseDur - reducedDurCH), // Total Amount Recovered
                                CH.ability.Name) // The Name
                            : "");
                        GCDUsage += (newActsIN > 0 ? // We added new ones
                            string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000}s : Recovered by {5}\n",
                                newActsIN, // Total Acts, limited by its other uses
                                newActsIN * TimeOver20Perc, // Acts Over 20, limited by its other uses
                                newActsIN * TimeUndr20Perc, // Acts Under 20, limited by its other uses
                                baseDur - reducedDurIN, // Amount Recovered Per
                                newActsIN * (baseDur - reducedDurIN), // Total Amount Recovered
                                IN.ability.Name) // The Name
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

            percTimeInFear = timelostwhilefeared / FightDuration;
            percTimeInRoot = timelostwhilerooted / FightDuration;
            percTimeInStun = timelostwhilestunned / FightDuration;
            percTimeInMove = timelostwhilemoving / FightDuration;
            //percTimeInSilence = timelostwhilesilenced / FightDuration;
            percTimeInDisarm = timelostwhiledisarmed / FightDuration;
            
            (GetWrapper<SecondWind>().ability as SecondWind).NumStunsOverDur = TimesRooted + TimesStunned;

            return percTimeInFear + percTimeInRoot + percTimeInStun + percTimeInMove /*+ percTimeInSilence*/ + percTimeInDisarm;
        }

        #region OLD METHODS
#if FALSE
        private float CalculateFear()
        {
            float percTimeInFear = 0f;
            if (BossOpts.FearingTargs && BossOpts.Fears.Count > 0)
            {
                float timeUnder20 = CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f;
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
                foreach (Impedance f in BossOpts.Fears)
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
            if (BossOpts.RootingTargs && BossOpts.Roots.Count > 0)
            {
                float timeUnder20 = CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f;
                float timelostwhilerooted = 0f;
                float BaseRootDur = 0f, rootActs = 0f, reducedDur = 0f,
                      MaxTimeRegain = 0f,
                      ChanceYouAreRooted = 1f;
                AbilWrapper HF = GetWrapper<HeroicFury>();          float HFMaxActs = HF.ability.Activates;                      float HFActualActs = 0f;
                AbilWrapper EM = GetWrapper<EveryManForHimself>();  float EMMaxActs = EM.ability.Activates - EM.allNumActivates; float EMOldActs = EM.allNumActivates;
                AbilWrapper EA = GetWrapper<EscapeArtist>();        float EAMaxActs = EA.ability.Activates - EA.allNumActivates; float EAOldActs = EA.allNumActivates;
                TimesRooted = 0f;
                foreach (Impedance r in BossOpts.Roots)
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
            if (BossOpts.StunningTargs && BossOpts.Stuns.Count > 0)
            {
                float timeUnder20 = CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f;
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
                foreach (Impedance s in BossOpts.Stuns)
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
            if (BossOpts.MovingTargs && BossOpts.Moves.Count > 0)
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
                float timeUnder20 = CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f;
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
                foreach (Impedance m in BossOpts.Moves)
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
        #region Berserker Rage
        private static readonly SpecialEffect _SE_ZERKERDUMMY = new SpecialEffect(Trigger.Use,
            new Stats() { BonusAgilityMultiplier = 1f }, // this is just so we can use a Perc Mod without having to make a new stat
            10f, 30f);
        #endregion
        #region Battle Shout
        /// <summary>2d Array,  Glyph of Battle 0-1, Booming Voice 0-2, Cata no longer has Comm Presence</summary>
        private static readonly SpecialEffect[/*Glyph:0-1*/][/*boomVoice:0-2*/] _SE_BattleShout = {
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, new Stats() { Strength = 1395f, Agility = 1395f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 0 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 0 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Strength = 1395f, Agility = 1395f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 1 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 1 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Strength = 1395f, Agility = 1395f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 2 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 2 * 0.25f))) },
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, new Stats() { Strength = 1395f, Agility = 1395f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 0 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 0 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Strength = 1395f, Agility = 1395f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 1 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 1 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Strength = 1395f, Agility = 1395f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 2 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 2 * 0.25f))) },
        };
        #endregion
        #region Commanding Shout
        /// <summary>2d Array, Glyph of Command 0-1, Booming Voice 0-2, Cata no longer has Comm Presence</summary>
        private static readonly SpecialEffect[/*Glyph:0-1*/][/*boomVoice:0-2*/] _SE_CommandingShout = {
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, new Stats() { Stamina = 1485f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 0 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 0 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Stamina = 1485f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 1 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 1 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Stamina = 1485f, }, ((2f + (false ? 2f : 0f)) * 60f * (1f + 2 * 0.25f)), ((2f + (false ? 2f : 0f)) * 60f * (1f + 2 * 0.25f))) },
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, new Stats() { Stamina = 1485f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 0 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 0 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Stamina = 1485f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 1 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 1 * 0.25f))),
                                  new SpecialEffect(Trigger.Use, new Stats() { Stamina = 1485f, }, ((2f + (true  ? 2f : 0f)) * 60f * (1f + 2 * 0.25f)), ((2f + (true  ? 2f : 0f)) * 60f * (1f + 2 * 0.25f))) },
        };
        #endregion
        #region Demoralizing Shout
        private static readonly SpecialEffect[] _SE_DemoralizingShout = {
            new SpecialEffect(Trigger.Use, new Stats() { PhysicalDamageTakenMultiplier = -0.10f, }, 30f + (false ? 15f : 0f), 30f + (false ? 15f : 0f)),
            new SpecialEffect(Trigger.Use, new Stats() { PhysicalDamageTakenMultiplier = -0.10f, }, 30f + (true  ? 15f : 0f), 30f + (true  ? 15f : 0f)),
        };
        #endregion
        #region Recklessness, Shattering Throw, ThunderClap, Sunder Armor, Sweeping Strikes
        //private static Dictionary<float, SpecialEffect> _SE_Recklessness = new Dictionary<float, SpecialEffect>();
        //private static Dictionary<float, SpecialEffect> _SE_ShatteringThrow = new Dictionary<float, SpecialEffect>();
        //private static Dictionary<float, SpecialEffect> _SE_ThunderClap = new Dictionary<float, SpecialEffect>();
        //private static Dictionary<float, SpecialEffect> _SE_SunderArmor = new Dictionary<float, SpecialEffect>();
        //private static Dictionary<float, SpecialEffect> _SE_SweepingStrikes = new Dictionary<float, SpecialEffect>();
        #endregion
        #endregion

        public void AddValidatedSpecialEffects(Stats statsTotal, WarriorTalents talents)
        {
            try
            {
                Ability ST = GetWrapper<ShatteringThrow>().ability,
                        BTS = GetWrapper<BattleShout>().ability,
                        CS = GetWrapper<CommandingShout>().ability,
                        DS = GetWrapper<DemoralizingShout>().ability,
                        TH = GetWrapper<ThunderClap>().ability,
                        SN = GetWrapper<SunderArmor>().ability,
                        SW = GetWrapper<SweepingStrikes>().ability,
                        RK = GetWrapper<Recklessness>().ability;
                if (BTS.Validated) { statsTotal.AddSpecialEffect(_SE_BattleShout[talents.GlyphOfBattle ? 0 : 1][talents.BoomingVoice]); }
                if (CS.Validated) { statsTotal.AddSpecialEffect(_SE_CommandingShout[talents.GlyphOfCommand ? 0 : 1][talents.BoomingVoice]); }
                if (DS.Validated) { statsTotal.AddSpecialEffect(_SE_DemoralizingShout[talents.GlyphOfDemoralizingShout ? 0 : 1]); }
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
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { TargetArmorReduction = 0.20f, }, ST.Duration, ST.Cd, ST.MHAtkTable.AnyLand));
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
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BossAttackSpeedMultiplier = -0.10f, }, TH.Duration, TH.Cd, TH.MHAtkTable.AnyLand));
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
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { TargetArmorReduction = 0.04f, }, SN.Duration, SN.Cd, SN.MHAtkTable.AnyLand, 5));
                }
                float landedAtksInterval = LandedAtksOverDur / FightDuration;
                float critRate = CriticalAtksOverDur / AttemptedAtksOverDur;
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
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { BonusTargets = 1f, }, landedAtksInterval * 5f, SW.Cd));
                }
                if (RK.Validated && CombatFactors.FuryStance) {
                    /*try {
                        float interval = (float)Math.Round(landedAtksInterval * 3f, 3);
                        if (!_SE_Recklessness.ContainsKey(interval)) {
                            try {
                                _SE_Recklessness.Add(interval, new SpecialEffect(Trigger.Use, new Stats() { PhysicalCrit = 1f }, landedAtksInterval * 3f, RK.Cd));
                            } catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                        }
                        statsTotal.AddSpecialEffect(_SE_Recklessness[interval]);
                    } catch (Exception) { } // Do nothing, this is a Silverlight retard bug*/
                    statsTotal.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { PhysicalCrit = 1f }, landedAtksInterval * 3f, RK.Cd));
                }
                /*if (talents.Flurry > 0 && CalcOpts.FuryStance)
                { // NOTE!!!! This comment is still using the old method, we need to cache values like you see above
                    //float value = talents.Flurry * 0.05f;
                    SpecialEffect flurry = new SpecialEffect(Trigger.MeleeCrit,
                        new Stats() { PhysicalHaste = talents.Flurry * 0.05f, }, landedAtksInterval * 3f, 0f);
                    statsTotal.AddSpecialEffect(flurry);
                }*/
            } catch (Exception ex) {
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error in creating Special Effects Caches",
                    ex.Message, ex.InnerException,
                    "AddValidatedSpecialEffects(...)", "No Additional Info", ex.StackTrace);
                eb.Show();
            }
        }

        internal void ResetHitTables()
        {
            foreach (AbilWrapper aw in GetAbilityList())
            {
                if (aw.ability.CanCrit)
                {
                    aw.ability.MHAtkTable.Reset();
                    aw.ability.OHAtkTable.Reset();
                }
            }
        }
    }
}
