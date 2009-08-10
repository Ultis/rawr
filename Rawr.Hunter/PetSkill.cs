using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rawr.Hunter
{
    public class PetSkill
    {
        public double CD = 0; // not used, but referenced in old code
        public int Cost = 0; // not used, but referenced in old code
        public int Type = 0; // not used, but referenced in old code
        public int Talent = 0;  // not used, but referenced in old code

        public bool can_crit;
        public double cooldown;
        public int focus;
        public PetSkillType type;
        public int Min;
        public int Max;

        public PetSkill(bool can_crit, double cooldown, int focus, PetSkillType type)
        {
            this.can_crit = can_crit;
            this.cooldown = cooldown;
            this.focus = focus;
            this.type = type;
            Min = 0;
            Max = 0;
        }

        public PetSkill(bool can_crit, double cooldown, int focus, PetSkillType type, int min, int max)
        {
            this.can_crit = can_crit;
            this.cooldown = cooldown;
            this.focus = focus;
            this.type = type;
            Min = min;
            Max = max;
        }
    }

    public class PetSkillPriorityRotation
    {
        public List<PetSkillInstance> skills = new List<PetSkillInstance>();
        private CalculationOptionsHunter options;
        private Character character;

        private static Dictionary<PetAttacks, PetSkill> skillLibrary = new Dictionary<PetAttacks, PetSkill>() 
        { 
            // common skills
            {PetAttacks.Growl, new PetSkill(false, 5, 15, PetSkillType.NonDamaging)},
            {PetAttacks.Cower, new PetSkill(false, 20, 25, PetSkillType.NonDamaging)},

            // focus dumps
            {PetAttacks.Claw, new PetSkill(true, 1.25, 25, PetSkillType.FocusDump)},
            {PetAttacks.Bite, new PetSkill(true, 1.25, 25, PetSkillType.FocusDump)},
            {PetAttacks.Smack, new PetSkill(true, 1.25, 25, PetSkillType.FocusDump)},

            // movement skills
            {PetAttacks.Dive, new PetSkill(false, 32, 20, PetSkillType.NonDamaging)},
            {PetAttacks.Dash, new PetSkill(false, 32, 20, PetSkillType.NonDamaging)},
            {PetAttacks.Charge, new PetSkill(false, 25, 35, PetSkillType.Unique)},

            // family skills
            {PetAttacks.SonicBlast, new PetSkill(true, 80, 0, PetSkillType.SpellSpecial)},
            {PetAttacks.Swipe, new PetSkill(true, 5, 20, PetSkillType.PhysicalSpecial, 90, 126)},
            {PetAttacks.Snatch, new PetSkill(true, 60, 20, PetSkillType.Unique, 89, 125)}, // not sure what kind of damage this does...
            {PetAttacks.Gore, new PetSkill(true, 10, 20, PetSkillType.PhysicalSpecial, 122, 164)},
            {PetAttacks.DemoralizingScreech, new PetSkill(true, 10, 20, PetSkillType.PhysicalSpecial, 85, 129)},
            {PetAttacks.Rake, new PetSkill(true, 10, 20, PetSkillType.Damaging)},
            {PetAttacks.Prowl, new PetSkill(false, 10, 0, PetSkillType.NonDamaging)},
            {PetAttacks.FroststormBreath, new PetSkill(true, 10, 20, PetSkillType.SpellSpecial, 128, 172)},
            {PetAttacks.LavaBreath, new PetSkill(true, 10, 20, PetSkillType.SpellSpecial, 128, 172)},
            {PetAttacks.Pin, new PetSkill(false, 60, 0, PetSkillType.NonDamaging)},
            {PetAttacks.BadAttitude, new PetSkill(false, 120, 0, PetSkillType.NonDamaging)}, // technically a damaging ability, but only if you get hit
            {PetAttacks.MonstrousBite, new PetSkill(true, 10, 20, PetSkillType.PhysicalSpecial, 91, 123)},
            {PetAttacks.FireBreath, new PetSkill(false, 10, 20, PetSkillType.Unique)},
            {PetAttacks.Pummel, new PetSkill(false, 30, 20, PetSkillType.NonDamaging)},
            {PetAttacks.TendonRip, new PetSkill(true, 20, 20, PetSkillType.PhysicalSpecial, 49, 69)},
            {PetAttacks.SerenityDust, new PetSkill(false, 60, 0, PetSkillType.NonDamaging)},
            {PetAttacks.NetherShock, new PetSkill(true, 40, 0, PetSkillType.SpellSpecial, 64, 86)},
            {PetAttacks.SavageRend, new PetSkill(true, 60, 20, PetSkillType.Unique)},
            {PetAttacks.Ravage, new PetSkill(true, 40, 0, PetSkillType.PhysicalSpecial, 106, 150)},
            {PetAttacks.Stampede, new PetSkill(false, 60, 0, PetSkillType.NonDamaging)},
            {PetAttacks.ScorpidPoison, new PetSkill(false, 10, 20, PetSkillType.Unique)},
            {PetAttacks.PoisonSpit, new PetSkill(false, 10, 20, PetSkillType.Unique)},
            {PetAttacks.VenomWebSpray, new PetSkill(false, 120, 0, PetSkillType.Unique, 46, 68)},
            {PetAttacks.Web, new PetSkill(false, 120, 0, PetSkillType.NonDamaging)},
            {PetAttacks.SpiritStrike, new PetSkill(true, 10, 20, PetSkillType.Unique)},
            {PetAttacks.SporeCloud, new PetSkill(false, 10, 20, PetSkillType.SpellSpecial)},
            {PetAttacks.DustCloud, new PetSkill(false, 40, 20, PetSkillType.NonDamaging)}, // why does the spreadsheet think this causes damage?
            {PetAttacks.ShellShield, new PetSkill(false, 180, 0, PetSkillType.NonDamaging)},
            {PetAttacks.Warp, new PetSkill(false, 15, 0, PetSkillType.NonDamaging)},
            {PetAttacks.Sting, new PetSkill(true, 6, 20, PetSkillType.SpellSpecial, 64, 86)},
            {PetAttacks.LightningBreath, new PetSkill(true, 10, 20, PetSkillType.SpellSpecial, 80, 120)},
            {PetAttacks.FuriousHowl, new PetSkill(false, 40, 20, PetSkillType.NonDamaging)},
            {PetAttacks.AcidSpit, new PetSkill(true, 10, 20, PetSkillType.SpellSpecial, 124, 176)},

            // other?
            {PetAttacks.Thunderstomp, new PetSkill(true, 10, 20, PetSkillType.SpellSpecial, 236, 334)}, // check numbers
            {PetAttacks.Rabid, new PetSkill(false, 45, 0, PetSkillType.NonDamaging)},
            {PetAttacks.WolverineBite, new PetSkill(true, 0, 0, PetSkillType.Unique)},
            {PetAttacks.Bullheaded, new PetSkill(false, 180, 0, PetSkillType.NonDamaging)},
            {PetAttacks.CallOfTheWild, new PetSkill(false, 300, 0, PetSkillType.NonDamaging)},
            {PetAttacks.Taunt, new PetSkill(false, 180, 0, PetSkillType.NonDamaging)},
            {PetAttacks.RoarOfRecovery, new PetSkill(false, 180, 0, PetSkillType.NonDamaging)},
            {PetAttacks.RoarOfSacrifice, new PetSkill(false, 30, 0, PetSkillType.NonDamaging)},
            {PetAttacks.LickYourWounds, new PetSkill(false, 180, 0, PetSkillType.NonDamaging)},
            {PetAttacks.LastStand, new PetSkill(false, 360, 0, PetSkillType.NonDamaging)},
        };

        public PetSkillPriorityRotation(Character character, CalculationOptionsHunter options)
        {
            this.character = character;
            this.options = options;
        }

        public void AddSkill(PetAttacks skillType)
        {

            // don't add non-skills to the rotation
            if (skillType == PetAttacks.None) return;

            // check that this skill is not already in the rotation
            foreach (PetSkillInstance S in skills)
            {
                if (S.skillType == skillType) return;
            }

            // don't add skills that require a talent we are missing
            if (skillType == PetAttacks.Bullheaded && options.petBullheaded == 0) return;
            if (skillType == PetAttacks.CallOfTheWild && options.petCallOfTheWild == 0) return;
            if (skillType == PetAttacks.Dash && options.petDiveDash == 0) return;
            if (skillType == PetAttacks.Dive && options.petDiveDash == 0) return;
            if (skillType == PetAttacks.LastStand && options.petLastStand == 0) return;
            if (skillType == PetAttacks.LickYourWounds && options.petLickYourWounds == 0) return;
            if (skillType == PetAttacks.Rabid && options.petRabid == 0) return;
            if (skillType == PetAttacks.RoarOfRecovery && options.petRoarOfRecovery == 0) return;
            if (skillType == PetAttacks.RoarOfSacrifice && options.petRoarOfSacrifice == 0) return;
            if (skillType == PetAttacks.Taunt && options.petTaunt == 0) return;
            if (skillType == PetAttacks.WolverineBite && options.petWolverineBite == 0) return;

            // it looks good - create an instance wrapper
            skills.Add(new PetSkillInstance(this, skillType, skillLibrary[skillType]));
        }

        public double owlsFocus = 0;
        public double fpsGained = 0;

        public void calculateTimings()
        {
            CalculateSkillData();

            PetSkillInstance PrevSkill = null;
            foreach (PetSkillInstance S in skills)
            {
                S.CalculateTimings(PrevSkill);
                PrevSkill = S;
            }

            Debug.WriteLine("calc'd!");
        }

        private void CalculateSkillData()
        {
            // This function is called before we calculate timings, to allow
            // us to calculate cooldown and focus cost for the skill

            double longevityCooldownAdjust = 1 - character.HunterTalents.Longevity * 0.1;

            foreach (PetSkillInstance S in skills)
            {
                S.cooldown = S.skillData.cooldown;
                S.focus = S.skillData.focus;
                S.can_crit = S.skillData.can_crit;
                S.causes_kc_damage = true;

                if (S.skillType == PetAttacks.Dive) S.cooldown -= options.petDiveDash * 8;
                if (S.skillType == PetAttacks.Dash) S.cooldown -= options.petDiveDash * 8;

                if (S.skillData.type != PetSkillType.FocusDump) S.cooldown *= longevityCooldownAdjust;

                if (S.skillData.type == PetSkillType.NonDamaging) S.causes_kc_damage = false;
                if (S.skillType == PetAttacks.Charge) S.causes_kc_damage = false; // charge only causes damage once - maybe i will model this...
                if (S.skillType == PetAttacks.VenomWebSpray) S.causes_kc_damage = false; // only causes a tick
            }
        }

        public double dps;
        public double petSpecialFrequency = 0;
        public double kc_dps = 0;

        public void calculateDPS()
        {
            dps = 0;
            double crittableInverseSum = 0;

            foreach (PetSkillInstance S in skills)
            {
                dps += S.dps;
                kc_dps += S.kc_dps;

                crittableInverseSum += S.crittable_freq > 0 ? 1 / S.crittable_freq : 0;
            }

            petSpecialFrequency = crittableInverseSum > 0 ? 1 / crittableInverseSum : 0;
        }
    }

    // This class is used for building the priority list.
    // It contains a pointer to the skill it represents.
    // This object will hold all of the rotation calculations.

    public class PetSkillInstance
    {
        public PetAttacks skillType;
        public PetSkill skillData;
        private PetSkillPriorityRotation priorityRotation;

        public PetSkillInstance(PetSkillPriorityRotation priorityRotation, PetAttacks skillType, PetSkill skillData)
        {
            this.priorityRotation = priorityRotation;
            this.skillType = skillType;
            this.skillData = skillData;
        }

        private const double PetGCD = 1.25;

        public double focus = 0; // D
        public double cooldown = 0; // E
        public double damage = 0; // scattered all over 'Pet Calculations'

        public bool can_crit = false;
        public bool causes_kc_damage = false;

        public double dps = 0; // I
        public double possible_freq = 0; // K
        public double gcd_left = 0; // L
        public double gcd_needed = 0; // M
        public double gcd_used = 0; // N
        public double temp_frequency = 0; // O
        public double crittable_freq = 0; // P
        public double fps_needed = 0; // Q
        public double fps_available = 0; // R
        public double frequency = 0; // S (and F)
        public double kc_dps = 0; // T       

        public void CalculateTimings(PetSkillInstance PrevSkill)
        {
            possible_freq = cooldown - (cooldown % PetGCD) + ((cooldown % PetGCD) > 0 ? PetGCD : 0);

            gcd_left = 1;
            if (PrevSkill != null)
            {
                gcd_left = PrevSkill.gcd_left - PrevSkill.gcd_used;
            }

            gcd_needed = cooldown >= 30 ? 0 : possible_freq > 0 ? PetGCD / possible_freq : 0;
            
            gcd_used = 1 - (gcd_left - gcd_needed);
            if (PrevSkill != null)
            {
                gcd_used = gcd_left > gcd_needed ? gcd_needed : gcd_left;
            }

            temp_frequency = gcd_used > 0 ? PetGCD / gcd_used : cooldown >= 30 ? cooldown : 0;

            fps_needed = (1 - priorityRotation.owlsFocus) * (temp_frequency > 0 ? focus / temp_frequency : 0);

            fps_available = priorityRotation.fpsGained;
            if (PrevSkill != null)
            {
                fps_available = PrevSkill.fps_available - PrevSkill.fps_needed;
            }

            frequency = 0;
            if (fps_available > 0) frequency = temp_frequency * (fps_needed / fps_available);
            if (fps_available > fps_needed) frequency = temp_frequency;

            crittable_freq = can_crit ? frequency : 0;
        }

        public void CalculateDPS()
        {
            dps = frequency > 0 ? damage / frequency : 0;
            kc_dps = causes_kc_damage ? dps : 0;
        }
    }
}
