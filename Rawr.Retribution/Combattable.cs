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
        public BaseCombatTable(BossOptions bossOption, Stats stats, Attacktype type)
        {
            Stats = stats;
            Attacktype = type;
            LevelDif = bossOption.Level - 85;
            IsBehind = bossOption.InBack;
        }

        public const float BLOCKED_DAMAGE = .7f;
        public const float GLANCE_DAMAGE = .75f;

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
        public abstract float CritBonus { get; }
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
                            crit = Math.Max(Math.Min(AbilityCritCorr + Stats.PhysicalCrit + StatConversion.NPC_LEVEL_CRIT_MOD[LevelDif], 1f), 0f);
                            break;
                        case Attacktype.Ranged:
                            crit = Math.Max(Math.Min(AbilityCritCorr + Stats.PhysicalCrit + StatConversion.GetRatingFromPhysicalCrit(Stats.RangedCritRating) + StatConversion.NPC_LEVEL_CRIT_MOD[LevelDif], 1f), 0f);
                            break;
                        case Attacktype.Spell:
                            crit = Math.Max(Math.Min(AbilityCritCorr + Stats.SpellCrit + StatConversion.NPC_LEVEL_SPELL_CRIT_MOD[LevelDif], 1f), 0f);
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

        public virtual float CombatTableMultiplier() {
            return ChanceToHit +
                   CritBonus * ChanceToCrit;
        }

        public override string ToString()
        {
            string fmtstring = "Combattable:";
            object[] param = {ChanceToHit, ChanceToCrit, AbilityCritCorr * ChanceToLand, ChanceToMiss, AbilityMissCorr};

            fmtstring += "\n{0,5:00.00%} Hit";
            if (CanCrit)
                fmtstring += "\n{1,5:00.00%} Crit" + (AbilityCritCorr > 0f ? " (Abil Crit: {2:P})" : "");
            if (CanMiss)
                fmtstring += "\n{3,5:00.00%} Miss" + (AbilityMissCorr > 0f ? " (Abil Miss:-{4:P})" : "");

            return string.Format(fmtstring, param);
        }
    }

    public abstract class BasePhysicalCombatTable : BaseCombatTable
    {
        public BasePhysicalCombatTable(BossOptions bossOption, Stats stats, Attacktype type) : base(bossOption, stats, type) { }
        
        public override float CritBonus { get { return 2f * (1f + Stats.BonusCritDamageMultiplier); } }
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

        public override float CombatTableMultiplier() {
                return base.CombatTableMultiplier() *
                       (1f - BLOCKED_DAMAGE * ChanceToBlock);
        }

        public override string ToString()
        {
            string fmtstring = base.ToString();
            object[] param = { ChanceToDodge, ChanceToParry, ChanceToBlock};

            if (CanDodge)
                fmtstring += "\n{0,5:00.00%} Dodge";
            if (CanParry)
                fmtstring += "\n{1,5:00.00%} Parry";
            if (CanBlock)
                fmtstring += "\n{2,5:00.00%} Blocked Attacks";
                
            return string.Format(fmtstring, param);
        }
    }

    public class BasePhysicalWhiteCombatTable : BasePhysicalCombatTable
    {
        public BasePhysicalWhiteCombatTable(BossOptions bossOption, Stats stats, Attacktype type) : base(bossOption, stats, type) { }

        public float ChanceToGlance { get { return StatConversion.WHITE_GLANCE_CHANCE_CAP[LevelDif]; }}
        public override float ChanceToHit
        {
            get { return Math.Max(1f - ChanceToGlance - ChanceToBlock - ChanceToDodge - ChanceToParry - ChanceToMiss - ChanceToCrit, 0f); }
        }
        public override float ChanceToCrit
        {
            get { return (base.ChanceToCrit / ChanceToLand) * (ChanceToLand - ChanceToBlock); }
        }

        public override float CombatTableMultiplier() {
                // White attacks can only be blocked or glanced
                return base.CombatTableMultiplier() / (1f - BLOCKED_DAMAGE * ChanceToBlock) + 
                       (BLOCKED_DAMAGE * ChanceToBlock) +
                       (GLANCE_DAMAGE * ChanceToGlance);
        }

        public override string ToString()
        {
            string fmtstring = base.ToString();
            int pos = fmtstring.IndexOf("% Hit") + 5;
            fmtstring = fmtstring.Insert(pos, "\n{0,5:00.00%} Glance");

            return string.Format(fmtstring, ChanceToGlance);
        }
    }

    public class BasePhysicalYellowCombatTable : BasePhysicalCombatTable
    {
        public BasePhysicalYellowCombatTable(BossOptions bossOption, Stats stats, Attacktype type) : base(bossOption, stats, type) { }

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
        public BaseSpellCombatTable(BossOptions bossOption, Stats stats, Attacktype type) : base(bossOption, stats, type) { }

        public override float CritBonus { get { return 1.5f * (1f + Stats.BonusSpellCritDamageMultiplier); } }
        public override float ChanceToHit
        {
            get { return Math.Max(1f - ChanceToMiss - ChanceToCrit, 0f); }
        }
    }
}