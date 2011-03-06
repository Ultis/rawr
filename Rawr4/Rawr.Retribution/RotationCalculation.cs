using System;
using System.Collections.Generic;

namespace Rawr.Retribution
{
    public abstract class RotationCalculation
    {
        public Skill CS { get { return skills[DamageAbility.CrusaderStrike]; } }
        public Skill HoLCS { get { return skills[DamageAbility.HandOfLightCS]; } }
        public Skill TV { get { return skills[DamageAbility.TemplarsVerdict]; } }
        public Skill HoLTV { get { return skills[DamageAbility.HandOfLightTV]; } }
        public Skill Judge { get { return skills[DamageAbility.Judgement]; } }
        public Skill Exo { get { return skills[DamageAbility.Exorcism]; } }
        public Skill HW { get { return skills[DamageAbility.HolyWrath]; } }
        public Skill HoW { get { return skills[DamageAbility.HammerOfWrath]; } }
        public Skill Cons { get { return skills[DamageAbility.Consecration]; } }
        public Skill Seal { get { return skills[DamageAbility.Seal]; } }
        public Skill SealDot { get { return skills[DamageAbility.SealDot]; } }
        public Skill SoC { get { return skills[DamageAbility.SoC]; } }
        public White White { get { return (White)skills[DamageAbility.White]; } }

        public CombatStats Combats { get; private set; }

        public RotationCalculation(CombatStats combats)
        {
            if (combats == null)
                throw new ArgumentNullException("combats");

            Combats = combats;
            dpChance = combats.Talents.DivinePurpose * PaladinConstants.DP_CHANCE;
            fightlength = combats.CalcOpts.FightLength * 60;

            casts[DamageAbility.Consecration] = 0f;
            casts[DamageAbility.CrusaderStrike] = 0f;
            casts[DamageAbility.Exorcism] = 0f;
            casts[DamageAbility.Inquisition] = 0f;
            casts[DamageAbility.HammerOfWrath] = 0f;
            casts[DamageAbility.HolyWrath] = 0f;
            casts[DamageAbility.Judgement] = 0f;
            casts[DamageAbility.TemplarsVerdict] = 0f;

            remainingCd[Ability.CrusaderStrike] = -1f;
            remainingCd[Ability.TemplarsVerdict] = -1f;
            remainingCd[Ability.Exorcism] = -1f;
            remainingCd[Ability.Inquisition] = -1f;
            remainingCd[Ability.HolyWrath] = -1f;
            remainingCd[Ability.HammerOfWrath] = -1f;
            remainingCd[Ability.Consecration] = -1f;
            remainingCd[Ability.Judgement] = -1f;

            skills[DamageAbility.CrusaderStrike] = new CrusaderStrike(combats);
            skills[DamageAbility.HandOfLightCS] = new HandofLight(combats, CS.AverageDamage());
            skills[DamageAbility.TemplarsVerdict] = new TemplarsVerdict(combats);
            skills[DamageAbility.HandOfLightTV] = new HandofLight(combats, TV.AverageDamage());
            skills[DamageAbility.Exorcism] = new Exorcism(combats);
            skills[DamageAbility.Inquisition] = new Inquisition(combats);
            skills[DamageAbility.HolyWrath] = new HolyWrath(combats);
            skills[DamageAbility.HammerOfWrath] = new HammerOfWrath(combats);
            skills[DamageAbility.Consecration] = new Consecration(combats);

            skills[DamageAbility.White] = new White(combats);
            skills[DamageAbility.SoC] = new SealOfCommand(combats);

            switch (combats.CalcOpts.Seal)
            {
                case SealOf.Righteousness:
                    skills[DamageAbility.Seal] = new SealOfRighteousness(combats);
                    skills[DamageAbility.SealDot] = new NullSealDoT(combats);
                    skills[DamageAbility.Judgement] = new JudgementOfRighteousness(combats);
                    break;

                case SealOf.Truth:
                    skills[DamageAbility.Seal] = new SealOfTruth(combats);
                    skills[DamageAbility.SealDot] = new SealOfTruthDoT(combats, 0f);
                    skills[DamageAbility.Judgement] = new JudgementOfTruth(combats, 0f);
                    float stack = 5f;// AverageSoTStackSize();
                    skills[DamageAbility.SealDot] = new SealOfTruthDoT(combats, stack);
                    skills[DamageAbility.Judgement] = new JudgementOfTruth(combats, stack);
                    break;

                default:
                    skills[DamageAbility.Seal] = new NullSeal(combats);
                    skills[DamageAbility.SealDot] = new NullSealDoT(combats);
                    skills[DamageAbility.Judgement] = new NullJudgement(combats);
                    break;
            }
            CalcRotation();
        }

        private static float fightcorrVal = 5f;
        private static float latency = .1f;
        private static float inqRefresh = 4f;

        private Ability[] allAb = { Ability.Consecration, Ability.CrusaderStrike, Ability.Exorcism, Ability.HammerOfWrath, Ability.HolyWrath, Ability.Inquisition, Ability.Judgement, Ability.TemplarsVerdict };

        private float inquptime = 0f;
        private float holyPower = 0f;
        private float time = 0f;
        private bool below20 = false;
        private float dpChance;
        private float holyPowerDP = 0f;
        private float fightlength;

        public void CalcRotation()
        {
            RotState state = RotState.CS;
            float tempFightlength = fightlength * fightcorrVal;

            while (time < tempFightlength)
            {
                DoInq();
                switch (state)
                {
                    case RotState.CS:
                        DoCS();
                        state = RotState.FillerOne;
                        break;
                    case RotState.FillerOne:
                        DoFiller();
                        state = RotState.FillerTwo;
                        break;
                    case RotState.FillerTwo:
                        if (remainingCd[Ability.CrusaderStrike] >= .5f)
                        {
                            if (DoFiller())
                                state = RotState.CS;
                        }
                        else
                        {
                            DoNothing();
                            state = RotState.CS;
                        }
                        break;
                }
            }

            //Correct to float values
            foreach (KeyValuePair<DamageAbility, Skill> kvp in skills)
            {
                if (casts.ContainsKey(kvp.Key))
                    casts[kvp.Key] = casts[kvp.Key] / fightcorrVal;
            }
            
            casts[DamageAbility.HandOfLightCS] = casts[DamageAbility.CrusaderStrike];
            casts[DamageAbility.HandOfLightTV] = casts[DamageAbility.TemplarsVerdict];
            casts[DamageAbility.White] = fightlength / Combats.AttackSpeed;
            casts[DamageAbility.Seal] = (float)(fightlength * SealProcsPerSec(Seal));
            casts[DamageAbility.SoC] = (float)(fightlength * SealProcsPerSec(SoC));
            casts[DamageAbility.SealDot] = (float) (fightlength * SealDotProcPerSec(Seal));

            //Inq only last until end of fight not longer => prevent > 100% uptime
            inquptime = (inquptime - remainingCd[Ability.Inquisition]) / tempFightlength;

            //UsagePerSecCalc
            foreach (KeyValuePair<DamageAbility, Skill> kvp in skills)
            {
                kvp.Value.UsagePerSec = casts[kvp.Key] / (double) fightlength;
                kvp.Value.InqUptime = inquptime;
            }
            casts[DamageAbility.Seal] = (float)(fightlength * SealProcsPerSec(Seal));
            casts[DamageAbility.SoC] = (float)(fightlength * SealProcsPerSec(SoC));
            skills[DamageAbility.Seal].UsagePerSec = casts[DamageAbility.Seal] / (double)fightlength;
            skills[DamageAbility.SoC].UsagePerSec = casts[DamageAbility.SoC] / (double)fightlength;
        }

        private void DoInq()
        {
            if ((remainingCd[Ability.Inquisition] <= inqRefresh) && (HasHolyPower(3)))
            {
                inquptime += skills[DamageAbility.Inquisition].GetCooldown() - (remainingCd[Ability.Inquisition] > 0f ? remainingCd[Ability.Inquisition] : 0f);
                DoCast(Ability.Inquisition);
                UseHolyPower(3);
                holyPowerDP += dpChance;
            }
        }

        private void DoCS()
        {
            //Cast Crusaderstrike
            if (remainingCd[Ability.CrusaderStrike] <= 0f)
            {
                DoCast(Ability.CrusaderStrike);
                holyPower += 1;
            }
        }

        private bool DoFiller()
        {
            //Cast Templar's Verdict
            if (HasHolyPower(3))
            {
                DoCast(Ability.TemplarsVerdict);
                UseHolyPower(3);
                holyPowerDP += dpChance;
            } else
            //Cast Hammer of Wrath
            if (below20 && remainingCd[Ability.HammerOfWrath] <= 0f)
            {
                DoCast(Ability.HammerOfWrath);
                holyPowerDP += dpChance;
            } else
            //Cast Exo
            if (remainingCd[Ability.Exorcism] <= 0f)
            {
                DoCast(Ability.Exorcism);
                holyPowerDP += dpChance;
            } else
            //Cast Judge
            if (remainingCd[Ability.Judgement] <= 0f)
            {
                DoCast(Ability.Judgement);
                holyPowerDP += dpChance;
            } else
            //Cast Holy Wrath
            if (remainingCd[Ability.HolyWrath] <= 0f)
            {
                DoCast(Ability.HolyWrath);
                holyPowerDP += dpChance;
            } else
            //Cast Cons
            if (remainingCd[Ability.Consecration] <= 0f)
            {
                DoCast(Ability.Consecration);
            } else 
            {
                //Wait till one cd is ready
                DoNothing();
                return false;
            }
            return true;
        }

        private void DoNothing()
        {
            TriggerCD(GetLowestCd());
        }

        private float GetLowestCd()
        {
            float lCd = 100;
            foreach (KeyValuePair<Ability, float> kvp in remainingCd)
            {
                if (kvp.Value >= 0 && kvp.Value < lCd && kvp.Key != Ability.Inquisition)
                    lCd = kvp.Value;
            }
            return lCd;
        }

        private void DoCast(Ability ability)
        {
            casts[(DamageAbility)ability] += 1f;
            remainingCd[ability] = skills[(DamageAbility)ability].GetCooldown();
            TriggerCD(skills[(DamageAbility)ability].GetGCD() + latency);
        }

        private void TriggerCD(float CD)
        {
            time += CD;
            foreach (Ability abi in allAb)
                remainingCd[abi] -= CD;
        }

        private bool HasHolyPower(int ReqHP)
        {
            if (holyPowerDP > 1f)
                return true;
            else
                return holyPower >= ReqHP;
        }

        private void UseHolyPower(int ReqHP)
        {
            if (holyPowerDP > 1f)
                holyPowerDP -= 1f;
            else
                holyPower -= ReqHP;
        }

        private Dictionary<Ability, float> remainingCd = new Dictionary<Ability,float>();
        private Dictionary<DamageAbility, float> casts = new Dictionary<DamageAbility, float>();
        private Dictionary<DamageAbility, Skill> skills = new Dictionary<DamageAbility, Skill>();

        public abstract void SetCharacterCalculations(CharacterCalculationsRetribution calc);

        public void SetDPS(CharacterCalculationsRetribution calc)
        {
            SetCharacterCalculations(calc);

            //calc.AverageSoVStack = AverageSoTStackSize();

            calc.WhiteSkill = White;
            calc.SealSkill = Seal;
            calc.SealDotSkill = SealDot;
            calc.CommandSkill = SoC;
            calc.JudgementSkill = Judge;
            calc.TemplarsVerdictSkill = TV;
            calc.HandOfLightTVSkill = HoLTV;
            calc.CrusaderStrikeSkill = CS;
            calc.HandOfLightCSSkill = HoLCS;
            calc.ConsecrationSkill = Cons;
            calc.ExorcismSkill = Exo;
            calc.HolyWrathSkill = HW;
            calc.HammerOfWrathSkill = HoW;

            calc.DPSPoints = DPS() + calc.OtherDPS;
        }

        public float DPS()
        {
            return
                White.GetDPS() +
                Seal.GetDPS() +
                SealDot.GetDPS() +
                SoC.GetDPS() +
                Judge.GetDPS() +
                CS.GetDPS() +
                HoLCS.GetDPS() +
                TV.GetDPS() +
                HoLTV.GetDPS() +
                Exo.GetDPS() +
                HW.GetDPS() +
                Cons.GetDPS() +
                HoW.GetDPS();
        }

        public double SealProcsPerSec(Skill seal)
        {
            if (seal.GetType() == typeof(SealOfTruth))
                return GetMeleeAttacksPerSec() + GetRangedAttacksPerSec() + GetAbilityHitsPerSecond(Exo);
            else if (seal.GetType() == typeof(SealOfCommand))
                return GetMeleeAttacksPerSec();
            else
                return GetMeleeAttacksPerSec();
        }

        public double SealDotProcPerSec(Skill seal)
        {
            if (seal.GetType() == typeof(SealOfTruth))
                return (3f / (1 + Combats.Stats.PhysicalHaste)) / 3f; 
            else
                return 0d;
        }
        
        public float GetCrusaderStrikeCD()
        {
            return (float) (1 / CS.UsagePerSec);
        }

        public float GetJudgementCD()
        {
            return (float) (1 / Judge.UsagePerSec);
        }

        public double GetAbilityHitsPerSecond(Skill skill)
        {
            return
                skill.UsagePerSec *
                skill.ChanceToLand() *
                skill.Targets() *
                skill.TickCount();
        }

        public double GetAbilityCritsPerSecond(Skill skill)
        {
            return
                skill.UsagePerSec *
                skill.ChanceToCrit() *
                skill.Targets() *
                skill.TickCount();
        }

        public double GetMeleeAttacksPerSec()
        {
            return
                GetAbilityHitsPerSecond(CS) +
                White.ChanceToLand() / Combats.AttackSpeed +
                GetAbilityHitsPerSecond(TV);
        }

        private double GetRangedAttacksPerSec()
        {
            return
                GetAbilityHitsPerSecond(Judge) +
                GetAbilityHitsPerSecond(HoW);
        }

        public double GetSpellAttacksPerSec()
        {
            return
                GetAbilityHitsPerSecond(Exo) +
                GetAbilityHitsPerSecond(HW) +
                GetAbilityHitsPerSecond(Cons) / Cons.TickCount();
        }

        public double GetPhysicalAttacksPerSec()
        {
            return
                GetMeleeAttacksPerSec() +
                GetRangedAttacksPerSec();
        }

        public double GetMeleeCritsPerSec()
        {
            return
                GetAbilityCritsPerSecond(CS) +
                White.ChanceToCrit() / Combats.AttackSpeed +
                GetAbilityCritsPerSecond(TV);
        }

        public double GetRangeCritsPerSec()
        {
            return
                GetAbilityCritsPerSecond(Judge) +
                GetAbilityCritsPerSecond(HoW);
        }

        public double GetSpellCritsPerSec()
        {
            return
                GetAbilityCritsPerSecond(Exo) +
                GetAbilityHitsPerSecond(HW) / HW.TickCount() +
                GetAbilityCritsPerSecond(Cons) / Cons.TickCount();
        }

        public double GetPhysicalCritsPerSec()
        {
            return
                GetMeleeCritsPerSec() +
                GetRangeCritsPerSec();
        }

        public double GetAttacksPerSec()
        {
            return
                GetMeleeAttacksPerSec() +
                GetRangedAttacksPerSec() +
                GetSpellAttacksPerSec();
        }
    }
}
