using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public enum Attacktype
    {
        MeleeMH = 0,
        MeleeOH,
        Ranged,
        Spell
    }

    public abstract class BaseCombatTable
    {
        public Stats Stats { get; set; }
        public int LevelDif { get; set; }
        public bool IsBehind { get; set; }
        public Attacktype Attacktype { get; set; }

        public bool CanMiss = true;
        public float AbilityMissCorr = 0f;
        public virtual float ChanceToMiss
        {
            get
            {
                if (!CanMiss)
                    return 0f;
                else
                    switch (Attacktype)
                    {
                        case Attacktype.MeleeMH:
                            return Math.Max(AbilityMissCorr + StatConversion.WHITE_MISS_CHANCE_CAP[LevelDif] - Stats.PhysicalHit, 0f);
                        case Attacktype.MeleeOH:
                            return Math.Max(AbilityMissCorr + StatConversion.WHITE_MISS_CHANCE_CAP_DW[LevelDif] - Stats.PhysicalHit, 0f);
                        case Attacktype.Ranged:
                            return Math.Max(AbilityMissCorr + StatConversion.WHITE_MISS_CHANCE_CAP[LevelDif]  - Stats.PhysicalHit - 
                                            StatConversion.GetHitFromRating(Stats.RangedHitRating), 0f);
                        case Attacktype.Spell:
                            return Math.Max(AbilityMissCorr + StatConversion.GetSpellMiss(-1 * LevelDif, false) - Stats.SpellHit, 0f);
                        default:
                            return 0f;
                    }
            }
        }

        public bool CanCrit = true;
        public float AbilityCritCorr = 0f;
        public virtual float ChanceToCrit { 
            get {
                if (!CanCrit)
                    return 0f;
                else
                {
                    float crit;
                    switch (Attacktype)
                    {
                        case Attacktype.MeleeMH: case Attacktype.MeleeOH:
                            crit = Math.Min(AbilityCritCorr + Stats.PhysicalCrit + StatConversion.NPC_LEVEL_CRIT_MOD[LevelDif], 1f);
                            break;
                        case Attacktype.Ranged:
                            crit = Math.Min(AbilityCritCorr + Stats.PhysicalCrit + StatConversion.GetRatingFromPhysicalCrit(Stats.RangedCritRating) + StatConversion.NPC_LEVEL_CRIT_MOD[LevelDif], 1f);
                            break;
                        case Attacktype.Spell:
                            crit = Math.Min(AbilityCritCorr + Stats.SpellCrit + StatConversion.NPC_LEVEL_SPELL_CRIT_MOD[LevelDif], 1f);
                            break;
                        default:
                            return 0f;
                    }
                    return crit * ChanceToLand;
                }
            }
        }

        public abstract float ChanceToHit { get; }

        public virtual float ChanceToLand 
        { 
            get { return Math.Max(1f - ChanceToMiss, 0f); }
        }
        
        public BaseCombatTable (Stats stats, Attacktype type)
        {
            Stats = stats;
            Attacktype = type;
            LevelDif = 3;
            IsBehind = true;
        }
    }

    public abstract class BasePhysicalCombatTable : BaseCombatTable
    {
        public BasePhysicalCombatTable(Stats stats, Attacktype type) : base(stats, type) { }

        public virtual bool CanDodge
        {
            get { return CanMiss && Attacktype != Attacktype.Ranged; }
        }
        public virtual float ChanceToDodge
        {
            get { return CanDodge ? Math.Max(StatConversion.WHITE_DODGE_CHANCE_CAP[LevelDif] - StatConversion.GetDodgeParryReducFromExpertise(Stats.Expertise), 0f) : 0f; }
        }
        public virtual bool CanParry
        {
            get { return CanMiss && !IsBehind && Attacktype != Attacktype.Ranged; }
        }
        public virtual float ChanceToParry
        {
            get { return CanParry ? Math.Max(StatConversion.WHITE_PARRY_CHANCE_CAP[LevelDif] - StatConversion.GetDodgeParryReducFromExpertise(Stats.Expertise), 0f) : 0f; }
        }
        public virtual bool CanBlock
        {
            get { return !IsBehind; }
        }
        public virtual float ChanceToBlock
        {
            get { return CanBlock ? StatConversion.WHITE_BLOCK_CHANCE_CAP[LevelDif] : 0f; }
        }
        public override float ChanceToLand
        {
            get { return Math.Max(base.ChanceToLand - ChanceToDodge - ChanceToParry, 0f); }
        }
    }

    public class BasePhysicalWhiteCombatTable : BasePhysicalCombatTable
    {
        public BasePhysicalWhiteCombatTable(Stats stats, Attacktype type) : base(stats, type)
        {
            _glanceChance = StatConversion.WHITE_GLANCE_CHANCE_CAP[LevelDif];
        }

        public float _glanceChance;
        public override float ChanceToHit
        {
            get { return Math.Max(1f - _glanceChance - ChanceToBlock - ChanceToDodge - ChanceToParry - ChanceToMiss - ChanceToCrit, 0f); }
        }
        public override float ChanceToCrit
        {
            get { return (base.ChanceToCrit / ChanceToLand) * (ChanceToLand - ChanceToBlock); }
        }
    }

    public class BasePhysicalYellowCombatTable : BasePhysicalCombatTable
    {
        public BasePhysicalYellowCombatTable(Stats stats, Attacktype type) : base(stats, type) { }

        public override float ChanceToBlock
        {
            get { return ChanceToLand * base.ChanceToBlock; }
        }
        public override float ChanceToHit
        {
            get { return Math.Max(ChanceToLand - ChanceToCrit, 0f); }
        }
    }

    public class BaseSpellCombatTable : BaseCombatTable
    {
        public BaseSpellCombatTable(Stats stats, Attacktype type) : base(stats, type) { }

        public override float ChanceToHit
        {
            get { return Math.Max(1f - ChanceToMiss - ChanceToCrit, 0f); }
        }
    }
}