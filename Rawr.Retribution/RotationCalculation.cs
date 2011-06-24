using System;
using System.Collections.Generic;

namespace Rawr.Retribution
{
    struct PartRotationInfo
    {
        public Double Time;
        public float NormGCD;
        public bool Below20;
        public bool Zeal;

        public PartRotationInfo (Double time, float normGCD, bool below20, bool zeal) {
            Time = time;
            NormGCD = normGCD;
            Below20 = below20;
            Zeal = zeal;
        }
    }

    public class RotationCalculation
    {
        public Skill CS { get { return skills[DamageAbility.CrusaderStrike]; } }
        public Skill TV { get { return skills[DamageAbility.TemplarsVerdict]; } }
        public Skill Judge { get { return skills[DamageAbility.Judgement]; } }
        public Skill Exo { get { return skills[DamageAbility.Exorcism]; } }
        public Skill HW { get { return skills[DamageAbility.HolyWrath]; } }
        public Skill HoW { get { return skills[DamageAbility.HammerOfWrath]; } }
        public Skill Cons { get { return skills[DamageAbility.Consecration]; } }
        public Skill Seal { get { return skills[DamageAbility.Seal]; } }
        public Skill SealDot { get { return skills[DamageAbility.SealDot]; } }
        public Skill SoC { get { return skills[DamageAbility.SoC]; } }
        public Skill GoaK { get { return skills[DamageAbility.GoaK]; } }
        public White White { get { return (White)skills[DamageAbility.White]; } }

        private Dictionary<DamageAbility, float> casts = new Dictionary<DamageAbility, float>();
        private Dictionary<DamageAbility, Skill> skills = new Dictionary<DamageAbility, Skill>();

        public StatsRetri Stats { get; private set; }
        public Character Character { get; private set; }
        public CalculationOptionsRetribution CalcOpts { get; private set; }

        public RotationCalculation(Character character, StatsRetri stats)
        {
            Character = character;
            Stats = stats;
            CalcOpts = character.CalculationOptions as CalculationOptionsRetribution;
            dpChance = character.PaladinTalents.DivinePurpose * PaladinConstants.DP_CHANCE;

            #region Initialization
            casts[DamageAbility.Consecration] = 0f;
            casts[DamageAbility.CrusaderStrike] = 0f;
            casts[DamageAbility.Exorcism] = 0f;
            casts[DamageAbility.Inquisition] = 0f;
            casts[DamageAbility.HammerOfWrath] = 0f;
            casts[DamageAbility.HolyWrath] = 0f;
            casts[DamageAbility.Judgement] = 0f;
            casts[DamageAbility.TemplarsVerdict] = 0f;
            casts[DamageAbility.GoaK] = 0f;

            skills[DamageAbility.CrusaderStrike] = new CrusaderStrike(Character, Stats);
            skills[DamageAbility.TemplarsVerdict] = new TemplarsVerdict(Character, Stats);
            skills[DamageAbility.White] = new White(Character, Stats);
            skills[DamageAbility.Exorcism] = new Exorcism(Character, Stats, White.CT.ChanceToLand);
            skills[DamageAbility.Inquisition] = new Inquisition(Character, Stats, CalcOpts.HPperInq);
            skills[DamageAbility.HolyWrath] = new HolyWrath(Character, Stats);
            skills[DamageAbility.HammerOfWrath] = new HammerOfWrath(Character, Stats);
            skills[DamageAbility.Consecration] = new Consecration(Character, Stats);
            skills[DamageAbility.GoaK] = new GuardianOfTheAncientKings(Character, Stats);
            skills[DamageAbility.SoC] = new SealOfCommand(Character, Stats);

            switch (CalcOpts.Seal)
            {
                case SealOf.Righteousness:
                    skills[DamageAbility.Seal] = new SealOfRighteousness(Character, Stats);
                    skills[DamageAbility.SealDot] = new NullSealDoT(Character, Stats);
                    skills[DamageAbility.Judgement] = new JudgementOfRighteousness(Character, Stats);
                    break;
                case SealOf.Truth:
                    skills[DamageAbility.Seal] = new SealOfTruth(Character, Stats);
                    skills[DamageAbility.SealDot] = new SealOfTruthDoT(Character, Stats, 5f);
                    skills[DamageAbility.Judgement] = new JudgementOfTruth(Character, Stats, 5f);
                    break;
                default:
                    skills[DamageAbility.Seal] = new NullSeal(Character, Stats);
                    skills[DamageAbility.SealDot] = new NullSealDoT(Character, Stats);
                    skills[DamageAbility.Judgement] = new NullJudgement(Character, Stats);
                    break;
            }
            #endregion

            CalcRotation();
        }

        #region Rotation
        private readonly Ability[] allAb = { Ability.Consecration, Ability.CrusaderStrike, Ability.Exorcism, Ability.HammerOfWrath, Ability.HolyWrath, Ability.Inquisition, Ability.Judgement, Ability.TemplarsVerdict };
        private float dpChance;

        private void DoRotation(double fightlength, float normGCD, bool below20, bool zeal, Dictionary<DamageAbility, float> tmpCast)
        {
            float old_holyPower; 
            float holyPower = 0f;
            float holyPowerDP;
            double numOfGCD = fightlength / normGCD;
            
            int iterator = 0;
            while (iterator < 10)
            {
                double remainingNumOfGCD = numOfGCD;
                old_holyPower = holyPower;
                holyPower = 0f;

                holyPowerDP = (tmpCast[DamageAbility.Inquisition] +
                               tmpCast[DamageAbility.TemplarsVerdict] +
                               tmpCast[DamageAbility.Exorcism] +
                               tmpCast[DamageAbility.Judgement] +
                               tmpCast[DamageAbility.HolyWrath])
                              * dpChance;
                float addHPWrapUpTime = Math.Max(CalcOpts.HPperInq / (zeal ? 3f : 1f), 1f)* CS.CooldownWithLatency;
                float dpProcWrapUpTime = (float) fightlength / holyPowerDP;
                addHPWrapUpTime = dpProcWrapUpTime / (1f + dpProcWrapUpTime / addHPWrapUpTime);

                //Inq has the highest priority
                if (old_holyPower > 0f) {
                    tmpCast[DamageAbility.Inquisition] = (float)fightlength / (skills[DamageAbility.Inquisition].CooldownWithLatency - CalcOpts.InqRefresh + addHPWrapUpTime);
                    remainingNumOfGCD -= skills[DamageAbility.Inquisition].GCDPercentage * tmpCast[DamageAbility.Inquisition];
                    holyPower -= tmpCast[DamageAbility.Inquisition] * CalcOpts.HPperInq;
                }

                //Do Holy Power TV
                if (old_holyPower > 0f) {
                    tmpCast[DamageAbility.TemplarsVerdict] = (old_holyPower / 3f);
                    remainingNumOfGCD -= skills[DamageAbility.TemplarsVerdict].GCDPercentage * tmpCast[DamageAbility.TemplarsVerdict];
                }

                //Do CS
                tmpCast[DamageAbility.CrusaderStrike] = (float)fightlength / skills[DamageAbility.CrusaderStrike].CooldownWithLatency;
                remainingNumOfGCD -= skills[DamageAbility.CrusaderStrike].GCDPercentage * tmpCast[DamageAbility.CrusaderStrike];
                holyPower += tmpCast[DamageAbility.CrusaderStrike] * (zeal ? 3f : 1f) * skills[DamageAbility.CrusaderStrike].CT.ChanceToLand;

                //Do HoW
                if (below20) {
                    tmpCast[DamageAbility.HammerOfWrath] = (float)fightlength / skills[DamageAbility.HammerOfWrath].CooldownWithLatency;
                    remainingNumOfGCD -= skills[DamageAbility.HammerOfWrath].GCDPercentage * tmpCast[DamageAbility.HammerOfWrath];
                }

                //Do DP TV
                if (holyPowerDP > 0f) {
                    tmpCast[DamageAbility.TemplarsVerdict] += holyPowerDP;
                    remainingNumOfGCD -= skills[DamageAbility.TemplarsVerdict].GCDPercentage * holyPowerDP;
                }

                //Do Exo
                if (remainingNumOfGCD > 0f) {
                    tmpCast[DamageAbility.Exorcism] = (float)fightlength / skills[DamageAbility.Exorcism].CooldownWithLatency;
                    remainingNumOfGCD -= skills[DamageAbility.Exorcism].GCDPercentage * tmpCast[DamageAbility.Exorcism];
                }

                //Do Judge
                if (remainingNumOfGCD > 0f) {
                    tmpCast[DamageAbility.Judgement] = (float)Math.Min(remainingNumOfGCD, fightlength / skills[DamageAbility.Judgement].CooldownWithLatency);
                    remainingNumOfGCD -= skills[DamageAbility.Judgement].GCDPercentage * tmpCast[DamageAbility.Judgement];
                }

                //Do HW
                if (remainingNumOfGCD > 0f) {
                    tmpCast[DamageAbility.HolyWrath] = (float)Math.Min(remainingNumOfGCD, fightlength / skills[DamageAbility.HolyWrath].CooldownWithLatency);
                    remainingNumOfGCD -= skills[DamageAbility.HolyWrath].GCDPercentage * tmpCast[DamageAbility.HolyWrath];
                }
                iterator++;
            }
        }

        public void CalcRotation()
        {
            float normGCD = (1.5f + .1f);
            float lostTime = Impedance.GetTotalImpedancePercs(Character.BossOptions, PLAYER_ROLES.MeleeDPS, Stats.MovementSpeed, Stats.FearDurReduc, Stats.StunDurReduc, 
                                                                                                            Stats.SnareRootDurReduc, Stats.SilenceDurReduc);
            float fightlength = Character.BossOptions.BerserkTimer;
            float fightLengthAttacking = fightlength * (1f - lostTime);
            float timeZeal = (fightlength / PaladinConstants.ZEAL_COOLDOWN) * (PaladinConstants.ZEAL_DURATION + (Stats.T12_4P ? 15f : 0f));

            PartRotationInfo[] infos = new PartRotationInfo[] { new PartRotationInfo((fightLengthAttacking - timeZeal) * (1d - Character.BossOptions.Under20Perc), normGCD, false, false), //Above 20, no Zeal
                                                                new PartRotationInfo(timeZeal * (1d - Character.BossOptions.Under20Perc),                          normGCD, false, true),  //Above 20, Zeal 
                                                                new PartRotationInfo((fightLengthAttacking - timeZeal) * Character.BossOptions.Under20Perc,        normGCD, true,  false), //Under 20, no Zeal 
                                                                new PartRotationInfo(timeZeal * Character.BossOptions.Under20Perc,                                 normGCD, true,  true) };//Under 20, Zeal             

            Dictionary<DamageAbility, float> tmpCasts = new Dictionary<DamageAbility, float>();
            foreach (DamageAbility abil in allAb)
            {
                tmpCasts.Add(abil, 0f);
            }
            foreach (PartRotationInfo info in infos) 
            {
                DoRotation(info.Time, info.NormGCD, info.Below20, info.Zeal, tmpCasts);
                foreach (DamageAbility abil in allAb)
                {
                    casts[abil] += tmpCasts[abil];
                    tmpCasts[abil] = 0f;
                }
            }
            
            casts[DamageAbility.White] = fightLengthAttacking / AbilityHelper.WeaponSpeed(Character, Stats.PhysicalHaste);
            casts[DamageAbility.GoaK] = fightlength / PaladinConstants.GOAK_COOLDOWN;
            casts[DamageAbility.SoC] = casts[DamageAbility.Seal] = (float)(fightlength * SealProcsPerSec(Seal));
            casts[DamageAbility.SealDot] = (float)(fightlength * SealDotProcPerSec(Seal));

            float inquptime = Math.Min((casts[DamageAbility.Inquisition] * skills[DamageAbility.Inquisition].Cooldown) / fightLengthAttacking, 1f);

            //UsagePerSecCalc
            foreach (KeyValuePair<DamageAbility, Skill> kvp in skills)
            {
                kvp.Value.UsagePerSec = casts[kvp.Key] / (double)fightlength;
                kvp.Value.InqUptime = inquptime;
            }
            //Seals
            casts[DamageAbility.SoC] = casts[DamageAbility.Seal] = (float)(fightlength * SealProcsPerSec(Seal));
            skills[DamageAbility.SoC].UsagePerSec = skills[DamageAbility.Seal].UsagePerSec = casts[DamageAbility.Seal] / (double)fightlength;
        }
        #endregion

        public void SetDPS(CharacterCalculationsRetribution calc)
        {
            calc.WhiteSkill = White;
            calc.SealSkill = Seal;
            calc.SealDotSkill = SealDot;
            calc.CommandSkill = SoC;
            calc.JudgementSkill = Judge;
            calc.TemplarsVerdictSkill = TV;
            calc.CrusaderStrikeSkill = CS;
            calc.ConsecrationSkill = Cons;
            calc.ExorcismSkill = Exo;
            calc.HolyWrathSkill = HW;
            calc.HammerOfWrathSkill = HoW;
            calc.GoakSkill = GoaK;

            calc.DPSPoints = White.GetDPS() +
                Seal.GetDPS() +
                SealDot.GetDPS() +
                SoC.GetDPS() +
                Judge.GetDPS() +
                CS.GetDPS() +
                TV.GetDPS() +
                Exo.GetDPS() +
                HW.GetDPS() +
                Cons.GetDPS() +
                HoW.GetDPS() +
                GoaK.GetDPS() +
                calc.OtherDPS;
        }

        #region Ability per second
        public double SealProcsPerSec(Skill seal)
        {
            if (seal.GetType() == typeof(SealOfTruth))
                return GetMeleeAttacksPerSec() + GetRangedAttacksPerSec() + GetAbilityHitsPerSecond(Exo);
            if (seal.GetType() == typeof(SealOfRighteousness))
                return GetMeleeAttacksPerSec() + GetAbilityHitsPerSecond(HoW);
            return 0d;
        }

        public double SealDotProcPerSec(Skill seal)
        {
            if (seal.GetType() == typeof(SealOfTruth))
                return 1 / (3d / (1 + Stats.PhysicalHaste));
            return 0d;
        }

        public double GetAbilityHitsPerSecond(Skill skill)
        {
            return
                skill.UsagePerSec *
                skill.CT.ChanceToLand *
                skill.AvgTargets *
                skill.TickCount;
        }

        public double GetAbilityCritsPerSecond(Skill skill)
        {
            return
                skill.UsagePerSec *
                skill.CT.ChanceToCrit *
                skill.AvgTargets *
                skill.TickCount;
        }

        public double GetMeleeAttacksPerSec()
        {
            return
                GetAbilityHitsPerSecond(CS) +
                GetAbilityHitsPerSecond(White) + 
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
                GetAbilityHitsPerSecond(SealDot);
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
                GetAbilityCritsPerSecond(White) + 
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
                GetAbilityCritsPerSecond(HW) +
                GetAbilityCritsPerSecond(SealDot);
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
        #endregion
    }
}
