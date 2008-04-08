using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Reflection.Emit;

namespace Rawr
{
	/// <summary>
	/// A Stats object represents a collection of stats on an object, such as an Item, Enchant, or Buff.
	/// </summary>
    [Serializable]
	public class Stats
	{
		private float[] _rawData = new float[112];
		/// <summary>
		/// The properties for each stat. In order to add additional stats for Rawr to track,
		/// first add properties here, for each stat. Apply a Category attribute to assign it to
		/// a category in the item editor. Optionally, apply a DisplayName attribute. If no
		/// DisplayName attribute is applied, the property name will be used, with spaces between
		/// each word, detected by capitalization (AttackPower becomes "Attack Power"). If the
		/// stat is a multiplier, add the Multiplicative attribute.
		/// </summary>
		#region Stat Properties

        [System.ComponentModel.DefaultValueAttribute(0f)]
		[Category("Base Stats")]
		public float Armor { get { return _rawData[0]; } set { _rawData[0] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
		public float Health { get { return _rawData[1]; } set { _rawData[1] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
		public float Mana { get { return _rawData[2]; } set { _rawData[2] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Agility { get { return _rawData[3]; } set { _rawData[3] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Stamina { get { return _rawData[4]; } set { _rawData[4] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float AttackPower { get { return _rawData[5]; } set { _rawData[5] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Strength { get { return _rawData[6]; } set { _rawData[6] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float WeaponDamage { get { return _rawData[7]; } set { _rawData[7] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float ScopeDamage { get { return _rawData[8]; } set { _rawData[8] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Penetration")]
        public float ArmorPenetration { get { return _rawData[9]; } set { _rawData[9] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Intellect { get { return _rawData[10]; } set { _rawData[10] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Spirit { get { return _rawData[11]; } set { _rawData[11] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Frost Res")]
        public float FrostResistance { get { return _rawData[12]; } set { _rawData[12] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Nature Res")]
        public float NatureResistance { get { return _rawData[13]; } set { _rawData[13] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Fire Res")]
        public float FireResistance { get { return _rawData[14]; } set { _rawData[14] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Shadow Res")]
        public float ShadowResistance { get { return _rawData[15]; } set { _rawData[15] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Arcane Res")]
        public float ArcaneResistance { get { return _rawData[16]; } set { _rawData[16] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Resist")]
        public float AllResist { get { return _rawData[17]; } set { _rawData[17] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Crit")]
        public float SpellCritRating { get { return _rawData[18]; } set { _rawData[18] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Frost Crit")]
        public float SpellFrostCritRating { get { return _rawData[19]; } set { _rawData[19] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Damage")]
        public float SpellDamageRating { get { return _rawData[20]; } set { _rawData[20] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Shadow Damage")]
        public float SpellShadowDamageRating { get { return _rawData[21]; } set { _rawData[21] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Fire Damage")]
        public float SpellFireDamageRating { get { return _rawData[22]; } set { _rawData[22] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Frost Damage")]
        public float SpellFrostDamageRating { get { return _rawData[23]; } set { _rawData[23] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Arcane Damage")]
        public float SpellArcaneDamageRating { get { return _rawData[24]; } set { _rawData[24] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Nature Damage")]
        public float SpellNatureDamageRating { get { return _rawData[25]; } set { _rawData[25] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Penetration")]
        public float SpellPenetration { get { return _rawData[26]; } set { _rawData[26] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Hit")]
        public float SpellHitRating { get { return _rawData[27]; } set { _rawData[27] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        public float Healing { get { return _rawData[28]; } set { _rawData[28] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Haste")]
        public float SpellHasteRating { get { return _rawData[29]; } set { _rawData[29] = value; } }

        // percentage mana generation while casting
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Combat Mana Regeneration")]
        public float SpellCombatManaRegeneration { get { return _rawData[30]; } set { _rawData[30] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Crit")]
        public float CritRating { get { return _rawData[31]; } set { _rawData[31] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Hit")]
        public float HitRating { get { return _rawData[32]; } set { _rawData[32] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Dodge")]
        public float DodgeRating { get { return _rawData[33]; } set { _rawData[33] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Parry")]
        public float ParryRating { get { return _rawData[34]; } set { _rawData[34] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Block")]
        public float BlockRating { get { return _rawData[35]; } set { _rawData[35] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Block Value")]
        public float BlockValue { get { return _rawData[36]; } set { _rawData[36] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Defense")]
        public float DefenseRating { get { return _rawData[37]; } set { _rawData[37] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        public float Resilience { get { return _rawData[38]; } set { _rawData[38] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Expertise")]
        public float ExpertiseRating { get { return _rawData[39]; } set { _rawData[39] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Haste")]
        public float HasteRating { get { return _rawData[40]; } set { _rawData[40] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Mana per 5 sec")]
        public float Mp5 { get { return _rawData[41]; } set { _rawData[41] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Health per 5 sec")]
        public float Hp5 { get { return _rawData[42]; } set { _rawData[42] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float BloodlustProc { get { return _rawData[43]; } set { _rawData[43] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float TerrorProc { get { return _rawData[44]; } set { _rawData[44] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		[DisplayName("% Miss")]
        public float Miss { get { return _rawData[45]; } set { _rawData[45] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusShredDamage { get { return _rawData[46]; } set { _rawData[46] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		public float BonusMangleDamage { get { return _rawData[47]; } set { _rawData[47] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		[DisplayName("Bonus Rip Damage Per Combo Point Per Tick")]
		public float BonusRipDamagePerCPPerTick { get { return _rawData[48]; } set { _rawData[48] = value; } }

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Multiplicative]
		[DisplayName("% CStrike Dmg")]
		public float BonusCrusaderStrikeDamageMultiplier { get { return _rawData[49]; } set { _rawData[49] = value; } }

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[DisplayName("Windfury")]
		public float WindfuryAPBonus { get { return _rawData[50]; } set { _rawData[50] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestorePerHit { get { return _rawData[51]; } set { _rawData[51] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestorePerCast { get { return _rawData[52]; } set { _rawData[52] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		public float MangleCostReduction { get { return _rawData[53]; } set { _rawData[53] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		public float ExposeWeakness { get { return _rawData[54]; } set { _rawData[54] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		public float Bloodlust { get { return _rawData[55]; } set { _rawData[55] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		public float DrumsOfWar { get { return _rawData[56]; } set { _rawData[56] = value; } }

        // threat dealt is damage * (1 + ThreatMultiplier)
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ThreatMultiplier { get; set; }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		public float DrumsOfBattle { get { return _rawData[57]; } set { _rawData[57] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ArcaneBlastBonus { get { return _rawData[58]; } set { _rawData[58] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float SpellDamageFromIntellectPercentage { get { return _rawData[59]; } set { _rawData[59] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float SpellDamageFromSpiritPercentage { get { return _rawData[60]; } set { _rawData[60] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage Increase for 6 sec on Crit")]
        public float SpellDamageFor6SecOnCrit { get { return _rawData[61]; } set { _rawData[61] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (50% 5 sec/Crit)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor5SecOnCrit_50 { get { return _rawData[62]; } set { _rawData[62] = value; } }

        // 15% change, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (15% 6 sec/Cast)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor6SecOnCast_15_45 { get { return _rawData[63]; } set { _rawData[63] = value; } }

        // 10% change, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (10% 6 sec/Hit)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor6SecOnHit_10_45 { get { return _rawData[64]; } set { _rawData[64] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec/Resist)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnResist { get { return _rawData[65]; } set { _rawData[65] = value; } }

        // trinket effect, does not sum up over gear, 2 trinkets with this effect is not equivalent to 1 trinket with double effect
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor20SecOnUse2Min { get { return _rawData[66]; } set { _rawData[66] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (15 sec/1.5 min)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor15SecOnUse90Sec { get { return _rawData[67]; } set { _rawData[67] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor20SecOnUse2Min { get { return _rawData[68]; } set { _rawData[68] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Mp5 on Cast (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float Mp5OnCastFor20SecOnUse2Min { get { return _rawData[69]; } set { _rawData[69] = value; } }

        // 5% chance, no cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnHit_5 { get { return _rawData[70]; } set { _rawData[70] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float BonusManaGem { get { return _rawData[71]; } set { _rawData[71] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float BonusManaPotion { get { return _rawData[72]; } set { _rawData[72] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (15 sec/Gem)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor15SecOnManaGem { get { return _rawData[73]; } set { _rawData[73] = value; } }

        // 10% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnHit_10_45 { get { return _rawData[74]; } set { _rawData[74] = value; } }

        // 20% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (15 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor15SecOnCrit_20_45 { get { return _rawData[75]; } set { _rawData[75] = value; } }

        // 20% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnCrit_20_45 { get { return _rawData[76]; } set { _rawData[76] = value; } }

        // Starfire idol
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Starfire damage bonus")]
        [Category("Equipment Procs")]
        public float StarfireDmg { get { return _rawData[77]; } set { _rawData[77] = value; } }

        // Moonfire idol
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Moonfire damage bonus")]
        [Category("Equipment Procs")]
        public float MoonfireDmg { get { return _rawData[78]; } set { _rawData[78] = value; } }

        // Wrath idol
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Wrath damage bonus")]
        [Category("Equipment Procs")]
        public float WrathDmg { get { return _rawData[79]; } set { _rawData[79] = value; } }

        // Moonkin Aura idol
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell critical bonus")]
        [Category("Equipment Procs")]
        public float IdolCritRating { get { return _rawData[80]; } set { _rawData[80] = value; } }

        // Moonkin 4-piece T4 bonus
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float InnervateCooldownReduction { get { return _rawData[81]; } set { _rawData[81] = value; } }

        // Moonkin 4-piece T5 bonus
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float StarfireBonusWithDot { get { return _rawData[82]; } set { _rawData[82] = value; } }

        // Moonkin 2-piece T6 bonus
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MoonfireExtension { get { return _rawData[83]; } set { _rawData[83] = value; } }
        // Moonkin 4-piece T6 bonus
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float StarfireCritChance { get { return _rawData[84]; } set { _rawData[84] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float EvocationExtension { get { return _rawData[85]; } set { _rawData[85] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float LightningCapacitorProc { get { return _rawData[86]; } set { _rawData[86] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        public float BonusMageNukeMultiplier { get { return _rawData[87]; } set { _rawData[87] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		[Multiplicative]
		[DisplayName("% Agility")]
        public float BonusAgilityMultiplier { get { return _rawData[88]; } set { _rawData[88] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		[Multiplicative]
		[DisplayName("% Strength")]
        public float BonusStrengthMultiplier { get { return _rawData[89]; } set { _rawData[89] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		[Multiplicative]
		[DisplayName("% Stamina")]
        public float BonusStaminaMultiplier { get { return _rawData[90]; } set { _rawData[90] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Int")]
        public float BonusIntellectMultiplier { get { return _rawData[91]; } set { _rawData[91] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		[Multiplicative]
		[DisplayName("% Armor")]
        public float BonusArmorMultiplier { get { return _rawData[92]; } set { _rawData[92] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		[Multiplicative]
		[DisplayName("% AP")]
        public float BonusAttackPowerMultiplier { get { return _rawData[93]; } set { _rawData[93] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% SP")]
        public float BonusSpellPowerMultiplier { get { return _rawData[94]; } set { _rawData[94] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Fire Damage")]
        public float BonusFireSpellPowerMultiplier { get { return _rawData[95]; } set { _rawData[95] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Shadow Damage")]
        public float BonusShadowSpellPowerMultiplier { get { return _rawData[96]; } set { _rawData[96] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Arcane Damage")]
        public float BonusArcaneSpellPowerMultiplier { get { return _rawData[97]; } set { _rawData[97] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Nature Damage")]
        public float BonusNatureSpellPowerMultiplier { get { return _rawData[98]; } set { _rawData[98] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Frost Damage")]
        public float BonusFrostSpellPowerMultiplier { get { return _rawData[99]; } set { _rawData[99] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Spirit")]
        public float BonusSpiritMultiplier { get { return _rawData[100]; } set { _rawData[100] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Crit Dmg")]
        public float BonusCritMultiplier { get { return _rawData[101]; } set { _rawData[101] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Spell Crit Dmg")]
        public float BonusSpellCritMultiplier { get { return _rawData[102]; } set { _rawData[102] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
		[Multiplicative]
		[DisplayName("% Rip Dmg")]
        public float BonusRipDamageMultiplier { get { return _rawData[103]; } set { _rawData[103] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Physical Dmg")]
        public float BonusPhysicalDamageMultiplier { get { return _rawData[104]; } set { _rawData[104] = value; } }

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Category("Combat Ratings")]
		[DisplayName("LotP Crit")]
		public float LotPCritRating { get { return _rawData[105]; } set { _rawData[105] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MageSpellCrit { get { return _rawData[106]; } set { _rawData[106] = value; } }

        // Unseen Moon idol
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Unseen Moon proc")]
        [Category("Equipment Procs")]
        public float UnseenMoonDamageBonus { get { return _rawData[107]; } set { _rawData[107] = value; } }

		[System.ComponentModel.DefaultValueAttribute(0f)]
		public float ShatteredSunMightProc { get { return _rawData[108]; } set { _rawData[108] = value; } }

		[System.ComponentModel.DefaultValueAttribute(0f)]
		public float CrushChanceReduction { get { return _rawData[109]; } set { _rawData[109] = value; } }


        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float CatFormStrength { get { return _rawData[109]; } set { _rawData[110] = value; } }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float AverageAgility { get { return _rawData[110]; } set { _rawData[111] = value; } }


		#endregion

		/// <summary>
		/// Adds together two stats, when using a + operator. When adding additional stats for
		/// Rawr to track, after adding the stat property, also add a line for it to this method,
		/// to properly combine the stat, as appropriate.
		/// </summary>
		/// <param name="a">The first Stats object to combine.</param>
		/// <param name="b">The second Stats object to combine.</param>
		/// <returns>The combined Stats object.</returns>
		public static Stats operator +(Stats a, Stats b)
		{
			return new Stats()
			{
				//NOTE: This is hard-coded, not using reflection, due to speed and maintainability.
				//GetValue and SetValue via reflection are *extremely* slow, and cause noticable lag in the app.
				//We also tried at one point to dynamically generate this code at runtime, which worked, but was
				//very complex and not maintainable by anyone who didn't already know wtf it was doing. So,
				//we're back to just hard-coding it, which isn't that big of a deal.
				Armor = a.Armor + b.Armor,
				Health = a.Health + b.Health,
				Agility = a.Agility + b.Agility,
				Stamina = a.Stamina + b.Stamina,
				AttackPower = a.AttackPower + b.AttackPower,
				Strength = a.Strength + b.Strength,
				Intellect = a.Intellect + b.Intellect,
				Spirit = a.Spirit + b.Spirit,
				WeaponDamage = a.WeaponDamage + b.WeaponDamage,
				ArmorPenetration = a.ArmorPenetration + b.ArmorPenetration,
				FrostResistance = a.FrostResistance + b.FrostResistance,
				NatureResistance = a.NatureResistance + b.NatureResistance,
				FireResistance = a.FireResistance + b.FireResistance,
				ShadowResistance = a.ShadowResistance + b.ShadowResistance,
				ArcaneResistance = a.ArcaneResistance + b.ArcaneResistance,
				AllResist = a.AllResist + b.AllResist,
				CritRating = a.CritRating + b.CritRating,
				LotPCritRating = a.LotPCritRating + b.LotPCritRating,
				HitRating = a.HitRating + b.HitRating,
				DodgeRating = a.DodgeRating + b.DodgeRating,
				ParryRating = a.ParryRating + b.ParryRating,
				BlockRating = a.BlockRating + b.BlockRating,
                BlockValue = a.BlockValue + b.BlockValue,
				DefenseRating = a.DefenseRating + b.DefenseRating,
				Resilience = a.Resilience + b.Resilience,
				ExpertiseRating = a.ExpertiseRating + b.ExpertiseRating,
				HasteRating = a.HasteRating + b.HasteRating,
				Mp5 = a.Mp5 + b.Mp5,
                Hp5 = a.Hp5 + b.Hp5,
				BloodlustProc = a.BloodlustProc + b.BloodlustProc,
				TerrorProc = a.TerrorProc + b.TerrorProc,
				Miss = a.Miss + b.Miss,
				BonusShredDamage = a.BonusShredDamage + b.BonusShredDamage,
				BonusMangleDamage = a.BonusMangleDamage + b.BonusMangleDamage,
				BonusRipDamagePerCPPerTick = a.BonusRipDamagePerCPPerTick + b.BonusRipDamagePerCPPerTick,
				MangleCostReduction = a.MangleCostReduction + b.MangleCostReduction,
				ExposeWeakness = a.ExposeWeakness + b.ExposeWeakness,
				Bloodlust = a.Bloodlust + b.Bloodlust,
				DrumsOfBattle = a.DrumsOfBattle + b.DrumsOfBattle,
				DrumsOfWar = a.DrumsOfWar + b.DrumsOfWar,
				BonusAgilityMultiplier = (1f + a.BonusAgilityMultiplier) * (1f + b.BonusAgilityMultiplier) - 1f,
                BonusStrengthMultiplier = (1f + a.BonusStrengthMultiplier) * (1f + b.BonusStrengthMultiplier) - 1f,
				BonusStaminaMultiplier = (1f + a.BonusStaminaMultiplier) * (1f + b.BonusStaminaMultiplier) - 1f,
				BonusArmorMultiplier = (1f + a.BonusArmorMultiplier) * (1f + b.BonusArmorMultiplier) - 1f,
				BonusAttackPowerMultiplier = (1f + a.BonusAttackPowerMultiplier) * (1f + b.BonusAttackPowerMultiplier) - 1f,
				BonusCritMultiplier = (1f + a.BonusCritMultiplier) * (1f + b.BonusCritMultiplier) - 1f,
				BonusRipDamageMultiplier = (1f + a.BonusRipDamageMultiplier) * (1f + b.BonusRipDamageMultiplier) - 1f,
				BonusIntellectMultiplier = (1f + a.BonusIntellectMultiplier) * (1f + b.BonusIntellectMultiplier) - 1f,
				BonusSpellCritMultiplier = (1f + a.BonusSpellCritMultiplier) * (1f + b.BonusSpellCritMultiplier) - 1f,
				BonusSpellPowerMultiplier = (1f + a.BonusSpellPowerMultiplier) * (1f + b.BonusSpellPowerMultiplier) - 1f,
                BonusFireSpellPowerMultiplier = (1f + a.BonusFireSpellPowerMultiplier) * (1f + b.BonusFireSpellPowerMultiplier) - 1f,
                BonusFrostSpellPowerMultiplier = (1f + a.BonusFrostSpellPowerMultiplier) * (1f + b.BonusFrostSpellPowerMultiplier) - 1f,
                BonusArcaneSpellPowerMultiplier = (1f + a.BonusArcaneSpellPowerMultiplier) * (1f + b.BonusArcaneSpellPowerMultiplier) - 1f,
                BonusShadowSpellPowerMultiplier = (1f + a.BonusShadowSpellPowerMultiplier) * (1f + b.BonusShadowSpellPowerMultiplier) - 1f,
                BonusNatureSpellPowerMultiplier = (1f + a.BonusNatureSpellPowerMultiplier) * (1f + b.BonusNatureSpellPowerMultiplier) - 1f,
                BonusSpiritMultiplier = (1f + a.BonusSpiritMultiplier) * (1f + b.BonusSpiritMultiplier) - 1f,
                ThreatMultiplier = (1f + a.ThreatMultiplier) * (1f + b.ThreatMultiplier) - 1f,
                SpellCritRating = a.SpellCritRating + b.SpellCritRating,
                SpellFrostCritRating = a.SpellFrostCritRating + b.SpellFrostCritRating,
                SpellDamageRating = a.SpellDamageRating + b.SpellDamageRating,
				SpellFireDamageRating = a.SpellFireDamageRating + b.SpellFireDamageRating,
				SpellHasteRating = a.SpellHasteRating + b.SpellHasteRating,
				SpellHitRating = a.SpellHitRating + b.SpellHitRating,
				SpellShadowDamageRating = a.SpellShadowDamageRating + b.SpellShadowDamageRating,
				SpellFrostDamageRating = a.SpellFrostDamageRating + b.SpellFrostDamageRating,
                SpellArcaneDamageRating = a.SpellArcaneDamageRating + b.SpellArcaneDamageRating,
                SpellNatureDamageRating = a.SpellNatureDamageRating + b.SpellNatureDamageRating,
                SpellPenetration = a.SpellPenetration + b.SpellPenetration,
                Mana = a.Mana + b.Mana,
                LightningCapacitorProc = a.LightningCapacitorProc + b.LightningCapacitorProc,
                ArcaneBlastBonus = a.ArcaneBlastBonus + b.ArcaneBlastBonus,
                SpellDamageFor6SecOnCrit = a.SpellDamageFor6SecOnCrit + b.SpellDamageFor6SecOnCrit,
                EvocationExtension = a.EvocationExtension + b.EvocationExtension,
                BonusMageNukeMultiplier = (1f + a.BonusMageNukeMultiplier) * (1f + b.BonusMageNukeMultiplier) - 1f,
                ManaRestorePerHit = a.ManaRestorePerHit + b.ManaRestorePerHit,
                ManaRestorePerCast = a.ManaRestorePerCast + b.ManaRestorePerCast,
                BonusManaGem = a.BonusManaGem + b.BonusManaGem,
                BonusManaPotion = Math.Max(a.BonusManaPotion, b.BonusManaPotion), // does not stack
                SpellDamageFor10SecOnHit_10_45 = a.SpellDamageFor10SecOnHit_10_45 + b.SpellDamageFor10SecOnHit_10_45,
                SpellDamageFromIntellectPercentage = a.SpellDamageFromIntellectPercentage + b.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = a.SpellDamageFromSpiritPercentage + b.SpellDamageFromSpiritPercentage,
                SpellDamageFor10SecOnResist = a.SpellDamageFor10SecOnResist + b.SpellDamageFor10SecOnResist,
                SpellDamageFor15SecOnCrit_20_45 = a.SpellDamageFor15SecOnCrit_20_45 + b.SpellDamageFor15SecOnCrit_20_45,
                SpellCombatManaRegeneration = a.SpellCombatManaRegeneration + b.SpellCombatManaRegeneration,
                SpellHasteFor5SecOnCrit_50 = a.SpellHasteFor5SecOnCrit_50 + b.SpellHasteFor5SecOnCrit_50,
                SpellHasteFor6SecOnCast_15_45 = a.SpellHasteFor6SecOnCast_15_45 + b.SpellHasteFor6SecOnCast_15_45,
                SpellDamageFor10SecOnHit_5 = a.SpellDamageFor10SecOnHit_5 + b.SpellDamageFor10SecOnHit_5,
                SpellHasteFor6SecOnHit_10_45 = a.SpellHasteFor6SecOnHit_10_45 + b.SpellHasteFor6SecOnHit_10_45,
                SpellDamageFor10SecOnCrit_20_45 = a.SpellDamageFor10SecOnCrit_20_45 + b.SpellDamageFor10SecOnCrit_20_45,
                Healing = a.Healing + b.Healing,
                StarfireDmg = a.StarfireDmg + b.StarfireDmg,
                WrathDmg = a.WrathDmg + b.WrathDmg,
                MoonfireDmg = a.MoonfireDmg + b.MoonfireDmg,
                IdolCritRating = a.IdolCritRating + b.IdolCritRating,
                UnseenMoonDamageBonus = a.UnseenMoonDamageBonus + b.UnseenMoonDamageBonus,
                BonusPhysicalDamageMultiplier = (1f + a.BonusPhysicalDamageMultiplier)*(1f+b.BonusPhysicalDamageMultiplier) -1f,
                BonusCrusaderStrikeDamageMultiplier = (1f + a.BonusCrusaderStrikeDamageMultiplier)*(1f+b.BonusCrusaderStrikeDamageMultiplier) -1f,
				MageSpellCrit = a.MageSpellCrit + b.MageSpellCrit,
				ShatteredSunMightProc = a.ShatteredSunMightProc + b.ShatteredSunMightProc,
				CrushChanceReduction = a.CrushChanceReduction + b.CrushChanceReduction,
				CatFormStrength = a.CatFormStrength + b.CatFormStrength,
				AverageAgility = a.AverageAgility + b.AverageAgility ,
				WindfuryAPBonus = a.WindfuryAPBonus + b.WindfuryAPBonus
			};
		}

		public bool Equals(Stats other)
		{
			return this == other;
		}
		public ArrayUtils.CompareResult CompareTo(Stats other)
		{
			if (ReferenceEquals(other, null)) return 0;
			return ArrayUtils.AllCompare(this._rawData, other._rawData);
		}
		//int IComparable.CompareTo(object other)
		//{
		//    return CompareTo(other as Stats);
		//}

        public override int GetHashCode()
        {
            return _rawData.GetHashCode();
        }

		public override bool Equals(object obj)
		{
			return this == (obj as Stats);
		}
		public static bool operator ==(Stats x, Stats y)
		{
			if (ReferenceEquals(x, y) || (ReferenceEquals(x, null) && ReferenceEquals(y, null))) return true;
			if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
			return ArrayUtils.AllEqual(x._rawData, y._rawData);
		}
		public static bool operator !=(Stats x, Stats y)
		{
			return !(x == y);
		}
		public static bool operator >(Stats x, Stats y)
		{
			return AllCompare(x, y, ArrayUtils.CompareOption.GreaterThan);
		}
		public static bool operator >=(Stats x, Stats y)
		{
			return AllCompare(x, y, ArrayUtils.CompareOption.GreaterThan | ArrayUtils.CompareOption.Equal);
		}
		public static bool operator <(Stats x, Stats y)
		{
			return AllCompare(x, y, ArrayUtils.CompareOption.LessThan);
		}
		public static bool operator <=(Stats x, Stats y)
		{
			return AllCompare(x, y, ArrayUtils.CompareOption.LessThan | ArrayUtils.CompareOption.Equal);
		}
		private static bool AllCompare(Stats x, Stats y, ArrayUtils.CompareOption comparison)
		{
			if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) throw new ArgumentNullException();
			return ArrayUtils.AllCompare(x._rawData, y._rawData, comparison);
		}
		
		//with hands held high into the sky so blue
        public Stats() { }

		//as the ocean opens up to swallow you
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (PropertyInfo info in PropertyInfoCache)
			{
				float value = (float)info.GetValue(this, null);
				if (value != 0)
				{
					if (IsMultiplicative(info))
					{
						value *= 100;
					}

					value = (float)Math.Round(value * 100f) / 100f;

					sb.AppendFormat("{0}{1}, ", value, Extensions.DisplayName(info));
				}
			}

			return sb.ToString().TrimEnd(' ', ',');
		}

		public Stats Clone()
		{
            Stats clone = (Stats)this.MemberwiseClone();
            clone._rawData = (float[])clone._rawData.Clone();
            return clone;
		}

		#region Multiplicative Handling
		private static PropertyInfo[] _propertyInfoCache = null;
		private static List<PropertyInfo> _multiplicativeProperties = new List<PropertyInfo>();

        static Stats()
        {
			List<PropertyInfo> items = new List<PropertyInfo>();

            foreach (PropertyInfo info in typeof(Stats).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding))
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    items.Add(info);
                }
            }
            _propertyInfoCache = items.ToArray();

            foreach (PropertyInfo info in _propertyInfoCache)
            {
                if (info.GetCustomAttributes(typeof(MultiplicativeAttribute), false).Length > 0)
                {
                    _multiplicativeProperties.Add(info); 
                }
            }
        }

		public static PropertyInfo[] PropertyInfoCache
		{
			get { return _propertyInfoCache; }
        }

        public static bool IsMultiplicative(PropertyInfo info)
        {
            return _multiplicativeProperties.Contains(info);
		}
		#endregion

        private class PropertyComparer : IComparer<PropertyInfo>
        {
            public int Compare(PropertyInfo x, PropertyInfo y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public static String[] StatNames
        {
            get{
                String[] names = new string[PropertyInfoCache.Length];
                for (int i = 0; i < PropertyInfoCache.Length; i++)
                {
                    names[i] = Extensions.DisplayName(PropertyInfoCache[i]);
                }
                Array.Sort(names);
                return names;
            }        
        }

        public IDictionary<PropertyInfo, float> Values(StatFilter filter)
        {
            SortedList<PropertyInfo, float> dict = new SortedList<PropertyInfo, float>(new PropertyComparer());
            foreach (PropertyInfo info in PropertyInfoCache)
            {
                if(info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    float value = (float)info.GetValue(this, null);
                    if (filter(value))
                    {
                        dict[info] = value;
                    }
                }
            }
            return dict;
        }
	}

	public delegate bool StatFilter(float value);

	[AttributeUsage(AttributeTargets.Property)]
	public class MultiplicativeAttribute : Attribute { }

	public static class Extensions
	{
		// requires .net 3.5 public static string LongName(this PropertyInfo info)
		// allows it to be called like
		//   info.LongName()
		// instead of
		//   Extensions.LongName(info)

		public static PropertyInfo UnDisplayName(string displayName)
		{
			foreach (PropertyInfo info in Stats.PropertyInfoCache)
			{
				if (DisplayName(info).Trim() == displayName.Trim())
					return info;
			}
			return null;
		}

		public static string DisplayName(PropertyInfo info)
		{
			string prettyName = null;

			object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
			if (attributes.Length == 1 && attributes[0] is DisplayNameAttribute && (attributes[0] as DisplayNameAttribute).DisplayName != null)
			{
				prettyName = (attributes[0] as DisplayNameAttribute).DisplayName;
			}
			else
			{
				prettyName = SpaceCamel(info.Name);
			}
			if (!prettyName.StartsWith("%"))
				prettyName = " " + prettyName;
			return prettyName;
		}
		public static string SpaceCamel(String name)
		{
			return System.Text.RegularExpressions.Regex.Replace(
					name,
					"([A-Z])",
					" $1",
					System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
		}

		public static string UnSpaceCamel(String name)
		{
			return System.Text.RegularExpressions.Regex.Replace(
					name,
					"( )",
					"",
					System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
		}
	}
}
