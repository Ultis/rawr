using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr {
    #region Subclasses
    /// <summary>Enumerator for creating a list of possible values for the Level box</summary>
    public enum POSSIBLE_LEVELS { NPC_80 = 80, NPC_81, NPC_82, NPC_83, }
    /// <summary>
    /// Enumerator for attack types, this mostly is for raid members that aren't
    /// being directly attacked to know when AoE damage is coming from the boss
    /// </summary>
    public enum ATTACK_TYPES { AT_MELEE, AT_RANGED, AT_AOE, }
    /// <summary>A single Attack of various types</summary>
    public struct Attack {
        /// <summary>The Name of the Attack</summary>
        public string Name;
        /// <summary>The type of damage done, use the ItemDamageType enumerator to select</summary>
        public ItemDamageType DamageType;
        /// <summary>The Unmitigated Damage per Hit for this attack</summary>
        public float DamagePerHit;
        /// <summary>The maximum number of party/raid members this attack can hit</summary>
        public float MaxNumTargets;
        /// <summary>The frequency of this attack (in seconds)</summary>
        public float AttackSpeed;
        /// <summary>The Attack Type (for AoE vs single-target Melee/Ranged)</summary>
        public ATTACK_TYPES AttackType;
    }
    #endregion
    public class BossList {
        // Constructors
        public BossList() {
            DamageTypes = new ItemDamageType[] { ItemDamageType.Physical, ItemDamageType.Nature, ItemDamageType.Arcane, ItemDamageType.Frost, ItemDamageType.Fire, ItemDamageType.Shadow, ItemDamageType.Holy, };
            list = new BossHandler[] {
                // ==== Tier 7 Content ====
                // Naxxramas
                new AnubRekhan_10(),
                new GrandWidowFaerlina_10(),
                new Maexxna_10(),
                new NoththePlaguebringer_10(),
                new HeigantheUnclean_10(),
                new Loatheb_10(),
                new InstructorRazuvious_10(),
                new GothiktheHarvester_10(),
                new FourHorsemen_10(),
                new Patchwerk_10(),
                new Grobbulus_10(),
                new Gluth_10(),
                new Thaddius_10(),
                new Sapphiron_10(),
                new KelThuzad_10(),
                // The Obsidian Sanctum
                new Shadron_10(),
                new Tenebron_10(),
                new Vesperon_10(),
                new Sartharion_10(),
                // Vault of Archavon
                new ArchavonTheStoneWatcher_10(),
                // The Eye of Eternity
                new Malygos_10(),
                // ==== Tier 7.5 Content ====
                // Naxxramas
                new AnubRekhan_25(),
                new GrandWidowFaerlina_25(),
                new Maexxna_25(),
                new NoththePlaguebringer_25(),
                new HeigantheUnclean_25(),
                new Loatheb_25(),
                new InstructorRazuvious_25(),
                new GothiktheHarvester_25(),
                new FourHorsemen_25(),
                new Patchwerk_25(),
                new Grobbulus_25(),
                new Gluth_25(),
                new Thaddius_25(),
                new Sapphiron_25(),
                new KelThuzad_25(),
                // The Obsidian Sanctum
                new Shadron_25(),
                new Tenebron_25(),
                new Vesperon_25(),
                new Sartharion_25(),
                // Vault of Archavon
                new ArchavonTheStoneWatcher_25(),
                // The Eye of Eternity
                new Malygos_25(),
                // ==== Tier 8 Content ====
                // Vault of Archavon
                new EmalonTheStormWatcher_10(),
                // Ulduar
                new Auriaya_10(),
                new Hodir_10(),
                // ==== Tier 8.5 Content ====
                // Vault of Archavon
                new EmalonTheStormWatcher_25(),
                // Ulduar
                new Auriaya_25(),
                new Hodir_25(),
                // ==== Tier 9 (10) Content ====
                // Vault of Archavon
                new KoralonTheFlameWatcher_10(),
                // Trial of the Crusader
                // ==== Tier 9 (25) Content ====
                // Vault of Archavon
                new KoralonTheFlameWatcher_25(),
                // Trial of the Crusader
                // ==== Tier 9 (10) H Content ====
                // Trial of the Grand Crusader
                // ==== Tier 9 (25) H Content ====
                // Trial of the Grand Crusader
            };
            TheEZModeBoss  = GenTheEZModeBoss(list);
            TheAvgBoss     = GenTheAvgBoss(list);
            TheHardestBoss = GenTheHardestBoss(list);
            // This one is for filtered lists, defaults to the full list above
            calledList = GenCalledList(FilterType.Content,"All");
        }
        #region Variables
        /// <summary>Global setting, the Character's level should always be 80 until the next expansion</summary>
        public const int NormCharLevel = 80;
        /// <summary>The primary list, this hold all bosses regardless of filters</summary>
        public BossHandler[] list;
        /// <summary>This is what modules actually see and is based upon current filters</summary>
        public BossHandler[] calledList;
        /// <summary>Variable for storing Damage Type (Physical, Nature, Holy, etc)</summary>
        private ItemDamageType[] DamageTypes;
        /// <summary>Checks all the bosses to find the easiest of each stat and combines them to a single boss. Does NOT pick the easiest boss in the list but MAKES a new one. This IS NOT affected by filters.</summary>
        public BossHandler TheEZModeBoss;
        /// <summary>Checks all the bosses to total up stats and average them out and combines them to a single boss, this is what most users should base their characters against. This IS NOT affected by filters.</summary>
        public BossHandler TheAvgBoss;
        /// <summary>Checks all the bosses to find the worst of each stat and combines them to a single boss. Does NOT pick the hardest boss in the list but MAKES a new one. This IS NOT affected by filters.</summary>
        public BossHandler TheHardestBoss;
        /// <summary>Checks all the bosses to find the easiest of each stat and combines them to a single boss. Does NOT pick the easiest boss in the list but MAKES a new one. This IS affected by filters.</summary>
        public BossHandler TheEZModeBoss_Called;
        /// <summary>Checks all the bosses to total up stats and average them out and combines them to a single boss, this is what most users should base their characters against. This IS affected by filters.</summary>
        public BossHandler TheAvgBoss_Called;
        /// <summary>Checks all the bosses to find the worst of each stat and combines them to a single boss. Does NOT pick the hardest boss in the list but MAKES a new one. This IS affected by filters.</summary>
        public BossHandler TheHardestBoss_Called;
        #endregion
        #region Functions
        // Called List Generation and Interaction
        public enum FilterType { Content=0, Instance, Version, Name }
        public BossHandler[] GenCalledList(FilterType ftype, string Filter) {
            if (Filter.Equals("All",StringComparison.OrdinalIgnoreCase)) {
                // Resets the calledList to the full, unfiltered List
                TheEZModeBoss_Called  = TheEZModeBoss;
                TheAvgBoss_Called     = TheAvgBoss;
                TheHardestBoss_Called = TheHardestBoss;
                return calledList = list;
            }
            // Generate a list based upon the specialized Filter, only 1 thing can be compared at a time though
            List<BossHandler> retList = new List<BossHandler>();
            foreach (BossHandler boss in list) {
                switch (ftype) {
                    case FilterType.Content:  { if (boss.Content.Equals( Filter,StringComparison.OrdinalIgnoreCase)) { retList.Add(boss); } break; }
                    case FilterType.Instance: { if (boss.Instance.Equals(Filter,StringComparison.OrdinalIgnoreCase)) { retList.Add(boss); } break; }
                    case FilterType.Version:  { if (boss.Version.Equals( Filter,StringComparison.OrdinalIgnoreCase)) { retList.Add(boss); } break; }
                    case FilterType.Name:     { if (boss.Name.Equals(    Filter,StringComparison.OrdinalIgnoreCase)) { retList.Add(boss); } break; }
                    default: { /*Invalid type, do nothing*/ break; }
                }
            }
            BossHandler[] retList2 = retList.ToArray();
            // Gen the special bosses based upon this filtered list
            TheEZModeBoss_Called  = GenTheEZModeBoss(retList2);
            TheAvgBoss_Called     = GenTheAvgBoss(retList2);
            TheHardestBoss_Called = GenTheHardestBoss(retList2);
            return calledList = retList2;
        }
        // Name Calling
        public List<string> GetBossNames() {
            List<string> names = new List<string>() { };
            names.Add(TheEZModeBoss.Name);
            names.Add(TheAvgBoss.Name);
            names.Add(TheHardestBoss.Name);
            foreach (BossHandler boss in calledList) {
                names.Add(boss.Name);
            }
            return names;
        }
        public string[] GetBossNamesAsArray() { return GetBossNames().ToArray(); }
        public List<string> GetBetterBossNames() {
            List<string> names = new List<string>() { };
            names.Add(TheEZModeBoss.Name);
            names.Add(TheAvgBoss.Name);
            names.Add(TheHardestBoss.Name);
            foreach (BossHandler boss in calledList) {
                string name = boss.Content + " : " + boss.Instance + " (" + boss.Version + ") " + boss.Name;
                names.Add(name);
            }
            return names;
        }
        public string[] GetBetterBossNamesAsArray() { return GetBetterBossNames().ToArray(); }
        public BossHandler GetBossFromName(string name) {
            BossHandler retBoss = new BossHandler();
            if      (TheEZModeBoss_Called.Name  == name) { retBoss = TheEZModeBoss_Called;  }
            else if (TheAvgBoss_Called.Name     == name) { retBoss = TheAvgBoss_Called;     }
            else if (TheHardestBoss_Called.Name == name) { retBoss = TheHardestBoss_Called; }
            else {
                foreach (BossHandler boss in calledList) {
                    if(boss.Name == name){
                        retBoss = boss;
                        break;
                    }
                }
            }
            return retBoss;
        }
        public BossHandler GetBossFromBetterName(string name) {
            BossHandler retBoss = new BossHandler();
            if      (TheEZModeBoss_Called.Name  == name) { retBoss = TheEZModeBoss_Called;  }
            else if (TheAvgBoss_Called.Name     == name) { retBoss = TheAvgBoss_Called;     }
            else if (TheHardestBoss_Called.Name == name) { retBoss = TheHardestBoss_Called; }
            else {
                foreach (BossHandler boss in calledList) {
                    string checkName = boss.Content + " : " + boss.Instance + " (" + boss.Version + ") " + boss.Name;
                    if(checkName == name){
                        retBoss = boss;
                        break;
                    }
                }
            }
            return retBoss;
        }
        public List<string> GetFilterList(FilterType ftype) {
            List<string> names = new List<string>() { };
            switch (ftype) {
                case FilterType.Content: {
                    if (!names.Contains(TheEZModeBoss_Called.Content)) { names.Add(TheEZModeBoss_Called.Content); }
                    if (!names.Contains(TheAvgBoss_Called.Content)) { names.Add(TheAvgBoss_Called.Content); }
                    if (!names.Contains(TheHardestBoss_Called.Content)) { names.Add(TheHardestBoss_Called.Content); }
                    foreach (BossHandler boss in calledList) { if (!names.Contains(boss.Content)) { names.Add(boss.Content); } }
                    break;
                }
                case FilterType.Instance: {
                    if (!names.Contains(TheEZModeBoss_Called.Instance)) { names.Add(TheEZModeBoss_Called.Instance); }
                    if (!names.Contains(TheAvgBoss_Called.Instance)) { names.Add(TheAvgBoss_Called.Instance); }
                    if (!names.Contains(TheHardestBoss_Called.Instance)) { names.Add(TheHardestBoss_Called.Instance); }
                    foreach (BossHandler boss in calledList) { if (!names.Contains(boss.Instance)) { names.Add(boss.Instance); } }
                    break;
                }
                case FilterType.Name: {
                    if (!names.Contains(TheEZModeBoss_Called.Name)) { names.Add(TheEZModeBoss_Called.Name); }
                    if (!names.Contains(TheAvgBoss_Called.Name)) { names.Add(TheAvgBoss_Called.Name); }
                    if (!names.Contains(TheHardestBoss_Called.Name)) { names.Add(TheHardestBoss_Called.Name); }
                    foreach (BossHandler boss in calledList) { if (!names.Contains(boss.Name)) { names.Add(boss.Name); } }
                    break;
                }
                case FilterType.Version: {
                    if (!names.Contains(TheEZModeBoss_Called.Version)) { names.Add(TheEZModeBoss_Called.Version); }
                    if (!names.Contains(TheAvgBoss_Called.Version)) { names.Add(TheAvgBoss_Called.Version); }
                    if (!names.Contains(TheHardestBoss_Called.Version)) { names.Add(TheHardestBoss_Called.Version); }
                    foreach (BossHandler boss in calledList) { if (!names.Contains(boss.Version)) { names.Add(boss.Version); } }
                    break;
                }
                default: { /*Invalid type, do nothing*/ break; }
            }
            return names;
        }
        public string[] GetFilterListAsArray(FilterType ftype) { return GetFilterList(ftype).ToArray(); }
        /// <summary>
        /// Generates a Fight Info description listing the stats of the fight as well as any comments listed for the boss
        /// </summary>
        /// <param name="bossName">The full name of the boss, can use BetterBossName</param>
        /// <param name="useBettername">Set to True if you are passing the BetterBossName version of the Boss Name for proper verification</param>
        /// <returns>The generated string</returns>
        public string GenInfoString(string bossName, bool useBettername) {
            BossHandler boss = useBettername ? GetBossFromBetterName(bossName) : GetBossFromName(bossName);
            string retVal = boss.GenInfoString();
            return retVal;
        }
        // The Special Bosses
        private BossHandler GenTheEZModeBoss(BossHandler[] passedList) {
            BossHandler retboss = new BossHandler();
            if (passedList.Length < 1) { return retboss; }
            float value = 0f;
            // Basics
            retboss.Name = "The Easiest Boss";
            value = 0f; foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.BerserkTimer); } retboss.BerserkTimer = (int)Math.Ceiling(value);
            value = passedList[0].Health; foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.Health); } retboss.Health = value;
            value = passedList[0].Armor; foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.Armor); } retboss.Armor = value;
            retboss.UseParryHaste = false;
            // Resistance
            foreach (ItemDamageType t in DamageTypes) {
                value = passedList[0].Resistance(t);
                foreach (BossHandler boss in passedList) {
                    value = Math.Min(value, boss.Resistance(t));
                }
                retboss.Resistance(t, value);
            }
            // Attacks
            {
                float perhit = passedList[0].MeleeAttack.DamagePerHit; foreach (BossHandler boss in passedList) { perhit = Math.Min(perhit, boss.MeleeAttack.Name == "Invalid" ? perhit : boss.MeleeAttack.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg = Math.Min(numtrg, boss.MeleeAttack.Name == "Invalid" ? numtrg : boss.MeleeAttack.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd = Math.Max(atkspd, boss.MeleeAttack.Name == "Invalid" ? atkspd : boss.MeleeAttack.AttackSpeed); }
                retboss.MeleeAttack = new Attack {
                    Name = "Melee Attack",
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = perhit,
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            {
                float perhit = passedList[0].SpellAttack.DamagePerHit; foreach (BossHandler boss in passedList) { perhit = Math.Min(perhit, boss.SpellAttack.Name == "Invalid" ? perhit : boss.SpellAttack.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg = Math.Min(numtrg, boss.SpellAttack.Name == "Invalid" ? numtrg : boss.SpellAttack.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd = Math.Max(atkspd, boss.SpellAttack.Name == "Invalid" ? atkspd : boss.SpellAttack.AttackSpeed); }
                retboss.SpellAttack = new Attack {
                    Name = "Spell Attack",
                    DamageType = ItemDamageType.Arcane,
                    DamagePerHit = perhit,//averaging min/max value
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            {
                float perhit = passedList[0].SpecialAttack_1.DamagePerHit; foreach (BossHandler boss in passedList) { perhit = Math.Min(perhit, boss.SpecialAttack_1.Name == "Invalid" ? perhit : boss.SpecialAttack_1.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg = Math.Min(numtrg, boss.SpecialAttack_1.Name == "Invalid" ? numtrg : boss.SpecialAttack_1.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd = Math.Max(atkspd, boss.SpecialAttack_1.Name == "Invalid" ? atkspd : boss.SpecialAttack_1.AttackSpeed); }
                retboss.SpecialAttack_1 = new Attack {
                    Name = "Special Attack 1",
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = perhit,
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            {
                float perhit = passedList[0].SpecialAttack_2.DamagePerHit; foreach (BossHandler boss in passedList) { perhit = Math.Min(perhit, boss.SpecialAttack_2.Name == "Invalid" ? perhit : boss.SpecialAttack_2.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg = Math.Min(numtrg, boss.SpecialAttack_2.Name == "Invalid" ? numtrg : boss.SpecialAttack_2.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd = Math.Max(atkspd, boss.SpecialAttack_2.Name == "Invalid" ? atkspd : boss.SpecialAttack_2.AttackSpeed); }
                retboss.SpecialAttack_2 = new Attack {
                    Name = "Special Attack 2",
                    DamageType = ItemDamageType.Frost,
                    DamagePerHit = perhit,//averaging min/max value
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            {
                float perhit = passedList[0].SpecialAttack_3.DamagePerHit; foreach (BossHandler boss in passedList) { perhit = Math.Min(perhit, boss.SpecialAttack_3.Name == "Invalid" ? perhit : boss.SpecialAttack_3.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg = Math.Min(numtrg, boss.SpecialAttack_3.Name == "Invalid" ? numtrg : boss.SpecialAttack_3.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd = Math.Max(atkspd, boss.SpecialAttack_3.Name == "Invalid" ? atkspd : boss.SpecialAttack_3.AttackSpeed); }
                retboss.SpecialAttack_3 = new Attack {
                    Name = "Special Attack 3",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = perhit,
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            {
                float perhit = passedList[0].SpecialAttack_4.DamagePerHit; foreach (BossHandler boss in passedList) { perhit = Math.Min(perhit, boss.SpecialAttack_4.Name == "Invalid" ? perhit : boss.SpecialAttack_4.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg = Math.Min(numtrg, boss.SpecialAttack_4.Name == "Invalid" ? numtrg : boss.SpecialAttack_4.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd = Math.Max(atkspd, boss.SpecialAttack_4.Name == "Invalid" ? atkspd : boss.SpecialAttack_4.AttackSpeed); }
                retboss.SpecialAttack_4 = new Attack {
                    Name = "Special Attack 4",
                    DamageType = ItemDamageType.Holy,
                    DamagePerHit = perhit,//averaging min/max value
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            // Situational Changes
            value = 0f;                               foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.InBackPerc_Melee  ); } retboss.InBackPerc_Melee   = value;
            value = 0f;                               foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.InBackPerc_Ranged ); } retboss.InBackPerc_Ranged  = value;
            value = passedList[0].MultiTargsPerc;     foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.MultiTargsPerc    ); } retboss.MultiTargsPerc     = value;
            value = passedList[0].MaxNumTargets;      foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.MaxNumTargets     ); } retboss.MaxNumTargets      = value;
            value = retboss.BerserkTimer;             foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.StunningTargsFreq ); } retboss.StunningTargsFreq  = value;
            value = passedList[0].StunningTargsDur;   foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.StunningTargsDur  ); } retboss.StunningTargsDur   = value;
            value = passedList[0].MovingTargsTime;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.MovingTargsTime   ); } retboss.MovingTargsTime    = value;
            value = passedList[0].DisarmingTargsPerc; foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.DisarmingTargsPerc); } retboss.DisarmingTargsPerc = value;
            value = retboss.BerserkTimer;             foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.FearingTargsFreq  ); } retboss.FearingTargsFreq   = value;
            value = passedList[0].FearingTargsDur;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.FearingTargsDur   ); } retboss.FearingTargsDur    = value;
            value = retboss.BerserkTimer;             foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.RootingTargsFreq  ); } retboss.RootingTargsFreq   = value;
            value = passedList[0].RootingTargsDur;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.RootingTargsDur   ); } retboss.RootingTargsDur    = value;
            //
            return retboss;
        }
        private BossHandler GenTheAvgBoss(BossHandler[] passedList) {
            BossHandler retboss = new BossHandler();
            if (passedList.Length < 1) { return retboss; }
            float value = 0f;
            int count = 0;
            bool use = false;
            // Basics
            retboss.Name = "The Average Boss";
            value = 0f; foreach (BossHandler boss in passedList) { value += boss.BerserkTimer; } value /= passedList.Length; retboss.BerserkTimer = (int)Math.Floor(value);
            value = 0f; foreach (BossHandler boss in passedList) { value += boss.Health; } value /= passedList.Length; retboss.Health = value;
            value = 0f; foreach (BossHandler boss in passedList) { value += boss.Armor; } value /= passedList.Length; retboss.Armor = value;
            use = false; foreach (BossHandler boss in passedList) { use |= boss.UseParryHaste; } retboss.UseParryHaste = use;
            // Resistance
            foreach (ItemDamageType t in DamageTypes) {
                value = 0f;
                foreach (BossHandler boss in passedList) {
                    value += boss.Resistance(t);
                }
                value /= passedList.Length;
                retboss.Resistance(t, value);
            }
            #region Attacks
            {
                count = 0;
                float perhit = passedList[0].MeleeAttack.DamagePerHit; foreach (BossHandler boss in passedList) { perhit += boss.MeleeAttack.Name == "Invalid" ? 0f : boss.MeleeAttack.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg += boss.MeleeAttack.Name == "Invalid" ? 0f : boss.MeleeAttack.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd += boss.MeleeAttack.Name == "Invalid" ? 0f : boss.MeleeAttack.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.MeleeAttack = new Attack { Name = "Melee Attack", DamageType = ItemDamageType.Physical, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                count = 0;
                float perhit = passedList[0].SpellAttack.DamagePerHit; foreach (BossHandler boss in passedList) { perhit += boss.SpellAttack.Name == "Invalid" ? 0f : boss.SpellAttack.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg += boss.SpellAttack.Name == "Invalid" ? 0f : boss.SpellAttack.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd += boss.SpellAttack.Name == "Invalid" ? 0f : boss.SpellAttack.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.SpellAttack = new Attack { Name = "Spell Attack", DamageType = ItemDamageType.Arcane, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                count = 0;
                float perhit = passedList[0].SpecialAttack_1.DamagePerHit; foreach (BossHandler boss in passedList) { perhit += boss.SpecialAttack_1.Name == "Invalid" ? 0f : boss.SpecialAttack_1.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg += boss.SpecialAttack_1.Name == "Invalid" ? 0f : boss.SpecialAttack_1.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd += boss.SpecialAttack_1.Name == "Invalid" ? 0f : boss.SpecialAttack_1.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.SpecialAttack_1 = new Attack { Name = "Special Attack 1", DamageType = ItemDamageType.Fire, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                count = 0;
                float perhit = passedList[0].SpecialAttack_2.DamagePerHit; foreach (BossHandler boss in passedList) { perhit += boss.SpecialAttack_2.Name == "Invalid" ? 0f : boss.SpecialAttack_2.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg += boss.SpecialAttack_2.Name == "Invalid" ? 0f : boss.SpecialAttack_2.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd += boss.SpecialAttack_2.Name == "Invalid" ? 0f : boss.SpecialAttack_2.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.SpecialAttack_2 = new Attack { Name = "Special Attack 2", DamageType = ItemDamageType.Frost, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                count = 0;
                float perhit = passedList[0].SpecialAttack_3.DamagePerHit; foreach (BossHandler boss in passedList) { perhit += boss.SpecialAttack_3.Name == "Invalid" ? 0f : boss.SpecialAttack_3.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg += boss.SpecialAttack_3.Name == "Invalid" ? 0f : boss.SpecialAttack_3.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd += boss.SpecialAttack_3.Name == "Invalid" ? 0f : boss.SpecialAttack_3.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.SpecialAttack_3 = new Attack { Name = "Special Attack 3", DamageType = ItemDamageType.Nature, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                count = 0;
                float perhit = passedList[0].SpecialAttack_4.DamagePerHit; foreach (BossHandler boss in passedList) { perhit += boss.SpecialAttack_4.Name == "Invalid" ? 0f : boss.SpecialAttack_4.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg += boss.SpecialAttack_4.Name == "Invalid" ? 0f : boss.SpecialAttack_4.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd += boss.SpecialAttack_4.Name == "Invalid" ? 0f : boss.SpecialAttack_4.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.SpecialAttack_4 = new Attack { Name = "Special Attack 4", DamageType = ItemDamageType.Holy, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            #endregion
            // Situational Changes
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.InBackPerc_Melee; } value /= passedList.Length; retboss.InBackPerc_Melee = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.InBackPerc_Ranged; } value /= passedList.Length; retboss.InBackPerc_Ranged = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.MultiTargsPerc; } value /= passedList.Length; retboss.MultiTargsPerc = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.MaxNumTargets; } value /= passedList.Length; retboss.MaxNumTargets = (float)Math.Ceiling(value);
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += (boss.StunningTargsFreq > 0 && boss.StunningTargsFreq < boss.BerserkTimer) ? boss.StunningTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; retboss.StunningTargsFreq = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.StunningTargsDur; } value /= passedList.Length; retboss.StunningTargsDur = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.MovingTargsTime / boss.BerserkTimer; } value /= passedList.Length; retboss.MovingTargsTime = value * retboss.BerserkTimer;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.DisarmingTargsPerc; } value /= passedList.Length; retboss.DisarmingTargsPerc = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += (boss.FearingTargsFreq > 0 && boss.FearingTargsFreq < boss.BerserkTimer) ? boss.FearingTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; retboss.FearingTargsFreq = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.FearingTargsDur; } value /= passedList.Length; retboss.FearingTargsDur = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += (boss.RootingTargsFreq > 0 && boss.RootingTargsFreq < boss.BerserkTimer) ? boss.RootingTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; retboss.RootingTargsFreq = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.RootingTargsDur; } value /= passedList.Length; retboss.RootingTargsDur = value;
            //
            return retboss;
        }
        private BossHandler GenTheHardestBoss(BossHandler[] passedList) {
            BossHandler retboss = new BossHandler();
            if (passedList.Length < 1) { return retboss; }
            float value = 0f;
            // Basics
            retboss.Name = "The Hardest Boss";
            value = passedList[0].BerserkTimer; foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.BerserkTimer); } retboss.BerserkTimer = (int)Math.Floor(value);
            value = 0f; foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.Health); } retboss.Health = value;
            value = 0f; foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.Armor); } retboss.Armor = value;
            retboss.UseParryHaste = true;
            // Resistance
            foreach (ItemDamageType t in DamageTypes) {
                value = 0f;
                foreach (BossHandler boss in passedList) {
                    value = Math.Max(value, boss.Resistance(t));
                }
                retboss.Resistance(t, value);
            }
            #region Attacks
            {
                float perhit = passedList[0].MeleeAttack.DamagePerHit;  foreach (BossHandler boss in passedList) { perhit = Math.Max(perhit, boss.MeleeAttack.Name == "Invalid" ? perhit : boss.MeleeAttack.DamagePerHit); }
                float numtrg = passedList[0].MeleeAttack.MaxNumTargets; foreach (BossHandler boss in passedList) { numtrg = Math.Max(numtrg, boss.MeleeAttack.Name == "Invalid" ? numtrg : boss.MeleeAttack.MaxNumTargets); }
                float atkspd = passedList[0].MeleeAttack.AttackSpeed;   foreach (BossHandler boss in passedList) { atkspd = Math.Min(atkspd, boss.MeleeAttack.Name == "Invalid" ? atkspd : boss.MeleeAttack.AttackSpeed); }
                retboss.MeleeAttack = new Attack { Name = "Melee Attack", DamageType = ItemDamageType.Physical, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                float perhit = passedList[0].SpellAttack.DamagePerHit;  foreach (BossHandler boss in passedList) { perhit = Math.Max(perhit, boss.SpellAttack.Name == "Invalid" ? perhit : boss.SpellAttack.DamagePerHit); }
                float numtrg = passedList[0].SpellAttack.MaxNumTargets; foreach (BossHandler boss in passedList) { numtrg = Math.Max(numtrg, boss.SpellAttack.Name == "Invalid" ? numtrg : boss.SpellAttack.MaxNumTargets); }
                float atkspd = passedList[0].SpellAttack.AttackSpeed;   foreach (BossHandler boss in passedList) { atkspd = Math.Min(atkspd, boss.SpellAttack.Name == "Invalid" ? atkspd : boss.SpellAttack.AttackSpeed); }
                retboss.SpellAttack = new Attack { Name = "Spell Attack", DamageType = ItemDamageType.Arcane, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                float perhit = passedList[0].SpecialAttack_1.DamagePerHit;  foreach (BossHandler boss in passedList) { perhit = Math.Max(perhit, boss.SpecialAttack_1.Name == "Invalid" ? perhit : boss.SpecialAttack_1.DamagePerHit); }
                float numtrg = passedList[0].SpecialAttack_1.MaxNumTargets; foreach (BossHandler boss in passedList) { numtrg = Math.Max(numtrg, boss.SpecialAttack_1.Name == "Invalid" ? numtrg : boss.SpecialAttack_1.MaxNumTargets); }
                float atkspd = passedList[0].SpecialAttack_1.AttackSpeed;   foreach (BossHandler boss in passedList) { atkspd = Math.Min(atkspd, boss.SpecialAttack_1.Name == "Invalid" ? atkspd : boss.SpecialAttack_1.AttackSpeed); }
                retboss.SpecialAttack_1 = new Attack { Name = "Special Attack 1", DamageType = ItemDamageType.Holy, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                float perhit = passedList[0].SpecialAttack_2.DamagePerHit;  foreach (BossHandler boss in passedList) { perhit = Math.Max(perhit, boss.SpecialAttack_2.Name == "Invalid" ? perhit : boss.SpecialAttack_2.DamagePerHit); }
                float numtrg = passedList[0].SpecialAttack_2.MaxNumTargets; foreach (BossHandler boss in passedList) { numtrg = Math.Max(numtrg, boss.SpecialAttack_2.Name == "Invalid" ? numtrg : boss.SpecialAttack_2.MaxNumTargets); }
                float atkspd = passedList[0].SpecialAttack_2.AttackSpeed;   foreach (BossHandler boss in passedList) { atkspd = Math.Min(atkspd, boss.SpecialAttack_2.Name == "Invalid" ? atkspd : boss.SpecialAttack_2.AttackSpeed); }
                retboss.SpecialAttack_2 = new Attack { Name = "Special Attack 2", DamageType = ItemDamageType.Shadow, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                float perhit = passedList[0].SpecialAttack_3.DamagePerHit;  foreach (BossHandler boss in passedList) { perhit = Math.Max(perhit, boss.SpecialAttack_3.Name == "Invalid" ? perhit : boss.SpecialAttack_3.DamagePerHit); }
                float numtrg = passedList[0].SpecialAttack_3.MaxNumTargets; foreach (BossHandler boss in passedList) { numtrg = Math.Max(numtrg, boss.SpecialAttack_3.Name == "Invalid" ? numtrg : boss.SpecialAttack_3.MaxNumTargets); }
                float atkspd = passedList[0].SpecialAttack_3.AttackSpeed;   foreach (BossHandler boss in passedList) { atkspd = Math.Min(atkspd, boss.SpecialAttack_3.Name == "Invalid" ? atkspd : boss.SpecialAttack_3.AttackSpeed); }
                retboss.SpecialAttack_3 = new Attack { Name = "Special Attack 3", DamageType = ItemDamageType.Nature, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                float perhit = passedList[0].SpecialAttack_4.DamagePerHit;  foreach (BossHandler boss in passedList) { perhit = Math.Max(perhit, boss.SpecialAttack_4.Name == "Invalid" ? perhit : boss.SpecialAttack_4.DamagePerHit); }
                float numtrg = passedList[0].SpecialAttack_4.MaxNumTargets; foreach (BossHandler boss in passedList) { numtrg = Math.Max(numtrg, boss.SpecialAttack_4.Name == "Invalid" ? numtrg : boss.SpecialAttack_4.MaxNumTargets); }
                float atkspd = passedList[0].SpecialAttack_4.AttackSpeed;   foreach (BossHandler boss in passedList) { atkspd = Math.Min(atkspd, boss.SpecialAttack_4.Name == "Invalid" ? atkspd : boss.SpecialAttack_4.AttackSpeed); }
                retboss.SpecialAttack_4 = new Attack { Name = "Special Attack 4", DamageType = ItemDamageType.Fire, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            #endregion
            // Situational Changes
            value = passedList[0].InBackPerc_Melee; foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.InBackPerc_Melee  ); } retboss.InBackPerc_Melee   = value;
            value = passedList[0].InBackPerc_Ranged;foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.InBackPerc_Ranged ); } retboss.InBackPerc_Ranged  = value;
            value = 0f;                             foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.MultiTargsPerc    ); } retboss.MultiTargsPerc     = value;
            value = 0f;                             foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.MaxNumTargets     ); } retboss.MaxNumTargets      = (float)Math.Ceiling(value);
            value = passedList[0].BerserkTimer;     foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.StunningTargsFreq != 0 ? boss.StunningTargsFreq : value); } retboss.StunningTargsFreq  = value;
            value = 0f;                             foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.StunningTargsDur  ); } retboss.StunningTargsDur   = value;
            value = 0f;                             foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.MovingTargsTime / boss.BerserkTimer); } retboss.MovingTargsTime = value * retboss.BerserkTimer;
            value = 0f;                             foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.DisarmingTargsPerc); } retboss.DisarmingTargsPerc = value;
            value = passedList[0].BerserkTimer;     foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.FearingTargsFreq != 0 ? boss.FearingTargsFreq : value); } retboss.FearingTargsFreq  = value;
            value = 0f;                             foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.FearingTargsDur  ); } retboss.FearingTargsDur   = value;
            value = passedList[0].BerserkTimer;     foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.RootingTargsFreq != 0 ? boss.RootingTargsFreq : value); } retboss.RootingTargsFreq  = value;
            value = 0f;                             foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.RootingTargsDur  ); } retboss.RootingTargsDur   = value;
            //
            return retboss;
        }
        #endregion
    }
    public class BossHandler {
        public const int NormCharLevel = 80;
        public BossHandler() {
            // Basics
            Name = "Generic";
            Content = "Generic";
            Instance = "None";
            Version = "10 Man";
            Comment = "No comments have been written for this Boss.";
            BerserkTimer = 8 * 60; // The longest noted Enrage timer is 19 minutes, and seriously, if the fight is taking that long, then fail... just fail.
            SpeedKillTimer = 3 * 60; // Lots of Achievements run on this timer, so using it for generic
            Level = (int)POSSIBLE_LEVELS.NPC_83;
            Health = 1000000f;
            Armor = (float)StatConversion.NPC_ARMOR[Level - NormCharLevel];
            UseParryHaste = false;
            // Resistance
            ItemDamageType[] DamageTypes = new ItemDamageType[] { ItemDamageType.Physical, ItemDamageType.Nature, ItemDamageType.Arcane, ItemDamageType.Frost, ItemDamageType.Fire, ItemDamageType.Shadow, ItemDamageType.Holy, };
            foreach (ItemDamageType t in DamageTypes) { Resistance(t, 0f); }
            // Attacks
            MeleeAttack     = new Attack { Name = "Invalid", DamageType = ItemDamageType.Physical, DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f, AttackType = ATTACK_TYPES.AT_MELEE,  };
            SpellAttack     = new Attack { Name = "Invalid", DamageType = ItemDamageType.Arcane,   DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f, AttackType = ATTACK_TYPES.AT_RANGED, }; 
            SpecialAttack_1 = new Attack { Name = "Invalid", DamageType = ItemDamageType.Arcane,   DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f, AttackType = ATTACK_TYPES.AT_AOE,    };
            SpecialAttack_2 = new Attack { Name = "Invalid", DamageType = ItemDamageType.Arcane,   DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f, AttackType = ATTACK_TYPES.AT_AOE,    };
            SpecialAttack_3 = new Attack { Name = "Invalid", DamageType = ItemDamageType.Arcane,   DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f, AttackType = ATTACK_TYPES.AT_AOE,    };
            SpecialAttack_4 = new Attack { Name = "Invalid", DamageType = ItemDamageType.Arcane,   DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f, AttackType = ATTACK_TYPES.AT_AOE,    };
            // Situational Changes
            InBackPerc_Melee   = 0.00f; // Default to never in back
            InBackPerc_Ranged  = 0.00f; // Default to never in back
            MultiTargsPerc     = 0.00f; // Default to 0% multiple targets
            MaxNumTargets      =    1f; // Default to max 2 targets (though at 0%, this means nothing)
            StunningTargsFreq  =    0f; // Default to never stunning
            StunningTargsDur   = 5000f; // Default to stun durations of 5 seconds but since it's 0 stuns over dur, this means nothing
            MovingTargsTime    =    0f; // Default to not moving at all during fight
            DisarmingTargsPerc = 0.00f; // None of the bosses disarm
            TimeBossIsInvuln   =    0f; // Default to never invulnerable (Invuln. like KT in Phase 1)
            FearingTargsFreq   =    0f; // Default to never fearing
            FearingTargsDur    = 5000f; // Default to fear durations of 5 seconds but since it's 0 fears over dur, this means nothing
            RootingTargsFreq   =    0f; // Default to never rooting
            RootingTargsDur    = 5000f; // Default to root durations of 5 seconds but since it's 0 roots over dur, this means nothing
            // Fight Requirements
            Max_Players = 10;
            Min_Healers =  3;
            Min_Tanks   =  2;
        }

        #region Variables
        // Basics
        private string NAME,CONTENT,INSTANCE,VERSION,COMMENT;
        private float HEALTH,ARMOR;
        private int BERSERKTIMER,SPEEDKILLTIMER,LEVEL;
        private bool USERPARRYHASTE;
        // Resistance
        private float RESISTANCE_PHYSICAL,RESISTANCE_NATURE,RESISTANCE_ARCANE,RESISTANCE_FROST,RESISTANCE_FIRE,RESISTANCE_SHADOW,RESISTANCE_HOLY;
        // Attacks
        private Attack MELEEATTACK,SPELLATTACK,SPECIALATTACK_1,SPECIALATTACK_2,SPECIALATTACK_3,SPECIALATTACK_4;
        // Situational Changes
        private float INBACKPERC_MELEE, INBACKPERC_RANGED,
                      MULTITARGSPERC, MAXNUMTARGS,
                      STUNNINGTARGS_FREQ, STUNNINGTARGS_DUR,
                      MOVINGTARGSTIME,
                      DISARMINGTARGSPERC,
                      TIMEBOSSISINVULN,
                      FEARINGTARGS_FREQ, FEARINGTARGS_DUR,
                      ROOTINGTARGS_FREQ, ROOTINGTARGS_DUR;
        // Fight Requirements
        private int MAX_PLAYERS, MIN_HEALERS, MIN_TANKS;
        #endregion

        #region Get/Set
        // ==== Basics ====
        public string Name               { get { return NAME;               } set { NAME               = value; } }
        public string Content            { get { return CONTENT;            } set { CONTENT            = value; } }
        public string Instance           { get { return INSTANCE;           } set { INSTANCE           = value; } }
        public string Version            { get { return VERSION;            } set { VERSION            = value; } }
        public string Comment            { get { return COMMENT;            } set { COMMENT            = value; } }
        public int    Level              { get { return LEVEL;              } set { LEVEL              = value; } }
        public float  Health             { get { return HEALTH;             } set { HEALTH             = value; } }
        public float  Armor              { get { return ARMOR;              } set { ARMOR              = value; } }
        public int    BerserkTimer       { get { return BERSERKTIMER;       } set { BERSERKTIMER       = value; } }
        public int    SpeedKillTimer     { get { return SPEEDKILLTIMER;     } set { SPEEDKILLTIMER     = value; } }
        public bool   UseParryHaste      { get { return USERPARRYHASTE;     } set { USERPARRYHASTE     = value; } }
        // ==== Attacks ====
        public Attack MeleeAttack        { get { return MELEEATTACK;        } set { MELEEATTACK        = value; } }
        public Attack SpellAttack        { get { return SPELLATTACK;        } set { SPELLATTACK        = value; } }
        public Attack SpecialAttack_1    { get { return SPECIALATTACK_1;    } set { SPECIALATTACK_1    = value; } }
        public Attack SpecialAttack_2    { get { return SPECIALATTACK_2;    } set { SPECIALATTACK_2    = value; } }
        public Attack SpecialAttack_3    { get { return SPECIALATTACK_3;    } set { SPECIALATTACK_3    = value; } }
        public Attack SpecialAttack_4    { get { return SPECIALATTACK_4;    } set { SPECIALATTACK_4    = value; } }
        // ==== Situational Changes ====
        // Standing in back
        public float  InBackPerc_Melee   { get { return INBACKPERC_MELEE;   } set { INBACKPERC_MELEE   = value; } }
        public float  InBackPerc_Ranged  { get { return INBACKPERC_RANGED;  } set { INBACKPERC_RANGED  = value; } }
        // Multiple Targets
        public float  MultiTargsPerc     { get { return MULTITARGSPERC;     } set { MULTITARGSPERC     = value; } }
        public float  MaxNumTargets      { get { return MAXNUMTARGS;        } set { MAXNUMTARGS        = value; } }
        // Stunning Targets
        public float  StunningTargsFreq  { get { return STUNNINGTARGS_FREQ; } set { STUNNINGTARGS_FREQ = value; } }
        public float  StunningTargsDur   { get { return STUNNINGTARGS_DUR ; } set { STUNNINGTARGS_DUR  = value; } }
        // Moving Targets
        public float  MovingTargsTime    { get { return MOVINGTARGSTIME;    } set { MOVINGTARGSTIME    = value; } }
        // Disarming Targets
        public float  DisarmingTargsPerc { get { return DISARMINGTARGSPERC; } set { DISARMINGTARGSPERC = value; } }
        public float  TimeBossIsInvuln   { get { return TIMEBOSSISINVULN;   } set { TIMEBOSSISINVULN   = value; } }
        // Fearing Targets
        public float  FearingTargsFreq   { get { return FEARINGTARGS_FREQ;  } set { FEARINGTARGS_FREQ  = value; } }
        public float  FearingTargsDur    { get { return FEARINGTARGS_DUR ;  } set { FEARINGTARGS_DUR   = value; } }
        // Rooting Targets
        public float  RootingTargsFreq   { get { return ROOTINGTARGS_FREQ;  } set { ROOTINGTARGS_FREQ  = value; } }
        public float  RootingTargsDur    { get { return ROOTINGTARGS_DUR ;  } set { ROOTINGTARGS_DUR   = value; } }
        // ==== Fight Requirements ====
        public int    Max_Players        { get { return MAX_PLAYERS;        } set { MAX_PLAYERS        = value; } }
        public int    Min_Healers        { get { return MIN_HEALERS;        } set { MIN_HEALERS        = value; } }
        public int    Min_Tanks          { get { return MIN_TANKS;          } set { MIN_TANKS          = value; } }
        #endregion

        #region Functions
        /// <summary>A handler for Damage Reduction due to Resistance (Physical, Fire, etc). </summary>
        /// <returns>The Percentage of Damage to be removed (0.25 = 25% Damage Reduced, 100 Damage should become 75)</returns>
        public float Resistance(ItemDamageType type) {
            switch (type) {
                case ItemDamageType.Physical: return RESISTANCE_PHYSICAL;
                case ItemDamageType.Nature:   return RESISTANCE_NATURE;
                case ItemDamageType.Arcane:   return RESISTANCE_ARCANE;
                case ItemDamageType.Frost:    return RESISTANCE_FROST;
                case ItemDamageType.Fire:     return RESISTANCE_FIRE;
                case ItemDamageType.Shadow:   return RESISTANCE_SHADOW;
                case ItemDamageType.Holy:     return RESISTANCE_HOLY;
                default: break;
            }
            return 0f;
        }
        /// <summary>A handler for Damage Reduction due to Resistance (Physical, Fire, etc). This is the Set function</summary>
        /// <returns>The Percentage of Damage to be removed (0.25 = 25% Damage Reduced, 100 Damage should become 75)</returns>
        public float Resistance(ItemDamageType type, float newValue) {
            switch (type) {
                case ItemDamageType.Physical: return RESISTANCE_PHYSICAL = newValue;
                case ItemDamageType.Nature:   return RESISTANCE_NATURE   = newValue;
                case ItemDamageType.Arcane:   return RESISTANCE_ARCANE   = newValue;
                case ItemDamageType.Frost:    return RESISTANCE_FROST    = newValue;
                case ItemDamageType.Fire:     return RESISTANCE_FIRE     = newValue;
                case ItemDamageType.Shadow:   return RESISTANCE_SHADOW   = newValue;
                case ItemDamageType.Holy:     return RESISTANCE_HOLY     = newValue;
                default: break;
            }
            return 0f;
        }
        /// <summary>
        /// Generates a Fight Info description listing the stats of the fight as well as any comments listed for the boss
        /// </summary>
        /// <returns>The generated string</returns>
        public string GenInfoString() {
            string retVal = "";
            //
            retVal += "Name: " + Name + "\r\n";
            retVal += "Content: " + Content + "\r\n";
            retVal += "Instance: " + Instance + " (" + Version + ")\r\n";
            retVal += "Health: " + Health.ToString("#,##0") + "\r\n";
            TimeSpan ts = TimeSpan.FromMinutes(BerserkTimer/60);
            retVal += "Enrage Timer: " + ts.Minutes.ToString("00") + " Min " + ts.Seconds.ToString("00") + " Sec\r\n";
            ts = TimeSpan.FromMinutes(SpeedKillTimer/60);
            retVal += "Speed Kill Timer: " + ts.Minutes.ToString("00") + " Min " + ts.Seconds.ToString("00") + " Sec\r\n";
            retVal += "\r\n";
            retVal += "Fight Requirements:" + "\r\n";
            retVal += "Max Players: "  + Max_Players.ToString() + "\r\n";
            retVal += "Min Healers: "  + Min_Healers.ToString() + "\r\n";
            retVal += "Min Tanks: "    + Min_Tanks.ToString() + "\r\n";
            int room = Max_Players - Min_Healers - Min_Tanks;
            retVal += "Room for DPS: " + room.ToString() + "\r\n";
            retVal += "\r\n";
            float TotalDPSNeeded = Health / (BerserkTimer - TimeBossIsInvuln - MovingTargsTime);
            retVal += "To beat the Enrage Timer you need " + TotalDPSNeeded.ToString("#,##0") + " Total DPS\r\n";
            float dpsdps  =  TotalDPSNeeded * ((float)room      / ((float)Min_Tanks / 2f + (float)room)) / (float)room;
            float tankdps = (TotalDPSNeeded * ((float)Min_Tanks / ((float)Min_Tanks / 2f + (float)room)) / (float)Min_Tanks) / 2f;
            retVal += "Tanks Req'd DPS per: " + tankdps.ToString("#,##0") + "\r\n";
            retVal += "DPS Req'd DPS per: " + dpsdps.ToString("#,##0") + "\r\n";
            retVal += "\r\n";
            TotalDPSNeeded = Health / (SpeedKillTimer - TimeBossIsInvuln - SpeedKillTimer * (MovingTargsTime / BerserkTimer));
            retVal += "To beat the Speed Kill Timer you need " + TotalDPSNeeded.ToString("#,##0") + " Total DPS\r\n";
            dpsdps = TotalDPSNeeded * ((float)room / ((float)Min_Tanks / 2f + (float)room)) / (float)room;
            tankdps = (TotalDPSNeeded * ((float)Min_Tanks / ((float)Min_Tanks / 2f + (float)room)) / (float)Min_Tanks) / 2f;
            retVal += "Tanks Req'd DPS per: " + tankdps.ToString("#,##0") + "\r\n";
            retVal += "DPS Req'd DPS per: " + dpsdps.ToString("#,##0") + "\r\n";
            retVal += "\r\n";
            retVal += "Comment(s):\r\n" + Comment;
            //
            return retVal;
        }
        #endregion
    }
    #region T7 Content
    // ===== Naxxramas ================================
    // Spider Wing
    public class AnubRekhan_10 : BossHandler {
        public AnubRekhan_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Anub'Rekhan";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2230000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Impale",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4813f + 6187f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // ==== Situational Changes ====
            // When he Impales, he turns around and faces the raid
            // simming this by using the activates over fight and having him facing raid for 2 seconds
            float time = (BerserkTimer / SpecialAttack_1.AttackSpeed) * 2f;
            InBackPerc_Melee = 1f - time / BerserkTimer;
            // Locust Swarm: Every 80-120 seconds for 16 seconds you can't be on the target
            // Adding 4 seconds to the Duration for moving out before starts and then back in after
            MovingTargsTime = (BerserkTimer / (80f + 120f / 2f)) * (16f+4f);
            // Every time he Locust Swarms he summons a Crypt Guard
            // Let's assume it's up for 10 seconds
            time  = (BerserkTimer / 60f) * 10f;
            // Every time he spawns a Crypt Guard and it dies, x seconds
            // after he summons 10 scarabs from it's body
            // Assuming they are up for 8 sec
            time += ((BerserkTimer-20f) / 60f) *  8f;
            MaxNumTargets = 10f;
            MultiTargsPerc = time / BerserkTimer;
            // ==== Fight Requirements ====
        }
    }
    public class GrandWidowFaerlina_10 : BossHandler {
        public GrandWidowFaerlina_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Grand Widow Faerlina";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2231200f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Poison Bold Volley",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (/*Initial*/(2625f + 3375f) / 2.0f) + (/*Dot*/((1480f+1720f)/2.0f)*8f/2f),
                MaxNumTargets = 3,
                AttackSpeed = (7.0f+15.0f)/2.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            SpecialAttack_2 = new Attack {
                Name = "Rain of Fire",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (/*Dot*/((1750f+2750f)/2.0f)*6f/2f),
                MaxNumTargets = 10,
                AttackSpeed = (6.0f+18.0f)/2.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            // Every 6-18 seconds for 3 seconds she has to be moved to compensate for Rain of Fire
            MovingTargsTime = (BerserkTimer / SpecialAttack_2.AttackSpeed) * (3f);
            // Fight Requirements
            /* TODO:
             * Frenzy
             * Worshippers
             */
        }
    }
    public class Maexxna_10 : BossHandler {
        public Maexxna_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Maexxna";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2510000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Web Spray",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (1750f + 2250f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.90f;
            // 6 second stun every 40 seconds
            StunningTargsFreq = 40f;
            StunningTargsDur = 6000f;
            // 8 Adds every 40 seconds for 8 seconds (only 7300 HP each)
            MultiTargsPerc = ((BerserkTimer / 40f) * 8f) / BerserkTimer;
            MaxNumTargets = 8;
            // Fight Requirements
            Min_Tanks = 1;
            Min_Healers = 2;
            /* TODO:
             * Web Wrap
             * Poison Shock
             * Necrotic Poison
             * Frenzy
             */
        }
    }
    // Plague Quarter
    public class NoththePlaguebringer_10 : BossHandler {
        public NoththePlaguebringer_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Noth the Plaguebringer";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2500000f;
            BerserkTimer = (110 + 70) * 3; // He enrages after 3rd iteration of Phase 2
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Impale",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4813f + 6187f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 30 seconds 2 adds will spawn with 100k HP each, simming their life-time to 20 seconds
            MultiTargsPerc = (BerserkTimer / 30f) * (20f) / BerserkTimer;
            // Fight Requirements
            Min_Tanks   = 1;
            Min_Healers = 2;
            /* TODO:
             * Phase 2
             */
        }
    }
    public class HeigantheUnclean_10 : BossHandler {
        public HeigantheUnclean_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Heigan the Unclean";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 3060000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Decrepit Fever",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = 3000f / 3f * 21f,
                MaxNumTargets = 1,
                AttackSpeed = 30.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            };
            // Situational Changes
            InBackPerc_Melee = 0.25f;
            // We are assuming you are using the corner trick so you don't have
            // to dance as much in 10 man
            // Every 90 seconds for 45 seconds you must do the safety dance
            MovingTargsTime = (BerserkTimer / 90f) * 45f;
            // Fight Requirements
            Min_Tanks = 1;
            /* TODO:
             */
        }
    }
    public class Loatheb_10 : BossHandler {
        public Loatheb_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Loatheb";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 6693600f;
            BerserkTimer = 5 * 60; // Inevitable Doom starts to get spammed every 15 seconds
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Deathbloom",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (/*DoT*/200f / 1f * 6f) + (/*Bloom*/1200f),
                MaxNumTargets = 10,
                AttackSpeed = 30.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            };
            SpecialAttack_1 = new Attack {
                Name = "Inevitable Doom",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = 4000 / 30 * 120,
                MaxNumTargets = 10,
                AttackSpeed = 120.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            };
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Initial 10 seconds to pop first Spore then every 3rd spore
            // after that (90 seconds respawn then 10 sec moving to/back)
            MovingTargsTime = 10 + (BerserkTimer / 90) * 10;
            // Fight Requirements
            Min_Tanks = 1;
            /* TODO:
             * Necrotic Aura
             * Fungal Creep
             */
        }
    }
    // Military Quarter
    public class InstructorRazuvious_10 : BossHandler {
        public InstructorRazuvious_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Instructor Razuvious";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 3349000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Disrupting Shout",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4275f + 4725f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 15.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            SpecialAttack_2 = new Attack {
                Name = "Jagged Knife",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 5000 + (10000 / 5 * 5),
                MaxNumTargets = 1,
                AttackSpeed = 10.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Fight Requirements
            // Every 70-120 seconds for 16 seconds you can't be on the target
            // Adding 4 seconds to the Duration for moving out before starts and then back in after
            /* TODO:
             * Unbalancing Strike
             * Using the Understudies
             */
        }
    }
    public class GothiktheHarvester_10 : BossHandler {
        public GothiktheHarvester_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Gothik the Harvester";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 839000f;
            BerserkTimer = BerserkTimer - (4 * 60 + 34);
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Shadowbolt",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2880f + 3520f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 1.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Fight Requirements
            /* TODO:
             * Phase 1
             * Harvest Soul
             */
        }
    }
    public class FourHorsemen_10 : BossHandler {
        public FourHorsemen_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Four Horsemen";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 781000f * 4f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Korth'azz's Meteor",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (13775f + 15225f) / 2f,
                MaxNumTargets = 8,
                AttackSpeed = 15.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            SpecialAttack_2 = new Attack {
                Name = "Rivendare's Unholy Shadow",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2160f + 2640f) / 2f + (4800/2*4),
                MaxNumTargets = 8,
                AttackSpeed = 15.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            };
            SpecialAttack_3 = new Attack {
                Name = "Blaumeux's Shadow Bolt",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2357f + 2643f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            };
            SpecialAttack_4 = new Attack {
                Name = "Zeliek's Holy Bolt",
                DamageType = ItemDamageType.Holy,
                DamagePerHit = (2357f + 2643f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            };
            // Situational Changes
            InBackPerc_Melee = 0.75f;
            // Swap 1st 2 mobs once: 15
            // Get to the back once: 10
            // Bounce back and forth in the back: Every 30 sec for 10 sec but for only 40% of the fight
            MovingTargsTime = 15f + 10f + ((BerserkTimer * 0.40f) / 30f) * 10f;
            // Fight Requirements
            Min_Tanks = 3; // simming 3rd to show that 2 dps have to OT the back
            Min_Healers = 3;
            /* TODO:
             * Blaumeux's Void Zone
             */
        }
    }
    // Construct Quarter
    public class Patchwerk_10 : BossHandler {
        public Patchwerk_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Patchwerk";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            BerserkTimer = 6 * 60;
            Health = 4320000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Hateful Strike",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (19975f + 27025f) / 2f,
                MaxNumTargets = 1f,
                AttackSpeed = 1.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Fight Requirements
            /* TODO:
             * Frenzy
             */
        }
    }
    public class Grobbulus_10 : BossHandler {
        public Grobbulus_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Grobbulus";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2928000f;
            BerserkTimer = 12 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 8 seconds for 3 seconds Grob has to be kited to
            // avoid Poison Cloud Farts. This goes on the entire fight
            MovingTargsTime  = (BerserkTimer / 8f) * 3f;
            // Every 20 seconds 1/10 chance to get hit with Mutating Injection
            // You have to run off for 10 seconds then run back for 4-5
            MovingTargsTime += ((BerserkTimer / 20f) * (10f+(4f+5f)/2f)) * 0.10f;
            /* TODO:
             * Slime Spray
             * Occasional Poins Cloud Ticks that are unavoidable
             */
        }
    }
    public class Gluth_10 : BossHandler {
        public Gluth_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Gluth";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 3230000f;
            BerserkTimer = 8 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 40000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            /* TODO:
             * Decimate
             * Enrage
             * Mortal Wound
             * Zombie Chows
             */
        }
    }
    public class Thaddius_10 : BossHandler {
        public Thaddius_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Thaddius";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 3850000f + 838300f; // one player only deals with one of the add's total health + thadd's health
            BerserkTimer = 6 * 60; // Need to verify if starts at beg. of combat or beg. of Thadd
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpellAttack = new Attack {
                Name = "Chain Lightning",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (3600f+4400f)/2f,
                MaxNumTargets = 3f,
                AttackSpeed = 15.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.50f;
            // Every 30 seconds, polarity shift, 3 sec move
            // 50% chance that your polarity will change
            MovingTargsTime = ((BerserkTimer / 30f) * 3f) * 0.50f;
            /* TODO:
             * Better handle of Feugen and Stalagg
             */
        }
    }
    // Frostwyrm Lair
    public class Sapphiron_10 : BossHandler {
        public Sapphiron_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Sapphiron";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 4250000f;
            BerserkTimer = 15 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Frost Aura",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = 1200f,
                MaxNumTargets = 10,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            SpecialAttack_2 = new Attack {
                Name = "Life Drain",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (((4376f+5624f)/2f) * 3f) * 4f,
                MaxNumTargets = 2,
                AttackSpeed = 24.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 45(+30) seconds for 30 seconds Sapph is in the air
            // He stops this at 10% hp
            MovingTargsTime = ((BerserkTimer / (45f+30f)) * 30f) * 0.90f;
            // Fight Requirements
            Min_Tanks = 3;
            /* TODO:
             * Chill (The Blizzard)
             * Ice Bolt
             */
        }
    }
    public class KelThuzad_10 : BossHandler {
        public KelThuzad_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Kel'Thuzad";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2230000f;
            BerserkTimer = 19 * 60;
            SpeedKillTimer = 6 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Impale",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4813f + 6187f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Phase 1, no damage to KT
            MovingTargsTime = 3f*60f + 48f;
            TimeBossIsInvuln = 3f * 60f + 48f;
            // Phase 2 & 3, gotta move out of Shadow Fissures periodically
            // We're assuming they pop every 30 seconds and you have to be
            // moved for 6 seconds and there's a 1/10 chance he will select
            // you over someone e;se
            MovingTargsTime = (((BerserkTimer - MovingTargsTime) / 30f) * 6f) * 0.10f;
            // Fight Requirements
            /* TODO:
             * The Mobs in Phase 1
             */
        }
    }
    // ===== The Obsidian Sanctum =====================
    public class Shadron_10 : BossHandler {
        public Shadron_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Shadron";
            Content = "T7";
            Instance = "The Obsidian Sanctum";
            Version = "10 Man";
            Health = 976150f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6188f + 8812f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            SpecialAttack_2 = new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6938f + 8062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 60 seconds for 20 seconds dps has to jump into the portal and kill the add
            MovingTargsTime  = (BerserkTimer / (60f+20f)) * (20f);
            // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
            // 1/10 chance he'll pick you
            MovingTargsTime += ((BerserkTimer / SpecialAttack_1.AttackSpeed) * (5f + 1f)) * 0.10f;
            // Fight Requirements
            Min_Tanks = 1;
            Min_Healers = 2;
            /* TODO:
             * The Acolyte Add
             */
        }
    }
    public class Tenebron_10 : BossHandler {
        public Tenebron_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Tenebron";
            Content = "T7";
            Instance = "The Obsidian Sanctum";
            Version = "10 Man";
            Health = 976150f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6188f + 8812f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            SpecialAttack_2 = new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6938f + 8062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 30 seconds for 20 seconds dps has to jump onto the 6 adds that spawn
            MovingTargsTime  = (BerserkTimer / (30f+20f)) * (20f);
            MultiTargsPerc = (BerserkTimer / (30f + 20f)) * (20f) / BerserkTimer;
            MaxNumTargets = 6f + 1f;
            // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
            // 1/10 chance he'll pick you
            MovingTargsTime += ((BerserkTimer / SpecialAttack_1.AttackSpeed) * (5f + 1f)) * 0.10f;
            // Fight Requirements
            // Fight Requirements
            Min_Healers = 2;
            /* TODO:
             * The Adds' abilities
             */
        }
    }
    public class Vesperon_10 : BossHandler {
        public Vesperon_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Vesperon";
            Content = "T7";
            Instance = "The Obsidian Sanctum";
            Version = "10 Man";
            Health = 976150f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6188f + 8812f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            SpecialAttack_2 = new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6938f + 8062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
            // 1/10 chance he'll pick you
            MovingTargsTime = ((BerserkTimer / SpecialAttack_1.AttackSpeed) * (5f + 1f)) * 0.10f;
            // Fight Requirements
            Min_Tanks = 1;
            Min_Healers = 2;
            /* TODO:
             * The adds, which optimally you would ignore
             */
        }
    }
    public class Sartharion_10 : BossHandler {
        public Sartharion_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Sartharion";
            Content = "T7";
            Instance = "The Obsidian Sanctum";
            Version = "10 Man";
            Health = 2510100f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Fire Breath",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (8750f + 11250f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 45 seconds for 10 seconds you gotta move for Lava Waves
            MovingTargsTime = (BerserkTimer / 45f) * (10f);
            // Fight Requirements
            Min_Tanks = 1;
            /* TODO:
             */
        }
    }
    // ===== The Vault of Archavon ====================
    public class ArchavonTheStoneWatcher_10 : BossHandler {
        public ArchavonTheStoneWatcher_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Archavon The Stone Watcher";
            Content = "T7";
            Instance = "The Vault of Archavon";
            Version = "10 Man";
            Health = 2300925f;
            BerserkTimer = 5 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Impale",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4813f + 6187f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.75f;
            // Every 30 seconds for 5 seconds you gotta catch up to him as he jumps around
            MovingTargsTime = (BerserkTimer / (30f)) * (5f);
            // Fight Requirements
            /* TODO:
             * Rock Shards
             * Crushing Leap
             * Stomp (this also stuns)
             * Impale (this also stuns)
             */
        }
    }
    // ===== The Eye of Eternity ======================
    public class Malygos_10 : BossHandler {
        public Malygos_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Malygos";
            Content = "T7";
            Instance = "The Eye of Eternity";
            Version = "10 Man";
            Health = 2230000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            };
            SpecialAttack_1 = new Attack {
                Name = "Impale",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4813f + 6187f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 70-120 seconds for 16 seconds you can't be on the target
            // Adding 4 seconds to the Duration for moving out before starts and then back in after
            MovingTargsTime = (BerserkTimer / (70f + 120f / 2f)) * (16f+4f);
            /* TODO:
             */
        }
    }
    #endregion
    #region T7.5 Content
    // ===== Naxxramas ================================
    // Spider Wing
    public class AnubRekhan_25 : AnubRekhan_10 {
        public AnubRekhan_25() {
            // If not listed here use values from 10 man version
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 6763325f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = MeleeAttack.Name,
                DamageType = MeleeAttack.DamageType,
                DamagePerHit = 120000f,
                MaxNumTargets = MeleeAttack.MaxNumTargets,
                AttackSpeed = MeleeAttack.AttackSpeed,
            };
            SpecialAttack_1 = new Attack {
                Name = SpecialAttack_1.Name,
                DamageType = SpecialAttack_1.DamageType,
                DamagePerHit = (5688f + 7312f) / 2f,
                MaxNumTargets = SpecialAttack_1.MaxNumTargets,
                AttackSpeed = SpecialAttack_1.AttackSpeed,
            };
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            // ==== Situational Changes ====
            // When he Impales, he turns around and faces the raid
            // simming this by using the activates over fight and having him facing raid for 2 seconds
            float time = (BerserkTimer / SpecialAttack_1.AttackSpeed) * 2f;
            InBackPerc_Melee = 1f - time / BerserkTimer;
            // Locust Swarm: Every 80-120 seconds for 16 seconds you can't be on the target
            // Adding 4 seconds to the Duration for moving out before starts and then back in after
            MovingTargsTime = (BerserkTimer / (80f + 120f / 2f)) * (16f + 4f);
            // Every time he Locust Swarms he summons 2 Crypt Guards
            // Let's assume it's up for 10 seconds
            time = (BerserkTimer / 60f) * 10f;
            // Every time he spawns a Crypt Guard and it dies, x seconds
            // after he summons 10 scarabs from each's body
            // Assuming they are up for 8 sec
            time += ((BerserkTimer - 20f) / 60f) * 8f;
            MaxNumTargets = 20f;
            MultiTargsPerc = time / BerserkTimer;
            // ==== Fight Requirements ====
        }
    }
    public class GrandWidowFaerlina_25 : GrandWidowFaerlina_10 {
        public GrandWidowFaerlina_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 6763325;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Poison Bold Volley",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (/*Initial*/(33755f + 4125f) / 2.0f) + (/*Dot*/((1900f+2100f)/2.0f)*8f/2f),
                MaxNumTargets = 3,
                AttackSpeed = (7.0f+15.0f)/2.0f,
            };
            SpecialAttack_2 = new Attack {
                Name = "Rain of Fire",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (/*Dot*/((3700f+4300f)/2.0f)*6f/2f),
                MaxNumTargets = 10,
                AttackSpeed = (6.0f+18.0f)/2.0f,
            };
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            // Situational Changes
            // Fight Requirements
            /* TODO:
             * Frenzy
             * Worshippers
             */
        }
    }
    public class Maexxna_25 : Maexxna_10 {
        public Maexxna_25() {
            // If not listed here use values from 10 man version
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 7600000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = MeleeAttack.Name,
                DamageType = MeleeAttack.DamageType,
                DamagePerHit = 120000f,
                MaxNumTargets = MeleeAttack.MaxNumTargets,
                AttackSpeed = MeleeAttack.AttackSpeed,
            };
            SpecialAttack_1 = new Attack {
                Name = SpecialAttack_1.Name,
                DamageType = SpecialAttack_1.DamageType,
                DamagePerHit = (2188f + 2812f) / 2f,
                MaxNumTargets = 25,
                AttackSpeed = SpecialAttack_1.AttackSpeed,
            };
            // Situational Changes
            // 8 Adds every 40 seconds for 10 seconds (only 14000 HP each)
            MultiTargsPerc = ((BerserkTimer / 40f) * 10f) / BerserkTimer;
            MaxNumTargets = 8;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 1;
            Min_Healers = 4;
            /* TODO:
             * Web Wrap
             * Poison Shock
             * Necrotic Poison
             * Frenzy
             */
        }
    }
    // Plague Quarter
    public class NoththePlaguebringer_25 : NoththePlaguebringer_10 {
        public NoththePlaguebringer_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 2500000f;
            BerserkTimer = (110 + 70) * 3; // He enrages after 3rd iteration of Phase 2
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Impale",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4813f + 6187f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
            };
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 30 seconds 2 adds will spawn with 100k HP each, simming their life-time to 20 seconds
            MultiTargsPerc = (BerserkTimer / 30f) * (20f) / BerserkTimer;
            // Fight Requirements
            Min_Tanks   = 1;
            Min_Healers = 2;
            /* TODO:
             * Phase 2
             */
        }
    }
    public class HeigantheUnclean_25 : HeigantheUnclean_10 {
        public HeigantheUnclean_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 3060000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Decrepit Fever",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = 3000f / 3f * 21f,
                MaxNumTargets = 1,
                AttackSpeed = 30.0f,
            };
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            // Situational Changes
            InBackPerc_Melee = 0.25f;
            // We are assuming you are using the corner trick so you don't have
            // to dance as much in 10 man
            // Every 90 seconds for 45 seconds you must do the safety dance
            MovingTargsTime = (BerserkTimer / 90f) * 45f;
            // Fight Requirements
            Min_Tanks = 1;
            /* TODO:
             */
        }
    }
    public class Loatheb_25 : Loatheb_10 {
        public Loatheb_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 6693600f;
            BerserkTimer = 5 * 60; // Inevitable Doom starts to get spammed every 15 seconds
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Deathbloom",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (/*DoT*/200f / 1f * 6f) + (/*Bloom*/1200f),
                MaxNumTargets = 10,
                AttackSpeed = 30.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Inevitable Doom",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = 4000 / 30 * 120,
                MaxNumTargets = 10,
                AttackSpeed = 120.0f,
            };
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Initial 10 seconds to pop first Spore then every 3rd spore
            // after that (90 seconds respawn then 10 sec moving to/back)
            MovingTargsTime = 10 + (BerserkTimer / 90) * 10;
            // Fight Requirements
            Min_Tanks = 1;
            /* TODO:
             * Necrotic Aura
             * Fungal Creep
             */
        }
    }
    // Military Quarter
    public class InstructorRazuvious_25 : InstructorRazuvious_10 {
        public InstructorRazuvious_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 3349000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Disrupting Shout",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4275f + 4725f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 15.0f,
            };
            SpecialAttack_2 = new Attack {
                Name = "Jagged Knife",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 5000 + (10000 / 5 * 5),
                MaxNumTargets = 1,
                AttackSpeed = 10.0f,
            };
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Fight Requirements
            // Every 70-120 seconds for 16 seconds you can't be on the target
            // Adding 4 seconds to the Duration for moving out before starts and then back in after
            /* TODO:
             * Unbalancing Strike
             * Using the Understudies
             */
        }
    }
    public class GothiktheHarvester_25 : GothiktheHarvester_10 {
        public GothiktheHarvester_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 839000f;
            //BerserkTimer = (8 * 60) - (4 * 60 + 34);
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Shadowbolt",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2880f + 3520f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 1.0f,
            };
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Fight Requirements
            /* TODO:
             * Phase 1
             * Harvest Soul
             */
        }
    }
    public class FourHorsemen_25 : FourHorsemen_10 {
        public FourHorsemen_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 781000f * 4f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Korth'azz's Meteor",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (13775f + 15225f) / 2f,
                MaxNumTargets = 8,
                AttackSpeed = 15.0f,
            };
            SpecialAttack_2 = new Attack {
                Name = "Rivendare's Unholy Shadow",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2160f + 2640f) / 2f + (4800/2*4),
                MaxNumTargets = 8,
                AttackSpeed = 15.0f,
            };
            SpecialAttack_3 = new Attack {
                Name = "Blaumeux's Shadow Bolt",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2357f + 2643f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_4 = new Attack {
                Name = "Zeliek's Holy Bolt",
                DamageType = ItemDamageType.Holy,
                DamagePerHit = (2357f + 2643f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 2.0f,
            };
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            // Situational Changes
            InBackPerc_Melee = 0.75f;
            // Swap 1st 2 mobs once: 15
            // Get to the back once: 10
            // Bounce back and forth in the back: Every 30 sec for 10 sec but for only 40% of the fight
            MovingTargsTime = 15f + 10f + ((BerserkTimer * 0.40f) / 30f) * 10f;
            // Fight Requirements
            Min_Tanks = 3; // simming 3rd to show that 2 dps have to OT the back
            Min_Healers = 3;
            /* TODO:
             * Blaumeux's Void Zone
             */
        }
    }
    // Construct Quarter
    public class Patchwerk_25 : Patchwerk_10 {
        public Patchwerk_25() {
            // If not listed here use values from 10 man version
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 13000000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = MeleeAttack.Name,
                DamageType = MeleeAttack.DamageType,
                DamagePerHit = 120000f,
                MaxNumTargets = MeleeAttack.MaxNumTargets,
                AttackSpeed = MeleeAttack.AttackSpeed,
            };
            SpecialAttack_1 = new Attack {
                Name = SpecialAttack_1.Name,
                DamageType = SpecialAttack_1.DamageType,
                DamagePerHit = (79000f + 81000f) / 2f,
                MaxNumTargets = SpecialAttack_1.MaxNumTargets,
                AttackSpeed = SpecialAttack_1.AttackSpeed,
            };
            // Situational Changes
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 3;
            Min_Healers = 4;
            /* TODO:
             * Frenzy
             */
        }
    }
    public class Grobbulus_25 : Grobbulus_10 {
        public Grobbulus_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 2928000f;
            BerserkTimer = 12 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 8 seconds for 3 seconds Grob has to be kited to
            // avoid Poison Cloud Farts. This goes on the entire fight
            MovingTargsTime  = (BerserkTimer / 8f) * 3f;
            // Every 20 seconds 1/10 chance to get hit with Mutating Injection
            // You have to run off for 10 seconds then run back for 4-5
            MovingTargsTime += ((BerserkTimer / 20f) * (10f+(4f+5f)/2f)) * 0.10f;
            /* TODO:
             * Slime Spray
             * Occasional Poins Cloud Ticks that are unavoidable
             */
        }
    }
    public class Gluth_25 : Gluth_10 {
        public Gluth_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 3230000f;
            BerserkTimer = 8 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 40000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             * Decimate
             * Enrage
             * Mortal Wound
             * Zombie Chows
             */
        }
    }
    public class Thaddius_25 : Thaddius_10 {
        public Thaddius_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 3850000f + 838300f; // one player only deals with one of the add's total health + thadd's health
            BerserkTimer = 6 * 60; // Need to verify if starts at beg. of combat or beg. of Thadd
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpellAttack = new Attack {
                Name = "Chain Lightning",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (3600f+4400f)/2f,
                MaxNumTargets = 3f,
                AttackSpeed = 15.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.50f;
            // Every 30 seconds, polarity shift, 3 sec move
            // 50% chance that your polarity will change
            MovingTargsTime = ((BerserkTimer / 30f) * 3f) * 0.50f;
            /* TODO:
             * Better handle of Feugen and Stalagg
             */
        }
    }
    // Frostwyrm Lair
    public class Sapphiron_25 : Sapphiron_10 {
        public Sapphiron_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 4250000f;
            BerserkTimer = 15 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Frost Aura",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = 1200f,
                MaxNumTargets = 10,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_2 = new Attack {
                Name = "Life Drain",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (((4376f+5624f)/2f) * 3f) * 4f,
                MaxNumTargets = 2,
                AttackSpeed = 24.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 45(+30) seconds for 30 seconds Sapph is in the air
            // He stops this at 10% hp
            MovingTargsTime = ((BerserkTimer / (45f+30f)) * 30f) * 0.90f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 3;
            Min_Healers = 4;
            /* TODO:
             * Chill (The Blizzard)
             * Ice Bolt
             */
        }
    }
    public class KelThuzad_25 : KelThuzad_10 {
        public KelThuzad_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 2230000f;
            BerserkTimer = 19 * 60;
            SpeedKillTimer = 6 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Impale",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4813f + 6187f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Phase 1, no damage to KT
            MovingTargsTime = 3f*60f + 48f;
            TimeBossIsInvuln = 3f * 60f + 48f;
            // Phase 2 & 3, gotta move out of Shadow Fissures periodically
            // We're assuming they pop every 30 seconds and you have to be
            // moved for 6 seconds and there's a 1/10 chance he will select
            // you over someone e;se
            MovingTargsTime = (((BerserkTimer - MovingTargsTime) / 30f) * 6f) * 0.10f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 3;
            Min_Healers = 4;
            /* TODO:
             * The Mobs in Phase 1
             */
        }
    }
    // ===== The Obsidian Sanctum =====================
    public class Shadron_25 : Shadron_10 {
        public Shadron_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 976150f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6188f + 8812f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            };
            SpecialAttack_2 = new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6938f + 8062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 60 seconds for 20 seconds dps has to jump into the portal and kill the add
            MovingTargsTime  = (BerserkTimer / (60f+20f)) * (20f);
            // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
            // 1/10 chance he'll pick you
            MovingTargsTime += ((BerserkTimer / SpecialAttack_1.AttackSpeed) * (5f + 1f)) * 0.10f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             * The Acolyte Add
             */
        }
    }
    public class Tenebron_25 : Tenebron_10 {
        public Tenebron_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 976150f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6188f + 8812f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            };
            SpecialAttack_2 = new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6938f + 8062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 30 seconds for 20 seconds dps has to jump onto the 6 adds that spawn
            MovingTargsTime  = (BerserkTimer / (30f+20f)) * (20f);
            MultiTargsPerc = (BerserkTimer / (30f + 20f)) * (20f) / BerserkTimer;
            MaxNumTargets = 6f + 1f;
            // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
            // 1/10 chance he'll pick you
            MovingTargsTime += ((BerserkTimer / SpecialAttack_1.AttackSpeed) * (5f + 1f)) * 0.10f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             * The Adds' abilities
             */
        }
    }
    public class Vesperon_25 : Vesperon_10 {
        public Vesperon_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 976150f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6188f + 8812f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            };
            SpecialAttack_2 = new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6938f + 8062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
            // 1/10 chance he'll pick you
            MovingTargsTime = ((BerserkTimer / SpecialAttack_1.AttackSpeed) * (5f + 1f)) * 0.10f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             * The adds, which optimally you would ignore
             */
        }
    }
    public class Sartharion_25 : Sartharion_10 {
        public Sartharion_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 2510100f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Fire Breath",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (8750f + 11250f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 45 seconds for 10 seconds you gotta move for Lava Waves
            MovingTargsTime = (BerserkTimer / 45f) * (10f);
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             */
        }
    }
    // ===== The Vault of Archavon ====================
    public class ArchavonTheStoneWatcher_25 : ArchavonTheStoneWatcher_10 {
        public ArchavonTheStoneWatcher_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 2300925f;
            BerserkTimer = 5 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Impale",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4813f + 6187f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.75f;
            // Every 30 seconds for 5 seconds you gotta catch up to him as he jumps around
            MovingTargsTime = (BerserkTimer / (30f)) * (5f);
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             * Rock Shards
             * Crushing Leap
             * Stomp (this also stuns)
             * Impale (this also stuns)
             */
        }
    }
    // ===== The Eye of Eternity ======================
    public class Malygos_25 : Malygos_10 {
        public Malygos_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 2230000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            SpecialAttack_1 = new Attack {
                Name = "Impale",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4813f + 6187f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 70-120 seconds for 16 seconds you can't be on the target
            // Adding 4 seconds to the Duration for moving out before starts and then back in after
            MovingTargsTime = (BerserkTimer / (70f + 120f / 2f)) * (16f+4f);
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             */
        }
    }
    #endregion
    #region T8 Content
    // ===== The Vault of Archavon ====================
    public class EmalonTheStormWatcher_10 : BossHandler {
        public EmalonTheStormWatcher_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Emalon the Storm Watcher";
            Content = "T8";
            Instance = "The Vault of Archavon";
            Version = "10 Man";
            Health = 2789000f;
            BerserkTimer = 6 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 90000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Every 45 seconds for 18 seconds dps has to be on the overcharged add (it wipes the raid at 20 sec)
            // Adding 5 seconds to the Duration for moving out before starts and then 5 for back in after
            MovingTargsTime  = (BerserkTimer / (45f + 18f)) * (18f + 5f + 5f);
            // Lightning Nova, usually happens a few seconds after the overcharged add dies
            // (right when most melee reaches the boss again) Simming 4 to run out and 4 to get back
            MovingTargsTime += (BerserkTimer / (45f + 18f)) * (4f + 4f);
            // Fight Requirements
            /* TODO:
             * Adds Damage
             * Chain Lightning Damage
             * Lightning Nova Damage
             */
        }
    }
    // ===== Ulduar ===================================
    // The Siege
        // TODO: Flame Leviathan
        // TODO: Ignis the Furnace Master
        // TODO: Razorscale
        // TODO: XT-002 Deconstructor
    // The Antechamber
        // TODO: Assembly of Iron
        // TODO: Kologarn
    public class Auriaya_10 : BossHandler {
        public Auriaya_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Auriaya";
            Content = "T8";
            Instance = "Ulduar";
            Version = "10 Man";
            BerserkTimer = 10 * 60;
            Health = 3137625f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.00f; // This is a boss where you CANNOT be behind her or she Fubar's the raid
            // She summons extra targets a lot, most of the time, they are within melee range of persons on the boss
            // Guardian Swarm: Marks a player and summons a pack of 10 Swarming Guardians with low health around them soon after.
            // Feral Defender: If you leave him alone, he's up about 90% of the fight
            MultiTargsPerc = 0.90f; // need to sim this out
            MaxNumTargets  = 10f; // need to drop this down to only when the swarm is up
            // Terrifying Screech: Raid-wide fear for 5 seconds. Magic effect.
            // Going to assume the CD is 45 sec for now (cuz I know she doesnt do it every 8 sec)
            FearingTargsFreq = 45f;
            FearingTargsDur = 5f * 1000f;
            // Fight Requirements
            /* TODO:
             */
        }
    }
    // The Keepers
        // TODO: Mimiron
        // TODO: Freya
        // TODO: Thorim
    public class Hodir_10 : BossHandler {
        public Hodir_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Hodir";
            Content = "T8";
            Instance = "Ulduar";
            Version = "10 Man";
            BerserkTimer = 8 * 60;
            Health = 8115990f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.75f; // he moves A LOT so it's hard to stay behind him at all times
            // Freeze: Inflicts 5,550 to 6,450 Frost damage to players within 10 yards. Also roots
            // the targets in place for 10 seconds. The rooting component of the spell is a magic debuff.
            // Going to assume the CD is 45 sec for now
            RootingTargsFreq = 45f;
            RootingTargsDur = 10f * 1000f;
            // Fight Requirements
            Min_Tanks = 1;
            /* TODO:
             */
        }
    }
    // The Descent into Madness
        // TODO: General Vezax
        // TODO: Yogg-Saron
    // Supermassive
    // TODO: Algalon the Observer
    #endregion
    #region T8.5 Content
    // ===== The Vault of Archavon ====================
    public class EmalonTheStormWatcher_25 : EmalonTheStormWatcher_10 {
        public EmalonTheStormWatcher_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T8.5";
            Version = "25 Man";
            Health = 2789000f;
            BerserkTimer = 6 * 60;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 90000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Every 45 seconds for 18 seconds dps has to be on the overcharged add (it wipes the raid at 20 sec)
            // Adding 5 seconds to the Duration for moving out before starts and then 5 for back in after
            MovingTargsTime  = (BerserkTimer / (45f + 18f)) * (18f + 5f + 5f);
            // Lightning Nova, usually happens a few seconds after the overcharged add dies
            // (right when most melee reaches the boss again) Simming 4 to run out and 4 to get back
            MovingTargsTime += (BerserkTimer / (45f + 18f)) * (4f + 4f);
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             * Adds Damage
             * Chain Lightning Damage
             * Lightning Nova Damage
             */
        }
    }
    // ===== Ulduar ===================================
    // The Siege
        // TODO: Flame Leviathan
        // TODO: Ignis the Furnace Master
        // TODO: Razorscale
        // TODO: XT-002 Deconstructor
    // The Antechamber
        // TODO: Assembly of Iron
        // TODO: Kologarn
    public class Auriaya_25 : Auriaya_10 {
        public Auriaya_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T8.5";
            Version = "25 Man";
            BerserkTimer = 10 * 60;
            Health = 16734000f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            // Situational Changes
            //InBackPerc_Melee = 0.00f; // This is a boss where you CANNOT be behind her or she Fubar's the raid
            // She summons extra targets a lot, most of the time, they are within melee range of persons on the boss
            // Guardian Swarm: Marks a player and summons a pack of 10 Swarming Guardians with low health around them soon after.
            // Feral Defender: If you leave him alone, he's up about 90% of the fight
            //MultiTargsPerc = 0.90f; // need to sim this out
            //MaxNumTargets  = 10f; // need to drop this down to only when the swarm is up
            // Terrifying Screech: Raid-wide fear for 5 seconds. Magic effect.
            // Going to assume the CD is 45 sec for now (cuz I know she doesnt do it every 8 sec)
            //FearingTargsFreq = 45f;
            //FearingTargsDur = 5f * 1000f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             */
        }
    }
    // The Keepers
        // TODO: Mimiron
        // TODO: Freya
        // TODO: Thorim
    public class Hodir_25 : Hodir_10 {
        public Hodir_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T8.5";
            Version = "25 Man";
            Health = 32477904f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            // Situational Changes
            //InBackPerc_Melee = 0.75f; // he moves A LOT so it's hard to stay behind him at all times
            // Freeze: Inflicts 5,550 to 6,450 Frost damage to players within 10 yards. Also roots
            // the targets in place for 10 seconds. The rooting component of the spell is a magic debuff.
            // Going to assume the CD is 45 sec for now
            //RootingTargsFreq = 45f;
            //RootingTargsDur = 10f * 1000f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 1;
            Min_Healers = 4;
            /* TODO:
             */
        }
    }
    // The Descent into Madness
        // TODO: General Vezax
        // TODO: Yogg-Saron
    // Supermassive
    // TODO: Algalon the Observer
    #endregion
    #region T9 (10) Content
    // ===== The Vault of Archavon ====================
    public class KoralonTheFlameWatcher_10 : BossHandler {
        public KoralonTheFlameWatcher_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Koralon the Flame Watcher";
            Content = "T9";
            Instance = "The Vault of Archavon";
            Version = "10 Man";
            Health = 4183500f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Fight Requirements
            Min_Tanks = 2;
            Min_Healers = 3;
            /* TODO:
             * I haven't done this fight yet so I can't really model it myself right now
             */
        }
    }
    #endregion
    #region T9 (10) H Content
    #endregion
    #region T9 (25) Content
    // ===== The Vault of Archavon ====================
    public class KoralonTheFlameWatcher_25 : KoralonTheFlameWatcher_10 {
        public KoralonTheFlameWatcher_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T9.5";
            Version = "25 Man";
            Health = 4183500f;
            // Resistance
            // Attacks
            MeleeAttack = new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             * I haven't done this fight yet so I can't really model it myself right now
             */
        }
    }
    #endregion
    #region T9 (25) H Content
    #endregion
}
