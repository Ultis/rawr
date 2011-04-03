using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
    public abstract class CombatTable {
        public static CombatTable NULL = new NullCombatTable();
        protected Character Char;
        protected BossOptions bossOpts;
        protected CombatFactors combatFactors;
        protected Base.StatsWarrior StatS;
        protected Skills.Ability Abil;
        protected bool useSpellHit = false;
        protected bool useRangedHit = false;

        public bool isWhite;
        public bool isMH;

        /// <summary>The Level Difference between you and the Target. Ranges from 0 to +3 (Cata: 85-88)</summary>
        public int LevelDif { get { return bossOpts.Level - Char.Level; } }

        public float Miss { get; protected set; }
        public float Dodge { get; protected set; }
        public float Parry { get; protected set; }
        public float Block { get; protected set; }
        public float Glance { get; protected set; }
        public float Crit { get; protected set; }
        public float Hit { get; protected set; }

        private float _anyLand = 0f;
        private float _anyNotLand = 1f;
        /// <summary>Any attack that lands: 1f - AnyNotLand</summary>
        public float AnyLand { get { return _anyLand; } }
        /// <summary>Any attack that does not land: Dodges, Parries or Misses</summary>
        public float AnyNotLand { get { return _anyNotLand; } }
        private bool _alwaysHit = false;

        protected virtual void Calculate() {
            _anyNotLand = Dodge + Parry + Miss;
            _anyLand = 1f - _anyNotLand;
        }
        protected virtual void CalculateAlwaysHit()
        {
            _alwaysHit = true;
            Miss = Dodge = Parry = Block = Glance = Crit = 0f;
            Hit = 1f;
            _anyLand = 1f;
            _anyNotLand = 0f;
        }

        protected void Initialize(Character character, Base.StatsWarrior stats, CombatFactors cf, BossOptions bo,
            Skills.Ability ability, bool ismh, bool usespellhit, bool userangedhit, bool alwaysHit)
        {
            Char = character;
            StatS = stats;
            bossOpts = bo;
            combatFactors = cf;
            Abil = ability;
            isWhite = (Abil == null);
            isMH = ismh;
            useSpellHit = usespellhit;
            useRangedHit = userangedhit;
            // Start a calc
            Reset(alwaysHit);
        }
        protected void Reset(bool alwaysHit)
        {
            if (alwaysHit) CalculateAlwaysHit();
            else Calculate();
        }
        public void Reset()
        {
            if (_alwaysHit) return;
            Reset(false);
        }
    }

    public class NullCombatTable : CombatTable
    {
        public NullCombatTable()
        {
            Block = Crit = Hit = Dodge = Glance = Miss = Parry = 0;
        }
    }
    public class AttackTable : CombatTable {
        protected override void Calculate() {
            float tableSize = 0f;

            // Miss
            if (useSpellHit) {
                Miss = Math.Min(1f - tableSize, Math.Max(StatConversion.GetSpellMiss(LevelDif,false)
                    - (StatConversion.GetSpellHitFromRating(StatS.HitRating, Char.Class) + StatS.SpellHit), 0f));
            } else {
                Miss = Math.Min(1f - tableSize, isWhite ? combatFactors.CWmiss : combatFactors.CYmiss);
            }
            tableSize += Miss;
            // Dodge
            if (!useRangedHit && (isWhite || Abil.CanBeDodged)) {
                Dodge = Math.Min(1f - tableSize, isMH ? combatFactors.CMHdodge : combatFactors.COhdodge);
                tableSize += Dodge;
            } else { Dodge = 0f; }
            // Parry
            if (!useRangedHit && (isWhite || Abil.CanBeParried)) {
                Parry = Math.Min(1f - tableSize, isMH ? combatFactors.CMHparry : combatFactors.COhparry);
                tableSize += Parry;
            } else { Parry = 0f; }
            // Block
            if (!useRangedHit && (isWhite || Abil.CanBeBlocked)) {
                Block = Math.Min(1f - tableSize, isMH ?  combatFactors.CMHblock : combatFactors.COhblock);
                tableSize += Block;
            } else { Block = 0f; }
            // Glancing Blow
            if (!useRangedHit && isWhite) {
                Glance = Math.Min(1f - tableSize, combatFactors.CGlance);
                tableSize += Glance;
            } else { Glance = 0f; }
            // Critical Hit
            Crit = 0;
            if (isWhite) {
                float critValueToUse = (isMH ? combatFactors.CMHwcrit : combatFactors.COhwcrit)
                                            + StatConversion.NPC_LEVEL_CRIT_MOD[LevelDif];
                foreach (WeightedStat ws in combatFactors.CritProcs)
                {
                    float modCritChance = Math.Min(1f - tableSize, critValueToUse + StatConversion.GetCritFromRating(ws.Value, Char.Class));
                    Crit += ws.Chance * modCritChance;
                }
                tableSize += Crit;
            } else if (Abil.CanCrit) {
                float critValueToUse = StatConversion.NPC_LEVEL_CRIT_MOD[LevelDif]
                    + (isMH ? combatFactors.CMHycrit : combatFactors.COhycrit)
                    + Abil.BonusCritChance;
                foreach (WeightedStat ws in combatFactors.CritProcs)
                {
                    float modCritChance = Math.Min(1f - tableSize, (critValueToUse + StatConversion.GetCritFromRating(ws.Value, Char.Class)) * (1f - Dodge - Miss));
                    Crit += ws.Chance * modCritChance;
                }
                tableSize += Crit;
            }
            // Normal Hit
            Hit = Math.Max(0f, 1f - tableSize);
            base.Calculate();
        }

        public AttackTable() { }

        public AttackTable(Character character, Base.StatsWarrior stats, CombatFactors cf, BossOptions bo, bool ismh, bool useSpellHit, bool useRangedHit, bool alwaysHit)
        {
            Initialize(character, stats, cf, bo, null, ismh, useSpellHit, useRangedHit, alwaysHit);
        }

        public AttackTable(Character character, Base.StatsWarrior stats, CombatFactors cf, BossOptions bo, Skills.Ability ability, bool ismh, bool useSpellHit, bool useRangedHit, bool alwaysHit)
        {
            Initialize(character, stats, cf, bo, ability, ismh, useSpellHit, useRangedHit, alwaysHit);
        }
    }
}
