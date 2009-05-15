using System;

namespace Rawr.DPSWarr {
    public class CombatFactors {
        public CombatFactors(Character character, Stats stats) {
			_stats = stats;
			_mainHand = character.MainHand == null ? new Knuckles() : character.MainHand.Item;
			_offHand = character.OffHand == null ? new Knuckles() : character.OffHand.Item;
            _talents = character.WarriorTalents;
            _calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            _characterRace = character.Race;
        }

        private Stats _stats;
        private Item _mainHand;
        private Item _offHand;
        private WarriorTalents _talents;
        private CalculationOptionsDPSWarr _calcOpts;
        private Character.CharacterRace _characterRace;

        public Item MainHand { get { return _mainHand; } }
        public Item OffHand { get { return _offHand; } }

        public float DamageReduction {
            get {
                if(_calcOpts==null){
				    return Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(80,12900,_stats.ArmorPenetration,0f,_stats.ArmorPenetrationRating));
                }else{
                    return Math.Max(0f, 1f - StatConversion.GetArmorDamageReduction(_calcOpts.TargetLevel,_calcOpts.TargetArmor,_stats.ArmorPenetration,0f,_stats.ArmorPenetrationRating));
                }
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
        public float AvgMhWeaponDmg { get { return CalcAverageWeaponDamage(MainHand, _stats); } }
        public float NormalizedMhWeaponDmg { get { return CalcNormalizedWeaponDamage(MainHand, _stats); } }
        public float AvgOhWeaponDmg { get { return CalcAverageWeaponDamage(OffHand, _stats)*(0.5f+_talents.DualWieldSpecialization*0.025f); } }
        public float NormalizedOhWeaponDmg { get { return CalcNormalizedWeaponDamage(OffHand, _stats); } }
        public float YellowMissChance { get { var missChance = 8f - HitPercent; return missChance < 0f ? 0f : missChance/100; } }
        public float WhiteMissChance {
            get {
                var missChance = (MainHand.Slot == Item.ItemSlot.TwoHand && _talents.TitansGrip != 1 ? 8f : 27f );
                missChance -= HitPercent;
                return missChance < 0f ? 0f : missChance/100; 
            }
        }
        public float MhExpertise    { get { return CalcExpertise(MainHand); } }
        public float OhExpertise    { get { return CalcExpertise(OffHand); } }
        public float MhDodgeChance  { get { return CalcDodgeChance(MhExpertise)/100f; } }
        public float OhDodgeChance  { get { return CalcDodgeChance(OhExpertise)/100f; } }
        public float MhCrit         { get { return CalcCrit(MainHand); } }
        public float MhYellowCrit   { get { return CalcYellowCrit(MainHand); } }
        public float OhCrit         { get { return CalcCrit(OffHand); } }
        public float OhYellowCrit   { get { return CalcYellowCrit(OffHand); } }
        public float ProbMhWhiteHit { get { return 1f - WhiteMissChance - MhDodgeChance; } }
        public float ProbOhWhiteHit { get { return 1f - WhiteMissChance - OhDodgeChance; } }
        public float TotalHaste {
            get {   
                //TODO:  Add WindFury Totem (a straight haste bonus as of patch 3.0)
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
        public float OffHandSpeed { get { return OffHand.Speed / TotalHaste; } }
        public float BonusWhiteCritDmg {
            get {
                float baseCritDmg = (2 * (1f + _stats.BonusCritMultiplier) - 1);
                baseCritDmg *= (MainHand.Type == Item.ItemType.TwoHandAxe || MainHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 1;
                return (2 * (1f + _stats.BonusCritMultiplier) - 1) * (1 + _talents.PoleaxeSpecialization * 0.01f);
            }
        }
        public float BonusYellowCritDmg { get { return BonusWhiteCritDmg*(1+_talents.Impale*0.1f); } }
        public float HitPercent { get { return _stats.PhysicalHit + StatConversion.GetHitFromRating(_stats.HitRating,Character.CharacterClass.Warrior); } }
        public float DamageBonus {
            get {
                return (1+_talents.TwoHandedWeaponSpecialization * 0.02f)*(1+_stats.BonusPhysicalDamageMultiplier)*
                        (1+_stats.BonusDamageMultiplier)*(1f+ (_talents.DeathWish*0.2f)*(30f/(180f*(1 - 0.11f*_talents.IntensifyRage))))*(1f+0.02f*_talents.WreckingCrew);
            }
        }
        private float CalcCrit(Item weapon) {
            if (weapon == null || weapon.MaxDamage == 0) { return 0f; }
            var crit = _stats.PhysicalCrit + StatConversion.GetCritFromRating(_stats.CritRating);
            if (_calcOpts != null) {
                //if (_calcOpts.TargetLevel == 83) { crit -= 0.048f; }
                if (_calcOpts.FuryStance) { crit += 0.03f; }
            }

            crit += (weapon.Type == Item.ItemType.TwoHandAxe || MainHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0;
            
            return crit;
        }
        private float CalcYellowCrit(Item weapon) {
            var crit = _stats.PhysicalCrit + StatConversion.GetCritFromRating(_stats.CritRating);
            crit *= (1 - YellowMissChance - MhDodgeChance);
            if (_calcOpts != null) {
                //if (_calcOpts.TargetLevel == 83) { crit -= 0.048f; }
                if (_calcOpts.FuryStance) { crit += 0.03f; }
            }

            crit += (weapon.Type == Item.ItemType.TwoHandAxe || MainHand.Type == Item.ItemType.Polearm) ? 0.01f * _talents.PoleaxeSpecialization : 0;

            return crit;
        }
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
        private float CalcDodgeChance(float mhExpertise) {
            var mhDodgeChance = 6.5f - .25f * mhExpertise;
            mhDodgeChance -= _talents.WeaponMastery;
            if (mhDodgeChance < 0f) { mhDodgeChance = 0f; }
            return mhDodgeChance;
        }
        private float CalcAverageWeaponDamage(Item weapon, Stats stats) {
            //damageMod*((AP/14+wepDPS)*wepSpeed)
            return DamageBonus * ((stats.AttackPower / 14 + weapon.DPS) * weapon.Speed);
            //return (weapon.MinDamage + weapon.MaxDamage + stats.WeaponDamage * 2) / 2.0f;
        }
        private float CalcNormalizedWeaponDamage(Item weapon, Stats stats) {
            float baseDamage = weapon.Speed * weapon.DPS;
            baseDamage += stats.AttackPower / 14 * 3.3f;
            return baseDamage;
        }
        private float CalcFlurryUptime(Stats stats) {
            float uptime = 1;
            float weaponDiff = OffHand.Speed/MainHand.Speed;
            float mhpercent = weaponDiff/(1+weaponDiff);
            float ohpercent = 1-mhpercent;
            float consumeRate = (1 + _talents.Flurry * 0.05f) * (1f + StatConversion.GetHasteFromRating(_stats.HasteRating,Character.CharacterClass.Warrior)) * (1f + _stats.PhysicalHaste)
                                * (1 / MainHand.Speed + 1 / OffHand.Speed);

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
        public class Knuckles : Item {
            public Knuckles() {
                Speed = 2f;
                MaxDamage = 0;
                MinDamage = 0;
            }
        }
    }
}