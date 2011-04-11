using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.HealPriest
{
    public class ResultDamage
    {
    }

    public class ResultHeal
    {
    }

    public abstract class SpellCata
    {
        protected Character character;
        protected Stats stats;

        public bool IsInstant { get; protected set; }
        public bool HasDirectDamage { get; protected set; }
        public bool HasOverTimeDamage { get; protected set; }
        public bool HasDirectHeal { get; protected set; }
        public bool HasOverTimeHeal { get; protected set; }
        public bool HasAbsorb { get; protected set; }

        public int MaxTargets { get; protected set; }
        public int Targets { get; protected set; }

        public readonly float BaseScalar85 = 945.188842773438f;
        
        public float BaseDirectValue { get; protected set; }
        public float BaseDirectVariation { get; protected set; }
        public float BaseDirectCoefficient { get; protected set; }

        public float BaseOverTimeValue { get; protected set; }
        public float BaseOverTimeCoefficient { get; protected set; }
        public float BaseOverTimeTickPeriod { get; protected set; }
        public float BaseOverTimeTicks { get; protected set; }

        public float BaseManaCost { get; protected set; }
        public float ManaCost { get; protected set; }

        public float BaseCastTime { get; protected set; }
        public float CastTime { get; protected set; }
        public readonly float BaseGlobalCooldown = 1.5f;
        public float GlobalCooldown { get; protected set; }
        public float CritChance { get; protected set; }
        public float CritMultiplier { get; protected set; }

        public virtual void SetPriestInformation(Character character, Stats stats) { this.character = character; this.stats = stats;  UpdateSpell(); }
        
        public virtual void UpdateSpell() {
            HasDirectDamage = false; HasDirectHeal = false; HasAbsorb = false; HasOverTimeDamage = false; HasOverTimeHeal = false;

            float hc = 1f / (1f + stats.SpellHaste);
            IsInstant = BaseCastTime == 0f;
            if (IsInstant)
                CastTime = 0f;
            else
                CastTime = Math.Max(1f, BaseCastTime * hc);
            GlobalCooldown = Math.Max(1f, BaseGlobalCooldown * hc);
            CritChance = stats.SpellCrit;
            CritMultiplier = 1.5f * (1f + stats.BonusCritHealMultiplier);
            ManaCost = BaseManaCost * BaseStats.GetBaseStats(character).Mana; // priestInformation.BaseMana;
            if (IsInstant)
                ManaCost *= PriestInformation.GetMentalAgility(character.PriestTalents.MentalAgility);
        }

        public override string ToString()
        {
            string retval = String.Format("Cast Time: {0} seconds", (IsInstant) ? String.Format("Instant {0}", GlobalCooldown.ToString("0.00")) : CastTime.ToString("0.00"));
            if (ManaCost > 0)
                retval += String.Format("\nMana Cost: {0}", ManaCost.ToString("0"));
            if (CritChance > 0)
                retval += String.Format("\nCrit Chance: {0}%", (CritChance * 100f).ToString("0.00"));
            if (Targets > 0)
                retval += String.Format("\nTargets hit: {0}", Targets);
            if (HasDirectDamage)
                retval += ToStringDirectDamage();
            if (HasOverTimeDamage)
                retval += ToStringOverTimeDamage();
            if (HasDirectHeal)
                retval += ToStringDirectHeal();
            if (HasOverTimeHeal)
                retval += ToStringOverTimeHeal();
            if (HasAbsorb)
                retval += ToStringAbsorb();
            return retval;
        }

        protected virtual string ToStringDirectDamage()
        {
            return String.Empty;
        }

        protected virtual string ToStringOverTimeDamage()
        {
            return String.Empty;
        }

        protected virtual string ToStringDirectHeal()
        {
            return String.Empty;
        }

        protected virtual string ToStringOverTimeHeal()
        {
            return String.Empty;
        }

        protected virtual string ToStringAbsorb()
        {
            return String.Empty;
        }
    }

    public abstract class HealSpell : SpellCata
    {
        protected float DiscMastery = 0f;
        protected float HolyMastery = 0f;
        protected ePriestSpec ps = ePriestSpec.Spec_ERROR;

        public float DirectHealMinHit { get; protected set; }
        public float DirectHealMaxHit { get; protected set; }
        public float DirectHealMinCrit { get; protected set; }
        public float DirectHealMaxCrit { get; protected set; }
        public float DirectHealAvgHit { get; protected set; }
        public float DirectHealAvgCrit { get; protected set; }
        public float DirectHealAvg { get; protected set; }

        public float OverTimeHealHit { get; protected set; }
        public float OverTimeHealCrit { get; protected set; }
        public float OverTimeHealAvg { get; protected set; }
        public float OverTimeTickPeriod { get; protected set; }
        public float OverTimeTicks { get; protected set; }
        public float OverTimeDuration { get; protected set; }

        public float Holy_HoT_Hit { get; protected set; }
        public float Holy_HoT_Crit { get; protected set; }
        public float Holy_HoT_Avg { get; protected set; }
        public float Holy_HoT_TickPeriod { get; protected set; }
        public float Holy_HoT_Ticks { get; protected set; }
        public float Holy_HoT_Duration { get; protected set; }

        public float AbsorbMinHit { get; protected set; }
        public float AbsorbMaxHit { get; protected set; }
        public float AbsorbMinCrit { get; protected set; }
        public float AbsorbMaxCrit { get; protected set; }
        public float AbsorbAvgHit { get; protected set; }
        public float AbsorbAvgCrit { get; protected set; }
        public float AbsorbAvg { get; protected set; }

        public HealSpell() { }

        public virtual float HPS()
        {
            float targets = (Targets > 0) ? Targets : 1f;
            float castTime = (IsInstant) ? GlobalCooldown : CastTime;
            float hot = (OverTimeTickPeriod > 0) ? (OverTimeHealAvg / OverTimeTickPeriod) : 0f;
            float hhot = (Holy_HoT_TickPeriod > 0) ? (Holy_HoT_Avg / Holy_HoT_TickPeriod) : 0f;
            return ((DirectHealAvg + AbsorbAvg) / castTime + hot + hhot) * targets;
        }

        public virtual float HPCT()
        {
            float targets = (Targets > 0) ? Targets : 1f;
            float castTime = (IsInstant) ? GlobalCooldown : CastTime;
            float hot = OverTimeHealAvg * OverTimeTicks;
            float hhot = Holy_HoT_Avg * Holy_HoT_Ticks;
            return ((DirectHealAvg + AbsorbAvg + hot + hhot) / castTime) * targets;
        }

        public virtual float HPM()
        {
            float targets = (Targets > 0) ? Targets : 1f;
            float hot = OverTimeHealAvg * OverTimeTicks;
            float hhot = Holy_HoT_Avg * Holy_HoT_Ticks;
            return ((DirectHealAvg + AbsorbAvg + hot + hhot) / ManaCost) * targets;
        }

        public override void SetPriestInformation(Character character, Stats stats)
        {
            this.character = character;
            this.stats = stats;

            ps = PriestSpec.GetPriestSpec(character.PriestTalents);
            if (ps == ePriestSpec.Spec_Disc)
            {
                DiscMastery = (8f + StatConversion.GetMasteryFromRating(stats.MasteryRating)) * 0.025f;
            }
            else if (ps == ePriestSpec.Spec_Holy)
            {
                HolyMastery = (8f + StatConversion.GetMasteryFromRating(stats.MasteryRating)) * 0.0125f;
            }
            UpdateSpell();
        }

        public override void UpdateSpell()
        {
            base.UpdateSpell();
            DirectHealMinHit = 0; DirectHealMaxHit = 0; DirectHealAvgHit = 0;
            DirectHealMinCrit = 0; DirectHealMaxCrit = 0; DirectHealAvgCrit = 0;
            DirectHealAvg = 0;

            OverTimeHealHit = 0; OverTimeHealCrit = 0; OverTimeHealAvg = 0;
            OverTimeTickPeriod = 0; OverTimeTicks = 0;

            Holy_HoT_Hit = 0; Holy_HoT_Crit = 0; Holy_HoT_Avg = 0;
            Holy_HoT_Duration = 0; Holy_HoT_TickPeriod = 0; Holy_HoT_Ticks = 0;

            AbsorbMinHit = 0; AbsorbMaxHit = 0; AbsorbAvgHit = 0;
            AbsorbMinCrit = 0; AbsorbMaxCrit = 0; AbsorbAvgCrit = 0;
            AbsorbAvg = 0;
        }

        public override string ToString()
        {
            return String.Format("HPS: {0}\nHPCT: {1}\nHPM: {2}\n{3}", HPS().ToString("0"), HPCT().ToString("0"), HPM().ToString("0.00"), base.ToString());
        }


        protected override string ToStringDirectHeal()
        {
            string s = base.ToStringDirectHeal();
            if (DirectHealAvgHit > 0)
            {
                if (DirectHealMinHit == DirectHealMaxHit)
                    s += String.Format("\nHeal Hit: {0}", DirectHealAvgHit.ToString("0"));
                else
                    s += String.Format("\nHeal Hit: {0}-{1}, Avg {2}", DirectHealMinHit.ToString("0"), DirectHealMaxHit.ToString("0"), DirectHealAvgHit.ToString("0"));
            }
            if (DirectHealAvgHit != DirectHealAvgCrit)
            {
                if (DirectHealMinCrit == DirectHealMaxCrit)
                    s += String.Format("\nHeal Crit: {0}", DirectHealAvgCrit.ToString("0"));
                else
                    s += String.Format("\nHeal Crit: {0}-{1}, Avg {2}", DirectHealMinCrit.ToString("0"), DirectHealMaxCrit.ToString("0"), DirectHealAvgCrit.ToString("0"));
            }
            s += String.Format("\nHeal Avg: {0}", DirectHealAvg.ToString("0"));
            return s;
        }

        protected override string ToStringOverTimeHeal()
        {
            string s = base.ToStringOverTimeHeal();
            if (OverTimeHealAvg > 0)
            {
                s += String.Format("\nHoT: Hit {0}", OverTimeHealHit.ToString("0"));
                if (OverTimeHealHit != OverTimeHealCrit)
                {
                    s += String.Format(", Crit {0}, Avg: {1}", OverTimeHealCrit.ToString("0"), OverTimeHealAvg.ToString("0"));
                }
                s += String.Format("\nHoT ticks {0} times every {1} seconds for a total of {2} seconds", OverTimeTicks.ToString("0"), OverTimeTickPeriod.ToString("0"), OverTimeDuration.ToString("0"));
            }
            if (Holy_HoT_Hit > 0)
            {
                s += String.Format("\nHoly HoT: Hit {0}", Holy_HoT_Hit.ToString("0"));
                if (Holy_HoT_Hit != Holy_HoT_Crit)
                {
                    s += String.Format(", Crit {0}, Avg {1}", Holy_HoT_Crit.ToString("0"), Holy_HoT_Avg.ToString("0"));
                }
                s += String.Format("\nHoly HoT ticks {0} times every {1} seconds for a total of {2} seconds", Holy_HoT_Ticks.ToString("0"), Holy_HoT_TickPeriod.ToString("0"), Holy_HoT_Duration.ToString("0"));
            }
            return s;
        }

        protected override string ToStringAbsorb()
        {
            string s = base.ToStringAbsorb();
            if (AbsorbAvgHit > 0)
            {
                if (AbsorbMinHit == AbsorbMaxHit)
                    s += String.Format("\nAbsorb Hit: {0}", AbsorbAvgHit.ToString("0"));
                else
                    s += String.Format("\nAbsorb Hit: {0}-{1}, Avg: {2}", AbsorbMinHit.ToString("0"), AbsorbMaxHit.ToString("0"), AbsorbAvgHit.ToString("0"));
            }
            if (AbsorbAvgHit != AbsorbAvgCrit)
            {
                if (AbsorbMinCrit == AbsorbMaxCrit)
                    s += String.Format("\nAbsorb Crit: {0}", AbsorbAvgCrit.ToString("0"));
                else
                    s += String.Format("\nAbsorb Crit: {0}-{1}, Avg: {2}", AbsorbMinCrit.ToString("0"), AbsorbMaxCrit.ToString("0"), AbsorbAvgCrit.ToString("0"));
            }
            if (AbsorbAvg > 0)
            {
                s += String.Format("\nAbsorb Avg: {0}", AbsorbAvg.ToString("0"));
            }

            return s;
        }
    }

    public abstract class DamageSpell : SpellCata
    {
        public float DirectDamageMinHit { get; protected set; }
        public float DirectDamageMaxHit { get; protected set; }
        public float DirectDamageMinCrit { get; protected set; }
        public float DirectDamageMaxCrit { get; protected set; }
        public float DirectDamageAvgHit { get; protected set; }
        public float DirectDamageAvgCrit { get; protected set; }
        public float DirectDamageAvg { get; protected set; }

        public float OverTimeDamageHit { get; protected set; }
        public float OverTimeDamageCrit { get; protected set; }
        public float OverTimeDamageAvg { get; protected set; }
        public float OverTimeTickPeriod { get; protected set; }
        public float OverTimeTicks { get; protected set; }

        public override void UpdateSpell()
        {
            base.UpdateSpell();
        }

        protected override string ToStringDirectDamage()
        {
            return base.ToStringDirectDamage();
        }

        protected override string ToStringOverTimeDamage()
        {
            return base.ToStringOverTimeDamage();
        }
    }

    /// actual spells!
    /// 

    public abstract class DirectHealSpell : HealSpell
    {
        protected float healBonus = 1;

        protected void DirectHealCalcs()
        {
            HasDirectHeal = true;

            float spellPowerBonus = stats.SpellPower * BaseDirectCoefficient;

            DirectHealMinHit = (BaseDirectValue * (1 - BaseDirectVariation / 2) + spellPowerBonus)
                * healBonus;
            DirectHealMaxHit = (BaseDirectValue * (1 + BaseDirectVariation / 2) + spellPowerBonus)
                * healBonus;
            DirectHealAvgHit = (DirectHealMinHit + DirectHealMaxHit) / 2;
            DirectHealMinCrit = DirectHealMinHit * CritMultiplier;
            DirectHealMaxCrit = DirectHealMaxHit * CritMultiplier;
            DirectHealAvgCrit = (DirectHealMinCrit + DirectHealMaxCrit) / 2;
            DirectHealAvg = DirectHealAvgHit * (1f - CritChance) + DirectHealAvgCrit * CritChance;

            if (ps == ePriestSpec.Spec_Disc)
            {
                float da = PriestInformation.GetDivineAegis(character.PriestTalents.DivineAegis) * (1f + DiscMastery);
                AbsorbMinCrit = DirectHealMinCrit * da;
                AbsorbMaxCrit = DirectHealMaxCrit * da;
                AbsorbAvgCrit = DirectHealAvgCrit * da;
                AbsorbAvg = AbsorbAvgHit * (1f - CritChance) + AbsorbAvgCrit * CritChance;
                HasAbsorb = true;
            }
            else if (ps == ePriestSpec.Spec_Holy)
            {
                Holy_HoT_TickPeriod = 1;
                Holy_HoT_Ticks = 6;
                Holy_HoT_Duration = Holy_HoT_TickPeriod * Holy_HoT_Ticks;
                Holy_HoT_Hit = DirectHealAvgHit * HolyMastery / Holy_HoT_Ticks;
                Holy_HoT_Crit = DirectHealAvgCrit * HolyMastery / Holy_HoT_Ticks;
                Holy_HoT_Avg = Holy_HoT_Hit * (1f - CritChance) + Holy_HoT_Crit * CritChance;
                HasOverTimeHeal = true;
            }
        }
    }

    public class SpellHeal : DirectHealSpell
    {
        public SpellHeal(Character character, Stats stats)
        {
            BaseDirectValue = 3.58699989318848f * BaseScalar85;
            BaseDirectCoefficient = 0.362f;
            BaseDirectVariation = 0.15f;

            BaseCastTime = 3.0f - PriestInformation.GetDivineFury(character.PriestTalents.DivineFury);
            BaseManaCost = 0.09f;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines))
                * (1f + PriestInformation.GetEmpoweredHealing(character.PriestTalents.EmpoweredHealing));
            base.UpdateSpell();
            DirectHealCalcs();
        }
    }

    public class SpellGreaterHeal : DirectHealSpell
    {
        public SpellGreaterHeal(Character character, Stats stats)
        {
            BaseDirectValue = 9.56400012969971f * BaseScalar85;
            BaseDirectCoefficient = 0.967f;
            BaseDirectVariation = 0.15f;

            BaseCastTime = 3.0f - PriestInformation.GetDivineFury(character.PriestTalents.DivineFury);
            BaseManaCost = 0.27f;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines))
                * (1f + PriestInformation.GetEmpoweredHealing(character.PriestTalents.EmpoweredHealing));
            base.UpdateSpell();
            DirectHealCalcs();
        }
    }
    
    public class SpellFlashHeal : DirectHealSpell
    {
        public SpellFlashHeal(Character character, Stats stats)
        {
            BaseDirectValue = 7.17399978637695f * BaseScalar85;
            BaseDirectCoefficient = 0.725f;
            BaseDirectVariation = 0.15f;

            BaseCastTime = 1.5f;
            BaseManaCost = 0.28f;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines))
                * (1f + PriestInformation.GetEmpoweredHealing(character.PriestTalents.EmpoweredHealing));
            base.UpdateSpell();
            DirectHealCalcs();
        }
    }

    public class SpellBindingHeal : DirectHealSpell
    {
        public SpellBindingHeal(Character character, Stats stats)
        {
            BaseDirectValue = 5.74599981307983f * BaseScalar85;
            BaseDirectCoefficient = 0.544f;
            BaseDirectVariation = 0.25f;

            BaseCastTime = 1.5f;
            BaseManaCost = 0.28f;

            Targets = 2;
            MaxTargets = 2;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines))
                * (1f + PriestInformation.GetEmpoweredHealing(character.PriestTalents.EmpoweredHealing));
            base.UpdateSpell();
            DirectHealCalcs();
        }
    }

    public class SpellPrayerOfHealing : DirectHealSpell
    {
        public SpellPrayerOfHealing(Character character, Stats stats)
        {
            BaseDirectValue = 3.35899996757507f * BaseScalar85;
            BaseDirectCoefficient = 0.34f;
            BaseDirectVariation = 0.055f;

            BaseCastTime = 2.5f;
            BaseManaCost = 0.26f;

            Targets = 5;
            MaxTargets = 5;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            float td = (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines));
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * td;
            base.UpdateSpell();
            DirectHealCalcs();
            if (ps == ePriestSpec.Spec_Disc)
            {   // Prayer of Healing has Absorbs for Hit and Crit
                float da = PriestInformation.GetDivineAegis(character.PriestTalents.DivineAegis) * (1f + DiscMastery);
                AbsorbMinHit = DirectHealMinHit * da;
                AbsorbMaxHit = DirectHealMaxHit * da;
                AbsorbAvgHit = DirectHealAvgHit * da;
                AbsorbMinCrit *= 2;
                AbsorbMaxCrit *= 2;
                AbsorbAvgCrit *= 2;
                AbsorbAvg = AbsorbAvgHit * (1f - CritChance) + AbsorbAvgCrit * CritChance;
            }
            if (character.PriestTalents.GlyphofPrayerofHealing)
            {
                OverTimeTicks = 2;
                OverTimeTickPeriod = 3;
                OverTimeDuration = OverTimeTicks * OverTimeTickPeriod;
                OverTimeHealHit = DirectHealAvgHit * 0.2f * td;
                OverTimeHealCrit = DirectHealAvgCrit * 0.2f * td;
                OverTimeHealAvg = OverTimeHealHit * (1f - CritChance) + OverTimeHealCrit * CritChance;
                HasOverTimeHeal = true;
            }

        }
    }

    public class SpellPowerWordShield : HealSpell
    {
        public SpellPowerWordShield(Character character, Stats stats)
        {
            BaseDirectValue = 0 * BaseScalar85;
            BaseDirectCoefficient = 0f;
            BaseDirectVariation = 0.0f;

            SetPriestInformation(character, stats);
        }
    }

    public class SpellResurrection : SpellCata
    {
        public SpellResurrection(Character character, Stats stats)
        {
            BaseCastTime = 10.0f;
            BaseManaCost = 0.6f;

            SetPriestInformation(character, stats);
            CritChance = 0f;
        }
    }
}
