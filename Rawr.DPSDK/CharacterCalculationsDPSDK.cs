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

        public float[] DPUdpsSub = new float[EnumHelper.GetCount(typeof(DKability))];
        public float[] DPUdamSub = new float[EnumHelper.GetCount(typeof(DKability))];
        public float[] DPUthreatSub = new float[EnumHelper.GetCount(typeof(DKability))];
        public float[] DPUtpsSub = new float[EnumHelper.GetCount(typeof(DKability))];

        public float WhiteHitChance;
        public float YellowHitChance;
        public float WhiteDPS
        {
            get { return dpsSub[(int)DKability.White] + dpsSub[(int)DKability.WhiteOH]; }
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

        public float BloodparasiteDPS
        {
            get { return dpsSub[(int)DKability.BloodParasite]; }
            set { dpsSub[(int)DKability.BloodParasite] = value; }
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

        public float GargoyleDPS
        {
            get { return dpsSub[(int)DKability.Gargoyle]; }
            set { dpsSub[(int)DKability.Gargoyle] = value; }
        }

        private float _WanderingPlagueDPS;
        public float WanderingPlagueDPS
        {
            get { return _WanderingPlagueDPS; }
            set { _WanderingPlagueDPS = value; }
        }

        public float GhoulDPS
        {
            get { return dpsSub[(int)DKability.Ghoul]; }
            set { dpsSub[(int)DKability.Ghoul] = value; }
        }

        // TODO: Fix this.
        public float OtherDPS
        {
            get { return dpsSub[(int)DKability.OtherArcane]; }
            set { dpsSub[(int)DKability.OtherArcane] = value; }
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

        private float _MHweaponDPS;
        public float MHWeaponDPS
        {
            get { return _MHweaponDPS; }
            set { _MHweaponDPS = value; }
        }

        private float _OHWeaponDPS;
        public float OHWeaponDPS
        {
            get { return _OHWeaponDPS; }
            set { _OHWeaponDPS = value; }
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

        private StatsDK _basicStats;
        public StatsDK BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
        }

        public float m_RuneCD {get; set; }

        public void DPSBreakdown(Rotation rot, float[] petDPS)
        {
            float fTotalPetDPS = 0;
            foreach (DKability b in EnumHelper.GetValues(typeof(DKability)))
            {
                petDPS[(int)b] *= rot.CurRotationDuration;
                fTotalPetDPS += petDPS[(int)b];
            }
            float fTotalDam = rot.TotalDamage + fTotalPetDPS;
            float fTotalDPS = fTotalDam / rot.CurRotationDuration;
            // We already have the total damage done.
            if (null != rot.ml_Rot)
            {
                foreach (AbilityDK_Base a in rot.ml_Rot)
                {
                    // take each instance of each ability and gather the sums.
                    damSub[a.AbilityIndex] += a.TotalDamage;
                    threatSub[a.AbilityIndex] += a.TotalThreat;
                }
            }
            foreach (DKability b in EnumHelper.GetValues(typeof(DKability)))
            {
                damSub[(int)b] += petDPS[(int)b];
                dpsSub[(int)b] = DPSPoints * (damSub[(int)b] / fTotalDam);
                tpsSub[(int)b] = rot.m_TPS * (threatSub[(int)b] / rot.TotalThreat);
            }
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

            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Health",            BasicStats.Health.ToString("N0"));
            dictValues.Add("Strength",          BasicStats.Strength.ToString("N0"));
            dictValues.Add("Agility",           string.Format("{0:0}*Provides {1:P} crit chance", BasicStats.Agility, StatConversion.GetCritFromAgility(BasicStats.Agility, CharacterClass.DeathKnight)));
            dictValues.Add("Attack Power",      BasicStats.AttackPower.ToString("N0"));
            dictValues.Add("Crit Rating",       string.Format("{0:0}*Provides {1:P} crit chance", critRating, StatConversion.GetCritFromRating(critRating, CharacterClass.DeathKnight)));
            dictValues.Add("Hit Rating",        string.Format("{0:0}*Negates {1:P} melee miss / {2:P} spell miss", hitRating, StatConversion.GetPhysicalHitFromRating(hitRating, CharacterClass.DeathKnight), StatConversion.GetSpellHitFromRating(hitRating,CharacterClass.DeathKnight)));
            dictValues.Add("Expertise",         string.Format("{0:0.00} / {1:0.00}*Negates {2:P} / {3:P} dodge chance", MHExpertise, OHExpertise, StatConversion.GetDodgeParryReducFromExpertise(MHExpertise), StatConversion.GetDodgeParryReducFromExpertise(OHExpertise)));
            dictValues.Add("Haste Rating",      string.Format("{0:0}*Increases attack speed by {1:P}", BasicStats.HasteRating, StatConversion.GetHasteFromRating(BasicStats.HasteRating, CharacterClass.DeathKnight)));

            dictValues.Add("Armor",             BasicStats.Armor.ToString("N0"));
            dictValues.Add("Resilience",        BasicStats.Resilience.ToString("F0"));
            dictValues.Add("Mastery",           string.Format("{0:N0}*Rating: {1:N0}", BasicStats.Mastery, BasicStats.MasteryRating));

            dictValues.Add("Weapon Damage",     MHWeaponDamage.ToString("N2") + " / " + OHWeaponDamage.ToString("N2"));
            dictValues.Add("Attack Speed",      MHAttackSpeed.ToString("N2") + " / " + OHAttackSpeed.ToString("N2"));
            dictValues.Add("Crit Chance", string.Format("{0:P}", BasicStats.PhysicalCrit));
            dictValues.Add("Avoided Attacks",   string.Format("{0:P}*{1:P} Dodged, {2:P} Missed", AvoidedAttacks, DodgedAttacks, MissedAttacks));
            dictValues.Add("Enemy Mitigation",  string.Format("{0:P}*{1:0} effective enemy armor", EnemyMitigation, EffectiveArmor));

            dictValues.Add("White HitChance", string.Format("{0:P0.00}*Include Glance & Crit Chance", WhiteHitChance * 100));
            dictValues.Add("Yellow HitChance", string.Format("{0:P0.00}", YellowHitChance * 100));

            foreach (int i in EnumHelper.GetValues(typeof(DKability)))
            {
                dictValues.Add(Enum.GetName(typeof(DKability), i), string.Format("{0:N2}*{1:P}", dpsSub[i], (dpsSub[i] / DPSPoints)));
            }
            dictValues.Add("Total DPS", DPSPoints.ToString("N2"));

            dictValues.Add("Rotation Duration", RotationTime.ToString() + " secs");
            dictValues.Add("Blood", Blood.ToString() );
            dictValues.Add("Frost", Frost.ToString() );
            dictValues.Add("Unholy", Unholy.ToString() );
            dictValues.Add("Death", Death.ToString() );
            dictValues.Add("Runic Power", Death.ToString() );
            dictValues.Add("RE Runes", FreeRERunes.ToString("N2"));
            dictValues.Add("Rune Cooldown", m_RuneCD.ToString("N2"));

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
            dictValues.Add(szName, string.Format("{0:N0} ({1:N2} DPS)*{2:N0} ({3:N2} TPS)", DPUdamSub[(int)eAbility], DPUdpsSub[(int)eAbility], DPUthreatSub[(int)eAbility], DPUtpsSub[(int)eAbility]));
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
