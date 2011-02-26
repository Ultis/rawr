using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
	public abstract class Rotation
    {
        public static IEnumerable<Ability[]> GetAllRotations()
        {
			Ability[] abilities = new Ability[(int)Ability.Last + 1];

            for (int ability = 0; ability <= (int)Ability.Last; ability++)
                abilities[ability] = (Ability)ability;

			return Utilities.GetDifferentElementPermutations(abilities);
        }

        protected Rotation(CombatStats combats)
        {
            if (combats == null)
                throw new ArgumentNullException("combats");
			
            Combats = combats;
            CS = new CrusaderStrike(combats);
            TV = new TemplarsVerdict(combats);
            Exo = new Exorcism(combats);
            HW = new HolyWrath(combats);
            HoW = new HammerOfWrath(combats);
            Cons = new Consecration(combats);
			White = new White(combats);
            SoC = new SealOfCommand(combats);

            switch (combats.CalcOpts.Seal)
            {
                case SealOf.Righteousness:
                    Seal = new SealOfRighteousness(combats);
                    SealDot = new NullSealDoT(combats);
                    Judge = new JudgementOfRighteousness(combats);
                    break;

                case SealOf.Truth:
                    float stack = AverageSoTStackSize();
                    Seal = new SealOfTruth(combats, stack);
                    SealDot = new SealOfTruthDoT(combats, stack);
                    Judge = new JudgementOfTruth(combats, stack);
                    break;

                default:
                    Seal = new NullSeal(combats);
                    SealDot = new NullSealDoT(combats);
					Judge = new NullJudgement(combats);
                    break;
            }
        }

        public Skill CS { get; private set; }
        public Skill TV { get; private set; }
        public Skill Judge { get; private set; }
        public Skill Exo { get; private set; }
        public Skill HW { get; private set; }
        public Skill HoW { get; private set; }
        public Skill Cons { get; private set; }
		public Skill Seal { get; private set; }
        public Skill SealDot { get; private set; }
        public Skill SoC { get; private set; }
        public White White { get; private set; }
        public CombatStats Combats { get; private set; }

        public abstract void SetCharacterCalculations(CharacterCalculationsRetribution calc);

        public void SetDPS(CharacterCalculationsRetribution calc)
        {
            SetCharacterCalculations(calc);

            calc.AverageSoVStack = AverageSoTStackSize();

            calc.WhiteDPS = White.WhiteDPS();
            calc.SealDPS = SealDPS(Seal, SealDot);
            calc.CommandDPS = GetSoCDps(SoC);
            calc.JudgementDPS = GetAbilityDps(Judge);
            calc.CrusaderStrikeDPS = GetAbilityDps(CS);
            calc.TemplarsVerdictDPS = GetAbilityDps(TV);
            calc.HolyWrathDPS = GetAbilityDps(HW);
            calc.ConsecrationDPS = GetAbilityDps(Cons);
            calc.ExorcismDPS = GetAbilityDps(Exo);
            calc.HammerOfWrathDPS = GetAbilityDps(HoW);

            calc.WhiteSkill = White;
            calc.SealSkill = Seal;
            calc.CommandSkill = SoC;
            calc.JudgementSkill = Judge;
            calc.TemplarsVerdictSkill = TV;
            calc.CrusaderStrikeSkill = CS;
            calc.ConsecrationSkill = Cons;
            calc.ExorcismSkill = Exo;
            calc.HolyWrathSkill = HW;
			calc.HammerOfWrathSkill = HoW;

            calc.DPSPoints =
                calc.WhiteDPS +
                calc.SealDPS +
                calc.CommandDPS +
                calc.JudgementDPS +
				calc.CrusaderStrikeDPS +
                calc.TemplarsVerdictDPS+
                calc.ExorcismDPS +
                calc.HolyWrathDPS+
				calc.ConsecrationDPS +
                calc.HammerOfWrathDPS +
                calc.OtherDPS;
        }

        public float DPS()
        {
            return 
                White.WhiteDPS() + 
                SealDPS(Seal, SealDot)+ 
                GetSoCDps(SoC) +
                GetAbilityDps(Judge) +
				GetAbilityDps(CS) + 
                GetAbilityDps(TV) +
                GetAbilityDps(Cons) + 
                GetAbilityDps(Exo) +
                GetAbilityDps(HW) +
                GetAbilityDps(HoW);
        }

        public float SealProcsPerSec(Skill seal)
        {
            if (seal.GetType() == typeof(SealOfTruth))
                return GetMeleeAttacksPerSec();
            else
                return GetMeleeAttacksPerSec() + GetAbilityHitsPerSecond(Judge);
        }

        public float AverageSoTStackSize()
		{
            float averageTimeOnMob = Combats.CalcOpts.FightLength * 60f / (Combats.CalcOpts.TargetSwitches + 1);
            float timeToMaxStack = 5f / GetSoTAttacksPerSec();
            if (averageTimeOnMob > timeToMaxStack)
            {
				return (2.5f * timeToMaxStack + 5f * (averageTimeOnMob - timeToMaxStack)) / averageTimeOnMob;
            }
            else
            {
                return 2.5f * averageTimeOnMob / timeToMaxStack;
            }
        }

		public virtual float SealDPS(Skill seal, Skill sealdot)
        {
            return sealdot.AverageDamage() / 3f + seal.AverageDamage() * SealProcsPerSec(seal);
        }

        public float GetSoCDps(Skill soc)
        {
            return soc.AverageDamage() * GetAttacksPerSec();
        }

        public float GetAbilityDps(Skill skill)
        {
            return skill.AverageDamage() * GetAbilityUsagePerSecond(skill);
        }

        public float GetAbilityHitsPerSecond(Skill skill)
		{
            return 
                GetAbilityUsagePerSecond(skill) *
                skill.ChanceToLand() * 
                skill.Targets() * 
                skill.TickCount();
        }

        public float GetAbilityCritsPerSecond(Skill skill)
        {
            return 
                GetAbilityUsagePerSecond(skill) *
                skill.ChanceToCrit() * 
                skill.Targets() * 
                skill.TickCount();
		}

        public float GetCrusaderStrikeCD()
        {
            return 1 / GetAbilityUsagePerSecond(CS);
        }

        public float GetJudgementCD()
        {
            return 1 / GetAbilityUsagePerSecond(Judge);
        }

        public abstract float GetAbilityUsagePerSecond(Skill skill);

        private float GetSoTAttacksPerSec()
        {   //TODO: Add real calc
            return .7f;
            /* return GetPhysicalAttacksPerSec() + GetAbilityUsagePerSecond(Exo);*/
        }

        public float GetMeleeAttacksPerSec()
        {
            // Melee hit procs can be triggered by:
            // - Crusader Strike hits
            // - Divine Storm hits
			// - Weapon swing hits
            // - Templars Verdict hits
            return
                GetAbilityHitsPerSecond(CS) +
                White.ChanceToLand() / Combats.AttackSpeed +
                GetAbilityHitsPerSecond(TV);
		}

        public float GetPhysicalAttacksPerSec()
        {
            // Physical hit procs can be triggered by:
            // - Crusader Strike hits
            // - Divine Storm hits
            // - Weapon swing hits
            // - Templar's Verdict hits
            // - Judgement hits
            // - Hammer of Wrath hits
            return
                GetAbilityHitsPerSecond(CS) +
                White.ChanceToLand() / Combats.AttackSpeed +
				GetAbilityHitsPerSecond(TV) +
                GetAbilityHitsPerSecond(Judge) +
                GetAbilityHitsPerSecond(HoW);
        }

        public float GetSpellAttacksPerSec()
        {
            // Spell hit procs can be triggered by:
            // - Exorcism hits
            // - Holy Wrath hits
            // - Consecration hits (first tick)
            return
                GetAbilityHitsPerSecond(Exo) +
                GetAbilityHitsPerSecond(HW) +
                GetAbilityHitsPerSecond(Cons) / Cons.TickCount();
        }

        public float GetMeleeCritsPerSec()
        {
            // Melee crit procs can be triggered by:
            // - Crusader Strike crits
            // - Divine Storm crits on each target
            // - Weapon swing crits
            // - Templar's Verdict crits
            return
                GetAbilityCritsPerSecond(CS) +
                White.ChanceToCrit() / Combats.AttackSpeed +
                GetAbilityCritsPerSecond(TV);
       }

        public float GetPhysicalCritsPerSec()
        {
            // Physical crit procs can be triggered by:
            // - Crusader Strike crits
            // - Divine Storm crits on each target
            // - Weapon swing crits
            // - Templar's Verdicts crits
            // - Judgement crits
            // - Hammer of Wrath crits
            return
                GetAbilityCritsPerSecond(CS) +
                White.ChanceToCrit() / Combats.AttackSpeed +
                GetAbilityCritsPerSecond(TV) + 
                GetAbilityCritsPerSecond(Judge) +
                GetAbilityCritsPerSecond(HoW);
        }

        public float GetSpellCritsPerSec()
        {
            // Spell hit procs can be triggered by:
            // - Exorcism crits
            // - Holy Wrath crits
            // - Consecration crits (first tick)
            return
                GetAbilityCritsPerSecond(Exo) +
                GetAbilityHitsPerSecond(HW) / HW.TickCount() +
                GetAbilityCritsPerSecond(Cons) / Cons.TickCount();
        }

        public float GetAttacksPerSec()
        {
            // Damage done procs and damage or healing done procs can be triggered by:
            // - Crusader Strike hits
            // - Divine Storm hits
            // - Weapon swing hits
            // - Templar's Verdict hits
            // - Judgement hits
            // - Hammer of Wrath hits
            // - Holy Wrah hits
            // - Consecration damage ticks
            // - Exorcism hits

            return
                GetAbilityHitsPerSecond(CS) +
                White.ChanceToLand() / Combats.AttackSpeed +
                GetAbilityHitsPerSecond(TV) + 
                GetAbilityHitsPerSecond(Judge) +
                GetAbilityHitsPerSecond(HoW) +
                GetAbilityHitsPerSecond(HW) +
                GetAbilityHitsPerSecond(Cons) +
                GetAbilityHitsPerSecond(Exo);
        }
    }
}
