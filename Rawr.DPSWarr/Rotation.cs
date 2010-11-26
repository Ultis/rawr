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
            public float RageO20 { get { return ability.GetRageUseOverDur(numActivatesO20); } }
            public float DPSO20 { get { return ability.GetDPS(numActivatesO20); } }
            public float HPSO20 { get { return ability.GetHPS(numActivatesO20); } }
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
            public float RageU20 { get { return ability.GetRageUseOverDur(numActivatesU20); } }
            public float DPSU20 { get { return ability.GetDPS(numActivatesU20); } }
            public float HPSU20 { get { return ability.GetHPS(numActivatesU20); } }
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
            public float allDPS { get { return DPSO20 + DPSU20; } }
            public float allHPS { get { return HPSO20 + HPSU20; } }

            public override string ToString()
            {
                if (ability == null) return "NULLed";
                return string.Format("{0} : Rage {1:0.##} : DPS {2:0.##} : HPS {3:0.##}", ability.Name, allRage, allDPS, allHPS);
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

        private Dictionary<Type,AbilWrapper> AbilityList;

        public List<AbilWrapper> GetDamagingAbilities() { return new List<AbilWrapper>(AbilityList.Values).FindAll(e => e.isDamaging); }
        public List<AbilWrapper> GetMaintenanceAbilities() { return new List<AbilWrapper>(AbilityList.Values).FindAll(e => e.ability.isMaint); }
        public List<AbilWrapper> GetAbilityList() { return new List<AbilWrapper>(AbilityList.Values); }
        public List<AbilWrapper> GetAbilityListThatGCDs() { return new List<AbilWrapper>(AbilityList.Values).FindAll(e => e.ability.UsesGCD); }

        public AbilWrapper GetWrapper<T>() { return AbilityList[typeof(T)]; }

        public float _HPS_TTL;
        public float _DPS_TTL_O20, _DPS_TTL_U20;
        public string GCDUsage = "";
        protected CharacterCalculationsDPSWarr calcs = null;
        
        public bool _needDisplayCalcs = true;
        
        //protected float FightDuration;
        protected float TimeLostGCDsO20, TimeLostGCDsU20;
        protected float RageGainedWhileMoving;
        public float TimesStunned = 0f;
        public float TimesFeared = 0f;
        public float TimesRooted = 0f;
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
            AbilityList.Clear();
            WhiteAtks.InvalidateCache();
            // Whites
            //WhiteAtks = new Skills.WhiteAtks(Char, StatS, CombatFactors);
            // Anti-Debuff
            AddAbility(new AbilWrapper(new Skills.HeroicFury(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.EveryManForHimself(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.Charge(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.Intercept(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.Intervene(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            // Rage Generators
            AddAbility(new AbilWrapper(new Skills.SecondWind(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.BerserkerRage(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.DeadlyCalm(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.InnerRage(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            // Maintenance
            AddAbility(new AbilWrapper(new Skills.BattleShout(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.CommandingShout(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.DemoralizingShout(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.SunderArmor(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.Hamstring(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.EnragedRegeneration(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            // Periodics
            AddAbility(new AbilWrapper(new Skills.ShatteringThrow(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.SweepingStrikes(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.DeathWish(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));
            AddAbility(new AbilWrapper(new Skills.Recklessness(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));

            // Arms abilities
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
            Skills.Ability WW = new Skills.WhirlWind(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts);
            AddAbility(new AbilWrapper(WW));
            Ability BT = new Skills.BloodThirst(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts);
            AddAbility(new AbilWrapper(BT));
            AddAbility(new AbilWrapper(new Skills.BloodSurge(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts, SL, WW, BT)));
            AddAbility(new AbilWrapper(new Skills.RagingBlow(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts)));

            DW = new Skills.DeepWounds(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, BossOpts);
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

        protected virtual float RageGenOverDur_AngerO20 { get { return CombatFactors.FuryStance ? 0f : (1.0f / 3.0f) * (FightDuration * (1f - (CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f))); } }
        protected virtual float RageGenOverDur_AngerU20 { get { return CombatFactors.FuryStance ? 0f : (1.0f / 3.0f) * (FightDuration * (CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f)); } }
        /// <summary>
        /// Anger Management is an Arms Spec Bonus in Cata, 1 rage every 3 seconds
        /// </summary>
        protected virtual float RageGenOverDur_Anger { get { return RageGenOverDur_AngerO20 + RageGenOverDur_AngerU20; } }
        
        protected virtual float RageGenOverDur_OtherO20 {
            get {
                float rage = RageGenOverDur_AngerO20               // Anger Management Talent
                            + RageGenOverDur_IncDmg * (1f - (CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f))              // Damage from the bosses
                            + (100f * StatS.ManaorEquivRestore) * (1f - (CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f))  // 0.02f becomes 2f
                            + StatS.BonusRageGen * (1f - (CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f));                // Bloodrage, Berserker Rage, Mighty Rage Pot

                foreach (AbilWrapper aw in GetAbilityList()) { if (aw.allRage < 0) { rage += (-1f) * aw.RageO20; } }

                return rage;
            }
        }
        protected virtual float RageGenOverDur_OtherU20 {
            get {
                float rage = RageGenOverDur_AngerU20               // Anger Management Talent
                            + RageGenOverDur_IncDmg * (CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f)              // Damage from the bosses
                            + (100f * StatS.ManaorEquivRestore) * (CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f)  // 0.02f becomes 2f
                            + StatS.BonusRageGen * (CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f);                // Bloodrage, Berserker Rage, Mighty Rage Pot

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
                float FightDurOver20 = FightDuration * (1f - (float)BossOpts.Under20Perc);
                float numAffectedItems = _SE_BattleTrance[Talents.BattleTrance].GetAverageProcsPerSecond(
                    FightDurOver20 / ms.allNumActivates, ms.ability.MHAtkTable.AnyLand, 3.3f, FightDurOver20)
                    * FightDurOver20;
                float percAffectedVsUnAffected = numAffectedItems / (AttemptedAtksOverDurMH * (1f - (float)BossOpts.Under20Perc));
                return 1f - percAffectedVsUnAffected;
            }
        }
        protected float RageMOD_Total { get { return RageMOD_DeadlyCalm; } }

        public int FightDuration { get { return BossOpts.BerserkTimer; } }

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
            if (!aw.ability.Validated) return 0f;

            float Abil_GCDs = Math.Min(GCDsAvailableO20, aw.ability.Activates * (1f - totalPercTimeLost));
            // If we are using Executer phase, distribute the abilities like they should be
            aw.numActivatesO20    = Abil_GCDs * (CalcOpts.M_ExecuteSpam ? 1f - (float)BossOpts.Under20Perc : 1.00f);
            aw.numActivatesU20 = Abil_GCDs * (CalcOpts.M_ExecuteSpam ?      (float)BossOpts.Under20Perc : 0.00f);
            if (_needDisplayCalcs && Abil_GCDs > 0) {
                GCDUsage += string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                    aw.allNumActivates, aw.numActivatesO20, aw.numActivatesU20,
                    aw.ability.Name, (!aw.ability.UsesGCD ? " (Doesn't use GCDs)" : ""));
            }

            _HPS_TTL += aw.allHPS;
            _DPS_TTL_O20 += aw.allDPS;
            return aw.ability.GetRageUseOverDur(Abil_GCDs) * (aw.ability.RageCost > 0 ? RageMOD_Total : 1f);
        }
        #endregion

        #region Lost Time due to Combat Factors
        private float _emActs/*, _emRecovery, _emRecoveryTotal*/;

        /// <summary>
        /// Calculates percentage of time lost due to moving, being rooted, etc
        /// </summary>
        /// <param name="MS">Placeholder right now for juggernaut handling.  Fury should pass null</param>
        /// <returns>Percentage of time lost as a float</returns>
        protected float CalculateTimeLost()
        {
            _emActs = 0f; //_emRecovery = 0f; _emRecoveryTotal = 0f;
            TimeLostGCDsO20 = TimeLostGCDsU20 = 0;
            RageGainedWhileMoving = 0; 

            float percTimeInMovement = CalculateMovement();
            float percTimeInFear = CalculateFear();
            float percTimeInStun = CalculateStun();
            float percTimeInRoot = CalculateRoot();
            return Math.Min(1f, percTimeInStun + percTimeInMovement + percTimeInFear + percTimeInRoot);
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
                AbilWrapper HF = GetWrapper<HeroicFury>();
                float HFMaxActs = HF.ability.Activates;
                float HFActualActs = 0f;
                AbilWrapper EM = GetWrapper<EveryManForHimself>();
                float EMMaxActs = EM.ability.Activates - EM.allNumActivates;
                float EMOldActs = EM.allNumActivates;
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
                            _emActs = EM.allNumActivates;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseRootDur - MaxTimeRegain);
                            float percEMdVsUnEMd = EMNewActs / rootActs;
                            timelostwhilerooted += (reducedDur * rootActs * percEMdVsUnEMd * ChanceYouAreRooted)
                                                 + (BaseRootDur * rootActs * (1f - percEMdVsUnEMd) * ChanceYouAreRooted);
                        } else {
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
                }
                TimeLostGCDsO20 += Math.Min(NumGCDsO20, (BaseRootDur * TimesRooted * (1f - timeUnder20)) / LatentGCD);
                TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * HF.numActivatesO20) / LatentGCD);
                TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * _emActs * (1f - timeUnder20)) / LatentGCD);

                TimeLostGCDsU20 += Math.Min(NumGCDsU20, (BaseRootDur * TimesRooted * timeUnder20) / LatentGCD);
                TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * HF.numActivatesU20) / LatentGCD);
                TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * _emActs * timeUnder20) / LatentGCD);

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
                AbilWrapper EM = GetWrapper<EveryManForHimself>();
                float EMMaxActs = EM.ability.Activates - EM.allNumActivates;
                float EMOldActs = EM.allNumActivates;
                TimesStunned = 0f;
                foreach (Impedance s in BossOpts.Stuns)
                {
                    ChanceYouAreStunned = s.Chance;
                    BaseStunDur = Math.Max(0f, (s.Duration / 1000f * (1f - StatS.StunDurReduc)));
                    stunActs = FightDuration / s.Frequency;
                    if (stunActs > 0f)
                    {
                        TimesStunned += stunActs;
                        if (Char.Race == CharacterRace.Human && EMMaxActs - EM.allNumActivates > 0f) {
                            MaxTimeRegain = Math.Max(0f, (BaseStunDur - LatentGCD - CalcOpts.React / 1000f));

                            float availEMacts = EM.ability.Activates - EM.allNumActivates;
                            float EMNewActs = Math.Min(stunActs, availEMacts);
                            EM.numActivatesO20 += EMNewActs * (1f - timeUnder20);
                            EM.numActivatesU20 += EMNewActs * timeUnder20;
                            _emActs = EM.allNumActivates;
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
                if (_needDisplayCalcs && TimesStunned > 0) {
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
                }
                TimeLostGCDsO20 += Math.Min(NumGCDsO20, (BaseStunDur * TimesStunned * (1f - timeUnder20)) / LatentGCD);
                TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * _emActs * (1f - timeUnder20)) / LatentGCD);

                TimeLostGCDsU20 += Math.Min(NumGCDsU20, (BaseStunDur * TimesStunned * timeUnder20) / LatentGCD);
                TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * _emActs * timeUnder20) / LatentGCD);

                percTimeInStun = timelostwhilestunned / FightDuration;

                SecondWind SndW = GetWrapper<SecondWind>().ability as SecondWind;
                SndW.NumStunsOverDur += stunActs;
            }
            
            return percTimeInStun;
        }

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
                            _emActs = EM.allNumActivates;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseFearDur - MaxTimeRegain);
                            float percEMdVsUnEMd = EMNewActs / fearActs;
                            timelostwhilefeared += (reducedDur * fearActs * percEMdVsUnEMd * ChanceYouAreFeared)
                                                 + (BaseFearDur * fearActs * (1f - percEMdVsUnEMd) * ChanceYouAreFeared);
                        } else {
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
                }
                TimeLostGCDsO20 += Math.Min(NumGCDsO20, (BaseFearDur * TimesFeared * (1f - timeUnder20)) / LatentGCD);
                TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * BZ.numActivatesO20) / LatentGCD);
                TimeLostGCDsO20 -= Math.Min(TimeLostGCDsO20, (reducedDur * _emActs * (1f - timeUnder20)) / LatentGCD);

                TimeLostGCDsU20 += Math.Min(NumGCDsU20, (BaseFearDur * TimesFeared * timeUnder20) / LatentGCD);
                TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * BZ.numActivatesU20) / LatentGCD);
                TimeLostGCDsU20 -= Math.Min(TimeLostGCDsU20, (reducedDur * _emActs * timeUnder20) / LatentGCD);

                percTimeInFear = timelostwhilefeared / FightDuration;
            }
            
            return percTimeInFear;
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
        private static Dictionary<float, SpecialEffect> _SE_Recklessness = new Dictionary<float, SpecialEffect>();
        private static Dictionary<float, SpecialEffect> _SE_ShatteringThrow = new Dictionary<float, SpecialEffect>();
        private static Dictionary<float, SpecialEffect> _SE_ThunderClap = new Dictionary<float, SpecialEffect>();
        private static Dictionary<float, SpecialEffect> _SE_SunderArmor = new Dictionary<float, SpecialEffect>();
        private static Dictionary<float, SpecialEffect> _SE_SweepingStrikes = new Dictionary<float, SpecialEffect>();
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
                if (ST.Validated)
                {
                    float value = (float)Math.Round(ST.MHAtkTable.AnyLand, 3);
                    if (!_SE_ShatteringThrow.ContainsKey(value))
                    {
                        try {
                            _SE_ShatteringThrow.Add(value, new SpecialEffect(Trigger.Use, new Stats() { TargetArmorReduction = 0.20f, }, ST.Duration, ST.Cd, ST.MHAtkTable.AnyLand));
                        } catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                    }
                    statsTotal.AddSpecialEffect(_SE_ShatteringThrow[value]);
                }
                if (TH.Validated)
                {
                    float value = (int)((float)Math.Round(TH.MHAtkTable.AnyLand, 3)*1000);
                    if (!_SE_ThunderClap.ContainsKey(value)) {
                        try {
                            _SE_ThunderClap.Add(value, new SpecialEffect(Trigger.Use, new Stats() { BossAttackSpeedMultiplier = -0.10f, }, TH.Duration, TH.Cd, TH.MHAtkTable.AnyLand));
                        } catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                    }
                    statsTotal.AddSpecialEffect(_SE_ThunderClap[value]);
                }
                if (SN.Validated)
                {
                    float value = (float)Math.Round(SN.MHAtkTable.AnyLand, 3);
                    if (!_SE_SunderArmor.ContainsKey(value))
                    {
                        try {
                            _SE_SunderArmor.Add(value, new SpecialEffect(Trigger.Use, new Stats() { TargetArmorReduction = 0.04f, }, SN.Duration, SN.Cd, SN.MHAtkTable.AnyLand, 5));
                        } catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                    }
                    statsTotal.AddSpecialEffect(_SE_SunderArmor[value]);
                }
                float landedAtksInterval = LandedAtksOverDur / FightDuration;
                float critRate = CriticalAtksOverDur / AttemptedAtksOverDur;
                if (SW.Validated)
                {
                    float interval = (float)Math.Round(landedAtksInterval * 5f, 3);
                    if (!_SE_SweepingStrikes.ContainsKey(interval))
                    {
                        try {
                            _SE_SweepingStrikes.Add(interval, new SpecialEffect(Trigger.Use, new Stats() { BonusTargets = 1f, }, landedAtksInterval * 5f, SW.Cd));
                        } catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                    }
                    statsTotal.AddSpecialEffect(_SE_SweepingStrikes[interval]);
                }
                if (RK.Validated && CombatFactors.FuryStance)
                {
                    float interval = (float)Math.Round(landedAtksInterval * 3f, 3);
                    if (!_SE_Recklessness.ContainsKey(interval))
                    {
                        try {
                            _SE_Recklessness.Add(interval, new SpecialEffect(Trigger.Use, new Stats() { PhysicalCrit = 1f /*- critRate*/ }, landedAtksInterval * 3f, RK.Cd));
                        } catch (Exception) { } // Do nothing, this is a Silverlight retard bug
                    }
                    statsTotal.AddSpecialEffect(_SE_Recklessness[interval]);
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
