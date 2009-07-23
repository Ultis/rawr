using System;

namespace Rawr.DPSWarr {
    public class CombatFactors {
        public CombatFactors(Character character, Stats stats) {
			StatS = stats;
			MH = character.MainHand == null ? new Knuckles() : character.MainHand.Item;
			OH = character.OffHand == null ? null : character.OffHand.Item ;
            Talents = character.WarriorTalents;
            CalcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            Char = character;
        }
        #region Global Variables
        private Stats StatS;
        private WarriorTalents Talents;
        private CalculationOptionsDPSWarr CalcOpts;
        private Character Char;
        public Item MH;
        public Item OH;
        #endregion

        #region Weapon Damage Calcs
        #region Major Damage Factors
        public float DamageBonus {
            get {
                float bonus  = 1f + StatS.BonusDamageMultiplier;
                      bonus *= 1f + StatS.BonusPhysicalDamageMultiplier;
                      bonus *= 1f + Talents.WreckingCrew * 0.02f;
                return bonus;
            }
        }
        public float DamageReduction {
            get {
                float armorReduction;
                float arpenBuffs =
                    ((Char.MainHand != null && Char.MainHand.Type == ItemType.TwoHandMace) ? Talents.MaceSpecialization * 0.03f : 0.00f) +
                    (!CalcOpts.FuryStance ? 0.1f : 0.0f);
                if(CalcOpts==null){
                    // you're supposed to pass the character level, not the target level.  GC misspoke.
				    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level,(int)StatConversion.NPC_BOSS_ARMOR,StatS.ArmorPenetration,arpenBuffs,StatS.ArmorPenetrationRating)); // default is vs raid boss
                }else{
                    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(Char.Level,CalcOpts.TargetArmor,StatS.ArmorPenetration,arpenBuffs,StatS.ArmorPenetrationRating));
                }

                return armorReduction;
            }
        }
        public float HealthBonus {
            get {
                float bonus = 1f + StatS.BonusHealingReceived;
                return bonus;
            }
        }
        #endregion
        #region Normalized Weapon Damage
        public float NormalizedMhWeaponDmg { get { return CalcNormalizedWeaponDamage(MH); } }
        public float NormalizedOhWeaponDmg { get { return CalcNormalizedWeaponDamage(OH); } }
        private float CalcNormalizedWeaponDamage(Item weapon) {
            float baseDamage = weapon.Speed * weapon.DPS;
            baseDamage += StatS.AttackPower / 14f * 3.3f;
            return baseDamage;
        }
        #endregion
        #region Average Weapon Damage
        public float AvgMhWeaponDmg { get { return CalcAverageWeaponDamage(MH, true); } }
        public float AvgOhWeaponDmg { get { return CalcAverageWeaponDamage(OH, false); } }
        public float AvgWeaponDmg(Item i, bool isMH) { return CalcAverageWeaponDamage(i, isMH); }
        private float CalcAverageWeaponDamage(Item weapon, bool isMH) {
            if(weapon==null){return 0f;}
            // removed the DamageBonus from here, as it was causing double dipping down the line.  Damage Bonus should be done
            // only at the absolute end of calculations to prevent this
            return ((StatS.AttackPower / 14f + weapon.DPS) * weapon.Speed)
                * (!isMH ? 0.5f + Talents.DualWieldSpecialization * 0.025f : 1f);
        }
        #endregion
        #region Weapon Crit Damage
        public float BonusWhiteCritDmg {
            get {
                float baseCritDmg = (2f * (1f + StatS.BonusCritMultiplier) - 1f);
                baseCritDmg *= 1f + ((MH.Type == ItemType.TwoHandAxe || MH.Type == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f);
                return baseCritDmg;
            }
        }
        public float BonusYellowCritDmg { get { return BonusWhiteCritDmg * (1f + Talents.Impale * 0.1f); } }
        #endregion
        #region Weapon Blocked Damage
        public float ReducWhBlockedDmg {
            get {
                float baseBlockedDmg = 0.70f;// 70% damage
                return baseBlockedDmg;
            }
        }
        public float ReducYwBlockedDmg { get { return ReducWhBlockedDmg; } }
        #endregion
        #region Weapon Glanced Damage
        public float ReducWhGlancedDmg {
            get {
                float baseGlancedDmg = 0.70f;// 70% damage
                return baseGlancedDmg;
            }
        }
        #endregion
        #region Speed
        public float TotalHaste {
            get {
                float totalHaste = 1f + StatS.PhysicalHaste; // BloodFrenzy is handled in GetCharacterStats
                totalHaste      *= 1f + StatConversion.GetHasteFromRating(StatS.HasteRating,CharacterClass.Warrior); // Multiplicative
                totalHaste      *= 1f + Talents.Flurry * 0.05f * CalcFlurryUptime(StatS);
                return totalHaste;
            }
        }
        public float MHSpeed { get { return MH.Speed / TotalHaste; } }
        public float OHSpeed { get { return (OH == null ? 0f : OH.Speed / TotalHaste); } }
        #endregion
        #endregion
        #region Attack Table
        #region Hit Rating
        public float HitPerc { get { return StatConversion.GetHitFromRating(StatS.HitRating, CharacterClass.Warrior); } }
        #endregion
        #region Expertise Rating
        public float GetDPRfromExp(float Expertise) {return StatConversion.GetDodgeParryReducFromExpertise(Expertise, CharacterClass.Warrior);}
        public static float GetRacialExpertiseFromWeapon(CharacterRace r, Item weapon) {
            if(weapon != null){
                if      (r == CharacterRace.Human) {
                    if (weapon.Type == ItemType.OneHandSword || weapon.Type == ItemType.OneHandMace
                        || weapon.Type == ItemType.TwoHandSword || weapon.Type == ItemType.TwoHandMace) {
                        return 3f;
                    }
                }else if(r == CharacterRace.Dwarf) {
                    if (weapon.Type == ItemType.OneHandMace || weapon.Type == ItemType.TwoHandMace) {
                        return 5f;
                    }
                }else if(r == CharacterRace.Orc) {
                    if (weapon.Type == ItemType.OneHandAxe || weapon.Type == ItemType.TwoHandAxe) {
                        return 5f;
                    }
                }
            }
            return 0f;
        }
        private float CalcExpertise(Item weapon) {
            if (weapon == null || weapon.MaxDamage == 0f) { return 0f; }
            float baseExpertise = StatS.Expertise;
            baseExpertise += StatConversion.GetExpertiseFromRating(StatS.ExpertiseRating);
            baseExpertise += GetRacialExpertiseFromWeapon(Char.Race,weapon);
            return baseExpertise;
        }
        public float MhExpertise { get { return CalcExpertise(MH); } }
        public float OhExpertise { get { return CalcExpertise(OH ); } }
        #endregion

        #region Miss
        public float MissPrevBonuses {
            get {
                return StatS.PhysicalHit        // Hit Perc bonuses like Draenei Racial
                        + HitPerc;               // Bonus from Hit Rating
            }
        }
        public float WhMissChance {
            get {
                float missChance =
                    // Determine which cap to use
                    (Talents.TitansGrip == 1f && OH != null
                        && MH.Slot == ItemSlot.TwoHand
                        && OH.Slot == ItemSlot.TwoHand ?
                       StatConversion.WHITE_MISS_CHANCE_CAP_DW : StatConversion.WHITE_MISS_CHANCE_CAP)
                    // Reduce the Perc by dees much
                       - MissPrevBonuses;
                return (float)Math.Max(0f, missChance); 
            }
        }
        public float YwMissChance { get { return (float)Math.Max(0f, StatConversion.YELLOW_MISS_CHANCE_CAP - MissPrevBonuses); } }
        #endregion
        #region Dodge
        public float MhDodgeChance  { get { return CalcDodgeChance(MhExpertise); } }
        public float OhDodgeChance  { get { return CalcDodgeChance(OhExpertise); } }
        public float CalcDodgeChance(float Expertise) {
            float DodgeChance = StatConversion.WHITE_DODGE_CHANCE_CAP - GetDPRfromExp(Expertise);
            DodgeChance -= Talents.WeaponMastery / 100f;
            return (float)Math.Max(0f,DodgeChance);
        }
        #endregion
        #region Parry
        public float MhParryChance  { get { return CalcParryChance(MhExpertise); } }
        public float OhParryChance  { get { return CalcParryChance(OhExpertise); } }
        public float CalcParryChance(float Expertise) {
            float ParryChance = StatConversion.WHITE_PARRY_CHANCE_CAP - GetDPRfromExp(Expertise);
            return (float)Math.Max(0f, CalcOpts.InBack ? ParryChance * (1f - CalcOpts.InBackPerc/100f) : ParryChance);
        }
        #endregion
        #region Glance
        public float GlanceChance { get { return StatConversion.WHITE_GLANCE_CHANCE_CAP; } }
        #endregion
        #region Block
        public float MhBlockChance  { get { return CalcBlockChance(); } }
        public float OhBlockChance  { get { return CalcBlockChance(); } }
        public float CalcBlockChance() {
            float BlockChance = StatConversion.WHITE_BLOCK_CHANCE_CAP;
            return (float)Math.Max(0f, CalcOpts.InBack ? BlockChance * (1f - CalcOpts.InBackPerc / 100f) : BlockChance);
        }
        #endregion
        #region Crit
        public float MhWhCritChance {
            get {
                if (MH == null || MH.MaxDamage == 0f) { return 0f; }
                float crit = StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating);
                crit += (MH.Type == ItemType.TwoHandAxe || MH.Type == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f;
                return crit;
            }
        }
        public float MhYwCritChance {
            get {
                if (MH == null || MH.MaxDamage == 0f) { return 0f; }
                float crit = StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating);
                crit += (MH.Type == ItemType.TwoHandAxe || MH.Type == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f;
                crit *= (1f - YwMissChance - MhDodgeChance);
                return crit;
            }
        }
        public float OhWhCritChance {
            get {
                if (OH == null || OH.MaxDamage == 0f) { return 0f; }
                float crit = StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating);
                crit += (OH.Type == ItemType.TwoHandAxe || OH.Type == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f;
                return crit;
            }
        }
        public float OhYwCritChance {
            get {
                if (OH == null || OH.MaxDamage == 0f) { return 0f; }
                float crit = StatS.PhysicalCrit + StatConversion.GetCritFromRating(StatS.CritRating);
                crit += (OH.Type == ItemType.TwoHandAxe || OH.Type == ItemType.Polearm) ? 0.01f * Talents.PoleaxeSpecialization : 0f;
                crit *= (1f - YwMissChance - OhDodgeChance);
                return crit;
            }
        }
        #endregion
        #region Chance of Hitting (be it Ordinary, Glance or Blocked, but not Crit)
        // White
        //public float ProbWhiteHit(Item i)  { float exp = CalcExpertise(i); return 1f - WhMissChance - CalcCrit(i) - CalcDodgeChance(exp) - CalcParryChance(exp); }
        public float ProbMhWhiteHit  { get { return 1f - WhMissChance - MhWhCritChance - MhDodgeChance - MhParryChance; } }
        public float ProbOhWhiteHit  { get { return 1f - WhMissChance - OhWhCritChance - OhDodgeChance - OhParryChance; } }
        // Yellow (Doesn't Glance and has different MissChance Cap)
        //public float ProbYellowHit(Item i) { float exp = CalcExpertise(i); return 1f - YwMissChance - CalcCrit(i) - CalcDodgeChance(exp) - CalcParryChance(exp); }
        public float ProbMhYellowHit { get { return 1f - YwMissChance - MhYwCritChance - MhDodgeChance - MhParryChance - MhBlockChance; } }
        public float ProbOhYellowHit { get { return 1f - YwMissChance - OhYwCritChance - OhDodgeChance - OhParryChance - OhBlockChance; } }
        #endregion
        #endregion
        #region Other
        private float CalcFlurryUptime(Stats stats) {
            float uptime = 1f;
            float OHSpeed = (OH == null ? 1f : OH.Speed);
            float weaponDiff = OHSpeed / MH.Speed;
            float mhpercent = weaponDiff/(1f+weaponDiff);
            float ohpercent = 1f - mhpercent;
            float consumeRate = (1f + Talents.Flurry * 0.05f)
                              * (1f + StatConversion.GetHasteFromRating(StatS.HasteRating,CharacterClass.Warrior))
                              * (1f + StatS.PhysicalHaste)
                              * (1f / MH.Speed + 1f / OHSpeed);

            float BTperSec = 0.1875f;
            float WWperSec = 0.1250f;

            uptime  = (float)System.Math.Pow(1f - MhWhCritChance, 1f * mhpercent * 3f);
            uptime *= (float)System.Math.Pow(1f - MhWhCritChance, 0f * mhpercent * 3f);

            uptime *= (float)System.Math.Pow(1f - OhWhCritChance, ohpercent * 3f);

            uptime *= (float)System.Math.Pow(1f - MhWhCritChance, 3f / consumeRate * (BTperSec * (1f + MhWhCritChance) + WWperSec));
            uptime *= (float)System.Math.Pow(1f - OhWhCritChance, 3f / consumeRate * WWperSec);

            uptime  = 1f - uptime;

            return uptime;
        }
        #endregion
        public class Knuckles : Item {
            public Knuckles() {
                Speed = 2f;
                MaxDamage = 0;
                MinDamage = 0;
            }
        }
    }
}
