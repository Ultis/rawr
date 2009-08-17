using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr {
    public class BossHandler {
        #region Subclasses
        /// <summary>Enumerator for simplifaction of Damage type matching</summary>
        public enum DAMAGETYPES {
            PHYSICAL=0,
            SPELL,
            ARCANE,
            FROST,
            FIRE,
            SHADOW,
            HOLY,
        }
        /// <summary>Enumerator for creating a list of possible values for the Level box</summary>
        public enum POSSIBLE_LEVELS {
            NPC_80=80,
            NPC_81,
            NPC_82,
            NPC_83,
        }
        /// <summary>Enumerator for creating a list of possible values for the Armor box</summary>
        /*public enum POSSIBLE_ARMORS {
            NPC_80 = (int)StatConversion.NPC_ARMOR[80],
            NPC_81 = (int)StatConversion.NPC_ARMOR[81],
            NPC_82 = (int)StatConversion.NPC_ARMOR[82],
            BOSS   = (int)StatConversion.NPC_ARMOR[83],
            CUSTOM = 10000,
        }*/
        /// <summary>A single Attack of various types</summary>
        public struct Attack {
            /// <summary>The type of damage done, use the DAMAGETYPES enumerator to select</summary>
            public DAMAGETYPES DamageType;
            /// <summary>The Unmitigated Damage per Hit for this attack</summary>
            public float DamagePerHit;
            /// <summary>The maximum number of party/raid members this attack can hit</summary>
            public float MaxNumTargets;
            /// <summary>The frequency of this attack (in seconds)</summary>
            public float AttackSpeed;
        }
        #endregion

        #region Constructors
        public BossHandler() {
            // Basics
            Name = "Generic";
            BerserkTimer = 15f * 60f; // The longest noted Enrage timer is 15 minutes, and seriously, if the fight is taking that long, then fail... just fail.
            Level = 83;
            Health = 1000000f;
            Armor = (float)StatConversion.NPC_ARMOR[Level];
            UseParryHaste = false;
            // Resistance
            RESISTANCE_PHYSICAL = 0f;
            RESISTANCE_SPELL = 0f;
            RESISTANCE_ARCANE = 0f;
            RESISTANCE_FROST = 0f;
            RESISTANCE_FIRE = 0f;
            RESISTANCE_SHADOW = 0f;
            RESISTANCE_HOLY = 0f;
            // Attacks
            MeleeAttack     = new Attack { DamageType = DAMAGETYPES.PHYSICAL, DamagePerHit = 0f, MaxNumTargets = 0f, AttackSpeed = 0f };
            SpellAttack     = new Attack { DamageType = DAMAGETYPES.SPELL   , DamagePerHit = 0f, MaxNumTargets = 0f, AttackSpeed = 0f }; 
            SpecialAttack_1 = new Attack { DamageType = DAMAGETYPES.FROST   , DamagePerHit = 0f, MaxNumTargets = 0f, AttackSpeed = 0f };
            SpecialAttack_2 = new Attack { DamageType = DAMAGETYPES.FROST   , DamagePerHit = 0f, MaxNumTargets = 0f, AttackSpeed = 0f };
            SpecialAttack_3 = new Attack { DamageType = DAMAGETYPES.FROST   , DamagePerHit = 0f, MaxNumTargets = 0f, AttackSpeed = 0f };
            SpecialAttack_4 = new Attack { DamageType = DAMAGETYPES.FROST   , DamagePerHit = 0f, MaxNumTargets = 0f, AttackSpeed = 0f };
            // Situational Changes
        }
        #endregion

        #region Variables
        // Basics
        private string NAME;
        private float BERSERKTIMER,HEALTH,ARMOR;
        private int LEVEL;
        private bool USERPARRYHASTE;
        // Resistance
        private float RESISTANCE_PHYSICAL,RESISTANCE_SPELL,RESISTANCE_ARCANE,RESISTANCE_FROST,RESISTANCE_FIRE,RESISTANCE_SHADOW,RESISTANCE_HOLY;
        // Attacks
        private Attack MELEEATTACK,SPELLATTACK,SPECIALATTACK_1,SPECIALATTACK_2,SPECIALATTACK_3,SPECIALATTACK_4;
        // Situational Changes
        private float MULTITARGSPERC,MAXNUMTARGS,MOVINGTARGSPERC,STUNNINGTARGSPERC,DISARMINGTARGSPERC;
        #endregion

        #region Get/Set
        public string Name               { get { return NAME;               } set { NAME               = value; } }
        public int    Level              { get { return LEVEL;              } set { LEVEL              = value; } }
        public float  Health             { get { return HEALTH;             } set { HEALTH             = value; } }
        public float  Armor              { get { return ARMOR;              } set { ARMOR              = value; } }
        public float  BerserkTimer       { get { return BERSERKTIMER;       } set { BERSERKTIMER       = value; } }

        public Attack MeleeAttack        { get { return MELEEATTACK;        } set { MELEEATTACK        = value; } }
        public Attack SpellAttack        { get { return SPELLATTACK;        } set { SPELLATTACK        = value; } }
        public Attack SpecialAttack_1    { get { return SPECIALATTACK_1;    } set { SPECIALATTACK_1    = value; } }
        public Attack SpecialAttack_2    { get { return SPECIALATTACK_2;    } set { SPECIALATTACK_2    = value; } }
        public Attack SpecialAttack_3    { get { return SPECIALATTACK_3;    } set { SPECIALATTACK_3    = value; } }
        public Attack SpecialAttack_4    { get { return SPECIALATTACK_4;    } set { SPECIALATTACK_4    = value; } }

        public float  MultiTargsPerc     { get { return MULTITARGSPERC;     } set { MULTITARGSPERC     = value; } }
        public float  MaxNumTargets      { get { return MAXNUMTARGS;        } set { MAXNUMTARGS        = value; } }
        public float  MovingTargsPerc    { get { return MOVINGTARGSPERC;    } set { MOVINGTARGSPERC    = value; } }
        public float  StunningTargsPerc  { get { return STUNNINGTARGSPERC;  } set { STUNNINGTARGSPERC  = value; } }
        public float  DisarmingTargsPerc { get { return DISARMINGTARGSPERC; } set { DISARMINGTARGSPERC = value; } }

        public bool   UseParryHaste      { get { return USERPARRYHASTE;     } set { USERPARRYHASTE     = value; } }
        #endregion

        #region Functions
        /// <summary> A handler for Damage Reduction due to Resistance (Physical, Fire, etc). </summary>
        /// <returns>The Percentage of Damage to be removed (0.25 = 25% Damage Reduced, 100 Damage should become 75)</returns>
        public float Resistance(DAMAGETYPES type){
            switch (type) {
                case DAMAGETYPES.PHYSICAL   : return RESISTANCE_PHYSICAL;
                case DAMAGETYPES.SPELL      : return RESISTANCE_SPELL;
                case DAMAGETYPES.ARCANE     : return RESISTANCE_ARCANE;
                case DAMAGETYPES.FROST      : return RESISTANCE_FROST;
                case DAMAGETYPES.FIRE       : return RESISTANCE_FIRE;
                case DAMAGETYPES.SHADOW     : return RESISTANCE_SHADOW;
                case DAMAGETYPES.HOLY       : return RESISTANCE_HOLY;
                default: break;
            }
            return 0f;
        }
        #endregion
    }
    public class Patchwerk : BossHandler {
        Patchwerk() {
            Name = "Patchwerk";
            MeleeAttack = new Attack { DamageType = DAMAGETYPES.PHYSICAL, DamagePerHit = 60000f, MaxNumTargets = 1f, AttackSpeed = 2.0f };
        }
    }
}
