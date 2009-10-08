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
    public enum POSSIBLE_LEVELS { LVLP0 = 80, LVLP1, LVLP2, LVLP3, }
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
    /// <summary>Stores a Stun that the Boss Performs</summary>
    public struct Stun {
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
    }
    /// <summary>Stores a Fear that the Boss Performs</summary>
    public struct Fear {
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
    }
    /// <summary>Stores a Root that the Boss Performs</summary>
    public struct Root {
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
    }
    /// <summary>Stores a Move that the Boss Performs</summary>
    public struct Move {
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
    }
    /// <summary>Stores a Disarm that the Boss Performs</summary>
    public struct Disarm {
        public float Frequency;
        public float Duration;
        public float Chance;
        public bool Breakable;
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
            #region Attacks
            /*if (passedList[0].Attacks.Count > 0) {
                foreach (Attack a in passedList[0].Attacks) {
                    float perhit = passedList[0].Attacks[0].DamagePerHit; foreach (BossHandler boss in passedList) { perhit = Math.Min(perhit, boss.Attacks[0].Name == "Invalid" ? perhit : boss.Attacks[0].DamagePerHit); }
                    float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg = Math.Min(numtrg, boss.Attacks[0].Name == "Invalid" ? numtrg : boss.Attacks[0].MaxNumTargets); }
                    float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd = Math.Max(atkspd, boss.Attacks[0].Name == "Invalid" ? atkspd : boss.Attacks[0].AttackSpeed); }
                    retboss.Attacks.Add(new Attack {
                        Name = "Melee Attack",
                        DamageType = ItemDamageType.Physical,
                        DamagePerHit = perhit,
                        MaxNumTargets = numtrg,
                        AttackSpeed = atkspd,
                    });
                }
            }*/
            #endregion
            // Situational Changes
            // In Back
            value = 0f;     foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.InBackPerc_Melee  ); } retboss.InBackPerc_Melee   = value;
            value = 0f;     foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.InBackPerc_Ranged ); } retboss.InBackPerc_Ranged  = value;
            // Multi-targs
            value = 0;      foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.MultiTargsPerc    ); } retboss.MultiTargsPerc     = value;
            value = 3;      foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.MaxNumTargets     ); } retboss.MaxNumTargets      = value;
            // Stun
            value = 0;      foreach (BossHandler boss in passedList) { if (boss.StunningTargsFreq  <= 0) { value = 0f; break; } else { value = Math.Max(value, boss.StunningTargsFreq  ); } } retboss.StunningTargsFreq  = (value >= retboss.BerserkTimer || value <= 0 ? 0000 : value);
            value = 10*1000;foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.StunningTargsDur  ); } retboss.StunningTargsDur   = (value >= retboss.BerserkTimer || value <= 0 ? 5000 : value);
            // Move
            value = 0;      foreach (BossHandler boss in passedList) { if (boss.MovingTargsFreq    <= 0) { value = 0f; break; } else { value = Math.Max(value, boss.MovingTargsFreq    ); } } retboss.MovingTargsFreq    = (value >= retboss.BerserkTimer || value <= 0 ? 0000 : value);
            value = 5000;   foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.MovingTargsDur    ); } retboss.MovingTargsDur     = (value >= retboss.BerserkTimer || value <= 0 ? 5000 : value);
            // Fear
            value = 0;      foreach (BossHandler boss in passedList) { if (boss.FearingTargsFreq   <= 0) { value = 0f; break; } else { value = Math.Max(value, boss.FearingTargsFreq   ); } } retboss.FearingTargsFreq   = (value >= retboss.BerserkTimer || value <= 0 ? 0000 : value);
            value = 5000;   foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.FearingTargsDur   ); } retboss.FearingTargsDur    = (value >= retboss.BerserkTimer || value <= 0 ? 5000 : value);
            // Root
            value = 0;      foreach (BossHandler boss in passedList) { if (boss.RootingTargsFreq   <= 0) { value = 0f; break; } else { value = Math.Max(value, boss.RootingTargsFreq   ); } } retboss.RootingTargsFreq   = (value >= retboss.BerserkTimer || value <= 0 ? 0000 : value);
            value = 5000;   foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.RootingTargsDur   ); } retboss.RootingTargsDur    = (value >= retboss.BerserkTimer || value <= 0 ? 5000 : value);
            // Disarm
            value = 0;      foreach (BossHandler boss in passedList) { if (boss.DisarmingTargsFreq <= 0) { value = 0f; break; } else { value = Math.Max(value, boss.DisarmingTargsFreq ); } } retboss.DisarmingTargsFreq = (value >= retboss.BerserkTimer || value <= 0 ? 0000 : value);
            value = 5000;   foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.DisarmingTargsDur ); } retboss.DisarmingTargsDur  = (value >= retboss.BerserkTimer || value <= 0 ? 5000 : value);
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
            /*{
                count = 0;
                float perhit = passedList[0].Attacks[0].DamagePerHit; foreach (BossHandler boss in passedList) { perhit += boss.Attacks[0].Name == "Invalid" ? 0f : boss.Attacks[0].DamagePerHit; if (boss.Attacks[0].Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg += boss.Attacks[0].Name == "Invalid" ? 0f : boss.Attacks[0].MaxNumTargets; if (boss.Attacks[0].Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd += boss.Attacks[0].Name == "Invalid" ? 0f : boss.Attacks[0].AttackSpeed; if (boss.Attacks[0].Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.Attacks.Add(new Attack { Name = "Melee Attack", DamageType = ItemDamageType.Physical, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, });
            }*/
            #endregion
            // Situational Changes
            // In Back
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.InBackPerc_Melee; } value /= passedList.Length; retboss.InBackPerc_Melee = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.InBackPerc_Ranged; } value /= passedList.Length; retboss.InBackPerc_Ranged = value;
            // Multi-targs
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.MultiTargsPerc; } value /= passedList.Length; retboss.MultiTargsPerc = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.MaxNumTargets; } value /= passedList.Length; retboss.MaxNumTargets = (float)Math.Ceiling(value);
            // Stun
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += (boss.StunningTargsFreq > 0 && boss.StunningTargsFreq < boss.BerserkTimer) ? boss.StunningTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; retboss.StunningTargsFreq = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.StunningTargsDur; } value /= passedList.Length; retboss.StunningTargsDur = value;
            // Move
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += (boss.MovingTargsFreq > 0 && boss.MovingTargsFreq < boss.BerserkTimer) ? boss.MovingTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; retboss.MovingTargsFreq = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.MovingTargsDur; } value /= passedList.Length; retboss.MovingTargsDur = value;
            // Fear
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += (boss.FearingTargsFreq > 0 && boss.FearingTargsFreq < boss.BerserkTimer) ? boss.FearingTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; retboss.FearingTargsFreq = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.FearingTargsDur; } value /= passedList.Length; retboss.FearingTargsDur = value;
            // Root
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += (boss.RootingTargsFreq > 0 && boss.RootingTargsFreq < boss.BerserkTimer) ? boss.RootingTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; retboss.RootingTargsFreq = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.RootingTargsDur; } value /= passedList.Length; retboss.RootingTargsDur = value;
            // Disarm
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += (boss.DisarmingTargsFreq > 0 && boss.DisarmingTargsFreq < boss.BerserkTimer) ? boss.DisarmingTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; retboss.DisarmingTargsFreq = value;
            value = 0f; count = 0; foreach (BossHandler boss in passedList) { value += boss.DisarmingTargsDur; } value /= passedList.Length; retboss.DisarmingTargsDur = value;
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
            /*{
                float perhit = passedList[0].Attacks[0].DamagePerHit; foreach (BossHandler boss in passedList) { perhit = Math.Max(perhit, boss.Attacks[0].Name == "Invalid" ? perhit : boss.Attacks[0].DamagePerHit); }
                float numtrg = passedList[0].Attacks[0].MaxNumTargets; foreach (BossHandler boss in passedList) { numtrg = Math.Max(numtrg, boss.Attacks[0].Name == "Invalid" ? numtrg : boss.Attacks[0].MaxNumTargets); }
                float atkspd = passedList[0].Attacks[0].AttackSpeed; foreach (BossHandler boss in passedList) { atkspd = Math.Min(atkspd, boss.Attacks[0].Name == "Invalid" ? atkspd : boss.Attacks[0].AttackSpeed); }
                retboss.Attacks[0] = new Attack { Name = "Melee Attack", DamageType = ItemDamageType.Physical, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }*/
            #endregion
            // Situational Changes
            // In Back
            value = 1.00f;      foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.InBackPerc_Melee   ); } retboss.InBackPerc_Melee  = value;
            value = 1.00f;      foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.InBackPerc_Ranged  ); } retboss.InBackPerc_Ranged = value;
            // Multi-targs
            value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.MultiTargsPerc     ); } retboss.MultiTargsPerc    = value;
            value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.MaxNumTargets      ); } retboss.MaxNumTargets     = (float)Math.Ceiling(value);
            // Stun
            value = 19 * 20;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.StunningTargsFreq  != 0 ? boss.StunningTargsFreq  : value); } retboss.StunningTargsFreq  = Math.Max(20,value);
            value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.StunningTargsDur   ); } retboss.StunningTargsDur  = Math.Min(10*1000,value);
            // Move
            value = 19 * 20;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.MovingTargsFreq    != 0 ? boss.MovingTargsFreq    : value); } retboss.MovingTargsFreq    = Math.Max(20,value);
            value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.MovingTargsDur     ); } retboss.MovingTargsDur    = Math.Min(10*1000,value);
            // Fear
            value = 19 * 20;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.FearingTargsFreq   != 0 ? boss.FearingTargsFreq   : value); } retboss.FearingTargsFreq   = Math.Max(20,value);
            value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.FearingTargsDur    ); } retboss.FearingTargsDur   = Math.Min(10*1000,value);
            // Root
            value = 19 * 20;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.RootingTargsFreq   != 0 ? boss.RootingTargsFreq   : value); } retboss.RootingTargsFreq   = Math.Max(20,value);
            value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.RootingTargsDur    ); } retboss.RootingTargsDur   = Math.Min(10*1000,value);
            // Disarm
            value = 19 * 20;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.DisarmingTargsFreq != 0 ? boss.DisarmingTargsFreq : value); } retboss.DisarmingTargsFreq = Math.Max(20,value);
            value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.DisarmingTargsDur  ); } retboss.DisarmingTargsDur = Math.Min(10*1000,value);
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
            Level = (int)POSSIBLE_LEVELS.LVLP3;
            Health = 1000000f;
            Armor = (float)StatConversion.NPC_ARMOR[Level - NormCharLevel];
            UseParryHaste = false;
            // Resistance
            ItemDamageType[] DamageTypes = new ItemDamageType[] { ItemDamageType.Physical, ItemDamageType.Nature, ItemDamageType.Arcane, ItemDamageType.Frost, ItemDamageType.Fire, ItemDamageType.Shadow, ItemDamageType.Holy, };
            foreach (ItemDamageType t in DamageTypes) { Resistance(t, 0f); }
            // Attacks
            Attacks = new List<Attack>();
            // Situational Changes
            InBackPerc_Melee   = 0.00f; // Default to never in back
            InBackPerc_Ranged  = 0.00f; // Default to never in back
            MultiTargsPerc     = 0.00f; // Default to 0% multiple targets
            MaxNumTargets      =    1f; // Default to max 1 targets (though at 0%, this means nothing)
            StunningTargsFreq  =    0f; // Default to never stunning
            StunningTargsDur   = 5000f; // Default to stun durations of 5 seconds but since it's 0 stuns over dur, this means nothing
            MovingTargsFreq    =    0f; // Default to never moving
            MovingTargsDur     = 5000f; // Default to move durations of 5 seconds but since it's 0 moves over dur, this means nothing
            DisarmingTargsFreq =    0f; // Default to never disarming
            DisarmingTargsDur  = 5000f; // Default to disarm durations of 5 seconds but since it's 0 disarms over dur, this means nothing
            FearingTargsFreq   =    0f; // Default to never fearing
            FearingTargsDur    = 5000f; // Default to fear durations of 5 seconds but since it's 0 fears over dur, this means nothing
            RootingTargsFreq   =    0f; // Default to never rooting
            RootingTargsDur    = 5000f; // Default to root durations of 5 seconds but since it's 0 roots over dur, this means nothing
            TimeBossIsInvuln   =    0f; // Default to never invulnerable (Invuln. like KT in Phase 1)
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
        private List<Attack> ATTACKS;
        // Situational Changes
        public List<Stun> Stuns = new List<Stun>();
        public List<Fear> Fears = new List<Fear>();
        public List<Root> Roots = new List<Root>();
        public List<Move> Moves = new List<Move>();
        public List<Disarm> Disarms = new List<Disarm>();
        private float INBACKPERC_MELEE, INBACKPERC_RANGED,
                      MULTITARGSPERC, MAXNUMTARGS,
                      STUNNINGTARGS_FREQ, STUNNINGTARGS_DUR,
                      MOVINGTARGS_FREQ, MOVINGTARGS_DUR,
                      DISARMINGTARGS_FREQ, DISARMINGTARGS_DUR,
                      FEARINGTARGS_FREQ, FEARINGTARGS_DUR,
                      ROOTINGTARGS_FREQ, ROOTINGTARGS_DUR,
                      TIMEBOSSISINVULN;
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
        public List<Attack> Attacks { get { return ATTACKS; } set { ATTACKS = value; } }
        // ==== Situational Changes ====
        // Standing in back
        public float  InBackPerc_Melee   { get { return INBACKPERC_MELEE;   } set { INBACKPERC_MELEE   = value; } }
        public float  InBackPerc_Ranged  { get { return INBACKPERC_RANGED;  } set { INBACKPERC_RANGED  = value; } }
        // Multiple Targets
        public float  MultiTargsPerc     { get { return MULTITARGSPERC;     } set { MULTITARGSPERC     = value; } }
        public float  MaxNumTargets      { get { return MAXNUMTARGS;        } set { MAXNUMTARGS        = value; } }
        // Stunning Targets
        public float  StunningTargsFreq  {
            get {
                if (Stuns.Count > 0) {
                    // Adds up the total number of stuns and evens them out over the Berserk Timer
                    float numStunsOverDur = 0;
                    foreach (Stun s in Stuns) {
                        numStunsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numStunsOverDur;
                    return freq;
                } else {
                    return STUNNINGTARGS_FREQ;
                }
            }
            set { STUNNINGTARGS_FREQ = value; }
        }
        public float  StunningTargsDur   {
            get {
                if (Stuns.Count > 0) {
                    // Averages out the Stun Durations
                    float TotalStunDur = 0;
                    foreach (Stun s in Stuns) { TotalStunDur += s.Duration; }
                    float dur = TotalStunDur / Stuns.Count;
                    return dur;
                } else {
                    return STUNNINGTARGS_DUR ;
                }
            }
            set { STUNNINGTARGS_DUR  = value; }
        }
        public float  StunningTargsTime {
            get {
                float time = StunningTargsFreq * StunningTargsDur / BerserkTimer;
                return time;
            }
        }
        // Moving Targets
        public float  MovingTargsFreq   {
            get {
                if (Moves.Count > 0) {
                    // Adds up the total number of Moves and evens them out over the Berserk Timer
                    float numMovesOverDur = 0;
                    foreach (Move s in Moves) {
                        numMovesOverDur += (BerserkTimer / s.Frequency) * s.Chance;
                    }
                    float freq = BerserkTimer / numMovesOverDur;
                    return freq;
                } else {
                    return MOVINGTARGS_FREQ;
                }
            }
            set { MOVINGTARGS_FREQ = value; }
        }
        public float  MovingTargsDur    {
            get {
                if (Moves.Count > 0) {
                    // Averages out the Move Durations
                    float TotalMoveDur = 0;
                    foreach (Move s in Moves) { TotalMoveDur += s.Duration; }
                    float dur = TotalMoveDur / Moves.Count;
                    return dur;
                } else {
                    return MOVINGTARGS_DUR;
                }
            }
            set { MOVINGTARGS_DUR = value; }
        }
        public float  MovingTargsTime {
            get {
                float time = MovingTargsFreq * MovingTargsDur / BerserkTimer;
                return time;
            }
        }
        // Disarming Targets
        public float  DisarmingTargsFreq   {
            get {
                if (Disarms.Count > 0) {
                    // Adds up the total number of Disarmes and evens them out over the Berserk Timer
                    float numDisarmsOverDur = 0;
                    foreach (Disarm s in Disarms) {
                        numDisarmsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numDisarmsOverDur;
                    return freq;
                } else {
                    return DISARMINGTARGS_FREQ;
                }
            }
            set { DISARMINGTARGS_FREQ = value; }
        }
        public float  DisarmingTargsDur    {
            get {
                if (Disarms.Count > 0) {
                    // Averages out the Disarme Durations
                    float TotalDisarmeDur = 0;
                    foreach (Disarm s in Disarms) { TotalDisarmeDur += s.Duration; }
                    float dur = TotalDisarmeDur / Disarms.Count;
                    return dur;
                } else {
                    return DISARMINGTARGS_DUR;
                }
            }
            set { DISARMINGTARGS_DUR = value; }
        }
        public float  DisarmingTargsTime {
            get {
                float time = DisarmingTargsFreq * DisarmingTargsDur / BerserkTimer;
                return time;
            }
        }
        // Fearing Targets
        public float  FearingTargsFreq  {
            get {
                if (Fears.Count > 0) {
                    // Adds up the total number of stuns and evens them out over the Berserk Timer
                    float numFearsOverDur = 0;
                    foreach (Fear s in Fears) {
                        numFearsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numFearsOverDur;
                    return freq;
                } else {
                    return FEARINGTARGS_FREQ;
                }
            }
            set { FEARINGTARGS_FREQ = value; }
        }
        public float  FearingTargsDur   {
            get {
                if (Fears.Count > 0) {
                    // Averages out the Fear Durations
                    float TotalFearDur = 0;
                    foreach (Fear s in Fears) { TotalFearDur += s.Duration; }
                    float dur = TotalFearDur / Fears.Count;
                    return dur;
                } else {
                    return FEARINGTARGS_DUR;
                }
            }
            set { FEARINGTARGS_DUR = value; }
        }
        public float  FearingTargsTime {
            get {
                float time = FearingTargsFreq * FearingTargsDur / BerserkTimer;
                return time;
            }
        }
        // Rooting Targets
        public float  RootingTargsFreq  {
            get {
                if (Roots.Count > 0) {
                    // Adds up the total number of Roots and evens them out over the Berserk Timer
                    float numRootsOverDur = 0;
                    foreach (Root s in Roots) {
                        numRootsOverDur += BerserkTimer / s.Frequency;
                    }
                    float freq = BerserkTimer / numRootsOverDur;
                    return freq;
                } else {
                    return ROOTINGTARGS_FREQ;
                }
            }
            set { ROOTINGTARGS_FREQ = value; }
        }
        public float  RootingTargsDur   {
            get {
                if (Roots.Count > 0) {
                    // Averages out the Root Durations
                    float TotalRootDur = 0;
                    foreach (Root s in Roots) { TotalRootDur += s.Duration; }
                    float dur = TotalRootDur / Roots.Count;
                    return dur;
                } else {
                    return ROOTINGTARGS_DUR;
                }
            }
            set { ROOTINGTARGS_DUR = value; }
        }
        public float  RootingTargsTime {
            get {
                float time = RootingTargsFreq * RootingTargsDur / BerserkTimer;
                return time;
            }
        }
        // Other
        public float  TimeBossIsInvuln   { get { return TIMEBOSSISINVULN;   } set { TIMEBOSSISINVULN   = value; } }
        // ==== Fight Requirements ====
        public int    Max_Players        { get { return MAX_PLAYERS;        } set { MAX_PLAYERS        = value; } }
        public int    Min_Healers        { get { return MIN_HEALERS;        } set { MIN_HEALERS        = value; } }
        public int    Min_Tanks          { get { return MIN_TANKS;          } set { MIN_TANKS          = value; } }
        // ==== Methods for Pulling DPS ===============
        /// <summary>Public function for the DPS Gets so we can re-use code. Includes a full player defend table.</summary>
        /// <param name="type">The type of attack to check: AT_MELEE, AT_RANGED, AT_AOE</param>
        /// <param name="BossDamageBonus">Perc value (0.10f = 110% Base damage)</param>
        /// <param name="BossDamagePenalty">Perc value (0.10f = 90% Base damage)</param>
        /// <param name="p_missPerc">Perc value (0.08f = 8% Chance for Boss to Miss Player)</param>
        /// <param name="p_dodgePerc">Perc value (0.201f = 20.10% Chance for Player to Dodge Boss Attack)</param>
        /// <param name="p_parryPerc">Perc value (0.1375f = 13.75% Chance for Player to Parry Boss Attack)</param>
        /// <param name="p_blockPerc">Perc value (0.065f = 6.5% Chance for Player to Block Boss Attack)</param>
        /// <param name="p_blockVal">How much Damage is absorbed by player's Shield in Block Value</param>
        /// <returns>The DPS value requested, returns zero if no Attacks have been created for the Boss or there are no Attacks of that Type.</returns>
        public float GetDPSByType(ATTACK_TYPES type, float BossDamageBonus, float BossDamagePenalty,
                                  float p_missPerc, float p_dodgePerc, float p_parryPerc, float p_blockPerc, float p_blockVal)
        {
            if (Attacks.Count <= 0) { return 0f; } // make sure there were some TO put in there
            List<Attack> attacks = new List<Attack>();
            foreach (Attack a in Attacks) { if (a.AttackType == type) { attacks.Add(a); } }
            if (attacks.Count <= 0) { return 0f; } // make sure there were some put in there

            float retDPS = 0f;

            foreach (Attack a in attacks) {
                float damage = a.DamagePerHit * (1f + BossDamageBonus) * (1f - BossDamagePenalty),
                      damageOnUse = damage * (1f - p_missPerc - p_dodgePerc - p_parryPerc - p_blockPerc), // takes out the player's defend table
                      swing = a.AttackSpeed;
                      damageOnUse += (damage - p_blockVal) * p_blockPerc; // Adds reduced damage from blocks back in
                float acts = BerserkTimer / swing,
                      avgDmg = damageOnUse * acts,
                      dps = avgDmg / BerserkTimer;
                retDPS += dps;
            }

            return retDPS;
        }
        /// <summary>Public function for the DPS Gets so we can re-use code.</summary>
        /// <param name="type">The type of attack to check: AT_MELEE, AT_RANGED, AT_AOE</param>
        /// <param name="BossDamageBonus">Perc value (0.10f = 110% Base damage)</param>
        /// <param name="BossDamagePenalty">Perc value (0.10f = 90% Base damage)</param>
        /// <param name="p_missPerc">Perc value (0.08f = 8% Chance for Boss to Miss Player)</param>
        /// <returns>The DPS value requested, returns zero if no Attacks have been created for the Boss or there are no Attacks of that Type.</returns>
        public float GetDPSByType(ATTACK_TYPES type, float BossDamageBonus, float BossDamagePenalty, float p_missPerc) {
            return GetDPSByType(type, BossDamageBonus, BossDamagePenalty, p_missPerc, 0, 0, 0, 0);
        }
        /// <summary>Public function for the DPS Gets so we can re-use code.</summary>
        /// <param name="type">The type of attack to check: AT_MELEE, AT_RANGED, AT_AOE</param>
        /// <param name="BossDamageBonus">Perc value (0.10f = 110% Base damage)</param>
        /// <param name="BossDamagePenalty">Perc value (0.10f = 90% Base damage)</param>
        /// <returns>The DPS value requested, returns zero if no Attacks have been created for the Boss or there are no Attacks of that Type.</returns>
        public float GetDPSByType(ATTACK_TYPES type, float BossDamageBonus, float BossDamagePenalty) {
            return GetDPSByType(type, BossDamageBonus, BossDamagePenalty, 0, 0, 0, 0, 0);
        }
        /// <summary>
        /// Gets Raw DPS of all attacks that are Melee type. DPS and Healing characters should not normally see this damage.
        /// Tanks will recieve this damage.
        /// </summary>
        public float DPS_SingleTarg_Melee { get { float dps = GetDPSByType(ATTACK_TYPES.AT_MELEE, 0, 0); return dps; } }
        /// <summary>
        /// Gets Raw DPS of all attacks that are Ranged type. DPS and Healing characters will use this
        /// to determine incoming damage to Raid, on specific targets. Tanks will recieve this damage in
        /// addition to the Melee single-target under chance methods.
        /// </summary>
        public float DPS_SingleTarg_Ranged { get { float dps = GetDPSByType(ATTACK_TYPES.AT_RANGED, 0, 0, 0); return dps; } }
        /// <summary>
        /// Gets Raw DPS of all attacks that are AoE type. DPS and Healing characters will use this
        /// to determine incoming damage to Raid. Tanks will recieve this damage in addition to the
        /// Melee single-target.
        /// </summary>
        public float DPS_AoE { get { float dps = GetDPSByType(ATTACK_TYPES.AT_AOE, 0, 0); return dps; } }
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
        /// <param name="BossDamageBonus">Perc value (0.10f = 110% Base damage)</param>
        /// <param name="BossDamagePenalty">Perc value (0.10f = 90% Base damage)</param>
        /// <param name="p_missPerc">Perc value (0.08f = 8% Chance for Boss to Miss Player)</param>
        /// <param name="p_dodgePerc">Perc value (0.201f = 20.10% Chance for Player to Dodge Boss Attack)</param>
        /// <param name="p_parryPerc">Perc value (0.1375f = 13.75% Chance for Player to Parry Boss Attack)</param>
        /// <param name="p_blockPerc">Perc value (0.065f = 6.5% Chance for Player to Block Boss Attack)</param>
        /// <param name="p_blockVal">How much Damage is absorbed by player's Shield in Block Value</param>
        /// <returns>The generated string</returns>
        public string GenInfoString(float BossDamageBonus, float BossDamagePenalty,
                                  float p_missPerc, float p_dodgePerc, float p_parryPerc, float p_blockPerc, float p_blockVal)
        {
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
            retVal += "This boss does the following Damage Per Second Amounts, factoring Armor and Defend Tables where applicable:" + "\r\n";
            retVal += "Single Target Melee: " + GetDPSByType(ATTACK_TYPES.AT_MELEE,BossDamageBonus, BossDamagePenalty,
                                  p_missPerc, p_dodgePerc, p_parryPerc, p_blockPerc, p_blockVal).ToString("0.0") + "\r\n";
            retVal += "Single Target Ranged: " + GetDPSByType(ATTACK_TYPES.AT_RANGED, BossDamageBonus, BossDamagePenalty,
                                  p_missPerc).ToString("0.0") + "\r\n";
            retVal += "Raid AoE: " + GetDPSByType(ATTACK_TYPES.AT_AOE, BossDamageBonus, 0).ToString("0.0") + "\r\n";
            retVal += "\r\n";
            retVal += "Comment(s):\r\n" + Comment;
            //
            return retVal;
        }
        /// <summary>
        /// Generates a Fight Info description listing the stats of the fight as well as any comments listed for the boss
        /// </summary>
        /// <returns>The generated string</returns>
        public string GenInfoString() { return GenInfoString(0,0,0,0,0,0,0); }
        #endregion
    }
}
