using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage.SequenceReconstruction
{
    public class CooldownConstraint
    {
        public SequenceGroup Group;
        public double Cooldown;
        public double Duration;
        public bool ColdSnap;
    }

    public class SequenceItem : ICloneable
    {
        public static CharacterCalculationsMage Calculations;

        private SequenceItem() { }
        public SequenceItem(int index, double duration) : this(index, duration, null) { }

        public SequenceItem(int index, double duration, List<SequenceGroup> group)
        {
            if (group == null) group = new List<SequenceGroup>();
            this.Group = group;
            this.index = index;
            this.Duration = duration;
            this.spell = Calculations.SolutionVariable[index].Spell;
            this.castingState = Calculations.SolutionVariable[index].State;
            this.segment = Calculations.SolutionVariable[index].Segment;
            if (castingState == null) castingState = Calculations.BaseState;

            minTime = 0;
            maxTime = Calculations.CalculationOptions.FightDuration;

            if (index == Calculations.ColumnIdleRegen)
            {
                mps = -Calculations.BaseState.ManaRegen;
            }
            else if (index == Calculations.ColumnWand)
            {
                spell = Calculations.BaseState.GetSpell(SpellId.Wand);
                mps = spell.CostPerSecond - spell.ManaRegenPerSecond;
            }
            else if (index == Calculations.ColumnEvocation || index == Calculations.ColumnManaPotion || index == Calculations.ColumnManaGem)
            {
                mps = 0;
            }
            else if (index == Calculations.ColumnDrumsOfBattle)
            {
                mps = -Calculations.BaseState.ManaRegen5SR;
            }
            else if (index == Calculations.ColumnDrinking)
            {
                maxTime = 0;
                mps = -Calculations.BaseState.ManaRegenDrinking;
            }
            else if (index == Calculations.ColumnTimeExtension)
            {
                minTime = maxTime;
            }
            else if (index == Calculations.ColumnAfterFightRegen)
            {
                mps = -Calculations.BaseState.ManaRegenDrinking;
                minTime = maxTime;
            }
            else
            {
                mps = spell.CostPerSecond - spell.ManaRegenPerSecond;
            }
        }

        private int index;
        public int Index
        {
            get
            {
                return index;
            }
        }

        public bool IsManaPotionOrGem
        {
            get
            {
                return index == Calculations.ColumnManaPotion || index == Calculations.ColumnManaGem;
            }
        }

        public bool IsEvocation
        {
            get
            {
                return index == Calculations.ColumnEvocation;
            }
        }

        private int segment;
        public int Segment
        {
            get
            {
                return segment;
            }
        }

        public double Duration;
        public double Timestamp;

        private double minTime;
        public double MinTime
        {
            get
            {
                return minTime;
            }
            set
            {
                minTime = Math.Max(minTime, value);
            }
        }

        private double maxTime;
        public double MaxTime
        {
            get
            {
                return maxTime;
            }
            set
            {
                maxTime = Math.Min(maxTime, value);
            }
        }

        public List<SequenceGroup> Group;
        public SequenceGroup SuperGroup;

        // helper variables
        public int SuperIndex;
        public List<SequenceGroup> Tail;
        public int CooldownHex;
        public int OrderIndex;

        private Spell spell;
        public Spell Spell
        {
            get
            {
                return spell;
            }
        }

        private CastingState castingState;
        public CastingState CastingState
        {
            get
            {
                return castingState;
            }
        }

        private double mps;
        public double Mps
        {
            get
            {
                return mps;
            }
        }

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        public SequenceItem Clone()
        {
            SequenceItem clone = (SequenceItem)MemberwiseClone();
            clone.Group = new List<SequenceGroup>(Group);
            return clone;
        }

        #endregion

        public override string ToString()
        {
            if (spell == null) return index.ToString();
            return castingState.BuffLabel + "+" + spell.Name;
        }
    }
}
