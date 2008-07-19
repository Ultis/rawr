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
            this.variableType = Calculations.SolutionVariable[index].Type;
            this.Duration = duration;
            this.spell = Calculations.SolutionVariable[index].Spell;
            this.castingState = Calculations.SolutionVariable[index].State;
            this.segment = Calculations.SolutionVariable[index].Segment;
            if (castingState == null) castingState = Calculations.BaseState;

            minTime = 0;
            maxTime = Calculations.CalculationOptions.FightDuration;

            if (variableType == VariableType.IdleRegen)
            {
                mps = -(Calculations.BaseState.ManaRegen * (1 - Calculations.CalculationOptions.Fragmentation) + Calculations.BaseState.ManaRegen5SR * Calculations.CalculationOptions.Fragmentation);
            }
            else if (variableType == VariableType.Wand)
            {
                spell = Calculations.BaseState.GetSpell(SpellId.Wand);
                mps = spell.CostPerSecond - spell.ManaRegenPerSecond;
            }
            else if (variableType == VariableType.Evocation || variableType == VariableType.ManaPotion || variableType == VariableType.ManaGem)
            {
                mps = 0;
            }
            else if (variableType == VariableType.DrumsOfBattle)
            {
                mps = -Calculations.BaseState.ManaRegen5SR;
            }
            else if (variableType == VariableType.Drinking)
            {
                maxTime = 0;
                mps = -Calculations.BaseState.ManaRegenDrinking;
            }
            else if (variableType == VariableType.TimeExtension)
            {
                minTime = maxTime;
            }
            else if (variableType == VariableType.AfterFightRegen)
            {
                mps = -Calculations.BaseState.ManaRegenDrinking;
                minTime = maxTime;
            }
            else if (variableType == VariableType.Spell)
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

        private VariableType variableType;
        public VariableType VariableType
        {
            get
            {
                return variableType;
            }
        }

        public bool IsManaPotionOrGem
        {
            get
            {
                return variableType == VariableType.ManaPotion || variableType == VariableType.ManaGem;
            }
        }

        public bool IsEvocation
        {
            get
            {
                return variableType == VariableType.Evocation;
            }
        }

        private int segment;
        public int Segment
        {
            get
            {
                return segment;
            }
            set
            {
                segment = value;
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
