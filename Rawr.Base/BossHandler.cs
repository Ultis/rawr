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
    }
    #endregion
    public class BossList {
        // Constructors
        public BossList() {
            list = new BossHandler[] {
                // Tier 7 Content
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
                // Tier 7.5 Content
                new Patchwerk_25(),
                new Maexxna_25(),
                new AnubRekhan_25(),
            };
            DamageTypes = new ItemDamageType[] { ItemDamageType.Physical, ItemDamageType.Nature, ItemDamageType.Arcane, ItemDamageType.Frost, ItemDamageType.Fire, ItemDamageType.Shadow, ItemDamageType.Holy, };
            TheEZModeBoss  = GenTheEZModeBoss();
            TheAvgBoss     = GenTheAvgBoss();
            TheHardestBoss = GenTheHardestBoss();
        }
        #region Variables
        public const int NormCharLevel = 80;
        public BossHandler[] list;
        private ItemDamageType[] DamageTypes;
        public BossHandler TheEZModeBoss;
        public BossHandler TheAvgBoss;
        public BossHandler TheHardestBoss;
        #endregion
        #region Functions
        public List<string> GetBossNames() {
            List<string> names = new List<string>() { };
            names.Add(TheEZModeBoss.Name);
            names.Add(TheAvgBoss.Name);
            names.Add(TheHardestBoss.Name);
            foreach (BossHandler boss in list) {
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
            foreach (BossHandler boss in list) {
                string name = boss.Content + " : " + boss.Instance + " (" + boss.Version + ") " + boss.Name;
                names.Add(name);
            }
            return names;
        }
        public string[] GetBetterBossNamesAsArray() { return GetBetterBossNames().ToArray(); }
        public BossHandler GetBossFromName(string name) {
            BossHandler retBoss = new BossHandler();
            if      (TheEZModeBoss.Name  == name) { retBoss = TheEZModeBoss;  }
            else if (TheAvgBoss.Name     == name) { retBoss = TheAvgBoss;     }
            else if (TheHardestBoss.Name == name) { retBoss = TheHardestBoss; }
            else {
                foreach (BossHandler boss in list) {
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
            if      (TheEZModeBoss.Name  == name) { retBoss = TheEZModeBoss;  }
            else if (TheAvgBoss.Name     == name) { retBoss = TheAvgBoss;     }
            else if (TheHardestBoss.Name == name) { retBoss = TheHardestBoss; }
            else {
                foreach (BossHandler boss in list) {
                    string checkName = boss.Content + " : " + boss.Instance + " (" + boss.Version + ") " + boss.Name;
                    if(checkName == name){
                        retBoss = boss;
                        break;
                    }
                }
            }
            return retBoss;
        }
        private BossHandler GenTheEZModeBoss() {
            BossHandler retboss = new BossHandler();
            float value = 0f;
            // Basics
            retboss.Name = "The Easiest Boss";
            value = 0f; foreach (BossHandler boss in list) { value = Math.Max(value, boss.BerserkTimer); } retboss.BerserkTimer = (int)Math.Ceiling(value);
            value = list[0].Health; foreach (BossHandler boss in list) { value = Math.Min(value, boss.Health); } retboss.Health = value;
            value = list[0].Armor; foreach (BossHandler boss in list) { value = Math.Min(value, boss.Armor); } retboss.Armor = value;
            retboss.UseParryHaste = false;
            // Resistance
            foreach (ItemDamageType t in DamageTypes) {
                value = list[0].Resistance(t);
                foreach (BossHandler boss in list) {
                    value = Math.Min(value, boss.Resistance(t));
                }
                retboss.Resistance(t, value);
            }
            // Attacks
            {
                float perhit = list[0].MeleeAttack.DamagePerHit; foreach (BossHandler boss in list) { perhit = Math.Min(perhit, boss.MeleeAttack.Name == "Invalid" ? perhit : boss.MeleeAttack.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in list) { numtrg = Math.Min(numtrg, boss.MeleeAttack.Name == "Invalid" ? numtrg : boss.MeleeAttack.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in list) { atkspd = Math.Max(atkspd, boss.MeleeAttack.Name == "Invalid" ? atkspd : boss.MeleeAttack.AttackSpeed); }
                retboss.MeleeAttack = new Attack {
                    Name = "Melee Attack",
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = perhit,
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            {
                float perhit = list[0].SpellAttack.DamagePerHit; foreach (BossHandler boss in list) { perhit = Math.Min(perhit, boss.SpellAttack.Name == "Invalid" ? perhit : boss.SpellAttack.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in list) { numtrg = Math.Min(numtrg, boss.SpellAttack.Name == "Invalid" ? numtrg : boss.SpellAttack.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in list) { atkspd = Math.Max(atkspd, boss.SpellAttack.Name == "Invalid" ? atkspd : boss.SpellAttack.AttackSpeed); }
                retboss.SpellAttack = new Attack {
                    Name = "Spell Attack",
                    DamageType = ItemDamageType.Arcane,
                    DamagePerHit = perhit,//averaging min/max value
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            {
                float perhit = list[0].SpecialAttack_1.DamagePerHit; foreach (BossHandler boss in list) { perhit = Math.Min(perhit, boss.SpecialAttack_1.Name == "Invalid" ? perhit : boss.SpecialAttack_1.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in list) { numtrg = Math.Min(numtrg, boss.SpecialAttack_1.Name == "Invalid" ? numtrg : boss.SpecialAttack_1.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in list) { atkspd = Math.Max(atkspd, boss.SpecialAttack_1.Name == "Invalid" ? atkspd : boss.SpecialAttack_1.AttackSpeed); }
                retboss.SpecialAttack_1 = new Attack {
                    Name = "Special Attack 1",
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = perhit,
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            {
                float perhit = list[0].SpecialAttack_2.DamagePerHit; foreach (BossHandler boss in list) { perhit = Math.Min(perhit, boss.SpecialAttack_2.Name == "Invalid" ? perhit : boss.SpecialAttack_2.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in list) { numtrg = Math.Min(numtrg, boss.SpecialAttack_2.Name == "Invalid" ? numtrg : boss.SpecialAttack_2.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in list) { atkspd = Math.Max(atkspd, boss.SpecialAttack_2.Name == "Invalid" ? atkspd : boss.SpecialAttack_2.AttackSpeed); }
                retboss.SpecialAttack_2 = new Attack {
                    Name = "Special Attack 2",
                    DamageType = ItemDamageType.Frost,
                    DamagePerHit = perhit,//averaging min/max value
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            {
                float perhit = list[0].SpecialAttack_3.DamagePerHit; foreach (BossHandler boss in list) { perhit = Math.Min(perhit, boss.SpecialAttack_3.Name == "Invalid" ? perhit : boss.SpecialAttack_3.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in list) { numtrg = Math.Min(numtrg, boss.SpecialAttack_3.Name == "Invalid" ? numtrg : boss.SpecialAttack_3.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in list) { atkspd = Math.Max(atkspd, boss.SpecialAttack_3.Name == "Invalid" ? atkspd : boss.SpecialAttack_3.AttackSpeed); }
                retboss.SpecialAttack_3 = new Attack {
                    Name = "Special Attack 3",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = perhit,
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            {
                float perhit = list[0].SpecialAttack_4.DamagePerHit; foreach (BossHandler boss in list) { perhit = Math.Min(perhit, boss.SpecialAttack_4.Name == "Invalid" ? perhit : boss.SpecialAttack_4.DamagePerHit); }
                float numtrg = 1f; foreach (BossHandler boss in list) { numtrg = Math.Min(numtrg, boss.SpecialAttack_4.Name == "Invalid" ? numtrg : boss.SpecialAttack_4.MaxNumTargets); }
                float atkspd = 0f; foreach (BossHandler boss in list) { atkspd = Math.Max(atkspd, boss.SpecialAttack_4.Name == "Invalid" ? atkspd : boss.SpecialAttack_4.AttackSpeed); }
                retboss.SpecialAttack_4 = new Attack {
                    Name = "Special Attack 4",
                    DamageType = ItemDamageType.Holy,
                    DamagePerHit = perhit,//averaging min/max value
                    MaxNumTargets = numtrg,
                    AttackSpeed = atkspd,
                };
            }
            // Situational Changes
            value = 0f;                         foreach (BossHandler boss in list) { value = Math.Max(value, boss.InBackPerc_Melee  ); } retboss.InBackPerc_Melee   = value;
            value = 0f;                         foreach (BossHandler boss in list) { value = Math.Max(value, boss.InBackPerc_Ranged ); } retboss.InBackPerc_Ranged  = value;
            value = list[0].MultiTargsPerc;     foreach (BossHandler boss in list) { value = Math.Min(value, boss.MultiTargsPerc    ); } retboss.MultiTargsPerc     = value;
            value = list[0].MaxNumTargets;      foreach (BossHandler boss in list) { value = Math.Min(value, boss.MaxNumTargets     ); } retboss.MaxNumTargets      = value;
            value = retboss.BerserkTimer;       foreach (BossHandler boss in list) { value = Math.Max(value, boss.StunningTargsFreq ); } retboss.StunningTargsFreq  = value;
            value = list[0].StunningTargsDur;   foreach (BossHandler boss in list) { value = Math.Min(value, boss.StunningTargsDur  ); } retboss.StunningTargsDur   = value;
            value = list[0].MovingTargsTime;    foreach (BossHandler boss in list) { value = Math.Min(value, boss.MovingTargsTime   ); } retboss.MovingTargsTime    = value;
            value = list[0].DisarmingTargsPerc; foreach (BossHandler boss in list) { value = Math.Min(value, boss.DisarmingTargsPerc); } retboss.DisarmingTargsPerc = value;
            //
            return retboss;
        }
        private BossHandler GenTheAvgBoss() {
            BossHandler retboss = new BossHandler();
            float value = 0f;
            int count = 0;
            bool use = false;
            // Basics
            retboss.Name = "The Average Boss";
            value = 0f; foreach (BossHandler boss in list) { value += boss.BerserkTimer; } value /= list.Length; retboss.BerserkTimer = (int)Math.Floor(value);
            value = 0f; foreach (BossHandler boss in list) { value += boss.Health; } value /= list.Length; retboss.Health = value;
            value = 0f; foreach (BossHandler boss in list) { value += boss.Armor; } value /= list.Length; retboss.Armor = value;
            use = false; foreach (BossHandler boss in list) { use |= boss.UseParryHaste; } retboss.UseParryHaste = use;
            // Resistance
            foreach (ItemDamageType t in DamageTypes) {
                value = 0f;
                foreach (BossHandler boss in list) {
                    value += boss.Resistance(t);
                }
                value /= list.Length;
                retboss.Resistance(t, value);
            }
            #region Attacks
            {
                count = 0;
                float perhit = list[0].MeleeAttack.DamagePerHit; foreach (BossHandler boss in list) { perhit += boss.MeleeAttack.Name == "Invalid" ? 0f : boss.MeleeAttack.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in list) { numtrg += boss.MeleeAttack.Name == "Invalid" ? 0f : boss.MeleeAttack.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in list) { atkspd += boss.MeleeAttack.Name == "Invalid" ? 0f : boss.MeleeAttack.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.MeleeAttack = new Attack { Name = "Melee Attack", DamageType = ItemDamageType.Physical, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                count = 0;
                float perhit = list[0].SpellAttack.DamagePerHit; foreach (BossHandler boss in list) { perhit += boss.SpellAttack.Name == "Invalid" ? 0f : boss.SpellAttack.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in list) { numtrg += boss.SpellAttack.Name == "Invalid" ? 0f : boss.SpellAttack.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in list) { atkspd += boss.SpellAttack.Name == "Invalid" ? 0f : boss.SpellAttack.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.SpellAttack = new Attack { Name = "Spell Attack", DamageType = ItemDamageType.Arcane, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                count = 0;
                float perhit = list[0].SpecialAttack_1.DamagePerHit; foreach (BossHandler boss in list) { perhit += boss.SpecialAttack_1.Name == "Invalid" ? 0f : boss.SpecialAttack_1.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in list) { numtrg += boss.SpecialAttack_1.Name == "Invalid" ? 0f : boss.SpecialAttack_1.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in list) { atkspd += boss.SpecialAttack_1.Name == "Invalid" ? 0f : boss.SpecialAttack_1.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.SpecialAttack_1 = new Attack { Name = "Special Attack 1", DamageType = ItemDamageType.Fire, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                count = 0;
                float perhit = list[0].SpecialAttack_2.DamagePerHit; foreach (BossHandler boss in list) { perhit += boss.SpecialAttack_2.Name == "Invalid" ? 0f : boss.SpecialAttack_2.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in list) { numtrg += boss.SpecialAttack_2.Name == "Invalid" ? 0f : boss.SpecialAttack_2.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in list) { atkspd += boss.SpecialAttack_2.Name == "Invalid" ? 0f : boss.SpecialAttack_2.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.SpecialAttack_2 = new Attack { Name = "Special Attack 2", DamageType = ItemDamageType.Frost, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                count = 0;
                float perhit = list[0].SpecialAttack_3.DamagePerHit; foreach (BossHandler boss in list) { perhit += boss.SpecialAttack_3.Name == "Invalid" ? 0f : boss.SpecialAttack_3.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in list) { numtrg += boss.SpecialAttack_3.Name == "Invalid" ? 0f : boss.SpecialAttack_3.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in list) { atkspd += boss.SpecialAttack_3.Name == "Invalid" ? 0f : boss.SpecialAttack_3.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.SpecialAttack_3 = new Attack { Name = "Special Attack 3", DamageType = ItemDamageType.Nature, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                count = 0;
                float perhit = list[0].SpecialAttack_4.DamagePerHit; foreach (BossHandler boss in list) { perhit += boss.SpecialAttack_4.Name == "Invalid" ? 0f : boss.SpecialAttack_4.DamagePerHit; if (boss.MeleeAttack.Name != "Invalid") { count++; } } perhit /= (float)count;
                count = 0; float numtrg = 1f; foreach (BossHandler boss in list) { numtrg += boss.SpecialAttack_4.Name == "Invalid" ? 0f : boss.SpecialAttack_4.MaxNumTargets; if (boss.MeleeAttack.Name != "Invalid") { count++; } } numtrg /= (float)count;
                count = 0; float atkspd = 0f; foreach (BossHandler boss in list) { atkspd += boss.SpecialAttack_4.Name == "Invalid" ? 0f : boss.SpecialAttack_4.AttackSpeed; if (boss.MeleeAttack.Name != "Invalid") { count++; } } atkspd /= (float)count;
                retboss.SpecialAttack_4 = new Attack { Name = "Special Attack 4", DamageType = ItemDamageType.Holy, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            #endregion
            // Situational Changes
            value = 0f; count = 0; foreach (BossHandler boss in list) { value += boss.InBackPerc_Melee; } value /= list.Length; retboss.InBackPerc_Melee = value;
            value = 0f; count = 0; foreach (BossHandler boss in list) { value += boss.InBackPerc_Ranged; } value /= list.Length; retboss.InBackPerc_Ranged = value;
            value = 0f; count = 0; foreach (BossHandler boss in list) { value += boss.MultiTargsPerc; } value /= list.Length; retboss.MultiTargsPerc = value;
            value = 0f; count = 0; foreach (BossHandler boss in list) { value += boss.MaxNumTargets; } value /= list.Length; retboss.MaxNumTargets = value;
            value = 0f; count = 0; foreach (BossHandler boss in list) { value += (boss.StunningTargsFreq > 0 && boss.StunningTargsFreq < boss.BerserkTimer) ? boss.StunningTargsFreq : retboss.BerserkTimer; } value /= list.Length; retboss.StunningTargsFreq = value;
            value = 0f; count = 0; foreach (BossHandler boss in list) { value += boss.StunningTargsDur; } value /= list.Length; retboss.StunningTargsDur = value;
            value = 0f; count = 0; foreach (BossHandler boss in list) { value += boss.MovingTargsTime / boss.BerserkTimer; } value /= list.Length; retboss.MovingTargsTime = value * retboss.BerserkTimer;
            value = 0f; count = 0; foreach (BossHandler boss in list) { value += boss.DisarmingTargsPerc; } value /= list.Length; retboss.DisarmingTargsPerc = value;
            //
            return retboss;
        }
        private BossHandler GenTheHardestBoss() {
            BossHandler retboss = new BossHandler();
            float value = 0f;
            // Basics
            retboss.Name = "The Hardest Boss";
            value = list[0].BerserkTimer; foreach (BossHandler boss in list) { value = Math.Min(value, boss.BerserkTimer); } retboss.BerserkTimer = (int)Math.Floor(value);
            value = 0f; foreach (BossHandler boss in list) { value = Math.Max(value, boss.Health); } retboss.Health = value;
            value = 0f; foreach (BossHandler boss in list) { value = Math.Max(value, boss.Armor); } retboss.Armor = value;
            retboss.UseParryHaste = true;
            // Resistance
            foreach (ItemDamageType t in DamageTypes) {
                value = 0f;
                foreach (BossHandler boss in list) {
                    value = Math.Max(value, boss.Resistance(t));
                }
                retboss.Resistance(t, value);
            }
            #region Attacks
            {
                float perhit = list[0].MeleeAttack.DamagePerHit;  foreach (BossHandler boss in list) { perhit = Math.Max(perhit, boss.MeleeAttack.Name == "Invalid" ? perhit : boss.MeleeAttack.DamagePerHit); }
                float numtrg = list[0].MeleeAttack.MaxNumTargets; foreach (BossHandler boss in list) { numtrg = Math.Max(numtrg, boss.MeleeAttack.Name == "Invalid" ? numtrg : boss.MeleeAttack.MaxNumTargets); }
                float atkspd = list[0].MeleeAttack.AttackSpeed;   foreach (BossHandler boss in list) { atkspd = Math.Min(atkspd, boss.MeleeAttack.Name == "Invalid" ? atkspd : boss.MeleeAttack.AttackSpeed); }
                retboss.MeleeAttack = new Attack { Name = "Melee Attack", DamageType = ItemDamageType.Physical, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                float perhit = list[0].SpellAttack.DamagePerHit;  foreach (BossHandler boss in list) { perhit = Math.Max(perhit, boss.SpellAttack.Name == "Invalid" ? perhit : boss.SpellAttack.DamagePerHit); }
                float numtrg = list[0].SpellAttack.MaxNumTargets; foreach (BossHandler boss in list) { numtrg = Math.Max(numtrg, boss.SpellAttack.Name == "Invalid" ? numtrg : boss.SpellAttack.MaxNumTargets); }
                float atkspd = list[0].SpellAttack.AttackSpeed;   foreach (BossHandler boss in list) { atkspd = Math.Min(atkspd, boss.SpellAttack.Name == "Invalid" ? atkspd : boss.SpellAttack.AttackSpeed); }
                retboss.SpellAttack = new Attack { Name = "Spell Attack", DamageType = ItemDamageType.Arcane, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                float perhit = list[0].SpecialAttack_1.DamagePerHit;  foreach (BossHandler boss in list) { perhit = Math.Max(perhit, boss.SpecialAttack_1.Name == "Invalid" ? perhit : boss.SpecialAttack_1.DamagePerHit); }
                float numtrg = list[0].SpecialAttack_1.MaxNumTargets; foreach (BossHandler boss in list) { numtrg = Math.Max(numtrg, boss.SpecialAttack_1.Name == "Invalid" ? numtrg : boss.SpecialAttack_1.MaxNumTargets); }
                float atkspd = list[0].SpecialAttack_1.AttackSpeed;   foreach (BossHandler boss in list) { atkspd = Math.Min(atkspd, boss.SpecialAttack_1.Name == "Invalid" ? atkspd : boss.SpecialAttack_1.AttackSpeed); }
                retboss.SpecialAttack_1 = new Attack { Name = "Special Attack 1", DamageType = ItemDamageType.Holy, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                float perhit = list[0].SpecialAttack_2.DamagePerHit;  foreach (BossHandler boss in list) { perhit = Math.Max(perhit, boss.SpecialAttack_2.Name == "Invalid" ? perhit : boss.SpecialAttack_2.DamagePerHit); }
                float numtrg = list[0].SpecialAttack_2.MaxNumTargets; foreach (BossHandler boss in list) { numtrg = Math.Max(numtrg, boss.SpecialAttack_2.Name == "Invalid" ? numtrg : boss.SpecialAttack_2.MaxNumTargets); }
                float atkspd = list[0].SpecialAttack_2.AttackSpeed;   foreach (BossHandler boss in list) { atkspd = Math.Min(atkspd, boss.SpecialAttack_2.Name == "Invalid" ? atkspd : boss.SpecialAttack_2.AttackSpeed); }
                retboss.SpecialAttack_2 = new Attack { Name = "Special Attack 2", DamageType = ItemDamageType.Shadow, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                float perhit = list[0].SpecialAttack_3.DamagePerHit;  foreach (BossHandler boss in list) { perhit = Math.Max(perhit, boss.SpecialAttack_3.Name == "Invalid" ? perhit : boss.SpecialAttack_3.DamagePerHit); }
                float numtrg = list[0].SpecialAttack_3.MaxNumTargets; foreach (BossHandler boss in list) { numtrg = Math.Max(numtrg, boss.SpecialAttack_3.Name == "Invalid" ? numtrg : boss.SpecialAttack_3.MaxNumTargets); }
                float atkspd = list[0].SpecialAttack_3.AttackSpeed;   foreach (BossHandler boss in list) { atkspd = Math.Min(atkspd, boss.SpecialAttack_3.Name == "Invalid" ? atkspd : boss.SpecialAttack_3.AttackSpeed); }
                retboss.SpecialAttack_3 = new Attack { Name = "Special Attack 3", DamageType = ItemDamageType.Nature, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            {
                float perhit = list[0].SpecialAttack_4.DamagePerHit;  foreach (BossHandler boss in list) { perhit = Math.Max(perhit, boss.SpecialAttack_4.Name == "Invalid" ? perhit : boss.SpecialAttack_4.DamagePerHit); }
                float numtrg = list[0].SpecialAttack_4.MaxNumTargets; foreach (BossHandler boss in list) { numtrg = Math.Max(numtrg, boss.SpecialAttack_4.Name == "Invalid" ? numtrg : boss.SpecialAttack_4.MaxNumTargets); }
                float atkspd = list[0].SpecialAttack_4.AttackSpeed;   foreach (BossHandler boss in list) { atkspd = Math.Min(atkspd, boss.SpecialAttack_4.Name == "Invalid" ? atkspd : boss.SpecialAttack_4.AttackSpeed); }
                retboss.SpecialAttack_4 = new Attack { Name = "Special Attack 4", DamageType = ItemDamageType.Fire, DamagePerHit = perhit, MaxNumTargets = numtrg, AttackSpeed = atkspd, };
            }
            #endregion
            // Situational Changes
            value = list[0].InBackPerc_Melee;   foreach (BossHandler boss in list) { value = Math.Min(value, boss.InBackPerc_Melee  ); } retboss.InBackPerc_Melee   = value;
            value = list[0].InBackPerc_Ranged;  foreach (BossHandler boss in list) { value = Math.Min(value, boss.InBackPerc_Ranged ); } retboss.InBackPerc_Ranged  = value;
            value = 0f;                         foreach (BossHandler boss in list) { value = Math.Max(value, boss.MultiTargsPerc    ); } retboss.MultiTargsPerc     = value;
            value = 0f;                         foreach (BossHandler boss in list) { value = Math.Max(value, boss.MaxNumTargets     ); } retboss.MaxNumTargets      = value;
            value = list[0].BerserkTimer;       foreach (BossHandler boss in list) { value = Math.Min(value, boss.StunningTargsFreq != 0 ? boss.StunningTargsFreq : value); } retboss.StunningTargsFreq  = value;
            value = 0f;                         foreach (BossHandler boss in list) { value = Math.Max(value, boss.StunningTargsDur  ); } retboss.StunningTargsDur   = value;
            value = 0f;                         foreach (BossHandler boss in list) { value = Math.Max(value, boss.MovingTargsTime / boss.BerserkTimer); } retboss.MovingTargsTime = value * retboss.BerserkTimer;
            value = 0f;                         foreach (BossHandler boss in list) { value = Math.Max(value, boss.DisarmingTargsPerc); } retboss.DisarmingTargsPerc = value;
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
            BerserkTimer = 15 * 60; // The longest noted Enrage timer is 15 minutes, and seriously, if the fight is taking that long, then fail... just fail.
            Level = (int)POSSIBLE_LEVELS.NPC_83;
            Health = 1000000f;
            Armor = (float)StatConversion.NPC_ARMOR[Level - NormCharLevel];
            UseParryHaste = false;
            // Resistance
            ItemDamageType[] DamageTypes = new ItemDamageType[] { ItemDamageType.Physical, ItemDamageType.Nature, ItemDamageType.Arcane, ItemDamageType.Frost, ItemDamageType.Fire, ItemDamageType.Shadow, ItemDamageType.Holy, };
            foreach (ItemDamageType t in DamageTypes) { Resistance(t, 0f); }
            // Attacks
            MeleeAttack     = new Attack { Name = "Invalid", DamageType = ItemDamageType.Physical, DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f };
            SpellAttack     = new Attack { Name = "Invalid", DamageType = ItemDamageType.Arcane,   DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f }; 
            SpecialAttack_1 = new Attack { Name = "Invalid", DamageType = ItemDamageType.Arcane,   DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f };
            SpecialAttack_2 = new Attack { Name = "Invalid", DamageType = ItemDamageType.Arcane,   DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f };
            SpecialAttack_3 = new Attack { Name = "Invalid", DamageType = ItemDamageType.Arcane,   DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f };
            SpecialAttack_4 = new Attack { Name = "Invalid", DamageType = ItemDamageType.Arcane,   DamagePerHit = 0f, MaxNumTargets = 1f, AttackSpeed = 2f };
            // Situational Changes
            InBackPerc_Melee   = 0.00f; // Default to never in back
            InBackPerc_Ranged  = 0.00f; // Default to never in back
            MultiTargsPerc     = 0.00f; // Default to 0% multiple targets
            MaxNumTargets      =    1f; // Default to max 2 targets (though at 0%, this means nothing)
            StunningTargsFreq  =    0f; // Default to never stunning
            StunningTargsDur   = 5000f; // Default to stun durations of 5 seconds but since it's 0 stuns over dur, this means nothing
            MovingTargsTime    =    0f; // Default to not moving at all during fight
            DisarmingTargsPerc = 0.00f; // None of the bosses disarm
        }

        #region Variables
        // Basics
        private string NAME,CONTENT,INSTANCE,VERSION;
        private float HEALTH,ARMOR;
        private int BERSERKTIMER,LEVEL;
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
                      DISARMINGTARGSPERC;
        #endregion

        #region Get/Set
        // ==== Basics ====
        public string Name               { get { return NAME;               } set { NAME               = value; } }
        public string Content            { get { return CONTENT;            } set { CONTENT            = value; } }
        public string Instance           { get { return INSTANCE;           } set { INSTANCE           = value; } }
        public string Version            { get { return VERSION;            } set { VERSION            = value; } }
        public int    Level              { get { return LEVEL;              } set { LEVEL              = value; } }
        public float  Health             { get { return HEALTH;             } set { HEALTH             = value; } }
        public float  Armor              { get { return ARMOR;              } set { ARMOR              = value; } }
        public int    BerserkTimer       { get { return BERSERKTIMER;       } set { BERSERKTIMER       = value; } }
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
        #endregion

        #region Functions
        /// <summary> A handler for Damage Reduction due to Resistance (Physical, Fire, etc). </summary>
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
        /// <summary> A handler for Damage Reduction due to Resistance (Physical, Fire, etc). This is the Set function</summary>
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
            /* TODO:
             */
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
                Name = "Poison Bold Volley",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (/*Initial*/(2625f + 3375f) / 2.0f) + (/*Dot*/((1480f+1720f)/2.0f)*8f/2f),
                MaxNumTargets = 3,
                AttackSpeed = (7.0f+15.0f)/2.0f,
            };
            SpecialAttack_2 = new Attack {
                Name = "Rain of Fire",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (/*Dot*/((1750f+2750f)/2.0f)*6f/2f),
                MaxNumTargets = 10,
                AttackSpeed = (6.0f+18.0f)/2.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.75f;
            // Every 6-18 seconds for 3 seconds she has to be moved to compensate for Rain of Fire
            MovingTargsTime = (BerserkTimer / SpecialAttack_2.AttackSpeed) * (3f);
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
            };
            SpecialAttack_1 = new Attack {
                Name = "Web Spray",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (1750f + 2250f) / 2f,
                MaxNumTargets = 10,
                AttackSpeed = 40.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.90f;
            // 6 second stun every 40 seconds
            StunningTargsFreq = 40f;
            StunningTargsDur = 6000f;
            // 8 Adds every 40 seconds for 8 seconds (only 7300 HP each)
            MultiTargsPerc = ((BerserkTimer / 40f) * 8f) / BerserkTimer;
            MaxNumTargets = 8;
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
            // Every 30 seconds 2 adds will spawn with 100k HP each, simming their life-time to 20 seconds
            MultiTargsPerc = (BerserkTimer / 30f) * (20f) / BerserkTimer;
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
            };
            SpecialAttack_1 = new Attack {
                Name = "Decrepit Fever",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = 3000f / 3f * 21f,
                MaxNumTargets = 1,
                AttackSpeed = 30.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.25f;
            // We are assuming you are using the corner trick so you don't have
            // to dance as much in 10 man
            // Every 90 seconds for 45 seconds you must do the safety dance
            MovingTargsTime = (BerserkTimer / 90f) * 45f;
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
            Health = 2230000f;
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
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Initial 10 seconds to pop first Spore then every 3rd spore
            // after that (90 seconds respawn then 10 sec moving to/back)
            MovingTargsTime = 10 + (BerserkTimer / 90) * 10;
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
            // Situational Changes
            InBackPerc_Melee = 0.95f;
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
            };
            SpecialAttack_1 = new Attack {
                Name = "Shadowbolt",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2880f + 3520f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 1.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 0.95f;
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
            // Situational Changes
            InBackPerc_Melee = 0.75f;
            // Swap 1st 2 mobs once: 15
            // Get to the back once: 10
            // Bounce back and forth in the back: Every 30 sec for 10 sec but for only 40% of the fight
            MovingTargsTime = 15f + 10f + ((BerserkTimer * 0.40f) / 30f) * 10f;
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
            };
            SpecialAttack_1 = new Attack {
                Name = "Hateful Strike",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (19975f + 27025f) / 2f,
                MaxNumTargets = 1f,
                AttackSpeed = 1.0f,
            };
            // Situational Changes
            InBackPerc_Melee = 1.00f;
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
            // Phase 2 & 3, gotta move out of Shadow Fissures periodically
            // We're assuming they pop every 30 seconds and you have to be
            // moved for 6 seconds and there's a 1/10 chance he will select
            // you over someone e;se
            MovingTargsTime += (((BerserkTimer - MovingTargsTime) / 30f) * 6f) * 0.10f;
            /* TODO:
             * The Mobs in Phase 1
             */
        }
    }
#if FALSE
    // ===== The Obsidian Sanctum =====================
    public class AnubRekhan_10 : BossHandler {
        public AnubRekhan_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Anub'Rekhan (10 Man)";
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
            /* TODO:
             */
        }
    }
    public class AnubRekhan_10 : BossHandler {
        public AnubRekhan_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Anub'Rekhan (10 Man)";
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
            /* TODO:
             */
        }
    }
    public class AnubRekhan_10 : BossHandler {
        public AnubRekhan_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Anub'Rekhan (10 Man)";
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
            /* TODO:
             */
        }
    }
    public class AnubRekhan_10 : BossHandler {
        public AnubRekhan_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Anub'Rekhan (10 Man)";
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
            /* TODO:
             */
        }
    }
    // ===== The Vault of Archavon ====================
    public class AnubRekhan_10 : BossHandler {
        public AnubRekhan_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Anub'Rekhan (10 Man)";
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
            /* TODO:
             */
        }
    }
    public class AnubRekhan_10 : BossHandler {
        public AnubRekhan_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Anub'Rekhan (10 Man)";
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
            /* TODO:
             */
        }
    }
    public class AnubRekhan_10 : BossHandler {
        public AnubRekhan_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Anub'Rekhan (10 Man)";
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
            /* TODO:
             */
        }
    }
    // ===== The Eye of Eternity ======================
    public class AnubRekhan_10 : BossHandler {
        public AnubRekhan_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Anub'Rekhan (10 Man)";
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
            /* TODO:
             */
        }
    }
#endif
    #endregion
    #region T7.5 Content
    public class AnubRekhan_25 : AnubRekhan_10 {
        public AnubRekhan_25() {
            // If not listed here use values from 10 man version
            // Basics
            Name = "Anub'Rekhan";
            Content = "T7.5";
            Instance = "Naxxramas";
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
            // Situational Changes
            /* TODO:
             */
        }
    }
    public class Maexxna_25 : Maexxna_10 {
        public Maexxna_25() {
            // If not listed here use values from 10 man version
            // Basics
            Name = "Maexxna";
            Content = "T7.5";
            Instance = "Naxxramas";
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
            /* TODO:
             * Web Wrap
             * Poison Shock
             * Necrotic Poison
             * Frenzy
             */
        }
    }
    public class Patchwerk_25 : Patchwerk_10 {
        public Patchwerk_25() {
            // If not listed here use values from 10 man version
            // Basics
            Name = "Patchwerk";
            Content = "T7.5";
            Instance = "Naxxramas";
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
            /* TODO:
             * Frenzy
             */
        }
    }
    #endregion
    #region T8 Content
    #endregion
    #region T8.5 Content
    #endregion
    #region T9 (10) Content
    #endregion
    #region T9 (10) H Content
    #endregion
    #region T9 (25) Content
    #endregion
    #region T9 (25) H Content
    #endregion
}
