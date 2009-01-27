using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public class AttackModel
    {
        private Character   Character;
        private Stats       Stats;

        // Populate and use this for ability references in the future
        public AbilityModelList Abilities;

        private AttackModelMode _threatModelMode;
        public AttackModelMode ThreatModelMode
        {
            get { return _threatModelMode; }
            set { _threatModelMode = value; Calculate(); }
        }

        private RageModelMode _rageModelMode;
        public RageModelMode RageModelMode
        {
            get { return _rageModelMode; }
            set { _rageModelMode = value; Calculate(); }
        }

        private float _threatPerSecond = 0.0f;
        public float ThreatPerSecond
        {
            get { return _threatPerSecond; }
            private set { _threatPerSecond = value; }
        }

        private float _damagePerSecond = 0.0f;
        public float DamagePerSecond
        {
            get { return _damagePerSecond; }
            private set { _damagePerSecond = value; }
        }

        public void Calculate()
        {
            // Almost everything here requires a weapon+shield, so no reason to bother if none is equipped
            // Can add some fallbacks here later...
            if (Character.MainHand == null || Character.OffHand == null)
                return;

            float modelLength = 0.0f;
            float modelThreat = 0.0f;
            float modelDamage = 0.0f;
            float modelCrits = 0.0f;

            AbilityModel WhiteAttack    = new AbilityModel(Character, Stats, Ability.None);
            AbilityModel HeroicStrike   = new AbilityModel(Character, Stats, Ability.HeroicStrike);
            AbilityModel ShieldSlam     = new AbilityModel(Character, Stats, Ability.ShieldSlam);
            AbilityModel Revenge        = new AbilityModel(Character, Stats, Ability.Revenge);
            AbilityModel SunderArmor    = new AbilityModel(Character, Stats, Ability.SunderArmor);
            AbilityModel Devastate      = new AbilityModel(Character, Stats, Ability.Devastate);
            AbilityModel Shockwave      = new AbilityModel(Character, Stats, Ability.Shockwave);
            AbilityModel ConcussionBlow = new AbilityModel(Character, Stats, Ability.ConcussionBlow);
            AbilityModel DeepWounds     = new AbilityModel(Character, Stats, Ability.DeepWounds);
            AbilityModel DamageShield   = new AbilityModel(Character, Stats, Ability.DamageShield);

            switch (ThreatModelMode)
            {
                case AttackModelMode.Basic:
                    {
                        // Basic Rotation
                        // Shield Slam -> Revenge -> Sunder Armor -> Sunder Armor
                        modelLength = 6.0f;
                        modelThreat = ShieldSlam.Threat + Revenge.Threat + SunderArmor.Threat * 2;
                        modelDamage = ShieldSlam.Damage + Revenge.Damage;
                        modelCrits  = ShieldSlam.CritPercentage + Revenge.CritPercentage;
                        break;
                    }
                case AttackModelMode.Devastate:
                    {
                        // Devastate Rotation
                        // Shield Slam -> Revenge -> Devastate -> Devastate
                        if (Character.WarriorTalents.Devastate == 1)
                        {
                            modelLength =   6.0f;
                            modelThreat =   ShieldSlam.Threat + Revenge.Threat + Devastate.Threat * 2;
                            modelDamage =   ShieldSlam.Damage + Revenge.Damage + Devastate.Damage * 2;
                            modelCrits  =   ShieldSlam.CritPercentage + Revenge.CritPercentage + Devastate.CritPercentage * 2;
                        }
                        break;
                    }
                case AttackModelMode.SwordAndBoard:
                    {
                        // Sword And Board Rotation
                        // Requires 3 points in Sword and Board
                        // Shield Slam > Revenge > Devastate
                        // The distribution of abilities in the model is as follows:
                        // 1.0 * Shield Slam + 0.73 * Revenge + 1.4596 * Devastate
                        // The cycle length is 4.7844s, abilities per cycle is 3.1896
                        if (Character.WarriorTalents.SwordAndBoard == 3)
                        {
                            modelLength =   4.7644f;
                            modelThreat =   (1.0f * ShieldSlam.Threat) +
                                            (0.73f * Revenge.Threat) +
                                            (1.4596f * Devastate.Threat);
                            modelDamage =   (1.0f * ShieldSlam.Damage) +
                                            (0.73f * Revenge.Damage) +
                                            (1.4596f * Devastate.Damage);
                            modelCrits =    (1.0f * ShieldSlam.CritPercentage) +
                                            (0.73f * Revenge.CritPercentage) +
                                            (1.4596f * Devastate.CritPercentage);
                        }
                        break;
                    }
                case AttackModelMode.Full:
                    {
                        // Sword And Board + Shockwave/Concussion Blow Rotation
                        // Requires 3 points in Sword and Board, Shockwave, and Concussion Blow
                        // Shield Slam > Revenge > Devastate @ 3s Shield Slam Cooldown > Concussion Blow > Shockwave > Devastate
                        // The distribution of abilities in the model is as follows:
                        // 1.0 * Shield Slam + 0.73 * Revenge + 1.133 * Devastate + 0.3266 * (Concussion Blow/Shockwave/Devastate)
                        // The cycle length is 4.7844s, abilities per cycle is 3.1896
                        if (Character.WarriorTalents.SwordAndBoard == 3 && Character.WarriorTalents.ConcussionBlow == 1 && Character.WarriorTalents.Shockwave == 1)
                        {
                            modelLength =   4.7644f;
                            modelThreat =   (1.0f * ShieldSlam.Threat) +
                                            (0.73f * Revenge.Threat) +
                                            (1.133f * Devastate.Threat) +
                                            (0.3266f * ((ConcussionBlow.Threat + Shockwave.Threat + Devastate.Threat) / 3));
                            modelDamage =   (1.0f * ShieldSlam.Damage) +
                                            (0.73f * Revenge.Damage) +
                                            (1.133f * Devastate.Damage) +
                                            (0.3266f * ((ConcussionBlow.Damage + Shockwave.Damage + Devastate.Damage) / 3));
                            modelCrits =    (1.0f * ShieldSlam.CritPercentage) +
                                            (0.73f * Revenge.CritPercentage) +
                                            (1.133f * Devastate.CritPercentage) +
                                            (0.3266f * ((ConcussionBlow.CritPercentage + Shockwave.CritPercentage + Devastate.CritPercentage) / 3));
                        }
                        break;
                    }
            }

            // White Damage
            float weaponHits = modelLength / Lookup.WeaponSpeed(Character, Stats);
            if (RageModelMode == RageModelMode.Infinite)
            {
                // Convert all white hits to heroic strikes
                modelThreat += HeroicStrike.Threat * weaponHits;
                modelDamage += HeroicStrike.Damage * weaponHits;
                modelCrits  += HeroicStrike.CritPercentage * weaponHits;
            }
            else
            {
                // Normal white hits if we aren't using infinite rage, add some logic for a hybrid system later...
                modelThreat += WhiteAttack.Threat * weaponHits;
                modelDamage += WhiteAttack.Damage * weaponHits;
                modelCrits  += WhiteAttack.CritPercentage * weaponHits;
            }

            // Damage Shield, hardcoded at 2.0s attack speed... need to add attack speed as a character option
            float attackerHits = modelLength / 2.0f;
            modelThreat += DamageShield.Threat * attackerHits;
            modelDamage += DamageShield.Damage * attackerHits;
            modelCrits  += DamageShield.CritPercentage * attackerHits;

            // Deep Wounds
            modelThreat += DeepWounds.Threat * modelCrits;
            modelDamage += DeepWounds.Damage * modelCrits;

            ThreatPerSecond = modelThreat / modelLength;
            DamagePerSecond = modelDamage / modelLength;
        }

        public AttackModel(Character character, Stats stats, AttackModelMode threatModelMode)
            : this(character, stats, threatModelMode, RageModelMode.Infinite)
        {
        }

        public AttackModel(Character character, Stats stats, AttackModelMode threatModelMode, RageModelMode rageModelMode)
        {
            Character        = character;
            Stats            = stats;
            _threatModelMode = threatModelMode;
            _rageModelMode   = rageModelMode;
            Calculate();
        }
    }
}