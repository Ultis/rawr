using System;

namespace Rawr.DPSWarr {
    public class CombatFactors {
        public CombatFactors(Character character, Stats stats) {
			_stats = stats;
			_mainHand = character.MainHand == null ? new Knuckles() : character.MainHand.Item;
			_offHand = character.OffHand == null ? null : character.OffHand.Item ;
            _talents = character.WarriorTalents;
            _calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            _characterRace = character.Race;
            _character = character;
        }
        #region Global Variables
        private Stats _stats;
        private Item _mainHand;
        private Item _offHand;
        private WarriorTalents _talents;
        private CalculationOptionsDPSWarr _calcOpts;
        private Character.CharacterRace _characterRace;
        private Character _character;
        
        public Item MainHand { get { return _mainHand; } }
        public Item OffHand { get { return _offHand; } }
        #endregion

        #region Weapon Damage Calcs
        #region Major Damage Factors
        public float DamageBonus {
            get {
                float bonus  = 1f + _stats.BonusDamageMultiplier;
                      bonus *= 1f + _stats.BonusPhysicalDamageMultiplier;
                      bonus *= 1f + _talents.WreckingCrew * 0.02f;
                return bonus;
            }
        }
        public float DamageReduction {
            get {
                float armorReduction;
                float arpenBuffs =
                    ((_character.MainHand != null && _character.MainHand.Type == Item.ItemType.TwoHandMace) ? _talents.MaceSpecialization * 0.03f : 0.00f) +
                    (!_calcOpts.FuryStance ? 0.1f : 0.0f);
                if(_calcOpts==null){
                    // you're supposed to pass the character level, not the target level.  GC misspoke.
				    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(_character.Level,(int)StatConversion.NPC_BOSS_ARMOR,_stats.ArmorPenetration,arpenBuffs,_stats.ArmorPenetrationRating)); // default is vs raid boss
                }else{
                    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(_character.Level,_calcOpts.TargetArmor,_stats.ArmorPenetration,arpenBuffs,_stats.ArmorPenetrationRating));
                }

                return armorReduction;
            }
        }
        public float HealthBonus {
            get {
                float bonus = 1f + _stats.BonusHealingReceived;
                return bonus;
            }
        }
        #endregion
        #region Normalized Weapon Damage
        public float NormalizedMhWeaponDmg { get { return CalcNormalizedWeaponDamage(MainHand, _stats); } }
        public float NormalizedOhWeaponDmg { get { return CalcNormalizedWeaponDamage(OffHand, _stats); } }
        private float CalcNormalizedWeaponDamage(Item weapon, Stats stats) {
            float baseDamage = weapon.Speed * weapon.DPS;
            baseDamage += stats.AttackPower / 14 * 3.3f;
            return baseDamage;
        }
        #endregion
        #region Average Weapon Damage
        public float AvgWeaponDmg(Item i, bool isMH) { return CalcAverageWeaponDamage(i, _stats, isMH); }
        public float AvgMhWeaponDmg { get { return CalcAverageWeaponDamage(MainHand,_stats,true); } }
        public float AvgOhWeaponDmg { get { return CalcAverageWeaponDamage(OffHand,_stats,false); } }
        private float CalcAverageWeaponDamage(Item weapon, Stats stats,bool isMH) {
            if(weapon==null){return 0f;}
            // removed the DamageBonus from here, as it was causing double dipping down the line.  Damage Bonus should be done
            // only at the absolute end of calculations to prevent this
            return ((stats.AttackPower / 14 + weapon.DPS) * weapon.Speed)
                * (!isMH ? 0.5f + _talents.DualWieldSpecialization * 0.025f : 1f);
        }
        #endregion
        #region Weapon Crit Damage
        public float BonusWhiteCritDmg {
            get {
                float baseCritDmg = (2f * (1f + _stats.BonusCritMultiplier) - 1f);
                baseCritDmg *= 1f + ((MainHand.Type == Item.ItemType.TwoHandAxe || MainHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0f);
                return baseCritDmg;
            }
        }
        public float BonusYellowCritDmg { get { return BonusWhiteCritDmg * (1f + _talents.Impale * 0.1f); } }
        #endregion
        #region Weapon Blocked Damage
        public float BonusWhBlockedDmg {
            get {
                float baseBlockedDmg = 1f;
                baseBlockedDmg *= 0.70f; // 70% damage
                return baseBlockedDmg;
            }
        }
        public float BonusYwBlockedDmg {
            get {
                return BonusWhBlockedDmg;
            }
        }
        #endregion
        #region Speed
        public float TotalHaste {
            get {
                float totalHaste = 1f + _stats.PhysicalHaste; // BloodFrenzy is handled in GetCharacterStats
                totalHaste      *= 1f + StatConversion.GetHasteFromRating(_stats.HasteRating,Character.CharacterClass.Warrior); // Multiplicative
                totalHaste      *= 1f + _talents.Flurry * 0.05f * CalcFlurryUptime(_stats);
                return totalHaste;
            }
        }
        public float MainHandSpeed { get { return MainHand.Speed / TotalHaste; } }
        public float OffHandSpeed { get { return (OffHand == null ? 0f : OffHand.Speed / TotalHaste); } }
        #endregion
        #endregion
        #region Attack Table
        #region Hit Rating
        public float HitPerc { get { return StatConversion.GetHitFromRating(_stats.HitRating, Character.CharacterClass.Warrior); } }
        #endregion
        #region Expertise Rating
        public float GetDPRfromExp(float Expertise) {return StatConversion.GetDodgeParryReducFromExpertise(Expertise, Character.CharacterClass.Warrior);}
        public static float GetRacialExpertiseFromWeapon(Character.CharacterRace r, Item weapon) {
            if(weapon != null){
                if      (r == Character.CharacterRace.Human) {
                    if (weapon.Type == Item.ItemType.OneHandSword || weapon.Type == Item.ItemType.OneHandMace
                        || weapon.Type == Item.ItemType.TwoHandSword || weapon.Type == Item.ItemType.TwoHandMace) {
                        return 3f;
                    }
                }else if(r == Character.CharacterRace.Dwarf) {
                    if (weapon.Type == Item.ItemType.OneHandMace || weapon.Type == Item.ItemType.TwoHandMace) {
                        return 5f;
                    }
                }else if(r == Character.CharacterRace.Orc) {
                    if (weapon.Type == Item.ItemType.OneHandAxe || weapon.Type == Item.ItemType.TwoHandAxe) {
                        return 5f;
                    }
                }
            }
            return 0f;
        }
        private float CalcExpertise(Item weapon) {
            if (weapon == null || weapon.MaxDamage == 0f) { return 0f; }
            float baseExpertise = _stats.Expertise;
            baseExpertise += StatConversion.GetExpertiseFromRating(_stats.ExpertiseRating);
            baseExpertise += GetRacialExpertiseFromWeapon(_characterRace,weapon);
            return baseExpertise;
        }
        public float MhExpertise { get { return CalcExpertise(MainHand); } }
        public float OhExpertise { get { return CalcExpertise(OffHand ); } }
        #endregion

        #region Miss
        public float MissPrevBonuses {
            get {
                return _stats.PhysicalHit        // Hit Perc bonuses like Draenei Racial
                        + HitPerc;               // Bonus from Hit Rating
            }
        }
        public float WhMissChance {
            get {
                float missChance =
                    // Determine which cap to use
                    (_talents.TitansGrip == 1f && OffHand != null
                        && MainHand.Slot == Item.ItemSlot.TwoHand
                        && OffHand.Slot == Item.ItemSlot.TwoHand ?
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
            DodgeChance -= _talents.WeaponMastery / 100f;
            return (float)Math.Max(0f,DodgeChance);
        }
        #endregion
        #region Parry
        public float MhParryChance  { get { return CalcParryChance(MhExpertise); } }
        public float OhParryChance  { get { return CalcParryChance(OhExpertise); } }
        public float CalcParryChance(float Expertise) {
            float ParryChance = StatConversion.WHITE_PARRY_CHANCE_CAP - GetDPRfromExp(Expertise);
            return (float)Math.Max(0f, _calcOpts.InBack ? ParryChance * (1f - _calcOpts.InBackPerc/100f) : ParryChance);
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
            return (float)Math.Max(0f, _calcOpts.InBack ? BlockChance * (1f - _calcOpts.InBackPerc / 100f) : BlockChance);
        }
        #endregion
        #region Crit
        /*public float CalcCrit(Item weapon) {
            if (weapon == null || weapon.MaxDamage == 0f) { return 0f; }
            float crit = _stats.PhysicalCrit + StatConversion.GetCritFromRating(_stats.CritRating);
            crit += (weapon.Type == Item.ItemType.TwoHandAxe || weapon.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0f;
            return crit;
        }*/
        /* Commenting this out, because yellow uses 2-roll attack
         * private float CalcYellowCrit(Item weapon) {
            if (weapon == null || weapon.MaxDamage == 0f) { return 0f; }
            float crit = _stats.PhysicalCrit + StatConversion.GetCritFromRating(_stats.CritRating);
            crit += (weapon.Type == Item.ItemType.TwoHandAxe || weapon.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0f;
            crit *= (1f - YwMissChance - MhDodgeChance);
            return crit;
        }*/
        public float MhCrit
        {
            get
            {
                if (MainHand == null || MainHand.MaxDamage == 0f) { return 0f; }
                float crit = _stats.PhysicalCrit + StatConversion.GetCritFromRating(_stats.CritRating);
                crit += (MainHand.Type == Item.ItemType.TwoHandAxe || MainHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0f;
                return crit;
                //return CalcCrit(MainHand); 
            }
        }
        public float MhYellowCrit
        {
            get
            {
                if (MainHand == null || MainHand.MaxDamage == 0f) { return 0f; }
                float crit = _stats.PhysicalCrit + StatConversion.GetCritFromRating(_stats.CritRating);
                crit += (MainHand.Type == Item.ItemType.TwoHandAxe || MainHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0f;
                crit *= (1f - YwMissChance - MhDodgeChance);
                return crit;
                //return CalcYellowCrit(MainHand);
            }
        }
        public float OhCrit
        {
            get
            {
                if (OffHand == null || OffHand.MaxDamage == 0f) { return 0f; }
                float crit = _stats.PhysicalCrit + StatConversion.GetCritFromRating(_stats.CritRating);
                crit += (OffHand.Type == Item.ItemType.TwoHandAxe || OffHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0f;
                return crit;
                //return CalcCrit(OffHand);
            }
        }
        public float OhYellowCrit
        {
            get
            {
                if (OffHand == null || OffHand.MaxDamage == 0f) { return 0f; }
                float crit = _stats.PhysicalCrit + StatConversion.GetCritFromRating(_stats.CritRating);
                crit += (OffHand.Type == Item.ItemType.TwoHandAxe || OffHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0f;
                crit *= (1f - YwMissChance - OhDodgeChance);
                return crit;
                //return CalcYellowCrit(OffHand);
            }
        }
        #endregion
        #region Chance of Hitting (be it Ordinary, Glance or Blocked, but not Crit)
        // White
        //public float ProbWhiteHit(Item i)  { float exp = CalcExpertise(i); return 1f - WhMissChance - CalcCrit(i) - CalcDodgeChance(exp) - CalcParryChance(exp); }
        public float ProbMhWhiteHit  { get { return 1f - WhMissChance - MhCrit - MhDodgeChance - MhParryChance; } }
        public float ProbOhWhiteHit  { get { return 1f - WhMissChance - OhCrit - OhDodgeChance - MhParryChance; } }
        // Yellow (Doesn't Glance and has different MissChance Cap)
        //public float ProbYellowHit(Item i) { float exp = CalcExpertise(i); return 1f - YwMissChance - CalcCrit(i) - CalcDodgeChance(exp) - CalcParryChance(exp); }
        public float ProbMhYellowHit { get { return 1f - YwMissChance - MhCrit - MhDodgeChance - MhParryChance - MhBlockChance; } }
        public float ProbOhYellowHit { get { return 1f - YwMissChance - OhCrit - OhDodgeChance - OhParryChance - OhBlockChance; } }
        #endregion
        #endregion
        #region Other
        private float CalcFlurryUptime(Stats stats) {
            float uptime = 1f;
            float OHSpeed = (OffHand == null ? 1f : OffHand.Speed);
            float weaponDiff = OHSpeed / MainHand.Speed;
            float mhpercent = weaponDiff/(1f+weaponDiff);
            float ohpercent = 1f - mhpercent;
            float consumeRate = (1f + _talents.Flurry * 0.05f)
                              * (1f + StatConversion.GetHasteFromRating(_stats.HasteRating,Character.CharacterClass.Warrior))
                              * (1f + _stats.PhysicalHaste)
                              * (1f / MainHand.Speed + 1f / OHSpeed);

            float BTperSec = 0.1875f;
            float WWperSec = 0.1250f;

            uptime  = (float)System.Math.Pow(1f - MhCrit, 1f * mhpercent * 3f);
            uptime *= (float)System.Math.Pow(1f - MhCrit, 0f * mhpercent * 3f);

            uptime *= (float)System.Math.Pow(1f - OhCrit,      ohpercent * 3f);

            uptime *= (float)System.Math.Pow(1f - MhCrit, 3f / consumeRate * (BTperSec * (1f + MhCrit) + WWperSec));
            uptime *= (float)System.Math.Pow(1f - OhCrit, 3f / consumeRate * WWperSec);

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
