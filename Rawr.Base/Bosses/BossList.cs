using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Rawr.Bosses;

namespace Rawr {
    public class BossList {
        // Constructors
        public BossList()
        {
            #region Instance Declarations
            AnubRekhan anubrekhan = new AnubRekhan();
            GrandWidowFaerlina grandwidow = new GrandWidowFaerlina();
            Maexxna maexxna = new Maexxna();
            NoththePlaguebringer noth = new NoththePlaguebringer();
            ArchavonTheStoneWatcher archavon = new ArchavonTheStoneWatcher();
            Malygos malygos = new Malygos();
            LordMarrowgar lordmarrowgar = new LordMarrowgar();
            #endregion
            list = new BossHandler[] {
                #region ==== Tier 7 Content ====
                // Naxxramas
                anubrekhan[0],anubrekhan[1],
                grandwidow[0],grandwidow[1],
                maexxna[0],maexxna[1],
                noth[0],noth[1],
                new HeigantheUnclean_10(),new HeigantheUnclean_25(),
                new Loatheb_10(),new Loatheb_25(),
                new InstructorRazuvious_10(),new InstructorRazuvious_25(),
                new GothiktheHarvester_10(),new GothiktheHarvester_25(),
                new FourHorsemen_10(),new FourHorsemen_25(),
                new Patchwerk_10(),new Patchwerk_25(),
                new Grobbulus_10(),new Grobbulus_25(),
                new Gluth_10(),new Gluth_25(),
                new Thaddius_10(),new Thaddius_25(),
                new Sapphiron_10(),new Sapphiron_25(),
                new KelThuzad_10(),new KelThuzad_25(),
                // The Obsidian Sanctum
                new Shadron_10(),new Shadron_25(),
                new Tenebron_10(),new Tenebron_25(),
                new Vesperon_10(),new Vesperon_25(),
                new Sartharion_10(),new Sartharion_25(),
                // Vault of Archavon
                archavon[0],archavon[1],
                // The Eye of Eternity
                malygos[0],malygos[1],
                #endregion
                #region ==== Tier 8 Content ====
                // Vault of Archavon
                new EmalonTheStormWatcher_10(),new EmalonTheStormWatcher_25(),
                // Ulduar
                new IgnistheFurnaceMaster_10(),
                new Razorscale_10(),
                new XT002Deconstructor_10(),
                new AssemblyofIron_10(),
                new Kologarn_10(),
                new Auriaya_10(),new Auriaya_25(),
                new Mimiron_10(),
                new Freya_10(),
                new Thorim_10(),
                new Hodir_10(),new Hodir_25(),
                new GeneralVezax_10(),
                new YoggSaron_10(),
                new AlgalontheObserver_10(),
                #endregion
                #region ==== Tier 9 Content ====
                // Vault of Archavon
                new KoralonTheFlameWatcher_10(),new KoralonTheFlameWatcher_25(),
                // Trial of the Crusader
                #endregion
                #region ==== Tier 10 Content ====
                // Icecrown Citadel
                lordmarrowgar[0],lordmarrowgar[1],lordmarrowgar[2],lordmarrowgar[3],
                #endregion
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
        private ItemDamageType[] dts = null;
        private ItemDamageType[] DamageTypes {
            get
            {
                return dts ?? (new ItemDamageType[] { 
                    ItemDamageType.Physical,
                    ItemDamageType.Frost,
                    ItemDamageType.Fire,
                    ItemDamageType.Nature,
                    ItemDamageType.Arcane,
                    ItemDamageType.Shadow,
                    ItemDamageType.Holy,
                });
            }
        }
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
                    case FilterType.Content:  { if (boss.ContentString.Equals( Filter,StringComparison.OrdinalIgnoreCase)) { retList.Add(boss); } break; }
                    case FilterType.Instance: { if (boss.Instance.Equals(Filter,StringComparison.OrdinalIgnoreCase)) { retList.Add(boss); } break; }
                    case FilterType.Version:  { if (boss.VersionString.Equals( Filter,StringComparison.OrdinalIgnoreCase)) { retList.Add(boss); } break; }
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
                string name = boss.ContentString + " : " + boss.Instance + " (" + boss.VersionString + ") " + boss.Name;
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
                    string checkName = boss.ContentString + " : " + boss.Instance + " (" + boss.VersionString + ") " + boss.Name;
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
                    if (!names.Contains(TheEZModeBoss_Called.ContentString)) { names.Add(TheEZModeBoss_Called.ContentString); }
                    if (!names.Contains(TheAvgBoss_Called.ContentString)) { names.Add(TheAvgBoss_Called.ContentString); }
                    if (!names.Contains(TheHardestBoss_Called.ContentString)) { names.Add(TheHardestBoss_Called.ContentString); }
                    foreach (BossHandler boss in calledList) { if (!names.Contains(boss.ContentString)) { names.Add(boss.ContentString); } }
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
                    if (!names.Contains(TheEZModeBoss_Called.VersionString)) { names.Add(TheEZModeBoss_Called.VersionString); }
                    if (!names.Contains(TheAvgBoss_Called.VersionString)) { names.Add(TheAvgBoss_Called.VersionString); }
                    if (!names.Contains(TheHardestBoss_Called.VersionString)) { names.Add(TheHardestBoss_Called.VersionString); }
                    foreach (BossHandler boss in calledList) { if (!names.Contains(boss.VersionString)) { names.Add(boss.VersionString); } }
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
        bool useGoodBoyAvg = true;
        public void ConvertList_Move(BossHandler[] passedList, BossHandler retboss) {
            BossHandler dummy = new BossHandler(); dummy.BerserkTimer = retboss.BerserkTimer;
            Impedance s;
            List<Impedance> moves = new List<Impedance>();
            //
            foreach (BossHandler boss in passedList)
            {
                s = boss.DynamicCompiler_Move;
                if (s.Frequency != -1f) {
                    moves.Add(s);
                } else if (useGoodBoyAvg) {
                    // Adds a move that doesn't actually occur
                    moves.Add(new Impedance()
                    {
                        Frequency = retboss.BerserkTimer,
                        Duration = 0f, //retboss.BerserkTimer,
                        Chance = 1f,
                        Breakable = true,
                    });
                }
            }
            dummy.Moves = moves;
            s = dummy.DynamicCompiler_Move;
            if (s.Frequency != -1) { retboss.Moves.Add(s); }
        }
        public void ConvertList_Stun(BossHandler[] passedList, BossHandler retboss) {
            BossHandler dummy = new BossHandler(); dummy.BerserkTimer = retboss.BerserkTimer;
            Impedance s;
            List<Impedance> stuns = new List<Impedance>();
            //
            foreach (BossHandler boss in passedList)
            {
                s = boss.DynamicCompiler_Stun;
                if (s.Frequency != -1f) {
                    stuns.Add(s);
                } else if(useGoodBoyAvg) {
                    // Adds a stun that doesn't actually occur
                    stuns.Add(new Impedance()
                    {
                        Frequency = retboss.BerserkTimer,
                        Duration = 0f, //retboss.BerserkTimer,
                        Chance = 1f,
                        Breakable = true,
                    });
                }
            }
            dummy.Stuns = stuns;
            s = dummy.DynamicCompiler_Stun;
            if (s.Frequency != -1) { retboss.Stuns.Add(s); }
        }
        public void ConvertList_Fear(BossHandler[] passedList, BossHandler retboss) {
            BossHandler dummy = new BossHandler(); dummy.BerserkTimer = retboss.BerserkTimer;
            Impedance s;
            List<Impedance> fears = new List<Impedance>();
            //
            foreach (BossHandler boss in passedList)
            {
                s = boss.DynamicCompiler_Fear;
                if (s.Frequency != -1f) {
                    fears.Add(s);
                } else if (useGoodBoyAvg) {
                    // Adds a fear that doesn't actually occur
                    fears.Add(new Impedance()
                    {
                        Frequency = retboss.BerserkTimer,
                        Duration = 0f, //retboss.BerserkTimer,
                        Chance = 1f,
                        Breakable = true,
                    });
                }
            }
            dummy.Fears = fears;
            s = dummy.DynamicCompiler_Fear;
            if (s.Frequency != -1) { retboss.Fears.Add(s); }
        }
        public void ConvertList_Root(BossHandler[] passedList, BossHandler retboss) {
            BossHandler dummy = new BossHandler(); dummy.BerserkTimer = retboss.BerserkTimer;
            Impedance s;
            List<Impedance> roots = new List<Impedance>();
            //
            foreach (BossHandler boss in passedList)
            {
                s = boss.DynamicCompiler_Root;
                if (s.Frequency != -1f) {
                    roots.Add(s);
                } else if (useGoodBoyAvg) {
                    // Adds a fear that doesn't actually occur
                    roots.Add(new Impedance()
                    {
                        Frequency = retboss.BerserkTimer,
                        Duration = 0f, //retboss.BerserkTimer,
                        Chance = 1f,
                        Breakable = true,
                    });
                }
            }
            dummy.Roots = roots;
            s = dummy.DynamicCompiler_Root;
            if (s.Frequency != -1) { retboss.Roots.Add(s); }
        }
        public void ConvertList_Dsrm(BossHandler[] passedList, BossHandler retboss) {
            BossHandler dummy = new BossHandler(); dummy.BerserkTimer = retboss.BerserkTimer;
            Impedance s;
            List<Impedance> disarms = new List<Impedance>();
            //
            foreach (BossHandler boss in passedList)
            {
                s = boss.DynamicCompiler_Disarm;
                if (s.Frequency != -1f) {
                    disarms.Add(s);
                } else if (useGoodBoyAvg) {
                    // Adds a fear that doesn't actually occur
                    disarms.Add(new Impedance()
                    {
                        Frequency = retboss.BerserkTimer,
                        Duration = 0f, //retboss.BerserkTimer,
                        Chance = 1f,
                        Breakable = true,
                    });
                }
            }
            dummy.Disarms = disarms;
            s = dummy.DynamicCompiler_Disarm;
            if (s.Frequency != -1) { retboss.Disarms.Add(s); }
        }
        private BossHandler GenTheEZModeBoss(BossHandler[] passedList) {
            useGoodBoyAvg = true;
            BossHandler retboss = new BossHandler();
            if (passedList.Length < 1) { return retboss; }
            float value = 0f;
            // Basics
            retboss.Name = "The Easiest Boss";
            value = 0f; foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.BerserkTimer); } retboss.BerserkTimer = (int)Math.Ceiling(value);
            value = passedList[0].Health; foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.Health); } retboss.Health = value;
            value = passedList[0].Armor; foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.Armor); } retboss.Armor = (int)value;
            // Resistance
            foreach (ItemDamageType t in DamageTypes) {
                value = (float)passedList[0].Resistance(t);
                foreach (BossHandler boss in passedList) {
                    value = (float)Math.Min(value, boss.Resistance(t));
                }
                retboss.Resistance(t, value);
            }
            #region Attacks
            {
                {
                    List<Attack> attacks = new List<Attack>();
                    foreach (BossHandler boss in passedList) {
                        attacks.AddRange(boss.GetFilteredAttackList(ATTACK_TYPES.AT_MELEE));
                    }
                    float perhit = 500000, numtrg = retboss.Max_Players, atkspd = 0f;
                    foreach (Attack a in attacks) {
                        if(a.Name != "Invalid"){
                            perhit = Math.Min(perhit, a.DamagePerHit);
                            numtrg = Math.Min(numtrg, a.MaxNumTargets);
                            atkspd = Math.Max(atkspd, a.AttackSpeed);
                        }
                    }
                    retboss.Attacks.Add(new Attack {
                        Name = "Avg Melee",
                        AttackType = ATTACK_TYPES.AT_MELEE,
                        DamageType = ItemDamageType.Physical,
                        DamagePerHit = perhit,
                        MaxNumTargets = numtrg,
                        AttackSpeed = atkspd,
                        IsTheDefaultMelee = true,
                    });
                }
                {
                    List<Attack> attacks = new List<Attack>();
                    foreach (BossHandler boss in passedList) {
                        attacks.AddRange(boss.GetFilteredAttackList(ATTACK_TYPES.AT_RANGED));
                    }
                    float perhit = 500000, numtrg = retboss.Max_Players, atkspd = 0f;
                    foreach (Attack a in attacks) {
                        if(a.Name != "Invalid"){
                            perhit = Math.Min(perhit, a.DamagePerHit);
                            numtrg = Math.Min(numtrg, a.MaxNumTargets);
                            atkspd = Math.Max(atkspd, a.AttackSpeed);
                        }
                    }
                    retboss.Attacks.Add(new Attack {
                        Name = "Avg Ranged",
                        AttackType = ATTACK_TYPES.AT_RANGED,
                        DamageType = ItemDamageType.Physical,
                        DamagePerHit = perhit,
                        MaxNumTargets = numtrg,
                        AttackSpeed = atkspd,
                    });
                }
                {
                    List<Attack> attacks = new List<Attack>();
                    foreach (BossHandler boss in passedList)
                    {
                        attacks.AddRange(boss.GetFilteredAttackList(ATTACK_TYPES.AT_AOE));
                    }
                    float perhit = 500000, numtrg = retboss.Max_Players, atkspd = 0f;
                    foreach (Attack a in attacks) {
                        if(a.Name != "Invalid"){
                            perhit = Math.Min(perhit, a.DamagePerHit);
                            numtrg = Math.Min(numtrg, a.MaxNumTargets);
                            atkspd = Math.Max(atkspd, a.AttackSpeed);
                        }
                    }
                    retboss.Attacks.Add(new Attack {
                        Name = "Avg AoE",
                        AttackType = ATTACK_TYPES.AT_AOE,
                        DamageType = ItemDamageType.Arcane,
                        DamagePerHit = perhit,
                        MaxNumTargets = numtrg,
                        AttackSpeed = atkspd,
                    });
                }

                /*{
                    float perhit = passedList[0].Attacks[0].DamagePerHit; foreach (BossHandler boss in passedList) { perhit = Math.Max(perhit, boss.Attacks[0].Name == "Invalid" ? perhit : boss.Attacks[0].DamagePerHit); }
                    float numtrg = passedList[0].Attacks[0].MaxNumTargets; foreach (BossHandler boss in passedList) { numtrg = Math.Max(numtrg, boss.Attacks[0].Name == "Invalid" ? numtrg : boss.Attacks[0].MaxNumTargets); }
                    float atkspd = passedList[0].Attacks[0].AttackSpeed; foreach (BossHandler boss in passedList) { atkspd = Math.Min(atkspd, boss.Attacks[0].Name == "Invalid" ? atkspd : boss.Attacks[0].AttackSpeed); }
                    retboss.Attacks[0] = new Attack { Name = "Melee Attack", DamageType = ItemDamageType.Physical, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
                }*/
            }
            #endregion
            // Situational Changes
            // In Back
            value = 0f;     foreach (BossHandler boss in passedList) { value = Math.Max(value, (float)boss.InBackPerc_Melee  ); } retboss.InBackPerc_Melee   = value;
            value = 0f;     foreach (BossHandler boss in passedList) { value = Math.Max(value, (float)boss.InBackPerc_Ranged ); } retboss.InBackPerc_Ranged  = value;
            // Multi-targs
            value = 0;      foreach (BossHandler boss in passedList) { value = Math.Min(value, (float)boss.MultiTargsPerc    ); } retboss.MultiTargsPerc     = value;
            value = 3;      foreach (BossHandler boss in passedList) { value = Math.Min(value, (float)boss.MaxNumTargets     ); } retboss.MaxNumTargets      = value;
            #region Impedances
            {   // Move
                float f = -1, d = -1;
                value = 0;      foreach (BossHandler boss in passedList) { if (boss.MovingTargsFreq    <= 0) { value = 0f; break; } else { value = Math.Max(value, boss.MovingTargsFreq); } } f = (value >= retboss.BerserkTimer || value <= 0 ? 0000 : value);
                value = 5000;   foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.MovingTargsDur); } d = (value >= retboss.BerserkTimer || value <= 0 ? 5000 : value);
                if (f <= 0 || d <= 0) {
                } else {
                    retboss.Moves.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {   // Stun
                float f = -1, d = -1;
                value = 0;      foreach (BossHandler boss in passedList) { if (boss.StunningTargsFreq  <= 0) { value = 0f; break; } else { value = Math.Max(value, boss.StunningTargsFreq  ); } } f  = (value >= retboss.BerserkTimer || value <= 0 ? 0000 : value);
                value = 10*1000;foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.StunningTargsDur  ); } d   = (value >= retboss.BerserkTimer || value <= 0 ? 5000 : value);
                if (f <= 0 || d <= 0) {
                } else {
                    retboss.Stuns.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {   // Fear
                float f = -1, d = -1;
                value = 0;      foreach (BossHandler boss in passedList) { if (boss.FearingTargsFreq   <= 0) { value = 0f; break; } else { value = Math.Max(value, boss.FearingTargsFreq   ); } } f   = (value >= retboss.BerserkTimer || value <= 0 ? 0000 : value);
                value = 5000;   foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.FearingTargsDur   ); } d    = (value >= retboss.BerserkTimer || value <= 0 ? 5000 : value);
                if (f <= 0 || d <= 0) {
                } else {
                    retboss.Fears.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {   // Root
                float f = -1, d = -1;
                value = 0;      foreach (BossHandler boss in passedList) { if (boss.RootingTargsFreq   <= 0) { value = 0f; break; } else { value = Math.Max(value, boss.RootingTargsFreq   ); } } f = (value >= retboss.BerserkTimer || value <= 0 ? 0000 : value);
                value = 5000;   foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.RootingTargsDur   ); } d = (value >= retboss.BerserkTimer || value <= 0 ? 5000 : value);
                if (f <= 0 || d <= 0) {
                } else {
                    retboss.Roots.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {   // Disarm
                float f = -1, d = -1;
                value = 0;      foreach (BossHandler boss in passedList) { if (boss.DisarmingTargsFreq <= 0) { value = 0f; break; } else { value = Math.Max(value, boss.DisarmingTargsFreq ); } } f = (value >= retboss.BerserkTimer || value <= 0 ? 0000 : value);
                value = 5000;   foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.DisarmingTargsDur ); } d  = (value >= retboss.BerserkTimer || value <= 0 ? 5000 : value);
                if (f <= 0 || d <= 0) {
                } else {
                    retboss.Disarms.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            #endregion
            //
            return retboss;
        }
        private BossHandler GenTheAvgBoss(BossHandler[] passedList) {
            useGoodBoyAvg = true;
            BossHandler retboss = new BossHandler();
            if (passedList.Length < 1) { return retboss; }
            float value = 0f;
            // Basics
            retboss.Name = "The Average Boss";
            value = 0f; foreach (BossHandler boss in passedList) { value += boss.BerserkTimer; } value /= passedList.Length; retboss.BerserkTimer = (int)Math.Floor(value);
            value = 0f; foreach (BossHandler boss in passedList) { value += boss.Health; } value /= passedList.Length; retboss.Health = value;
            value = 0f; foreach (BossHandler boss in passedList) { value += boss.Armor; } value /= passedList.Length; retboss.Armor = (int)value;
            // Resistance
            foreach (ItemDamageType t in DamageTypes) {
                value = 0f;
                foreach (BossHandler boss in passedList) {
                    value += (float)boss.Resistance(t);
                }
                value /= passedList.Length;
                retboss.Resistance(t, value);
            }
            #region Attacks
            {
                {
                    List<Attack> attacks = new List<Attack>();
                    foreach (BossHandler boss in passedList) {
                        attacks.AddRange(boss.GetFilteredAttackList(ATTACK_TYPES.AT_MELEE));
                    }
                    if (attacks.Count > 0)
                    {
                        float perhit = 0f, numtrg = 0f, atkspd = 0f;
                        foreach (Attack a in attacks)
                        {
                            if (a.Name != "Invalid")
                            {
                                perhit += a.DamagePerHit;
                                numtrg += a.MaxNumTargets;
                                atkspd += a.AttackSpeed;
                            }
                        }
                        perhit /= (float)attacks.Count;
                        numtrg /= (float)attacks.Count;
                        atkspd /= (float)attacks.Count;
                        retboss.Attacks.Add(new Attack
                        {
                            Name = "Avg Melee",
                            AttackType = ATTACK_TYPES.AT_MELEE,
                            DamageType = ItemDamageType.Physical,
                            DamagePerHit = perhit,
                            MaxNumTargets = numtrg,
                            AttackSpeed = atkspd,
                            IsTheDefaultMelee = true,
                        });
                    }
                }
                {
                    List<Attack> attacks = new List<Attack>();
                    foreach (BossHandler boss in passedList) {
                        attacks.AddRange(boss.GetFilteredAttackList(ATTACK_TYPES.AT_RANGED));
                    }
                    if (attacks.Count > 0)
                    {
                        float perhit = 0f, numtrg = 0f, atkspd = 0f;
                        foreach (Attack a in attacks)
                        {
                            if (a.Name != "Invalid")
                            {
                                perhit += a.DamagePerHit;
                                numtrg += a.MaxNumTargets;
                                atkspd += a.AttackSpeed;
                            }
                        }
                        perhit /= (float)attacks.Count;
                        numtrg /= (float)attacks.Count;
                        atkspd /= (float)attacks.Count;
                        retboss.Attacks.Add(new Attack
                        {
                            Name = "Avg Ranged",
                            AttackType = ATTACK_TYPES.AT_RANGED,
                            DamageType = ItemDamageType.Physical,
                            DamagePerHit = perhit,
                            MaxNumTargets = numtrg,
                            AttackSpeed = atkspd,
                        });
                    }
                }
                {
                    List<Attack> attacks = new List<Attack>();
                    foreach (BossHandler boss in passedList)
                    {
                        attacks.AddRange(boss.GetFilteredAttackList(ATTACK_TYPES.AT_AOE));
                    }
                    if (attacks.Count > 0)
                    {
                        float perhit = 0f, numtrg = 0f, atkspd = 0f;
                        foreach (Attack a in attacks)
                        {
                            if (a.Name != "Invalid")
                            {
                                perhit += a.DamagePerHit;
                                numtrg += a.MaxNumTargets;
                                atkspd += a.AttackSpeed;
                            }
                        }
                        perhit /= (float)attacks.Count;
                        numtrg /= (float)attacks.Count;
                        atkspd /= (float)attacks.Count;
                        retboss.Attacks.Add(new Attack
                        {
                            Name = "Avg AoE",
                            AttackType = ATTACK_TYPES.AT_AOE,
                            DamageType = ItemDamageType.Arcane,
                            DamagePerHit = perhit,
                            MaxNumTargets = numtrg,
                            AttackSpeed = atkspd,
                        });
                    }
                }

                /*
                int count = 0;
                float perhit = passedList[0].Attacks[0].DamagePerHit;
                foreach (BossHandler boss in passedList) {
                    perhit += boss.Attacks[0].Name == "Invalid" ? 0f : boss.Attacks[0].DamagePerHit;
                    if (boss.Attacks[0].Name != "Invalid") { count++; }
                }
                perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in passedList) { numtrg += boss.Attacks[0].Name == "Invalid" ? 0f : boss.Attacks[0].MaxNumTargets; if (boss.Attacks[0].Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in passedList) { atkspd += boss.Attacks[0].Name == "Invalid" ? 0f : boss.Attacks[0].AttackSpeed; if (boss.Attacks[0].Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.Attacks.Add(new Attack { Name = "Melee", DamageType = ItemDamageType.Physical, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, });
                */
            }
            #endregion
            // Situational Changes
            // In Back
            value = 0f; foreach (BossHandler boss in passedList) { value += (float)boss.InBackPerc_Melee; } value /= passedList.Length; retboss.InBackPerc_Melee = value;
            value = 0f; foreach (BossHandler boss in passedList) { value += (float)boss.InBackPerc_Ranged; } value /= passedList.Length; retboss.InBackPerc_Ranged = value;
            // Multi-targs
            value = 0f; foreach (BossHandler boss in passedList) { value += (float)boss.MultiTargsPerc; } value /= passedList.Length; retboss.MultiTargsPerc = value;
            value = 0f; foreach (BossHandler boss in passedList) { value += (float)boss.MaxNumTargets; } value /= passedList.Length; retboss.MaxNumTargets = (float)Math.Ceiling(value);
            #region Impedances
            {
                // Move
                float f = -1, d = -1;
                value = 0f; foreach (BossHandler boss in passedList) { value += (boss.MovingTargsFreq > 0 && boss.MovingTargsFreq < boss.BerserkTimer) ? boss.MovingTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; f = value;
                value = 0f; foreach (BossHandler boss in passedList) { value += boss.MovingTargsDur; } value /= passedList.Length; d = value;
                if (f == 0 || d == 0) {
                } else {
                    retboss.Moves.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {
                // Stun
                float f = -1, d = -1;
                value = 0f; foreach (BossHandler boss in passedList) { value += (boss.StunningTargsFreq > 0 && boss.StunningTargsFreq < boss.BerserkTimer) ? boss.StunningTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; f = value;
                value = 0f; foreach (BossHandler boss in passedList) { value += boss.StunningTargsDur; } value /= passedList.Length; d = value;
                if (f == 0 || d == 0) {
                } else {
                    retboss.Stuns.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {
                // Fear
                float f = -1, d = -1;
                value = 0f; foreach (BossHandler boss in passedList) { value += (boss.FearingTargsFreq > 0 && boss.FearingTargsFreq < boss.BerserkTimer) ? boss.FearingTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; f = value;
                value = 0f; foreach (BossHandler boss in passedList) { value += boss.FearingTargsDur; } value /= passedList.Length; d = value;
                if (f == 0 || d == 0) {
                } else {
                    retboss.Fears.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {
                // Root
                float f = -1, d = -1;
                value = 0f; foreach (BossHandler boss in passedList) { value += (boss.RootingTargsFreq > 0 && boss.RootingTargsFreq < boss.BerserkTimer) ? boss.RootingTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; f = value;
                value = 0f; foreach (BossHandler boss in passedList) { value += boss.RootingTargsDur; } value /= passedList.Length; d = value;
                if (f == 0 || d == 0) {
                } else {
                    retboss.Roots.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {
                // Disarm
                float f = -1, d = -1;
                value = 0f; foreach (BossHandler boss in passedList) { value += (boss.DisarmingTargsFreq > 0 && boss.DisarmingTargsFreq < boss.BerserkTimer) ? boss.DisarmingTargsFreq : retboss.BerserkTimer; } value /= passedList.Length; f = value;
                value = 0f; foreach (BossHandler boss in passedList) { value += boss.DisarmingTargsDur; } value /= passedList.Length; d = value;
                if (f == 0 || d == 0) {
                } else {
                    retboss.Disarms.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            #endregion
            //
            return retboss;
        }
        private BossHandler GenTheHardestBoss(BossHandler[] passedList) {
            useGoodBoyAvg = false;
            BossHandler retboss = new BossHandler();
            if (passedList.Length < 1) { return retboss; }
            float value = 0f;
            // Basics
            retboss.Name = "The Hardest Boss";
            value = passedList[0].BerserkTimer; foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.BerserkTimer); } retboss.BerserkTimer = (int)Math.Floor(value);
            value = 0f; foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.Health); } retboss.Health = value;
            value = 0f; foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.Armor); } retboss.Armor = (int)value;
            // Resistance
            foreach (ItemDamageType t in DamageTypes) {
                value = 0f;
                foreach (BossHandler boss in passedList) {
                    value = (float)Math.Max(value, boss.Resistance(t));
                }
                retboss.Resistance(t, value);
            }
            // Fight Requirements
            bool is25 = false;
            foreach (BossHandler boss in passedList) {
                if (boss.Version == BossHandler.Versions.V_25N || boss.Version == BossHandler.Versions.V_25H) {
                    is25 = true;
                    break;
                }
            }
            if (is25) {
                retboss.Max_Players = 25;
                retboss.Min_Tanks = 3;
                retboss.Min_Healers = 5;
            } else {
                retboss.Max_Players = 10;
                retboss.Min_Tanks = 3;
                retboss.Min_Healers = 3;
            }
            #region Attacks
            {
                {
                    List<Attack> attacks = new List<Attack>();
                    foreach (BossHandler boss in passedList) {
                        attacks.AddRange(boss.GetFilteredAttackList(ATTACK_TYPES.AT_MELEE));
                    }
                    float perhit = 0f, numtrg = 0f, atkspd = 45f;
                    foreach (Attack a in attacks) {
                        if(a.Name != "Invalid"){
                            perhit = Math.Max(perhit, a.DamagePerHit);
                            numtrg = Math.Max(numtrg, a.MaxNumTargets);
                            atkspd = Math.Min(atkspd, a.AttackSpeed);
                        }
                    }
                    retboss.Attacks.Add(new Attack {
                        Name = "Avg Melee",
                        AttackType = ATTACK_TYPES.AT_MELEE,
                        DamageType = ItemDamageType.Physical,
                        DamagePerHit = perhit,
                        MaxNumTargets = numtrg,
                        AttackSpeed = atkspd,
                        IsTheDefaultMelee = true,
                    });
                }
                {
                    List<Attack> attacks = new List<Attack>();
                    foreach (BossHandler boss in passedList) {
                        attacks.AddRange(boss.GetFilteredAttackList(ATTACK_TYPES.AT_RANGED));
                    }
                    float perhit = 0f, numtrg = 0f, atkspd = 45f;
                    foreach (Attack a in attacks) {
                        if(a.Name != "Invalid"){
                            perhit = Math.Max(perhit, a.DamagePerHit);
                            numtrg = Math.Max(numtrg, a.MaxNumTargets);
                            atkspd = Math.Min(atkspd, a.AttackSpeed);
                        }
                    }
                    retboss.Attacks.Add(new Attack {
                        Name = "Avg Ranged",
                        AttackType = ATTACK_TYPES.AT_RANGED,
                        DamageType = ItemDamageType.Physical,
                        DamagePerHit = perhit,
                        MaxNumTargets = numtrg,
                        AttackSpeed = atkspd,
                    });
                }
                {
                    List<Attack> attacks = new List<Attack>();
                    foreach (BossHandler boss in passedList)
                    {
                        attacks.AddRange(boss.GetFilteredAttackList(ATTACK_TYPES.AT_AOE));
                    }
                    float perhit = 0f, numtrg = 0f, atkspd = 45f;
                    foreach (Attack a in attacks) {
                        if(a.Name != "Invalid"){
                            perhit = Math.Max(perhit, a.DamagePerHit);
                            numtrg = Math.Max(numtrg, a.MaxNumTargets);
                            atkspd = Math.Min(atkspd, a.AttackSpeed);
                        }
                    }
                    retboss.Attacks.Add(new Attack {
                        Name = "Avg AoE",
                        AttackType = ATTACK_TYPES.AT_AOE,
                        DamageType = ItemDamageType.Arcane,
                        DamagePerHit = perhit,
                        MaxNumTargets = numtrg,
                        AttackSpeed = atkspd,
                    });
                }

                /*{
                    float perhit = passedList[0].Attacks[0].DamagePerHit; foreach (BossHandler boss in passedList) { perhit = Math.Max(perhit, boss.Attacks[0].Name == "Invalid" ? perhit : boss.Attacks[0].DamagePerHit); }
                    float numtrg = passedList[0].Attacks[0].MaxNumTargets; foreach (BossHandler boss in passedList) { numtrg = Math.Max(numtrg, boss.Attacks[0].Name == "Invalid" ? numtrg : boss.Attacks[0].MaxNumTargets); }
                    float atkspd = passedList[0].Attacks[0].AttackSpeed; foreach (BossHandler boss in passedList) { atkspd = Math.Min(atkspd, boss.Attacks[0].Name == "Invalid" ? atkspd : boss.Attacks[0].AttackSpeed); }
                    retboss.Attacks[0] = new Attack { Name = "Melee Attack", DamageType = ItemDamageType.Physical, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
                }*/
            }
            #endregion
            // Situational Changes
            // In Back
            value = 1.00f;      foreach (BossHandler boss in passedList) { value = (float)Math.Min(value, boss.InBackPerc_Melee   ); } retboss.InBackPerc_Melee  = value;
            value = 1.00f;      foreach (BossHandler boss in passedList) { value = (float)Math.Min(value, boss.InBackPerc_Ranged  ); } retboss.InBackPerc_Ranged = value;
            // Multi-targs
            value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, (float)boss.MultiTargsPerc     ); } retboss.MultiTargsPerc    = value;
            value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, (float)boss.MaxNumTargets      ); } retboss.MaxNumTargets     = (float)Math.Ceiling(value);
            #region Impedances
            {
                // Move
                float f = -1, d = -1;
                value = 19 * 20;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.MovingTargsFreq    > 0 ? boss.MovingTargsFreq    : value); } f    = Math.Max(20,value);
                value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.MovingTargsDur     ); } d    = Math.Min(10*1000,value);
                if (f == 0 || d == 0) {
                } else {
                    retboss.Moves.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {
                // Stun
                float f = -1, d = -1;
                value = 19 * 20;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.StunningTargsFreq  > 0 ? boss.StunningTargsFreq  : value); } f  = Math.Max(20,value);
                value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.StunningTargsDur   ); } d  = Math.Min(10*1000,value);
                if (f == 0 || d == 0) {
                } else {
                    retboss.Stuns.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {
                // Fear
                float f = -1, d = -1;
                value = 19 * 20;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.FearingTargsFreq   > 0 ? boss.FearingTargsFreq   : value); } f   = Math.Max(20,value);
                value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.FearingTargsDur    ); } d   = Math.Min(10*1000,value);
                if (f == 0 || d == 0) {
                } else {
                    retboss.Fears.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {
                // Root
                float f = -1, d = -1;
                value = 19 * 20;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.RootingTargsFreq   > 0 ? boss.RootingTargsFreq   : value); } f   = Math.Max(20,value);
                value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.RootingTargsDur    ); } d   = Math.Min(10*1000,value);
                if (f == 0 || d == 0) {
                } else {
                    retboss.Roots.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            {
                // Disarm
                float f = -1, d = -1;
                value = 19 * 20;    foreach (BossHandler boss in passedList) { value = Math.Min(value, boss.DisarmingTargsFreq < 0 ? boss.DisarmingTargsFreq : value); } f = Math.Max(20,value);
                value = 0f;         foreach (BossHandler boss in passedList) { value = Math.Max(value, boss.DisarmingTargsDur  ); } d = Math.Min(10*1000,value);
                if (f == 0 || d == 0) {
                } else {
                    retboss.Disarms.Add(new Impedance() { Frequency = f, Duration = d, Chance = 1f, Breakable = true, });
                }
            }
            #endregion
            //
            return retboss;
        }
        #endregion
    }
}
