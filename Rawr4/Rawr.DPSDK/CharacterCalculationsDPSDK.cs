using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DK;

namespace Rawr.DPSDK
{
    public class CharacterCalculationsDPSDK : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DPSPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float[] dpsSub = new float[EnumHelper.GetCount(typeof(DKability))];
        public float[] damSub = new float[EnumHelper.GetCount(typeof(DKability))];
        public float[] threatSub = new float[EnumHelper.GetCount(typeof(DKability))];
        public float[] tpsSub = new float[EnumHelper.GetCount(typeof(DKability))];

        public float WhiteDPS
        {
            get { return dpsSub[(int)DKability.White]; }
            set { dpsSub[(int)DKability.White] = value; }
        }

        private float _NecrosisDPS;
        public float NecrosisDPS
        {
            get { return _NecrosisDPS; }
            set { _NecrosisDPS = value; }
        }

        private float _BCBDPS;
        public float BCBDPS
        {
            get { return _BCBDPS; }
            set { _BCBDPS = value; }
        }

        public float DeathCoilDPS
        {
            get { return dpsSub[(int)DKability.DeathCoil]; }
            set { dpsSub[(int)DKability.DeathCoil] = value; }
        }

        public float IcyTouchDPS
        {
            get { return dpsSub[(int)DKability.IcyTouch]; }
            set { dpsSub[(int)DKability.IcyTouch] = value; }
        }

        public float PlagueStrikeDPS
        {
            get { return dpsSub[(int)DKability.PlagueStrike]; }
            set { dpsSub[(int)DKability.PlagueStrike] = value; }
        }

        public float FrostFeverDPS
        {
            get { return dpsSub[(int)DKability.FrostFever]; }
            set { dpsSub[(int)DKability.FrostFever] = value; }
        }

        public float BloodPlagueDPS
        {
            get { return dpsSub[(int)DKability.BloodPlague]; }
            set { dpsSub[(int)DKability.BloodPlague] = value; }
        }

        public float ScourgeStrikeDPS
        {
            get { return dpsSub[(int)DKability.ScourgeStrike]; }
            set { dpsSub[(int)DKability.ScourgeStrike] = value; }
        }

        public float UnholyBlightDPS
        {
            get { return dpsSub[(int)DKability.UnholyBlight]; }
            set { dpsSub[(int)DKability.UnholyBlight] = value; }
        }

        public float BloodparasiteDPS
        {
            get { return dpsSub[(int)DKability.BloodParasite]; }
            set { dpsSub[(int)DKability.BloodParasite] = value; }
        }

        // TODO: Fix this.
        public float OtherDPS
        {
            get { return dpsSub[(int)DKability.OtherArcane]; }
            set { dpsSub[(int)DKability.OtherArcane] = value; }
        }

        public float FrostStrikeDPS
        {
            get { return dpsSub[(int)DKability.FrostStrike]; }
            set { dpsSub[(int)DKability.FrostStrike] = value; }
        }

        public float HowlingBlastDPS
        {
            get { return dpsSub[(int)DKability.HowlingBlast]; }
            set { dpsSub[(int)DKability.HowlingBlast] = value; }
        }

        public float ObliterateDPS
        {
            get { return dpsSub[(int)DKability.Obliterate]; }
            set { dpsSub[(int)DKability.Obliterate] = value; }
        }

        public float DeathStrikeDPS
        {
            get { return dpsSub[(int)DKability.DeathStrike]; }
            set { dpsSub[(int)DKability.DeathStrike] = value; }
        }
        
        public float BloodStrikeDPS
        {
            get { return dpsSub[(int)DKability.BloodStrike]; }
            set { dpsSub[(int)DKability.BloodStrike] = value; }
        }

        public float HeartStrikeDPS
        {
            get { return dpsSub[(int)DKability.HeartStrike]; }
            set { dpsSub[(int)DKability.HeartStrike] = value; }
        }

        private float _GargoyleDPS;
        public float GargoyleDPS
        {
            get { return _GargoyleDPS; }
            set { _GargoyleDPS = value; }
        }

        private float _DRWDPS;
        public float DRWDPS
        {
            get { return _DRWDPS; }
            set { _DRWDPS = value; }
        }

        private float _WanderingPlagueDPS;
        public float WanderingPlagueDPS
        {
            get { return _WanderingPlagueDPS; }
            set { _WanderingPlagueDPS = value; }
        }

        private int _targetLevel;
        public int TargetLevel
        {
            get { return _targetLevel; }
            set { _targetLevel = value; }
        }

        private float _MHweaponDamage;
        public float MHWeaponDamage
        {
            get { return _MHweaponDamage; }
            set { _MHweaponDamage = value; }
        }

        private float _OHWeaponDamage;
        public float OHWeaponDamage
        {
            get { return _OHWeaponDamage; }
            set { _OHWeaponDamage = value; }
        }

        private float _MHattackSpeed;
        public float MHAttackSpeed
        {
            get { return _MHattackSpeed; }
            set { _MHattackSpeed = value; }
        }

        private float _OHattackSpeed;
        public float OHAttackSpeed
        {
            get { return _OHattackSpeed; }
            set { _OHattackSpeed = value; }
        }

        private float _avoidedAttacks;
        public float AvoidedAttacks
        {
            get { return _avoidedAttacks; }
            set { _avoidedAttacks = value; }
        }

        private float _dodgedMHAttacks;
        public float DodgedMHAttacks
        {
            get { return _dodgedMHAttacks; }
            set { _dodgedMHAttacks = value; }
        }

        private float _dodgedOHAttacks;
        public float DodgedOHAttacks
        {
            get { return _dodgedOHAttacks; }
            set { _dodgedOHAttacks = value; }
        }

        private float _dodgedAttacks;
        public float DodgedAttacks
        {
            get { return _dodgedAttacks; }
            set { _dodgedAttacks = value; }
        }

        private float _missedAttacks;
        public float MissedAttacks
        {
            get { return _missedAttacks; }
            set { _missedAttacks = value; }
        }

        private float _enemyMitigation;
        public float EnemyMitigation
        {
            get { return _enemyMitigation; }
            set { _enemyMitigation = value; }
        }
        private float _effectiveArmor;
        public float EffectiveArmor
        {
            get { return _effectiveArmor; }
            set { _effectiveArmor = value; }
        }
        private float _MHExpertise;
        public float MHExpertise
        {
            get { return _MHExpertise; }
            set { _MHExpertise = value; }
        }

        private float _OHExpertise;
        public float OHExpertise
        {
            get { return _OHExpertise; }
            set { _OHExpertise = value; }
        }

        public float GhoulDPS
        {
            get { return dpsSub[(int)DKability.Ghoul]; }
            set { dpsSub[(int)DKability.Ghoul] = value; }
        }

        private String _DRWStats;
        public String DRWStats
        {
            get { return _DRWStats; }
            set { _DRWStats = value; }
        }

        private StatsDK _basicStats;
        public StatsDK BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
        }

        #region Costs
        public float RotationTime { get; set; }
        public int Blood { get; set; }
        public int Unholy { get; set; }
        public int Frost { get; set; }
        public int Death { get; set; }
        public int RP { get; set; }
        public float FreeRERunes { get; set; }
        #endregion

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            float critRating = BasicStats.CritRating;
            float hitRating = BasicStats.HitRating;
            float attackPower = BasicStats.AttackPower;

            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Health",            BasicStats.Health.ToString("N0"));
            dictValues.Add("Strength",          BasicStats.Strength.ToString("N0"));
            dictValues.Add("Agility",           string.Format("{0:0}*Provides {1:P} crit chance", BasicStats.Agility, StatConversion.GetCritFromAgility(BasicStats.Agility, CharacterClass.DeathKnight)));
            dictValues.Add("Attack Power",      attackPower.ToString("N0"));
            dictValues.Add("Crit Rating",       string.Format("{0:0}*Provides {1:P} crit chance", critRating, StatConversion.GetCritFromRating(critRating, CharacterClass.DeathKnight)));
            dictValues.Add("Hit Rating",        string.Format("{0:0}*Negates {1:P} melee miss / {2:P} spell miss", hitRating, StatConversion.GetPhysicalHitFromRating(hitRating, CharacterClass.DeathKnight), StatConversion.GetSpellHitFromRating(hitRating,CharacterClass.DeathKnight)));
            dictValues.Add("Expertise",         string.Format("{0:0.00} / {1:0.00}*Negates {2:P} / {3:P} dodge chance", MHExpertise, OHExpertise, StatConversion.GetDodgeParryReducFromExpertise(MHExpertise), StatConversion.GetDodgeParryReducFromExpertise(OHExpertise)));
            dictValues.Add("Haste Rating",      string.Format("{0:0}*Increases attack speed by {1:P}", BasicStats.HasteRating, StatConversion.GetHasteFromRating(BasicStats.HasteRating, CharacterClass.DeathKnight)));
            dictValues.Add("Armor",             BasicStats.Armor.ToString("N0"));
            dictValues.Add("Resilience",        BasicStats.Resilience.ToString("F0"));
            dictValues.Add("Mastery",           string.Format("{0:N0}*Rating: {1:N0}", BasicStats.Mastery, BasicStats.MasteryRating));

            dictValues.Add("Weapon Damage",     MHWeaponDamage.ToString("N2") + " / " + OHWeaponDamage.ToString("N2"));
            dictValues.Add("Attack Speed",      MHAttackSpeed.ToString("N2") + " / " + OHAttackSpeed.ToString("N2"));
            dictValues.Add("Crit Chance",       string.Format("{0:P}", BasicStats.PhysicalCrit));
            dictValues.Add("Avoided Attacks",   string.Format("{0:P}*{1:P} Dodged, {2:P} Missed", AvoidedAttacks, DodgedAttacks, MissedAttacks));
            dictValues.Add("Enemy Mitigation",  string.Format("{0:P}*{1:0} effective enemy armor", EnemyMitigation, EffectiveArmor));

/*            dictValues.Add("BCB", string.Format("{0:N2}*{1:P}", BCBDPS, (float)BCBDPS / DPSPoints));
            dictValues.Add("Blood Boil", string.Format("{0:N2}*{1:P}", dpsSub[(int)DKability.BloodBoil], (float)damSub[(int)DKability.BloodBoil] / DPSPoints));
            dictValues.Add("Blood Plague",      string.Format("{0:N2}*{1:P}", BloodPlagueDPS, (float)BloodPlagueDPS/DPSPoints));
            dictValues.Add("Blood Strike",      string.Format("{0:N2}*{1:P}", BloodStrikeDPS, (float)BloodStrikeDPS/DPSPoints));
            dictValues.Add("Death Coil",        string.Format("{0:N2}*{1:P}", DeathCoilDPS, (float)DeathCoilDPS / DPSPoints));
            dictValues.Add("Death n Decay", string.Format("{0:N2}*{1:P}", dpsSub[(int)DKability.DeathNDecay], (float)dpsSub[(int)DKability.DeathNDecay] / DPSPoints));
            dictValues.Add("DRW", string.Format("{0:N2}*{1:P}, wait for " + DRWStats + " proc", DRWDPS, (float)DRWDPS / DPSPoints));
            dictValues.Add("Festering Strike", string.Format("{0:N2}*{1:P}", dpsSub[(int)DKability.FesteringStrike], (float)dpsSub[(int)DKability.FesteringStrike] / DPSPoints));
            dictValues.Add("Frost Fever", string.Format("{0:N2}*{1:P}", FrostFeverDPS, (float)FrostFeverDPS / DPSPoints));
            dictValues.Add("Frost Strike",      string.Format("{0:N2}*{1:P}", FrostStrikeDPS, (float)FrostStrikeDPS/DPSPoints));
            dictValues.Add("Gargoyle",          string.Format("{0:N2}*{1:P}", GargoyleDPS, (float)GargoyleDPS / DPSPoints));
            dictValues.Add("Heart Strike",      string.Format("{0:N2}*{1:P}", HeartStrikeDPS, (float)HeartStrikeDPS / DPSPoints));
            dictValues.Add("Howling Blast",     string.Format("{0:N2}*{1:P}", HowlingBlastDPS, (float)HowlingBlastDPS / DPSPoints));
            dictValues.Add("Icy Touch",         string.Format("{0:N2}*{1:P}", IcyTouchDPS, (float)IcyTouchDPS / DPSPoints));
            dictValues.Add("Necrosis",          string.Format("{0:N2}*{1:P}", NecrosisDPS, (float)NecrosisDPS / DPSPoints));
            dictValues.Add("Necrotic Strike", string.Format("{0:N2}*{1:P}", dpsSub[(int)DKability.NecroticStrike], (float)dpsSub[(int)DKability.NecroticStrike] / DPSPoints));
            dictValues.Add("Obliterate", string.Format("{0:N2}*{1:P}", ObliterateDPS, (float)ObliterateDPS / DPSPoints));
            dictValues.Add("Death Strike",      string.Format("{0:N2}*{1:P}", DeathStrikeDPS, (float)DeathStrikeDPS / DPSPoints));
            dictValues.Add("Plague Strike",     string.Format("{0:N2}*{1:P}", PlagueStrikeDPS, (float)PlagueStrikeDPS / DPSPoints));
            dictValues.Add("Rune Strike", string.Format("{0:N2}*{1:P}", dpsSub[(int)DKability.RuneStrike], (float)dpsSub[(int)DKability.RuneStrike] / DPSPoints));
            dictValues.Add("Scourge Strike", string.Format("{0:N2}*{1:P}", ScourgeStrikeDPS, (float)ScourgeStrikeDPS / DPSPoints));
            dictValues.Add("Unholy Blight",     string.Format("{0:N2}*{1:P}", UnholyBlightDPS, (float)UnholyBlightDPS / DPSPoints));
            dictValues.Add("Wandering Plague",  string.Format("{0:N2}*{1:P}", WanderingPlagueDPS, (float)WanderingPlagueDPS / DPSPoints));
            dictValues.Add("White",             string.Format("{0:N2}*{1:P}", WhiteDPS, (float)WhiteDPS / DPSPoints));
            dictValues.Add("Blood Parasite",    string.Format("{0:N2}*{1:P}", BloodparasiteDPS, (float)BloodparasiteDPS / DPSPoints));
            dictValues.Add("Other",             string.Format("{0:N2}*{1:P}", OtherDPS, (float)OtherDPS / DPSPoints));
*/
            dictValues.Add("Ghoul", string.Format("{0:N2}*{1:P}", GhoulDPS, (float)GhoulDPS / DPSPoints));

            dictValues.Add("Total DPS", DPSPoints.ToString("N2"));

            dictValues.Add("Rotation Duration", RotationTime.ToString() + " secs");
            dictValues.Add("Blood", Blood.ToString() );
            dictValues.Add("Frost", Frost.ToString() );
            dictValues.Add("Unholy", Unholy.ToString() );
            dictValues.Add("Death", Death.ToString() );
            dictValues.Add("Runic Power", Death.ToString() );
            dictValues.Add("RE Runes", FreeRERunes.ToString("N2"));

            PopulateSingleUseValues(dictValues, "BB", DKability.BloodBoil);
            PopulateSingleUseValues(dictValues, "BP", DKability.BloodPlague);
            PopulateSingleUseValues(dictValues, "BS", DKability.BloodStrike);
            PopulateSingleUseValues(dictValues, "DC", DKability.DeathCoil);
            PopulateSingleUseValues(dictValues, "DnD", DKability.DeathNDecay);
            PopulateSingleUseValues(dictValues, "DS", DKability.DeathStrike);
            PopulateSingleUseValues(dictValues, "Fest", DKability.FesteringStrike);
            PopulateSingleUseValues(dictValues, "FF", DKability.FrostFever);
            PopulateSingleUseValues(dictValues, "FS", DKability.FrostStrike);
            PopulateSingleUseValues(dictValues, "HS", DKability.HeartStrike);
            PopulateSingleUseValues(dictValues, "HB", DKability.HowlingBlast);
            PopulateSingleUseValues(dictValues, "IT", DKability.IcyTouch);
            PopulateSingleUseValues(dictValues, "NS", DKability.NecroticStrike);
            PopulateSingleUseValues(dictValues, "OB", DKability.Obliterate);
            PopulateSingleUseValues(dictValues, "PS", DKability.PlagueStrike);
            PopulateSingleUseValues(dictValues, "RS", DKability.RuneStrike);
            PopulateSingleUseValues(dictValues, "SS", DKability.ScourgeStrike);
            return dictValues;
        }

        private void PopulateSingleUseValues(Dictionary<string, string> dictValues, string szName, DKability eAbility)
        {
            dictValues.Add(szName, string.Format("{0:N0} ({1:N2} DPS)*{2:N0} ({3:N2} TPS)", damSub[(int)eAbility], dpsSub[(int)eAbility], threatSub[(int)eAbility], tpsSub[(int)eAbility]));
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health":
                    return BasicStats.Health;
                case "Nature Resistance":
                    return BasicStats.NatureResistance;
                case "Fire Resistance":
                    return BasicStats.FireResistance;
                case "Frost Resistance":
                    return BasicStats.FrostResistance;
                case "Shadow Resistance":
                    return BasicStats.ShadowResistance;
                case "Arcane Resistance":
                    return BasicStats.ArcaneResistance;
                case "Crit Rating":
                    return BasicStats.CritRating;
                case "Expertise Rating":
                    return BasicStats.ExpertiseRating;
                case "Hit Rating":
                    return BasicStats.HitRating;
                case "Haste Rating":
                    return BasicStats.HasteRating;
                case "Mastery":
                    return BasicStats.Mastery;
                case "Target Miss %":
                    return MissedAttacks * 100f;
                case "Target Dodge %":
                    return DodgedAttacks * 100f;
                case "Resilience": return BasicStats.Resilience;
                case "Spell Penetration": return BasicStats.SpellPenetration;

            }
            return 0;
        }
    }
}
