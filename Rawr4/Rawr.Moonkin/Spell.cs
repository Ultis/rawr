using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{
    public enum SpellSchool
    {
        Arcane,
        Nature,
        Spellstorm
    }
    public class Spell
    {
        public Spell() { AllDamageModifier = 1f; CriticalDamageModifier = 1.5f; }
        public Spell(Spell copy)
        {
            this.Name = copy.Name;
            this.School = copy.School == SpellSchool.Arcane ? SpellSchool.Arcane : SpellSchool.Nature;
            this.BaseCastTime = copy.BaseCastTime;
            this.BaseDamage = copy.BaseDamage;
            this.AllDamageModifier = copy.AllDamageModifier;
            this.BaseManaCost = copy.BaseManaCost;
            this.CriticalChanceModifier = copy.CriticalChanceModifier;
            this.CriticalDamageModifier = copy.CriticalDamageModifier;
            this.DotEffect = copy.DotEffect == null ? null : new DotEffect(copy.DotEffect);
            this.SpellDamageModifier = copy.SpellDamageModifier;
            this.BaseEnergy = copy.BaseEnergy;
        }
        public string Name { get; set; }
        public float BaseDamage { get; set; }
        public SpellSchool School { get; set; }
        public float SpellDamageModifier { get; set; }
        public float AllDamageModifier { get; set; }
        public float CriticalDamageModifier { get; set; }
        public float CriticalChanceModifier { get; set; }
        public float BaseCastTime { get; set; }
        public float BaseManaCost { get; set; }
        public DotEffect DotEffect { get; set; }
        public float BaseEnergy { get; set; }
        // Section for variables which get filled in during rotation calcs
        public float DamagePerHit { get; set; }
        public float CastTime { get; set; }
        public float AverageEnergy { get; set; }
    }
    public class DotEffect
    {
        public DotEffect() { AllDamageModifier = 1f;  }
        public DotEffect(DotEffect copy)
        {
            this.AllDamageModifier = copy.AllDamageModifier;
            this.Duration = copy.Duration;
            this.BaseDuration = copy.BaseDuration;
            this.TickDamage = copy.TickDamage;
            this.BaseTickLength = copy.BaseTickLength;
            this.TickLength = copy.TickLength;
            this.SpellDamageModifierPerTick = copy.SpellDamageModifierPerTick;
        }
        public float BaseDuration { get; set; }
        public float Duration { get; set; }
        public float BaseTickLength { get; set; }
        public float TickLength { get; set; }
        public float TickDamage { get; set; }
        public float SpellDamageModifier
        {
            get
            {
                return SpellDamageModifierPerTick * Duration / BaseTickLength;
            }
            set
            {
                SpellDamageModifierPerTick += value / Duration / BaseTickLength;
            }
        }
        public float AllDamageModifier { get; set; }
        public float NumberOfTicks { get; set; }
        public float SpellDamageModifierPerTick { get; set; }
        // Section for variables which get filled in during rotation calcs
        public float DamagePerHit { get; set; }
    }
}
