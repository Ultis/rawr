namespace Rawr.DPSWarr {
    public class Skills {
        public Skills(Character character,WarriorTalents talents, Stats stats, CombatFactors combatFactors,WhiteAttacks whiteStats) {
            _character = character;
            _talents = talents;
            _stats = stats;
            _combatFactors = combatFactors;
            _whiteStats = whiteStats;
            _calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
        }
        private Character _character;
        private WarriorTalents _talents;
        private Stats _stats;
        private CombatFactors _combatFactors;
        private WhiteAttacks _whiteStats;
        private CalculationOptionsDPSWarr _calcOpts;
        float heroicStrikePercent;
        float heroicStrikesPerSecond;

        #region Fury Skill Hits
        public float BloodThirstHits() {
            return (3.0f / 16)*_talents.Bloodthirst;//*(1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance);
        }
        public float WhirlWindHits() {
            float wwCount = 1.0f / 8;
            if (_combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand && _talents.TitansGrip != 1) {
                wwCount = 0;
            }
            return wwCount;/* * ((1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance)
                            + (1 - _combatFactors.YellowMissChance - _combatFactors.OhDodgeChance));*/
        }
        public float HeroicStrikeHits(float rageModifier) {
            float hsHits = (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance);
            hsHits *= (rageModifier + freeRage() / (15.0f - _talents.ImprovedHeroicStrike + heroicStrikeRage()));
            if (hsHits < 0) { hsHits = 0; }
            heroicStrikesPerSecond = hsHits;
            return hsHits;
        }
        public float BloodsurgeProcs() {
            float chance = _talents.Bloodsurge * 0.0666666666f;
            float procs = 3 + 4 + ((16 / _combatFactors.MainHandSpeed) * heroicStrikePercent);
            procs *= chance;
            //test -= (0.2f * 0.2f);
            procs = (procs / 16) - (chance * chance + 0.01f);
            if (procs < 0) { procs = 0; }
            return procs;
        }
        #endregion

        #region Arms Skill Hits
        public float TraumaHits() {
            if (_talents.Trauma == 0) { return 0.0f;  }
            return _combatFactors.MhCrit * _combatFactors.ProbMhWhiteHit /* * (1 - (15.0f/60.0f))*/; // last part is to calc uptime but not sure if that's accurate
        }
        public float MortalStrikeHits() {
            if (_talents.MortalStrike == 0) { return 0.0f; }
            return (1.0f / 5) * _talents.MortalStrike;//*(1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance);
        }
        public float OverpowerHits() {
            if (_calcOpts != null && _calcOpts.FuryStance) { return 0.0f; }
            double chance = 0.1*_talents.TasteForBlood;
            // 5 sec cooldown - (_talents.UnrelentingAssault*2.0f)
            float procs = (1f - (float)System.Math.Pow(1 - chance, 6)) / 18;
            return procs;
        }
        public float SuddenDeathHits() {
            if (_talents.SuddenDeath==0) { return 0.0f; }
            double chance = 0.03 * _talents.SuddenDeath;
            double totalHits = 18 / _combatFactors.MainHandSpeed;
            totalHits -= 0.5 * (SlamHits()*18);
            totalHits += (SlamHits() + MortalStrikeHits())*18;
            float hits = (1f - (float)System.Math.Pow(1 - chance, totalHits)) / 18;
            return 2*hits;
        }
        public float SlamHits() {
            if (_talents.ImprovedSlam == 2) {
                return (1.5f / 5);
            } else {
                return 0;
            }
        }
        public float SwordSpecHits() {
            if (_combatFactors.MainHand.Type != Item.ItemType.TwoHandSword) { return 0.0f; }
            float missChance = (1 - _combatFactors.WhiteMissChance - _combatFactors.MhDodgeChance);
            float wepSpeed = _combatFactors.MainHandSpeed;
            if (_combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand && _talents.TitansGrip != 1) {
                wepSpeed += (1.5f - (0.5f * _talents.ImprovedSlam)) / 5;
            }
            float whiteHits = missChance * (1 / wepSpeed);
            float attacks = 0.01f * _talents.SwordSpecialization;
            attacks *= missChance;
            attacks *= (MortalStrikeHits() + SlamHits() + OverpowerHits() + SuddenDeathHits() + whiteHits);
            return attacks;
        }
        public float BladestormHits() {return 6.0f / 90*_talents.Bladestorm;}
        #endregion

        #region Rage Calculations
        public float neededRage() {
            float BTRage = BloodThirstHits() * 30;
            float WWRage = WhirlWindHits() * 30;
            float MSRage = MortalStrikeHits() * 30;
            float OPRage = OverpowerHits() * 5;
            float SDRage = SuddenDeathHits() * 10;
            float SlamRage = SlamHits() * 15;
            float SweepingRage = _talents.SweepingStrikes * 30 * 0.5f; // 30 rage every half minute
            float rage = BTRage+WWRage+MSRage+OPRage+SDRage+SlamRage;
            return rage;  
        }

        public float BloodRageGain() {
            return (20+5*_talents.ImprovedBloodrage)/(60*(1-0.11f*_talents.IntensifyRage));
        }

        public float AngerManagementGain() {
            return _talents.AngerManagement/3.0f;
        }

        public float ImprovedBerserkerRage() {
            return _talents.ImprovedBerserkerRage * 10 / (30 * (1 - 0.11f * _talents.IntensifyRage));
        }

        public float OtherRage() {
            float rage = (14.0f / 8.0f*(1+_combatFactors.MhCrit-(1.0f-_combatFactors.ProbMhWhiteHit)));
            if(_combatFactors.OffHand.DPS > 0 && (_combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || _talents.TitansGrip == 1))
                rage += 7.0f/8.0f*(1+_combatFactors.OhCrit-(1.0f-_combatFactors.ProbOhWhiteHit));
            rage *= _combatFactors.TotalHaste;
            rage += AngerManagementGain() + ImprovedBerserkerRage() + BloodRageGain();
            rage *= 1 + _talents.EndlessRage * 0.25f;

            return rage;
        }

        public float freeRage() {
            return _whiteStats.whiteRageGen() + OtherRage() + SuddenDeathHits()*10 - neededRage()/*/(1.25f*_talents.EndlessRage)*/;
        }

        public float heroicStrikeRage() {
            //MHAverageDamage*ArmorDeal*15/4/cVal*(1+mhCritBonus*mhCrit-glanceChance*glanceReduc-whiteMiss-dodgeMH)+7/2*(1+mhCrit-whiteMiss-whiteDodge)
            float rage = _combatFactors.AvgMhWeaponDmg*_combatFactors.DamageReduction*15.0f/4/320.6f;
            rage *= (1.0f + _combatFactors.MhCrit * _combatFactors.BonusWhiteCritDmg
                            - (1.0f - _combatFactors.ProbMhWhiteHit) - (0.25f * 0.35f));
            rage += 7.0f / 2.0f * (1 + _combatFactors.MhCrit - (1.0f - _combatFactors.ProbMhWhiteHit));
            //int modNumber = 0; if (_talents.GlyphOfHeroicStrike) { modNumber = 10; }
            //rage += 1.0f * (1 + _combatFactors.MhCrit - (1.0f - _combatFactors.ProbMhWhiteHit)) * modNumber;
            
            return rage;
        }
        #endregion

        #region Fury Skills Damage
        public float Bloodthirst() {
            float btDamage = _stats.AttackPower * 0.5f * _combatFactors.DamageBonus*(1 + _talents.UnendingFury * 0.02f);
            btDamage *= (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg);
            btDamage *= BloodThirstHits();

            if (btDamage < 0) { btDamage = 0; }

            return _combatFactors.DamageReduction*btDamage*_talents.Bloodthirst;
        }
        public float Whirlwind() {
            if(_calcOpts != null && !_calcOpts.FuryStance){return 0.0f;}
            float wwDamage = _combatFactors.DamageBonus * (1.00f+0.1f*_talents.ImprovedWhirlwind)*(1.00f + _talents.UnendingFury * 0.02f);
            wwDamage *= (_combatFactors.NormalizedMhWeaponDmg * (1f - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance
                         + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg) +
                         (0.50f + _talents.DualWieldSpecialization * 0.025f) * _combatFactors.NormalizedOhWeaponDmg * 
                         (1.00f - _combatFactors.YellowMissChance - _combatFactors.OhDodgeChance
                         + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg));
            wwDamage *= WhirlWindHits();
            if (wwDamage < 0) { wwDamage = 0; }
            return _combatFactors.DamageReduction * wwDamage;
        }
        public float HeroicStrike() {
            heroicStrikePercent = _combatFactors.MainHandSpeed*HeroicStrikeHits(0);
            if (heroicStrikePercent > 1)
                heroicStrikePercent = 1;
            //if (_combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand && _talents.TitansGrip != 1)
            //    heroicStrikePercent = 0;

            float damageIncrease = heroicStrikePercent* _combatFactors.DamageReduction*((_combatFactors.DamageBonus*495)
                                   + _combatFactors.DamageReduction*_combatFactors.AvgMhWeaponDmg*(((_combatFactors.MhYellowCrit)-(_combatFactors.MhCrit))*
                                   (1+(_combatFactors.BonusYellowCritDmg-_combatFactors.BonusWhiteCritDmg))+(_combatFactors.WhiteMissChance-_combatFactors.YellowMissChance)+(0.25f*0.35f)));
            if (damageIncrease < 0){damageIncrease = 0;}
            return damageIncrease;
        }
        public float Bloodsurge() {
            float slamDamage = BloodsurgeProcs() / (1 - _combatFactors.MhDodgeChance - _combatFactors.YellowMissChance);
            slamDamage *= (1 - _combatFactors.MhDodgeChance - _combatFactors.YellowMissChance + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg);
            slamDamage *= _combatFactors.DamageReduction * (1 + _stats.BonusSlamDamage) * (1 + 0.02f * _talents.UnendingFury) * (_combatFactors.AvgMhWeaponDmg + (_combatFactors.DamageBonus * 250));
            if (slamDamage < 0) { slamDamage = 0; }
            return slamDamage;
        }
        #endregion
        #region Arms Skills Damage
        public float MortalStrike() {
            if (_talents.MortalStrike == 0) { return 0; }
            float msDamage = _combatFactors.DamageBonus *(1.1f)*(1+0.0333333f*_talents.ImprovedMortalStrike);
            msDamage *= (_combatFactors.NormalizedMhWeaponDmg * (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance
                         + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg)+380);
            msDamage *= MortalStrikeHits();
            msDamage *= (_talents.GlyphOfMortalStrike ? 1.10f : 0.00f);
            return msDamage * _combatFactors.DamageReduction;
        }
        public float Overpower() {
            if (_calcOpts != null && _calcOpts.FuryStance) { return 0.0f; }
            float opCrit = _combatFactors.MhYellowCrit + (0.25f * _talents.ImprovedOverpower);
            if (opCrit > 1) { opCrit = 1; }
            float overpowerDamage = _combatFactors.DamageBonus;
            overpowerDamage *= (_combatFactors.NormalizedMhWeaponDmg * (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance
                         + opCrit * _combatFactors.BonusYellowCritDmg));

            overpowerDamage *= OverpowerHits();
            return overpowerDamage * _combatFactors.DamageReduction * (1+_talents.UnrelentingAssault*0.10f);
        }
        public float Slam() {
            float slamDamage = _combatFactors.DamageBonus*(1+_stats.BonusSlamDamage);
            slamDamage *= (_combatFactors.AvgMhWeaponDmg * (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance
                         + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg)+250);
            slamDamage *= SlamHits();

            return slamDamage*_combatFactors.DamageReduction;
        }
        public float SuddenDeath() {
            if (_talents.SuddenDeath==0) { return 0.0f; }
            float executeRage = freeRage()*18 + (_talents.GlyphOfExecution ? 10.00f : 0.00f);
            executeRage -= (15 - 2.5f * _talents.ImprovedExecute);
            if (executeRage > 30){executeRage = 30;}
            float executeDamage = _combatFactors.DamageBonus;
            executeDamage *= ((_stats.AttackPower * 0.2f) + 1456 + executeRage * 38);
            executeDamage *= SuddenDeathHits();
            if (executeDamage < 0) { executeDamage = 0; }
            return executeDamage * _combatFactors.DamageReduction;
        }
        public float Rend() {
            if (_calcOpts != null && _calcOpts.FuryStance) { return 0.0f; }
            float rendDamageMod = (1 + (0.35f * 0.25f)); // Bonus Dmg over 75% HP
            rendDamageMod *= 1 + 0.1f * _talents.ImprovedRend; // +10% or 20% from Improved Rend Talent
            rendDamageMod *= _combatFactors.DamageBonus; // From gear and stuff
            rendDamageMod *= 1 + _stats.BonusBleedDamageMultiplier; // not sure where this is coming from, seems to be Zero all the time
            rendDamageMod *= 1 + ((_talents!=null&&_talents.GlyphOfRending) ? (2.00f/7.00f) : 0.00f); // Glyph of Rending adds 6 seconds (2 more ticks from 5 to new ttl of 7)
            rendDamageMod *= 1 + _talents.Trauma*0.15f*TraumaHits();
            float rendDamage = 380 * rendDamageMod; // Base Rend Dmg
            rendDamage += rendDamageMod * (_combatFactors.AvgMhWeaponDmg); // Add in Weapon dmg
            return rendDamage / 18; // divide by 18 because of the uptime maybe? I dont get this part
            //return rendDamage;
        }
        public float SwordSpec() {
            float damage = SwordSpecHits() * _combatFactors.AvgMhWeaponDmg;
            damage *= (1 + _combatFactors.MhCrit * _combatFactors.BonusWhiteCritDmg);
            return damage * _combatFactors.DamageReduction;
        }
        public float BladeStorm() {
            float wwDamage = _combatFactors.DamageBonus;
            wwDamage *= (_combatFactors.NormalizedMhWeaponDmg * (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance
                         + _combatFactors.MhYellowCrit * _combatFactors.BonusYellowCritDmg));
            wwDamage *= BladestormHits();

            if (wwDamage < 0)
                wwDamage = 0;
            return _combatFactors.DamageReduction * wwDamage;
        }
        #endregion

        public float Deepwounds() {
            float mhCrits = (1 / _combatFactors.MainHandSpeed) * _combatFactors.MhCrit * (1 - heroicStrikePercent);
            float ohCrits = (1 / _combatFactors.OffHandSpeed) * _combatFactors.OhCrit;

            #region Fury Deep Wounds
            float heroicCrits = (1 / _combatFactors.MainHandSpeed) * _combatFactors.MhYellowCrit * heroicStrikePercent;
            float bloodThirstCrits = BloodThirstHits() * _combatFactors.MhYellowCrit;
            float whirlWindCrits = WhirlWindHits() * _combatFactors.MhYellowCrit;
            
            float bloodsurgeCrits = 0;
            if(_talents.Bloodsurge > 1)
                bloodsurgeCrits = (0.2f / 3 * _talents.Bloodsurge) * (3 * (1 - _combatFactors.YellowMissChance - _combatFactors.MhDodgeChance) + 7 * heroicStrikesPerSecond) / 10 * _combatFactors.MhCrit;
            #endregion

            #region Arms Deep Wounds
            float mortalStrikeCrits = MortalStrikeHits() * _combatFactors.MhYellowCrit;
            float overPowerCrits = OverpowerHits() * (_combatFactors.MhYellowCrit + (_talents.ImprovedOverpower * 0.25f) > 1? 1.0f : (_combatFactors.MhYellowCrit + (_talents.ImprovedOverpower * 0.25f)));
            float suddenDeathCrits = SuddenDeathHits() * _combatFactors.MhYellowCrit;
            float slamCrits = SlamHits() * _combatFactors.MhYellowCrit;
            float bladestormCrits = BladestormHits() * _combatFactors.MhYellowCrit;
            float swordspecCrits = SwordSpecHits() * _combatFactors.MhCrit;
            #endregion

            float deepWoundsDamage = _combatFactors.AvgMhWeaponDmg*(mhCrits+heroicCrits+bloodThirstCrits+whirlWindCrits+bloodsurgeCrits
                                                                    + mortalStrikeCrits + overPowerCrits + suddenDeathCrits + slamCrits + bladestormCrits + swordspecCrits);
            deepWoundsDamage += _combatFactors.AvgOhWeaponDmg*ohCrits;
            deepWoundsDamage *= 1+_stats.BonusBleedDamageMultiplier;
            deepWoundsDamage *= 0.16f * _talents.DeepWounds;
            deepWoundsDamage *= 1 + _talents.Trauma * 0.15f * TraumaHits();
            deepWoundsDamage *= _combatFactors.DamageBonus;

            return deepWoundsDamage;
        }
    }
}

