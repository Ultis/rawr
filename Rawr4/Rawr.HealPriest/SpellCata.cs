using System;

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
        protected StatsPriest stats;

        public string Name { get; protected set; }

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
        public float BaseCooldown { get; protected set; }
        public float Cooldown { get; protected set; }

        public virtual void SetPriestInformation(Character character, StatsPriest stats) { this.character = character; this.stats = stats;  UpdateSpell(); }
        
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
            if (Cooldown > 0)
                retval += String.Format("\nCooldown: {0} seconds", Cooldown.ToString("0"));
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

        public float EoL_Hit { get; protected set; }
        public float EoL_Crit { get; protected set; }
        public float EoL_Avg { get; protected set; }
        public float EoL_TickPeriod { get; protected set; }
        public float EoL_Ticks { get; protected set; }
        public float EoL_Duration { get; protected set; }

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
            float hhot = (EoL_TickPeriod > 0) ? (EoL_Avg / EoL_TickPeriod) : 0f;
            return ((DirectHealAvg + AbsorbAvg) / castTime + hot + hhot) * targets;
        }

        public virtual float HPC()
        {
            float targets = (Targets > 0) ? Targets : 1f;
            float hot = OverTimeHealAvg * OverTimeTicks;
            float hhot = EoL_Avg * EoL_Ticks;
            return (DirectHealAvg + AbsorbAvg + hot + hhot) * targets;
        }

        public virtual float HPCT()
        {
            float castTime = (IsInstant) ? GlobalCooldown : CastTime;
            return HPC() / castTime;
        }

        public virtual float HPM()
        {
            float targets = (Targets > 0) ? Targets : 1f;
            float hot = OverTimeHealAvg * OverTimeTicks;
            float hhot = EoL_Avg * EoL_Ticks;
            return ((DirectHealAvg + AbsorbAvg + hot + hhot) / ManaCost) * targets;
        }

        public override void SetPriestInformation(Character character, StatsPriest stats)
        {
            this.character = character;
            this.stats = stats;
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

            EoL_Hit = 0; EoL_Crit = 0; EoL_Avg = 0;
            EoL_Duration = 0; EoL_TickPeriod = 0; EoL_Ticks = 0;

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
                s += String.Format("\nHoT ticks a total of {0} times, once every {1} seconds, for a duration of {2} seconds", OverTimeTicks.ToString("0"), OverTimeTickPeriod.ToString("0.00"), OverTimeDuration.ToString("0.00"));
            }
            if (EoL_Hit > 0)
            {
                s += String.Format("\nEcho of Light: Hit {0}", EoL_Hit.ToString("0"));
                if (EoL_Hit != EoL_Crit)
                {
                    s += String.Format(", Crit {0}, Avg {1}", EoL_Crit.ToString("0"), EoL_Avg.ToString("0"));
                }
                s += String.Format("\nEcho of Light ticks a total of {0} times, once every {1} seconds, for a duration of {2} seconds", EoL_Ticks.ToString("0"), EoL_TickPeriod.ToString("0"), EoL_Duration.ToString("0"));
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

        protected void DivineAegis()
        {
            float da = PriestInformation.GetDivineAegis(character.PriestTalents.DivineAegis) * (1f + stats.ShieldDiscipline);
            AbsorbMinCrit = DirectHealMinCrit * da;
            AbsorbMaxCrit = DirectHealMaxCrit * da;
            AbsorbAvgCrit = DirectHealAvgCrit * da;
            AbsorbAvg = AbsorbAvgHit * (1f - CritChance) + AbsorbAvgCrit * CritChance;
            HasAbsorb = true;
        }

        protected void EchoOfLight()
        {
            EoL_TickPeriod = 1;
            EoL_Ticks = 6;
            EoL_Duration = EoL_TickPeriod * EoL_Ticks;
            EoL_Hit = DirectHealAvgHit * stats.EchoofLight / EoL_Ticks;
            EoL_Crit = DirectHealAvgCrit * stats.EchoofLight / EoL_Ticks;
            EoL_Avg = EoL_Hit * (1f - CritChance) + EoL_Crit * CritChance;
            HasOverTimeHeal = true;
        }

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

            if (stats.PriestSpec == ePriestSpec.Spec_Disc)            
            {
                DivineAegis();
            }
            else if (stats.PriestSpec == ePriestSpec.Spec_Holy)
            {
                EchoOfLight();
            }
        }
    }

    public class SpellHeal : DirectHealSpell
    {
        public SpellHeal(Character character, StatsPriest stats)
        {
            Name = "Heal";

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
        public SpellGreaterHeal(Character character, StatsPriest stats)
        {
            Name = "Greater Heal";

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
        public SpellFlashHeal(Character character, StatsPriest stats)
        {
            Name = "Flash Heal";

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
        public SpellBindingHeal(Character character, StatsPriest stats)
        {
            Name = "Binding Heal";

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

    public class SpellSerenity : DirectHealSpell
    {
        public SpellSerenity(Character character, StatsPriest stats)
        {
            Name = "Holy Word Serenity";

            BaseDirectValue = 5.97700023651123f * BaseScalar85;
            BaseDirectCoefficient = 0.486f;
            BaseDirectVariation = 0.16f;

            BaseManaCost = 0.08f;

            BaseCooldown = 15f;
            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines));
            base.UpdateSpell();
            Cooldown = BaseCooldown * PriestInformation.GetTomeOfLight(character.PriestTalents.TomeOfLight);
            DirectHealCalcs();
        }
    }

    public class SpellRenew : DirectHealSpell
    {
        public SpellRenew(Character character, StatsPriest stats)
        {
            Name = "Renew";

            BaseOverTimeValue = 1.29499995708466f * BaseScalar85;
            BaseOverTimeCoefficient = 0.131f;
            BaseOverTimeTickPeriod = 3f;
            BaseOverTimeTicks = 4f;

            BaseManaCost = 0.17f;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines))
                * (1f + PriestInformation.GetImprovedRenew(character.PriestTalents.ImprovedRenew))
                * (character.PriestTalents.GlyphofRenew ? 1.1f : 1.0f);

            base.UpdateSpell();

            float spellPowerBonus = stats.SpellPower * BaseOverTimeCoefficient;

            OverTimeHealHit = (BaseOverTimeValue + spellPowerBonus) * healBonus;
            OverTimeHealCrit = OverTimeHealHit * CritMultiplier;
            OverTimeHealAvg = OverTimeHealHit * (1f - CritChance) + OverTimeHealCrit * CritChance;
            float haste = (1f + stats.SpellHaste);
            OverTimeTickPeriod = BaseOverTimeTickPeriod / haste;
            // You get k more ticks for a hot/dot that has m ticks at 0% haste if your haste is higher than (2k-1)/2m.
            OverTimeTicks = (float)Math.Floor(BaseOverTimeTicks * haste + 0.5f);
            OverTimeDuration = OverTimeTickPeriod * OverTimeTicks;
            HasOverTimeHeal = true;

            if (character.PriestTalents.DivineAegis > 0)
            {
                float da = PriestInformation.GetDivineAegis(character.PriestTalents.DivineAegis) * (1f + stats.ShieldDiscipline);
                AbsorbMinCrit = AbsorbMaxCrit = AbsorbAvgCrit = OverTimeHealCrit * da;
                AbsorbAvg = AbsorbAvgHit * (1f - CritChance) + AbsorbAvgCrit * CritChance;
                HasAbsorb = true;
            }

            if (character.PriestTalents.DivineTouch > 0)
            {
                DirectHealMinHit = DirectHealMaxHit = DirectHealAvgHit = OverTimeHealHit * OverTimeTicks;
                DirectHealMinCrit = DirectHealMaxCrit = DirectHealAvgCrit = OverTimeHealCrit * OverTimeTicks;
                DirectHealAvg = DirectHealAvgHit * (1f - CritChance) + DirectHealAvgCrit * CritChance;
                HasDirectHeal = true;
                if (stats.PriestSpec == ePriestSpec.Spec_Disc)
                {
                    DivineAegis();
                }
                else if (stats.PriestSpec == ePriestSpec.Spec_Holy)
                {
                    EchoOfLight();
                }
            }
        }

        protected override string ToStringOverTimeHeal()
        {
            float nonRatingHaste = (1f + stats.SpellHaste) / (1f + StatConversion.GetSpellHasteFromRating(stats.HasteRating));
            // Check how much haste is needed to gain an additional tick.
            float extraTickHaste = (OverTimeTicks + 0.5f) / BaseOverTimeTicks;
            float extraHasteRating = (extraTickHaste / nonRatingHaste - 1f) / StatConversion.GetSpellHasteFromRating(1f) - stats.HasteRating;
            
            return String.Format("{0}\n{1} more Haste Rating needed for an additional tick.",
                base.ToStringOverTimeHeal(),
                extraHasteRating.ToString("0"));
        }
    }

    public class SpellLightwell : DirectHealSpell
    {
        public SpellLightwell(Character character, StatsPriest stats)
        {
            Name = "Lightwell";

            BaseOverTimeValue = 3.04500007629395f * BaseScalar85;
            BaseOverTimeCoefficient = 0.308f;
            BaseOverTimeTickPeriod = 2f;
            BaseOverTimeTicks = 3f;

            BaseManaCost = 0.30f;

            MaxTargets = character.PriestTalents.GlyphofLightwell ? 15 : 10;
            Targets = MaxTargets;

            BaseCooldown = 3 * 60;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines));

            base.UpdateSpell();
            Cooldown = BaseCooldown;

            float spellPowerBonus = stats.SpellPower * BaseOverTimeCoefficient;

            OverTimeHealHit = (BaseOverTimeValue + spellPowerBonus) * healBonus;
            OverTimeHealCrit = OverTimeHealHit * CritMultiplier;
            OverTimeHealAvg = OverTimeHealHit * (1f - CritChance) + OverTimeHealCrit * CritChance;
            float haste = (1f + stats.SpellHaste);
            OverTimeTickPeriod = BaseOverTimeTickPeriod / haste;
            // You get k more ticks for a hot/dot that has m ticks at 0% haste if your haste is higher than (2k-1)/2m.
            OverTimeTicks = (float)Math.Floor(BaseOverTimeTicks * haste + 0.5f);
            OverTimeDuration = OverTimeTickPeriod * OverTimeTicks;
            HasOverTimeHeal = true;
        }

        protected override string ToStringOverTimeHeal()
        {
            float nonRatingHaste = (1f + stats.SpellHaste) / (1f + StatConversion.GetSpellHasteFromRating(stats.HasteRating));
            // Check how much haste is needed to gain an additional tick.
            float extraTickHaste = (OverTimeTicks + 0.5f) / BaseOverTimeTicks;
            float extraHasteRating = (extraTickHaste / nonRatingHaste - 1f) / StatConversion.GetSpellHasteFromRating(1f) - stats.HasteRating;

            return String.Format("{0}\n{1} more Haste Rating needed for an additional tick.",
                base.ToStringOverTimeHeal(),
                extraHasteRating.ToString("0"));
        }
    }

    public class SpellPenance : DirectHealSpell
    {
        public SpellPenance(Character character, StatsPriest stats)
        {
            Name = "Penance";

            BaseDirectValue = 3.1800000667572f * BaseScalar85;
            BaseDirectCoefficient = 0.321f;
            BaseDirectVariation = 0.122f;

            BaseCastTime = 2f;
            BaseManaCost = 0.14f;

            Targets = MaxTargets = 3;

            BaseCooldown = 12;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines));

            base.UpdateSpell();
            Cooldown = BaseCooldown - (character.PriestTalents.GlyphofPenance ? 2f : 0f);

            float spellPowerBonus = stats.SpellPower * BaseDirectCoefficient;
            DirectHealCalcs();
        }

        protected override string ToStringDirectHeal()
        {
            return String.Format("{0}\nPenance ticks a total of {1} times, once every {2} seconds, for a duration of {3} seconds",
                base.ToStringDirectHeal(),
                3,
                (CastTime / 2).ToString("0.00"),    // First tick is instant.
                CastTime.ToString("0.00")
                );
        }
    }

    public class SpellDivineHymn : DirectHealSpell
    {
        public SpellDivineHymn(Character character, StatsPriest stats)
        {
            Name = "Divine Hymn";

            BaseDirectValue = 4.24200010299683f * BaseScalar85;
            BaseDirectCoefficient = 0.429f;
            BaseDirectVariation = 0.0f;

            BaseCastTime = 8f;
            BaseOverTimeTickPeriod = 2f;
            BaseOverTimeTicks = BaseCastTime / BaseOverTimeTickPeriod;

            BaseManaCost = 0.36f;

            Targets = MaxTargets = 12;

            BaseCooldown = 60 * 8;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines));

            base.UpdateSpell();
            Cooldown = BaseCooldown;
            float haste = (1f + stats.SpellHaste);

            OverTimeTickPeriod = BaseOverTimeTickPeriod / haste;
            // You get k more ticks for a hot/dot that has m ticks at 0% haste if your haste is higher than (2k-1)/2m.
            OverTimeTicks = (float)Math.Floor(BaseOverTimeTicks * haste + 0.5f);
            OverTimeDuration = OverTimeTickPeriod * OverTimeTicks;

            DirectHealCalcs();
        }

        protected override string ToStringDirectHeal()
        {
            float nonRatingHaste = (1f + stats.SpellHaste) / (1f + StatConversion.GetSpellHasteFromRating(stats.HasteRating));
            // Check how much haste is needed to gain an additional tick.
            float extraTickHaste = (OverTimeTicks + 0.5f) / BaseOverTimeTicks;
            float extraHasteRating = (extraTickHaste / nonRatingHaste - 1f) / StatConversion.GetSpellHasteFromRating(1f) - stats.HasteRating;
            
            return String.Format("{0}\n{1} more Haste Rating needed for an additional tick.",
                base.ToStringDirectHeal(),
                extraHasteRating.ToString("0"));
        }
    }


    public class SpellPrayerOfMending : DirectHealSpell
    {
        public SpellPrayerOfMending(Character character, StatsPriest stats)
        {
            MaxTargets = 5;
            Initialize(character, stats, MaxTargets);
        }

        public SpellPrayerOfMending(Character character, StatsPriest stats, int targets)
        {
            MaxTargets = 5;
            Initialize(character, stats, targets);
        }

        protected void Initialize(Character character, StatsPriest stats, int targets)
        {
            Name = "Prayer of Mending";

            BaseDirectValue = 3.14400005340576f * BaseScalar85;
            BaseDirectCoefficient = 0.318f;
            BaseDirectVariation = 0.0f;

            Targets = targets;

            BaseManaCost = 0.18f;

            BaseCooldown = 10;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines));
            if (character.PriestTalents.GlyphofPrayerOfMending && Targets == 1)
                healBonus *= 1.6f;
            base.UpdateSpell();
            Cooldown = BaseCooldown;
            DirectHealCalcs();
        }
    }

    public class SpellPrayerOfHealing : DirectHealSpell
    {
        public SpellPrayerOfHealing(Character character, StatsPriest stats)
        {
            MaxTargets = 5;
            Initialize(character, stats, MaxTargets);
        }

        public SpellPrayerOfHealing(Character character, StatsPriest stats, int targets)
        {
            MaxTargets = 5;
            Initialize(character, stats, targets);
        }

        protected void Initialize(Character character, StatsPriest stats, int targets)
        {
            Name = "Prayer of Healing";

            BaseDirectValue = 3.35899996757507f * BaseScalar85;
            BaseDirectCoefficient = 0.34f;
            BaseDirectVariation = 0.055f;

            BaseCastTime = 2.5f;
            BaseManaCost = 0.26f;

            Targets = targets;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            float td = (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines));
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * td;
            base.UpdateSpell();
            DirectHealCalcs();
            if (stats.PriestSpec == ePriestSpec.Spec_Disc)
            {   // Prayer of Healing has Absorbs for Hit and Crit
                float da = PriestInformation.GetDivineAegis(character.PriestTalents.DivineAegis) * (1f + stats.ShieldDiscipline);
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

    public class SpellHolyNova : DirectHealSpell
    {
        public SpellHolyNova(Character character, StatsPriest stats)
        {
            MaxTargets = 5;
            Initialize(character, stats, MaxTargets);
        }

        public SpellHolyNova(Character character, StatsPriest stats, int targets)
        {
            MaxTargets = 5;
            Initialize(character, stats, targets);
        }

        protected void Initialize(Character character, StatsPriest stats, int targets)
        {
            Name = "Holy Nova";

            BaseDirectValue = 0.316000014543533f * BaseScalar85;
            BaseDirectCoefficient = 0.143f;
            BaseDirectVariation = 0.15f;

            BaseManaCost = 0.15f;

            Targets = targets;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines));
            base.UpdateSpell();
            DirectHealCalcs();
        }
    }

    public class SpellCircleOfHealing : DirectHealSpell
    {

        public SpellCircleOfHealing(Character character, StatsPriest stats)
        {
            MaxTargets = character.PriestTalents.GlyphofCircleOfHealing ? 6 : 5;
            Initialize(character, stats, MaxTargets);           
        }

        public SpellCircleOfHealing(Character character, StatsPriest stats, int targets)
        {
            MaxTargets = character.PriestTalents.GlyphofCircleOfHealing ? 6 : 5;
            Initialize(character, stats, targets);
        }

        protected void Initialize(Character character, StatsPriest stats, int targets)
        {
            Name = "Circle of Healing";

            BaseDirectValue = 2.57100009918213f * BaseScalar85;
            BaseDirectCoefficient = 0.26f;
            BaseDirectVariation = 0.10f;

            BaseManaCost = 0.21f;

            Targets = targets;

            BaseCooldown = 10;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines));
            base.UpdateSpell();
            Cooldown = BaseCooldown;
            DirectHealCalcs();
        }
    }

    public class SpellSanctuary : DirectHealSpell
    {
        private float ticks = 9;

        public SpellSanctuary(Character character, StatsPriest stats)
        {
            MaxTargets = 6;
            Initialize(character, stats, MaxTargets);
        }

        public SpellSanctuary(Character character, StatsPriest stats, int targets)
        {
            MaxTargets = 6;
            Initialize(character, stats, targets);
        }

        protected void Initialize(Character character, StatsPriest stats, int targets)
        {
            Name = "Holy Word Sanctuary";

            BaseDirectValue = 0.345999985933304f * BaseScalar85;
            BaseDirectCoefficient = 0.031f;
            BaseDirectVariation = 0.173f;

            BaseManaCost = 0.44f;

            Targets = targets;

            BaseCooldown = 40;

            SetPriestInformation(character, stats);
        }

        public override float HPC()
        {
            return base.HPC() * ticks;
        }
        public override float HPS()
        {
            float oldCastTime = CastTime;
            CastTime = 2;
            float result = base.HPS();
            CastTime = oldCastTime;
            return result;
        }
        public override float HPM()
        {
            return base.HPM() * ticks;
        }


        public override void UpdateSpell()
        {
            healBonus = (1f + stats.BonusHealingDoneMultiplier)
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines));
            base.UpdateSpell();
            Cooldown = BaseCooldown * (1f - PriestInformation.GetTomeOfLight(character.PriestTalents.TomeOfLight));
            DirectHealCalcs();
        }
    }

    public class SpellPowerWordShield : DirectHealSpell
    {
        public SpellPowerWordShield(Character character, StatsPriest stats)
        {
            Name = "Power Word: Shield";

            BaseDirectValue = 8.60879993438721f * BaseScalar85;
            BaseDirectCoefficient = 0.87f;
            BaseDirectVariation = 0.0f;

            BaseManaCost = 0.34f;

            BaseCooldown = 3;

            SetPriestInformation(character, stats);
        }

        public override void UpdateSpell()
        {
            float shieldBonus = (1f + PriestInformation.GetImprovedPowerWordShield(character.PriestTalents.ImprovedPowerWordShield))
                * (1f + PriestInformation.GetTwinDisciplines(character.PriestTalents.TwinDisciplines))
                * (1f + stats.ShieldDiscipline);
            float spellPowerBonus = stats.SpellPower * BaseDirectCoefficient;

            base.UpdateSpell();
            Cooldown = BaseCooldown - PriestInformation.GetSoulWarding(character.PriestTalents.SoulWarding);

            AbsorbMinHit = AbsorbMaxHit = AbsorbAvgHit = (BaseDirectValue + spellPowerBonus) * shieldBonus;

            if (character.PriestTalents.GlyphofPowerWordShield)
            {
                DirectHealMinHit = DirectHealMaxHit = DirectHealAvgHit = AbsorbAvgHit * 0.2f;
                DirectHealMinCrit = DirectHealMaxCrit = DirectHealAvgCrit = AbsorbAvgHit * 0.2f * CritMultiplier;
                DirectHealAvg = DirectHealAvgHit * (1f - CritChance) + DirectHealAvgCrit * CritChance;
                HasDirectHeal = true;
                if (stats.PriestSpec == ePriestSpec.Spec_Disc)
                {
                    DivineAegis();
                }
                else if (stats.PriestSpec == ePriestSpec.Spec_Holy)
                {
                    EchoOfLight();
                }
            }

            AbsorbAvg = AbsorbAvgHit + AbsorbAvgCrit * CritChance;
        }
    }

    public class SpellResurrection : SpellCata
    {
        public SpellResurrection(Character character, StatsPriest stats)
        {
            Name = "Resurrection";

            BaseCastTime = 10.0f;
            BaseManaCost = 0.6f;

            SetPriestInformation(character, stats);
            CritChance = 0f;
        }
    }

    public class SpellGiftOfTheNaaru : DirectHealSpell
    {
        public SpellGiftOfTheNaaru(Character character, StatsPriest stats)
        {
            SetPriestInformation(character, stats);
            CritChance = 0f;
        }

        public override void UpdateSpell()
        {
            base.UpdateSpell();
            HasOverTimeHeal = true;
            OverTimeTicks = 5;
            OverTimeTickPeriod = 3;
            OverTimeDuration = OverTimeTicks * OverTimeTickPeriod;
            OverTimeHealHit = OverTimeHealCrit = OverTimeHealAvg = stats.Health * 0.04f;            
        }
/*
        protected override string ToStringOverTimeHeal()
        {
            return 
        }*/
    }

}
