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
        }
        #region Global Variables
        private Stats _stats;
        private Item _mainHand;
        private Item _offHand;
        private WarriorTalents _talents;
        private CalculationOptionsDPSWarr _calcOpts;
        private Character.CharacterRace _characterRace;

        public Item MainHand { get { return _mainHand; } }
        public Item OffHand { get { return _offHand; } }
        #endregion

        #region Major Damage Factors
        public float DamageBonus {
            get {
                return (1+_talents.TwoHandedWeaponSpecialization * 0.02f)*(1+_stats.BonusPhysicalDamageMultiplier)*
                        (1+_stats.BonusDamageMultiplier)*(1f+ (_talents.DeathWish*0.2f)*(30f/(180f*(1 - 0.11f*_talents.IntensifyRage))))*(1f+0.02f*_talents.WreckingCrew);
            }
        }
        public float DamageReduction {
            get {
                float armorReduction;
                if(_calcOpts==null){
				    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(80,12900,_stats.ArmorPenetration,0f,_stats.ArmorPenetrationRating));
                }else{
                    armorReduction = Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(_calcOpts.TargetLevel,_calcOpts.TargetArmor,_stats.ArmorPenetration,0f,_stats.ArmorPenetrationRating));
                }
                if (_talents.TitansGrip == 1 && (MainHand.Slot == Item.ItemSlot.TwoHand || OffHand.Slot == Item.ItemSlot.TwoHand))
                    armorReduction *= 0.9f;

                return armorReduction;
                //return (15232.5f / (EffectiveBossArmor + 15232.5f)) > 1f ? 1f : (15232.5f / (EffectiveBossArmor + 15232.5f)); 
            }
        }
        /*public float EffectiveBossArmor {
			get {
				float armorReductionPercent = (1f - _stats.ArmorPenetration) * (1f - _stats.ArmorPenetrationRating / 1539.529991f);
				float reducedArmor = (float)_calcOpts.TargetArmor * (armorReductionPercent);

				return reducedArmor;
				//float totalArmor = _calcOpts.TargetArmor;
				//totalArmor *= (MainHand.Type == Item.ItemType.TwoHandMace) ? 1 - (0.03f * _talents.MaceSpecialization) : 1.0f;
				//totalArmor -= _stats.ArmorPenetration;
				//totalArmor *= 1 - (_stats.ArmorPenetrationRating * WarriorConversions.ArPToArmorPenetration / 100f);
				//return totalArmor;
			}
		} */
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
        public float AvgWeaponDmg(Item i,bool mainoroff) { return CalcAverageWeaponDamage(i, _stats,mainoroff); }
        public float AvgMhWeaponDmg { get { return CalcAverageWeaponDamage(MainHand,_stats,true); } }
        public float AvgOhWeaponDmg { get { return CalcAverageWeaponDamage(OffHand,_stats,false); } }
        private float CalcAverageWeaponDamage(Item weapon, Stats stats,bool mainoroff) {
            if(weapon==null){return 0f;}
            return DamageBonus * ((stats.AttackPower / 14 + weapon.DPS) * weapon.Speed)
                * (!mainoroff ? 0.5f + _talents.DualWieldSpecialization * 0.025f : 1f);
        }
        #endregion
        #region Weapon Crit Damage
        public float BonusWhiteCritDmg
        {
            get
            {
                float baseCritDmg = (2 * (1f + _stats.BonusCritMultiplier) - 1);
                baseCritDmg *= (MainHand.Type == Item.ItemType.TwoHandAxe || MainHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 1;
                return (2 * (1f + _stats.BonusCritMultiplier) - 1) * (1 + _talents.PoleaxeSpecialization * 0.01f);
            }
        }
        public float BonusYellowCritDmg { get { return BonusWhiteCritDmg * (1 + _talents.Impale * 0.1f); } }
        #endregion
        #region Attack Table
        public float YellowMissChance { get { var missChance = 0.08f - HitPercent; return missChance < 0f ? 0f : missChance; } }
        public float WhiteMissChance {
            get {
                var missChance = (MainHand.Slot == Item.ItemSlot.TwoHand && _talents.TitansGrip != 1 ? 8f : 27f );
                missChance -= HitPercent;
                return missChance < 0f ? 0f : missChance/100; 
            }
        }
        public float CalcCrit(Item weapon)
        {
            if (weapon == null || weapon.MaxDamage == 0) { return 0f; }
            var crit = _stats.PhysicalCrit + StatConversion.GetCritFromRating(_stats.CritRating);

            crit += (weapon.Type == Item.ItemType.TwoHandAxe || MainHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0;

            return crit;
        }
        private float CalcYellowCrit(Item weapon)
        {
            if(weapon == null) {return 0f;}
            var crit = _stats.PhysicalCrit + StatConversion.GetCritFromRating(_stats.CritRating);
            crit *= (1 - YellowMissChance - MhDodgeChance);
            //if (_calcOpts != null) {
            //if (_calcOpts.TargetLevel == 83) { crit -= 0.048f; }
            //    if (_calcOpts.FuryStance) { crit += 0.03f; }
            //}
            //crit += 0.01f * _talents.Cruelty;

            crit += (weapon.Type == Item.ItemType.TwoHandAxe || MainHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0;

            return crit;
        }
        public float HitPercent { get { return _stats.PhysicalHit / 100f + StatConversion.GetHitFromRating(_stats.HitRating, Character.CharacterClass.Warrior); } }
        public float ProbWhiteHit(Item i) { return 1f - WhiteMissChance - CalcCrit(i) - CalcDodgeChance(CalcExpertise(i)) - GlanceChance; }
        public float ProbMhWhiteHit { get { return 1f - WhiteMissChance - MhCrit - MhDodgeChance - GlanceChance; } }
        public float ProbOhWhiteHit { get { return 1f - WhiteMissChance - OhCrit - OhDodgeChance - GlanceChance; } }
        public float MhCrit { get { return CalcCrit(MainHand); } }
        public float MhYellowCrit { get { return CalcYellowCrit(MainHand); } }
        public float OhCrit { get { return CalcCrit(OffHand); } }
        public float OhYellowCrit { get { return CalcYellowCrit(OffHand); } }
        public float GlanceChance { get { return 0.25f; } }
        public float MhDodgeChance { get { return CalcDodgeChance(MhExpertise); } }
        public float OhDodgeChance { get { return CalcDodgeChance(OhExpertise); } }
        public float CalcDodgeChance(float mhExpertise)
        {
            var mhDodgeChance = 0.065f - 0.0025f * mhExpertise;
            mhDodgeChance -= _talents.WeaponMastery/100f;
            if (mhDodgeChance < 0f) { mhDodgeChance = 0f; }
            return mhDodgeChance;
        }
        #endregion
        #region Other
        public float MhExpertise    { get { return CalcExpertise(MainHand); } }
        public float OhExpertise    { get { return CalcExpertise(OffHand); } }
        public float TotalHaste {
            get {   
                float flurryUptime = CalcFlurryUptime(_stats);
                float flurryHaste = _talents.Flurry * 0.05f * flurryUptime;
                var totalHaste = 1f;
                totalHaste *= (1f + flurryHaste) * (1f + StatConversion.GetHasteFromRating(_stats.HasteRating,Character.CharacterClass.Warrior));
                totalHaste *= 1f + _stats.PhysicalHaste;
                // BloodFrenzy is handled in GetCharacterStats
                return totalHaste;
            }
        }
        public float MainHandSpeed { get { return MainHand.Speed / TotalHaste; } }
        public float OffHandSpeed { get { return (OffHand == null ? 1f : OffHand.Speed / TotalHaste); } }
        public static float GetRacialExpertiseFromWeapon(Character.CharacterRace r, Item weapon) {
            if (r == Character.CharacterRace.Human) {
                if (weapon != null && (weapon.Type == Item.ItemType.OneHandSword || weapon.Type == Item.ItemType.OneHandMace
                    || weapon.Type == Item.ItemType.TwoHandSword || weapon.Type == Item.ItemType.TwoHandMace)) {
                    return 3f;
                }
            } else if (r == Character.CharacterRace.Dwarf) {
                if (weapon != null && (weapon.Type == Item.ItemType.OneHandMace || weapon.Type == Item.ItemType.TwoHandMace)) {
                    return 5f;
                }
            } else if (r == Character.CharacterRace.Orc) {
                if (weapon != null && (weapon.Type == Item.ItemType.OneHandAxe || weapon.Type == Item.ItemType.TwoHandAxe)) {
                    return 5f;
                }
            }
            return 0f;
        }
        private float CalcExpertise(Item weapon) {
            if (weapon == null|| weapon.MaxDamage == 0) { return 0f; }
            float baseExpertise = _stats.Expertise + StatConversion.GetExpertiseFromRating(_stats.ExpertiseRating);

            baseExpertise += GetRacialExpertiseFromWeapon(_characterRace,weapon);

            return baseExpertise;
        }
        private float CalcFlurryUptime(Stats stats) {
            float uptime = 1;
            float OHSpeed = (OffHand == null ? 1f : OffHand.Speed);
            float weaponDiff = OHSpeed / MainHand.Speed;
            float mhpercent = weaponDiff/(1+weaponDiff);
            float ohpercent = 1-mhpercent;
            float consumeRate = (1 + _talents.Flurry * 0.05f) * (1f + StatConversion.GetHasteFromRating(_stats.HasteRating,Character.CharacterClass.Warrior)) * (1f + _stats.PhysicalHaste)
                                * (1 / MainHand.Speed + 1 / OHSpeed);

            float BTperSec = 0.1875f;
            float WWperSec = 0.1250f;

            uptime = (float)(System.Math.Pow((1f - MhCrit),(1f * mhpercent * 3f))
                         *System.Math.Pow((1 - (MhCrit)),(0 * mhpercent * 3)));

            uptime *= (float)(System.Math.Pow((1 - OhCrit), (ohpercent * 3)));

            uptime *= (float)(System.Math.Pow((1 - MhCrit), (3 / consumeRate * (BTperSec * (1 + MhCrit) + WWperSec))));
            uptime *= (float)(System.Math.Pow((1 - OhCrit), (3 / consumeRate * WWperSec)));
            uptime = 1 - uptime;

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
